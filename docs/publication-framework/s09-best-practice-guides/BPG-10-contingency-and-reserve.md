---
id: BPG-10
series: S09
series_name: Best Practice Guides
title: Contingency and management reserve
subtitle: Two reserves, two owners, two entirely different governance events
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, executive]
level: practitioner
reading_time_min: 17
summary: >
  Contingency and management reserve are not two sizes of the same pot. This guide sets out what each
  covers, who owns it, where it sits relative to the cost baseline, how contingency is derived from and
  kept traceable to the risk register, how a drawdown is approved and recorded without moving the budget
  at completion, and how to read the drawdown curve as an early-warning indicator months before the
  forecast catches up.
linkedin:
  format: article
  hook: >
    Thirty-six per cent of contingency drawn at forty per cent complete looks healthy — and can still be a
    quarter of a million short, because the test is remaining contingency against remaining exposure, not
    against progress.
  tags: [ProjectControls, RiskManagement, Contingency, CostEngineering]
  asset: one-pager
gated: false
related: [BPG-04, BPG-09, BPG-16, BPG-17, TPL-10]
bok_domains: [3, 12]
sources: []
placeholders: 0
---

# Contingency and management reserve

> Two reserves, two owners, two entirely different governance events.

**In one paragraph.** Contingency and management reserve are not two sizes of the same pot. This guide sets
out what each covers, who owns it, where it sits relative to the cost baseline, how contingency is derived
from and kept traceable to the risk register, how a drawdown is approved and recorded without moving the
budget at completion, and how to read the drawdown curve as an early-warning indicator months before the
forecast catches up.

**Who this is for.** Project controls managers, cost engineers, risk managers and project sponsors — and
anyone who has been asked "how much contingency is left" and found that the honest answer took a week to
assemble.

---

## 1. Two reserves answer two different questions

The distinction is not about size or seniority. It is about what kind of uncertainty each reserve is funding.

**Contingency reserve** funds **identified** risk — the entries in the risk register, plus quantified
estimating uncertainty. Someone has written the risk down, assessed it and priced it. Contingency sits
**inside** the cost baseline, is controlled by the project, and is drawn down as those identified risks
materialise.

**Management reserve** funds **unidentified** risk — the events nobody wrote down, and scope not yet
foreseen. It sits **outside** the cost baseline, is controlled by the sponsor or the governing body, and its
release is a change to the baseline rather than a movement within it.

The budget structure follows directly:

```
Cost baseline (BAC) = Σ control account budgets + contingency reserve
Total authorised budget = BAC + management reserve
```

Everything that matters operationally falls out of that arithmetic. Earned value performance is measured
against BAC, so contingency is inside the measurement and management reserve is not. Drawing contingency
does not change BAC. Releasing management reserve does.

Note also what the two reserves are *not*. Neither is an accounting provision. How reserves, provisions and
onerous-contract positions are recognised in financial statements is governed by the applicable reporting
framework and varies; a project budget reserve is a control instrument, and the accounting treatment is a
separate question for the finance function.

## 2. Where contingency comes from

Contingency has one legitimate parent: **analysed risk**. A percentage applied to the estimate is not
contingency, it is a habit. The moment anyone asks "what is this covering", a percentage has no answer, and
the reserve becomes indefensible in exactly the meeting where it needs defending.

Two derivations are standard, and they answer different questions.

**Expected monetary value (EMV)** sums `probability × impact` across the register. It gives the *mean*
outcome — the amount you would need if this project were run many times. It is simple, transparent and
traceable to individual risks, and it systematically underfunds any single project, because a single project
does not experience the mean.

**A probabilistic P-level**, typically from a Monte Carlo simulation of the register, gives the amount at
which the project has a stated confidence of not overrunning — commonly P80, meaning an 80 % chance the
outturn falls at or below the funded level. It captures how risks combine, including the effect of shared
drivers, which the EMV sum cannot.

