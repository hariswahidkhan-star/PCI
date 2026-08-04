---
id: AIG-12
series: S02
series_name: AI in Project Controls Guide
title: The AI-literate controls professional
subtitle: Six capabilities, the five questions that interrogate an AI-produced number, and how the underlying craft is kept alive
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, student, manager, employer]
level: practitioner
reading_time_min: 13
summary: >
  AI literacy in project controls is not prompt technique. It is six observable capabilities — choosing
  when not to use AI, judging whether data is fit and safe, directing an output precisely, interrogating an
  AI-produced number, governing and signing it, and keeping the underlying craft alive. This document
  defines each behaviourally, gives the five questions that take an AI-produced number apart in the right
  order, sets out what evidence of literacy an employer can actually observe, and treats deskilling as the
  governance problem it is.
linkedin:
  format: carousel
  hook: >
    AI literacy in project controls is not knowing how to prompt. It is being able to take an AI-produced
    number apart and put it back together — and knowing when not to reach for the tool at all.
  tags: [ProjectControls, Competency, ProfessionalDevelopment, CostEngineering, ResponsibleAI]
  asset: carousel-8
gated: false
related: [AIG-09, AIG-10, AIG-02, CMP-08, CER-08]
bok_domains: [13]
sources:
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
  - "PCI candidate AI-use policy (Institute, 2026)"
placeholders: 1
---

# The AI-literate controls professional

> What an AI-literate practitioner can actually do, stated as behaviour a colleague could observe.

**In one paragraph.** AI literacy in project controls is not prompt technique. It is six observable
capabilities — choosing when not to use AI, judging whether data is fit and safe, directing an output
precisely, interrogating an AI-produced number, governing and signing it, and keeping the underlying craft
alive. This document defines each behaviourally, gives the five questions that take an AI-produced number
apart in the right order, sets out what evidence of literacy an employer can actually observe, and treats
deskilling as the governance problem it is.

**Who this is for.** Cost engineers, planners, quantity surveyors and controls analysts developing their
practice; the managers who assess them; and employers writing a role specification that has to mean
something.

---

## 1. What AI literacy is not

Three things are routinely mistaken for it, and all three are cheap to acquire and worth little.

**Prompt technique.** Knowing phrasings that produce better output is useful and shallow. It transfers
poorly between tools, dates quickly, and says nothing about whether the practitioner can tell a good
output from a plausible one.

**Tool familiarity.** Having used several products is experience, not competence. The person who has used
six tools and verified nothing has practised the wrong thing six times.

**A position on AI.** Enthusiasm and scepticism are both available without any underlying capability. The
professional question is never whether AI is good; it is whether *this* output, for *this* decision, can
be relied on, and the answer is arrived at by work.

What literacy actually consists of is unglamorous: knowing which tasks a class of tool can do, being able
to tell whether an output is right, and being able to say why you believe it. That capability is
domain-first. A practitioner who cannot forecast without AI cannot verify a forecast produced with it —
which is the whole hinge of this document.

## 2. Six capabilities

Stated as behaviour, because a competency written as knowledge cannot be assessed.

**1. Choose — including choosing not to.** Matches the task to a capability class (`AIG-02`) rather than
reaching for the tool at hand, and declines AI where a deterministic rule is more transparent, where
confidentiality cannot be assured, where the data will not support it, or where verification would cost
more than doing the work. Observable behaviour: the practitioner can name a task in their own work that
they deliberately do not use AI for, and give the reason.

**2. Prepare — judge whether data is fit and safe.** Assesses whether the data supports the question
(`AIG-03`) and whether it may lawfully and contractually be entered into the tool proposed. Observable
behaviour: they check the data classification before the first paste, not after the first result.

**3. Direct — specify an output precisely.** Constructs an instruction containing the six elements in §4,
including the instruction to decline rather than guess. Observable behaviour: their instructions state
what to do when the answer is not in the source.

**4. Interrogate — take an AI-produced number apart.** Applies the five questions in §3 in order, and
reaches a view about the number rather than about the tool. Observable behaviour: they can say what a
figure would have to have been for the claim attached to it to be true — the discipline the worked example
in §10 demonstrates.

