# Domain 16 — Data, Automation and Responsible AI in Finance *(systematic AI treatment)*

> **Group:** Operating and the future (Domain 16 of 16, closing Part Four). **Target:** ~78 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). Every earlier domain carried an **AI in this KA** treatment; this
> domain is where the treatment becomes **arithmetic**. It composes registered symbols —
> `CFADS`, `DSCR`, `NPV`, `AF(r, n)`, `EMV`, the hundred-per-cent rule, the mesh-versus-layer
> interface count `n(n−1)/2`, and PML-AI's governance latency `E[wait] = M/2 + L` — and it
> proposes for the registry the six automation and assurance measures it derives: **cost per
> reviewed item**, **automation breakeven volume**, **total misclassification cost**,
> **break-even posterior**, **zero-failure validation sample size** and **revalidation
> interval**. Where a figure was derived earlier it is **cited, not re-derived**: `CFADS`
> **6,384,000**, debt service **5,009,635.23**, `DSCR` **1.2743**, the 1.20× covenant biting at
> `CFADS` **6,011,562** and the annual headroom of **372,438** come from Domain 10 (KA 10.1–10.2);
> the 3,250,352 debt-capacity consequence of a one-cell tax error and the year-twelve minimum
> `DSCR` of 1.1851 come from Domain 6 (KA 6.4.1); the working-capital treatment worth
> **600,000** of `CFADS` comes from Domain 2 (KA 2.3.1); model risk as a priced register line
> comes from Domain 11 (KA 11.4.3). British English; USD (+SAR where useful, indicative
> `USD 1 ≈ SAR 3.75`). Rates used throughout, stated once: a loaded analyst hour **USD 96.00**
> (**USD 1.60** a minute), a senior modeller or reviewer hour **USD 150.00**, a legal-and-finance
> specialist hour **USD 240.00**.

## Why this domain exists

Fifteen domains have each ended with a paragraph on where machine assistance earns its place and
where it must not go. Those paragraphs were judgments — well-founded, but judgments. They left one
question open, and it is the question a finance director actually has to answer: **at what point,
measured in money, does automating a piece of the finance function stop being a slide and start
being a decision that survives audit?** Nothing so far has priced an automation, chosen an alert
threshold, sized a validation suite, or stated what a human approval costs and what it buys. This
domain does all four, and it does them with the same decimal discipline the book applies to a debt
schedule, because an automation programme that cannot be costed cannot be governed.

The domain's central claim is that **responsible AI in finance is a quantitative discipline, and
the numbers usually point somewhere other than intuition.** Four results carry the chapter, and
each contradicts something a practitioner is likely to believe. Automation economics turn on the
**cost of the errors the automation makes**, not on the labour it displaces — so the more
consequential the work, the *more* volume automation needs before it pays, which is the opposite of
the usual argument for automating the important things first. An anomaly detector tuned to maximise
accuracy is tuned wrongly whenever a false positive and a missed error cost different amounts, and
the gap between the accuracy-optimal and the cost-optimal threshold is a real annual number
somebody is paying. Validation by testing proves far less than it appears to: the suite an
organisation can afford bounds the defect rate at a level that still admits hundreds of errors a
year at production volume, which is why continuous monitoring and human approval — not
pre-deployment testing — carry the assurance load. And the value of a control, like the value of a
loosened threshold, is a **marginal** quantity: a blanket four-eyes rule is close to
value-neutral, while the same rule above a derivable threshold is strongly positive.

Underneath all four sits the principle the suite has carried since its first page, now with an
arithmetic meaning rather than a rhetorical one: **AI proposes; the professional verifies, decides
and remains accountable.** Verification is not a moral posture in this domain. It is a line item
whose expected return, in every worked example below, runs between **eighteen and four hundred and
thirty times** its cost — and that is the finding a leader should take away, because it survives
disagreement about the technology.

**Learning objectives.** After this domain a candidate can: describe a financial-data spine and
price the architecture choice against a pairwise-reconciliation estate using the registered
interface count; compute the all-in cost per reviewed item for a manual and an automated process
including the cost of the errors each makes, and derive the breakeven volume, the volume at which
a specific asset does *not* justify automation, the automated error rate at which the case
reverses, and the consequence cost at which it reverses; explain why the breakeven rises with the
consequence of an error; read an anomaly detector as a classifier, compute precision, recall,
accuracy and total misclassification cost at several thresholds, identify the cost-minimising
threshold, demonstrate that it differs from the accuracy-maximising threshold, quantify the annual
cost of choosing the latter, and derive the break-even posterior probability and the marginal
precision test that locates the optimum; derive the number of scenarios required to surface a
failure mode of stated probability at stated confidence and explain why a reported worst case from
a generated set is a percentile rather than a stress; compute the probability that a multi-item
document extraction is entirely correct, derive the per-item accuracy a stated sweep confidence
requires, and defend a verification scope on the load-bearing subset; compute the real and apparent
saving from machine-assisted model building and price the review that is habitually omitted; test
an explanation for attributional completeness using the hundred-per-cent rule; derive the
zero-failure validation sample size for a stated confidence and defect-rate bound, adjust it for
one observed failure, and translate a validated defect rate into expected annual production errors;
identify and quantify label bias in a detector trained on a prior process's output, and size the
blind audit sample that measures it; build a model inventory tiered by expected annual loss and
derive each model's revalidation interval; price a managed against a private deployment and state
the breach consequence at which the answer flips; derive a dual-approval threshold from the
approver's cost and the loss it prevents, and compute the value destroyed by applying the control
blanket-fashion; price approval latency using the registered governance-latency formula; and set
out an AI governance frame — inventory, tiering, human approval, incident and rollback, disclosure
— that a lender's information covenants can be satisfied against.

**The master estate.** Kestrel Water SPC is in operation, with the structure Domains 10 and 15
established. Its sponsor group owns **six operating assets** and runs one finance function across
them, and that is the unit this domain analyses, because automation economics are a portfolio
question rather than a project question. The estate produces **56,400 consumption-and-billing
records a year** (Kestrel alone contributes **9,400**) that feed the revenue forecast, and
**48,000 payments and journal entries a year** of which **2.5 per cent** are genuinely erroneous.
Nine finance-relevant systems hold the data. A proposed financial-data spine costs **USD 900,000**
to build and **USD 140,000** a year to run; a separate review-automation platform commits
**USD 148,000** a year against a manual review pipeline costing **USD 13.60** a record in labour.
Against that estate the domain prices every judgment the earlier
fifteen made in words: Kestrel's covenant headroom of **USD 372,438** a year is the yardstick every
data-quality figure is measured against, because it is the amount of cash the project can lose
before a covenant fails, and a forecast error, a missed anomaly or a misread definition that
exceeds it is not an operational nuisance but a credit event.

---

## Knowledge Area 16.1 — Financial-data architecture, forecast automation and anomaly detection

*Topics: 16.1.1 the financial-data spine · 16.1.2 forecast automation and the economics of a
review · 16.1.3 anomaly detection as a classifier.*

### 16.1.1 The financial-data spine

**Definition.** A **financial-data spine** is a single governed layer through which every system
publishes and consumes financial facts, so that each fact has one authoritative representation.
Four properties make it a spine rather than a warehouse. **Grain** — every table states the level
at which one row is one fact (one meter reading, one invoice line, one period's `CFADS`), because
most reconciliation failures are grain mismatches rather than arithmetic errors. **Golden source**
— exactly one system is authoritative for each fact, named, and the others are derived.
**Lineage** — every reported figure can be traced to the transactions beneath it without human
reconstruction. And the property that matters most in a financing and is most often absent, the
**definitional layer**: the facility's defined terms are implemented **once**, as code, with a
clause reference, so that `CFADS` means in the data what it means in the document.

That last property is the finance-specific one, and it is why this topic is not an information
technology digression. Domain 2 (KA 2.3.1) showed Kestrel's `CFADS` moving by **600,000** on one
working-capital treatment, and Domain 10 (KA 10.1.1) showed that the same trading year therefore
supports a `DSCR` of either 1.39 or 1.27. In a nine-system estate with no definitional layer,
`CFADS` is implemented as many times as it is needed, each implementation drifts, and the
organisation discovers the drift when two papers disagree in front of a credit committee.

**Worked example 16.1.1 — pricing the architecture, then pricing the thing that actually matters.**

1. **Setup.** Nine finance-relevant systems: the billing platform, the meter-data historian, the
   general ledger, the treasury system, the fixed-asset register, the payroll system, the
   procurement and payables system, the model repository and the lender-reporting pack. Today each
   pair that must agree is reconciled directly. Each **pairwise** reconciliation consumes **12
   hours a month** (a monthly tie-out plus investigation of differences); a **spine feed** consumes
   **4 hours a month** (a load and an exception report). The loaded analyst hour is **USD 96.00**.
   The spine costs **USD 900,000** to build and **USD 140,000** a year to run. Appraise it at the
   board's **8 per cent** over **10 years**.
2. **Formula.** The registered interface count (PML-AI Domain 4, "mesh versus layered interfaces"):
   a mesh needs `n(n−1)/2` pairwise interfaces where a layer needs `n`. Annual cost = interfaces ×
   hours a month × 12 × rate. Then `NPV = net annual saving × AF(0.08, 10) − build`.
3. **Substitution.** Mesh interfaces `9 × 8 / 2 = 36`; spine feeds `9`. Mesh
   `36 × 12 × 12 × 96.00`; spine labour `9 × 4 × 12 × 96.00`, plus the 140,000 run cost.
   `AF(0.08, 10) = 6.710081`.
4. **Result.** Mesh **5,184 hours a year**, costing **USD 497,664**. Spine **432 hours a year**,
   costing **USD 41,472** of labour and **USD 181,472** all-in. Net annual saving **USD 316,192**;
   simple payback **2.8464 years**; **`NPV` = 316,192 × 6.710081 − 900,000 = +USD 1,221,674**.
5. **Interpretation.** The case passes, but read *why* it passes, because the reason is the
   convexity rather than the level. The mesh grows as the square of the estate: adding a tenth
   system adds **9** pairwise reconciliations — **1,296 hours** and **USD 124,416** a year —
   against **48 hours** and **USD 4,608** for one more spine feed, a ratio of **27 to 1**. At
   fifteen systems the mesh needs **105** reconciliations and **USD 1,451,520** a year against the
   spine's **USD 69,120**. So the spine is not a labour saving on today's estate; it is the only
   architecture whose cost is linear in an estate that will grow, and the appraisal should be run
   at the estate size the organisation expects rather than the one it has. Two cautions, and the
   second is the professional one. **The saving is only realised if the pairwise reconciliations are
   actually retired** — estates routinely acquire a spine and keep the reconciliations, which
   converts a 316,192 saving into a 181,472 cost — the spine's own 41,472 of feed labour plus its
   140,000 run cost, paid on top of an unchanged mesh — and that failure is a governance failure
   rather than a technical one. And **the labour case is not the case that matters.** A single definitional
   divergence in `CFADS` is worth **600,000** of reported cash (Domain 2, KA 2.3.1) — **1.6110
   times** Kestrel's entire annual covenant headroom of 372,438 — and a definition implemented
   wrongly in code does not misreport once: it misreports every period until somebody finds it, so
   over the same ten-year appraisal it is worth `600,000 × AF(0.08, 10) =` **USD 4,026,049** in
   present value, **3.2955 times** the 1,221,674 the hours produced. The business case that gets built is the labour one;
   the business case that decides is the definitional one. State both, and be honest that the
   second is the reason.

### 16.1.2 Forecast automation and the economics of a review

**Definitions.** **Forecast automation** replaces a manual cycle — pull the period's consumption
and billing records, screen them for plausibility, correct what is wrong, re-run the revenue
forecast — with a pipeline that ingests, screens, refers exceptions to a person, and publishes.
**Cost per reviewed item** is the all-in cost of processing one record, and it has two parts that
must both be counted: the **processing cost** (labour, platform, inference) and the **error cost**,
being the residual undetected-error rate multiplied by the consequence of an undetected error. A
comparison that omits the second part is not a comparison.

**Worked example 16.1.2 — the breakeven volume, and why it moves the wrong way.**

1. **Setup.** The estate's consumption-and-billing pipeline. **Manual:** **8.5 minutes** a record
   at **USD 1.60** a minute; the manual process leaves **1.80 per cent** of records with an
   undetected error; an undetected error costs **USD 320** (re-billing, credit notes, the
   downstream forecast correction and the reconciliation to the lender pack). **Automated:**
   committed fixed cost **USD 148,000** a year (licence, platform, model maintenance, monitoring
   and the governance overhead this domain later specifies); **USD 0.50** a record of platform and
   inference; **5.00 per cent** of records referred to a person for adjudication at **6.75
   minutes** each (adjudication is faster than full review because the pipeline presents the
   exception with its context); residual undetected-error rate **2.60 per cent** — *higher* than
   the manual process, which is the assumption most automation cases quietly omit.
2. **Formula.** Cost per item = processing cost + (undetected-error rate × consequence). Total
   annual cost = fixed + volume × cost per item. Breakeven volume `V*` where the two totals are
   equal: `V* = fixed ÷ (manual cost per item − automated cost per item)`.
3. **Substitution.** Manual `8.5 × 1.60 = 13.60`, plus `0.018 × 320 = 5.76`. Automated
   `0.50 + 0.05 × (6.75 × 1.60) = 0.50 + 0.54 = 1.04`, plus `0.026 × 320 = 8.32`. Advantage
   `19.36 − 9.36`. Breakeven `148,000 ÷ 10.00`.
4. **Result.** Manual **USD 19.36** a record; automated **USD 9.36** a record of variable cost;
   advantage exactly **USD 10.00**; **breakeven 14,800 records a year**. At Kestrel's own **9,400**
   records the automation costs **USD 54,000 a year more** than the manual process. Across the six-asset
   portfolio's **56,400** records it saves **USD 416,000 a year** (manual 1,091,904 against
   675,904 all-in, or **USD 11.9841** a record).
5. **Interpretation.** Three readings, in ascending order of usefulness. **First, the answer is an
   estate answer, not an asset answer.** The same platform is a 54,000 annual loss at one project
   and a 416,000 annual gain at six, and no amount of enthusiasm at the project changes that;
   the project's honest options are to wait for the estate, to buy a service priced per record
   rather than per year, or to decline. Automation cases fail at the asset level far more often
   than they fail technically. **Second, the error-cost term is where the case is decided, and
   omitting it flatters automation.** Ignore both error rates and the advantage rises to
   `13.60 − 1.04 = 12.56`, so the breakeven appears at **11,783 records** — **3,017 records**, or
   **20.3822 per cent**, lower than the truth. The sensitivities are worth carrying in the head: at
   the portfolio's 56,400 records the case survives an automated undetected-error rate up to
   **4.9050 per cent** (against the 2.60 modelled, so it tolerates nearly double) and a consequence
   cost up to **USD 1,241.99** an error. **Third — and this is the result that changes behaviour —
   the breakeven rises with the consequence of an error.** Reprice an undetected error at
   **USD 1,200** rather than 320 and both processes get dearer, but the automated one gets dearer
   faster, because its error rate is the higher of the two: manual **35.20**, automated **32.24**,
   advantage only **2.96**, and the **breakeven moves to 50,000 records a year**. At the
   portfolio's 56,400 the whole programme is then worth **USD 18,944** a year — arithmetically
   positive, professionally indistinguishable from nil. The intuition to abandon is "automate the
   important work first". The defensible rule is the reverse: **automate the high-volume,
   low-consequence work, and spend the freed capacity on the low-volume, high-consequence work
   where a human error rate of 1.80 per cent beats a machine's 2.60.** The caution that belongs
   beside every such case: both error rates are *measured* quantities, and an organisation that has
   not measured its manual error rate has no automation case at all, only an aspiration with a
   spreadsheet.

