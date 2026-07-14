# Domain 6 — Earned Value Management & Forecasting (EVM / EAC) *(flagship)*

> **Group:** Project management (Domain 6 of 8 in the PM group). **Target:** ~90 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md). This domain is the definitive home of the master
> formula symbols (`PV`, `EV`, `AC`, `BAC`, `CV`, `SV`, `CPI`, `SPI`, `EAC`, `ETC`, `VAC`, `TCPI`) — every
> other domain restates them from here. British English; USD (+SAR where useful).

## Why this domain exists

Earned value management (EVM) is the technique that answers, at any moment, the three questions a project
board actually asks: *how much have we done, what did it cost, and what will it cost to finish?* It does so by
integrating **three measures** — Planned Value (the baseline, Domain 3), Earned Value (physical progress
valued at budget), and Actual Cost (the true cost-to-date, Domain 5) — into cost and schedule variances,
performance indices, and forecasts. This domain covers the three measures and how earned value is *measured*
(KA 6.1); the variances and indices and how to read them (KA 6.2); **forecasting with the EAC family** — the
heart of the domain (KA 6.3); and how cost and schedule integrate, EVM's limitations, and the earned-schedule
refinement (KA 6.4). Everything in Domains 3–5 exists so that the numbers entering these formulae are true;
everything in Domain 4 is how their results are reported. EVM is where the discipline comes together.

**Learning objectives.** After this domain a candidate can: define `PV`, `EV`, `AC` and measure `EV` by the
common methods; compute and interpret `CV`, `SV`, `CPI`, `SPI` and `TCPI`; forecast `EAC`/`ETC`/`VAC` by the
main methods and select the one matching the variance's cause; and explain how EVM integrates cost and
schedule, its limitations, and how earned schedule addresses the schedule-index weakness.

**The master worked project.** One project runs through the whole domain. Using the 10-month, **`BAC` = USD
1,000,000** cost baseline built in Domain 3 (KA 3.3.3), take a **data date at the end of Month 5**, where the
plan says **`PV` = USD 520,000**. Measurement at that date gives **`EV` = USD 480,000** and **`AC` = USD
530,000**. Every KA below uses these figures.

---

## Knowledge Area 6.1 — EVM fundamentals

*Topics: 6.1.1 the three measures · 6.1.2 measuring earned value · 6.1.3 the integrated picture.*

### 6.1.1 The three measures

**Definitions.** EVM rests on three time-phased quantities, all expressed in the **same budget currency** so
they are directly comparable:

- **Planned Value (`PV`, or BCWS)** — the budgeted cost of the work *scheduled* to be done by the data date;
  the cost-baseline S-curve (Domain 3, KA 3.3). In the master project, `PV = 520,000`.
- **Earned Value (`EV`, or BCWP)** — the budgeted cost of the work *actually performed* by the data date;
  physical progress **valued at the budget rate**, never at actual cost. In the master project, `EV =
  480,000`.
- **Actual Cost (`AC`, or ACWP)** — the cost *actually incurred* for the work performed, including accruals
  (Domain 5, KA 5.2). In the master project, `AC = 530,000`.

The single most important conceptual point: **`EV` is measured at budget, not at actual cost.** That is what
lets `EV` be compared with `PV` (both at budget → schedule progress) and with `AC` (budget vs actual for the
same work → cost efficiency). Confusing "value earned" with "cost incurred" collapses the whole method.

### 6.1.2 Measuring earned value

**The principle.** `EV` is the budget of the work performed, so measuring it means measuring **physical
progress** and multiplying by budget. The common **measurement methods** (earning rules) trade objectivity
against granularity:

| Method | Rule | Best for |
|---|---|---|
| **0/100** | Earn 0 % until complete, then 100 % | Short work packages; avoids subjective part-progress |
| **50/50** | Earn 50 % at start, 50 % at completion | Short packages where some start credit is fair |
| **Percent complete** | Earn the assessed % complete | Longer packages; needs an objective basis |
| **Units completed** | (units done / total units) × budget | Repetitive, measurable work (e.g. metres, welds) |
| **Weighted milestones** | Earn defined value at each milestone | Long packages with objective interim milestones |

**Worked example 6.1.2 — the same package, five methods.**

1. **Setup.** A work package with budget **USD 100,000**; it is physically **40 % complete** and **has
   started but not finished**; of its total, **400 of 1,000 units** are done; the milestone worth 40 % of
   value is reached.
2. **Result by method.**
   - **0/100:** not complete → **`EV` = 0**.
   - **50/50:** started → **`EV` = 50,000**.
   - **Percent complete (40 %):** `0.40 × 100,000 = ` **40,000**.
   - **Units completed:** `(400/1,000) × 100,000 = ` **40,000**.
   - **Weighted milestone (40 % milestone reached):** **40,000**.
3. **Interpretation.** The *same* physical state yields `EV` from 0 to 50,000 depending on the earning rule —
   which is why the rule must be **fixed in advance** per work package and applied consistently. The 0/100 and
   50/50 rules trade accuracy for objectivity (no subjective % judgement); percent-complete and units are more
   precise but need an objective basis to resist optimism. Choosing objective rules is the main defence against
   the classic EVM failure: **`EV` inflated by optimistic progress claims**, which flatters both `SPI` and
   `CPI`.

**Worked example 6.1.2b — aggregating earned value across a portfolio of work packages.**

- **Setup.** A project has four work packages, each with its own earning rule fixed in advance. At the data
  date:

  | Work package | Budget | Earning rule | Status | `EV` |
  |---|---:|---|---|---:|
  | A | 100,000 | 0/100 | complete | 100,000 |
  | B | 200,000 | percent complete | 60 % | 120,000 |
  | C | 150,000 | units | 300/500 units | 90,000 |
  | D | 50,000 | 0/100 | not started | 0 |
  | **Total** | **500,000** | | | **310,000** |

  The plan and the cost ledger give, per package:

  | Work package | `PV` | `AC` |
  |---|---:|---:|
  | A | 100,000 | 105,000 |
  | B | 150,000 | 130,000 |
  | C | 120,000 | 100,000 |
  | D | 30,000 | 0 |
  | **Total** | **400,000** | **335,000** |

- **Formula.** `CPI = EV / AC`; `SPI = EV / PV` — computed on the **aggregated** `EV`, `AC` and `PV`.
- **Substitution.** `CPI = 310,000 / 335,000`; `SPI = 310,000 / 400,000`.
- **Result.** `CPI` = **0.93**; `SPI` = **0.78**.
- **Interpretation.** Earned value is measured **bottom-up** at the work-package level, each package under its
  own fixed earning rule, and then aggregated — the project-level `CPI` and `SPI` are **rollups**. A rollup
  can hide a struggling package: here package C has earned 90,000 against a `PV` of 120,000 and is well behind
  its own plan, a detail invisible in the single project `SPI` of 0.78. Always read the control-account detail
  beneath the aggregate (Domain 5, KA 5.3).

### 6.1.3 The integrated picture

**The principle.** Plotted together on the S-curve canvas (Domain 3, KA 3.3.2), the three measures reveal
performance at a glance: `EV` below `PV` means **behind schedule** (less done than planned); `AC` above `EV`
means **over cost** (paid more than the work was worth). The master project shows both — `EV` (480) is below
`PV` (520) *and* below `AC` (530). The rest of the domain quantifies exactly how much, and what it implies for
the finish.

> **Fig 6.1.1 — The three EVM curves at the data date.** *Caption:* `PV`, `EV` and `AC` at the end of Month 5.
> *Underlying data:* the Planned Value S-curve from Domain 3 {…, 520 at Month 5}; `EV` reaching 480; `AC`
> reaching 530 (USD 000). *Render-ready description:* the Month 1–10 S-curve; three points at Month 5 — `PV`
> 520 (blue baseline curve), `EV` 480 (green), `AC` 530 (amber); vertical gaps annotated "`SV` = EV − PV =
> (40)" between EV and PV and "`CV` = EV − AC = (50)" between EV and AC. *Animation storyboard (digital-only):*
> the baseline draws first; a "data date" line drops at Month 5; `EV` and `AC` markers appear and the two gap
> annotations extend, previewing the variances of KA 6.2.

### Key terms — KA 6.1

| Term | Meaning |
|---|---|
| **Planned Value (`PV`)** | Budgeted cost of work scheduled by the data date. |
| **Earned Value (`EV`)** | Budgeted cost of work performed — progress valued at budget. |
| **Actual Cost (`AC`)** | Cost actually incurred (incl. accruals) for the work performed. |
| **Earning rule / measurement method** | The rule converting physical progress to `EV` (0/100, 50/50, % complete, units, milestones). |

