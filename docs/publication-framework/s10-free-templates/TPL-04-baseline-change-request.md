---
id: TPL-04
series: S10
series_name: Free Templates
title: Baseline change request
subtitle: The form that records a decision rather than a receipt, and the register that tracks the position
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 13
summary: >
  A baseline change request is the document that moves scope, budget or dates from one approved state to
  another, and the record that has to survive an audit two years later. This template gives the form —
  originator through to implementation record, including the alternatives considered and the funding
  source that determines whether budget at completion moves at all — and the register that tracks every
  request, the running approved position and the remaining contingency.
linkedin:
  format: document
  hook: >
    A change request that records a cost impact and a schedule impact but not which alternatives were
    rejected is a receipt, not a decision record.
  tags: [ChangeControl, ProjectControls, CostEngineering, BaselineManagement]
  asset: one-pager
gated: false
related: [TPL-01, TPL-06, TPL-12, BPG-04, BPG-10, BPG-11]
bok_domains: [5, 8, 12]
sources: []
placeholders: 0
---

# Baseline change request

> The form that changes an approved baseline, and the register that says where the project stands.

**In one paragraph.** A baseline change request is the document that moves scope, budget or dates from one
approved state to another, and the record that has to survive an audit two years later. This template
gives the form — originator through to implementation record, including the alternatives considered and
the funding source that determines whether budget at completion moves at all — and the register that
tracks every request, the running approved position and the remaining contingency.

**Who this is for.** Project controls managers and cost engineers who administer change; control account
managers and package managers who raise it; and project directors and sponsors who approve it and will be
asked later why.

---

## 1. When to use this

Raise a request whenever something would make the approved baseline no longer describe the work. That
covers more than new scope:

- **Scope added, reduced or transferred** between elements or between parties.
- **A budget transfer** between control accounts, even where the total does not move. The total not moving
  is exactly why these go unrecorded, and why control account variances become uninterpretable.
- **A release from contingency** into a control account.
- **A correction to the baseline** — an estimating error, a quantity wrong in the take-off, a duplicated
  item.
- **A schedule change** that moves a contractual milestone or consumes float that another party has a
  claim on.
- **A change of measurement basis** — a different rule of credit, a different control-account level, a
  different forecasting method used for reporting. These change what the numbers mean without changing a
  single figure, and they are the least often recorded.

Do not raise one for a forecast movement. A forecast change is a report of what is expected; a baseline
change is a decision about what is authorised. Confusing the two produces a project whose baseline
follows its actuals, which is a project that cannot report a variance.

Do not raise one to make a variance disappear. The controls execution plan (`TPL-01` §3.5) should list
that explicitly as a prohibited reason for a re-baseline, so the prohibition can be cited rather than
argued.

## 2. How to complete it

**One change, one request.** Bundling three unrelated items into one request means the approver must
accept or reject all three. The bundle is usually assembled around the one item that would not survive on
its own.

**Complete the impact fields even when the impact is nil.** "Schedule impact: none — the affected
activities carry 40 days of total float, path not driving" is an assessment. A blank field is an omission,
and the difference matters when someone asks two years later whether the schedule effect was considered.

**Name the funding source before you compute the revised budget.** This is the field that determines
whether budget at completion moves, and it is where most registers go wrong:

- **Contingency release** transfers budget that is already inside the budget at completion from the
  contingency reserve into a control account. Distributed budget rises, contingency falls, budget at
  completion does not move.
- **Additional authorised funding** — a client variation, an approved supplementary allocation, a release
  from management reserve — adds budget that was not previously inside the budget at completion. Budget at
  completion rises.
- **Trade-off within the baseline** moves budget between control accounts. Nothing changes at project
  level, but two control-account variances change, which is precisely why it must be recorded.

This depends on the definition of budget at completion agreed in the controls execution plan
(`TPL-01` §3.2). Some organisations hold contingency inside it and some outside; both are defensible and
the register must state which, because otherwise the same set of approved changes produces two different
revised budgets.

**Write the alternatives honestly.** The alternatives field is the one that turns the form into a decision
record. Record what else was considered, what each would have cost in money and time, and why it was
rejected. "No alternative" is occasionally true and should then be justified in a sentence, because a
change with genuinely no alternative is unusual.

**State the estimate basis and its class.** A cost impact assembled from firm quotations and one built
from a rate applied to a guessed quantity are different objects and should not be approved on the same
form without the difference visible.

