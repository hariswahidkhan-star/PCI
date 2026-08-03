# Domain 4 — Performance Management, Variance Analysis & Management Reporting

## Why this domain exists

Measuring cost and revenue correctly (Domains 1–2) and planning them (Domain 3) is only worth doing if the
results **change a decision**. This domain is about turning numbers into managed performance: the principles
of performance management — KPIs, targets, thresholds and the difference between leading and lagging
indicators (KA 4.1); **variance analysis**, which decomposes the gap between plan and actual into its
*causes* — price, quantity, rate, efficiency — so the right lever is pulled (KA 4.2); **management reporting**
— designing the report and the project-controls dashboard for the audience and the decision (KA 4.3); and
**data visualisation and storytelling**, presenting controls data so it informs rather than distorts (KA
4.4). It closes the finance group and hands directly to the earned-value machinery of Domain 6, of which
cost/schedule variance is a special case.

**Learning objectives.** After this domain a candidate can: design KPIs with targets, thresholds and
tolerances and distinguish leading from lagging indicators; decompose a total cost variance into price/rate
and quantity/efficiency components and build a variance bridge from budget to actual; design an
audience-appropriate management report and exception report; and select and present visualisations that
inform decisions and avoid common distortions.

---

## Knowledge Area 4.1 — Performance management principles

*Topics: 4.1.1 KPIs, targets, thresholds and tolerances · 4.1.2 leading vs lagging indicators · 4.1.3
management by exception.*

### 4.1.1 KPIs, targets, thresholds and tolerances

**Definition & purpose.** A **key performance indicator (KPI)** is a measure chosen because it reflects
something that matters to an objective. A KPI is useful only when paired with a **target** (the value aimed
at), **thresholds** (the boundaries that trigger attention — often a red/amber/green band), and a
**tolerance** (the allowable deviation before action is required). "`CPI` = 0.97" means nothing until you
know the target is 1.00, the amber threshold is 0.95 and the tolerance is ±0.05 — at which point 0.97 is
*within tolerance* and needs watching, not escalation. Good KPI design resists two failure modes: **too many**
indicators (noise, no focus) and **gameable** ones (measures that improve on paper without improving
reality).

**Worked example 4.1.1 — setting thresholds from the data, not from habit.**

1. **Setup.** A portfolio's monthly `CPI` readings over two stable years average **1.00**, with observed
   variability of about **±0.03** in a normal month.
2. **Formula.** Set the amber threshold one variability-step from target and red at two steps:
   `amber = target − 1 step`; `red = target − 2 steps`.
3. **Substitution.** Amber `= 1.00 − 0.03 = 0.97`; red `= 1.00 − 0.06 = 0.94`.
4. **Result.** A new reading of **0.95** lands between amber (0.97) and red (0.94) → investigate with
   priority, but do not treat as red.
5. **Interpretation.** Thresholds derived from the measure's *own* normal variability separate signal from
   noise: a ±0.05 band chosen by habit would have called 0.95 "within tolerance" and missed the early
   signal, while a ±0.01 band would alarm monthly on noise — the twin failure modes of 4.1.3, made
   quantitative. The same discipline sets tolerances per control account, not one band for all.

### 4.1.2 Leading versus lagging indicators

**The principle.**

- **Lagging indicators** measure **outcomes that have already happened** — `CPI`, `SPI`, actual cost, schedule
  slippage, incidents. They are reliable but *late*: by the time `CPI` falls, the cost is spent.
- **Leading indicators** measure **things that predict** future outcomes — productivity trend, RFI turnaround,
  design-completion rate, near-misses, resource availability. They are earlier but *noisier*.

A controls professional balances both: leading indicators to **intervene in time**, lagging indicators to
**confirm** whether the intervention worked. Reporting only lagging measures guarantees you are always
managing the past.

### 4.1.3 Management by exception

**Definition & purpose.** **Management by exception** focuses attention on the items **outside tolerance**,
rather than reviewing everything equally. Control accounts within their thresholds are noted green and left
alone; those breaching amber/red get the narrative, the root-cause analysis and the action. This concentrates
scarce management attention where it changes outcomes, and is the organising principle of the exception report
(4.3). It depends entirely on thresholds being set sensibly — too tight and everything is an exception (noise);
too loose and real problems hide inside tolerance.

### Key terms — KA 4.1

| Term | Meaning |
|---|---|
| **KPI** | A measure chosen to reflect progress toward an objective. |
| **Target / threshold / tolerance** | The aim / the attention boundary / the allowable deviation. |
| **Lagging indicator** | Measures an outcome already realised (e.g. `CPI`). |
| **Leading indicator** | Measures a predictor of a future outcome (e.g. productivity trend). |
| **Management by exception** | Focusing on items outside tolerance. |

### Sample MCQs — KA 4.1

**MCQ 4.1-A `[4.1.2 · Analysis]`** Which is a *leading* indicator for project cost performance?
- A. Cost performance index (`CPI`) to date.
- B. Actual cost incurred.
- C. Weekly installed-quantity productivity trend. ✅
- D. Final cost variance at completion.

*Rationale:* A productivity trend predicts future cost outcomes — a leading indicator. `CPI`, actual cost and
final variance are all lagging (already-realised) measures.

**MCQ 4.1-B `[4.1.1 · Application]`** A KPI reads `CPI` = 0.97 against a target of 1.00, amber threshold 0.95,
tolerance ±0.05. The correct status is:
- A. Red — below target.
- B. Cannot be assessed without the schedule.
- C. Green — exactly on target.
- D. Within tolerance (green/watch) — 0.97 is above the 0.95 amber threshold. ✅

*Rationale:* 0.97 is below the 1.00 target but above the 0.95 amber threshold and within the ±0.05 tolerance —
watch, not escalate. It is not red, not on target, and can be assessed from the cost KPI alone.

**MCQ 4.1-C `[4.1.3 · Recall]`** Management by exception means that management attention is focused on:
- A. The items outside their tolerance, while in-tolerance items are noted and left alone. ✅
- B. Every control account equally, reviewed in full each period.
- C. Only the accounts reporting green.
- D. Lagging indicators in preference to leading ones.

*Rationale:* Management by exception concentrates scarce attention on out-of-tolerance items — the reds and
ambers get the narrative and the action. B dilutes attention across everything; C inverts the principle; D
confuses it with the indicator-type distinction.

**MCQ 4.1-D `[4.1.1 · Analysis]`** A team's reported KPI improves steadily while the underlying performance
it is meant to reflect does not. The most likely KPI design failure is:
- A. The indicator is gameable — it can improve on paper without reality improving. ✅
- B. Too few indicators are being reported.
- C. The tolerance is set too tight.
- D. The indicator is leading rather than lagging.

*Rationale:* A measure that moves without the underlying objective moving is the classic gameable-KPI
failure mode. B and C would produce noise or over-escalation, not a false improvement; D concerns timing of
the signal, not its integrity.

### Self-check — KA 4.1

1. Why report both leading and lagging indicators? *(Leading to intervene in time; lagging to confirm the
   intervention worked.)*
2. What two failure modes does threshold-setting risk? *(Too tight — everything is an exception; too loose —
   real problems hide within tolerance.)*

---

## Knowledge Area 4.2 — Variance analysis

*Topics: 4.2.1 what a variance is · 4.2.2 the flexed budget · 4.2.3 price/rate vs quantity/efficiency
variances · 4.2.4 the variance bridge · 4.2.5 favourable and adverse — reading them well.*

### 4.2.1 What a variance is

**Definition & purpose.** A **variance** is the difference between a planned (budgeted/standard) figure and an
actual one. A variance is **favourable (F)** when it improves profit (actual cost below plan, or actual
revenue above plan) and **adverse (A)** when it worsens it. The point of variance analysis is not to compute
the number — that is arithmetic — but to **attribute it to a cause** precise enough to act on: a cost overrun
because *rates* rose is a different problem, with different owners and remedies, from one because *more was
used* than planned.

### 4.2.2 The flexed budget

**The principle.** Before comparing budget to actual, the budget is **flexed** to the **actual level of
activity/output** — otherwise part of the "variance" is simply that more or less work was done. Comparing the
*flexed* budget (what the cost *should* have been for the output achieved) with actual isolates
**efficiency/price** effects from **volume** effects. Skipping the flex is a frequent error that blames a team
for a variance that is really a volume change.

### 4.2.3 Price/rate versus quantity/efficiency variances

**The decomposition.** A total cost variance for a resource splits cleanly into two:

```
Price (rate) variance      = (Actual price − Standard price) × Actual quantity
Quantity (usage) variance  = (Actual quantity − Standard quantity) × Standard price
Total variance             = Actual cost − Standard cost = Price variance + Quantity variance
```
- *Price/rate* variance isolates the effect of paying a different **unit price/rate** than planned.
- *Quantity/usage (efficiency)* variance isolates the effect of using a different **amount** than planned.

The same structure applies to labour, where "quantity" is hours: a **rate variance** (paid per hour) and an
**efficiency variance** (hours used for the output).

**Worked example 4.2.3 — decompose material and labour variances.**

1. **Setup.** For a work package's actual output:
   - **Material** — standard **1,000 units at USD 50** (= 50,000); actual **1,100 units at USD 52** (=
     57,200).
   - **Labour** — standard **2,000 hours at USD 40** (= 80,000); actual **2,100 hours at USD 42** (= 88,200).
2. **Formulae.** As above.
3. **Substitution & Result.**
   - Material price: `(52 − 50) × 1,100 = 2,200` **(A)**. Material quantity: `(1,100 − 1,000) × 50 = 5,000`
     **(A)**. Total material `= 57,200 − 50,000 = 7,200` **(A)** = `2,200 + 5,000`. ✓
   - Labour rate: `(42 − 40) × 2,100 = 4,200` **(A)**. Labour efficiency: `(2,100 − 2,000) × 40 = 4,000`
     **(A)**. Total labour `= 88,200 − 80,000 = 8,200` **(A)** = `4,200 + 4,000`. ✓