**5. Govern — verify, record and sign.** Applies the verification tier, keeps the reconstruction record
(`AIG-09` §3), records what changed in review and why, and knows the four things a signature asserts
(`AIG-10` §6). Observable behaviour: an output they signed six months ago can be reconstructed from what
they wrote down.

**6. Maintain — keep the craft alive.** Works problems by hand often enough to retain the fluency
verification depends on, and can explain a method to a colleague without a tool in front of them.
Observable behaviour: they can produce the calculation on a whiteboard.

Capabilities 4 and 6 are the ones that distinguish practitioners. The first three are learnable in weeks.
The last two are built over years and lost quietly.

## 3. Interrogating an AI-produced number

Five questions, in this order. The order matters: each is cheaper than the one after it, and a failure at
any point makes the rest unnecessary.

**1. What are the inputs, and are they from source?** Take the inputs from the controlled system, not from
the model's restatement of them. Confirm the data cut. Most wrong answers are wrong here, and this check
takes minutes.

**2. Is it dimensionally and directionally sensible?** Right order of magnitude, right sign, right units,
right period. A forecast that moved the wrong way, a percentage over an unstated base, a figure that is a
factor of ten out — these are visible before any method question and are missed because nobody looks.

**3. What method produced it, and does the method match the situation?** Ask which method, not whether the
answer is right. A defensible method producing an uncomfortable answer is a finding; an undisclosed method
producing a comfortable answer is a risk. Where the method is not stated, reconstruct it — `AIG-09` §10
shows this being done in three lines of arithmetic.

**4. What must be true for this to hold?** Name the assumptions the answer depends on, and test the load
each one carries. The question that gets to it fastest: *what would have to be different for this number
to be materially wrong?*

**5. Does it reconcile with what I already know?** Against the prior period, against the schedule, against
commitments, against the physical work. A number that is internally coherent and inconsistent with the
project is still wrong.

Only after all five is there a professional judgement to make — and that judgement is about the project,
not about the model.

## 4. Directing an output

A precise instruction contains six elements. This is not prompt craft; it is specification, and the same
elements would be required of a graduate given the same task.

**Role and standard** — the discipline perspective and the standard of care expected. **Task** — the one
thing to produce. **Source** — the data or documents to work from, and the instruction to use nothing
else. **Constraints** — length, format, tone, and what must not be included. **Output shape** — the table,
fields or structure required, so the result is checkable rather than merely readable. **Refusal
instruction** — what to do when the answer is not in the source: say "not found", do not infer, do not
estimate.

The sixth element is the one practitioners omit and the one that most reduces risk. A model asked a
question it cannot answer from the source will produce a plausible answer unless told not to. Adding "if
it is not in the attached documents, say 'not found'" converts a silent fabrication into a visible gap.

## 5. What evidence of literacy looks like

For an employer writing a specification, or a manager assessing one, these are observable — unlike
"familiar with AI tools", which is not.

| Capability | Evidence a manager can actually see |
|---|---|
| Choose | Can name a task they decline to use AI for, with the reason |
| Prepare | Checks data classification before use; can say what preparation an analysis needed |
| Direct | Instructions include a source constraint and a refusal instruction |
| Interrogate | Has rejected or materially changed an AI output, and can say what the check was |
| Govern | A signed output from six months ago can be reconstructed from their record |
| Maintain | Can produce the underlying calculation without a tool |

The single most informative question in an interview or an appraisal is: **"tell me about an AI output you
rejected, and how you knew."** A practitioner who has never rejected one has either not been using AI or
has not been checking it, and the follow-up question distinguishes those cases in under a minute.

## 6. Building it

A practice sequence that produces artefacts rather than attendance. Each item is small; the sequence is
what does the work.

**Recompute before you accept, for one month.** Every AI-assisted number that crosses your desk, taken
apart by §3 before it is used. This is slow and it is where the instinct comes from.

**Keep an error log.** Every AI output you found wrong, what the error was, and which of the five
questions caught it. After twenty entries you will know which failure modes your tools actually have,
which no general guidance can tell you.