**Route by threshold and record the route.** The thresholds live in `TPL-01` §3.5. The request records
which threshold applied and who therefore had authority — not merely who signed.

**Close the loop.** A request is not complete when it is approved; it is complete when the baseline has
been changed in the tools, the change verified, and the request marked implemented with the resulting
baseline version recorded. Approved-but-not-implemented is the most dangerous state a change can sit in,
because everyone believes the budget has moved and none of the reports agree.

**Using the tables.** Copy a table block, paste into a single spreadsheet column, split on the pipe
character, and delete the alignment row.

## 3. The template

### 3.1 The form

| Field | Entry | Completion note |
|---|---|---|
| **Identification** | | |
| Request number | | Sequential, never reused |
| Project / contract | | |
| Date raised | | |
| Originator — name, role, organisation | | A person, not a function |
| **Classification** | | |
| Change title | | One line, specific enough to be recognised in the register |
| Change class | | `Scope addition` · `Scope reduction` · `Scope transfer` · `Budget transfer` · `Estimate correction` · `Schedule only` · `Measurement basis` |
| Trigger | | `Client instruction` · `Design development` · `Site condition` · `Risk realised` · `Error or omission` · `Regulatory or consent` · `Third-party` |
| Risk register reference | | Where the change is a realised risk, the risk it realises |
| **Description and justification** | | |
| Description of change | | What changes, in the baseline's own terms. Reference the affected element identifiers. |
| Why it cannot be absorbed | | The test the request must pass before impact is even assessed |
| Justification | | Why the change should be approved, on the project's objectives rather than on convenience |
| **Affected baseline elements** | | |
| WBS elements affected | | Identifiers from `TPL-02` |
| Control accounts affected | | |
| Cost breakdown structure codes affected | | Codes from `TPL-03`; note any new code required |
| Schedule activities affected | | Activity identifiers |
| **Cost impact** | | |
| Direct cost | | |
| Indirect cost | | |
| Total cost impact | | Calculated — see §3.2 |
| Currency and basis | | Currency, nominal or constant prices, and whether escalation is included |
| Estimate basis and class | | Quotations, measured rates, parametric, judgement — and the estimate class |
| Estimate prepared by | | |
| **Schedule impact** | | |
| Activities affected and duration change | | |
| Effect on total float of the affected path | | Float before, float after |
| Is the affected path driving? | | Yes / no, and which path is |
| Effect on contractual milestones | | Named milestones, dates before and after |
| Effect on forecast completion | | |
| **Other impacts** | | |
| Scope impact | | What the project will and will not now deliver |
| Quality, safety and environmental impact | | |
| Risk impact | | Risks created, closed or changed, with register references |
| Interface impact | | Other parties affected and whether they have been consulted |
| **Options** | | |
| Alternatives considered | | Each option, its cost and time effect, and why it was rejected |
| Consequence of doing nothing | | Required. If it is genuinely nothing, the change is not needed. |
| **Funding** | | |
| Funding source | | `Contingency release` · `Additional authorised funding` · `Trade-off within baseline` |
| Effect on budget at completion | | Calculated — see §3.2 |
| Effect on remaining contingency | | Calculated — see §3.2 |
| **Approval** | | |
| Threshold applied and approval route | | From `TPL-01` §3.5 |
| Reviewed by — name, role, date, comment | | |
| Decision | | `Approved` · `Approved with conditions` · `Rejected` · `Deferred` · `Withdrawn` |
| Conditions of approval | | |
| Approved value | | Where different from the requested value, and why |
| Decision authority — name, role, date | | |
| **Implementation record** | | |
| Baseline version before / after | | Tool-level version identifiers, not descriptions |
| Cost baseline updated — by / date | | |
| Schedule baseline updated — by / date | | |
| Registers updated — risk, change, code register | | |
| Verification — by / date | | A second person confirms the baseline now matches the approved change |
| Implemented status | | `Not started` · `In progress` · `Implemented` · `Verified` |

### 3.2 Calculated fields on the form

**Total cost impact.** In words: direct cost plus indirect cost. With direct in `B20` and indirect in
`B21`: `=N(B20)+N(B21)`. Wrapping in `N()` treats a blank as zero rather than propagating text.

**Effect on budget at completion.** In words: the budget at completion moves only where the funding source
is additional authorised funding; a contingency release or an internal trade-off leaves it unchanged. With
the funding source in `B30` and the approved value in `B31`:

