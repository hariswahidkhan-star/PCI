# Domain 7 — Cost, Resources and Commercial Awareness
## Why this domain exists

A leader who can defend a date but not a number is half-equipped. Domain 6 built the schedule; this
domain builds the money that pays for it and the commercial arrangements that bind other people to
deliver it. It starts with estimating and budgeting: how a credible number is constructed and what
its accuracy claim actually means (KA 7.1); establishes the cost baseline and the forecasting that
keeps it honest (KA 7.2); builds **earned value** in full, because it is the only technique that
answers cost and schedule performance in one integrated language (KA 7.3); and closes with the
commercial awareness a leader cannot delegate: resource economics, procurement strategy, contract
models and cash flow (KA 7.4). Risk quantification deepens in Domain 8; contracts and supply
networks get their own treatment in Domain 10. What belongs here is the leader's own numeracy:
enough to know when a forecast is arithmetic and when it is hope.

**Learning objectives.** After this domain a candidate can: select an estimating method
appropriate to the available definition and state its accuracy class; **read an accuracy class as
a distribution and say what percentile the approved number sits at**; build a three-point cost
estimate and distinguish a tail that is a spread from a tail that is a scenario; assemble a cost
baseline separating contingency from management reserve, **and trend contingency draw against
progress to a required draw efficiency**; measure `AC` so that it covers the same work as `EV`,
**and compute what omitting accruals does to every index and forecast built on it**;
distinguish commitments from actuals and **compute the uncommitted exposure of a forecast**;
compute and interpret the full earned-value set (`CV`, `SV`, `CPI`, `SPI`); **show how much of a
reported index is an artefact of the earning rule chosen, including the level-of-effort
distortion identity**; forecast with each `EAC` method and choose the one matching the variance's
cause; **state where each forecast's funding runs out if efficiency persists**; compute `VAC` and
`TCPI`, **derive the identity family linking `TCPI` to every `EAC`, and convert a
recovery-to-budget promise into the descope or the efficiency gain it actually requires**; read
resource economics through blended rates, **burdened cost per productive hour and the priced cost
of a crew-week**; distinguish the main contract models by who carries cost risk and **compute the
hours breakeven between time-and-materials and a fixed price**; compute a cost-incentive fee and
the point of total assumption, **and show how the share ratio and the ceiling move it**; explain
why cash flow and profit differ on a project **and compute the working capital a project
absorbs**; **price the governance latency on a reserve release**; **verify an automated accrual,
ledger or commitment feed against the ledger, the following period's reversal and a hand-reproduced
coverage figure, and name the four determinations no tool may make**; and govern AI-produced cost
forecasts under the family verification rule.

**The master worked project.** Project Auriga continues from Domain 6 (the 25-week control-systems
upgrade for a regional utility) now with money attached. Its approved cost baseline is **`BAC` = USD
4,000,000**, assembled from **USD 3,640,000** of control-account budgets plus **USD 360,000** of
contingency, with **USD 240,000** of management reserve outside it for a total funding requirement
of **USD 4,240,000** (KA 7.1.3). The delivering organisation's fixed price to the utility is **USD
4,400,000**, so the bid margin is **USD 400,000**, 9.09 % of price (KA 7.4.4). At the **data date,
end of week 13**, the baseline says **`PV` = USD 2,080,000**; measurement gives **`EV` = USD
1,920,000** and **`AC` = USD 2,120,000**. Every calculation in KA 7.2–7.3 uses those four numbers,
and Auriga's engineering blended rate of **USD 130.625 per hour** (an engineer-week of **USD
5,225**, the figure Domain 9 costs rework with) carries the resource arithmetic of KA 7.4. Domain
6's **cost of delay of USD 45,000 per week** is the price of time throughout. **Meridian Care
Records** (the 40-clinic programme of Domains 1, 2, 15 and 16, approved cost **USD 2,400,000**, cost
of delay **USD 14,280 per week** at its planned 70 % adoption) returns in Case study C, because
reserve architecture and the latency of the authority that releases it are programme-scale cost
questions a single project cannot show.

**Reference points.** The body most often named in connection with the cost-engineering material of
KA 7.1 is **AACE International**, referenced for the existence and purpose of the cost-estimate
classification progression in its Total Cost Management framework (7.1.1). It is a professional
association's framework (voluntary, describing practice, neither legislation nor a certifiable
requirement), and it obliges nobody of itself unless an organisation or a contract adopts it. It is
named here and not reproduced: no class table, range or definition from it appears in this volume,
the treatment in 7.1.1 is written in this book's own words, and a reader who wants the framework
should obtain the current version from its publisher. Naming it implies no endorsement in either
direction.

---

## Knowledge Area 7.1 — Estimating and budgeting

*Topics: 7.1.1 estimating methods and accuracy classes · 7.1.2 three-point estimating ·
7.1.3 from estimate to budget.*

### 7.1.1 Estimating methods and accuracy classes

**The principle.** An estimate's accuracy is governed by how well the work is *defined*, not by
how much effort went into the arithmetic. The three standard methods trade definition against
speed:

| Method | How it works | Needs | Typical use |
|---|---|---|---|
| **Analogous (top-down)** | Scale a comparable past project | A true analogue and a scaling basis | Concept, screening (Domain 2) |
| **Parametric** | Cost = rate × parameter (per metre, per point, per kW) | A calibrated rate from real history | Early design, repetitive work |
| **Bottom-up** | Estimate each work package, then sum | A WBS at package level (Domain 4) | Baseline setting, control |

