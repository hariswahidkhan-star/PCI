---
id: TPL-12
series: S10
series_name: Free Templates
title: Change order log
subtitle: Every change, its status, its dates — and whether it is in the forecast
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 17
summary: >
  A change order log that tracks each change from first identification to agreement, records the date of
  every status transition, keeps the submitted, assessed and best-estimate values apart, and carries the
  one column most logs omit: whether the change is in the cost forecast, and for how much. The difference
  between the best estimate and the amount in the forecast is unrecognised exposure, and this log adds it
  up.
linkedin:
  format: document
  hook: >
    A change log tells you what is being managed. It rarely tells you what is in the forecast. Add one
    column — "amount included in the forecast" — subtract it from the best estimate, and you have the
    number your cost report is currently missing.
  tags: [ProjectControls, ChangeControl, CostEngineering, CommercialManagement, Variations]
  asset: one-pager
gated: false
related: [BPG-11, BPG-04, TPL-04, TPL-13, BPG-09]
bok_domains: [5, 7]
sources: []
placeholders: 0
---

# Change order log

> Every change, its status, its dates — and whether it is in the forecast.

**In one paragraph.** A change order log that tracks each change from first identification to agreement,
records the date of every status transition, keeps the submitted, assessed and best-estimate values apart,
and carries the one column most logs omit: whether the change is in the cost forecast, and for how much.
The difference between the best estimate and the amount in the forecast is unrecognised exposure, and this
log adds it up.

**Who this is for.** Quantity surveyors, commercial managers, cost engineers and project controls managers
who run the change process; and the project managers and finance partners who rely on a forecast that only
holds if this log is complete.

---

## 1. When to use this

Open it on day one, before the first change arrives, because the fields you cannot reconstruct are the early
ones — the date something was first identified, and whether a notice was served in time.

It does three jobs that nothing else does.

**It protects entitlement.** Most contracts make some relief conditional on notice, and some make it a
strict condition. The notice block in columns I to N is the part of this log that has to be right, and it is
the part most often filled in retrospectively, which is to say wrongly.

**It keeps the forecast honest.** A change with a real cost that is not in the forecast is a loss that has
already happened and has not yet been reported. Columns AE to AH exist to make that visible before the month
closes rather than at the final account.

**It is the audit trail.** When a change is disputed a year later, the argument is almost always about
sequence: what was known, when, and what was said about it. A log with a date against every status
transition answers that in one row.

It is not a substitute for the baseline change control process. A change to scope may or may not change the
baseline, and the two are separate decisions with separate approvals — `TPL-04 — Baseline change request` is
where that lives. This log tracks the commercial event; the baseline change request tracks what happens to
the measurement of performance.

## 2. How to complete it

### 2.1 Register it the day it appears, not the day it is agreed

The trigger for a row is the moment somebody becomes aware that the work may change — not the instruction,
not the quotation, not the agreement. Registering late is how notice periods are missed and how the log ends
up describing an outcome instead of a process. If it turns out not to be a change, close the row as
Withdrawn with a reason; a log with withdrawn rows in it is a log somebody is actually using.

### 2.2 Get the notice block right first

Column I asks whether notice is required, column J records the date of the trigger, and column K records the
notice period. Two points make the difference between a useful record and a misleading one.

**Know what starts the clock in your contract.** Some contracts run the period from the occurrence, some
from the date the party became aware, and some from the date the party ought reasonably to have become
aware. These are different dates and they lead to different answers. Record in column J the date that starts
the clock under *your* contract, and note in column AS which test you applied.

**Record the position honestly, including when it is bad.** Column N calculates whether the notice was
served inside the period. If it was not, the log should say so. A commercial team that discovers a late
notice in month four has options; one that discovers it in the final account has an argument it will
probably lose. Whether a late notice defeats entitlement, and what relief remains if it does, depends on the
contract and the law governing it — this log records the fact, not the consequence.

### 2.3 Keep three cost figures apart

