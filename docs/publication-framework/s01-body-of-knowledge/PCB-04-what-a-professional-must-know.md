---
id: PCB-04
series: S01
series_name: Body of Knowledge — Executive Summary
title: What a project controls professional must know
subtitle: Eleven propositions the Body of Knowledge asserts, and what each one is evidenced by
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [student, practitioner, manager, employer]
level: professional
reading_time_min: 6
summary: >
  The Body of Knowledge is a syllabus; the credential is a claim. This document states that claim as
  eleven propositions about what a project controls professional must be able to do, gives the concrete
  test behind each one, names where it sits in the framework, and states plainly what is not being
  claimed.
linkedin:
  format: post
  hook: >
    Eleven things a project controls professional should be able to do without the tool, the template or
    the spreadsheet doing the thinking. Each one is testable in a ten-minute conversation.
  tags: [ProjectControls, CostEngineering, Competence, EarnedValue, Scheduling]
  asset: checklist-pdf
gated: false
related: [PCB-01, PCB-02, PCB-03, PCB-06, CMP-03]
bok_domains: [1, 2, 3, 5, 6, 7, 9, 10, 11, 12, 13]
sources:
  - "PCL-AI Body of Knowledge, first authored draft (docs/bok/), per-domain learning objectives, August 2026"
  - "PCI Canonical Facts (docs/publication-framework/00-framework/CANONICAL-FACTS.md), §7 PCL-AI competency set, verified August 2026"
placeholders: 0
---

# What a project controls professional must know

> Eleven propositions the Body of Knowledge asserts, and what each one is evidenced by.

**In one paragraph.** The Body of Knowledge is a syllabus; the credential is a claim. This document states
that claim as eleven propositions about what a project controls professional must be able to do, gives the
concrete test behind each one, names where it sits in the framework, and states plainly what is not being
claimed. A reader can use it to audit themselves, or to work out in ten minutes whether the person opposite
them can do the job.

**Who this is for.** Practitioners deciding whether to certify, controls managers assessing their own team,
and interviewers who want a better question than "tell me about your earned value experience".

---

## 1. Why propositions and not a syllabus

A syllabus lists what is covered. A claim states what a holder can do, in terms specific enough to be wrong.
The eleven below are written to be falsifiable: each names something a professional either can or cannot do,
with a test attached. Where a proposition is a rule it says so; where it is a judgement it says so too,
because that difference is most of what separates a competent professional from a well-trained one.

There are eleven propositions across thirteen domains and no one-to-one correspondence — several draw on
three or four domains at once. They are also not the PCL-AI competency set, which is fourteen named
competencies treated in `CMP-03 — PCL-AI: the fourteen competencies`. The propositions are what those
competencies look like when someone is doing the work.

## 2. The eleven propositions

**1. Read the ledger your numbers come from.** Not keep the books — read them. Know which posting created a
figure, which account it landed in, how the cost code maps to the work breakdown structure element you
report against, and how the statements articulate. *Test:* given a cost report that disagrees with the
general ledger by a material amount, name three plausible causes before opening anything. *Where:*
Domain 1 (KAs 1.1, 1.2, 1.5), Domain 11 (KA 11.3).

**2. Know when cost and revenue land, and recognise an obligation before anyone invoices it.** The accrual
and matching concepts are not accounting trivia; they decide which period a variance appears in. A
professional who reports on invoices received is reporting last month. *Test:* explain the difference
between an accrual and a provision, and say which one an anticipated contract loss is. *Where:* Domain 1
(KAs 1.3, 1.4 — IAS 37), Domain 2 (KA 2.5).

**3. Build an estimate with a stated accuracy class, and phase it into a baseline earned value can
measure.** An estimate without a stated class and basis is an opinion with decimal places. A budget that has
not been time-phased is not a baseline — it is a total. *Test:* state the estimating method used, its
accuracy class, and the three assumptions the number is most sensitive to. *Where:* Domain 3 (KAs 3.1, 3.2,
3.3).

**4. Control commitment, not spend.** By the time an invoice arrives the decision is months old. The
commitment-to-accrual-to-actual cycle is what makes cost controllable rather than merely reportable. *Test:*
given a purchase order raised but not yet delivered, say what appears in the cost report this month and what
does not — and defend it. *Where:* Domain 5 (KAs 5.2, 5.4).

**5. Compute the indices, and know what they cannot see.** `CPI` and `SPI` are cheap to produce and easy to
over-read. A schedule performance index converges on 1.00 as a late project finishes, whatever the delay; a
cost performance index says nothing about work not yet in the baseline. *Test:* name two situations in which
`SPI` above 1.00 is consistent with a project finishing late. *Where:* Domain 6 (KAs 6.1, 6.2, 6.4),
Domain 4 (KA 4.2).

**6. Choose a forecasting method and defend it against the cause of the variance.** This is a judgement, not
a rule, and it is the single most consequential judgement in the discipline — as `PCB-03 — Why 40/40/20`
demonstrates, it moves reported revenue. The index-based forecast assumes past performance continues; a
bottom-up estimate to complete does not. Which is right depends on whether the variance came from something
that is finished or something that is ongoing. *Test:* name the cause of the current variance, then say
which estimate-at-completion method that cause justifies, and why. *Where:* Domain 6 (KA 6.3), Domain 3
(KA 3.4).

