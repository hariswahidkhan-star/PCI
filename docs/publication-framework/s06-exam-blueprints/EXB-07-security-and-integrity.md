---
id: EXB-07
series: S06
series_name: Exam Blueprint
title: Examination security and integrity
subtitle: How the item bank is protected, what proctoring actually does, why a result under review shows no score, and what candidates are obliged to do
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [student, practitioner, manager, employer]
level: practitioner
reading_time_min: 15
summary: >
  Examination security exists to protect one thing: that a pass means the same for everyone who earns it.
  This document sets out how the PCL-AI item bank is protected and how leakage is detected, what the
  proctoring arrangements actually do and honestly cannot do, how the secure exam client is designed so
  that the server rather than the candidate's machine owns the clock and the scoring, what evidence is
  captured and how long it is kept, and why a result placed under integrity review displays no score and no
  pass or fail outcome. It states candidate obligations plainly, and it says what is not misconduct. It
  applies to the PCL-AI only; no examination blueprint exists yet for PFL-AI or PML-AI.
linkedin:
  format: article
  hook: >
    If your result goes under integrity review, our system shows you no score and no pass or fail — not
    even privately. That rule protects you as much as it protects the credential, and this is the full
    reasoning behind it, along with what our proctoring can and cannot actually do.
  tags: [ExamSecurity, Certification, ProjectControls, AssessmentDesign]
  asset: one-pager
gated: false
related: [EXB-04, EXB-08, CER-06, CER-07, ETH-01]
sources: []
placeholders: 1
---

# Examination security and integrity

> What is protected, how, what the protections honestly cannot do, and what happens when a result is questioned.

**In one paragraph.** Examination security exists to protect one thing: that a pass means the same for
everyone who earns it. This document sets out how the PCL-AI item bank is protected and how leakage is
detected, what the proctoring arrangements actually do and honestly cannot do, how the secure exam client is
designed so that the server rather than the candidate's machine owns the clock and the scoring, what
evidence is captured and how long it is kept, and why a result placed under integrity review displays no
score and no pass or fail outcome. It states candidate obligations plainly, and it says what is not
misconduct. It applies to the PCL-AI only; no examination blueprint exists yet for PFL-AI or PML-AI.

**Who this is for.** Candidates preparing to sit; employers relying on the credential; and anyone auditing
whether the Institute's integrity claims describe a system or an intention.

---

## 1. What security is actually protecting

Not the Institute's revenue, and not its dignity. The thing being protected is **the value of every
credential already issued.**

A candidate who obtains a pass by cheating has not only obtained something they are not entitled to; they
have taken a small amount of meaning away from every other holder's credential, and from every employer who
relied on it. That is why examination security is a duty owed to certificants rather than a defensive
measure against candidates, and why the Institute publishes its arrangements instead of leaving them vague.

There is a second, less obvious protection. A rigorous integrity process protects the **innocent** candidate
— the one whose broadband failed, whose flatmate walked through the room, or whose face was momentarily lost
by a camera. A system that cannot distinguish an anomaly from misconduct will eventually punish someone who
did nothing wrong, and the arrangements below are designed with that failure in mind as much as the other.

## 2. Protecting the item bank

The item bank is the examination's most valuable and most vulnerable asset. Five arrangements protect it.

### 2.1 Published and live content are separated absolutely

**No live examination item is ever published.** Not in a sample paper, not in a training partner's course,
not in a marketing document, not on request.

**No published item is ever scored.** The sample items in `EXB-08 — Sample items with rationale` are
authored for publication, marked as study material, and held in a separate store from the live bank. They
are written to the same blueprint, the same format and the same cognitive levels — which is what makes them
useful — and they are not, and never become, live content.

The separation runs in both directions on purpose. A body that publishes "retired" live items is training
candidates on the exact material its bank was built from, and a body that promotes a published item into the
bank has published its own answer key.

### 2.2 Access is controlled and logged