Column W is what was submitted. Column X is what the other party has assessed. Column Y is the current best
estimate — the number the commercial team genuinely expects to settle at, which is usually neither of the
other two.

Merging them destroys the log. If you record only the submitted value, the forecast is optimistic and the
gap in column Z is invisible. If you record only the assessed value, you have adopted the other side's
position in your own forecast. If you record only the best estimate, you cannot show anyone how far apart
the parties are, which is the information a negotiation strategy is built from.

The same logic applies to time. Column AA is the days claimed, column AB the days agreed, and column AJ the
days actually reflected in the current schedule — three different numbers that are routinely assumed to be
one.

### 2.4 Answer the forecast question every month

Column AE takes Yes, No or Partial. Column AF takes the amount actually included. Column AG records the
basis — agreed value, assessed value, best estimate, risk-adjusted, or nil.

This is the column that saves people, and it saves them in a specific way. The failure it prevents is not
dishonesty; it is a handover gap. The commercial team knows a change is worth 240,000 and is confident of
recovering it. The cost engineer does not include it in the forecast because it is not agreed. Neither is
wrong, and both are behaving reasonably. The result is a forecast that is 240,000 light and a commercial
position that nobody has priced. The gap lives in the space between two competent people, and a single
column closes it.

State the convention the project uses for what goes into the forecast, and apply it to every row. Any
convention is defensible — include the assessed value, include the best estimate, include a risk-adjusted
value — provided it is written down, applied consistently, and the unrecognised exposure in column AH is
reported alongside the forecast rather than instead of it.

### 2.5 Use a report date, not today

Every date-difference formula below uses a named cell called `Report_Date` rather than `TODAY()`. This
matters more than it sounds: a log built on `TODAY()` produces different numbers every time it is opened, so
the log attached to March's report no longer agrees with March's report by April. Set `Report_Date` to the
reporting cut-off, and the log is reproducible.

## 3. The template

Header row in row 1, data from row 2. Formulas are written for row 2 and fill down. `Report_Date` and
`Original_Contract_Value` are named cells on a `Parameters` sheet.

### 3.1 Input columns

| Col | Field | What goes in it |
|---|---|---|
| A | Change ID | Permanent, never reused, e.g. `CHG-018` |
| B | Date first identified | When somebody first became aware the work might change |
| C | Origin | Client instruction · Design development · Site condition · Statutory or consent · Supply chain · Error or omission · Opportunity · Scope clarification |
| D | Originator | Named individual and organisation |
| E | Description | One sentence saying what changes. Not why, not who is to blame |
| F | Affected WBS or control account | Where the cost lands |
| G | Affected activity IDs | Where the time lands, in schedule identifiers |
| H | Contract mechanism relied on | The mechanism in general terms — variation, compensation event, change order, claim. Record the clause reference from your executed contract, not from memory of a standard form |
| I | Notice required? | Yes · No · Unknown |
| J | Notice trigger date | The date that starts the clock under your contract |
| K | Notice period (days) | From the contract. Note in column AS whether these are calendar or working days |
| M | Notice served date | |
| O | Status | Notified · Quoted · Instructed · Agreed · Disputed · Withdrawn · Rejected |
| P–T | Date notified · quoted · instructed · agreed · disputed | One date per transition, left blank until it happens. Never overwrite an earlier one |
| W | Cost — submitted | The value submitted |
| X | Cost — assessed | The value assessed by the other party |
| Y | Cost — current best estimate | What the commercial team expects to settle at |
| AA | Schedule — days claimed | |
| AB | Schedule — days agreed | |
| AD | Commercial position | Agreed · In negotiation · Gap noted · Reserved · Referred |
| AE | In the cost forecast? | Yes · No · Partial |
| AF | Amount included in the forecast | Currency |
| AG | Basis of inclusion | Agreed value · Assessed value · Best estimate · Risk-adjusted · Nil |
| AI | In the schedule? | Yes · No · Partial |
| AJ | Days reflected in the current schedule | |
| AL | Approval reference | Instruction number, minute reference, signed order reference |
| AM | Approved by | Named individual with the authority to approve |
| AN | Payment application first included in | |
| AO | Linked risk ID | Where the change is a realised risk, the ID from `TPL-10` |
| AP | Owner | The named person progressing this change |
| AQ | Next action | |
| AR | Next action date | |
| AS | Notes | Including which notice test applied, whether the period is calendar or working days, and any reservation |

