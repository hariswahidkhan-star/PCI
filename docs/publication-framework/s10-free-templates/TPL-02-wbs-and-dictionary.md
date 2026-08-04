---
id: TPL-02
series: S10
series_name: Free Templates
title: Work breakdown structure and WBS dictionary
subtitle: The hierarchy table, the dictionary entry form, and the roll-up formulas that keep them consistent
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 12
summary: >
  A work breakdown structure decomposes the scope of a project into elements that can be budgeted,
  assigned and measured; the dictionary is where each element acquires a description, a deliverable,
  acceptance criteria, an owner and a budget. This template provides both — a hierarchy table with
  level, parent and roll-up columns that compute themselves, and a dictionary entry form with the ten
  fields that determine whether a work package can be argued about at handover.
linkedin:
  format: document
  hook: >
    A work breakdown structure without a dictionary is a numbering scheme. The dictionary is where the
    acceptance criteria live, and acceptance criteria are what stop a work package being argued about
    at handover.
  tags: [WorkBreakdownStructure, ProjectControls, CostEngineering, ScopeManagement]
  asset: one-pager
gated: false
related: [TPL-01, TPL-03, TPL-05, BPG-02, BPG-03]
bok_domains: [5, 8]
sources: []
placeholders: 0
---

# Work breakdown structure and WBS dictionary

> A numbered, budgeted hierarchy of scope, and the definition behind every element in it.

**In one paragraph.** A work breakdown structure decomposes the scope of a project into elements that can
be budgeted, assigned and measured; the dictionary is where each element acquires a description, a
deliverable, acceptance criteria, an owner and a budget. This template provides both — a hierarchy table
with level, parent and roll-up columns that compute themselves, and a dictionary entry form with the ten
fields that determine whether a work package can be argued about at handover.

**Who this is for.** Project controls managers, cost engineers and planners setting up a project; and the
control account managers who will be asked to own an element and should insist on a dictionary entry
before they do.

---

## 1. When to use this

Build the structure before the estimate is loaded and before the schedule is coded, because both will
inherit whatever the structure decides. Anything created afterwards has to be mapped back, and mapping is
where scope quietly falls between two elements.

The structure earns its keep in four places, and if it is not serving all four it has been built for one
audience only:

- **Budget.** Every currency unit of the estimate lands on exactly one budget-bearing element.
- **Schedule.** Every activity rolls up to exactly one element.
- **Assignment.** Every element at control-account level has one named owner.
- **Measurement.** Progress and earned value are reported against elements, not against activities.

Rebuild it when scope changes in kind rather than in quantity — a new facility, a new contract, a new
delivery entity. Do not rebuild it because the numbering has become untidy; renumbering a live structure
breaks every historical comparison in the project, and untidiness is cheaper than that.

## 2. How to complete it

**Decompose by deliverable, not by activity or by discipline.** The test at each level is whether the
child elements together produce the parent and nothing else. "Piling and foundations" is a deliverable.
"Civil engineering" is a discipline and will collect work from three unrelated deliverables. "Excavate,
then pile, then pour" is a sequence and belongs in the schedule.

**Stop decomposing when the element passes the sizing rule.** Set that rule in the controls execution plan
(`TPL-01` §3.3) and apply it without exception. A workable rule has two limbs: a work package should not
exceed one reporting period's worth of measurable output, and it should not be so small that the effort of
statusing it exceeds the value of knowing.

**Set the control-account level deliberately.** The control account is where budget, schedule and
responsibility meet, and it is the level at which cost performance is reported. Setting it too high hides
the variance inside an aggregate; too low produces indices computed on numbers too small to be stable.

**Budget only budget-bearing elements.** Enter values against work packages and planning packages only.
Everything above them rolls up. If you enter a budget against a summary element as well, the total will
double-count and the error will not be obvious because both numbers look plausible.

**Store the identifier column as text.** A structure identifier such as `1.2` is a decimal number to a
spreadsheet unless the column is formatted as text, and the roll-up formulas below match on text.
Formatting the column as text before typing anything is the single change that prevents most of the
trouble people have with this sheet.

