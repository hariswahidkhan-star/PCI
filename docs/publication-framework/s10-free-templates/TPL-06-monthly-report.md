---
id: TPL-06
series: S10
series_name: Free Templates
title: Monthly project controls report
subtitle: A section structure that opens with the decisions required, and a one-page executive summary that survives being read alone
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, executive]
level: practitioner
reading_time_min: 18
summary: >
  A monthly project controls report exists to cause decisions, and most of them fail because the
  decisions are on page eleven. This template inverts the order: the decisions required come first, then
  a one-page executive summary that is true when read alone, then status, a variance narrative that names
  cause, action and owner, look-ahead, risk movement, the change position and the cash position — each
  with the fields defined and the calculated ones given in words and as spreadsheet expressions.
linkedin:
  format: document
  hook: >
    Put the decisions you need on page one. A monthly report whose first page is a status summary is
    asking to be skimmed, and a report that is skimmed causes nothing.
  tags: [ProjectReporting, ProjectControls, CostEngineering, ProjectGovernance]
  asset: one-pager
gated: true
related: [TPL-01, TPL-04, TPL-05, TPL-07, TPL-08, TPL-09, TPL-10, BPG-14, BPG-15]
bok_domains: [4, 6, 11]
sources:
  - "PCI Master Formula Sheet (docs/downloads/master-formula-sheet.md), August 2026 — published under the credential's retired code; the credential is PCL-AI"
placeholders: 0
---

# Monthly project controls report

> The report structure that puts the decisions first and makes the summary true on its own.

**In one paragraph.** A monthly project controls report exists to cause decisions, and most of them fail
because the decisions are on page eleven. This template inverts the order: the decisions required come
first, then a one-page executive summary that is true when read alone, then status, a variance narrative
that names cause, action and owner, look-ahead, risk movement, the change position and the cash position —
each with the fields defined and the calculated ones given in words and as spreadsheet expressions.

**Who this is for.** Project controls managers who produce the report; project directors who present it;
and sponsors, steering group members and client project managers who have to act on it in the twenty
minutes before the meeting.

---

## 1. When to use this

Use it for the recurring report that goes to people who can change the project's direction — sponsor,
steering group, client. Its cadence, data date and issue offset are fixed in the controls execution plan
(`TPL-01` §3.11) and it is assembled after the progress measurement sheet (`TPL-05`), the earned value
sheet (`TPL-07`) and the forecast comparison (`TPL-08`) have closed, in that order.

It is not the right instrument for two adjacent jobs. A weekly progress flash to the delivery team is
shorter, narrower and does not need an executive summary. A live dashboard answers "what is happening now";
this report answers "what has changed, why, and what needs deciding" — see `BPG-15` for how the two divide.

The test of whether it is working is not whether it is read. It is whether the decisions listed in §1 of
the report were taken by the dates the report asked for. Track that, and a report that has stopped causing
decisions becomes visible before it becomes a habit.

## 2. How to complete it

**Write section 1 last and put it first.** The decisions required are the output of the analysis, not the
introduction to it. Each entry names the decision, the person who must take it, the date by which, the
consequence of not taking it by then, and the section that supports it. A decision with no date is a
discussion topic.

**Make the executive summary true standing alone.** Assume it will be forwarded without the rest. Every
figure in it carries its units and basis, every judgement carries its ground, and nothing in it depends on
a caveat that appears later.

**Write the variance narrative as cause, action, owner, date, expected effect.** Five fields. A narrative
that restates the number is not a narrative: "cost variance of CU (220) thousand driven by adverse cost
performance" says nothing that the table above it did not. "Piling output averaged 3.1 piles per shift
against an estimate basis of 4.0, from harder-than-anticipated ground in the north-east quadrant" says
something a reader can act on.

**Report movement, not only position.** A cost performance index of 0.955 means little without last
period's. Movement is what tells a reader whether the project is deteriorating, stabilising or recovering,
and it is what a monthly cadence exists to show.

**Report the forecast with its range and its selected method.** A single estimate at completion with no
method named and no range invites a reader to treat it as fact. The comparison in `TPL-08` produces both.

