---
id: AIG-10
series: S02
series_name: AI in Project Controls Guide
title: "Human in the loop: what AI may and may not decide"
subtitle: The decision boundary, the tests that generate it, and the authority mechanics that make it hold
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, executive]
level: professional
reading_time_min: 13
summary: >
  "Human in the loop" is an authority statement, not a posture. This document distinguishes the three
  positions a human can actually hold, gives five tests that place any controls decision on the human side
  of the line, sets out the decision boundary those tests generate, and covers the mechanics that make it
  hold in practice — delegated authority thresholds, what a signature means, how rubber-stamping starts and
  is detected, and what changes when AI runs several steps in a chain.
linkedin:
  format: article
  hook: >
    "Human in the loop" means nothing until you can say which decisions the human makes, at what threshold,
    and what they must have done before signing.
  tags: [ProjectControls, AIGovernance, DecisionRights, ChangeControl, ResponsibleAI]
  asset: one-pager
gated: false
related: [AIG-08, AIG-09, AIG-04, BPG-04, BPG-10]
bok_domains: [12, 13]
sources:
  - "PCI Body of Knowledge, Domain 13 — AI for project controls and project management (Institute manuscript, 2026)"
  - "PCI candidate AI-use policy (Institute, 2026)"
placeholders: 0
---

# Human in the loop: what AI may and may not decide

> Where the line sits between a proposal and a decision, and what has to be true for the human side of it to mean anything.

**In one paragraph.** "Human in the loop" is an authority statement, not a posture. This document
distinguishes the three positions a human can actually hold, gives five tests that place any controls
decision on the human side of the line, sets out the decision boundary those tests generate, and covers
the mechanics that make it hold in practice — delegated authority thresholds, what a signature means, how
rubber-stamping starts and is detected, and what changes when AI runs several steps in a chain.

**Who this is for.** Project controls managers, cost and planning leads, change board members and project
directors who hold delegated authority, and the practitioners whose names go on AI-assisted outputs.

---

## 1. The phrase does no work on its own

"There is a human in the loop" is offered as an assurance in almost every AI deployment discussion, and on
its own it assures nothing. A human reading a screen is in the loop. A human clicking "approve" on forty
items in six minutes is in the loop. A human who could not reproduce the number if asked is in the loop.

The phrase becomes a control when it answers three questions: **which decisions** does the human make
rather than confirm; **at what threshold** does the decision move to someone more senior; and **what must
the human have done** before their signature is worth anything. This document answers those three, in that
order.

The discipline-specific boundaries are owned elsewhere in this series: `AIG-04 — AI-assisted cost
forecasting`, `AIG-05 — AI in scheduling` and `AIG-06 — AI for risk identification and quantification`.
What follows is the general apparatus that generates those lists, and the authority mechanics none of them
covers.

## 2. Three positions, only one of which is what people mean

| Position | What it means operationally | Where it is acceptable |
|---|---|---|
| **In the loop** | The system proposes; a human evaluates and decides; nothing proceeds without that decision | Any output meeting a test in §3 |
| **On the loop** | The system acts; a human monitors, samples and can intervene | High-volume, low-consequence work with a measured error rate and an active monitoring duty (Tier 3, `AIG-08` §4) |
| **Out of the loop** | The system acts; no human reviews | Only where the action is trivially reversible with no external reliance or contractual effect — in controls, almost nothing |

The distinction is not philosophical. *In the loop* costs review time per item and is the only position
that gives you a decision-maker. *On the loop* costs monitoring effort and gives you a detection lag:
errors happen, and you find them at the sampling frequency. Choosing "on the loop" accepts a known error
rate — a legitimate professional choice, and only legitimate when the rate has been measured and the
acceptance recorded.

Most deployments described as "human in the loop" are, on inspection, on the loop. That is often the right
design. It becomes a governance failure when the framework claims the first and operates the second, so
the monitoring duty that makes "on the loop" safe is never resourced.

## 3. Five tests that place a decision

Apply these to any controls decision an AI tool touches. **One positive answer puts the decision on the
human side of the line.** They are deliberately blunt, because a boundary that requires fine judgement to
apply will not survive a busy month.

**1. Is it irreversible within the reporting cycle?** A baseline changed, a contingency released, a notice
served, a payment certified — none can be quietly undone next period. Reversibility, not size, is the
first discriminator.