**Write the dictionary entry when the element is created, not later.** The entry takes fifteen minutes at
creation and an argument at handover.

**Using the tables.** Copy a table block, paste into a single spreadsheet column, split on the pipe
character, and delete the alignment row.

## 3. The template

### 3.1 Sheet 1 — the hierarchy

| Col | Field | Type | Definition and entry rule |
|---|---|---|---|
| A | WBS ID | Text | Dot-separated numeric path, e.g. `1.2.3`. Format the column as text before entry. Never reused, never renumbered once budgets are loaded. |
| B | Level | Calculated | Depth in the hierarchy, where the project element is level 1. |
| C | Parent WBS ID | Calculated | The identifier one level up. Blank for the project element. |
| D | Element title | Text | A noun phrase naming the deliverable. Not a verb, not a discipline. |
| E | Element type | List | `Project` · `Summary` · `Control account` · `Work package` · `Planning package` |
| F | Budget-bearing | List | `Yes` for work packages and planning packages; `No` for everything else. Drives the roll-up. |
| G | Control account code | Text | The control account this element belongs to or is. Blank above control-account level. |
| H | Responsible party | Text | Named role in the organisational breakdown structure. One only. |
| I | Budget entered | Number | Cost budget. Enter only where column F is `Yes`; leave blank elsewhere. |
| J | Rolled-up budget | Calculated | The element's total budget, computed from the budget-bearing elements beneath it. |
| K | % of project budget | Calculated | The element's share of the project total. |
| L | Budgeted hours | Number | Direct labour hours, where hours are controlled separately from cost. Same entry rule as column I. |
| M | Dictionary status | List | `Not started` · `Drafted` · `Approved` · `Superseded` |
| N | Notes | Text | Anything a reader of the hierarchy needs that is not in the dictionary. |

**Calculated column B — Level.** In words: the level is the number of dot separators in the identifier,
plus one. Spreadsheet:

```
=IF(A2="","",LEN(A2)-LEN(SUBSTITUTE(A2,".",""))+1)
```

`SUBSTITUTE` removes the dots; the difference in length is the count of dots. `1.2.3` has two dots and is
level 3.

**Calculated column C — Parent WBS ID.** In words: everything to the left of the last dot; blank if there
is no dot. Spreadsheet:

```
=IF(ISERROR(FIND(".",A2)),"",LEFT(A2,FIND("~",SUBSTITUTE(A2,".","~",LEN(A2)-LEN(SUBSTITUTE(A2,".",""))))-1))
```

The inner `SUBSTITUTE` replaces only the *last* dot with a tilde — the fourth argument selects which
occurrence — so `FIND` can locate it. For `1.2.3` this returns `1.2`.

**Calculated column J — Rolled-up budget.** In words: if the element carries a budget itself, use it;
otherwise sum the budgets of every budget-bearing element whose identifier begins with this element's
identifier followed by a dot. Spreadsheet, over a 200-row sheet:

```
=IF($F2="Yes",$I2,SUMIFS($I$2:$I$200,$F$2:$F$200,"Yes",$A$2:$A$200,$A2&".*"))
```

The criterion `$A2&".*"` uses the wildcard `*`, which `SUMIFS` supports on text. Including the dot in the
criterion is deliberate: it prevents `1.1` from matching `1.10`. The formula is level-independent — the
same expression works at every level and never double-counts, because it sums only budget-bearing rows.

**Calculated column K — % of project budget.** In words: the element's rolled-up budget divided by the
project total, which sits in the level-1 row. Spreadsheet, with the project row at row 2:

```
=IF($J$2=0,"",J2/$J$2)
```

Format as a percentage. Store percentages as decimal fractions with percentage formatting; do not type
`50` into a cell formatted as a percentage, which stores 5,000 per cent.

**Optional integrity check.** In words: flag any row whose parent identifier does not exist in the sheet.
Spreadsheet:

