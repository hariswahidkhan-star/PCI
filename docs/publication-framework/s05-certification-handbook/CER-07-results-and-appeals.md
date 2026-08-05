---
id: CER-07
series: S05
series_name: Certification Handbook
title: Results, scoring, appeals and complaints
subtitle: How the outcome is decided, what the domain breakdown will and will not tell you, and how to challenge a decision
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [student, practitioner, employer]
level: practitioner
reading_time_min: 12
summary: >
  A PCI result is issued immediately on submission with a domain-by-domain breakdown, and is
  criterion-referenced: everyone who demonstrates the standard passes, regardless of how many others do.
  This document explains how the score is produced, why the configured 65 % pass mark and the
  standard-setting study still to come are both true statements, what happens when a result is held or
  invalidated, how the credential is issued and verified, and how to appeal a decision or raise a
  complaint through the two separate routes that exist for them.
linkedin:
  format: article
  hook: >
    Criterion-referenced means no quota and no curve: everyone who demonstrates the standard passes,
    however many others do. Here is what that changes about how a result is decided.
  tags: [Certification, Assessment, ProjectControls, Governance]
  asset: one-pager
gated: false
related: [CER-01, CER-06, CER-08, ETH-06, EXB-05]
sources:
  - "PCI platform result-lifecycle, credential and appeals records (backend/schema.sql; Core/Lifecycle.cs), verified August 2026"
  - "PCI Candidate Handbook (docs/downloads/candidate-handbook.md), 2026"
  - "PCI Publication Framework — CANONICAL-FACTS.md §§4.1, 4.2, 8, August 2026"
placeholders: 2
---

# Results, scoring, appeals and complaints

> What the number means, what it cannot mean, and what to do when you think a decision is wrong.

**In one paragraph.** A PCI result is issued immediately on submission with a domain-by-domain breakdown,
and is criterion-referenced: everyone who demonstrates the standard passes, regardless of how many others
do. This document explains how the score is produced, why the configured 65 % pass mark and the
standard-setting study still to come are both true statements, what happens when a result is held or
invalidated, how the credential is issued and verified, and how to appeal a decision or raise a complaint
through the two separate routes that exist for them.

**Who this is for.** Candidates about to sit or just finished; unsuccessful candidates planning a resit;
credential holders whose status is under review; and employers verifying a credential they are relying on.

---

## 1. How the result is produced

### 1.1 Scored by the server, always

Scoring happens on PCI's servers. It never happens on your machine, in the browser or in the desktop
client, whichever route you sit by. The answer key is versioned, the item bank version is recorded against
your attempt, and a re-scoring can therefore be reproduced exactly.

### 1.2 Immediate, with a breakdown

On submission you receive your score, a pass or fail outcome, and a **domain-by-domain breakdown** showing
where you performed above, at or below target. For a successful candidate the breakdown is a professional
development map. For an unsuccessful one it is the map for a targeted resit.

### 1.3 Criterion-referenced, not graded on a curve

**Everyone who demonstrates the required standard passes, regardless of how many others do.** There is no
quota and no fixed proportion who succeed or fail. Your result is not affected by the cohort you happen to
sit alongside, by the day you sit, or by how the last hundred candidates performed.

The corollary is the part candidates like less: a near-miss is not rounded up because the paper was hard.
A criterion-referenced examination cannot make that concession without abandoning the thing that makes it
worth passing.

### 1.4 The pass mark: two true statements

**65 % is the pass mark configured in the platform today.** And **the definitive standard will be set by a
modified-Angoff standard-setting study**, in which qualified subject-matter experts judge, item by item,
what a just-competent candidate would be expected to achieve.

Both statements are true simultaneously, and the Institute publishes both. A body that had already run the
study would say so; a body that had not, and quoted a single number without qualification, would be
implying work it has not done. `EXB-05 — Standard setting: modified Angoff and the pass mark` explains the
method and what will change when the study reports.

### 1.5 What is not stated, and will not be invented

**No item count exists.** The number of items on the examination **will be confirmed by a formal job-task
analysis** — the study that establishes what the job actually requires and therefore what the examination
must sample. Until it reports, no count is published, and any figure quoted elsewhere did not come from
PCI. `EXB-06 — Job-task analysis: where the blueprint gets its authority` covers why the count follows the
analysis rather than preceding it.