**Work one method by hand each month, unaided.** A forecast, a variance bridge, a network pass, a
reconciliation, built from source with no tool. This is deliberate practice, and its purpose is not the
answer.

**Write one specification a week.** Take a task you did conversationally and write it out with the six
elements of §4. Compare the outputs.

**Verify something outside your specialism, with the specialist.** A planner checking a cost forecast with
a cost engineer learns the questions faster than either would alone.

**Teach one method to someone junior, without a tool.** Teaching is a severe test of understanding, and it
is also how the next generation of verifiers is produced — see §7.

## 7. The deskilling problem

The governance model of this whole series rests on verification, and verification presupposes a verifier
who can still do the work: recompute the forecast, spot the wrong assumption, recognise an unrealistic
duration. Those instincts have always been built by doing the work — historically by junior practitioners
doing precisely the tasks that AI now does first.

Today's verifiers trained before AI. The honest position is that where tomorrow's come from is an open
question with no settled answer, and that a function which waits for the evidence to appear in its own
error rates has waited too long. This is not an argument against using AI. It is an argument for producing
deliberately what the daily workflow used to produce as a by-product.

Three counter-measures are within any function's reach, and all three are cheap relative to one
undetected forecast error.

**Rotation through first-principles work.** Defined periods in which developing practitioners build an
estimate, a schedule or a reconciliation from source, with the tools off, so judgement forms on the task
rather than on reviewing a draft of it.

**Verification taught as a skill in its own right.** How to recompute, how to ground an extraction, how to
challenge a causal claim — taught and assessed rather than assumed. A reviewer who only confirms is
already deskilled.

**Review capability tracked as a capacity constraint.** If only two people in a function can genuinely
verify a Tier 1 output, that is a resourcing fact with the same status as any other single point of
failure, and it belongs on the risk register rather than in a training plan.

## 8. Where this sits in the Institute's scheme

The Institute treats AI literacy as part of the discipline, not as an adjacent subject. Two of the fourteen
seeded competencies for the **PCI AI Project Controls Leader (PCL-AI)** credential are **Responsible AI**
and **Human validation**, alongside AI-enabled project controls, predictive analytics and automation —
they sit beside cost management and earned value rather than after them. A separate, open-entry offering,
the **AI in Project Controls — Specialist Certificate (AIPC)**, is drawn from Domain 13 of the PCL-AI Body
of Knowledge and is assessed by scenario items plus an applied AI task.

Currency is treated as a live obligation rather than a one-off. Recertification runs on a three-year cycle
with a mandatory AI-currency component, and the continuing professional development category that
satisfies it is named "AI currency" in the Institute's own records
`[CONFIRM: binding CPD hours per three-year cycle — the student portal shows a target of 30 hours, and the
binding requirement will be published with the recertification rules]`. The reason for treating currency
as mandatory is in §7 rather than in any regulation: capability that is not exercised does not persist,
and a credential that certified verification once is worth less each year that nobody checks whether the
holder still can.

Detail on how the competencies are levelled and evidenced belongs to
`CMP-08 — Data, digital and AI competencies in depth`; the recertification mechanics belong to
`CER-08 — Recertification, CPD and the AI-currency requirement`.

## 9. How this goes wrong

**Fluency mistaken for competence.** The practitioner is quick, confident and productive with the tools,
and has never taken an output apart. The gap is invisible until a plausible output is wrong.

**Verification taught as a checklist.** People learn to tick "source-checked" without knowing what
source-checking a forecast involves. The form is completed and the check is not performed.

**Interrogation stopped at question three.** The method is confirmed, the assumptions are never named, and
the number is signed with a load-bearing assumption nobody has stated.

**The specialist who cannot be checked.** One person owns the AI-assisted workflow, and nobody else in the
function can verify it. This is presented as expertise and is a single point of failure.

**Training on the tool instead of the discipline.** The programme covers the product's features and is
obsolete at the next release. Nothing was taught that survives a change of vendor.

**Practice replaced by attendance.** Hours are logged, artefacts are not produced, and no calculation was
worked by hand. Currency is recorded and not held.