**2. Does it have contractual or legal effect?** Anything that asserts, concedes, waives or triggers a
right. See `AIG-07 — AI in document control and correspondence` §3 for how this plays out in writing.

**3. Will someone outside the project rely on it?** A board, client, lender, auditor or certifier.
External reliance converts an internal analysis into a representation, and representations are made by
people.

**4. Is the question fundamentally *why*, and contested?** Attribution of cause — who caused the delay,
whether escalation was foreseeable, whether rework is a defect or a variation — is the substance of most
disputes. A model can assemble the evidence; it cannot hold a position.

**5. Does it affect a person?** Allocation, performance assessment, selection, capability judgement. This
attracts data-protection and employment obligations that vary by jurisdiction, and a decision made about
someone by a system nobody can explain is indefensible in every one of them.

One gating condition sits on top of the tests: **an output that cannot be reconstructed cannot carry any
of these decisions**, whatever its apparent quality — see `AIG-09` §8.

## 4. The boundary

The tests generate the following. This is the function-level list; the discipline documents refine it.

| Decision | AI may | A competent human must |
|---|---|---|
| **Baseline change** | Assemble the change pack, quantify time and cost impact, check the register for related items | Decide whether it is approved, on what scope and value, and sign the baseline revision (`BPG-04`) |
| **Contingency release** | Match the drawdown to the risk that materialised, compute remaining balance and burn rate | Decide the release, its amount and the authority level it requires (`BPG-10`, §5) |
| **Extension of time entitlement** | Assemble the chronology, extract notices and windows, run delay analyses on stated assumptions | Decide the entitlement position, the analysis method, and what is claimed or resisted (`BPG-12`) |
| **A forecast submitted to a board or client** | Produce candidate forecasts, decompose movement, flag inconsistency with the schedule | Choose the basis, state the assumption, own the number and sign (`AIG-04`) |
| **Revenue recognition, provisioning, accrual judgement** | Draft the working, run consistency checks, map facts to the policy | Make the judgement against the standard and the entity's policy, with the accounting owner |
| **Acceptance of a subcontractor's programme or valuation** | Check logic, compare against contract dates, reconcile quantities | Accept, reject or accept with reservation — acceptance is a commercial act |
| **The contingency level adopted** | Run the simulation, rank drivers, present the distribution | Choose the confidence level consistent with the organisation's appetite, and justify it (`AIG-06`) |
| **What is disclosed, and how it is framed** | Draft, check internal consistency, flag omissions | Decide what is said, what is caveated and what is not said |
| **Decisions about people** | Nothing that scores or ranks individuals without a specific, lawful, explainable basis | Decide, on evidence they can explain to the person affected |
| **Changing a rule of credit or measurement basis** | Model the effect of the change | Decide it, and disclose the discontinuity in the reported trend |

Two things this table is not. It is not a list of tasks to keep AI away from — every row has a substantial
AI contribution in the middle column, and refusing that contribution is its own failure. And it is not a
maturity statement that relaxes as tools improve: the tests in §3 concern the nature of the decision, and
a more capable model does not make a contested entitlement position less contested.

## 5. Thresholds: writing authority so it binds

A boundary without thresholds collapses into either paralysis or nothing. Delegated authority is the
mechanism, and three rules make it work with AI in the picture.

**Use the project's existing schedule.** Do not invent an AI-specific authority ladder. AI-assisted
decisions run through the same delegated authority as any other, expressed the same way — a value, a
percentage of budget at completion (BAC), or a category. A parallel ladder guarantees divergence and gives
an approver two answers to choose between.

**Express the threshold on the decision, not the tool.** "Contingency releases up to 0.25 % of BAC:
controls manager" is a control. "AI-assisted releases: controls manager" is not, because it invites
splitting a decision into an AI-assisted part and a human part to land in a lower band.

**Name the aggregation rule.** Thresholds are defeated by division. Related releases within a period, or
against the same risk, aggregate for the threshold — and the rule is written down, because the split is
rarely deliberate and always available. §10's worked example turns on this.

Add one AI-specific provision, and only one: **the approver must be able to state the basis of the
proposal in their own words.** If they cannot, it is not ready for approval — a rule that is cheap to
apply and catches the rubber-stamp before it becomes habitual.