The gap between the two is the price of confidence, and stating it is good practice: *"the mean exposure is
1.74 million; funding to P80 costs 2.60 million; the 0.86 million difference buys the organisation's stated
risk appetite."* That sentence turns a reserve from a number into a decision the sponsor has actually taken.

Correlation matters here more than most registers admit. Risks driven by a single shared cause — one
supplier, one permitting authority, one commodity — do not behave independently, and treating them as
independent understates the tail without changing the mean. Quantification method belongs to
`BPG-17 — Quantitative schedule risk analysis` and the register itself to `BPG-16 — Risk registers that
work`; what this guide insists on is the link: **every unit of contingency should be traceable to a risk
identifier.**

## 3. Traceability, and why it is the whole control

A contingency line that cannot be decomposed into risks is a slush fund, and it will be spent like one.
Traceability delivers four things that no aggregate figure can:

1. **A drawdown test.** A request to draw contingency is answerable — does it correspond to a risk in the
   register, and did that risk occur? If not, the cost is an overrun, and calling it a contingency draw
   conceals a performance problem inside a risk provision.
2. **A release mechanism.** When a risk passes without occurring, its contingency can be released back to
   the pot or to the sponsor, deliberately and visibly.
3. **A re-assessment path.** When a risk's probability or impact changes, the required contingency changes
   with it, and the reserve is re-derived rather than argued about.
4. **A defence.** "This is what it covers, risk by risk" is the only answer that survives an assurance
   review.

The practical form is unglamorous: a contingency register keyed to risk identifiers, showing for each risk
the assessed exposure, the amount allocated, the amount drawn, the residual, and the status. It is one extra
column set on the risk register rather than a separate document, and it should reconcile to the total
contingency figure in the budget every month.

## 4. Holding it, and drawing it down

**Hold contingency centrally.** Contingency distributed into control account budgets stops being
contingency; it becomes budget, and budget is always eventually consumed. The control account manager who
has 60,000 of "risk allowance" inside a 900,000 package has no incentive to hand it back and no mechanism to
do so. Hold the reserve at project level and release it against evidence.

**Define the drawdown mechanism before the first draw.** It needs four elements:

- **A trigger** — the risk has occurred, or has become certain to occur.
- **Evidence** — what happened, which risk identifier it corresponds to, and the cost of the consequence.
- **An approval level** — thresholds by value, with the sponsor's involvement rising with the amount.
- **A record** — a numbered drawdown entry, posted through the change log so the movement is auditable.

**Record the draw as a transfer, not a variance.** This is the mechanical point most often muddled. When
contingency is drawn, budget moves *within* the baseline: the contingency pot decreases and a control
account budget increases by the same amount. BAC is unchanged. Planned value for the affected account
increases from the point of transfer forward, so the account's future earned value measurement is against
the revised budget. Nothing about the past is restated. If your process instead increases BAC when
contingency is drawn, contingency was never inside the baseline, and every historical index is
irreconcilable.

## 5. The drawdown curve as an early-warning indicator

Plot cumulative contingency drawn against time, with two comparators on the same axes: physical progress
(percent complete by earned value) and, separately, the remaining assessed exposure. The shape tells you
things the cost report will not say for another two quarters.

**Healthy.** Drawdown tracks the retirement of risk. Contingency falls as risks either occur (drawn) or pass
(released), and the remaining reserve stays at or above the remaining exposure throughout.

**Front-loaded.** Contingency consumed rapidly in the first third. Either the early risks were the severe
ones — legitimate, and the register should show it — or contingency is subsidising ordinary estimating
error, which will continue. A front-loaded curve on a project whose register still carries its major risks
is one of the strongest early-warning signals in project controls, because it means the reserve will be gone
before the exposure is.

**Flat then cliff.** Nothing drawn for months, then a large single draw. Almost always a reporting failure
rather than a risk profile: costs were absorbed into control accounts and only escalated when they became
impossible to absorb. Look for the corresponding CPI deterioration in the preceding periods.