### Sample MCQs — KA 6.1

**MCQ 6.1-A `[6.1.1 · Recall]`** Earned value (`EV`) is:
- A. The actual cost of the work performed.
- B. The budgeted cost of the work performed. ✅
- C. The budgeted cost of the work scheduled.
- D. The cash received to date.

*Rationale:* `EV` values physical progress at the **budget** rate. A is `AC`; C is `PV`; D is a cash measure,
unrelated.

**MCQ 6.1-B `[6.1.2 · Application]`** A USD 100,000 package is 40 % complete. Under the **50/50** rule (started,
not finished), `EV` is:
- A. USD 0
- B. USD 40,000
- C. USD 50,000 ✅
- D. USD 100,000

*Rationale:* 50/50 earns 50 % on start — **50,000** — regardless of the 40 % physical progress. A is 0/100; B
is percent-complete; D is complete.

**MCQ 6.1-C `[6.1.2 · Application]`** A work package with a budget of USD 250,000 earns under the **units
completed** rule. At the data date **600 of 800 units** are done. `EV` is:
- A. USD 150,000
- B. USD 187,500 ✅
- C. USD 125,000
- D. USD 250,000

*Rationale:* `EV = (600/800) × 250,000 = 0.75 × 250,000 = 187,500`. A uses a wrong denominator of 1,000 units
(60 %); C is the 50/50 rule's start credit; D would require all units complete.

**MCQ 6.1-D `[6.1.3 · Analysis]`** At the data date a project shows `EV` **above** `PV` but `AC` **above**
`EV`. The integrated picture is:
- A. Behind schedule and over cost.
- B. Ahead of schedule but over cost. ✅
- C. Ahead of schedule and under cost.
- D. Behind schedule but under cost.

*Rationale:* `EV > PV` means more work done than planned (ahead of schedule); `AC > EV` means the work cost
more than its budgeted value (over cost). A reverses both reads; C and D each misread one of the two gaps.

### Self-check — KA 6.1

1. Why must `EV` be measured at budget rather than actual cost? *(So it is comparable to `PV` for schedule and
   to `AC` for cost; measuring at actual collapses the method.)*
2. Why fix the earning rule per work package in advance? *(The same physical state gives different `EV` by
   rule; fixing it prevents optimistic, inconsistent progress claims.)*

---

## Knowledge Area 6.2 — Variances and performance indices

*Topics: 6.2.1 cost and schedule variance · 6.2.2 the performance indices · 6.2.3 the to-complete performance
index · 6.2.4 reading the indices together.*

### 6.2.1 Cost and schedule variance

**Formulae.**
```
Cost Variance      CV = EV − AC     (negative = over cost)
Schedule Variance  SV = EV − PV     (negative = behind schedule)
```
Both are in currency. A negative variance is adverse; a positive one favourable — but read with cause (Domain
4, KA 4.2.5).

**Worked example 6.2.1 — the master project's variances.**
`CV = EV − AC = 480,000 − 530,000 = ` **(USD 50,000)** (over cost);
`SV = EV − PV = 480,000 − 520,000 = ` **(USD 40,000)** (behind schedule).
The project is **both** over cost and behind schedule at Month 5.

### 6.2.2 The performance indices

**Formulae.**
```
Cost Performance Index      CPI = EV / AC     (>1 = under cost, efficient)
Schedule Performance Index  SPI = EV / PV     (>1 = ahead of schedule)
```
Both are ratios (dimensionless), which makes them **comparable across projects of different size** — a `CPI`
of 0.91 means the same thing on a USD 1m and a USD 1bn project (you are getting USD 0.91 of value per USD 1
spent).

**Worked example 6.2.2 — the master project's indices.**
`CPI = EV / AC = 480,000 / 530,000 = ` **0.91**;
`SPI = EV / PV = 480,000 / 520,000 = ` **0.92**.
Interpretation: for every USD 1 spent, USD 0.91 of budgeted value is being earned (a ~9 % cost inefficiency);
and work is progressing at ~92 % of the planned rate.

### 6.2.3 The to-complete performance index (TCPI)

**Definition & purpose.** The **`TCPI`** is the cost efficiency the *remaining* work must achieve to hit a
target — either the original `BAC` or the current `EAC`:

```
TCPI (to meet BAC) = (BAC − EV) / (BAC − AC)
TCPI (to meet EAC) = (BAC − EV) / (EAC − AC)
```
It is a **reality check**: if achieving the `BAC` now requires a `TCPI` far above the `CPI` already being
delivered, the `BAC` is no longer credible.

**Worked example 6.2.3 — is the BAC still achievable?**
`TCPI(BAC) = (1,000,000 − 480,000) / (1,000,000 − 530,000) = 520,000 / 470,000 = ` **1.11**.
Interpretation: to finish within the `BAC`, the remaining work must be performed at a `CPI` of **1.11** — a
~22 % improvement on the **0.91** achieved so far. A swing of that size, sustained, is rarely realistic
without a specific, credible intervention. The `TCPI` (1.11) diverging sharply from the `CPI` (0.91) is a
quantified signal that the `BAC` should give way to a higher `EAC` (KA 6.3).

### 6.2.4 Reading the indices together

**The professional angle.** The four numbers together tell a fuller story than any one:

| `CPI` | `SPI` | Situation |
|---|---|---|
| < 1 | < 1 | Over cost **and** behind — the master project (0.91, 0.92); usually the most serious |
| < 1 | > 1 | Over cost but ahead — often *buying* schedule with cost (acceleration) |
| > 1 | < 1 | Under cost but behind — possibly under-resourced/slow but efficient |
| > 1 | > 1 | Under cost and ahead — verify it is real, not optimistic `EV` |

The professional also watches **trend** (Domain 4, KA 4.1.2): a `CPI` sliding period on period matters more
than its level. And they interrogate a *too-good* result as hard as a bad one — a `CPI`/`SPI` both well above
1 can signal inflated `EV` rather than genuine outperformance.

### Key terms — KA 6.2

| Term | Meaning |
|---|---|
| **`CV` / `SV`** | Cost variance (`EV − AC`) / schedule variance (`EV − PV`). |
| **`CPI` / `SPI`** | Cost / schedule performance index (`EV/AC`, `EV/PV`). |
| **`TCPI`** | To-complete performance index — the efficiency remaining work must achieve for a target. |

### Sample MCQs — KA 6.2

**MCQ 6.2-A `[6.2.2 · Application]`** `EV` = 480,000; `AC` = 530,000; `PV` = 520,000. The `CPI` and `SPI` are:
- A. 0.91 and 0.92 ✅
- B. 0.92 and 0.91
- C. 1.10 and 1.08
- D. 1.02 and 1.04

*Rationale:* `CPI = 480/530 = 0.91`; `SPI = 480/520 = 0.92`. B swaps them; C and D invert the ratios.

**MCQ 6.2-B `[6.2.3 · Analysis]`** `BAC` = 1,000,000; `EV` = 480,000; `AC` = 530,000. The `TCPI` to meet `BAC`
is 1.11, while the `CPI` achieved is 0.91. This indicates:
- A. The BAC is comfortably achievable.
- B. The remaining work must be far more efficient than achieved so far, so the BAC is likely not credible. ✅
- C. The project is ahead of schedule.
- D. The EV is wrong.

*Rationale:* A required 1.11 against a delivered 0.91 is a large, usually unrealistic swing — the `BAC` is
probably no longer credible and an `EAC > BAC` is warranted. It says nothing about schedule and does not by
itself imply an `EV` error.

**MCQ 6.2-C `[6.2.1 · Application]`** With `EV` = 480,000 and `PV` = 520,000, the schedule variance is:
- A. USD +40,000
- B. USD (40,000) ✅
- C. USD (50,000)
- D. USD 0

*Rationale:* `SV = EV − PV = 480,000 − 520,000 = (40,000)` — behind schedule. A has the wrong sign; C is the
cost variance; D ignores the gap.

**MCQ 6.2-D `[6.2.3 · Application]`** `BAC` = 2,000,000; `EV` = 900,000; `AC` = 1,000,000. The `TCPI` to meet
the `BAC` is:
- A. 0.90
- B. 1.10 ✅
- C. 0.91
- D. 1.00

*Rationale:* `TCPI(BAC) = (BAC − EV)/(BAC − AC) = (2,000,000 − 900,000)/(2,000,000 − 1,000,000) =
1,100,000/1,000,000 = 1.10`. A is the `CPI` achieved to date (`900,000/1,000,000`); C inverts the `TCPI`
ratio; D assumes remaining work runs exactly to budget.