4. **Interpretation.** The overall **USD 15,400 adverse** cost variance is *not* one problem: USD 6,400 comes
   from paying more (material price + labour rate — a **market/procurement** issue), and USD 9,000 from using
   more (material usage + labour hours — a **productivity/wastage** issue). Those go to different owners with
   different remedies. That attribution — not the headline number — is the value of variance analysis.

### 4.2.4 The variance bridge

**Definition & purpose.** A **variance bridge** (or "bridge/waterfall") walks from the budgeted figure to the
actual, one variance component at a time, so a reader sees *what moved the number and by how much*. It is the
single most effective way to explain a result to a board.

**Worked example 4.2.4 — bridge from budget cost to actual cost.** Using 4.2.3 (budget total cost 130,000;
actual 145,400):

| Step | USD | Running total |
|---|---:|---:|
| **Budget cost** | | 130,000 |
| Material price | 2,200 (A) | 132,200 |
| Material quantity | 5,000 (A) | 137,200 |
| Labour rate | 4,200 (A) | 141,400 |
| Labour efficiency | 4,000 (A) | 145,400 |
| **Actual cost** | | **145,400** |

The bridge reconciles exactly (`130,000 + 2,200 + 5,000 + 4,200 + 4,000 = 145,400`), and it tells the *story*
the single "USD 15,400 over" figure hides.

> **Fig 4.2.1 — Budget-to-actual variance bridge.** *Caption:* what drove the USD 15,400 overrun. *Underlying
> data:* the bridge table above. *Render-ready description:* a waterfall chart — a starting bar "Budget
> 130,000", four rising red floating bars (Material price 2,200; Material qty 5,000; Labour rate 4,200;
> Labour efficiency 4,000), ending bar "Actual 145,400"; adverse steps in a warning colour, connectors
> between bars, values labelled. *Animation storyboard (digital-only):* each floating bar rises in sequence
> with its label, the running total ticking up from 130,000 to 145,400.

**Worked example 4.2.4b — fixed-overhead expenditure and volume variances.**

1. **Setup.** Budgeted fixed overhead is **USD 200,000** over a budgeted output of **10,000 units** — an
   overhead absorption rate (OAR) of USD 20/unit. Actual output is **9,000 units**; actual fixed overhead is
   **USD 205,000**.
2. **Formula.** `Overhead absorbed = OAR × actual output`; `Expenditure variance = Budgeted overhead − Actual
   overhead`; `Volume variance = Absorbed − Budgeted overhead`.
3. **Substitution.** `Absorbed = 20 × 9,000 = 180,000`; `Expenditure = 200,000 − 205,000 = (5,000)` **(A)**;
   `Volume = 180,000 − 200,000 = (20,000)` **(A)**.
4. **Result.** Total fixed-overhead variance `= Absorbed 180,000 − Actual 205,000 = (25,000)` **(A)**, which
   splits into a (5,000) expenditure variance (spent more than budget) and a (20,000) volume variance
   (produced fewer units than planned, under-absorbing fixed cost).
5. **Interpretation.** The two adverse causes are different — spending versus volume (under-absorption,
   cross-ref Domain 5, KA 5.1.3) — and go to different owners. Splitting them, not reporting a single
   (25,000), is the value of variance analysis.

### 4.2.5 Reading favourable and adverse well

**The professional angle.** A favourable variance is not automatically good news, nor an adverse one
automatically bad. A favourable **cost** variance may mean corners were cut (a quality risk that returns as
rework); a favourable **efficiency** variance driven by skipping inspections is a liability, not a saving. An
adverse variance may be the *right* decision (accelerating to protect a critical milestone). The professional
reads variances **with their causes and consequences**, not by sign alone — and reports them that way (4.3).

**AI in this KA.** Variance analysis is a strong AI use case: models can compute and decompose variances
automatically across hundreds of control accounts, cluster them by likely cause, and **draft the narrative**
("the adverse material variance is driven ~70 % by rate, ~30 % by usage"). The professional owns the
attribution and the judgement — an AI narrative that labels a deliberate acceleration cost an "overrun", or
misses that a favourable variance hides skipped scope, misleads a board. **AI proposes; the professional verifies, decides and remains accountable.**

### 4.2.6 Margin fade

**The principle.** **Margin fade** is the erosion of the forecast margin at completion across a contract's
life — the contractor's headline health metric, watched period by period at portfolio level. It is variance
analysis applied to the *forecast* rather than the period: each period's forecast margin (contract price
less `EAC`, as a percentage of price) is trended, and every step down is attributed through the EAC
movement bridge (Domain 3, KA 3.4.3). Fade has a characteristic signature: won at a competitive margin,
held through early procurement wins, eroded by execution (productivity, quantity growth), and crystallised
late by claims and closeout costs. A book of contracts that all fade tells you the *estimating* or the
*bidding* is wrong, not the delivery (cross-ref the ratchet of Advanced 3.A.5).

**Worked example 4.2.6 — reading a fade curve.**

1. **Setup.** A **USD 20,000,000** contract's forecast margin at completion, quarter by quarter:
   **12.0 % → 11.4 % → 10.1 % → 9.0 % → 8.2 %**.
2. **Formula.** `forecast margin = price − EAC`; fade per period = margin-% step × price.
3. **Substitution.** In money: `2,400,000 → 2,280,000 → 2,020,000 → 1,800,000 → 1,640,000`; steps of
   `120,000, 260,000, 220,000, 160,000` — total fade `= 2,400,000 − 1,640,000 = 760,000`.
4. **Result.** Nearly a *third* of the bid margin gone in four quarters, with the largest single step
   (260,000) in the quarter the structural quantities were remeasured.
5. **Interpretation.** Four one-sided steps is the ratchet signature (Advanced 3.A.5): treat it as one
   systematic under-forecast, not four surprises. The board question is not "what is the margin?" but
   "what is the *fade rate*, and what changed in the quarter it accelerated?" — and each step must
   reconcile to that quarter's EAC bridge (Domain 3, KA 3.4.3): fade with no bridge lines is unexplained
   drift, the worst kind.

### Key terms — KA 4.2

| Term | Meaning |
|---|---|
| **Variance** | Planned minus actual; favourable (F) improves profit, adverse (A) worsens it. |
| **Flexed budget** | The budget adjusted to the actual output level before comparison. |
| **Price/rate variance** | `(Actual price − Standard price) × Actual quantity`. |
| **Quantity/efficiency variance** | `(Actual quantity − Standard quantity) × Standard price`. |
| **Variance bridge** | A waterfall walking budget to actual by variance component. |

### Sample MCQs — KA 4.2

**MCQ 4.2-A `[4.2.3 · Application]`** Standard 1,000 units at USD 50; actual 1,100 units at USD 52. The
material **price** variance is:
- A. USD 2,000 (A)
- B. USD 2,200 (A) ✅
- C. USD 5,000 (A)
- D. USD 7,200 (A)

*Rationale:* `(52 − 50) × 1,100 = 2,200` adverse. A uses standard quantity (1,000) by mistake; C is the
*quantity* variance; D is the total.

**MCQ 4.2-B `[4.2.3 · Application]`** Same data. The material **quantity** variance is:
- A. USD 5,000 (A) ✅
- B. USD 5,200 (A)
- C. USD 2,200 (A)
- D. USD 200 (A)

*Rationale:* `(1,100 − 1,000) × 50 = 5,000` adverse (extra units at standard price). B applies the actual
price in error; C is the price variance; D miscounts.

**MCQ 4.2-C `[4.2.2 · Analysis]`** Why flex the budget to actual output before analysing variances?
- A. To make the budget larger.
- B. To separate efficiency/price effects from volume effects. ✅
- C. To comply with IFRS 15.
- D. To avoid computing variances at all.

*Rationale:* Flexing compares like with like (cost for the output actually achieved), isolating
price/efficiency from volume. It is not about inflating the budget, an IFRS matter, or avoiding analysis.

**MCQ 4.2-D `[4.2.5 · Analysis]`** A work package reports a large *favourable* cost variance. The best
professional response is to:
- A. Report it as a saving and move on.
- B. Increase the budget.
- C. Treat it as an error.
- D. Investigate the cause — a favourable variance can hide skipped scope, deferred cost or quality risk. ✅

*Rationale:* Sign alone is not the story; a favourable variance may reflect skipped/deferred work that returns
later. Investigating the cause is the professional response; it is neither automatically a clean saving nor an
error.

**MCQ 4.2-E `[4.2.4 · Application]`** Budgeted fixed overhead is USD 120,000 over a budgeted output of 6,000
units. Actual output is 5,500 units and actual fixed overhead is USD 118,000. The fixed-overhead **volume**
variance is:
- A. USD 2,000 (F)
- B. USD 8,000 (A)
- C. USD 10,000 (A) ✅
- D. USD 10,000 (F)

*Rationale:* The absorption rate is `120,000 / 6,000 = USD 20/unit`; absorbed `= 20 × 5,500 = 110,000`;
volume variance `= 110,000 − 120,000 = (10,000)` adverse — fewer units than planned under-absorb fixed cost.
A is the *expenditure* variance (`120,000 − 118,000`); B is the *total* overhead variance
(`110,000 − 118,000`); D has the sign wrong — producing less than plan is adverse, not favourable.

**MCQ 4.2-F `[4.2.1 · Recall]`** A variance is classified as **favourable** when:
- A. Actual differs from budget by any amount.
- B. The quantity variance is larger than the price variance.
- C. It improves profit — actual cost below plan, or actual revenue above plan. ✅
- D. It falls within the reporting tolerance.

*Rationale:* Favourable/adverse is defined by the effect on profit, not by the mere existence, composition
or size of the difference. A describes any variance; B compares components; D describes an in-tolerance
variance, which can be favourable or adverse.

### Self-check — KA 4.2

1. Split a total cost variance into its two generic components and give the formula for each. *(Price/rate:
   `(AP−SP)×AQ`; quantity/efficiency: `(AQ−SQ)×SP`.)*
2. Why is a variance bridge more useful to a board than a single "over by X" figure? *(It attributes the gap
   to named causes and magnitudes, showing what to act on.)*

---

## Knowledge Area 4.3 — Management reporting

