---
id: AIG-07
series: S02
series_name: AI in Project Controls Guide
title: AI in document control and correspondence
subtitle: Drafting, transmitting and triaging project correspondence without conceding a position
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 13
summary: >
  Document control is where AI meets the contract. This guide separates the correspondence AI may safely
  draft from the correspondence it may not, names the words and omissions that concede entitlement, sets
  out which register fields a model may populate and which it may never touch, and shows how to use AI to
  triage incoming correspondence for notification windows without letting the model become the control.
linkedin:
  format: post
  hook: >
    An AI assistant drafts in a cooperative register by default — which is precisely the wrong register
    for a notice under a construction contract.
  tags: [ProjectControls, DocumentControl, ContractAdministration, ResponsibleAI]
  asset: checklist-pdf
gated: false
related: [AIG-08, AIG-10, AIG-12, BPG-11, BPG-12]
bok_domains: [7, 13]
sources: []
placeholders: 0
---

# AI in document control and correspondence

> What a language model may safely do inside a document control function, and where a drafted sentence becomes a contractual position.

**In one paragraph.** Document control is where AI meets the contract. This guide separates the
correspondence AI may safely draft from the correspondence it may not, names the words and omissions that
concede entitlement, sets out which register fields a model may populate and which it may never touch, and
shows how to use AI to triage incoming correspondence for notification windows without letting the model
become the control.

**Who this is for.** Document controllers, contract administrators, commercial managers and project
controls managers who own the correspondence register, the transmittal log and the submittal schedule —
and the project managers who sign what leaves the office.

---

## 1. Correspondence is an instrument, not admin

A project's correspondence file is evidence. Two years after the event, an adjudicator, an auditor or a
claims consultant reads it in date order and reconstructs what the parties knew, when they knew it and
what they agreed. Every letter is a contemporaneous record that will be read by someone whose interests
are opposed to yours.

So AI in document control is not the low-risk application it appears to be. The obvious framing — "it is
only paperwork, the risky AI is in the forecasting" — is backwards. A wrong forecast is corrected next
month. A letter saying *"we accept that the delay to the pump house arose from our sequencing"* is not
corrected next month; it is quoted in a claim.

Two properties of assistant-class language models make this specific rather than theoretical.

First, **they draft in a cooperative register by default.** Models tuned to be helpful produce prose that
concedes, softens, apologises and proposes accommodation, because that is what most correspondence in
their training material does. Contract administration requires the opposite instinct: reserve, state the
clause, record the fact, claim the right. The default is not neutral, and it does not read as wrong — it
reads as *reasonable*, which is exactly how it survives review.

Second, **they complete patterns.** Asked to draft a notice, a model produces something notice-shaped: a
clause number that looks right, a notification period that looks right, a recipient that looks right,
because those elements belong in the pattern. Whether any of them matches *your* contract is a separate
question the model cannot answer unless the contract is in front of it and grounded — see
`AIG-03 — Data readiness: what AI needs before it is any use`.

## 2. Where AI genuinely earns its place

The value in document control is mostly in *reading*, not writing. Four applications are worth the
governance they cost.

**Incoming triage at coverage.** A model reads every incoming item — letters, site instructions, requests
for information (RFIs), minutes, emails — and flags those that may start a clock, create an obligation or
signal a variation. Its value is coverage: it reads everything, every day, without fatigue. Its output is
a queue for a human, never a decision. See §6 for the arithmetic of how good a triage screen has to be.

**Register enrichment.** Populating descriptive metadata on a document register — subject, discipline,
originating party, referenced drawing numbers, referenced clauses, related transmittal — from the document
itself. This is extraction, the capability class AI is strongest at.

**Retrieval across the correspondence set.** "What did we write to the subcontractor about access to
level 3, and when?" answered from the actual correspondence, each answer cited to a document reference.
Ungrounded, this question invites a fabricated letter; grounded in the document set with citations opened
and checked, it replaces a day of searching — and it summarises a forty-item RFI chain into a chronology
a project manager can read before a meeting.

