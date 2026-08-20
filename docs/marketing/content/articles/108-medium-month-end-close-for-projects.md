---
platform:      Medium
type:          process-guide
title:         Month-end close for projects: the five working-day run
meta:          A working month-end close for projects: the day-by-day calendar, the accrual arithmetic that decides CPI, five reconciliations, and the finance handover.
primary_kw:    month-end close for projects
secondary_kw:  project accruals, cut-off date, cost report reconciliation, goods received not invoiced
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /month-end-close-for-projects (own site #028)
schema:        HowTo
word_count:    1724
hashtags:      #ProjectControls #CostEngineering #ProjectFinance #EarnedValue #PMO
ab_id:         AB-00141
---

# Month-end close for projects: the five working-day run

A month-end close for projects is the sequence that makes progress, cost and the general ledger agree to one cut-off date. It runs in five moves: freeze, accrue, earn, forecast, reconcile. It fails almost always at the same place, which is earned value measured to a different date from actual cost.

Everything below assumes a calendar month and a five working-day close. Compress or extend the days to suit the business; do not reorder the moves.

## The one rule the whole close depends on

Earned value and actual cost must describe the same instant. If site progress is surveyed on the 28th and the ledger closes on the 31st, three days of cost sit against no work and the cost performance index reads low for a reason that has nothing to do with performance.

Pick the cut-off, write it into the procedure, and apply it to progress, commitments, accruals and the forecast alike. Where a survey genuinely cannot happen on the cut-off date, accrue the gap rather than moving the date.

## The month-end close for projects, day by day

| Day | Move | Owner | Output |
|---|---|---|---|
| WD−3 | Progress survey and quantity verification | Site quantity surveyor | Signed progress claim per control account |
| WD−2 | Commitment cut-off; no new POs coded to the period | Procurement | Frozen commitment register |
| WD−1 | Timesheet close and plant returns | Site administration | Hours and plant by cost code |
| WD+1 | Accrual schedule prepared | Cost engineer | Accruals with supporting evidence |
| WD+2 | Earned value computed; variances explained | Cost engineer | Cost report and variance narratives |
| WD+3 | EAC lock; movements above threshold approved | Project director and financial controller | Forecast change log |
| WD+4 | Ledger close; five reconciliations run | Finance and controls jointly | Signed reconciliation pack |
| WD+5 | Revenue, contract asset and provision test | Financial controller | Journals and the reporting pack |

The two days that get squeezed are WD−3 and WD+3. Squeezing the survey produces unverified progress; squeezing the forecast lock produces a number that changed after the ledger closed. Both are the same failure wearing different clothes.

## Accruals: the arithmetic that decides your CPI

Actual cost is not what the ledger says. It is what the ledger says, adjusted to the work actually performed by the cut-off.

Take a period where the ledger shows **£6.480m** posted to the project. Three adjustments apply.

Goods receipted and not invoiced, **£0.510m**: the steel is on site and consumed, and the invoice has not arrived, so add it. Subcontract work performed and not yet certified, **£0.365m**: the labour was expended in the period, so add it. Prepaid site insurance of **£0.075m** relating to next period: reverse it.

Corrected actual cost = 6.480 + 0.510 + 0.365 − 0.075 = **£7.280m**.

Earned value for the period is **£7.150m**. Corrected CPI = 7.150 ÷ 7.280 = **0.982**. Against the raw ledger it would have read 7.150 ÷ 6.480 = **1.103**.

Twelve points of CPI, produced by accrual discipline alone. The error runs in the flattering direction by default, because unbilled work performed is always the largest of the three adjustments.

## The materials trap

A separate **£0.240m** of cable is delivered to site and not installed. It correctly sits in actual cost, and it must not be earned.

Earn it and earned value becomes £7.390m, giving 7.390 ÷ 7.280 = **1.015**: a package apparently outperforming budget on the strength of a delivery note.

The same principle applies in the accounts, where cost incurred that is not proportionate to progress is excluded from the progress measure and the material is recognised at cost with no margin until installed. The controls rule and the accounting rule agree here, which is unusual and worth using. Align the earning rule with the revenue policy and one argument disappears from every close.

## Five reconciliations that must tie

| Reconciliation | Tie between | Tolerance | When it breaks |
|---|---|---|---|
| Commitment | PO register and ledger plus open commitment | Nil | Late PO coding, or a PO raised after cut-off |
| Actual cost | Ledger plus accruals, and cost report AC | Nil | An accrual booked in one system only |
| Earned value | Signed progress claim and EV in the report | Nil | Progress claimed after the survey was signed |
| Forecast | Cost report EAC and EAC used for revenue | Nil | The forecast moved after WD+3 |
| Contract position | Cumulative revenue less billings, and the contract asset or liability | Nil | Applications issued outside the billing cycle |

Every one is a nil-tolerance tie, and that is deliberate. A tolerance on a reconciliation becomes a hiding place within two months.

Where a difference cannot be resolved inside the close, book it, name it, and put it on next month's action list with an owner. An unexplained difference carried quietly is how a £4m surprise arrives in month eleven.

## Segregation: who claims and who verifies

The person who claims progress must not be the person who verifies it. On most sites the supervisor claims, the quantity surveyor verifies, and the cost engineer accepts only verified quantities into earned value.

Where a package is measured by assessed percentage rather than units, require a second signature and a stated basis. Assessed percentages drift upward under schedule pressure, and the drift is invisible in the report because the number looks exactly like a measured one.

Level of effort deserves the same scrutiny once a year rather than every month. Ask what share of the baseline sits on it and whether discrete work has quietly migrated there, because level of effort earns with time and therefore reports a cost performance index near 1.00 whatever happens on site.

## What to hand finance at WD+4

A package, not a spreadsheet emailed at midnight.

It contains actual cost by control account with the accrual schedule and its evidence, earned value with the signed progress claims behind it, and the forecast with a change log for every movement in the period.

It also contains the contingency position, the commitment register, and a short note on any control account breaching the variance threshold. Anything the financial controller has to ask for is something the close did not produce.

The forecast is the item carrying reporting consequences. Estimate at completion is the denominator of the progress fraction under a cost-to-cost measure, so moving it moves recognised revenue and margin in the same period.

## Where the close usually goes wrong

| Symptom | Actual cause | Fix |
|---|---|---|
| CPI improves every month and falls in December | Accruals understated all year and trued up at audit | Accrue against the commitment register, not against invoices received |
| Progress claimed then reversed | Survey done before the work was complete | Move the survey to WD−3 and require verification |
| Forecast changes after reporting | No EAC lock date | Lock at WD+3 with two named approvers |
| Cost report and accounts disagree | Two EAC figures in circulation | One forecast, reconciled monthly, recorded in the pack |
| The close takes nine days | Sequential handovers | Run accruals and progress in parallel; only the forecast is genuinely sequential |

The first row is the one that ends careers. A slow, steady understatement of accruals produces a cost report that looks good every month and a year-end that does not, and by then the forecast has been wrong for four quarters.

## The overlap this sits in

A financial controller is examined on cut-off, accruals and provisions, but rarely on progress measurement. A cost engineer is examined on progress measurement, but rarely on cut-off.

The close is the fortnight where both are required at once, and neither profession's examination covers the other side. That is the gap the PCI AI Project Finance Leader (PFL-AI) credential is built on, across 16 domains and 61 knowledge areas, with a Body of Knowledge weighted 40% finance and reporting, 40% project management and 20% governed AI.

The PCI Standards behind the credentials carry 113 mandatory standards and 532 process requirements, which is where close discipline of this kind is written down rather than left to habit.

## Frequently asked questions

**How long should a project month-end close take?**
Five working days is achievable on a single contract with clean commitment data, and seven to eight is normal in a multi-project business consolidating several ledgers. The constraint is rarely computation. It is waiting for verified progress and subcontractor valuations, so the improvements that work are moving the survey earlier and accruing rather than chasing.

**Should the cost report or the ledger be the source of truth?**
The ledger is the source of truth for cost incurred; the cost report is the source of truth for what that cost bought. They are reconciled, not ranked. When the cost report shows a figure the ledger cannot support, the difference is an accrual, a coding error or a timing issue, and naming which takes minutes if the reconciliation runs monthly.

**What evidence supports an accrual?**
A goods receipt note, a signed timesheet, a subcontractor's application, a delivery record, or a measured quantity with a rate against it. What does not support one is a percentage of last month. Accruals estimated as a proportion of the previous period are the mechanism by which a project drifts, because they reproduce the previous month's error exactly.

**Do small projects need this?**
The sequence yes, the calendar scaled. On a £2m project the survey, accruals and forecast can happen in one afternoon, and the five reconciliations still apply because their cost is minutes. What does not scale down is the segregation between claiming and verifying progress, which matters more on small teams rather than less.

**How does the close connect to earned value reporting?**
The close produces the inputs and earned value interprets them. Handle cut-off, accruals and verified quantities properly and the indices mean something. Handle them loosely and no amount of analysis rescues the report, because every index inherits the error in actual cost.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so treat this republish as distribution rather than as a backlink.*

*Internal links: this piece should link to [how the estimate at completion reaches the accounts](https://projectcontrolsinstitute.org/eac-accounting) with that anchor, to [the earned value management pillar](https://projectcontrolsinstitute.org/earned-value-management) with that anchor, and to [IFRS for project controls](https://projectcontrolsinstitute.org/ifrs-for-project-controls) with that anchor.*
