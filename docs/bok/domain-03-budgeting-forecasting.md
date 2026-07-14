# Domain 3 — Budgeting & Forecasting

> **Group:** Finance, accounting & reporting (Domain 3 of 4). **Target:** ~125 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md). Uses the master formula symbols (`PV`, `BAC`,
> `EV`, `AC`, `CPI`, `SPI`, `EAC`, `ETC`, `VAC`) — restated on use. British English; USD (+SAR where useful).

## Why this domain exists

A budget is the financial expression of a plan, and a forecast is the financial expression of reality
catching up with that plan. Between them sits most of what a project controls professional does: build a
credible estimate, phase it across the schedule into a **cost baseline**, and then — as the work proceeds —
forecast the **estimate at completion** honestly enough that decisions are made in time. This domain covers
budgeting fundamentals and the two kinds of reserve (KA 3.1); cost estimation, its classes and methods
(KA 3.2); the time-phased budget or **cost baseline** and its S-curve, which is the **Planned Value (`PV`)**
that earned value measures against (KA 3.3); forecasting and the **EAC family** (KA 3.4, developed fully in
Domain 6); and **cash-flow forecasting**, the difference between a project being *profitable* and being
*funded* (KA 3.5).

**Learning objectives.** After this domain a candidate can: distinguish top-down, bottom-up and zero-based
budgeting and separate contingency reserve from management reserve; select an estimating method and state its
accuracy class; build a time-phased cost baseline (`PV`) and read its S-curve; compute an estimate at
completion by the main methods and interpret the difference; and build a project cash-flow forecast and
identify the peak funding requirement.

---

## Knowledge Area 3.1 — Budgeting fundamentals

*Topics: 3.1.1 the purpose of a budget · 3.1.2 top-down, bottom-up and zero-based · 3.1.3 the project budget
baseline · 3.1.4 contingency reserve vs management reserve.*

### 3.1.1 The purpose of a budget

**Definition & purpose.** A **budget** is an approved, quantified plan of the resources an activity is
expected to consume over a period. For a project it is the **authorised cost of the scope**, phased over the
schedule. A budget does three jobs at once: it **authorises** spend, it sets the **baseline** against which
performance is measured (Domains 4 and 6), and it **communicates** intent to those who must deliver within
it. A budget nobody is accountable to, or that is never compared to actuals, is a wish, not a control.

### 3.1.2 Top-down, bottom-up and zero-based budgeting

**The approaches.**

- **Top-down** — senior management sets an overall figure (from strategy, an analogous project, or an
  affordability limit), which is then apportioned down. Fast, but risks being disconnected from what the work
  actually requires.
- **Bottom-up** — the budget is built by estimating each work package (Domain 1, KA 1.5) and summing upward.
  Slower and needs a defined scope/WBS, but is defensible and owned by those who will deliver.
- **Zero-based** — every cost is justified from a zero base each cycle, rather than rolled forward with an
  increment. Rigorous for recurring overhead; heavy to run every period.

In practice most project budgets are **bottom-up against a top-down affordability constraint** — the two are
reconciled, and the gap is where scope, contingency or ambition is adjusted.

**Worked example 3.1.2 — reconciling bottom-up to a top-down constraint.**

1. **Setup.** The bottom-up estimate totals **USD 10,600,000**; the sponsor's affordability limit is **USD
   10,000,000** — a **USD 600,000** gap.
2. **Formula.** Close the gap through explicit, owned decisions — descoping, value engineering, and a
   risk-based contingency review — never by silently shaving numbers.
3. **Substitution.** Deferred scope (a non-essential phase-2 fit-out) **(250,000)**; value engineering
   (respecified materials at equal function) **(200,000)**; contingency re-derived after mitigations (Domain
   12) **(150,000)** → total **(600,000)**.
4. **Result.** A reconciled budget of **USD 10,000,000**, with every reduction traceable to a decision and an
   owner in the basis of estimate (3.2.3).
5. **Interpretation.** The reconciliation IS the value of meeting in the middle — the gap is closed by visible
   trade-offs the sponsor approves, not by optimism. A budget cut without a matching scope/risk decision is
   simply a future overrun booked early.

### 3.1.3 The project budget baseline

**Definition & purpose.** The **cost baseline** (also **performance measurement baseline**, PMB, on its cost
axis) is the **approved, time-phased budget** against which cost performance is measured — it is the source of
**Planned Value (`PV`/BCWS)** in earned value. It is **version-controlled**: it changes only through approved
change control (Domain 5, KA 5.4), so that variance measured against it is meaningful. The total value of the
cost baseline is the **Budget at Completion (`BAC`)**.

### 3.1.4 Contingency reserve versus management reserve

**The principle — two reserves for two kinds of uncertainty.**

- **Contingency reserve** covers **identified risks** (the "known-unknowns" — risks in the register, Domain
  12) and quantified estimating uncertainty. It is **inside the cost baseline**, under the project manager's
  control, and drawn down as risks materialise.
- **Management reserve** covers **unidentified risks** (the "unknown-unknowns") and scope not yet foreseen. It
  sits **outside the cost baseline**, is controlled by management/the sponsor, and is not part of `PV` or the
  `BAC` used for earned-value measurement — drawing on it is a baseline *change*, not a variance.

```
Cost baseline (PMB)   = Σ control-account budgets + undistributed budget + contingency reserve   → its total is BAC
Total project budget  = Cost baseline (BAC) + management reserve
```

**Worked example 3.1.4 — assemble the budget.**

1. **Setup.** Control-account budgets total **USD 9,000,000**; a risk-based **contingency reserve** of **USD
   700,000**; the sponsor holds a **management reserve** of **USD 500,000**.
2. **Formula.** `BAC = control-account budgets + contingency`; `total budget = BAC + management reserve`.
3. **Substitution.** `BAC = 9,000,000 + 700,000 = 9,700,000`; `total budget = 9,700,000 + 500,000 =
   10,200,000`.
4. **Result.** **`BAC` = USD 9,700,000** (the earned-value baseline); **total authorised budget = USD
   10,200,000**.
5. **Interpretation.** Cost performance (`CPI`, EAC) is measured against the `BAC` of 9,700,000; the 500,000
   management reserve is not part of that measurement — releasing it re-baselines the project. Confusing the
   two is a classic error that either hides available funds or corrupts the variance.

> **Fig 3.1.1 — The budget waterfall.** *Caption:* from work-package budgets to total authorised budget.
> *Underlying data:* control accounts 9,000,000; + contingency 700,000 = cost baseline/BAC 9,700,000; +
> management reserve 500,000 = total budget 10,200,000. *Render-ready description:* a horizontal waterfall —
> a base bar "Control-account budgets 9.0m", a rising step "+ Contingency 0.7m" bracketed as "Cost baseline
> (BAC) 9.7m" in brand blue, a further step "+ Management reserve 0.5m" (grey, dashed, "outside baseline")
> to "Total budget 10.2m". Bracket labels distinguish what is inside vs outside the baseline.

### Key terms — KA 3.1

| Term | Meaning |
|---|---|
| **Cost baseline / PMB** | The approved, time-phased budget; source of Planned Value; total = BAC. |
| **Budget at Completion (`BAC`)** | The total value of the cost baseline. |
| **Contingency reserve** | For identified risks; inside the baseline; PM-controlled. |
| **Management reserve** | For unforeseen scope/risk; outside the baseline; management-controlled. |
| **Zero-based budgeting** | Justifying every cost from zero each cycle. |

### Sample MCQs — KA 3.1

**MCQ 3.1-A `[3.1.4 · Analysis]`** Which statement about management reserve is correct?
- A. It is part of the cost baseline and Planned Value.
- B. It sits outside the cost baseline; drawing on it is a baseline change, not a variance. ✅
- C. It covers identified risks in the risk register.
- D. It is controlled by the project scheduler.

*Rationale:* Management reserve is outside the PMB, for unknown-unknowns, released by management as a baseline
change. Contingency reserve (not management reserve) covers identified risks and sits inside the baseline; A,
C and D describe contingency or are simply wrong.

**MCQ 3.1-B `[3.1.4 · Application]`** Control-account budgets are USD 9,000,000, contingency reserve USD
700,000, management reserve USD 500,000. The BAC is:
- A. USD 9,000,000
- B. USD 9,700,000 ✅
- C. USD 10,200,000
- D. USD 500,000

*Rationale:* `BAC = 9,000,000 + 700,000 = 9,700,000` (contingency is inside the baseline; management reserve
is not). A omits contingency; C adds management reserve; D is the reserve alone.

**MCQ 3.1-C `[3.1.2 · Recall]`** Which budgeting approach requires every cost to be justified from a zero
base each cycle rather than rolled forward with an increment?
- A. Top-down budgeting.
- B. Bottom-up budgeting.
- C. Zero-based budgeting. ✅
- D. Rolling-wave budgeting.

*Rationale:* Zero-based budgeting rebuilds the case for every cost from zero each cycle — rigorous for
recurring overhead but heavy to run. Top-down apportions an overall figure; bottom-up builds from work
packages; neither requires re-justification from zero, and D is a planning technique, not a budgeting
approach defined here.