```
=IF(C2="","",IF(COUNTIF($A$2:$A$200,C2)=0,"ORPHAN — parent not in sheet",""))
```

### 3.2 Sheet 2 — the dictionary entry form

One form per element at control-account level and below. Reproduce the block for each element.

| Field | Entry | Notes on completion |
|---|---|---|
| WBS ID | | Must exist in Sheet 1 |
| Element title | | Identical to Sheet 1, column D |
| Level / parent | | From Sheet 1, columns B and C |
| Element type | | Work package, planning package or control account |
| Description of work | | What is done, in three to six sentences. Written so a competent stranger could scope it. |
| Deliverable(s) | | The physical or documentary output. Countable where possible. |
| Acceptance criteria | | The test that decides whether the deliverable is complete. Objective, evidenced, and agreed by whoever accepts it. |
| Responsible party | | Name and role. One accountable party, not a team. |
| Control account code | | The account against which cost performance is reported |
| Cost breakdown structure codes | | The codes to which cost in this element is charged — see `TPL-03` |
| Budget — cost | | Currency, amount, and the basis on which it is stated |
| Budget — hours | | Direct labour hours, if controlled |
| Basis of estimate reference | | Document and version. Not a description; a reference. |
| Progress measurement technique | | Which technique and which rule of credit applies — see `TPL-05` |
| Planned start / finish | | From the baseline schedule, with the schedule activity identifiers |
| Predecessors and interfaces | | What must exist before this element can proceed, and who owes it |
| Assumptions | | Every assumption the budget and duration depend on. If it turns out false, this is the list that justifies a change request. |
| Exclusions | | What a reader might reasonably expect to be here and is not, with the element identifier where it actually sits |
| Risk register references | | Risks whose realisation would change this element |
| Approved by / date | | The person who will be held to the budget |
| Revision | | Version and change reason |

**The two fields people skip.** *Acceptance criteria* and *exclusions* are the fields that do the work.
An element with a description and a budget but no acceptance criteria can be declared complete by anyone
with an opinion. An element with no exclusions absorbs, by default, everything adjacent to it that nobody
else claimed.

## 4. Worked fragment

*Illustrative figures.* A fictional facility upgrade project. Currency is stated in generic currency units
(CU) in thousands; the basis is nominal, single currency, with no escalation applied. Budgets are
distributed control-account budget only; the contingency reserve is held outside the structure and is not
shown here. Percentages are rounded to one decimal place, half away from zero.

### 4.1 Hierarchy extract

| WBS ID | Level | Parent | Element title | Type | Budget-bearing | CA code | Responsible | Budget entered | Rolled-up budget | % of project |
|---|---|---|---|---|---|---|---|---|---|---|
| 1 | 1 | | Facility upgrade project | Project | No | | Project director | | 8,000 | 100.0 % |
| 1.1 | 2 | 1 | Civil works | Control account | No | CA-1000 | Civil delivery manager | | 4,000 | 50.0 % |
| 1.1.1 | 3 | 1.1 | Site preparation and earthworks | Work package | Yes | CA-1000 | Civil delivery manager | 650 | 650 | 8.1 % |
| 1.1.2 | 3 | 1.1 | Piling and foundations | Work package | Yes | CA-1000 | Civil delivery manager | 1,900 | 1,900 | 23.8 % |
| 1.1.3 | 3 | 1.1 | Structural steel | Work package | Yes | CA-1000 | Civil delivery manager | 1,050 | 1,050 | 13.1 % |
| 1.1.4 | 3 | 1.1 | Civil supervision and quality assurance | Work package | Yes | CA-1000 | Civil delivery manager | 400 | 400 | 5.0 % |
| 1.2 | 2 | 1 | Mechanical works | Control account | No | CA-2000 | Mechanical delivery manager | | 3,000 | 37.5 % |
| 1.2.1 | 3 | 1.2 | Equipment procurement | Work package | Yes | CA-2000 | Procurement lead | 1,800 | 1,800 | 22.5 % |
| 1.2.2 | 3 | 1.2 | Mechanical installation | Work package | Yes | CA-2000 | Mechanical delivery manager | 1,200 | 1,200 | 15.0 % |
| 1.3 | 2 | 1 | Controls and commissioning | Control account | No | CA-3000 | Commissioning manager | | 1,000 | 12.5 % |
| 1.3.1 | 3 | 1.3 | Control system supply and configuration | Work package | Yes | CA-3000 | Systems engineer | 600 | 600 | 7.5 % |
| 1.3.2 | 3 | 1.3 | Commissioning and handover | Planning package | Yes | CA-3000 | Commissioning manager | 400 | 400 | 5.0 % |