*Topics: 4.3.1 designing the report for the decision · 4.3.2 the project-controls dashboard · 4.3.3 narrative
vs numbers · 4.3.4 cadence and audience · 4.3.5 the exception report.*

### 4.3.1 Designing the report for the decision

**The principle.** A management report exists to **support a decision**, and its design should start from that
decision, not from the data available. The test of a report is not how much it contains but whether a reader
can, in the time they have, see **where they are, where they are heading, what is off-track, and what is being
done about it**. Everything that does not serve that is noise. This is the reporting counterpart of the
management-vs-statutory distinction (Domain 2, KA 2.5): management reports are *for steering*, so they are
timely, forward-looking and structured by the work (project/WBS/control account).

### 4.3.2 The project-controls dashboard

**Definition & purpose.** A **project-controls dashboard** integrates the core measures onto one view: cost
(`BAC`, `AC`, `EAC`, `VAC`, `CPI`), schedule (`SPI`, milestones, critical-path status), the forecast, risk
exposure and the key leading indicators — each with its RAG status against tolerance (4.1.1). A good
dashboard follows an **overview-first, detail-on-demand** structure: the top level fits on one screen/page and
answers the four questions in 4.3.1; drill-downs expose the control-account detail behind any red. It shows
**trend**, not just a snapshot, because direction (4.1.2) is where the early warning lives.

### 4.3.3 Narrative versus numbers

**The principle.** Numbers state *what*; narrative states *why, so-what and now-what*. A controls report needs
both: a `CPI` of 0.92 is a fact; "the 0.92 `CPI` is driven by the steelwork rate rise (~USD 4k) and reworked
foundations (~USD 4k); the rate is now locked and rework closed, so the trend should recover next period" is a
*decision-ready* explanation. Narrative without numbers is opinion; numbers without narrative is a puzzle.
The best variance narrative is short, causal, quantified, and honest about uncertainty.

### 4.3.4 Cadence and audience

**The principle.** **Cadence** (weekly, monthly, milestone) and **level of detail** are matched to the
**audience** and the decision rhythm. A working team needs weekly, granular, action-focused reporting; a
project board needs monthly, summarised, exception-and-forecast reporting; an executive/portfolio view needs
periodic, highly aggregated, cross-project reporting. The **same underlying data** is aggregated differently
for each — which only works if it is coded correctly at source (Domain 1, KA 1.5) so aggregation is automatic,
not manual re-keying.

### 4.3.5 The exception report

**Definition & purpose.** An **exception report** presents only the items **outside tolerance** (4.1.3) — the
reds and ambers — each with its variance, root cause, impact and action/owner. It is the practical form of
management by exception and the antidote to the "everything is green except the things nobody read to page 40"
report. Well done, it *shortens* reports while *increasing* the attention on what matters.

**AI in this KA.** AI supports reporting end-to-end: assembling the dashboard from source data, detecting
which control accounts breach tolerance, **drafting the exception narratives**, and answering
natural-language questions over controls data ("why did project 1420's `EAC` move this month?"). The
professional owns accuracy and framing: a drafted narrative can misattribute cause or bury a caveat, and a
natural-language answer can be confidently wrong on a definition. Reports drive decisions and sometimes
external disclosure — so the human signs off. **AI proposes; the professional verifies, decides and remains accountable.**

### 4.3.6 Worked example — an exception report from a RAG dashboard

A monthly dashboard shows six control accounts with RAG status against a tolerance of `CPI` 0.95 (amber) /
0.90 (red):

| Control account | CPI | Status |
|---|---:|---|
| CA-01 Foundations | 1.02 | Green |
| CA-02 Structure | 0.98 | Green |
| CA-03 M&E | 0.93 | Amber |
| CA-04 Fit-out | 0.88 | Red |
| CA-05 External works | 1.01 | Green |
| CA-06 Commissioning | 0.86 | Red |

Management by exception (4.1.3) means the exception report focuses on the two reds (CA-04, CA-06) and the
amber (CA-03) — each with its variance, root cause, impact and action — and leaves the greens noted but
unelaborated. This turns a six-account status into a three-line action list, concentrating attention where it
changes outcomes.

### Key terms — KA 4.3

| Term | Meaning |
|---|---|
| **Management report** | A decision-support document, structured by the work and the audience. |
| **Controls dashboard** | An integrated cost/schedule/forecast/risk view with RAG status and trend. |
| **Overview-first, detail-on-demand** | Summary on one view, drill-down to the detail behind any red. |
| **Exception report** | Only the out-of-tolerance items, with cause, impact and action. |
| **Cadence** | The reporting frequency matched to audience and decision rhythm. |

### Sample MCQs — KA 4.3

**MCQ 4.3-A `[4.3.1 · Analysis]`** The best test of a management report's design is whether it:
- A. Lets the reader see status, direction, exceptions and actions in the time available. ✅
- B. Contains every available data point.
- C. Is as long as possible.
- D. Uses the most advanced charts.

*Rationale:* A report serves a decision — status, trajectory, exceptions and actions, digestible in the
reader's time. Completeness for its own sake, length and chart sophistication are not the test.

**MCQ 4.3-B `[4.3.3 · Application]`** Which is the most *decision-ready* reporting of a cost result?
- A. "`CPI` is 0.92."
- B. "Costs are over budget."
- C. "`CPI` 0.92, driven ~50/50 by a steel rate rise (now locked) and foundation rework (now closed); trend should recover next period." ✅
- D. "See the attached 40-page cost ledger."

*Rationale:* C pairs the number with cause, so-what and now-what. A is a bare fact; B is vague; D buries the
answer. Numbers plus causal, quantified narrative is the standard.

**MCQ 4.3-C `[4.3.5 · Application]`** A monthly dashboard shows eight control accounts: five green, two
amber and one red against their tolerances. The exception report should present:
- A. All eight accounts in equal detail.
- B. Only the red account.
- C. The red and the two amber accounts, each with variance, root cause, impact and action/owner. ✅
- D. The five green accounts, to evidence good performance.

*Rationale:* The exception report carries the out-of-tolerance items — reds *and* ambers — each with cause,
impact and action, leaving greens noted but unelaborated. A dilutes attention; B ignores the ambers that are
the earliest recoverable problems; D inverts the principle.

**MCQ 4.3-D `[4.3.4 · Recall]`** Reporting to a *project board* is best characterised as:
- A. Weekly, granular and action-list focused.
- B. Monthly, summarised, exception-and-forecast focused. ✅
- C. Periodic, highly aggregated and cross-project.
- D. Daily extracts of the raw cost ledger.

*Rationale:* Cadence and detail are matched to the audience: a board steers monthly on summaries, exceptions
and the forecast. A describes the working team's rhythm; C describes the executive/portfolio view; D serves
no decision at any level.

### Self-check — KA 4.3

1. What four questions should a top-level controls dashboard answer at a glance? *(Where are we; where are we
   heading; what is off-track; what is being done about it.)*
2. Why does audience-tailored reporting depend on good cost coding? *(The same data must aggregate
   automatically to each level; without source coding it becomes manual re-keying.)*

---

## Knowledge Area 4.4 — Data visualisation and storytelling for controls

*Topics: 4.4.1 choosing the right chart · 4.4.2 common distortions to avoid · 4.4.3 storytelling with
controls data.*

### 4.4.1 Choosing the right chart

**The principle.** The chart should match the **question**: trend over time → a line/S-curve; composition →
a stacked bar (rarely a pie, and never for many slices); comparison across items → a bar chart; a bridge of
contributions → a waterfall (as in 4.2.4); distribution/uncertainty → a range or box; correlation → a scatter.
The default for project controls is the **S-curve** (PV/EV/AC over time) and the **waterfall** (variance
bridges) because they answer the two questions controls asks most: *how are we tracking* and *what moved the
number*.

**Worked example 4.4.1 — the same four numbers, three ways.**

1. **Setup.** Monthly `CPI` readings: **0.98, 0.97, 0.96, 0.95** — one hundredth lost each month.
2. **Formula.** Chart-to-question fit (4.4.1): present the same data as (1) a table, (2) a line chart on an
   honest axis, (3) a bar chart with a truncated axis, and compare the message each sends.
3. **Substitution.** (1) In a **table**, the drift is arithmetic the reader must do — most won't. (2) As a
   **line chart** on an honest axis (say 0.90–1.05), a clear, gentle downward slope appears — the right
   choice for a trend question (4.4.1). (3) As a **bar chart** with the axis truncated at 0.94, the last bar
   looks a quarter the height of the first — a fabricated cliff (4.4.2).
4. **Result.** Same data, three different messages; only the line-with-honest-axis answers "how are we
   trending?" without editorialising.
5. **Interpretation.** Chart choice *is* an analytical decision; the reader experiences the chart, not the
   data. The controls professional chooses the form that lets the data speak — and the reviewer checks the
   axes before believing any picture.

### 4.4.2 Common distortions to avoid

**The principle — honesty in the picture.** A visualisation can mislead even with correct numbers. The
recurring distortions to avoid:

- **Truncated axes** — a y-axis not starting at zero exaggerates small differences (especially on bar charts).
- **Dual axes** — two different scales on one chart can manufacture a correlation that is not there.
- **3-D and decoration** — 3-D pies and heavy styling distort proportions and hide data.
- **Cherry-picked baselines/time-windows** — starting a trend at a flattering point.
- **Inconsistent scales** across small multiples, so panels look comparable when they are not.

The professional standard is that the *picture* must tell the same truth as the *numbers*. (For chart design
craft — colour, accessibility, layout — see the dataviz guidance referenced by the platform.)

### 4.4.3 Storytelling with controls data

**The principle.** A controls "story" is a short causal arc: **here is where we are (status) → here is what
changed and why (variance/cause) → here is where it takes us (forecast) → here is the decision (action)**. The
S-curve and the variance bridge are its two illustrations; the exception report is its focus. Storytelling is
not spin — it is the disciplined ordering of true facts so a decision-maker reaches the right decision quickly.
Done with integrity, it is the highest-value thing a controls professional produces, because it is where all
the measurement finally moves an outcome.