**Do not carry colour alone.** Where a status indicator is used, pair it with a word — `Behind`, `On plan`,
`Ahead` — so the report survives monochrome printing, projection and readers who do not distinguish the
colours.

**Cite rather than restate.** The definitions of the earned value measures belong to `TPL-07`, the rules of
credit to `TPL-05`, the forecast methods to `TPL-08`. The report carries the numbers and the meaning; it
does not re-teach the arithmetic every month.

**Close section 14 honestly.** A data quality statement that says which figures are estimated, which
cut-offs did not align, and what is not yet in the numbers costs half a page and buys the report its
credibility. It is also the section a reader remembers when something later turns out to have been
missing.

**Using the tables.** Copy a table block, paste into a single spreadsheet column, split on the pipe
character, and delete the alignment row.

## 3. The template

### 3.0 Control block

| Field | Entry |
|---|---|
| Project / contract | |
| Reporting period | |
| Data date | |
| Issue date | |
| Report version | |
| Prepared by / reviewed by / approved by | |
| Distribution | |
| Classification | |
| Basis: currency, scale, nominal or constant prices | |

### 3.1 Decisions required this period

The first page. Nothing precedes it.

| Ref | Decision required | Decision owner | Required by | Consequence if not taken by that date | Supporting section |
|---|---|---|---|---|---|
| | | | | | |

**Calculated column — days remaining.** In words: calendar days from the report's data date to the date the
decision is required.

```
=IF(OR($D2="",$B$2=""),"",$D2-$B$2)
```

Reference the data date cell, not `TODAY()`. A report is a fixed statement of a position, and a field that
recalculates when the file is reopened makes the issued report disagree with itself.

### 3.2 Executive summary — one page

Written to be read alone. The block below is the whole page.

| Element | Content |
|---|---|
| Position in one sentence | What state the project is in, and the single most important thing about it |
| Overall status | Schedule, cost, risk, change and cash, each with a word and, if used, a colour |
| Headline figures | Budget at completion, per cent complete, cost and schedule performance indices with last period's, selected estimate at completion with its range, forecast completion date with the variance in days |
| What changed this period | Three items at most, each one sentence, each stating the movement |
| What to worry about | One item. The thing that will matter in three months and is cheapest to act on now. |
| Decisions required | The references from §3.1, listed, with their dates |
| Basis and rounding | Units, currency, scale, data date and rounding rule |

### 3.3 Health, safety, environment and quality

| Field | Entry |
|---|---|
| Reportable incidents this period / cumulative | |
| Observations and near misses | |
| Open non-conformances, and any over the agreed ageing threshold | |
| Environmental or consent issues | |
| Anything requiring a decision | Cross-reference to §3.1 |

Report this first among the status sections and report it factually. It is not a controls output, and the
report is not the place to interpret it — but a controls report that omits it signals a set of priorities.

### 3.4 Progress and schedule status

| Field | Entry |
|---|---|
| Physical per cent complete this period / last period / movement | From `TPL-05` |
| Planned per cent complete at the data date | |
| Milestone table | Below |
| Critical path description | Which path is driving, and whether it changed this period |
| Float position | Total float on the driving path; whether it moved |
| Schedule health | Result of the last schedule quality review — see `TPL-14` |

| Milestone | Baseline date | Forecast / actual date | Variance (days) | Contractual? | Movement since last period |
|---|---|---|---|---|---|

**Calculated column — variance in days.** In words: forecast date less baseline date, in calendar days; a
positive figure is late.

```
=IF(OR($B2="",$C2=""),"",$C2-$B2)
```

### 3.5 Cost status

Carry the control-account table from `TPL-07` and add movement. Do not restate the formulas; cite the
sheet.

| Control account | BAC | PV | EV | AC | CV | SV | CPI | CPI last period | SPI | SPI last period | % complete |
|---|---|---|---|---|---|---|---|---|---|---|---|

Beneath it, the reconciliation to total authorised budget: measured total, undistributed budget,
contingency reserve, total.

### 3.6 Variance narrative

