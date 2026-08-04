---
id: BPG-14
series: S09
series_name: Best Practice Guides
title: Monthly reporting that gets read
subtitle: Lead with the decision, not the data
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, executive]
level: practitioner
reading_time_min: 14
summary: >
  A monthly report exists to cause a decision, and most of them do not. This guide sets out the one-page
  structure that leads with the decisions required rather than the data, the four-part variance narrative
  that names cause, effect, action and owner, how to report a control account whose indices are undefined,
  the arithmetic that makes a recommendation defensible, why reporting bad news early is a control
  discipline rather than a courtesy, and why a report nobody reads is a control failure rather than a
  communications one.
linkedin:
  format: carousel
  hook: >
    If the first page of your monthly report is a table of indices, the reader has to work out what you
    want. Lead with the decision required, its cost, and the date the option expires.
  tags: [ProjectControls, ProjectReporting, EarnedValue, ProjectManagement]
  asset: carousel-8
gated: false
related: [BPG-08, BPG-09, BPG-15, BPG-19, TPL-06]
bok_domains: [4, 6]
sources: []
placeholders: 0
---

# Monthly reporting that gets read

> Lead with the decision, not the data.

**In one paragraph.** A monthly report exists to cause a decision, and most of them do not. This guide sets
out the one-page structure that leads with the decisions required rather than the data, the four-part
variance narrative that names cause, effect, action and owner, how to report a control account whose indices
are undefined, the arithmetic that makes a recommendation defensible, why reporting bad news early is a
control discipline rather than a courtesy, and why a report nobody reads is a control failure rather than a
communications one.

**Who this is for.** Project controls managers and cost engineers who write the monthly report, and the
project managers and sponsors who have to act on one.

---

## 1. A report is an instrument, not a record

The test of a monthly report is not whether it is accurate, complete or on time. Those are entry
requirements. The test is whether it caused the right decision to be taken at the right moment.

That reframing changes what belongs on the page. A report designed as a record answers "what happened". A
report designed as an instrument answers four questions in order:

1. **What decisions do we need from you, and by when?**
2. **What is off track, why, and what is being done?**
3. **Where will this project finish, on what assumption?**
4. **What has changed since last time?**

Everything that does not serve one of those is supporting material. It is not deleted — it goes in the pack
behind the page, where the reader who wants it can find it and the reader who does not is not slowed down by
it.

## 2. The one page

The one-page report is not a summary of the pack. It is the report; the pack is the evidence. In order:

**Decisions required.** Two or three at most, each with the decision, the recommendation, the value, the
consequence of not deciding, and the date the option expires. This block goes first, before any data,
because it is what the meeting is for. A month with no decisions required says so in one line — that is
information too.

**Position.** The current cost and schedule status in the smallest number of figures that support the rest
of the page. Typically budget at completion, actual cost, earned value, the two indices, milestone status in
days against plan, and the funding position. Trend arrows against last period, not just values.

**Forecast.** The estimate at completion, its method, its assumption, its movement since last period and why
it moved. `BPG-09 — Estimate at completion: choosing and defending a method` sets out what makes that
defensible.

**Exceptions.** Only the items outside tolerance, each with the four-part narrative of §3. Accounts inside
tolerance are listed as such and not elaborated.

**Actions.** Open actions with owners and dates, and the ones that closed this period. An action list where
the dates never move is not being managed; an action list where they always move is not being enforced.

**Risk and change.** Remaining contingency against remaining exposure (see `BPG-10 — Contingency and
management reserve`), and the change position including instructed-but-unpriced work.

Tolerances decide what appears as an exception, and they are set in advance at control account level. Set
after the fact, they are not tolerances; they are explanations.

## 3. The variance narrative

The narrative is where most reports fail, and the failure is consistent: they describe the number again, in
words. "CA-30 M&E is showing an adverse cost variance driven by ongoing productivity challenges. The team is
working to recover the position and will update next month." Nothing there is false and nothing there is
usable.

A usable narrative has four parts and roughly four sentences.

**Cause — named, specific, evidenced.** Not "productivity challenges" but what happened, to which resource,
since when, and how it is known. The evidence source belongs in the sentence: site returns, timesheets,
delivery records, the certifier's assessment.

**Effect — quantified, in the units the decision needs.** Money for a cost variance, days for a schedule
variance, and the rate at which exposure accrues if the situation continues. A variance in currency is not
an effect; it is a measurement. The effect is what it does to the finish, the forecast or the funding.