**AI in this KA.** AI can propose the right chart for a dataset, generate visuals, and draft the narrative arc.
It can also *introduce* distortions (a default truncated axis, a misleading dual axis) if unsupervised, and
can write persuasive narrative that overstates certainty. The professional curates the visual and owns the
story's integrity. **AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 4.4

| Term | Meaning |
|---|---|
| **Chart-to-question fit** | Choosing the chart type that answers the actual question. |
| **Truncated axis** | A non-zero-based axis that exaggerates differences. |
| **Small multiples** | Repeated small charts — must share consistent scales. |
| **Storytelling** | Ordering true facts (status → cause → forecast → action) to drive a decision. |

### Sample MCQs — KA 4.4

**MCQ 4.4-A `[4.4.2 · Analysis]`** A bar chart makes a 2 % cost difference look enormous. The most likely
cause is:
- A. Too few bars.
- B. Using brand colours.
- C. A missing legend.
- D. A y-axis that does not start at zero (truncated axis). ✅

*Rationale:* A truncated y-axis exaggerates small differences on bar charts. Bar count, colour and legend do
not create that specific distortion.

**MCQ 4.4-B `[4.4.1 · Application]`** To explain *what drove* a cost result from budget to actual, the best
chart is a:
- A. Pie chart.
- B. Scatter plot.
- C. 3-D column chart.
- D. Waterfall (variance bridge). ✅

*Rationale:* A waterfall walks contributions from a start to an end value — exactly a variance bridge. Pies
show composition, scatters show correlation, and 3-D distorts.

**MCQ 4.4-C `[4.4.3 · Recall]`** The disciplined ordering of a controls "story" for a decision-maker is:
- A. Status → what changed and why → where it takes us (forecast) → the decision (action). ✅
- B. Action → forecast → status → cause.
- C. Forecast → status → action → cause.
- D. Cause → action → status → forecast.

*Rationale:* The causal arc runs status, then variance/cause, then forecast, then action — true facts ordered
so the reader reaches the right decision quickly. The other orderings put the conclusion before the evidence
or the remedy before the problem.

**MCQ 4.4-D `[4.4.2 · Analysis]`** A chart plots cost on the left y-axis and RFI count on a second right
y-axis, and the two lines track each other closely. The professional concern is that:
- A. RFIs should never appear on a cost chart.
- B. Dual axes let the scales be chosen so the apparent relationship is manufactured, not real. ✅
- C. The chart uses too many colours.
- D. A pie chart would have been more appropriate.

*Rationale:* With two independent scales, either axis can be stretched or shifted until the lines "correlate"
— a classic distortion even when every number is correct. A is too absolute (the pairing can be legitimate if
presented honestly); C and D do not address the scale manipulation at issue.

### Self-check — KA 4.4

1. Which two chart types are the workhorses of project controls, and what question does each answer? *(S-curve
   — how are we tracking over time; waterfall/variance bridge — what moved the number.)*
2. Name three ways a visualisation can mislead with correct numbers. *(Truncated axis; dual axes;
   3-D/decoration; cherry-picked baseline; inconsistent small-multiple scales.)*

---

## Advanced topics — Domain 4

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 4.A.1 — Mix and yield variances

**The principle.** When an input is a **blend** — an aggregate mix, a multi-grade labour gang — the usage
variance of 4.2.3 hides two different stories: the **composition** of the blend may have changed, and the
**total input per unit of output** may have changed. Splitting them gives the **mix variance** (actual input
in its actual mix versus the same total input in the *standard* mix, at standard prices) and the **yield
variance** (actual total input, in standard mix, versus the standard input for the actual output, at the
weighted standard price). Mix + yield = the usage variance — the decomposition discipline of 4.2.3 applied
one level deeper.

**Worked example 4.A.1 — mix and yield split.**

1. **Setup.** Standard blend: **60 % material A at USD 10** and **40 % material B at USD 25** — weighted
   standard price `(600 × 10 + 400 × 25) / 1,000 = USD 16/unit`. Standard yield: **1,000 input units → 800
   output units** (1.25 input per output). Actual: input **1,100 units** (715 A + 385 B); output **840
   units**.
2. **Formulae.** `Mix = Σ (actual qty − standard-mix share of actual input) × standard price`;
   `Yield = (actual total input − standard input for actual output) × weighted standard price`.
3. **Substitution.** Standard mix of 1,100 = 660 A + 440 B. Mix: A `(715 − 660) × 10 = 550` adverse-side, B
   `(385 − 440) × 25 = (1,375)` favourable-side → net **825 (F)**. Standard input for 840 output
   `= 840 × 1.25 = 1,050`; yield `= (1,100 − 1,050) × 16 = 800` **(A)**.
4. **Result.** Mix **825 (F)**, yield **800 (A)**, net usage **25 (F)**. Cross-check at standard prices:
   actual input `715 × 10 + 385 × 25 = 16,775`; standard input for actual output `630 × 10 + 420 × 25 =
   16,800`; `16,775 − 16,800 = (25)` → 25 (F). ✓
5. **Interpretation.** The near-zero usage variance is two large, offsetting causes: the blend was cheapened
   (more A, less B — favourable mix) *and* it took more total input per unit of output (adverse yield).
   Cheapening a blend often *causes* the yield loss — read the pair together (4.2.5), because "saving" on
   mix while losing on yield is a real and common false economy.

### Advanced 4.A.2 — Planning versus operational variances

**The principle.** A variance against a standard assumes the standard was *right*. When the environment
moves — a market rate rise, a revised norm — part of the variance is a **planning variance** (the original
standard was wrong, measured as revised standard versus original) and only the remainder is an **operational
variance** (execution against a *realistic* benchmark, measured as actual versus revised standard). The
benchmark is revised **before** the team is judged.

**Worked example 4.A.2 — splitting a labour rate variance.**

1. **Setup.** Original standard rate **USD 40/hour**; a market movement makes the realistic (revised)
   standard **USD 44/hour**; actual rate paid **USD 45/hour** over **2,000 actual hours**.
2. **Formulae.** `Planning = (revised standard − original standard) × actual hours`; `Operational = (actual −
   revised standard) × actual hours`.
3. **Substitution.** Planning `= (44 − 40) × 2,000 = 8,000` **(A)**; operational `= (45 − 44) × 2,000 =
   2,000` **(A)**.
4. **Result.** Total rate variance `= (45 − 40) × 2,000 = 10,000` **(A)** `= 8,000 + 2,000`. ✓
5. **Interpretation.** Of the USD 10,000 adverse, USD 8,000 was uncontrollable at site level — the standard,
   not the team, was wrong — and only USD 2,000 is genuinely operational.

**The fairness and behavioural point.** Holding a team to an unattainable standard does not motivate — it
destroys the credibility of the whole variance system, and gaming follows (4.1.1). Conversely, planning
variances discipline the *estimators*: a persistent planning variance says the standards process needs
fixing. One caution: revising a standard is re-baselining in miniature, so it needs the same authorisation
and audit trail as any baseline change (Domain 5, KA 5.4.3) — otherwise "the standard was wrong" becomes the
universal excuse.

### Advanced 4.A.3 — KPI system design and Goodhart's law

**The principle.** **Goodhart's law**: *when a measure becomes a target, it ceases to be a good measure*.
The moment a KPI carries consequences — bonuses, RAG status, board attention — people optimise the
**number**, and the number decouples from the reality it was chosen to reflect (the gameable-KPI failure
mode of 4.1.1, now with a mechanism).

**Gaming patterns in controls.** The recurring ones are worth naming. **Earned-value credit-chasing**:
claiming progress on easy-to-measure, high-credit work while deferring the awkward remainder, so `EV` (and
`SPI`/`CPI`, Domain 6) flatter the true position until the deferred work bites. **Deferring accruals to hit
a month**: holding received-but-uninvoiced cost out of the period understates `AC` and flatters `CPI`
(Domain 5, KA 5.2.2) — a cut-off manipulation dressed as timing. **Threshold-hugging**: managing a measure
to sit just inside tolerance, so management by exception (4.1.3) never looks. **Trend-window selection**:
starting the chart where the story looks best (4.4.2). None of these requires falsifying a number; each
exploits the gap between the measure and the objective.

**Countermeasures.** First, **paired indicators**: pair every gameable measure with the one it is most
likely gamed against — cost with quality/rework, schedule progress with remaining-work trend, accrual
completeness with goods-received records — so improving one at the other's expense is visible. Second,
**audit the measure**, not just the result: periodic independent checks that EV credit matches physical
progress and accruals match receipts. Third, **trend over snapshot**: a gamed measure usually reverts, and
the trend line shows the sawtooth a single month hides. Fourth, **refresh the set**: retire indicators once
their gaming pattern matures. The professional posture is not cynicism but design: assume Goodhart, and
build the KPI system so gaming is harder than performing.

### Advanced 4.A.4 — Portfolio reporting cadence

**The principle.** Across a portfolio, reporting is a **pyramid**: weekly at the working level, monthly at
project boards, quarterly (or periodic) at portfolio/executive level (4.3.4). The pyramid only works when
all three tiers draw on **one source of truth**, aggregated automatically through the coding structure
(Domain 1, KA 1.5) — the same control-account postings rolling up from work package to project to portfolio
without re-keying. Two dashboards that disagree are worse than one that is late: the meeting becomes a
reconciliation, not a decision.

**What each tier decides.** The **weekly** tier is action-focused: productivity, this week's exceptions,
next week's lookahead — decisions about *doing the work*. The **monthly** tier is the project board:
exceptions against tolerance, the forecast (`EAC`/`VAC`), change approvals, recovery plans — decisions about
*steering the project* (4.3.5). The **quarterly/portfolio** tier is allocation and escalation: which
projects get resources, reserves and attention; which tolerances or baselines need re-setting; what the
aggregate exposure means for the business — decisions about *steering the portfolio*.

**What each tier must not re-litigate.** The pyramid fails when tiers reach down. A project board that
re-works the site team's action list is spending its scarce attention below its decision level — the inverse
of management by exception (4.1.3). A portfolio review that re-opens a project's variance attribution is
redoing analysis it should be *testing*: its proper questions are whether the forecast is credible, the
tolerances still right, and the escalation honest. Equally, tiers must not re-argue what a lower tier
properly decided within its delegated tolerance — that is what the tolerance *is* (4.1.1). Each tier reads
the tier below by exception, adds its own decision, and passes only genuine escalations upward. The cadence
pyramid is management by exception applied vertically.