**MCQ 6.2-E `[6.2.4 · Analysis]`** A project reports `CPI` = 0.88 and `SPI` = 1.06. The most likely reading
is:
- A. The project is efficient but under-resourced.
- B. The project is buying schedule with cost — accelerating at a cost premium. ✅
- C. The project is over cost and behind schedule.
- D. The `EV` must be inflated.

*Rationale:* `CPI < 1` with `SPI > 1` is the over-cost-but-ahead quadrant, which often signals acceleration —
schedule gained at a cost premium. A describes the `CPI > 1`, `SPI < 1` quadrant; C describes both indices
below 1; D is a check reserved for results that look *too good* on both indices, not this pattern.

### Self-check — KA 6.2

1. Write the formulae for `CV`, `SV`, `CPI`, `SPI` and state what each sign/level means. *(`CV=EV−AC`,
   `SV=EV−PV`; `CPI=EV/AC`, `SPI=EV/PV`; <0 or <1 adverse.)*
2. What does a `TCPI` well above the achieved `CPI` tell you? *(The target — usually `BAC` — is likely no
   longer credible.)*

---

## Knowledge Area 6.3 — Forecasting with EVM: the EAC family *(the heart of the domain)*

*Topics: 6.3.1 the ETC/EAC identity · 6.3.2 the EAC methods and their assumptions · 6.3.3 selecting a method
· 6.3.4 VAC and the completion picture.*

### 6.3.1 The ETC/EAC identity

**The principle.** Every forecast shares one identity: the total cost is what has been spent plus what
remains.
```
EAC = AC + ETC
```
- `EAC` — estimate at completion (total forecast cost).
- `AC` — actual cost to date.
- `ETC` — estimate to complete (forecast cost of remaining work).

The methods differ **only** in how `ETC` is derived from performance to date — which is really a question
about *what assumption* to make regarding the remaining work.

### 6.3.2 The EAC methods and their assumptions

```
(a) Remaining work at the BUDGETED rate — today's variance was a one-off (atypical):
    ETC = BAC − EV                       →  EAC = AC + (BAC − EV)
(b) Remaining work at the CURRENT COST rate — the cost variance will persist (typical):
    ETC = (BAC − EV) / CPI               →  EAC = AC + (BAC − EV)/CPI  =  BAC / CPI
(c) Remaining work dragged by BOTH cost AND schedule performance:
    ETC = (BAC − EV) / (CPI × SPI)       →  EAC = AC + (BAC − EV)/(CPI × SPI)
(d) A fresh BOTTOM-UP estimate of the remaining work (re-estimate ETC directly):
    EAC = AC + ETC(bottom-up)
```

**Worked example 6.3.2 — four EACs on the master project.** `BAC = 1,000,000`; `AC = 530,000`; `EV = 480,000`;
`CPI = 0.9057`; `SPI = 0.9231`.

| Method | Assumption | Computation | `EAC` |
|---|---|---|---:|
| (a) budgeted rate | variance atypical | `530,000 + (1,000,000 − 480,000)` | **1,050,000** |
| (b) current CPI | variance persists | `1,000,000 / 0.9057` | **1,104,167** |
| (c) CPI × SPI | cost & schedule compound | `530,000 + 520,000/(0.9057×0.9231) = 530,000 + 520,000/0.8360` | **≈ 1,152,010** |
| (d) bottom-up | re-estimate remainder | `530,000 + ETC` (from a fresh estimate) | *as estimated* |

The methods bracket the outcome from **1.05m to 1.15m** — a USD 100,000 range that is *not* imprecision but
*different assumptions* about the remaining work.

**Worked example 6.3.2b — a bottom-up ETC (method d).**

- **Setup.** The master project (`BAC` = 1,000,000; `AC` = 530,000; `EV` = 480,000). The remaining work is a
  **commissioning phase** quite unlike the work performed to date, so performance to date is **not
  representative** of the remainder — the team re-estimates the remaining work from the bottom up. The fresh
  bottom-up estimate of the commissioning work gives `ETC` = **610,000**.
- **Formula.** `EAC = AC + ETC`.
- **Substitution.** `EAC = 530,000 + 610,000`.
- **Result.** `EAC` = **USD 1,140,000**.
- **Interpretation.** Method (d) is chosen precisely because the formula methods — which all **extrapolate
  past performance** — would misforecast a phase of different work. The bottom-up `EAC` of 1,140,000 happens
  to sit within the range of the formula methods (1,050,000 / 1,104,167 / 1,152,010), but it rests on a
  **re-estimate of the work ahead**, not an extrapolation of the work behind — a materially stronger basis
  when the character of the remaining work has changed.

### 6.3.3 Selecting a method

**The professional judgement.** The method is chosen to match the **cause** of the variance, and defended:

- Use **(a)** when the variance came from a **discrete, closed** event unlikely to recur (a one-off rate spike
  now locked, a rework episode now finished) — the remaining work will run to budget.
- Use **(b)** when the variance reflects a **systemic** cost inefficiency (productivity, rates) likely to
  **persist** — the most common default when a stable `CPI` trend exists.
- Use **(c)** when being **behind schedule is itself inflating cost** (extended preliminaries, disruption,
  acceleration) so cost and schedule performance **compound**.
- Use **(d)** when performance to date is **not representative** of the remainder (a phase change, a different
  work type ahead) — re-estimate the remaining work from the bottom up.

Selecting mechanically — always using one formula — is the classic error; the formulae encode assumptions, and
the professional owns the assumption. The `TCPI` reality check (6.2.3) and the `CPI` trend inform the choice.

### 6.3.4 VAC and the completion picture

**Formula.** `VAC = BAC − EAC` (negative = projected overrun).

