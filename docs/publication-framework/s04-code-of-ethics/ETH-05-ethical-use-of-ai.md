---
id: ETH-05
series: S04
series_name: Code of Ethics
title: The ethical use of AI and data
subtitle: Confidentiality, ownership and the duty to explain a number you submitted
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [student, practitioner, manager]
level: practitioner
reading_time_min: 17
summary: >
  The conduct obligations that attach to AI-assisted project controls work: keeping commercially
  sensitive and personal project data out of systems the data owner does not control, understanding
  who owns work an AI tool helped produce, and being able to explain — in your own words, without the
  tool open — any number you submitted under your name. Sets out the verification standard, what a
  material AI contribution is and how to disclose it, the short record that makes AI-assisted work
  defensible months later, and the deskilling risk that no policy can control for you.
linkedin:
  format: article
  hook: >
    If you cannot explain a number without the tool open — what it rests on, which assumption drives
    it, and what would have to be true for it to be wrong — you are not yet its author.
  tags: [ProjectControls, ResponsibleAI, DataEthics, CostEngineering, ProjectGovernance]
  asset: checklist-pdf
gated: false
related: [ETH-01, ETH-02, AIG-08, AIG-09, AIG-10]
bok_domains: [13]
sources:
  - "ETH-01 — The PCI code of ethics and professional conduct (this framework)"
  - "PCI Candidate AI-Use Policy, docs/downloads/candidate-ai-use-policy.md, repository copy read August 2026"
  - "PCI Code of Professional Conduct (predecessor), docs/downloads/code-of-professional-conduct.md, repository copy read August 2026"
placeholders: 2
---

# The ethical use of AI and data

> What you may put into a tool, who owns what comes out, and the number you have to be able to defend without it.

**In one paragraph.** This document sets out the conduct obligations that attach to AI-assisted
project controls work: keeping commercially sensitive and personal project data out of systems the
data owner does not control, understanding who owns work an AI tool helped produce, and being able to
explain — in your own words, without the tool open — any number you submitted under your name. It
covers the verification standard, what counts as a material AI contribution and how to disclose it,
the short record that makes AI-assisted work defensible months later, the handling of personal data
in analytics that affect people, and the deskilling risk that no policy can control on your behalf.

**Who this is for.** Practising cost engineers, planners, estimators and controls managers using AI
tools in live work, and the managers who sign off what those tools help produce.

---

## 1. What the Institute's rule actually allocates

*AI proposes; the professional disposes* is frequently read as a statement about how much automation
is appropriate. It is not. It is a statement about **where accountability sits**, and it settles one
question only: when an AI-assisted output turns out to be wrong, the answer to *who is answerable* is
the named human, every time.

That allocation is invariant. It does not shift with the sophistication of the tool, the vendor's
assurances, the organisation's procurement decision, or the fact that everyone in the market uses the
same product. `ETH-01 §10.6` puts it plainly: output carrying a certificant's name is theirs,
whatever produced it.

Three consequences follow, and the rest of this document is those three consequences in detail.

1. You are accountable for what you put **into** the tool — which makes confidentiality a
   first-order obligation rather than an IT policy (§3).
2. You are accountable for what comes **out** of it — which makes verification and the ability to
   explain a professional duty rather than a courtesy (§2).
3. You are accountable for **how it is understood** — which makes disclosure of a material AI
   contribution part of not misleading the reader (§5).

**What this document does not cover.** The technical governance of AI in a controls function — model
selection, data readiness, bias testing, auditability, the design of human-in-the-loop controls — is
owned by the S02 series and is not restated here. See `AIG-08 — Governing AI on a project`,
`AIG-09 — Bias, explainability and auditability` and `AIG-10 — Human in the loop`. This document
covers what the *individual* must do, and what happens to their standing if they do not.

---

## 2. The duty to explain a number you submitted

### 2.1 The obligation

`ETH-01 §10.3` requires a certificant to be able to explain, in their own words and without the tool
in front of them, what a submitted number rests on, how it was produced and why it holds. Inability
to do so is itself a breach — independently of whether the number is right.

