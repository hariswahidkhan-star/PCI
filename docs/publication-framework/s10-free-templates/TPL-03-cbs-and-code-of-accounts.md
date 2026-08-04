---
id: TPL-03
series: S10
series_name: Free Templates
title: Cost breakdown structure and code of accounts
subtitle: A five-segment coding structure, its mapping to the general ledger, and the rules for adding a code
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 12
summary: >
  A cost breakdown structure decides what questions you will be able to answer about cost for the life
  of the project, because a cost can only be analysed along the dimensions it was coded with. This
  template gives a five-segment code of accounts — project, area, discipline, cost type and resource
  class — with each segment's values defined, a mapping column to the general ledger, formulas that
  build and validate the code, and the six rules that govern adding a new one.
linkedin:
  format: document
  hook: >
    The moment to decide how a cost is coded is before it is incurred. A code of accounts written after
    the first invoice arrives becomes a reclassification exercise that never quite finishes.
  tags: [CostEngineering, ProjectControls, CostBreakdownStructure, ProjectAccounting]
  asset: one-pager
gated: false
related: [TPL-01, TPL-02, TPL-07, BPG-03, BPG-07]
bok_domains: [1, 5, 11]
sources: []
placeholders: 0
---

# Cost breakdown structure and code of accounts

> A coding structure that decides, in advance, which questions about cost you will be able to answer.

**In one paragraph.** A cost breakdown structure decides what questions you will be able to answer about
cost for the life of the project, because a cost can only be analysed along the dimensions it was coded
with. This template gives a five-segment code of accounts — project, area, discipline, cost type and
resource class — with each segment's values defined, a mapping column to the general ledger, formulas
that build and validate the code, and the six rules that govern adding a new one.

**Who this is for.** Cost engineers and cost controllers setting up or repairing a coding structure;
project controls managers who must agree it with finance; and project accountants who will have to live
with the mapping.

---

## 1. When to use this

Set the structure before the first commitment is raised. A cost that has been incurred against a code
cannot be re-analysed along a dimension the code never carried; it can only be reclassified, and
reclassification across a closed accounting period is an accounting event with its own approval
requirements rather than a spreadsheet edit.

Use it in three situations:

- **Project set-up**, alongside the work breakdown structure (`TPL-02`) and before the estimate is loaded.
  The two structures are different questions: the work breakdown structure asks *what scope*, the cost
  breakdown structure asks *what kind of cost*. Every commitment carries one identifier from each.
- **Taking over a project whose reports cannot be broken down.** If nobody can tell you what proportion
  of spend to date is subcontract, the structure is the cause and no amount of reporting effort
  substitutes.
- **Before a portfolio comparison.** Two projects can only be compared on cost type or discipline if both
  coded for it. Retrofitting a comparison across projects that coded differently produces a number with no
  defensible meaning.

## 2. How to complete it

**Design the segments backwards from the questions.** List the cost questions the project must answer —
to the client, to finance, to the estimating function for future benchmarking, to the tax and statutory
reporting process. Each recurring question is a segment. A segment that answers no question anyone has
asked is overhead on every transaction for the life of the project.

**Keep segments orthogonal.** Each segment answers a different question, and a value in one segment
should never imply a value in another. If discipline `CIV` always appears with cost type `40`, one of the
two is redundant and the structure is more granular than it is informative.

**Fix the segment lengths and never vary them.** Fixed-length segments allow the code to be parsed by
position, which is what makes a flat code list from a finance system usable. Variable-length segments
force every consumer to parse on the separator, and one missing separator breaks the whole extract.

**Include a "not applicable" value in every segment.** Plant hire has no meaningful skill class; a
project-wide cost has no area. Without an explicit value such as `ZZ` or `00`, users will either leave
the segment blank — which breaks fixed-length parsing — or invent a value.

**Map every code to exactly one general ledger account before opening it.** The mapping is the point where
project reporting and statutory reporting are reconciled. A code with no mapping produces a cost that
appears in the project report and cannot be found in the accounts, and that discrepancy is always
discovered at the least convenient moment.

