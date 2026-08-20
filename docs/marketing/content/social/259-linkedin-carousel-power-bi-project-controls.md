---
platform:      LinkedIn carousel
type:          carousel
title:         Power BI for project controls: what good looks like
meta:          The dashboard averaged CPI across control accounts and reported 1.080. The correct aggregate was 0.938. Twelve slides on a Power BI model that can be audited.
primary_kw:    Power BI for project controls
secondary_kw:  DAX measures, star schema, precision recall F1, row-level security
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        HowTo
word_count:    1132
hashtags:      #ProjectControls #AIGovernance #PMO #EarnedValue
ab_id:         AB-01376
---

# Power BI for project controls: what good looks like

*LinkedIn document post — 12 slides, 1080 × 1350. No link in the body; the link goes in the first comment.*

**Post caption (the first two lines carry the post):**

Your dashboard reported CPI 1.080. The project's actual CPI was 0.938.
Nothing was mistyped. Someone averaged a ratio across control accounts.

Twelve slides on building a project controls model in Power BI that survives being questioned.

---

**Slide 1 — A dashboard is a set of claims**

Every tile on a project controls dashboard asserts a number that somebody will act on. The question is not whether it looks good. It is whether the number can be traced back to a ledger entry and a progress record in under a minute.

**Slide 2 — Build the model before you build the page**

Most bad project dashboards are a visual problem caused by a data problem. Flat tables joined ad hoc produce measures that cannot aggregate correctly, and no amount of formatting fixes it.

**Slide 3 — The arithmetic**
Two control accounts at the same data date:

| Control account | EV | AC | CPI |
|---|---:|---:|---:|
| CA-1 Civils | £4.00m | £4.40m | 0.909 |
| CA-2 Commissioning | £0.50m | £0.40m | 1.250 |

**Average of the two CPIs** = (0.909 + 1.250) ÷ 2 = **1.080** → reads as ahead of cost
**Correct aggregate** = (4.00 + 0.50) ÷ (4.40 + 0.40) = 4.50 ÷ 4.80 = **0.938** → behind

The small account swings the average because an average of ratios ignores size.

Write it as `CPI = DIVIDE(SUM(EV), SUM(AC))`, never as an average of a per-row CPI column.

Roll the same error up to a project BAC of £48m and EAC = BAC ÷ CPI gives **£44.4m** on the averaged index against **£51.2m** on the correct one — a **£6.8m** reporting error from one line of DAX.

**Slide 4 — The shape: facts in the middle, dimensions around them**

Fact tables carry the events: cost transactions, earned value by period, activity progress, commitments. Dimension tables carry the things you slice by: WBS, control account, calendar, resource, supplier, cost type.

One direction of filtering, one relationship per pair, no fact-to-fact joins. Model problems almost always trace back to a shortcut taken here.

**Slide 5 — One date table, marked as the date table**

A single continuous calendar covering every period in the model, marked as the date table, related to each fact by its own date key. Add period number, fiscal period, and a flag for the current data date.

Without it, time intelligence silently returns the wrong period, and the error is invisible because the number still looks plausible.

**Slide 6 — Measures, not calculated columns**

A calculated column is fixed at refresh and cannot respond to the filter the user applied. A measure recomputes in context, which is the entire point of a dashboard someone slices.

The rule of thumb: if it is a number you would ever want summed, averaged or filtered, it is a measure.

**Slide 7 — One definition per metric, held centrally**

"Percent complete" means at least four different things: cost, hours, physical progress and schedule. If two pages compute it differently the dashboard has two truths and the meeting becomes an argument about the tool.

Define each metric once as a named measure, write the definition in the description field, and make every visual point at that measure.

**Slide 8 — The data date must be on the page**

Every project number is an assertion about a moment. Put the data date and the last refresh timestamp on every page, in text large enough to read from the back of the room.

A dashboard without an as-at stamp will be screenshotted and quoted three weeks later as current.

**Slide 9 — If you put a model on the dashboard, measure it**

Say an at-risk flag runs over 500 activities in the look-ahead window. It flags **80**. Of those, **52** actually slipped. **71** slipped in total.

Precision = 52 ÷ 80 = **0.650** — how often a flag was right
Recall = 52 ÷ 71 = **0.732** — how much of the real slippage it caught
F1 = 2 × (0.650 × 0.732) ÷ (0.650 + 0.732) = **0.689**

That is **28 false alarms** and **19 missed slips**. Publish those three numbers beside the flag, or the flag is a colour with no accountability.

**Slide 10 — The threshold is a cost decision, not a maths decision**

Loosen the same flag so it catches more: **120** flagged, **64** correct.

Precision = 64 ÷ 120 = **0.533**
Recall = 64 ÷ 71 = **0.901**
F1 = **0.670** — lower than before

But misses fall from 19 to **7**, and false alarms rise from 28 to **56**. At twenty minutes of a planner's time each, that is about **19 hours a month**. Against seven missed slips on driving activities, that trade is obviously worth taking.

F1 assumes a false alarm and a miss cost the same. On a project they never do. Set the threshold on the consequence, then report the metrics honestly.

**Slide 11 — Row-level security, before anyone external sees it**

A subcontractor should see their own packages and no one else's rates. Row-level security applied in the model, tested with the view-as function, and reviewed whenever the sharing list changes.

Commercial exposure through a shared dashboard is a governance failure, and it is far easier to prevent in the model than to explain afterwards.

**Slide 12 — What good looks like, in one line each**

Star schema. One marked date table. Measures with written definitions. Data date and refresh on every page. Any tile traceable to source in under a minute. Model quality metrics published beside any model output. Row-level security tested. Nothing on the page that nobody has ever acted on.

Governed AI is 20% of every PCI Body of Knowledge for this reason: a model that recommends is a model that must be measured, versioned and explainable. The PCI AI Project Controls Leader (PCL-AI) covers 13 domains and 61 knowledge areas across delivery, finance and governed AI.

---

#ProjectControls #AIGovernance #PMO #EarnedValue

**First comment:** Where reporting automation and AI genuinely belong in project controls, and where they do not: https://pciai.org/ai-in-project-controls

---

*Every figure above is illustrative arithmetic, not project data. Microsoft Power BI is named as the tool in common use; PCI claims no affiliation with or endorsement by Microsoft. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and follow-up comment): [AI in project controls](https://pciai.org/ai-in-project-controls) with the anchor "where automation genuinely belongs", [generative AI for project reports](https://pciai.org/generative-ai-project-reporting) with the anchor "speed without losing the audit trail", and [earned value management](https://projectcontrolsinstitute.org/earned-value-management) with the anchor "the formulas the dashboard is reporting".*
