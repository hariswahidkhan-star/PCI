---
platform:      Own site — projectcontrolsinstitute.org
type:          guide
title:         EAC accounting: how a forecast becomes reported profit
meta:          EAC accounting, worked in full: how estimate at completion drives measured progress, revenue, margin and provisions, and what an auditor asks for.
primary_kw:    EAC accounting
secondary_kw:  estimate at completion, cost-to-cost, cumulative catch-up, onerous contract provision
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,723
hashtags:      n/a (own site)
ab_id:         AB-00094
---

# EAC accounting: how a forecast becomes reported profit

EAC accounting is the point where a cost engineer's forecast stops being a management number. Under the cost-to-cost method, estimate at completion is the denominator of the progress fraction, so moving it changes measured progress, cumulative revenue, reported margin and, past a threshold, forces a loss provision.

No cash moves. No purchase order is signed. A revised spreadsheet cell arrives in the ledger as profit that has to be taken back.

## What EAC accounting actually means

Estimate at completion is the total forecast cost of a contract from inception to handover: cost incurred plus the estimate to complete.

Under an input measure of progress, revenue is recognised in proportion to cost incurred divided by total forecast cost. The forecast is the denominator, so the forecast sets the percentage, and the percentage sets the revenue.

That single chain is why finance departments care about a number produced in a cost report. It runs: EAC → measured progress → cumulative revenue → period revenue → gross margin → contract asset or liability → any provision required.

## Worked: what a £5m forecast movement does to the accounts

A fixed-price contract of £64.0m. Original forecast cost £52.0m, so an expected margin of £12.0m. Costs incurred to date are £26.0m.

**Position one, EAC £52.0m.** Progress = 26.0 / 52.0 = **50.00%**. Cumulative revenue = 0.5000 × 64.0 = **£32.00m**. Cost recognised £26.0m. Cumulative margin **£6.00m**.

**Position two, EAC moves to £57.0m.** Nothing has been spent that was not spent before. Progress = 26.0 / 57.0 = **45.61%**. Cumulative revenue = 0.4561 × 64.0 = **£29.19m**. Cost recognised is still £26.0m. Cumulative margin **£3.19m**.

The cross-check: total expected margin is now 64.0 − 57.0 = £7.0m, and 45.61% of £7.0m is £3.19m. The two routes agree, which is how you know the entry is right.

So a £5.0m movement in the forecast removed **£2.81m** of previously reported margin in one period. Revenue also fell in cumulative terms, which means the period revenue line can be negative even while the site is busy.

**Position three, EAC moves to £68.0m.** The contract is now expected to lose 64.0 − 68.0 = £4.0m. Progress = 26.0 / 68.0 = **38.24%**, cumulative revenue = **£24.47m** against costs of £26.0m, a cumulative loss of £1.53m already recognised.

The full expected loss must be taken as soon as it is expected, not spread. The provision is therefore 4.00 − 1.53 = **£2.47m**, booked in the period the forecast crossed the line. The loss recognition sits under the provisions standard, not the revenue one, which is a distinction people get wrong in interviews and in practice.

## Which EAC method survives an audit

The four forecasting methods are not equally acceptable as the reporting number. They differ in what they assume, and an auditor tests the assumption, not the arithmetic.

| Method | Formula | What it assumes | As a reporting EAC |
|---|---|---|---|
| Remaining at budget | AC + (BAC − EV) | The overrun was a one-off and is closed | Only with the cause isolated in the ledger and evidence it ended |
| Remaining at current CPI | BAC / CPI | Performance to date predicts the rest | Usually acceptable once the contract is materially advanced |
| Remaining at CPI × SPI | AC + (BAC − EV) / (CPI × SPI) | Schedule pressure keeps damaging cost | Rarely the reported figure; a stress case for the audit file |
| Bottom-up ETC | AC + a fresh estimate to complete | The remaining work differs in kind from the work done | The preferred basis, if the estimate carries the same rigour as the original |

Early in a contract the indices are noisy and a bottom-up estimate to complete is the only honest answer. Late in a contract the indices are a large sample and the bottom-up estimate is the one carrying optimism.

One test cuts through all four. Compute TCPI against the forecast you propose: (BAC − EV) / (EAC − AC). If it implies efficiency the contract has never demonstrated, the forecast is a wish with a spreadsheet attached.

## Change of estimate, not correction of error

A revised EAC is a change in an accounting estimate. It is recognised in the period the information arrives and in future periods, and prior periods are not restated.

The practical form of that is the cumulative catch-up: recompute cumulative revenue at the new progress percentage, compare it with cumulative revenue already recognised, and book the difference now. Position two above is a catch-up of −£2.81m.

The exception matters. If the previous EAC was wrong because a known cost was deliberately omitted or a rate was misapplied, that is an error, and errors are corrected differently and attract a very different conversation with the auditor. The line between the two is documentary: what was known, and when.

