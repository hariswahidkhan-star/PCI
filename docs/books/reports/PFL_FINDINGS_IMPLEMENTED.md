# PFL-AI — implementation record for the 17 open completeness findings

**Date:** 4 August 2026
**Volume:** PFL-AI Body of Knowledge (*PCI AI Project Finance Leader*)
**Scope worked:** `docs/books/pfl-ai/` — 16 manuscript files, `TOC.md`, and the derived
`GLOSSARY.md`, `QUESTION_BANK.md`, `APPENDICES.md`, `STANDARDS.md` (regenerated, not hand-edited).
One curated line in `docs/books/_build/make_standards.py` — see PFL-16.
**Worklist:** 17 findings (13 major, 4 moderate) from `open_PFL-AI.json`.
**Status:** 17 of 17 implemented. Three implemented differently from the proposed correction, with
reasons stated below (PFL-05, PFL-16, PFL-13/14). One consequential registry update deliberately
left open (PFL-16, §"Left open").

---

## Verification

| Check | Command | Result |
|---|---|---|
| Golden-answer arithmetic | `_build/verify_formulas.py` | **✓ all golden answers verified** (before and after) |
| Glossary | `make_glossary.py --check` | exit 0 — **0 open defects**, pfl-ai 503 terms |
| Question bank | `make_question_bank.py --check` | exit 0 — **0 open defects**, pfl-ai 462 items |
| Appendices | `make_appendices.py --check` | exit 0 |
| Standards register | `make_standards.py --check` | exit 0 — **0 open defects**, pfl-ai 14 references |
| Cross-references | scripted resolution of every `KA n.n[.n]`, `§n.n.n`, `Toolkit n.T.n` in all 16 manuscripts | 0 unresolved references introduced; the 4 pre-existing unresolved hits (`KA 6.4.1b`, `PML-AI D7 KA 7.3.4`, `PML-AI D8 KA 8.2.4`) are cross-volume or worked-example designators and predate this work |
| Reader-facing hygiene | grep for `TODO`/`TBD`/`FIXME`/draft markers/spec blocks/repository paths | none |
| House style | grep of the full diff for US spellings | none |
| Suite principle | grep of all principle occurrences | canonical string only; one pre-existing key-terms gloss that read "…decides, remains accountable" corrected to the exact form |

**No existing number was altered.** Every addition is qualitative — a decision walkthrough, a
governance rule, a triage list or a register field. No new arithmetic was introduced anywhere, so no
`_build/checks/pfl_*.py` module needed to change and the golden-answer suite is untouched.

**The legal sweep was not undone.** No caution was removed or weakened. Every removed line in the
diff is a line rewritten with its content preserved; the only deletions are re-flowed sentences,
extended lists and the two-word principle correction above. Additions follow the sweep's rules:
professional obligation stated, legal question referred to qualified counsel, no statement of what
the law or an external standard requires, no clause or article numbers, no edition years, no
characterisation of any transaction, payment, distribution or enforcement as lawful or unlawful, and
a data-protection caution wherever content directs recording information about identified
individuals.

---

## Finding-by-finding record

### PFL-02 (major) — the duty to correct
**D1 §1.3.1.** "Candour about numbers" extended into a standing rule with the four elements the
book's governance uses: the **trigger** (materiality judged against the recipient's decision, not
the size of the arithmetic; material until someone senior decides otherwise in writing); the
**decision rights** (the correction is owned by the signer of the original output, external
notification by the accountable principal, no commercial party may veto a correction that is owed,
and a request to defer one is itself an escalation event); the **timing and route** (before the next
decision that would rely on it, through the channel the finance documents provide for notices, not
informally); and the **record**. Explicitly states that internal supersession does not discharge the
obligation. Counsel pointer added: whether a failure to correct engages a representation, an
information undertaking, a warranty or an event of default is a question on the specific documents,
and the professional obligation is discharged first while that analysis runs alongside.
Cross-refers to D6 Case study B, D6 KA 6.4.3 and D13.
**Apparatus:** 2 key terms, MCQ 1.3-L, 2 self-check items (list renumbered), learning objective,
domain summary. **Mirror sentence** added to D6's Executive perspective, naming that Case study B
was found by the *lenders'* auditor and stating what was owed had the team found it themselves.