### Advanced 4.A.5 — Signal and noise: control-chart thinking for variance review

**The principle.** Every cost series wobbles. A variance-review culture that demands an explanation for
every wobble manufactures narrative — and teaches people to invent causes for noise — while one that
ignores everything misses the genuine shift. **Statistical process control**, applied here at awareness
level, resolves the dilemma: characterise the *normal* variation of the series first, then investigate only
what falls outside it. That is management by exception (4.1.3) with the exception line drawn from the data
rather than negotiated — the same discipline as the data-derived thresholds of 4.1.1.

**Worked example 4.A.5 — a control limit from six months of history.**

1. **Setup.** A control account's monthly cost variance, as a percentage of budget, over six months:
   **+1.2, −0.8, +0.6, −1.5, +0.9, −0.4**.
2. **Formulae.** `σ = √(Σ (x − mean)² / n)`; investigation band `= mean ± 2σ`.
3. **Substitution.** Mean `= 0.0 %`; the squared deviations sum to `5.66`, giving `σ = √(5.66 / 6) ≈ 0.97`
   percentage points; a ±2σ band is therefore about **±1.9 %**.
4. **Result.** Month 7 lands at **−2.6 %** — outside the band: investigate. Note the counterpart: the
   −1.5 % in month 4 was *inside* the band, so demanding a root-cause memo for it that month would have
   been narrative about noise.
5. **Interpretation.** The account's investigation threshold is ≈ ±1.9 %, reviewed as the history grows.

**The discipline, not the precision.** Six points is a short history and the limits are rough — the point
is the *discipline* (limits from data, revised as data accrues), not false precision. The tie to Goodhart
(Advanced 4.A.3) is direct: a negotiated threshold invites gaming; a data-derived one invites improvement.
What crosses the line feeds the exception report (4.3.5). On AI: fitting and refreshing control limits
across hundreds of accounts is a machine task; deciding what a genuine signal *means* — and resisting the
urge to explain noise — stays with the professional.

---

## Case study — Domain 4: turning a red month into a decision (healthcare)

*This end-of-domain case integrates the whole of Domain 4 on a single month of a single control account:
KPIs, tolerances and management by exception (KA 4.1); the flexed budget, the price/usage and
rate/efficiency decomposition and the variance bridge (KA 4.2); cause-based exception reporting and the
board narrative (KA 4.3); and honest presentation (KA 4.4). Every figure reconciles — work it through with a
calculator before reading the commentary.*

### Background

Amberwell Health Estates (a fictional healthcare-estates company) is delivering the fit-out of a new **hospital wing**: four storeys of wards,
treatment rooms and clinical support space above a ground-floor imaging suite. The programme is controlled
through control accounts reporting on a monthly cadence to a project board (KA 4.3.4). The account at the
centre of this case is **CA-07 Clinical fit-out** — metal-stud partitions, hygienic ceilings, specialist
wall protection, containment for medical-gas and data services, and clinical-grade finishes. Its cost KPI is
measured against a **flexed budget** for each month's actual output, with a target variance of zero and RAG
thresholds of ±5 % of standard cost (green within ±5 %, amber beyond it, red beyond ±10 %) — a target, a
threshold and a tolerance exactly as KA 4.1.1 prescribes.

For five months CA-07 has run green and, under management by exception (KA 4.1.3), has earned nothing more
than a status line. This month the dashboard turns **red**: actual cost of USD 367,600 against a flexed
standard cost of USD 330,000 — **USD 37,600 adverse**, an 11.4 % breach of the ±10 % red threshold. A red
status is not a verdict; it is a work order. Before the board meets in six working days, the controls team
must **decompose** the variance into its price, usage, rate, efficiency and overhead components;
**attribute** each component to a cause precise enough to have an owner; **assess** the consequence for the
forecast; and **propose** the action. One further fact, filed and forgotten, will matter at the end: last
month's report noted — without escalation — that RFIs querying services coordination in the ceiling voids
had doubled. That was a leading indicator (KA 4.1.2), and nobody acted on it.

### The month's data

The first discipline is the flex (KA 4.2.2). CA-07 delivered more installed output this month than the
original phasing assumed, so comparing actual cost with the *original* monthly budget would fold a volume
effect into the variance and mis-blame the team for doing more work. The budget is therefore flexed to the
month's **actual output**, priced at standard: 2,000 standard material units at USD 75 per unit; 3,000
standard labour hours at USD 50 per hour; and fixed overhead absorbed at USD 10 per standard labour hour.
Everything below is measured against that flexed standard cost of **USD 330,000**.

| Element | Flexed budget (standard) | USD | Actual | USD | Variance |
|---|---|---:|---|---:|---:|
| Materials | 2,000 units × USD 75 | 150,000 | 2,150 units × USD 78 | 167,700 | 17,700 (A) |
| Labour | 3,000 hours × USD 50 | 150,000 | 3,200 hours × USD 52 | 166,400 | 16,400 (A) |
| Fixed overhead | 3,000 hours × USD 10 | 30,000 | Incurred | 33,500 | 3,500 (A) |
| **Total** | | **330,000** | | **367,600** | **37,600 (A)** |

Every line is adverse, and the total — USD 37,600 (A) — is the number that turned the dashboard red. But
"37,600 over" is arithmetic, not analysis (KA 4.2.1): it says nothing about *which lever to pull*. The
materials line alone mixes two different problems — paying more per unit and using more units — and the
labour line does the same. The decomposition separates them.

### The decomposition

**Worked decomposition — CA-07, this month.**

1. **Setup.** Standards and actuals as tabled above: materials standard **2,000 units at USD 75**, actual
   **2,150 units at USD 78**; labour standard **3,000 hours at USD 50**, actual **3,200 hours at USD 52**;
   overhead absorbed **30,000** (USD 10 × 3,000 standard hours), incurred **33,500**.
2. **Formulae.** `Price/rate variance = (Actual price − Standard price) × Actual quantity`;
   `Usage/efficiency variance = (Actual quantity − Standard quantity) × Standard price` (KA 4.2.3);
   `Overhead variance = Actual overhead incurred − Overhead absorbed` (cf. 4.2.4b).
3. **Substitution.**
   - Material price: `(78 − 75) × 2,150 = 6,450` **(A)**
   - Material usage: `(2,150 − 2,000) × 75 = 11,250` **(A)**
   - Labour rate: `(52 − 50) × 3,200 = 6,400` **(A)**
   - Labour efficiency: `(3,200 − 3,000) × 50 = 10,000` **(A)**
   - Fixed overhead: `33,500 − 30,000 = 3,500` **(A)**
4. **Result.** The five components sum to `6,450 + 11,250 + 6,400 + 10,000 + 3,500 = 37,600` **(A)** —
   reconciling exactly to actual minus standard: `367,600 − 330,000 = 37,600` **(A)**. ✓ Element checks:
   materials `6,450 + 11,250 = 17,700 = 167,700 − 150,000` ✓; labour `6,400 + 10,000 = 16,400 = 166,400 −
   150,000` ✓; overhead `3,500 = 33,500 − 30,000` ✓.
5. **Interpretation.** The single red number is five numbers — and, as the cause analysis shows, the five
   numbers are **two causes and one consequence**. Note what honest decomposition prevents: without it, the
   largest single component (material usage, 11,250 (A)) could plausibly be presented as a "materials
   procurement problem" — wrong cause, wrong owner, wrong remedy.

### The variance bridge

For the board pack, the decomposition becomes a **variance bridge** (KA 4.2.4) — a walk from the flexed
standard cost to actual cost, one component at a time, so the reader sees *what moved the number and by how
much*:

| Step | USD | Running total |
|---|---:|---:|
| **Flexed budget (standard cost)** | | 330,000 |
| Material price | 6,450 (A) | 336,450 |
| Material usage | 11,250 (A) | 347,700 |
| Labour rate | 6,400 (A) | 354,100 |
| Labour efficiency | 10,000 (A) | 364,100 |
| Fixed overhead | 3,500 (A) | 367,600 |
| **Actual cost** | | **367,600** |

The bridge reconciles exactly: `330,000 + 6,450 + 11,250 + 6,400 + 10,000 + 3,500 = 367,600`. Rendered as a
waterfall (KA 4.4.1) — zero-based axis, no decoration, adverse steps in the warning colour (KA 4.4.2) — it
tells the story the headline figure hides.

> **Fig CS4.1 — CA-07 flexed budget to actual, current month.** *Caption:* what drove the USD 37,600 (A)
> month. *Underlying data:* the bridge table above. *Render-ready description:* a waterfall chart — starting
> bar "Flexed budget 330,000"; five rising red floating bars (Material price 6,450; Material usage 11,250;
> Labour rate 6,400; Labour efficiency 10,000; Overhead 3,500); ending bar "Actual 367,600"; connectors
> between bars, values labelled. *Animation storyboard (digital-only):* each floating bar rises in sequence
> with its label, the running total ticking up from 330,000 to 367,600.

### The cause analysis

The decomposition's five lines are still organised **by account line** — materials, labour, overhead. Causes
do not respect account lines, and neither should the report (KA 4.3.1). Investigated on the floor, the five
components regroup into three:

| Cause group | Components | USD | Share | Owner |
|---|---|---:|---:|---|
| Rework — ceiling-void services clash | Material usage 11,250 (A) + labour efficiency 10,000 (A) | 21,250 (A) | ~57 % | Design manager |
| Market movement — copper and agency labour | Material price 6,450 (A) + labour rate 6,400 (A) | 12,850 (A) | ~34 % | Procurement lead |
| Consequential overhead under-recovery | Fixed overhead 3,500 (A) | 3,500 (A) | ~9 % | — (consequence) |
| **Total** | | **37,600 (A)** | **100 %** | |