**Do not encode an accounting policy decision in the structure.** Whether an item is capitalised or
expensed depends on the financial reporting framework the reporting entity applies and on that entity's
own accounting policy, and it varies between jurisdictions and between entities. The mapping table
*records* that determination and names who made it; it does not make it, and this template does not state
what the answer should be.

**Using the tables.** Copy a table block, paste into a single spreadsheet column, split on the pipe
character, and delete the alignment row.

## 3. The template

### 3.1 The code structure

Five segments, separated by hyphens, fixed length, total sixteen characters:

```
PPP-AA-DDD-TT-RR
 │   │   │   │  └── Resource class      2 characters
 │   │   │   └───── Cost type           2 digits
 │   │   └───────── Discipline          3 letters
 │   └───────────── Area                2 digits
 └───────────────── Project             3 characters
```

Example: `FU1-10-CIV-10-SK` reads as project FU1, area 10, civil discipline, direct labour, skilled craft.

### 3.2 Segment definitions

**Segment 1 — Project (3 characters).** The project, contract or delivery entity whose cost this is.
Assigned centrally, not by the project. Needed even on a single-project sheet, because the sheet will
eventually be consolidated.

**Segment 2 — Area (2 digits).** The physical, geographic or facility area in which the cost is incurred.

| Value | Meaning |
|---|---|
| 00 | Project-wide; not attributable to an area |
| 10 | *(define — e.g. utilities building)* |
| 20 | *(define — e.g. process area)* |
| 30 | *(define — e.g. external works and site infrastructure)* |
| 90 | Temporary works and site establishment |
| ZZ | Not applicable |

**Segment 3 — Discipline (3 letters).** The engineering or delivery discipline that owns the work.
Suggested values: `CIV` civil, `STR` structural, `MEC` mechanical, `PIP` piping, `ELE` electrical,
`ICA` instrumentation and control, `ARC` architectural, `PMC` project management and controls,
`HSE` health, safety and environment, `ZZZ` not applicable.

**Segment 4 — Cost type (2 digits).** The nature of the cost. This is the segment finance cares about and
the one most often left out.

| Value | Cost type | Includes |
|---|---|---|
| 10 | Direct labour | Own-workforce labour charged to the project at a rate |
| 20 | Permanent materials | Materials and equipment forming part of the finished asset |
| 30 | Construction plant and equipment | Owned, hired or operated plant used to build, not delivered |
| 40 | Subcontract | Work executed under a subcontract, of any pricing basis |
| 50 | Site indirects | Site establishment, supervision, temporary facilities, site services |
| 60 | Expenses and other | Travel, permits, insurances charged to the project, sundry |
| 70 | Risk provision | Contingency held against identified risk. Never charged directly; released by change (`TPL-04`) |
| 80 | Escalation provision | Provision for price movement, where held separately |

**Segment 5 — Resource class (2 characters).** The rate-bearing resource group within the cost type. The
valid values depend on the cost type, which is deliberate and must be enforced by the code register rather
than by a free-text field.

| Cost type | Valid resource classes |
|---|---|
| 10 Direct labour | `EN` engineering and technical staff · `SR` supervision · `SK` skilled craft · `SS` semi-skilled and general |
| 20 Permanent materials | `BU` bulk materials · `TE` tagged equipment · `CS` consumables |
| 30 Plant | `OP` operated hire · `DR` dry hire · `OW` owned plant |
| 40 Subcontract | `LS` lump sum · `RM` remeasurable · `DT` daywork and time-and-materials |
| 50, 60, 70, 80 | `ZZ` not applicable |

### 3.3 Sheet 1 — the code register

