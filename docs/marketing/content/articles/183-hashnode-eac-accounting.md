---
platform:      Hashnode
type:          guide
title:         EAC accounting: when a forecast becomes reported profit
meta:          EAC accounting worked in full: how estimate at completion drives measured progress, revenue, margin and provisions, and what an auditor asks for.
primary_kw:    EAC accounting
secondary_kw:  estimate at completion, cost-to-cost, cumulative catch-up, onerous contract provision
pillar:        Cost control and estimating
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> projectcontrolsinstitute.org/eac-accounting
schema:        Article
word_count:    1800
hashtags:      #finance #python #datascience #tutorial
ab_id:         AB-00094
---

# EAC accounting: when a forecast becomes reported profit

EAC accounting is the point where a cost engineer's forecast stops being a management number. Under a cost-to-cost measure of progress, estimate at completion is the denominator of the progress fraction, so moving it changes measured progress, cumulative revenue, reported margin and, past a threshold, forces a loss provision.

No cash moves. No purchase order is signed. A revised spreadsheet cell arrives in the ledger as profit that has to be handed back.

*This describes practice. It is not accounting, tax or legal advice.*

## What the chain actually is

Estimate at completion is the total forecast cost of a contract from inception to handover: cost incurred plus the estimate to complete.

Under an input measure of progress, revenue is recognised in proportion to cost incurred divided by total forecast cost. The forecast is the denominator, so the forecast sets the percentage, and the percentage sets the revenue.

That chain runs: EAC → measured progress → cumulative revenue → period revenue → gross margin → contract asset or liability → any provision required. It is a pure function of three inputs, which is why it is worth writing as one.

```python
def position(price, eac, cost_incurred):
    progress = cost_incurred / eac
    cum_revenue = progress * price
    cum_margin = cum_revenue - cost_incurred
    expected_margin = price - eac
    provision = max(0.0, -expected_margin + cum_margin)
    return progress, cum_revenue, cum_margin, expected_margin, provision
```

## Worked: what a £5m forecast movement does to the accounts

A fixed-price contract of £64.0m. Original forecast cost £52.0m, so an expected margin of £12.0m. Costs incurred to date are £26.0m.

**Position one, EAC £52.0m.** Progress = 26.0 ÷ 52.0 = **50.00%**. Cumulative revenue = 0.5000 × 64.0 = **£32.00m**. Cost recognised £26.0m. Cumulative margin **£6.00m**.

**Position two, EAC moves to £57.0m.** Nothing has been spent that was not spent before. Progress = 26.0 ÷ 57.0 = **45.61%**. Cumulative revenue = 0.4561 × 64.0 = **£29.19m**. Cost recognised is still £26.0m. Cumulative margin **£3.19m**.

Cross-check it: total expected margin is now 64.0 − 57.0 = £7.0m, and 45.61% of £7.0m is £3.19m. Two routes, same answer, which is how you know the entry is right.

So a £5.0m movement in the forecast removed **£2.81m** of previously reported margin in one period. Cumulative revenue also fell, so the period revenue line can be negative while the site is busy.

**Position three, EAC moves to £68.0m.** The contract is now expected to lose 64.0 − 68.0 = £4.0m. Progress = 26.0 ÷ 68.0 = **38.24%**, cumulative revenue = **£24.47m** against costs of £26.0m, so £1.53m of loss is already recognised.

The full expected loss must be taken as soon as it is expected, not spread. The provision is therefore 4.00 − 1.53 = **£2.47m**, booked in the period the forecast crossed the line. That recognition sits under the provisions standard rather than the revenue one, which is a distinction people get wrong in interviews and in practice.

| EAC | Progress | Cumulative revenue | Cumulative margin | Provision |
|---:|---:|---:|---:|---:|
| £52.0m | 50.00% | £32.00m | £6.00m | — |
| £57.0m | 45.61% | £29.19m | £3.19m | — |
| £68.0m | 38.24% | £24.47m | −£1.53m | £2.47m |

## Which EAC method survives an audit

The four forecasting methods are not equally acceptable as the reported number. They differ in what they assume, and an auditor tests the assumption, not the arithmetic.

| Method | Formula | What it assumes | As a reporting EAC |
|---|---|---|---|
| Remaining at budget | AC + (BAC − EV) | The overrun was a one-off and is closed | Only with the cause isolated in the ledger and evidence it ended |
| Remaining at current CPI | BAC ÷ CPI | Performance to date predicts the rest | Usually acceptable once the contract is materially advanced |
| Remaining at CPI × SPI | AC + (BAC − EV) ÷ (CPI × SPI) | Schedule pressure keeps damaging cost | Rarely the reported figure; a stress case for the audit file |
| Bottom-up ETC | AC + a fresh estimate to complete | The remaining work differs in kind from the work done | The preferred basis, if the estimate carries the rigour of the original |

Early in a contract the indices are noisy and a bottom-up estimate to complete is the only honest answer. Late on, the indices are a large sample and the bottom-up estimate carries the optimism.

One test cuts through all four. Compute TCPI against the forecast you propose, (BAC − EV) ÷ (EAC − AC). If it implies an efficiency the contract has never demonstrated, the forecast is a wish with a spreadsheet attached. The four methods are worked on one dataset in [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas).

