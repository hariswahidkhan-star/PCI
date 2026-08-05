---
id: BPG-15
series: S09
series_name: Best Practice Guides
title: Dashboards and data visualisation for controls
subtitle: Designing a control instrument, not a wall decoration
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 16
summary: >
  A controls dashboard is an instrument that changes decisions, and it should be engineered like one.
  This guide sets out the question a chart must answer before it is drawn, how to choose a chart form for
  the four questions controls actually asks, why an unreliable cut-off silently corrupts every tile above
  it, how to make a dashboard readable without relying on colour, and the discipline of one number per
  decision. It includes a fully worked axis-and-restatement example in which a dashboard shows an uptick
  while the arithmetic shows a fall into red.
linkedin:
  format: article
  hook: >
    Your dashboard shows this month's cost performance index up one hundredth. The same tile has restated
    downwards by an average of three hundredths every month for five months. The uptick is not an uptick —
    it is a data-completeness artefact, and the arithmetic can tell you so before the meeting.
  tags: [ProjectControls, DataVisualisation, Dashboards, CostEngineering, Reporting]
  asset: carousel-8
gated: false
related: [BPG-14, BPG-08, BPG-07, BPG-16, TPL-06, AIG-03]
bok_domains: [4, 6]
sources:
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 4 — Performance Management, Variance Analysis and Management Reporting, first authored draft, August 2026"
  - "PCL-AI Body of Knowledge (docs/bok/), Domain 6 — Earned Value Management and Forecasting, first authored draft, August 2026"
  - "PCI Canonical Facts (docs/publication-framework/00-framework/CANONICAL-FACTS.md), verified August 2026"
placeholders: 0
---

# Dashboards and data visualisation for controls

> Designing a control instrument, not a wall decoration.

**In one paragraph.** A controls dashboard is an instrument that changes decisions, and it should be
engineered like one. This guide sets out the question a chart must answer before it is drawn, how to choose
a chart form for the four questions controls actually asks, why an unreliable cut-off silently corrupts
every tile above it, how to make a dashboard readable without relying on colour, and the discipline of one
number per decision. It includes a fully worked axis-and-restatement example in which a dashboard shows an
uptick while the arithmetic shows a fall into red.

**Who this is for.** Project controls managers and reporting leads who own a dashboard; cost engineers and
planners whose numbers feed one; and PMO leads deciding what a portfolio view should contain.

---

## 1. The question comes before the chart

The commonest way to build a bad dashboard is to start from the data warehouse and ask what can be
displayed. The result is technically accurate, visually busy and decisionally inert: nobody can say what any
tile is for, so nobody acts on any of them, and the dashboard becomes something the controls team maintains
rather than something the project uses.

Invert the order. Before a chart is drawn, write down three things.

**The decision.** Not a topic — a decision. "Whether to release the second tranche of contingency" is a
decision. "Cost performance" is a topic. If no decision can be named, the chart is being drawn for
completeness, and completeness is not a reason.

**The threshold.** The value at which the decision changes. If a reader can look at the chart and cannot
tell whether they are on the acting side of a line, the chart has not been designed; it has been rendered.
A threshold may be a tolerance band, a funded confidence level or a contractual date, but it exists, and it
belongs on the chart.

**The owner.** The person who makes that decision, and the cadence at which they make it. A chart designed
for a weekly site meeting and a chart designed for a quarterly investment board have different aggregation,
different periods and often different measures, even when they describe the same work.

Write those three lines and most candidate charts delete themselves. That is the point: a dashboard's
quality is set far more by what it excludes than by what it contains, because attention is the scarce
resource in every meeting it was built to serve.

## 2. A dashboard is a control instrument

The word carries a bad analogy. A car's dashboard is passive: it reports what the engine is doing. A controls
dashboard should be closer to an instrument on a process plant — a reading against a setpoint, with an alarm
band, feeding a control action. Three properties follow.

**It shows position against a threshold, not position alone.** A cost performance index (CPI, earned value
divided by actual cost) of 0.96 means nothing on its own. Against a tolerance of 0.98 it is an exception;
against a tolerance of 0.90 it is noise. The threshold is part of the reading.

**It shows direction, not only level.** A measure sitting comfortably inside tolerance but moving one
hundredth a month in the wrong direction is a different management situation from the same value moving
nowhere, and a snapshot tile cannot tell them apart. Every measure carrying a threshold carries a trend.

**It has a defined response.** For each tile, someone should be able to answer: *what happens when this goes
amber?* "We discuss it" makes the tile decoration. "The control account manager produces a recovery plan
within five working days and the change is logged" makes it an instrument.

The corollary is worth stating plainly: a tile with no defined response should be removed, however
interesting it is. Interesting is a property of the analyst, not of the project.

## 3. Choosing the form for the question