| Col | Field | Type | Definition and entry rule |
|---|---|---|---|
| A | Project | Text | Segment 1 |
| B | Area | Text | Segment 2. Store as text so leading zeros survive. |
| C | Discipline | Text | Segment 3 |
| D | Cost type | Text | Segment 4. Store as text. |
| E | Resource class | Text | Segment 5 |
| F | Full code | Calculated | The assembled sixteen-character code |
| G | Description | Text | Plain-language description of what belongs here |
| H | Budget | Number | Approved budget against this code |
| I | General ledger account | Text | The single account this code maps to |
| J | General ledger account name | Text | As it appears in the chart of accounts |
| K | Cost centre | Text | Where the receiving entity uses cost centres |
| L | Capital / expense determination | Text | The determination made, the policy relied on, and who made it. Not a project controls decision — see §2. |
| M | Opened by / date | Text | Who created the code and when |
| N | Effective from period | Text | The accounting period from which the code may be charged |
| O | Status | List | `Proposed` · `Open` · `Closed` |
| P | Length check | Calculated | Structural validation |
| Q | Duplicate check | Calculated | Uniqueness validation |

**Calculated column F — Full code.** In words: the five segments joined by hyphens. Spreadsheet:

```
=IF(COUNTBLANK(A2:E2)>0,"",TEXTJOIN("-",TRUE,A2:E2))
```

Where `TEXTJOIN` is unavailable, use `=IF(COUNTBLANK(A2:E2)>0,"",A2&"-"&B2&"-"&C2&"-"&D2&"-"&E2)`.

**Calculated column P — Length check.** In words: a valid assembled code is exactly sixteen characters;
anything else means a segment is the wrong length. Spreadsheet:

```
=IF(F2="","",IF(LEN(F2)=16,"OK","CHECK — segment length"))
```

**Calculated column Q — Duplicate check.** In words: flag a code that appears more than once in the
register. Spreadsheet:

```
=IF(F2="","",IF(COUNTIF($F$2:$F$500,F2)>1,"DUPLICATE","OK"))
```

**Roll-up by any segment.** In words: sum the budget where the chosen segment column matches a given
value. Spreadsheet, summing all civil budget:

```
=SUMIFS($H$2:$H$500,$C$2:$C$500,"CIV")
```

Always roll up on the segment columns, not on the assembled code. Where you receive a flat list of codes
from a finance system with no segment columns, the segments can be recovered by position because the
lengths are fixed — `LEFT(F2,3)` for project, `MID(F2,5,2)` for area, `MID(F2,8,3)` for discipline,
`MID(F2,12,2)` for cost type and `MID(F2,15,2)` for resource class. Rebuild the segment columns from
those and roll up on the columns; do not embed the extraction inside every summation.

### 3.4 Sheet 2 — the general ledger mapping summary

One row per general ledger account, for reconciliation with finance.

| Col | Field | Definition |
|---|---|---|
| A | General ledger account | The account code |
| B | Account name | As in the chart of accounts |
| C | Cost breakdown structure codes mapped | List, or a count |
| D | Budget mapped | Sum of budget across the codes mapped to this account |
| E | Actual cost per project ledger | From the controls system |
| F | Actual cost per general ledger | From finance |
| G | Difference | Calculated |
| H | Difference % | Calculated |
| I | Explanation and owner | Required whenever column H exceeds the agreed tolerance |

**Calculated column G — Difference.** In words: the project ledger figure less the general ledger figure.
Spreadsheet: `=E2-F2`.

**Calculated column H — Difference %.** In words: the difference expressed as a proportion of the general
ledger figure, which is the authoritative one. Spreadsheet:

```
=IF(F2=0,"",G2/F2)
```

**Calculated column D — Budget mapped.** In words: the total budget of every code mapped to this account.
Spreadsheet, referencing Sheet 1:

```
=SUMIFS('Sheet 1'!$H$2:$H$500,'Sheet 1'!$I$2:$I$500,$A2)
```

### 3.5 Rules for adding a code

These belong in the controls execution plan (`TPL-01` §3.3) and are quoted here so the register carries
them.

