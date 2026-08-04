---
id: PCB-06
series: S01
series_name: Body of Knowledge — Executive Summary
title: Using the Body of Knowledge: a guide for employers
subtitle: Role design, hiring and development built on a published framework rather than a job-advert template
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [manager, executive, employer]
level: leader
reading_time_min: 8
summary: >
  What a hiring manager can and cannot conclude from a PCL-AI credential, and how to use the underlying
  thirteen-domain framework for three jobs it does well: writing a role specification that describes
  competence rather than tools, structuring an interview that tests judgement, and assessing a controls
  function's capability gaps with a weighted coverage measure. Includes a worked gap assessment and a
  usable checklist.
linkedin:
  format: document
  hook: >
    Most project controls job adverts specify software. A published body of knowledge lets you specify
    competence instead — and lets you measure what your function is actually missing.
  tags: [ProjectControls, Hiring, RoleDesign, CapabilityBuilding, PMO]
  asset: one-pager
gated: false
related: [PCB-01, PCB-02, PCB-04, CMP-10, CER-04, CAR-07]
bok_domains: [1, 2, 3, 5, 6, 7, 9, 10, 11, 12, 13]
sources:
  - "PCL-AI Body of Knowledge, first authored draft (docs/bok/), thirteen domains and sixty-one knowledge areas, August 2026"
  - "Examination Blueprint (docs/downloads/examination-blueprint.md), §2 group weights and §6 limits, August 2026"
  - "PCI Canonical Facts (docs/publication-framework/00-framework/CANONICAL-FACTS.md), verified August 2026"
placeholders: 2
---

# Using the Body of Knowledge: a guide for employers

> Role design, hiring and development built on a published framework rather than a job-advert template.

**In one paragraph.** This document explains how to use the thirteen-domain framework behind the PCL-AI
credential for three jobs it does well: writing a role specification that describes competence rather than
tools, structuring an interview that tests judgement, and assessing a controls function's capability gaps
with a weighted coverage measure. It includes a worked gap assessment and a checklist for a resourcing
meeting.

**Who this is for.** Heads of project controls, PMO leads, engineering and commercial directors, and the
hiring managers and HR business partners who write the specification and sit on the panel.

---

## 1. What the credential tells you before you use the framework

A PCL-AI holder has demonstrated competence against a published thirteen-domain framework, at a published
standard, in one ninety-minute examination scored criterion-referenced rather than on a curve. The
propositions they claim to be able to act on are in `PCB-04 — What a project controls professional must
know`; the limits on that claim — point-in-time validity, no accreditation, no outcome guarantee, and the
separation from membership grades — are set out in `PCB-01 — The Project Controls Body of Knowledge —
executive summary` §5, and an employer should read them before relying on the credential.

Three points bear specifically on a hiring decision. **Eligibility is three years' professional experience
in any field**, not three years in project controls — sector and domain experience remain yours to assess.
**One route requires no examination**: of the three published routes, standard, founding member and honorary
fellow, the honorary route involves no sitting, so verify which route earned a credential that matters to a
decision (`CER-04 — Routes into PCI: standard, founding, honorary`). And **PCI owns the standard and the
examination while Certuvo is the separate official training partner** — the certification decision is
independent of who prepared the candidate, or whether anyone did.

## 2. Using the domains as a role specification

Most controls specifications describe software, which selects for tool familiarity and produces a role
rewritten every time the tool changes. A published framework lets you specify the competence and treat the
tool as a training item.

The method takes about an hour. Mark each of the thirteen domains **required**, **supporting** or **not
applicable**; then for each *required* domain name the artefact the post-holder owns, the frequency at which
they produce it, and who reviews it. That last step turns a framework into a specification. "Domain 6
required" tells a candidate nothing; "owns the monthly earned value report across fourteen control accounts,
published to the project board by working day five, reviewed by the controls manager" tells them exactly
what they are applying for.

A pattern for three common posts — a starting point, not a standard:

| Post | Required domains | Supporting | Typical owned artefact |
|---|---|---|---|
| Cost engineer | 1, 3, 5, 6 | 2, 7, 11, 12 | Monthly cost report and commitment register |
| Planner / scheduler | 3, 6, 10, 12 | 8, 9, 5 | Baselined and progressed schedule, critical-path narrative |
| Controls manager | 3, 4, 5, 6, 10, 12 | all remaining | Integrated monthly report and the estimate at completion |