Access to live item content is restricted to the roles that require it — the Chief Examiner and Assessment
Lead, the subject-matter experts reviewing a specific item, and the systems that assemble forms. Privileged
actions in the platform are written to an audit log. Nobody has standing access to the whole bank because
their job title is senior.

### 2.3 Exposure is tracked and forms are assembled, not fixed

Every use of an item on a form is recorded. There is no single fixed paper: each form is assembled to the
blueprint from the bank, so two candidates sitting at the same time are not answering the same set of items
in the same order. An item used beyond its tracked exposure threshold leaves the scored bank whether or not
any leak is suspected — see `EXB-04 — How items are written, reviewed and retired` §10.

### 2.4 Leakage is detected statistically, not only by report

Waiting to be told about a leak is not a control. The bank's own statistics are the detector.

An item whose difficulty drifts steadily downwards — more candidates answering correctly, with no change in
the population and no change to the item — is behaving exactly as a leaked item behaves. So is an item whose
discrimination collapses while its difficulty falls, which is the signature of an item that weak candidates
have memorised. Continuous monitoring of item statistics (`EXB-04` §9) is therefore a security control as
much as a psychometric one.

Response patterns are also examined: implausibly fast correct responses, answer sequences shared across
unconnected candidates, and clusters of candidates performing far better on a subset of items than on the
rest of the form.

### 2.5 What happens on a confirmed or suspected leak

Retirement is **immediate and does not wait for proof.** An item under credible suspicion leaves the scored
bank at once, because the cost of retiring a sound item is small and the cost of scoring a leaked one is
paid by every candidate on the form.

Results already issued are not disturbed by a retirement. A candidate scored against the bank version in
force at their sitting keeps that result. The one exception is a confirmed mis-key, where affected results
are rescored and every affected candidate is notified — including those whose result improves and those for
whom nothing changes.

### 2.6 Reconstructability

Every scored attempt records the bank version and the answer-key version it was scored against, and an
immutable score snapshot is written at submission. A result can therefore be reconstructed exactly as it was
computed, years later, even if items have since been edited or retired. This is what makes a late appeal
answerable with evidence rather than with recollection — see `CER-07 — Results, scoring, appeals and
complaints`.

## 3. Delivery: what proctoring does

The PCL-AI is delivered online under live proctoring. Delivery arrangements, booking and the technical
requirements are governed by the published policies and summarised in `CER-06 — Online proctoring and the
secure exam client`; what follows is the integrity design.

### 3.1 Before the session starts

**Identity verification.** Valid government-issued photo identification is required, and the name on the
booking must match it exactly, character for character. Name mismatch is the most avoidable cause of
exam-day failure and the easiest to fix a week in advance.

**Room scan.** The candidate shows the workspace and its surroundings. The purpose is to establish that the
space is clear of notes, devices and second screens before the examination opens, not to inspect a home.

**System check.** The candidate's machine, camera, microphone, displays and network are checked against
requirements. This is run on the actual machine, network and room to be used, well before the day.

**Consent.** What is captured, why, and how long it is kept is disclosed and consented to before capture
begins.

### 3.2 The launch window and the launch code

The session opens **15 minutes before** the booked time, with a **30-minute grace period** after it. Outside
that window the session does not open.

The portal hands the desktop client a **short-lived, single-use launch code** — not a reusable credential.
The code is redeemed once, against the Institute's own service, and is worthless afterwards. This matters
for a specific reason: a stolen bearer token is a standing key to a candidate's account, and a redeemed
single-use code is nothing at all.

### 3.3 The server owns the clock and the scoring

Two design decisions that a candidate cannot see and that determine whether the examination can be
manipulated at all.

**The clock is the server's.** The client does not decide how much time remains. A heartbeat to the server
returns the canonical remaining seconds, and a forced submission at expiry is driven by the server, not by
the candidate's machine. Tampering with a local clock, suspending a process or disconnecting the network
does not create time.