## Change of estimate, not correction of error

A revised EAC is a change in an accounting estimate. It is recognised in the period the information arrives and in future periods, and prior periods are not restated.

The practical form is the cumulative catch-up: recompute cumulative revenue at the new progress percentage, compare it with revenue already recognised, and book the difference now. Position two above is a catch-up of −£2.81m.

The exception matters. If the previous EAC was wrong because a known cost was omitted or a rate misapplied, that is an error, corrected differently and attracting a different conversation with the auditor. The line between the two is documentary: what was known, and when.

## What the auditor asks for

Four things, and a project that cannot produce them will have its forecast challenged whatever the arithmetic says.

The basis of estimate behind the estimate to complete: quantities, rates, productivity assumptions, and which are contracted rather than assumed. The change log showing every EAC movement in the period with a cause and an owner.

Evidence that commitments, accruals and the forecast reconcile, because committed cost cannot exceed EAC without explanation. And the contingency position, since a forecast drawing down contingency to hold a headline number has already moved.

## How EAC movement lands on each line

| Statement effect | Direction when EAC rises | Note |
|---|---|---|
| Cumulative revenue | Falls | Lower measured progress on the same transaction price |
| Cost of sales | Unchanged in the period | Actual cost is what it is; only revenue recognition moves |
| Gross margin | Falls | The whole movement lands here |
| Contract asset | Falls | Less revenue recognised against unchanged billings |
| Contract liability | Rises | If billings already exceed revenue, the excess grows |
| Provision | Appears once expected margin turns negative | Full expected loss, immediately |

Two second-order effects follow, and they are usually why the finance director calls. Reported margin feeds covenant tests on interest cover and gearing, and a material margin movement in a listed group is disclosable.

## The overlap EAC accounting sits in

A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy. An engineer is examined on progress measurement and float. Almost nobody is examined on both, and the estimate at completion is where the two meet.

The failure that follows is familiar. The cost engineer moves a forecast in week two without knowing it is a reporting event, and the financial controller receives it in week three with no way to test whether the estimate to complete is credible.

The PCI AI Project Finance Leader (PFL-AI) examines that crossing directly across 16 domains and 61 knowledge areas, with the Body of Knowledge weighted 40 per cent finance and reporting, 40 per cent project management and 20 per cent governed AI. The calculation content behind the PFL-AI and PCI Project Management Leader – AI (PML-AI) volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

## Governance that stops a forecast surprising the board

Set an EAC lock date in the close calendar, two working days before the ledger closes, after which a movement needs the project director and the financial controller together.

Require a written cause for every movement above a stated threshold, phrased as an event. "Piling rates re-tendered 18 per cent above estimate" is a cause. "Alignment to latest view" is not.

Reconcile the EAC in the cost report to the EAC in the revenue calculation every month, and record it. When those two diverge, the cost report and the accounts describe different projects.

Trend the forecast rather than the variance. A contract whose EAC has risen every month for five months has a forecasting problem wherever it currently sits, and the trend is visible long before the number is alarming.

## Frequently asked questions

**Is estimate at completion the same as forecast final cost?**
In most organisations yes, provided both mean total cost from inception to completion including accruals and commitments. The confusion arises where a business forecasts only the remaining year, or excludes contingency held centrally. Fix the definition in the cost control procedure before comparing anyone's numbers with anyone else's.

**Why does revenue fall when nothing has been spent?**
Because revenue tracks measured progress, and progress under a cost-to-cost method is cost incurred over total forecast cost. Raising the denominator lowers the fraction, so cumulative revenue falls and the reduction is recognised at once as a catch-up. The accounts restate what has been earned to date at the new margin percentage.

**Does a rising EAC always create a provision?**
No. A provision is required only when the contract is expected to be loss-making overall, and then the whole expected loss is taken immediately rather than spread. Until then a rising forecast simply compresses margin. The step from margin erosion to a provision is a cliff rather than a slope, so a forecast approaching break-even needs the most scrutiny.

**Who owns the EAC, the project or finance?**
The project owns the estimate; finance owns the accounting treatment. That split works only when the project understands the number is a reporting input and finance can interrogate the basis of estimate. Where one side owns both, the forecast drifts towards whatever the reporting cycle needs, which is the failure this discipline exists to prevent.

**How does this relate to earned value?**
Earned value produces the evidence and a cost-to-cost measure consumes it: EV supports the claim that the estimate to complete is credible, and actual cost supplies the numerator of the progress fraction. The wider treatment is in [IFRS for project controls](https://projectcontrolsinstitute.org/ifrs-for-project-controls).

---

*First published on projectcontrolsinstitute.org; the canonical is set through Draft Settings to the original, because the accounting page is the one that should rank.*

*Internal links: this guide should link to [IFRS for project controls](https://projectcontrolsinstitute.org/ifrs-for-project-controls) with that anchor, to [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) with that anchor, and to [project budgeting and forecasting](https://projectcontrolsinstitute.org/project-budgeting-and-forecasting) with the anchor "building and maintaining the control budget".*