Two rules keep this honest. **Do not require all thirteen domains for every post** — a specification
requiring everything selects for candidates willing to overstate and leaves the role impossible to develop
into. And **do not use the examination group weights as a role weighting**: forty per cent examination
weight on finance is not forty per cent of a cost engineer's week.

## 3. Using the framework in hiring

A domain map beats a competency form as an interview structure, because it produces questions with right
answers. Use one probe per required domain — the probes in `PCB-04` §2 are written for this and share a
shape worth copying: give a situation and a little data, then ask what the candidate would do next and why.
One who names the cause of a variance before selecting a forecasting method is demonstrating judgement; one
who names a formula is demonstrating recall.

Three additions earn their place on any panel. **Ask for the working, not the tool** — "show me how you
would find total float on this ten-activity network" cannot be answered from a template. **Ask one question
about being wrong** — "tell me about a forecast you defended that turned out optimistic: what did you miss,
and what changed afterwards?" There is no correct answer; you are assessing whether the candidate treats a
forecast as a position they own. **Ask one AI question and score the governance, not the enthusiasm** — "you
have an AI-generated variance narrative; which two figures do you verify before it goes to the board, and
against what?"

Do not ask candidates to reproduce examination content. Sample items are published study material; live
content is secured and never published.

## 4. Worked example — a capability gap assessment

*Illustrative figures. A management heuristic, not a PCI instrument — the Institute publishes no such
index.*

A controls function of nine people. For each of the thirteen domains the head of function records how many
of the nine can work **unsupervised** in it: one deliberately blunt judgement made in a single session, not
a survey.

**Recorded counts (people, out of 9).** Group A: D1 = 2, D2 = 1, D3 = 7, D4 = 6. Group B: D5 = 8, D6 = 5,
D7 = 4, D8 = 6, D9 = 2, D10 = 7, D11 = 1, D12 = 4. Group C: D13 = 1.

**Step 1 — unweighted coverage.** Competent person-domains `= 16 + 37 + 1 = 54`; slots `= 13 × 9 = 117`.
`Unweighted coverage = 54 ÷ 117 = 46.2 %`

**Step 2 — coverage by group**, as the mean count across the group's domains, over the nine people.
`Group A = (2 + 1 + 7 + 6) ÷ 4 = 4.000 → 4.000 ÷ 9 = 44.4 %`
`Group B = (8 + 5 + 4 + 6 + 2 + 7 + 1 + 4) ÷ 8 = 4.625 → 4.625 ÷ 9 = 51.4 %`
`Group C = 1 ÷ 1 = 1.000 → 1.000 ÷ 9 = 11.1 %`

**Step 3 — the weighted index**, at the published group weights of 0.40, 0.40 and 0.20.
`Index = (0.40 × 0.4444) + (0.40 × 0.5139) + (0.20 × 0.1111) = 0.1778 + 0.2056 + 0.0222 = 40.6 %`

The weighted index sits 5.6 percentage points below the unweighted 46.2 per cent because the thinnest
coverage — one person in nine in Domain 13 — is where a fifth of the framework's weight rests on a single
domain. The headline number flatters the function; the weighted one does not.

**Step 4 — two development options**, each adding four person-domain competencies.

*Option 1 — four people developed in Domain 13*, taking D13 from 1 to 5.
`Group C = 5 ÷ 9 = 55.6 %`; `Index = 0.1778 + 0.2056 + (0.20 × 0.5556) = 49.4 %`

*Option 2 — two people in Domain 1 and two in Domain 2*, taking D1 from 2 to 4 and D2 from 1 to 3.
`Group A = (4 + 3 + 7 + 6) ÷ 4 = 5.000 → 5.000 ÷ 9 = 55.6 %`; `Index = (0.40 × 0.5556) + 0.2056 + 0.0222 = 45.0 %`

Option 1 moves the index by 8.9 percentage points, option 2 by 4.4 — **exactly twice the movement for the
same training effort**, because the twenty per cent AI weight rides on one domain while the forty per cent
finance weight spreads across four.