### 3.2 Calculated columns

| Col | Field | Formula in words | Spreadsheet expression |
|---|---|---|---|
| L | Notice due date | The trigger date plus the notice period, where notice is required | `=IF(OR($I2<>"Yes",$J2="",$K2=""),"",$J2+$K2)` |
| N | Notice position | Not required where no notice applies; otherwise whether it was served, served in time, or the period has expired unserved | `=IF($I2<>"Yes","Not required",IF($M2="",IF(AND($L2<>"",Report_Date>$L2),"Not served — period expired","Not served"),IF($M2<=$L2,"Served within the period","Served late")))` |
| U | Date of latest status change | The most recent of the five transition dates; blank if none has occurred | `=IF(MAX($P2:$T2)=0,"",MAX($P2:$T2))` |
| V | Days in current status | Days from the latest transition to the report date | `=IF($U2="","",Report_Date-$U2)` |
| Z | Cost gap | Submitted less assessed; blank until both exist | `=IF(COUNT($W2:$X2)<2,"",$W2-$X2)` |
| AC | Schedule gap | Days claimed less days agreed; blank until both exist | `=IF(COUNT($AA2:$AB2)<2,"",$AA2-$AB2)` |
| AH | Unrecognised cost exposure | Best estimate less the amount included in the forecast, treating a blank inclusion as zero | `=IF($Y2="","",$Y2-N($AF2))` |
| AK | Unrecognised time exposure | Days claimed less days reflected in the current schedule, treating a blank as zero | `=IF($AA2="","",$AA2-N($AJ2))` |

Summary cells, to be placed above the log and read at every monthly review:

| Field | Formula in words | Spreadsheet expression |
|---|---|---|
| Total current best estimate | Sum of the best-estimate column | `=SUM($Y:$Y)` |
| Total in the forecast | Sum of the amounts included | `=SUM($AF:$AF)` |
| Total unrecognised cost exposure | Sum of the exposure column | `=SUM($AH:$AH)` |
| Share of change value not in the forecast | Unrecognised exposure divided by total best estimate; blank if there is no change value | `=IF(SUM($Y:$Y)=0,"",SUM($AH:$AH)/SUM($Y:$Y))` |
| Change value as a share of the original contract | Total best estimate divided by the original contract value; blank if that value is unset | `=IF(N(Original_Contract_Value)=0,"",SUM($Y:$Y)/Original_Contract_Value)` |
| Notices at risk | Count of rows where the notice period has expired without service | `=COUNTIF($N:$N,"Not served — period expired")` |
| Total unrecognised time exposure | Sum of the time-exposure column | `=SUM($AK:$AK)` |

### 3.3 Pasting it into a spreadsheet

Copy the header line into cell A1 and split on the pipe character. Format the date columns, set `Report_Date`
on the `Parameters` sheet, then apply the §3.2 formulas to row 2 and fill down.

```
Change ID|Date first identified|Origin|Originator|Description|Affected WBS|Affected activity IDs|Contract mechanism|Notice required?|Notice trigger date|Notice period (days)|Notice due date|Notice served date|Notice position|Status|Date notified|Date quoted|Date instructed|Date agreed|Date disputed|Latest status change|Days in current status|Cost submitted|Cost assessed|Cost best estimate|Cost gap|Days claimed|Days agreed|Schedule gap|Commercial position|In the cost forecast?|Amount in forecast|Basis of inclusion|Unrecognised cost exposure|In the schedule?|Days reflected in schedule|Unrecognised time exposure|Approval reference|Approved by|Application first included|Linked risk ID|Owner|Next action|Next action date|Notes
```