## 6. What a signature actually asserts

A signature on an AI-assisted output asserts four things, and these are what it will be tested against.

1. **The inputs came from source**, not from the model's restatement of them.
2. **The method is known and appropriate**, and the reason for choosing it can be stated.
3. **The assumptions are stated** beside the answer, so a reader can challenge them.
4. **Differences between the model's output and the signed output are recorded**, with reasons.

"The model produced it" is not a defence, and neither is "the tool is approved" — the register governs
which tools may be used and says nothing about whether *this* output is right. The verification record of
`AIG-09` §3 is what the signature rests on.

The corollary is uncomfortable and worth stating plainly: **a reviewer who could not have produced the
output by another route is not in a position to sign it.** Not every signatory must redo every
calculation, but the capability must exist in the review, and where it does not the output goes to someone
who has it. This is why the deskilling problem in `AIG-12 — The AI-literate controls professional` is a
governance problem, not a training-department problem.

## 7. How rubber-stamping starts

It does not start with negligence. It starts with a run of correct outputs. The tool is accurate for
several cycles, review finds nothing, and review time falls because nothing rewards it. The reviewer
begins checking that the output looks reasonable rather than that it is right — a much faster operation,
indistinguishable from the outside. Then an output is wrong in a way that looks reasonable, and it passes.

Three detections, none expensive.

**Review time per output.** If time booked to verification has fallen materially while volume has not,
that is the signal. It is measurable if verification is a costed line (`AIG-08` §10), invisible if not.

**Change rate.** If no AI-assisted output has been materially changed in review for several cycles, either
the tool is excellent or the review is not happening. Both are worth knowing; only one is being assumed.

**Spot reconstruction.** Once a quarter, pick a signed output at random and ask its signatory to
reconstruct it (`AIG-09` §1). It is the only test that distinguishes the two cases above, and it takes
under an hour.

## 8. When AI runs several steps

Chained and agentic systems — where the tool retrieves the data, computes, drafts and assembles without
stopping — change the shape of the control, not the principle.

A single-step tool that errs produces one wrong output and one verification catches it. A chain that errs
at step two carries the error forward, and every later step builds on it plausibly. The error is not
merely propagated; it is polished. By the final output, the workings that would have exposed it are gone.

The response is to move verification **from per-output to per-workflow**. The professional assures the
design of the chain — which steps are permitted, which data each may touch, where it must stop — and
inserts checkpoints where a consequential intermediate result is inspected before the chain continues.
Sensible checkpoints in controls: after extraction and before computation (is this the right cut?), after
computation and before narrative (do the numbers reconcile?), and before anything leaves the function.

The audit trail must now record the chain, not just the answer. The decisions in §4 do not move: a chain
may prepare a contingency release pack end to end, and the release is still decided by the person with the
authority.

## 9. How this goes wrong

**The loop with no decision in it.** The workflow includes a human step, and that step is confirmation
rather than decision. Nobody has ever declined. The control exists on the diagram.

**Authority set on the tool rather than the decision.** The threshold reads "AI-assisted analyses require
manager approval". A decision is split into a model-produced analysis and a human-entered figure and lands
below the line without anyone intending it.

**Thresholds defeated by division.** Four contingency releases of 90,000 against the same risk in one
month, each within a lower authority. No aggregation rule, no visibility, no decision at the level the
total warranted.

**Signature without capability.** The signatory cannot recompute and has no access to someone who can
within the reporting timetable. The signature is a formality and everyone involved knows it.

**Boundary rewritten by capability claims.** A tool improves, and the argument follows that entitlement or
recognition judgements can now be automated. The tests in §3 concern the nature of the decision: nothing
about a better tool makes a contested cause uncontested.

**Reasonableness substituted for correctness.** Six good cycles, then review becomes a glance — invisible
until an output is wrong and reasonable at the same time.

**Chains verified only at the end.** The final pack is checked carefully and every intermediate step
trusted. An error introduced at extraction is confirmed by everything built on it.

**Escalation with no route.** The framework says contested items escalate; nothing says to whom, by when,
or what happens if the reporting deadline arrives first. Under pressure the item is signed.

## 10. Worked example — a contingency release and the authority it needs

*Illustrative figures.* One project, one month. Currency USD. Thresholds expressed as a percentage of
budget at completion.