**The warning that makes this usable.** That doubling is an artefact of the arithmetic, not a finding about
your team: the *index* is twice as sensitive to Domain 13, which is not to say Domain 13 training is twice
as valuable to your projects. If the function's live problem is a disputed revenue position, option 2 is the
right decision and the index is the wrong instrument. Figures are computed unrounded and shown to one
decimal place, which is why the option indices do not reconcile exactly to the sum of the rounded
increments. Use the index to find where you are thin, never to choose what to do about it.

## 5. Using the framework for development

**Find the single points of failure first.** A domain covered by one person is a resilience problem before
it is a capability problem. Here that is Domains 11 and 13 — and business process cycles is the one most
functions never think to develop, because it looks like finance's job until an invoice dispute proves
otherwise.

**Set development against artefacts, not courses.** "Understands earned value" is unassessable. "Produces
the monthly estimate at completion for two control accounts, reviewed by the controls manager, for three
consecutive months" has a completion test; `CAR-07 — Building a portfolio of evidence` covers how an
individual records it.

**Decide deliberately whether to sponsor certification.** An examination validates knowledge; it does not
substitute for the development above. Fees are route-dependent and discountable: `[CONFIRM: examination fee
— platform seeds USD 500, legacy candidate pack states USD 350]`. Budget also for recertification on a
three-year cycle with its mandatory AI-currency component; the student portal shows a target of thirty hours
of continuing professional development per cycle, but `[CONFIRM: the binding CPD requirement, to be
published with the recertification rules]`.

## 6. What not to use the framework for

**Not as a pay scale** — it says nothing about remuneration and PCI publishes no salary data; any figure
attributed to the Institute today is not ours. **Not to screen out non-certified candidates** — many
excellent controls professionals are certified by nobody, and the framework's hiring value is the structure
it gives your questions, which works identically on people who hold nothing. **Not as accreditation, to your
client** — "our team is PCI-certified" is accurate; implying accredited or government-recognised
certification is not. **Not for the sibling credentials** — this framework is PCL-AI only, and no
examination blueprint exists yet for PFL-AI or PML-AI.

## 7. Checklist

- [ ] Every open controls post has each of the thirteen domains marked required, supporting or not applicable
- [ ] Every required domain names the artefact, its frequency, and its reviewer
- [ ] No post requires all thirteen domains
- [ ] Examination group weights are not being used as a role or workload weighting
- [ ] The interview has one judgement probe per required domain, plus one "being wrong" question
- [ ] The AI question scores governance and verification, not tool enthusiasm
- [ ] Current team coverage is recorded per domain, with single points of failure flagged
- [ ] Development objectives are stated as artefacts produced and reviewed, not courses attended
- [ ] Anyone relying on a credential has verified it, including which route it was earned by
- [ ] No accreditation, recognition or outcome claim is being made on PCI's behalf

---

## Related

- `PCB-01 — The Project Controls Body of Knowledge — executive summary` — what the credential asserts and its limits
- `PCB-02 — The thirteen domains at a glance` — the domain map used for the specification method in §2
- `PCB-04 — What a project controls professional must know` — the probes to build an interview from
- `CMP-10 — Assessing competence — evidence, rubrics, moderation` — a rigorous alternative to the heuristic in §4
- `CER-04 — Routes into PCI: standard, founding, honorary` — why route verification matters in §1
- `CAR-07 — Building a portfolio of evidence` — the individual counterpart to the development plan in §5

## Sources and standards

- PCL-AI Body of Knowledge, first authored draft (`docs/bok/`), August 2026: the thirteen domains and
  sixty-one knowledge areas used throughout.
- Examination Blueprint (`docs/downloads/examination-blueprint.md`), August 2026: §2, the group weights
  applied in §4; §6, the blueprint's published limits, including that it guarantees nothing about outcomes.
- PCI Canonical Facts (`docs/publication-framework/00-framework/CANONICAL-FACTS.md`), verified August 2026:
  §2.1 accreditation wording, §4 examination configuration and recertification, §4.3 the unresolved fee
  conflict, §4.4 eligibility, §5 application routes, §6 membership grades.

ISO/IEC 17024 is named as the personnel-certification standard the framework is developed with reference to.
No edition, clause or wording is cited, because none was verified for this document. The capability
assessment in §4 uses illustrative figures and represents no real organisation or team.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