> **Fig 16.1.2 — Where automation starts to pay, and what the error cost does to the answer.**
> Line chart, x-axis annual review volume 0–70,000 records, y-axis net annual saving from
> automation (USD −220k to +780k). Three lines all begin at **−148,000** (the committed automated
> fixed cost) and rise at the per-record advantage: slate dashed at 12.56 a record (error cost
> ignored) crossing zero at **11,783**; brand blue at 10.00 (USD 320 an undetected error) crossing
> at **14,800**; crimson at 2.96 (USD 1,200 an undetected error) crossing at **50,000**. Dashed
> vertical markers at **Kestrel alone, 9,400 records** (the blue line at **−54,000**) and at the
> **six-asset portfolio, 56,400 records** (blue **+416,000**, crimson only **+18,944**). Footer
> states that raising the consequence of an undetected error from 320 to 1,200 moves the breakeven
> from 14,800 to 50,000 because the automated error rate is the higher of the two. Source: PCI
> original. Alt text: three straight lines rising from a common negative starting point and
> crossing the zero line at progressively higher review volumes as the assumed cost of an
> undetected error increases.

**The forecast-quality translation.** The pipeline's purpose is a better forecast, and forecast
quality has to be expressed in covenant terms to mean anything to a board. Measured over the
estate's last three years, the manual cycle's revenue forecast carried a mean absolute percentage
error of **4.8 per cent** and the automated pipeline's **3.1 per cent**. On Kestrel's `CFADS` of
**6,384,000** those are error bands of **USD 306,432** and **USD 197,904** — **82.2773 per cent**
and **53.1374 per cent** of the annual covenant headroom of 372,438. That is the sentence that
belongs in the paper: the improvement is not "1.7 points of accuracy", it is **the difference
between a forecast band that nearly consumes the covenant headroom and one that consumes half of
it.** Note also what the comparison does not license — a lower average error does not mean a
smaller tail, and the covenant is tested in periods (Domain 10, KA 10.2.1), so the figure to
monitor is the worst period's error, not the mean.

### 16.1.3 Anomaly detection as a classifier

**Definitions.** An anomaly detector assigns each item a score and raises an **alert** above a
threshold. Four counts follow: **true positives** (`TP`, real errors caught), **false positives**
(`FP`, clean items investigated), **false negatives** (`FN`, real errors missed) and **true
negatives** (`TN`). From them: **recall** = `TP/(TP + FN)`, the share of real errors caught;
**precision** = `TP/(TP + FP)`, the share of alerts that are real; **accuracy** =
`(TP + TN)/total`, the share of all items classified correctly. And the measure that should govern
the choice of threshold, which none of those three is: **total misclassification cost** =
`FP × cost of a false positive + FN × cost of a false negative`.

**Worked example 16.1.3 — the threshold, chosen properly.**

1. **Setup.** The estate's payment-and-journal detector: **48,000** items a year, of which
   **2.5 per cent — 1,200** are genuinely erroneous and **46,800** are clean. Investigating a false
   positive costs **USD 40** (25 minutes of an analyst). A missed error costs **USD 320**. Five
   candidate thresholds have been measured on a held-out year.
2. **Formula.** Recall, precision and accuracy as defined; total cost `= FP × 40 + FN × 320`. The
   decision rule for one marginal item: act when the probability the item is a real error exceeds
   the **break-even posterior** `= cost of a false positive ÷ cost of a false negative`.
3. **Result.**

   | Threshold | `TP` | `FP` | `FN` | Recall | Precision | Accuracy | Alerts | **Total cost** |
   |---|---|---|---|---|---|---|---|---|
   | T1 (score ≥ 0.90) | 600 | 110 | 600 | 50.0 % | 84.5070 % | 98.5208 % | 710 | 196,400 |
   | T2 (≥ 0.80) | 720 | 205 | 480 | 60.0 % | 77.8378 % | **98.5729 %** | 925 | 161,800 |
   | T3 (≥ 0.70) | 840 | 400 | 360 | 70.0 % | 67.7419 % | 98.4167 % | 1,240 | 131,200 |
   | T4 (≥ 0.60) | 960 | 950 | 240 | 80.0 % | 50.2618 % | 97.5208 % | 1,910 | **114,800** |
   | T5 (≥ 0.45) | 1,080 | 2,900 | 120 | 90.0 % | 27.1357 % | 93.7083 % | 3,980 | 154,400 |

4. **Result, stated.** Total cost is minimised at **T4 — USD 114,800 a year**. Accuracy is
   maximised at **T2 — 98.5729 per cent**, where the total cost is **USD 161,800**. Choosing the
   accuracy-maximising threshold therefore costs **USD 47,000 a year**.
5. **Interpretation.** The headline is the one the spine of this domain demands: **maximising
   accuracy is the wrong objective whenever a false positive and a false negative cost different
   amounts**, and here they differ eight-fold. Accuracy weights every item equally, so it is
   dominated by the 46,800 clean items and rewards a detector for leaving them alone; cost weights
   each item by its consequence, and a missed error costs eight investigations. The 47,000 is not
   an abstraction — it is an annual sum the finance function is paying for a metric choice nobody
   minuted. Three further readings do the professional work. **The optimum looks bad on
   precision.** At T4 only **50.2618 per cent** of alerts are real, so the team working the queue
   experiences a coin flip and will lobby to tighten the threshold; the leader's job is to explain
   that a queue which is half false is the *correct* queue at these costs, and to resource the
   **1,910 alerts** — at 25 minutes each, about **795.83 hours** a year, or **USD 76,400** of
   analyst time — rather than tune the model to make the queue feel better. **The decision rule is marginal, not average.** The break-even
   posterior is `40 ÷ 320 = 12.5 per cent`: act on an item whenever there is at least a **one in
   eight** chance it is real. Average precision at every threshold in the table exceeds 12.5 per
   cent, including T5's 27.1357 per cent — yet T5 is worse than T4, because what matters is the
   quality of the **additional** alerts a looser threshold buys. From T3 to T4 the step adds 120
   real errors and 550 false ones: marginal precision **17.9104 per cent**, above 12.5, benefit
   `120 × 320 = 38,400` against cost `550 × 40 = 22,000`, net **+16,400** — take it. From T4 to T5
   the step adds 120 real and **1,950** false: marginal precision **5.7971 per cent**, benefit
   38,400 against cost **78,000**, net **−39,600** — refuse it. The optimum is where marginal
   precision crosses the break-even posterior, and every step's net exactly reconciles to the
   change in the cost column, which is the arithmetic check to run on any such table. **And the
   two cost inputs are the real argument.** They are estimates, and the threshold is a function of
   their ratio alone, so the productive discussion in the room is "what does a missed error
   actually cost us?" rather than "which model is best". Change the ratio to 1:4 and the optimum
   tightens to T3; change it to 1:20 and it loosens to T5, the last threshold measured. State the
   ratio, own it, and revisit it when the consequence changes — for instance when the same detector begins to screen items that
   feed a covenant certificate rather than a management report.

> **Fig 16.1.3 — The accuracy-maximising threshold is not the cost-minimising threshold.**
> Combination chart over the five thresholds of the table. Bars (left axis, USD 0–220k) show total
> annual misclassification cost — 196,400 · 161,800 · 131,200 · **114,800 in crimson (the cost
> minimum)** · 154,400 — each labelled with its score cut, recall, precision and alert count. A
> crimson dashed line on a right-hand axis (92–99.6 %) shows accuracy — 98.5208 · **98.5729 (the
> peak, at T2)** · 98.4167 · 97.5208 · 93.7083 — so the two optima sit two thresholds apart, with
> a dashed connector annotating the **47,000 a year** difference. Footer states the break-even
> posterior of 40 ÷ 320 = 12.5 per cent and the marginal precisions of 17.9104 per cent (T3→T4)
> and 5.7971 per cent (T4→T5). Source: PCI original. Alt text: descending then rising bars of
> total annual cost with a separate accuracy line that peaks at a different threshold from the one
> where cost is lowest.

### AI in this KA

**Where it earns its place.** Everything in this knowledge area is machine work by construction,
and the highest-value application is the one organisations skip: using the pipeline's own output to
**measure** the quantities the business case assumed. The manual error rate, the automated error
rate, the referral rate, the adjudication time and the realised consequence of an undetected error
are all observable once the pipeline is instrumented, and a business case that is re-run on
measured inputs each year is a governed automation rather than a purchased one. Machine assistance
also belongs in the threshold work — scoring a held-out year at fifty thresholds and tabulating
cost is trivial for a machine and tedious for a person.

**Where it must not go.** No model may set its own operating threshold, because the threshold
encodes the organisation's view of what a missed error costs, and that is a judgment with an owner.
Nor may a detector's alert be closed by the same system that raised it: adjudication is the human
step that converts a score into a finding, and a pipeline that both flags and clears has no control
in it at all (16.4.2 prices exactly this). And no automation business case may be approved on
processing costs alone — the omission of the error-cost term is the specific, nameable defect that
makes automation look 20.3822 per cent better than it is.

**Verification, concretely.** Recompute one period's cost per item by hand from the loaded rate,
the minutes and the measured error rate, and require agreement to the cent. Confirm that every
threshold row's total cost reconciles to the previous row's plus the marginal benefit less the
marginal cost — an arithmetic identity that catches transcription errors instantly. Confirm the
prevalence used in the table against a blind audit rather than against the detector's own labels
(16.3.3 shows what happens when this is skipped). Reconcile the pipeline's published forecast to
the ledger for one period, line by line, and express the residual as a percentage of covenant
headroom rather than of revenue. **AI proposes; the professional verifies, decides and remains
accountable.**

### Key terms — KA 16.1

| Term | Meaning |
|---|---|
| **Financial-data spine** | A single governed publishing layer; `n` feeds in place of `n(n−1)/2` reconciliations. |
| **Grain** | The level at which one row is one fact; most reconciliation failures are grain failures. |
| **Golden source** | The one system authoritative for a fact; all others are derived. |
| **Definitional layer** | The facility's defined terms implemented once, as code, with clause references. |
| **Cost per reviewed item** | Processing cost + (undetected-error rate × consequence of an error). |
| **Automation breakeven volume** | Committed fixed cost ÷ the per-item cost advantage. |
| **Recall / precision / accuracy** | Real errors caught ÷ real errors · real alerts ÷ alerts · all correct ÷ all items. |
| **Total misclassification cost** | `FP` × false-positive cost + `FN` × false-negative cost; the objective to minimise. |
| **Break-even posterior** | False-positive cost ÷ false-negative cost; the probability above which acting pays. |
| **Marginal precision** | Additional true positives ÷ additional alerts from loosening a threshold. |

### Sample MCQs — KA 16.1

**MCQ 16.1-A `[16.1.2 · Application]`** Manual review costs 13.60 a record and leaves 1.80 % of
records with an undetected error; automated processing costs 1.04 a record and leaves 2.60 %; an
undetected error costs 320; the automated platform's committed fixed cost is 148,000 a year. The
breakeven volume is:
- A. 11,783 records a year
- B. 14,800 records a year ✅
- C. 15,812 records a year
- D. 142,308 records a year

*Rationale:* All-in costs are `13.60 + 0.018 × 320 = 19.36` and `1.04 + 0.026 × 320 = 9.36`; the
advantage is 10.00 and `148,000 ÷ 10.00 = 14,800`. A omits the error-cost term from both sides
(advantage 12.56, `148,000 ÷ 12.56 = 11,783`) — the specific defect of 16.1.2. C divides the fixed
cost by the automated all-in cost per record (`148,000 ÷ 9.36`) instead of by the advantage,
treating the comparison as an absorption problem rather than a differential one. D makes the same
error against the automated *processing* cost alone (`148,000 ÷ 1.04`), ignoring the manual
alternative entirely.

**MCQ 16.1-B `[16.1.2 · Analysis]`** An organisation reprices an undetected error from USD 320 to
USD 1,200 on the same two processes — manual review 13.60 a record leaving 1.80 % of records with an
undetected error, automated processing 1.04 a record leaving 2.60 %, on a committed fixed cost of
148,000 a year. The breakeven volume:
- A. falls, because errors are now more expensive and automation catches more of them
- B. is unchanged, because the error rates are unchanged
- C. rises from 14,800 to 50,000, because the automated process has the higher error rate so a larger consequence erodes its advantage ✅
- D. becomes irrelevant, because at a high enough consequence neither process is acceptable

*Rationale:* Manual becomes `13.60 + 0.018 × 1,200 = 35.20` and automated
`1.04 + 0.026 × 1,200 = 32.24`; the advantage falls from 10.00 to 2.96 and `148,000 ÷ 2.96 =
50,000`. A assumes the automation is the more accurate process, which the given rates contradict.
B confuses the rates with their monetised effect. D is a governance observation, not the arithmetic
asked for — and at 56,400 records the automation is still positive, by 18,944.

**MCQ 16.1-C `[16.1.3 · Analysis]`** A detector's five thresholds give accuracies of 98.5208,
98.5729, 98.4167, 97.5208 and 93.7083 per cent and total misclassification costs of 196,400,
161,800, 131,200, 114,800 and 154,400. The threshold that should be operated, and why:
- A. the second — it maximises accuracy, which is the standard classification metric
- B. the fifth — it maximises recall, and missing an error is the expensive outcome
- C. the fourth — it minimises total cost, which is the only objective that reflects the different consequences of the two error types ✅
- D. the first — its precision of 84.5070 % gives the investigation team the most reliable queue

*Rationale:* Accuracy weights all items equally and is dominated by the 46,800 clean ones; cost
weights each by consequence, and the minimum is 114,800 at T4 — choosing T2 on accuracy costs
47,000 a year. B over-corrects: T5's extra 120 catches cost 78,000 of investigation to save
38,400. D optimises the queue's comfort rather than the firm's loss.