That last clause is the one people argue with, so it is worth stating the reasoning. A number you
cannot explain is a number you cannot defend, cannot correct, cannot sensitise and cannot withdraw. It
is also a number you cannot tell is wrong, which means the review step everyone assumes has happened
has not happened. Correctness that you cannot demonstrate is indistinguishable, to the person relying
on it, from luck.

### 2.2 The four-question test

Before a number leaves you, answer all four without opening anything:

1. **What data does it rest on?** Which source, as at what date, covering what scope, with what known
   gaps.
2. **What method produced it?** Not the product name — the method. "A cost-performance-index-based
   extrapolation of remaining work" is a method; "the forecasting module" is a brand.
3. **Which assumption drives it?** The one input that, if wrong, changes the answer most.
4. **What would have to be true for it to be wrong?** If you cannot name a failure mode, you have not
   understood the output well enough to submit it.

A practitioner who can answer these four is the author of the number regardless of what produced the
first draft. One who cannot is transmitting, and `ETH-01 §5.1.1` prohibits knowingly transmitting
something you cannot stand behind.

### 2.3 Verification is proportionate, not uniform

`ETH-01 §10.2` sets the depth of check by the consequence of being wrong. A drafting suggestion for a
narrative paragraph needs a read. A completion forecast feeding a funding decision needs an
independent recomputation by a different method.

**Illustrative figures. Fictitious example, for method only.**

An AI-assisted forecasting tool proposes an estimate at completion of **CU 87,300,000**. The
independent check uses a different method — a cumulative cost performance index extrapolation — on
the same reported data:

```
BAC = 82,000,000     EV = 30,000,000     AC = 32,400,000
CPI = EV ÷ AC = 30,000,000 ÷ 32,400,000 = 0.926
EAC = BAC ÷ CPI = 82,000,000 × (32,400,000 ÷ 30,000,000) = 82,000,000 × 1.08 = 88,560,000
Difference = 88,560,000 − 87,300,000 = 1,260,000
1,260,000 ÷ 82,000,000 = 0.015 = 1.5 % of budget at completion
```

*Assumption stated with the answer:* the index method assumes remaining work performs at the
cumulative index to date, which is one defensible assumption among several.

The check has not failed. Two methods differing by 1.5 % of budget is unremarkable. **The
verification is complete only when you can say why they differ** — for example, that the tool weights
recent periods more heavily than the cumulative index does, and that recent performance has improved.
If you cannot account for the gap, you have found something, and the correct response is to
investigate rather than to pick the more convenient figure.

That last sentence is where verification most often fails in practice. Choosing between two numbers
on grounds of preference, having generated both, is `ETH-01 §5.1.3` — starting from the answer — with
extra steps.

---

## 3. Confidential data and AI tools

### 3.1 What actually leaves when you paste

`ETH-01 §10.5` prohibits entering confidential project, commercial or personal data into an AI tool
outside the data owner's control and policies. The obligation exists because pasting is a
**disclosure**, and disclosure without authority breaches `ETH-01 §9.2` whether or not anything
adverse follows.

Treat the following as unresolved unless your organisation has established otherwise for the specific
tool and the specific account: whether inputs are retained, for how long, and where; whether they are
used to improve the service; who inside the provider can access them; whether they are processed in a
jurisdiction your client's contract permits; and what happens to them if the provider is acquired or
fails. These are not paranoid questions — they are the questions your client's confidentiality clause
already asks of you, and the answers vary by product, by plan tier and by configuration.

### 3.2 What counts as confidential in this discipline

More than people assume. Rates and rate build-ups; margin and mark-up; productivity norms; bid and
tender positions; claims strategy and delay analysis; unpriced variations; contract terms;
supply-chain pricing; resource histograms detailed enough to reveal commercial capacity; and any
personal data at all. Also: the *existence* of certain things — that a project is in dispute, that a
package is being re-tendered, that a forecast has moved — which can be commercially significant on
its own.