```
=IF(B30="Additional authorised funding",N(B31),0)
```

**Effect on remaining contingency.** In words: contingency falls only where the change is funded by a
contingency release.

```
=IF(B30="Contingency release",-N(B31),0)
```

### 3.3 The register

One row per request. This is the sheet that answers "where do we stand".

| Col | Field | Type | Definition and entry rule |
|---|---|---|---|
| A | Request number | Text | Matches the form |
| B | Date raised | Date | |
| C | Title | Text | |
| D | Originator | Text | |
| E | Class | List | As on the form |
| F | Trigger | List | As on the form |
| G | Status | List | `Draft` · `Submitted` · `Under review` · `Approved` · `Rejected` · `Deferred` · `Withdrawn` · `Implemented` |
| H | Funding source | List | `Contingency release` · `Additional authorised funding` · `Trade-off within baseline` · blank until decided |
| I | Cost impact requested | Number | |
| J | Cost impact approved | Number | Blank unless status is `Approved` or `Implemented` |
| K | Schedule impact requested (days) | Number | |
| L | Schedule impact approved (days) | Number | |
| M | Decision date | Date | Blank while open |
| N | Decision authority | Text | |
| O | Days open | Calculated | |
| P | Cumulative approved cost | Calculated | |
| Q | Revised budget at completion | Calculated | |
| R | Contingency remaining | Calculated | |
| S | Baseline version implemented | Text | |
| T | Implementation verified — by / date | Text | |

Hold three control values in fixed cells above the table: original budget at completion in `$C$1`, opening
contingency in `$C$2`, and the reporting cut-off date in `$C$3`.

**Calculated column O — Days open.** In words: calendar days from the date raised to the decision date;
where no decision has been made, to the reporting cut-off. Spreadsheet:

```
=IF(B5="","",IF(M5="",$C$3-B5,M5-B5))
```

Use the cut-off cell rather than `TODAY()`. `TODAY()` recalculates every time the file is opened, so a
register issued with the monthly report will report different ageing next week than it did on the day it
was approved.

**Calculated column P — Cumulative approved cost.** In words: the running total of approved cost impacts
down the register. Spreadsheet, with the expanding range anchored at the first data row:

```
=SUMIFS($J$5:J5,$G$5:G5,"Approved")+SUMIFS($J$5:J5,$G$5:G5,"Implemented")
```

Two terms are needed because an implemented change is still an approved change; a single criterion would
drop rows as they progress to `Implemented`.

**Calculated column Q — Revised budget at completion.** In words: the original budget at completion plus
only those approved changes funded by additional authorised funding. Spreadsheet:

```
=$C$1+SUMIFS($J$5:J5,$G$5:G5,"Approved",$H$5:H5,"Additional authorised funding")+SUMIFS($J$5:J5,$G$5:G5,"Implemented",$H$5:H5,"Additional authorised funding")
```

**Calculated column R — Contingency remaining.** In words: opening contingency less every approved
contingency release to this point.

```
=$C$2-SUMIFS($J$5:J5,$G$5:G5,"Approved",$H$5:H5,"Contingency release")-SUMIFS($J$5:J5,$G$5:G5,"Implemented",$H$5:H5,"Contingency release")
```

**Summary block for the monthly report.** Approved this period, approved to date, pending value, rejected
to date, contingency drawn to date, contingency remaining, and pending value as a proportion of
contingency remaining. That last figure is the one that gets a sponsor's attention:

```
=IF(R_latest=0,"",pending_value/R_latest)
```

## 4. Worked fragment

*Illustrative figures.* A fictional facility upgrade project. Currency is generic currency units (CU) in
thousands, nominal basis, no escalation, rounded to the nearest thousand. The reporting cut-off is
31 May 2026. Control values: original budget at completion CU 8,100 thousand, comprising distributed
control-account budget of CU 7,700 thousand and a contingency reserve of CU 400 thousand. On this project
the contingency reserve sits inside budget at completion, as recorded in the controls execution plan.

### 4.1 Register extract

