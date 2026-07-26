# Domain 7 — Cost, Resources and Commercial Awareness *(quantitative flagship)*

> **Group:** Delivering the work (Domain 7 of 6 in Part Two — the cost counterpart to Domain 6's
> schedule). **Target:** ~78 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain is this book's home of the earned-value symbols —
> `PV`, `EV`, `AC`, `BAC`, `CV`, `SV`, `CPI`, `SPI`, `EAC`, `ETC`, `VAC`, `TCPI` — carried
> unchanged from the PCI family master table. Note the family notation-clash rule: `PV` here is
> **Planned Value**; present value is written `PV(x)` or in words (PFL-AI, Domain 3).
> British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

A leader who can defend a date but not a number is half-equipped. Domain 6 built the schedule;
this domain builds the money that pays for it and the commercial arrangements that bind other
people to deliver it. It starts with estimating and budgeting — how a credible number is
constructed and what its accuracy claim actually means (KA 7.1); establishes the cost baseline
and the forecasting that keeps it honest (KA 7.2); builds **earned value** in full, because it is
the only technique that answers cost and schedule performance in one integrated language
(KA 7.3); and closes with the commercial awareness a leader cannot delegate — resource
economics, procurement strategy, contract models and cash flow (KA 7.4). Risk quantification
deepens in Domain 8; contracts and supply networks get their own treatment in Domain 10. What
belongs here is the leader's own numeracy: enough to know when a forecast is arithmetic and when
it is hope.

**Learning objectives.** After this domain a candidate can: select an estimating method
appropriate to the available definition and state its accuracy class; build a three-point cost
estimate; assemble a cost baseline separating contingency from management reserve; compute and
interpret the full earned-value set (`CV`, `SV`, `CPI`, `SPI`); forecast with each `EAC` method
and choose the one matching the variance's cause; compute `VAC` and `TCPI` and explain what
`TCPI` demands of the remaining work; read resource economics through blended rates; distinguish
the main contract models by who carries cost risk; compute a cost-incentive fee and the point of
total assumption; explain why cash flow and profit differ on a project; and govern AI-produced
cost forecasts under the family verification rule.

**The master worked project.** Project Auriga continues from Domain 6 — the 25-week
control-systems upgrade for a regional utility — now with money attached. Its approved cost
baseline is **`BAC` = USD 4,000,000**. At the **data date, end of week 13**, the baseline says
**`PV` = USD 2,080,000**; measurement gives **`EV` = USD 1,920,000** and **`AC` =
USD 2,120,000**. Every calculation in KA 7.2–7.3 uses these four numbers.

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

**Accuracy classes.** Mature cost practice publishes an estimate's *class* alongside its number
— a range that narrows as definition matures (the AACE Total Cost Management framework's
class-5-to-class-1 progression is the reference treatment, described here in this book's own
words). The professional discipline is simple and widely broken: **an estimate is never a single
number without a range and a class**. "USD 4 million" is not an estimate; "USD 4 million,
−15 %/+30 %, at a class consistent with 30 % design completion" is.