**MCQ 16.1-D `[16.1.3 · Analysis]`** A false positive costs 40 and a missed error 320. Loosening
the threshold from T4 to T5 would add 120 true positives and 1,950 false positives. The correct
conclusion is:
- A. accept — the marginal alerts still have 27.1357 % precision overall, well above the 12.5 % break-even
- B. refuse — marginal precision is 5.7971 %, below the 12.5 % break-even, and the step costs 78,000 to save 38,400 ✅
- C. accept — recall rises from 80 % to 90 %, and recall is the measure lenders ask about
- D. indeterminate without knowing the anomaly base rate

*Rationale:* The test is on the *marginal* alerts: `120 ÷ (120 + 1,950) = 5.7971 %`, and the net is
−39,600, which reconciles exactly to the cost column rising from 114,800 to 154,400. A quotes T5's
*average* precision, which is the standing error in threshold decisions. C treats a ratio as an
objective. D is false — the counts given already embed the base rate.

### Self-check — KA 16.1

1. *Why does the automation breakeven rise as the consequence of an undetected error rises?* —
   Because the automated process has the higher residual error rate, so a larger consequence
   penalises it more than the manual process, eroding the per-item advantage: 14,800 records at
   USD 320 an error, 50,000 at USD 1,200.
2. *State the one-line decision rule for acting on an alert, and the test that locates the optimal
   threshold.* — Act when the probability the item is real exceeds the false-positive cost divided
   by the false-negative cost (here 12.5 per cent, one in eight); the optimum is where the
   **marginal** precision of loosening crosses that figure, not where average precision or accuracy
   looks best.
3. *What does a data spine actually buy in a financing, beyond hours?* — One implementation of each
   defined term, so `CFADS` means in the data what it means in the document; the 600,000
   definitional divergence of Domain 2 is 1.6110 times Kestrel's annual covenant headroom, and
   because a mis-implemented definition recurs every period it is worth 4,026,049 in present value
   over the ten-year appraisal — 3.2955 times the whole labour case.

---

## Knowledge Area 16.2 — Scenario generation, document review and model assistance

*Topics: 16.2.1 scenario generation and the coverage question · 16.2.2 document review and the
load-bearing subset · 16.2.3 model assistance and the verification that must replace the build.*

### 16.2.1 Scenario generation and the coverage question

**Definition.** **Scenario generation** is the machine production of many coherent states of the
world — price paths, demand paths, availability profiles, macro sets — for evaluation through the
financial model. It is genuinely valuable and genuinely easy to misreport, because a generated set
answers a different question from a **defined case**. Domain 6 (KA 6.4.2) distinguished sensitivity
from scenario and Domain 7 built the revenue stresses; neither is re-derived here. What is new is
the question a generated set forces: **how many scenarios, and what does the worst one mean?**

**Worked example 16.2.1 — coverage, and the worst case that is not a stress.**

1. **Setup.** An analyst reports: "we ran 40 scenarios; three breached the covenant, and our worst
   case shows a `DSCR` of 1.04." A second analyst reports a 500-scenario run and quotes its minimum
   `DSCR` as the downside case. Assess both statements. Assume each scenario independently has a
   **5 per cent** probability of containing the failure mode of interest.
2. **Formula.** Probability that a set of `k` independent scenarios contains **at least one**
   instance of a mode of probability `p`: `1 − (1 − p)ᵏ`. The `k` needed for a stated detection
   confidence `1 − α`: `k ≥ ln(α) ÷ ln(1 − p)`. The expected percentile of the minimum of `k`
   draws: approximately `1 ÷ (k + 1)`.
3. **Substitution.** `1 − 0.95⁴⁰`; then `ln(0.05) ÷ ln(0.95)`; then `ln(0.10) ÷ ln(0.99)`; then
   `1 ÷ 501`.
4. **Result.** A 40-scenario set contains at least one instance of a 5 per cent mode with
   probability **0.871488** — so "three of our forty breached" is close to the expected outcome and
   carries almost no information. To be **95 per cent** confident of surfacing a 5 per cent mode
   requires **59** scenarios; to be **90 per cent** confident of surfacing a **1 per cent** mode
   requires **230**. The minimum of 500 draws sits at about the **0.1996th percentile**.
5. **Interpretation.** Two errors are being made, and they are opposite. The first analyst has
   **under-covered and over-interpreted**: forty scenarios cannot demonstrate the absence of a
   1-in-100 mode (which needs 230 for even 90 per cent confidence), and the presence of three
   breaches is unsurprising rather than alarming. The second has **mislabelled a percentile as a
   stress**: the minimum of 500 draws is a 1-in-500 event whose value depends almost entirely on
   the tail of the input distributions, which is the part of a generated model nobody has
   validated. A defined bank case is a different object — it is a *chosen* set of assumptions with
   an owner, which is exactly why lenders insist on it (Domain 6, KA 6.4.1b: the flat case is
   demanded not because anyone believes it but because it is the case that finds the defect). The
   professional discipline follows in three parts. **Report `k` with every scenario statement**, and
   report the *share* of scenarios that breach rather than the count, because a count without `k` is
   meaningless. **Choose `k` from the smallest probability you need to see**, not from a run time —
   `ln(α) ÷ ln(1 − p)` takes ten seconds and settles the argument. And **never let a generated
   minimum replace a defined case in a credit paper**; present both, labelled, with the generated
   set describing shape and the defined case carrying the covenant test. The standing caution: the
   independence this arithmetic assumes is usually false. Scenarios drawn from one correlated
   generator explore less of the space than their count suggests, so `k` computed this way is a
   **floor**, and Domain 11 (KA 11.4.1) already priced what a correlation assumption is worth when
   it is wrong.

### 16.2.2 Document review and the load-bearing subset

**Definition.** **Machine document review** extracts structured facts — defined terms, thresholds,
test dates, notice periods, caps — from long agreements. On a project financing this is among the
most valuable applications available, because the documents are long, the terms are numerous and
the consequences of missing one are quantified elsewhere in this book. It is also the application
with the most misleading performance statistics, and the reason is combinatorial.

**Worked example 16.2.2 — why 92 per cent per item is worthless, and what to do instead.**

1. **Setup.** A facility agreement contains **340** defined terms. **26** of them are
   *load-bearing*: they feed the model, a covenant test or a reported ratio. An extraction tool is
   measured at **92 per cent** per-item accuracy. The question a reviewer must answer is not "how
   accurate is the tool?" but "what is the probability the deliverable is right?"
2. **Formula.** For `m` independent items each correct with probability `a`, the probability that
   **all** are correct is `aᵐ`. The per-item accuracy required for a stated sweep confidence `c` is
   `a ≥ c^(1/m)`.
3. **Substitution.** `0.92²⁶`; then `0.95^(1/26)`.
4. **Result.** The probability that all 26 load-bearing definitions are extracted correctly is
   **0.114415** — so there is an **88.5585 per cent** chance that at least one is wrong. Across all
   340 terms the expected number of errors is **27.2**. To be **95 per cent** confident of a clean
   sweep of 26 items, per-item accuracy must reach **99.8029 per cent**.
5. **Interpretation.** This is the single most important arithmetic in the domain for anyone
   tempted to rely on an extraction, and it generalises: **per-item accuracy is the wrong statistic
   whenever the deliverable requires every item to be right.** A tool reported as "92 per cent
   accurate" delivers a correct 26-item summary about one time in nine. And the required 99.8029
   per cent is not a procurement target — no honest vendor will offer it, and no buyer should
   believe it — so the resolution is not a better model. It is a **change of scope**: do not
   require the machine to be right, require the load-bearing subset to be **verified**. Price that.
   Twenty-six definitions at half an hour each of a legal-and-finance specialist at USD 240.00 an
   hour is **USD 3,120**. Set it against one known consequence: Domain 2's working-capital
   treatment of `CFADS` is worth **600,000** of reported cash, which is **1.6110 times** Kestrel's
   annual covenant headroom of 372,438, and the verification is **192.3077 times** cheaper than
   that single error. The professional posture that follows is precise rather than pious. Let the
   tool read all 340 terms and produce the register — that is real and large value, because a human
   reading 340 terms will also miss some and will take days. Then **verify the 26 against the
   document, by a named person, with the date recorded**, and treat the other 314 as indicative
   until one of them becomes load-bearing. Two cautions. **The subset must be chosen before the
   tool runs**, or the tool's own confidence scores will choose it, and a model is least confident
   where it is wrong in ways it can detect — not where it is wrong in ways it cannot. And
   **superseded drafts are the commonest failure mode in practice**, not misreading: an extraction
   from the wrong version is 100 per cent accurate against the wrong document, which no accuracy
   statistic can see. Case study B is that failure, priced.

### 16.2.3 Model assistance and the verification that must replace the build

**Definition.** **Model assistance** is machine help in constructing a financial model — formula
generation, schedule construction, restructuring a workbook, writing the check block, translating a
term sheet into a debt module. Domain 6 built the model and its six invariants; this topic asks only
what changes when the model is built with assistance, and the answer is that **the verification
burden moves and grows.**

The mechanism is worth stating precisely, because it is usually described as a productivity gain
and it is really a redistribution. A modeller who builds a schedule by hand acquires, as a
by-product, a mental model of how it works — where the circularity sits, which cell drives which,
what a wrong answer would look like. A reviewer of a machine-built schedule has none of that, so
review must reconstruct it, and reconstruction is slower than construction was for the person who
did it. The saving is real; it is just much smaller than the build-time comparison suggests.

**Worked example 16.2.3 — the real saving, the apparent saving, and the price of the difference.**

1. **Setup.** A sculpted debt module. **Hand-built:** 40 hours to build, 8 hours to review.
   **Machine-assisted:** 6 hours to produce, and — because the reviewer must reconstruct the
   construction logic — **26** hours to review properly. A senior modeller or reviewer costs
   **USD 150.00** an hour. The organisation's observed record is that an *unreviewed*
   machine-assisted module carries a material defect with probability **0.35**; Domain 6 (KA 6.4.1)
   priced one such defect — a one-cell tax error — at **USD 3,250,352** of lost debt capacity.
2. **Formula.** Cost = hours × rate. `EMV` = probability × consequence (the registered
   expected-monetary-value form, PML-AI KA 8.2).
3. **Substitution.** Hand `(40 + 8) × 150`; machine with full review `(6 + 26) × 150`; machine with
   the habitual 8-hour review `(6 + 8) × 150`. `EMV = 0.35 × 3,250,352`.
4. **Result.** Hand-built **USD 7,200**; machine-assisted with proper review **USD 4,800** — a
   **real saving of USD 2,400, or 33.3333 per cent**. Reviewed at the habitual 8 hours the cost is
   **USD 2,100**, an **apparent saving of 70.8333 per cent**. The omitted 18 hours of review are
   worth **USD 2,700**. The expected cost of the defect they would have caught is
   **USD 1,137,623.20** — **421.3419 times** the review.
5. **Interpretation.** The number to carry into a management conversation is **a third, not two
   thirds**. An organisation that books the 70.8333 per cent has not made a saving; it has
   converted USD 2,700 of certain cost into USD 1,137,623.20 of expected cost, and it has done so
   invisibly, because the omission leaves no artefact. Three consequences follow. **Fund the review
   explicitly, as a line, at the ratio the organisation has measured** — here roughly four hours of
   review per hour of assisted build, against one per five hand-built; the ratio is the governable
   quantity and it should be in the model-governance standard, not left to the reviewer's diary.
   **Require the assistant to produce the audit trail, not just the model**: a written statement of
   what each block does, the invariants it should satisfy, and the check block itself, which is
   work a machine does well and which cuts reconstruction time — this is the single most effective
   way to make the 26 hours smaller honestly. **And keep the accountability where Domain 6 put it**:
   the modeller who ships the module owns its defects regardless of who typed the formulae, and the
   organisation that treats "the tool built it" as mitigation has no model governance. The caution
   worth stating plainly: the 0.35 defect probability is this organisation's measured figure on its
   own work, not a general property of assisted modelling, and a firm that has not measured its own
   should assume a figure high enough to make the review unarguable and then measure.

### AI in this KA

**Where it earns its place.** All three topics *are* the application, so the useful question is
what machine assistance adds to their **governance**, and the answer is measurement and coverage.
Machines are good at computing what a scenario set covers, at diffing an extraction against a
previous version of the same agreement (which catches the superseded-draft failure directly), and
at generating the invariant tests a reviewer would otherwise write by hand. A second, genuinely
strong use: having one model **critique** another's output — an adversarial pass over an extraction
register or a generated scenario set, asked to find the internally inconsistent entries — because
inconsistency detection is cheap for a machine and does not require the machine to be right about
the substance.

**Where it must not go.** No machine may choose the load-bearing subset (16.2.2), decide `k`
(16.2.1) or sign a model off (16.2.3), because each of those is a scoping decision that determines
what will *not* be checked. Nor may a generated scenario minimum be presented as a bank case, or a
critique pass be treated as verification: an adversarial model that finds nothing has not
established that there is nothing, and treating a silent critique as assurance is the most seductive
failure in this knowledge area.

**Verification, concretely.** Recompute `aᵐ` for the actual subset size before relying on any
extraction summary — one line of arithmetic that reframes the whole procurement. Diff the
extraction against the executed document's defined-terms clause, not against a data room copy, and
record the version identifier. For scenario work, publish `k`, the share of scenarios breaching,
and the defined case separately. For assisted models, run Domain 6's six invariants plus the
effective-tax-rate check before any output is quoted, and record the review hours actually spent
against the standard's ratio — an unfunded review is the defect this knowledge area exists to
prevent. **AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 16.2

| Term | Meaning |
|---|---|
| **Scenario coverage** | `1 − (1 − p)ᵏ`: the chance `k` scenarios contain a mode of probability `p`. |
| **Required `k`** | `ln(α) ÷ ln(1 − p)`: scenarios needed to surface a mode at `1 − α` confidence. |
| **Generated minimum** | The worst of `k` draws; a percentile (≈ `1/(k+1)`), not a defined stress case. |
| **Load-bearing subset** | The extracted items that feed a model, covenant or reported ratio; the verification scope. |
| **Sweep probability** | `aᵐ`: the chance an `m`-item extraction is entirely correct at per-item accuracy `a`. |
| **Reconstruction cost** | The review time a machine-built artefact needs because no one holds its construction logic. |
| **Superseded-draft error** | Extraction from the wrong version; 100 per cent accurate against the wrong document. |

### Sample MCQs — KA 16.2

**MCQ 16.2-A `[16.2.2 · Application]`** A tool extracts defined terms at 92 % per-item accuracy.
Twenty-six of them are load-bearing. The probability that all 26 are correct is closest to:
- A. 92.00 %
- B. 11.44 % ✅
- C. 88.56 %
- D. 99.80 %

*Rationale:* `0.92²⁶ = 0.114415`. A mistakes per-item accuracy for deliverable accuracy — the
error the topic exists to correct. C is the complement, the probability that at least one is wrong.
D is the per-item accuracy that *would* be required for a 95 % clean sweep, `0.95^(1/26)`.

