---
id: AIG-04
series: S02
series_name: AI in Project Controls Guide
title: AI-assisted cost forecasting
subtitle: What a model may contribute to a forecast, what it must never decide, and how to validate a number it produced
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: professional
reading_time_min: 14
summary: >
  A method document on using AI in cost forecasting. It sets out the six contributions a model can
  legitimately make — completeness of actuals, candidate estimates at completion, driver decomposition,
  portfolio trend detection, ranges and narrative drafting — and the five decisions that remain human,
  including which forecast to defend, whether a variance cause is closed, baseline change and contingency
  release. Its centre is a seven-step validation protocol for an AI-produced forecast number, worked
  through on a control account with the substitutions shown, so a practitioner can apply it at the next
  month-end.
linkedin:
  format: article
  hook: >
    A model can produce an estimate at completion in seconds. Deciding which estimate at completion you are
    prepared to defend is still a professional judgement, and it is the whole job.
  tags: [ProjectControls, EarnedValue, Forecasting, CostEngineering]
  asset: checklist-pdf
gated: false
related: [AIG-03, AIG-05, AIG-10, BPG-08, BPG-09]
bok_domains: [3, 6, 13]
sources:
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
  - "PCI Body of Knowledge, Domain 6 — Earned value management and forecasting (Institute manuscript, 2026)"
placeholders: 0
---

# AI-assisted cost forecasting

> What a model may contribute to a forecast, what it must never decide, and how to validate a number it
> produced.

**In one paragraph.** A method document on using AI in cost forecasting. It sets out the six contributions
a model can legitimately make — completeness of actuals, candidate estimates at completion, driver
decomposition, portfolio trend detection, ranges and narrative drafting — and the five decisions that
remain human, including which forecast to defend, whether a variance cause is closed, baseline change and
contingency release. Its centre is a seven-step validation protocol for an AI-produced forecast number,
worked through on a control account with the substitutions shown, so a practitioner can apply it at the
next month-end.

**Who this is for.** Cost engineers, cost managers, control account managers and project controls managers
who produce or challenge a forecast, and the project managers who have to defend one.

---

## 1. The problem a forecast actually has

A forecast is a claim about the future that a named person must defend, sometimes years later, sometimes in
a dispute. Its quality has never rested mainly on the arithmetic — the arithmetic of the earned value
management (EVM) family is simple — but on three harder things: whether the cost to date it starts from is
complete, whether the method chosen matches the reason the variance occurred, and whether the person
presenting it can explain the movement since last period in terms a director can act on.

AI changes the cost of producing candidates and explanations. It does not touch the defence. A model that
returns an estimate at completion (EAC) in under a second has given you a number and none of the three
things above.

The terms used throughout, expanded once: **planned value (PV)** is the budgeted cost of work scheduled;
**earned value (EV)** the budgeted cost of work performed; **actual cost (AC)** the cost incurred for that
work; **budget at completion (BAC)** the total authorised budget; **estimate at completion (EAC)** the
forecast final cost; **estimate to complete (ETC)** the forecast cost of the remaining work; **cost
performance index (CPI)** is `EV ÷ AC`; **schedule performance index (SPI)** is `EV ÷ PV`; **to-complete
performance index (TCPI)** is the efficiency the remaining work must achieve to hit a target; **variance at
completion (VAC)** is `BAC − EAC`. Method selection among the EAC family belongs to `BPG-09 — Estimate at
completion`; this document assumes it and concentrates on the AI contribution and the validation.

## 2. What AI can legitimately contribute

**2.1 Completeness of the cost to date.** The most common cause of a wrong forecast is not a wrong method
but an incomplete AC: goods received not invoiced (GRNI) unaccrued, commitments not reflected,
subcontractor valuations in transit, a late invoice run. Classification and matching capability is genuinely
good at reconciling purchase orders, receipts and invoices at volume, proposing accruals from receipt
records, and flagging the lines where the three disagree. This is the highest-value, lowest-risk
contribution on the list, and it improves the forecast without touching the method at all.

**2.2 Candidate EACs, generated across methods.** Producing the CPI-based, composite and
remaining-work-at-budget variants for every control account, every period, is tedious and therefore is
often done for a handful of accounts. A model does all of them and ranks the divergence. The value is not
the numbers — they are arithmetic — but the **spread**: accounts where the methods agree need less
attention than those where they diverge widely, which is a useful way to direct scarce analytical time.