### 3.3 Anonymisation is harder than it looks

Removing the client's name is not anonymisation. Project controls data is unusually identifying,
because the detail that makes it useful is the detail that makes it traceable: a scope description, a
country, a start year, a contract value and a package structure will frequently identify a project to
anyone in that market.

The working test in `ETH-01 §10.5` is residual: **where what remains would still identify the
project, the party or the person to a knowledgeable reader, it is still confidential.** If you need
AI help with a method, reconstruct the problem with invented figures at a plausible scale — which is
also what the Institute's candidate policy requires of people studying.

### 3.4 The practical test, and the honest follow-up

The test the Institute uses, and it works: **would you be comfortable if the data owner watched you
do it?**

If the answer arrives slowly, do not paste. If you have already pasted something you should not
have, the obligation is `ETH-01 §5.7` applied to a disclosure rather than a number: tell whoever owns
the data and whoever owns the incident process in your organisation, promptly, with what was
disclosed and when. This is uncomfortable and it is much less costly than the alternative, which is
the same conversation later with an additional question about why you did not raise it.

### 3.5 The organisational answer

Individual restraint is not a control. Where a controls function relies on AI, someone must decide
which tools are approved, on what account terms, with what data classification, and with what
enforcement — and that decision belongs to the organisation, not to each practitioner at the moment
of temptation. Where no such decision exists, raising the gap is itself a professional act (`ETH-01
§5.9`), and it is one of the more useful things a controls manager can put in front of a governance
board this year.

---

## 4. Who owns work an AI tool helped produce

Three separate questions get collapsed into one, and separating them resolves most of the confusion.

### 4.1 The legal question — which varies, and is not ours to answer

Whether, and to what extent, material generated with substantial AI assistance attracts copyright or
similar protection, and who owns it if it does, is a matter of national law. Treatment differs between
jurisdictions, the position is developing, and outcomes can turn on how much human authorship went
into the result. **This document does not state the law of anywhere** (`ETH-01 §3.3`). Where ownership
of a deliverable actually matters — a model, a database, a report you intend to license or reuse —
take advice from a qualified adviser in the relevant jurisdiction.

### 4.2 The contractual question — which is usually answerable today

Two contracts govern, and both are readable this afternoon:

- **The vendor's terms**, which state what rights you have in outputs, what rights the provider takes
  in inputs, and what the provider may do with either.
- **Your client or employer contract**, which typically assigns intellectual property in deliverables,
  may restrict subcontracting or processing by third parties, may require disclosure of tools used,
  and may prohibit transferring data outside defined boundaries.

The second frequently constrains the first. A client contract requiring that deliverables be produced
by named personnel, or prohibiting disclosure to third parties without consent, has something to say
about a tool that processes the deliverable — and it said it before AI was in scope.

### 4.3 The professional question — which does not vary at all

Whatever the law says and whatever the vendor's terms say, `ETH-01 §10.6` fixes the position for the
purposes of this Code: **the named human owns the output professionally.** Accountability is not
transferred by the vendor's disclaimer, not shared with the model, and not reduced by the tool's
limitations.

This has a practical edge that catches people out. If you cannot establish that you are entitled to
use the output — because the vendor's terms restrict it, or because your client's contract does — you
must not submit it as a deliverable. "I did not read the terms" is the same defence as "the model said
so", and `ETH-01 §10.3` has already disposed of it.

---

## 5. Disclosing a material AI contribution

### 5.1 When disclosure is required

`ETH-01 §10.4` requires the contribution to be visible where it is **material to the conclusion** —
that is, where a reviewer's assessment of the output would change if they knew. Working line:

| Usually not material | Usually material |
|---|---|
| Drafting or tidying narrative you then rewrote | A forecast, range or probability the tool generated |
| Formatting, summarising your own text | A risk score, classification or ranking driven by a model |
| Explaining a method to you before you applied it | A quantity, rate or allowance derived from a model's inference |
| Searching your own documents | Anything you could not reproduce by another route |