### PFL-03 (major) — the grantor's decision rule
**D4 new topic 4.3.4, "The other side of the table: how a public grantor decides."** Value for
money as a comparison of **risk-adjusted whole-life cost** against a **constructed counterfactual**
the authority owns and the bidder cannot verify; the three consequences (the bidder's IRR is not an
input; a risk priced into the bid but drafted back to the authority fails twice; the rate, risk
adjustments and sensitivities are set by published guidance rather than by the analyst).
**Affordability** as a separate, year-by-year budget fact rather than an appraisal output, with the
point that the two tests are independent — a bid can pass one and fail the other, and re-profiling
payments to fit the envelope changes affordability and not whole-life value. Three questions for the
bid team before pricing. Standing caution: guidance is owned, revised and jurisdiction-specific, and
nothing is reproduced or summarised — the practice is described in the book's own words, consistent
with `STANDARDS.md`'s stated policy.
**Apparatus:** topic list, 3 key terms, MCQ 4.3-I, 3 self-check items, learning objective, domain
summary, `TOC.md` KA 4.3 entry, and the existing "Public-sector appraisal" industry-variations
bullet extended to point at the new topic.

### PFL-04 (major) — control in the shareholders' agreement
**D5 §5.2.3, new block "Control: reserved matters, transfers and deadlock."** Reserved-matter
classes and the logic that generates them (anything that changes the risk the minority underwrote),
with the working rule that **the threshold matters as much as the list** — a 75 % matter is
controlled by any holder above 25 %, so the veto map is arithmetic. The **two-approval problem**:
several reserved matters are also lender or agent consents, on separate timetables, so the register
must show both per decision class. Transfer restrictions — lock-in, pre-emption, tag/drag,
change-of-control consent — and the structural point that lenders' security is over shares that are
not freely transferable, which is what a direct agreement exists to fix. Deadlock mechanisms read as
**control terms**, with who each favours. Default and dilution tied back to the funding obligation
already priced. Existing counsel caution retained and extended to all of these terms.
**Toolkit 5.T.2** gains a second sheet — the **decision-rights map**: per decision class, who
proposes, who approves and at what threshold, whether lender consent is also required, the consent
timetable, the deadlock mechanism, and a veto-map footer.
**Apparatus:** 8 key terms, MCQ 5.2-H, 4 self-check items.

### PFL-05 (major) — the data-handling precondition in the AI sections
A data precondition added immediately before the "must not go" paragraph in each named block:
**D5 KA 5.3, D6 §6.4.4, D7 KA 7.1, D7 KA 7.4, D8 KA 8.1.** Common form: the material is confidential
project material, processed only in an environment approved for that data classification **and
permitted by the confidentiality undertakings that cover it**, and establishing that permission is a
*precondition of the task, not a review of it* — usually a **permitted-recipient** question rather
than a tool-quality one.
Specific additions where the risk is specific: **D7 KA 7.1** — unsigned drafts carry the
counterparty's position as well as the project's, and grantor tender rules frequently restrict where
bid material may be processed. **D7 KA 7.4** — counterparty credit assessment is material about an
identified third party, with a data-protection caution for directors, owners and politically exposed
persons in a control chain, and the retention position set at the same time. **D6 §6.4.4** —
uploading a workbook for a structure scan discloses everything else in the workbook. **D8 KA 8.1** —
the benchmark-library caution the book had nowhere: outturn data is usually the *client's*
confidential information, permission on one engagement is not permission on the next, **a benchmark
set containing a single comparable is that comparable relabelled**, and where contributing projects
belong to competitors the assembly and circulation of cost information is a question for counsel and
compliance before the library is built. A recorded permission basis per source engagement is
required, held with the library.
**Apparatus:** D8 self-check gains an item on the permission basis; Toolkit 8.T.1 gains a
*benchmark provenance* line.
**Deviation from the proposed correction:** the finding asks each block to cross-refer to
"Domain 1 KA 1.3.3". The confidentiality rule ("confidentiality travels with the data") is in
**§1.3.4**, not §1.3.3 (which is conflicts and independence). All five cross-references point to
KA 1.3.4, and forward to Domain 16 as asked. Citing 1.3.3 would have been a broken pointer.