**2.3 Driver decomposition.** Given cost, quantity, rate and commitment data, a model can decompose a
movement into components: rate against usage, escalation against scope, this package against that one.
Decomposition is arithmetic and it is checkable. **Attribution of cause is not decomposition** — the model
can tell you that 130,000 of the movement is in rework quantities; it cannot tell you that the rework was
caused by late design, and it should not be asked to.

**2.4 Trend and early warning across a portfolio.** Watching CPI, quantity growth, commitment burn and
productivity across hundreds of accounts, and raising a signal when a trend is sustained rather than when a
threshold is breached, is work that scales badly for humans and well for machines. Sustained-decline
detection across three or four periods is the single most useful early-warning pattern in cost control.

**2.5 Ranges and scenarios.** Producing a range around a central forecast, and running scenarios —
escalation at a different rate, productivity recovering or not — lets the professional present a range with
named drivers instead of a single point with false precision. The confidence level and the scenario set are
professional choices; the computation is not.

**2.6 Narrative drafting.** A first-draft variance narrative from the numbers, in the house format, saves
real time. Every figure in it is recomputed before it leaves, and every causal claim is either confirmed
against the actual analysis or deleted.

## 3. What a model must not decide

Five decisions sit outside the model regardless of how well it performs. `AIG-10 — Human in the loop` owns
the general rule; these are the specifics for cost forecasting.

**Which EAC is reported.** Selecting the forecast to be defended is a judgement about the future behaviour
of a specific job. The model proposes candidates; the professional selects, states the assumption the
selection embodies, and owns it.

**Whether the variance cause is closed or persisting.** This is the assumption on which the entire EAC
family turns — the CPI-based EAC assumes the past performance continues, the remaining-work-at-budget
variant assumes it does not. Answering it requires knowing whether the design error was corrected, whether
the crew has been replaced, whether the supplier has been changed. That knowledge is on the job, not in the
data.

**Baseline change.** A forecast that exceeds budget is a forecast; a change to the budget is a governance
act with an approver and an audit trail. Models must never write to the baseline, and no automated
workflow should update a budget field. `BPG-04 — Baselining and baseline change control` covers the
control.

**Contingency release.** Drawing contingency is a management decision made against defined criteria and
appetite, and it is the point at which a forecast becomes money. A model may compute the exposure and the
remaining balance; the release decision is made by the named authority, minuted.

**Reclassification of scope as variation.** Whether work is a variation, a claim or absorbed scope is a
contractual judgement with entitlement consequences, and it moves the forecast materially. Extraction may
locate the clause and the correspondence; the position is taken by a commercial professional, with legal
review where the exposure warrants it.

## 4. Validating an AI-produced forecast number

Seven checks, in order. The first four are non-negotiable for any figure that will be reported; the last
three are proportionate to the stakes.

1. **Recompute from source.** Take PV, EV, AC and BAC from the controlled system, not from the model's
   output, and recompute CPI, SPI and the candidate EACs by hand or in a controlled sheet. If the model's
   inputs and yours differ, stop: the discrepancy is the finding.
2. **Check the method against the cause.** Name the assumption the model's EAC embodies — performance
   continues, performance recovers, remaining work at budget — and test it against what is actually
   happening on the job. A model with no site knowledge will usually assume continuation or a
   history-derived recovery, and the second of those is where quiet optimism enters.
3. **Run the TCPI reality check.** Compute the efficiency the remaining work must achieve to land on the
   budget, and compare it with the efficiency achieved so far. A required index far above the achieved one
   is a forecast that has not yet admitted what it knows. Note the trap in §6: TCPI computed against a
   CPI-based EAC returns the achieved CPI by construction and checks nothing.
4. **Reconcile to commitments and the cost to date.** Confirm that the AC used is complete — accruals on
   service date, GRNI included, subcontract valuations current — and that the forecast is not lower than
   the sum of committed cost plus reasonable remaining exposure.
5. **Bridge the movement.** Explain the change from last period as a small number of named components that
   sum exactly to the movement. A movement that cannot be bridged is not yet understood, whoever produced
   it.
6. **Compare against one independent simple method.** A quantity-based or rate-based estimate to complete,
   done crudely, is a valuable check on a sophisticated one. Where the two disagree materially, the
   sophistication is on trial, not the simplicity.
7. **Check for double counting.** Risk allowances carried both in the forecast and in contingency;
   escalation applied both in the rate and in a provision; a variation counted in both the forecast and the
   change log. Double counting is the most common defect in AI-assembled forecasts, because assembly from
   multiple sources is exactly what the tooling makes easy.