---

## 2. Publication is immediate by default

Immediate publication is the default. Proctoring and identity evidence is collected for audit and does
**not** by itself delay a valid result: the automatic rules that could hard-block a result are switched
off by default and reviewed by people instead (`CER-06` §8).

So the normal case is the simple one. You submit; you see your result; the credential issues on a pass.

---

## 3. Reading the domain breakdown honestly

The breakdown tells you where your performance sat relative to target in each domain. It is the most
useful diagnostic you will get, and it repays being read carefully rather than emotionally.

Three cautions, offered as **professional judgement** rather than as rules:

1. **A domain band is not a precise measurement.** Each domain is sampled by a subset of the examination,
   and a subset carries more measurement noise than the whole. Treat a single weak domain as a signal
   worth investigating, not as a calibrated score.
2. **Weakness clusters.** A candidate weak in Domain 6 (earned value and forecasting) is frequently weak
   in Domain 3 (budgeting and forecasting) for the same underlying reason. Fix the reason, not the band.
3. **The comfortable domains are where preparation is usually wasted.** Practitioners are strong in two or
   three domains and thinner elsewhere; planners often under-prepare finance and accounting, cost
   engineers often under-prepare schedule logic. Spend your time in proportion to the published weighting
   and your weaknesses, not your confidence.

---

## 4. The result lifecycle

A result moves through recorded states, and it is worth knowing which one you are in.

| State | What it means for you |
|---|---|
| **Released** | Scored and published. On a pass, the credential issues. |
| **Held** | Under review for a genuine question about the validity of the attempt. **No score, no pass or fail and no domain breakdown are shown** until it resolves. |
| **Credential issued** | A pass has been converted into an issued credential with its own identifier. |
| **Invalidated** | The attempt has been set aside following a decision. Any credential issued from it is revoked. |
| **Reinstated** | An earlier invalidation has been reversed and the position restored. |

Two points follow. A held result is deliberately blank rather than provisional — showing a number that
might be withdrawn invites decisions the review may reverse (`CER-06` §9). And invalidation and
reinstatement are **decisions**, made by people, recorded with a reason, and appealable.

---

## 5. The credential

### 5.1 The certification decision

Passing the examination is the central requirement, not the whole of it. Certification is awarded after
eligibility, assessment, verification and PCI policy requirements are all met. The decision is independent
of any training you did or did not take.

### 5.2 What you receive

A **verifiable digital credential** with a **unique credential identifier**, a **downloadable
certificate**, and a downloadable score report. The credential carries an issue date and an expiry date
three years later.

### 5.3 Verification

Anyone holding the credential identifier can check its status through PCI's public verification.
Verification is **expiry-aware and status-aware**: it reflects an active credential, an expired one and a
revoked one differently. That is the point of it. A verification service that only ever says "yes" is a
brochure.

### 5.4 Using it accurately

The credential belongs to the named individual. It describes competence **at the point of assessment, and
thereafter under maintenance**. It may not be transferred, altered or used in a misleading way, and it
does not imply accreditation, endorsement, or any employment or salary outcome. Holders are bound by the
Code of Ethics and the professional conduct policy, including full accountability for AI-assisted work.

---

## 6. Appeals

### 6.1 What may be appealed

Any decision that affects you: an eligibility decision, an examination or result decision, an
invalidation, an accommodation decision, or a certification decision.

The platform recognises four case types, and choosing the right one gets your case to the right people
first time:

| Type | Use it for |
|---|---|
| **Result appeal** | A decision about the outcome of an attempt |
| **Invalidation appeal** | A decision to set aside an attempt or revoke a credential |
| **Complaint** | PCI's conduct, service or process (§7) |
| **Ethics** | A concern about professional conduct — yours, another holder's, or the Institute's |

### 6.2 Who decides

An appeal is reviewed **independently of the people who made the original decision**, and is considered by
the **Appeals Panel**, a standing governance body distinct from those that make certification decisions.
**Raising an appeal never prejudices your standing** — not with the panel, not with the Institute, and not
in any future application.

### 6.3 How to raise one

Submit the appeal from your account, stating the decision you are appealing and your grounds, and attach
supporting evidence where you have it. A case is recorded as submitted, moves to under review, and closes
as **upheld**, **dismissed** or **withdrawn**, with the decision and its date recorded against it.