**Rework — 21,250 (A), ~57 %.** The material usage variance (150 extra units) and the labour efficiency
variance (200 extra hours) share one root cause: a **services design clash in the ceiling void**. The
medical-gas containment and the ventilation ductwork were coordinated into the same zone above the ward
corridors; installed partition heads and ceiling grid had to be stripped out and rebuilt once the clash
surfaced on site. Extra materials and extra hours are the *same event wearing two account codes*. This is a
**coordination failure**, and its owner is the **design manager** — not the site team whose productivity
figures carry the damage. It is also where the leading indicator returns: the doubling RFI trend on services
coordination *was* this clash, visible a month before it reached the cost ledger.

**Market movement — 12,850 (A), ~34 %.** The material price variance (USD 3 per unit across 2,150 units)
traces to **copper-driven price rises** on containment and cabling components; the labour rate variance (USD
2 per hour across 3,200 hours) traces to **agency labour** engaged at premium rates in a tight regional
market. Neither is a site failure; both are market facts. The owner is **procurement**, and the exposure is
**partially mitigable**: forward-ordering copper-based components for the remaining floors locks in today's
price against further movement, and reducing agency reliance at the next crew rotation trims the rate
premium — though neither action recovers the money already spent.

**Consequential overhead — 3,500 (A), ~9 %.** The overhead variance is **under-recovery driven by the extra
hours**: supervision, plant and site running costs were held on the floor for the extended working, taking
incurred overhead to 33,500 against the 30,000 absorbed at standard. It is a **consequence, not a cause** —
resolve the rework and the rate exposure, and this line largely resolves itself. Giving it its own "action"
in the report would be noise dressed as diligence.

### The exception report entry

The dashboard shows the red; the exception report (KA 4.3.5) carries the entry — short, causal, quantified
and actionable, in the form a board member actually reads:

> **CA-07 Clinical fit-out — RED.** Month cost variance **USD 37,600 (A)** against a flexed budget of USD
> 330,000 (11.4 %; red threshold ±10 %). Causes: ~57 % rework following a services design clash in the
> ceiling void (21,250 (A)); ~34 % market movement on copper components and agency labour rates
> (12,850 (A)); ~9 % consequential overhead under-recovery (3,500 (A)). Actions: clash resolution complete
> and signed off; copper components forward-ordered for remaining floors; agency reliance reduced from next
> rotation. Recovery to green expected over two months. Owners: design manager (rework); procurement lead
> (rates). Date: 14 July 2026.

### The board narrative

At the board, the entry becomes two spoken paragraphs — numbers, cause, forecast and action in one causal
arc (KA 4.3.3, 4.4.3), decision-ready and without spin:

> "CA-07 Clinical fit-out is red this month: USD 367,600 actual against a flexed budget of USD 330,000 —
> USD 37,600 adverse. About 57 % of that (USD 21,250) is rework: a design clash between the medical-gas
> containment and the ductwork in the ceiling void forced strip-out and re-installation along the ward
> corridors. About 34 % (USD 12,850) is market movement — copper prices and agency labour rates — and the
> remaining 9 % (USD 3,500) is overhead under-recovered on the extra hours: a consequence of the rework, not
> a separate problem.
>
> "The clash is resolved and signed off, so the rework component should not repeat. We have forward-ordered
> copper components for the remaining floors, which caps the price exposure, though agency rates remain a
> live risk we cannot fully control, and we have reflected that residual exposure in the EAC. We expect
> CA-07 back within tolerance over the next two months and will report the trend monthly until it is. The
> uncomfortable lesson is ours to own: the RFI trend flagged this clash a month early, and we have now added
> services-coordination RFIs to the leading-indicator panel with an amber trigger."

Nothing here is spin: the numbers are exact, the causes are attributed, the forecast is stated with its
uncertainty, and the team's own miss — the unescalated RFI trend — is on the table rather than under it.

### What the credential expects

This case is Domain 4 end-to-end, and the credential examines every step of it. The comparison was made
against a **flexed budget** (KA 4.2.2), so the volume of work done never contaminated the variance. The red
total was **decomposed** into price, usage, rate, efficiency and overhead components that reconcile to the
penny (KA 4.2.3) and were communicated as a **variance bridge** (KA 4.2.4). The components were then
**regrouped by cause, not by account**, each with an owner and an action, and reported by **exception** —
one red entry, not forty green pages (KA 4.3.5, 4.1.3). The board heard **numbers plus causal narrative plus
forecast plus action** (KA 4.3.3), illustrated honestly (KA 4.4). And the lasting lesson is the earliest one:
the design-clash RFI trend was a **leading indicator** (KA 4.1.2) visible a month before the cost landed —
the cheapest point of intervention is always before the variance exists. A candidate who can compute these
five variances, reconcile the bridge, attribute the causes and draft that exception entry has demonstrated
the professional core of this domain.

---

## Case study B — Domain 4: five projects, thirty minutes (energy utility portfolio)

*The first Domain 4 case study went deep on one control account. This one goes wide: a utility's monthly
performance pack across five capital projects, where the disciplines change scale but not nature — a
portfolio RAG dashboard with declared tolerances (KA 4.1.1), one red project decomposed flex-first
(KAs 4.2.2–4.2.3), a misleading chart caught before it reached the board (KA 4.4.2), and an exception
discipline that spends a board's thirty minutes where they change outcomes (KAs 4.1.3, 4.3.5; Advanced
4.A.4). Every figure reconciles.*

### Background

*Gridholm Networks*, a fictional energy utility, runs a rolling capital portfolio. This month's pack covers
five projects: **P1** a 132 kV substation refurbishment, **P2** an overhead-line renewal campaign, **P3** an
underground cable replacement, **P4** a smart-meter rollout and **P5** a peaking-plant turbine overhaul.
Each project's monthly cost KPI is its variance against a **flexed budget** for the month's actual output,
with portfolio-standard tolerances declared in advance (4.1.1): **green** within ±5 % of flexed standard
cost, **amber** beyond ±5 %, **red** beyond ±10 %. The portfolio board meets monthly for thirty minutes —
which is not a constraint to be lamented but the design assumption the whole pack is built around: under
management by exception (4.1.3), those minutes belong to the projects outside tolerance, and to nothing
else.

### The dashboard (KA 4.1.1)

The month's portfolio position, flexed budgets against actuals (USD 000):

| Project | Flexed budget | Actual | Variance | % of flexed | RAG |
|---|---:|---:|---:|---:|---|
| P1 Substation refurbishment | 820 | 807 | 13 (F) | 1.6 % | Green |
| P2 Overhead-line renewal | 640 | 692 | 52 (A) | 8.1 % | Amber |
| P3 Cable replacement | 450 | 513 | 63 (A) | 14.0 % | Red |
| P4 Smart-meter rollout | 380 | 371 | 9 (F) | 2.4 % | Green |
| P5 Turbine overhaul | 510 | 498 | 12 (F) | 2.4 % | Green |
| **Portfolio** | **2,800** | **2,881** | **81 (A)** | **2.9 %** | — |

The rows and totals reconcile ✓ — and the totals row is the dashboard's first lesson. At portfolio level
the month is **2.9 % adverse**: inside the ±5 % band, nominally green. A pack that reported only the
aggregate would have shown the board a calm month while P3 breached the red line, because three favourable
projects part-net against one failing one. **Aggregation is where exceptions go to hide** — the portfolio
row is context, never the verdict, and the RAG discipline is applied per project, against tolerances each
project's board agreed in advance rather than negotiated after the fact (4.1.1; Advanced 4.A.3's gaming
warning applies to tolerances as much as to targets).

### The red, decomposed — flex first (KAs 4.2.2, 4.2.3)

P3 laid **1,500 metres** of cable this month against an original phasing assumption of 1,400 m, so the
first discipline is the flex (4.2.2). The original monthly budget was `1,400 m × USD 300/m = 420,000`;
comparing actual cost with *that* would report `513,000 − 420,000 = 93,000` (A) — 22.1 % — of which
`100 m × 300 = 30,000` is simply the cost of doing more work. The budget is flexed to actual output at the
standard **USD 300/m** (materials 180 + labour 2.0 hours at 50 = 100 + plant absorbed 20):
`1,500 × 300 = ` **USD 450,000**, and the true variance is **USD 63,000 (A)** — 14.0 %, still red, but now
all performance, no volume.

**Worked decomposition — P3, this month.**

1. **Setup.** Flexed standards: materials **1,500 m at USD 180**; labour **3,000 hours at USD 50**; plant
   absorbed **USD 30,000** (20 × 1,500 m). Actuals: **1,590 m** of cable at **USD 185**; **3,300 hours** at
   **USD 54**; plant incurred **USD 40,650**. Actual total `294,150 + 178,200 + 40,650 = 513,000`.
2. **Formulae.** `Price/rate = (actual price − standard price) × actual quantity`;
   `usage/efficiency = (actual quantity − standard quantity) × standard price` (4.2.3);
   `plant = incurred − absorbed`.
3. **Substitution.**
   - Material price: `(185 − 180) × 1,590 = 7,950` **(A)**
   - Material usage: `(1,590 − 1,500) × 180 = 16,200` **(A)**
   - Labour rate: `(54 − 50) × 3,300 = 13,200` **(A)**
   - Labour efficiency: `(3,300 − 3,000) × 50 = 15,000` **(A)**
   - Plant: `40,650 − 30,000 = 10,650` **(A)**
4. **Result.** `7,950 + 16,200 + 13,200 + 15,000 + 10,650 = 63,000` **(A)** ✓ — reconciling to
   `513,000 − 450,000`. Element checks: materials `7,950 + 16,200 = 24,150 = 294,150 − 270,000` ✓; labour
   `13,200 + 15,000 = 28,200 = 178,200 − 150,000` ✓.
5. **Interpretation.** Regrouped by cause (4.3.1), five components become three: **ground conditions —
   USD 31,200 (A), ~50 %** (usage + efficiency: uncharted services forced hand-digging and re-routes, which
   lengthened cable runs and slowed crews — one event wearing two account codes, owner: the engineering
   manager, with a ground-survey action for the remaining sections); **market movement — USD 21,150 (A),
   ~34 %** (copper in the cable price, agency jointers in the labour rate, owner: procurement, partially
   mitigable by forward-ordering the remaining drums); and **consequential plant — USD 10,650 (A), ~17 %**
   (excavator hire extended by the slow going: a consequence, not a cause, needing no action of its own).