**Never drawn.** Either the risks are not materialising — say so, and consider releasing reserve — or draws
are being avoided because the approval process is painful, in which case the same costs are hiding inside
control accounts and the contingency figure is meaningless.

**The test that matters is not the burn ratio.** Comparing "percentage of contingency drawn" with
"percentage complete" is the intuitive check and it is close to useless, because contingency is not consumed
in proportion to progress; it is consumed in proportion to *risk occurrence*, which is lumpy. The
defensible test is:

```
Remaining contingency  ≥  Remaining exposure (at the same confidence level as it was set)
```

Run it every month. §8 shows a project that passes the burn-ratio test comfortably and fails this one.

## 6. Management reserve: release is a baseline change

Management reserve is used when something occurs that the register did not and reasonably could not
contain — a new statutory requirement, a client-directed change of a kind nobody anticipated, a systemic
event. Three consequences follow, and all three are frequently missed.

**It is a re-baselining event.** Releasing reserve into the baseline increases BAC. Every earned value
measure computed against the old BAC is now on a different basis, and the change must be logged with its
date, so a reader of the trend knows where the discontinuity is. `BPG-04 — Baselining and baseline change
control` owns the procedure.

**It is a governance decision, not a controls decision.** The project may request; the sponsor or governing
body releases. A project that can draw management reserve on its own authority does not have a management
reserve.

**It is not a solution to an exhausted contingency.** When the register has outgrown the reserve, the
correct response is to re-derive contingency from the current register, show the shortfall, and ask for a
decision — funding it, de-scoping, or accepting the exposure. Silently drawing management reserve to cover
identified risks converts a visible funding gap into an invisible one, and destroys the distinction that
makes either reserve meaningful.

## 7. Schedule contingency, briefly

The same logic applies in time. Schedule contingency is the float deliberately held against identified
schedule risk, owned by the project and drawn down as risks occur; it is not the incidental float that falls
out of the network, and it should be visible as a named buffer rather than hidden in inflated durations.
Padding individual activities is the schedule equivalent of distributing contingency into control accounts:
the protection is consumed silently and cannot be managed. Quantification of schedule risk is the subject of
`BPG-17 — Quantitative schedule risk analysis`.

Where the two interact — a delay that draws both time and time-related cost — take care not to fund the same
event twice, once as schedule contingency and again as cost contingency for prolongation. Price it once, in
the register, against one risk identifier.

## 8. How this goes wrong

**Contingency set as a percentage.** Ten per cent, because it has always been ten per cent. It is
undefendable, it bears no relation to this project's risk profile, and it is the first thing an assurance
reviewer will ask about.

**Contingency distributed into control accounts.** It is spent. Always.

**Drawdown without a risk identifier.** Ordinary overruns re-labelled as risk events. The reserve depletes,
the CPI looks better than it should, and the register never changes — a combination that should trigger
immediate investigation.

**Double-counting risk and escalation.** A commodity price risk priced in the register *and* covered by an
escalation allowance in the estimate. Both are legitimate instruments; funding the same exposure twice is
not, and it inflates the reserve in a way that is easy to attack.

**Treating unspent contingency as a saving.** At completion, contingency not drawn should be explained, not
celebrated. Either the register overstated the exposure, which is an estimating lesson worth capturing, or
the risks genuinely did not materialise, which is luck worth distinguishing from performance.

**Reporting the pot without the exposure.** "Contingency remaining: 1.65 million" is not information. "1.65
million remaining against 1.90 million of assessed remaining exposure" is.

**Using contingency to fund scope.** Scope growth is a change, and it belongs in the change control process
with a decision about who pays. Absorbing it in contingency hides the growth and defunds the risks the
reserve exists for.

**No release discipline.** Risks pass, and their contingency stays in the pot without being re-assessed.
Over time the reserve stops corresponding to the register in either direction, and nobody trusts the figure.

## 9. Worked example

*Illustrative figures.* Currency USD; a 20-month project; risk impacts are the assessed cost consequence
should the risk occur; the confidence level for contingency is P80, taken from a Monte Carlo model of the
register; all figures at the stated data dates.