## What the auditor asks for

Four things, in this order, and a project that cannot produce them will have its forecast challenged whatever the arithmetic says.

The basis of estimate for the estimate to complete: quantities, rates, productivity assumptions, and which are contracted versus assumed. The change log showing every EAC movement in the period with a cause and an owner.

Evidence that commitments, accruals and the forecast reconcile: committed cost cannot exceed EAC without an explanation. And the contingency position, because a forecast that draws down contingency to hold a headline number is a forecast that has already moved.

## How EAC movement lands on each line

| Financial statement effect | Direction when EAC rises | Note |
|---|---|---|
| Cumulative revenue | Falls | Lower measured progress on the same transaction price |
| Cost of sales | Unchanged in the period | Actual cost is what it is; only recognition of revenue moves |
| Gross margin | Falls | The whole movement lands here |
| Contract asset | Falls | Less revenue recognised against unchanged billings |
| Contract liability | Rises | If billings already exceed revenue, the excess grows |
| Provision | Appears once expected margin turns negative | Full expected loss, immediately |

Two second-order effects follow and are usually the reason the finance director calls. Reported margin feeds covenant tests on interest cover and gearing, and a margin movement in a listed group is a disclosable event if it is material.

## The overlap this sits in

A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy. An engineer is examined on progress measurement and float. Almost nobody is examined on both, and the EAC is precisely where the two meet.

The result is a familiar failure. The cost engineer moves a forecast in week two of the month without knowing it is a reporting event; the financial controller receives it in week three and has no way to test whether the estimate to complete is credible.

The PCI AI Project Finance Leader (PFL-AI) credential examines that overlap directly, across 16 domains and 61 knowledge areas, with the Body of Knowledge weighted 40% finance and reporting, 40% project management and 20% governed AI. Every calculation in the PFL-AI and PCI Project Management Leader – AI (PML-AI) materials is machine-checked, 15,613 checks all passing, and that suite covers PFL-AI and PML-AI only.

## Governance that stops a forecast surprising the board

Set an EAC lock date in the close calendar, two working days before the ledger closes, after which a movement needs the project director and the financial controller together.

Require a written cause for every movement above a stated threshold, phrased as an event rather than an adjustment. "Piling rates re-tendered 18% above estimate" is a cause. "Alignment to latest view" is not.

Reconcile the EAC used in the cost report to the EAC used in the revenue calculation every month and record the reconciliation. When those two diverge, the cost report and the accounts are describing different projects.

Trend the forecast rather than the variance. A contract whose EAC has risen every month for five months has a forecasting problem regardless of where it currently sits, and the trend is visible long before the number is alarming.

## Frequently asked questions

**Is estimate at completion the same as forecast final cost?**
In most organisations, yes, provided both mean total cost from inception to completion including accruals and committed cost. The confusion arises where a business uses a forecast that covers only the remaining year, or excludes contingency held centrally. Fix the definition in the cost control procedure before comparing anyone's numbers to anyone else's.

**Why does revenue fall when nothing has been spent?**
Because revenue tracks measured progress, and progress under a cost-to-cost method is cost incurred over total forecast cost. Raising the denominator lowers the fraction, so cumulative revenue falls and the reduction is recognised at once as a catch-up. The margin percentage on the whole contract has changed, and the accounts restate what has been earned to date at that new percentage.

**Does a rising EAC always create a provision?**
No. A provision is required only when the contract is expected to be loss-making overall, and then the whole expected loss is taken immediately rather than spread. Until that point a rising forecast simply compresses margin. The step from margin erosion to a provision is a cliff, not a slope, which is why the forecast that approaches break-even needs the most scrutiny.

**Who owns the EAC, the project or finance?**
The project owns the estimate; finance owns the accounting treatment of it. That split works only when the project understands that the number is a reporting input and finance can interrogate the basis of estimate. Where one side owns both, the forecast tends to drift towards whatever the reporting cycle needs, which is the failure this discipline exists to prevent.

**How does this relate to earned value?**
Earned value produces the evidence for the estimate, and a cost-to-cost measure of progress consumes it: EV supports the claim that the estimate to complete is credible, and AC supplies the numerator. The wider accounting treatment is set out in [IFRS for project controls](https://projectcontrolsinstitute.org/ifrs-for-project-controls), and the four forecasting methods are worked in full in [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas).

---

*Internal links: this guide should link to [IFRS for project controls](https://projectcontrolsinstitute.org/ifrs-for-project-controls) with that anchor, to [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) with that anchor, and to [project budgeting and forecasting](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting) with that anchor; the month-end close and IFRS 15 for construction pieces should link back here with the anchor "how the estimate at completion reaches the accounts".*