**MCQ 16.2-B `[16.2.2 · Analysis]`** Given that result, the defensible professional response is:
- A. reject machine extraction and read all 340 terms manually
- B. procure a tool with 99.8029 % per-item accuracy
- C. use the tool for all 340 terms, then verify the 26 load-bearing terms against the executed document by a named person on a recorded date — about USD 3,120 against a single known 600,000 exposure ✅
- D. accept the register and rely on the tool's confidence scores to flag which entries to check

*Rationale:* The remedy is a change of scope, not of tool: verification of the load-bearing subset
is 192.3077 times cheaper than one 600,000 definitional error. A discards the tool's genuine value
on the other 314 terms. B specifies an unachievable target. D lets the model choose its own
verification scope, and a model is least confident where it can detect its own error — not where it
cannot.

**MCQ 16.2-C `[16.2.1 · Analysis]`** An analyst reports "40 scenarios run, 3 breached, worst-case
`DSCR` 1.04". The soundest critique is:
- A. 40 is too few to demonstrate the absence of a low-probability mode — 230 are needed for 90 % confidence on a 1 % mode — and the worst of 40 draws is a percentile, not a defined stress case ✅
- B. 3 breaches out of 40 is a 7.5 % breach probability, which should be reported as the covenant risk
- C. the run is adequate; scenario counts above 30 are conventionally sufficient
- D. the analyst should have reported the mean `DSCR` across the 40 scenarios

*Rationale:* Coverage and labelling are the two defects: `ln(0.10) ÷ ln(0.99) = 230`, and a
generated minimum is roughly the `1/(k+1)` percentile of an unvalidated tail. B treats a sample
frequency from an unvalidated generator as a probability. C invents a convention. D reports the
statistic covenants never test (Domain 10: the minimum, not the average).

**MCQ 16.2-D `[16.2.3 · Application]`** A module takes 40 hours to build and 8 to review by hand,
or 6 hours assisted with 26 hours of review. At USD 150 an hour, the saving from assistance is:
- A. 70.8333 %
- B. 33.3333 % ✅
- C. 85.0000 %
- D. nil, since total hours are similar

*Rationale:* `(48 − 32) × 150 = 2,400` on 7,200, or 33.3333 %. A is the apparent saving when the
review is left at the hand-built 8 hours — the omission worth 2,700 of review against 1,137,623.20
of expected defect cost. C compares build hours only (6 against 40). D ignores the 16-hour
difference.

### Self-check — KA 16.2

1. *Why is per-item extraction accuracy the wrong statistic?* — Because the deliverable needs every
   load-bearing item right: 92 per cent per item gives 0.114415 for 26 items, and a 95 per cent
   sweep would need 99.8029 per cent per item.
2. *How is `k` chosen for a scenario run?* — From the smallest probability the run must be able to
   surface: `k ≥ ln(α) ÷ ln(1 − p)` — 59 for a 5 per cent mode at 95 per cent confidence, 230 for a
   1 per cent mode at 90 per cent — and the result is a floor, because correlated generators
   explore less than their count implies.
3. *What does machine assistance do to a model's total cost?* — It moves cost from build to review
   and reduces the total by about a third, not two thirds; booking the larger figure converts
   USD 2,700 of review into USD 1,137,623.20 of expected defect cost.

---

## Knowledge Area 16.3 — Explainability, validation, bias and model risk

*Topics: 16.3.1 explainability as an accounting identity · 16.3.2 validation — how many test cases,
honestly derived · 16.3.3 bias in a finance model · 16.3.4 model-risk governance: inventory,
tiering, revalidation.*

### 16.3.1 Explainability as an accounting identity

**Definition.** **Explainability** in a finance context is not a philosophical property of a model;
it is the ability to state, for a specific output, **how much of it came from what** — in the units
of the output, adding up. A global statement about which inputs matter in general is
*interpretability* and is useful for design. What a decision, an audit or a lender's question
requires is a **local, additive attribution** of this period's number.

The test is therefore one the book already owns. PML-AI's **hundred-per-cent rule** — Σ children −
parent = 0 at every level — applies unchanged: an explanation whose components do not reconcile to
the output is not an explanation, it is a commentary. Suppose an automated forecast publishes
`CFADS` of **6,384,000** and attributes it across drivers — volume, tariff, escalation, cash
operating cost, cash tax, working-capital movement — summing to **6,190,000**. The residual is
**USD 194,000**: **3.0388 per cent** of the forecast, and **52.0892 per cent** of Kestrel's annual
covenant headroom of 372,438. The professional reading is exact: **the model has an unexplained
component larger than half the project's covenant headroom**, so any conclusion drawn from the
attribution about which driver to manage is unsafe, and the correct response is to find the
residual, not to describe it as model complexity. Three further disciplines belong here.
**Attribution must be in currency, not in importance scores** — a ranking cannot be reconciled and
therefore cannot be audited. **The counterfactual must be stated**: "volume contributed 1.2m"
means nothing without "relative to what", and the baseline is a choice with an owner. And
**explanation is not justification**: a faithful account of how a model reached a number says
nothing about whether the number is right, which is 16.3.2's job. Where a model cannot be given an
additive local attribution at all, the honest options are to use it only for triage and never for
a reported figure, or to replace it with one that can — and for a covenant-relevant number the
second is usually the right answer.

### 16.3.2 Validation — how many test cases, honestly derived

**Definition.** **Validation** is the evidence that a model performs as claimed on cases it has not
seen. The question a governance committee always asks and rarely gets answered is how many test
cases are enough, and it has a derivable answer under stated assumptions.

**Worked example 16.3.2 — the sample size, and the assurance it does not give.**

1. **Setup.** A covenant-certificate assembler must be validated before deployment. The committee
   wants to be **95 per cent confident** that its defect rate is **below 1 per cent**. Test cases
   are independent and representative of production; all of them pass. Then: what changes if one
   case fails; and what does the validated bound imply at a production volume of **56,400** uses a
   year?
2. **Formula.** If the true defect rate were `p`, the probability that `n` independent tests all
   pass is `(1 − p)ⁿ`. Requiring that probability to be at most `α` gives
   `n ≥ ln(α) ÷ ln(1 − p)`. With one failure permitted, the condition becomes
   `(1 − p)ⁿ + n·p·(1 − p)ⁿ⁻¹ ≤ α`, solved numerically. Expected annual production errors =
   `p × uses`.
3. **Substitution.** `ln(0.05) ÷ ln(0.99)`; then the two-term condition at `p = 0.01`; then
   `0.01 × 56,400`; then `ln(0.05) ÷ ln(1 − 10/56,400)`.
4. **Result.** **`n ≥ 298.0729`, so 299 test cases**, all passing. The sample-size ladder, at
   95 per cent confidence: **59** cases bound the rate below 5 per cent, **149** below 2 per cent,
   **299** below 1 per cent, **598** below 0.5 per cent, **2,995** below 0.1 per cent. Raising
   confidence to 99 per cent at the 1 per cent bound requires **459**. Permitting **one** failure
   at the 1 per cent bound and 95 per cent confidence requires **473**. And a model validated to
   "below 1 per cent" and used 56,400 times a year is consistent with **564 errors a year**; to
   bound expected production errors at **10** a year the defect rate must be below
   **0.017730 per cent**, which requires **16,895** passing test cases.
5. **Interpretation.** The ladder is the useful artefact — it converts an unbounded argument into a
   choice — but the honest content of this example is the last line, and it is uncomfortable. **The
   test suite an organisation can afford proves far less than the assurance it wants.** Nobody is
   going to construct 16,895 representative test cases for a covenant-certificate assembler; 299 is
   already a serious programme. So pre-deployment testing cannot be the control that makes a
   high-volume model safe, and an organisation that behaves as though it were has mis-sited its
   assurance. The load must be carried by three things instead: **continuous monitoring** of the
   production error rate, which accumulates evidence at production volume rather than at test
   volume and reaches 16,895 observations in **about four months** (16,895 ÷ 56,400 of a year,
   109 days), against the years a test programme would need; **human approval** at the points where
   consequence is concentrated (KA 16.4.3, and the tiering of 16.3.4, which is how one decides
   where); and **rollback**, because the realistic response to a discovered defect is to stop, not
   to have prevented it. Four cautions on the arithmetic itself, all of which a reviewer should
   press. The formula assumes **independence** — 299 near-duplicate cases are not 299 tests, and
   the commonest inflation of a validation claim is a suite generated from one template. It assumes
   **representativeness** — a suite drawn from historical cases cannot bound the error rate on the
   cases the model will actually meet, which is 16.3.3's problem. It gives a **one-sided bound on a
   binary outcome**, so it says nothing about the *size* of a defect: 299 passes are consistent
   with a rare but catastrophic failure, which is why tiering is by consequence and not by rate.
   And a single failure moves the requirement from 299 to **473** — a **58.19 per cent** increase —
   which is the arithmetic reason "we fixed it and re-ran the failing case" is not a validation:
   the suite must grow, not be repaired.

### 16.3.3 Bias in a finance model

**Definition.** **Bias**, in this domain, is a systematic difference between what a model was
trained or calibrated on and what it is used on. It is a measurable property, not a moral one, and
in project finance it takes four concrete forms worth naming. **Training-period bias** — a demand
model calibrated on a period whose conditions no longer hold, which Domain 7's demand work must
guard against. **Survivorship bias** — a comparables set of completed, financed, surviving projects,
which systematically overstates achievable performance because the failures are absent.
**Label bias** — a detector trained on the errors a previous process happened to find, so it
inherits that process's blind spots. And **attribute bias with legal exposure** — a
counterparty- or location-derived score that operates as a proxy for a protected characteristic;
what is lawful here differs materially between jurisdictions and this book does not state any
jurisdiction's position, but the professional obligation is constant: know which attributes the
model uses, know what they proxy for, and take legal advice before a score of this kind affects
access to credit or employment.

Label bias is the one that bites hardest in an automated finance function, because it is invisible
in every metric the model reports.

**Worked example 16.3.3 — the recall that was never 80 per cent.**

1. **Setup.** The detector of 16.1.3, operating at T4: **48,000** items, **1,200** assumed
   anomalies, **960** caught, **950** false positives, reported recall **80 per cent**, reported
   total cost **USD 114,800**. The labels it was trained on came from the previous manual process.
   A **blind audit** is run over the **46,090** items the detector did *not* flag: **1,000**
   sampled, **17** genuine errors found.
2. **Formula.** Extrapolated missed errors = unflagged population × sample error rate. True
   population = caught + extrapolated. True recall = caught ÷ true population. Restated cost =
   `FP × 40 + true FN × 320`. Audit sample size for a `±e` bound at `1 − α` confidence on a
   proportion `p`: `n ≥ z²·p·(1 − p) ÷ e²`.
3. **Substitution.** `17 ÷ 1,000 = 1.70 per cent`; `46,090 × 0.0170`; then `960 ÷ 1,744`; then
   `950 × 40 + 784 × 320`; and for the sample size `1.96² × 0.025 × 0.975 ÷ 0.01²`.
4. **Result.** Extrapolated missed errors **784**, against the 240 the model's own arithmetic
   assumed. True anomaly population **1,744**, **45.3333 per cent** above the assumed 1,200. True
   recall **55.0459 per cent**, not 80. Restated total cost **USD 288,880** against the reported
   114,800 — a factor of **2.5164** — with the false-negative cost understated by **USD 174,080**.
   The audit that revealed this cost **USD 13,600** — 1,000 items re-reviewed at 8.5 minutes each,
   **USD 13.60** at the loaded analyst rate, a desk re-review rather than the 25-minute
   investigation an alert requires — and identified missed errors worth **USD 250,880** a year, a
   return of **18.4471 times**. A sample of **937** would
   have sufficed for a ±1 percentage-point bound at 95 per cent confidence on a 2.5 per cent rate.
5. **Interpretation.** The mechanism deserves stating slowly, because it is the most
   under-appreciated failure in automated controls. **A detector trained on a prior process's
   findings learns to reproduce that process, including what it could not see.** Its measured
   recall is recall *against the labels*, and if the labels are 60 per cent complete then a bounding
   estimate of true recall is `0.80 × 0.60 = 48 per cent` — the audit's 55.0459 per cent sits
   between that bound and the claim, which is what one should expect. Every metric in 16.1.3's table
   was computed correctly and every one was measuring the wrong population. Three consequences.
   **No threshold change fixes this.** If the 784 missed errors are precisely the kinds the old
   process could not detect, loosening the threshold does not reach them: scaling the T4-to-T5
   marginal benefit by the 1.4533 population factor gives **USD 55,808** against an unchanged
   USD 78,000 of investigation cost, so T4 remains optimal and the answer is a **model change** —
   new features, and the audit's 17 confirmed errors as a new label source — not a tuning change.
   **The blind audit is the only instrument that measures this, and it must be blind**: a sample
   drawn from items the model scored highly, or reviewed by the people who set the labels,
   reproduces the bias. Budget it as a permanent line, sized by `z²p(1 − p)/e²` for the precision
   the decision needs, and re-run it annually. **And restate the business case afterwards.** The
   automation's claimed saving in 16.1.2 was computed on assumed error rates; the audit is how those
   become measured, and a case that is never restated is a case that was never governed. The
   standing caution: extrapolating 17 findings to 784 carries real sampling error — the 95 per cent
   interval around a 1.70 per cent rate on 1,000 draws is wide enough that the point estimate should
   be reported with its bound, and the decision should be robust to the low end of it.

### 16.3.4 Model-risk governance: inventory, tiering, revalidation

**Definitions.** **Model risk** is, on the shared registry's definition, the risk of loss from
decisions based on flawed, misused or misunderstood models — financial or AI. Domain 11 (KA 11.4.3)
established it as a priced line in the operating risk register and made the point that its
probability is a function of governance rather than of nature; that analysis is not repeated. What
this topic adds is the **governance instrument**: a **model inventory** (every model in use, with
its purpose, owner, inputs, outputs, dependencies and the decisions it touches), a **tier** for
each, and a **revalidation interval** derived from the tier rather than set by convention.

Tiering by "materiality" is the usual practice and it is unfalsifiable. Tiering by **expected
annual loss** is derivable: `EMV` per year = uses a year × defect probability × consequence per
defect, on the registered expected-monetary-value form. Set a tolerance for the loss an
organisation is willing to accumulate between validations, and the interval follows:
`revalidation interval = tolerance ÷ annual EMV`.

**Worked example 16.3.4 — the inventory that sets its own calendar.**

1. **Setup.** Five models in the estate's finance function, with the uses, defect probabilities and
   consequences measured or estimated as shown. The tolerance for accumulated expected loss between
   validations is **USD 50,000** per model. Consequences are drawn from figures already established:
   Kestrel's annual covenant headroom **372,438** (a covenant-forecast error consumes it), the
   forecast band **306,432** at the manual error level (16.1.2), the **600,000** definitional
   exposure (Domain 2), a **325,000** waiver fee (Case study B) and **320** per undetected payment
   error (16.1.3).