One block per control account breaching the reporting threshold set in `TPL-01`. Five fields, all
required.

| Field | Entry rule |
|---|---|
| Control account and variance | The figure, so the block stands alone |
| Cause | The physical or commercial reason. Not a restatement of the number. Where more than one cause applies, quantify each. |
| Action | What is being done, specifically |
| Owner | A named person |
| By when | A date |
| Expected effect | What the reader should see in next period's numbers if the action works. This is what makes the narrative testable next month. |
| Status of last period's action | Done, in progress, or abandoned — and if abandoned, why |

That last row is the one that changes behaviour. A narrative that is never checked against the previous
month's promise becomes a monthly essay.

### 3.7 Forecast

| Field | Entry |
|---|---|
| Selected estimate at completion and method | From `TPL-08` |
| Range across methods | Lowest and highest, with the spread as a percentage of budget at completion |
| Variance at completion | |
| Movement since last period, and what caused it | Performance, scope, or a change of method — say which |
| To-complete performance index to budget at completion, against the achieved cost performance index | The credibility test |
| Forecast completion date and its basis | |
| Remaining contingency, and pending change requests against it | From `TPL-04` |
| Can the forecast be absorbed? | The single sentence a sponsor needs |

### 3.8 Look-ahead

| Period | Key activities | Milestones due | Planned value in the period | Resource or access constraints | What could stop it |
|---|---|---|---|---|---|
| Next period | | | | | |
| Following two periods | | | | | |

State planned value for the coming period explicitly. It is the figure against which next month's earned
value will be judged, and printing it in advance makes next month's schedule variance a shared
expectation rather than a surprise.

### 3.9 Risk movement

| Field | Entry |
|---|---|
| Risks opened this period | Reference, description, probability, impact, expected value |
| Risks closed | Reference, and whether closed by mitigation, by realisation or by expiry |
| Risks changed | Reference, what moved, and why |
| Top exposures | The largest few by expected value |
| Total expected value of the register | |
| Exposure against remaining contingency | The comparison, with the caveat below |

**Calculated column — expected value.** In words: probability multiplied by impact, in the same currency
and scale as the rest of the report.

```
=IF(OR($B2="",$C2=""),"",$B2*$C2)
```

State the caveat every time: a sum of expected values is not a confidence level and is not the amount of
contingency required. It is an arithmetic aggregate of point estimates, useful for ranking and for
movement, and it says nothing about the distribution of outcomes. Where the project needs that, it needs a
quantitative analysis — see `TPL-11`.

### 3.10 Change position

From the register in `TPL-04`.

| Field | Entry |
|---|---|
| Approved this period — number and value | |
| Approved to date — number and value | |
| Pending — number and value, with the oldest and its age | |
| Rejected or withdrawn to date | |
| Revised budget at completion | |
| Contingency drawn to date / remaining | |
| Pending value against remaining contingency | |
| Approved but not yet implemented | The list. This should normally be empty. |

### 3.11 Cash position

| Field | Entry |
|---|---|
| Value certified this period / cumulative gross | |
| Retention held | |
| Net certified for payment | Calculated |
| Cash received cumulative | |
| Receivables outstanding | Calculated |
| Uncertified work in progress | Calculated |
| Project cash invested | Calculated |
| Days sales outstanding | Calculated |
| Forecast net cash movement next period | From `TPL-09` |
| Payment issues and disputes | |

**Net certified for payment.** In words: gross certified value less retention held.

```
=N(B2)-N(C2)
```

**Receivables outstanding.** In words: net certified for payment less cash received.

```
=N(D2)-N(E2)
```

**Uncertified work in progress.** In words: actual cost incurred less gross certified value — work done
and paid for by the project but not yet certified by the client.

```
=N(F2)-N(B2)
```

**Project cash invested.** In words: actual cost incurred less cash received. It should equal uncertified
work in progress plus retention held plus receivables outstanding, and checking that identity every month
finds errors in the certification data.

```
=N(F2)-N(E2)
```

**Days sales outstanding.** In words: receivables outstanding divided by the average daily certified value
for the period. With receivables in `G2`, value certified in the period in `H2` and days in the period in
`I2`:

```
=IF(OR(N(H2)=0,N(I2)=0),"",G2/(H2/I2))
```

State the base period. Days sales outstanding computed on a single month is volatile on a project with
lumpy certification, and it is a different figure from one computed on a rolling three months. Whichever
you use, use it every month.

Note in the report that certified value is not necessarily recognised revenue. Whether, when and how much
revenue is recognised depends on the financial reporting framework the reporting entity applies and on its
accounting policy, and it varies between jurisdictions and entities. The controls report states what has
been certified and what has been received; it does not state the accounting result.

### 3.12 Interfaces and dependencies

| Interface / party | What is owed | To whom | Due | Status | Effect if late |
|---|---|---|---|---|---|

### 3.13 Actions and decisions log

| Ref | Action or decision | Owner | Raised | Due | Status | Note |
|---|---|---|---|---|---|---|

Carry every item forward until it is closed. An item that disappears without being closed teaches readers
that the log is decorative.

### 3.14 Data quality and basis statement

| Field | Entry |
|---|---|
| Cut-off dates actually applied to each input | |
| Where cut-offs did not align, and the effect | |
| Figures that are estimated or accrued rather than actual | |
| Known omissions | |
| Changes of method or basis this period | |
| Where artificial intelligence assisted in preparing this report, and who validated the output | Per the register in `TPL-01` §3.13 |

### 3.15 Appendices

Full control-account table · full change register · full risk register extract · schedule extract ·
glossary of terms used in this report.

## 4. Worked fragment

*Illustrative figures.* The executive summary page and the cash block for the fictional facility upgrade
project used throughout this series.

- **Currency and scale:** generic currency units (CU), thousands.
- **Basis:** nominal, single currency, no escalation.
- **Reporting period:** May 2026. **Data date:** 31 May 2026. **Issue date:** 8 June 2026, six working days
  after the data date per `TPL-01` §4.
- **Rounding:** amounts to the nearest CU thousand; indices to three decimal places; percentages to one
  decimal place, half away from zero. Adverse figures in parentheses.

### 4.1 Section 1 — decisions required

| Ref | Decision required | Owner | Required by | Consequence if not taken by that date | Section |
|---|---|---|---|---|---|
| D-01 | Approve or reject BCR-014, ground improvement at 22 obstructed pile positions, CU 295 thousand from contingency | Project sponsor | 19 Jun 2026 | Piling cannot resume at those positions. Structural steel erection in the north-east quadrant stops from approximately 17 Jul 2026, at which point the delay moves onto the driving path. | §3.10, §3.6 |
| D-02 | Confirm the funding route for the forecast variance at completion of CU (430) thousand, given remaining contingency of CU 300 thousand and BCR-014 pending against it | Project sponsor with finance | 30 Jun 2026 | The forecast overrun continues to be reported against contingency that would not cover it if BCR-014 is approved | §3.7 |
| D-03 | Approve the proposed recovery plan for the 21-day forecast slip, or accept a forecast practical completion of 21 May 2027 | Project director | 30 Jun 2026 | Recovery options requiring long-lead procurement close at the end of June | §3.4 |

### 4.2 Section 2 — executive summary

