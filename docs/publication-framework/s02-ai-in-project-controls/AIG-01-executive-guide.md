---
id: AIG-01
series: S02
series_name: AI in Project Controls Guide
title: AI in project controls — the executive guide
subtitle: What changes, what does not, and what a director should require before approving use
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [executive, manager]
level: leader
reading_time_min: 12
summary: >
  This guide sets out the Institute's position on artificial intelligence in a project controls function:
  what AI genuinely changes (the economics of coverage, of cycle time and of early warning), what it does
  not change (accountability for every number that leaves the function), and the six things a director
  should require before approving AI-assisted controls work. It closes with a value case netted honestly,
  the failure modes we see most often, and an approval gate usable in a meeting. The Institute's position
  throughout is that AI proposes; the professional disposes — every output must be explainable, validated
  and owned by a competent human.
linkedin:
  format: article
  hook: >
    Approving AI in a controls function is not a tools decision. It is a decision about who signs the
    number and how that signature is earned.
  tags: [ProjectControls, AIGovernance, CostEngineering, ProjectManagement]
  asset: one-pager
gated: false
related: [AIG-02, AIG-03, AIG-08, AIG-10, AIG-11]
bok_domains: [13]
sources:
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
  - "PCI candidate AI-use policy (Institute, 2026)"
  - "ISO/IEC 42001:2023, Information technology — Artificial intelligence — Management system"
  - "NIST AI Risk Management Framework 1.0 (National Institute of Standards and Technology, January 2023)"
placeholders: 0
---

# AI in project controls — the executive guide

> What changes, what does not, and what a director should require before approving use.

**In one paragraph.** This guide sets out the Institute's position on artificial intelligence in a project
controls function: what AI genuinely changes (the economics of coverage, of cycle time and of early
warning), what it does not change (accountability for every number that leaves the function), and the six
things a director should require before approving AI-assisted controls work. It closes with a value case
netted honestly, the failure modes we see most often, and an approval gate usable in a meeting. The
Institute's position throughout is that AI proposes; the professional disposes — every output must be
explainable, validated and owned by a competent human.

**Who this is for.** Project controls directors, heads of PMO (project management office), commercial and
finance directors sponsoring controls change, and the project controls managers who will be asked to make
the decision work.

---

## 1. What you are actually approving

When a director approves AI in a controls function, they rarely think they are approving anything
consequential — a licence, a pilot, a feature already embedded in a platform the organisation owns. What
is being approved is narrower and more serious than that: **a change to how a number becomes trustworthy
before it is signed.**

Every figure a controls function releases — a forecast, a valuation, an accrual, a reported percentage
complete — carries an implicit assurance: a competent person produced it by a defensible method, from
identified data, and would be able to explain it under challenge months later. AI does not remove that
assurance requirement. It changes where the human effort sits: less in production, far more in
verification. A function that adopts AI without moving effort into verification has not become more
efficient; it has become faster at releasing unverified numbers.

That is the whole executive question, and everything below is a way of answering it in specifics.

## 2. What genuinely changes

Three economics move, and it is worth being precise about which three, because organisations routinely buy
one and get charged for another.

**Coverage becomes cheap.** The binding constraint on much of controls has always been reading and
checking capacity: nobody reads all sixty subcontracts, all the correspondence, every line of the ledger,
every activity in a 1,500-line schedule. Machine capability that reads everything at low marginal cost
changes which controls are feasible at all. This is the single largest and most under-claimed benefit — not
that AI is clever, but that it does not tire. See `AIG-02 — What AI actually does in a controls function`
for what coverage is worth, task by task.

**Cycle time compresses — and creates review debt.** Month-end assembly, variance narrative drafting,
schedule health checking and document extraction all compress substantially. The compression is real. What
comes with it is a new obligation: the exceptions that used to be found by the act of assembly are now
found only if somebody deliberately looks. A pipeline fails silently where a human assembler would have
noticed. Budget the review time, or the saving is borrowed rather than earned.

**Warning arrives earlier.** A model watching a trend across a portfolio can raise a signal months before a
single bad reporting period forces it into a meeting. This is the benefit most worth having and the one
most often wasted, because early warning is only valuable to an organisation whose governance can act on a
signal that is not yet certain. If your change-control forum requires certainty before it will meet, an
earlier signal will simply sit in a log.

## 3. What does not change

**Accountability.** A model cannot be accountable — it cannot be questioned, sanctioned, or asked what it
was thinking. For every AI-influenced estimate, forecast, valuation or disclosure, a named person is
accountable. "It was the model's output" is not a defence available to anybody in the chain, including the
director who approved the tool.

**The standard of care.** The duty to verify a figure against source, to state assumptions, to distinguish
what is measured from what is inferred, and to disclose material uncertainty is the discipline's existing
standard. AI does not lower it. If anything it raises the practical bar, because a more capable model that
is wrong is more convincingly wrong: fluent, internally consistent, correctly formatted and false.