**Action — specific, resourced, with a date.** What will be done, what it costs, when it starts, and what it
is expected to achieve. "Monitoring" is not an action. "Recovery plan being developed" is an action only if
it has a date and an author.

**Owner — a person.** Not a company, not a team, not "the project". One name.

Two supporting habits. Report **trend before status**, because direction is where the warning lives — an
index of 0.94 that was 0.98 two months ago is a different report from an index of 0.94 that was 0.90.
And **say what you do not know**: an honest "the cause of the M&E variance is not yet established;
investigation reports on the 12th" is far stronger than a confident attribution that turns out to be wrong,
because the second one costs you every subsequent narrative.

## 4. Reporting what the arithmetic cannot say

Two situations recur and both are handled badly by default.

**Undefined indices.** A control account with no planned value and no earned value cannot produce a
performance index — the division is undefined. Reporting a blank, a zero or a spurious 1.00 are all worse
than reporting the fact. Where an account has cost booked against it but no value earned, that is itself the
finding, and it belongs in the narrative rather than in the index column.

**Rollups that conceal.** A project-level index is a spend-weighted average and will comfortably hide one
account in trouble behind three that are not. The convention that fixes it costs one line: report the
project index *and* the two or three accounts driving it, every month, whether or not they breach tolerance.
A reader who only ever sees the aggregate is not being informed; they are being averaged at.

## 5. Bad news early is a control, not a courtesy

The argument for early disclosure is usually made in ethical terms. It is stronger in arithmetic terms:
**the value of a decision decays, and for most project problems it decays at a knowable rate.**

Once a slip is forecast, its cost accrues at the sum of the daily rates it triggers — liquidated damages,
extended preliminaries, standing plant, retained supervision. That is a number, and it can be put on the
page. Against it sits the cost of the intervention that would prevent it, which is generally a step cost —
mobilising crews, changing shift patterns, adding supervision — and which usually only works if taken while
enough work remains for it to bite.

Two consequences follow.

**Deferring the decision does not reduce the exposure; it removes the options.** The intervention that
works with three months of remaining work does not work with three weeks, and the exposure is unchanged.

**A report that reveals a problem the month after the option expired has failed**, however accurate it is.
This is why "no surprises" is a control requirement rather than a cultural aspiration, and why a controls
function that softens a message to survive a meeting has stopped doing its job. The Institute's position on
this is deliberate: the professional's obligation is to the accuracy of the number and the timeliness of the
warning, and neither is negotiable against the comfort of the room.

The corollary is that a reporting culture which punishes early bad news guarantees late bad news. If red
accounts produce interrogation rather than decisions, the next red account will be amber for two months
first. Sponsors set this, not report writers — but report writers should say so when it happens.

## 6. Cadence, audience and the pack

The same underlying data serves different rhythms, and the aggregation should be automatic rather than
re-keyed:

| Audience | Cadence | What they need |
|---|---|---|
| Delivery team | Weekly | Granular, action-focused, forward two to four weeks |
| Project board | Monthly | Exceptions, forecast, decisions required |
| Portfolio or executive | Monthly or quarterly | Cross-project comparison, aggregate funding, escalations only |

Automation deserves a clear boundary here. A tool can assemble the pack, detect the accounts breaching
tolerance, compute the indices and draft the exception narratives; it cannot decide whether a cause has been
correctly attributed, nor whether a recommendation is the right one, nor what to say when the data is
ambiguous. AI proposes; the professional disposes — and signs, because a report drives decisions and
sometimes external disclosure.

## 7. A report nobody reads is a control failure

If the report is not read, the control it was supposed to exert does not exist. That is not a communications
problem to be solved with better formatting; it is the same class of failure as a missed accrual or an
unmeasured variance, and it should be treated with the same seriousness.

The diagnostic questions are uncomfortable and worth asking annually:

- When did a decision last change because of something in the monthly report?
- Which sections has nobody asked a question about in six months?
- How long is the pack, and how long is the meeting?
- Does the report arrive with enough time before the meeting to be read, or is it tabled?
- Are the same three exceptions carried forward every month with the same action and a new date?

The last one is the most telling. A rolling exception is either not being acted on or not actually an
exception, and both answers require someone to do something other than re-issue the report.

## 8. How this goes wrong

**Leading with the data.** Page one is a table of indices and the reader has to reverse-engineer what is
wanted from them. Decisions go first.