In Markdown, split it into three blocks sharing the change ID: identification and notice (A to N), status and
value (O to AD), and forecast and approval (AE to AS).

## 4. Worked fragment

*Illustrative figures.* Currency-neutral units. Report date 30 April 2026. Notice periods in calendar days.
Original contract value 24,000,000.

**Identification and notice**

| Change ID | Origin | Description | Notice required? | Trigger date | Period | Due | Served | Notice position |
|---|---|---|---|---|---|---|---|---|
| CHG-011 | Design development | Revised louvre specification to the plant room façade | No | — | — | — | — | Not required |
| CHG-018 | Client instruction | Additional fire dampers to the level 3 riser | No | — | — | — | — | Not required |
| CHG-023 | Site condition | Buried services encountered on the southern access route | Yes | 4 Mar 26 | 14 | 18 Mar 26 | 16 Mar 26 | Served within the period |

**Status and value**

| Change ID | Status | Latest transition | Days in status | Submitted | Assessed | Best estimate | Cost gap | Days claimed | Days agreed | Schedule gap |
|---|---|---|---|---|---|---|---|---|---|---|
| CHG-011 | Agreed | 13 Feb 26 | 76 | 62,000 | 62,000 | 62,000 | 0 | 0 | 0 | 0 |
| CHG-018 | Instructed | 6 Mar 26 | 55 | 148,000 | 96,000 | 120,000 | 52,000 | 5 | 0 | 5 |
| CHG-023 | Notified | 16 Mar 26 | 45 | — | — | 240,000 | — | 12 | — | — |

**Forecast position**

| Change ID | In the forecast? | Amount in forecast | Basis | Unrecognised cost exposure | In the schedule? | Days in schedule | Unrecognised time exposure |
|---|---|---|---|---|---|---|---|
| CHG-011 | Yes | 62,000 | Agreed value | 0 | Yes | 0 | 0 |
| CHG-018 | Partial | 96,000 | Assessed value | 24,000 | No | 0 | 5 |
| CHG-023 | No | 0 | Nil | 240,000 | No | 0 | 12 |

**The substitutions.**

CHG-023 notice due date: `4 March 2026 + 14 days = 18 March 2026`. Served 16 March, which is on or before
18 March, so the position is "Served within the period".

CHG-018 days in current status: latest transition 6 March 2026, report date 30 April 2026, so
`30 April − 6 March = 55 days`. CHG-011: `30 April − 13 February = 76 days`. CHG-023:
`30 April − 16 March = 45 days`.

CHG-018 unrecognised cost exposure: `120,000 − 96,000 = 24,000`. CHG-023: `240,000 − 0 = 240,000`.
CHG-011: `62,000 − 62,000 = 0`.

Totals: current best estimate `120,000 + 240,000 + 62,000 = 422,000`. Amount in the forecast
`96,000 + 0 + 62,000 = 158,000`. Unrecognised cost exposure `24,000 + 240,000 + 0 = 264,000`, and as a
check, `158,000 + 264,000 = 422,000`. Share of change value not in the forecast:
`264,000 ÷ 422,000 = 0.626`, or 62.6 %. Total unrecognised time exposure: `0 + 5 + 12 = 17` working days.

**What the fragment is telling you.** Every one of these three changes is being competently managed. Each
has an owner, a status and a next action, and anyone reviewing the log would say the change process is
working. And the cost forecast is understating this project by 264,000 — nearly two-thirds of the change
value on the log — while the schedule carries none of the 17 days claimed. Neither of those two facts
appears anywhere in a change log without columns AE to AK. That is the entire argument for the template.

The second reading is about CHG-018: it has been Instructed for 55 days with a 52,000 gap between what was
submitted and what was assessed, and no date in the Agreed column. A change that has been instructed but not
agreed is work being executed at a price nobody has settled, and 55 days is long enough for the record of
what was actually done to start degrading. That row is the one for the commercial meeting.

## 5. Common mistakes

**Registering changes when they are instructed.** By then the notice period may have run and the early
factual record — what was known and when — is already being reconstructed from memory.