**7. Build the network and compute the float without the tool.** A professional who cannot run a forward and
backward pass by hand cannot tell when the software is wrong, and scheduling software is confidently wrong
whenever the logic is. *Test:* on a ten-activity network, produce early and late dates, total and free
float, and the critical path — on paper. *Where:* Domain 10 (KAs 10.1, 10.2, 10.3).

**8. Read the contract for who bears the overrun, and reconcile billing, earned value and revenue to each
other.** Three numbers describe the same work: what has been certified, what has been earned at budget, and
what has been recognised at price. They are rarely equal, they should never be unexplained, and the
difference between them is the over- or under-billed position. *Test:* given the three figures, state the
contract asset or liability and say what it means for cash. *Where:* Domain 7 (KAs 7.1, 7.2, 7.4, 7.5),
Domain 2 (KA 2.2).

**9. Quantify risk into contingency, and keep management reserve separate.** Contingency is quantified,
owned by the project and drawn against identified risk. Management reserve is not, and is not the project
manager's to spend. Merging them destroys both the forecast and the governance. *Test:* explain how the
current contingency figure was derived and what event would justify a draw against it. *Where:* Domain 12
(KAs 12.1, 12.2, 12.3), Domain 3 (KA 3.1).

**10. Measure adaptive delivery as rigorously as predictive delivery.** Increasingly the software, systems
and design elements of large programmes run on short feedback cycles with evolving scope. A controls
professional who can only measure a fixed baseline is blind to that work, and "it's agile, so we don't
measure it" is a controls failure wearing a methodology's clothes. *Test:* say how you would report cost
performance and a completion forecast for a team working from a backlog. *Where:* Domain 9 (KAs 9.3, 9.5,
9.6), Domain 8 (KA 8.6).

**11. Use AI on controls work and remain accountable for the output.** The Institute's position: AI proposes;
the professional disposes. Nothing produced by a model enters a report until it is explainable, validated and
owned by a competent human — which requires knowing what the model was given, what it cannot know, and which
part of its output is the part that would be wrong. *Test:* take an AI-generated forecast narrative and name
the two figures you would verify first, and against what. *Where:* Domain 13 (KAs 13.2, 13.3, 13.5, 13.6).

## 3. How each proposition is evidenced

The propositions are assessed through four-option, single-best-answer items, mostly at the application and
analysis cognitive levels, in scenarios that cross domains deliberately — because propositions 6, 8 and 11
are not separable from the others in practice. No formula sheet is provided: selecting the right formula is
part of proposition 6.

The examination is a sample, not a census. It cannot observe proposition 7 done on paper, or proposition 11
done under time pressure with a real model. What it establishes is that the candidate knows what the right
answer looks like and why the plausible wrong ones are wrong. Employers wanting the rest should use the
propositions as an interview and development structure; `PCB-06 — Using the Body of Knowledge: a guide for
employers` sets out how.

## 4. What is not claimed

None of the eleven asserts experience, seniority or results. Eligibility is three years of professional
experience in any field, which establishes that a candidate has worked — not that they have worked in
project controls. A pass says the propositions were demonstrated against a published standard at a sitting;
it does not say they are being practised today, and it predicts no project outcome, salary or promotion.

Nor is the list a maturity model. Nothing claims a professional who can do all eleven is finished; judgement
deepens with exposure to work these propositions only describe.

## 5. How this goes wrong

**Treating the tests as examination content.** The tests in §2 are conversational probes for self-audit and
interviews. They are not sample items, are drawn from no item bank, and answering them well predicts nothing
about a sitting.

**Reading proposition 7 as nostalgia.** Nobody schedules a programme by hand. The proposition is about being
able to tell when the tool is wrong, which requires having done it by hand often enough to recognise a
result that cannot be right.

**Using the list as a job description.** A competence claim is not a role. A real role weights the
propositions by what the project needs — a graduate planner and a controls manager on the same programme
should be strong in different ones. That mapping is an employer's judgement, not the Institute's.

---

## Related

- `PCB-01 — The Project Controls Body of Knowledge — executive summary` — what the credential asserts and its four limits
- `PCB-02 — The thirteen domains at a glance` — the domains and knowledge areas cited against each proposition
- `PCB-03 — Why 40/40/20 — the weighting and what it claims` — why propositions 1, 2 and 8 carry the weight they do
- `PCB-06 — Using the Body of Knowledge: a guide for employers` — turning these propositions into interviews and development plans
- `CMP-03 — PCL-AI: the fourteen competencies` — the formal competency set these propositions express

## Sources and standards

- PCL-AI Body of Knowledge, first authored draft (`docs/bok/`), August 2026: the per-domain learning
  objectives, from which the propositions and their knowledge-area references are drawn.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  §4.4 eligibility, §7 the fourteen PCL-AI competencies, §10 the house formulations quoted in proposition 11.

IAS 37 is named in proposition 2 as the standard governing provisions; its principle is described in the
Institute's own words in the Body of Knowledge and is not reproduced here. No edition is cited, because none
was verified for this document.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