**Narrative that restates the number.** "CPI is 0.86, which is below target." The reader can see that. Why,
what it does, what is being done, by whom.

**Cause without evidence.** Attribution by intuition, presented with the same confidence as measurement. If
the cause is not yet established, say so and give the date it will be.

**Actions without owners or dates.** An action list that is a list of intentions.

**Green because red is unwelcome.** The status that is set by the temperature of the last meeting rather
than by the tolerance. It works exactly once.

**The rolling amber.** An account that has been amber for five months with the same narrative. Amber is a
state of being about to become something; a permanent amber is a decision that has not been taken.

**Everything in the pack, nothing on the page.** Forty pages, no summary judgement. Volume is not rigour,
and a reader who cannot find the decision will make one without the report.

**Bad news timed to arrive with its solution.** Waiting a month to report a problem so that a recovery plan
can be presented alongside it is a defensible instinct and a serious error. The month is exactly what was
needed to act.

**Inconsistent basis between months.** Method changed, tolerance changed, scope of the account changed, and
none of it annotated. The trend line becomes meaningless and nobody says so.

**No one owns the report.** It is assembled from contributions and signed by nobody. The moment a figure is
challenged, it turns out that no single person could defend the page.

## 9. Worked example

*Illustrative figures.* Currency USD; a project of five control accounts at the Month 8 data date; earned
value measured under fixed earning rules; the ledger closed at the same date with accruals booked; indices to
three decimal places.

### 9.1 The position block

| Control account | BAC | PV | EV | AC | CPI | SPI |
|---|---:|---:|---:|---:|---:|---:|
| CA-10 Civils | 5,200,000 | 4,100,000 | 4,050,000 | 3,980,000 | 1.018 | 0.988 |
| CA-20 Structure | 7,400,000 | 4,600,000 | 4,300,000 | 4,720,000 | 0.911 | 0.935 |
| CA-30 M&E | 6,800,000 | 2,900,000 | 2,520,000 | 2,940,000 | **0.857** | **0.869** |
| CA-40 Fit-out | 3,600,000 | 900,000 | 880,000 | 860,000 | 1.023 | 0.978 |
| CA-50 Commissioning | 2,000,000 | 0 | 0 | 40,000 | n/a | n/a |
| **Project** | **25,000,000** | **12,500,000** | **11,750,000** | **12,540,000** | **0.937** | **0.940** |

```
Project CPI = EV ÷ AC = 11,750,000 ÷ 12,540,000 = 0.937
Project SPI = EV ÷ PV = 11,750,000 ÷ 12,500,000 = 0.940
EAC (persisting cost performance) = BAC ÷ CPI = BAC × AC ÷ EV
                                  = 25,000,000 × 12,540,000 ÷ 11,750,000 = 26,680,851
VAC = BAC − EAC = 25,000,000 − 26,680,851 = (1,680,851)
```

Two things the aggregate is hiding, and both belong on the page:

- CA-30 M&E is at 0.857, well below the project figure of 0.937, and it is the account that determines the
  commissioning start.
- CA-50 has 40,000 of actual cost against zero earned value, so no index can be computed. The report says
  exactly that, and treats the 40,000 as an open item rather than absorbing it into a rollup.

### 9.2 The narrative, before and after

**As usually written.** *"M&E is behind due to ongoing challenges with subcontractor resourcing. The team is
working hard to recover and will provide an update next month."*

**As it should be written.**

> **CA-30 M&E — red.** CPI 0.857, SPI 0.869; both down from 0.91 and 0.92 at Month 6.
>
> *Cause.* The containment subcontractor has averaged 11 of the 18 operatives committed since Week 22,
> verified from site access returns. Containment productivity is running at 0.81 h/m against a budget of
> 0.62 h/m — `0.81 ÷ 0.62 = 1.31`, or 31 % above budget hours per metre.
>
> *Effect.* Commissioning handover is forecast to slip six weeks (42 calendar days). Exposure accrues at
> 35,500 per calendar day: liquidated damages 24,000 and extended site preliminaries 11,500. Total exposure
> if the slip is accepted: `35,500 × 42 = 1,491,000`.
>
> *Action.* Mobilise four supplementary crews and one additional supervisor, cost 620,000, starting within
> ten working days; recovery of the full six weeks assumes the added crews achieve current measured
> productivity.
>
> *Owner.* M&E package manager. *Decision required at this meeting.*

### 9.3 The arithmetic that makes it a decision