**The decisions that stay human.** Some decisions must not be delegated to a model regardless of its
measured accuracy, because the thing being decided is entitlement, appetite or accountability rather than
computation. Baseline change acceptance, extension-of-time (EOT) entitlement, contingency release, revenue
recognition and provisioning, and any decision about individuals all sit here. `AIG-10 — Human in the loop:
what AI may and may not decide` owns the full decision-rights schedule; the method documents in this series
state the specific prohibitions for forecasting, scheduling and risk.

## 4. Where the capability is weak

An executive who knows only the strengths will approve the wrong pilot. Current capability is weak at
**causal reasoning** — it detects that cost and a variable moved together, not that one caused the other;
at **genuinely novel scope**, where there is no comparable history to learn from; at **contractual
judgement**, where the answer depends on entitlement rather than on language patterns; and at **multi-step
arithmetic**, where plausible and correct diverge silently. It has no notion of truth, only of likelihood,
which is why fabricated figures and citations — hallucinations — remain a live risk rather than a solved
one. `AIG-02` maps this weakness onto specific controls tasks.

## 5. Six things to require before approving use

These are the Institute's recommended minimum. They are deliberately answerable in a paragraph each; if a
proposal cannot answer them, it is not ready for approval rather than badly written.

**5.1 The task class, not the tool.** State which class of work is being automated — extraction,
classification, drafting, forecasting from tabular data, anomaly detection, retrieval-grounded answering —
and why that class fits this task better than a deterministic rule. Where a rule suffices, a rule is
better: it is transparent, testable and cheap to audit. Approving "AI for controls" approves nothing
reviewable.

**5.2 A data-fitness statement.** Which datasets feed it, who owns them, how they are coded, and what
proportion currently fails validity, completeness and cut-off checks. Most AI failures in controls are data
failures wearing a model's clothes. `AIG-03 — Data readiness` gives the tests; require the results, not the
intention.

**5.3 A verification design with a named checker.** What is checked, by whom, at what frequency, and what
proportion — per item, or by sample against a stated rule. A verification step that is described as
"reviewed by the team" will not happen. Require the sampling rule and the exception threshold in writing.

**5.4 A named owner and an audit trail.** One person accountable for the output's performance, and a record
for each material AI-assisted output of what the model produced, who reviewed it, what changed in review
and why. This is the artefact that lets a number be defended a year later, in a dispute, to an auditor. The
control framework that houses it is `AIG-08 — Governing AI on a project`.

**5.5 A value case with the governance cost netted.** Licences, integration, data remediation, training and
the ongoing verification effort — all of it — set against measured savings, not vendor-claimed ones. The
most common way an AI business case flatters itself is by pricing the licence and omitting the checking.
§7 works one through.

**5.6 A stop rule.** What result would cause this to be discontinued, decided before the pilot starts. A
pilot that cannot fail is not a pilot; it is a procurement with extra steps.

## 6. Sequencing

The order that works is policy, then pilot, then standardise, then integrate. Policy first is not
bureaucratic instinct: without an approved-tool register and a data rule, a pilot generates confidentiality
exposure and un-auditable outputs that must later be unpicked. Two sequencing errors are common enough to
name. The first is **integrating before governing** — embedding AI in the month-end before anyone has
defined who checks what, which produces speed and quiet risk in the same quarter. The second is
**governing without ever piloting**: a policy written in the abstract, applied to no real workflow, that
turns into paperwork nobody follows.

Between them sits an honest question a director should ask every quarter: *which rung are we actually on,
and what evidence supports the claim?* An organisation that says "integrated" while three people use a
public assistant unregistered is on the first rung, not the fourth.

## 7. How this goes wrong

**The saving is claimed gross.** A forty-per-cent faster close is reported as a forty-per-cent cost
reduction. It is not: the verification, monitoring and data work the governance model requires are real
recurring costs and belong in the same table. See §8.

**Verification is assumed rather than resourced.** The proposal says outputs will be checked; no one's
objectives, capacity or job description changes; within two cycles the check has become a glance. This is
the single most common failure, and it is invisible until an error survives to a board paper.

**A pilot succeeds on the vendor's data.** A demonstration on curated examples proves the tool works on
curated examples. Evidence means the tool scored against your own data, with the correct answers
established by your professionals beforehand. `AIG-11 — Evaluating AI tools` covers the due-diligence
protocol.

**Confidential data leaves before anyone notices.** Commercially sensitive rates, contract positions,
personal data and unpublished results are pasted into ungoverned tools because no register exists and no
alternative was provided. The remedy is to provide a governed tool early, not to prohibit and hope.

**The near-miss is never reported.** A function reporting no AI incidents is more likely ungoverned than
infallible: in an ad-hoc culture the hallucinated clause travels upward as fact instead of landing in an
incident log. Treat the first reported near-miss as the governance working.

**Deskilling arrives quietly.** If AI drafts every narrative, codes every line and proposes every forecast,
the verification the whole model depends on eventually falls to reviewers who have never done the work
unaided. Keep some first-principles work in the function deliberately; `AIG-12 — The AI-literate controls
professional` deals with how.

