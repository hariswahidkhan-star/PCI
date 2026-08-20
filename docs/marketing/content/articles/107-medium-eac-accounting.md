---
platform:      Medium
type:          guide
title:         EAC accounting: the forecast that moves reported profit
meta:          EAC accounting worked in full: how estimate at completion sets measured progress, revenue and margin, when it forces a provision, and what auditors test.
primary_kw:    EAC accounting
secondary_kw:  estimate at completion, cost-to-cost, cumulative catch-up, onerous contract provision
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /eac-accounting (own site #027)
schema:        Article
word_count:    1704
hashtags:      #ProjectControls #ProjectFinance #CostEngineering #EarnedValue #ConstructionAccounting
ab_id:         AB-00094
---

# EAC accounting: the forecast that moves reported profit

EAC accounting is the point where a cost engineer's forecast stops being a management number. Under a cost-to-cost measure of progress, estimate at completion is the denominator of the progress fraction, so moving it changes measured progress, cumulative revenue, reported margin and, past a threshold, forces a loss provision.

No cash moves. No purchase order is signed. A revised cell in a spreadsheet arrives in the ledger as profit that has to be handed back.

## What EAC accounting actually means

Estimate at completion is the total forecast cost of a contract from inception to handover: cost incurred plus the estimate to complete.

Under an input measure of progress, revenue is recognised in proportion to cost incurred divided by total forecast cost. The forecast is the denominator, so the forecast sets the percentage and the percentage sets the revenue.

The chain runs: EAC → measured progress → cumulative revenue → period revenue → gross margin → contract asset or liability → any provision required. Six links, one of which sits in a cost report that finance rarely reads in detail.

## Worked: what a £5m forecast movement does to the accounts

A fixed-price contract of **£64.0m**. Original forecast cost **£52.0m**, so an expected margin of £12.0m. Costs incurred to date are **£26.0m**.

**Position one, EAC £52.0m.** Progress = 26.0 ÷ 52.0 = **50.00%**. Cumulative revenue = 0.5000 × 64.0 = **£32.00m**. Cost recognised £26.0m. Cumulative margin **£6.00m**.

**Position two, EAC moves to £57.0m.** Nothing has been spent that was not spent before. Progress = 26.0 ÷ 57.0 = **45.61%**. Cumulative revenue = 0.4561 × 64.0 = **£29.19m**. Cost recognised is still £26.0m, so cumulative margin is **£3.19m**.

The cross-check: total expected margin is now 64.0 − 57.0 = £7.0m, and 45.61% of £7.0m is £3.19m. Two routes, same answer, which is how you know the entry is right.

A £5.0m movement in the forecast therefore removed **£2.81m** of previously reported margin in a single period. Cumulative revenue also fell, which means the period revenue line can print negative while the site is at its busiest.

**Position three, EAC moves to £68.0m.** The contract is now expected to lose 64.0 − 68.0 = **£4.0m**. Progress = 26.0 ÷ 68.0 = **38.24%**, cumulative revenue = **£24.47m** against costs of £26.0m, so £1.53m of loss is already recognised through normal measurement.

The full expected loss must be taken as soon as it is expected rather than spread. The provision is 4.00 − 1.53 = **£2.47m**, booked in the period the forecast crossed the line. That step sits under the provisions standard, IAS 37, not the revenue standard, and people get it wrong in interviews and in practice.

## Which EAC method survives an audit

The four forecasting methods are not equally acceptable as the reporting number, because they assume different things about the work still to come. An auditor tests the assumption, not the arithmetic.

| Method | Formula | What it assumes | As a reporting EAC |
|---|---|---|---|
| Remaining at budget | AC + (BAC − EV) | The overrun was a one-off and has closed | Only with the cause isolated in the ledger and evidence it ended |
| Remaining at current CPI | BAC ÷ CPI | Performance to date predicts the rest | Usually acceptable once the contract is materially advanced |
| Remaining at CPI × SPI | AC + (BAC − EV) ÷ (CPI × SPI) | Schedule pressure keeps damaging cost | Rarely the reported figure; a stress case for the audit file |
| Bottom-up ETC | AC + a fresh estimate to complete | The remaining work differs in kind from the work done | The preferred basis, if it carries the rigour of the original estimate |

Early in a contract the indices are a small and noisy sample, and a bottom-up estimate to complete is the only honest answer. Late in a contract the indices are a large sample and the bottom-up estimate is the one carrying the optimism.

One test cuts across all four. Compute the to-complete performance index against the forecast you propose: TCPI = (BAC − EV) ÷ (EAC − AC). If it implies an efficiency the contract has never demonstrated, the forecast is a wish with a spreadsheet attached.

## Change of estimate, not correction of error

A revised EAC is a change in an accounting estimate. It is recognised in the period the information arrives and in future periods, and prior periods are not restated.

The practical form is the cumulative catch-up: recompute cumulative revenue at the new progress percentage, compare it with cumulative revenue already recognised, and book the difference now. Position two above is a catch-up of −£2.81m.

The exception matters. If the previous EAC was wrong because a known cost was deliberately omitted or a rate was misapplied, that is an error, corrected differently and attracting a very different conversation. The line between the two is documentary: what was known, and when.

## How an EAC movement lands on each line

| Financial statement effect | Direction when EAC rises | Note |
|---|---|---|
| Cumulative revenue | Falls | Lower measured progress on the same transaction price |
| Cost of sales | Unchanged in the period | Actual cost is what it is; only recognition moves |
| Gross margin | Falls | The whole movement lands here |
| Contract asset | Falls | Less revenue recognised against unchanged billings |
| Contract liability | Rises | Where billings already exceed revenue, the excess grows |
| Provision | Appears once expected margin turns negative | The full expected loss, immediately |

Two second-order effects follow, and they are usually why the finance director rings. Reported margin feeds covenant tests on interest cover and gearing, and in a listed group a material margin movement can be a disclosable event.

## What the auditor asks for

The basis of estimate behind the estimate to complete: quantities, rates, productivity assumptions, and which of them are contracted rather than assumed.

The change log showing every EAC movement in the period with a cause and a named owner.

Evidence that commitments, accruals and the forecast reconcile, because committed cost cannot exceed EAC without an explanation.

The contingency position, because a forecast that quietly draws down contingency to hold a headline number is a forecast that has already moved.

## Governance that stops a forecast surprising the board

Set an EAC lock date in the close calendar, two working days before the ledger closes, after which a movement needs the project director and the financial controller together.

Require a written cause for every movement above a stated threshold, phrased as an event rather than an adjustment. "Piling rates re-tendered 18% above estimate" is a cause. "Alignment to latest view" is not.

Reconcile the EAC in the cost report to the EAC used in the revenue calculation every month, and record it. When those two diverge, the cost report and the accounts are describing different projects.

Trend the forecast rather than the variance. A contract whose EAC has risen every month for five months has a forecasting problem wherever it currently sits, and the trend is visible long before the number is alarming.

## The overlap this sits in

A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy. An engineer is examined on progress measurement and float. Almost nobody is examined on both, and the estimate at completion is exactly where the two meet.

The familiar failure follows from that. The cost engineer moves a forecast in week two of the month without knowing it is a reporting event, and the financial controller receives it in week three with no way to test whether the estimate to complete is credible.

The PCI AI Project Finance Leader (PFL-AI) credential examines that overlap directly, across 16 domains and 61 knowledge areas, with a Body of Knowledge weighted 40% finance and reporting, 40% project management and 20% governed AI. The calculations in the PFL-AI and PCI Project Management Leader – AI (PML-AI) materials are machine-checked, 15,613 checks all passing, and that suite covers PFL-AI and PML-AI only.

## Frequently asked questions

**Is estimate at completion the same as forecast final cost?**
In most organisations yes, provided both mean total cost from inception to completion including accruals and committed cost. Confusion arises where a business uses a forecast covering only the remaining year, or excludes contingency held centrally. Fix the definition in the cost control procedure before comparing anyone's number to anyone else's.

**Why does revenue fall when nothing has been spent?**
Because revenue tracks measured progress, and progress under cost-to-cost is cost incurred over total forecast cost. Raising the denominator lowers the fraction, so cumulative revenue falls and the reduction is booked at once as a catch-up. The margin percentage for the whole contract has changed, and the accounts restate what has been earned so far at that new percentage.

**Does a rising EAC always create a provision?**
No. A provision is required only when the contract is expected to be loss-making overall, and then the whole expected loss is taken immediately rather than spread. Until that point a rising forecast simply compresses margin. The step from margin erosion to a provision is a cliff rather than a slope, which is why a forecast approaching break-even needs the most scrutiny.

**Who owns the EAC, the project or finance?**
The project owns the estimate; finance owns the accounting treatment of it. That split works only when the project understands the number is a reporting input and finance can interrogate the basis of estimate. Where one side owns both, the forecast drifts towards whatever the reporting cycle needs, which is the failure this discipline exists to prevent.

**How does this relate to earned value?**
Earned value produces the evidence for the estimate and a cost-to-cost measure consumes it. EV supports the claim that the estimate to complete is credible, and actual cost supplies the numerator of the progress fraction. If the earned value inputs are unreliable, the revenue number is unreliable in exactly the same proportion.

---

*PCI publishes certification requirements and does not provide accounting, legal or tax advice. The standards named here are described in the Institute's own words rather than reproduced.*

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so this republish is here for readers rather than for link equity.*

*Internal links: this piece should link to [IFRS for project controls](https://projectcontrolsinstitute.org/ifrs-for-project-controls) with that anchor, to [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) with that anchor, and to [month-end close for projects](https://projectcontrolsinstitute.org/month-end-close-for-projects) with the anchor "the close that produces these inputs".*