Controls asks a small number of questions repeatedly. Matching form to question is most of the craft.

| Question the reader has | Form that answers it | Common wrong choice |
|---|---|---|
| How are we tracking against plan over time? | Line or cumulative S-curve, with the plan, the actual and the forecast as three series | A bar chart per period, which hides cumulative position |
| What moved the number since last period? | Waterfall (variance bridge), ordered by contribution | A table of variances, which makes the reader do the addition |
| How do items compare, and which are worst? | Horizontal bar, sorted by value, labelled with the number | An unsorted bar chart, which turns comparison into a search |
| What is the spread of possible outcomes? | Histogram or cumulative distribution curve with the funded confidence level marked | A single point estimate presented as fact |
| What is this total made of? | Stacked bar, few categories, consistent order across periods | A pie chart with more than about five slices |
| Are two measures related? | Scatter, with the relationship stated in words | A dual-axis line chart, which manufactures relationships |

Two rows mislead most often. **Dual axes** let the designer choose two scales, and that choice — not the
data — determines whether the lines appear to move together. If the point is that two measures are related,
say so in words and show a scatter or a ratio; if the point is that both matter, stack two charts on a shared
time axis. **Distributions** are generated constantly by controls — schedule risk output, cost risk output,
estimate ranges — and then reported as one number. A cumulative curve with the funded confidence level marked
lets a reader see how much the answer moves for a small change in confidence, and that slope is often the most
decision-relevant thing on the dashboard. `BPG-17 — Quantitative schedule risk analysis` covers how those
distributions are produced and what they may honestly be said to mean.

## 4. The cut-off problem: a dashboard is only as reliable as its data date

Every tile inherits the reliability of the data date beneath it. This is not a data-quality footnote; it is
the single largest source of dashboard error, and it is systematic rather than random.

Cost arrives late. Subcontractor applications, goods-received records, remote timesheets and internal
recharges all land after the period they belong to, while earned value is often computed on time because
your own team generates it. At first publication, therefore, the numerator of every efficiency measure is
more complete than the denominator, so the measure flatters — *every month*, in the *same direction*. That is
a bias, not noise, and a bias can be measured. `BPG-07 — Accruals and cut-off discipline` covers the accrual
practice; what matters here is what it does to the picture.

Three design consequences follow.

**Mark provisional points.** The most recent point on any trend is the least reliable point, and it is the
one the meeting acts on. Distinguish it — a hollow marker, a dashed final segment, an explicit "provisional"
label — so nobody reads it as equivalent to the settled history behind it.

**Publish the restatement history.** Keep, for each measure, the value as first published and the value as
restated one period later. The average movement between them is the measure's completeness bias — the most
useful diagnostic number a reporting function can hold. Section 8 works it through.

**Snapshot, do not re-render.** A dashboard that recomputes history from live data on every open changes its
own past silently: two people looking at "the March chart" in April and in June see different charts, and
neither knows it. Store each period's figures as issued, and make restatement a visible act.

## 5. Accessibility is a correctness requirement

Meaning encoded in colour alone is meaning some readers do not receive. Assume that part of your audience
cannot reliably separate red from green, that someone will print the report in greyscale before the meeting,
and that a projector will flatten whatever palette you chose. Under those assumptions a red-amber-green
status that is *only* a colour communicates nothing to a meaningful part of the room.

The fix is redundant encoding — every status carries at least two signals:

- a **shape or symbol** as well as a fill, so the category survives greyscale;
- a **label** with the actual value and the threshold it breached, so the reader is not decoding at all;
- **position**, which is the strongest visual channel available — a value plotted against a marked
  threshold line communicates status before any colour is processed.

Two further rules. Keep every category in the same order and the same encoding across every tile, because a
reader who must relearn the key on each one will stop reading tiles. And give every chart a caption that
doubles as its alternative text: what is shown, over what period, and what the reading is. "CPI trend" is a
title. "Cost performance index by month, January to June, falling from 1.01 to 0.95 against a tolerance of
0.98" is an accessible description, and it is also the sentence the reader repeats in the meeting.

## 6. One number per decision

The strongest discipline in dashboard design, and the hardest to hold, is that each decision gets one
headline number. Not a panel of five related indices from which the reader must synthesise a position —
one number, with its threshold, its direction and its provisional status.

This is not simplification for its own sake. When several measures that can disagree support one decision,
the meeting reconciles measures instead of deciding, and the person with the strongest view picks whichever
measure supports it. Deciding in advance which number governs removes that choice, which is why it is
resisted.

The supporting detail does not disappear; it moves. The structure that works is overview first, detail on
demand: one screen answering where we are, where we are heading, what is outside tolerance and what is being
done — with every red and amber traceable in one step to the control account and its narrative. Every
promotion of detail onto the front page costs attention the headline numbers were competing for.