2. **Formula.** `annual EMV = uses × p × consequence`; `interval = 50,000 ÷ annual EMV`.
3. **Result.**

   | Model | Uses a year | `p` | Consequence (USD) | **Annual `EMV`** | Interval | Policy outcome |
   |---|---|---|---|---|---|---|
   | M1 Covenant-forecast model | 12 | 0.030 | 372,438 | 134,077.68 | 0.372918 yr = **136.12 d** | Quarterly |
   | M2 Payment anomaly detector | 48,000 | 0.001 | 320 | 15,360.00 | 3.255208 yr = 1,188.15 d | Annual (policy floor) |
   | M3 Consumption forecaster | 12 | 0.050 | 306,432 | 183,859.20 | 0.271947 yr = **99.26 d** | Quarterly |
   | M4 Defined-term extractor | 4 | 0.885585 | 600,000 | 2,125,404.00 | 0.023525 yr = **8.59 d** | **Verify every use** |
   | M5 Covenant-certificate assembler | 4 | 0.020 | 325,000 | 26,000.00 | 1.923077 yr = 701.92 d | Annual (policy floor) |

4. **Interpretation.** The instrument's value is that it produces a **calendar an auditor can
   challenge on its inputs** rather than a tier label nobody can argue with. Read three results.
   **M4's interval of 8.59 days is shorter than the interval between its uses** — four facilities a
   year, roughly 91 days apart — which means the rule's own arithmetic says the model must be
   verified at **every** use. That is exactly the conclusion 16.2.2 reached from the
   combinatorics of a 26-item sweep, arrived at independently from a governance rule, and the
   agreement of two derivations is the strongest evidence in this domain that the conclusion is
   right. **The high-volume model is the low-tier one.** M2 touches 48,000 items but each defect
   costs 320, so its expected annual loss is the smallest in the inventory and annual revalidation
   is generous — while M1 and M3, used twelve times a year each, need quarterly attention because
   one bad output consumes a covenant headroom. The instinct to govern by volume is precisely
   inverted. **And the policy floor matters as much as the formula.** M2 and M5 compute to intervals
   above a year, and the standard should nonetheless cap the interval at twelve months, because the
   formula prices *known* failure modes and a year of undetected drift is the unpriced one. Two
   cautions. The tolerance of 50,000 is a policy parameter with an owner, and halving it halves
   every interval, so it belongs in the standard and not in a spreadsheet. And each `p` is an
   estimate that the monitoring of 16.3.2 should be replacing with a measurement — the inventory is
   a living instrument, and a model whose `p` has never been measured should carry the pessimistic
   figure until it has.

### AI in this KA

**Where it earns its place.** Assurance work has its own automatable core, and it is worth taking:
generating a validation suite's boundary and adversarial cases from a specification; monitoring
production error rates and raising a drift alert; computing the inventory's `EMV` and interval
columns from the monitoring feed so the calendar updates itself; producing the additive attribution
of 16.3.1 for every published figure; and diffing a model's behaviour between versions so a change
that alters an output nobody expected to change is surfaced before release rather than after.

**Where it must not go.** A model may not validate itself, in any of the several forms this takes:
generating its own test cases from its own training distribution, scoring its own explanations for
plausibility, or setting the `p` in its own tier. It may not decide the tolerance, the tier or the
interval, which are risk-appetite decisions. And an explanation must never be **generated as
narrative** — a fluent paragraph describing why a model produced a figure, unconstrained by the
additive reconciliation of 16.3.1, is the most dangerous artefact in this knowledge area, because
it is persuasive precisely where it is unverifiable.

**Verification, concretely.** Reconcile every attribution to its output and require the residual to
be nil or explained, in currency (16.3.1). Recompute the validation sample size from the stated
bound and confidence, and check the suite for independence by asking how many distinct templates
generated it. Confirm the audit sample of 16.3.3 was drawn from unflagged items and reviewed by
people outside the labelling process. Recompute one row of the inventory's `EMV` and interval by
hand. And require that every `p` in the inventory names its source — measured, estimated, or
inherited — because an inventory of estimates presented as measurements is worse than no inventory.
**AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 16.3

| Term | Meaning |
|---|---|
| **Local additive attribution** | An explanation of one output in the output's own units, reconciling to it. |
| **Attribution residual** | Output − Σ attributions; must be nil or explained (the hundred-per-cent rule). |
| **Zero-failure sample size** | `n ≥ ln(α) ÷ ln(1 − p)`: tests needed to bound a defect rate at `1 − α` confidence. |
| **Label bias** | A detector trained on a prior process's findings inherits its blind spots. |
| **Blind audit** | A sample of *unflagged* items reviewed independently, to measure what the model cannot see. |
| **Model inventory** | Every model in use, with purpose, owner, dependencies, decisions touched and tier. |
| **Revalidation interval** | Tolerance for accumulated expected loss ÷ annual `EMV`, capped by policy. |

### Sample MCQs — KA 16.3

**MCQ 16.3-A `[16.3.2 · Application]`** How many independent, representative test cases, all
passing, are needed to be 95 % confident that a model's defect rate is below 1 %?
- A. 100
- B. 299 ✅
- C. 95
- D. 459

*Rationale:* `n ≥ ln(0.05) ÷ ln(0.99) = 298.0729`, so 299 — the "rule of three" at `3/p`. A is the
reciprocal of the bound, which gives only a 63 % confidence. C confuses the confidence level with a
count. D is the requirement at 99 % confidence, not 95 %.

**MCQ 16.3-B `[16.3.2 · Analysis]`** That model passes its 299 cases and is then used 56,400 times
a year. The correct statement is:
- A. the validation shows the model will produce fewer than 10 errors a year
- B. the validation is consistent with up to about 564 errors a year, so monitoring, human approval at concentrated-consequence points and rollback must carry the assurance ✅
- C. the validation is invalid because the test volume is smaller than the production volume
- D. the validation guarantees a 99 % success rate in production

*Rationale:* A 1 % bound at 56,400 uses admits `0.01 × 56,400 = 564` errors; bounding expected
errors at 10 would need 16,895 passing cases. A inverts the bound. C misstates the requirement —
test volume need not match production volume, it must be independent and representative. D reads a
one-sided confidence bound as a point guarantee.

**MCQ 16.3-C `[16.3.3 · Analysis]`** A detector reports 80 % recall at a total cost of 114,800. A
blind audit of 1,000 of the 46,090 unflagged items finds 17 genuine errors. The most important
consequence is:
- A. the threshold should be loosened until recall reaches 90 %
- B. the true anomaly population is about 1,744 rather than 1,200, true recall is 55.0459 %, and the restated cost is 288,880 — the missed errors are the kinds the labels never contained, so the answer is a model change, not a threshold change ✅
- C. the audit sample is too small to act on
- D. the detector should be withdrawn

*Rationale:* Extrapolating 1.70 % over 46,090 gives 784 missed errors; recall and cost restate to
55.0459 % and 288,880. Loosening (A) does not reach errors the model cannot see — scaling the
T4→T5 benefit by 1.4533 gives 55,808 against 78,000 of cost, so T4 remains optimal. C is wrong:
937 would suffice for a ±1-point bound at 95 % confidence. D discards a control that still avoids
substantial loss.

**MCQ 16.3-D `[16.3.4 · Application]`** A model is used 4 times a year, fails with probability
0.885585 per use, and a failure costs 600,000. With a tolerance of 50,000 of accumulated expected
loss between validations, the revalidation interval is:
- A. annual, since four uses a year is low volume
- B. about 8.59 days — shorter than the interval between uses, so the model must be verified at every use ✅
- C. quarterly, matching the use frequency
- D. about 3.26 years, since the annual `EMV` is small

*Rationale:* `EMV = 4 × 0.885585 × 600,000 = 2,125,404`; `50,000 ÷ 2,125,404 = 0.023525`
years. A and C tier by volume or convenience rather than by expected loss — the inversion the topic
exists to correct. D is the interval for the payment detector (M2), whose `EMV` is 15,360.

### Self-check — KA 16.3

1. *What makes an explanation auditable?* — Local additive attribution in the output's own units,
   reconciling to the output; a 194,000 residual on a 6,384,000 forecast is 52.0892 per cent of
   covenant headroom and disqualifies any conclusion drawn from the attribution.
2. *Why can pre-deployment testing not make a high-volume model safe?* — 299 passing cases bound
   the defect rate below 1 per cent, which at 56,400 uses still admits 564 errors a year; bounding
   at 10 would need 16,895 cases, so monitoring, human approval and rollback carry the load.
3. *What does a blind audit measure that no model metric can?* — The errors absent from the labels:
   1,000 unflagged items yielding 17 findings restated the population from 1,200 to 1,744, recall
   from 80 to 55.0459 per cent, and cost from 114,800 to 288,880.

---

## Knowledge Area 16.4 — Privacy, cybersecurity, human approval and AI governance

*Topics: 16.4.1 confidentiality, privacy and the deployment choice · 16.4.2 cybersecurity of an
automated finance function · 16.4.3 human approval — what it is, and what it costs · 16.4.4 the AI
governance frame.*

### 16.4.1 Confidentiality, privacy and the deployment choice

**Definitions.** A project financing generates three distinguishable classes of sensitive data, and
conflating them produces bad controls. **Commercially confidential information** — the financial
model, the price deck, the bank case, draft documents, the offtaker's consumption profile — is
protected by contract, and its disclosure damages a live negotiation immediately and irreversibly.
**Personal data** — employee, contractor and sometimes customer information — is protected by law
that differs materially between jurisdictions; this book does not state any jurisdiction's
requirements, and the professional obligation is to establish which regimes apply to each data set
before it is processed, and to take advice. **Statutorily or contractually restricted
information** — price-sensitive information about a listed sponsor, or data whose location is
restricted by a licence or a concession — carries its own consequences independent of the other
two.

Two practical rules cut across all three. **Minimisation beats protection**: the surest control on
a diligence data set is that the personal data was never loaded, and redaction at ingestion is
cheaper than every control downstream. And **retention by a processor is the risk that gets
missed** — whether a service retains inputs, for how long, whether they train on them, and in which
jurisdiction they rest are contractual questions to settle before use, not afterwards.

**Worked example 16.4.1 — the deployment choice, and the number at which it flips.**

1. **Setup.** The automation platform of 16.1.2 can run as a **managed service** at
   **USD 148,000** a year or as a **private deployment** inside the sponsor's own environment at
   **USD 410,000** a year. The security function assesses the annual probability of a
   confidentiality breach involving model or deal data at **0.80 per cent** on the managed service
   and **0.15 per cent** privately. The consequence of such a breach during a live financing —
   re-pricing, remedial legal and communications cost, and the relationship damage Domain 10
   (KA 10.4.4) priced qualitatively — is assessed at **USD 6,500,000**.
2. **Formula.** `EMV` avoided = probability differential × consequence. Compare with the cost
   differential. Breakeven consequence = cost differential ÷ probability differential.
3. **Substitution.** `(0.008 − 0.0015) × 6,500,000`; `410,000 − 148,000`;
   `262,000 ÷ 0.0065`.
4. **Result.** `EMV` avoided **USD 42,250** a year against a cost differential of
   **USD 262,000** — so on these figures the private deployment is **not** justified, by
   USD 219,750 a year. The breakeven consequence is **USD 40,307,692**.
5. **Interpretation.** The value of this calculation is not its answer but the sentence it forces:
   **state the consequence at which your choice flips, then argue about whether the consequence is
   really that large.** USD 40.3 million is a large number for a routine operating pipeline, which
   is why the managed service is defensible for one — and it is *not* an implausible number for the
   information set of a live acquisition, a refinancing at scale or a competitive bid, where the
   loss is the transaction rather than the data. So the professionally correct architecture is
   usually neither of the two options as posed: **segment the data**, run the high-volume operating
   pipeline on the managed service, and keep the live-transaction information set — model, price
   deck, bank case, draft documents — inside the private environment, where the volume is low and
   the 262,000 differential applies to a fraction of the work. Three cautions. The probabilities
   are **assessments, not measurements**, and no organisation has enough breach history to measure
   them, so the arithmetic's honest role is to structure the judgment rather than to settle it —
   present it as a breakeven, never as an `NPV`. The consequence is **not fully monetisable**: a
   breach that costs a licence, or a regulatory consequence in one jurisdiction, is not on this
   scale at all, and where that is possible the calculation should not be run — the answer is the
   private deployment regardless of arithmetic. And a private deployment is **not automatically
   safer**: it moves the risk from a specialist provider's controls to the organisation's own, and
   an under-resourced private environment can carry the higher probability, which is a real
   inversion and should be tested rather than assumed.

### 16.4.2 Cybersecurity of an automated finance function

**Definition.** Domain 11 (KA 11.4.1) established that cyber risk on an industrial project is
primarily an **availability** risk on the operational-technology network and therefore a `CFADS`
risk with the same shape as any outage. This topic addresses the different exposure created by
automating the **finance** function, which is not availability but **integrity and authority**:
the risk that money moves, or a figure is reported, on an instruction nobody competent gave.

Three attack surfaces are specific to an automated finance function and none of them existed in a
manual one. **Instruction injection through ingested content**: an assistant that reads supplier
invoices, emails or documents will read whatever is in them, including text crafted to look like
instructions, so any pipeline that both reads external content and can act must treat ingested text
as data and never as direction. **Label and data poisoning**: an adversary who can influence what a
detector learns — by shaping which items get labelled, or by operating just below a known
threshold — degrades the control quietly, and 16.3.3's blind audit is the only routine instrument
that would notice. **Authority accumulation**: automation tends to concentrate permissions, because
it is easier to give a pipeline broad access than to scope it, and the result is a single identity
that can read the ledger, initiate a payment and update the record of what it did.

The control that answers the third is separation of duties, and it should be sized rather than
declared.

**Worked example 16.4.2 — the dual-approval threshold, derived.**

1. **Setup.** The estate makes **4,800** payments a year totalling **USD 68,000,000**. A second
   approver costs **15 minutes** at **USD 1.60** a minute — **USD 24.00** a payment. **0.20 per
   cent** of payments are erroneous or fraudulent, and a second approval catches **85 per cent** of
   those, so **0.17 per cent** of payment value is protected per payment reviewed. **1,150**
   payments, worth **USD 63,400,000**, exceed the threshold derived below; the remaining **3,650**,
   worth **USD 4,600,000**, fall under it.
2. **Formula.** Expected loss avoided on one payment = caught rate × payment value. The control
   pays where that exceeds the approver's cost, so the threshold is
   `V* = approver cost ÷ caught rate`. Net value of a policy = `caught rate × value reviewed −
   approver cost × payments reviewed`.
3. **Substitution.** `24.00 ÷ 0.0017`; then the two policies on the value and count above and
   below.