**Submittal and transmittal status reasoning.** Comparing the submittal schedule to the transmittal log
and flagging items that are overdue, superseded, or approved-with-comments and never resubmitted — the
least glamorous and most reliably useful item on the list.

Everything here is *proposal*. None of it is release.

## 3. Four classes of outgoing correspondence

Not all outgoing document is equally exposed. Classify it before you decide what AI may touch. The class
determines the drafting rule, not the author's confidence.

| Class | Examples | May AI draft? | Release rule |
|---|---|---|---|
| **A — Transmittal and routine** | Drawing transmittals, document issue sheets, distribution notes, acknowledgements of receipt with no substantive content | Yes, from the register data | Document controller checks the register fields against source; no free text beyond the standard form |
| **B — Technical and informational** | Covering letters to submittals, progress narratives to the client, meeting minutes, responses to technical queries with no commercial content | Yes, as a first draft | Discipline lead verifies technical content; controls manager checks no commercial or entitlement statement has entered the text |
| **C — Commercial** | Valuation covering letters, variation quotations, responses to a payment assessment, correspondence about rates or quantities | Draft only from a controlled template and only where the numbers come from a verified source | Commercial manager verifies every figure to source and signs; the AI's role is limited to assembling the controlled wording |
| **D — Notices and contractual positions** | Notices of delay, notices of a compensable event, claims correspondence, responses alleging breach, anything relying on a clause or reserving a right | **No** — not drafted by a model | Drafted by the person accountable for the position, on the contract's own wording, with legal review where the exposure warrants it |

The C/D boundary is where most functions get into difficulty, because a valuation covering letter can
become a contractual position in a single sentence. The practical rule: **if the letter relies on a
clause, asserts or resists an entitlement, characterises a cause, or agrees anything, it is class D**,
whatever the register calls it.

Class D is not a prohibition on AI in the vicinity. A model may retrieve the relevant clauses for the
drafter, assemble the chronology from the register, and check a human-drafted notice against a compliance
checklist. It may not produce the words that go out.

## 4. The sentences that concede

An AI-drafted letter concedes in ways that are easy to read past because they are grammatical, polite and
plausible. These are the recurring patterns worth training reviewers to catch.

**Admissions of causation.** *"Following the delay caused by our late issue of the reinforcement
drawings…"* The model wrote a causal link because the surrounding text implied one. Causation is the
contested issue in almost every delay dispute; it is settled by analysis, not by a subordinate clause in
a covering letter.

**Unqualified acceptance.** *"We accept the revised programme."* Accepting a programme can accept its
logic, durations and sequencing constraints, and can undermine a later position that the programme was
unachievable. The reserved form — *"we acknowledge receipt; our comments follow and our rights under
clause [x] are reserved"* — is a different letter.

**Apology as admission.** *"We apologise for the delay in returning the submittal."* Where the return
period is contractual and the delay is disputed, this sentence is a gift.

**Waiver by generosity.** *"In the interests of collaboration we will absorb the additional cost on this
occasion."* A model asked for a "constructive tone" produces this unprompted.

**Characterisation that defeats a claim.** *"This is a minor adjustment and we do not expect it to affect
the programme."* Said about work that later needs an extension of time (EOT), this is the sentence the
other side reads out first.

**Blanket no-cost-no-time confirmations.** *"We confirm there is no cost or time implication."* Correct
only where it has been assessed; a model produces it because it is a common closing formula.

**Defective notices.** Most notice regimes require a specific set of elements: the clause relied on, the
event, the date the party became aware, the effect claimed, and delivery by the contractual method to the
named recipient within a stated period. An AI-drafted notice typically reads well and satisfies three of
the five. A notice that reads beautifully and goes to the wrong address is void.

**Silent scope expansion.** A drafted RFI response that answers the question and then helpfully explains
what the contractor "should" also do has instructed work.

The reviewer's discipline is not "read for tone". It is: **read every verb whose subject is your own
organisation, and every sentence containing a causal connective, and ask whether you would sign it
standing in front of an adjudicator.**

## 5. The register: which fields a model may populate

