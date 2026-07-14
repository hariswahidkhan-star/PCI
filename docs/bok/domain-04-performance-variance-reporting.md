# Domain 4 — Performance Management, Variance Analysis & Management Reporting

> **Group:** Finance, accounting & reporting (Domain 4 of 4). **Target:** ~115 pages.
> **Binds to:** [`00-style-spine.md`](00-style-spine.md). British English; USD (+SAR where useful); five-line
> worked examples; adverse amounts in parentheses.

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
- B. Within tolerance (green/watch) — 0.97 is above the 0.95 amber threshold. ✅
- C. Cannot be assessed without the schedule.
- D. Green — exactly on target.

*Rationale:* 0.97 is below the 1.00 target but above the 0.95 amber threshold and within the ±0.05 tolerance —
watch, not escalate. It is not red, not on target, and can be assessed from the cost KPI alone.

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
misses that a favourable variance hides skipped scope, misleads a board. **AI proposes, the professional
disposes.**

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
- B. Investigate the cause — a favourable variance can hide skipped scope, deferred cost or quality risk. ✅
- C. Increase the budget.
- D. Treat it as an error.

*Rationale:* Sign alone is not the story; a favourable variance may reflect skipped/deferred work that returns
later. Investigating the cause is the professional response; it is neither automatically a clean saving nor an
error.

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
external disclosure — so the human signs off. **AI proposes, the professional disposes.**

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
- A. Contains every available data point.
- B. Lets the reader see status, direction, exceptions and actions in the time available. ✅
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
story's integrity. **AI proposes, the professional disposes.**

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
- B. A y-axis that does not start at zero (truncated axis). ✅
- C. Using brand colours.
- D. A missing legend.

*Rationale:* A truncated y-axis exaggerates small differences on bar charts. Bar count, colour and legend do
not create that specific distortion.

**MCQ 4.4-B `[4.4.1 · Application]`** To explain *what drove* a cost result from budget to actual, the best
chart is a:
- A. Pie chart.
- B. Scatter plot.
- C. Waterfall (variance bridge). ✅
- D. 3-D column chart.

*Rationale:* A waterfall walks contributions from a start to an end value — exactly a variance bridge. Pies
show composition, scatters show correlation, and 3-D distorts.

### Self-check — KA 4.4

1. Which two chart types are the workhorses of project controls, and what question does each answer? *(S-curve
   — how are we tracking over time; waterfall/variance bridge — what moved the number.)*
2. Name three ways a visualisation can mislead with correct numbers. *(Truncated axis; dual axes;
   3-D/decoration; cherry-picked baseline; inconsistent small-multiple scales.)*

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

*Domain 4 is a first authored draft pending SME technical review before it feeds the exam blueprint. This
completes the finance group (Domains 1–4, ~40 % of the book).*