### The chart that nearly misled the board (KA 4.4.2)

P2's amber comes with a recovery claim. Its submission includes a bar chart of erection productivity —
poles per week over the last four weeks: **25.0, 25.5, 26.0, 26.5** — drawn with the y-axis starting at
**24.5**. On that axis the bars stand 0.5, 1.0, 1.5 and 2.0 units tall: the last bar is **four times** the
first, and the visual says *productivity has surged; no escalation needed*. The portfolio analyst applies
4.4.2's first check — a truncated axis on a bar chart — and redraws it zero-based with the standard rate
marked. The redrawn picture says something different: a real but modest **6 %** improvement, against a
required run-rate of **28 poles per week** to hold the flexed budget. The recovery is progress, not
recovery. P2 keeps its amber, with a dated trend checkpoint next month instead of the quiet downgrade to
green the original chart was drafted to support. Nobody had faked a number — every figure on the truncated
chart was correct — which is precisely 4.4.2's point: **the picture must tell the same truth as the
numbers**, and chart review is part of pack assurance, not decoration.

### Thirty minutes, spent by exception (KAs 4.1.3, 4.3.5; Advanced 4.A.4)

The pack the board receives is one dashboard page, one red exception entry, one amber watch note — and
three green status lines that earn no narrative at all (4.1.3). The exception entry, in the 4.3.5 form:

> **P3 Cable replacement — RED.** Month cost variance **USD 63,000 (A)** against a flexed budget of USD
> 450,000 (14.0 %; red threshold ±10 %). Causes: ~50 % ground conditions — uncharted services forcing
> hand-digging and re-routes (31,200 (A)); ~34 % market movement on copper and agency jointer rates
> (21,150 (A)); ~17 % consequential plant hire (10,650 (A)). Actions: ground-penetrating survey of
> remaining sections commissioned; remaining cable drums forward-ordered; EAC updated for residual rate
> exposure. Owners: engineering manager (ground); procurement lead (rates). Recovery to amber expected in
> two months.

The thirty minutes then spend themselves: roughly twenty on P3 — testing the attribution, approving the
survey cost against contingency and the revised EAC — and ten on P2's watch note and the portfolio trend.
The board does not re-work P3's variance arithmetic or re-open the site team's action list: in the cadence
pyramid of Advanced 4.A.4, the portfolio tier *tests* the analysis and takes the decisions only it can
take; re-litigating the tier below is management by exception inverted. And the greens cost nothing —
which is the entire economics of the discipline: attention flows to the 63,000 that needs a decision, not
the 2,800,000 that does not.

### What the credential expects

A candidate should be able to run this pack end-to-end at portfolio scale: set and read **KPIs with
targets, thresholds and tolerances** declared in advance, and explain why per-project RAG beats the
aggregate row — netting is where exceptions hide (4.1.1); **flex before decomposing**, quantifying the
30,000 volume effect the unflexed comparison would have mis-blamed (4.2.2); decompose the red into
price/rate, usage/efficiency and plant components that **reconcile exactly**, then regroup them by cause
with owners and actions (4.2.3, 4.3.1); catch a **truncated-axis** chart whose every number is true and
whose message is false, and redraw it against the required run-rate (4.4.2); and write the **exception
entry** that lets a board govern five projects in thirty minutes, each tier of the cadence pyramid deciding
at its own level (4.3.5; Advanced 4.A.4). On AI: anomaly detection can rank the portfolio's variances,
draft the decomposition and even flag distorted charts in a submitted pack — but tolerance-setting, cause
attribution and the decision to keep P2 amber are professional judgements, owned and signed (13.5.6):
**AI proposes; the professional verifies, decides and remains accountable.**

---

## Executive perspective — Domain 4

**What the executive must hold onto.** **Variance without cause is noise**: a total variance means nothing
until it is measured against a flexed budget (4.2.2), decomposed into price/rate and quantity/efficiency
components, and regrouped by cause with an owner and an action — anything less is a number to worry about,
not a decision to take. And **leading indicators buy the time that lagging ones cannot** (4.1.2): by the
time `CPI` falls, the cost is spent, so the cheapest point of intervention is always before the variance
exists. Management by exception (4.1.3) is how the board's scarce attention gets spent where it changes
outcomes.

**Six questions to ask from the chair.**

1. Was this variance measured against a flexed budget, or is the volume of work contaminating the number?
2. What are the causes, in proportions that reconcile to the total — and who owns the action on each?
3. Which leading indicator would have shown this a month earlier, and is it on the panel with a trigger now?
4. What does the trend say — is this a single red month, or the third of three?
5. What sits behind this favourable variance — a genuine saving, skipped scope, or cost deferred into next
   quarter?
6. What decision is this report asking us to take — and if none, why is it on the agenda?

**The traps at board level.**

- **Reading a favourable variance as good news.** A favourable cost or efficiency variance may be corners
  cut — a quality liability that returns as rework — while an adverse one may be the right decision, such as
  accelerating to protect a critical milestone; sign alone tells a board nothing (4.2.5).
- **The snapshot instead of the trend.** A single red month treated as a crisis, or a slow drift that stays
  just inside tolerance ignored for a year — thresholds catch levels, but only trend reporting catches
  direction.
- **The pack as reassurance.** Forty green pages, gameable KPIs and dashboards with no decision attached
  bury the one entry that matters; volume of reporting is not quality of control (4.3).
- **Persuasive pictures.** Truncated axes, unanchored dual axes and cherry-picked windows can make the same
  data tell opposite stories; a board that never questions the chart is delegating its judgement to the
  chart-maker (4.4.2).

**What good looks like.** The board receives a short, exception-driven pack: an integrated dashboard with
RAG status *and* trend, and an exception report in which every out-of-tolerance entry is causal, quantified,
actioned, owned and dated — the CA-07 pattern of the Domain 4 case study — rather than pages of undifferentiated
green. A leading-indicator panel sits beside the lagging measures, with amber triggers that escalate before
cost lands. Narratives pair numbers with cause, forecast and action, and include the team's own misses;
charts answer the question asked and distort nothing. Above all, meetings end in decisions, because that is
what the reporting was designed for (4.3.1).

---

## Calculation exercises — Domain 4

Work each exercise before reading its solution; every step uses only this domain's methods.

**Exercise 4.1** — A work package's budget assumes **8,000 units** of output at a variable cost of **USD 30
per unit**, plus **USD 100,000** of fixed cost. The month actually produces **9,000 units** at a total actual
cost of **USD 385,000**. Compute the original (unflexed) budget, the flexed budget for the actual output, and
the variance on each comparison — and show how much of the raw budget-to-actual gap is purely a volume effect.

**Solution 4.1.**

1. Original budget `= 100,000 + 30 × 8,000 = 340,000`.
2. Flexed budget (4.2.2) `= 100,000 + 30 × 9,000 = 370,000` — what the cost *should* have been for 9,000
   units.
3. Unflexed comparison: `385,000 − 340,000 = 45,000` **(A)**.
4. Flexed comparison: `385,000 − 370,000 = 15,000` **(A)** — the genuine price/efficiency variance.
5. Volume effect `= 370,000 − 340,000 = 30,000`: the extra 1,000 units *should* cost `30 × 1,000 = 30,000`
   more. Reconciliation: `30,000 + 15,000 = 45,000`. ✓

Without the flex, the team would be blamed for a USD 45,000 "overrun" of which USD 30,000 is simply more work
done.

**Exercise 4.2** — For a work package's actual output, standards and actuals are: **material** — standard
**2,000 kg at USD 12/kg**; actual **2,200 kg at USD 11/kg**. **Labour** — standard **1,500 hours at USD
36/hour**; actual **1,400 hours at USD 40/hour**. Decompose the total cost variance into material price,
material usage, labour rate and labour efficiency variances, and reconcile the four components to the total.

**Solution 4.2.**

1. Standard cost: material `2,000 × 12 = 24,000`; labour `1,500 × 36 = 54,000`; total **78,000**. Actual
   cost: material `2,200 × 11 = 24,200`; labour `1,400 × 40 = 56,000`; total **80,200**.
2. Material price `= (11 − 12) × 2,200 = 2,200` **(F)**; material usage `= (2,200 − 2,000) × 12 = 2,400`
   **(A)**. Total material `= 24,200 − 24,000 = 200` **(A)** `= 2,400 (A) − 2,200 (F)`. ✓
3. Labour rate `= (40 − 36) × 1,400 = 5,600` **(A)**; labour efficiency `= (1,400 − 1,500) × 36 = 3,600`
   **(F)**. Total labour `= 56,000 − 54,000 = 2,000` **(A)** `= 5,600 (A) − 3,600 (F)`. ✓
4. Grand total `= 80,200 − 78,000 = 2,200` **(A)** `= 200 (A) + 2,000 (A)`. ✓

The small headline variance hides offsetting causes — a cheaper but more wasteful material, and faster but
dearer labour — each with a different owner.

**Exercise 4.3** — A control account's budget cost is **USD 500,000**; its actual cost is **USD 527,500**.
Variance analysis attributes the gap to five components: material price **12,000 (A)**, material usage
**4,500 (F)**, labour rate **6,000 (A)**, labour efficiency **9,000 (A)** and overhead expenditure **5,000
(A)**. Build the five-step variance bridge from budget to actual with running totals, confirm it reconciles,
and state the net variance.

**Solution 4.3.**

| Step | USD | Running total |
|---|---:|---:|
| **Budget cost** | | 500,000 |
| Material price | 12,000 (A) | 512,000 |
| Material usage | 4,500 (F) | 507,500 |
| Labour rate | 6,000 (A) | 513,500 |
| Labour efficiency | 9,000 (A) | 522,500 |
| Overhead expenditure | 5,000 (A) | 527,500 |
| **Actual cost** | | **527,500** |

Net variance `= 12,000 − 4,500 + 6,000 + 9,000 + 5,000 = 27,500` **(A)**, and the bridge reconciles:
`500,000 + 27,500 = 527,500`. ✓ The bridge (4.2.4) shows the favourable usage variance *partly offsetting*
three adverse causes — the story a single "27,500 over" hides. The largest single driver (material price,
12,000) is where attention goes first.