**At sanction — assembling the budget.**

| Element | Amount |
|---|---:|
| Σ control account budgets | 24,000,000 |
| Contingency reserve (set at P80) | 2,600,000 |
| **Budget at completion (BAC)** | **26,600,000** |
| Management reserve (sponsor-held, outside baseline) | 1,400,000 |
| **Total authorised budget** | **28,000,000** |

**Deriving the contingency.** The register carries five priced risks:

| Risk | Description | Probability | Impact | `EMV = P × I` |
|---|---|---:|---:|---:|
| R-01 | Consent granted later than programmed | 0.40 | 900,000 | `0.40 × 900,000 = 360,000` |
| R-02 | Ground conditions worse than survey | 0.30 | 1,500,000 | `0.30 × 1,500,000 = 450,000` |
| R-03 | Steel price movement beyond the hedged volume | 0.50 | 700,000 | `0.50 × 700,000 = 350,000` |
| R-04 | Specialist subcontractor default | 0.15 | 2,000,000 | `0.15 × 2,000,000 = 300,000` |
| R-05 | Rework arising from late design change | 0.35 | 800,000 | `0.35 × 800,000 = 280,000` |
| | **Total EMV (the mean)** | | | **1,740,000** |

The Monte Carlo model of the same register, capturing the shared exposure between R-02 and R-05 (both
driven by the same late design package), returns a P80 of **2,600,000**. Contingency is set there.

```
Cost of confidence = P80 − EMV = 2,600,000 − 1,740,000 = 860,000
```

That 860,000 is the amount the organisation is paying to move from funding the average outcome to funding an
80 % confidence outcome. It is a sponsor's decision, and it is recorded as one.

**At Month 9 — the drawdown position.**

Two risks have occurred. R-01 materialised and the consent delay cost 640,000 in standing time and
resequencing — less than the 900,000 assessed impact. R-05 materialised in part, drawing 310,000.

```
Contingency drawn      = 640,000 + 310,000 = 950,000
Remaining contingency  = 2,600,000 − 950,000 = 1,650,000
```

Both draws were posted as transfers into the affected control accounts. BAC remains 26,600,000; the
contingency pot inside it fell by 950,000 and control account budgets rose by the same amount.

**The comfortable test.** Earned value shows the project 40.0 % complete. Contingency drawn is
`950,000 ÷ 2,600,000 = 36.5 %`. Drawing 36.5 % of the reserve at 40.0 % complete looks disciplined, and this
is the figure that appears on most reserve reports.

**The test that matters.** The register is re-assessed at the same date:

| Risk | Status at Month 9 | Probability | Impact | `EMV` |
|---|---|---:|---:|---:|
| R-01 | Occurred and closed; drawn | — | — | — |
| R-02 | Open; trial pits raised the probability | 0.45 | 1,500,000 | `0.45 × 1,500,000 = 675,000` |
| R-03 | Closed — remaining volume price-fixed; contingency released | — | — | 0 |
| R-04 | Open, unchanged | 0.15 | 2,000,000 | `0.15 × 2,000,000 = 300,000` |
| R-05 | Occurred in part; residual closed | — | — | — |
| R-06 | New: commissioning resource availability | 0.25 | 1,200,000 | `0.25 × 1,200,000 = 300,000` |
| | **Remaining EMV** | | | **1,275,000** |

Re-running the model on the remaining register at the same P80 confidence gives a **remaining exposure of
1,900,000**.

```
Remaining contingency = 1,650,000
Remaining exposure (P80) = 1,900,000
Shortfall = 1,900,000 − 1,650,000 = (250,000)
```

The project is 250,000 short of the confidence level it was funded to, at Month 9 of 20 — while the burn
ratio says it is fine. The gap opened because R-02's probability rose after the trial pits and a new risk
(R-06) entered the register; both are ordinary events, and neither is visible in the drawdown percentage.