| Ref | Raised | Title | Status | Funding | Cost requested | Cost approved | Sched. req. (days) | Decision date | Days open | Cum. approved | Revised BAC | Contingency remaining |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| BCR-011 | 03 Mar 26 | Revised electrical room layout | Implemented | Contingency release | 120 | 100 | 0 | 18 Mar 26 | 15 | 100 | 8,100 | 300 |
| BCR-012 | 14 Mar 26 | Client-instructed additional metering | Implemented | Additional authorised funding | 200 | 200 | 0 | 02 Apr 26 | 19 | 300 | 8,300 | 300 |
| BCR-013 | 21 Apr 26 | Correction to steel tonnage in the estimate | Rejected | — | 145 | — | 0 | 06 May 26 | 15 | 300 | 8,300 | 300 |
| BCR-014 | 12 May 26 | Ground improvement — obstructions at pile positions | Under review | Contingency release (proposed) | 295 | — | +9 | — | 19 | 300 | 8,300 | 300 |

**Verification of the days open.** BCR-011: 3 to 18 March is 15 days. BCR-012: 14 March to 2 April is
17 days remaining in March plus 2 in April, so 19 days. BCR-013: 21 to 30 April is 9 days plus 6 in May,
so 15 days. BCR-014 is undecided, so it ages to the cut-off: 12 to 31 May is 19 days.

**Verification of the budget position.** BCR-011 is a contingency release: distributed budget rises from
7,700 to 7,800, contingency falls from 400 to 300, and budget at completion is unchanged at 8,100.
BCR-012 is additional authorised funding: distributed budget rises from 7,800 to 8,000, contingency stays
at 300, and budget at completion rises to 8,300. BCR-013 was rejected and changes nothing. The distributed
figure of CU 8,000 thousand is the budget at completion used for performance measurement in `TPL-07` and
for forecasting in `TPL-08`; the CU 300 thousand of contingency is held outside the control accounts and
is not earned against.

**The position the register is really reporting.** BCR-014 requests CU 295 thousand from a contingency
reserve of CU 300 thousand. If approved, CU 5 thousand of contingency remains, against 52.5 per cent of
the distributed budget still to be earned — the earned value sheet at `TPL-07` shows earned value at
47.5 per cent of budget at completion at this cut-off. That sentence, not the total of the register, is
what belongs on the first page of the monthly report.

### 4.2 Form extract — BCR-014

| Field | Entry |
|---|---|
| Request number | BCR-014 |
| Date raised | 12 May 2026 |
| Originator | Civil delivery manager (named) |
| Change title | Ground improvement at obstructed pile positions |
| Change class | Scope addition |
| Trigger | Site condition |
| Risk register reference | R-014 unknown obstructions — realised |
| Description of change | Excavate and remove buried obstructions at 22 pile positions in the utilities building footprint and replace with engineered fill to formation level before piling resumes at those positions. Affects WBS 1.1.2, control account CA-1000. |
| Why it cannot be absorbed | The work is outside the scope described in the dictionary entry for 1.1.2, whose stated assumption is that no obstructions requiring removal exist below formation level. There is no allowance in the work package for it. |
| Justification | Piling cannot proceed at the affected positions and the obstructions cannot be piled through. The alternative locations assessed do not satisfy the structural design. |
| Direct cost | 260 |
| Indirect cost | 35 |
| Total cost impact | 295 |
| Currency and basis | CU thousands, nominal, no escalation |
| Estimate basis and class | Subcontractor quotation for excavation and disposal against a measured obstruction survey; fill quantities from the survey at contract rates. Firm quotation for the majority; measured rates for the remainder. |
| Schedule impact | ACT-1020 piling extended by 9 calendar days. Total float on that path 12 days before, 3 days after. The path is not currently driving; the driving path runs through mechanical installation. |
| Effect on contractual milestones | None |
| Risk impact | Closes R-014 as realised. Opens R-031: further obstructions outside the surveyed area, unquantified. |
| Alternatives considered | (1) Relocate the affected piles — rejected: the structural design does not permit the required offsets, and redesign was assessed at 6 weeks and greater cost. (2) Pile deeper through the obstructions — rejected: the piling subcontractor will not warrant integrity and the specified acceptance regime could not be met. (3) Defer the affected positions to the end of the piling sequence — rejected: it does not avoid the work and it would move the path onto the driving path. |
| Consequence of doing nothing | 22 piles cannot be installed; the foundation to the north-east quadrant cannot be completed; the structural steel sequence stops at that quadrant within approximately four weeks. |
| Funding source | Contingency release (proposed) |
| Effect on budget at completion | Nil — the release moves budget already inside budget at completion from the contingency reserve into CA-1000 |
| Effect on remaining contingency | Reduces remaining contingency from 300 to 5 |
| Decision | Under review at the reporting cut-off |