4. **Result.** **Threshold `V*` = USD 14,117.65**, so dual approval pays on any payment above about
   **USD 14,118**. **Blanket dual approval on all 4,800 payments** costs **USD 115,200** and avoids
   **USD 115,600** — a net value of **USD 400 a year**. **Thresholded dual approval on the 1,150
   payments above 14,118** costs **USD 27,600** and avoids **USD 107,780** — a net value of
   **USD 80,180**. The 3,650 small payments cost **USD 87,600** to approve and avoid **USD 7,820**,
   destroying **USD 79,780**, which reconciles exactly to the difference between the two policies.
5. **Interpretation.** The blanket rule — the rule almost every finance function actually has — is
   **value-neutral to three significant figures**, and it achieves that by combining a strongly
   positive control on large payments with a strongly negative one on small ones. That is the
   general lesson of this domain restated in a third setting: **the value of a control, like the
   value of a looser alert threshold and the value of a verification step, is a marginal quantity,
   and averaging destroys the information.** Four practical consequences. **Derive the threshold and
   put it in the policy**, with its two inputs visible — the approver's cost and the caught rate —
   because both are arguable and neither is usually written down. **Spend the released capacity on
   the exposure that has no control**: 87,600 a year of approver time bought 7,820 of protection,
   and the same money spent on the blind audit of 16.3.3 bought a 18.4471-times return. **Do not
   read this as an argument against controls on small payments** — a *sampling* control on the
   sub-threshold population costs a fraction of blanket approval and preserves deterrence, which the
   arithmetic above does not model and which matters, because a published threshold is an
   instruction to an adversary to stay below it. That last point is the professional caution, and it
   is why the threshold should be reviewed against the observed distribution of attempts, not set
   once. **And the caught rate is the input to defend**: at 85 per cent the control is strong; a
   second approver who signs without looking has a caught rate near nil, at which point the whole
   115,200 is waste and the arithmetic says so.

### 16.4.3 Human approval — what it is, and what it costs

**Definition.** **Human approval** is a named person taking responsibility for an output before it
has an effect. It has three levels that must not be confused. **Review** — a person examines the
output and can stop it. **Approval** — a person is accountable for it having proceeded.
**Certification** — a person makes a representation to a third party, with the consequences that
attach; Domain 14's treatment of draw-request certifications stands, and nothing in an automated
pipeline may generate a representation.

Approval architecture is usually discussed as though it were free, and it is not. PML-AI's
registered governance-latency formula gives the price directly: the expected wait for a committee
decision is `E[wait] = M/2 + L`, for a meeting interval `M` and a paper lead time `L`.

**Worked example 16.4.3 — what an approval gate costs while it waits.**

1. **Setup.** A change to the automation pipeline of 16.1.2 would deliver the **USD 416,000** a
   year of saving established there. Two approval designs: a **quarterly AI governance committee**
   (`M` = 60 days of effective interval between usable decision slots, `L` = 10 days of paper lead
   time) and a **weekly delegated panel** with a defined mandate (`M` = 7 days,
   `L` = 2 days).
2. **Formula.** `E[wait] = M/2 + L`. Value forgone = `E[wait] × annual benefit ÷ 365`.
3. **Substitution.** `60/2 + 10 = 40` days; `7/2 + 2 = 5.5` days; `416,000 ÷ 365 = 1,139.7260` a
   day.
4. **Result.** The committee design forgoes **USD 45,589.04** of value per change; the delegated
   panel forgoes **USD 6,268.49**. The difference — **USD 39,320.55 per change** — is the price of
   the governance design, paid whether or not the committee changes the decision.
5. **Interpretation.** The point is emphatically **not** that approval should be removed. It is
   that **approval must be tiered by consequence, because the tier is where the cost sits**, and an
   organisation that routes every model change to one quarterly committee is paying roughly 39,000
   a change for assurance it does not need on most of them while giving inadequate attention to the
   few that matter. The design this domain endorses has three tiers derived from the instruments
   already built. Changes to a model in the **top tier** of 16.3.4's inventory — M1 and M3, where
   one bad output consumes a covenant headroom — go to the committee, and the 45,589 is well spent
   because the exposure is 372,438 a year of headroom. Changes to **threshold parameters** go to a
   delegated panel with a written mandate, because 16.1.3 showed the decision is a cost-ratio
   judgment that a small standing group can take competently and repeatedly. Changes that alter
   **what is reported to a lender** go nowhere near either: they are an information-covenant matter
   (Domain 10, KA 10.4.1) and require the finance director's own decision. Two cautions worth
   holding. **Latency is not the only cost of a gate** — PML-AI's gate-net-value form prices the
   defect the gate catches against the delay it imposes, and a gate whose catch rate is nil is pure
   cost — so measure what the committee actually changes. And **the delegated mandate must be
   written**, naming which parameters may move, within what bounds, and what triggers escalation,
   because an undocumented delegation is not a tier, it is an absence of control that will be
   described as one after an incident.

### 16.4.4 The AI governance frame

**Definition.** **AI governance** in a finance function is the set of arrangements that makes the
use of models decidable, reversible and attributable. Stripped of framework language, it answers
three questions for every model: **what is it for**, **who verifies its output before it has an
effect**, and **who is accountable for the decision it informs**. A programme that cannot answer
those three for every entry in its inventory does not have governance, whatever documentation it
holds.

Six components carry the weight, and each has been derived rather than asserted in this domain. The
**inventory and tiering** of 16.3.4, with intervals that follow from expected loss and a policy
floor. The **validation and monitoring** regime of 16.3.2, with monitoring sized to carry the
assurance that testing cannot. The **human-approval architecture** of 16.4.3, tiered so that its
latency cost is spent where consequence is. The **audit trail**: for every published figure, the
model version, the input data version, the attribution (16.3.1), and the named verifier with a
date — which is what makes an output defensible a year later, when the person has moved on.
**Incident response and rollback**, including the number nobody computes: 16.A.2 prices a quarter's
suspension of the estate's pipeline at **USD 141,000**, and a rollback plan without that figure has
not been tested. And **disclosure**, which is the component most often forgotten in a financing:
where a model materially affects a reported figure or a covenant certificate, whether and how that
is disclosed to lenders is an information-covenant question, and a sponsor who is asked about it for
the first time during a waiver negotiation has lost the initiative Domain 10 (KA 10.4.4) identified
as the most valuable asset in a covenant relationship.

**On external frameworks, accurately.** Several published frameworks are useful reference points
and are named here in the book's own words, with no text reproduced. **ISO/IEC 42001** specifies
requirements for an artificial-intelligence management system, in the same management-system idiom
as ISO/IEC 27001 for information security, and is therefore the natural home for the inventory,
tiering and approval arrangements above. **ISO/IEC 23894** offers guidance on managing risk in
artificial-intelligence systems. The **NIST AI Risk Management Framework**, published by the United
States National Institute of Standards and Technology, is a voluntary, function-based framework
widely used as a structuring device. The **European Union's Artificial Intelligence Act** is a
risk-tiered regulatory regime whose obligations depend on how a system is classified and on the
role a party plays in relation to it, applying in phases; whether and how it reaches a particular
project company is a legal question that depends on facts and jurisdiction, and it is not answered
in this book. In addition, banking supervisors in several jurisdictions publish model-risk
management expectations for regulated lenders. A project company is not usually within their scope
— but **its lenders are**, and the expectation therefore propagates to the borrower through
diligence questionnaires and information covenants, which is why a sponsor with a defensible model
inventory has a commercial advantage and not merely a tidy file. None of these frameworks is
endorsed by the Institute, none is reproduced, and none substitutes for advice on the law
applicable to a specific project.

### AI in this KA

**Where it earns its place.** Governance work has real automatable substance: maintaining the
inventory from the deployment pipeline so that no model is in production without an entry;
monitoring for models in use that are *not* in the inventory, which is the commonest governance
failure and is detectable from access logs; assembling the audit trail automatically at publication
time, because an audit trail assembled later is an assertion; tracking approvals against mandates
and flagging the ones that exceeded them; and drafting the disclosure language a lender pack needs,
for a human to settle.

**Where it must not go.** No model may approve, certify or disclose. No model may set its own tier,
tolerance or mandate. And no model may write the governance framework it will be governed by — a
recursion that sounds absurd and is nonetheless the most likely way an organisation ends up with a
policy that reads well, cites the right frameworks, and imposes no constraint anyone has costed.
The test to apply to any such document is the one this domain has applied throughout: **which
number in it would change a decision?** A governance document with no numbers governs nothing.

**Verification, concretely.** Reconcile the inventory against production access logs and require
the difference to be nil. Sample five published figures and attempt to reproduce each from the
recorded model version, data version and attribution alone; a figure that cannot be reproduced from
its own trail is not auditable. Test the rollback by running it, and record the cost. Confirm that
every approval in the last quarter was within a written mandate, and that the exceptions were
escalated. And confirm that the disclosure position on model-derived figures has been stated to
lenders in a document, not in a conversation. **AI proposes; the professional verifies, decides and
remains accountable.**

### Key terms — KA 16.4

| Term | Meaning |
|---|---|
| **Minimisation** | Not loading the sensitive data; the cheapest and strongest confidentiality control. |
| **Breakeven consequence** | Cost differential ÷ probability differential; the loss at which a deployment choice flips. |
| **Instruction injection** | Ingested content crafted to be read as direction; treat all ingested text as data. |
| **Label poisoning** | Influencing what a detector learns; detectable only by blind audit. |
| **Authority accumulation** | One automated identity able to read, act and record; the separation-of-duties failure. |
| **Dual-approval threshold** | Approver cost ÷ caught rate; the value above which a second approval pays. |
| **Review / approval / certification** | Can stop it · accountable for it · represents it to a third party. |
| **Governance latency** | `E[wait] = M/2 + L`; the value forgone while a gate waits. |

### Sample MCQs — KA 16.4

**MCQ 16.4-A `[16.4.2 · Application]`** A second approver costs USD 24 a payment; 0.20 % of
payments are bad and a second approval catches 85 % of those. The payment value above which dual
approval pays is closest to:
- A. USD 12,000
- B. USD 14,118 ✅
- C. USD 120,000
- D. USD 80,000

*Rationale:* Caught rate `0.0020 × 0.85 = 0.0017`; `24.00 ÷ 0.0017 = 14,117.65`. A omits the catch
rate and divides by 0.0020. C misplaces a decimal in the bad-payment rate, using 0.02 % rather than
0.20 % (`24.00 ÷ 0.0002`). D uses the **15 %** a second approval misses instead of the 85 % it
catches (`24.00 ÷ 0.0003`) — the sign-of-the-control error.

**MCQ 16.4-B `[16.4.2 · Analysis]`** On the same figures, 4,800 payments a year total 68,000,000,
of which 1,150 payments worth 63,400,000 exceed the threshold. Comparing blanket dual approval with
thresholded dual approval:
- A. blanket is better, because it catches more bad payments in total
- B. they are equivalent, because the same catch rate applies
- C. thresholded is worth 80,180 a year against blanket's 400, because the 3,650 small payments cost 87,600 to approve and protect only 7,820 ✅
- D. neither is worthwhile, since the blanket policy nets only 400

*Rationale:* Blanket costs 115,200 and avoids 115,600; thresholded costs 27,600 and avoids 107,780.
A is true and irrelevant — the extra catches cost more than they save. B ignores that the value
protected varies with payment size while the approver's cost does not. D draws the wrong conclusion
from the blanket result: the control is strongly positive where it is targeted.

**MCQ 16.4-C `[16.4.1 · Analysis]`** A managed service costs 148,000 a year with an assessed 0.80 %
annual breach probability; a private deployment costs 410,000 at 0.15 %. The most defensible use of
the arithmetic is:
- A. compute the `EMV` avoided of 42,250, conclude the managed service wins, and close the question
- B. state the breakeven consequence of 40,307,692 and then debate whether the information at risk is worth that — which segments the answer: operating pipeline managed, live-transaction data private ✅
- C. choose the private deployment, because confidentiality cannot be quantified
- D. choose the managed service, because the probability differential is only 0.65 percentage points

*Rationale:* The arithmetic's role is to locate the argument, not to end it: 262,000 ÷ 0.0065 =
40,307,692, a figure implausible for a routine pipeline and entirely plausible for a live
transaction. A over-reads an assessment as a measurement. C abandons a usable structure. D confuses
a small differential with a small consequence.

**MCQ 16.4-D `[16.4.3 · Application]`** A change worth 416,000 a year waits for a committee with a
60-day effective interval and a 10-day paper lead time. The value forgone while it waits is closest
to:
- A. USD 6,268
- B. USD 45,589 ✅
- C. USD 68,384
- D. nil — the benefit accrues once approved

*Rationale:* `E[wait] = 60/2 + 10 = 40` days at `416,000 ÷ 365 = 1,139.7260` a day. A is the
delegated panel's 5.5-day wait. C prices the full 60-day meeting interval in place of `M/2 + L`,
dropping both the expected-half rule and the paper lead time. D ignores that a delayed benefit is a
forgone benefit.

### Self-check — KA 16.4

1. *How is a dual-approval threshold derived, and what does a blanket rule do?* — Approver cost ÷
   caught rate: USD 24.00 ÷ 0.0017 = USD 14,118. A blanket rule nets USD 400 a year by combining a
   strongly positive control above the threshold with an USD 79,780 loss below it.
2. *What does the deployment breakeven of USD 40,307,692 tell a director?* — That the managed
   service is defensible for a routine operating pipeline and indefensible for a live-transaction
   information set, so the answer is to segment the data rather than to choose once.
3. *What are the three levels of human involvement, and which may never be automated?* — Review
   (can stop it), approval (accountable for it), certification (represents it to a third party).
   Certification may never be generated by a model; a pipeline produces a draft for a person to
   verify and sign.

---

## Advanced topics — Domain 16

### 16.A.1 Agentic automation and the authorisation boundary

An **agent** differs from an assistant in one respect that changes every calculation in this
domain: it can **act**, not merely propose. The temptation is obvious — the 16.4.2 pipeline already
identifies which payments are sound, so allowing it to release them removes the approver's cost
entirely. Price that. Removing the thresholded human approval saves **USD 27,600** of approver time
and adds **USD 107,780** of expected loss, a net destruction of **USD 80,180** a year, which is
precisely the value of the control. Removing approval altogether takes expected loss from
**USD 20,400** (0.20 per cent of value, 15 per cent of it uncaught) to **USD 136,000**. The
inversion is exact and general: **an agent that replaces a control does not inherit the control's
effect, it inherits its absence**, and the saving it books is the control's cost while the loss it
creates is the control's benefit.

Three design rules follow, each with an arithmetic basis in this domain. **Scope authority by
consequence, using the threshold that already exists**: an agent may release below the 14,118
threshold, where human approval was destroying value anyway, and may only *prepare* above it. **Keep
the recording separate from the acting** — an identity that can both move money and amend the record
of having moved it defeats reconstruction, and 16.4.4's audit trail is the control being defeated.
And **treat every ingested document as data**: an agent with release authority that reads supplier
correspondence has an instruction-injection surface directly onto the payment run, which is the
single most consequential new exposure in an automated finance function. The honest summary is that
agentic automation is defensible exactly where the human control was uneconomic, and nowhere else —
which is a narrower claim than the technology's advocates make and a broader one than its sceptics
allow.