**Rejection treated as obstruction.** A practitioner who declines outputs is seen as slowing delivery.
Within two cycles they stop, and the function loses the only signal it had that review was real.

## 10. Worked example — interrogating a causal claim

*Illustrative figures.* One control account, one period. Currency USD; figures cumulative to the data
date; percentages stated against the variance as base.

**Setup.** An AI-drafted variance narrative states: *"The unfavourable cost variance of USD 320,000 is
approximately 80 % driven by rate escalation on structural steel."* The reviewer applies question 5 of §3
— does it reconcile with what I already know — and has two facts from source: structural steel cost to
date is **USD 1,600,000**, and the escalation applied per the procurement record is **4.5 %**.

**Step 1 — compute the escalation effect.**
`escalation effect = steel cost to date × escalation rate = 1,600,000 × 0.045 = USD 72,000`

**Step 2 — express it against the variance.**
`share = 72,000 ÷ 320,000 = 0.225 = 22.5 %`

**Step 3 — test the claim from the other direction.** For escalation to be 80 % of the variance:
`0.80 × 320,000 = USD 256,000`, which would require an escalation rate of
`256,000 ÷ 1,600,000 = 0.16 = 16 %` — three and a half times the rate in the procurement record.

**Result.** The claim is wrong. Escalation accounts for **22.5 %** of the variance, not 80 %. The
remaining `320,000 − 72,000 = USD 248,000`, or **77.5 %** of the variance, is unattributed.

**Interpretation.** Note what the check cost: two multiplications and a division, using two figures the
reviewer already had. Note also what it produced. It did not merely correct a narrative — it revealed that
USD 248,000 of variance has no explanation, which is now the most important open item on the account and
which the drafted narrative would have closed off. This is the difference between reviewing text and
interrogating a number: the first fixes a sentence, the second finds the work. The assumptions this answer
depends on are worth stating with it — that steel cost to date is complete at the data date, and that the
4.5 % in the procurement record is the rate actually applied rather than the rate agreed.

## 11. Checklist

Use this on yourself, or on a role specification.

- [ ] I can name a task in my own work that I deliberately do not use AI for, and say why.
- [ ] I check the data classification before the first paste, not after the first result.
- [ ] My instructions name the source and say what to do when the answer is not in it.
- [ ] I take inputs from the controlled system, never from the model's restatement of them.
- [ ] I apply the five questions of §3 in order, and I get past question three.
- [ ] I can name the assumptions any number I signed depends on.
- [ ] I have rejected or materially changed an AI output in the last quarter, and I can say what the check was.
- [ ] An output I signed six months ago could be reconstructed from what I wrote down.
- [ ] I work one method by hand each month, unaided.
- [ ] I keep a log of AI errors I have caught, and which question caught them.
- [ ] My function has more than one person who can verify a Tier 1 output.

The capability that matters is not the ability to get a good answer out of a tool. It is the ability to
tell, quickly and for reasons you can state, that the good-looking answer in front of you is wrong — and
that capability is built by doing the work, which is now something a profession has to arrange on purpose.

---

## Related

- `AIG-09 — Bias, explainability and auditability` — the record that capability 5 produces, and the reconstruction discipline behind question 3
- `AIG-10 — Human in the loop: what AI may and may not decide` — what a signature asserts, and why review capability is a governance constraint
- `AIG-02 — What AI actually does in a controls function` — the capability classes capability 1 chooses between
- `CMP-08 — Data, digital and AI competencies in depth` — how these competencies are levelled and evidenced
- `CER-08 — Recertification, CPD and the AI-currency requirement` — the currency obligation referred to in §8

## Sources and standards

- PCI Body of Knowledge, Domain 13 (AI for Project Controls & Project Management), `docs/bok/` — explained in our own words, not reproduced.
- The Institute's candidate AI-use policy (`docs/downloads/`) — the position on AI in preparation, in the examination and in professional practice.

Competency and credential facts in §8 are taken from the Institute's own seeded competency sets and
published scheme documents. The continuing professional development requirement is not yet fixed and is
marked as a placeholder rather than stated.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