**Setup.** Project BAC is **USD 48,000,000**. Contingency held is **USD 2,400,000** (5.0 % of BAC:
`2,400,000 ÷ 48,000,000 = 0.05`). The delegated authority schedule for contingency release reads: controls
manager up to **0.25 % of BAC**; project director up to **1.00 % of BAC**; sponsor above that. An
AI-assisted analysis matches a materialised risk to a proposed release of **USD 180,000** and produces the
supporting pack.

**Step 1 — convert the thresholds to money.**

- Controls manager `= 0.25 % × 48,000,000 = 0.0025 × 48,000,000 = USD 120,000`
- Project director `= 1.00 % × 48,000,000 = 0.01 × 48,000,000 = USD 480,000`

**Step 2 — place the decision.** `180,000 > 120,000` and `180,000 ≤ 480,000`, so the release is the
**project director's decision**, not the controls manager's.

**Step 3 — apply the aggregation rule.** Two releases of **USD 95,000** and **USD 70,000** were made
earlier in the same month against the same risk. Aggregated: `95,000 + 70,000 + 180,000 = USD 345,000`.
This is still within the director's band (`345,000 ≤ 480,000`) but it is now `345,000 ÷ 2,400,000 = 14.4 %`
of the total contingency drawn against a single risk in one month — a fact the director should be told and
would not have been told by three separate approvals.

**Step 4 — what AI contributed.** The tool matched the drawdown to the risk, computed the remaining balance
(`2,400,000 − 345,000 = USD 2,055,000`) and the burn rate, and assembled the evidence. It did not decide
that the risk had materialised, that the amount was right, or that the remaining contingency is adequate
for the risks that have not.

**Result.** A director-level decision, made on an aggregated view, with a remaining contingency of
**USD 2,055,000** against a residual exposure the director must now judge.

**Interpretation.** The arithmetic is trivial; the control is not. Without the aggregation rule of §5,
three defensible individual approvals produce an undisclosed concentration — each approver within
authority, nobody seeing the total. Note also what the example refuses to do: treat the model's confidence
in the match as a reason to lower the authority level. The threshold is a property of the decision's
consequence, and the consequence of an over-drawn contingency does not change with how the recommendation
was produced.

## 11. Checklist

- [ ] The framework states, per use, whether the human is in the loop or on the loop — and where it is on the loop, the error rate is measured and monitoring has a named owner.
- [ ] Every decision an AI tool touches has been run through the five tests in §3.
- [ ] The boundary in §4 has been adapted to this project's decisions and written into the plan.
- [ ] Thresholds are expressed on the decision rather than on AI involvement, and use the project's existing delegated authority schedule.
- [ ] An aggregation rule exists for related decisions within a period.
- [ ] Approvers can state the basis of a proposal in their own words.
- [ ] Signatories know the four assertions in §6, and outputs go to someone who can verify them when the first reviewer cannot.
- [ ] Review time and change rate are visible, and a spot reconstruction is done each quarter.
- [ ] Chained workflows have named checkpoints, and the audit trail records the chain.
- [ ] Escalation names a person and a deadline, and says what happens when the reporting date arrives first.

The line is not drawn to keep AI out of the work. It is drawn so that when a decision is questioned, there
is a person who made it, on grounds they can state — which is the only thing that has ever made a project
controls number worth anything.

---

## Related

- `AIG-08 — Governing AI on a project — the control framework` — the register, tiers and records this boundary is enforced through
- `AIG-09 — Bias, explainability and auditability` — why an unreconstructable output cannot carry a decision at all
- `AIG-04 — AI-assisted cost forecasting` — the forecasting-specific boundary these tests generate
- `BPG-04 — Baselining and baseline change control` — the change authority this document assumes exists
- `BPG-10 — Contingency and management reserve` — the underlying discipline behind the worked example

## Sources and standards

- PCI Body of Knowledge, Domain 12 (Risk Management for Project Controls) and Domain 13 (AI for Project Controls & Project Management), `docs/bok/` — explained in our own words, not reproduced.

Delegated authority schedules, accounting judgements and obligations relating to decisions about people
are governed by an organisation's own policies and by law that varies between jurisdictions. This document
describes the controls discipline and is not legal or accounting advice.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
