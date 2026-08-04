---
id: AIG-06
series: S02
series_name: AI in Project Controls Guide
title: AI for risk identification and quantification
subtitle: What a model can add to a risk process, and how it manufactures confidence when nobody is watching
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: professional
reading_time_min: 14
summary: >
  A method document on AI in project risk work. It separates identification — where machine reading of
  correspondence, history and the estimate genuinely finds candidate risks a workshop misses — from
  quantification, where the same tooling can manufacture confidence the inputs do not support. It names
  what a model cannot see, including the blind spots it inherits from your own register; sets out the six
  ways a simulation flatters itself; states the decisions that remain human, including the confidence level
  and contingency release; and works an expected-monetary-value and simulation example in which a single
  unexamined correlation assumption moves the contingency by more than the smallest risk in the register.
linkedin:
  format: newsletter
  hook: >
    A simulation will return a smooth curve and a precise number from inputs that were guesses. The curve
    is not evidence; it is arithmetic performed on judgement.
  tags: [ProjectControls, RiskManagement, Contingency, ArtificialIntelligence]
  asset: checklist-pdf
gated: false
related: [AIG-04, AIG-05, BPG-16, BPG-17, TPL-10]
bok_domains: [12, 13]
sources:
  - "PCI Body of Knowledge, Domain 12 — Risk management for project controls (Institute manuscript, 2026)"
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
  - "ISO 31000:2018, Risk management — Guidelines"
placeholders: 0
---

# AI for risk identification and quantification

> What a model can add to a risk process, and how it manufactures confidence when nobody is watching.

**In one paragraph.** A method document on AI in project risk work. It separates identification — where
machine reading of correspondence, history and the estimate genuinely finds candidate risks a workshop
misses — from quantification, where the same tooling can manufacture confidence the inputs do not support.
It names what a model cannot see, including the blind spots it inherits from your own register; sets out
the six ways a simulation flatters itself; states the decisions that remain human, including the confidence
level and contingency release; and works an expected-monetary-value and simulation example in which a
single unexamined correlation assumption moves the contingency by more than the smallest risk in the
register.

**Who this is for.** Risk managers, cost engineers and planners who maintain quantified registers, project
controls managers who set contingency recommendations, and the project directors who approve them.

---

## 1. Two failures, and only one of them is new

Risk processes fail in two recognisable ways. The first is old: a register that has become an issues list —
things that have already happened, written as risks, with mitigation columns describing work already done.
Nothing in it looks forward, and the workshop that produced it drew on the same six people's recent
experience.

The second failure is newer and more dangerous: a register that is thin, unexamined and unevidenced, put
through a simulation, and returned as a smooth distribution with a contingency figure to the currency unit.
Everything about the output signals rigour — the curve, the percentiles, the tornado chart — and none of
that rigour is in the inputs. **Simulation transports precision from the arithmetic to the judgement, where
it does not belong.**

AI has a genuine contribution to make to the first failure and a strong tendency to worsen the second.
Treating identification and quantification as separate problems, with separate controls, is the whole
method of this document. The register itself is owned by `BPG-16 — Risk registers that work` and the
analysis technique by `BPG-17 — Quantitative schedule risk analysis`; what follows is the AI discipline
around them.

## 2. Identification: what machine reading genuinely adds

**Candidate risks from analogous history.** Where completed projects have registers and, better, records of
which risks actually occurred, a model can propose the risks that materialised on similar work and are
absent from this register. The output is a list of candidates for a human to accept, reject or reword — and
the rejections matter as much as the acceptances, because they are where the reasoning is recorded.

**Risks visible in correspondence.** Requests for information (RFIs), non-conformance reports (NCRs),
site instructions, minutes and letters contain the early signals of most cost and delay risk: a supplier
querying a delivery date, a repeated query about an interface, an access restriction mentioned in passing.
Machine reading covers the whole corpus rather than the sample a human can manage, and this coverage is the
strongest argument for AI anywhere in risk work.

**Structural gaps.** Cross-reading the register against the schedule and estimate surfaces omissions
mechanically: long-lead packages with no supply risk, activities on the driving path with no risk
attached, single-source suppliers with no alternative, work in a jurisdiction where consents are not
represented. These are not clever inferences; they are joins that nobody has time to do by hand.

