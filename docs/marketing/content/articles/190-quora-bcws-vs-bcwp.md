---
platform:      Quora
type:          qa-list
title:         BCWS vs BCWP: the old names for PV and EV explained
meta:          BCWS vs BCWP: BCWS is planned value, BCWP is earned value. The old labels, the modern ones, and a worked month showing exactly what each answers.
primary_kw:    BCWS vs BCWP
secondary_kw:  planned value, earned value, ACWP, earned value formulas
pillar:        Earned value management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1,636
hashtags:      n/a (Quora)
ab_id:         AB-00264
---

# BCWS vs BCWP: the old names for PV and EV explained

BCWS vs BCWP is a difference of one word. BCWS is the budgeted cost of work scheduled, now called planned value; BCWP is the budgeted cost of work performed, now called earned value. BCWS is what the baseline said you would have completed in money terms by today; BCWP is the budget value of what you actually completed.

The confusion comes from the shared first two letters. Both are priced at budget rates, and the word that separates them is *scheduled* against *performed*.

## BCWS vs BCWP: what is the difference, exactly?

Three measures carry two vocabularies. The arithmetic never changed.

| Old name | Stands for | Modern name | The question it answers |
|---|---|---|---|
| BCWS | Budgeted cost of work scheduled | Planned value (PV) | What was the baseline value of the work due by this date? |
| BCWP | Budgeted cost of work performed | Earned value (EV) | What is the baseline value of the work actually done? |
| ACWP | Actual cost of work performed | Actual cost (AC) | What did that completed work actually cost? |

Two of them use budget rates and one uses real money. That is the whole structure: BCWS and BCWP are both priced from the baseline, so comparing them isolates schedule progress; ACWP brings in real cost, so comparing it with BCWP isolates efficiency.

The older labels originate in the defence cost and schedule control criteria used by government buyers, which is why they persist in government contracting long after commercial practice moved to PV, EV and AC.

## A worked month, in the old vocabulary

A package has a budget at completion of $12,400k. At the month 7 cut-off:

- BCWS (planned value) = **$5,100k**
- BCWP (earned value) = **$4,590k**
- ACWP (actual cost) = **$5,270k**

Schedule variance = BCWP − BCWS = 4,590 − 5,100 = **−$510k**. Less work has been done than the baseline required, expressed in money.

Cost variance = BCWP − ACWP = 4,590 − 5,270 = **−$680k**. The work done cost more than its budget value.

| Measure | Formula in old names | Formula in modern names | Result |
|---|---|---|---:|
| Schedule variance | BCWP − BCWS | EV − PV | **−$510k** |
| Cost variance | BCWP − ACWP | EV − AC | **−$680k** |
| Schedule performance index | BCWP ÷ BCWS | EV ÷ PV | 4,590 ÷ 5,100 = **0.900** |
| Cost performance index | BCWP ÷ ACWP | EV ÷ AC | 4,590 ÷ 5,270 = **0.871** |
| Schedule variance % | SV ÷ BCWS | SV ÷ PV | −510 ÷ 5,100 = **−10.0%** |
| Cost variance % | CV ÷ BCWP | CV ÷ EV | −680 ÷ 4,590 = **−14.8%** |
| Per cent complete | BCWP ÷ BAC | EV ÷ BAC | 4,590 ÷ 12,400 = **37.0%** |
| Per cent spent | ACWP ÷ BAC | AC ÷ BAC | 5,270 ÷ 12,400 = **42.5%** |

Read the last two lines together. The package is 37.0 per cent done and 42.5 per cent spent, which is the same story the cost performance index of 0.871 tells, in a form a sponsor grasps immediately.

## What the two indices are actually saying

A cost performance index of 0.871 means the project earns 87.1 cents of budgeted value for every dollar it spends. It is a productivity measure, and it is the more reliable of the two.

A schedule performance index of 0.900 means 90 per cent of the value planned by this date has been earned. It is a money measure wearing a schedule label, which is where it misleads.

Neither index says anything about whether the baseline was sensible. A programme built to hit a promised date rather than to model the work produces confident indices about a fiction.

## The forecast that follows

If the efficiency to date continues, the forecast outturn is BAC ÷ CPI = 12,400 ÷ 0.871 = **$14,237k**, computed exactly as 12,400 × 5,270 ÷ 4,590.

Variance at completion is 12,400 − 14,237 = **−$1,837k**.

The to-complete performance index asks what efficiency is now required to land on the original budget: (BAC − BCWP) ÷ (BAC − ACWP) = (12,400 − 4,590) ÷ (12,400 − 5,270) = 7,810 ÷ 7,130 = **1.095**.

The team has delivered 0.871 and must now deliver 1.095, a 26 per cent improvement in productivity, for the budget to hold. If nothing structural has changed, that forecast is a hope with a formula attached.