**MCQ 3.1-D `[3.1.4 · Application]`** A project's cost baseline (`BAC`) is USD 12,400,000, of which USD
900,000 is contingency reserve; management reserve is USD 600,000. The total authorised project budget is:
- A. USD 11,500,000
- B. USD 12,400,000
- C. USD 13,000,000 ✅
- D. USD 13,900,000

*Rationale:* `Total budget = BAC + management reserve = 12,400,000 + 600,000 = 13,000,000`; the contingency
is already inside the baseline. A wrongly strips the contingency out; B forgets the management reserve; D
double-counts the contingency by adding it again on top of the BAC.

### Self-check — KA 3.1

1. Contrast contingency reserve and management reserve by *type of uncertainty*, *location* and *controller*.
   *(Identified vs unidentified risk; inside vs outside the baseline; PM vs management.)*
2. Why must the cost baseline be version-controlled? *(So variance measured against it is meaningful; it
   changes only through approved change control.)*

---

## Knowledge Area 3.2 — Cost estimation

*Topics: 3.2.1 estimate classes and accuracy · 3.2.2 estimating methods (analogous, parametric, bottom-up) ·
3.2.3 the basis of estimate.*

### 3.2.1 Estimate classes and accuracy

**Definition & purpose.** An estimate's reliability depends on how mature the scope definition is behind it.
The **AACE International estimate-classification** framework describes five classes, from **Class 5** (earliest,
concept-screening, based on very limited definition) to **Class 1** (most mature, near-complete definition,
suitable for a definitive bid or control estimate). As definition matures, the **expected accuracy range
narrows**. Indicative ranges (which vary by industry and should be stated, not assumed):

| Class | Maturity / purpose | Indicative accuracy range |
|---|---|---|
| 5 | Concept screening | roughly −30 % to +50 % (can be wider) |
| 4 | Study / feasibility | roughly −20 % to +30 % |
| 3 | Budget authorisation / control | roughly −15 % to +20 % |
| 2 | Control / bid | roughly −10 % to +15 % |
| 1 | Definitive / check estimate | roughly −5 % to +10 % |

The professional discipline is to **state the class and range** with every estimate. A single-point number
with no range invites false precision — and a Class 5 concept figure quoted as if it were Class 1 is one of
the most common causes of later "overruns" that were never really overruns, merely early estimates treated as
commitments.

### 3.2.2 Estimating methods

**The three principal methods.**

- **Analogous (top-down)** — scale the known cost of a *similar* past project by a driver (size, capacity).
  Fast, low-effort, low-accuracy; useful early. `Estimate = past cost × (this driver / past driver)`.
- **Parametric** — apply a statistically-derived **rate** to a measured **parameter**. `Estimate =
  parameter × rate` (e.g. cost per m², per MW, per km). Accuracy depends on the quality of the rate and the
  homogeneity of the work.
- **Bottom-up (definitive)** — estimate each work package from first principles (quantities × rates, labour
  hours × rates) and sum upward. Highest effort and accuracy; needs a defined WBS.

**Worked example 3.2.2 — the same building, three ways.**

1. **Setup.** Estimate an office of **5,000 m²**. A similar recent office of **4,500 m²** cost **USD
   10,000,000**. A parametric rate of **USD 2,200/m²** is available. A bottom-up build-up of packages sums to
   **USD 11,300,000**.
2. **Analogous.** `10,000,000 × (5,000 / 4,500) = 10,000,000 × 1.1111 = 11,111,111` → **~USD 11.11m** (Class
   4–5).
3. **Parametric.** `5,000 × 2,200 = 11,000,000` → **USD 11.00m** (Class 3–4).
4. **Bottom-up.** **USD 11.30m** (Class 1–2), with a basis of estimate behind every package.
5. **Interpretation.** The three converge around USD 11m, which *increases confidence*, but they are not
   interchangeable: the analogous figure is a sanity check, the parametric a planning estimate, the bottom-up
   the number to bid or baseline. Presenting the bottom-up figure **with its class and range** (e.g. USD
   11.30m, Class 2, −10 % / +15 % → USD 10.17m to USD 13.00m) is the professional output.

**Worked example 3.2.2b — a bottom-up build-up with contingency and range.**

1. **Setup.** Build a bottom-up estimate from three work packages: **substructure** (1,200 labour hours at
   USD 60/hour plus USD 80,000 materials), **superstructure** (2,000 hours at USD 60/hour plus USD 150,000
   materials) and **fit-out** (1,500 hours at USD 55/hour plus USD 120,000 materials), with an **8 %
   contingency** on the subtotal, reported as a Class 2 estimate at −10 % / +15 %.
2. **Formula.** Per package: `estimate = (hours × rate) + materials`; then `subtotal = Σ packages`;
   `total = subtotal × 1.08`; range = `total × 0.90` to `total × 1.15`.
3. **Substitution.** Substructure `= 1,200 × 60 + 80,000 = 72,000 + 80,000 = 152,000`; superstructure
   `= 2,000 × 60 + 150,000 = 120,000 + 150,000 = 270,000`; fit-out `= 1,500 × 55 + 120,000 = 82,500 +
   120,000 = 202,500`. Subtotal `= 152,000 + 270,000 + 202,500 = 624,500`; contingency `= 624,500 × 0.08 =
   49,960`; total `= 624,500 + 49,960 = 674,460`. Range: `674,460 × 0.90 = 607,014`; `674,460 × 1.15 =
   775,629`.
4. **Result.** Total estimate **≈ USD 674,000**, stated as **Class 2, −10 % / +15 % → USD 607,000 to USD
   776,000**.
5. **Interpretation.** A bottom-up estimate is defensible because every line traces to a quantity × rate or a
   work-package build-up — a reviewer can challenge the hours, the rate or the materials allowance, not just
   the total. And it is reported **with its class and range**, not as a bare point, consistent with the
   discipline of 3.2.1.

**3.2.2c Sector mini-case — a data-centre estimate maturing through the classes.**

A hyperscale data-centre programme illustrates the estimate journey of 3.2.1. At concept (**Class 5**), a
capacity-scaled figure of **USD 50m** carries roughly **−30 %/+50 %** (≈ USD 35m–75m) — and is quoted to the
board *with* that range. After feasibility (**Class 3**), improved definition supports **USD 58m** at
**−15 %/+20 %** (**USD 49.3m–69.6m**). At the definitive stage (**Class 1**), the bottom-up estimate is **USD
61m** at **−8 %/+8 %** (**USD 56.1m–65.9m**). The point is that the estimate did not "grow" from 50m to 61m —
the **range narrowed** as definition matured, and the final figure sits inside every earlier range.
Presenting each stage with its class and range (3.2.1) is what prevented the concept figure being treated as
a commitment — the false-precision pitfall of quoting an early point number without its band.

### 3.2.3 The basis of estimate (BoE)

**Definition & purpose.** The **basis of estimate** is the document that records *how* the estimate was
built: the scope and assumptions, inclusions and **exclusions**, the rates and sources used, the estimate
class, the risks and the contingency logic. It is what makes an estimate **auditable and defensible**, and
what lets a reviewer challenge assumptions rather than argue about a single number. A change in an assumption
recorded in the BoE is traceable to a change in the estimate — the same discipline that later governs the
cost baseline (3.1.3).

**Common pitfall.** Treating an early, wide-range estimate as a firm commitment. The remedy is cultural and
documentary: always state the class and range, and keep the BoE current, so stakeholders anchor on a *range
that tightens* as definition matures, not a *point that appears to move*.

### Key terms — KA 3.2

| Term | Meaning |
|---|---|
| **Estimate class (AACE 5–1)** | Maturity-based classification driving expected accuracy range. |
| **Analogous / parametric / bottom-up** | Top-down scaling / rate × parameter / work-package build-up. |
| **Accuracy range** | The expected low/high band around a point estimate for its class. |
| **Basis of estimate (BoE)** | The auditable record of scope, assumptions, rates, exclusions and class. |

### Sample MCQs — KA 3.2

**MCQ 3.2-A `[3.2.2 · Application]`** A 4,500 m² building cost USD 10,000,000. Estimated analogously, a
comparable 5,000 m² building costs about:
- A. USD 9,000,000
- B. USD 10,000,000
- C. USD 11,111,111 ✅
- D. USD 12,500,000

*Rationale:* `10,000,000 × (5,000/4,500) = 11,111,111`. A scales the wrong way; B ignores the size change; D
overscales.

**MCQ 3.2-B `[3.2.1 · Analysis]`** A concept-stage (Class 5) estimate is quoted to a board as a firm budget
with no range. The main risk is:
- A. The estimate is too conservative.
- B. False precision — a wide-range early figure is treated as a commitment, so later refinement reads as an "overrun." ✅
- C. It violates IFRS 15.
- D. Nothing, provided it was bottom-up.

*Rationale:* A Class 5 figure carries a wide range; presenting it as a point commitment invites apparent
overruns as the estimate matures. It is not an IFRS matter, and a Class 5 estimate is by definition not
bottom-up/definitive.