Where the model was trained on your historical projects, one further check applies before reliance rather
than at each use: **is the training history comparable to this job** in scope, contract type, location and
price basis? `AIG-03 — Data readiness` §2.5 gives the normalisation this depends on.

## 5. How this goes wrong

**The model's number becomes the anchor.** The first figure on the page shapes every discussion that
follows. When a model produces the first figure, the professional's role silently changes from forecasting
to adjudicating the model — and adjudicating downwards is uncomfortable in a meeting. Produce your own
candidate before you look at the model's, at least on material accounts.

**Recovery is assumed because history recovered.** A model trained on completed projects learns that
late-project CPI often improves — partly because projects genuinely recover, and partly because late scope
gets moved, claims get settled and budgets get changed. It cannot distinguish those, so it forecasts
recovery on a job where nothing has changed.

**The forecast is precise and unfounded.** An EAC quoted to the currency unit from inputs that are ranges
communicates confidence the analysis does not have. Round to the precision the inputs support and state the
range.

**Trend detection is tuned until it stops complaining.** Early warning that generates too many signals gets
turned down until it generates none, and the tuning is done by whoever finds the alerts annoying rather
than by whoever owns the risk. Threshold changes are control changes: they get an owner and a record.

**The narrative is believed because it is fluent.** A drafted variance narrative attributes the overrun to
weather; the actual analysis shows a rate variance on imported material. Fluency is not evidence. Every
causal claim in a generated narrative is either traced to the analysis or removed.

**Incomplete actuals are forecast forward.** The most expensive arithmetic error in cost control is
forecasting from an AC missing a month of accruals: CPI looks strong, the EAC looks comfortable, and both
correct sharply when the invoices land. Accelerating the cycle makes this worse, because there is less
elapsed time for late documents to arrive before the pack closes.

**Nobody re-tests the model after the portfolio changes.** A model that performed well on building work is
applied to civils, or the delivery model shifts from reimbursable to lump sum, and performance degrades
without an alarm. Re-test on a change in the work, not only on a calendar.

## 6. Worked example — validating an AI-produced EAC

*Illustrative figures.* One control account, month 14. Currency USD, all figures cumulative to the data
date, rounded to the nearest whole currency unit. Not benchmark data.

**Controlled inputs, taken from source rather than from the model:**

| Measure | Value |
|---|---|
| Budget at completion (BAC) | 8,400,000 |
| Planned value (PV) | 5,000,000 |
| Earned value (EV) | 4,620,000 |
| Actual cost (AC) | 5,100,000 |

**Step 1 — recompute.**

`CV = EV − AC = 4,620,000 − 5,100,000 = −480,000`
`SV = EV − PV = 4,620,000 − 5,000,000 = −380,000`
`CPI = EV ÷ AC = 4,620,000 ÷ 5,100,000 = 0.906` (0.905882, shown to three decimals)
`SPI = EV ÷ PV = 4,620,000 ÷ 5,000,000 = 0.924`

**Step 2 — candidate forecasts.**

Performance continues, cost only:
`EAC = BAC ÷ CPI = 8,400,000 ÷ 0.905882 = 9,272,727`
(equivalently `BAC × AC ÷ EV = 8,400,000 × 5,100,000 ÷ 4,620,000 = 9,272,727`)

`VAC = BAC − EAC = 8,400,000 − 9,272,727 = −872,727`

Performance continues, cost and schedule pressure both persisting:
`CPI × SPI = 0.905882 × 0.924 = 0.837035`
`ETC = (BAC − EV) ÷ (CPI × SPI) = 3,780,000 ÷ 0.837035 = 4,515,939`
`EAC = AC + ETC = 5,100,000 + 4,515,939 = 9,615,939`

**The model's proposal: EAC = 8,960,000**, with a decomposition attributing the variance largely to a
design-rework driver and a note that comparable accounts in the training history recovered part of their
cost variance after the rework driver closed.

**Step 3 — TCPI reality check.** To land on budget, the remaining work must be performed at:

`TCPI to BAC = (BAC − EV) ÷ (BAC − AC) = 3,780,000 ÷ (8,400,000 − 5,100,000) = 3,780,000 ÷ 3,300,000 = 1.145`

Achieved efficiency is 0.906. Nothing on the account has changed that would support a jump to 1.145, so any
forecast at or near budget is already refuted.