The distinguishing question is not how much the tool did. It is whether the reader, knowing, would
scrutinise the output differently.

### 5.2 What disclosure looks like

A sentence in the basis of estimate or the report's method note. Not a legal disclaimer, not a banner,
and not an apology:

> "Draft duration ranges were generated by an AI-assisted analysis of the historical package data
> listed in Appendix B, and were reviewed and adjusted by the planning lead; the correlation
> assumptions are the planning lead's judgement."

That sentence tells a reviewer exactly where to look — which is the entire purpose.

### 5.3 The AI-assist record

`ETH-01 §10.7` requires a short record where AI materially contributed to a submitted output. It is
four fields and it takes a minute:

| Field | Content |
|---|---|
| Tool and version | What was used, and its configuration if it matters |
| Inputs supplied | What data went in — and confirmation that it was permitted to go in |
| Check performed | The independent verification, its method, and its result |
| Named owner | The person accountable for the output, and the date |

The value of this record is entirely in the future. Nine months later, when a forecast is being
examined in a dispute, an audit or an appeal, the difference between a professional who can produce
this and one who cannot is the difference between an explanation and an argument.

### 5.4 Disclosure to the Institute

`[CONFIRM: whether PCI requires an AI-assistance declaration on CPD evidence, application evidence and appeal submissions, and in what form]`

The obligation that does not depend on that confirmation is `ETH-01 §11.1`: everything said to the
Institute must be true and complete, and evidence must be genuine. Evidence of professional work that
you did not perform is not evidence of your competence, whatever produced it.

---

## 6. Personal data, and analytics that affect people

Project controls increasingly touches data about people: timesheets, productivity by crew, competence
records, absence, access logs, and — in the examination context — proctoring material. Three
obligations apply, and they are easy to state and easy to overlook.

**Purpose.** Data gathered to control a project is used to control the project. Repurposing
productivity data into an assessment of named individuals is a new purpose, and it requires authority
(`ETH-01 §9.2`).

**Proportionality.** The question is not whether an analysis is technically possible but whether the
intrusion is proportionate to the control objective. Crew-level productivity analysis is normal
project controls; individual-level behavioural inference from the same data is a different activity
with different obligations.

**Consequence.** Where an analysis will affect a person's standing, pay or employment, a human must
be able to explain the basis of the conclusion to the person affected — which is `ETH-01 §10.3` with
the stakes raised. An adverse inference no one can explain should not be acted on.

The applicable data-protection framework differs by jurisdiction and, frequently, by the location of
the workforce rather than the head office. `ETH-01 §9.4` requires compliance with the framework that
applies; determining which one that is, in a multinational supply chain, is a question for your
organisation's data-protection function, not for the practitioner alone.

---

## 7. Deskilling — the obligation no policy can enforce

`ETH-01 §8.3` requires competence to be maintained. AI creates a specific and slow-moving threat to
it: a practitioner who has never performed the calculation cannot sense when the output is wrong, and
sensing that is most of what experience consists of.

This is a genuine conduct matter and not a nostalgic one, because it feeds directly back into §2.
The four-question test in §2.2 is only answerable by someone who understands the method. A
practitioner who has always had the method performed for them will pass the test by reciting the
tool's description of itself — which is exactly the failure the test was written to catch.

Two habits hold the line, and both are cheap. Perform the core calculations of your discipline by
hand or in a plain spreadsheet often enough that the method stays yours — not as a purity exercise
but as calibration. And when a tool gives you an answer, form your own expectation of the answer
*before* you look at it. Estimating first is what turns tool use into judgement rather than
acceptance.

The Institute's position on study is the same and is set out in the Candidate AI-Use Policy: use AI to
check and to explain, not to substitute; if the AI did the thinking, you have not learned it.

---

## 8. AI, the Institute and the examination

Three settings, one principle, different rules — and the difference is a frequent source of honest
confusion, so it is stated plainly.