1. **A code is created by the cost controller on a written request**, never by a user typing a new value
   into a free-text field. The register in §3.3 is the only place codes exist.
2. **A new code is opened only when an existing code cannot carry the cost without losing a distinction
   somebody has asked for.** "It would be tidier" is not a reason; name the report the new code serves.
3. **Segment values are added, never redefined.** Changing what `20` means in the area segment
   invalidates every historical figure coded to it. If a value is wrong, close it and open a new one.
4. **A closed value is never reused.** Reuse makes two different things share a history.
5. **Every code maps to exactly one general ledger account before it is opened**, and the mapping is
   agreed with finance, not asserted by the project.
6. **A code is effective from a stated accounting period.** No retrospective charging into a closed period
   and no re-coding of a closed period without a documented reclassification approved through the same
   route as a journal entry.

## 4. Worked fragment

*Illustrative figures.* A fictional facility upgrade project. Currency is generic currency units (CU) in
thousands, nominal basis, no escalation. The general ledger account numbers are an illustrative chart of
accounts and are not a recommendation; every organisation's chart differs. Percentages are to two decimal
places here because they are shares of a subtotal, and rounding them to one would obscure the check.

### 4.1 Code register extract — civil control account, area 10

| Full code | Description | Budget | GL account | GL account name | Length check |
|---|---|---|---|---|---|
| FU1-10-CIV-10-SK | Civil direct labour — skilled craft | 620 | 5100 | Direct labour | OK |
| FU1-10-CIV-10-SR | Civil direct labour — supervision | 180 | 5100 | Direct labour | OK |
| FU1-10-CIV-20-BU | Civil permanent materials — bulk (concrete, reinforcement) | 1,240 | 5200 | Materials | OK |
| FU1-10-CIV-30-DR | Civil construction plant — dry hire | 310 | 5300 | Plant hire | OK |
| FU1-10-CIV-40-RM | Civil subcontract — remeasurable (piling) | 1,450 | 5400 | Subcontract | OK |
| FU1-10-CIV-50-ZZ | Civil site indirects | 200 | 5500 | Site indirects | OK |
| | **Total — control account CA-1000** | **4,000** | | | |

**Verification.** 620 + 180 + 1,240 + 310 + 1,450 + 200 = 4,000, which is the budget of control account
CA-1000 in the work breakdown structure at `TPL-02` §4.1 and the budget at completion for that control
account in the earned value sheet at `TPL-07` §4.

**Verification of the length check.** `FU1-10-CIV-10-SK` is 3 + 1 + 2 + 1 + 3 + 1 + 2 + 1 + 2 = 16
characters, so column P returns `OK`. A code of any other length has a segment entered at the wrong width
and will fail positional parsing downstream.

### 4.2 Roll-up by cost type — same extract

| Cost type | Description | Budget | Share of control account |
|---|---|---|---|
| 10 | Direct labour | 800 | 20.00 % |
| 20 | Permanent materials | 1,240 | 31.00 % |
| 30 | Construction plant | 310 | 7.75 % |
| 40 | Subcontract | 1,450 | 36.25 % |
| 50 | Site indirects | 200 | 5.00 % |
| | **Total** | **4,000** | **100.00 %** |

**Verification.** Direct labour is 620 + 180 = 800. The shares are 800 ÷ 4,000 = 20.00 %,
1,240 ÷ 4,000 = 31.00 %, 310 ÷ 4,000 = 7.75 %, 1,450 ÷ 4,000 = 36.25 % and 200 ÷ 4,000 = 5.00 %, summing
to 100.00 %.

This is the analysis the structure exists to produce, and it is the one that cannot be produced at all if
the cost type segment was omitted at set-up. Note what it tells you that the work breakdown structure
cannot: over a third of this control account is subcontract, so its cost performance will be driven by
subcontract administration rather than by productivity, and the forecasting method chosen in `TPL-08`
should reflect that.

## 5. Common mistakes

