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

### Self-check — KA 3.5

1. Why can cumulative project cash be negative while the job is profitable? *(Costs are paid before billings
   are collected; retention and payment terms widen the gap.)*
2. Name three levers that change the peak funding requirement. *(Payment terms, retention, billing cadence,
   advances, margin.)*

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