**Exercise 4.4** — Budgeted fixed overhead is **USD 360,000** over a budgeted output of **12,000 units**.
Actual output is **12,500 units** and actual fixed overhead is **USD 372,000**. Derive the overhead
absorption rate (OAR), the overhead absorbed, and the fixed-overhead expenditure and volume variances — and
reconcile them to the total fixed-overhead variance.

**Solution 4.4.**

1. `OAR = 360,000 / 12,000 = USD 30/unit`.
2. `Absorbed = OAR × actual output = 30 × 12,500 = 375,000`.
3. Expenditure variance `= Budgeted overhead − Actual overhead = 360,000 − 372,000 = (12,000)` **(A)** —
   spent more than budget.
4. Volume variance `= Absorbed − Budgeted overhead = 375,000 − 360,000 = 15,000` **(F)** — 500 more units
   than planned, over-absorbing fixed cost.
5. Total `= Absorbed − Actual = 375,000 − 372,000 = 3,000` **(F)** `= 15,000 (F) − 12,000 (A)`. ✓

The net favourable total conceals an adverse spending problem rescued by higher volume — splitting the two
(4.2.4b) is what sends each to its owner.

**Exercise 4.5** — A package's original standard price for a key material is **USD 30 per unit**. During the
period, the market price of the material moved to **USD 33 per unit** (a general, documented market movement).
The project actually paid **USD 34 per unit** for **10,000 units**. (a) Compute the total price variance
against the original standard. (b) Split it into a planning variance and an operational variance (Advanced
4.A.2). (c) In one sentence: who should answer for each part?

**Solution 4.5.**

1. Total price variance `= (34 − 30) × 10,000 = 40,000` **(A)**.
2. Planning variance `= (33 − 30) × 10,000 = 30,000` **(A)** — the market moved; the original standard is
   stale.
3. Operational variance `= (34 − 33) × 10,000 = 10,000` **(A)** — paid above even the current market.
   Check: `30,000 + 10,000 = 40,000`. ✓
4. The planning variance belongs to whoever owns estimating assumptions and escalation provisions (Domain 3,
   Advanced 3.A.1) — it is information, not blame; the operational variance belongs to procurement — it is
   the controllable part, and judging buyers against the stale 30 rather than the current 33 would punish
   them for the market and hide the real 10,000 (Advanced 4.A.2; Goodhart, Advanced 4.A.3).

---

## Practitioner's toolkit — Domain 4

Adoption-ready artefacts; adapt the column headings and thresholds to your organisation, then keep them
stable.

### Toolkit 4.T.1 — Variance investigation form

One row per out-of-tolerance control account, completed before the exception report is drafted.

| Control account | Variance (F/A) | Flexed? | Split (rate/usage/mix) | Root cause | One-off or systemic | Action | Owner | Date |
|---|---:|---|---|---|---|---|---|---|
| CA-04 Fit-out — material | 7,200 (A) | Yes (4.2.2) | Rate 2,200 (A) / usage 5,000 (A) | Supplier price rise on the framework rate; cutting wastage above norm on site | Rate systemic until renegotiated; usage one-off (crew now briefed) | Renegotiate call-off rate; reinstate wastage checks at the saw bench | Procurement lead / site manager | 12 Jul |
| CA-04 Fit-out — labour | 8,200 (A) | Yes (4.2.2) | Rate 4,200 (A) / efficiency 4,000 (A) | Overtime premium to hold the milestone; learning curve on the new crew | Rate one-off (deliberate acceleration, now ended); efficiency systemic until crew up to speed | Confirm acceleration closed; pair new crew with supervisor for two weeks | Project manager | 12 Jul |

**Usage note.** The form enforces the KA 4.2 sequence: confirm the budget was **flexed** first (4.2.2), so
volume is not mistaken for inefficiency; then **split** the variance into rate and usage/efficiency (4.2.3),
because the two go to different owners with different remedies — the example rows carry the 4.2.3 work
package, where the USD 15,400 adverse total resolves into a USD 6,400 paying-more problem and a USD 9,000
using-more problem. The **one-off or systemic** column is the judgement the forecast depends on: it decides
which `EAC` assumption is defensible (3.4.2) and whether a favourable variance is a saving or a warning
(4.2.5). Completed forms feed the exception report (4.3.5) with cause, impact, action and owner already
attributed.

### Toolkit 4.T.2 — Report design checklist

Run against every recurring report before it is issued — and again whenever the format is changed.

- [ ] The audience and the **decision the report supports** are named before any content is chosen (4.3.1)
- [ ] **Status, direction, exceptions and actions** are all visible in one view — the four questions
      answered on one screen or page (4.3.2)
- [ ] Overview first, detail on demand — a drill-down exists behind every red (4.3.2)
- [ ] Every headline number is paired with a short **causal narrative** — why, so-what, now-what (4.3.3)
- [ ] **Trend** is shown, not just a snapshot — direction is where the early warning lives (4.1.2, 4.3.2)
- [ ] Exceptions carry **cause, impact and action**, each with a named owner (4.3.5)
- [ ] Greens are noted but unelaborated — attention concentrates on the reds and ambers (4.3.5)
- [ ] Cadence and level of detail match the audience's decision rhythm (4.3.4)
- [ ] **Axes are honest** — zero-based bars, no manufactured dual-axis correlations, consistent scales
      across small multiples, no cherry-picked windows (4.4.2)
- [ ] The chart matches the question — S-curve for *how are we tracking*, waterfall for *what moved the
      number* (4.4.1)

**Usage note.** This is the KA 4.3 discipline as a pre-issue gate: a report that passes all ten lets its
reader see where they are, where they are heading, what is off-track and what is being done about it — in
the time they have (4.3.1). The narrative and exception items (4, 6) are where AI-drafted commentary needs
the closest check, since a fluent draft can misattribute cause or bury a caveat — **AI proposes; the professional verifies, decides and remains accountable**. The honesty items (9, 10) hold the 4.4 standard that the picture must tell the same
truth as the numbers. Keep the checklist stable so failures are comparable across cycles; a recurring failure
on the same item is a design problem, not a drafting one.

---

## Exam preparation — Domain 4

**How this domain is examined.** Domain 4 pairs one heavily numerical knowledge area with three
interpretive ones: **recall** items test the KPI vocabulary (target, threshold, tolerance), the exception
principle and chart-to-question fit; **application** items decompose variances, build bridges and compute
fixed-overhead splits; **analysis** items test reading — a favourable variance's cause, a leading
indicator's warning, a distorted chart's manufactured story. The numerical items sit almost entirely in
KA 4.2: flexed budgets, price/usage and rate/efficiency splits, expenditure/volume variances and the
reconciling bridge. The sample MCQs and calculation exercises in this domain are drawn from the same
blueprint as — but kept strictly separate from — the live examination bank.

**Calculation traps.** The distractors in this domain's items punish specific, recurring mistakes:

- **Skipping the flex before decomposing** — comparing actual cost to the *original* budget folds a volume
  effect into the variance and blames the team for doing more work (exercise 4.1: a 45,000 "overrun" that is
  really 15,000).
- **Using actual instead of standard price in the usage variance** — the split is `(AP − SP) × AQ` for
  price and `(AQ − SQ) × SP` for usage; crossing the terms gives the 5,200 distractor, not 5,000
  (MCQs 4.2-A and 4.2-B).
- **Volume versus expenditure confusion in fixed overheads** — answering the expenditure variance when the
  volume variance is asked, or reporting the total; and getting the volume *sign* wrong — producing fewer
  units than plan under-absorbs and is adverse (MCQ 4.2-E; exercise 4.4).
- **Dropping the `(A)`/`(F)` signs in a bridge** — with offsetting components, an unsigned bridge fails to
  reconcile to actual (exercise 4.3).
- **Misreading favourable as good news** — a favourable variance can hide skipped scope or deferred cost;
  sign alone is never the answer to a cause question (MCQ 4.2-D).
- **Declaring a status without the tolerance** — `CPI` 0.97 against a 1.00 target is *within* a 0.95 amber
  threshold: watch, not escalate (MCQ 4.1-B).

**Time management.** Price/usage and rate/efficiency splits are fast once the two formulae are on paper;
bridges and overhead items need a running total and a sign check at each step. Write the formulae down
first — `(AP − SP) × AQ`, `(AQ − SQ) × SP`, `absorbed = OAR × actual output` — then substitute; the
distractors are built from crossed terms.

**Reflection questions.**

1. Which of last month's reported variances on your project were measured against a flexed budget, and
   which against the original phasing?
2. What leading indicator on your current project is quietly doubling — the equivalent of the RFI trend
   nobody escalated in the CA-07 case?
3. Which KPI on your dashboard could improve on paper without reality improving, and what would you pair it
   with to make that visible?
4. What decision did your last exception report ask its readers to take — and did the meeting actually take
   one?

---

## Domain 4 summary

Performance management makes measurement matter: KPIs paired with targets, thresholds and tolerances;
**leading** indicators to intervene in time and **lagging** indicators to confirm; and **management by
exception** to focus attention. **Variance analysis** decomposes the plan-to-actual gap into price/rate and
quantity/efficiency causes — against a **flexed budget** — and communicates them through a **variance bridge**,
so the right owner pulls the right lever. **Management reporting** designs for the decision and the audience,
integrates a **controls dashboard** with RAG status and trend, pairs numbers with causal narrative, and
reports by **exception**. **Data visualisation** matches chart to question — the S-curve and the waterfall
above all — and tells a true story without distortion. Together these close the finance group and lead
straight into earned value (Domain 6), where cost and schedule variance are formalised.

**Cross-references.** Management vs statutory reporting → 2.5; the Planned Value S-curve → 3.3; the EAC/VAC
forecast on the dashboard → 3.4, Domain 6; cost coding that makes aggregation automatic → 1.5; risk exposure
on the dashboard → Domain 12; automated commentary and NL querying → Domain 13, KA 13.5; chart-design craft →
the platform's dataviz guidance.