*The trap.* Computing TCPI against the CPI-based EAC gives
`(BAC − EV) ÷ (EAC − AC) = 3,780,000 ÷ (9,272,727 − 5,100,000) = 3,780,000 ÷ 4,172,727 = 0.906` — exactly
the achieved CPI, by construction. It looks like a passed check and is arithmetically empty. The check that
bites is against the budget, or against a *different* forecast.

**Step 4 — reconcile to commitments.** Committed cost on the account is 7,940,000, of which 5,100,000 is
incurred. A forecast of 8,960,000 leaves `8,960,000 − 7,940,000 = 1,020,000` for all remaining uncommitted
work and for any growth in the committed packages. The professional checks that figure against the
remaining scope; if the uncommitted scope is plainly worth more than 1,020,000, the model's forecast fails
here, before any argument about method.

**Step 5 — bridge the movement.** Last period's reported EAC was 9,050,000; this period's CPI-based
candidate is 9,272,727, a movement of `9,272,727 − 9,050,000 = +222,727`. The decomposition offered:

`+150,000` rate escalation on imported material · `+130,000` rework quantities in the containment package ·
`−57,273` scope removed by approved variation

`150,000 + 130,000 − 57,273 = 222,727` — the bridge closes exactly, and each component is then traced: the
escalation to the procurement record, the rework to the non-conformance log, the variation to the change
register.

**Step 6 — the judgement.** The model's 8,960,000 sits below both candidates because it assumes partial
recovery once the rework driver closes. The professional's question is not whether the model is
sophisticated but whether **this** driver has closed on **this** account. If the design revision is issued,
the rework scope is bounded and the crew is back on planned work, a recovery assumption is defensible and
should be stated as an assumption. If the revision is still in review, it is not, and the honest forecast
sits between the CPI-based 9,272,727 and the composite 9,615,939.

**Step 7 — what is reported.** A forecast of **9,270,000** (rounded to the nearest 10,000, reflecting input
precision), stated as: performance-continues basis, cost only; assumes the rework driver closes in the
current period with no further growth; range to 9,620,000 if schedule pressure persists; TCPI to budget of
1.145 shown as the reason budget is no longer credible; AI-assisted, verified and owned by the named cost
engineer.

**Assumptions this answer depends on.** That EV is measured on rules of credit that have not changed this
period; that AC is complete at the data date, including accruals on service date; that the approved
variation is fully reflected in both BAC and the change register; and that the training history behind the
model's recovery assumption is comparable to this account, which §4's comparability check must establish
separately.

## 7. Checklist — before an AI-assisted forecast leaves the function

1. **Inputs from source.** PV, EV, AC and BAC taken from the controlled system and recomputed, not accepted
   from the model's output.
2. **Cost to date complete.** Accruals on service date, GRNI included, subcontract valuations current,
   commitments reconciled.
3. **Assumption named.** The forecast's assumption about the variance cause is stated in one sentence a
   director can challenge.
4. **TCPI to budget computed** and shown, not TCPI to the forecast that produced it.
5. **Movement bridged** to components that sum exactly, each traced to a record.
6. **Independent check done** by one simple method, with any material disagreement explained.
7. **No double counting** between forecast, risk allowance, contingency and change log.
8. **Precision honest.** Rounded to what the inputs support, with a range where one exists.
9. **Ownership recorded.** Named professional, AI assistance disclosed, and a note of what the model
   proposed and what changed in review.

---

## Related

- `AIG-03 — Data readiness: what AI needs before it is any use` — the comparability and cut-off conditions
  §4 and §5 depend on.
- `AIG-05 — AI in scheduling — and what must not be automated` — the schedule side of the same forecast,
  and why a cost forecast that ignores the critical path is incomplete.
- `AIG-10 — Human in the loop: what AI may and may not decide` — the general decision-rights schedule
  behind §3.
- `BPG-08 — Earned value in practice` — the measurement discipline the inputs to §6 assume.
- `BPG-09 — Estimate at completion — choosing and defending a method` — how the method is selected, which
  this document treats as given.

## Sources and standards

- **PCI Body of Knowledge, Domain 6** — *Earned value management and forecasting* (Institute manuscript,
  2026). The forecasting family and the TCPI reality check are explained there; this document applies them
  to an AI-produced number.
- **PCI Body of Knowledge, Domain 13** — *AI for project controls and project management* (Institute
  manuscript, 2026), Knowledge Area 13.5.3, the forecasting workflow whose propose–verify–own shape §4
  operationalises.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