**Scoring is never performed on the candidate's machine.** The client displays items and transmits
responses. It does not hold the answer key, and it does not compute a result. There is nothing on the
candidate's machine worth attacking for a better score.

**The client is pinned to the Institute's own host.** It will only talk to an allowlisted service over
HTTPS. A launch instruction pointing it somewhere else is ignored and the client refuses to start, which
removes an entire category of attack in which a candidate is directed to a service that will score them
generously.

### 3.4 Kiosk lockdown, and what it honestly cannot do

The desktop client applies user-space lockdown: it holds the foreground, blocks common hotkeys and
clipboard use, watches for prohibited processes, detects additional displays, and flags remote-desktop and
virtual-machine indicators.

**It runs as an ordinary application, not as part of the operating system, and it says so.** It cannot block
the operating system's own secure attention sequence. It cannot see a second computer sitting on the desk,
a phone under the table or a person out of shot. It cannot certify that a room is empty.

That honesty is deliberate, and it has a practical consequence: **the client's job is to detect and record,
not to make cheating impossible.** No consumer-hardware proctoring system can make cheating impossible, and
a body claiming otherwise is either mistaken or selling something. What the arrangements do is raise the
effort required, capture what happened, and put a trained human in front of the evidence.

### 3.5 What the session records

Integrity signals are time-stamped, streamed to the Institute's service and compiled into a session record.
The categories are: session lifecycle events; identity verification outcome and room-scan completion; focus
loss and regain; prohibited processes; display connection and removal; virtual-machine and remote-desktop
indicators; blocked clipboard and hotkey attempts; camera signals such as no face detected, multiple faces
or prolonged absence; audio signals such as speech or loud sound; network drops and restores; missed
heartbeats and client crash-resume; answer saves and submission; and any manual flag raised by a proctor.

Each signal carries a severity from informational through low, medium and high to critical.

### 3.6 An automated flag is not a verdict

This is the most important sentence in the section, and it describes how the platform is actually
configured.

**Proctoring and identity signals are audit-only by default.** They are recorded, they are compiled into the
session record, and they inform human review. Automatic invalidation of a result on the strength of a
proctoring signal or an identity check is an *optional* configuration that is switched off by default —
which means, in the arrangement the Institute operates, no candidate's result is destroyed by an algorithm.

A cat walking across a desk, a delivery at the door, a network drop in a country with unreliable
connectivity — all of these generate signals, and none of them is misconduct. The system's job is to notice;
a person's job is to decide.

## 4. Evidence, privacy and retention

Proctoring evidence is personal data captured under consent for a single purpose: deciding whether an
examination session was conducted properly.

**Access is need-to-know.** Session evidence is available to the people conducting a review, not to
administrators generally.

**It is used for that purpose only.** It is not used for research, marketing or product development, and it
does not inform any assessment judgement about the candidate's answers.

**It is retained for a defined period and then the artefacts are purged**, with the metadata record kept for
audit. The retention period is `[CONFIRM: retention period for proctoring evidence]`.

**Accommodation information is handled separately.** Where a candidate has an approved adjustment — extra
time, agreed rest breaks, assistive technology — the proctoring arrangements are configured in advance so
that a legitimate break is never flagged as an irregularity. The proctor knows an arrangement exists and
what is needed to run the session, not why it was granted, and the information is kept apart from
assessment judgements.

## 5. The held-result rule

**A result under integrity review shows no score, no pass or fail outcome, and no credential.** Not a
provisional score. Not a score marked "subject to review". Nothing.

The candidate sees a clear message: the responses were submitted successfully, the result is on hold pending
an examination integrity check, and the Institute will notify them through the portal when the review
concludes.

The rule is enforced in two independent places — the server redacts the score before it leaves the service,
and the desktop client's presentation layer refuses to display a score for a held submission regardless of
what any payload contains. Two enforcement points for one rule is deliberate: a rule this consequential
should not depend on a single piece of code being correct.