### PFL-06 (major) — the model after close
**D6 §6.4.3, new block "The model after close."** The closing model becomes a **contractual object**
defined by the finance documents; four consequences — it is **locked** and amended only through the
documents' own mechanism (proposal, evidence, agent or lender consent), with the practical control
that the closing model is filed as a read-only artefact whose integrity can be demonstrated later;
the operating model must **reconcile** to it and the reconciliation is itself a deliverable; the
model is the **factual basis of a representation**, so a discovered error engages the duty to
correct (cross-refers PFL-02); and read-write custody is a control, not an IT preference. Standing
caution: the defined term, what is locked, the consent threshold, the reconciliation form and any
representation wording are drafting matters that differ by facility and governing law.
**Toolkit 6.T.3** gains an **access-control and custody** line and a **retention** line — the
closing model, audit report and finding register, provenance table, change log, base-case
reconciliation and AI-edit log retained *together* as one evidential set, for the longest of the
facility's life and tail, the applicable limitation period and any statutory requirement, in a form
that opens without the original toolchain, under a named custodian.
**Apparatus:** 4 key terms, MCQ 6.4-I, 4 self-check items.

### PFL-07 (major) — who measures, who certifies, and what happens while it is disputed
**D7 §7.1.3, new subsection after the worked example.** The measurement source and its
calibration/audit regime as a bankability condition, including the **deemed-availability rule on
metering failure** ("a structure in which the offtaker owns the meters and failure is deemed nil has
transferred more risk than the multiplier suggests"). The **certification chain** and the three
common architectures, with the observation that the burden of proof is worth more than a percentage
point of multiplier, and what happens to a month nobody challenges. **Evidence retention** through
the challenge window *and* the period in which a dispute can still be brought. **Escalation**, cost
allocation, who may settle on the SPV's behalf, and the lock that a settlement moving a covenant
ratio is not an operational decision. **The cash-timing consequence** of a pay-then-argue mechanism
— a contested operational judgment becomes an immediate coverage event before anyone has ruled,
against escrow, which converts the same exposure into counterparty credit and delay. Standing
caution referring the pending-resolution question, the remedy for a wrongful deduction and the
conclusiveness of an unchallenged period to counsel in the governing jurisdiction.
**Toolkit 7.T.1** gains a **Section B — the governance of the measurement**, one block per measured
quantity: measurement, certification, evidence, dispute, cash timing.
**Apparatus:** 4 key terms, MCQ 7.1-H, 4 self-check items.

### PFL-08 (major) — subsidy control on the funding plan
**D9 §9.4.1, closing paragraph.** Every form in the support table raises a question the funding plan
cannot answer for itself: has the support been **properly granted**? Many jurisdictions are
understood to operate régimes governing whether and how public support may be given, and there are
international disciplines between states. The consequence is characteristically described as
**recovery from the beneficiary, with interest** — and the beneficiary is the project company — so
this is a hole in the funding plan rather than a grantor problem, and it opens after close when the
money is spent. The professional requirement: a **written legal confirmation of lawful grant** (or
evidence that any required notification, clearance or approval has been made and obtained), obtained
before financial close, on the condition-precedent schedule with a named owner, retained with the
closing set. Two cautions: the régime, thresholds, procedure and consequences are jurisdiction- and
time-specific and nothing here states them anywhere; and this is not a matter on which a financial
adviser, a model or a sponsor's own view is worth anything.
**Cross-referenced into D13 §13.3.1** as a third-party condition category, with the reason it sits
there (neither sponsors nor lenders control it).
**Apparatus:** 3 key terms in KA 9.4, 2 self-check items; Toolkit 9.T.3 carries the confirmation.

### PFL-09 (major) — prohibitions in the two Domain 10 AI sections
Both blocks rewritten to the domain's own **earns its place / must not go / verification**
structure.
**KA 10.1** prohibitions: no model may adopt a `CFADS` definition not traced clause by clause to the
executed facility (the failure mode stated as a rule, not only as a check); no model may select the
sizing ratio, sculpting target or target coverage, which are credit judgments owned by the lender
and negotiated by the sponsor; and no machine-produced capacity number may circulate without the
Toolkit 10.T.4 sizing-basis statement attached.
**KA 10.2** prohibitions: no model may certify a covenant compliance position; no ratio may be
reported as "the" ratio without its definition source recorded; and no model may resolve a
definitional ambiguity where the documents conflict — that is for counsel and the agent, not a
default. Both close on the canonical principle.

### PFL-10 (major) — the hedging mandate
**D11 new §11.3.1b "The hedging mandate"**, placed before the currency topic, with five lettered
parts: **(a) decision rights** — board-approved policy on a mandate agreed with lenders, execution
by a named officer within limits stated in figures, execution separated from confirmation;
**(b) the prohibition** — hedging beyond the amortisation profile or with no underlying exposure is
speculation and is characteristically prohibited by the finance documents, and over-hedging on a
declining balance is a **breach rather than an inefficiency**, with reconciliation of notional to
schedule at inception and at every prepayment, sweep or resize; **(c) documentation and evidence** —
board resolution, hedging strategy letter to the agent, master agreement and schedule, any credit
support annexe, confirmations reconciled line by line, counterparty credit approval with issuer
standing and downgrade replacement terms, and mark-to-market reported at each test date;
**(d) escalation** — covenanted-ratio breach, counterparty downgrade and any proposed unwind,
novation or restructure go to the board and the agent **before** execution; **(e) accounting** —
losing hedge-accounting designation puts mark-to-market through profit and loss and can disturb
accounting-based covenants and reported net worth with no cash moving, and availability,
documentation and consequences are framework- and jurisdiction-specific and confirmed with the
reporting accountants **before** a hedge is transacted. Standing caution added.
**Apparatus:** topic list, 5 key terms, MCQ 11.3-H, 4 self-check items; Toolkit 11.T.2 gains the
mandate reference and the last notional-to-profile reconciliation date.

### PFL-11 (major) — notice, records and the back-to-back check
**D12 §12.4.3, new block at the head of the topic, before the worked example.** Five dated facts to
establish before any probability is assigned: **notice** as commonly a condition precedent to
entitlement, with the instruction to **compute nothing until the notice position is established**,
because a probability set applied to an unpreserved entitlement produces a defensible-looking
expected recovery that then travels into a board paper, a lender report and a provision; the
**back-to-back check** upstream, with the gap between periods named as an uninsured exposure to be
identified at contract stage; **contemporaneous records** maintained from the date of the event
rather than assembled at claim stage; a **named owner** (the project company's contract manager) and
an internal deadline set well inside the contractual period; and the standing caution that whether a
time bar operates strictly and whether it can be relieved are jurisdiction- and form-specific
questions for counsel.
**Toolkit 12.T.3** — Section 4 may not be started until the header's notice line is a dated fact,
with the upstream back-to-back position recorded beside it, plus a records-retention line.
**Apparatus:** 3 key terms, MCQ 12.4-H, 3 self-check items.

### PFL-12 (major) — the professional obligation in distress
**D15 §15.4.1, boxed "Professional obligation" before any arithmetic.** The diagnostic is a
**mandatory escalation trigger**: the finding goes in writing to the SPV board and to counsel in the
jurisdiction of incorporation *and* under the governing law of the finance documents, which are
frequently not the same and can give different answers, before the three levers are discussed with a
lender. Directors' duties: many jurisdictions are understood to shift the focus of those duties
wholly or partly towards creditors as insolvency approaches, with personal consequences for
continued trading, distributions or preferring a creditor — all jurisdiction-specific, none stated
here, and the professional point that the directors are no longer taking ordinary commercial
decisions. **No distribution once the sustainable-service test fails without a written confirmation,
on the figures then current, that it remains lawful**, distinguished from the finance documents'
contractual lock-up test.
**§15.4.3** gains a third qualification: the enforcement floor is a model of a **legally available**
enforcement, and moratoria, court-supervised processes, service-continuity obligations, grantor
step-in and the reopening of earlier payments can each move it — so the position is confirmed with
counsel before the floor is used as a negotiating boundary, since a boundary drawn on an unavailable
enforcement is an assumption presented as a constraint.
**Apparatus:** 3 key terms, MCQ 15.4-G, 2 self-check items; Toolkit 15.T.3 gains a pre-use gate and
Toolkit 15.T.2 files the distribution confirmations.

### PFL-13 (major) — record retention across D9–D16
**§16.4.4** gains a **retention** block stating the rule and its basis (longest of the applicable
limitation period, the facility's life and tail, and any statutory tax, accounting or regulatory
requirement), the three disciplines that make a period meaningful (model version, input data version
and attribution retained *alongside* the output; a form that opens without the toolchain that
produced it, with export terms negotiated in the contract rather than at renewal; a named custodian
with recorded handover), and the two opposing cautions — periods are jurisdiction-specific, and a
minimisation or deletion obligation over information about identified individuals can cut across
them, reconciled with the data-protection adviser when the arrangement is designed.
**§13.1.5** gains the same rule for the **closing set**, itemised.
**All 25 toolkits in D9–D16** now carry an explicit retention line naming period, form and
custodian — including **Toolkit 13.T.3 Part D** (the close register, condition-precedent evidence
and signed funds-flow reconciliation), **Toolkit 13.T.2** (the model-audit finding register, with
accepted-not-corrected findings retained with their written rationale) and **Toolkit 16.T.3** (a
per-model retention and custody row, with a model whose trail cannot be produced after its platform
is replaced recorded as **unretained** — a finding, not a housekeeping note).
**Apparatus:** 3 key terms and 3 self-check items in D13 KA 13.1; 3 key terms and 3 self-check items
in D16 KA 16.4.

### PFL-14 (moderate) — retention in the toolkit preamble and Toolkit 1.T.3
The standing toolkit preamble, which appears identically in all 16 domains, now carries the
retention sentence: retained at least as long as the obligation it supports, in a form that opens
without the tool that created it, with a named custodian after the engagement ends; minimum periods
set by organisation policy and jurisdiction-specific statutory, tax and limitation requirements,
which the book does not state; and a data-protection clause for registers holding information about
identified individuals.
**Toolkit 1.T.3** gains a **Retention and custody** block (period and its basis, form, named
custodian, handover on personnel change, and the personal-information caution) and a **corrections**
row carrying the PFL-02 record. Explicit retention lines also added to the other registers the
finding names: **3.T.1** (as a table row), **4.T.4**, **5.T.1**, **6.T.3**, **7.T.3**, **8.T.1**.

### PFL-15 (moderate) — day-count conventions
**D3 §3.3.4.** `actual/actual` added to the table with its typical home. New block **"Three things
the table does not settle"**: *which actual/actual* (a family whose variants differ in the
denominator, and the convention for which leap year is the defining case); *which 30/360* (variants
differing on 31st-day and month-end treatment, which can accrue different interest over the same
period, so the register records the variant and not the family); and *the rolling convention*, which
the table did not mention at all — following, modified following, preceding, whether period end
dates roll, the end-of-month rule, and the business-day centres. The load-bearing sentence: **a
model can implement the day-count basis perfectly and still build the wrong schedule**, and the
failure is silent because the schedule still balances against itself.
**Numbers untouched.** The worked example's setup now states that the illustrative quarter is on an
unadjusted schedule and names the three conventions it prices, so the "three conventions" heading
stays accurate beside a four-row table. The existing counsel caution is retained and extended to the
variant and the rolling convention.
**Toolkit 3.T.1** gains three explicit rows — day-count *variant*, business-day convention with its
centres and whether period end dates roll, and the end-of-month rule — plus a retention row.
**Apparatus:** 3 key terms, 2 self-check items.

### PFL-16 (moderate) — naming the anchor for the estimate-class ladder
**D8 §8.1.2.** The existing disclaimer sentence is kept **verbatim**, and the anchor is now named in
the same form D5 uses for the Equator Principles: AACE International's Recommended Practices on
cost-estimate classification, characterised as voluntary professional guidance, not a standard and
not a regulatory requirement, the property of their publisher, named for identification only so a
reader can locate and map onto the framework their own organisation uses, with nothing reproduced,
summarised or paraphrased, no association implied, and a pointer to obtain current versions from the
publisher. The practical reason is stated: a class-based contingency argument only works if both
people in the room know which ladder the other is standing on.
**No edition year or revision date is given**, consistent with `STANDARDS.md`'s stated policy — this
departs from the finding's supporting note, which cites "revised 7 August 2020" from the authorities
registry. Printing that date in the volume would breach the register's own rule.
**`STANDARDS.md`** now discloses AACE for D8. The generator already carried an AACE entry (used by
PML-AI D7) whose description read "…in its Total Cost Management framework", which would have been
an inaccurate disclosure of the new D8 reference; the curated description in
`_build/make_standards.py` was broadened to cover both usages and to state the
identification-only/never-reproduced characterisation. That is the single change made outside
`pfl-ai/`, and it was required for the generated disclosure to be true. **Deviation:** the finding
asks for a "cost management" grouping; the generator's existing family is **"Cost engineering"**,
and a second near-identical family would have been a defect, so the entry lands there.

### PFL-17 (moderate) — related-party governance where the arithmetic depends on it
**D12 §12.3.2** gains a **fourth discipline**: where the guarantor is in the sponsor's group, the
cover and the equity support are not independent, so identify **every other obligation resting on
that same obligor** — equity commitment, contingent equity, cost-overrun undertaking, the in-balance
cash call, guarantees on the group's other projects — and **state the aggregate**, because the
events that call one call the others. "A security package whose parent guarantee and whose equity
support are the same covenant has diversified nothing."
**§12.1.1** extends the arm's-length rule beyond O&M to the EPC contract and the security package:
disclosure, testing on an arm's-length basis by someone outside the commercial line, and recording
the approving body — with the note that a related-party contract is characteristically a reserved
matter (cross-refers PFL-04) and any consent requirement is established rather than assumed.
**Toolkit 12.T.2** gains *related party (Y/N) and the relationship*, *other group obligations
resting on the same obligor*, a *disclosure and approving body* field with date and any consent
required, a **same-obligor aggregate** footer, and the rule that a related-party instrument is not
counted as cover until those are recorded.
**Apparatus:** 2 key terms, MCQ 12.3-F, 2 self-check items.

### PFL-18 (moderate) — accounting and régime in the handback topic
**D15 §15.4.5**, closing block. *(a) Accounting:* the residual obligation is normally recognised as
a **provision**, unwinds through the income statement over the concession and is **remeasured on the
condition survey**, so reported profit and net assets can move with no cash moving — and where any
covenant or distribution test has an accounting basis, a remeasurement can move it in a year when
nothing about the project has changed. Framework and jurisdiction dependence stated; confirmation
with the reporting accountants required; no treatment stated. *(b) Régime:* the section's
behavioural conclusion ("below the discount rate, sponsors rationally defer and grantors rationally
insist") is qualified — it holds only where the obligation is purely contractual, because in several
sectors the form, timing and quantum of decommissioning or handback security may be prescribed by
statute or a regulator, reviewed periodically and revised, with no scope to defer. The professional
first step is to establish whether the obligation is contractual, regulatory or both.
*(c) Owner and approver:* the recommended reserve-plus-bond structure is proposed by the finance
director and approved by the SPV board, checked for any consent it requires, and the **condition-survey
date** and the **estimate vintage** become standing board-reported items rather than things
discovered two to five years before expiry.
**Apparatus:** 5 key terms, 3 self-check items.

---

## Apparatus added, in total

| Item | Count |
|---|---|
| New topics | 2 (`4.3.4`, `11.3.1b`) — both added to their topic lists; 4.3.4 also to `TOC.md` |
| New MCQs | 9 — 1.3-L, 4.3-I, 5.2-H, 6.4-I, 7.1-H, 11.3-H, 12.3-F, 12.4-H, 15.4-G (bank 453 → **462** items, 0 defects) |
| New key terms | 48 across 13 Knowledge Areas (glossary 455 → **503** terms, 0 defects) |
| New self-check items | 41 across 13 Knowledge Areas |
| Toolkit blocks/rows added | 5.T.2 decision-rights map · 7.T.1 Section B · 6.T.3 access-control + retention · 12.T.2 related-party columns · 13.T.3 Part D · 16.T.3 retention row · 3.T.1 three convention rows · 1.T.3 retention block · retention lines on all 25 D9–D16 toolkits and on 3.T.1, 4.T.4, 5.T.1, 6.T.3, 7.T.3, 8.T.1 |
| Learning objectives / summaries updated | D1, D4 |

---

## Left open

**`registries/EXTERNAL_AUTHORITIES.md` — link EXT-065/EXT-066 to the new PFL-AI D8 usage.**
Finding PFL-16 asks for this. `registries/` is outside the scope set for this work
(`docs/books/pfl-ai/` plus `_build/checks/pfl_*.py`), and it is a cross-corpus register that another
agent may hold. The precise edit needed: in the "used in" column of **EXT-065** (AACE RP 17R-97) and
**EXT-066** (RP 18R-97), which currently read `Laws PCL-LAW-03-01`, add `PFL-AI D8`. No other change
is required — the existing note on both rows ("no accuracy ranges or class tables are reproduced")
already describes the treatment §8.1.2 applies.

**Not a defect, recorded for the reader of this file.** Four cross-references in the corpus do not
resolve to a topic heading: `KA 6.4.1b` (D16 → a D6 worked example), `PML-AI D7 KA 7.3.4` and
`PML-AI D8 KA 8.2.4` (D8, twice — cross-volume). All predate this work and none was introduced by
it; they are worked-example and cross-volume designators rather than broken pointers, but they are
noted here because a stricter reference checker will flag them.

**Concurrency note.** PML-AI was being edited in the same working tree while this work ran. The four
derived-file generators write both volumes; they were run to completion and both volumes pass
`--check` with 0 defects. Nothing in `pml-ai/manuscript/` was touched by this work.

**Not committed**, as instructed. (A `Checkpoint: in-progress implementation of the open
completeness findings` commit made by another process during the session captured the D1–D2 edits;
everything else is uncommitted in the working tree.)