**One cost column.** The submitted, assessed and best-estimate figures are three different pieces of
information and they are needed for three different purposes. A log with one column has thrown two of them
away.

**Overwriting status dates.** Columns P to T are a history, not a status field. Overwriting the notified date
with the instructed date destroys the only record of how long each stage took, which is exactly what is
argued about later.

**Blank forecast columns.** The most dangerous state of column AE is empty, because empty reads as "not yet
considered" and behaves as "no". Every row should carry Yes, No or Partial by the monthly cut-off, even if
the answer is No with a reason.

**Adopting the other side's assessment as your forecast.** Putting the assessed value into the forecast and
leaving column Y unpopulated makes the gap disappear from your own reporting. Keep the best estimate, report
the exposure, and let the negotiation be about a number that is visible.

**A log that never closes rows.** Agreed, Withdrawn and Rejected are terminal statuses with dates. A log
where nothing ever leaves is one nobody trusts, and the summary totals become meaningless because settled
changes are still counted as exposure.

**Time impacts tracked only in days claimed.** Column AJ — days actually reflected in the schedule — is the
time equivalent of the forecast column, and it is omitted even more often. A schedule that does not carry
the agreed extensions will show a completion date the project cannot achieve and cannot explain.

**Using `TODAY()`.** Covered in §2.5. A saved report should say the same thing next month as it said this
month.

## 6. Adapting it

**Safe to change.** The origin taxonomy, the status list, the currency, and the addition of columns your
contract or governance requires — a package reference, a client reference number, a sub-contract flow-down
identifier, a retention or bond consequence. If your contract has more status stages than the five here, add
a date column for each rather than reusing one.

**Safe to add.** A margin block, showing the value, the cost and the margin on each change, which is how a
contractor finds out that the changes it fought hardest for were the least profitable. A cumulative chart of
best estimate against amount in the forecast over time, which shows whether the gap is being closed or
grown. A column for the party bearing the risk, where a change flows down to a subcontractor.

**Do not change.** The separation of submitted, assessed and best estimate. The dated status transitions.
The notice block. And columns AE to AH — if you take one thing from this template into a log you already
have, take those.

### 6.1 Before the monthly cut-off

- Every change identified this period has a row, including the ones that may turn out not to be changes.
- Every row where notice is required has a trigger date, a period, and a served date or an explanation.
- No row shows "Not served — period expired" without an entry in the notes and an escalation.
- Every row has a current best estimate, including rows that have not been quoted.
- Every row carries Yes, No or Partial in the forecast column, with a basis.
- The unrecognised cost exposure total has been given to whoever owns the cost forecast, in writing.
- The unrecognised time exposure total has been given to whoever owns the schedule, in writing.
- Any row instructed more than one reporting period ago and still not agreed is on the commercial agenda.
- Terminal rows have been closed with a date, and the summary totals exclude them or are reported both ways.
- `Report_Date` is set to this period's cut-off before the log is issued.

---

## Related

- `BPG-11 — Change orders and variations` — the method: identification, valuation, negotiation and the
  behaviours that make a change process work
- `BPG-04 — Baselining and baseline change control` — why a commercial change and a baseline change are
  separate decisions with separate approvals
- `TPL-04 — Baseline change request` — the instrument that moves the baseline, once a change is agreed
- `TPL-13 — Claim and extension-of-time narrative structure` — where a disputed change goes when the
  commercial position does not resolve
- `BPG-09 — Estimate at completion — choosing and defending a method` — how the unrecognised exposure in
  column AH reaches the forecast

## Sources and standards

This is an original instrument developed by the Institute. It reproduces no third-party template, form or
contract wording. The status sequence, the notice mechanics and the valuation stages described here are
general commercial practice expressed in the Institute's own words. No standard form of contract is quoted
and no clause number is cited: the mechanism, the notice test and the consequence of late notice are
determined by the contract in front of you and by the law governing it, and nothing in this template should
be read as advice on either.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