A document register's fields are not equal. Split them, and put the split in the procedure.

**Model may populate, human samples:** subject line, discipline, document type, originating party,
referenced drawing and specification numbers, referenced correspondence, keywords, summary text, proposed
distribution list.

**Model may propose, human confirms every instance:** the reply-required flag, the response due date, the
contractual clause referenced, the link to a variation or claim reference, the risk or issue register
link.

**Human only, never model-populated:** the date of receipt of record, the notice classification, the
notification window expiry, the transmittal number, the revision status, the approval status, and any
field a downstream calculation or a legal deadline reads.

The rule behind the split: **a model may populate a field a human will read; it may not populate a field
a system will act on.** A wrong keyword costs a search. A wrong notification-window expiry costs the
claim.

## 6. Incoming triage: recall matters more than precision

For an incoming-correspondence screen, the two error rates are not symmetric. *Precision* asks how many
of the flagged items really mattered — the cost of false alarms is reviewer time. *Recall* asks how many
of the items that really mattered were flagged — the cost of a miss is a lapsed notification window.

A screen that flags too much wastes hours. A screen that misses one letter loses an entitlement. Tune,
evaluate and accept a triage model on **recall**, and treat its precision purely as a workload number.

The design consequence functions get wrong: the triage model is not the control. The control remains the
contractual notice diary — the independently maintained list of every obligation with a date, owned by
the contract administrator and reconciled to the register at a stated frequency. The model shortens the
reading; the diary catches the miss. Where a model *is* the only line of defence, the function has
replaced a control with a convenience.

## 7. Confidentiality across the document set

A project document set is among the most commercially sensitive data an organisation holds: rates,
subcontract terms, claims strategy, personal data in correspondence, and in some sectors security
information. Two rules follow.

**Nothing leaves the governed environment.** Contracts, commercial correspondence and claims material are
processed only in tools approved for that data class in the permitted-use register — see
`AIG-08 — Governing AI on a project — the control framework` §3.

**Retrieval must respect the permission model.** A retrieval system indexed over the whole document
management system will answer from material the asker is not entitled to see — a claims strategy note
surfaced to a joint-venture partner's user, a personnel matter surfaced to a site engineer. Before it is
switched on across a project, the test is not "does it answer well" but "does an unprivileged account get
an unprivileged answer". Run that test with a named unprivileged account and record the result.

## 8. How this goes wrong

**The letter nobody read as a legal document.** A class C letter is drafted by a model, reviewed by
someone checking the numbers, and signed by someone checking the letterhead. Nobody read it as a
contractual instrument. The concession is found eighteen months later by the other side's consultant.

**The clause number that came from nowhere.** A drafted notice cites a clause that is a plausible place
for a notice provision in a common standard form, but not the clause in *this* contract. The reviewer,
who expected the same clause, did not open the contract.

**The triage model that became the control.** Six months of accurate flagging builds confidence, the
notice diary quietly stops being reconciled, and the one letter the model does not recognise — a scanned
attachment with unusual phrasing — is never flagged. The failure is invisible until the window has closed.

**Register poisoning at scale.** A bulk enrichment run populates 4,000 register entries with model-derived
metadata. Two per cent are wrong, nobody knows which two per cent, and the register is what downstream
reporting draws on. Bulk runs need a sampled acceptance test *before* the write.

**Retrieval that answers from the wrong version.** A grounded model answers a specification question from
revision B while revision D governs, because the index holds both and nothing said which is current.
Every retrieval answer in document control carries a document reference *and* a revision, and the
reviewer checks the revision rather than merely that a citation exists.

**Minutes that create agreements.** A meeting assistant drafts minutes recording that a party "agreed" to
something they discussed. Circulated unchallenged, minutes become evidence of agreement. Minutes are class
B correspondence with a class D tail: actions and any agreement language are confirmed with the named
owners before circulation.

**Volume mistaken for diligence.** AI makes it cheap to write more letters. More letters is not a better
correspondence position; it is a longer file for the other side to mine.

## 9. Worked example — sizing a triage screen