**Hygiene.** De-duplicating near-identical entries, merging entries describing the same event from two
packages, standardising categories, and rewriting entries into a disciplined cause–event–effect form are
tasks a language model does well and a busy risk manager does last.

## 3. What identification cannot see

**Genuinely novel risk.** A model proposes what resembles what it has read. First-of-a-kind work, a new
contracting structure, an unprecedented regulatory change or a technology with no delivery record produce
nothing to resemble.

**Your own blind spots, reproduced.** This is the limit that matters most and is named least. A model
trained or grounded on your organisation's registers learns your organisation's habits — including the
categories you never populate and the risks your culture does not write down. It will return a register
that looks complete by your own historical standard, which is precisely the standard in question.

**What people choose not to say.** Some risks are absent because raising them is uncomfortable: a
sponsor's unrealistic date, a partner's capability, a decision already taken. No document contains them, so
no model can read them. Surfacing them remains a matter of trust between people, and it is one reason the
workshop survives.

**Cause.** A model can observe that projects with a particular characteristic overran. It cannot establish
that the characteristic caused the overrun, and a risk register built on association will attach mitigations
to correlates rather than to causes.

## 4. Quantification: contribution and temptation

The legitimate contributions are real and largely mechanical: proposing three-point ranges from historical
spread for review; fitting distributions and reporting the fit; running the simulation with correlations;
producing sensitivity and driver rankings; tracking drawdown against the exposure remaining. All of that is
computation, and computation is what machines are for.

The temptation is equally specific. Six ways a quantified output flatters itself:

**Precision that outruns the inputs.** A register scored by judgement in bands returns a P80 to the nearest
currency unit. Reporting it that way is a claim about the analysis that the analysis cannot support. Round
to the precision the inputs justify and say what that precision is.

**Default ranges presented as evidence.** Ranges of plus or minus ten per cent applied across a register
because that is the template's default produce a distribution built on a habit. Every range should record
where it came from: measured spread, supplier quotation, owner judgement, or default — and the proportion
that are defaults should be reported alongside the result.

**Zero correlation, unexamined.** Independence is the default in most tools and is almost never true:
productivity risks move together, weather affects several activities, a single supplier fails once and
appears in four risks. Independence narrows the distribution and lowers the upper percentiles, which is to
say it makes the answer more comfortable and less true. §9 quantifies this on a small register.

**A distribution fitted to too few points.** Fitting a shape to five historical observations is curve
drawing, not estimation. Where evidence is thin, a simple triangular or uniform range honestly labelled as
judgement is more defensible than a fitted curve that implies data.

**Iteration counts that are not checked for stability.** The test is not a number of iterations but whether
the reported percentiles are stable when the simulation is re-run. Run it twice; if the P80 moves
materially between runs, the result is noise at the precision being reported.

**Risks excluded because they cannot be modelled.** The unquantifiable risk — a consent refused, a partner
withdrawing — drops out of the register because it does not fit the arithmetic, and the total exposure
silently excludes the things most likely to matter. Carry them separately and report them alongside the
number, explicitly.

To this add the failure mode AI introduces directly: a model asked to **explain** a simulation output will
produce a fluent explanation whether or not one exists. A narrative that says the P80 is driven by supply
chain and weather is a hypothesis to be checked against the sensitivity output, not a finding.

## 5. What must not be decided by a model

**The confidence level.** Whether contingency is set at P50, P80 or elsewhere is a statement of the
organisation's risk appetite, made by the accountable authority — not a modelling parameter and not a
convention to be inherited from a template.

**Contingency release.** Drawdown is a management decision against defined criteria, minuted, with the
remaining exposure reassessed. A model may report the balance and the exposure; it may not release.

**Risk acceptance.** Deciding to carry a risk untreated is an accountable judgement about appetite and
capacity, not an output of a score.

**Whether a risk is closed.** Closure means the exposure has genuinely gone — not that the date passed or
that nobody has updated the entry. A model may flag stale entries; an owner closes them.

**Ownership.** Every risk has a named owner who can act. Assignment is a management act; a model proposing
an owner from a pattern in the register is proposing, not deciding.

## 6. Validating a quantified risk output