### 5.1 Why the rule is absolute

**A number, once seen, cannot be unseen.** Show a candidate 71 per cent and then invalidate the attempt, and
you have created a grievance that no amount of subsequent process will settle. The candidate will
reasonably feel something was taken from them.

**A provisional score prejudices the review.** Once a figure has been disclosed, everyone involved is
deciding whether to *withdraw* a pass rather than whether the session was sound. Those are different
questions and they do not produce the same answers.

**Partial disclosure invites reconstruction.** A candidate who is shown a domain breakdown but not a total,
or a score but not an outcome, can often infer the rest — and a candidate who can infer the outcome has been
told it.

**It protects the candidate as much as the credential.** A held result that resolves in the candidate's
favour is released as a normal result, and nothing about the review is visible on the credential or the
verification record. Had a provisional score been shown and then confirmed, the episode would follow the
candidate as an explanation they had to give for years.

### 5.2 What a hold is not

A hold is **not a finding of misconduct**, and it is not an accusation. Holds arise from technical
irregularities and process anomalies as well as from conduct questions. Most resolve without any adverse
finding at all.

## 6. How an integrity review works

Fair process is the point, and its elements are not negotiable.

**The candidate is told what is alleged**, specifically enough to respond to. "An irregularity was detected"
is not a case a person can answer.

**The candidate is given a genuine opportunity to respond**, in writing, with time to prepare, and may
provide their own account and evidence.

**The decision is taken by people with no conflict of interest** — not by the proctor who raised the flag,
and not by anyone with an interest in the outcome.

**The evidence is examined, not summarised.** A severity label is a pointer to a recording, not a
substitute for looking at it.

**Outcomes are proportionate**, and range from no action, through a technical re-sit at no fault to the
candidate, to invalidation of the result and, in the most serious cases, loss of the right to resit or
withdrawal of an existing certification.