That single-method forecast is one of four available, and each carries a different assumption about cause: that the overrun is closed, that performance to date continues, that recovering time keeps costing money, or that the remaining scope is worth re-pricing from the bottom up. [Which of those four assumptions you are signing](https://projectcontrolsinstitute.org/four-eac-formulas) is a judgement about what went wrong, not a calculation.

## Why BCWP stops telling the truth about time

Both schedule measures collapse at the end of a project, and this is the defect the three-letter era never solved.

As the work finishes, BCWP rises to meet BAC and BCWS also rises to BAC, so schedule variance drives to zero and the index drives to 1.00 — even on a project that is months late. A schedule measure that reports perfection while the site is still open is worse than no measure.

Earned schedule fixes it by asking a time question instead of a money one: at what date on the baseline curve was $4,590k of planned value due? If the cumulative BCWS curve reached 4,590 at month 6.4 and the actual time is month 7.0, then the time-based index is 6.4 ÷ 7.0 = **0.914** and the time variance is **−0.6 months**.

| Measure | Basis | Result | Behaviour near completion |
|---|---|---:|---|
| SV = BCWP − BCWS | Money | −$510k | Drives to zero regardless of lateness |
| SPI = BCWP ÷ BCWS | Money ratio | 0.900 | Drives to 1.00 regardless of lateness |
| Earned schedule variance | Time | −0.6 months | Stays honest to the final period |
| SPI(t) | Time ratio | 0.914 | Stays honest to the final period |

Report the money variance if your client requires it, and manage against the time one.

## Where the old names still turn up

Government and defence contracts that specify an earned value management system frequently keep the original vocabulary in the clause and the reporting format.

Older report templates, spreadsheets inherited from a previous programme, and some tool column headings still carry the three letters. Textbooks published before the vocabulary shifted use them throughout.

Practically, treat them as synonyms and say so in your report glossary. Arguments about which name is correct waste more time than the translation ever does.

## Three mistakes the old names cause

**Reading ACWP as committed cost.** Actual cost is the cost of work performed, so purchase orders raised for materials not yet installed do not belong in it. Including them depresses the cost performance index and manufactures a variance that does not exist.

**Reading BCWP as cash or revenue.** Earned value is priced at budget rates for internal control. It is not what you may invoice and it is not what may be recognised as revenue, both of which follow the contract and the accounting standard.

**Comparing BCWP and ACWP across different cut-offs.** If progress is measured to the 25th and costs are booked to the 30th, the index is a comparison of two different months. Most disputed cost performance indices are a cut-off problem, not a productivity problem.

All three are definition and cut-off problems rather than productivity problems, which is why they survive so long. The arithmetic is right; the inputs are describing different things.

## Frequently asked questions

**Is BCWS the same as the budget?**
No. BCWS is the portion of the budget that the baseline scheduled to be completed by a given date, so it is time-phased. The whole budget is the budget at completion. On the numbers above, BCWS at month 7 is $5,100k while BAC is $12,400k, and they only converge when the last activity is due to finish.

**Can BCWP exceed BCWS?**
Yes, and it means more work has been completed than the baseline planned for that date, which reads as ahead of schedule in money terms. Treat it carefully: it often reflects easy scope pulled forward while difficult scope stalls. Check which control accounts produced the surplus before reporting it as good news.

**Which is used to calculate the cost performance index?**
BCWP divided by ACWP — earned value divided by actual cost. BCWS plays no part in any cost measure, because it contains no information about what the work cost. Mixing BCWS into a cost index is the single most common error when people first meet the three-letter vocabulary.

**Do PMI and government contracts use different formulas?**
The formulas are identical; only the vocabulary and the reporting formality differ. A government earned value management system specifies surveillance, baseline control and reporting formats far more tightly than commercial practice, but BCWP ÷ ACWP is the same calculation in both worlds.

**Should I write PV and EV or BCWS and BCWP in my report?**
Use whichever your contract or client specifies, and define both in the glossary on the first page. If you have a free choice, PV, EV and AC read better to anyone outside the discipline, including the finance team who will be asked about the same numbers at period end.

**Does earned value tell me my revenue?**
No, and treating it that way causes real damage. Earned value measures delivery progress at budget rates. Revenue depends on the contract and on the measure of progress the accounting standard requires, commonly costs incurred against total expected costs, which uses actual cost and forecast rather than budget value.

---

*Internal links: this answer should link to [earned value management](https://projectcontrolsinstitute.org/earned-value-management) with the anchor "what earned value management measures and where it fails", to [the earned value formulas cheat sheet](https://projectcontrolsinstitute.org/earned-value-formulas-cheat-sheet) with the anchor "every earned value formula with a worked dataset", and to [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) with the anchor "which estimate at completion to publish, and why". Quora rule: the single in-body link sits after the question is fully answered, never in the opening.*