*Illustrative figures.* Figures are per calendar month for a single project; time is reviewer minutes;
rounding to the nearest whole item.

**Setup.** The project receives **480** incoming correspondence items a month. Of these, **45** are
genuinely entitlement-bearing — they start a clock, create an obligation or signal a variation. A triage
model flags **62** items, of which **41** are genuinely entitlement-bearing. Reviewing one item takes
**6 minutes**.

**Formulae.** `precision = true flags ÷ total flags`; `recall = true flags ÷ true cases`;
`review minutes = items reviewed × minutes per item`.

**Substitution.**

- Precision `= 41 ÷ 62 = 0.661 = 66.1 %`
- Recall `= 41 ÷ 45 = 0.911 = 91.1 %`
- Missed items `= 45 − 41 = 4`
- Unflagged items `= 480 − 62 = 418`; a 10 % residual sample `= 41.8 → 42` items
- Review effort with the screen `= (62 × 6) + (42 × 6) = 372 + 252 = 624` minutes `= 10.4` hours
- Review effort reading everything `= 480 × 6 = 2,880` minutes `= 48.0` hours

**Result.** The screen cuts the monthly reading from 48.0 hours to 10.4 hours, at a recall of 91.1 % —
and leaves **4 entitlement-bearing items unflagged every month**.

**Interpretation.** The saving is real and the residual exposure is unacceptable on its own terms. The
10 % sample is expected to catch `4 × 10 % = 0.4` of the four misses — it will usually catch none of them.
The sample is a *drift detector*, telling you whether recall is degrading; it is not a safety net. The
safety net is the contractual notice diary of §6, reconciled independently. The case for the screen is
workload; the case against relying on it is the four items — both true at once, and both stated to
whoever approves the deployment.

## 10. Checklist

Take this into the meeting where AI in document control is proposed.

- [ ] Every outgoing document type is classified A–D (§3), and the classification is in the procedure, not in someone's head.
- [ ] Class D is drafted by the accountable person, not by a model, and the procedure says so.
- [ ] The reviewer of any AI-drafted letter has been briefed on the concession patterns in §4.
- [ ] Register fields are split into model-populated, model-proposed and human-only (§5), and the human-only list includes every field a calculation or a deadline reads.
- [ ] The triage model has a measured recall on a dated sample, and the number is known to whoever approved it.
- [ ] The contractual notice diary exists, is owned by a named person, and is reconciled to the register at a stated frequency.
- [ ] Retrieval has been tested with an unprivileged account and the result recorded.
- [ ] Every retrieval answer carries a document reference **and** a revision.
- [ ] Bulk enrichment runs have a sampled acceptance test before the write.
- [ ] Meeting minutes' actions and any agreement language are confirmed with named owners before circulation.
- [ ] Contracts and commercial correspondence are processed only in tools approved for that data class.

The correspondence file will be read by someone who is not on your side. Write it for that reader, and
decide in advance which sentences a machine is allowed to contribute.

---

## Related

- `AIG-08 — Governing AI on a project — the control framework` — the permitted-use register, data classes and verification tiers this document assumes are in place
- `AIG-10 — Human in the loop: what AI may and may not decide` — the decision boundary that puts contractual positions on the human side, and the authority thresholds that go with it
- `AIG-12 — The AI-literate controls professional` — how a reviewer builds the instinct to catch a conceding sentence
- `BPG-11 — Change orders and variations` — the variation process the correspondence register feeds
- `BPG-12 — Claims and extension of time` — what the correspondence file has to survive

## Sources and standards

- PCI Body of Knowledge, Domain 7 (Contracts, Commercial Management, BoQ, Invoicing & Revenue) and Domain 13 (AI for Project Controls & Project Management), `docs/bok/` — the source material this series draws on; explained here in our own words, not reproduced.
- The Institute's candidate AI-use policy (`docs/downloads/`) — the governing position on AI in preparation, examination and practice.

No external standard is reproduced in this document. Notice provisions, notification periods and delivery
requirements differ between contract forms and jurisdictions: this document describes the discipline, and
your contract states the rule.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