**MCQ 3.2-C `[3.2.3 · Recall]`** The primary purpose of a basis of estimate is to:
- A. Replace the risk register.
- B. Make the estimate auditable and defensible by recording scope, assumptions, rates and exclusions. ✅
- C. Set the pass mark for the estimate.
- D. Serve as the contract.

*Rationale:* The BoE documents how the estimate was built so it can be challenged and defended. It is not the
risk register, a threshold, or the contract.

**MCQ 3.2-D `[3.2.2 · Application]`** A pipeline is estimated parametrically at USD 850,000 per km for
12 km, plus 15 % contingency on the base. The total estimate is:
- A. USD 1,530,000
- B. USD 8,670,000
- C. USD 10,200,000
- D. USD 11,730,000 ✅

*Rationale:* Base `= 12 × 850,000 = 10,200,000`; with contingency `10,200,000 × 1.15 = 11,730,000`. A is the
contingency alone (`10,200,000 × 0.15`); B deducts the contingency instead of adding it; C omits the
contingency step entirely.

**MCQ 3.2-E `[3.2.1 · Recall]`** Under the AACE estimate-classification framework, which class reflects
near-complete scope definition and suits a definitive bid or check estimate?
- A. Class 5
- B. Class 4
- C. Class 3
- D. Class 1 ✅

*Rationale:* Classes run from 5 (earliest, concept screening, widest range) to 1 (most mature, narrowest
range, definitive/check estimate). Class 5 and 4 are concept and feasibility stages; Class 3 supports budget
authorisation, not a definitive bid.

### Self-check — KA 3.2

1. As an estimate moves from Class 5 to Class 1, what happens to its accuracy range and why? *(It narrows, as
   scope definition matures.)*
2. Name the three estimating methods and when each fits. *(Analogous — early/sanity; parametric — planning
   with good rates; bottom-up — definitive, needs a WBS.)*

---

## Knowledge Area 3.3 — The time-phased budget / cost baseline (Planned Value)

*Topics: 3.3.1 spreading cost over the schedule · 3.3.2 the S-curve · 3.3.3 building a time-phased budget.*

### 3.3.1 Spreading cost over the schedule

**Definition & purpose.** A total budget (`BAC`) becomes a **control** only once it is **phased across the
schedule** — each control account's budget spread over the periods in which its work is planned. The result
is the **time-phased cost baseline**: the cumulative planned spend over time, which *is* **Planned Value
(`PV`/BCWS)**. Without phasing, you can compare total budget to total actual only at the end — too late;
with phasing, you can compare *planned-to-date* against *actual-* and *earned-to-date* at every reporting
period (Domains 4 and 6).

### 3.3.2 The S-curve

**The principle.** Plotted cumulatively over time, planned spend on most projects traces an **S-curve**: a
slow start (mobilisation, few resources), a steep middle (peak execution), and a tapering finish
(commissioning, demobilisation). The S-curve is the single most recognisable artefact in project controls: it
is the picture of `PV`, and later the canvas on which `EV` (earned) and `AC` (actual) are drawn to reveal
performance at a glance.

### 3.3.3 Building a time-phased budget — worked

**Worked example 3.3.3 — phase a USD 1,000,000 baseline across 10 months.**

1. **Setup.** A control account with `BAC` = **USD 1,000,000** over **10 months**, planned to an S-shaped
   profile (slow–fast–slow).
2. **Method.** Assign a monthly planned value; accumulate to the cumulative `PV` curve.
3. **Result.**

   | Month | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 |
   |---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
   | Monthly PV (000) | 40 | 70 | 110 | 140 | 160 | 150 | 130 | 100 | 60 | 40 |
   | Cumulative PV (000) | 40 | 110 | 220 | 360 | 520 | 670 | 800 | 900 | 960 | 1,000 |

   Monthly values sum to **1,000** (= `BAC`). ✓ The cumulative row is the Planned Value S-curve.
4. **Interpretation.** At the end of Month 5 the plan says **USD 520,000** should have been accomplished
   (`PV = 520,000`). When actual progress is measured (`EV`) and actual cost booked (`AC`) at that date, the
   three numbers together give schedule and cost performance (Domain 6). The baseline's *shape* matters: a
   back-loaded plan will always look "behind" early even when on track, so the phasing must reflect the real
   execution plan, not a straight line.

> **Fig 3.3.1 — The Planned Value S-curve.** *Caption:* cumulative planned spend over the 10-month baseline.
> *Underlying data:* the cumulative row above {40, 110, 220, 360, 520, 670, 800, 900, 960, 1,000} in
> USD 000. *Render-ready description:* x-axis Months 1–10, y-axis cumulative USD 000 to 1,000; a smooth
> S-shaped brand-blue curve through the cumulative points; light grey vertical bars for the monthly PV.
> *Animation storyboard (digital-only):* the curve draws month by month; a moving vertical "data date" line
> sweeps across, and at Month 5 a marker reads "PV = 520" — later reused to overlay EV and AC in Domain 6.

### Key terms — KA 3.3

| Term | Meaning |
|---|---|
| **Time-phased budget** | The `BAC` spread across the schedule by period. |
| **Planned Value (`PV`/BCWS)** | Cumulative planned spend to date — the cost-baseline curve. |
| **S-curve** | The characteristic cumulative-spend shape: slow–fast–slow. |

### Sample MCQs — KA 3.3

**MCQ 3.3-A `[3.3.3 · Application]`** With the monthly plan {40, 70, 110, 140, 160, …} (USD 000), the Planned
Value at the end of Month 4 is:
- A. USD 140,000
- B. USD 360,000 ✅
- C. USD 320,000
- D. USD 520,000

*Rationale:* `PV` is cumulative: `40 + 70 + 110 + 140 = 360` (USD 000). A is only Month 4; D is Month 5's
cumulative; C miscounts.

**MCQ 3.3-B `[3.3.2 · Analysis]`** A project's baseline is straight-lined even though execution ramps up
slowly. The likely early effect is that the project will:
- A. Always appear ahead of schedule.
- B. Appear behind against Planned Value even when on plan, because PV is overstated early. ✅
- C. Show no schedule variance ever.
- D. Have a higher BAC.

*Rationale:* A straight line front-loads `PV` relative to a slow real ramp, so early `EV` lags `PV`, showing a
misleading negative schedule variance. It does not change `BAC` or guarantee zero variance.

**MCQ 3.3-C `[3.3.1 · Recall]`** Spreading the `BAC` across the schedule period by period produces the
time-phased cost baseline. Which earned-value quantity *is* that cumulative curve?
- A. Earned Value (`EV`).
- B. Actual Cost (`AC`).
- C. Planned Value (`PV`). ✅
- D. Estimate at Completion (`EAC`).

*Rationale:* The cumulative planned spend to date is Planned Value (`PV`/BCWS) — the baseline curve earned
value measures against. `EV` measures work accomplished and `AC` cost booked, both from performance data,
not the plan; `EAC` is a forecast, not the baseline.

**MCQ 3.3-D `[3.3.3 · Application]`** A baseline shows cumulative Planned Value of USD 670,000 at the end of
Month 6 and USD 900,000 at the end of Month 8. The planned spend for Months 7 and 8 together is:
- A. USD 230,000 ✅
- B. USD 670,000
- C. USD 900,000
- D. USD 1,570,000

*Rationale:* Cumulative curves are differenced to recover period values: `900,000 − 670,000 = 230,000`. B
and C are the cumulative readings themselves, not the two-month increment; D adds the two cumulative values
instead of subtracting them.

### Self-check — KA 3.3

1. What earned-value quantity *is* the time-phased cost baseline? *(Planned Value, `PV`/BCWS.)*
2. Why must the baseline's phasing reflect the real execution plan rather than a straight line? *(Otherwise
   early schedule variance is an artefact of the phasing, not of performance.)*

---

## Knowledge Area 3.4 — Forecasting

*Topics: 3.4.1 what forecasting is · 3.4.2 the estimate at completion (EAC) family · 3.4.3 rolling forecasts
and trend analysis.* (The full EAC treatment is Domain 6; here the logic is introduced within budgeting.)

### 3.4.1 What forecasting is

**Definition & purpose.** A **forecast** is the current best estimate of a future outcome given performance to
date. In cost terms the headline forecast is the **Estimate at Completion (`EAC`)** — what the whole job will
cost by the time it finishes — and its companion the **Estimate to Complete (`ETC`)** — what the *remaining*
work will cost from now. A forecast is not the baseline and not a wish: it is an honest projection that should
change as evidence changes, and its value is entirely in being produced **early enough to act on**.

### 3.4.2 The estimate at completion (EAC) family

**The formulae.** All `EAC` methods share the identity `EAC = AC + ETC`; they differ in how `ETC` is
estimated from performance to date. Restating the spine symbols: `AC` actual cost to date; `EV` earned value;
`BAC` budget at completion; `CPI = EV/AC`; `SPI = EV/PV`.

```
(a) Remaining work at the budgeted rate (current variance is atypical):
    EAC = AC + (BAC − EV)
(b) Remaining work at the current cost-performance rate (variance is typical):
    EAC = BAC / CPI
(c) Remaining work affected by BOTH cost and schedule performance:
    EAC = AC + (BAC − EV) / (CPI × SPI)
Variance at completion:   VAC = BAC − EAC
```