The alternatives block is what makes this document defensible. Two years later, the question will not be
whether CU 295 thousand was a reasonable price for the work — it will be whether the project should have
been doing that work at all. Only the alternatives field answers that.

## 5. Common mistakes

**Approving without a funding source.** The change is approved, the budget moves, and nobody can say
whether budget at completion changed. Six months on, two reports quote different revised budgets and both
can trace their arithmetic.

**No record of budget transfers.** Because the project total does not move, transfers are treated as
administration. They are the reason a control account with a large favourable variance sits next to one
with a large adverse variance and neither manager recognises their own numbers.

**Impact fields left blank rather than assessed as nil.** A blank is indistinguishable from an oversight.
Write the nil assessment and the grounds for it.

**A cost impact with no estimate basis.** A firm quotation and a judgement are both legitimate inputs and
must not be approved on the same form without the difference visible to the approver.

**Approved but never implemented.** The register says the budget is 8,300; the cost tool still says 8,100.
Both are quoted in the same meeting. The implementation record and the verification field exist for this,
and the register should be reviewed for approved-not-implemented rows every period.

**Contingency spent without a change request.** Contingency drawn by adjusting a control account budget
directly leaves no link between the risk that was identified and the money that was spent, which is
exactly the link a sponsor asks for when contingency runs low.

**Changing the baseline to remove a variance.** A re-baseline that resets earned value to actual cost
produces a cost performance index of exactly one, and a project that has lost its ability to forecast.
The prohibition belongs in the controls execution plan so that citing it is not an argument.

**A register with no ageing.** Without days open, a request that has sat under review for eleven weeks
looks the same as one raised on Tuesday, and slow decisions are a cost the project never sees.

## 6. Adapting it

**Safe to change.** Field order and grouping. The class and trigger lists, which should match how your
organisation analyses change. Adding fields for contract clause reference, client change number,
insurance recovery, or delay-analysis method where they apply. Splitting the register by originating party
on a project with several contractors.

**Change with care.** Merging the request into the change order log (`TPL-12`) works only where every
baseline change is also a contract variation. On most projects it is not: baseline changes include
internal transfers, corrections and contingency releases that never reach the contract. Keep both and
cross-reference by number.

**Do not remove.** Alternatives considered, consequence of doing nothing, funding source, and the
implementation and verification record. Those four are the difference between a decision record and a
receipt.

**On a small project**, the form can be a single page and the register a single sheet — but keep every
field, and let the answers be short. A one-line alternatives entry is a decision recorded.

## 7. Completion checklist

- [ ] One change per request, with a title recognisable in the register
- [ ] Affected element, control account, code and activity identifiers all listed
- [ ] Every impact field completed, including those assessed as nil, with the grounds stated
- [ ] Estimate basis and class stated, and the preparer named
- [ ] Schedule impact states float before and after, and whether the path is driving
- [ ] Alternatives considered recorded with their cost and time effects and the reason for rejection
- [ ] Consequence of doing nothing stated
- [ ] Funding source selected, and the effect on budget at completion computed from it
- [ ] Approval route matches the threshold in the controls execution plan
- [ ] Approved value recorded where it differs from the requested value, with the reason
- [ ] Baseline versions before and after recorded at implementation
- [ ] Implementation verified by a second person
- [ ] Register ages open requests to the reporting cut-off, not to today's date
- [ ] Contingency remaining and pending value reported together on the same line

---

## Related

- `TPL-01 — Project controls execution plan` — where thresholds, routing and the budget definition are set
- `TPL-06 — Monthly project controls report` — where the change position is reported
- `TPL-12 — Change order log` — the contractual variation record this cross-references
- `BPG-04 — Baselining and baseline change control` — the reasoning behind the controls here
- `BPG-10 — Contingency and management reserve` — custody, release tests and drawdown discipline
- `BPG-11 — Change orders and variations` — the commercial treatment of client-instructed change

## Sources and standards

No external source is cited. This is an original instrument. The distinction between contingency reserve
and management reserve, and the identity that a contingency release does not move budget at completion,
follow the budget definitions taught in the Institute's Body of Knowledge and stated in the Institute's
master formula sheet; they are applied here in the Institute's own words, with the caveat in §2 that
organisations legitimately differ on whether contingency sits inside budget at completion.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