Where several measures genuinely bear on one decision, combine them explicitly and publish the combination
rule. An explicit rule can be argued with and improved; an implicit one is re-invented at every meeting.

## 7. How this goes wrong

**The dashboard that reports everything available.** Forty tiles, none with a defined response. It survives
because removing a tile requires someone to say it is not needed. The test that breaks the deadlock: ask each
tile's owner what decision changed because of it in the last six months. Tiles with no answer come off.

**The axis chosen after the data is seen.** A truncated axis turns a movement of a few hundredths into a
cliff; a zero-based axis on a ratio that lives between 0.9 and 1.1 turns a serious decline into a flat line.
Both are defensible in isolation and both are choices about the message. Fix the axis to the decision
thresholds *before* the period's data arrives, and keep it fixed across periods.

**Comparing the provisional to the settled.** The current month is compared with a fully restated prior
month and the difference reported as movement. The worked example below quantifies this; it is the failure
that most often causes a project to relax in the month it should have escalated.

**The refreshing dashboard with no memory.** Nobody can reconstruct what was reported when a decision was
taken, which makes the dashboard useless as evidence and, on a project heading for a dispute, worse.

**Colour doing all the work.** Coloured circles with no labels, shapes or values: fast to build, unreadable
in print, ambiguous to some readers, impossible to quote in minutes.

**Two dashboards that disagree.** A project view and a portfolio view built on separately maintained
extracts. The meeting becomes a reconciliation. One source, aggregated through the coding structure, is the
only cure; where reconciliation is unavoidable, publish the difference and its cause on both views rather
than letting it be discovered.

**The chart redrawn until it is acceptable.** Rarely conscious — a series of individually reasonable choices
about axis, window and grouping, each nudging the picture. The countermeasure is procedural: fix the chart
specification with the tolerance, and record any mid-period change to it alongside the change to the data.

## 8. Worked example

*Illustrative figures. Units are dimensionless index values unless stated. Period is monthly, months 1 to
6 of a delivery phase. Tolerance for this control account: amber below 0.98, red below 0.95. All arithmetic
is shown; percentages of plot height are exact.*

### 8.1 The setup

A control account's cost performance index (CPI) is reported monthly on a dashboard tile. The settled
series — each month's value after one period of restatement — and the current month's provisional value
are:

| Month | Value on the tile | Status of the value |
|---|---:|---|
| 1 | 1.01 | settled |
| 2 | 0.99 | settled |
| 3 | 0.98 | settled |
| 4 | 0.96 | settled |
| 5 | 0.95 | settled |
| 6 | 0.96 | **provisional** — first publication |

Read naively, month 6 is an improvement: 0.96 against 0.95, up 0.01, and back above the red line.

### 8.2 Part A — what the axis does to the same six numbers

The full movement across the series is `1.01 − 0.95 = 0.06`.

**On a zero-based axis of 0.00 to 1.20**, the plot spans 1.20 index points, so the movement occupies
`0.06 ÷ 1.20 = 0.05`, or **5 % of the plot height**. Six months of steady decline through two tolerance
thresholds renders as a flat line.

**On an axis truncated to 0.92–1.02**, the plot spans 0.10, so the movement occupies
`0.06 ÷ 0.10 = 0.60`, or **60 % of the plot height**, and month 5 sits at
`(0.95 − 0.92) ÷ 0.10 = 0.30`, near the floor. The same decline renders as a collapse.

**On an axis of 0.85–1.05, chosen because it contains both tolerance thresholds with margin**, the plot
spans 0.20 and the points sit at these heights:

| Month | Value | Height on plot = (value − 0.85) ÷ 0.20 |
|---|---:|---:|
| 1 | 1.01 | 0.16 ÷ 0.20 = 80 % |
| 2 | 0.99 | 0.14 ÷ 0.20 = 70 % |
| 3 | 0.98 | 0.13 ÷ 0.20 = 65 % |
| 4 | 0.96 | 0.11 ÷ 0.20 = 55 % |
| 5 | 0.95 | 0.10 ÷ 0.20 = 50 % |
| 6 (provisional) | 0.96 | 0.11 ÷ 0.20 = 55 % |

The amber threshold of 0.98 sits at `(0.98 − 0.85) ÷ 0.20 = 65 %` and the red threshold of 0.95 at
`(0.95 − 0.85) ÷ 0.20 = 50 %` — both on the chart, both crossed by the series, and the movement occupies
30 percentage points of plot height between month 1 and month 5. **The axis is set by the thresholds, not
by the data**, which is why it can be fixed before the period opens and does not need a decision after the
numbers are known.

### 8.3 Part B — what the provisional point is actually worth

The reporting function has kept, for each month, the value as first published and the value after one
period of restatement:

| Month | First published | Restated one month later | Movement |
|---|---:|---:|---:|
| 1 | 1.05 | 1.01 | −0.04 |
| 2 | 1.02 | 0.99 | −0.03 |
| 3 | 1.00 | 0.98 | −0.02 |
| 4 | 0.99 | 0.96 | −0.03 |
| 5 | 0.98 | 0.95 | −0.03 |

Mean restatement:

```
sum of movements = (−0.04) + (−0.03) + (−0.02) + (−0.03) + (−0.03) = −0.15
mean            = −0.15 ÷ 5 = −0.03
```

Every movement is negative. That is the signature of a completeness bias rather than random error: late
cost lands in the denominator after publication, so first publication is systematically optimistic by
roughly three hundredths.

Applying the measured bias to the provisional month-6 value:

```
expected settled month 6 = 0.96 + (−0.03) = 0.93
```

**The reading changes.** The tile shows 0.96, an apparent rise of 0.01 on the settled month-5 value of
0.95, sitting in amber. The bias-adjusted expectation is 0.93 — a **fall of 0.02** on month 5
(`0.93 − 0.95 = −0.02`) and **below the red threshold of 0.95**. The month the dashboard says has stabilised
is, on the project's own restatement history, the month it went red.

### 8.4 What the numbers do and do not support

The adjusted figure is an expectation, not a measurement, and it does not license reporting 0.93 as the CPI.
The correct treatment is to report 0.96 as provisional, state the measured bias of −0.03 on the face of the
chart, and take the escalation the red threshold requires — recording that it was triggered by the adjusted
expectation, so that if month 6 settles at 0.96 after all, the decision is reviewable rather than
embarrassing.

Three assumptions carry the result: the history is five points, which is short; the bias is assumed stable,
which fails if its cause is fixed or worsens; and it is specific to this control account's cost-capture
pattern, so it may not transfer to another account, let alone another project.

## 9. Checklist

Take this into the design review, not into the reading of the report.

**Per tile**

- [ ] The decision this tile supports is written down, in decision form, not topic form.
- [ ] The threshold at which that decision changes is written down and drawn on the chart.
- [ ] The response when the threshold is breached is written down, with an owner and a timescale.
- [ ] The chart form matches the question — trend, variance, comparison, distribution or composition.
- [ ] The vertical axis is fixed to the thresholds and does not change when the data does.
- [ ] The most recent point is visually marked as provisional.
- [ ] Status is carried by at least two channels: never colour alone.
- [ ] The caption states what is shown, over what period, and what the reading is.

**Per dashboard**

- [ ] The top level fits one screen and answers where we are, where we are heading, what is outside
      tolerance, and what is being done.
- [ ] Every red and amber reaches its control account and its narrative in one step.
- [ ] Each decision has exactly one headline number; supporting measures sit at the level below.
- [ ] Categories, ordering and encoding are identical across every tile.
- [ ] Every tile that failed the "what decision changed" test has been removed.

**Per period**

- [ ] Published figures are snapshotted; history does not silently re-render.
- [ ] First-published and restated values are recorded for every measure carrying a threshold.
- [ ] The restatement bias is recomputed and shown on the face of the affected charts.
- [ ] Project and portfolio views reconcile, or the difference is published on both.
- [ ] Any change to a chart specification is recorded alongside the data change.

A dashboard built this way is slower to produce and considerably shorter. It also becomes quotable: when a
number on it moves, the meeting knows what changes, who acts and by when — which is the only property that
distinguishes an instrument from a picture of one.

---

## Related

- `BPG-14 — Monthly reporting that gets read` — the report the dashboard sits inside, and the narrative that must accompany it
- `BPG-08 — Earned value in practice` — where the indices on the tiles come from and what they legitimately mean
- `BPG-07 — Accruals and cut-off discipline` — the cost-capture practice that determines whether any tile can be trusted
- `BPG-16 — Risk registers that work` — the risk exposure that belongs on the dashboard, and the form it should take
- `TPL-06 — Monthly project controls report` — the reporting structure these tiles feed
- `AIG-03 — Data readiness: what AI needs before it is any use` — the upstream data conditions that determine what can be automated

## Sources and standards

- PCL-AI Body of Knowledge (`docs/bok/`), Domain 4 — Performance Management, Variance Analysis and
  Management Reporting, first authored draft, August 2026: chart-to-question fit, common distortions,
  dashboard structure, exception reporting and cadence.
- PCL-AI Body of Knowledge (`docs/bok/`), Domain 6 — Earned Value Management and Forecasting, first
  authored draft, August 2026: the definition and behaviour of the performance indices used in the worked
  example.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  naming, status and claims policy.

No external statistic, benchmark or vendor capability is cited, because none was verified for this guide.
All figures in §8 are illustrative and were constructed for teaching.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