**Worked example 3.4.2 — three EACs on one status.**

1. **Setup.** `BAC` = **USD 1,000,000**. At the data date: `AC` = **520,000**, `EV` = **480,000**, `PV` =
   **500,000**.
2. **Indices.** `CPI = 480,000 / 520,000 = 0.92`; `SPI = 480,000 / 500,000 = 0.96`.
3. **Substitution & Result.**
   - (a) `EAC = 520,000 + (1,000,000 − 480,000) = 520,000 + 520,000 = ` **USD 1,040,000**.
   - (b) `EAC = 1,000,000 / 0.9231 = ` **USD 1,083,333**.
   - (c) `EAC = 520,000 + 520,000 / (0.9231 × 0.96) = 520,000 + 520,000/0.8862 = 520,000 + 586,807 = ` **USD
     1,106,807**.
   - `VAC` (method b) `= 1,000,000 − 1,083,333 = ` **(USD 83,333)** — a projected overrun.
4. **Interpretation.** The three answers — 1.04m, 1.08m, 1.11m — bracket the outcome and encode *different
   assumptions*: (a) says today's overrun was a one-off; (b) says it will persist at the current cost rate;
   (c) says being both over-cost and behind schedule will compound. The professional does not pick a formula
   mechanically; they choose the assumption that matches the *cause* of the variance and defend it. This is
   the crux developed in Domain 6, KA 6.3.

**Worked example 3.4.2b — a rolling forecast across two periods (the CPI trend).**

1. **Setup.** The same project, `BAC` = **USD 1,000,000**, forecast by method (b) at two successive data
   dates. **Period 1 (Month 4):** `EV` = 350,000; `AC` = 370,000; `PV` = 360,000. **Period 2 (Month 5):**
   `EV` = 480,000; `AC` = 530,000; `PV` = 520,000.
2. **Formula.** Each period: `CPI = EV / AC`; `EAC = BAC / CPI`.
3. **Substitution.** Period 1: `CPI = 350,000 / 370,000 = 0.95`; `EAC = 1,000,000 / CPI ≈ 1,057,000`.
   Period 2: `CPI = 480,000 / 530,000 = 0.91`; `EAC = 1,000,000 / CPI ≈ 1,104,000`.
4. **Result.** `CPI` **0.95 → 0.91**; `EAC` **≈ USD 1,057,000 → ≈ USD 1,104,000** in a single month.
5. **Interpretation.** Re-produced each period, the forecast reveals a deteriorating `CPI` trend (0.95 →
   0.91) and a rising `EAC` (1,057,000 → 1,104,000). The **trend across periods** is the early warning — a
   single period's `CPI` could be timing noise, but two consecutive readings moving the same way demand
   investigation of the driver behind them (3.4.3, and Domain 6, KA 6.3).

### 3.4.3 Rolling forecasts and trend analysis

**The principle.** A **rolling forecast** is re-produced every period (rather than once a year), always
looking a fixed horizon ahead — so the forecast never goes stale. **Trend analysis** looks at the *direction*
of `CPI`/`SPI` and cost over successive periods: a `CPI` drifting down period after period is a stronger
signal than any single month's value, and often the earliest warning of a systemic problem. Driver-based
forecasting (linking the forecast to the physical drivers — quantities, productivity, rates — rather than
extrapolating money alone) is more robust still, and is where AI adds most (below).

**AI in this KA.** Forecasting is one of AI's strongest project-controls use cases: machine-learning models
can project `EAC` from performance trends and leading indicators, generate scenario ranges, and surface
early-warning signals a monthly review would miss (a productivity driver turning before it shows in `CPI`).
The limits are equally real: models trained on unrepresentative history mislead; they can be confidently
wrong; and a forecast is an input to a professional's judgement, not a substitute for it — the person remains
accountable for the number defended to the board. **AI proposes, the professional disposes.** (Predictive EAC
and driver analysis are developed in Domain 6 and Domain 13, KA 13.5.)

### Key terms — KA 3.4

| Term | Meaning |
|---|---|
| **EAC / ETC** | Estimate at completion / to complete; `EAC = AC + ETC`. |
| **CPI / SPI** | Cost / schedule performance index (`EV/AC`, `EV/PV`). |
| **VAC** | Variance at completion (`BAC − EAC`). |
| **Rolling forecast** | A forecast re-produced each period over a fixed forward horizon. |
| **Trend analysis** | Reading the direction of indices/cost over successive periods. |

### Sample MCQs — KA 3.4

**MCQ 3.4-A `[3.4.2 · Application]`** `BAC` = 1,000,000; `AC` = 520,000; `EV` = 480,000. Using `EAC = BAC/CPI`,
the forecast is closest to:
- A. USD 1,000,000
- B. USD 1,040,000
- C. USD 1,083,333 ✅
- D. USD 1,106,807

*Rationale:* `CPI = 480,000/520,000 = 0.9231`; `EAC = 1,000,000/0.9231 = 1,083,333`. A ignores performance; B
is the "atypical" method (a); D is the CPI×SPI method (c).

**MCQ 3.4-B `[3.4.2 · Analysis]`** A team is both over-cost and behind schedule, and believes the two will
compound on the remaining work. The most appropriate EAC method is:
- A. `EAC = AC + (BAC − EV)`
- B. `EAC = BAC / CPI`
- C. `EAC = AC + (BAC − EV)/(CPI × SPI)` ✅
- D. `EAC = BAC`

*Rationale:* The CPI×SPI method (c) reflects remaining work being dragged by *both* cost and schedule
performance. A assumes the variance was atypical; B reflects cost only; D assumes on-budget completion.

**MCQ 3.4-C `[3.4.3 · Analysis]`** Which is the strongest early-warning signal of a systemic cost problem?
- A. A single month's CPI below 1.0.
- B. A CPI that drifts down over several consecutive periods. ✅
- C. Actual cost exceeding Planned Value in one month.
- D. A positive schedule variance.

*Rationale:* A sustained downward *trend* in `CPI` is more diagnostic than any single reading. One month
below 1.0, or `AC` above `PV` in a month, can be timing; a positive `SV` is not a cost warning at all.

**MCQ 3.4-D `[3.4.2 · Application]`** `BAC` = USD 800,000; `EV` = USD 300,000; `AC` = USD 320,000. The
variance to date is judged a one-off, so remaining work will proceed at the budgeted rate. The EAC is:
- A. USD 500,000
- B. USD 800,000
- C. USD 820,000 ✅
- D. USD 853,333

*Rationale:* For an atypical variance, `EAC = AC + (BAC − EV) = 320,000 + 500,000 = 820,000`. A is the
remaining work (`ETC`) alone, not the completion forecast; B ignores the overrun already incurred; D applies
the persisting-CPI method (`800,000 / 0.9375`), contradicting the one-off judgement.

**MCQ 3.4-E `[3.4.1 · Recall]`** The Estimate to Complete (`ETC`) is best defined as:
- A. The forecast total cost of the whole job at completion.
- B. The current best estimate of the cost of the *remaining* work from now. ✅
- C. The difference between `BAC` and `EAC`.
- D. The actual cost incurred to date.

*Rationale:* `ETC` is the forward-looking cost of the work still to be done, linked to the completion
forecast by `EAC = AC + ETC`. A describes `EAC`; C is `VAC`, the variance at completion; D is `AC`.

### Self-check — KA 3.4

1. State the shared identity behind every EAC method and the three common ways `ETC` is estimated. *(`EAC =
   AC + ETC`; remaining work at budgeted rate; at current `CPI`; at `CPI × SPI`.)*
2. Why is a `CPI` trend more useful than a single-period `CPI`? *(It filters timing noise and reveals systemic
   drift early.)*

---

## Knowledge Area 3.5 — Cash-flow forecasting

*Topics: 3.5.1 profit is not cash · 3.5.2 building a project cash-flow forecast · 3.5.3 the funding
requirement and its drivers.*

### 3.5.1 Profit is not cash

**The principle.** As Domain 1 (KA 1.2.6) showed, a profitable project can still run out of cash. Cost is
committed and paid on the *supply* side (labour weekly, suppliers on their terms), while cash arrives on the
*demand* side only after work is billed, certified and paid — typically weeks or months later, and reduced by
**retention** held back until completion (Domain 7, KA 7.4). The **cash-flow forecast** models this timing so
the project can be **funded**, not merely *shown to be profitable*.

### 3.5.2 Building a project cash-flow forecast — worked

**Worked example 3.5.2 — a simple monthly cash forecast.**

1. **Setup.** A 5-month package: monthly **cost/outflow** paid in the month incurred; **billing** is one
   month in arrears (cash collected the month after the value is earned). Figures (USD 000):

   | Month | 1 | 2 | 3 | 4 | 5 | 6 |
   |---|---:|---:|---:|---:|---:|---:|
   | Cost paid out | 200 | 300 | 300 | 200 | 100 | — |
   | Cash received (1-month lag) | — | 220 | 330 | 330 | 220 | 110 |
   | Net cash in month | (200) | (80) | 30 | 130 | 120 | 110 |
   | **Cumulative cash** | **(200)** | **(280)** | **(250)** | **(120)** | **0** | **110** |

   *(Billings are cost + 10 % margin, collected a month later; total received `220+330+330+220+110 = 1,210` =
   total cost 1,100 + 110 margin. ✓)*