**Verification of the roll-up.** Civil works: 650 + 1,900 + 1,050 + 400 = 4,000. Mechanical works:
1,800 + 1,200 = 3,000. Controls and commissioning: 600 + 400 = 1,000. Project: 4,000 + 3,000 + 1,000 =
8,000, which matches the sum of the eight budget-bearing rows and is the figure the earned value sheet in
`TPL-07` uses as its budget at completion.

**Verification of the percentages.** The level-3 shares of the project total sum to 100.0 per cent:
8.125 + 23.75 + 13.125 + 5.0 + 22.5 + 15.0 + 7.5 + 5.0 = 100.0. Displayed to one decimal place they read
8.1, 23.8, 13.1, 5.0, 22.5, 15.0, 7.5 and 5.0 — which sum visibly to 100.1 because of rounding. Do not
force the displayed figures to add; footnote the rounding instead.

Element `1.3.2` is a planning package rather than a work package: the commissioning scope is budgeted but
not yet decomposed, and it carries a date by which it must be. That is a legitimate state during
execution and a serious one at closeout.

### 4.2 Dictionary entry extract — element 1.1.2

| Field | Entry |
|---|---|
| WBS ID | 1.1.2 |
| Element title | Piling and foundations |
| Level / parent | 3 / 1.1 |
| Element type | Work package |
| Description of work | Install bored piles to the design schedule across the utilities building footprint, construct pile caps and ground beams, and complete backfill to formation level. Includes setting out, pile integrity testing and reinstatement of the working platform. Excludes the working platform itself, which is constructed under 1.1.1. |
| Deliverable(s) | 240 bored piles installed and tested to the specified acceptance regime; 60 pile caps and associated ground beams cast and cured. |
| Acceptance criteria | Pile integrity test results issued and accepted for every pile; concrete cube results at 28 days meeting the specified characteristic strength for every pour; as-built setting-out survey issued and within the specified tolerance; no open non-conformance reports against the element. |
| Responsible party | Civil delivery manager (named individual) |
| Control account code | CA-1000 |
| Cost breakdown structure codes | `FU1-10-CIV-10-SK`, `FU1-10-CIV-10-SR`, `FU1-10-CIV-20-BU`, `FU1-10-CIV-40-RM` |
| Budget — cost | CU 1,900 thousand, nominal, no escalation |
| Budget — hours | 9,000 direct labour hours |
| Basis of estimate reference | Estimate BOE-CIV-004 rev C |
| Progress measurement technique | Piling: units complete on pile count. Pile caps: weighted milestone. See `TPL-05`. |
| Planned start / finish | 6 April 2026 / 30 October 2026 (activities ACT-1020, ACT-1030) |
| Predecessors and interfaces | Working platform complete and surveyed (1.1.1); piling design issued for construction by engineering; ground investigation report accepted by the client. |
| Assumptions | No obstructions below formation level requiring removal; continuous access to the full footprint from the start date; single piling rig throughout; cost includes materials, plant and subcontract, while the hours figure is direct labour only. |
| Exclusions | Working platform (1.1.1). Ground improvement or obstruction removal, which is not in the baseline. Building slab, which is in 1.1.3. |
| Risk register references | R-014 unknown obstructions; R-022 piling rig availability |
| Approved by / date | Project director, 18 February 2026 |
| Revision | Rev B — assumption on rig count added after estimate review |