**Common pitfall — precision mistaken for accuracy.** A bottom-up estimate summing 400 packages
to `USD 4,183,662` looks authoritative and is no more accurate than its worst assumption. Leaders
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
5. **Interpretation.** The most-likely figure is 750,000, but the *expected* cost is 780,000 —
   the right-skewed tail adds USD 30,000 before anything goes wrong. Budgeting at the mode
   systematically under-funds a portfolio of such packages; the difference between mode and mean,
   summed across a project, is a large part of what contingency exists to cover (7.1.3, and
   Domain 8's quantification).

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
curve** (the S-curve) — and that curve *is* Planned Value (KA 7.3). A budget with no phasing
cannot be measured against; the schedule (Domain 6) is therefore a precondition of cost control,
not a parallel activity.

### AI in this KA

Estimating is where AI assistance is most seductive and most in need of provenance. A model can
produce a plausible parametric rate instantly, and it cannot tell you whether that rate came from
comparable work, a different market, or nowhere at all. The governed workflow: **AI proposes** a
method, a rate and a range; the estimator supplies the *calibration evidence* (which projects,
which years, which escalation basis — the source discipline of the shared registry); the range
and class are stated; and a named human owns the number. **AI proposes; the professional verifies,
decides and remains accountable.** An estimate whose basis cannot be produced on request is not an
estimate.

### Key terms — KA 7.1

| Term | Meaning |
|---|---|
| **Analogous / parametric / bottom-up** | Scale an analogue · rate × parameter · sum the packages. |
| **Accuracy class** | The definition-linked range accompanying an estimate; never optional. |
| **Control account** | The WBS × organisation point where scope, budget and actuals integrate. |
| **Contingency reserve** | Inside the baseline; funds identified risks; PM-controlled. |
| **Management reserve** | Outside the baseline; unknown-unknowns; sponsor-controlled. |
| **`BAC`** | Budget at Completion — the time-phased cost baseline's total. |

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

*Rationale:* Management reserve sits *outside* the baseline (so D is wrong and A is technically
true but misleading), and its release is a sponsor-level signal, not a project-level convenience.
It is not yet a breach (C) — it is the early warning that precedes one.

**MCQ 7.1-C `[7.1.1 · Recall]`** Which statement about a bottom-up estimate summing to
USD 4,183,662 is soundest?
- A. its precision indicates high accuracy
- B. its accuracy is bounded by its assumptions and definition maturity, and it must still carry a range and class ✅
- C. rounding it would reduce its accuracy
- D. bottom-up estimates do not need accuracy classes

*Rationale:* Precision (digits) and accuracy (closeness to outturn) are independent; definition
maturity governs the latter. Rounding changes no information (C), and every estimate carries a
class (D).

### Self-check — KA 7.1

1. *Where do contingency and management reserve sit, and who spends each?* — Contingency inside
   the baseline, PM-controlled; management reserve outside it, sponsor-controlled.
2. *Why is the time-phased baseline a precondition for cost control?* — Because the phased
   cumulative curve is Planned Value; without it there is nothing to measure performance against.
3. *What two things must accompany every estimate?* — A range and an accuracy class tied to
   definition maturity.

---

## Knowledge Area 7.2 — The cost baseline, actuals and forecasting

*Topics: 7.2.1 measuring actual cost · 7.2.2 the forecasting question · 7.2.3 baseline integrity.*

### 7.2.1 Measuring actual cost

**The principle.** `AC` must cover the same work as `EV`, in the same period, or every index
built from them is fiction. Three mechanics decide whether it does:

- **Accruals.** Work received but not yet invoiced belongs in this period's `AC`. Omit accruals
  and cost performance looks excellent until the invoices land — the commonest cause of a
  "sudden" overrun that was months old.
- **Commitment vs actual.** A purchase order is a *commitment*, not a cost. Both matter: `AC`
  drives performance, commitments drive the funding forecast. Confusing them double-counts or
  hides money.
- **Open-commitment hygiene.** Stale purchase orders left open inflate the forecast; cleansing
  them is a standing month-end task.

### 7.2.2 The forecasting question

A forecast answers one question — *what will this cost when it is done?* — and the honest answer
depends entirely on **why** the current variance exists. That judgment is the leader's; the
arithmetic is KA 7.3.3's `EAC` family. The rule to internalise now: **a forecast is a statement
about the remaining work, not an extrapolation ritual.** A variance caused by a closed,
non-recurring event says nothing about what is left; a variance caused by a systemic productivity
shortfall says everything.

### 7.2.3 Baseline integrity

The baseline's authority rests on its stability. Four standing rules: budgets of completed or
open work packages are never retrospectively adjusted (except to correct an authorised error);
re-baselining happens only through change control, with an audit trail (Domain 4, KA 4.3);
transfers between control accounts are logged, not silent; and the baseline never absorbs a
variance by moving budget to where the money went. A baseline edited to match reality has stopped
measuring anything — the cost analogue of Domain 6's pinned milestone.

### Key terms — KA 7.2

| Term | Meaning |
|---|---|
| **`AC`** | Actual Cost of the work performed, including period accruals. |
| **Accrual** | Cost of work received but not yet invoiced, recognised in the period. |
| **Commitment** | A contractual obligation (e.g. a PO); a funding fact, not yet a cost. |
| **Re-baselining** | Domain 4's instrument (KA 4.3.3) seen from earned value: it resets the `PV` curve every index is measured against, which is exactly why it cannot be used to retire an adverse variance. |

### Sample MCQs — KA 7.2

**MCQ 7.2-A `[7.2.1 · Analysis]`** A project's `CPI` has read 1.02 for four months; then one
month it drops to 0.91 with no change in productivity. The likeliest explanation is:
- A. genuine sudden inefficiency
- B. accruals were not being recognised, so earlier `AC` understated cost and this month absorbed the catch-up ✅
- C. the baseline was too generous
- D. `EV` was over-claimed this month

*Rationale:* A step change in `CPI` without an operational change points at measurement, and
missing accruals are the classic cause — earlier periods flattered, one period punished. D would
raise, not lower, the earlier readings' credibility, and C would show as a stable, not stepped,
pattern.

**MCQ 7.2-B `[7.2.3 · Recall]`** Which action preserves baseline integrity?
- A. moving budget from an underspent control account to cover an overspend, without record
- B. re-baselining through change control with an audit trail when scope genuinely changes ✅
- C. adjusting a completed package's budget to match its actual cost
- D. reducing remaining budgets so the total still equals `BAC`

*Rationale:* Only governed change preserves the measurement. A is a silent transfer, C is
retrospective adjustment, D is the classic "make the numbers add up" manoeuvre — each destroys
comparability.

### Self-check — KA 7.2

1. *Why must `AC` include accruals?* — So it covers the same work as `EV` in the same period;
   otherwise every index is misstated.
2. *Is a purchase order a cost?* — No: a commitment. It informs the funding forecast, not
   performance.
3. *What single question does a forecast answer?* — What the remaining work will cost, given why
   the variance exists.

---

## Knowledge Area 7.3 — Earned value: measurement, variances and forecasting

*Topics: 7.3.1 the three measures · 7.3.2 variances and indices · 7.3.3 the `EAC` family ·
7.3.4 `VAC` and `TCPI`.*

### 7.3.1 The three measures

**Definitions.** All three are expressed in the same budget currency so they are directly
comparable:

- **Planned Value (`PV`)** — the budgeted cost of the work *scheduled* by the data date: the
  time-phased baseline of KA 7.1.3.
- **Earned Value (`EV`)** — the budgeted cost of the work *actually performed*: physical progress
  **valued at the budget rate**, never at what it cost.
- **Actual Cost (`AC`)** — the cost actually incurred for that work, accruals included (KA 7.2.1).

**The single most important conceptual point:** `EV` is measured *at budget*. That is what lets
`EV` be compared with `PV` (both at budget → schedule progress) and with `AC` (budget vs actual
for the same work → cost efficiency). Confusing "value earned" with "cost incurred" collapses the
method.

**Earning rules.** How `EV` is claimed decides how much it can be gamed: **0/100** (nothing until
complete — objective, good for short packages); **50/50**; **percent complete** (needs an
objective basis); **units completed** (best where output is countable); **weighted milestones**;
and **level of effort**, where `EV` is set equal to `PV` by the calendar, so it can *never* show a
schedule variance. Level of effort dilutes whatever it is mixed into — formal practice segregates
it and caps its share of a control account.

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
5. **Interpretation.** Behind and over: for every dollar spent the project is producing 91 cents
   of budgeted work, and it has delivered 92 % of what it planned to by now. The percent-complete
   versus percent-spent pair (48 vs 53) is the same story in the form a board absorbs fastest.
   Note this is the *cost* view of the very slippage Domain 6's case study recovered on the
   schedule side — the two domains are describing one project.

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
5. **Interpretation.** The same four measured numbers support forecasts spanning
   **USD 408,056** — over 10 % of `BAC`. The spread is not uncertainty in the arithmetic; it is
   the *assumption* about the remaining work, made explicit. Choosing among them is the leader's
   judgment, and it must be stated: if Auriga's overrun was the contaminated-ground remediation
   of Domain 6 — a discrete, now-closed event — method (a) is right and the others over-forecast.
   If it reflects a systemic productivity shortfall that will persist, (b). If schedule pressure
   is now driving overtime and expediting, (c). A forecast presented without its assumption named
   is not a forecast; it is a number.

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
5. **Interpretation.** To finish at budget, the remaining 52 % of work must run at **1.11** — an
   11 % efficiency *gain* by a team currently achieving 0.91. That is a 22-point swing, and the
   honest question in the room is what specifically would deliver it. Meanwhile `TCPI` to the
   (b) forecast is 0.91 — exactly today's `CPI`, which is the arithmetic identity worth
   internalising: **forecasting with `BAC/CPI` is precisely the assumption that nothing changes.**
   `TCPI` is the reality check on every recovery promise: a required index far above demonstrated
   performance is a plan that needs a mechanism, not encouragement.

> **Fig 7.3.2 — The `EAC` fan and what `TCPI` demands.** Two-panel figure. Left: Auriga's
> forecast fan from the week-13 data date — three dotted continuations to 4,200,000 (budgeted
> rate), 4,416,667 (`BAC/CPI`) and 4,608,056 (`CPI×SPI`), with `BAC` 4,000,000 as a horizontal
> reference. Right: a bar pair — demonstrated `CPI` 0.91 against required `TCPI` 1.11 to recover
> to `BAC` — with the 0.20 gap annotated "the gap a recovery plan must actually close".
> Source: PCI original. Alt text: a fan of three cost forecasts rising above the budget line,
> beside bars contrasting achieved cost efficiency with the higher efficiency needed to recover.

### AI in this KA

Earned value is arithmetic on four numbers, so machine computation is safe and instant
verification is cheap — which makes unverified AI output indefensible here rather than merely
risky. The deterministic checks: `EV/BAC` must equal the claimed percent complete; `CV = EV − AC`
and `CPI = EV/AC` recomputed independently; `TCPI` to `BAC/CPI` must equal `CPI` (7.3.4's
identity); and every `EAC` must state its assumption. Where AI forecasts from trend data, the
calibration rule of Domain 6 (KA 6.4) applies unchanged: show the record of past forecasts
against outturn before the number enters a board pack, and name the human who owns it.

### Key terms — KA 7.3

| Term | Meaning |
|---|---|
| **`PV` `EV` `AC`** | Budgeted cost of work scheduled · performed · the cost actually incurred. |
| **`CV` `SV`** | `EV − AC` · `EV − PV`, in currency. |
| **`CPI` `SPI`** | `EV/AC` · `EV/PV`, as ratios. |
| **`EAC` `ETC`** | Estimate at / to complete; `EAC = AC + ETC`. |
| **`VAC`** | `BAC − EAC` — forecast variance at completion. |
| **`TCPI`** | Efficiency the remaining work must achieve to hit a stated target. |
| **Level of effort** | Earning by calendar (`EV ≡ PV`); can never show schedule variance. |

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

*Rationale:* A discrete, closed cause makes the variance **atypical**, so remaining work is
forecast at the budgeted rate. B assumes the inefficiency persists and C that it compounds with
schedule pressure — both contradict the stated cause. D ignores money already spent above budget.

**MCQ 7.3-C `[7.3.4 · Application]`** With `BAC` 4,000,000, `EV` 1,920,000 and `AC` 2,120,000,
the `TCPI` required to complete at `BAC` is:
- A. 0.91
- B. 1.00
- C. 1.11 ✅
- D. 1.21

*Rationale:* `(4,000,000 − 1,920,000)/(4,000,000 − 2,120,000) = 2,080,000/1,880,000 = 1.11`.
A is the demonstrated `CPI`; B assumes recovery needs only par performance; D uses `PV` in place
of `AC` in the denominator.

**MCQ 7.3-D `[7.3.1 · Analysis]`** A control account is 70 % level-of-effort by budget. Its
reported `SPI` of 1.00 most likely means:
- A. the account is exactly on schedule
- B. little about schedule: level of effort earns by the calendar, so `EV ≡ PV` for most of the account regardless of progress ✅
- C. the discrete work is ahead, offsetting a delay
- D. the earning rules were misapplied

*Rationale:* LOE sets `EV` equal to `PV` by construction, so a heavily-LOE account reads 1.00
whatever happens — which is why practice segregates and caps it. C invents an offset the data
cannot show; the rules may have been applied entirely correctly (D), and that is the problem.

### Self-check — KA 7.3

1. *Why is `EV` measured at budget, not at cost?* — So it is comparable with `PV` (schedule) and
   with `AC` (efficiency); at cost it would collapse into `AC`.
2. *What does `TCPI` = 1.11 against `CPI` = 0.91 tell a leader?* — Recovery to budget requires a
   22-point efficiency swing; the plan needs a named mechanism, not optimism.
3. *State the identity linking `TCPI` and `BAC/CPI`.* — `TCPI` to an `EAC` of `BAC/CPI` equals
   the current `CPI`: that forecast *is* the assumption that nothing changes.

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
5. **Interpretation.** The blended rate makes options commensurable: a week of schedule
   compression staffed from this pool costs a computable amount, and *shifting the mix* changes
   the price as much as changing the headcount — adding five specialists moves the blend more than
   adding five technicians. This is the number that turns Domain 6's "crash it" instruction into
   an expected-value decision.

### 7.4.2 Contract models and cost risk

Every contract model is an answer to one question: **who carries cost risk?**

| Model | Buyer pays | Cost risk sits with | Suits |
|---|---|---|---|
| **Firm fixed price (FFP)** | An agreed price, whatever it costs | **Seller** | Well-defined scope; priced-in risk premium |
| **Fixed price incentive (FPIF)** | Target cost/fee with a share ratio, capped by a ceiling | **Shared, then seller above the ceiling** | Definable scope with real uncertainty |
| **Cost plus fixed fee (CPFF)** | Allowable cost + a fixed fee | **Buyer** | Development, unclear scope |
| **Cost plus incentive fee (CPIF)** | Allowable cost + a fee varying with performance | **Shared** | Scope where effort is uncertain but effort quality matters |
| **Time and materials (T&M)** | Rates × quantities | **Buyer** | Staff augmentation, short or open-ended work |

The leader's discipline: **risk transferred is risk priced.** An FFP contract for ill-defined
scope does not remove the risk — it converts it into a premium plus a claims exposure when the
scope moves (Domain 10, KA 10.4). Conversely a cost-plus contract on well-defined work pays for
uncertainty that no longer exists.

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
4. **Result.** Fee **USD 60,000** (down from 150,000); buyer pays **USD 2,360,000**.
   **`PTA` = USD 2,428,571.43** — and at that cost the buyer pays exactly the
   USD 2,450,000 ceiling.
5. **Interpretation.** The share ratio does its job up to the `PTA`: both parties lose money on
   overrun, so both want efficiency. **Above the `PTA` the seller bears 100 %** of further cost —
   which is the moment the incentive inverts: a seller heading past it has no financial reason to
   spend more on your project and every reason to argue that the extra cost is *your* scope
   change. Knowing where the `PTA` sits is therefore a **delivery** insight, not an accounting
   one: it predicts when a commercial relationship will turn adversarial (Domain 10, KA 10.4;
   Domain 11's negotiation).

### 7.4.4 Cash flow versus profit

A project can be profitable and still fail for lack of cash — the same truth PFL-AI establishes
for financings (its Domain 1). For a delivery leader the mechanism is payment terms: cost is
incurred as work happens; cash arrives when invoices are approved and paid. On Auriga, with
`AC` = USD 2,120,000 at week 13 and 60-day terms, roughly **USD 742,000** of incurred cost (about
35 %) is still unpaid — an exposure carried by whoever is funding the work. The leader's
obligations are practical: know the terms in both directions (client and subcontractors),
front-load nothing you cannot fund, and never let a retention or milestone-payment structure be
agreed without someone computing its cash profile. Where the project sits inside a financed
asset, this is precisely the CFADS conversation PFL-AI Domain 10 has with lenders.

### AI in this KA

Contract analytics — extracting terms, comparing models, flagging inconsistent clauses — is real
and useful AI assistance, and it is also decision support rather than legal or commercial advice.
Two boundaries: an AI-produced reading of a clause is verified against the clause itself before
anyone relies on it (the document-against-summary check), and commercial and legal positions go
to qualified counsel (Domain 10). Fee arithmetic and `PTA` computations are deterministic — they
get the same golden-answer treatment as the earned-value set.

### Key terms — KA 7.4

| Term | Meaning |
|---|---|
| **Blended rate** | Weighted average cost per hour of a mixed resource pool. |
| **FFP / FPIF / CPFF / CPIF / T&M** | Contract models ordered by who carries cost risk. |
| **Share ratio** | The agreed buyer/seller split of over- and under-run. |
| **Ceiling price** | The cap on the buyer's total exposure under an incentive contract. |
| **Point of total assumption (`PTA`)** | The cost above which the seller bears 100 % of further overrun. |
| **Retention** | A withheld percentage of payment, released on completion criteria. |

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

*Rationale:* Beyond the `PTA` the ceiling binds the buyer, so every further dollar is the
seller's — which predictably redirects the seller's effort from efficiency to entitlement. B
reverses the exposure; A is fiction; D misses that sharing has *stopped*.

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
- C. USD 115.00
- D. USD 140.00

*Rationale:* `10,450/80 = 130.63`. A averages the three rates unweighted; C weights toward the
cheapest grade only; D takes the middle rate as representative.

### Self-check — KA 7.4

1. *State the one question every contract model answers.* — Who carries cost risk.
2. *Why is the `PTA` a delivery concern, not just a commercial one?* — Above it the seller bears
   all further cost, so behaviour shifts from efficiency to entitlement.
3. *How can a profitable project run out of cash?* — Cost is incurred as work happens; cash
   arrives on payment terms — the gap must be funded.

---

## Advanced topics — Domain 7

### 7.A.1 Earned schedule — closing `SPI`'s late-project blind spot

`SPI` converges on 1.00 as a project finishes, whatever its lateness, because `EV` and `PV` both
approach `BAC`. **Earned schedule** restates progress in time: `ES` is the date at which the
value now earned was *planned* to have been earned, and `SPI(t) = ES / AT`. Auriga's week-13
status, with value earned that the baseline expected by week 12, gives
`SPI(t) = 12/13 = 0.92` — and unlike `SPI` it stays meaningful to the last week. This is the
bridge flagged in Domain 6 (KA 6.4.3), now with its cost-side companions.

### 7.A.2 EVM's limitations, stated plainly

Earned value measures conformance to *plan*, not value to the *customer*: a project can score
1.00 on both indices while building the wrong thing (Domain 5's acceptance criteria are the
defence). It says nothing about quality (Domain 9). It is only as honest as its earning rules and
its accruals. And it is blind to risk that has not yet materialised — Domain 8's contingency
analysis, not `CPI`, tells you whether what remains is adequately funded. EVM is a
measurement system, not a management philosophy; leaders who treat the indices as targets get
optimised indices.

### 7.A.3 The reviewer's cost eye

Invariants a reviewer runs before trusting a cost report: `EV/BAC` equals the claimed percent
complete; `CV = EV − AC` and `SV = EV − PV` reconcile to the stated indices; `EAC` = `AC` + a
stated `ETC`, with the assumption named; `TCPI` to `BAC/CPI` equals `CPI`; sum of control-account
budgets + contingency equals `BAC`; management reserve sits outside that total; no completed
package's budget has moved since last period; accruals are present in the current period; and
level-of-effort share is disclosed per control account. Any violation is a defect somewhere —
find it before the board builds on it.

---

## Industry variations — Domain 7

- **Construction and EPC.** Full EVM against a resource-loaded baseline; measured-work valuations
  and retention dominate cash; FIDIC-style variation and claims machinery makes the `PTA`
  conversation routine.
- **Government and defence programmes.** Formal EVMS compliance with surveillance, strict LOE
  caps and control-account discipline; forecasting is auditable rather than discretionary.
- **Technology and product delivery.** Cost is overwhelmingly people, so blended rates and
  capacity, not materials, drive the number; where cadence replaces a fixed baseline, throughput
  and cost-per-increment stand in for `CPI` (Domain 13), and hybrid programmes report both.
- **Energy and resources.** Long-lead equipment and commodity exposure put escalation and
  currency at the centre of the estimate (PFL-AI Domain 3's machinery); contingency is sized
  quantitatively as a matter of course.
- **Public services and transformation.** Benefits, not cost variance, are the accountability
  currency (Domain 16); the leader's task is keeping cost reporting honest while the value case
  is what gets debated.

## Case study — Domain 7: the forecast the board actually needed (utilities)

**Situation.** Auriga's week-13 review. The cost report shows `CPI` 0.91, `SPI` 0.92, `CV`
(200,000), `SV` (160,000). The programme director's paper proposes reporting **`EAC`
USD 4,200,000** — method (a) — on the grounds that the overrun was the contaminated-ground
remediation, a discrete event now closed and remediated (Domain 6's case study).

**The challenge.** The assurance reviewer asks three questions. *Is the cause genuinely closed?*
Yes for the remediation — but the recovery bought a second civil crew and a fast-track of
installation, both of which continue for six more weeks, and neither is in method (a)'s
"remaining work at budgeted rate" assumption. *What does recovery to `BAC` require?* `TCPI` 1.11
against a demonstrated 0.91. *What is the mechanism?* There isn't one — the recovery spend
increases cost to protect the date.

**The outcome.** The board is given a **range with named assumptions**: 4,200,000 if the closed
event is the whole story; 4,416,667 if current efficiency persists; and a bottom-up (method d)
re-estimate of the remaining 52 %, commissioned that week, as the number they will actually
manage to. Contingency draw is authorised against the identified risk; management reserve is not
touched. The minute records the `TCPI` gap and the explicit finding that **no recovery-to-budget
plan exists** — so nobody later claims one was promised.

**What the domain teaches here.** A single-number forecast hides the only thing that matters: the
assumption. `TCPI` converts optimism into an arithmetic claim someone has to defend, and the
honest report is the one that survives the question "what would have to be true?"

## Case study B — Domain 7: past the point of total assumption (technology)

**Situation.** A systems-integration subcontract ran on an incentive structure: target cost
USD 2,000,000, target fee 150,000, 70/30 share, ceiling 2,450,000 — so `PTA` USD 2,428,571. By
month eight the supplier's internal cost was tracking to 2,600,000, well past the `PTA`.

**What happened.** The supplier's behaviour changed abruptly and, in hindsight, rationally: fresh
change requests on work previously treated as in-scope, slower responses on defect fixes that
earned nothing, and a claim that the buyer's late environment provision had caused the growth.
The buyer's team read it as bad faith. It was arithmetic: past the `PTA` every additional
engineer-hour came out of the supplier's own margin, and the only route back to profit was
re-characterising cost as buyer-caused scope.

**The outcome.** The parties re-set commercially — a re-baselined target cost recognising the
genuine environment delay (documented, and partly the buyer's), a revised ceiling, and a
time-and-materials envelope for the disputed remainder. Delivery recovered within two months of
the reset. The retrospective's finding: the buyer had never computed the `PTA`, so a foreseeable
inflection was experienced as a betrayal.

**What the domain teaches here.** Commercial structures create behaviour. Computing the `PTA` at
signature — and watching the supplier's cost trend against it — turns an adversarial surprise
into a managed conversation held early, while both parties still have options (Domain 10's
supplier governance; Domain 11's negotiation).

---

## Executive perspective — Domain 7

What a project leader cannot delegate in this domain:

- **The forecast's assumption.** Analysts compute `EAC`; the leader owns *which* method and why,
  in one sentence a board can challenge. Reporting a number without its assumption is the
  domain's cardinal sin.
- **The `TCPI` reality test.** Before endorsing any recovery-to-budget plan: what index does the
  remaining work require, what is being demonstrated, and what specific mechanism closes the gap?
- **Reserve discipline.** Contingency and management reserve kept distinct, spent under stated
  authority, with consumption trended against progress — the erosion in MCQ 7.1-B is a leadership
  signal, not a bookkeeping detail.
- **Measurement integrity.** Earning rules fixed in advance, accruals present, level-of-effort
  share disclosed, no retrospective budget edits. Cost systems fail morally before they fail
  arithmetically — the same sentence as Domain 6, and the same reason.
- **The commercial shape.** Which model, why, and where the `PTA` sits. A leader who cannot say
  who carries cost risk on their largest package is not yet in control of it.

## Calculation exercises — Domain 7

**Exercise 7.1** `BAC` 4,000,000; `PV` 2,080,000; `EV` 1,920,000; `AC` 2,120,000. Compute `CV`,
`SV`, `CPI`, `SPI`, percent complete and percent spent.
*Solution.* `CV` (200,000); `SV` (160,000); `CPI` 0.91; `SPI` 0.92; complete `1,920,000/4,000,000
=` **48.0 %**; spent `2,120,000/4,000,000 =` **53.0 %**. Common error: computing percent complete
from `AC/BAC` — that is percent *spent*, and reporting it as progress overstates the project.

**Exercise 7.2** Same data. Compute `EAC` by methods (a), (b) and (c), and the corresponding
`VAC`.
*Solution.* (a) `2,120,000 + 2,080,000 =` **4,200,000**, `VAC` **(200,000)**.
(b) `4,000,000/0.905660 =` **4,416,667**, `VAC` **(416,667)**.
(c) `2,120,000 + 2,080,000/(0.905660 × 0.923077) =` **4,608,056**, `VAC` **(608,056)**.
Common error: rounding `CPI` to 0.91 before dividing — `4,000,000/0.91 = 4,395,604`, USD 21,063
adrift. Indices are display; arithmetic is full precision.

**Exercise 7.3** Same data. Compute `TCPI` to `BAC` and to the method-(b) `EAC`, and interpret.
*Solution.* To `BAC`: `2,080,000/1,880,000 =` **1.11**. To (b): `2,080,000/(4,416,667 −
2,120,000) = 2,080,000/2,296,667 =` **0.91**. The second equals the current `CPI` — the identity
of 7.3.4: `BAC/CPI` forecasts *are* "nothing changes". Common error: using `PV` rather than `AC`
in the denominator (giving 1.08) and concluding recovery is nearly free.

**Exercise 7.4** Incentive subcontract: target cost 2,400,000; target fee 180,000; share 80/20;
ceiling 2,900,000. The seller finishes at 2,650,000. Find the fee, what the buyer pays, and the
`PTA`.
*Solution.* Overrun 250,000; fee `180,000 − 250,000 × 0.20 =` **130,000**; buyer pays
`2,650,000 + 130,000 =` **2,780,000** (below the 2,900,000 ceiling). Target price 2,580,000;
`PTA = 2,400,000 + (2,900,000 − 2,580,000)/0.80 = 2,400,000 + 400,000 =` **2,800,000**. Common
error: applying the buyer's 80 % share to the fee reduction.

**Exercise 7.5** A pool of 30 at USD 105/h, 20 at USD 150/h and 10 at USD 220/h. Compute the
blended rate, then the cost of adding four weeks of a 5-person crew at 40 h/week.
*Solution.* `(30×105 + 20×150 + 10×220)/60 = (3,150 + 3,000 + 2,200)/60 = 8,350/60 =`
**USD 139.17/h**. Crew cost `5 × 40 × 4 × 139.17 =` **USD 111,333**. Common error: using the
blended rate for a crew drawn entirely from one grade — the blend applies to a representative
mix, not to any arbitrary subset.

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
ceiling · **`PTA` and current cost trend against it** · payment terms both directions and the
cash profile · retention and release criteria · variation and claims route · escalation contacts.
Reviewed at every commercial checkpoint, not filed at signature.

## Exam preparation — Domain 7

**The calculation traps.** Percent spent reported as percent complete (Exercise 7.1) · rounding
`CPI` before dividing (Exercise 7.2) · `PV` instead of `AC` in the `TCPI` denominator
(Exercise 7.3) · applying the wrong party's share to an incentive fee (Exercise 7.4) · `EV`
valued at actual cost rather than budget · reading a level-of-effort-heavy `SPI` as schedule
performance (MCQ 7.3-D) · quoting `SPI` late in a project where it must converge on 1.00
(7.A.1) · confusing commitments with actuals · treating contingency and management reserve as one
pot.

**Reflection questions.**
1. Your cost report shows one `EAC`. What must accompany it before you will sign it? *(The method
   and the assumption about remaining work; `TCPI` to `BAC` beside it.)*
2. On your largest package: who carries cost risk, where is the `PTA`, and how close is the
   supplier's cost trend to it? *(7.4.2–7.4.3; toolkit 7.T.3.)*
3. Which invariant in 7.A.3 would have caught the last cost surprise you experienced — and why
   wasn't it running?

## Domain 7 summary

Cost leadership begins with an honest number: a method matched to definition maturity, a range and
class always stated, three-point thinking where tails are real, and a budget built through control
accounts into a time-phased baseline with contingency inside it and management reserve outside.
Measurement then has to be earned — `AC` with accruals, `EV` at budget under earning rules fixed
in advance — because everything downstream is arithmetic on those numbers: `CV`, `SV`, `CPI`,
`SPI` at Auriga's week 13 (0.91 and 0.92; 48 % complete against 53 % spent), the `EAC` family
spanning USD 408,056 on identical data because they encode different assumptions, and `VAC` and
`TCPI` turning recovery talk into a defensible index (1.11 required against 0.91 demonstrated).
The commercial half is the same discipline pointed outward: blended rates make options
commensurable, contract models allocate cost risk at a price, and the point of total assumption
predicts when a supplier's incentives invert. Throughout, the leadership rule holds — the number
is only as good as the assumption named beside it, and machine-produced forecasts reach a board
only with a calibration record and a human owner. Domain 8 quantifies the risk this domain
reserves for; Domain 10 takes the commercial relationships into their own depth.