2. **Result.** Cumulative cash is **negative from Month 1 to Month 4**, bottoming at **(USD 280,000)** at the
   end of Month 2 — the **peak funding requirement** — then recovering to break-even in Month 5 and a positive
   **USD 110,000** (the profit, in cash) by Month 6.
3. **Interpretation.** The project is profitable (USD 110k) yet needs **USD 280,000 of funding** at its
   trough to bridge the gap between paying for work and being paid for it. A cash-flow forecast is what turns
   that from a surprise into a planned facility. Lengthening client payment terms, or higher retention, deepens
   and widens the trough; advance payments or milestone front-loading lift it.

> **Fig 3.5.1 — Project cash-flow curve and the funding trough.** *Caption:* cumulative cash over the job.
> *Underlying data:* cumulative row {−200, −280, −250, −120, 0, 110} (USD 000). *Render-ready description:*
> x-axis Months 1–6, y-axis cumulative cash from −300 to +150; a line dipping to a labelled trough
> "Peak funding requirement −280 (Month 2)" then rising through zero at Month 5 to +110; area below zero
> shaded to signal the funding need. *Animation storyboard (digital-only):* the curve draws month by month;
> the shaded funding area fills as the line goes negative and drains as it recovers, with the trough value
> pulsing at its lowest point.

**Worked example 3.5.2b — how a mobilisation advance reshapes the trough.**

1. **Setup.** Take the base forecast of worked example 3.5.2, whose cumulative-cash trough is **(USD
   280,000)** at the end of Month 2. Now suppose the client pays a **USD 150,000 mobilisation advance** at
   the start of the job (Month 0), all other flows unchanged.
2. **Formula.** An advance received before the flows begin lifts the *entire* cumulative-cash curve by its
   amount: `new cumulative cash (each month) = base cumulative cash + advance`; in particular `new trough =
   base trough + advance`.
3. **Substitution.** `New trough = (280,000) + 150,000`.
4. **Result.** New peak funding requirement = **(USD 130,000)**, still at the end of Month 2.
5. **Interpretation.** The advance more than halves the peak funding requirement (from 280,000 to 130,000)
   **without changing profit** — it is pure timing. Conversely, higher retention or longer client payment
   terms would deepen the trough (3.5.3). This is why the funding trough is a **lever** a controls/commercial
   professional actively manages — through advances, terms, retention and billing cadence — not a fixed fact.

**Worked example 3.5.2c — retention's effect on the same forecast.**

1. **Setup.** Take the base forecast of worked example 3.5.2 (receipts {220, 330, 330, 220, 110} in Months
   2–6; trough **(280,000)** at Month 2; final cumulative cash **+110,000**). The client now withholds **10 %
   retention** on every receipt, released only after the defects period — beyond this six-month window.
2. **Formula.** `Withheld each month = 10 % × receipt`; the cumulative cash curve falls by the cumulative
   amount withheld.
3. **Substitution.** Withheld = {22, 33, 33, 22, 11} (USD 000) = **121** in total. By the end of Month 2 only
   the Month-2 receipt of 220 has arrived, so cumulative withheld = 22 → trough = (280) − 22 = **(302)**. By
   Month 6, cumulative withheld = 121, so closing cash = 110 − 121 = **(11)**.
4. **Result.** The trough deepens to **(USD 302,000)** and the job ends the window **cash-negative at
   (USD 11,000)** despite its USD 110,000 profit — the profit is locked in retention until release.
5. **Interpretation.** Retention converts profit into a deferred receivable (Domain 7, KA 7.4.3b); the
   forecast must model its release explicitly or the job appears to "lose" cash it has actually earned.
   Retention and payment terms are the two heaviest levers on the funding trough (3.5.3).

### 3.5.3 The funding requirement and its drivers

**The professional angle.** The **peak funding requirement** (the deepest point of the cumulative cash curve)
is the number a project or business must arrange finance to cover. Its drivers are exactly the levers a
controls/commercial professional manages: **payment terms** (client and supplier), **retention** percentage
and release, **billing cadence** (monthly vs milestone), **advance/mobilisation payments**, and the **margin**
itself. Modelling these — and their downside scenarios — is how a project avoids the profitable-but-insolvent
trap. This KA feeds directly into the commercial cycle (Domain 7) and the O2C/P2P process cycles (Domain 11).

**AI in this KA.** AI supports cash forecasting by learning payment-behaviour patterns (which clients pay
late, seasonal effects), generating scenario ranges for the funding trough, and flagging when actual
collections drift from forecast. As ever, the professional owns the assumptions (a model that assumes
historic payment behaviour continues can badly misjudge a distressed client) and the decision. **AI proposes,
the professional disposes.**

### Key terms — KA 3.5

| Term | Meaning |
|---|---|
| **Cash-flow forecast** | A time-phased projection of cash in and out, separate from profit. |
| **Peak funding requirement** | The deepest point of cumulative cash — the finance to arrange. |
| **Retention** | Cash withheld from payments until completion/defects periods pass. |
| **Payment terms** | The lag between billing and collection (and between receipt and paying suppliers). |

### Sample MCQs — KA 3.5

**MCQ 3.5-A `[3.5.2 · Analysis]`** In the worked forecast, cumulative cash is (200), (280), (250), (120), 0,
110 (USD 000). The peak funding requirement is:
- A. USD 200,000 in Month 1
- B. USD 280,000 in Month 2 ✅
- C. USD 120,000 in Month 4
- D. USD 110,000 in Month 6

*Rationale:* The peak funding requirement is the *deepest negative* cumulative cash — (280) at the end of
Month 2. A and C are shallower points; D is the final positive balance (the profit).

**MCQ 3.5-B `[3.5.3 · Analysis]`** Which change would *deepen* a project's funding trough, all else equal?
- A. Shorter client payment terms.
- B. A mobilisation advance from the client.
- C. Longer client payment terms and higher retention. ✅
- D. Monthly rather than milestone billing.

*Rationale:* Longer terms and higher retention delay and reduce inflows, deepening the trough. Shorter terms,
an advance, and more frequent billing all *lift* the curve.

**MCQ 3.5-C `[3.5.1 · Recall]`** The main reason a profitable project can still need funding is:
- A. Depreciation.
- B. The timing gap between paying for work and being paid for it. ✅
- C. Corporation tax.
- D. Management reserve.

*Rationale:* Cash out (paying for work) precedes cash in (being paid), so cumulative cash goes negative even
on a profitable job. Depreciation is non-cash; tax and reserves are not the core timing driver.

**MCQ 3.5-D `[3.5.2 · Application]`** A package pays out costs of USD 150,000, USD 250,000 and USD 200,000
in Months 1–3, and collects receipts of USD 0, USD 165,000 and USD 275,000 in the same months. Cumulative
cash at the end of Month 3 is:
- A. (USD 600,000)
- B. (USD 235,000)
- C. (USD 160,000) ✅
- D. USD 75,000

*Rationale:* Total receipts `165,000 + 275,000 = 440,000` less total costs `600,000` gives `(160,000)` —
equivalently `(150) + (85) + 75 = (160)` in thousands, month by month. A ignores the receipts entirely; B
stops at the end of Month 2, missing the Month-3 recovery; D is Month 3's net flow alone, not the cumulative
position.

**MCQ 3.5-E `[3.5.3 · Recall]`** The peak funding requirement of a project is:
- A. Its total cost at completion.
- B. The deepest negative point of the cumulative cash curve — the finance that must be arranged. ✅
- C. The profit expected in cash at the end of the job.
- D. The retention withheld by the client over the job.

*Rationale:* The peak funding requirement is the trough of cumulative cash, sized so the project can be
funded through the gap between paying for work and being paid. A is a cost, not a funding, measure; C is the
closing positive balance; D is one *driver* of the trough, not the requirement itself.

### Self-check — KA 3.5

1. Why can cumulative project cash be negative while the job is profitable? *(Costs are paid before billings
   are collected; retention and payment terms widen the gap.)*
2. Name three levers that change the peak funding requirement. *(Payment terms, retention, billing cadence,
   advances, margin.)*

---

## Advanced topics — Domain 3

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 3.A.1 — Escalation and real versus nominal estimates

**The principle.** Every estimate is priced at a moment in time. A **base-date estimate** states cost at
the price level of a stated date — the basis recorded in the BoE (3.2.3). An **out-turn estimate**
("money of the day") adds **escalation** to the price level of the years in which the money will actually
be spent. Converting one to the other uses the **time-phased budget** (3.3): each period's phased cost is
escalated by the factor for its **year of expenditure**, so the phasing that produces the `PV` curve also
prices the escalation.

**Worked example 3.A.1 — escalating a base-date estimate to out-turn.**