**Adverse decisions carry a right of appeal**, reviewed independently of the people who made the original
decision. Raising an appeal never prejudices a candidate's standing. The procedure is in `CER-07 — Results,
scoring, appeals and complaints`.

## 7. Candidate obligations

Stated positively, because a list of prohibitions is easy to read and hard to follow.

**Sit your own examination, and let nobody else sit any part of it.** Impersonation is misconduct in both
directions — the person who sits and the person who is sat for.

**Present genuine identification** in the name the booking is made in.

**Work alone, with nothing but the interface.** No notes, no reference material, no formula sheet, no second
device, no second screen, no assistance from another person, and **no AI tool of any kind.** The examination
assesses whether you can do this without help; using an assistant to answer it produces a credential that
attests something untrue about you.

**Follow the proctor's instructions** throughout the session.

**Keep the content confidential, during and after.** Examination items are secured content. Reconstructing
them from memory, sharing them in a study group or a forum, soliciting them from someone who has sat, or
selling or buying them is misconduct however informal the setting and however long afterwards. This is the
obligation candidates most often breach without intending to — a well-meaning "here's what came up" post in
a study group is a breach, and it damages the credential the poster has just earned.

**Report a disruption immediately**, to the proctor during the session and to the Institute as soon as it
ends, while the detail is fresh. A contemporaneous report from the candidate is the single most useful piece
of evidence in a technical review, and it is nearly always in the candidate's favour.

## 8. What is not misconduct

Candidates worry about the wrong things, so the Institute says this explicitly.

Looking away from the screen to think is not misconduct. Speaking aloud while working through a calculation
is not misconduct. A network drop is not misconduct. Somebody entering the room without your knowledge is
not misconduct on your part. Requesting an accommodation has no bearing on how an examination is marked, in
either direction, and it does not increase scrutiny. Using approved scratch working, where the delivery
arrangements permit it, is not misconduct.

If something goes wrong, say so at the time. Candidates who report a problem immediately are in a
substantially better position than candidates who say nothing and hope.

## 9. How this goes wrong

**Believing that lockdown equals security.** Kiosk software is one layer, it runs in user space, and a body
that treats it as the whole control has stopped thinking. Detection, statistics, human review and the
integrity of the item bank all matter more.

**Automating the verdict.** The efficient design is to let a severity threshold invalidate results
automatically. It is also the design that will eventually destroy an innocent candidate's result and be
unable to explain why. The Institute's platform defaults to audit-only for exactly this reason.

**Showing a provisional score.** Almost every body that does this does it to be helpful, and it is the
single most damaging thing that can be done to a review's fairness.

**Publishing live items as practice material.** Usually requested with good intentions by a training partner
who wants realistic preparation. It converts the examination into a memory test and cannot be undone.

**Treating security as a candidate-relations problem.** Weak enforcement to avoid complaints transfers the
cost onto every honest candidate, silently, and the people who paid it never find out.

**Retaining evidence indefinitely.** Proctoring evidence is intrusive personal data captured for one
purpose. Keeping it because storage is cheap is a privacy failure that the passage of time makes worse, not
better.

**Vague allegations.** "An irregularity was detected in your session" gives a candidate nothing to answer
and makes a fair process impossible. Specificity is a fairness requirement, not a courtesy.

## 10. Checklist

For a candidate, before the day:

- [ ] The name on my booking matches my photo identification exactly, character for character.
- [ ] I have run the system check on the actual machine, network and room I will use — not a different one.
- [ ] Any accommodation I need is requested and approved in advance, not raised on the day.
- [ ] My workspace will be clear of notes, devices and second screens, and second monitors are disconnected.
- [ ] I know the launch window opens 15 minutes before my booked time and closes after the 30-minute grace period.
- [ ] I have read the conduct rules and understand that no AI tool of any kind may be used.
- [ ] I know that if anything goes wrong I tell the proctor immediately and report it to PCI the same day.
- [ ] I understand that examination content is confidential during and after the sitting.

For anyone auditing an examination body's integrity arrangements:

- [ ] Are published and live items provably separate, in both directions?
- [ ] Is exposure tracked, and does over-exposure retire an item without a leak being proven?
- [ ] Is leakage detected statistically, or only when someone reports it?
- [ ] Can any historical result be reconstructed exactly as it was scored?
- [ ] Is scoring performed server-side, with the clock owned by the server?
- [ ] Do automated proctoring signals inform human review, or do they decide?
- [ ] Is a result under review displayed without any score or outcome?
- [ ] Is the allegation put to the candidate specifically enough to answer?
- [ ] Is the deciding body independent of the person who raised the flag?
- [ ] Is proctoring evidence retained for a defined period and then purged?

---

Every control described here is a cost — to the Institute, and sometimes to a candidate's convenience. The
reason to carry it is that a credential is a claim made to third parties who cannot verify it themselves. An
employer relying on a PCL-AI is relying on the proposition that the person in front of them sat the
examination, alone, without assistance, against items nobody had seen. Everything above exists to make that
proposition true, and publishing it is how the Institute invites being held to it.

## Related

- `EXB-04 — How items are written, reviewed and retired` — exposure tracking, item statistics and retirement, from the psychometric side
- `EXB-08 — Sample items with rationale` — the published study items, and the separation that keeps them apart from the bank
- `CER-06 — Online proctoring and the secure exam client` — the candidate-facing delivery arrangements and technical requirements
- `CER-07 — Results, scoring, appeals and complaints` — how a held result is resolved and how an adverse decision is appealed
- `ETH-01 — The PCI code of ethics and professional conduct` — the professional obligations that outlast the examination

## Sources and standards

The delivery, proctoring and held-result arrangements described here are the Institute's platform
configuration and secure-exam client design as verified in August 2026. The certification framework is
developed with reference to ISO/IEC 17024 personnel-certification principles; PCI is **not accredited** by
ANAB, IAS or any other ISO/IEC 17024 accreditation body.

No external citation is claimed for this document.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