**Worked example 6.3.4 — variance at completion.** Taking method (b) as the defended forecast:
`VAC = 1,000,000 − 1,104,167 = ` **(USD 104,167)** — a projected overrun of ~10 %. Reported with its method
and assumption ("`EAC` USD 1.10m on the persisting-`CPI` basis; `VAC` (USD 104k); to recover to `BAC` would
need remaining `CPI` of 1.11 vs 0.91 achieved — not currently credible"), this is a **decision-ready** forecast
(Domain 4, KA 4.3.3): it tells the board the likely outcome, the assumption behind it, and why the original
budget is no longer realistic — early enough to act.

> **Fig 6.3.1 — The EAC fan.** *Caption:* four forecasts diverging from the data date. *Underlying data:* from
> Month 5 (`AC` 530), forecast completion at `EAC` (a) 1,050, (b) 1,104, (c) 1,152 (USD 000), against `BAC`
> 1,000. *Render-ready description:* the S-curve to Month 5, then a **fan** of dashed forecast curves to the
> planned finish — one per method — each labelled with its `EAC`; a horizontal reference line at `BAC` 1,000
> showing all forecasts above it (the `VAC` gap). *Animation storyboard (digital-only):* at the data date the
> single actual curve splits into the fan of forecasts, each annotated with its assumption; the `BAC` line and
> the `VAC` gaps highlight.

### Key terms — KA 6.3

| Term | Meaning |
|---|---|
| **`EAC` / `ETC`** | Estimate at / to complete; `EAC = AC + ETC`. |
| **EAC (a)–(d)** | Budgeted-rate / current-CPI / CPI×SPI / bottom-up methods, each an assumption. |
| **`VAC`** | Variance at completion (`BAC − EAC`). |

### Sample MCQs — KA 6.3

**MCQ 6.3-A `[6.3.2 · Application]`** `BAC` = 1,000,000; `AC` = 530,000; `EV` = 480,000. `EAC = AC + (BAC − EV)`
gives:
- A. USD 1,000,000
- B. USD 1,050,000 ✅
- C. USD 1,104,167
- D. USD 1,152,010

*Rationale:* `530,000 + (1,000,000 − 480,000) = 530,000 + 520,000 = 1,050,000`. C is the `BAC/CPI` method; D is
`CPI×SPI`; A is the `BAC`.

**MCQ 6.3-B `[6.3.3 · Analysis]`** A cost overrun was caused by a one-off rate spike, now locked by a fixed
supply agreement; the remaining work is expected to run to budget. The most appropriate EAC method is:
- A. `EAC = BAC/CPI`
- B. `EAC = AC + (BAC − EV)` ✅
- C. `EAC = AC + (BAC − EV)/(CPI × SPI)`
- D. `EAC = BAC`

*Rationale:* A discrete, closed cause means the variance is **atypical**, so remaining work is forecast at the
budgeted rate — method (a). B and C assume the variance persists or compounds; D ignores cost already sunk
above budget.

**MCQ 6.3-C `[6.3.4 · Application]`** With `EAC` = 1,104,167 and `BAC` = 1,000,000, the `VAC` is:
- A. USD +104,167
- B. USD (104,167) ✅
- C. USD (50,000)
- D. USD 0

*Rationale:* `VAC = BAC − EAC = 1,000,000 − 1,104,167 = (104,167)` — a projected overrun. A has the wrong sign;
C is the current `CV`; D ignores the forecast.

**MCQ 6.3-D `[6.3.2 · Application]`** `BAC` = 800,000; `EV` = 300,000; `AC` = 375,000. Assuming the cost
variance **persists**, the `EAC` is:
- A. USD 875,000
- B. USD 1,000,000 ✅
- C. USD 640,000
- D. USD 800,000

*Rationale:* A persisting variance calls for `EAC = BAC/CPI`; `CPI = 300,000/375,000 = 0.80`, so `EAC =
800,000/0.80 = 1,000,000`. A is the budgeted-rate method (`AC + (BAC − EV)`), which assumes the variance was
atypical; C multiplies by `CPI` instead of dividing; D simply restates the `BAC`.

**MCQ 6.3-E `[6.3.1 · Recall]`** In the identity `EAC = AC + ETC`, the `ETC` is:
- A. The total forecast cost of the project.
- B. The forecast cost of the remaining work. ✅
- C. The cost actually incurred to date.
- D. The variance between budget and forecast at completion.

*Rationale:* `ETC` is the estimate to complete — the forecast cost of the work still to be done; the methods
differ only in how it is derived. A is the `EAC` itself; C is `AC`; D is the `VAC`.

### Self-check — KA 6.3

1. State `EAC = AC + ETC` and the four ways to derive `ETC`. *(Budgeted rate; current `CPI`; `CPI × SPI`;
   bottom-up.)*
2. Which method fits a systemic, persisting cost inefficiency, and which fits a one-off closed event? *(Persisting →
   `BAC/CPI`; one-off → `AC + (BAC − EV)`.)*

---

## Knowledge Area 6.4 — Integrating cost & schedule; limitations; earned schedule

*Topics: 6.4.1 EVM as cost-schedule integration · 6.4.2 the limitations of EVM · 6.4.3 earned schedule ·
6.4.4 EVM and adaptive delivery · 6.4.5 sector mini-case — a deteriorating trend.*

### 6.4.1 EVM as cost-schedule integration

**The principle.** EVM's distinctive strength is that it measures **cost and schedule in one integrated
framework**, anchored to physical progress. `CV` and `CPI` speak to cost; `SV` and `SPI` speak to schedule;
and because both are built from the same `EV`, they cannot tell contradictory stories about how much work is
done. A cost report and a schedule report maintained separately can drift apart; EVM forces them onto one
foundation — which is exactly why the control account (Domain 5, KA 5.3) integrates both.

### 6.4.2 The limitations of EVM

**The honest limits.** EVM is powerful but not complete, and a professional states its limits:

- **`SV`/`SPI` are in cost units, not time**, and both **converge to 0/1 at completion regardless of lateness**
  — at the end, all planned value has been earned, so `SPI → 1` even on a late project. This makes the
  schedule indices **misleading late in a project** (the earned-schedule fix, 6.4.3).
- **EVM does not see the critical path.** A healthy overall `SPI` can hide a critical-path activity slipping
  while non-critical work runs ahead. EVM must be read **alongside** critical-path analysis (Domain 10).
- **`EV` is only as honest as the earning rules and progress claims** (6.1.2) — optimistic `EV` flatters
  everything.
- **Data quality** (Domain 5, KA 5.2.4) governs all of it — accruals, coding and reconciliation must be sound.

### 6.4.3 Earned schedule

**Definition & purpose.** **Earned schedule (ES)** addresses the time-blindness of `SV`/`SPI` by translating
earned value into **time**: `ES` is the point *on the time axis* at which the current `EV` **should have been
earned** according to the baseline. Comparing it to the actual time (`AT`) gives time-based measures:

```
SV(t) = ES − AT          (in time units, e.g. months)
SPI(t) = ES / AT
```
Because `ES` is read against the baseline curve in **time**, `SPI(t)` does **not** artificially converge to 1
at completion — it keeps reporting lateness meaningfully to the end. In the master project, `EV` = 480,000
corresponds to a point on the baseline curve **before** Month 5 (the plan reached 480 partway through the
month), so `ES < AT` and `SPI(t) < 1` — confirming the delay in time terms.

**Worked example 6.4.3b — computing earned schedule.**

- **Setup.** The master project at the end of Month 5, so actual time `AT` = 5 months; `EV` = 480,000. The
  Planned Value S-curve (Domain 3) has cumulative `PV` of **360,000 at Month 4** and **520,000 at Month 5**.
  `ES` is the time on the baseline at which cumulative `PV` equals the current `EV` (480,000) — it lies
  between Month 4 (360,000) and Month 5 (520,000).
- **Formula.** Interpolate `ES` between the bracketing months; then `SV(t) = ES − AT` and `SPI(t) = ES / AT`.
- **Substitution.** `ES = 4 + (480,000 − 360,000) / (520,000 − 360,000) = 4 + 120,000/160,000 = 4 + 0.75`.
- **Result.** `ES` = **4.75 months**; `SV(t) = 4.75 − 5 = ` **(0.25) months** (about a quarter-month behind);
  `SPI(t) = 4.75 / 5 = ` **0.95**.
- **Interpretation.** Compare with the cost-based `SPI = EV / PV = 480,000 / 520,000 = ` **0.92**: earned
  schedule expresses the same lateness in **time** (~0.25 month behind) rather than in currency, and — unlike
  the cost-based index — `SPI(t)` will not drift to 1.0 as the project completes, so it keeps reporting the
  delay meaningfully to the end.

### 6.4.4 EVM and adaptive delivery

**The bridge to Domain 9.** Classical EVM assumes a **fixed scope baseline**, which is exactly what adaptive/
agile delivery does not have. Domain 9 (KA 9.5) develops **AgileEVM** — applying `EV`, `CPI`, `SPI` and `EAC`
where scope is variable, using release/sprint budgets and story-point progress — together with its assumptions
and limitations, and reconciles it back to these formulae and to IFRS 15 revenue (Domain 2). The notation
here is deliberately the notation used there, so the classical and adaptive treatments stay one language.

### 6.4.5 Sector mini-case — a defence programme's deteriorating trend

A defence systems programme with **`BAC` = USD 50,000,000** reports the following at two successive data
dates:

| Data date | `PV` | `EV` | `AC` | `CPI` | `SPI` |
|---|---:|---:|---:|---:|---:|
| Month 3 | 12,000,000 | 11,000,000 | 11,500,000 | 0.96 | 0.92 |
| Month 6 | 24,000,000 | 21,000,000 | 23,000,000 | 0.91 | 0.88 |

Neither month's `CPI` is catastrophic in isolation; the signal is the **trend** — `CPI` deteriorating from
0.96 to 0.91 (and `SPI` from 0.92 to 0.88) — which is a stronger warning than either single value (Domain 4,
KA 4.1.2 on leading vs lagging indicators; Domain 3, KA 3.4.3 on trend analysis). Forecasting on the Month 6
performance: `EAC = BAC / CPI = 50,000,000 / 0.91 = ` **54,945,000** (approx. USD 54.9m), so `VAC =
50,000,000 − 54,945,000 = ` **(4,945,000)**. The reality check confirms the picture: `TCPI` to recover to
`BAC` `= (BAC − EV) / (BAC − AC) = (50,000,000 − 21,000,000) / (50,000,000 − 23,000,000) = 29,000,000 /
27,000,000 = ` **1.07** — the remaining work would have to run at 1.07 against the 0.91 achieved, which,
combined with the **worsening** trend, makes the `BAC` not credible.

The controls professional's response: **escalate early on the trend** rather than waiting for a single bad
month; re-baseline expectations to the ~USD 54.9m `EAC`; and drive a **specific recovery action** (a named
cause, an owner, a date) rather than hoping the average improves.

**AI in this KA.** EVM forecasting is a leading AI use case (Domains 3.4, 13.5): predictive `EAC` models,
early-warning systems that fuse `CPI`/`SPI` trends with leading indicators, and driver analysis that explains
*why* performance is moving. AI can also help detect inflated `EV` (progress claims inconsistent with cost or
physical data). Its limits are the domain's limits: a model cannot see the critical path unless given the
schedule, can be confidently wrong, and forecasts a number the professional must still own and defend. **AI
proposes, the professional disposes.**

### Key terms — KA 6.4

| Term | Meaning |
|---|---|
| **Cost-schedule integration** | Measuring both from one `EV` so they cannot contradict. |
| **`SPI` convergence** | The tendency of `SPI` → 1 at completion regardless of lateness. |
| **Earned schedule (`ES`)** | Earned value expressed as a point on the time axis; gives `SV(t)`, `SPI(t)`. |
| **AgileEVM** | Earned value applied to variable-scope adaptive delivery (Domain 9). |

### Sample MCQs — KA 6.4

**MCQ 6.4-A `[6.4.2 · Analysis]`** Why is `SPI` misleading late in a project?
- A. It is measured in time units.
- B. It converges to 1 at completion because all planned value is eventually earned, even if late. ✅
- C. It ignores actual cost.
- D. It cannot be computed after 50 % complete.

*Rationale:* At completion `EV = PV = BAC`, so `SPI = 1` regardless of lateness — hence its late-project
weakness, which earned schedule addresses. `SPI` is in *cost* units (not time), and is computable throughout.

**MCQ 6.4-B `[6.4.3 · Recall]`** Earned schedule improves on `SV`/`SPI` by expressing progress in:
- A. Cost.
- B. Time. ✅
- C. Units of work.
- D. Risk exposure.

*Rationale:* Earned schedule converts `EV` to a point on the time axis, giving time-based `SV(t)`/`SPI(t)`
that do not converge to 1 at the end.

**MCQ 6.4-C `[6.4.2 · Analysis]`** A project shows `SPI` = 1.02 overall, yet a critical-path activity is
slipping. This illustrates that:
- A. `SPI` always detects critical-path slippage.
- B. EVM does not see the critical path; it must be read with critical-path analysis. ✅
- C. The `EV` must be wrong.
- D. The project is definitely on time.

*Rationale:* Aggregate `SPI` can be healthy while a critical activity slips, because EVM does not model logic/
critical path — it must accompany CPM (Domain 10). It does not prove an `EV` error or on-time completion.

**MCQ 6.4-D `[6.4.3 · Application]`** At the end of Month 8 (`AT` = 8), `EV` = 440,000. The baseline shows
cumulative `PV` of 400,000 at Month 6 and 480,000 at Month 7. `ES` and `SV(t)` are:
- A. `ES` = 6.5 months; `SV(t)` = (1.5) months ✅
- B. `ES` = 6.5 months; `SV(t)` = +1.5 months
- C. `ES` = 7.0 months; `SV(t)` = (1.0) month
- D. `ES` = 6.0 months; `SV(t)` = (2.0) months

*Rationale:* `ES = 6 + (440,000 − 400,000)/(480,000 − 400,000) = 6 + 40,000/80,000 = 6.5` months; `SV(t) = ES
− AT = 6.5 − 8 = (1.5)` months behind. B has the wrong sign; C rounds `ES` up to the next month; D omits the
interpolation entirely.

**MCQ 6.4-E `[6.4.5 · Analysis]`** A programme's `CPI` moves from 0.96 at Month 3 to 0.91 at Month 6, with
neither value catastrophic in isolation. The strongest warning signal is:
- A. The Month 6 `CPI` level of 0.91 on its own.
- B. The deteriorating period-on-period trend, which warrants escalation before a single bad month arrives. ✅
- C. Nothing — both values round to about 1.
- D. The Month 3 `CPI`, because earlier data is always more reliable.

*Rationale:* A `CPI` sliding period on period matters more than its level — the trend is the earlier, stronger
warning, prompting escalation and a specific recovery action. A and D each fixate on a single reading; C
dismisses a real ~5-point deterioration.

### Self-check — KA 6.4

1. Give two limitations of EVM and how each is mitigated. *(Schedule indices time-blind/converge → earned
   schedule; no critical-path view → read with CPM; optimistic `EV` → objective earning rules; data quality →
   Domain 5 discipline.)*
2. What does earned schedule measure that `SV` does not? *(Schedule performance in *time* units that stays
   meaningful to completion.)*

---

## Advanced topics — Domain 6

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 6.A.1 — Management reserve and the EVM baseline

**The principle.** **Management reserve (MR)** sits **outside** the performance measurement baseline
(Domain 3, KA 3.1.4): `BAC` is the sum of the control-account budgets, any undistributed budget and the
contingency reserve — MR is above that line, held by management for unknown-unknowns. Two consequences
follow. First, **variances are never computed against MR**: `CV`, `CPI` and `VAC` measure performance
against the baseline the project committed to, and quietly padding the comparator with reserve would make
every index a flattering lie. Second, **using MR re-baselines**: releasing reserve into the PMB is a formal
baseline change — logged, authorised, dated — after which `BAC` rises and every subsequent `PV`, `TCPI` and
`VAC` is read against the new figure. A reserve release is a decision, not an adjustment.

**The formal discipline.** On large programmes the baseline stays auditable through two mechanisms. The
**undistributed budget (UB)** holds budget for authorised scope not yet assigned to control accounts — a
temporary parking place, drawn down as work is defined, so that authorised scope never floats outside the
baseline unaccounted for. **Control-account budget logs** record every movement into and out of each
control account — scope transfers, contingency draw-downs, authorised changes — so that at any data date
the sum of control-account budgets plus UB plus remaining contingency reconciles exactly to the original
`BAC` plus authorised changes. Retroactive changes to the budgets of open or completed work packages are
prohibited except to correct errors: rewriting history rewrites `PV`, and with it every variance ever
reported.

**The failure mode** is the *rubber baseline* — repeated re-baselining that resets variances to zero and
launders a deteriorating `CPI` into a fresh start. The log discipline exists precisely so that a reviewer
can see how many times the baseline moved, by how much, and on whose authority.

### Advanced 6.A.2 — EVM lite versus formal EVMS

**The principle.** A full **earned-value management system (EVMS)** — formal system guidelines and
criteria, documented procedures, independent surveillance, integrated baseline reviews — is the apparatus
of major government programmes, where a public funder mandates demonstrable compliance (the world of KA
6.4.5's defence programme and this domain's highways case). Most commercial projects need the *method*
without the *apparatus*: a scaled **'EVM-lite'** with fewer, larger control accounts (Domain 5, KA 5.3);
deliberately simple earning rules — 0/100, 50/50 and units completed in preference to subjective
percent-complete (6.1.2); a monthly cadence aligned to the ledger close so `AC` arrives accrual-complete;
and a report built around a handful of measures — `CPI`, `SPI`, `EAC`, `TCPI` — rather than a full formal
data set.

**What can be scaled away** is ceremony and granularity: the number of control accounts, the depth of the
reporting formats, the independent surveillance function. **What cannot be scaled away** are the three
things that make the arithmetic mean anything: **earning rules fixed in advance** per work package —
otherwise `EV` is negotiable and every index inherits the optimism (6.1.2); **a controlled baseline** —
otherwise `PV` is whatever this month's plan says and `SV` measures nothing (Domain 3; Advanced 6.A.1); and
**honest `AC`**, complete with accruals — otherwise `CPI` is computed on an understated denominator, the
classic flattering failure (Domain 5, KA 5.2). Strip those and the formulae still compute — EVM fails
politely, producing plausible numbers about nothing.

**The professional judgement** is proportionality: matching the weight of the system to the size and risk
of the project, while refusing to trade away the three invariants. A small project run on ten control
accounts and 0/100 rules can be more honest than a large one drowning in unverified percent-complete.

### Advanced 6.A.3 — The percent-complete plateau and 'watermelon' reporting

**The pattern.** Many projects race to "90 % complete" and then stay there for months — the
**percent-complete plateau**. The tail work (punch lists, testing, commissioning documentation, closeout)
consumes time and cost while little budgeted value remains to earn, so reported progress barely moves. The
cost-based `SPI` is at its blindest here — converging to 1 regardless of lateness (6.4.2) — and subjective
percent-complete claims are at their most optimistic, because "nearly done" is the easiest claim to make
and the hardest to falsify. The companion pathology is **'watermelon' reporting**: green on the outside,
red on the inside — RAG statuses that stay green while the underlying indices slide, usually because status
is self-assessed and no one reconciles the colour to the numbers.

**The countermeasures** are all applications of this domain's own discipline:

- **0/100 or milestone rules for closing work packages** (6.1.2) — nothing is allowed to sit at "95 %"
  indefinitely; the final tranche of value is earned only on verified completion, so the plateau becomes
  visible as an `EV` curve that has genuinely stalled.
- **Quantity-based `EV` wherever the work allows** — units completed against budget resist optimism in a
  way assessed percentages do not.
- **Trend review** (Domain 4, KA 4.1.2) — an `EV` curve flat across successive periods while `AC` climbs is
  a leading indicator of a troubled tail, and a status that stays green against a sliding `CPI`/`SPI` trend
  is the watermelon signature; the reconciliation of narrative to numbers is a standing check.
- **Separate tracking of punch-list and closeout effort** — giving the tail its own control account or work
  packages, with their own budget and earning rules, so closeout is measured work rather than an untracked
  residue smeared across finished accounts.

The through-line: the plateau is not a measurement curiosity, it is where inflated `EV` and unfunded tail
effort hide. The professional interrogates a long-standing "nearly done" as hard as any overrun (6.2.4).

### Advanced 6.A.4 — Earned schedule beyond the basics

**`SPI(t)` as a trend.** A single `SPI(t)` reading is a position; the **trend across periods** is the
signal — exactly the logic of the deteriorating `CPI` in KA 6.4.5. Because `SPI(t)` does not converge to 1
at completion (6.4.3), its trend stays meaningful through the back half of a project, when the cost-based
`SPI` has gone quiet: an `SPI(t)` sliding period on period late in a programme is reporting a real,
worsening delay that the conventional index can no longer see.

**Forecasting the completion date.** Earned schedule extends naturally from measuring lateness to
forecasting it: as a first cut, forecast duration = planned duration / `SPI(t)` — the time-domain analogue
of `EAC = BAC / CPI`.

1. **Setup.** The master project: planned duration **10 months**; at Month 5, `ES` = 4.75 months and
   `SPI(t)` = **0.95** (6.4.3b).
2. **Formula.** `Forecast duration = planned duration / SPI(t)`.
3. **Substitution.** `10 / 0.95 = 10.53`.
4. **Result.** Forecast duration ≈ **10.5 months** — roughly half a month late.
5. **Interpretation.** The forecast assumes the schedule performance achieved to date **persists uniformly**
   across the remaining work — the same assumption family as the `EAC` methods (6.3.2), and it must be
   stated with the number. If the remaining work differs in character, or a recovery action is genuinely
   funded and resourced, the extrapolation is the wrong basis, just as method (b) is the wrong `EAC` for an
   atypical variance.

**The standing limit.** Earned schedule aggregates all work into one time measure; it still **cannot see
the critical path**. A healthy `SPI(t)` can coexist with a critical activity slipping while float-rich work
runs ahead — the same blindness as the cost-based indices (6.4.2). The completion date is determined on the
network, through schedule progressing and baseline comparison (Domain 10, KA 10.4.2), which is why the case
study cross-checks its earned-schedule read against the critical path before believing it. Earned schedule
**complements — never replaces — CPM**: it prices and trends the drift the network cannot quantify; the
network says whether the drift moves the end date.

---

## Case study — Domain 6: a full EVM cycle on a highways programme (government/infrastructure)

### Background

The Ministry of Transport has appropriated **`BAC` = USD 80,000,000** for a publicly funded highways
upgrade — widening a strategic corridor, replacing three structures and renewing the drainage and pavement
along 40 km of carriageway — on a **30-month schedule**. The programme is now reporting at **Month 12**, and
the controls team is running the full EVM cycle this domain teaches: the three measures (KA 6.1), the
variances and indices (KA 6.2), the `EAC` fan and its defended method selection (KA 6.3), and the
earned-schedule and critical-path reads that keep the schedule picture honest (KA 6.4).

The government context sharpens everything. The 80m is a **fixed appropriation**, voted and gazetted — not a
commercial budget that a board can quietly extend. On this programme an `EAC` above 80,000,000 is not just a
forecast; it is a **ministerial conversation**: a formal request for a funding uplift, a descoping decision, or
a recovery plan, each with public accountability attached. The controls professional's numbers will be read
by people who did not build them and cannot re-derive them — so every figure must arrive with its assumption
stated and its arithmetic checkable (Domain 4, KA 4.3.3).

### The three measures at Month 12 (KA 6.1)

At the Month 12 data date the three measures, all in the same budget currency, are:

| Measure | Value (USD) | Source and basis |
|---|---:|---|
| `PV` | 32,000,000 | The cost-baseline S-curve (Domain 3, KA 3.3) — the budgeted cost of work scheduled by Month 12 |
| `EV` | 28,800,000 | Earned **bottom-up** at control-account level under earning rules fixed in advance (KA 6.1.2) |
| `AC` | 31,300,000 | The true cost-to-date from the ledger, **including month-end accruals** (Domain 5, KA 5.2) |

Each figure carries the discipline of an earlier domain. The `PV` of 32,000,000 is simply the baseline read at
Month 12 — no judgement involved, provided the baseline is under change control (Domain 3). The `EV` of
28,800,000 is the sum of the control accounts, each earning under its own pre-agreed rule: units completed
for the earthworks (cubic metres moved against budget), weighted milestones for the structures, percent
complete with an objective quantity basis for the drainage. No account was allowed to claim subjective
progress — the defence against optimistic `EV` that flatters both indices (KA 6.1.2). The `AC` of 31,300,000
includes the month-end accruals for work done but not yet invoiced — without them, `AC` would be understated
and the `CPI` flattered, the classic data-quality failure Domain 5 exists to prevent. At a glance, the
integrated picture (KA 6.1.3) is already uncomfortable: `EV` sits below `PV` (behind schedule) **and** below
`AC` (over cost).

### Variances and indices (KA 6.2)

- **Setup.** `PV` = 32,000,000; `EV` = 28,800,000; `AC` = 31,300,000; `BAC` = 80,000,000.
- **Formula.** `CV = EV − AC`; `SV = EV − PV`; `CPI = EV / AC`; `SPI = EV / PV`;
  `TCPI (to BAC) = (BAC − EV) / (BAC − AC)`.
- **Substitution.** `CV = 28,800,000 − 31,300,000`; `SV = 28,800,000 − 32,000,000`;
  `CPI = 28,800,000 / 31,300,000`; `SPI = 28,800,000 / 32,000,000`;
  `TCPI = (80,000,000 − 28,800,000) / (80,000,000 − 31,300,000) = 51,200,000 / 48,700,000`.
- **Result.** `CV` = **(USD 2,500,000)**; `SV` = **(USD 3,200,000)**; `CPI` = **0.92**; `SPI` = **0.90**;
  `TCPI (to BAC)` = **1.05**.
- **Interpretation.** The programme is **over cost and behind schedule** — the most serious quadrant of the
  KA 6.2.4 table. For every dollar spent, USD 0.92 of budgeted value is being earned; work is progressing at
  90 % of the planned rate. The `TCPI` is the reality check: to finish within the 80,000,000 appropriation,
  the remaining work must run at a cost efficiency of **1.05** against the **0.92** actually achieved — a
  swing of roughly **14 %**, sustained across eighteen months of remaining work. That is not impossible, but
  it does not happen by drift or by hoping the average improves; it happens only through a **specific,
  credible intervention** with a named cause, an owner and a date (compare KA 6.4.5). Absent such an
  intervention, the honest position is that the `BAC` is no longer a credible forecast of the outcome, and
  the analysis must move to the `EAC` family.

### The EAC fan (KA 6.3)

The team computes the three formula methods on the identity `EAC = AC + ETC` (KA 6.3.1–6.3.2), each encoding
a different assumption about the remaining work:

| Method | Assumption | Computation | `EAC` (USD) |
|---|---|---|---:|
| (a) budgeted rate | Month 1–12 variance was atypical; remainder runs to budget | `31,300,000 + (80,000,000 − 28,800,000) = 31,300,000 + 51,200,000` | **82,500,000** |
| (b) current `CPI` | the cost inefficiency persists | `80,000,000 / 0.9201` | **≈ 86,940,000** |
| (c) `CPI × SPI` | schedule drag compounds the cost inefficiency | `31,300,000 + 51,200,000 / (0.9201 × 0.90) = 31,300,000 + 51,200,000 / 0.8281 ≈ 31,300,000 + 61,830,000` | **≈ 93,130,000** |

The fan runs from **82.5m to 93.1m** — a spread of more than USD 10,000,000 that is not imprecision but
**three different assumptions** about the work ahead. The professional's job (KA 6.3.3) is to select the
assumption that matches the **cause** of the variance, and to defend the selection.

The variance analysis (Domain 4, KA 4.2) traces the overrun to two drivers. First, a **systemic productivity
shortfall on the earthworks**: haul distances are longer than the estimate assumed and ground conditions have
been consistently poorer than the site investigation indicated, so the units-completed control accounts have
been earning below their budgeted rate month after month — a **persisting** cause, and the earthworks continue
for another year. Second, a **schedule-driven prolongation element**: the delay is extending time-related
preliminaries (site establishment, traffic management, supervision), so being behind schedule is itself adding
cost. On that causal analysis, method (a) is not defensible — nothing about the variance is a closed, one-off
event. Method (b), the persisting-`CPI` forecast, is defended as the **central case**: **`EAC` ≈ USD 86.9m**.
Method (c) is reported alongside it as the **downside scenario** — the outcome if the prolongation element is
not arrested and schedule drag continues to compound the cost inefficiency. The variance at completion on the
central case:

`VAC = BAC − EAC = 80,000,000 − 86,940,000 = ` **(USD 6,940,000)** — a projected overrun of just under 9 %
against the appropriation.

Note what the team did *not* do: it did not pick the lowest number because it was the most comfortable, and it
did not run one formula mechanically because it always has. It matched method to cause, kept the alternatives
visible, and checked the result against the `TCPI` — an `EAC` of 86.9m is exactly what a required 1.05
against an achieved 0.92 was already signalling.

### Earned schedule (KA 6.4.3)

The `SV` of (3,200,000) and `SPI` of 0.90 state the schedule position in **currency**, and — as KA 6.4.2
warns — the cost-based `SPI` will drift back towards 1.0 as the programme completes, whatever happens. The
team therefore also computes the position in **time**. The baseline S-curve shows cumulative `PV` of
**26,000,000 at Month 10** and **29,000,000 at Month 11** — so the plan expected the current `EV` of
28,800,000 to have been earned between Months 10 and 11.

- **Setup.** Actual time `AT` = 12 months; `EV` = 28,800,000; cumulative `PV` = 26,000,000 at Month 10 and
  29,000,000 at Month 11.
- **Formula.** Interpolate `ES` between the bracketing months; then `SV(t) = ES − AT` and `SPI(t) = ES / AT`.
- **Substitution.** `ES = 10 + (28,800,000 − 26,000,000) / (29,000,000 − 26,000,000) = 10 + 2,800,000 /
  3,000,000 = 10 + 0.93`.
- **Result.** `ES` = **10.93 months**; `SV(t) = 10.93 − 12 = ` **(1.07) months**; `SPI(t) = 10.93 / 12 = `
  **0.91**.
- **Interpretation.** The programme is about **a month behind in time terms** — the work done by Month 12 is
  the work the baseline expected shortly before Month 11. On a 30-month schedule that is material, and unlike
  the cost-based `SPI`, `SPI(t)` **stays honest to the end** (KA 6.4.2–6.4.3): it will keep reporting the
  lateness meaningfully through the back half of the programme, when the cost-based index would be converging
  to 1.0 and quietly going blind.

### The critical-path cross-check (KA 6.4.2)

Earned schedule says the programme is about a month behind *in aggregate* — but EVM does not see the critical
path, and an aggregate delay can be noise if it sits on activities with float. The schedule team therefore
cross-checks against the network (Domain 10), and confirms that the slippage sits **on the critical path**:
the structures works have missed their planned possession dates, and the possessions drive the corridor
opening. The one-month aggregate delay is therefore a real one-month threat to completion, not non-critical
work running late around a healthy spine. This is the two-way discipline KA 6.4.2 requires — EVM read **with**
the network, each covering the other's blind spot: earned value quantifies the size and cost of the drift the
network cannot price, and the network confirms whether the drift matters to the end date, which the aggregate
indices cannot tell.

### The governance conversation (government angle)

The Month 12 report now goes up the line with a central **`EAC` ≈ USD 86.9m against the USD 80m
appropriation**. Because the appropriation is fixed, the (6,940,000) `VAC` is not an internal reforecast — it
forces a decision, and the report frames the three options honestly:

1. **A funding uplift request** of approximately USD 6.9m — politically costly, slow, and requiring the
   programme to show that the forecast is robust and the causes understood, not simply that money ran out.
2. **Descoping a later phase** — deferring, say, the final pavement-renewal sections to a future programme,
   bringing the remaining scope's cost inside the appropriation, with the trade-offs made explicit.
3. **A recovery plan targeting the earthworks productivity driver** — revised haul routes, re-sequenced cut
   and fill, additional plant — which carries its **own cost and its own risk**, and whose case must be tested
   against the `TCPI`: recovery means sustaining a remaining-work efficiency near 1.05 against 0.92 achieved,
   so the plan must credibly explain *what changes* to produce a ~14 % swing.

The controls professional does not make that choice — ministers and the programme board do. The professional's
contribution is what this domain has built: the **honest fan** (82.5m / 86.9m / 93.1m) rather than a single
false-precision number; the **defended assumption** behind the 86.9m central case (persisting earthworks
productivity, traced to cause); the **`TCPI` reality check** that quantifies what recovery would actually
demand; and the **earned-schedule time picture**, cross-checked against the critical path, so the schedule
story is stated in months as well as dollars. Decision-ready, no spin (Domain 4, KA 4.3.3): the likely
outcome, the assumption behind it, the alternatives, and why the original figure no longer holds — delivered
at Month 12, early enough for every option to still be open.

### What the credential expects

This case study is the whole of Domain 6 run once, end to end. From **KA 6.1**, the three measures — a `PV`
read from a controlled baseline, an `EV` earned bottom-up under fixed earning rules, an `AC` complete with
accruals from **Domain 5's data layer**, because a `CPI` computed on an understated `AC` is a flattering lie.
From **KA 6.2**, the variances and indices read together, and the `TCPI` used as the quantified test of
whether the `BAC` remains credible. From **KA 6.3**, the `EAC` fan computed in full, the method **selected to
match the cause** and defended, and the `VAC` stated against the target that matters — here, a fixed public
appropriation. From **KA 6.4**, the earned-schedule translation into time and the critical-path cross-check
that keeps the aggregate indices honest. And from **Domain 4**, the reporting standard that turns the
arithmetic into a decision: options, assumptions, and consequences, not just numbers. A predictive `EAC` model
(KA 13.5.3) would likely have surfaced the earthworks drift a month or two earlier from the control-account
trend — a genuine advantage — but the assumption behind the forecast, and the conversation with the ministry,
remain the professional's to own and defend.

---

## Executive perspective — Domain 6

**What the executive must hold onto.** `EV` — physical progress valued at budget, never at actual cost —
is the one honest progress currency, and it is only as honest as the earning rules and progress claims
behind it (KA 6.1). An `EAC` is not a prediction handed down; it is **an assumption made visible** — each
method encodes a different view of the remaining work, and the professional's job is to defend the
assumption, not the arithmetic (KA 6.3). And the `TCPI` is the board's credibility test: when the
efficiency the remaining work must achieve diverges sharply from the `CPI` actually being delivered, the
budget has stopped being credible and the honest number is a higher `EAC`.

**Six questions to ask from the chair.**

1. What earning rules produced this `EV`, and who verified the progress claims behind it?
2. Does the `AC` include accruals, or are we computing `CPI` on an understated cost?
3. Which `EAC` method is this, what assumption does it encode, and why does that assumption match the
   *cause* of the variance?
4. What `TCPI` does holding the `BAC` now require, and is that efficiency credible against the `CPI`
   achieved to date?
5. Is the slippage behind that `SPI` on the critical path, or non-critical work running late around a
   healthy spine?
6. What is the `CPI` trend over the last three periods — stable, or sliding?

**The traps at board level.**

- **Aggregate `SPI` hiding a critical-path slip.** EVM does not see the critical path, and the cost-based
  `SPI` drifts to 1.0 as any project — however late — completes; insist on the network cross-check and the
  earned-schedule view in time units (KA 6.4).
- **Optimistic `EV`.** Inflated progress claims flatter every variance, index and forecast at once; a
  `CPI` and `SPI` both comfortably above 1 deserve as much interrogation as a bad result.
- **The single confident number.** A lone `EAC` with no method stated is false precision; the spread of the
  fan is not imprecision but different assumptions about the remaining work, and the board should see it.
- **Treating `VAC` as an internal reforecast.** Against a fixed appropriation or funding envelope, a
  negative `VAC` is not a bookkeeping update — it forces a decision on funding, descope or recovery, and
  a recovery plan must answer to the `TCPI`.

**What good looks like.** The monthly pack carries `PV`, `EV` and `AC` from controlled baselines, fixed
earning rules and an accrual-complete ledger, so nobody in the room is debating whether the inputs are
true. Forecasts arrive as a fan with the central case defended — method, assumption, cause — and the
`TCPI` is quoted whenever anyone proposes to hold the original budget. Schedule health is stated in months
as well as currency, cross-checked against the critical path. Above all, a deteriorating trend surfaces
while every option is still open, because the organisation treats a rising `EAC` as information to act on,
not an admission to defer.

---

## Calculation exercises — Domain 6

Work each exercise before reading its solution; every step uses only this domain's methods.

**Exercise 6.1** — A project has four work packages, each earning under a rule fixed in advance. Package A
(budget **USD 200,000**, 0/100) is complete. Package B (budget **USD 300,000**, 50/50) has started but not
finished. Package C (budget **USD 400,000**, units completed) has done **450 of 600 units**. Package D
(budget **USD 100,000**, weighted milestones) has reached its 30 % milestone. Aggregated `PV` is **USD
800,000** and aggregated `AC` is **USD 850,000**. Compute each package's `EV`, the project `EV`, and the
project `CPI` and `SPI`.

**Solution 6.1.**
1. A (0/100, complete): `EV` = **200,000**.
2. B (50/50, started): `EV = 0.50 × 300,000 = ` **150,000**.
3. C (units): `EV = (450 / 600) × 400,000 = 0.75 × 400,000 = ` **300,000**.
4. D (milestones): `EV = 0.30 × 100,000 = ` **30,000**.
5. Project `EV = 200,000 + 150,000 + 300,000 + 30,000 = ` **USD 680,000**.
6. `CPI = EV / AC = 680,000 / 850,000 = ` **0.80**; `SPI = EV / PV = 680,000 / 800,000 = ` **0.85** — a
   rollup of package-level earning, over cost and behind schedule (6.1.2b).

**Exercise 6.2** — A programme has `BAC` = **USD 5,000,000**. At the data date, `EV` = **2,000,000**, `AC` =
**2,500,000**, `PV` = **2,200,000**. Compute `CV`, `SV`, `CPI`, `SPI` and the `TCPI` to meet the `BAC`, and
judge whether the `BAC` remains credible.

**Solution 6.2.**
1. `CV = EV − AC = 2,000,000 − 2,500,000 = ` **(USD 500,000)** (over cost).
2. `SV = EV − PV = 2,000,000 − 2,200,000 = ` **(USD 200,000)** (behind schedule).
3. `CPI = EV / AC = 2,000,000 / 2,500,000 = ` **0.80**; `SPI = EV / PV = 2,000,000 / 2,200,000 = ` **0.91**.
4. `TCPI (to BAC) = (BAC − EV) / (BAC − AC) = (5,000,000 − 2,000,000) / (5,000,000 − 2,500,000) =
   3,000,000 / 2,500,000 = ` **1.20**.
5. Judgement: the remaining work would have to run at **1.20** against **0.80** achieved — a 50 %
   efficiency swing, sustained. Absent a specific, credible intervention, the `BAC` is no longer credible
   and the honest position is an `EAC > BAC` (6.2.3).

**Exercise 6.3** — A project has `BAC` = **USD 3,000,000**; at the data date `EV` = **1,200,000**, `AC` =
**1,500,000**, `PV` = **1,250,000**. The remaining work is dominated by a commissioning phase quite unlike
the civils performed to date, and a fresh bottom-up estimate of that remainder gives `ETC` = **USD
2,100,000**. Compute the three formula EACs and the bottom-up `EAC`, select one, defend the selection, and
state `VAC` on the selected forecast.

**Solution 6.3.**
1. `CPI = 1,200,000 / 1,500,000 = ` **0.80**; `SPI = 1,200,000 / 1,250,000 = ` **0.96**.
2. (a) `EAC = AC + (BAC − EV) = 1,500,000 + 1,800,000 = ` **USD 3,300,000**.
3. (b) `EAC = BAC / CPI = 3,000,000 / 0.80 = ` **USD 3,750,000**.
4. (c) `EAC = AC + (BAC − EV) / (CPI × SPI) = 1,500,000 + 1,800,000 / 0.768 = 1,500,000 + 2,343,750 = `
   **USD 3,843,750**.
5. (d) `EAC = AC + ETC = 1,500,000 + 2,100,000 = ` **USD 3,600,000**.
6. Select **(d)**: performance to date is not representative of a commissioning-dominated remainder, so the
   formula methods extrapolate the wrong work (6.3.3); the re-estimate happens to sit inside the fan
   (3,300,000–3,843,750). `VAC = BAC − EAC = 3,000,000 − 3,600,000 = ` **(USD 600,000)**.

**Exercise 6.4** — A project's baseline shows cumulative `PV` by month (USD 000): **150, 340, 560, 800,
1,000, 1,150** for Months 1–6. At the end of **Month 5** (`AT` = 5), measurement gives `EV` = **USD
620,000**. Compute the earned schedule `ES`, then `SV(t)` and `SPI(t)`, and compare `SPI(t)` with the
cost-based `SPI`.

**Solution 6.4.**
1. Bracket `EV` = 620,000 on the baseline: cumulative `PV` is 560,000 at Month 3 and 800,000 at Month 4, so
   `ES` lies between Months 3 and 4.
2. Interpolate: `ES = 3 + (620,000 − 560,000) / (800,000 − 560,000) = 3 + 60,000 / 240,000 = 3 + 0.25 = `
   **3.25 months**.
3. `SV(t) = ES − AT = 3.25 − 5 = ` **(1.75) months** — a month and three quarters behind in time terms.
4. `SPI(t) = ES / AT = 3.25 / 5 = ` **0.65**.
5. Cost-based `SPI = EV / PV = 620,000 / 1,000,000 = ` **0.62** — a similar signal today, but only `SPI(t)`
   will keep reporting the lateness meaningfully as the project completes (6.4.3).

**Exercise 6.5** — A project with `BAC` = **USD 4,000,000** reports two successive periods. **Month 4:**
`EV` = 900,000, `AC` = 1,000,000, `PV` = 960,000. **Month 5:** `EV` = 1,360,000, `AC` = 1,600,000, `PV` =
1,700,000. Compute `CPI` and `SPI` for both periods, state the trend, recompute `EAC = BAC / CPI` at each
data date, and state the movement in the forecast and the Month 5 `VAC`.

**Solution 6.5.**
1. Month 4: `CPI = 900,000 / 1,000,000 = ` **0.90**; `SPI = 900,000 / 960,000 = ` **0.94** (0.9375).
2. Month 5: `CPI = 1,360,000 / 1,600,000 = ` **0.85**; `SPI = 1,360,000 / 1,700,000 = ` **0.80**.
3. Trend: both indices are **deteriorating** (`CPI` 0.90 → 0.85; `SPI` 0.94 → 0.80) — two consecutive
   readings moving the same way, a stronger warning than either level alone (6.4.5).
4. Month 4 forecast: `EAC = 4,000,000 / 0.90 ≈ ` **USD 4,444,444**.
5. Month 5 forecast: `EAC = 4,000,000 / 0.85 ≈ ` **USD 4,705,882** — a rise of **≈ USD 261,438** in one
   period. `VAC = 4,000,000 − 4,705,882 = ` **(USD 705,882)**; the trend, not the level, is the escalation
   trigger.

---

## Domain 6 summary

Earned value integrates three measures in one currency — **`PV`** (planned), **`EV`** (performed, valued at
budget) and **`AC`** (actual) — with `EV` measured by a fixed earning rule to resist optimism. From them come
the variances (`CV = EV − AC`, `SV = EV − PV`) and dimensionless indices (`CPI = EV/AC`, `SPI = EV/PV`) that
compare across projects, and the `TCPI` reality check on whether a target remains credible. Forecasting is the
heart of the domain: `EAC = AC + ETC`, with `ETC` derived at the budgeted rate, the current `CPI`, the
`CPI × SPI` drag, or a fresh bottom-up estimate — the professional selecting the assumption that matches the
variance's cause and reporting `EAC`/`VAC` decision-ready. EVM's power is cost-schedule integration; its limits
— time-blind schedule indices, no critical-path view, dependence on honest `EV` and clean data — are addressed
by earned schedule, critical-path analysis (Domain 10) and the discipline of Domains 4–5. The notation
established here is the language reused for adaptive delivery in Domain 9.

**Cross-references.** The Planned Value baseline → 3.3; the EAC family introduced → 3.4; true cost-to-date and
control accounts → 5.2–5.3; variance reading and reporting → 4.2–4.3; revenue recognition that EAC feeds →
2.2.6; critical path → Domain 10; AgileEVM → 9.5; predictive EAC and driver analysis → 13.5.

*Domain 6 is a first authored draft pending SME technical review before it feeds the exam blueprint.*