1. **Setup.** A base-date estimate of **USD 10,000,000** is phased **4.0m / 3.5m / 2.5m** over Years 1–3
   (per the 3.3 baseline); escalation is **4 % per annum** from the base date.
2. **Formula.** `Out-turn = Σ (year's phased cost × (1 + e)^n)`, where `e` is the annual escalation rate
   and `n` the years from base date to expenditure.
3. **Substitution.** Year 1: `4,000,000 × 1.04 = 4,160,000`; Year 2: `3,500,000 × 1.0816 = 3,785,600`;
   Year 3: `2,500,000 × 1.124864 = 2,812,160`.
4. **Result.** Out-turn estimate **≈ USD 10,757,760** — an escalation allowance of **≈ USD 757,760** on top
   of the base-date figure.
5. **Interpretation.** The two numbers answer different questions: the base-date estimate compares options
   at a consistent price level; the out-turn estimate is what must actually be funded.

**The trap.** Comparing a *base-date* estimate to *out-turn* actuals and calling the ≈ 758,000 difference an
"overrun". It is price movement, not performance — a cousin of the false-precision pitfall of 3.2.1, where
an apparent overrun was never an overrun at all. The remedy is documentary: state the basis (real or
nominal, base date, escalation assumptions) with every estimate, exactly as class and range are stated.

### Advanced 3.A.2 — Currency and location factors in parametric estimating

**The principle.** A parametric rate (3.2.2) is only valid for the place, time and scope it was derived
from. Before a historical rate is applied elsewhere it must be adjusted for **location** — productivity
(labour skill and norms), logistics (remoteness, import content) and market conditions (how heated the
local market is) — for **currency**, converted at a stated basis rather than an incidental daily rate, and
for **time** (escalation from the rate's base year, 3.A.1).

**Worked example 3.A.2 — adjusting a library rate.**

1. **Setup.** Estimate a **5,000 m²** office using a library rate of **USD 2,000/m²** derived **two years
   ago** in a different market. Location factor for the destination market: **1.15**; escalation **5 % per
   annum** for two years.
2. **Formula.** `Estimate = parameter × rate × location factor × (1 + e)^n`.
3. **Substitution.** `2,000 × 1.15 = 2,300`; `2,300 × 1.05² = 2,300 × 1.1025 = 2,535.75`;
   `5,000 × 2,535.75 = 12,678,750`.
4. **Result.** **≈ USD 12.68m**, against **USD 10.0m** from the raw rate — the adjustments move the answer
   by roughly a quarter.
5. **Interpretation.** The adjustments are as material as the rate itself, so each factor must be evidenced
   and recorded, not asserted.

**The library needs its own BoE.** A rate library whose entries do not document their **basis** — base year,
location, and scope (what the rate includes and excludes) — silently misleads: an all-in rate applied as if
it excluded preliminaries, or a 2019 rate applied unescalated, produces a confident wrong answer. This is
the BoE discipline of 3.2.3 applied to the library itself; an undocumented rate is a Class 5 input dressed
up as Class 2 (3.2.1).

### Advanced 3.A.3 — Probabilistic cost estimating

**From a point to a distribution.** A bottom-up estimate (3.2.2) can carry a **range per line item**
(typically three-point: optimistic, most likely, pessimistic), plus stated **correlations** between items
that share drivers (market prices, weather, productivity). A **Monte Carlo** simulation samples the lines
together and returns a distribution of the total, read at **P-levels** (cross-ref 12.2.3 and 12.3.1): a
**P80** total is one the out-turn has an 80 % chance of not exceeding.

**Why summing per-item P80s does not give a P80 total.** Unless the items are perfectly correlated, they do
not all land at their bad end in the same run — the **portfolio effect**. The sum of conservative line items
is therefore *more* conservative than the portfolio.

**Worked example 3.A.3 — the portfolio effect.**

1. **Setup.** Ten independent packages, each with mean **USD 100,000** and item-level **P80 = USD 112,000**.
2. **Formula.** `Σ item P80s ≥ P80 of the total`, with equality only under perfect correlation.
3. **Substitution.** Sum of item P80s: `10 × 112,000 = 1,120,000`. Simulating the ten packages together
   returns a total-distribution **P80 ≈ 1,038,000** against a mean of 1,000,000.
4. **Result.** Line-item conservatism overstates the portfolio P80 by **≈ USD 82,000**.
5. **Interpretation.** Contingency built by stacking per-line conservatism is over-funded and unauditable;
   contingency is derived at **portfolio level** from the simulation (12.3.1), with correlations stated —
   correlation pushes the total's P80 back towards the sum of the item P80s.

**Funding level is policy, not mathematics.** Funding contingency at **P50** makes exhaustion an even-money
bet; **P80** accepts a 20 % chance of exceedance; higher levels tie up capital the portfolio needs
elsewhere — the deliberate sponsor choice this domain's case study walks through.

### Advanced 3.A.4 — Rolling-wave budgeting and undistributed budget

**The principle.** On a long programme, detailing every far-term work package at authorisation is false
precision (3.2.1). **Rolling-wave budgeting** details the near-term scope into work packages with
distributed, time-phased budgets, and holds far-term scope as **planning packages** whose budget sits as
**undistributed budget (UB)** — already inside the baseline in the 3.1.4 identity (`PMB = Σ control-account
budgets + undistributed budget + contingency reserve`). UB is budget for *defined scope not yet
decomposed*; it is not a reserve, and distributing it to control accounts changes no `BAC`.

**Worked example 3.A.4 — assembling a rolling-wave baseline.**

1. **Setup.** A four-year programme with `BAC` = **USD 20,000,000**: Year-1 scope detailed into
   control-account budgets of **USD 7,500,000**; later waves held as planning packages with **UB of USD
   11,300,000**; contingency reserve **USD 1,200,000**.
2. **Formula.** `PMB = Σ control-account budgets + UB + contingency` (3.1.4).
3. **Substitution.** `7,500,000 + 11,300,000 + 1,200,000 = 20,000,000`.
4. **Result.** **`BAC` = USD 20,000,000**, of which 11,300,000 awaits distribution as each wave is planned.
5. **Interpretation.** The whole 20,000,000 is inside the baseline and phased into `PV` — near-term at
   work-package fidelity, far-term at planning-package level — so earned value can run from day one.

**The discipline.** Budget is distributed from UB to control accounts **before the work starts** — through
baseline change control (3.1.3, and Domain 5, KA 5.4) — and never retro-fitted to actuals. A budget matched
to actuals after the fact makes variance vanish by construction (`CPI` is driven to 1) and destroys the
baseline's meaning. Applied properly, rolling wave is what keeps `PV` honest on long programmes (cross-ref
3.3 and 6.1): the near-term S-curve reflects the real resource-loaded plan rather than a straight-line
guess for years an estimator cannot yet see — the phasing trap of 3.3.2 avoided by design.

---

## Case study — Domain 3: budgeting and forecasting an offshore-wind package (energy)

### Background

A developer is building an offshore-wind farm, and this case follows one of its critical-path packages:
the **onshore substation** — the civils, structures, electrical plant and commissioning work that connect
the export cable to the grid. The package is let to a contractor whose project controls team must do, in
order, everything this domain teaches: **assemble** an authorised budget with its reserves correctly placed
(KA 3.1), **phase** that budget across the schedule into the Planned Value curve (KA 3.3), **forecast**
honestly when performance data starts to disagree with the plan (KA 3.4), keep the **cash** consequences in
view alongside the cost ones (KA 3.5), and manage the **reserves** with discipline when the forecast
overruns them (KAs 3.1.4 and 12.3). The numbers are simplified but the sequence — and the decisions at each
step — are exactly what the credential expects a professional to walk through end-to-end.

### Assembling the budget (KA 3.1)

The estimate is bottom-up (3.2.2), built from a defined WBS and recorded in a basis of estimate (3.2.3).
The **control-account budgets** total **USD 14,000,000**. The project's quantified risk register (Domain 12)
is run through a Monte Carlo simulation, and the sponsor's policy is to fund contingency at the **P80**
confidence level: a **contingency reserve of USD 1,200,000**. Because contingency covers *identified* risks,
it sits **inside** the cost baseline, under the project manager's control. The sponsor separately holds a
**management reserve of USD 800,000** for unknown-unknowns — **outside** the baseline, released only through
change control (Domain 5, KA 5.4).

| Component | USD | Position |
|---|---:|---|
| Control-account budgets | 14,000,000 | Inside the baseline |
| Contingency reserve (Monte Carlo P80) | 1,200,000 | Inside the baseline |
| **Cost baseline → `BAC`** | **15,200,000** | The earned-value baseline |
| Management reserve | 800,000 | Outside the baseline |
| **Total authorised budget** | **16,000,000** | |

The **P80** deserves a sentence, because it is where Domain 3 meets Domain 12. Funding contingency at P80
means the simulation says there is an 80 % chance the identified risks will cost no more than the funded
amount — and, by the same token, a 20 % chance they will cost more. The sponsor chose that confidence level
deliberately: funding to P50 would make contingency exhaustion an even-money bet, while funding to P95 would
tie up capital the portfolio needs elsewhere. The reserve is therefore *expected* to be drawn on, and *may*
legitimately be exceeded — which is exactly the situation this case will reach, and why the escalation route
in the final section exists by design rather than as an admission of failure.