The first assumption in that entry is doing a great deal of work. When obstructions are found, the
existence of the written assumption is the difference between a change request and an argument. The
worked change request in `TPL-04` is the one that follows from it.

## 5. Common mistakes

**Decomposing by discipline.** A branch called "Civil", "Mechanical" and "Electrical" reads tidily and
measures nothing, because a single deliverable is then split across three branches and no element can be
declared complete. Discipline belongs in the cost breakdown structure (`TPL-03`), where it is a coding
segment, not in the work breakdown structure, where it is a scope boundary.

**Budgeting summary elements as well as work packages.** The total doubles somewhere and the error is
invisible because every individual number looks right. Column F exists to make this structurally
impossible.

**Renumbering a live structure.** Every historical report, change request, risk reference and schedule
code then points at the wrong element. If an identifier is wrong, retire it and create a new one; never
reassign.

**Identifiers stored as numbers.** `1.10` and `1.1` become the same value, the roll-up wildcard stops
matching, and the level formula returns nonsense. Format the column as text first.

**A dictionary that repeats the title.** "Description: piling and foundations works" adds nothing. If the
description does not let a competent stranger scope the element, it is not a description.

**Acceptance criteria written as adjectives.** "Completed to a satisfactory standard" cannot be failed and
therefore cannot be passed. Name the test, the document that records it and the person who signs it.

**No exclusions.** The element with no exclusions is the one that absorbs everything nobody else claimed,
and the absorption is discovered at the point when the budget is already spent.

**Planning packages that are never decomposed.** A planning package is a promise to decompose later. Give
each one a date and put that date on the mobilisation log, or it will still be a planning package at
handover.

## 6. Adapting it

**Safe to change.** The number of levels. Identifier formats — alphanumeric segments work with the level
and parent formulas provided the separator is a dot and the column is text. Adding columns for contract
package, site or delivery entity. Splitting the hierarchy sheet by branch on a very large project,
provided the roll-up ranges are widened to match.

**Change with care.** Moving the control-account level after budgets are loaded changes what every
historical index was computed on. If you must, keep both and report the change explicitly for one period.

**Do not remove.** Column F (budget-bearing), because without it the roll-up double-counts; and the
acceptance criteria and exclusions fields in the dictionary, because those are the two fields the document
exists for.

**On a small project**, the hierarchy may be three levels and the dictionary a single sheet with one row
per element and the same field names as column headings. The fields do not shrink; only the formatting
does.

## 7. Completion checklist

- [ ] WBS ID column formatted as text before any entry
- [ ] Every level-3 and below element decomposes its parent completely and adds nothing else
- [ ] Sizing rule from `TPL-01` §3.3 applied without exception
- [ ] Control-account level set and stated
- [ ] Budgets entered only where budget-bearing is `Yes`
- [ ] Roll-up total equals the sum of budget-bearing rows, checked independently
- [ ] Orphan check returns no results
- [ ] Every control account has one named responsible party
- [ ] Every element at control-account level and below has a dictionary entry
- [ ] Every dictionary entry has objective acceptance criteria and explicit exclusions
- [ ] Every assumption that the budget depends on is written down
- [ ] Every planning package has a date by which it will be decomposed
- [ ] Structure agreed with the schedule owner and the cost breakdown structure owner before budgets load

---

## Related

- `TPL-01 — Project controls execution plan` — where the sizing rule and control-account level are set
- `TPL-03 — Cost breakdown structure and code of accounts` — the coding dimension this structure does not carry
- `TPL-05 — Progress measurement and rules of credit sheet` — how the elements defined here are measured
- `BPG-02 — The work breakdown structure` — the reasoning behind decomposition choices
- `BPG-03 — Cost breakdown structure and the code of accounts` — how the two structures work together

## Sources and standards

No external source is cited. This is an original instrument; the decomposition principles it applies are
common to established project management and cost engineering practice, explained here in the Institute's
own words, and no third-party structure, template or wording is reproduced.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