1. **Confirm the register is a register.** Entries are forward-looking, in cause–event–effect form, with
   named owners. Issues masquerading as risks are removed to the issues log before anything is quantified.
2. **Trace the inputs.** For each of the largest contributors, confirm probability and impact with the
   owner, and record the evidence class: measured, quoted, judged or default.
3. **Report the default share.** State what proportion of ranges are template defaults. If it is high, the
   distribution is a picture of the template.
4. **Test the correlation assumption.** Re-run with a plausible alternative correlation and report the
   movement. An assumption whose alternative moves the answer materially is a finding, not a setting.
5. **Check stability.** Re-run and confirm the reported percentiles are stable at the precision being
   reported.
6. **Sanity-check against a deterministic sum.** Compare with the sum of expected monetary values (EMV).
   The two answer different questions, and the relationship between them should be explicable — see §9.
7. **Check for double counting.** Risk allowances embedded in the estimate, contingency held at package
   level, and provisions in the forecast frequently overlap. Reconcile explicitly, once, and record it.
8. **List what is excluded.** Every risk left out of the quantification, and why, reported with the result.

## 7. How this goes wrong

**The output is adopted because it is quantified.** A number with a distribution behind it beats a number
without one in most meetings, regardless of which rests on better inputs. The defence is to present the
evidence class of the inputs alongside the result every time.

**The model's candidate risks are accepted wholesale.** Two hundred proposed risks are pasted into the
register to demonstrate thoroughness. The register becomes unusable, owners disengage, and real risks are
buried among plausible ones. Accept candidates one at a time, with a reason.

**Historical registers are treated as historical outcomes.** A register records what people thought might
happen, not what did. Learning from registers alone teaches a model the profession's anxieties rather than
its experience. Learning from *realised* risk requires recording, at closeout, which risks actually
occurred — a discipline most organisations lack and can start this year.

**Correlation is set once and never revisited.** The assumption is made during set-up by whoever built the
model, is not visible in the output, and survives every subsequent review because nobody knows it is there.

**The tornado is read as a cause list.** A sensitivity ranking shows which inputs move the answer, which
depends on the ranges assigned as much as on the risks themselves. A risk with a wide default range will
outrank a real risk with a tight measured one.

**Contingency drifts into a target.** Once a P80 figure is in a budget, pressure to reduce it produces
quiet changes to input ranges rather than an honest argument about appetite. Version the register, and
require that any change to an input range carries the owner's name and reason.

**Confidentiality is lost through the register.** A quantified register is a map of where a project is
weakest and what the organisation believes about its suppliers. It goes into governed tools only.

## 8. Worked example — EMV, simulation and one unexamined assumption

*Illustrative figures.* A five-line extract from a quantified cost risk register. Currency USD, all figures
at current price basis, impacts stated as most likely cost of occurrence. Not benchmark data.

| Risk | Probability | Impact | Expected monetary value |
|---|---|---|---|
| R1 Consent decision later than programmed | 0.30 | 900,000 | `0.30 × 900,000 = 270,000` |
| R2 Rock encountered in bulk excavation | 0.45 | 400,000 | `0.45 × 400,000 = 180,000` |
| R3 Single-source supplier fails to deliver | 0.15 | 1,200,000 | `0.15 × 1,200,000 = 180,000` |
| R4 Installation productivity below assumed rate | 0.60 | 250,000 | `0.60 × 250,000 = 150,000` |
| R5 Currency movement on imported plant | 0.35 | 300,000 | `0.35 × 300,000 = 105,000` |
| **Sum of EMV** | | | `270,000 + 180,000 + 180,000 + 150,000 + 105,000 = 885,000` |

**Step 1 — what the EMV sum is and is not.** USD 885,000 is the probability-weighted total. It is not a
contingency: no single outcome equals it, and it says nothing about the spread. Its use is as a check on
the simulation, not as an answer.

**Step 2 — the simulation, as first run.** With impacts ranged and risks modelled as independent, the run
returns **P50 = 1,010,000** and **P80 = 1,340,000** (illustrative outputs from the ranges assumed for this
example). At the organisation's stated appetite, contingency is recommended at P80.