**Accuracy classes.** Mature cost practice publishes an estimate's *class* alongside its number: a
range that narrows as definition matures (the AACE Total Cost Management framework's
class-5-to-class-1 progression is the reference treatment, described here in this book's own words).
The professional discipline is simple and widely broken: **an estimate is never a single number
without a range and a class**. "USD 4 million" is not an estimate; "USD 4 million, −15 %/+30 %, at a
class consistent with 30 % design completion" is.

That second formulation contains far more than a caveat, and almost nobody reads what it says.

**Worked example 7.1.1 — what Auriga's accuracy class actually claims.**

1. **Setup.** Auriga's sanction estimate is **USD 4,000,000** with a declared class range of **−15
   %/+30 %** at 30 % design completion. The programme board approves that number as the baseline.
   Read the range as a **triangular distribution** with the point estimate as its most likely value
   (the least assumption-heavy reading of a three-point statement), and ask three questions: how
   wide is the claim, where does the approved number sit inside it, and what would funding at a
   stated confidence have cost?
2. **Formula.** Bounds `lo = P(1 − d⁻)`, `hi = P(1 + d⁺)`. For a triangular distribution on
   (`a`, `m`, `b`): mean `= (a + m + b)/3`; the cumulative probability at the mode is
   `(m − a)/(b − a)`; median `= b − √((b − a)(b − m)/2)`; the `p`-quantile above the mode is
   `b − √((1 − p)(b − a)(b − m))`; standard deviation
   `σ = √((a² + m² + b² − ab − am − bm)/18)`. For comparison, the PERT weighting of the same
   three points is `(a + 4m + b)/6`.
3. **Substitution.** `lo = 4,000,000 × 0.85`; `hi = 4,000,000 × 1.30`; mean
   `= (3,400,000 + 4,000,000 + 5,200,000)/3`; `F(mode) = 600,000/1,800,000`;
   P80 `= 5,200,000 − √(0.20 × 1,800,000 × 1,200,000)`; median
   `= 5,200,000 − √(1,800,000 × 1,200,000/2)`.
4. **Result.** Range **USD 3,400,000 to USD 5,200,000**: a band of **USD 1,800,000**, or **45.0 %**
   of the point estimate. `σ` = **USD 374,166**. The mean is **USD 4,200,000**; the median **USD
   4,160,770**; the P80 **USD 4,542,733**. The approved USD 4,000,000 sits at the **33.33rd
   percentile** of the estimate's own declared range. The PERT weighting of the same three points
   gives **USD 4,100,000**.
5. **Interpretation.** The arithmetic turns a routine caveat into four statements a board can be
   held to, and the first one is uncomfortable.

   **Auriga was funded at a number it had a two-in-three chance of exceeding.** Not because anybody
   was careless, but because an asymmetric range with the point estimate at its mode puts only `(m −
   a)/(b − a)` of the distribution at or below that point: here exactly one third. The general
   result is worth carrying: **whenever the upside tail is longer than the downside tail, the point
   estimate is below the mean and below the median**, so approving the point estimate is approving a
   sub-50 % confidence position. It is a legitimate thing to do; it is not a legitimate thing to do
   unknowingly, and the sentence that makes it knowing is "this baseline is a P33".

   **The mean of the declared range is USD 4,200,000, which is exactly the week-13 `EAC` that
   Auriga's own data produces under method (a) (KA 7.3.3).** That coincidence is worth pausing on,
   because it is the shape of most cost surprises: the eventual outturn was inside the estimate's
   stated accuracy from the first day. What went wrong was not the estimate but the **funding
   decision taken against it**: approving the mode and reporting the variance against the mode. A
   reviewer's question follows directly: *is the current forecast outside the original class range,
   or merely above the point?* Outside the range is an estimating failure; above the point but
   inside the range is a funding-level decision coming home.

   **Confidence has a price and it is computable.** Funding at the P80 rather than the point
   requires **USD 542,733** more; funding at the mean requires **USD 200,000** more. Auriga's
   baseline in fact carries **USD 360,000** of contingency inside the 4,000,000 (KA 7.1.3), which is
   a different structure again, because that contingency was sized bottom-up against identified
   risks rather than read off a distribution, and the two figures answer different questions. Where
   both are available they should be reconciled explicitly: a risk-based contingency far below the
   distribution-based number means either the register is incomplete (Domain 8, KA 8.2.4 aggregates
   it properly) or the range was drawn too wide.

   **Two cautions, both of which a reviewer should raise.** The distribution shape is an assumption,
   not a fact: triangular gives a mean 200,000 above the point, the PERT weighting 100,000, so the
   *direction* is robust and the *magnitude* is not, and no more than "the mean is 100,000 to
   200,000 above the point, 2.5 % to 5.0 % of the estimate" should be claimed. And a class range is
   a statement about **definition maturity**, not a probability distribution anybody measured;
   treating it as one is a modelling convenience that must be labelled. What it is *not* is
   decoration. The honest use is the one above: convert the range into the percentile the approved
   number occupies, and say so out loud.

> **Fig 7.1.1 — What an accuracy class actually says.** A triangular density on Auriga's declared
> class range: x-axis outturn cost USD 3.2m–5.45m, apex at the USD 4,000,000 point estimate, falling
> to zero at 3,400,000 and 5,200,000. The area to the left of the point estimate is shaded crimson
> and annotated **"only 33.3 % of the declared range lies at or below the number that was
> approved"**; the area to the right is shaded brand blue. Four pins mark the point estimate / `BAC`
> **4,000,000 (33.3rd percentile)**, the median **4,160,770**, the mean **4,200,000, labelled "=
> week-13 `EAC`(a)"**, and the **P80 4,542,733 (+13.57 % on the point)**. A bracket above the curve
> records **band 3,400,000–5,200,000 = 1,800,000, 45.0 % of the point estimate; σ = 374,166**, and a
> footnote records that a PERT weighting of the same three points gives 4,100,000. Source: PCI
> original. Alt text: a right-skewed triangular distribution over a cost range, with the approved
> point estimate marked one third of the way into the distribution and the mean, median and
> eightieth percentile all to its right.

**Common pitfall: precision mistaken for accuracy.** A bottom-up estimate summing 400 packages to
`USD 4,183,662` looks authoritative and is no more accurate than its worst assumption. Leaders
should be more suspicious of an over-precise number than a rounded one.

### 7.1.2 Three-point estimating

Cost, like duration (Domain 6, KA 6.4.3), is a distribution. The same PERT weighting applies:

```
Cₑ = (o + 4m + p) / 6          σ = (p − o) / 6
```

**Worked example 7.1.2 — Auriga's control-hardware package.**

1. **Setup.** Procurement of control hardware is estimated **optimistic USD 680,000**,
   **most-likely USD 750,000**, **pessimistic USD 1,000,000** (the tail: a single-source
   controller with volatile lead-time pricing).
2. **Formula.** `Cₑ = (o + 4m + p)/6`; `σ = (p − o)/6`.
3. **Substitution.** `Cₑ = (680,000 + 3,000,000 + 1,000,000)/6 = 4,680,000/6`;
   `σ = 320,000/6`.
4. **Result.** `Cₑ` = **USD 780,000**; `σ` ≈ **USD 53,333**.
5. **Interpretation.** The most-likely figure is 750,000, but the *expected* cost is 780,000. The
   right-skewed tail adds USD 30,000 before anything goes wrong. Budgeting at the mode
   systematically under-funds a portfolio of such packages; the difference between mode and mean,
   summed across a project, is a large part of what contingency exists to cover (7.1.3, and Domain
   8's quantification).

   **Budget the mean, hold the tail centrally.** Auriga's control account CA-20 is budgeted at **USD
   780,000**: the `Cₑ`, not the mode (7.1.3's table). Budgeting the mode would have loaded **USD
   30,000** of arithmetically certain under-funding into the baseline of this one package, and a
   project of forty such packages inherits forty such gaps that no manager can be held accountable
   for. The professionally right structure is the one used here: **each control account budgeted at
   its expected value, the dispersion held once, centrally, as contingency.**

   **The mode-to-mean gap is 0.5625 σ, and that ratio is the tell.** It is 4.00 % of the mode here:
   small enough to be dismissed in a meeting and large enough to matter forty times over.

   **The pessimistic value is 4.125 σ above the mean, and that is a warning, not a spread.** The
   optimistic value sits 1.875 σ below it. A tail four standard deviations out is not the upper end
   of a continuum of outcomes; it is a **discrete event** (the single-source controller not being
   available at the quoted price), which has its own probability and its own response. The
   professional treatment is to carry it in the risk register with that probability and cost (Domain
   8, KA 8.2.2), *not* to smear it into a σ that then implies a symmetric spread nobody believes.
   Three-point estimating is the wrong instrument for a bimodal outcome, and asymmetry of this
   severity is the diagnostic.

   **Sizing from σ needs its approximation stated.** Treating `Cₑ` and `σ` as a normal
   approximation, a P80 for this package is `780,000 + 0.8416 × 53,333 =` **USD 824,885**, so **USD
   44,885** of contingency above the mean, or USD 74,885 above the mode. That is a defensible
   working figure and it is an approximation twice over. The PERT `σ` is itself a rule of thumb, and
   a distribution with a 4 σ tail is not normal. Quote it as an order of magnitude, aggregate
   properly at project level (Domain 8, KA 8.2.4, where correlation between packages does the real
   work), and never present a package-level P80 as though it were measured.

### 7.1.3 From estimate to budget

**The structure.** An estimate becomes a controllable budget through a deliberate hierarchy:

```
work-package estimates
  → control-account budgets            (WBS × organisation — the management-control points)
  → + contingency reserve              → the COST BASELINE (BAC): the PM's authority
  → + management reserve               → the total project funding requirement
```

Two rules carry the discipline. **Contingency is inside the baseline**, sized against identified
risks (Domain 8) and spent by the project manager under a stated protocol. **Management reserve
is outside the baseline**, held for unknown-unknowns, and released only by the sponsor or change
authority (Domain 3's decision rights) — releasing it *changes* the baseline through change
control (Domain 4, KA 4.4). Blurring the two is how projects appear to be "on budget" while
consuming their own funding runway.

**Time-phasing.** The baseline is spread across the schedule to produce the **cumulative cost
curve** (the S-curve), and that curve *is* Planned Value (KA 7.3). A budget with no phasing cannot
be measured against; the schedule (Domain 6) is therefore a precondition of cost control, not a
parallel activity.

**Worked example 7.1.3 — assembling Auriga's baseline, and reading the contingency trend.**

1. **Setup.** Auriga's six control accounts, each budgeted at its expected value (7.1.2's
   discipline), with contingency sized bottom-up against the identified risks of Domain 8's
   register and management reserve set by the sponsor:

   | Control account | Budget (USD) |
   |---|---|
   | CA-10 Project management and engineering | 620,000 |
   | CA-20 Control hardware procurement | 780,000 |
   | CA-30 Civil and enabling works | 540,000 |
   | CA-40 Installation | 1,020,000 |
   | CA-50 Testing and commissioning | 480,000 |
   | CA-60 Training and handover | 200,000 |
   | **Control-account total** | **3,640,000** |

   Contingency is **USD 360,000**; management reserve is **USD 240,000**. At the week-13 data date
   the project is **48.00 %** complete by `EV/BAC` (KA 7.3.2) and has drawn **USD 190,000** of
   contingency: USD 150,000 for the ground-remediation risk and USD 40,000 for control-hardware
   price escalation.
2. **Formula.** `BAC` = control accounts + contingency. Total funding requirement = `BAC` +
   management reserve. Contingency draw index = (draw ÷ contingency) ÷ percent complete. Draw per
   point of progress = draw ÷ percent complete. Projected total draw = that rate × 100. Required
   draw efficiency for the remainder = remaining contingency ÷ (demonstrated rate × remaining
   points of progress).
3. **Substitution.** `BAC = 3,640,000 + 360,000`; funding `= 4,000,000 + 240,000`. Index
   `= (190,000/360,000)/0.48 = 0.5278/0.48`. Rate `= 190,000/48.00`. Required efficiency
   `= 170,000/(3,958.33 × 52.00)`.
4. **Result.** `BAC` = **USD 4,000,000**; total funding requirement **USD 4,240,000**. Contingency
   is **9.89 %** of the control-account total and **9.00 %** of `BAC`; management reserve is **6.00
   %** of `BAC`. Contingency is **52.78 %** drawn at **48.00 %** complete, a draw index of
   **1.0995**. At the demonstrated rate of **USD 3,958.33 per point of progress** the full job needs
   **USD 395,833**, a shortfall of **USD 35,833** (9.95 % of the contingency, and 14.93 % of the
   management reserve). The remaining USD 170,000 must cover 52.00 points, i.e. USD 3,269.23 per
   point: a **required draw efficiency of 0.8259**, meaning the remaining work must consume
   contingency **17.41 % more slowly** than the first half did.
5. **Interpretation.** The third of the readings below is a reviewer's invariant most cost reports
   would fail.

   **Contingency has a `TCPI` of its own, and it should be reported.** The structure is identical to
   KA 7.3.4's: a demonstrated rate, a remaining allowance, and the efficiency the remainder must
   achieve. Reporting "contingency 52.8 % drawn" invites a shrug; reporting "the remaining work must
   draw contingency 17.41 % more slowly than the work to date, and there is no stated reason why it
   would" invites a decision. The threshold to watch is the **draw index**: above 1.00 the reserve
   is depleting faster than the work is being done, and the projected shortfall (here USD 35,833) is
   the number that eventually becomes a management-reserve request. It is visible now, at 48 %
   complete, which is the whole point of trending it.

   **The two reserves are not interchangeable and the arithmetic shows why.** The 35,833 projected
   shortfall is 14.93 % of the management reserve, so the reserve can absorb it; but absorbing it is
   a **sponsor** act that changes the baseline through change control (Domain 4, KA 4.4), not a
   project-level top-up. A project that quietly funds a contingency overrun from management reserve
   has converted an early warning into an invisible one, and MCQ 7.1-B's erosion pattern is what
   that looks like three periods later.

   **The draw must reconcile to the cost variance, and here it does not.** `CV` at week 13 is **(USD
   200,000)** while contingency drawn is **USD 190,000**: a gap of **USD 10,000**. That gap is not a
   rounding artefact; it means either a risk was funded without a draw being recorded, or USD 10,000
   of overrun has no identified cause. Both are findings. The invariant is worth carrying into every
   review: **identified-risk draws plus unattributed variance must equal the cost variance**, and
   the unattributed component is the one to interrogate, because a variance with no cause cannot be
   forecast (KA 7.2.2) and therefore cannot be recovered.

   The caution: none of this arithmetic is valid if contingency was sized as a percentage rather
   than against a register. A flat "10 % contingency" has no risks behind it, so a draw against it
   cannot be reconciled to anything, and the trend above degenerates into an observation about
   spending speed. Domain 8, KA 8.2.4 is where the number comes from; this KA is where it is
   governed.

### AI in this KA

Estimating is where AI assistance is most seductive and most in need of provenance. A model can
produce a plausible parametric rate instantly, and it cannot tell you whether that rate came from
comparable work, a different market, or nowhere at all. The governed workflow: **AI proposes** a
method, a rate and a range; the estimator supplies the *calibration evidence* (which projects, which
years, which escalation basis, the source discipline of the shared registry); the range and class
are stated; and a named human owns the number. **AI proposes; the professional verifies, decides and
remains accountable.** An estimate whose basis cannot be produced on request is not an estimate.

Two placements are worth being exact about, because 7.1.1 and 7.1.3 create work that machines do
well and work they must not touch. Converting a declared class range into percentiles, means and a
funding-confidence comparison (and repeating it across every package and every candidate
distribution shape) is mechanical, exactly checkable and almost never done by hand; that is a good
use. **Choosing the shape is not**, because the shape encodes a belief about how the tail behaves,
and 7.1.2's four-sigma pessimistic value shows what happens when the wrong shape is applied to a
discrete event: a scenario is quietly turned into a spread. Likewise, computing the contingency draw
index and the required draw efficiency each period is routine; deciding whether the draw pattern
means the register was incomplete or the work is simply front-loaded on its risks is a judgement
about the project, and belongs to whoever will have to ask for the management reserve.

### Key terms — KA 7.1

| Term | Meaning |
|---|---|
| **Analogous / parametric / bottom-up** | Scale an analogue · rate × parameter · sum the packages. |
| **Accuracy class** | The definition-linked range accompanying an estimate; never optional. |
| **Control account** | The WBS × organisation point where scope, budget and actuals integrate. |
| **Contingency reserve** | Inside the baseline; funds identified risks; PM-controlled. |
| **Management reserve** | Outside the baseline; unknown-unknowns; sponsor-controlled. |
| **`BAC`** | Budget at Completion: the time-phased cost baseline's total. |
| **Total funding requirement** | `BAC` + management reserve: the cash the sponsor must have available; Auriga's is USD 4,240,000. |
| **Approval percentile** | The position of the approved number inside its own declared range; Auriga's baseline is a P33. |
| **Contingency draw index** | (Contingency drawn ÷ contingency) ÷ percent complete; above 1.00 the reserve is depleting faster than the work is being done. |
| **Required draw efficiency** | Remaining contingency ÷ (demonstrated draw rate × remaining progress) — the `TCPI` of the reserve. |

### Sample MCQs — KA 7.1

**MCQ 7.1-A `[7.1.2 · Application]`** A package is estimated o = 680,000, m = 750,000,
p = 1,000,000. Its PERT expected cost is:
- A. USD 750,000
- B. USD 780,000 ✅
- C. USD 810,000
- D. USD 840,000

*Rationale:* `(680,000 + 4 × 750,000 + 1,000,000)/6 = 780,000`. A is the mode; C is the
unweighted three-point mean; D over-weights the pessimistic value.

**MCQ 7.1-B `[7.1.3 · Analysis]`** A project reports "on budget" while having consumed 60 % of
its management reserve at 40 % complete. The correct reading is:
- A. genuinely on budget — management reserve exists to be spent
- B. the baseline is intact but the project's total funding is eroding faster than progress; the trend belongs in the next report to the sponsor ✅
- C. a baseline breach requiring immediate re-baselining
- D. an accounting error, since management reserve is inside the baseline

*Rationale:* Management reserve sits *outside* the baseline (so D is wrong and A is technically true
but misleading), and its release is a sponsor-level signal, not a project-level convenience. It is
not yet a breach (C). It is the early warning that precedes one.

**MCQ 7.1-C `[7.1.1 · Recall]`** Which statement about a bottom-up estimate summing to
USD 4,183,662 is soundest?
- A. its precision indicates high accuracy
- B. its accuracy is bounded by its assumptions and definition maturity, and it must still carry a range and class ✅
- C. rounding it would reduce its accuracy
- D. bottom-up estimates do not need accuracy classes

*Rationale:* Precision (digits) and accuracy (closeness to outturn) are independent; definition
maturity governs the latter. Rounding changes no information (C), and every estimate carries a
class (D).

**MCQ 7.1-D `[7.1.1 · Analysis]`** A point estimate of USD 4,000,000 carries a declared class
range of −15 %/+30 %. Read as a triangular distribution, its expected cost is:
- A. USD 4,000,000
- B. USD 4,100,000
- C. USD 4,200,000 ✅
- D. USD 4,300,000

*Rationale:* `(3,400,000 + 4,000,000 + 5,200,000)/3 = 4,200,000` (7.1.1). A treats the point
estimate as the mean, which only holds for a symmetric range; B applies the PERT weighting, which
weights the mode four times and is a different shape from the one asked for; D averages the two
range endpoints and ignores the mode entirely.

**MCQ 7.1-E `[7.1.3 · Application]`** Control accounts total 3,640,000; contingency is 360,000;
management reserve is 240,000. `BAC` and the total funding requirement are:
- A. 3,640,000 and 4,000,000
- B. 4,000,000 and 4,240,000 ✅
- C. 4,240,000 and 4,240,000
- D. 4,000,000 and 4,000,000

*Rationale:* Contingency is inside the baseline and management reserve outside it (7.1.3). A
excludes contingency from `BAC`, which would leave the project's own risk funding outside its
authority; C puts management reserve inside the baseline, the error MCQ 7.1-B's misreading rests
on; D omits the reserve from the funding requirement, so the sponsor holds no cash for it.

**MCQ 7.1-F `[7.1.3 · Analysis]`** Contingency of 360,000 is 190,000 drawn at 48.00 % complete.
The draw efficiency the remaining work must achieve is:
- A. 0.4722
- B. 0.8259 ✅
- C. 0.9095
- D. 1.0995

*Rationale:* `170,000/(3,958.33 × 52.00) = 0.8259`, the remainder must draw 17.41 % more slowly than
demonstrated (7.1.3). A is the remaining balance as a share of the total, which is a level not an
index; C inverts the two shares; D is the draw index itself: the rate at which contingency is being
consumed relative to progress, not the efficiency required to recover.

### Self-check — KA 7.1

1. *Where do contingency and management reserve sit, and who spends each?* — Contingency inside
   the baseline, PM-controlled; management reserve outside it, sponsor-controlled.
2. *Why is the time-phased baseline a precondition for cost control?* — Because the phased
   cumulative curve is Planned Value; without it there is nothing to measure performance against.
3. *What two things must accompany every estimate?* — A range and an accuracy class tied to
   definition maturity.
4. *What percentile of its own declared range was Auriga's baseline approved at, and why?* — The
   33.33rd: an asymmetric −15 %/+30 % range puts only `(m − a)/(b − a)` of the distribution at or
   below the mode, so the approved number had a two-in-three chance of being exceeded.
5. *What does a contingency draw index above 1.00 mean, and what number does it eventually
   produce?* — The reserve is depleting faster than the work is being done; on Auriga it projects a
   USD 35,833 shortfall, which becomes a management-reserve request unless the draw rate changes.
6. *What must the contingency draw reconcile to?* — The cost variance. Auriga's USD 190,000 of
   draws against a `CV` of (USD 200,000) leaves USD 10,000 of variance with no identified cause,
   which is a finding because an unexplained variance cannot be forecast.

---

## Knowledge Area 7.2 — The cost baseline, actuals and forecasting

*Topics: 7.2.1 measuring actual cost · 7.2.2 the forecasting question · 7.2.3 baseline integrity.*

### 7.2.1 Measuring actual cost

**The principle.** `AC` must cover the same work as `EV`, in the same period, or every index
built from them is fiction. Three mechanics decide whether it does:

- **Accruals.** Work received but not yet invoiced belongs in this period's `AC`. Omit accruals and
  cost performance looks excellent until the invoices land: the commonest cause of a "sudden"
  overrun that was months old.
- **Commitment vs actual.** A purchase order is a *commitment*, not a cost. Both matter: `AC`
  drives performance, commitments drive the funding forecast. Confusing them double-counts or
  hides money.
- **Open-commitment hygiene.** Stale purchase orders left open inflate the forecast; cleansing
  them is a standing month-end task.

Each is usually stated and left; the size of what they do to a report is not intuitive.

**Worked example 7.2.1 — the accrual that made Auriga look perfect.**

1. **Setup.** At week 13 Auriga has genuinely incurred **USD 2,120,000** against `EV` of
   **USD 1,920,000**. The ledger, however, shows only **USD 1,920,000** of invoiced cost: the
   installation subcontractor's work for weeks 11–13 has been received and accepted but not yet
   invoiced, and **USD 200,000** of it has not been accrued. Weeks 14–17 then run at a true `EV` of
   **USD 480,000** for a true cost of **USD 520,000**, and the accrual backlog is invoiced in that
   period. Compare what the report says with what is true, in both periods and cumulatively.
2. **Formula.** `CPI = EV/AC`; `CV = EV − AC`; `EAC = BAC/CPI`; `TCPI = (BAC − EV)/(BAC − AC)`.
   Period index = period `EV` ÷ period `AC`. Cumulative index = cumulative `EV` ÷ cumulative `AC`.
3. **Substitution.** Reported `CPI = 1,920,000/1,920,000`; true `CPI = 1,920,000/2,120,000`.
   Reported `EAC(b) = 4,000,000/1.0000`; true `= 4,000,000/0.905660`. Period reported
   `= 480,000/(520,000 + 200,000)`; period true `= 480,000/520,000`. Cumulative after week 17
   `= 2,400,000/2,640,000`.
4. **Result.**

   | At week 13 | Reported | True |
   |---|---|---|
   | `AC` | 1,920,000 | 2,120,000 |
   | `CV` | 0 | (200,000) |
   | `CPI` | **1.0000** | **0.9057** |
   | `EAC(b)` | 4,000,000 | **4,416,667** |
   | `TCPI` to `BAC` | 1.0000 | **1.1064** |

   The single omission understates the forecast by **USD 416,667**: **10.42 % of `BAC`**. In the
   catch-up period the reported `AC` is `520,000 + 200,000 =` **USD 720,000**, so the reported
   period `CPI` is **0.6667** against a true period `CPI` of **0.9231**: an error of **0.2564**, and
   a reported figure implying the period's work cost **50.0 %** more than budget. Cumulatively after
   week 17 both versions converge on `EV` 2,400,000 against `AC` 2,640,000, `CPI` **0.9091**.
5. **Interpretation.** The invariant is the useful part: **an accrual omission is a timing error,
   so it cancels cumulatively and misstates every period in between.**

   **The direction of the distortion reverses, and the second half is the one that gets
   investigated.** Weeks 1–13 report perfection; week 17 reports a collapse. The true story is that
   performance *improved* (0.9057 to 0.9231) in exactly the period the report shows falling apart.
   Anyone acting on the period reading will investigate the wrong thing, and the operational
   explanations offered in that meeting will be inventions, because there is nothing operational to
   explain.

   **The plausibility test is the detection mechanism.** A single-period `CPI` of 0.6667 implies
   costs 50 % above budget for four weeks. Ask what physically happened to cause that; if the answer
   is thin, the cause is measurement, not production, and the ledger is where to look. This is MCQ
   7.2-A's step change with the arithmetic attached: **a step in `CPI` with no operational
   counterpart is a measurement event.**

   **The forecast error is the real damage, not the index.** Reported `TCPI` to `BAC` was 1.0000
   ("no recovery required"), while the true figure was 1.1064. A board told the project needs par
   performance to finish at budget, when it in fact needs an 11 % efficiency gain, has been given
   the opposite of a warning. And the reported `EAC(b)` of exactly `BAC` is the tell: **when `CPI`
   reads exactly 1.0000, check the accruals before celebrating**, because a real project rarely
   lands on the integer.

   **The cure is a control, not diligence.** `AC` must never be below invoices plus
   receipted-but-uninvoiced value, the reconciliation must be a signed month-end step, and the
   accrual basis must be stated in the report. The caution runs the other way too: an *over*-accrual
   flatters the following period exactly as symmetrically, so a project whose `CPI` improves in
   every period after a bad month should be asked whether the bad month was over-accrued.

**Worked example 7.2.1b — commitments, coverage and the exposure nobody reports.**

1. **Setup.** Auriga at week 13: `AC` **USD 2,120,000**; open purchase orders and subcontract
   commitments for work not yet received total **USD 1,180,000**. Of those open commitments, **USD
   120,000** relates to scope already delivered and invoiced: stale lines nobody has closed. The
   forecast in use is method (b), `EAC` **USD 4,416,667** (KA 7.3.3). Twelve weeks remain.
2. **Formula.** Uncommitted balance of the baseline = `BAC` − `AC` − open commitments. Commitment
   coverage = (`AC` + open commitments) ÷ `EAC`. Uncommitted balance of the *forecast* = `EAC` −
   `AC` − open commitments. Escalation exposure = uncommitted forecast balance × price movement.
3. **Substitution.** Baseline uncommitted `= 4,000,000 − 2,120,000 − 1,180,000`. Coverage
   `= 3,300,000/4,416,667`; after cleansing `= 3,180,000/4,416,667`. Forecast uncommitted
   `= 4,416,667 − 3,300,000`.
4. **Result.** The baseline leaves **USD 700,000** uncommitted; the forecast leaves **USD
   1,116,667**. Commitment coverage is **74.72 %** of the method-(b) forecast (78.57 % of the
   method-(a) forecast). Cleansing the USD 120,000 of stale lines moves coverage to exactly **72.00
   %** and raises the baseline's uncommitted balance to **USD 820,000**; while changing `AC`, `CPI`,
   `CV` and every `EAC` by nothing at all. The USD 1,116,667 still to be committed over twelve weeks
   is **USD 93,056 per week** of new commitment, and a 5 % adverse price movement on it is **USD
   55,833**: **27.92 %** of the entire cost variance the week-13 review is about to discuss.
5. **Interpretation.** The identity is the thing to remember, and it is exact:
   **USD 1,116,667 − USD 700,000 = USD 416,667 = |`VAC`(b)|.** The gap between what the baseline
   leaves to be bought and what the forecast says will actually have to be bought *is* the forecast
   variance at completion, seen from the funding side rather than the performance side. Two people
   arguing about whether the overrun is real are arguing about the same number twice, and the
   funding view is usually the one a sponsor accepts, because it is a statement about money not yet
   spent rather than a criticism of money already spent.

   **Commitment coverage answers a question `CPI` cannot: how much of the forecast is still exposed
   to prices?** At 74.72 % coverage, a quarter of the outturn is unfixed. That is the number to
   report beside `CPI` when a market is moving, and its sensitivity (5 % on the remainder being more
   than a quarter of the variance to date) is what makes escalation an *active* commercial matter
   rather than an estimating footnote. Long-lead items are where coverage is bought (Domain 10, KA
   10.1.3 prices the lead time itself).

   **Cleansing commitments makes the picture look worse and be more true, which is why it does not
   happen.** The stale USD 120,000 was silently claiming that scope was already secured. Removing it
   moves coverage down by 2.72 points and leaves an extra USD 120,000 visibly still to buy. Nothing
   in any performance index rewards the exercise; the whole return is a funding forecast that is not
   lying. That is precisely why it belongs in a standing month-end checklist with a named owner
   (Toolkit 7.T.1) rather than in anyone's judgement.

   The caution: commitment coverage is not a measure of performance and must never be read as one. A
   project can be 100 % committed and badly over budget, indeed committing early at bad prices is
   one way to get there. Coverage bounds *uncertainty* in the forecast, not its level.

### 7.2.2 The forecasting question

A forecast answers one question (*what will this cost when it is done?*), and the honest answer
depends entirely on **why** the current variance exists. That judgment is the leader's; the
arithmetic is KA 7.3.3's `EAC` family. The rule to internalise now: **a forecast is a statement
about the remaining work, not an extrapolation ritual.** A variance caused by a closed,
non-recurring event says nothing about what is left; a variance caused by a systemic productivity
shortfall says everything.

### 7.2.3 Baseline integrity

The baseline's authority rests on its stability. Four standing rules: budgets of completed or open
work packages are never retrospectively adjusted (except to correct an authorised error);
re-baselining happens only through change control, with an audit trail (Domain 4, KA 4.3); transfers
between control accounts are logged, not silent; and the baseline never absorbs a variance by moving
budget to where the money went. A baseline edited to match reality has stopped measuring anything:
the cost analogue of Domain 6's pinned milestone.

### AI in this KA

**Where it earns its place.** This knowledge area is where the most valuable automation in the whole
cost discipline sits, because everything here is reconciliation and nobody has time to do it by
hand. **Reconciling ledger actuals to the cost report** line by line, and producing the *list* of
differences rather than only the total, since the total is what a project already knows and the list
is what it can act on. **Identifying periods with no accrual posted against evidence that work was
received** (goods-receipt notes, timesheets, site records, progress certificates), which is exactly
the omission Worked example 7.2.1 costs at USD 416,667 of forecast error, and which is invisible in
any index until the invoice lands. **Sweeping open purchase orders** to compute commitment coverage
and to flag lines with no receipt movement for a stated period, which is the stale-commitment
hygiene of 7.2.1b that never happens because nothing in a performance index rewards it. And
**flagging control-account transfers not carried by a change reference**, which is 7.2.3's baseline
integrity rule expressed as a query rather than as an aspiration.

**Where it must not go.** Four determinations, and each of them moves money or moves the measurement
basis. **Determining whether work has been received**: a measurement judgement belonging to a named
human with the standing to make it, and the input on which every accrual rests. **Setting or
adjusting an accrual**: an accrual is an assertion about value received, and a model that estimates
one from a run rate has invented the very number the control exists to test. **Authorising a
control-account transfer**, which is a change decision under Domain 4, KA 4.3. And **re-baselining**
in any form, since a baseline edited to match reality has stopped measuring anything. A tool may
propose all four and may compute what each would do; the authorisation is a person's, recorded.

**Verification, concretely.** Three checks, each cheap and each reproducible by hand. **Recompute
the `AC`-plus-accrual total against the ledger** for the period and confirm it reconciles to the
signed month-end position: the control 7.2.1 names, run as arithmetic rather than as an assurance.
**Confirm the reversal in the following period**: an accrual is a timing entry, so it must reverse,
and an accrual that does not reverse is either a real cost that has been double-counted or an
adjustment wearing an accrual's clothing. That test is what makes 7.2.1's invariant (an accrual
omission cancels cumulatively and misstates every period in between) checkable rather than merely
stated. And **reproduce commitment coverage by hand for one period**, including the stale-line
cleanse, because a coverage figure that nobody has ever recomputed is the number most likely to be
quietly wrong in the direction that flatters. Where any of these feeds a report a board reads, the
verification card of Domain 1, KA 1.4.3 applies: state who verified it, how, and what the check
would have caught.

### Key terms — KA 7.2

| Term | Meaning |
|---|---|
| **`AC`** | Actual Cost of the work performed, including period accruals. |
| **Accrual** | Cost of work received but not yet invoiced, recognised in the period. |
| **Commitment** | A contractual obligation (e.g. a PO); a funding fact, not yet a cost. |
| **Commitment coverage** | (`AC` + open commitments) ÷ `EAC`: the share of the forecast whose price is already fixed; 74.72 % on Auriga. |
| **Uncommitted forecast balance** | `EAC` − `AC` − open commitments; its excess over the baseline's uncommitted balance equals \|`VAC`\|. |
| **Re-baselining** | Domain 4's instrument (KA 4.3.3) seen from earned value: it resets the `PV` curve every index is measured against, which is exactly why it cannot be used to retire an adverse variance. |

### Sample MCQs — KA 7.2

**MCQ 7.2-A `[7.2.1 · Analysis]`** A project's `CPI` has read 1.02 for four months; then one
month it drops to 0.91 with no change in productivity. The likeliest explanation is:
- A. genuine sudden inefficiency
- B. accruals were not being recognised, so earlier `AC` understated cost and this month absorbed the catch-up ✅
- C. the baseline was too generous
- D. `EV` was over-claimed this month

*Rationale:* A step change in `CPI` without an operational change points at measurement, and missing
accruals are the classic cause, earlier periods flattered, one period punished. D would raise, not
lower, the earlier readings' credibility, and C would show as a stable, not stepped, pattern.

**MCQ 7.2-B `[7.2.3 · Recall]`** Which action preserves baseline integrity?
- A. moving budget from an underspent control account to cover an overspend, without record
- B. re-baselining through change control with an audit trail when scope genuinely changes ✅
- C. adjusting a completed package's budget to match its actual cost
- D. reducing remaining budgets so the total still equals `BAC`

*Rationale:* Only governed change preserves the measurement. A is a silent transfer, C is
retrospective adjustment, D is the classic "make the numbers add up" manoeuvre, each destroys
comparability.

**MCQ 7.2-C `[7.2.1 · Application]`** `EV` is 1,920,000; invoiced cost is 1,920,000; a further
200,000 of work has been received and accepted but neither invoiced nor accrued. Reported and true
`CPI` are:
- A. 1.00 and 0.91 ✅
- B. 0.91 and 1.00
- C. 1.00 and 1.00
- D. 0.91 and 0.91

*Rationale:* The report divides `EV` by the understated `AC` of 1,920,000 (1.00); the truth divides
by 2,120,000 (0.9057, shown as 0.91) — 7.2.1. B reverses the two; C treats the receipted work as a
commitment rather than a cost, which is the substantive error the accrual rule exists to prevent; D
assumes the accrual was recognised, in which case there would be nothing to detect.

**MCQ 7.2-D `[7.2.1 · Analysis]`** In the month a 200,000 accrual backlog is finally invoiced, the
reported period `CPI` is 0.67 while true period performance was 0.92. The soundest reading is:
- A. productivity collapsed by a third in that period
- B. the period absorbed a timing correction: the cumulative index is now right and the period
  reading is not ✅
- C. `EV` was over-claimed in the period
- D. the baseline for that period was too generous

*Rationale:* Accrual omissions cancel cumulatively and misstate the periods in between (7.2.1), so
the cumulative `CPI` of 0.9091 is now correct while the period figure is an artefact. A takes the
artefact at face value and would launch an investigation into production that has nothing to find;
C would depress the *earlier* periods too, not this one alone; D would show as a stable bias rather
than a single-period step.

**MCQ 7.2-E `[7.2.1 · Application]`** `AC` is 2,120,000, open commitments are 1,180,000 and the
`EAC` in use is 4,416,667. The balance of the forecast still to be committed is:
- A. USD 700,000
- B. USD 1,116,667 ✅
- C. USD 1,316,667
- D. USD 3,236,667

*Rationale:* `4,416,667 − 2,120,000 − 1,180,000 = 1,116,667` (7.2.1b). A measures against `BAC`
rather than the forecast, and the 416,667 difference between A and B is exactly \|`VAC`(b)\|; C
uses `EV` in place of `AC`, double-counting the cost variance; D omits `AC` altogether.

### Self-check — KA 7.2

1. *Why must `AC` include accruals?* — So it covers the same work as `EV` in the same period;
   otherwise every index is misstated.
2. *Is a purchase order a cost?* — No: a commitment. It informs the funding forecast, not
   performance.
3. *What single question does a forecast answer?* — What the remaining work will cost, given why
   the variance exists.
4. *State the accrual invariant and what it implies for period reporting.* — An accrual omission is
   a timing error: it cancels cumulatively and misstates every period in between, so a period index
   with no operational counterpart is a measurement event, not a performance event.
5. *What does commitment coverage tell you that `CPI` cannot?* — How much of the forecast is still
   exposed to price movement: at Auriga's 74.72 %, a 5 % movement on the uncommitted remainder is
   USD 55,833, or 27.92 % of the cost variance to date.
6. *Why does cleansing stale commitments never improve a report?* — Because it changes no
   performance index and makes the funding forecast look worse while making it true, which is why it
   needs to be a checklist step with an owner rather than a matter of judgement.

---

## Knowledge Area 7.3 — Earned value: measurement, variances and forecasting

*Topics: 7.3.1 the three measures · 7.3.2 variances and indices · 7.3.3 the `EAC` family ·
7.3.4 `VAC` and `TCPI`.*

### 7.3.1 The three measures

**Definitions.** All three are expressed in the same budget currency so they are directly
comparable:

- **Planned Value (`PV`)**, the budgeted cost of the work *scheduled* by the data date: the
  time-phased baseline of KA 7.1.3.
- **Earned Value (`EV`)**, the budgeted cost of the work *actually performed*: physical progress
  **valued at the budget rate**, never at what it cost.
- **Actual Cost (`AC`)**: the cost actually incurred for that work, accruals included (KA 7.2.1).

**The single most important conceptual point:** `EV` is measured *at budget*. That is what lets
`EV` be compared with `PV` (both at budget → schedule progress) and with `AC` (budget vs actual
for the same work → cost efficiency). Confusing "value earned" with "cost incurred" collapses the
method.

**Earning rules.** How `EV` is claimed decides how much it can be gamed: **0/100** (nothing until
complete, objective, good for short packages); **50/50**; **percent complete** (needs an objective
basis); **units completed** (best where output is countable); **weighted milestones**; and **level
of effort**, where `EV` is set equal to `PV` by the calendar, so it can *never* show a schedule
variance. Level of effort dilutes whatever it is mixed into, formal practice segregates it and caps
its share of a control account.

How much difference the choice makes is the question that decides whether earning rules are a
technicality or a governance matter.

**Worked example 7.3.1 — one physical state, three earned values.**

1. **Setup.** A **USD 400,000** group of ten equal work packages inside Auriga's installation
   control account CA-40. At the data date: **4** packages complete, **3** in progress at an
   objectively measured **30 %** physical completion, **3** not started. The baseline expected
   **6** complete by now, so `PV` = **USD 240,000**. Actual cost for the group is
   **USD 210,000**. Compute `EV`, `SPI` and `CPI` under 0/100, 50/50 and objective
   percent-complete earning.
2. **Formula.** 0/100: `EV = ` complete packages × package budget. 50/50: complete × budget +
   in-progress × budget/2. Percent complete: complete × budget + Σ(in-progress physical % ×
   budget). Then `SPI = EV/PV`, `CPI = EV/AC`.
3. **Substitution.** 0/100 `= 4 × 40,000`. 50/50 `= 160,000 + 3 × 20,000`. Percent complete
   `= 160,000 + 3 × 12,000`.
4. **Result.**

   | Earning rule | `EV` | `SPI` | `CPI` | Reads as |
   |---|---|---|---|---|
   | 0/100 | 160,000 | **0.6667** | **0.7619** | badly late, badly over |
   | 50/50 | 220,000 | **0.9167** | **1.0476** | slightly late, **under budget** |
   | Percent complete (objective, 30 %) | 196,000 | **0.8167** | **0.9333** | late and over |

   The spread in `EV` is **USD 60,000** (**15.00 %** of the group's budget), and the `SPI` spread is
   **0.25**, on a physical state that did not change between rows.
5. **Interpretation.** The decisive observation is in the third column of the middle row. **The
   earning rule alone flips the account from over-running (`CPI` 0.9333) to under-running (`CPI`
   1.0476).** Nothing was misreported, nothing was manipulated, and the two readings support
   opposite management decisions. That is why the earning rule is a control, not a preference.

   **The direction of each rule's bias is predictable, so a reviewer can compute it.** 50/50
   overstates by **USD 24,000** (12.24 % of the objective figure), because the in-progress work is
   only 30 % done; 0/100 understates by **USD 36,000**, or 18.37 %. The breakeven is exact and worth
   carrying: **50/50 is unbiased precisely when in-progress work averages 50 % physical
   completion**, and at genuine 50 % it and objective percent-complete agree at USD 220,000; while
   0/100 still reads 160,000, because it credits nothing until a package closes. So 50/50 flatters a
   control account whose in-progress work is young, which is the normal condition of an account that
   has just started, and exactly when a project most wants reassurance.

   **Which rule to choose follows from package duration, not from taste.** 0/100 is objective and
   cheap and its understatement is harmless where packages are short relative to the reporting
   period: a package that starts and finishes inside a month never sits half-earned at a data date.
   For long packages 0/100 produces a sawtooth that hides real progress, and objective percent
   complete earns its administrative cost. 50/50 is a convenience that should be reserved for
   packages short enough that the error cannot matter, and its use on long packages is the single
   commonest source of an over-stated `EV`.

   **The rule must be fixed before the period it measures.** A rule chosen after the data date is
   not a measurement convention but a selection among answers, and the 15 % spread above is the size
   of the discretion involved. This is the cost analogue of Domain 6's pinned milestone: the defect
   is not arithmetic but the loss of a fixed reference. State the rule per package in the plan,
   disclose any change, and, the reviewer's test, recompute one account under a different rule to
   see how much of the reported position is the convention rather than the work.

**The level-of-effort distortion, computed.** Because level of effort sets `EV ≡ PV`, a control
account that mixes it with discrete work reports a blend, and the blend has a closed form. With `s`
the level-of-effort share of the account's budget and `SPI_d` the discrete work's own schedule
performance index:

```
reported SPI = s × 1 + (1 − s) × SPI_d = 1 − (1 − s)(1 − SPI_d)
distortion   = reported SPI − SPI_d    = s × (1 − SPI_d)
```

An account 70 % level of effort whose discrete work is running at `SPI_d` **0.60** reports **0.88**:
a distortion of **0.28**, which is to say the report conceals nearly all of the problem. The same
account under a **20 %** level-of-effort cap would report **0.68**, a distortion of **0.08**. And
the share required to make a discrete `SPI` of 0.60 report as 0.95 or better is `s ≥ 0.875`, so
**87.5 % level of effort makes a two-fifths schedule shortfall disappear into a figure a steering
committee would wave through.** The distortion is *linear in the level-of-effort share*, which is
precisely why the countermeasure is a cap expressed as a share of budget, and why the share must be
disclosed per control account rather than for the project as a whole.

### 7.3.2 Variances and indices

```
CV = EV − AC        SV = EV − PV
CPI = EV / AC       SPI = EV / PV
```

**Worked example 7.3.2 — Auriga at week 13.**

1. **Setup.** `BAC` = 4,000,000; at the data date `PV` = 2,080,000, `EV` = 1,920,000,
   `AC` = 2,120,000.
2. **Formula.** As above; variances in currency, indices as ratios (two decimals).
3. **Substitution.** `CV = 1,920,000 − 2,120,000`; `SV = 1,920,000 − 2,080,000`;
   `CPI = 1,920,000/2,120,000`; `SPI = 1,920,000/2,080,000`.
4. **Result.** `CV` = **(USD 200,000)** · `SV` = **(USD 160,000)** · `CPI` = **0.91** ·
   `SPI` = **0.92**. Auriga is **48.0 % complete** (`EV/BAC`) having spent **53.0 %** of budget
   (`AC/BAC`).
5. **Interpretation.** Behind and over: for every dollar spent the project is producing 91 cents of
   budgeted work, and it has delivered 92 % of what it planned to by now. The percent-complete
   versus percent-spent pair (48 vs 53) is the same story in the form a board absorbs fastest. Note
   this is the *cost* view of the very slippage Domain 6's case study recovered on the schedule
   side. The two domains are describing one project.

   **The percent-complete/percent-spent gap is the cost variance, exactly.** `%spent − %complete =
   (AC − EV)/BAC`, so the 5.00-point gap between 53.00 % and 48.00 % **is** the USD 200,000, and the
   identity holds always. That makes the two percentages a legitimate board summary rather than a
   simplification: nothing is lost in the translation, and a report where the gap and the `CV`
   disagree has an arithmetic error in it.

   **Express the overrun as a rate, because a rate can be projected and a total cannot.** The work
   performed so far cost **10.42 %** more than it was budgeted to: `1/CPI − 1 = 1/0.905660 − 1`.
   That is the same 10.42 % that appears again as the ratio between removed scope and saved cash in
   KA 7.3.4, and it is the number that scales: USD 200,000 is a fact about the past, 10.42 % is a
   claim about the production process.

   **Express the schedule variance in time, because currency conceals the size of it.** Auriga's
   `SV` of (USD 160,000) is exactly one week of planned value at this point on the curve, where the
   baseline is rising at USD 160,000 per week. So `SV` (160,000) means **one week behind**, which is
   both more useful and more challengeable than a currency figure, and it is the bridge to earned
   schedule in 7.A.1. The general caution: `SV` in currency is not comparable between projects or
   even between phases of one project, because it is scaled by the local slope of the `PV` curve.

   **Both indices are cumulative, so they lag, and neither says whether things are improving.** A
   cumulative `CPI` of 0.91 at 48 % complete cannot distinguish a project that has stabilised from
   one still deteriorating; only the period index can, and 7.2.1 shows how easily the period index
   is corrupted. The reviewer's pairing is therefore cumulative index for level and period index
   for direction, with the accrual reconciliation done before either is believed. And the whole set
   rests on the earning rules of 7.3.1: on the arithmetic there, a 15 % `EV` swing was available
   from the convention alone, which is larger than the variance being discussed here.

> **Fig 7.3.1 — Auriga's earned-value S-curves at week 13.** Line chart, x-axis weeks 0–25,
> y-axis cumulative USD 0–4.4m. Three cumulative series to the data date: `PV` rising to
> 2,080,000 at week 13 and on to `BAC` 4,000,000 at week 25; `EV` to 1,920,000; `AC` to
> 2,120,000. Vertical dashed data-date line at week 13 with `CV` and `SV` annotated as the
> vertical gaps (AC−EV = 200,000; PV−EV = 160,000). Forecast continuations to `EAC` shown dotted.
> Source: PCI original. Alt text: three cumulative cost curves diverging at the week-thirteen data
> date, with actual cost above and earned value below the planned value curve.

### 7.3.3 The `EAC` family — forecasting

```
ETC = the remaining work's forecast cost          EAC = AC + ETC
(a) EAC = AC + (BAC − EV)                 remaining work at the BUDGETED rate
(b) EAC = BAC / CPI                       current cost efficiency PERSISTS
(c) EAC = AC + (BAC − EV) / (CPI × SPI)   cost AND schedule pressure compound
(d) EAC = AC + bottom-up ETC              re-estimate the remainder from scratch
```

**Worked example 7.3.3 — the same project, four futures.**

1. **Setup.** Auriga's week-13 figures, forecast to completion by each method.
2. **Formula.** (a)–(c) above; `CPI` = 0.905660, `SPI` = 0.923077 at full precision.
3. **Substitution.** (a) `2,120,000 + (4,000,000 − 1,920,000)`;
   (b) `4,000,000 / 0.905660`;
   (c) `2,120,000 + 2,080,000/(0.905660 × 0.923077)`.
4. **Result.** (a) **USD 4,200,000** · (b) **USD 4,416,667** · (c) **USD 4,608,056**.
5. **Interpretation.** The same four measured numbers support forecasts spanning **USD 408,056**:
   over 10 % of `BAC`. The spread is not uncertainty in the arithmetic; it is the *assumption* about
   the remaining work, made explicit. Choosing among them is the leader's judgment, and it must be
   stated: if Auriga's overrun was the contaminated-ground remediation of Domain 6 (a discrete,
   now-closed event) method (a) is right and the others over-forecast. If it reflects a systemic
   productivity shortfall that will persist, (b). If schedule pressure is now driving overtime and
   expediting, (c). A forecast presented without its assumption named is a number, not a forecast.

   **Scale the spread against the remaining work, not against `BAC`.** USD 408,056 is 10.20 % of
   `BAC` and **19.62 %** of the USD 2,080,000 still to be earned. The second figure is the honest
   one, because the disagreement is entirely about the remainder, the methods agree exactly about
   the past. A disagreement of a fifth about the work still to do is a large disagreement, and
   describing it as "about ten per cent" understates it by half.

   **Every method is an implied efficiency for the remaining work, and stating it that way ends most
   arguments.** Method (a) assumes the remainder runs at **1.0000**: budgeted rate, i.e. a 10.42 %
   improvement on demonstrated performance. Method (b) assumes **0.9057**, today's `CPI` exactly.
   Method (c) assumes **0.8360**, the product `CPI × SPI`. So the choice is not between three
   formulae but between three claims about productivity, and each is falsifiable. The gap between
   (b) and (c) — **USD 191,389** — is the price the forecast puts on schedule pressure alone; asking
   "who is claiming that recovering the date will cost a further **6.97 points** of efficiency on
   top of the **9.43** already lost — **16.40 %** below the budgeted rate in total, not on top of it
   — and through what mechanism?" is a better question than comparing formula names.

   **Method (c) is a different hypothesis, not more conservative.** `CPI × SPI` compounds two
   indices that are not independent and were never designed to be multiplied, and it is defensible
   only where a specific, describable mechanism links lateness to cost — overtime, expediting
   premiums, out-of-sequence work, extended site establishment. Where that mechanism exists, (c) is
   right and (b) under-forecasts. Where it does not, (c) is pessimism with an equation on it, and it
   does the same damage as optimism: it destroys the credibility of the next forecast.

   **Method (d) is the only one whose basis can be checked, and the only one that can be lower than
   (a).** A bottom-up re-estimate of the remainder can legitimately land below USD 4,200,000
   (because the remaining scope may be less exposed than the scope that overran), and no
   index-extrapolation method can ever produce that. Its cost is real (it takes a team a week or
   more, and it consumes the same engineers who are delivering) and it is the only method that
   survives cross-examination at a funding gate. The professional practice is therefore: report the
   index methods weekly as a *trend*, and commission (d) whenever the decision at stake is money.

**Worked example 7.3.3b — where each forecast's funding actually runs out.**

1. **Setup.** The board must authorise funding, and it will authorise the `EAC` it is given. Assume
   the demonstrated `CPI` of **0.905660** persists. For each forecast, how much budgeted work does
   the money it authorises actually buy, and at what point does the project stop?
2. **Formula.** Funded `ETC` = `EAC` − `AC`. Budgeted work that `ETC` buys at efficiency `e` =
   `ETC × e`. `EV` at exhaustion = `EV` + that amount; unfunded budgeted work = `BAC` − `EV` at
   exhaustion. Weeks short = unfunded work ÷ the baseline's average planned rate over the remaining
   weeks.
3. **Substitution.** Method (a): `ETC = 4,200,000 − 2,120,000 = 2,080,000`; buys
   `2,080,000 × 0.905660`. Remaining planned rate `= 2,080,000/12` over weeks 14–25.
4. **Result.**

   | Forecast | Funded `ETC` | Budgeted work it buys at `CPI` 0.9057 | `EV` reached | Position |
   |---|---|---|---|---|
| (a) 4,200,000 | 2,080,000 | 1,883,774 | 3,803,774 | **95.09 % complete (money gone**) |
   | (b) 4,416,667 | 2,296,667 | 2,080,000 | 4,000,000 | **exactly complete** |
   | (c) 4,608,056 | 2,488,056 | 2,253,333 | 4,173,333 | over-funded by 173,333 of budget-equivalent |

   Method (a) leaves **USD 196,226** of budgeted work unfunded: **4.91 %** of `BAC`, and at the
   baseline's average remaining rate of USD 173,333 per week, **1.13 weeks** of work.
5. **Interpretation.** The result is a sentence a sponsor cannot misunderstand: **on its own
   demonstrated performance, the method-(a) forecast funds Auriga to about one and a bit weeks short
   of the finish line.** That is a far more useful statement than "the forecast may be USD 216,667
   light", because it names when the problem arrives and what it will feel like, a second funding
   request at 95 % complete, which is the worst moment in a project's life to ask for money, since
   every alternative to paying has already expired. Method (b)'s exactness is not a coincidence but
   the definition of the method: `BAC/CPI` is precisely the funding that completes the scope at
   today's efficiency, which is the same identity KA 7.3.4 finds from the `TCPI` side.

   **The asymmetry of forecast error is the governance point.** Over-forecasting costs the
   organisation the opportunity value of committed but unspent funds, real, and recoverable at the
   next portfolio review (Domain 15). Under-forecasting costs a mid-project funding crisis at the
   moment of maximum sunk cost and minimum optionality, which is where escalation of commitment
   (Domain 2, KA 2.4.2) does its worst work. **The two errors are not symmetric, so a forecast
   should not be chosen as though they were**, and a leader who genuinely cannot distinguish between
   causes should say so and fund nearer (b) than (a) while commissioning method (d).

   **Method (c)'s "over-funding" is only over-funding if efficiency does not deteriorate.** Its USD
   2,488,056 buys USD 2,253,333 of budgeted work at `CPI` alone (an apparent surplus of USD
   173,333), but at the compounded rate of 0.8360 it completes the scope exactly, to the dollar.
   That is the test of whether (c) is right: it is the correct number if and only if the compounded
   efficiency materialises. Presenting (c) without that conditional is how a project acquires a
   reputation for hoarding contingency, and presenting (a) without the conditional above is how it
   acquires a reputation for surprises.

### 7.3.4 `VAC` and `TCPI`

```
VAC = BAC − EAC                          the forecast overrun/underrun
TCPI = (BAC − EV) / (BAC − AC)           efficiency the REMAINING work must achieve to hit BAC
TCPI = (BAC − EV) / (EAC − AC)           …to hit a revised EAC
```

**Worked example 7.3.4 — what recovery would actually require.**

1. **Setup.** Auriga's week-13 figures; test recovery to `BAC`, and to the method-(b) forecast.
2. **Formula.** `VAC = BAC − EAC`; `TCPI` as above.
3. **Substitution.** `VAC(a) = 4,000,000 − 4,200,000`; `VAC(b) = 4,000,000 − 4,416,667`.
   `TCPI(BAC) = 2,080,000 / (4,000,000 − 2,120,000) = 2,080,000/1,880,000`.
   `TCPI(EAC b) = 2,080,000 / (4,416,667 − 2,120,000)`.
4. **Result.** `VAC(a)` = **(USD 200,000)** · `VAC(b)` = **(USD 416,667)**.
   `TCPI` to `BAC` = **1.11**; `TCPI` to the (b) forecast = **0.91**.
5. **Interpretation.** To finish at budget, the remaining 52 % of work must run at **1.11**: an 11 %
   efficiency *gain* by a team currently achieving 0.91. That is a 22-point swing, and the honest
   question in the room is what specifically would deliver it. Meanwhile `TCPI` to the (b) forecast
   is 0.91, exactly today's `CPI`, which is the arithmetic identity worth internalising:
   **forecasting with `BAC/CPI` is precisely the assumption that nothing changes.** `TCPI` is the
   reality check on every recovery promise: a required index far above demonstrated performance is a
   plan that needs a mechanism, not encouragement.

   **State the swing as a ratio, not a difference.** "1.11 against 0.91" understates the demand,
   because the two are a ratio: `1.106383/0.905660 =` **1.2216**, so the remaining work must run
   **22.16 %** more efficiently than everything done so far. A 22 % productivity improvement,
   mid-project, on the same scope with the same team and the same suppliers, is not a management
   intention; it is an engineering change, a scope change or a commercial change, and the recovery
   plan should name which.

   **The identity generalises, and once it is seen the `EAC` family documents itself.** `TCPI`
   computed to any `EAC` equals the efficiency that `EAC` assumes for the remaining work:

   | Target | `TCPI` = (`BAC` − `EV`) ÷ (target − `AC`) | Equals |
   |---|---|---|
   | `BAC` 4,000,000 | 2,080,000 / 1,880,000 = **1.1064** | the recovery demand |
| `EAC`(a) 4,200,000 | 2,080,000 / 2,080,000 = **1.0000** | budgeted rate, method (a)'s assumption |
| `EAC`(b) 4,416,667 | 2,080,000 / 2,296,667 = **0.9057** | `CPI`, method (b)'s assumption |
| `EAC`(c) 4,608,056 | 2,080,000 / 2,488,056 = **0.8360** | `CPI × SPI`, method (c)'s assumption |

   **Choosing an `EAC` is choosing a `TCPI`**, so the two should always be reported as a pair; and
   any `EAC` whose `TCPI` differs from the efficiency its author claims to be assuming contains an
   arithmetic error. That single check catches more defective cost reports than any other in this
   domain, and it takes one division.

   The caution about `TCPI` runs the other way as well. A `TCPI` **below** demonstrated `CPI` is not
   good news. It means the target has already been relaxed to something easier than current
   performance, which is what re-baselining to the forecast does, and it is how a project comes to
   report favourable indices against a target nobody would have approved at sanction (7.2.3).

**Worked example 7.3.4b — what `TCPI` 1.11 costs in scope, not in exhortation.**

1. **Setup.** The board will not authorise more than the original **USD 4,000,000** of cost.
   Auriga's team is achieving `CPI` **0.905660** and can offer no mechanism to improve it. The only
   remaining lever is scope: how much budgeted work must come out for the recovery to be arithmetic
   rather than aspiration?
2. **Formula.** With `D` the budgeted value of scope removed and the cost target `T`:
   `TCPI = (BAC − D − EV)/(T − AC)`. Setting `TCPI = CPI` and solving,
   `D = (BAC − EV) − CPI × (T − AC)`.
3. **Substitution.** `D = 2,080,000 − 0.905660 × (4,000,000 − 2,120,000) = 2,080,000 − 1,702,641`.
4. **Result.** `D` = **USD 377,358** (exactly `20,000,000/53`), which is **9.43 %** of `BAC` and
   **18.14 %** of the work remaining. For comparison, completing the full scope at the demonstrated
   efficiency needs **USD 416,667** of additional funding (that is `EAC`(b) − `BAC`, i.e.
   \|`VAC`(b)\|).
5. **Interpretation.** The identity between those two figures is the one to carry out of this
   domain: `416,667/377,358 =` **1.104167 = 1/`CPI`**. **At a `CPI` of 0.9057, one dollar of
   budgeted scope removed saves USD 1.10 of forecast cash**; because the scope you do not do is
   scope you do not do inefficiently. That converts a vague recovery conversation into a single
   exchange rate the sponsor can act on, and it explains why descoping is a more powerful lever than
   its size suggests: the leverage is exactly `1/CPI`, and it grows as performance worsens.

   **The number reframes the meeting.** "We need a 22 % efficiency improvement" invites optimism.
   "Recovery to the approved 4,000,000 requires USD 377,358 of budgeted scope to come out, and here
   are the three candidate packages" invites a decision, with a named owner, at the authority that
   owns scope. `TCPI` is what makes the second sentence available.

   **Three cautions, and the first is the one that gets skipped.** Removing 9.43 % of budgeted scope
   does not remove 9.43 % of the benefit, and it may remove far more: on a control-systems upgrade
   the cheap-to-build packages are frequently the ones the operating case depends on. Every descope
   must be priced against benefit (Domain 2, KA 2.3) and tested against acceptance criteria (Domain
   5, KA 5.4.2) before its cost saving is booked. Second, a "descope" that is really a **deferral**
   does not save the money at all. It moves it to a later period and usually a higher price, which
   makes it a portfolio decision (Domain 15) rather than a project one, and the honest report says
   so. Third, the arithmetic assumes the removed scope carries its budgeted cost at the same
   demonstrated efficiency; removing scope from a package that was running *well* saves less than
   the formula says, and the sequence for choosing candidates therefore starts from the accounts
   with the worst performance, not the ones easiest to argue for.

> **Fig 7.3.2 — The EAC fan and what TCPI demands.** Two-panel figure. Left: Auriga's forecast fan
> from the week-13 data date, three dotted continuations to 4,200,000 (budgeted rate), 4,416,667
> (`BAC/CPI`) and 4,608,056 (`CPI×SPI`), with `BAC` 4,000,000 as a horizontal reference. Right: a
> bar pair (demonstrated `CPI` 0.91 against required `TCPI` 1.11 to recover to `BAC`) with the 0.20
> gap annotated "the gap a recovery plan must actually close". Source: PCI original. Alt text: a fan
> of three cost forecasts rising above the budget line, beside bars contrasting achieved cost
> efficiency with the higher efficiency needed to recover.

### AI in this KA

Earned value is arithmetic on four numbers, so machine computation is safe and instant verification
is cheap, which makes unverified AI output indefensible here rather than merely risky. The
deterministic checks: `EV/BAC` must equal the claimed percent complete; `CV = EV − AC` and `CPI =
EV/AC` recomputed independently; `TCPI` to `BAC/CPI` must equal `CPI` (7.3.4's identity); `TCPI` to
each of the other `EAC`s must equal that method's own implied efficiency (1.0000, `CPI`, `CPI ×
SPI`), which is a four-division audit of the whole forecast set; the
percent-spent-minus-percent-complete gap must reproduce `CV/BAC`; and every `EAC` must state its
assumption. A model asked to "check the earned-value numbers" will confirm arithmetic and miss the
two things that actually go wrong: an earning rule applied inconsistently between periods, and a
level-of-effort share that makes the reported `SPI` meaningless (7.3.1). Both are questions about
convention rather than computation, so both stay with the professional. Where AI forecasts from
trend data, the calibration rule of Domain 6 (KA 6.4) applies unchanged: show the record of past
forecasts against outturn before the number enters a board pack, and name the human who owns it.

### Key terms — KA 7.3

| Term | Meaning |
|---|---|
| **`PV` `EV` `AC`** | Budgeted cost of work scheduled · performed · the cost actually incurred. |
| **`CV` `SV`** | `EV − AC` · `EV − PV`, in currency. |
| **`CPI` `SPI`** | `EV/AC` · `EV/PV`, as ratios. |
| **`EAC` `ETC`** | Estimate at / to complete; `EAC = AC + ETC`. |
| **`VAC`** | `BAC − EAC`, forecast variance at completion. |
| **`TCPI`** | Efficiency the remaining work must achieve to hit a stated target. |
| **Level of effort** | Earning by calendar (`EV ≡ PV`); can never show schedule variance. |
| **Earning rule** | The convention by which physical progress becomes `EV`; fixed before the period it measures. On Auriga's ten-package group the choice moved `EV` by 15.00 % of budget. |
| **Level-of-effort distortion** | `s × (1 − SPI_d)`: the amount a level-of-effort share `s` adds to a reported `SPI`; 0.28 at a 70 % share with discrete `SPI` 0.60. |
| **Implied `ETC` efficiency** | The efficiency an `EAC` assumes for the remaining work; equal to the `TCPI` computed to that `EAC`. |
| **Descope exchange rate** | `1/CPI`, the forecast cash saved per unit of budgeted scope removed; USD 1.10 per dollar on Auriga. |

### Sample MCQs — KA 7.3

**MCQ 7.3-A `[7.3.2 · Application]`** `BAC` 4,000,000; `PV` 2,080,000; `EV` 1,920,000;
`AC` 2,120,000. `CPI` and `SPI` are:
- A. 0.91 and 0.92 ✅
- B. 1.10 and 1.08
- C. 0.92 and 0.91
- D. 0.91 and 1.02

*Rationale:* `CPI = 1,920,000/2,120,000 = 0.91`; `SPI = 1,920,000/2,080,000 = 0.92`. B inverts
both ratios; C swaps them (dividing `EV` by the wrong denominator); D miscomputes `SPI` against
`BAC`-derived progress.

**MCQ 7.3-B `[7.3.3 · Analysis]`** The overrun was caused by a one-off ground-remediation event,
now closed, and the remaining work is expected to run to budget. The appropriate `EAC` is:
- A. `AC + (BAC − EV)` = 4,200,000 ✅
- B. `BAC/CPI` = 4,416,667
- C. `AC + (BAC − EV)/(CPI × SPI)` = 4,608,056
- D. `BAC` = 4,000,000

*Rationale:* A discrete, closed cause makes the variance **atypical**, so remaining work is forecast
at the budgeted rate. B assumes the inefficiency persists and C that it compounds with schedule
pressure, both contradict the stated cause. D ignores money already spent above budget.

**MCQ 7.3-C `[7.3.4 · Application]`** With `BAC` 4,000,000, `EV` 1,920,000 and `AC` 2,120,000,
the `TCPI` required to complete at `BAC` is:
- A. 0.91
- B. 1.00
- C. 1.11 ✅
- D. 1.08

*Rationale:* `(4,000,000 − 1,920,000)/(4,000,000 − 2,120,000) = 2,080,000/1,880,000 = 1.11`. A is
the demonstrated `CPI`; B assumes recovery needs only par performance; D uses `PV` in place of `AC`
in the denominator: `2,080,000/1,920,000 = 1.08`; and so makes recovery look nearly free (Exercise
7.3).

**MCQ 7.3-D `[7.3.1 · Analysis]`** A control account is 70 % level-of-effort by budget. Its
reported `SPI` of 1.00 most likely means:
- A. the account is exactly on schedule
- B. little about schedule: level of effort earns by the calendar, so `EV ≡ PV` for most of the account regardless of progress ✅
- C. the discrete work is ahead, offsetting a delay
- D. the earning rules were misapplied

*Rationale:* LOE sets `EV` equal to `PV` by construction, so a heavily-LOE account reads 1.00
whatever happens, which is why practice segregates and caps it. C invents an offset the data cannot
show; the rules may have been applied entirely correctly (D), and that is the problem.

**MCQ 7.3-E `[7.3.1 · Application]`** Ten equal work packages of 40,000: four complete, three at an
objectively measured 30 % physical completion, three not started. `EV` under 50/50 and under
objective percent-complete earning is:
- A. 160,000 and 196,000
- B. 220,000 and 196,000 ✅
- C. 220,000 and 220,000
- D. 196,000 and 220,000

*Rationale:* 50/50 credits half of each in-progress package (`160,000 + 60,000`); percent complete
credits the measured 30 % (`160,000 + 36,000`) — 7.3.1. A gives the 0/100 figure for the first rule;
C assumes the in-progress work is genuinely 50 % done, which is the only condition under which 50/50
is unbiased; D transposes the two.

**MCQ 7.3-F `[7.3.1 · Analysis]`** A control account is 70 % level of effort by budget and its
discrete work is running at `SPI` 0.60. The account's reported `SPI` is:
- A. 0.60
- B. 0.88 ✅
- C. 1.00
- D. 0.42

*Rationale:* `1 − (1 − 0.70)(1 − 0.60) = 0.88`, a distortion of `0.70 × 0.40 = 0.28` (7.3.1). A
ignores that the level-of-effort portion reads 1.00 by construction; C assumes it swamps the
account entirely; D multiplies the share by the discrete index instead of blending them.

**MCQ 7.3-G `[7.3.4 · Analysis]`** For which target is `TCPI` exactly 1.00 on Auriga's week-13
figures?
- A. `BAC` = 4,000,000
- B. `AC + (BAC − EV)` = 4,200,000 ✅
- C. `BAC/CPI` = 4,416,667
- D. `AC + (BAC − EV)/(CPI × SPI)` = 4,608,056

*Rationale:* `TCPI` to any `EAC` equals the efficiency that `EAC` assumes, and method (a) assumes
the budgeted rate (7.3.4). A gives 1.1064, the recovery demand; C gives `CPI` 0.9057; D gives `CPI ×
SPI` 0.8360: each the assumption of its own method.

**MCQ 7.3-H `[7.3.3 · Evaluation]`** The board authorises the method-(a) forecast of 4,200,000 and
the demonstrated `CPI` of 0.9057 persists. The funding is exhausted at:
- A. 100.00 % complete
- B. 95.09 % complete ✅
- C. 90.57 % complete
- D. 52.00 % complete

*Rationale:* The funded `ETC` of 2,080,000 buys `2,080,000 × 0.905660 = 1,883,774` of budgeted work,
taking `EV` to 3,803,774 (7.3.3b). A is the position only if the remainder runs at the budgeted rate,
which is the assumption being tested; C applies `CPI` to the whole of `BAC` rather than to the funded
remainder; D is the share of work remaining, not a completion point.

### Self-check — KA 7.3

1. *Why is `EV` measured at budget, not at cost?* — So it is comparable with `PV` (schedule) and
   with `AC` (efficiency); at cost it would collapse into `AC`.
2. *What does `TCPI` = 1.11 against `CPI` = 0.91 tell a leader?* — Recovery to budget requires a
   22-point efficiency swing; the plan needs a named mechanism, not optimism.
3. *State the identity linking `TCPI` and `BAC/CPI`.* — `TCPI` to an `EAC` of `BAC/CPI` equals
   the current `CPI`: that forecast *is* the assumption that nothing changes.
4. *Generalise that identity.* — `TCPI` to any `EAC` equals the efficiency that `EAC` assumes for
   the remaining work: 1.0000 for method (a), `CPI` for (b), `CPI × SPI` for (c). Choosing an `EAC`
   is choosing a `TCPI`, so report them as a pair.
5. *How much scope must come out for Auriga to finish at 4,000,000 without improving?* —
   USD 377,358 of budgeted work, 9.43 % of `BAC`; and because a dollar of budget removed saves
   `1/CPI` = USD 1.10 of cash, that is equivalent to the USD 416,667 of extra funding method (b)
   requires.
6. *When is 50/50 earning unbiased, and when does it flatter?* — Unbiased when in-progress work
   averages exactly 50 % physical completion; it flatters whenever that average is lower, which is
   the normal state of a recently started account.

---

## Knowledge Area 7.4 — Resource economics, procurement strategy and cash

*Topics: 7.4.1 resource economics and blended rates · 7.4.2 contract models and cost risk ·
7.4.3 incentive fees and the point of total assumption · 7.4.4 cash flow versus profit.*

### 7.4.1 Resource economics and blended rates

Most project cost is people. A leader who reasons in headcount rather than **cost per unit of
capability** will mis-price every option in Domain 6's compression decisions.

**Worked example 7.4.1 — Auriga's engineering blended rate.**

1. **Setup.** The engineering pool is 40 technicians at USD 95/hour, 25 engineers at
   USD 140/hour, 15 senior specialists at USD 210/hour.
2. **Formula.** Blended rate = Σ(count × rate) / Σ(count).
3. **Substitution.** `(40×95 + 25×140 + 15×210) / 80 = (3,800 + 3,500 + 3,150)/80 = 10,450/80`.
4. **Result.** **USD 130.63 per hour** (130.625 exactly; ≈ SAR 490 indicatively).
5. **Interpretation.** The blended rate makes options commensurable: a week of schedule compression
   staffed from this pool costs a computable amount, and *shifting the mix* changes the price as
   much as changing the headcount, adding five specialists moves the blend more than adding five
   technicians. This is the number that turns Domain 6's "crash it" instruction into an
   expected-value decision.

   **The mix moves the price more than the size does, and the arithmetic ranks the moves.**
   Re-grading five technicians into specialists (same 80 people) takes the blend to **USD
   137.8125**, **+5.50 %**. *Adding* five specialists to make 85 gives USD 135.2941, **+3.57 %**.
   Adding five technicians gives USD 128.5294, **−1.60 %**. So the ordering is re-grading, then
   growing at the top, then growing at the bottom, and a leader who negotiates headcount while the
   mix moves underneath them has not negotiated the cost at all. The practical control is to fix the
   *mix* in the plan, not just the number, and to report it.

   **The engineer-week that follows is the unit the rest of the book uses.** `130.625 × 40 =`
   **USD 5,225** per engineer-week (≈ SAR 19,594 indicatively), which is the figure Domain 9,
   KA 9.2.3 uses to cost rework capacity and Domain 8 uses in response pricing. Registering the
   rate once and reusing it is what makes those numbers comparable across domains.

   **The blend is only valid for work drawn in the pool's proportions, and the error when it is not
   is large.** A task performed entirely by senior specialists costs USD 210 per hour: **60.77 %**
   above the blend, an under-pricing of USD 79.375 an hour if the blend is used. That is the
   commonest misuse of a blended rate: applying it to a small, senior-heavy activity such as
   commissioning support or an interface investigation, where it can understate cost by more than
   half again. The discipline is to blend within a *grade-homogeneous* scope and to price
   senior-only work at senior rates.

**Worked example 7.4.1b — what an hour of capacity actually costs.**

1. **Setup.** The USD 95 per hour in the estimate above is a charge-out rate. Auriga's finance team
   builds the underlying cost of a technician hour: base pay **USD 55.00** per paid hour; statutory
   and employer on-costs **32 %**; **15 %** of paid hours are non-productive (leave, training,
   standby, weather, travel); recoverable overhead — site establishment, supervision, tools,
   information systems, **18 %**. What does one *productive* hour cost, and what does the estimate's
   rate actually recover?
2. **Formula.** Paid-hour cost `= base × (1 + on-costs)`. Cost per productive hour
   `= paid-hour cost ÷ (1 − non-productive share) × (1 + overhead)`. The paid-to-productive
   multiplier is `1/(1 − n)`.
3. **Substitution.** `55.00 × 1.32 = 72.60`; `72.60/0.85 = 85.4118`; `85.4118 × 1.18`.
4. **Result.** Paid-hour cost **USD 72.60**; per productive hour before overhead **USD 85.4118**;
   **full cost per productive hour USD 100.7859**. The paid-to-productive multiplier is **1.17647**.
   Against the USD 95.00 charge-out rate the estimate **under-recovers by USD 5.7859 an hour, 5.74 %
   of cost.**
5. **Interpretation.** The first duty is to know which rate a number is. **A rate quoted per *paid*
   hour understates the cost of a productive hour by `1/(1 − n)` (17.65 % here), and the two are
   confused constantly**, because both are called "the labour rate". An estimate built on paid-hour
   rates against a schedule built on productive hours is under-funded by that multiplier before any
   risk materialises, and the discrepancy is invisible because both documents look right.

   **Utilisation is the most leveraged number on the sheet and nobody owns it.** Cost per productive
   hour scales as `1/utilisation`, so losing five points from 85 % to 80 % raises it to **USD
   107.0850**: **+6.25 %**, exactly the ratio `0.85/0.80`. On Auriga, where labour is 62 % of `BAC`
   (**USD 2,480,000**) that is **USD 155,000**, which is **77.50 %** of the entire USD 200,000 cost
   variance the week-13 board meeting exists to discuss. **Five points of utilisation is
   three-quarters of the variance everyone is arguing about**, and it is lost in ways nobody
   records: waiting for access, waiting for a permit, waiting for a decision (Domain 3's governance
   latency, priced in Case study C), re-mobilising after a stand-down.

   **The 5.74 % under-recovery is a finding, not an error.** It may be deliberate (a rate agreed
   commercially below full cost to win work, with the shortfall funded from elsewhere), or it may be
   an unnoticed drift as on-costs rose. Which of those it is matters, and the only way to know is to
   maintain the build-up and re-derive it annually. A charge-out rate carried unchanged for three
   years is almost certainly recovering less than it did.

   **A jurisdiction caution that must not be skipped.** The composition of on-costs (statutory
   contributions, leave entitlements, end-of-service or severance provisions, insurance), and their
   accounting and tax treatment differ substantially between jurisdictions, and so does what may be
   recovered as overhead in a reimbursable contract. The 32 % and 18 % here are illustrative figures
   for a fictitious project. Build the rate with your own payroll and finance functions, and where a
   contract permits cost recovery, confirm the allowable basis with qualified advisers before
   relying on it.

**Worked example 7.4.1c — the price of a crew-week, four ways.**

1. **Setup.** Domain 6, KA 6.3.1 establishes the ladder of capacity options (re-sequence, spend
   float, overtime, second shift, extend), and ranks it by cost without pricing it. Price it. An
   Auriga field crew is **4 technicians × 40 hours** at the USD 95 charge-out rate. The options for
   adding one crew-week of capacity: re-sequence within float; overtime at a **1.5×** premium; an
   agency crew at **USD 128** per hour achieving **85 %** of in-house productivity; or accept the
   delay, on a zero-float activity, at Domain 6's cost of delay of **USD 45,000 per week**.
2. **Formula.** Straight-time crew-week `= 4 × 40 × rate`. Overtime crew-week `= straight ×
   premium`. Agency cost per *productive* crew-week `= 4 × 40 × agency rate ÷ relative
   productivity`. Delay cost per week is given. Breakeven relative productivity between agency and
   overtime: `agency nominal ÷ overtime cost`.
3. **Substitution.** Straight `= 4 × 40 × 95 = 15,200`. Overtime `= 15,200 × 1.5`. Agency
   `= 20,480/0.85`. Breakeven `= 20,480/22,800`, equivalently `128/142.50`.
4. **Result.**

   | Option | Cost per crew-week of capacity | × straight time |
   |---|---|---|
   | Re-sequence within float | **0** cash (float consumed) | — |
   | Straight-time crew (if available) | **15,200** | 1.0000 |
   | Overtime at 1.5× | **22,800** | 1.5000 |
   | Agency crew, 85 % productivity | **24,094** | 1.5851 |
   | Accept the delay (zero float) | **45,000** | 2.9605 |

   The agency crew beats overtime only if its relative productivity is at least **89.82 %**;
   symmetrically, overtime beats the agency crew only while sustained-overtime productivity holds
   above **94.63 %**. The dearest capacity option is **53.54 %** of the delay cost, and USD 45,000
   buys **2.96** straight-time crew-weeks, **1.97** overtime crew-weeks or **1.87** agency
   crew-weeks.
5. **Interpretation.** The ladder Domain 6 asserts is confirmed and (more usefully) the gaps between
   its rungs are now numbers.

   **Any of these beats the delay, so the decision is which to buy, not whether.** On a zero-float
   activity every purchased option costs between **33.78 %** and **53.54 %** of doing nothing. That
   is the general shape once a cost of delay has been computed, and it is why computing one changes
   behaviour: without USD 45,000 on the table, "we cannot afford overtime" sounds prudent.

   **The two breakevens are the real content, because they are the arguments that actually happen.**
   Nobody disputes that an agency crew costs more per hour; the dispute is about how productive it
   will be, and 89.82 % is the number that dispute is about. Likewise the case against sustained
   overtime is not that it costs 1.5× but that its productivity decays: **the moment sustained
   overtime productivity falls below 94.63 %, the agency crew is the cheaper purchase**, at 90 %
   productivity overtime effectively costs USD 25,333 per crew-week and has lost. Both breakevens
   are checkable against the project's own timesheet and output records, which converts a preference
   into a measurement.

   **Capacity only buys time where the activity is resource-limited.** Adding a crew to an activity
   constrained by logic, curing time, a possession window or a single test facility buys nothing at
   all (Domain 6, KA 6.2 and 6.3.2), and the four prices above are then all wasted. Establish that
   the constraint is capacity before pricing capacity.

   **What the table excludes should be stated when it is used.** Onboarding, induction and
   supervision load for new crews; the lead time each option needs before it can start (Domain 6's
   point, and often decisive); the quality consequence of fatigue or unfamiliarity, which Domain 9
   prices as escaped defects at USD 12,000 each; and the float that the free option consumes, which
   is the project's risk buffer and has a value Domain 8 can quantify. **"Free" is the option whose
   price is hardest to see**, and a leader who spends float without saying so has bought capacity on
   credit.

### 7.4.2 Contract models and cost risk

Every contract model is an answer to one question: **who carries cost risk?**

| Model | Buyer pays | Cost risk sits with | Suits |
|---|---|---|---|
| **Firm fixed price (FFP)** | An agreed price, whatever it costs | **Seller** | Well-defined scope; priced-in risk premium |
| **Fixed price incentive (FPIF)** | Target cost/fee with a share ratio, capped by a ceiling | **Shared, then seller above the ceiling** | Definable scope with real uncertainty |
| **Cost plus fixed fee (CPFF)** | Allowable cost + a fixed fee | **Buyer** | Development, unclear scope |
| **Cost plus incentive fee (CPIF)** | Allowable cost + a fee varying with performance | **Shared** | Scope where effort is uncertain but effort quality matters |
| **Time and materials (T&M)** | Rates × quantities | **Buyer** | Staff augmentation, short or open-ended work |

The leader's discipline: **risk transferred is risk priced.** An FFP contract for ill-defined scope
does not remove the risk. It converts it into a premium plus a claims exposure when the scope moves
(Domain 10, KA 10.4). Conversely a cost-plus contract on well-defined work pays for uncertainty that
no longer exists. Domain 10, KA 10.3.2 works the full outturn arithmetic: what each model pays the
two parties at the same actual cost, and why an expected-value comparison of fixed price against
cost-plus buys variance rather than money. What belongs *here* is the calculation a delivery leader
does before that conversation starts: the point at which the simplest two options cross.

**Worked example 7.4.2 — fixed price or time and materials: where they cross.**

1. **Setup.** A defined configuration-and-testing package on Auriga. A supplier quotes a firm fixed
   price of **USD 480,000**. The alternative is time and materials from the same engineering pool at
   the blended **USD 130.625** per hour (KA 7.4.1). The team's own three-point estimate of the hours
   is **optimistic 2,900, most likely 3,200, pessimistic 4,100**.
2. **Formula.** Breakeven hours `= fixed price ÷ hourly rate`. T&M cost `= hours × rate`. Premium
   `= fixed price − T&M cost at the estimate`. PERT expected hours `= (o + 4m + p)/6`; triangular
   mean `= (o + m + p)/3`. For a triangular distribution, the probability of exceeding a value `x`
   above the mode is `(b − x)²/((b − a)(b − m))`.
3. **Substitution.** Breakeven `= 480,000/130.625`. T&M at the mode `= 3,200 × 130.625`. PERT hours
   `= (2,900 + 12,800 + 4,100)/6`. Exceedance `= (4,100 − 3,674.6411)²/(1,200 × 900)`.
4. **Result.** Breakeven **3,674.64 hours**. At the most likely 3,200 hours, T&M costs **USD
   418,000**, so the fixed price carries a premium of **USD 62,000: 14.83 %** over the estimate
   (12.92 % of the fixed price itself). At the pessimistic 4,100 hours T&M costs **USD 535,563**,
   USD 55,563 *worse* than the fixed price; at the optimistic 2,900 it costs **USD 378,813**, USD
   101,188 better. PERT expected hours are **3,300** (T&M **USD 431,063**, a premium of **11.35 %**;
   the triangular mean is **3,400 hours**) T&M **USD 444,125**, a premium of **8.08 %**. The
   probability that the hours exceed the breakeven is **16.75 %**.
5. **Interpretation.** The identity is exact and worth keeping: **the fixed-price premium expressed
   as a percentage of the estimate *is* the hours overrun at which the two options break even.**
   14.83 % premium, 14.83 % overrun tolerance. So the question at the table is never "is USD 62,000
   a lot?" but "how confident are we that this package will not run more than 14.83 % over its
   estimated hours?": a question the estimating team can actually answer, and one whose answer here
   is *not very*, since a 14.83 % overrun on 3,200 hours is well inside the declared range.

   **State the premium against expectation, not against the mode.** Against expected hours the
   premium is **8.08 % to 11.35 %** depending on the distribution shape, not 14.83 %. Quoting the
   larger figure makes the fixed price look worse than it is, and quoting no figure at all is how
   fixed prices get accepted or rejected on temperament. What the buyer receives for that
   USD 35,875–48,938 is the removal of a 16.75 % chance of paying more than 480,000 and of a tail
   that reaches USD 535,563 — the variance-for-money trade Domain 10, KA 10.3.2 quantifies properly.

   **The distributions are not the same distribution, and this is the error the arithmetic hides.**
   Under time and materials nobody is paid to be efficient: the supplier's revenue rises with the
   hours, so the hours estimate that was drawn for an in-house or fixed-price case does not
   transfer. The correct comparison prices T&M against a *shifted* distribution, and since the size
   of the shift is unknowable in advance, the practical countermeasures are structural, an hours cap
   or not-to-exceed envelope, a fixed rate schedule, an agreed productivity or output measure, and
   approval gates at stated cumulative hours. **A T&M arrangement without a cap is an open account,
   not a contract model.**

   **And the fixed price only covers the scope specified.** The 14.83 % tolerance is worthless if
   the scope is 30 % defined, because the variations will arrive priced without competition (MCQ
   7.4-C, and Domain 10, KA 10.4.1 on entitlement). The order of operations is therefore fixed:
   define the scope, then choose the model, then compute the breakeven, never the reverse.

### 7.4.3 Incentive fees and the point of total assumption

**The mechanics.** Under an incentive contract, buyer and seller agree a **target cost**, a
**target fee** and a **share ratio** (buyer/seller) for over- and under-runs, with a **ceiling
price** capping the buyer's exposure.

**Worked example 7.4.3 — the incentive that stops incentivising.**

1. **Setup.** Auriga's installation subcontract: target cost **USD 2,000,000**, target fee
   **USD 150,000**, share ratio **70/30** (buyer 70 %, seller 30 %), ceiling price
   **USD 2,450,000**. The seller finishes at an actual cost of **USD 2,300,000**.
2. **Formula.** Fee = target fee − (overrun × seller share). Buyer pays = actual cost + fee
   (subject to the ceiling). Point of total assumption
   `PTA = target cost + (ceiling − target price)/buyer share`, where target price = target cost +
   target fee.
3. **Substitution.** Overrun `2,300,000 − 2,000,000 = 300,000`; fee
   `150,000 − 300,000 × 0.30 = 150,000 − 90,000`. Buyer pays `2,300,000 + 60,000`.
   Target price `2,150,000`; `PTA = 2,000,000 + (2,450,000 − 2,150,000)/0.70`.
4. **Result.** Fee **USD 60,000** (down from 150,000); buyer pays **USD 2,360,000**. **`PTA` = USD
   2,428,571.43**; and at that cost the buyer pays exactly the USD 2,450,000 ceiling.
5. **Interpretation.** The share ratio does its job up to the `PTA`: both parties lose money on
   overrun, so both want efficiency. **Above the `PTA` the seller bears 100 %** of further cost,
   which is the moment the incentive inverts: a seller heading past it has no financial reason to
   spend more on your project and every reason to argue that the extra cost is *your* scope change.
   Knowing where the `PTA` sits is therefore a **delivery** insight, not an accounting one: it
   predicts when a commercial relationship will turn adversarial (Domain 10, KA 10.4; Domain 11's
   negotiation).

   **Verify the `PTA` by walking it, because the check is one line and it catches sign errors.** At
   a cost of 2,428,571.43 the overrun is 428,571.43, so the fee is `150,000 − 0.30 × 428,571.43 =`
   **USD 21,428.57**, and the buyer pays `2,428,571.43 + 21,428.57 =` **USD 2,450,000**: the
   ceiling, exactly. Any `PTA` that fails to reproduce the ceiling this way has been computed with
   the wrong share.

   **The fee is not yet exhausted at the `PTA`, and that boundary has a rule.** The seller's fee
   reaches zero only at `2,000,000 + 150,000/0.30 =` **USD 2,500,000**, above the `PTA`. So on this
   structure the *ceiling* binds first, which is what makes the `PTA` the meaningful inflection. The
   general condition is worth carrying: **the ceiling binds before the fee is exhausted only while
   the ceiling is below `target price + target fee × buyer share ÷ seller share`**, here `2,150,000
   + 150,000 × (0.70/0.30) =` **USD 2,500,000**. At a ceiling of exactly 2,500,000 the `PTA` and the
   zero-fee cost coincide at 2,500,000; above it the `PTA` is a nominal figure, because the seller
   has already lost its whole fee before the buyer's cap engages. A leader shown a `PTA` should
   therefore also be shown the zero-fee cost, and told which comes first.

   **The share ratio moves the `PTA`, and it moves it the way nobody expects.**

   | Buyer / seller share | `PTA` | Fee at the `PTA` | Comes before the zero-fee cost? |
   |---|---|---|---|
| 50 / 50 | 2,600,000 | (150,000) | No: zero-fee cost is 2,300,000 |
| 60 / 40 | 2,500,000 | (50,000) | No: zero-fee cost is 2,375,000 |
   | **66.67 / 33.33** | **2,450,000** | **0** | Exactly coincides, at the ceiling |
| 70 / 30 (Auriga) | **2,428,571** | 21,429 | Yes: zero-fee cost is 2,500,000 |
   | 80 / 20 | 2,375,000 | 75,000 | Yes |
   | 90 / 10 | 2,333,333 | 116,667 | Yes |

   **The more of the overrun the buyer absorbs, the *earlier* the inflection arrives**: moving from
   70/30 to a superficially more generous 90/10 brings the `PTA` forward by **USD 95,238**, because
   the buyer's fixed headroom to the ceiling is consumed faster. A buyer who concedes share in
   negotiation to appear reasonable has bought an earlier adversarial turn, and should know it. Note
   too the boundary in the third row: on this structure a buyer share below **66.67 %** means the
   ceiling never binds before the fee is gone.

   **Sensitivity to the ceiling is `1/buyer share`, so the ceiling is the stronger lever.** Each
   dollar added to the ceiling moves the `PTA` by **1.4286** dollars at a 70 % buyer share. And the
   figure to monitor monthly is the *headroom*: with the seller's cost forecast at 2,300,000, the
   distance to the `PTA` is **USD 128,571, 5.59 %** of that forecast. **A single 6 % cost movement
   puts this relationship past its inflection**, which is why toolkit 7.T.3 asks for the cost trend
   against the `PTA` at every commercial checkpoint rather than the `PTA` alone. Domain 10, KA
   10.3.2 reads the same figure as a statement about the seller's marginal dollar.

   **A counsel pointer, because this is an enforceability-sensitive area.** Ceilings, share ratios,
   fee adjustment mechanics and their interaction with variation, notice and claims provisions vary
   by contract form and by jurisdiction, and so does the treatment of a seller trading past a
   ceiling. The arithmetic here is contract-neutral; the drafting is not. Take qualified legal advice
   on the instrument before relying on any of it commercially (Domain 10, KA 10.4).

### 7.4.4 Cash flow versus profit

A project can be profitable and still fail for lack of cash: the same truth PFL-AI establishes for
financings (its Domain 1). For a delivery leader the mechanism is payment terms: cost is incurred as
work happens; cash arrives when invoices are approved and paid. On Auriga, with `AC` = USD 2,120,000
at week 13 and 60-day terms, roughly **USD 742,000** of incurred cost (about 35 %) is still unpaid:
an exposure carried by whoever is funding the work. The leader's obligations are practical: know the
terms in both directions (client and subcontractors), front-load nothing you cannot fund, and never
let a retention or milestone-payment structure be agreed without someone computing its cash profile.
Where the project sits inside a financed asset, this is precisely the CFADS conversation PFL-AI
Domain 10 has with lenders.

That last obligation is the one routinely left undone, so here is the computation.

**Worked example 7.4.4 — Auriga's cash, its profit, and the gap between them.**

1. **Setup.** Auriga is delivered under a fixed price to the utility of **USD 4,400,000** against a
   `BAC` of 4,000,000, a bid margin of USD 400,000. Work is valued monthly (weeks 4, 8, 12 …) at the
   contract rate, **5 % retention** is withheld from each payment, and the client's terms are **60
   days** from invoice. Half the retention is released at practical completion and half after the
   defects period. At week 13: `EV` **1,920,000**, of which **1,760,000** had been earned by the
   week-12 valuation, **880,000** by the week-8 valuation and **320,000** by the week-4 valuation;
   `AC` **2,120,000**, of which **USD 742,000** sits in supplier invoices within terms and accruals
   not yet paid. By week 13 only the week-4 invoice has been paid. What is the position?
2. **Formula.** Revenue recognised `= (price/BAC) × EV`. Margin to date `=` revenue `− AC`. Cash
   paid out `= AC −` payables. Cash received `=` net-of-retention value of invoices whose terms have
   expired. Funding absorbed `=` cash paid out `−` cash received. Days of spend `=` funding absorbed
   `÷` (`AC` ÷ elapsed days). Client exposure `=` receivables `+` retention held `+` unbilled work
   in progress.
3. **Substitution.** Price factor `= 4,400,000/4,000,000 = 1.10`. Revenue `= 1.10 × 1,920,000`.
   Certified to week 12 `= 1.10 × 1,760,000 = 1,936,000`, retention `= 96,800`. Week-4 payment
   `= 1.10 × 320,000 × 0.95`. Cash out `= 2,120,000 − 742,000`. Days of spend
   `= 1,043,600 ÷ (2,120,000/91)`.
4. **Result.**

   | Position at week 13 | USD |
   |---|---|
   | Cost incurred (`AC`) | 2,120,000 |
   | less payables (35.00 % of `AC`) | (742,000) |
   | **Cash paid out** | **1,378,000** |
   | less cash received (the week-4 invoice, net of retention) | (334,400) |
   | **Funding absorbed** | **1,043,600** |
   | Revenue recognised at 1.10 × `EV` | 2,112,000 |
   | **Margin to date** | **(8,000)** |
   | Receivables outstanding | 1,504,800 |
   | Retention held | 96,800 |
   | Unbilled work in progress (week 13, at 1.10) | 176,000 |
   | **Total client exposure** | **1,777,600** |

   The funding absorbed is **26.09 %** of `BAC` and **44.80 days** of spend at the current rate of
   USD 23,297 a day.
5. **Interpretation.** Set the two headline numbers side by side. **The cost report's headline is a
   variance of USD 200,000. The bank account's headline is USD 1,043,600**: **5.22 times** larger,
   payable now, and absent from every document in the week-13 pack.

   **The working-capital identity ties the two statements together and should be used as a check.**
   Funding absorbed `=` client exposure `−` payables `+` loss to date:
   `1,777,600 − 742,000 + 8,000 =` **1,043,600**, exactly. So cash is not a separate subject from
   performance; it is performance plus the timing of two sets of payment terms. A cash forecast that
   cannot be reconciled to the cost report through that identity has an error in one of them.

   **Express the requirement in days of spend, because that is the number that transfers.**
   USD 1,043,600 means nothing across a portfolio; **44.80 days of spend** is immediately comparable
   between projects of different sizes and immediately actionable by a treasurer. A project running
   at 45 days of spend has a month and a half of its own cost outstanding at all times, and every
   week of client payment delay adds directly to it.

   **Payment terms are the largest single lever, and they are agreed by people who never see this
   table.** Moving the client's terms from 60 days to 30 brings the week-8 invoice inside the data
   date (a further **USD 585,200** received), and cuts the funding absorbed to **USD 458,400**, a
   **56.08 %** reduction. Nothing about the work changes. That is the whole of the case for treating
   payment terms as a delivery parameter rather than a procurement formality, and for computing the
   profile *before* signature: after signature the same improvement has to be bought.

   **Retention is margin that is not cash, and the proportion is startling.** Retention at
   completion will be `5 % × 4,400,000 =` **USD 220,000 (55.00 %** of the entire bid margin) with
   **USD 110,000 (27.50 %** of margin) held beyond practical completion into the defects period. A
   project can therefore be complete, profitable and still waiting on more than a quarter of its
   profit, which is why release criteria and their evidence belong in the closeout plan (Domain 16)
   rather than being discovered afterwards.

   **The forecast method decides whether this contract makes money at all.** At the fixed price of
   4,400,000: method (a)'s `EAC` of 4,200,000 leaves a margin of **USD 200,000 (+4.55 %** of price);
   method (b)'s 4,416,667 leaves **(USD 16,667) (−0.38 %)**; method (c)'s 4,608,056 leaves **(USD
   208,056) (−4.73 %)**. So KA 7.3.3's choice of assumption is not a reporting preference. It is the
   difference between a profitable contract and a loss-making one, and preserving the bid margin of
   400,000 requires an `EAC` of exactly `BAC`, which is `TCPI` 1.11 (KA 7.3.4). **The `TCPI`
   conversation and the margin conversation are the same conversation**, and a commercial manager
   and a project controller who do not know that will present two irreconcilable papers to the same
   board.

   **An accounting caution.** How revenue is recognised on a contract, and how retentions, accruals
   and unbilled work in progress are presented, are matters for the applicable financial reporting
   framework and the entity's own accounting policy, and they vary by jurisdiction. The 1.10 × `EV`
   basis used above is a transparent illustrative convention for a fictitious project, not a
   statement of how any entity must report. Agree the basis with the finance function before any
   figure of this kind leaves the project.

> **Fig 7.4.1 — Auriga at week 13: the cost report says 200,000; the bank account says 1,043,600.**
> Two-panel figure. Left, a cash bridge in five columns: cost incurred **2,120,000**, less payables
> **(742,000)** (35.0 % of `AC`), giving cash paid out **1,378,000**; less cash received
> **(334,400)** giving funding absorbed **1,043,600**, annotated **44.80 days of spend** and drawn
> in crimson. Right, a bar pair titled "same work, two client payment terms": funding absorbed
> **1,043,600** under 60-day terms against **458,400** under 30-day terms, annotated **−56.08 % from
> one term**. A header records revenue recognised 2,112,000 against cost 2,120,000 — a margin of
> (8,000) — and total client exposure 1,777,600 = receivables 1,504,800 + retention 96,800 +
> unbilled 176,000. Source: PCI original. Alt text: a descending cash bridge from cost incurred to
> the funding a project is absorbing, beside two bars showing that halving the client's payment
> terms more than halves that funding requirement.

### AI in this KA

Contract analytics (extracting terms, comparing models, flagging inconsistent clauses) is real and
useful AI assistance, and it is also decision support rather than legal or commercial advice. Two
boundaries: an AI-produced reading of a clause is verified against the clause itself before anyone
relies on it (the document-against-summary check), and commercial and legal positions go to
qualified counsel (Domain 10). Fee arithmetic and `PTA` computations are deterministic: they get the
same golden-answer treatment as the earned-value set.

Three further placements are worth being specific about, because the temptation in this KA is to
delegate the wrong half. Assembling a **rate build-up** from payroll and overhead data across many
grades and cost centres, the mechanical part of 7.4.1b, is a strong application, and the resulting
cost per productive hour is exactly checkable. Sweeping a portfolio of contracts for payment terms,
retention percentages and release criteria and producing the aggregate cash profile of 7.4.4 is work
most organisations do not do at all because it is tedious across dozens of documents; a model does
it quickly and every extracted term is verifiable against its clause. What must **not** be delegated
is the **utilisation** assumption and the **relative productivity** figure of 7.4.1c: both are
judgements about a specific workforce in a specific setting, both drive their answers almost
entirely, and a model asked for either will supply a confident, well-formatted number with no
provenance: the failure mode Domain 9, KA 9.1 describes for detection rates. Where the evidence does
not exist, the correct output is the breakeven — 89.82 %, 94.63 %: plus the statement that nobody
has measured which side of it the project sits on.

### Key terms — KA 7.4

| Term | Meaning |
|---|---|
| **Blended rate** | Weighted average cost per hour of a mixed resource pool. |
| **FFP / FPIF / CPFF / CPIF / T&M** | Contract models ordered by who carries cost risk. |
| **Share ratio** | The agreed buyer/seller split of over- and under-run. |
| **Ceiling price** | The cap on the buyer's total exposure under an incentive contract. |
| **Point of total assumption (`PTA`)** | The cost above which the seller bears 100 % of further overrun. |
| **Zero-fee cost** | `target cost + target fee ÷ seller share`, where the incentive fee is exhausted; on Auriga USD 2,500,000, above the `PTA`, which is why the ceiling binds first. |
| **`PTA` headroom** | The distance from the seller's current cost forecast to the `PTA`; 5.59 % on Auriga, and the figure to trend monthly. |
| **Cost per productive hour** | Burdened pay ÷ (1 − non-productive share) × (1 + overhead); USD 100.79 against a USD 95.00 charge-out rate. |
| **Paid-to-productive multiplier** | `1/(1 − n)`, 1.17647 at a 15 % non-productive share; the commonest silent under-funding in a labour estimate. |
| **Cost per crew-week of capacity** | The priced rungs of Domain 6's capacity ladder, from 15,200 at straight time to 45,000 for accepting the delay (7.4.1c). |
| **T&M/fixed-price breakeven** | `fixed price ÷ hourly rate`; the premium as a percentage of the estimate equals the hours overrun at which the two options cross. |
| **Retention** | A withheld percentage of payment, released on completion criteria; Auriga's 5 % is 55.00 % of the bid margin at completion. |
| **Funding absorbed** | Cash paid out − cash received; equals client exposure − payables + loss to date. Best stated in days of spend (44.80 on Auriga). |

### Sample MCQs — KA 7.4

**MCQ 7.4-A `[7.4.3 · Application]`** Target cost 2,000,000; target fee 150,000; share 70/30;
actual cost 2,300,000. The seller's fee is:
- A. USD 150,000
- B. USD 60,000 ✅
- C. USD 90,000
- D. USD 45,000

*Rationale:* The seller absorbs 30 % of the 300,000 overrun: `150,000 − 90,000 = 60,000`. A
ignores the incentive; C states the fee reduction rather than the fee; D applies the buyer's
share to the fee.

**MCQ 7.4-B `[7.4.3 · Analysis]`** Target cost 2,000,000, target fee 150,000, ceiling 2,450,000,
buyer share 70 %. The `PTA` is 2,428,571, and its delivery significance is that above it:
- A. the contract becomes void
- B. the buyer absorbs all further cost
- C. the seller bears 100 % of further cost, so the incentive inverts and cost growth becomes a scope-change argument ✅
- D. the fee becomes negative but risk-sharing continues unchanged

*Rationale:* Beyond the `PTA` the ceiling binds the buyer, so every further dollar is the seller's,
which predictably redirects the seller's effort from efficiency to entitlement. B reverses the
exposure; A is fiction; D misses that sharing has *stopped*.

**MCQ 7.4-C `[7.4.2 · Analysis]`** A leader lets an FFP contract for scope that is only 30 %
defined. The most likely outcome is:
- A. cost risk is genuinely eliminated
- B. a priced-in risk premium plus a claims-and-variations exposure as the scope is defined ✅
- C. the seller absorbs all scope growth at no cost to the buyer
- D. the contract converts automatically to cost-plus

*Rationale:* Fixed price transfers risk *at a price* and only for the scope actually specified;
undefined scope returns as variations. A and C mistake the contractual form for the underlying
uncertainty; D invents a mechanism.

**MCQ 7.4-D `[7.4.1 · Application]`** A pool of 40 at USD 95/h, 25 at USD 140/h and 15 at
USD 210/h has a blended rate of:
- A. USD 148.33
- B. USD 130.63 ✅
- C. USD 112.31
- D. USD 140.00

*Rationale:* `10,450/80 = 130.63`. A averages the three rates unweighted (`445/3`); C blends only
the two cheapest grades, dropping the specialists (`7,300/65`); D takes the middle rate as
representative.

**MCQ 7.4-E `[7.4.1 · Application]`** Base pay USD 55.00 per paid hour; on-costs 32 %; 15 % of paid
hours are non-productive; recoverable overhead 18 %. The full cost of one productive hour is:
- A. USD 85.41
- B. USD 100.79 ✅
- C. USD 98.52
- D. USD 72.60

*Rationale:* `55.00 × 1.32 ÷ 0.85 × 1.18 = 100.7859` (7.4.1b). A stops before overhead; C adds 15 %
rather than dividing by 0.85, understating by the difference between an uplift and a divisor; D is
the paid-hour cost, which is what most rate cards actually contain.

**MCQ 7.4-F `[7.4.1 · Evaluation]`** An agency crew charges USD 128 per hour against an in-house
USD 95 with overtime at 1.5×. The agency crew is the cheaper purchase per productive crew-week only
if its relative productivity is at least:
- A. 74.22 %
- B. 89.82 % ✅
- C. 100.00 %
- D. 66.67 %

*Rationale:* `128/142.50 = 0.8982`, the agency rate against the overtime rate (7.4.1c). A compares
the agency rate with straight time and so ignores the overtime premium being avoided; C assumes any
productivity shortfall disqualifies the option, which the arithmetic refutes; D inverts the 1.5
premium.

**MCQ 7.4-G `[7.4.2 · Application]`** A firm fixed price of USD 480,000 is offered against time and
materials at USD 130.625 per hour, with the package estimated at 3,200 hours. The breakeven hours and
the premium over the estimate are:
- A. 3,674.64 hours and 14.83 % ✅
- B. 3,674.64 hours and 12.92 %
- C. 3,428.57 hours and 14.83 %
- D. 3,200 hours and nil

*Rationale:* `480,000/130.625 = 3,674.64`; the premium is `62,000/418,000 = 14.83 %`, which equals
the hours overrun at breakeven (7.4.2). B expresses the premium against the fixed price rather than
the estimate, understating the tolerance the buyer is purchasing; C uses the mid-grade rate of 140
instead of the blend; D assumes the estimate is the crossing point.

**MCQ 7.4-H `[7.4.4 · Application]`** The contract price is USD 4,400,000 against a `BAC` of
4,000,000. At week 13 `EV` is 1,920,000 and `AC` is 2,120,000. The margin recognised to date is:
- A. USD 400,000
- B. (USD 8,000) ✅
- C. USD 200,000
- D. (USD 200,000)

*Rationale:* Revenue at `1.10 × 1,920,000 = 2,112,000` against cost of 2,120,000 (7.4.4). A is the
bid margin, which assumes nothing has gone wrong; C and D read the cost variance as the margin,
which omits the 10 % mark-up earned on the work performed and so mis-states the position in both
directions.

### Self-check — KA 7.4

1. *State the one question every contract model answers.* — Who carries cost risk.
2. *Why is the `PTA` a delivery concern, not just a commercial one?* — Above it the seller bears
   all further cost, so behaviour shifts from efficiency to entitlement.
3. *How can a profitable project run out of cash?* — Cost is incurred as work happens; cash arrives
   on payment terms. The gap must be funded.
4. *Which way does the `PTA` move when the buyer takes a larger share of the overrun, and why?* —
   Earlier: fixed headroom to the ceiling is consumed faster, so 70/30 to 90/10 brings it forward by
   USD 95,238. Below a 66.67 % buyer share this structure's ceiling never binds before the fee is
   exhausted.
5. *What is the difference between a paid-hour rate and a productive-hour rate?* — The factor `1/(1
   − n)` (17.65 % at Auriga's 15 % non-productive share), and confusing them under-funds a labour
   estimate before any risk materialises.
6. *State Auriga's capacity ladder in money.* — Per crew-week: re-sequencing free but paid in float,
   straight time 15,200, overtime 22,800, agency at 85 % productivity 24,094, accepting the delay
   45,000, so on a zero-float activity every purchased option beats doing nothing.
7. *What does the funding absorbed at week 13 equal, and how is it best expressed?* — USD 1,043,600
   (client exposure 1,777,600 less payables 742,000 plus the 8,000 loss) best stated as **44.80 days
   of spend**, and halved to 458,400 by 30-day rather than 60-day client terms.

---

## Advanced topics — Domain 7

### 7.A.1 Earned schedule — closing `SPI`'s late-project blind spot

`SPI` converges on 1.00 as a project finishes, whatever its lateness, because `EV` and `PV` both
approach `BAC`. **Earned schedule** restates progress in time: `ES` is the date at which the value
now earned was *planned* to have been earned, and `SPI(t) = ES / AT`. This is the bridge flagged in
Domain 6 (KA 6.4.3), now with its cost-side companions. And and the size of the blind spot it closes
is worth computing rather than asserting.

**Worked example 7.A.1 — the same project, read twice, at week 13 and at week 27.**

1. **Setup.** Auriga's baseline `PV` curve reaches **1,920,000 at week 12** and **2,080,000 at week
   13**: a local slope of USD 160,000 a week. At week 13 the project has earned exactly
   **1,920,000**. Now project forward: suppose the work is not complete at week 25 and at **week
   27** `EV` stands at **3,920,000**, against a baseline whose `PV` was **3,880,000 at week 24** and
   reaches `BAC` **4,000,000 at week 25** (after which `PV` stays at `BAC`, having nothing left to
   plan). Planned duration `PD` = **25 weeks**. Read the schedule position both ways at both dates.
2. **Formula.** `ES` = the baseline time at which the `EV` now earned was planned to be earned,
   interpolated between the bracketing periods: `ES = t + (EV − PV_t)/(PV_{t+1} − PV_t)`.
   `SPI(t) = ES/AT`, with `AT` the actual time elapsed. Time forecast
   `IEAC(t) = PD/SPI(t)`. For comparison `SPI = EV/PV`.
3. **Substitution.** Week 13: `ES = 12` exactly, so `SPI(t) = 12/13`; `SPI = 1,920,000/2,080,000`.
   Week 27: `ES = 24 + (3,920,000 − 3,880,000)/(4,000,000 − 3,880,000) = 24 + 1/3`;
   `SPI(t) = 24.3333/27`; `SPI = 3,920,000/4,000,000`; `IEAC(t) = 25/0.901235`.
4. **Result.**

   | | `SPI` | `ES` | `SPI(t)` | Time forecast |
   |---|---|---|---|---|
| Week 13 | **0.9231** | 12.0000 | **0.9231** | 27.0833 weeks (2.0833 late) |
| Week 27 | **0.9800** | 24.3333 | **0.9012** | 27.7397 weeks (2.7397 late) |

   At week 13 the two indices agree exactly. At week 27 they differ by **0.0788**: `SPI` reports the
   project **2 %** behind while it is in fact **2.74 weeks, 10.96 %**, late.
5. **Interpretation.** The week-13 agreement is not a coincidence and it is the more instructive
   half. **`SPI` and `SPI(t)` coincide wherever the `PV` curve is locally linear**, because
   `SV/slope` is then exactly the number of weeks of lateness: Auriga's `SV` of (160,000) is one
   week of planned value at 160,000 a week, so `ES` is 12 and `SPI(t)` is 12/13, the same 0.9231
   that `EV/PV` gives. That is why `SPI` is perfectly serviceable mid-project on a smooth S-curve,
   and it is worth saying so rather than presenting earned schedule as a correction to a broken
   measure.

   **The divergence appears where the curve flattens, which is exactly where the decisions get
   expensive.** Past the planned finish, `PV` is pinned at `BAC` and has nothing left to compare
   against, so `SPI` is arithmetically compelled towards 1.00 and reports 0.98 for a project nearly
   three weeks late. `SPI(t)` keeps working because its denominator is *time*, which does not run
   out. The rule for practice: **quote `SPI` while the `PV` curve is still rising steeply, and
   `SPI(t)` from the point at which it flattens**, and never quote `SPI` in the last 15 % of a
   project without the time-based figure beside it (7.A.2's limitation list; the exam-preparation
   trap is the same one).

   **The time forecast is what makes the index worth computing, because it converts into money at a
   known rate.** `PD/SPI(t)` gives 27.08 weeks at the data date (2.0833 weeks late), which at Domain
   6's cost of delay of USD 45,000 per week is **USD 93,750** of delay cost, a figure directly
   comparable with the compression options Domain 6 prices and with the recovery spend the week-13
   case study argues about. Note the consistency check: at week 27 the remaining baseline work is
   `25 − 24.3333 =` 0.6667 weeks, which at the demonstrated time-efficiency of 0.9012 takes 0.7397
   weeks, completion at 27.7397, exactly the `IEAC(t)`. Any earned-schedule presentation should
   reproduce that agreement.

   **Two cautions, both about the baseline rather than the technique.** `ES` is read *off the `PV`
   curve*, so it inherits every defect of that curve, a curve phased by straight-line spread rather
   than by the schedule gives a confidently wrong `ES`, and the interpolation above assumes the
   curve is meaningful between period points. And re-baselining resets `ES` exactly as it resets
   `SPI` (7.2.3): a project that re-phases its `PV` curve to match its actual progress will report
   `SPI(t)` of 1.00 while remaining as late as it was the day before, which is why the integrity
   rules of KA 7.2.3 are a precondition for every index in this domain and not only for the cost
   ones.

### 7.A.2 EVM's limitations, stated plainly

Earned value measures conformance to *plan*, not value to the *customer*: a project can score 1.00
on both indices while building the wrong thing (Domain 5's acceptance criteria are the defence). It
says nothing about quality (Domain 9). It is only as honest as its earning rules and its accruals.
And it is blind to risk that has not yet materialised: Domain 8's contingency analysis, not `CPI`,
tells you whether what remains is adequately funded. EVM is a measurement system, not a management
philosophy; leaders who treat the indices as targets get optimised indices.

### 7.A.3 The reviewer's cost eye

Invariants a reviewer runs before trusting a cost report: `EV/BAC` equals the claimed percent
complete; `CV = EV − AC` and `SV = EV − PV` reconcile to the stated indices; `EAC` = `AC` + a stated
`ETC`, with the assumption named; `TCPI` to `BAC/CPI` equals `CPI`; sum of control-account budgets +
contingency equals `BAC`; management reserve sits outside that total; no completed package's budget
has moved since last period; accruals are present in the current period; and level-of-effort share
is disclosed per control account. Any violation is a defect somewhere, find it before the board
builds on it.

Seven more, each of which this domain has now derived, and each a single line of arithmetic:

- **The percent-spent minus percent-complete gap must equal `CV/BAC`.** Auriga: 53.00 % − 48.00 % =
  5.00 %, and `200,000/4,000,000 = 5.00 %` (7.3.2).
- **`TCPI` to every reported `EAC` must equal that method's own implied efficiency**: 1.0000 for
  method (a), `CPI` for (b), `CPI × SPI` for (c). A mismatch means the forecast and its stated
  assumption are not the same document (7.3.4).
- **Contingency drawn plus unattributed variance must equal `CV`.** Auriga's USD 190,000 of draws
  against a `CV` of 200,000 leaves USD 10,000 with no named cause (7.1.3).
- **`AC` must not be below invoices plus receipted-but-uninvoiced value**, and a cumulative `CPI` of
  exactly 1.0000 is a prompt to check that before it is a cause for satisfaction (7.2.1).
- **Commitment coverage must be stated wherever prices are moving**, and the excess of the
  forecast's uncommitted balance over the baseline's must equal \|`VAC`\| (7.2.1b).
- **The earning rule per package must be the same rule as last period**, and one account should be
  recomputed under an alternative rule to size the discretion involved: 15 % of budget on Auriga's
  ten-package group (7.3.1).
- **`SPI` must not be quoted alone in the last 15 % of a project**, where it is compelled towards
  1.00; the time-based figure belongs beside it (7.A.1).

And two outside the earned-value set that decide whether the report describes a viable project at
all: the **funding absorbed**, stated in days of spend (44.80 on Auriga), and the **`PTA` headroom**
on every incentive package (5.59 %). Neither appears in a conventional cost report, and both change
decisions.

---

## Industry variations — Domain 7

Each variation below changes a *specific* number in this domain, not merely its emphasis.

- **Construction and EPC.** Full EVM against a resource-loaded baseline, with `EV` claimed from
  measured-work valuations, so the earning rule is effectively units-completed and the earning-rule
  discretion of 7.3.1 is largely designed out, at the cost of a monthly measurement effort.
  Retention is the binding cash term: on Auriga's structure a 5 % retention is **USD 220,000**, or
  **55.00 %** of the bid margin, and half of it is held past completion. Variation and claims
  machinery under the standard international forms makes the `PTA` conversation routine rather than
  exceptional, so the headroom figure of 7.4.3 belongs on the monthly commercial report.
- **Government and defence programmes.** Formal earned-value management-system compliance with
  surveillance, control-account discipline and (the operative difference) a **hard cap on the
  level-of-effort share**, because 7.3.1's distortion identity `s × (1 − SPI_d)` is exactly what a
  surveillance regime exists to bound: a 20 % cap holds the distortion to 0.08 where a 70 % share
  allows 0.28. Forecasting is auditable rather than discretionary, which in practice means method
  (d) is commissioned at defined points rather than when someone asks for it.
- **Technology and product delivery.** Cost is overwhelmingly people, so the numbers that move the
  outturn are the blended rate, the mix and **utilisation**: 7.4.1b's result (five points of
  utilisation being 6.25 % of the labour bill, USD 155,000 on Auriga's labour content) is the whole
  game, and materials barely register. Where cadence replaces a fixed baseline, throughput and
  cost-per-increment stand in for `CPI` (Domain 13), and hybrid programmes report both; the
  commercial default is time and materials, which makes 7.4.2's cap-and-rate-schedule discipline the
  difference between a contract and an open account.
- **Energy and resources.** Long-lead equipment and commodity exposure move the centre of gravity to
  **commitment coverage** and escalation: 7.2.1b's arithmetic (a 5 % movement on the uncommitted
  remainder being 27.92 % of the cost variance to date) is why the coverage figure is reported
  monthly here and rarely elsewhere, and why early commitment is bought deliberately at a price
  (Domain 10, KA 10.1.3; PFL-AI Domain 3 for escalation and currency machinery). Contingency is
  sized quantitatively as a matter of course, so the draw-index trend of 7.1.3 has a register behind
  it.
- **Public services and transformation.** Benefits, not cost variance, are the accountability
  currency (Domain 16), and the cost-side consequence is the one Case study C computes: contingency
  gets sized against **delivery** risk while the exposure sits on the **benefit** side, where
  Meridian's unreserved annual gap of USD 293,760 was **2.4480 times** its entire contingency. The
  leader's task is keeping cost reporting honest while the value case is what gets debated, and
  extending reserve thinking to the risks that actually decide whether the money was worth spending.

## Case study — Domain 7: the forecast the board actually needed (utilities)

**Situation.** Auriga's week-13 review. The cost report shows `CPI` 0.91, `SPI` 0.92, `CV`
(200,000), `SV` (160,000). The programme director's paper proposes reporting **`EAC` USD
4,200,000**, method (a), on the grounds that the overrun was the contaminated-ground remediation, a
discrete event now closed and remediated (Domain 6's case study).

**The challenge.** The assurance reviewer asks three questions. *Is the cause genuinely closed?* Yes
for the remediation, but the recovery bought a second civil crew and a fast-track of installation,
both of which continue for six more weeks, and neither is in method (a)'s "remaining work at
budgeted rate" assumption. *What does recovery to `BAC` require?* `TCPI` 1.11 against a demonstrated
0.91. *What is the mechanism?* There isn't one: the recovery spend increases cost to protect the
date.

**The outcome.** The board is given a **range with named assumptions**: 4,200,000 if the closed
event is the whole story; 4,416,667 if current efficiency persists; and a bottom-up (method d)
re-estimate of the remaining 52 %, commissioned that week, as the number they will actually manage
to. Contingency draw is authorised against the identified risk; management reserve is not touched.
The minute records the `TCPI` gap and the explicit finding that **no recovery-to-budget plan
exists**, so nobody later claims one was promised.

**What the domain teaches here.** A single-number forecast hides the only thing that matters: the
assumption. `TCPI` converts optimism into an arithmetic claim someone has to defend, and the
honest report is the one that survives the question "what would have to be true?"

## Case study B — Domain 7: past the point of total assumption (technology)

**Situation.** A systems-integration subcontract ran on an incentive structure: target cost USD
2,000,000, target fee 150,000, 70/30 share, ceiling 2,450,000, so `PTA` USD 2,428,571. By month
eight the supplier's internal cost was tracking to 2,600,000, well past the `PTA`.

**What happened.** The supplier's behaviour changed abruptly and, in hindsight, rationally: fresh
change requests on work previously treated as in-scope, slower responses on defect fixes that
earned nothing, and a claim that the buyer's late environment provision had caused the growth.
The buyer's team read it as bad faith. It was arithmetic: past the `PTA` every additional
engineer-hour came out of the supplier's own margin, and the only route back to profit was
re-characterising cost as buyer-caused scope.

**The outcome.** The parties re-set commercially, a re-baselined target cost recognising the genuine
environment delay (documented, and partly the buyer's), a revised ceiling, and a time-and-materials
envelope for the disputed remainder. Delivery recovered within two months of the reset. The
retrospective's finding: the buyer had never computed the `PTA`, so a foreseeable inflection was
experienced as a betrayal.

**What the domain teaches here.** Commercial structures create behaviour. Computing the `PTA` at
signature (and watching the supplier's cost trend against it) turns an adversarial surprise into a
managed conversation held early, while both parties still have options (Domain 10's supplier
governance; Domain 11's negotiation).

## Case study C — Domain 7: the reserve that could not be reached in time (public health)

**Situation.** Meridian Care Records (the 40-clinic shared clinical-records programme of Domains 1,
2, 15 and 16) is at month 14. Its approved cost of **USD 2,400,000** is structured as **USD
2,160,000** of control-account budgets plus **USD 120,000** of contingency inside a baseline of
**USD 2,280,000**, with **USD 120,000** of management reserve outside it. Contingency is **5.56 %**
of the control-account total; management reserve is **5.26 %** of the baseline. Installation is
running well (this is the programme that will deliver 40 of 40 clinics), and **USD 74,000** of
contingency has been drawn against installation risks, leaving **USD 46,000**.

Adoption is not running well. Sixteen clinics are using the system where the case assumed
twenty-eight (Domain 1, KA 1.3.2). The clinical lead brings a costed **adoption-support package**
(floor-walking, workflow coaching and local super-users in the twelve clinics not yet using the
system) at **USD 168,000**, expected to move adoption from 40 % to the planned 70 %.

**The arithmetic the programme had not done.** Twelve clinics at Domain 1's benefit rate of USD 510 a
week each are worth **USD 6,120 a week**, or **USD 293,760** a year over a 48-week operating year. So
the package pays for itself in **27.4510 weeks** of operation. That is not the problem.

The problem is funding it. Remaining contingency is USD 46,000, so **USD 122,000** must come from
elsewhere; and USD 122,000 exceeds the *entire* USD 120,000 management reserve by USD 2,000. This is
therefore not a reserve draw at all but a request for additional funding, decided by the programme
board through change control (Domain 4, KA 4.4). Meridian's board sits on a four-week cycle with a
two-week paper deadline, so Domain 3's governance latency applies unchanged: `E[wait] = M/2 + L =
4/2 + 2 =` **4 weeks** (Domain 3, KA 3.2.3).

**Which cost of delay applies, and the error that is easy to make here.** Meridian's registered cost
of delay is **USD 14,280 a week**, and using it would price the wait at 4 × 14,280 = USD 57,120.
That would be wrong by a factor of **2.3333**. The registered figure is the benefit of the *whole*
28-clinic adoption the case assumed; this decision unlocks only the **12** additional clinics, worth
**USD 6,120 a week**. **The cost of delay applicable to a decision is the benefit that decision
unlocks, not the programme's headline figure**, and applying a portfolio-level rate to a
package-level choice inflates every business case built on it. So the four-week wait costs `4 ×
6,120 =` **USD 24,480**.

**The outcome.** The board approved the package at its next meeting, and the four-week wait cost
**14.57 %** of the package's own price, a whole month of benefit from twelve clinics, bought by
nobody and noticed by no one. The retrospective made three findings, each a number rather than a
sentiment.

*The reserve was pointed entirely at the half of the programme that was succeeding.* All USD 120,000
of contingency was sized against installation risks. The benefit-side exposure (the gap between 40 %
and 70 % adoption) is worth **USD 293,760** a year, **2.4480 times** the whole contingency, and
carried no reserve at all. Contingency sized only against delivery risk on a benefit-driven
programme funds the risks that will not decide the outcome.

*Decision-rights design is a cost parameter.* Had the sponsor pre-authorised a delegated
benefit-protection band of USD 150,000 to the programme director, decided in the director's own
weekly cycle with no paper lead time (`E[wait] = 0.5` weeks) the latency cost would have been **USD
3,060** instead of USD 24,480, a saving of **USD 21,420** on this single decision. The delegation is
free to create and costs nothing when unused; one decision pays for it many times over. Note the
sizing constraint: the decision here is the **USD 122,000** of additional funding, so any band below
that figure would have accelerated nothing, a delegation set below the size of the decisions it
exists to accelerate is decorative, which is Domain 3, KA 3.2.3's threshold-design point measured in
money. The USD 150,000 band above clears it with room for the next one.

*The package was late because it was unbudgeted, not because it was unaffordable.* At a 27.4510-week
payback it would have been approved at sanction had anyone computed it. What the funding structure
lacked was not money but a *line*: enabling change was outside the programme's cost baseline
entirely, which is Domain 2, KA 2.3's enabling-change point seen from the cost side.

**What the domain teaches here.** Reserve architecture is not bookkeeping. Which risks a reserve is
sized against decides what the programme can respond to; where the release authority sits decides
how fast; and both are priced by the same cost of delay that prices everything else. **A reserve
that cannot be reached inside the window in which the decision matters is a provision, not a
reserve.** The instruments are Domain 3's delegation thresholds and Domain 8's aggregation: this
domain's contribution is insisting that the latency be costed and that contingency be sized against
the risks that decide the benefit, not only those that decide the delivery.

---

## Executive perspective — Domain 7

What a project leader cannot delegate in this domain:

- **The forecast's assumption.** Analysts compute `EAC`; the leader owns *which* method and why,
  in one sentence a board can challenge. Reporting a number without its assumption is the
  domain's cardinal sin.
- **The `TCPI` reality test.** Before endorsing any recovery-to-budget plan: what index does the
  remaining work require, what is being demonstrated, and what specific mechanism closes the gap?
- **Reserve discipline.** Contingency and management reserve kept distinct, spent under stated
  authority, with consumption trended against progress: the erosion in MCQ 7.1-B is a leadership
  signal, not a bookkeeping detail.
- **Measurement integrity.** Earning rules fixed in advance, accruals present, level-of-effort share
  disclosed, no retrospective budget edits. Cost systems fail morally before they fail
  arithmetically: the same sentence as Domain 6, and the same reason.
- **The commercial shape.** Which model, why, and where the `PTA` sits. A leader who cannot say
  who carries cost risk on their largest package is not yet in control of it.
- **The confidence the baseline was approved at.** Not the number, the percentile. Auriga's
  4,000,000 was a P33 of its own declared range, and a leader who does not know that figure for
  their own baseline is managing to a target whose difficulty nobody has stated.
- **The cash position, in days of spend.** USD 1,043,600, 44.80 days, against a cost variance of
  200,000. The variance is the subject of the meeting and the cash is the thing that can stop the
  project, and it is the leader who has to insist both appear in the same pack. Payment terms and
  retention structures are delivery parameters, priced before signature, not procurement detail.
- **Whether the reserve can be reached in time.** Case study C's finding generalises: a reserve
  behind a four-week decision cycle cost USD 24,480 to open on a decision worth USD 6,120 a week,
  and no amount of the reserve being adequate compensates for its being slow. Delegated bands for
  benefit-protecting spend are free to create, must be set above the size of the decisions they
  exist to accelerate, and are a leadership decision rather than an administrative one: as is
  insisting that a cost of delay be the benefit *this* decision unlocks and not the programme's
  headline rate.

## Calculation exercises — Domain 7

**Exercise 7.1** `BAC` 4,000,000; `PV` 2,080,000; `EV` 1,920,000; `AC` 2,120,000. Compute `CV`,
`SV`, `CPI`, `SPI`, percent complete and percent spent. *Solution.* `CV` (200,000); `SV` (160,000);
`CPI` 0.91; `SPI` 0.92; complete `1,920,000/4,000,000 =` **48.0 %**; spent `2,120,000/4,000,000 =`
**53.0 %**. Common error: computing percent complete from `AC/BAC`; that is percent *spent*, and
reporting it as progress overstates the project.

**Exercise 7.2** Same data. Compute `EAC` by methods (a), (b) and (c), and the corresponding
`VAC`.
*Solution.* (a) `2,120,000 + 2,080,000 =` **4,200,000**, `VAC` **(200,000)**.
(b) `4,000,000/0.905660 =` **4,416,667**, `VAC` **(416,667)**.
(c) `2,120,000 + 2,080,000/(0.905660 × 0.923077) =` **4,608,056**, `VAC` **(608,056)**.
Common error: rounding `CPI` to 0.91 before dividing — `4,000,000/0.91 = 4,395,604`, USD 21,062
adrift. Indices are display; arithmetic is full precision.

**Exercise 7.3** Same data. Compute `TCPI` to `BAC` and to the method-(b) `EAC`, and interpret.
*Solution.* To `BAC`: `2,080,000/1,880,000 =` **1.11**. To (b): `2,080,000/(4,416,667 − 2,120,000) =
2,080,000/2,296,667 =` **0.91**. The second equals the current `CPI`, the identity of 7.3.4:
`BAC/CPI` forecasts *are* "nothing changes". Common error: using `PV` rather than `AC` in the
denominator (giving 1.08) and concluding recovery is nearly free.

**Exercise 7.4** Incentive subcontract: target cost 2,400,000; target fee 180,000; share 80/20;
ceiling 2,900,000. The seller finishes at 2,650,000. Find the fee, what the buyer pays, and the
`PTA`.
*Solution.* Overrun 250,000; fee `180,000 − 250,000 × 0.20 =` **130,000**; buyer pays
`2,650,000 + 130,000 =` **2,780,000** (below the 2,900,000 ceiling). Target price 2,580,000;
`PTA = 2,400,000 + (2,900,000 − 2,580,000)/0.80 = 2,400,000 + 400,000 =` **2,800,000**. Common
error: applying the buyer's 80 % share to the fee reduction.

**Exercise 7.5** A pool of 30 at USD 105/h, 20 at USD 150/h and 10 at USD 220/h. Compute the blended
rate, then the cost of adding four weeks of a 5-person crew at 40 h/week. *Solution.* `(30×105 +
20×150 + 10×220)/60 = (3,150 + 3,000 + 2,200)/60 = 8,350/60 =` **USD 139.17/h**. Crew cost `5 × 40 ×
4 × 139.17 =` **USD 111,333**. Common error: using the blended rate for a crew drawn entirely from
one grade. The blend applies to a representative mix, not to any arbitrary subset.

**Exercise 7.6** A point estimate of USD 4,000,000 carries a declared class range of −15 %/+30 %.
Treating it as a triangular distribution, find the bounds, the mean, the median, the P80, and the
percentile at which the point estimate sits. Then state the PERT-weighted mean of the same three
points. *Solution.* Bounds **3,400,000** and **5,200,000**: a band of 1,800,000, **45.0 %** of the
point. Mean `(3,400,000 + 4,000,000 + 5,200,000)/3 =` **4,200,000**. Median `5,200,000 − √(1,800,000
× 1,200,000/2) =` **4,160,770**. P80 `5,200,000 − √(0.20 × 1,800,000 × 1,200,000) =` **4,542,733**.
Percentile at the point `= (4,000,000 − 3,400,000)/1,800,000 =` **33.33 %**. PERT `(3,400,000 +
16,000,000 + 5,200,000)/6 =` **4,100,000**. Common error: reporting the point estimate as the
expected cost, with an asymmetric range the mean always exceeds the mode, so the approved figure had
a **66.67 %** chance of being exceeded.

**Exercise 7.7** Control accounts total 3,640,000; contingency 360,000; management reserve 240,000.
At 48.00 % complete, 190,000 of contingency has been drawn while `CV` is (200,000). Compute `BAC`,
the total funding requirement, the draw index, the projected total draw and the draw efficiency the
remainder must achieve, then state the reconciliation finding. *Solution.* `BAC` **4,000,000**;
funding requirement **4,240,000**. Draw index `(190,000/360,000)/0.48 =` **1.0995**. Rate
`190,000/48.00 =` **3,958.33 per point**, so projected total draw **395,833** and a shortfall of
**35,833** (14.93 % of the management reserve). Required efficiency `170,000/(3,958.33 × 52.00) =`
**0.8259**. The remainder must draw **17.41 %** more slowly. Finding: draws of 190,000 against a
`CV` of 200,000 leave **10,000** of variance with no identified cause. Common error: reading the
remaining balance (170,000/360,000 = 0.4722) as the required efficiency; that is a level, not an
index, and it says nothing about the rate.

**Exercise 7.8** `EV` 1,920,000; invoiced cost 1,920,000; a further 200,000 received and accepted
but neither invoiced nor accrued. The next period earns 480,000 for a true cost of 520,000 and the
backlog is invoiced in it. Compute reported and true `CPI` and `EAC(b)` at the data date, the
reported and true period `CPI`, and the cumulative `CPI` afterwards. *Solution.* Reported `CPI`
**1.0000**, `EAC(b)` **4,000,000**; true `CPI` **0.9057**, `EAC(b)` **4,416,667**: a forecast
understatement of **416,667**, 10.42 % of `BAC`. Period reported `480,000/720,000 =` **0.6667**
against a true **0.9231**, an error of **0.2564**. Cumulative afterwards `2,400,000/2,640,000 =`
**0.9091** either way. Common error: treating the 0.6667 as a production collapse. It implies costs
50.0 % above budget for the period, and the true movement was an *improvement* from 0.9057 to
0.9231.

**Exercise 7.9** Ten equal packages of 40,000 in one account: four complete, three at 30 % measured
physical completion, three not started. `PV` 240,000; `AC` 210,000. Compute `EV`, `SPI` and `CPI`
under 0/100, 50/50 and objective percent-complete earning. Then compute the reported `SPI` of a
separate account that is 70 % level of effort whose discrete work runs at `SPI` 0.60. *Solution.*
0/100 `EV` **160,000** (`SPI` 0.6667, `CPI` 0.7619); 50/50 **220,000** (0.9167, **1.0476**); percent
complete **196,000** (0.8167, 0.9333), an `EV` spread of **60,000**, 15.00 % of budget, on one
physical state. The level-of-effort account reports `1 − (1 − 0.70)(1 − 0.60) =` **0.88**, a
distortion of **0.28**. Common error: reading the 50/50 `CPI` of 1.0476 as good news, the rule, not
the work, produced it, and the same account read objectively is over-running at 0.9333.

**Exercise 7.10** Auriga's week-13 data. Compute `TCPI` to `BAC` and to each of the three `EAC`s,
and state what each equals. *Solution.* To `BAC` `2,080,000/1,880,000 =` **1.1064** (the recovery
demand). To `EAC(a)` `2,080,000/2,080,000 =` **1.0000** = the budgeted rate. To `EAC(b)`
`2,080,000/2,296,667 =` **0.9057** = `CPI`. To `EAC(c)` `2,080,000/2,488,056 =` **0.8360** = `CPI ×
SPI`. **`TCPI` to any `EAC` equals the efficiency that `EAC` assumes**, so choosing a forecast is
choosing a `TCPI`. Common error: believing `TCPI` measures something independent of the forecast. It
does not, and a `TCPI` that disagrees with the author's stated assumption is an arithmetic error in
the report.

**Exercise 7.11** The board will fund no more than the approved 4,000,000 and no efficiency
improvement is available. How much budgeted scope must be removed, and what is the exchange rate
between scope removed and cash saved? *Solution.* `D = (BAC − EV) − CPI × (BAC − AC) = 2,080,000 −
0.905660 × 1,880,000 =` **377,358** (exactly 20,000,000/53): **9.43 %** of `BAC` and **18.14 %** of
the remaining work. Completing the full scope instead needs **416,667** of extra funding, and
`416,667/377,358 =` **1.104167 = 1/CPI**: **one dollar of budgeted scope removed saves USD 1.10 of
forecast cash.** Common error: booking the saving without pricing the benefit lost, 9.43 % of budget
is not 9.43 % of benefit, and on an integrated system it is frequently much more.

**Exercise 7.12** A crew is 4 technicians × 40 h at USD 95/h. Price one crew-week of extra capacity
by overtime at 1.5×, by an agency crew at USD 128/h achieving 85 % relative productivity, and by
accepting a week's delay at USD 45,000. Then find the two productivity breakevens. *Solution.*
Straight time **15,200**; overtime **22,800** (1.50×); agency **24,094** per productive crew-week
(1.5851×); delay **45,000** (2.9605×). The agency crew beats overtime at a relative productivity of
`128/142.50 =` **89.82 %** or better; overtime beats the agency crew only while sustained-overtime
productivity holds above `22,800/24,094 =` **94.63 %**. Common error: comparing the agency's USD 128
with the in-house USD 95 and rejecting it; the relevant comparison is with the USD 142.50 overtime
rate it displaces, and the 24,094 is still 53.54 % of the cost of doing nothing.

**Exercise 7.13** A firm fixed price of USD 480,000 against time and materials at USD 130.625/h,
with hours estimated optimistic 2,900 / most likely 3,200 / pessimistic 4,100. Find the breakeven
hours, the premium over the most-likely estimate, the T&M cost at each of the three points, and the
probability (triangular) that the hours exceed the breakeven. *Solution.* Breakeven `480,000/130.625
=` **3,674.64 h**. T&M at 3,200 h **418,000**, so a premium of **62,000 = 14.83 %**: identical to
the hours overrun at breakeven. At 2,900 h **378,813**; at 4,100 h **535,563**, which is 55,563
worse than the fixed price. Exceedance `(4,100 − 3,674.6411)²/(1,200 × 900) =` **16.75 %**. Common
error: expressing the premium against the fixed price (12.92 %) rather than against the estimate.
That understates the overrun tolerance the buyer is actually purchasing.

**Exercise 7.14** Contract price 4,400,000 against `BAC` 4,000,000. At week 13 `EV` 1,920,000, `AC`
2,120,000, payables 742,000, cash received 334,400. Compute the margin to date, the funding
absorbed, the funding in days of spend, and the margin at completion under each of the three `EAC`s.
*Solution.* Revenue `1.10 × 1,920,000 =` 2,112,000, so margin to date **(8,000)**. Cash out
`2,120,000 − 742,000 =` 1,378,000; funding absorbed `1,378,000 − 334,400 =` **1,043,600** = 26.09 %
of `BAC`. Daily spend `2,120,000/91 =` 23,297, so **44.80 days of spend**. Margin at completion:
method (a) **+200,000** (+4.55 % of price); (b) **(16,667)** (−0.38 %); (c) **(208,056)** (−4.73 %).
Common error: assuming the bid margin of 400,000 survives. It requires an `EAC` of exactly `BAC`,
which is `TCPI` 1.11, so the margin conversation and the recovery conversation are one conversation.

**Exercise 7.15** Target cost 2,000,000; target fee 150,000; ceiling 2,450,000. Compute the `PTA` at
buyer shares of 50 %, 70 % and 90 %, the fee at the `PTA` in each case, and the zero-fee cost. State
which of the `PTA` and the zero-fee cost binds first at 70/30. *Solution.* `PTA = 2,000,000 +
300,000/s`: at 50 % **2,600,000**, at 70 % **2,428,571**, at 90 % **2,333,333**. Fee at the `PTA` `=
150,000 − (PTA − 2,000,000)(1 − s)`: **(150,000)**, **21,429** and **116,667**. Zero-fee cost `=
2,000,000 + 150,000/(1 − s)`: 2,300,000, **2,500,000**, 3,500,000. At 70/30 the `PTA` (2,428,571)
arrives **before** the fee is exhausted (2,500,000), so the ceiling binds first and the `PTA` is the
real inflection; at 50/50 it does not, and the `PTA` there is nominal. Common error: assuming a more
generous buyer share pushes the `PTA` further away, it brings it **closer**, by 95,238 between 70/30
and 90/10, because fixed headroom to the ceiling is consumed faster.

**Exercise 7.16** Auriga's baseline reaches `PV` 1,920,000 at week 12 and 2,080,000 at week 13, and
`EV` at week 13 is 1,920,000. Compute `ES`, `SPI(t)` and the time forecast. Then, at week 27 with
`EV` 3,920,000 against baseline `PV` of 3,880,000 at week 24 and `BAC` at week 25, compute `SPI`,
`ES`, `SPI(t)` and the time forecast, and explain the divergence. *Solution.* Week 13: `ES`
**12.0000**, `SPI(t)` `12/13 =` **0.9231** — equal to `SPI`, because the `PV` curve is locally
linear and the `SV` of (160,000) is exactly one week of planned value. Forecast `25/0.923077 =`
**27.0833 weeks**, 2.0833 late, which at USD 45,000 per week is **93,750** of delay cost. Week 27:
`ES = 24 + 40,000/120,000 =` **24.3333**, `SPI(t)` **0.9012**, forecast `25/0.901235 =` **27.7397
weeks** (2.7397 weeks or **10.96 %** late), while `SPI` reads **0.98**. Common error: quoting `SPI`
late in a project: past the planned finish `PV` is pinned at `BAC`, so `SPI` is arithmetically
driven to 1.00 and reported 2 % lateness for an 11 % overrun.

## Practitioner's toolkit — Domain 7

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 7.T.1 — Cost-report integrity checklist (run before publishing the month)

- [ ] Earning rules fixed in advance per package and unchanged this period; any change disclosed.
- [ ] `EV` claims sampled against physical evidence, not accepted on assertion.
- [ ] Accruals recognised for work received but not invoiced; open commitments cleansed.
- [ ] `PV` read from the current controlled baseline, phased over the approved schedule.
- [ ] No retro-fitted budgets; transfers between control accounts logged.
- [ ] `EV/BAC` reconciles to the reported percent complete; indices recompute from the four numbers.
- [ ] Every `EAC` states its method **and its assumption**; `TCPI` to `BAC` reported beside it.
- [ ] Contingency and management reserve shown separately, with consumption vs progress.
- [ ] Level-of-effort share disclosed per control account.
- [ ] AI-produced forecasts marked, calibration record attached, human owner named.

### Toolkit 7.T.2 — Estimate basis sheet (one per estimate)

Method (analogous/parametric/bottom-up) · definition maturity and accuracy class · range
(−x %/+y %) · rate sources with dates and escalation basis · quantities and their source ·
exclusions stated explicitly · risks feeding contingency (link to the register) · reviewer and
date. An estimate whose basis sheet cannot be produced is not releasable.

### Toolkit 7.T.3 — Commercial one-pager (per major package)

Model (FFP/FPIF/CPFF/CPIF/T&M) and why · who carries cost risk · target cost, fee, share ratio,
ceiling · **`PTA`, the zero-fee cost, which binds first, and the current cost trend against the
`PTA` expressed as headroom in per cent** · for T&M, the hours cap, the rate schedule and the
breakeven against any fixed-price alternative · payment terms both directions and the
cash profile · retention and release criteria · variation and claims route · escalation contacts.
Reviewed at every commercial checkpoint, not filed at signature.

### Toolkit 7.T.4 — Cash and commitment page (one per period, beside the cost report)

A single page, six lines, none of which appears in a conventional cost report and each of which has
changed a decision on the projects in this domain:

- **Cash paid out** = `AC` − payables, with the payables figure and its share of `AC`.
- **Cash received**, and the date of the oldest unpaid certified invoice.
- **Funding absorbed** = the difference: reported both in currency and in **days of spend**, so it
  is comparable across the portfolio.
- **Client exposure** = receivables + retention held + unbilled work in progress, with the
  reconciliation check *funding absorbed = client exposure − payables + loss to date*.
- **Retention**: held now, due at completion, held beyond completion; and each as a percentage of
  the contract margin.
- **Commitment coverage** = (`AC` + open commitments) ÷ `EAC`, with the uncommitted forecast balance,
  the weekly rate at which it must now be committed, and the exposure to a stated price movement.
  Stale open commitments cleansed this period are stated explicitly, since cleansing them worsens the
  page and improves its truth.

## Exam preparation — Domain 7

**The calculation traps.** Percent spent reported as percent complete (Exercise 7.1) · rounding
`CPI` before dividing (Exercise 7.2) · `PV` instead of `AC` in the `TCPI` denominator
(Exercise 7.3) · applying the wrong party's share to an incentive fee (Exercise 7.4) · `EV`
valued at actual cost rather than budget · reading a level-of-effort-heavy `SPI` as schedule
performance (MCQ 7.3-D) · quoting `SPI` late in a project where it must converge on 1.00
(7.A.1) · confusing commitments with actuals · treating contingency and management reserve as one
pot · reporting a point estimate as though it were the expected cost of an asymmetric range
(Exercise 7.6) · reading a remaining reserve *balance* as the reserve's required *efficiency*
(Exercise 7.7) · taking a single-period index at face value in the month an accrual backlog clears
(Exercise 7.8) · using 50/50 earning on long packages and reading the resulting favourable `CPI` as
performance (Exercise 7.9) · assuming `TCPI` says something independent of the `EAC` it is computed
to (Exercise 7.10) · booking a descope saving without pricing the benefit removed (Exercise 7.11) ·
comparing an agency hourly rate with straight time rather than with the overtime rate it displaces
(Exercise 7.12) · expressing a fixed-price premium against the price rather than the estimate
(Exercise 7.13) · assuming a bid margin survives a forecast overrun (Exercise 7.14) · assuming a
more generous share ratio pushes the `PTA` further away (Exercise 7.15) · adding a non-productive
time allowance as an uplift rather than dividing by utilisation (MCQ 7.4-E).

**The identities worth memorising.** `%spent − %complete = CV/BAC` · `1/CPI − 1` is the rate at which
the work performed exceeded its budget · `TCPI` to any `EAC` equals that `EAC`'s implied efficiency
(1.0000 / `CPI` / `CPI × SPI`) · a dollar of budgeted scope removed saves `1/CPI` dollars of forecast
cash · reported `SPI` = `1 − (1 − s)(1 − SPI_d)`, so level-of-effort distortion is `s × (1 − SPI_d)` ·
paid-hour to productive-hour cost is `1/(1 − n)`, and cost per productive hour scales as
`1/utilisation` · a fixed-price premium in per cent equals the hours overrun at which T&M breaks even ·
funding absorbed = client exposure − payables + loss to date · `E[wait] = M/2 + L` prices the latency
of the authority that releases a reserve.

**Reflection questions.**
1. Your cost report shows one `EAC`. What must accompany it before you will sign it? *(The method
   and the assumption about remaining work; `TCPI` to `BAC` beside it.)*
2. On your largest package: who carries cost risk, where is the `PTA`, and how close is the
   supplier's cost trend to it? *(7.4.2–7.4.3; toolkit 7.T.3.)*
3. Which invariant in 7.A.3 would have caught the last cost surprise you experienced, and why wasn't
   it running?
4. What percentile of its own declared range was your current baseline approved at, and who in the
   governance chain knows that number? *(7.1.1.)*
5. How many days of spend is your project absorbing, and what would 30-day rather than 60-day client
   terms be worth? *(7.4.4; toolkit 7.T.4.)*
6. If your board refused any further funding tomorrow, how much budgeted scope would have to come
   out, and which packages, priced against benefit? *(7.3.4b.)*
7. Where does the authority to release your largest reserve sit, what is `E[wait]` for it, and what
   does that wait cost per week? *(Case study C.)*

## Domain 7 summary

Cost leadership begins with an honest number, and honesty here is arithmetic rather than character:
a method matched to definition maturity, a range and class always stated; and *read*, since Auriga's
approved USD 4,000,000 sat at the 33.33rd percentile of its own declared range, whose mean of
4,200,000 is the very forecast week 13 produced. Three-point thinking where tails are real, with a
4.125 σ tail recognised as a scenario for Domain 8 rather than a spread; a budget built through
control accounts into a time-phased baseline with contingency inside it (USD 360,000) and management
reserve outside (USD 240,000) for a funding requirement of USD 4,240,000; and the contingency trend
read as an index of its own, where a draw of 52.78 % at 48.00 % complete demands that the remainder
run 17.41 % more frugally than the first half.

Measurement then has to be earned (`AC` with accruals, `EV` at budget under earning rules fixed in
advance), and both halves of that sentence carry a number. One unrecognised USD 200,000 accrual
makes a project reporting `CPI` 0.91 report 1.00 and understates its forecast by USD 416,667, then
punishes the innocent period in which the backlog clears. One choice of earning rule moves `EV` by
15.00 % of a control account's budget and flips it from over-running to under-running. Everything
downstream is arithmetic on numbers that fragile: `CV`, `SV`, `CPI`, `SPI` at Auriga's week 13 (0.91
and 0.92; 48 % complete against 53 % spent, whose 5-point gap *is* the USD 200,000); the `EAC`
family spanning USD 408,056 (a fifth of the remaining work), because each encodes a different
implied efficiency, and funding the cheapest of them buys the project only to 95.09 % complete; and
`VAC` and `TCPI` turning recovery talk into a defensible claim, since `TCPI` to any `EAC` is exactly
the efficiency that forecast assumes, and recovery to budget here means either 22.16 % more
productivity or USD 377,358 of scope out, at an exchange rate of `1/CPI`, USD 1.10 of cash saved per
dollar of budget removed.

The commercial half is the same discipline pointed outward. Blended rates make options commensurable
and the mix moves the price more than the headcount does; behind the rate sits a cost per
*productive* hour of USD 100.79 where five points of utilisation are worth USD 155,000:
three-quarters of the variance being debated; Domain 6's capacity ladder prices out at 15,200
straight, 22,800 overtime, 24,094 agency and 45,000 to do nothing, with the arguments living at
breakevens of 89.82 % and 94.63 %. Contract models allocate cost risk at a price whose crossing
point is computable (a 14.83 % fixed-price premium is exactly a 14.83 % tolerance on hours), and the
point of total assumption predicts when a supplier's incentives invert, moving *closer* as the buyer
concedes share and becoming nominal once the ceiling passes the zero-fee cost. Beneath all of it
runs cash: a project whose reported problem is USD 200,000 is absorbing USD 1,043,600, 44.80 days of
spend, half of which one payment term would return, holding retention worth 55.00 % of its margin,
and whose profitability is decided by which `EAC` is believed. At programme scale Meridian adds the
last lesson: a reserve sized entirely against delivery risk while the exposure sat on the benefit
side, behind an authority whose four-week wait cost USD 24,480 — 14.57 % of the decision it was
blocking, and a reminder that the cost of delay applicable to a decision is the benefit that
decision unlocks, not the programme's headline rate.

Throughout, the leadership rule holds: the number is only as good as the assumption named beside it,
and machine-produced forecasts reach a board only with a calibration record and a human owner.
Domain 8 quantifies the risk this domain reserves for; Domain 9 costs the rework this domain's
engineer-week prices; Domain 10 takes the commercial relationships into their own depth.