**Building the cost breakdown structure as a copy of the work breakdown structure.** They answer different
questions. If the coding structure repeats the scope hierarchy, the project has one dimension of analysis
where it needs two, and cost type — the dimension finance and estimating both want — is missing.

**Omitting the cost type segment.** It is the most commonly omitted and the most commonly needed. Without
it there is no answer to what proportion of spend is labour, and therefore no basis for a productivity
conversation or for a future estimate.

**Variable-length segments.** A structure where the project code is sometimes two characters and sometimes
four cannot be parsed by position, and every downstream extract needs bespoke handling.

**Codes stored as numbers.** An area value of `01` becomes `1`, the assembled code fails the length check,
and positional extraction returns the wrong segment.

**A free-text code field on the requisition form.** Within a month there will be three spellings of the
same code and a reconciliation that never closes. Codes come from a controlled list.

**One code, several general ledger accounts.** As soon as a code can land in more than one account, the
reconciliation in §3.4 stops being arithmetic and becomes an investigation.

**Treating a capitalisation determination as a coding convention.** Whether costs are capitalised or
expensed follows the applicable financial reporting framework and the entity's accounting policy, varies
between jurisdictions, and is finance's determination to make. The register records it and names who made
it. A project controls function that decides it has created an accounting exposure it cannot see.

**Charging directly to the risk provision code.** Contingency is released into a control account by an
approved change (`TPL-04`) and spent there. Charging costs straight to code type `70` destroys the audit
trail between the risk that was identified and the money that was spent.

## 6. Adapting it

**Safe to change.** Segment lengths, provided they are fixed and the length check and positional
extraction are updated to match. The values inside each segment. Adding a sixth segment — contract package,
funding source, work-type — where a recurring question needs it. Reordering segments, provided you do it
before any cost is incurred and update the extraction positions.

**Change with care.** Adding a segment to a live structure means every existing code is short and every
historical comparison needs a default value. It is possible, but do it at a period boundary with a
documented effective date, not mid-period.

**Do not remove.** The cost type segment, the general ledger mapping column, and the effective-from
period. Those three are what make the structure reconcilable to the accounts rather than a private
project taxonomy.

**Where the client imposes a coding structure**, adopt theirs as the reporting code and keep your own as
the internal code, with the mapping held in the register as an additional column. Do not try to work in
one structure that satisfies both; the compromise loses distinctions each side actually needs.

## 7. Completion checklist

- [ ] Every segment answers a question someone has actually asked
- [ ] Segment lengths fixed and documented; all segment columns formatted as text
- [ ] Every segment has an explicit "not applicable" value
- [ ] Valid resource classes constrained by cost type in the register, not left free-text
- [ ] Every code maps to exactly one general ledger account, agreed with finance
- [ ] Capital or expense determination recorded with the policy relied on and the person who made it
- [ ] Length check and duplicate check return `OK` on every row
- [ ] Roll-up by cost type reconciles to the control account budget
- [ ] Codes carry an effective-from accounting period
- [ ] The six rules in §3.5 are written into the controls execution plan and agreed with finance
- [ ] Requisition and timesheet systems draw the code from the controlled list, not from free text
- [ ] A reconciliation cycle and a tolerance are agreed before the first period close

---

## Related

- `TPL-01 — Project controls execution plan` — where the coding rules and the reconciliation cycle are agreed
- `TPL-02 — Work breakdown structure and WBS dictionary` — the scope dimension this structure complements
- `TPL-07 — Earned value calculation sheet` — where coded actual cost becomes a performance measure
- `BPG-03 — Cost breakdown structure and the code of accounts` — the design reasoning in full
- `BPG-07 — Accruals and cut-off discipline` — why the effective-from period rule matters at close

## Sources and standards

No external source is cited. The segment scheme, values and rules here are an original instrument. Chart
of accounts numbers in §4 are illustrative and do not represent any organisation's chart. Accounting
treatment referred to in §2 and §5 varies by financial reporting framework and by jurisdiction, and this
document deliberately states no treatment as universal.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