### 6.4 Timescales

`[CONFIRM: the deadline for submitting an appeal after the decision being appealed]` and
`[CONFIRM: the published acknowledgement and decision timescales for appeals and complaints]`.

Until those are published, submit promptly and keep your own record of when you did. This handbook will
not invent a number that a candidate might rely on and then be held to.

### 6.5 What an appeal is and is not

An appeal asks whether the decision was correctly made under the published rules. It is **not** a request
for a second opinion on the same evidence, **not** a route to have the standard adjusted for one
candidate, and **not** a substitute for a resit. State grounds, not disappointment: what rule you say was
misapplied, and what evidence supports that.

### 6.6 Misconduct outcomes

Where an appeal concerns alleged misconduct, the investigation process and the range of outcomes are
governed by the published misconduct procedure and are owned by `ETH-06 — Raising concerns: complaints,
investigations, sanctions`. What this handbook commits to is the process: you are told what is alleged,
you are given a genuine opportunity to respond, the people deciding have no conflict of interest, and an
adverse decision carries a right of appeal.

---

## 7. Complaints

A **complaint** is about PCI's conduct, service or processes rather than about a decision that changed
your status. It is a **separate route with separate people**, deliberately, so that neither kind of
concern is decided by those it is about.

Use a complaint where a process failed you — an unanswered request, a session run badly, information that
turned out to be wrong. Use an appeal where a decision went against you. If you are unsure, say so in the
submission; getting the route wrong delays a case but does not forfeit it.

---

## 8. Support cases are not appeals

Routine problems — a booking that will not open, a failed upload, a payment that did not register — go
through support, which is faster and better suited to them. Raising a support case does not preserve an
appeal deadline, and raising an appeal is not the way to get a password reset. Use `CER-03` §5.1 to
self-diagnose first: most "urgent" cases before a sitting are an outstanding consent or an identity
document.

---

## 9. How this goes wrong

- **Reading a held result as a fail.** It is neither a pass nor a fail. There is no number yet. §4.
- **Expecting a curve.** Criterion-referenced means the standard does not move for the cohort or for a
  hard paper. §1.3.
- **Quoting 65 % as the final standard, or the standard-setting study as evidence that 65 % is wrong.**
  Both statements are true. §1.4.
- **Repeating an item count from somewhere else.** None exists. §1.5.
- **Appealing a result because it was disappointing.** State grounds under the published rules. §6.5.
- **Missing a deadline while waiting for a support ticket.** §8.
- **Announcing a pass before the credential issues.** The certification decision follows the result. §5.1.
- **Assuming an expired credential still verifies as active.** Verification is expiry-aware, and
  employers use it. §5.3.

---

## 10. Writing an appeal that can be decided

An appeal that a panel can act on contains, in this order:

- [ ] The decision you are appealing, identified precisely — the attempt, the booking or the case
      reference, and its date
- [ ] The date you received the decision
- [ ] Your grounds, stated as **which rule or process you say was misapplied**
- [ ] What actually happened, in sequence, with times
- [ ] Evidence, attached: screenshots with visible timestamps, the support case reference, correspondence
- [ ] Anything you reported at the time, and when you reported it
- [ ] What outcome you are asking for
- [ ] Your contact details and credential or candidate identifier

Two things to do before you submit: check that the outcome you are asking for is one the panel can
actually give, and check that everything you assert is something you could evidence if asked.

---

## Related

- `CER-01 — Certification handbook — master` — the journey this outcome concludes
- `CER-06 — Online proctoring and the secure exam client` — what triggers a hold, and what evidence exists
- `CER-08 — Recertification, CPD and the AI-currency requirement` — what happens after the credential issues
- `ETH-06 — Raising concerns: complaints, investigations, sanctions` — the investigation process and the range of outcomes
- `EXB-05 — Standard setting: modified Angoff and the pass mark` — how the definitive standard will be set

## Sources and standards

- PCI platform result-lifecycle, credential and appeals records, verified August 2026 — the result states
  in §4, the four appeal case types in §6.1 and the case statuses in §6.3.
- PCI Candidate Handbook (`docs/downloads/candidate-handbook.md`), 2026.
- PCI Publication Framework, `CANONICAL-FACTS.md` §§4.1, 4.2 and 8, August 2026 — including the governance
  bodies named in §6.2.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