**What the report should say.** "Contingency remaining 1.65 million against remaining assessed exposure of
1.90 million at P80 — a shortfall of 0.25 million. Driven by R-02, whose probability rose to 0.45 following
the trial pit results, and by new risk R-06. Options: fund the gap from management reserve as a baseline
change, reduce exposure by advancing the ground investigation to convert R-02 into a known quantity, or
accept a reduced confidence level and record the decision. Recommendation: advance the ground investigation
at an estimated 140,000, which retires the largest single exposure. Decision required this month; the option
closes when the foundation sequence starts."

**If management reserve were released.** Suppose the sponsor releases 800,000 of management reserve to fund
the gap and a small scope addition:

```
New BAC = 26,600,000 + 800,000 = 27,400,000
Management reserve remaining = 1,400,000 − 800,000 = 600,000
Total authorised budget = 27,400,000 + 600,000 = 28,000,000   (unchanged)
```

The total authorised budget does not move — the money was always authorised. What moves is the baseline, and
therefore every earned value comparison from that date forward. The change log records the release, its
date and its reason, and the performance trend is annotated so no reader mistakes the step change for a
performance improvement.

**Assumptions this example depends on.** Probabilities and impacts are the assessed values recorded in the
register at each date; the Monte Carlo outputs are model results at the stated confidence level; risk
impacts are cost only, with any time-related consequence priced once and not duplicated in schedule
contingency; no escalation allowance covers the same exposure as R-03.

## 10. Checklist

**Setting it**

- [ ] Contingency is derived from the risk register, not from a percentage.
- [ ] The derivation method and confidence level are stated (EMV, or a named P-level).
- [ ] The gap between the mean and the funded level is disclosed as the cost of confidence.
- [ ] Shared drivers between risks have been considered, not assumed away.
- [ ] Escalation, currency and risk allowances do not cover the same exposure twice.

**Holding it**

- [ ] Contingency is held at project level, not distributed into control account budgets.
- [ ] Management reserve is held outside the baseline, by the sponsor.
- [ ] The budget structure reconciles: control accounts + contingency = BAC; BAC + management reserve =
      total authorised budget.

**Drawing it**

- [ ] Every draw cites a risk identifier and the evidence that the risk occurred.
- [ ] Approval thresholds by value are defined and followed.
- [ ] Draws are posted as transfers within the baseline; BAC is unchanged.
- [ ] Every draw is in the change log with a number, date and approver.
- [ ] Contingency for risks that have passed is released deliberately, not left in the pot.

**Reading it**

- [ ] Remaining contingency is compared with remaining exposure at the same confidence level, monthly.
- [ ] The drawdown curve is plotted against risk retirement, not against percent complete.
- [ ] Any shortfall is reported with options and a recommendation, not as a status.
- [ ] Management reserve releases are annotated on every performance trend they affect.
- [ ] At completion, undrawn contingency is explained rather than presented as a saving.

---

## Related

- `BPG-16 — Risk registers that work` — the register that contingency must be traceable to; without it,
  nothing in this guide is possible.
- `BPG-17 — Quantitative schedule risk analysis` — where a defensible P-level comes from, and how
  correlation moves the tail.
- `BPG-04 — Baselining and baseline change control` — the procedure for a management reserve release and
  for logging drawdown transfers.
- `BPG-09 — Estimate at completion: choosing and defending a method` — what belongs in a forecast and what
  sits beside it as held reserve.
- `TPL-10 — Risk register` — the instrument, including the contingency allocation columns used in §9.

## Sources and standards

The two-reserve structure and the derivation of contingency from analysed risk are described in published
cost and risk management frameworks, among them the AACE International Total Cost Management framework, the
PMBOK Guide and the ISO 31000 risk management principles; they are explained here in our own words and no
text or table is reproduced. Accounting treatment of provisions and reserves is governed by the applicable
financial reporting framework and is outside the scope of this guide. The internal references are BoK Domain
3 (Budgeting & Forecasting) and BoK Domain 12 (Risk Management for Project Controls). All figures in §9 are
illustrative and were computed for this document.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