> **Position in one sentence.** The project is 47.5 per cent complete, running about five per cent over
> budget on the work performed and forecasting practical completion 21 days later than the contractual
> date, and it does not have enough contingency to cover both the pending change and the forecast overrun.
>
> **Status.** Schedule: **Behind**. Cost: **Behind**. Risk: **Deteriorating**. Change: **Elevated**.
> Cash: **On plan**.
>
> **Headline figures.** Budget at completion CU 8,000 thousand (distributed), plus CU 300 thousand
> contingency, total authorised CU 8,300 thousand. Per cent complete 47.5 per cent against 34.0 per cent
> last period. Cost performance index 0.955, from 0.968 last period. Schedule performance index 0.950,
> from 0.961. Selected estimate at completion CU 8,430 thousand by the bottom-up method, within a range
> across methods of CU 8,180 to CU 8,610 thousand. Variance at completion CU (430) thousand, being
> 5.4 per cent of budget at completion. Forecast practical completion 21 May 2027, 21 days after the
> contractual date of 30 April 2027.
>
> **What changed this period.** (1) The structural steel primary frame was signed off, releasing 40 per
> cent credit on the largest civil activity and lifting the control account from 35.1 to 55.0 per cent
> complete. (2) Buried obstructions were confirmed at 22 pile positions, realising risk R-014 and
> generating change request BCR-014 at CU 295 thousand. (3) The forecast moved from CU 8,180 to CU 8,430
> thousand as the piling productivity shortfall was accepted as structural rather than transient — a
> change of forecast method from Method 2 to Method 4, disclosed at §3.7.
>
> **What to worry about.** Contingency. Remaining contingency is CU 300 thousand. BCR-014 requests
> CU 295 thousand of it. The forecast variance at completion is CU (430) thousand, and the expected value
> of the open risk register is CU 275 thousand. Those three figures cannot all be funded from what is
> left, and the decision about which of them is funded is better taken in June than in November.
>
> **Decisions required.** D-01 by 19 June 2026 · D-02 by 30 June 2026 · D-03 by 30 June 2026.
>
> **Basis.** All figures in generic currency units, thousands, nominal, single currency, no escalation.
> Data date 31 May 2026. Amounts to the nearest thousand; indices to three decimal places; percentages to
> one decimal place.

That page is the whole report for most of its readers, and everything in it is either a figure from a
sheet elsewhere in this series or a judgement with its ground stated in the same sentence.

### 4.3 Section 11 — cash position

| Field | Value | Derivation |
|---|---|---|
| Gross value certified — cumulative | 3,600 | Input |
| Gross value certified — this period | 620 | Input |
| Retention held (5 per cent of gross certified) | 180 | 3,600 × 0.05 |
| Net certified for payment | 3,420 | 3,600 − 180 |
| Cash received — cumulative | 3,150 | Input |
| Receivables outstanding | 270 | 3,420 − 3,150 |
| Actual cost incurred (from `TPL-07`) | 3,980 | Input |
| Uncertified work in progress | 380 | 3,980 − 3,600 |
| Project cash invested | 830 | 3,980 − 3,150 |
| Days sales outstanding (May base, 31 days) | 13.5 days | 270 ÷ (620 ÷ 31) |

**Verification of the identity.** Project cash invested should equal uncertified work in progress plus
retention held plus receivables outstanding: 380 + 180 + 270 = 830, which matches 3,980 − 3,150 = 830. The
identity holding is the check that the certification and receipt data are consistent with the cost ledger;
when it fails, one of the three inputs is stated to a different cut-off.

**Verification of days sales outstanding.** Average daily certified value in May = 620 ÷ 31 = 20.0.
Receivables 270 ÷ 20.0 = 13.5 days. Reported on a single-month base, which is stated because a rolling
three-month base would give a different figure from the same receivables.

The retention percentage is a term of the illustrative contract, not a norm, and whether certified value
corresponds to recognised revenue depends on the applicable financial reporting framework and the entity's
accounting policy.

## 5. Common mistakes

**Opening with status.** The reader who has twenty minutes reads what is in front of them. If page one is
a status summary, the decisions on page eleven do not get taken, and the report has failed while looking
complete.

**An executive summary that is not true alone.** A summary that says "cost performance index 0.955" with
the qualification that two control accounts are excluded, appearing on page nine, is a defect. It will be
forwarded, and the qualification will not.

**A variance narrative that restates the number.** "Unfavourable variance due to higher than expected
costs" is the number in words. The reader needs the physical cause and its size.

**No owner, or a function as owner.** "Procurement" cannot be asked how it is going. A person can.

**No status on last period's action.** Without it, the narrative section becomes a monthly essay that
nobody is accountable for. One row fixes it.

**Position without movement.** Every headline figure needs last period's beside it. A project that is
behind and recovering and one that is behind and deteriorating look identical in a single-period report,
and they require opposite decisions.