In waterfall terms (Fig 3.1.1's pattern): control accounts 14,000,000 + contingency 1,200,000 = **`BAC`
15,200,000** *inside* the baseline; + management reserve 800,000 = **total budget 16,000,000**, with the
final step *outside* the baseline. Every `CPI`, `EAC` and `VAC` that follows is measured against the
15,200,000 — never the 16,000,000. Blurring that boundary is the classic error of 3.1.4: it either hides
funds the project is entitled to draw, or corrupts every variance reported from here on.

### Phasing the baseline (KA 3.3)

The 15,200,000 is then spread across the 12-month schedule to an S-curve: a slow start while the site
mobilises and piling begins, a steep middle through peak civils and electrical installation, and a taper
through testing and commissioning. The phasing came **from the resource-loaded schedule, not a straight
line** — so any early variance against it will mean performance, not a phasing artefact (the trap of MCQ
3.3-B). The monthly plan (USD 000):

| Month | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| Monthly PV | 400 | 700 | 1,100 | 1,500 | 1,800 | 1,800 | 1,900 | 1,750 | 1,450 | 1,150 | 900 | 750 |
| Cumulative PV | 400 | 1,100 | 2,200 | 3,700 | 5,500 | 7,300 | 9,200 | 10,950 | 12,400 | 13,550 | 14,450 | 15,200 |

Monthly values sum to **15,200** (= `BAC`). ✓ By the end of **Month 6** the plan says `PV` =
**USD 7,300,000** should have been accomplished. Because the phasing is honest, the Month 6 comparison that
follows is meaningful: had the baseline been straight-lined at 15,200/12 ≈ 1,267 per month, the plan would
have claimed `PV` = 7,600,000 at Month 6 and part of the apparent shortfall would have been an artefact of
lazy phasing rather than of the contractor's performance. That cumulative curve is the canvas on which the
next section draws `EV` and `AC`.

### The forecast at Month 6 (KA 3.4)

At the Month 6 data date, progress measurement (Domain 4) and the cost ledger report trouble: piling
productivity is below tender assumptions and electrical materials prices have risen.

1. **Setup.** `BAC` = **USD 15,200,000**; at Month 6, `PV` = **7,300,000**, `EV` = **USD 7,000,000**
   earned, `AC` = **USD 7,900,000** spent.
2. **Formula.** `CPI = EV / AC`; `EAC = BAC / CPI` (method (b) — the variance is judged typical);
   `VAC = BAC − EAC`.
3. **Substitution.** `CPI = 7,000,000 / 7,900,000 = 0.8861 ≈ 0.89`; `EAC = 15,200,000 / 0.8861 ≈
   17,154,000`; `VAC = 15,200,000 − 17,154,000 = (1,954,000)`.
4. **Result.** **`EAC` ≈ USD 17,154,000** — a projected overrun of **USD 1,954,000** against the baseline.
5. **Interpretation.** Why the persisting-`CPI` method? Because of the **trend**: `CPI` has read **0.94,
   0.91, 0.89** over the last three months (3.4.3). A single soft month could be timing noise; three
   consecutive readings moving the same way point to a **systemic** driver — ground conditions and market
   prices that will keep applying to the remaining work — so assuming the variance is atypical (method (a))
   would be optimism, not analysis. The professional chooses the assumption that matches the *cause* of the
   variance and defends it (Domain 6, KA 6.3.3). The forecast's value is that it lands at Month 6, while
   there is still time to act — not at Month 12, as a post-mortem.

### Cash and funding (KA 3.5)

The same Month 6 review re-runs the cash-flow forecast, because cost and cash are different questions
(3.5.1). The contract pays on **45-day terms** with **5 % retention** withheld until the defects period:
the contractor pays labour weekly and suppliers monthly, but collects certified billing a month and a half
later, and 5 % of every certificate later still. The cash model shows a **peak funding requirement of
USD 3,400,000 at Month 7** — the deepest point of the cumulative cash curve, sitting just after the spend
peak of the S-curve, where outflows have crested but the matching receipts are still in transit. Crucially,
the cost overrun **worsens the trough**: every month of `CPI` at 0.89 means more cash paid out for the same
certified billing, so the funding requirement deepens even though the client-side flows are unchanged. The
treasury facility is therefore **re-confirmed alongside the `EAC`** — the controls team reports the revised
cost forecast and the revised funding trough in the same review, because a project that arranges finance for
the baseline trough and then overruns its costs discovers the profitable-but-unfunded trap of 3.5.3 in the
worst possible month. The levers of 3.5.3 are also on the table: the commercial team examines whether
milestone billing can be brought forward or supplier terms extended to lift the curve, while the downside
scenario — the client slipping certification by a month while `CPI` stays at 0.89 — is modelled explicitly
so the facility headroom covers it. Cost and cash forecasts move together, not separately.

### The reserves decision (KAs 3.1.4, 12.3)

Now the budgeting discipline of KA 3.1 is tested. The projected overrun is **USD 1,954,000**. Of the
original 1,200,000 contingency, **USD 350,000** has already been drawn against register risks that
materialised earlier in the job, leaving `1,200,000 − 350,000 = ` **USD 850,000** available. The `VAC`
therefore **exceeds the remaining contingency**, and the response comes in two disciplined steps:

1. **Draw the remaining contingency — legitimately.** The **850,000** is drawn against the materialised
   register risks it exists to cover — the ground-conditions and materials-price risks were both *identified*
   risks, quantified in the Monte Carlo model that set the P80 in the first place. This is contingency doing
   its job: drawn down inside the baseline, under the project manager's authority, logged against specific
   register entries (Domain 12, KA 12.3). Uncovered balance: `1,954,000 − 850,000 = ` **USD 1,104,000**.
2. **Take the uncovered balance to the sponsor — visibly.** The **1,104,000** cannot be absorbed silently.
   It goes to the sponsor as a **re-baselining case** through change control (Domain 5, KA 5.4): a request to
   release management reserve of up to **800,000** against genuinely unforeseen severity, plus a
   **scope/value-engineering review** to close the remainder of `1,104,000 − 800,000 = ` **≈ USD 304,000** —
   respecified equipment, resequenced commissioning, or descoped non-essential work, each a visible, owned
   decision in the pattern of worked example 3.1.2.

The discipline matters more than the arithmetic. **Contingency draw-down is normal** — it was funded at P80
precisely because risks were expected to materialise. **Exceeding it is a re-baselining event** — a formal,
visible change that resets the `BAC` so future variances stay meaningful, never a silent overspend smeared
across the remaining control accounts (Domain 12, KA 12.3.3). A team that quietly books the overrun without
the sponsor conversation has not saved the project embarrassment; it has destroyed the baseline's integrity
and everyone's ability to trust the next forecast.

### What the credential expects

This case is the Domain 3 chain in one pass. The candidate should be able to: assemble the budget with
**contingency inside and management reserve outside** the baseline, and state why `CPI` is measured against
15,200,000, not 16,000,000 (3.1.4); explain that the phased baseline *is* `PV`, and that its S-curve shape
must come from the schedule (3.3); compute `CPI`, `EAC` and `VAC` from a status, and **defend the choice of
EAC method from the trend**, not pick a formula mechanically (3.4.2–3.4.3, Domain 6); read the **peak
funding requirement** and explain why a cost overrun deepens it (3.5); and run the two-step reserves
response — draw contingency against the register, escalate the excess as a visible re-baselining case
(3.1.4, 12.3). On the AI dimension: predictive `EAC` models and cash-collection models (Domain 13,
KA 13.5.3) would likely have flagged both the `CPI` drift and the deepening trough a month or two earlier —
but the professional still owns the persisting-variance assumption, the reserve arithmetic and the sponsor
conversation. **AI proposes, the professional disposes.**

---

## Executive perspective — Domain 3

**What the executive must hold onto.** An estimate is a **range that narrows as definition matures** (3.2.1);
a single-point figure quoted without its class and accuracy range is a risk decision in disguise, and a
budget approved without the matching scope and risk decisions is a future overrun with a signature on it.
Contingency sits **inside** the baseline for identified risk; management reserve sits **outside** it, and its
release is the sponsor's decision, not the project's (3.1.4). And the **funding trough is as
decision-relevant as the profit line**: a profitable project can still be unfunded, and the peak funding
requirement (3.5.3) is a number the board must arrange, not merely note.

**Six questions to ask from the chair.**

1. What class is this estimate, what accuracy range comes with it, and where is the basis of estimate?
2. What confidence level set the contingency, and which register risks is it funded to cover?
3. Does the phasing of this baseline come from the schedule, or was the S-curve spread by formula (3.3)?
4. Which EAC method is this forecast built on, and why is that assumption right for the cause of the
   variance?
5. How much contingency has been drawn, against which risks — and at what point does the shortfall come to
   us as a re-baselining case rather than a silent overspend?
6. What is the peak funding requirement, in which month does it bite, and does the facility headroom cover
   the downside scenario?

**The traps at board level.**

- **Treating a Class 5 estimate as a commitment.** An early concept figure carries a range of roughly −30 %
  to +50 %; quoted as a promise, it manufactures "overruns" that were never overruns — merely maturity being
  ignored.
- **Reserves misread in both directions.** Contingency draw-down is normal — it was funded because risks
  were expected to materialise — while management reserve is not the project's cushion; alarm at legitimate
  draws and comfort at silent ones are equally wrong.
- **The forecast that never moves.** An EAC held at budget month after month, then corrected at the end, was
  never a forecast; boards should be more suspicious of a static EAC than of an honest deterioration.
- **Reading profit and ignoring cash.** Approving the cost forecast without re-confirming the funding trough
  invites the profitable-but-unfunded trap — cost and cash forecasts must move together.

**What good looks like.** Every estimate arrives with its class, range and a basis of estimate that states
scope, method, rates, assumptions and exclusions (3.2.3). The baseline is time-phased from the schedule,
change-controlled, and never silently reshaped; contingency draws are logged against specific register
entries, and an exhausted contingency triggers a visible sponsor conversation, not a smeared overspend. The
forecast is re-produced every period as a rolling forecast, with the EAC method defended from the trend
rather than picked mechanically (3.4), and the cash-flow forecast and peak funding requirement are reported
in the same review as the cost position.

---

## Calculation exercises — Domain 3

Work each exercise before reading its solution; every step uses only this domain's methods.

**Exercise 3.1** — A pipeline project's control-account budgets total **USD 8,400,000**. A Monte Carlo run
over the risk register supports a **contingency reserve of USD 600,000**, and the sponsor separately holds a
**management reserve of USD 450,000**. State the `BAC` and the total authorised budget. At Month 7 an
unforeseen regulatory requirement is approved through change control and the sponsor releases **USD 200,000**
of management reserve into the baseline. State the new `BAC`, the remaining management reserve, and the new
total authorised budget.

**Solution 3.1.**
1. `BAC = control-account budgets + contingency = 8,400,000 + 600,000 = ` **USD 9,000,000**.
2. `Total budget = BAC + management reserve = 9,000,000 + 450,000 = ` **USD 9,450,000**.
3. The release is a baseline *change*, not a variance: new `BAC = 9,000,000 + 200,000 = ` **USD 9,200,000**.
4. Remaining management reserve `= 450,000 − 200,000 = ` **USD 250,000**.
5. New total budget `= 9,200,000 + 250,000 = ` **USD 9,450,000** — unchanged. The release moves funds from
   outside to inside the baseline; it creates no new money. Every `CPI`, `EAC` and `VAC` from Month 7 onward
   is measured against 9,200,000, never against 9,450,000 (3.1.4).

**Exercise 3.2** — Estimate a warehouse of **8,000 m²** two ways. A parametric rate of **USD 1,500/m²** is
available from recent regional data. A similar completed warehouse of **6,400 m²** cost **USD 10,240,000**.
Compute the parametric and the analogous estimates. The team adopts the parametric figure and, given the
maturity of definition, classifies it as **Class 3 (−15 % / +20 %)**. State the accuracy range around the
adopted figure.

**Solution 3.2.**
1. Parametric: `estimate = parameter × rate = 8,000 × 1,500 = ` **USD 12,000,000**.
2. Analogous: `estimate = past cost × (this driver / past driver) = 10,240,000 × (8,000 / 6,400) =
   10,240,000 × 1.25 = ` **USD 12,800,000**.
3. Range on the adopted parametric figure: low `= 12,000,000 × 0.85 = ` **USD 10,200,000**; high
   `= 12,000,000 × 1.20 = ` **USD 14,400,000**.
4. Reported result: **USD 12,000,000, Class 3, −15 % / +20 % → USD 10,200,000 to USD 14,400,000**. The
   analogous figure of 12,800,000 sits inside that band — convergence that increases confidence (3.2.2) —
   but the number is quoted *with its class and range*, never as a bare point (3.2.1).

**Exercise 3.3** — A control account with `BAC` = **USD 1,200,000** runs over six months to an S-shaped
monthly plan (USD 000): **100, 200, 300, 300, 200, 100**. Verify the phasing against the `BAC`, build the
cumulative `PV` curve, read `PV` at a data date at the end of **Month 4**, state what percentage of the
`BAC` is planned to be accomplished by then, and recover the planned spend for Months 3 and 4 together from
the cumulative curve.

**Solution 3.3.**
1. Check: `100 + 200 + 300 + 300 + 200 + 100 = 1,200` (USD 000) = `BAC`. ✓
2. Cumulative `PV` (USD 000): **100, 300, 600, 900, 1,100, 1,200**.
3. At the end of Month 4, `PV` = **USD 900,000**.
4. Share of `BAC`: `900,000 / 1,200,000 = ` **75 %** planned to be accomplished.
5. Months 3–4 together, by differencing the cumulative curve: `900 − 300 = ` **USD 600,000**. The cumulative
   row *is* the Planned Value S-curve — the baseline `EV` and `AC` will be measured against (3.3.3).

**Exercise 3.4** — A project has `BAC` = **USD 2,400,000**. At the data date the status is `PV` =
**1,200,000**, `EV` = **960,000**, `AC` = **1,000,000**. Variance analysis traces the overrun to a systemic
productivity shortfall expected to persist. Compute `CPI` and `SPI`, then all three formula EACs, choose the
method matching the cause, and state `VAC` on the chosen method.

**Solution 3.4.**
1. `CPI = EV / AC = 960,000 / 1,000,000 = ` **0.96**; `SPI = EV / PV = 960,000 / 1,200,000 = ` **0.80**.
2. (a) Atypical: `EAC = AC + (BAC − EV) = 1,000,000 + 1,440,000 = ` **USD 2,440,000**.
3. (b) Typical: `EAC = BAC / CPI = 2,400,000 / 0.96 = ` **USD 2,500,000**.
4. (c) Compounding: `EAC = AC + (BAC − EV) / (CPI × SPI) = 1,000,000 + 1,440,000 / 0.768 = 1,000,000 +
   1,875,000 = ` **USD 2,875,000**.
5. A persisting cost cause fits method (b): `EAC` = **USD 2,500,000**; `VAC = BAC − EAC = 2,400,000 −
   2,500,000 = ` **(USD 100,000)** — a projected overrun, chosen for the *cause*, not by habit (3.4.2).

**Exercise 3.5** — A five-month package pays out costs in the month incurred (USD 000): **100, 200, 250,
150, 100**. It bills each month at **cost + 20 % margin**, collected **one month in arrears**, and the
client withholds **10 % retention** on every receipt, released only after this six-month window. Build the
monthly net and cumulative cash positions for Months 1–6, identify the peak funding requirement, state the
closing cash position, and reconcile it with the margin.

**Solution 3.5.**
1. Billings (cost × 1.20, USD 000): 120, 240, 300, 180, 120 = **960** in total against costs of **800**.
2. Receipts land a month later, net of 10 % retention: gross {120, 240, 300, 180, 120} in Months 2–6, less
   retention {12, 24, 30, 18, 12} → net **{108, 216, 270, 162, 108}**; total withheld = **96**.
3. Monthly cash (USD 000):

   | Month | 1 | 2 | 3 | 4 | 5 | 6 |
   |---|---:|---:|---:|---:|---:|---:|
   | Net cash in month | (100) | (92) | (34) | 120 | 62 | 108 |
   | **Cumulative cash** | **(100)** | **(192)** | **(226)** | **(106)** | **(44)** | **64** |

4. Peak funding requirement = **(USD 226,000)** at the end of Month 3.
5. Closing position = **USD 64,000**. Reconciliation: margin `= 960 − 800 = 160`; cash 64 + retention
   outstanding 96 `= 160`. ✓ The other 96,000 of profit is locked in retention until release (3.5.2c).

---

## Domain 3 summary

Budgeting turns a plan into an authorised, time-phased **cost baseline** whose total is the `BAC`, with two
distinct reserves — contingency (identified risk, inside the baseline) and management reserve (unforeseen,
outside it). Estimates carry a **class and accuracy range** that tightens as scope matures, are built
analogously, parametrically or bottom-up, and are made defensible by a **basis of estimate**. Phasing the
`BAC` across the schedule produces the **Planned Value S-curve** that earned value measures against.
Forecasting projects the **`EAC`** honestly and early — by methods that encode different assumptions about
whether today's variance is atypical, typical, or compounding — and re-produces it each period as a rolling
forecast reading the trend. Finally, **cash-flow forecasting** models the timing gap that makes a profitable
project still need funding, and sizes the **peak funding requirement** the business must arrange.

**Cross-references.** Profit vs cash → 1.2.6; cost coding/control accounts → 1.5; revenue vs billing (the
inflow side) → 2.2.7, 7.4–7.5; performance measurement and variance → Domain 4; the full EVM/EAC treatment →
Domain 6; contract types, retention and payment terms → Domain 7; risk and contingency derivation → Domain 12;
predictive forecasting → Domain 13, KA 13.5.

*Domain 3 is a first authored draft pending SME technical review before it feeds the exam blueprint.*