### 16.A.2 The automation you cannot switch off

Every automation case in this domain is a comparison with a manual process, and the comparison
assumes the manual process still exists. It does not, after about a year. The **fallback capacity**
question is therefore part of the case, and it is a staffing lead-time question rather than a cost
question. Restoring manual review over the estate's **56,400** records at **8.5 minutes** each
requires **7,990 hours** a year — **4.99375 full-time equivalents** at 1,600 productive hours — and
that capability cannot be assembled inside a reporting cycle, whatever the budget.

The number that belongs in the rollback plan is the cost of a **suspension**, because the fixed
cost is committed while the manual cost returns. Suspending the pipeline for one quarter means
paying the quarter's committed fixed cost of **USD 37,000** *and* reviewing 14,100 records manually
at 19.36 all-in — **USD 272,976** — for a total of **USD 309,976**, against the automated quarter's
**USD 168,976**: an incremental **USD 141,000** for one quarter's suspension. Two consequences.
**Rollback is a funded option, not a right**, and a plan that does not name its cost has not been
tested — the 141,000 should sit in the incident procedure beside the technical steps. And
**capability decay should be resisted deliberately**: retaining each pipeline's manual procedure as
documented, exercised work — a blind sample reviewed manually every period, which the audit
discipline of 16.3.3 already requires and prices — keeps the fallback real at almost no marginal
cost. That is the rare control
that satisfies two purposes at once, and it is the strongest argument for the audit line surviving
its first budget round.

### 16.A.3 The reviewer's automation eye

Invariants to test on any automation, model or governance claim in a finance function:

- Every automation business case carries an **error-cost line for both processes**, and both error
  rates are measured rather than assumed; omitting them flatters automation by 20.3822 per cent on
  16.1.2's figures.
- The **breakeven volume** is stated, and the case is assessed at the volume the entity actually
  has — 9,400 records is a 54,000 loss where 56,400 is a 416,000 gain.
- The breakeven is **recomputed at the consequence cost the work actually carries**; a case built at
  320 an error does not transfer to work worth 1,200 an error.
- Any threshold is justified by **total misclassification cost**, never by accuracy; and the
  optimum is confirmed by **marginal** precision against the break-even posterior
  (false-positive cost ÷ false-negative cost).
- Each threshold row's total cost **reconciles** to the adjacent row plus marginal benefit less
  marginal cost.
- The **prevalence** underlying any precision or recall figure comes from a **blind audit**, not
  from the model's own labels; recall against incomplete labels overstates itself (80 per cent
  claimed, 55.0459 per cent measured).
- Scenario statements carry **`k`** and the **share** breaching, and no generated minimum is
  presented as a defined case.
- Extraction reliance is assessed on **`aᵐ` for the load-bearing subset**, and that subset is
  chosen **before** the tool runs, verified against the **executed** document, with the version
  identifier recorded.
- Every published figure has a **local additive attribution** that reconciles to it, with a nil or
  explained residual, in currency.
- Every validation claim states its **confidence and bound**, and the sample size reconciles to
  `ln(α) ÷ ln(1 − p)`; the suite's **independence** is evidenced; and the claim is translated into
  **expected annual production errors**.
- Every model in production has an **inventory entry**, a tier derived from `uses × p ×
  consequence`, a revalidation interval capped by policy, and a named owner — and the inventory
  reconciles to production access logs.
- Every approval sits within a **written mandate**; certifications are never machine-generated; and
  the **latency cost** of the gate is known.
- The **rollback cost** is computed and the rollback has been exercised.

---

## Industry variations — Domain 16

- **Water and regulated utilities.** Telemetry is dense and consumption data is regulated, so the
  data spine's value is high and the deployment question of 16.4.1 is constrained by licence
  conditions on data location rather than by the arithmetic. Regulatory reset cycles break the
  training period, so the training-period bias of 16.3.3 is structural: a consumption model
  calibrated across a reset is calibrated on two different worlds.
- **Contracted power and availability payments.** Revenue is formulaic, so forecast automation is
  the easiest win in the estate and the anomaly detector's prevalence is low — which raises the
  false-positive burden at any recall, because a low base rate means most alerts are false however
  good the model. Threshold economics dominate, and the 16.1.3 arithmetic is the whole discussion.
- **Transport concessions.** Patronage data is high-volume, granular and genuinely
  non-stationary, so 56,400-record-scale automation pays easily while forecast automation is the
  hardest case in the book: the model that fitted last year's ramp is the model that will mislead
  through the next disruption, and validation must be on out-of-period data or it proves nothing.
- **Digital infrastructure.** Contract terms are numerous, bespoke and frequently amended, which
  makes document review the highest-value application and the superseded-draft failure of 16.2.2
  the highest-probability one — Case study B is drawn from this sector for that reason. Tenant data
  is commercially sensitive third-party data, so minimisation is a contractual obligation, not a
  preference.
- **Mining and resources.** Price-deck scenario generation is central and the sign-change appraisal
  pathologies of Domain 4 make generated-scenario reporting especially treacherous; a generated
  minimum across a commodity path distribution is a statement about the tail of an assumed price
  process, and should be labelled as such.
- **Public-sector and development-finance-supported projects.** Explainability is not optional: a
  figure that affects a public payment must be reconstructable by an auditor from its own trail, so
  16.3.1's additive attribution and 16.4.4's audit trail move from good practice to condition of
  funding, and models that cannot support them are simply unusable however accurate.

---

## Case study — Domain 16: the automation that lost at one asset and won at six (water)

**Situation.** Kestrel Water's sponsor group approved an automation platform for its
consumption-and-billing review on a business case built at the Kestrel project, where the finance
team had the appetite and the data. The case compared **USD 13.60** a record of manual review with
**USD 1.04** a record of automated processing and a committed **USD 148,000** a year, and showed a
saving. It carried no error-cost line for either process.

**What happened.** Three things, in sequence, each worth a number.

*The volume error.* At Kestrel's **9,400** records a year the platform cost **USD 235,984** against
the manual process's **USD 181,984** — **USD 54,000 a year more**, not less. Restated properly, with
the manual undetected-error rate measured at **1.80 per cent** and the automated at **2.60 per
cent** against a consequence of **USD 320**, the all-in costs are **19.36** and **9.36** a record
and the breakeven is **14,800 records a year**. The original case, omitting error costs, had implied
a breakeven of **11,783** — **20.3822 per cent** flattering — and even that was above Kestrel's
volume. The programme was rescoped to the six-asset estate's **56,400** records, where it saves
**USD 416,000 a year**. The rescoping was the right answer arrived at for the wrong reason: nobody
had computed a breakeven at all.

*The threshold error.* The payment-and-journal detector deployed alongside it was tuned, on the
vendor's recommendation, to maximise accuracy: **98.5729 per cent** at threshold T2, with **925**
alerts a year and a total misclassification cost of **USD 161,800**. Recomputed on the estate's own
costs — **USD 40** to investigate a false positive, **USD 320** for a missed error — the
cost-minimising threshold is **T4**, at **USD 114,800**, with **1,910** alerts and precision of
only **50.2618 per cent**. Moving the threshold saved **USD 47,000 a year** and was resisted for two
quarters by the team working the queue, whose objection — that half the alerts were now false — was
factually correct and economically irrelevant, since the break-even posterior is `40 ÷ 320 =
12.5 per cent`.

*The label-bias discovery.* In the second year the finance director commissioned a **blind audit**
of the items the detector had *not* flagged: **1,000** of **46,090**, reviewed independently of the
labelling team at 8.5 minutes an item, a cost of **USD 13,600**. It found **17** genuine errors — a **1.70 per cent**
rate, extrapolating to **784** missed errors a year against the **240** the detector's own
arithmetic assumed. The true anomaly population was therefore about **1,744**, not 1,200 — **45.3333
per cent** higher — the detector's true recall was **55.0459 per cent**, not 80, and the restated
total cost at T4 was **USD 288,880**, a factor of **2.5164** on the reported figure. The
false-negative cost had been understated by **USD 174,080** a year.

**How it resolved.** The estate adopted four changes, each traceable to one of the numbers above.
The business case is now restated annually on measured error rates rather than assumed ones. The
threshold is a delegated-panel decision within a written mandate, reviewed when either cost input
moves, rather than a vendor default. The blind audit became a permanent annual line, sized at
**1,000** records (above the **937** a ±1-percentage-point bound at 95 per cent confidence
requires), which returned **18.4471 times** its cost in its first year; the same discipline was
extended as a periodic manual sample across the billing pipeline, which is how the fallback capacity
of 16.A.2 is kept exercised. And the detector was **retrained** rather than retuned, using the audit's
confirmed findings as a new label source, because scaling the T4-to-T5 marginal benefit by the
1.4533 population factor gives **USD 55,808** against an unchanged **USD 78,000** of investigation
cost — no threshold reaches errors the labels never contained.

**What the domain teaches here.** Three failures, one cause. The volume error, the threshold error
and the label bias were all failures to compute something computable, and each was defended with a
qualitative argument — the platform is strategic, accuracy is the standard metric, the model reports
80 per cent recall. **Every one of those arguments dissolved on contact with arithmetic that took
under an hour.** The programme's total annual improvement from arithmetic alone, before any
technology changed, was **USD 470,000** of rescoping and **USD 47,000** of retuning, against a
restated loss estimate that was **USD 174,080** worse than believed — and the honesty of the third
number is what made the first two credible to the board.

## Case study B — Domain 16: the definition read from the wrong draft (digital infrastructure)

**Situation.** A hyperscale data-centre financing carried **USD 130,000,000** of senior debt with
annual debt service of **USD 17,000,000**, a **1.25× `DSCR`** covenant and a **1.20×** distribution
lock-up. The borrower's finance team used a document-extraction tool to build its covenant register
from the facility agreement's **340** defined terms, and reported the first annual compliance
certificate on that register.

**What happened.** The register's `CFADS` definition had been extracted from a **late negotiation
draft** rather than the executed agreement. In the executed version, **capitalised maintenance** was
struck **above** `CFADS`; in the draft it sat below. The reported `CFADS` was
**USD 22,780,000** and the reported `DSCR` **1.3400** — comfortably inside covenant. On the executed
definition, `CFADS` was **USD 20,230,000**, a difference of **USD 2,550,000**, and the `DSCR` was
**1.1900**.

The consequences followed mechanically. The **1.25× covenant** required `CFADS` of
**USD 21,250,000**, so the project was short by **USD 1,020,000** and in breach from the first test
date. The **1.20× lock-up** trigger of **USD 20,400,000** was also crossed, by **USD 170,000**, so
distributions were trapped — and the sponsor had already declared a dividend against the 1.3400
figure. The tool's per-item accuracy was never the issue: the extraction was faithful to the
document it was given, which is the failure mode no accuracy statistic can detect, and the
combinatorial arithmetic of 16.2.2 had already said what to expect — at 92 per cent per-item
accuracy the probability that all **26** load-bearing definitions were right was **0.114415**.

**How it resolved.** The sponsors injected an equity cure of **USD 1,020,000** to restore the
covenant, and paid a waiver fee of **25 basis points** on the outstanding **USD 130,000,000** —
**USD 325,000** — for the historic breach and the mis-stated certificate. Total direct cost
**USD 1,345,000**, before the declared dividend was reversed and before the lenders moved the
facility to monthly reporting for a year. The verification that would have prevented it —
**26** load-bearing definitions at half an hour each of a legal-and-finance specialist at
**USD 240.00** an hour — costs **USD 3,120**. The direct cost was **431.0897 times** the
verification.

**What the domain teaches here.** The extraction was accurate, the arithmetic was correct, and the
certificate was false. **The only control that would have caught it operates on the executed
document and records the version identifier**, and it is the control 16.3.4's tiering rule
independently demands: with four uses a year, an 88.5585 per cent chance of at least one wrong
load-bearing term and a 600,000 consequence, the model's annual `EMV` is **USD 2,125,404** and its
revalidation interval is **8.59 days** — shorter than the interval between its uses, which is the
formula's way of saying *verify every time*. Two derivations, from combinatorics and from expected
loss, converge on the same instruction, and the borrower had implemented neither.

---

## Executive perspective — Domain 16

What a project finance director cannot delegate in this domain:

- **The error-cost line in every automation case.** Not the labour saving — the two error rates and
  the consequence per error. Their omission flattered Case study A's breakeven by 20.3822 per cent
  and hid a 54,000 annual loss at the asset that sponsored the programme.
- **The threshold, because it encodes what a missed error costs us.** The number is a cost-ratio
  judgment with an owner, not a model setting: 12.5 per cent break-even posterior, T4 not T2,
  47,000 a year.
- **The blind audit, and its permanence.** It is the only instrument that measures what the model
  cannot see, and it is the first line cut in a budget round. It restated a claimed 114,800 loss to
  288,880 and returned 18.4471 times its cost.
- **The verification scope on load-bearing items.** Twenty-six definitions, named verifier, recorded
  date, executed document, version identifier — USD 3,120 against the 431.0897 multiple Case study B
  paid.
- **The approval architecture and its price.** 45,589 a change through a quarterly committee against
  6,268 through a mandated panel: tier it by consequence, write the mandate, and keep certification
  and lender disclosure to the director's own hand.
- **The rollback, funded and exercised.** USD 141,000 for a quarter's suspension and 4.99375
  full-time equivalents to restore manual capacity — figures that must exist before an incident, not
  after.

## Calculation exercises — Domain 16

**Exercise 16.1** Manual review takes 7 minutes a record at a loaded USD 96.00 an hour and leaves
2.20 per cent of records with an undetected error. An automated pipeline costs USD 0.95 a record in
processing and leaves 3.50 per cent undetected, on a committed fixed cost of USD 96,000 a year. An
undetected error costs USD 500. Find the breakeven volume.
*Solution.* Manual `7 × 1.60 = 11.20` plus `0.022 × 500 = 11.00`, total **USD 22.20**. Automated
`0.95` plus `0.035 × 500 = 17.50`, total **USD 18.45**. Advantage **USD 3.75**; breakeven
`96,000 ÷ 3.75 =` **25,600 records a year**. *Common error:* omitting the error-cost terms, which
gives an advantage of `11.20 − 0.95 = 10.25` and an apparent breakeven of 9,366 records — 63.4 per
cent too low, and the specific defect of 16.1.2.