**A single forecast figure with no method and no range.** It reads as fact, and the assumption behind it
disappears. See `TPL-08`.

**Status carried by colour alone.** It fails in monochrome, on a projector, and for a proportion of every
audience. Pair the colour with a word, always.

**Expected value read as a contingency requirement.** A register expected value of CU 275 thousand does not
mean CU 275 thousand of contingency is enough. It is a sum of point estimates and carries no confidence
level.

**A report that never says what it does not know.** The data quality section costs half a page and is what
distinguishes a report that can be relied on from one that merely looks complete.

**Late enough that it describes history.** A report six weeks after its data date describes a project that
no longer exists. If the offset cannot be met, shorten the report rather than delay it.

## 6. Adapting it

**Safe to change.** Section order below §3.2 — health and safety, progress, cost and cash can be
sequenced to suit the audience. Adding sections for procurement status, quality metrics, workforce and
resourcing, sustainability reporting, or community and stakeholder matters. Adding client-specific
appendices. Reporting at package level as well as project level on a large programme.

**Change with care.** Merging the decisions section into the executive summary. It saves a page and
usually loses the required-by dates, which are the part that makes the section work. If you merge them,
keep the dates in a table, not in prose.

**Do not remove.** Section 1 and its dates; the movement figures beside every headline; the cause, action,
owner and expected effect in the variance narrative; the status of last period's action; and the data
quality statement. Those five are what make the report an instrument rather than a record.

**On a small project**, this can be four pages: decisions, executive summary, a combined progress and cost
section with the variance narrative, and cash with change and risk. Do not drop the decisions page. It is
the reason the report exists.

**Where the client mandates a format**, produce theirs and keep this one internally for the month it takes
to notice which fields the mandated format omits. Usually it is the movement figures and the status of
last period's actions.

## 7. Completion checklist

- [ ] Decisions section is first, with an owner, a required-by date and a stated consequence for each
- [ ] Executive summary reads true with nothing else attached
- [ ] Every headline figure carries last period's figure beside it
- [ ] Units, currency, scale, basis, data date and rounding stated on the summary page
- [ ] Every control account over the reporting threshold has a variance block with all five fields
- [ ] Status of last period's actions recorded, including any abandoned
- [ ] Forecast reported with its method, its range and the credibility test
- [ ] Contingency remaining reported alongside pending change value and forecast variance at completion
- [ ] Risk expected value reported with the caveat that it is not a contingency requirement
- [ ] Approved-but-not-implemented changes listed, or confirmed as none
- [ ] Cash identity checks: cash invested equals uncertified work plus retention plus receivables
- [ ] Days sales outstanding states its base period
- [ ] No status carried by colour alone
- [ ] Data quality statement completed, including any AI assistance and who validated it
- [ ] Issued within the working-day offset committed in the controls execution plan

---

## Related

- `TPL-01 — Project controls execution plan` — where cadence, distribution and thresholds are fixed
- `TPL-04 — Baseline change request` — the source of the change position in §3.10
- `TPL-05 — Progress measurement and rules of credit sheet` — the source of the progress position
- `TPL-07 — Earned value calculation sheet` — the source of the cost position and the indices
- `TPL-08 — Estimate at completion scenario comparison` — the source of the forecast and its range
- `TPL-09 — Cash flow forecast` — the forward cash position referenced in §3.11
- `TPL-10 — Risk register` — the source of the risk movement in §3.9
- `BPG-14 — Monthly reporting that gets read` — why the order of sections is the whole design
- `BPG-15 — Dashboards and data visualisation for controls` — how a live dashboard and this report divide the work

## Sources and standards

- PCI Master Formula Sheet (`docs/downloads/master-formula-sheet.md`), August 2026: the expected monetary
  value and days sales outstanding formulas used in §3.9 and §3.11. Published under the credential's
  retired code; the credential is PCL-AI.

This is an original instrument. The retention percentage, certification figures and dates in §4 are
illustrative and do not describe any real project. Revenue recognition and accounting treatment vary by
financial reporting framework and by jurisdiction, and no treatment is presented here as universal.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