`Uplift over the EMV sum = 1,340,000 − 885,000 = 455,000`, which is
`455,000 ÷ 885,000 = 0.514 = 51.4 %` above the EMV sum

That relationship is explicable: the P80 sits above the mean because the register contains a low-probability,
high-impact entry (R3) that stretches the upper tail. If a simulation ever returns a P80 *below* the EMV
sum, something is wrong with the model, not with the register.

**Step 3 — testing the correlation assumption.** R2 and R4 are both ground and productivity related: if the
excavation is harder than assumed, installation productivity is likely to suffer too. The analyst re-runs
with a correlation of 0.30 between them, leaving every other input untouched. The result:

**P80 = 1,455,000**, a movement of `1,455,000 − 1,340,000 = 115,000`, or
`115,000 ÷ 1,340,000 = 0.086 = 8.6 %` of the original recommendation

**Step 4 — read what that means.** A single assumption, invisible in the output and made once during
set-up, moved the contingency recommendation by USD 115,000 — more than the entire expected monetary value
of R5 (USD 105,000), which had a line in the register, an owner and a discussion at the workshop. The
correlation had none of those.

**Step 5 — what is reported.** A contingency recommendation of **USD 1,450,000** — the correlated run,
rounded to the nearest 50,000 because the inputs are judgement-based and do not support finer precision —
presented with: the confidence level and who set it; the correlation assumption and its effect; the share
of impact ranges that are owner-judged rather than measured; and a separate statement of the two risks that
could not be sensibly quantified and are therefore excluded from the figure. The recommendation is made by
the named risk manager; the release of any part of it is a decision for the project director against the
defined criteria.

**Assumptions this answer depends on.** That probabilities and impacts were confirmed with owners rather
than carried forward from the last update; that the impact ranges behind the simulation are as stated and
were not template defaults; that the P-values were stable across re-runs; that the 0.30 correlation is a
considered judgement about ground conditions and productivity rather than a demonstration figure; and that
no part of these five risks is also carried as an allowance inside the base estimate.

## 9. Checklist — before a quantified risk output informs a decision

1. **Register hygiene done.** Forward-looking entries, cause–event–effect wording, named owners, issues
   removed to the issues log.
2. **Top contributors confirmed with owners** in the current period, not inherited.
3. **Evidence class recorded** for every impact range: measured, quoted, judged or default — and the default
   share reported.
4. **Correlation stated and tested**, with the movement from a plausible alternative reported.
5. **Stability confirmed** by re-running and comparing the reported percentiles.
6. **EMV cross-check done**, and the relationship between the EMV sum and the chosen percentile explained.
7. **Double counting reconciled** across base estimate allowances, package contingency and the forecast.
8. **Exclusions listed** — every risk left out of the quantification and why.
9. **Precision honest** — rounded to what the inputs support, with the confidence level and its owner
   stated.
10. **Decisions attributed.** The confidence level, the recommendation and any release each carry a name.

---

## Related

- `AIG-04 — AI-assisted cost forecasting` — where contingency and forecast meet, and the double-counting
  check at §6.7.
- `AIG-05 — AI in scheduling — and what must not be automated` — the schedule ranges that feed quantitative
  schedule risk analysis.
- `BPG-16 — Risk registers that work` — the register discipline this document assumes.
- `BPG-17 — Quantitative schedule risk analysis` — the analysis technique, in method detail.
- `TPL-10 — Risk register` — the instrument, including the evidence-class and correlation fields §6 relies
  on.

## Sources and standards

- **ISO 31000:2018**, *Risk management — Guidelines*. In our own words: it describes risk management as an
  integrated, structured activity built on defined context, explicit criteria, and identification,
  analysis, evaluation and treatment steps, with information quality and human judgement treated as part of
  the process rather than as inputs to a calculation. Its insistence that criteria are set before analysis
  is the principle behind §5.
- **PCI Body of Knowledge, Domain 12** — *Risk management for project controls* (Institute manuscript,
  2026). Expected monetary value, quantification and contingency setting as taught by the Institute.
- **PCI Body of Knowledge, Domain 13** — *AI for project controls and project management* (Institute
  manuscript, 2026), Knowledge Area 13.5.9, the risk workflow and its warning about contingency set by an
  unexamined algorithm.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