| Option | Cost now | Exposure avoided | Net |
|---|---:|---:|---:|
| A — accelerate | 620,000 | 1,491,000 | `1,491,000 − 620,000 = 871,000` |
| B — accept the slip | 0 | 0 | `(1,491,000)` |

Option A is worth 871,000 **if the acceleration delivers the full six weeks**. State the sensitivity rather
than letting someone else find it:

```
If the supplementary crews achieve 80 % of current measured productivity:
  Recovery achieved  = 6 weeks × 0.80 = 4.8 weeks
  Residual slip      = 6 − 4.8 = 1.2 weeks = 8.4 calendar days
  Residual exposure  = 8.4 × 35,500 = 298,200
  Net benefit        = 1,491,000 − 620,000 − 298,200 = 572,800
```

The recommendation survives its own sensitivity: even at 80 % effectiveness, accelerating is worth 572,800
more than accepting the slip. That sentence — not the index table — is the report.

### 9.4 What the decisions block looks like

> **Decisions required this month**
>
> 1. **Approve 620,000 for M&E acceleration** (CA-30). Avoids exposure of 1,491,000 from a forecast six-week
>    slip to commissioning handover; net benefit 871,000, or 572,800 if the added crews achieve only 80 % of
>    current productivity. **The option expires when the commissioning front is handed over**; after that,
>    crews cannot be added to the critical work. *Recommendation: approve.*
> 2. **Note the 40,000 of cost on CA-50** with no earned value. Investigation reports next period; treat as
>    an open item, not as a cost variance, until the cause is established.

**Assumptions this example depends on.** Earning rules unchanged since baseline; ledger and progress share
the Month 8 data date; the six-week slip is the output of a schedule analysis, not an extrapolation of the
schedule performance index; the liquidated damages rate and preliminaries rate are as stated in the contract
for this example; the acceleration cost is a quotation, not an estimate; and no part of the exposure is
recoverable under an extension of time, which is a separate assessment — see `BPG-12 — Claims and extension
of time`.

## 10. Checklist

**Before you write**

- [ ] You can name the decisions this report needs to cause.
- [ ] Tolerances were set before the period.
- [ ] The data date is the same for progress, cost and schedule.
- [ ] Last month's actions have been checked for closure, not copied forward.

**The page**

- [ ] Decisions required appear first, with value, consequence and expiry date.
- [ ] The forecast states its method and its assumption, and explains its movement.
- [ ] Every exception has cause, effect, action and a named owner.
- [ ] Causes cite their evidence source.
- [ ] Effects are quantified in the units the decision needs — days, money, or a rate per day.
- [ ] Trend is shown alongside status.
- [ ] The project rollup is accompanied by the accounts driving it.
- [ ] Accounts with undefined indices are reported as such, never as zero or 1.00.
- [ ] Remaining contingency is shown against remaining exposure.
- [ ] Instructed-but-unpriced change is disclosed.

**Integrity**

- [ ] Any change of basis, tolerance or account scope is annotated on the trend.
- [ ] Nothing has been softened to suit the expected reception.
- [ ] Anything not yet known is stated as not yet known, with a date.
- [ ] One named person can defend every figure on the page.

**Afterwards**

- [ ] The report reached the reader with time to be read, not tabled at the meeting.
- [ ] Decisions taken are recorded against the items that requested them.
- [ ] Exceptions carried for more than two periods are escalated as a structural issue, not re-issued.

---

## Related

- `BPG-08 — Earned value in practice` — where the position block's numbers come from, and what they cannot
  say.
- `BPG-09 — Estimate at completion: choosing and defending a method` — the forecast block, and how to
  explain a movement.
- `BPG-15 — Dashboards and data visualisation for controls` — the visual layer over the same data, and the
  distortions to avoid in it.
- `BPG-19 — Project controls assurance and health checks` — how to tell whether the reporting system is
  actually controlling anything.
- `TPL-06 — Monthly project controls report` — the instrument implementing the structure in §2.

## Sources and standards

Management reporting principles — designing the report around the decision, management by exception,
tolerance-based escalation — are common to published project management and cost engineering frameworks,
including the PMBOK Guide and the AACE International Total Cost Management framework; they are described
here in our own words and no text or table is reproduced. Internal references are BoK Domain 4 (Performance
Management, Variance Analysis & Management Reporting) and BoK Domain 6 (EVM/EAC). All figures in §9 are
illustrative and were computed for this document.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