**Exercise 16.2** A detector screens 30,000 items a year of which 900 are genuinely erroneous. A
false positive costs USD 60 to investigate; a missed error costs USD 900. Three thresholds give:
A — 450 true positives, 90 false; B — 675 true positives, 300 false; C — 855 true positives, 1,500
false. Compute accuracy and total cost at each, choose the threshold, and confirm the choice
marginally.
*Solution.* Accuracy: A `(450 + 29,010)/30,000 =` **98.2000 %**; B `(675 + 28,800)/30,000 =`
**98.2500 %**; C `(855 + 27,600)/30,000 =` **94.8500 %**. Total cost: A `90 × 60 + 450 × 900 =`
**410,400**; B `300 × 60 + 225 × 900 =` **220,500**; C `1,500 × 60 + 45 × 900 =` **130,500**.
**Choose C** — accuracy is maximised at B, and choosing B would cost **USD 90,000 a year**.
Marginal confirmation: B to C adds 180 true and 1,200 false positives, marginal precision
`180/1,380 =` **13.0435 %**, above the break-even posterior `60 ÷ 900 =` **6.6667 %** (one in 15);
benefit `180 × 900 = 162,000` against cost `1,200 × 60 = 72,000`, net **+90,000**, which reconciles
exactly to the fall in total cost. *Common error:* selecting B on accuracy — the 90,000 mistake this
domain exists to prevent.

**Exercise 16.3** How many independent, representative test cases, all passing, are needed to be
95 per cent confident that a model's defect rate is below 2 per cent? And at 99 per cent
confidence?
*Solution.* `n ≥ ln(α) ÷ ln(1 − p)`. At 95 per cent: `ln(0.05) ÷ ln(0.98) = 148.2837`, so **149**
cases. At 99 per cent: `ln(0.01) ÷ ln(0.98) = 227.9482`, so **228**. *Common error:* using `1/p =
50` or `100/p`-style reasoning; a defect rate bound of 2 per cent needs roughly `3/p` cases at
95 per cent confidence, not `1/p`, and the difference between 50 and 149 is the difference between a
claim and evidence.

**Exercise 16.4** An extraction tool is 95 per cent accurate per item. Eighteen extracted terms are
load-bearing. What is the probability the deliverable is entirely correct, and what per-item
accuracy would a 99 per cent clean sweep require?
*Solution.* `0.95¹⁸ =` **0.397214**, so a **39.7214 per cent** chance all eighteen are right and a
**60.2786 per cent** chance at least one is wrong. For a 99 per cent sweep,
`a ≥ 0.99^(1/18) =` **99.9442 per cent** per item. *Common error:* reporting the tool's 95 per cent
as the reliability of the register — the mistake that produced Case study B's false certificate.

**Exercise 16.5** An estate makes 3,600 payments a year totalling USD 52,000,000. A second approver
costs 12.5 minutes at USD 96.00 an hour. 0.25 per cent of payments are bad and a second approval
catches 80 per cent of them. Derive the dual-approval threshold, then value blanket against
thresholded approval given that 820 payments worth USD 48,900,000 exceed the threshold.
*Solution.* Approver cost `12.5 × 1.60 =` **USD 20.00**; caught rate `0.0025 × 0.80 =` **0.0020**;
threshold `20.00 ÷ 0.0020 =` **USD 10,000**. Blanket: cost `3,600 × 20 =` **72,000**, avoided
`0.0020 × 52,000,000 =` **104,000**, net **+32,000**. Thresholded: cost `820 × 20 =` **16,400**,
avoided `0.0020 × 48,900,000 =` **97,800**, net **+81,400**. The 2,780 sub-threshold payments cost
**55,600** and avoid **6,200**, destroying **49,400** — exactly the difference between the two
policies. *Common error:* concluding from the blanket policy's positive 32,000 that the policy is
sound; the control is strongly positive above the threshold and strongly negative below it, and only
the marginal view shows it.

## Practitioner's toolkit — Domain 16

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 16.T.1 — The automation business case, on one page

Mandatory lines, in this order, with no line permitted to be blank: the task and the **volume** the
deciding entity actually has (asset, estate, group); **manual cost per item** = minutes × loaded
rate, with the rate stated; **measured manual undetected-error rate** and its source; **consequence
per undetected error** and its derivation; **manual all-in cost per item**; **automated committed
fixed cost** a year; **automated processing cost per item**, including the referral rate and
adjudication time; **measured or assumed automated undetected-error rate**, flagged as which;
**automated all-in cost per item**; **advantage per item**; **breakeven volume**; **net annual
value at the actual volume**; the **automated error rate** and the **consequence cost** at which the
case reverses; the **rollback cost** for one period's suspension; and the **fallback capacity**
in full-time equivalents and lead time. Rule: a case with no error-cost line is returned unread.

### Toolkit 16.T.2 — Threshold and alert-queue sheet (per detector, per review)

Per candidate threshold: score cut · `TP` · `FP` · `FN` · `TN` · recall · precision · accuracy ·
alert count · adjudication hours · **total misclassification cost**. Above the table: the
false-positive cost and the false-negative cost, each with its owner and derivation, and the derived
**break-even posterior**. Below it: the **marginal** true positives, false positives, precision,
benefit, cost and net for each step, with the arithmetic check that each row's net reconciles to the
change in total cost. Footer lines: the operating threshold and who approved it; the date of the last
**blind audit** of unflagged items, its sample size, findings, extrapolated missed errors and
restated cost; and the prevalence in use, marked *labelled* or *audited*.

### Toolkit 16.T.3 — Model inventory, tier and approval record

One row per model in production, reconciled quarterly to production access logs — an unreconciled
inventory is the governance failure this artefact exists to prevent. Columns: model and version ·
purpose · owner · inputs and their golden sources · outputs and the decisions they touch · whether
any output reaches a lender or a certificate · **uses a year** · **defect probability `p`**, marked
*measured / estimated / inherited* · **consequence per defect** and its derivation · **annual
`EMV`** · **revalidation interval** = tolerance ÷ `EMV`, with the policy cap applied · date last
validated, sample size, confidence and bound · monitoring in place and the drift trigger ·
**approval tier and written mandate reference** · attribution method and the last reconciliation
residual · rollback procedure, its cost, and the date it was last exercised · named verifier for the
current period. Header: the tolerance parameter and its owner.

## Exam preparation — Domain 16

**What is assessed.** All-in cost per reviewed item for a manual and an automated process, and the
breakeven volume; the effect on the breakeven of the consequence per error and of each process's
error rate; the volume, error-rate and consequence breakevens as sensitivities; a classifier's
recall, precision, accuracy and total misclassification cost across thresholds, the cost-minimising
threshold, the annual cost of choosing the accuracy-maximising one, the break-even posterior, and
the marginal-precision test; scenario coverage `1 − (1 − p)ᵏ` and the required `k` for a stated
confidence; the sweep probability `aᵐ` and the per-item accuracy a stated sweep confidence demands;
the real against the apparent saving from machine-assisted model building, and the expected cost of
an omitted review; attributional completeness and the residual as a share of covenant headroom;
zero-failure validation sample size, the one-failure adjustment, and the translation of a validated
bound into expected annual production errors; label bias, the extrapolation from a blind audit, the
restated recall and cost, and the audit sample size for a stated bound; model tiering by annual
`EMV` and the derived revalidation interval; the deployment breakeven consequence; the
dual-approval threshold and the value of blanket against thresholded application; and governance
latency `E[wait] = M/2 + L` priced against a daily benefit.

**The calculations to do under time pressure.** `minutes × rate + rate of error × consequence`
for each process, then `fixed ÷ advantage`. The consequence and error-rate breakevens by
rearranging the same equation. `FP × cost + FN × cost` down a threshold column, then the marginal
step: `Δ TP × FN cost − Δ FP × FP cost`, and `FP cost ÷ FN cost` for the posterior. `ln(α) ÷ ln(1 −
p)` for both scenario coverage and validation sample size — the same formula twice, which is worth
noticing. `aᵐ` and `c^(1/m)`. `uses × p × consequence`, then `tolerance ÷ EMV`. `approver cost ÷
caught rate`, then cost and avoided loss on each population. `M/2 + L`, times `annual benefit ÷
365`.

**The traps.** Omitting the error-cost term from an automation case (Exercise 16.1, MCQ 16.1-A,
Case study A) · assuming the automated error rate equals or beats the manual one when the case
depends on it (16.1.2) · appraising an automation at the estate's volume when the deciding entity
is one asset, or the reverse (16.1.2, Case study A) · assuming a higher consequence per error
strengthens the automation case when it weakens it (MCQ 16.1-B) · selecting a threshold on accuracy
(Exercise 16.2, MCQ 16.1-C, Case study A) · testing a threshold step on *average* rather than
marginal precision (MCQ 16.1-D) · quoting recall or precision computed against incomplete labels
(16.3.3, MCQ 16.3-C, Case study A) · attempting to fix label bias by moving the threshold
(16.3.3) · drawing an audit sample from flagged items or having it reviewed by the labelling team
(16.3.3) · reporting a scenario count without `k`, or a generated minimum as a bank case (16.2.1,
MCQ 16.2-C) · reading per-item extraction accuracy as deliverable accuracy (Exercise 16.4,
MCQ 16.2-A, Case study B) · extracting from a data-room draft rather than the executed document
(16.2.2, Case study B) · letting the tool's confidence scores choose the verification scope
(MCQ 16.2-B) · booking the apparent saving from assisted model building while leaving the review at
its hand-built hours (16.2.3, MCQ 16.2-D) · treating a validation bound as a
production guarantee (MCQ 16.3-B) · repairing a failing test case instead of enlarging the suite
from 299 to 473 (16.3.2) · tiering models by transaction volume rather than by expected loss
(MCQ 16.3-D, 16.3.4) · omitting the policy cap on a computed revalidation interval (16.3.4) ·
presenting a deployment `EMV` as a decision rather than its breakeven consequence as a question
(MCQ 16.4-C) · deriving a dual-approval threshold without the catch rate (MCQ 16.4-A) · concluding
that a blanket control is sound because its net is positive (Exercise 16.5, MCQ 16.4-B) · pricing an
approval gate at zero (MCQ 16.4-D) · and assuming an agent inherits the effect of the control it
replaces rather than its absence (16.A.1).

**How the domain connects.** Domain 2 supplied the definitional exposure — 600,000 of `CFADS` on one
working-capital treatment — that makes the definitional layer of 16.1.1 and the verification scope
of 16.2.2 worth their cost. Domain 6 built the model, its six invariants and the effective-rate
check that 16.2.3's review must run, and supplied the 3,250,352 defect consequence that prices it.
Domain 10 supplied `CFADS`, the debt service, the 1.20× covenant and the 372,438 of annual headroom
that every data-quality figure in this domain is measured against, and the information covenants
that make model disclosure a contractual matter. Domain 11 established model risk as a priced
register line whose probability is a function of governance, which is the claim 16.3.4 turns into a
calendar. Domain 13's model audit is the external counterpart of 16.3.2's validation, and Domain 14
established that no pipeline may generate a representation. PML-AI supplied three registered forms
used unchanged: the mesh-versus-layer interface count, `EMV`, and governance latency. Backwards,
this domain gives every earlier **AI in this KA** section its arithmetic; forwards, it is the last
domain, and what it hands on is not another technique but a standard of proof — that a claim about
automation, like a claim about coverage, is worth exactly what its arithmetic can carry.

## Domain 16 summary
Responsible AI in finance is a quantitative discipline, and its four central results all contradict
a common belief. **Automation is decided by error costs, not labour.** Manual review of the estate's
consumption-and-billing records costs **USD 13.60** a record plus **1.80 per cent** of undetected
errors at **USD 320** — **USD 19.36** all-in — against the pipeline's **USD 1.04** plus **2.60 per
cent**, or **USD 9.36**, so on a committed **USD 148,000** the breakeven is **14,800 records a
year**: a **USD 54,000** annual loss at Kestrel's 9,400 and a **USD 416,000** annual gain across the
estate's 56,400. Omitting the error terms moves the apparent breakeven to **11,783** and flatters
automation by **20.3822 per cent**; repricing an undetected error at **USD 1,200** moves the real
breakeven to **50,000** and leaves the whole programme worth **USD 18,944** — so the more
consequential the work, the more volume automation needs, and the rule is to automate high-volume,
low-consequence work rather than important work. **Accuracy is the wrong objective for a detector.**
Over 48,000 payments and journals at a 2.5 per cent anomaly rate, with a false positive costing
**USD 40** and a missed error **USD 320**, cost is minimised at **USD 114,800** (threshold T4,
80 per cent recall, precision only **50.2618 per cent**) while accuracy peaks at **98.5729 per
cent** two thresholds away, where cost is **USD 161,800** — a **USD 47,000** annual price for a
metric choice. The rule is a **12.5 per cent** break-even posterior, one in eight, tested on
*marginal* precision: **17.9104 per cent** from T3 to T4 earns 16,400 and **5.7971 per cent** from
T4 to T5 loses 39,600. **Validation proves less than it appears.** **299** passing independent cases
bound a defect rate below 1 per cent at 95 per cent confidence — and at 56,400 uses a year that
still admits **564** errors; bounding expected errors at ten would need **16,895** cases, and one
observed failure raises the 299 to **473** — which is why monitoring, tiered human approval and a
funded rollback carry the assurance, not testing. **And controls, thresholds and verification are
marginal quantities.** Blanket dual approval nets **USD 400** a year; the same control above a
derived **USD 14,118** threshold nets **USD 80,180**, because 3,650 small payments cost 87,600 to
approve and protect 7,820. Around those four results sit the domain's other derivations: a data
spine costing **USD 900,000** returns an `NPV` of **+USD 1,221,674** on hours while its real value
is the single implementation of `CFADS` whose divergence is worth **1.6110** times Kestrel's annual
covenant headroom of **372,438**; a 92 per cent-accurate extractor gets all **26** load-bearing
definitions right **11.4415 per cent** of the time, so a 95 per cent sweep would demand
**99.8029 per cent** per item and the answer is **USD 3,120** of human verification instead — the
control Case study B omitted at a cost of **USD 1,345,000**, or **431.0897** times; machine-assisted
model building saves a real **33.3333 per cent**, not the apparent 70.8333, and the omitted 18 hours
of review are **USD 2,700** against **USD 1,137,623.20** of expected defect cost; a blind audit of
**1,000** unflagged items restated the detector's recall from 80 to **55.0459 per cent** and its cost
from 114,800 to **USD 288,880**, returning **18.4471** times its cost; a model inventory tiered on
`uses × p × consequence` gives the defined-term extractor an annual `EMV` of **USD 2,125,404** and an
**8.59-day** interval that independently reproduces "verify every use"; a deployment choice turns on
a breakeven consequence of **USD 40,307,692** rather than on an `EMV`; and an approval gate costs
**USD 45,589.04** a change through a quarterly committee against **USD 6,268.49** through a mandated
panel. Every one of those verification steps returns between eighteen and four hundred and thirty
times its cost, which is the domain's real finding and the one that survives every disagreement about the
technology: **AI proposes; the professional verifies, decides and remains accountable — and the
verification is the cheapest line in the budget.**