| Setting | Position |
|---|---|
| **Preparation** | Encouraged, with honest-learning cautions and no confidential data |
| **Examination** | Prohibited, in any form, however obtained (`ETH-01 §11.2.3`) |
| **Professional practice** | Governed — verified, explainable, disclosed where material, owned |

The examination prohibition does not rest on detection, and the Institute does not claim that
invigilation technology catches everything. It rests on what the credential is for: a pass obtained
with a tool's help would certify the tool.

`[CONFIRM: whether the Institute's AI standard is published as a separate policy document, and its exact title, so that this document can cite it rather than describe it]`

---

## 9. How this goes wrong

**Pasting first, thinking second.** The single most common breach in this area, and the one with the
least deliberation behind it. It takes four seconds and cannot be undone.

**Verifying against the same source.** Checking an AI-produced forecast using the same tool, the same
data and the same method verifies nothing. Independence of method is the whole point.

**Accepting the confident answer.** Fluency is not accuracy. AI-generated project controls content is
frequently plausible and specifically wrong — a formula misremembered, a term misapplied, a standard
misdescribed — and the confident register makes it harder to catch, not easier.

**Disclosing everything, so that disclosure means nothing.** A blanket "AI tools may have been used"
on every document tells a reviewer nothing and buys no credit. Disclose where it is material and say
where.

**Treating the vendor's assurance as your verification.** A supplier's statement that a model is
accurate is a commercial position. `ETH-01 §10.2` requires *your* check.

**Assuming the organisation has decided.** Many practitioners assume an approved-tools position exists
somewhere. Where none does, the practitioner is making the policy — one paste at a time.

---

## 10. Two checklists

**Before you put anything in.**

- [ ] Is this data confidential, personal, or commercially significant by its existence alone?
- [ ] Is this tool approved by my organisation for this classification of data?
- [ ] Do I know whether inputs are retained, and where they are processed?
- [ ] Does the client contract permit processing by a third party?
- [ ] If I have removed names, would a knowledgeable reader still identify the project or person?
- [ ] Would I be comfortable if the data owner watched me do this?

**Before you submit anything out.**

- [ ] Can I state the data, the method, the driving assumption and the failure mode without opening
      the tool?
- [ ] Have I verified by an independent method, proportionate to the consequence of being wrong?
- [ ] Can I explain any difference between the two results?
- [ ] Is the AI contribution material — and if so, is it visible where a reviewer will find it?
- [ ] Have I recorded tool, inputs, check and owner?
- [ ] Am I entitled to use this output as a deliverable under the vendor's terms and my client's
      contract?

The last question in each list is the one people skip, and both of them are the ones that surface
nine months later — when the forecast is being examined by someone who was not in the room and has
only the record to go on.

---

## Related

- `ETH-01 — The PCI code of ethics and professional conduct` — §10 states the binding obligations this document explains
- `ETH-02 — The principles explained` — the reasoning behind the governed-AI principle
- `AIG-08 — Governing AI on a project: the control framework` — the organisational controls this document assumes exist
- `AIG-09 — Bias, explainability and auditability` — the technical treatment of explainability
- `AIG-10 — Human in the loop: what AI may and may not decide` — where the decision boundary is drawn

## Sources and standards

- `ETH-01 — The PCI code of ethics and professional conduct` (this framework) — §9, §10 and §11.2.3.
- PCI *Candidate AI-Use Policy*, `docs/downloads/candidate-ai-use-policy.md`, repository copy read August 2026 — the three settings in §8, the study cautions in §7, and the data-privacy test in §3.4.
- PCI *Code of Professional Conduct* (predecessor instrument), `docs/downloads/code-of-professional-conduct.md`, repository copy read August 2026 — Principle 8.

No external standard, vendor document, statute or study is cited, and none should be inferred. The
statements about variation in copyright and data-protection law are general observations for which a
qualified adviser in the relevant jurisdiction is the correct source; this document is not legal
advice. The worked example in §2.3 is illustrative and was computed for this document.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