**The wrong problem is automated.** Speeding up a report that nobody uses, or a reconciliation that exists
because two systems disagree, entrenches the waste. Ask what would happen if the output stopped, before
asking how to produce it faster.

## 8. Worked example — the value case, netted

*Illustrative figures.* A controls function of six people, one portfolio, AI-assisted coding and
reconciliation at month-end. Currency USD; effort in person-days of 8 hours; loaded cost USD 640 per
person-day (8 × USD 80). All figures are for teaching, not benchmarks.

**Baseline.** Three staff spend six working days each per monthly cycle on assembly and reconciliation:

`3 × 6 × 12 = 216 person-days per year`

**After.** The same cycle takes three staff three and a half days each:

`3 × 3.5 × 12 = 126 person-days per year`

`Gross saving = 216 − 126 = 90 person-days per year`

**The new work the model creates.** Model monitoring, exception-rule maintenance, refreshing the sample of
inputs used to score the model, and keeping the tool register current: 1.5 person-days per month.

`1.5 × 12 = 18 person-days per year`

`Net effort saved = 90 − 18 = 72 person-days per year`

`Net effort saved in money = 72 × 640 = USD 46,080 per year`

**Netting the cost.** Licences plus amortised integration and data remediation: USD 38,000 per year
(assumed here; in a real case this is quoted and evidenced).

`Net annual value = 46,080 − 38,000 = USD 8,080`

**Read it honestly.** The cycle-time headline is genuine — `(18 − 10.5) ÷ 18 = 41.7 %` less effort per
close — and the net financial value is USD 8,080, which will not survive a rounding error in the
assumptions. Two things follow. First, the defensible justification is not the money; it is the 72
person-days moved from assembling numbers to interrogating them, plus whatever the earlier warning is
worth, which should be argued explicitly rather than assumed. Second, the case is fragile: if governance
effort is understated by half a day a month, that is `6 × 640 = USD 3,840` a year, and net value falls to
`8,080 − 3,840 = USD 4,240`. A director who is shown a gross saving of USD 46,080 with no governance line
is being shown the wrong number.

## 9. The director's approval gate

A usable list for the meeting in which the decision is taken. Six questions, one required answer each.

1. **Task class.** Which class of capability is this, and why is a deterministic rule not the better
   answer? *(Required: the class named, and the rule alternative explicitly considered.)*
2. **Data fitness.** What proportion of the feeding dataset currently fails validity, completeness and
   cut-off checks, and who owns the remediation? *(Required: a measured figure, not an intention.)*
3. **Verification.** Who checks what, how often, at what sample rate, and what happens to an exception?
   *(Required: a named role and a written sampling rule.)*
4. **Accountability.** Who owns the output's performance, and where is the trail of what the model
   proposed, who reviewed it and what changed? *(Required: one name, one artefact.)*
5. **Netted value.** What is the value with licences, integration, data work, training and verification
   effort all netted against measured savings? *(Required: a net figure and its sensitivity.)*
6. **Stop rule.** What result would cause us to discontinue this, and who decides? *(Required: a threshold
   agreed before the pilot begins.)*

A seventh question is worth asking annually rather than at approval: **where have we decided not to use AI,
and why?** A function that cannot answer it has not been choosing.

---

## Related

- `AIG-02 — What AI actually does in a controls function` — the capability map behind §2 and §4, task by
  task, including where the capability is genuinely weak.
- `AIG-03 — Data readiness: what AI needs before it is any use` — the tests behind the data-fitness
  statement required at §5.2.
- `AIG-08 — Governing AI on a project — the control framework` — the control framework that houses the
  register, the policy and the audit trail.
- `AIG-10 — Human in the loop: what AI may and may not decide` — the full decision-rights schedule
  summarised at §3.
- `AIG-11 — Evaluating AI tools — a buyer's due-diligence guide` — how to test a vendor claim against your
  own data before committing.

## Sources and standards

- **PCI Body of Knowledge, Domain 13** — *AI for project controls and project management* (Institute
  manuscript, 2026). The source for the propose–verify–own workflow shape and the governance position
  summarised here.
- **PCI candidate AI-use policy** (Institute, 2026). The Institute's published position on AI in
  preparation, in assessment and in practice.
- **ISO/IEC 42001:2023**, *Information technology — Artificial intelligence — Management system*. In our
  own words: it describes a management-system approach to AI — policy, defined roles, risk assessment,
  controls over the AI lifecycle and periodic review — the same shape as any other management system. It is
  a useful skeleton for §5 and §6; it is not a substitute for the discipline-specific verification a
  controls function needs.
- **NIST AI Risk Management Framework 1.0** (National Institute of Standards and Technology, January 2023).
  In our own words: a voluntary framework organising AI risk work into governing, mapping, measuring and
  managing — helpful for structuring the evidence a director should ask for, and explicit that measurement
  without governance is not assurance.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
