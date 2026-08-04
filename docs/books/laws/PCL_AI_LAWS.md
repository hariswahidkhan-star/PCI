# PCL-AI Professional Laws — PCI AI Project Controls Leader

**Status:** Certification Law set for the **PCL-AI** credential (PCI AI Project Controls Leader).
Version 2.0 — reconstructed under the [PCI Professional Laws Charter](PCI_PROFESSIONAL_LAWS_CHARTER.md)
and the [PCI Law Drafting Manual](PCI_LAW_DRAFTING_MANUAL.md). **Thirty-three laws** carrying
**one hundred and forty-five process requirements**, anchored to the thirteen-domain PCL-AI Body of
Knowledge (`../../bok/`). This edition supersedes the twenty-law set drafted on the earlier
eighteen-field structure; the superseded identifiers are recorded law by law in element 25 and are
never reused.

> **PCI Professional Laws are private professional certification requirements established by Project
> Controls Institute Global. They are not legislation, government regulation, legal advice or
> substitutes for applicable laws, contractual obligations, regulatory requirements or authoritative
> professional standards. Where an applicable legal, regulatory, contractual or authoritative
> requirement imposes a higher or different obligation, that requirement prevails.**

---

## How to read these laws

**The Charter and the Manual govern.** What a law *is* — its status, the instrument hierarchy, the
priority order when requirements conflict, due process, interpretation, amendment, exceptions and the
consequences available to PCI — is settled in the Charter. How a law is *written* — normative
language, one obligation per clause, identifiers, defined terms, the twenty-five mandatory elements,
external-reference classification, prohibited drafting patterns and the twenty-five audit questions —
is settled in the Manual. Where any doubt arises about a law below, those two instruments prevail
over it.

**Normative language, and the ISO mapping.** PCI uses modern must-drafting, exclusively.

| Word | Force in a PCI Law |
|---|---|
| **must** | Mandatory PCI professional requirement |
| **must not** | Prohibited practice; doing it is a breach |
| **should** | Recommendation only — not used to create an obligation anywhere in this set |
| **may** | Permission |
| **can** | Capability or possibility — never permission |

A reader who works to ISO/IEC drafting conventions expects the requirement to be marked by the
mandatory verb those conventions use, and may misread `must` as an external constraint. It is not.
**In a PCI Law, `must` is the requirement form**, and it carries in a PCI Law exactly the force that
the ISO/IEC mandatory verb carries in an ISO/IEC document. PCI does not use that verb, in any field
of any law, and a draft containing it fails gate. This edition was drafted to that rule and checked
against it.

**Identifiers.** Each law is cited as `PCI-PCL-LAW-DD.NN`, where `DD` is the two-digit PCL-AI Body of
Knowledge domain of primary anchorage and `NN` a sequence within that domain. Process requirements
are cited as `PCI-PCL-LAW-DD.NN-PR-NN`. **Process requirements are mandatory** — Charter §3, Level 4 —
and a breach of one is a breach. Citation is by identifier and never by page number, because pagination
changes. Where a law reaches beyond its anchor domain, the anchor is the domain that *teaches* it and
the reach is stated in element 3.

**These laws sit under the Foundational Laws.** The foundational set binds every PCI credential
holder, PCL-AI included. It is cited here in the Charter §3 identifier form `PCI-FND-LAW-NN`; the
subjects are:

| ID | Subject | ID | Subject |
|---|---|---|---|
| `PCI-FND-LAW-01` | Professional accountability | `PCI-FND-LAW-09` | Confidentiality and approved technology |
| `PCI-FND-LAW-02` | Evidence before assertion | `PCI-FND-LAW-10` | Competence and limitation |
| `PCI-FND-LAW-03` | Independent verification | `PCI-FND-LAW-11` | Escalation of material misstatement |
| `PCI-FND-LAW-04` | Human decision authority | `PCI-FND-LAW-12` | Record integrity |
| `PCI-FND-LAW-05` | Transparent assumptions | `PCI-FND-LAW-13` | No silent override |
| `PCI-FND-LAW-06` | Source and version integrity | `PCI-FND-LAW-14` | Responsible AI |
| `PCI-FND-LAW-07` | Data lineage | `PCI-FND-LAW-15` | Correction duty |
| `PCI-FND-LAW-08` | Conflict disclosure | | |

The published foundational file [`PCI_FOUNDATIONAL_LAWS.md`](PCI_FOUNDATIONAL_LAWS.md) carries these
fifteen laws under these identifiers, so every citation below resolves against it directly. The
superseded `PCI-LAW-F-NN` identifiers are recorded, for historical traceability only, in
[`LAW_CONCORDANCE.md`](LAW_CONCORDANCE.md); no live citation uses them.
**No law below reduces a foundational obligation.** Each names the foundational law it serves in
element 19 and adds what project controls specifically requires — a certification law that only
restated its foundational parent was either sharpened or dropped during the audit recorded at the end
of this file.

**External references are classified, never reproduced.** Every reference in element 17 carries the
issuing organisation, the title, the subject it is cited for, what was checked about its edition or
effective date, its nature under Manual §6, the date its currency was checked, and its applicability
limitation. Real instruments are named and characterised in PCI's own words; the official publication
always governs. No clause number, article, edition, judicial decision or requirement is asserted
unless it was verified, and the register at
[`../registries/EXTERNAL_AUTHORITIES.md`](../registries/EXTERNAL_AUTHORITIES.md) is the single
disclosure point behind these entries.

**Two instruments needing the Manual's later categories.** **ANSI/EIA-748** (a US national standard
published by SAE International under ANSI accreditation) is classified under Manual §6 **category 11,
national standard** — it binds only where a contract or procurement regime imports it. The **NIST AI
Risk Management Framework** is a voluntary framework issued by a national standards institute and is
classified by its subject with its voluntary status and origin stated at the point of use. Both were
originally recorded here as a gap in a ten-category vocabulary; the Manual has since added categories
11 and 12 for exactly these cases, so the gap is closed. The original finding is recorded as Q13 in
the audit table.

**No endorsement, affiliation or accreditation is claimed or implied.** Naming an external instrument
means only that it exists and is relevant to the subject under discussion. No standards body,
professional institute, government, supervisory authority or financial institution has reviewed,
approved, endorsed or accredited these laws, the PCL-AI credential or Project Controls Institute
Global.

**Nothing here is legal, tax or accounting advice.** These laws set professional conduct within PCI's
certification scope. Statutory recognition, capitalisation, tax treatment, contractual entitlement
and the interpretation of contract terms are jurisdiction-specific and belong to qualified advisers.

**The suite principle** applies to every law in this set, in its one approved formulation:

> **AI proposes; the professional verifies, decides and remains accountable.**

---

## Definitions

These definitions decide compliance. They are interpretive, not obligations — no requirement is
created here; every requirement lives in an identified law or process requirement (Charter §3). A
term is used below only in the sense given here.

**Where a term is also defined in the Foundational Laws.** Several terms below — *material*,
*independent*, *verified*, *evidence*, *competent reviewer*, *decision owner*, *escalation threshold*,
*approved*, *current* — are also defined, at `D-01` to `D-30`, in
[`PCI_FOUNDATIONAL_LAWS.md`](PCI_FOUNDATIONAL_LAWS.md). **They now carry the same wording in both
places.** Each was reconciled to the canonical definition recorded in
[`PCI_LAW_DEFINITIONS_REGISTER.md`](PCI_LAW_DEFINITIONS_REGISTER.md), which also records what this
volume previously said and why the change was made. Where a credential legitimately measures a limb
differently — the metric in which materiality is quantified, for instance — the canonical definition
states that as an application rule rather than as a rival definition.

Three reading rules remain, and they still matter for any term this reconciliation did not reach.
First, **where a foundational law states its own defined term by its `D-NN` number, that definition
governs that foundational obligation**, and nothing here narrows it. Second, **where a definition here
and a foundational definition both bear on the same act, the one producing the wider obligation
governs** — Charter §4 states that a PCI Law never lowers an obligation, and this volume's own rule is
that no law below reduces a foundational one. Third, a term defined here and not there is a PCL-AI
term and carries only the sense given here.

**How element 21 samples are drawn.** Where a law's element 21 tests "a sample selected on a stated
basis", the sample is selected by the reviewer performing the test, not by the professional whose work
is under review, and the reviewer records the basis of selection. A test performed on a population the
subject of the test chose is not the test element 21 describes.

### A. Terms that decide compliance

**material.** *(Canonical — `D-15`.)* An item, error, omission, variance, difference or fact is
*material* if any of the following is true: (a) were it wrong, omitted or reversed, a decision within
the scope of the work would or could have been taken differently, including its timing, its conditions
or the authority at which it had to be taken; (b) it changes a reported figure by more than the
quantified tolerance published for that figure; (c) it affects a contractual, regulatory, tax or
financial-reporting position; (d) it affects the safety of a person; (e) it affects a party's reliance;
or (f) it meets the adopting organisation's published materiality criteria. **A matter bearing on
safety, legality, a licence or permission, a statutory duty, or the truth of a statement made to a
decision-maker is material irrespective of size**, and no documented threshold reduces it. Where no
criteria are published, the *decision owner* records which of (a) to (e) applies, and why, before the
output is issued. Materiality is determined on the position as known at the time of the act, and is
judged twice: on the item alone, and on the accumulation of items of the same kind since the test was
last applied. **In PCL-AI work, limb (b) is measured by the materiality rule** — the quantum, the basis
on which the quantum is measured, and the person who set it. Where the adopting organisation's
governance publishes a materiality rule for project reporting, that rule applies; where it does not,
the applicable rule is the one the professional has recorded with the deliverable, stating the quantum,
the basis and their own name, applied consistently between periods. The obligation to record and apply
it sits in element 11 of each law, not here. **PCI sets no percentage.**

**independent.** *(Canonical — `D-12`.)* A person is *independent* in relation to a specified matter
where all of the following are true: (a) they did not perform the act, prepare the item or any part of
it, or direct, specify or approve it; (b) they did not select, build or configure the tool, model or AI
system that produced it; (c) they hold no conflict in its outcome and no financial interest in the
matter or in a party to it; (d) they receive no fee, bonus, continuing mandate, success payment or
other benefit that varies with the conclusion reached, and their remuneration, appraisal or
continuation in the engagement is not determined by the outcome the item supports; (e) they are not
accountable for the outcome the item reports on; and (f) they satisfy the reporting-line limb. **The
reporting-line limb:** they do not report to the preparer in respect of the work under review, and are
not in the reporting line of the person accountable for the outcome for the purpose of that matter;
reporting to the preparer's line manager on unrelated work does not by itself defeat independence, but
being appraised on the outcome does. Independence is a fact about a relationship to a specified matter,
never a state of mind, a job title or a permanent designation. Where no person inside the project
organisation meets these tests, independence is obtained from outside it — a functional line, another
project, an internal audit function, a parent entity or an external party.

**verified.** *(Canonical — `D-26`.)* An item, figure, statement, extraction or machine output is
*verified* where a named person who is a *competent reviewer* for that item has applied to it, against
**evidence**, at least one of the eight admissible methods, and has recorded the method used, the
source or population tested and, where a sample was used, its selection basis, the inputs used, the
scope tested, the date, the result, and every difference found together with its resolution. **The
eight admissible methods** — the list at `PCI-FND-LAW-03-PR-01` — are: independent recomputation,
source tracing, clause-to-summary comparison, sampling on a stated basis, reconciliation, boundary
testing, sensitivity analysis, and named expert judgement recorded with its reasoning. Reading an
output and finding it plausible is not verification, and an item on which no such record exists is not
verified however carefully it was produced.

**current.** *(Canonical — `D-30`.)* A record, figure, extract, document or version is *current* where
it is the latest version issued by the system or authority that owns it as at the deliverable's
**cut-off** or, where the deliverable states none, as at its issue date — and where its version
identifier and its extraction date and time are recorded on or with the deliverable. A record whose
version cannot be identified is not current, whatever its age.

**competent reviewer.** *(Canonical — `D-04`.)* A named individual who, in relation to a particular
item, satisfies all of: (a) their competence in the subject matter is evidenced by a qualification, an
assessed competence record held by the adopting organisation, or documented experience of comparable
work, recorded for that class of work before the review begins; (b) they are able to state what would
make the item wrong and which method would detect that error; and (c) they are able to perform the
verification method the law requires, and to reach a conclusion on the matter, without assistance from
the preparer or reliance on the preparer's explanation of it. Competence is evidenced by the
demonstrated ability to reproduce the calculation or trace the record — never by job title, seniority
or availability. **Independence is not a limb of competence:** where a law requires the reviewer to be
*independent*, that requirement is imposed by that law's element 10 and is tested separately.

**decision owner.** *(Canonical — `D-08`.)* The single named individual holding authority, under the
applicable governance arrangement or documented delegation schedule, to take, withhold, approve,
reject, amend or defer the decision that the deliverable supports; who bears its consequence; and who
answers for it afterwards. The accountability is held by one person, is not delegable, and is recorded
before the output is relied upon. A committee is not a decision owner; where a body approves
collectively, its named chair — or the named authority the delegation schedule assigns — is the
decision owner, and the record names that person. "The team", "management", "the business", "the
sponsor", "the lenders" and "the organisation" are never decision owners.

**evidence.** *(Canonical — `D-11`.)* A dated record that exists independently of the assertion it
supports, that identifies its source, its version where it has one, and its author or issuing system,
and that a person other than the author of the assertion can retrieve, examine and use to reach the
same conclusion **without asking that author**. The following are not evidence: an output of an AI
system that does not identify the source of what it asserts; an AI-generated summary of a record, as
evidence of the underlying fact — the underlying record is; a statement that a system, model or tool
produced a figure, unaccompanied by the inputs and the method; a restatement of the assertion in a
second document by the same author; a preparer's own statement offered in support of their own
assertion; an unrecorded recollection; an unattributed file; an undated extract; an unversioned working
copy; an unretained screen view or a screenshot with no source reference; and a dashboard or screen
state that cannot be reproduced.

**approved.** *(Canonical — `D-29`.)* A decision, document, figure or version is *approved* where the
person holding authority for that decision under the applicable governance arrangement or recorded
delegation of authority has given assent identifiably, recording the date, the version assented to and
the scope of the assent. Silence, absence of objection, unrecorded verbal assent, assent by a person
outside their recorded authority, and assent recorded after the item was used are not approval.

**commitment.** A financial obligation the project has entered into for goods, services or works not
yet fully received or performed, evidenced by an executed contract, purchase order, subcontract, work
order, call-off, or other instrument with financial effect, and measured at the value remaining after
deducting the value received or performed to date.

**accrual.** The recorded cost of goods, services or works received or performed at the **cut-off**
and not yet invoiced, measured from evidence of what was received or performed, and never from the
invoice or payment profile.

**cut-off.** The stated date and time at which the population of transactions, progress records and
status data included in a deliverable is fixed, together with the rule that decides whether a record
falls inside it or outside it. A deliverable that states no cut-off has none.

**objective evidence of progress.** A dated record produced by, or verifiable against, a source other
than the person claiming the progress, demonstrating that the work claimed has occurred — for
example a jointly signed measurement, an inspection or test record, a delivery or goods-receipt
record, an accepted deliverable, an approved milestone certificate, a client-certified quantity, or a
system record of completed transactions. A percentage asserted by the person performing or
supervising the work, unsupported by such a record, is not objective evidence of progress.

**open end.** An activity or milestone in a schedule network, other than the network's single start
milestone and single finish milestone, whose start is not driven by a predecessor relationship, or
whose finish drives no successor relationship — including an activity whose only predecessor is a
start-to-start relationship, so that its finish drives nothing, and an activity whose only successor
is a finish-to-finish relationship, so that its start is driven by nothing.

**escalation threshold.** *(Canonical — `D-10`.)* The escalation threshold for a matter is reached at
the earliest moment any of the following becomes true: the matter is *material*; it creates a risk to
the safety of a person; it would change, or would have changed, a decision already taken or about to be
taken; it affects an output already issued outside the professional's own organisation; it affects a
contractual, regulatory, tax or financial-reporting position; or the escalation criteria published by
the adopting organisation or recorded in the delegation schedule are met. On reaching it the
professional must raise the matter in writing to the **decision owner** and, where the law says so,
above the decision owner. **Any event stated in element 13 of a law below is additional to those six
triggers and never in place of them**; a matter that reaches the threshold requires escalation under
`PCI-FND-LAW-11` whether or not it appears in any element 13. **The threshold names a destination and a
time, and the absence of either does not remove the duty:** where no destination is documented the
matter goes to the next authority above the decision owner for it, and where no time is documented the
time is the foundational period at `D-20` — one working day where the matter creates a risk to the
safety of a person or an ongoing financial loss, five working days otherwise, running from the moment
the professional first knows or suspects the matter rather than from the moment they confirm it.
Reaching the threshold obliges escalation; the obligation does not depend on the professional's
expectation of how the recipient will react, or on the matter being resolved afterwards.

### B. Subject-matter terms

**project controls deliverable.** Any cost, schedule, earned value, forecast, change, risk,
contingency, commercial or performance output issued to a decision owner, a client, a lender, an
auditor, an assessor or a governance body — in any medium, including a model, a register, a schedule
file, a dashboard and a slide.

**source record.** The record held in the system of record — the ledger, the commitment system, the
contract file, the schedule file, the timesheet or plant system, the measurement record — from which
a controls figure derives.

**performance measurement baseline.** The approved, time-phased combination of scope, budget and
schedule against which performance is measured, at the version identified in its approval record.
Abbreviated *baseline* throughout.

**control account.** The point in the work breakdown structure at which scope, budget, schedule and
actual cost are brought together for management, and at which one named individual is accountable.

**re-baseline.** Replacement of an approved baseline with a new one, so that variances are measured
against the new baseline from a stated date.

**change.** Any event, instruction, decision or discovery that alters, or is expected to alter, the
approved scope, the approved budget or the approved schedule. An **approved change** is one that has
been *approved* as defined by the person holding change authority.

**trend.** A known or emerging cost or schedule effect that is not yet an approved change and not yet
a recorded actual cost, and that is expected to affect the forecast.

**duplicate cost.** The same underlying cost event recorded more than once in the cost position —
through double posting, an accrual not reversed against the invoice that superseded it, a commitment
counted alongside the actual cost that discharged it, or one event coded to two cost codes.

**reproducible.** A figure is *reproducible* where a **competent reviewer**, using only the retained
records and the recorded method, arrives at the same figure.

**restatement.** Reissue of a previously reported figure, identified as a correction, showing the
figure as originally reported, the corrected figure, the cause and the periods affected.

### C. AI terms

**AI tool.** Software that generates, classifies, predicts, extracts, summarises, matches or
optimises output using machine-learning or generative models, whether it is a standalone assistant or
a feature embedded in a controls, scheduling, commercial or accounting application.

**AI assistance.** Any use of an AI tool in producing a project controls deliverable.

**material AI assistance.** *(Canonical — carried to the whole corpus by
[`PCI_LAW_DEFINITIONS_REGISTER.md`](PCI_LAW_DEFINITIONS_REGISTER.md).)* AI assistance in producing an
output is *material* where removing the AI-generated contribution would change a figure in the output
by more than the applicable materiality measurement, or would change a recommendation, a classification
that affects entitlement, coding, ranking or eligibility, or a stated conclusion. *Material AI
contribution* means the same thing. Volume of use, licence cost and whether a human edited the output
afterwards are irrelevant to the test.

**tool configuration record.** The record of which AI tool, which model or version, and which
material settings, prompts or data sources produced a given output, retained so that the output is
**reproducible**.

### D. Roles

Roles are defined by function, not by job title. Where an adopting organisation uses a different
title, the role is held by whoever performs the function. Where one individual holds two of these
roles on a small project, that is permitted — but it never removes an independence requirement, which
is then met from outside the project under the definition of **independent**.

| Role | Function |
|---|---|
| **project controls lead** | The individual accountable for the integrity of the project's cost, schedule and performance information as issued |
| **cost engineer** | The individual who prepares and maintains the cost position for one or more control accounts |
| **planner** | The individual who prepares, statuses and issues the schedule |
| **control account owner** | The individual accountable for the scope, budget, schedule and cost of one control account |
| **commercial lead** | The individual accountable for the contractual and commercial position, including variations, claims and payment applications |
| **risk lead** | The individual accountable for the risk register and the quantified risk analysis |
| **baseline approval authority** | The individual holding recorded authority to approve or reject a baseline or a re-baseline |
| **change authority** | The individual or body holding recorded authority to approve or reject a change at its value |
| **decision owner** | As defined in §A |

---
## Domain 1 — Foundations of Accounting for Project Controls

### PCI LAW PCI-PCL-LAW-01.01 — Cost Cut-Off Integrity

**1. Normative requirement.** A credential holder must record each project cost in the reporting
period in which the underlying work was performed or the goods or services were received, determined
by the deliverable's stated **cut-off**.

**2. Purpose.** Controls a specific, observed failure: period-end cost is moved across the cut-off —
by holding an invoice, by pre-booking work not yet done, or by an unwritten "we always close on the
25th" habit — and every downstream figure that consumes actual cost (cost performance index, estimate
at completion, variance, accrual, forecast cash) becomes wrong while remaining internally consistent
and therefore undetectable in the report itself.

**3. Scope.** All PCL-AI candidates and credential holders who prepare, process, review, approve or
give assurance over project cost records, period-end cost positions, or the actual-cost feed into
performance measurement — on any project of any size, under any delivery model, and under any
accounting framework. It applies to management reporting; it does not decide the statutory
accounting treatment, which is the responsibility of the entity's finance function.

**4. Defined terms.** *cut-off*, *accrual*, *commitment*, *evidence*, *material*, *approved*,
*source record*, *decision owner*, *escalation threshold* — as defined in the Definitions section.

**5. Required actions.** The professional must apply a written cut-off rule to every reporting
period, and must be able to show for any transaction which side of that rule it fell and why.

- **PCI-PCL-LAW-01.01-PR-01 — Written cut-off rule.** The cut-off rule in force must state the date
  and time at which each source system is frozen, the treatment of goods received but not invoiced,
  the treatment of invoices received after the freeze, and the person who may authorise a departure;
  and it must be retained with the period's cost position.
- **PCI-PCL-LAW-01.01-PR-02 — Boundary review.** Before the period's cost position is issued, the
  professional must review the transactions recorded either side of the cut-off within a window
  stated in the cut-off rule, and must trace each one to a dated receipt, service record or work
  record that places it on the side where it was recorded.
- **PCI-PCL-LAW-01.01-PR-03 — Late-transaction register.** Every transaction recorded, moved,
  reversed or re-dated after the cut-off has been applied must be entered in a late-transaction
  register giving the amount, the underlying date relied on, the reason and the approver.
- **PCI-PCL-LAW-01.01-PR-04 — Judgemental allocation log.** Where cost is allocated between periods,
  work packages or control accounts by judgement rather than by a source record, the basis of the
  allocation, the person who set it and the person who approved it must be recorded.

**6. Prohibited actions.** Holding a known cost out of the period in which it belongs; recording cost
for work not yet performed or goods not yet received; re-dating a transaction to change the period it
falls in; changing the cut-off date between periods to improve a reported result; issuing a cost
position with no stated cut-off.

**7. Required evidence.** The cut-off rule in force for the period; the period-end reconciliation
from the cost position to the ledger or cost system; the boundary-review record with the transactions
tested and the exceptions found; the late-transaction register; the judgemental allocation log; the
identity and date of the person who approved the period position.

**8. Responsible role.** The **cost engineer** for the preparation of the period cost position; the
**project controls lead** for its issue. Where an entry is made in the books of account, the
responsible finance controller holds that entry — this law does not transfer that responsibility.

**9. Approval authority.** The **project controls lead** approves the period cost position. A
departure from the cut-off rule may be approved only by the person the rule names, and only in
writing before the position is issued.

**10. Independence requirement.** The boundary review under PR-02 must be performed by a person
**independent** of the person who posted or approved the transactions being tested. On a project too
small to provide one, independence is met from outside the project.

**11. Materiality or threshold.** The cut-off rule applies to every transaction without a value
threshold — a cut-off with a de minimis is not a cut-off. The **materiality rule** decides two
things only: which exceptions found in the boundary review must be corrected before issue rather than
in the next period, and which reach the escalation threshold. The window tested under PR-02 is set by
the adopting organisation's governance on the basis of its own invoice and goods-receipt lead times,
and is stated in the cut-off rule. *Scaling:* on a USD 2 million refurbishment the window is
typically a few days either side and the review is a complete pass of a short transaction list; on a
USD 5 billion programme the same rule is executed as a dated query across each source system with
sampling under PR-02's stated basis. The obligation is identical; only the execution scales.

**12. Exception and waiver.** A departure from the cut-off rule may be approved by the person named
in the rule, for one period only, on a written justification stating the reason and the effect on the
reported result. The compensating control is disclosure of the departure and its quantified effect in
the same deliverable. A departure taken to change a reported result is never an exception; it is a
breach. No exception is permitted to the requirement to state a cut-off.

**13. Escalation trigger.** Any instruction, request or pressure to record a cost in a period other
than the one its underlying date supports; or the discovery of a cut-off departure that is
**material** and was not disclosed.

**14. AI application.** AI may match invoices to goods receipts, purchase orders and commitments;
flag transactions whose posting date and underlying document date diverge; rank boundary transactions
by risk to focus the review; and draft the late-transaction register from system data.

**15. AI prohibition.** AI must not decide the period in which a cost is recognised, approve a
period-end adjustment, authorise a departure from the cut-off rule, or certify a period as closed.

**16. AI verification.** Source tracing: for every AI-proposed match or reclassification that is
material, the professional must open the underlying document and confirm the date and the amount
against it, and must record the document reference. For non-material AI-proposed items, sampling on a
basis stated in the verification record, with the whole population re-examined if the sample yields
any error of principle.

**17. External reference.**

- **IFRS Foundation / IASB — IAS 37 *Provisions, Contingent Liabilities and Contingent Assets*.**
  Cited for the reporting boundary between an accrual, a provision and a contingent liability, which
  the records this law requires must be capable of supporting. Edition: in force; no clause number
  asserted. Nature: Manual §6 category 2, authoritative financial-reporting standard. Currency
  checked 2026-08-03 (register EXT-006). Applicability: mandatory only for entities applying IFRS
  Accounting Standards in a jurisdiction that has adopted them; it binds no one through this law.
- **AACE International — *Total Cost Management Framework*.** Cited for the cost-control cycle within
  which period cut-off sits. Edition: not asserted — not independently verified. Nature: Manual §6
  category 5, professional framework; not regulatory authority. Register EXT-064, unverified at
  2026-08-03. Applicability: persuasive only, and only where an organisation adopts it.

**18. Jurisdictional caution.** Statutory recognition, capitalisation and tax deductibility of
project cost follow local GAAP and local tax law. The controls position produced under this law is
not the statutory position, and must not be presented as one without advice from a qualified local
accounting and tax adviser.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) and `PCI-FND-LAW-06` (source
and version integrity) govern. This law adds the period-assignment obligation those foundational laws
do not reach. See also `PCI-PCL-LAW-01.02`, `PCI-PCL-LAW-01.03`, `PCI-PCL-LAW-05.01`,
`PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-11.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 1 Foundations of Accounting for Project
Controls · KA 1.3 Accrual accounting and the matching concept — period cut-off and matching; KA 1.5
Chart of accounts and cost coding for projects. Also Domain 5 · KA 5.2 The cost control cycle.

**21. Compliance test.** Compliance is demonstrated when, for the period under review, all four hold:
(a) a written cut-off rule stating a date and time is retained with the cost position; (b) for every
transaction in the tested window, or for a sample selected on the basis stated in the verification
record, the reviewer can name the dated document that places the transaction on the side of the
cut-off where it was recorded; (c) every post-cut-off movement appears in the late-transaction
register with an underlying date and an approver; and (d) the cost position reconciles to the ledger
or cost system with every difference itemised and explained. A transaction for which the reviewer can
name no such document is a failure of this test. Two reviewers applying (b) to the same window and the same
sample basis produce the same list of exceptions.

**22. Breach indicators.** A cost position issued with no stated cut-off; a cut-off date that moves
between periods without a recorded reason; invoices dated before the cut-off first posted several
periods later; a spike of reversals in the days after each close; goods-received-not-invoiced
balances that never age; a late-transaction register with entries but no approvers; period results
that repeatedly land just inside a tolerance.

**23. Consequence within PCI authority.** Correction required and the output withheld until
corrected; additional review of the credential holder's work; escalation within PCI's process;
failure of the associated examination competency; ethics review; certification investigation,
suspension or withdrawal. Each is subject to due process and a right of appeal. PCI can impose no
fine, no civil or criminal liability and no other consequence.

**24. Examination application.** Scenario judgement: a candidate is given a transaction list
straddling a period end with supporting documents and must state which entries belong in the period
and which do not, and identify the entry that cannot be supported. Escalation decision: an
instruction to "leave that one until next month" from a person senior to the candidate. Evidence
selection: choosing, from a list, the record that proves the period assignment.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 (initial draft in the mandatory
structure); stages 4–13 outstanding · Approval date: not yet approved · Effective: on approval ·
Supersedes PCL-LAW-01-01 *Cost Recognition and Cut-Off* of the eighteen-field set; that identifier is
retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-01.02 — Accrual Completeness and Basis

**1. Normative requirement.** At each reporting **cut-off** a credential holder must include in the
cost position an **accrual** for every quantity of work performed or goods and services received and
not yet invoiced.

**2. Purpose.** Controls two opposite and equally corrosive failures. Omitted accruals understate
actual cost, flatter the cost performance index, and delay bad news until the invoices arrive in a
later period. Unsupported accruals create hidden reserve that is later released to absorb a
deterioration nobody has to explain. Both destroy the ability of the cost position to mean anything.

**3. Scope.** All candidates and credential holders who prepare, review or approve period-end cost
positions, accrual schedules, or the actual-cost feed into earned value and forecasting, on any
project. Preparation, review and approval are all in scope; assurance over the accrual position is in
scope.

**4. Defined terms.** *accrual*, *commitment*, *cut-off*, *evidence*, *material*, *objective evidence
of progress*, *source record*, *competent reviewer*.

**5. Required actions.** The professional must build the accrual from evidence of what was received
or performed, and must be able to show the derivation of every accrual line.

- **PCI-PCL-LAW-01.02-PR-01 — Accrual register with basis.** Every accrual must be recorded as a line
  carrying its value, the work or goods it represents, the **source record** it was derived from, the
  method of derivation and the preparer's name.
- **PCI-PCL-LAW-01.02-PR-02 — Completeness sweep against commitments.** Before issue, the accrual
  position must be tested against the open **commitment** population and the period's goods-receipt
  and progress records, and every commitment with receipted or performed value and no invoice and no
  accrual must be either accrued or explained in the register.
- **PCI-PCL-LAW-01.02-PR-03 — True-up of the prior period.** Each prior-period accrual must be
  compared with the invoice or record that superseded it, and the difference recorded; a pattern of
  differences in one direction must be stated in the register with its cause.
- **PCI-PCL-LAW-01.02-PR-04 — Separation of accrual from provision and contingency.** An accrual must
  not be used to carry an amount that is a provision, a contingency, a risk allowance or a management
  reserve; each must be recorded and reported separately.

**6. Prohibited actions.** Omitting a known liability from the period; suppressing accruals to protect
a performance index; creating an accrual with no derivation; carrying an unallocated "cushion";
netting unrelated over- and under-accruals so that neither is visible; releasing an accrual to absorb
an unrelated overspend.

**7. Required evidence.** The accrual register with a basis per line; the completeness-sweep record
showing the commitment and receipt population tested and the exceptions; the prior-period true-up
comparison; the reconciliation of accruals to the ledger; the approver's identity and date.

**8. Responsible role.** The **cost engineer** prepares; the **project controls lead** issues; the
**control account owner** confirms the quantity of work performed for accruals in their control
account. Entries in the books of account remain with the responsible finance controller.

**9. Approval authority.** The **project controls lead** approves the accrual position for controls
purposes. An accrual that is material and rests on judgement rather than a source record must
additionally be approved by the **decision owner** for the cost position.

**10. Independence requirement.** The completeness sweep under PR-02 must be performed or re-performed
by a person **independent** of the person who set the accrual values. Independence is not required
for the mechanical derivation of accruals from receipt records.

**11. Materiality or threshold.** Every accrual is required regardless of value; there is no de
minimis for completeness, because a population of small omissions is exactly how a material omission
is assembled. The **materiality rule** decides which individually judgemental accruals need the
second approval under element 9, and which true-up differences must be explained rather than merely
recorded. *Scaling:* on a USD 2 million refurbishment the sweep is a line-by-line comparison of an
open-order report against the accrual register; on a USD 5 billion programme it is an automated
exception report against the same rule, sampled for testing. The rule does not change with project
size — only the instrument that executes it.

**12. Exception and waiver.** No exception is permitted to the completeness obligation. Where a
liability is known to exist but cannot yet be measured, the professional must record it in the
accrual register at the best estimate available with the estimation uncertainty stated, and must not
omit it. Where no estimate at all is possible, the item must be disclosed in the cost position as an
unmeasured known liability, with the reason.

**13. Escalation trigger.** Discovery that a known liability was excluded from the accrual position;
an instruction to reduce or delay an accrual without a change in the underlying receipt or progress
evidence; an accrual for which no basis can be produced on request.

**14. AI application.** AI may propose accrual candidates from commitment, timesheet, goods-receipt
and progress data; detect commitments with receipted value and no invoice; compare the accrual
pattern with prior periods and flag gaps; and draft the register.

**15. AI prohibition.** AI must not determine that the accrual position is complete, set the value of
a judgemental accrual, approve the period position, or certify that a liability does not exist.

**16. AI verification.** Reconciliation and source tracing: the professional must reconcile the
AI-proposed accrual population to the open-commitment and goods-receipt reports in total and by
control account, and must trace every material AI-proposed accrual to the receipt or progress record
that supports it. Where AI reports "no missing accruals", the professional must test that conclusion
by re-running the completeness sweep independently of the tool, because a tool cannot evidence an
absence.

**17. External reference.**

- **IFRS Foundation / IASB — IAS 37 *Provisions, Contingent Liabilities and Contingent Assets*.**
  Cited for the boundary between accruals, provisions and contingent liabilities that PR-04 keeps
  visible. Edition: in force; no clause number asserted. Nature: Manual §6 category 2, authoritative
  financial-reporting standard. Currency checked 2026-08-03 (register EXT-006). Applicability:
  entities applying IFRS Accounting Standards in an adopting jurisdiction only.

**18. Jurisdictional caution.** Whether an item is an accrual, a provision or a contingent liability
in the statutory accounts is a local-GAAP determination made by the reporting entity. Obtain local
accounting advice before treating the controls accrual position as the statutory position.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) governs; this law adds the
completeness obligation and the prohibition on unsupported accrual, which the foundational law does
not reach. See also `PCI-PCL-LAW-01.01`, `PCI-PCL-LAW-05.01`, `PCI-PCL-LAW-06.03`,
`PCI-PCL-LAW-12.02`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 1 · KA 1.3 Accrual accounting and the
matching concept; KA 1.4 Cost provisions and cost accruals. Also Domain 6 · KA 6.1 EVM fundamentals —
actual cost.

**21. Compliance test.** Compliance is demonstrated when, at the period under review: (a) every line
in the accrual register carries a value, a source record reference and a derivation method; (b) the
completeness sweep record shows the open-commitment and goods-receipt population tested, and every
item in that population with received value, no invoice and no accrual carries a written explanation;
(c) each prior-period accrual is matched to the invoice or record that superseded it, with the
difference stated; and (d) no register line is described only as contingency, reserve, cushion,
rounding or "management adjustment". A reviewer selecting any five register lines can, from retained
records alone, reproduce each value. Two reviewers performing (b) on the same population return the
same exception list.

**22. Breach indicators.** Accrual totals that are round numbers or identical between periods;
accruals released in the same period as an unrelated overspend; goods-received-not-invoiced balances
materially larger than the accrual position; true-up differences consistently in one direction; a
register line with no source reference; subcontractor progress records that do not appear anywhere in
the accrual derivation.

**23. Consequence within PCI authority.** Correction required and the output withheld until
corrected; additional review; escalation within PCI's process; failure of the associated examination
competency; ethics review; certification investigation, suspension or withdrawal — each subject to
due process and a right of appeal.

**24. Examination application.** Calculation review: a candidate is given an open-commitment report,
a goods-receipt listing and a draft accrual register, and must identify the omitted liability and the
unsupported accrual. Ethical dilemma: a request to "hold the accrual until the claim is agreed".

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Supersedes PCL-LAW-01-02 *Accrual Completeness*; that identifier
is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-01.03 — Cost Classification and Cost-Code Integrity

**1. Normative requirement.** A credential holder must code each project cost to the cost code and
**control account** that represents the work the cost was incurred on.

**2. Purpose.** Controls the failure that makes every other cost control unenforceable: cost coded to
where budget remains rather than to where the work happened. Miscoding is invisible at project total,
survives every reconciliation of totals, and destroys variance analysis, earned value, unit-rate
history and the estimating database built from it — often for years after the project closes.

**3. Scope.** All candidates and credential holders who assign, review, approve, correct or give
assurance over cost coding, chart-of-accounts structure, or the mapping between the cost breakdown
structure and the work breakdown structure. It covers preparation, review, approval and assurance,
and applies to reclassification after original posting as strictly as to original posting.

**4. Defined terms.** *control account*, *source record*, *evidence*, *material*, *approved*,
*duplicate cost*, *competent reviewer*, *escalation threshold*.

**5. Required actions.** The professional must code cost by reference to the work performed, evidenced
by the source record, and must record every reclassification with its reason.

- **PCI-PCL-LAW-01.03-PR-01 — Coding structure mapped and published.** The cost breakdown structure
  in use must be mapped to the work breakdown structure and to the control account structure, the
  mapping must be published to those who code cost, and each code must carry a written definition of
  what belongs in it.
- **PCI-PCL-LAW-01.03-PR-02 — Reclassification record.** Every movement of recorded cost between cost
  codes or control accounts after original posting must be recorded with the amount, the origin, the
  destination, the reason, the preparer and the approver, and must be visible in the period in which
  it is made.
- **PCI-PCL-LAW-01.03-PR-03 — Suspense and holding codes.** Cost held in a suspense, holding,
  unallocated or interface code must be reported separately from coded cost, must carry the date it
  entered suspense, and must be cleared to a definitive code within the period stated by the adopting
  organisation's governance.

**6. Prohibited actions.** Coding cost to a code because budget remains there; coding to a code
because the correct code is closed, overspent or attracts scrutiny; leaving cost in suspense to avoid
showing a variance; reclassifying cost after a report has been issued without recording the
reclassification; creating or reopening a code to absorb an overspend.

**7. Required evidence.** The published coding structure with code definitions; the mapping to the
work and control account structures; the reclassification record for the period; the suspense-code
ageing report; the source records supporting a sample of coded transactions.

**8. Responsible role.** The **cost engineer** for the coding of transactions; the **control account
owner** for confirming that cost charged to their control account represents work performed there;
the **project controls lead** for the integrity of the structure and the reclassification record.

**9. Approval authority.** The **project controls lead** approves changes to the coding structure. A
reclassification that is **material** must be approved by the **control account owner** losing the
cost and the **control account owner** receiving it, both recorded.

**10. Independence requirement.** Approval of a material reclassification must not rest solely with
the person who prepared it. The reviewer of the coding sample under element 21 must be **independent**
of the person who coded the transactions tested.

**11. Materiality or threshold.** Coding correctness is required for every transaction; there is no
value threshold below which miscoding is acceptable, because unit-rate history is built from small
transactions. The **materiality rule** decides which reclassifications require dual control-account
approval and which reach the escalation threshold. The suspense-clearance period is set by the
adopting organisation's governance and stated in its cost procedure. *Scaling:* on a USD 2 million
refurbishment a handful of codes and a monthly manual review satisfy this law; on a USD 5 billion
programme the same obligation is met by validation rules at the point of entry plus exception
reporting, because manual review of the population is not possible — the law requires the outcome,
not the method.

**12. Exception and waiver.** Temporary use of a holding code is permitted where the correct code
cannot be determined at the time of posting, provided PR-03 is satisfied. No exception is permitted
to the reclassification record, and none to the prohibition on coding by budget availability.

**13. Escalation trigger.** An instruction to code cost to a control account other than the one where
the work was performed; discovery of a material miscoding that has already been reported externally;
suspense balances that exceed the clearance period and are material.

**14. AI application.** AI may propose a cost code from the invoice text, the purchase order, the
requisition and historical coding patterns; detect coding anomalies against those patterns; identify
probable **duplicate cost**; and draft the reclassification record.

**15. AI prohibition.** AI must not finally assign a cost code without human confirmation where the
assignment is material or where the tool's confidence is below the threshold set under element 11;
must not approve a reclassification; and must not clear a suspense balance.

**16. AI verification.** Sampling with a stated basis plus source tracing: the professional must test
AI-assigned codes against the underlying invoice, purchase order or work record, using a sample
stratified to include the highest-value assignments, the lowest-confidence assignments and a random
selection of the remainder; must record the sample basis, the size and the error rate; and must
re-code the whole affected population where an error of principle is found rather than correcting
only the sampled item.

**17. External reference.**

- **AACE International — *Total Cost Management Framework*.** Cited for the role of the cost
  breakdown structure in the cost-control cycle. Edition not asserted — not independently verified.
  Nature: Manual §6 category 5, professional framework; not regulatory authority. Register EXT-064,
  unverified at 2026-08-03. Applicability: persuasive only, on adoption.
- **DAMA International — *DAMA-DMBOK: Data Management Body of Knowledge*.** Cited only for the
  existence of reference-data and master-data disciplines that a coding structure depends on.
  Edition: 2nd edition (2017) recorded in the register; no content reproduced or structurally
  mirrored. Nature: Manual §6 category 7, industry guidance. Currency checked 2026-08-03 (register
  EXT-092). Applicability: a commercially published body of knowledge with no standard-setter's
  authority; persuasive only.

**18. Jurisdictional caution.** The statutory chart of accounts, the treatment of capital versus
revenue expenditure and the tax classification of project cost are matters of local law and local
accounting policy. A controls cost code is not a statutory account, and mapping between them requires
advice from the entity's accounting function.

**19. Related PCI Laws.** `PCI-FND-LAW-07` (data lineage) governs; this law adds the domain-specific
obligation that lineage runs to the *work*, not merely to the *system of origin*. See also
`PCI-PCL-LAW-01.01`, `PCI-PCL-LAW-03.01`, `PCI-PCL-LAW-05.01`, `PCI-PCL-LAW-13.02`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 1 · KA 1.5 Chart of accounts and cost
coding for projects. Also Domain 5 · KA 5.3 Cost breakdown and control accounts.

**21. Compliance test.** Compliance is demonstrated when: (a) a published mapping exists between the
cost breakdown structure, the work breakdown structure and the control accounts, and every code
carries a written definition; (b) for a sample of transactions selected on a stated basis, the
reviewer can name the source record that shows the work the cost was incurred on, and that record
supports the code used; (c) every reclassification in the period appears in the reclassification
record with a reason and two named approvers where the materiality rule requires them; and (d) no
suspense balance exceeds the clearance period without a recorded reason. Two reviewers testing the
same sample against the same code definitions reach the same exception list — which is why the written
code definitions in (a) are part of the test and not an administrative nicety.

**22. Breach indicators.** Reclassification volume rising towards period end; cost codes whose spend
tracks their remaining budget rather than their work; a suspense balance that never falls; codes with
definitions that overlap; transfers between control accounts with a reason recorded as
"realignment"; unit rates derived from the system that practitioners privately distrust.

**23. Consequence within PCI authority.** Correction required and the output withheld until
corrected; additional review; escalation; failure of the associated examination competency; ethics
review; certification investigation, suspension or withdrawal — each subject to due process and a
right of appeal.

**24. Examination application.** Evidence selection: given four invoices and a coding structure, the
candidate identifies the transaction coded to the wrong control account and names the record that
proves it. Scenario judgement: a control account owner asks for a transfer "to tidy up the
presentation" days before a governance review.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · New law — no predecessor in the eighteen-field set, which
addressed cost coding only inside PCL-LAW-01-01.

---
## Domain 3 — Budgeting & Forecasting

### PCI LAW PCI-PCL-LAW-03.01 — Scope Completeness of the Performance Measurement Baseline

**1. Normative requirement.** A credential holder must ensure that the **performance measurement
baseline** contains all of the authorised scope and none of the unauthorised scope.

**2. Purpose.** Controls the failure that makes a baseline dishonest from the first day: work that is
known to be required is left out of the baseline — because its estimate is uncomfortable, because it
"sits with another party", or because nobody owned it — so that every variance thereafter is measured
against a target the project was never going to meet, and the first forecast increase is presented as
a surprise when it was a certainty.

**3. Scope.** All candidates and credential holders who prepare, review, approve or give assurance
over a baseline or a baseline component, on any project of any size and any delivery model,
predictive or adaptive. It applies at original baselining, at every **re-baseline**, and at the
incorporation of any approved change.

**4. Defined terms.** *performance measurement baseline*, *control account*, *approved*, *evidence*,
*material*, *decision owner*, *competent reviewer*, *escalation threshold*.

**5. Required actions.** The professional must trace the authorised scope into the baseline and
demonstrate that the trace is complete in both directions.

- **PCI-PCL-LAW-03.01-PR-01 — Two-way scope trace.** Every element of authorised scope must be traced
  to at least one baseline element, and every baseline element must be traced back to authorised
  scope; both directions must be recorded, and every unmatched item on either side must carry a
  written disposition.
- **PCI-PCL-LAW-03.01-PR-02 — Work and cost breakdown alignment.** The work breakdown structure and
  the cost breakdown structure must be mapped to one another so that each control account has a
  single defined scope, a single budget and no scope shared with another control account; overlaps
  and gaps found in the mapping must be resolved before approval, not annotated.
- **PCI-PCL-LAW-03.01-PR-03 — Named control account ownership.** Every control account must carry the
  name of one individual accountable for its scope, budget, schedule and cost before the baseline is
  approved; a control account with no named owner, or with a team, function or role title in place of
  a name, must not be included in an approved baseline.
- **PCI-PCL-LAW-03.01-PR-04 — Time-phasing to the schedule.** Each control account budget must be
  distributed across time using the approved schedule dates for the work it funds, and the
  distribution method for each control account must be recorded; budget must not be time-phased by
  spreading evenly, by matching a funding profile, or by any method not stated in the record.
- **PCI-PCL-LAW-03.01-PR-05 — Excluded scope stated.** Scope that is known, expected or foreseen but
  deliberately excluded from the baseline must be listed in an exclusions register issued with the
  baseline, with the reason for exclusion and the party expected to carry it.

**6. Prohibited actions.** Approving a baseline with authorised scope missing; including scope not yet
authorised in order to absorb a budget; carrying an unallocated lump that is not traceable to scope;
leaving a control account without a named owner; time-phasing budget to match a desired cash or
performance profile rather than the schedule; recording a known exclusion nowhere.

**7. Required evidence.** The two-way scope trace with dispositions; the work-to-cost breakdown
mapping; the control account register with named owners; the time-phasing record stating the method
per control account; the exclusions register; the approval record for the baseline version.

**8. Responsible role.** The **project controls lead** for the completeness of the baseline as
assembled; each **control account owner** for the completeness of their own control account; the
**baseline approval authority** for the decision to approve.

**9. Approval authority.** The **baseline approval authority** approves the baseline. A control
account may enter an approved baseline only with its **control account owner**'s recorded acceptance.

**10. Independence requirement.** The two-way scope trace must be performed or re-performed by a
person **independent** of the estimator and of the person who assembled the baseline. Independence is
not required for the mechanical mapping under PR-02.

**11. Materiality or threshold.** Completeness is absolute in principle — every element of authorised
scope must appear — and the **materiality rule** governs only the consequence of a gap found: whether
it must be closed before approval, or may be approved with the gap disclosed and closed by a stated
date. *Scaling:* on a USD 2 million refurbishment the two-way trace is a single table with tens of
rows; on a USD 5 billion programme it is executed tier by tier, each tier tracing to the tier above,
and the professional must record the tier at which the trace was performed. In both cases the test in
element 21 is the same test.

**12. Exception and waiver.** A baseline may be approved with an identified scope gap only where the
**baseline approval authority** records the gap, its estimated value, the reason it cannot be closed,
the date by which it will be closed, and the compensating control (a stated allowance held outside the
control account, disclosed as such). No exception is permitted to PR-03 or PR-05.

**13. Escalation trigger.** Discovery that authorised scope is absent from an approved baseline and
the absence is **material**; an instruction to approve a baseline whose scope trace is incomplete; a
control account presented for approval with no named owner.

**14. AI application.** AI may compare scope documents, contract schedules, estimates and the work
breakdown structure to propose candidate gaps and overlaps; cluster similar scope descriptions across
documents; and draft the two-way trace table.

**15. AI prohibition.** AI must not determine that scope is complete, approve a baseline, assign
control account ownership, or decide that a scope item is out of scope.

**16. AI verification.** Clause-to-summary comparison plus source tracing: for every AI-proposed
match, the professional must open the underlying scope document and confirm that the matched text
describes the same work; for every AI-reported gap the professional must confirm the gap against the
authorising document; and because a tool cannot evidence an absence, an AI report of "no gaps" must be
tested by an independent manual trace of a sample stratified by value and by scope interface.

**17. External reference.**

- **AACE International — *Total Cost Management Framework*.** Cited for baseline and control-account
  concepts in the cost-control cycle. Edition not asserted — unverified. Nature: Manual §6 category 5,
  professional framework. Register EXT-064, unverified at 2026-08-03. Persuasive only, on adoption.
- **Project Management Institute — *A Guide to the Project Management Body of Knowledge (PMBOK
  Guide)*.** Cited for the concept of a performance measurement baseline. Edition deliberately not
  asserted. Nature: Manual §6 category 5, professional framework; not regulatory authority. Register
  EXT-060, checked 2026-08-03. Persuasive only.

**18. Jurisdictional caution.** Whether scope is authorised is a contractual and corporate-authority
question determined by the governing contract and the entity's delegation of authority. Where the
answer is contested, it requires advice from qualified counsel and is not settled by this law.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) governs. This law adds the
two-way completeness obligation and named control-account ownership, neither of which is a
foundational requirement. See also `PCI-PCL-LAW-03.02`, `PCI-PCL-LAW-03.03`, `PCI-PCL-LAW-01.03`,
`PCI-PCL-LAW-05.03`, `PCI-PCL-LAW-06.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 3 · KA 3.1 Budgeting fundamentals; KA 3.3
The time-phased budget / cost baseline (Planned Value). Also Domain 5 · KA 5.3 Cost breakdown and
control accounts; Domain 8 · KA 8.2 Planning.

**21. Compliance test.** Compliance is demonstrated when: (a) a two-way trace record exists in which
every authorised scope element names at least one baseline element and every baseline element names
its authorising document, with a written disposition against each unmatched item; (b) every control
account in the approved baseline names one individual owner; (c) each control account's time-phasing
record states the method used and that method refers to schedule dates; and (d) an exclusions register
was issued with the baseline. A reviewer picking any three authorised scope elements at random can
locate each in the baseline, and picking any three control accounts can name their owners and their
time-phasing method. Two reviewers performing (a) on the same documents produce the same unmatched
list.

**22. Breach indicators.** A baseline whose total equals a funding limit exactly; control accounts
owned by a department; budget spread evenly across a duration for work known to be back-loaded; scope
described in the estimate but absent from the work breakdown structure; an unallocated budget line;
the first forecast increase arriving in the first reporting period after approval.

**23. Consequence within PCI authority.** Correction required and the baseline withheld from use
until corrected; additional review; escalation; failure of the associated examination competency;
ethics review; certification investigation, suspension or withdrawal — each subject to due process and
a right of appeal.

**24. Examination application.** Scenario judgement: a candidate receives an authorised scope list, a
work breakdown structure and a control account register, and must identify the missing scope, the
ownerless control account and the control account with shared scope. Escalation decision: pressure to
approve a baseline before the trace is complete because a board date is fixed.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-03-02 *Baseline Integrity*, which
bundled scope completeness, approval and change control into one requirement; that identifier is
retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-03.02 — Baseline Approval, Version Control and the Change Prohibition

**1. Normative requirement.** A credential holder must not alter an approved **performance measurement
baseline** except by incorporating an **approved change** or by an authorised **re-baseline** under
`PCI-PCL-LAW-03.03`.

**2. Purpose.** Controls the quiet failure in which the target moves to meet the result. A baseline
that can be edited between reports makes variance meaningless, makes earned value arithmetic
unauditable, and removes the only fixed point against which a project can be held to account — and
because the edit is usually small and well-intentioned, it is rarely recorded and almost never
detected without version control.

**3. Scope.** All candidates and credential holders who hold, edit, publish, review, approve or give
assurance over a baseline or any of its components — scope, time-phased budget or schedule — on any
project. It applies to the baseline files themselves and to every derived artefact that restates them.

**4. Defined terms.** *performance measurement baseline*, *approved*, *change*, *re-baseline*,
*current*, *material*, *evidence*, *reproducible*.

**5. Required actions.** The professional must hold the approved baseline under version control and
must be able to show what changed between any two versions and why.

- **PCI-PCL-LAW-03.02-PR-01 — Version identity.** Every issued baseline must carry a unique version
  identifier, its approval date, the approver's name and the identifier of the version it supersedes;
  and every deliverable that reports variance must state the baseline version it was measured against.
- **PCI-PCL-LAW-03.02-PR-02 — Immutable retained copy.** The approved copy of each baseline version
  must be retained in a form that cannot be edited in place, so that any later version can be compared
  with it line by line.
- **PCI-PCL-LAW-03.02-PR-03 — Change-to-baseline register.** Each movement between two baseline
  versions must be reconciled, line by line, to the approved changes or the authorised re-baseline
  that caused it, and any movement that reconciles to neither must be reversed before the new version
  is issued.
- **PCI-PCL-LAW-03.02-PR-04 — Schedule baseline held to the same rule.** The approved schedule
  baseline dates must not be altered by a progress update, a re-scheduling run, a calendar change or a
  logic change; only an approved change or an authorised re-baseline may alter them.

**6. Prohibited actions.** Editing an approved baseline in place; re-issuing a baseline without a new
version identifier; reporting variance without naming the baseline version; moving budget between
control accounts without an approved change; allowing a scheduling tool to overwrite baseline dates
during a status update; retaining only the current version.

**7. Required evidence.** The version register; the retained immutable copies; the line-by-line
reconciliation between consecutive versions; the approved changes referenced by that reconciliation;
the approval record for each version.

**8. Responsible role.** The **project controls lead** for baseline custody and version control; the
**planner** for the schedule baseline; the **baseline approval authority** for approval.

**9. Approval authority.** The **baseline approval authority** approves each baseline version. No
other person may authorise a change to an approved baseline, and authority to approve a change of a
given value is set by the adopting organisation's recorded delegation of authority.

**10. Independence requirement.** The reconciliation under PR-03 must be performed by a person
**independent** of the person who assembled the new version. Custody of the immutable copies must not
sit with the person who edits the working baseline.

**11. Materiality or threshold.** The prohibition applies to any alteration of any size — a
materiality threshold on a baseline edit would defeat the law, because the defeat is assembled from
individually immaterial edits. The **materiality rule** governs only which unreconciled movements
must be escalated as opposed to corrected and recorded. *Scaling:* on a USD 2 million refurbishment
version control is met by dated, read-only copies in a controlled folder; on a USD 5 billion
programme by a managed repository with checksums. Both satisfy PR-02; neither is required to buy a
tool.

**12. Exception and waiver.** No exception is permitted to the prohibition in element 1. A correction
of a demonstrable clerical error in an approved baseline — a transposed figure, a mis-keyed date —
may be made by the **baseline approval authority** on a written record showing the original value,
the corrected value, the evidence of the error and the effect on reported variance, issued as a new
version and disclosed in the next report. That is a correction, not a change, and it is the only
route.

**13. Escalation trigger.** Discovery of a baseline movement that reconciles to no approved change;
an instruction to adjust the baseline to reduce a reported variance; the absence of a retained copy of
a superseded version.

**14. AI application.** AI may compare two baseline versions and produce the line-by-line difference;
match each difference to entries in the change register; and draft the reconciliation for review.

**15. AI prohibition.** AI must not approve a baseline version, decide that a movement is justified,
apply a change to a baseline without recorded human approval, or delete or overwrite a retained
version.

**16. AI verification.** Independent recomputation and reconciliation: the professional must confirm
the AI-produced difference by re-running the comparison in a second, independent manner (a different
tool, a pivot of the two files, or a manual check of the totals by control account), must confirm that
the sum of the matched changes equals the movement in total and by control account, and must
personally examine every difference the tool could not match.

**17. External reference.**

- **SAE International (ANSI-accredited) — ANSI/EIA-748 *Earned Value Management Systems*.** Cited for
  the existence of a recognised set of management-system expectations under which baseline control
  operates. **Edition and guideline count deliberately not asserted**, because the guideline count
  changed at the most recent revision. Nature: a **national standard** — Manual §6
  **category 11**, which binds only where a contract or procurement regime imports it. Currency checked 2026-08-03 (register
  EXT-130 / EXT-090). Applicability: binding only where a contract or a procurement regime imports it;
  it imposes nothing through this law.
- **ISO — ISO 21508 *Earned value management in project and programme management*.** Cited for the
  existence of an international treatment of baseline control. Edition: 2018 recorded in the register,
  with a second edition in development; no clause asserted. Nature: Manual §6 category 3,
  international voluntary standard. Checked 2026-08-03 (register EXT-029). Applicability: voluntary
  unless adopted by regulation or contract.

**18. Jurisdictional caution.** Where a contract or a public procurement regime imports a
management-system standard, its requirements are contractual or regulatory obligations of that
project and may exceed this law. Their interpretation is a matter for qualified counsel and the
contract administrator.

**19. Related PCI Laws.** `PCI-FND-LAW-06` (source and version integrity) and `PCI-FND-LAW-13` (no
silent override) govern. This law adds the specific prohibition on baseline alteration and the
line-by-line reconciliation obligation. See also `PCI-PCL-LAW-03.01`, `PCI-PCL-LAW-03.03`,
`PCI-PCL-LAW-05.04`, `PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-10.03`, `PCI-PCL-LAW-11.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 3 · KA 3.3 The time-phased budget / cost
baseline (Planned Value). Also Domain 5 · KA 5.4 Change control and cost impact; Domain 6 · KA 6.1
EVM fundamentals.

**21. Compliance test.** Compliance is demonstrated when a reviewer can take the two most recent
approved baseline versions, subtract one from the other by control account, and match every
difference to an entry in the change register or to an authorised re-baseline, with no residual; and
when every performance report issued in the period names the baseline version it used, and that
version exists in the retained set. An unmatched residual of any value is a failure of this test. Two reviewers
performing this subtraction on the same two files obtain the same residual.

**22. Breach indicators.** A baseline file whose modification date falls after its approval date;
performance reports that do not name a baseline version; control account budgets that move between
reports with no corresponding change entry; a schedule whose baseline dates changed after a status
update; superseded versions unavailable; a variance that improves without a corresponding recovery in
the work.

**23. Consequence within PCI authority.** Correction required and the affected reports withheld or
reissued; additional review; escalation; failure of the associated examination competency; ethics
review; certification investigation, suspension or withdrawal — each subject to due process and a
right of appeal.

**24. Examination application.** Calculation review: the candidate is given two baseline versions and
a change register and must identify the movement that no change supports. Ethical dilemma: a request
to "just realign the phasing" before a governance review, with no change raised.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-03-02 *Baseline Integrity*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-03.03 — Authority to Re-baseline

**1. Normative requirement.** A credential holder must not use a **re-baseline** for performance
measurement until the **baseline approval authority** has approved it in writing.

**2. Purpose.** Controls the most consequential legitimate act in project controls being performed
informally. A re-baseline erases accumulated variance; done without authority, it converts a visible,
accountable failure into an invisible fresh start, and destroys the record that would have shown
whether the original plan was ever achievable.

**3. Scope.** All candidates and credential holders who propose, prepare, review, approve or give
assurance over a re-baseline, a re-programme, a re-plan, a budget reallocation that resets variances,
or an equivalent act under any name, on any project. Scope includes partial re-baselines of one
control account or one phase.

**4. Defined terms.** *re-baseline*, *performance measurement baseline*, *approved*, *material*,
*decision owner*, *evidence*, *escalation threshold*.

**5. Required actions.** The professional must present a re-baseline as a decision with its cost
recorded, not as an administrative refresh.

- **PCI-PCL-LAW-03.03-PR-01 — Statement of accumulated variance.** The re-baseline submission must
  state the cost and schedule variance accumulated against the outgoing baseline at the effective
  date, by control account, and must state that these variances will no longer be visible in
  subsequent reporting.
- **PCI-PCL-LAW-03.03-PR-02 — Reason and alternatives.** The submission must state the reason for
  re-baselining, why the outgoing baseline can no longer serve as a measurement basis, and what
  alternative to re-baselining was considered.
- **PCI-PCL-LAW-03.03-PR-03 — Effective date and no retrospection.** The re-baseline must carry an
  effective date, and performance for periods before that date must continue to be reported against
  the outgoing baseline; historical variance must not be restated onto the new baseline.
- **PCI-PCL-LAW-03.03-PR-04 — Retention of the outgoing baseline.** The outgoing baseline and its
  final variance position must be retained for the retention period of the project record and must
  remain retrievable for comparison.
- **PCI-PCL-LAW-03.03-PR-05 — Aggregation of partial and successive re-baselines.** Partial
  re-baselines of separate control accounts or phases, and successive re-baselines within the period
  the adopting organisation's delegation states, must be aggregated for the purpose of determining
  which authority approves, and the aggregation applied must be recorded with each submission. Where
  the aggregate accumulated variance removed reaches the level at which the delegation requires a
  higher authority, that authority approves, and a series of separately approved partial re-baselines
  must not be used in place of it.

**6. Prohibited actions.** Re-baselining to remove an adverse variance rather than because the
baseline no longer represents the authorised plan; applying a re-baseline retrospectively; presenting
a re-baseline as a routine update; discarding the outgoing baseline; re-baselining one control
account to absorb another's overspend.

**7. Required evidence.** The re-baseline submission with PR-01 and PR-02 content; the written
approval naming the approver and the effective date; the retained outgoing baseline and its final
variance; the reporting record showing pre-effective-date periods still measured against the outgoing
baseline; the aggregation record required by PR-05.

**8. Responsible role.** The **project controls lead** prepares and submits; the **baseline approval
authority** approves; the **decision owner** for the project accepts the consequence for reporting.

**9. Approval authority.** The **baseline approval authority** alone, in writing, before use. Where
the adopting organisation's delegation places a re-baseline of a stated value or effect above that
authority, the higher authority named in the delegation approves.

**10. Independence requirement.** The **baseline approval authority** must be **independent** of the
person who prepared the re-baseline, and must not hold a performance benefit that turns on the removal
of the accumulated variance.

**11. Materiality or threshold.** Approval is required for every re-baseline, of any value, because a
re-baseline is a change in the measurement basis rather than a quantum. The adopting organisation's
delegation of authority sets which level approves, and the **materiality rule** determines which
re-baselines must additionally be disclosed to external recipients of prior reports. *Scaling:* on a
USD 2 million refurbishment the approval is a single recorded decision by the project sponsor; on a
USD 5 billion programme the same law operates through the tiered delegation already in place. Neither
case permits a re-baseline by silence.

**12. Exception and waiver.** No exception is permitted. Where a re-baseline is needed urgently — for
example after a contractual reset — the approval may be given provisionally in writing with a stated
expiry, on condition that the full submission follows by a stated date; a provisional approval that
expires without the submission voids the re-baseline and reporting reverts to the outgoing baseline.

**13. Escalation trigger.** A re-baseline proposed with the effect, and without the stated purpose, of
removing an adverse variance; use of a re-baseline before written approval; a request to restate
history onto the new baseline.

**14. AI application.** AI may compute accumulated variance by control account for PR-01, model
alternative re-baseline options, and draft the submission's factual sections.

**15. AI prohibition.** AI must not approve a re-baseline, decide that one is warranted, set its
effective date, or generate the justification presented as the preparer's reasoning.

**16. AI verification.** Independent recomputation: the professional must recompute the accumulated
variance in PR-01 from the outgoing baseline and the actual position without using the tool that
produced it, and must reconcile the two results before the submission is issued.

**17. External reference.**

- **ISO — ISO 21508 *Earned value management in project and programme management*.** Cited for the
  existence of an international treatment of baseline maintenance. Edition: 2018 per register, second
  edition in development; no clause asserted. Nature: Manual §6 category 3, international voluntary
  standard. Checked 2026-08-03 (EXT-029). Voluntary unless adopted by contract or regulation.

**18. Jurisdictional caution.** Where a contract, a funder's conditions or a public-sector approval
regime governs re-baselining, those requirements prevail over this law and their interpretation
belongs to qualified counsel and the contract administrator.

**19. Related PCI Laws.** `PCI-FND-LAW-13` (no silent override) governs; this law adds the specific
approval, disclosure and retention obligations for the act that most often constitutes a silent
override in project controls. See also `PCI-PCL-LAW-03.01`, `PCI-PCL-LAW-03.02`, `PCI-PCL-LAW-04.03`,
`PCI-PCL-LAW-12.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 3 · KA 3.3 The time-phased budget / cost
baseline. Also Domain 12 · KA 12.3 Contingency and management reserve — drawing down and
re-baselining; Domain 5 · KA 5.4 Change control and cost impact.

**21. Compliance test.** Compliance is demonstrated when, for each re-baseline in the period under
review: (a) a written approval exists, dated before the first report measured against the new
baseline, naming the approver; (b) the submission states accumulated variance by control account at
the effective date; (c) reports for periods before the effective date, as retained, still show
variance against the outgoing baseline; (d) the outgoing baseline is retrievable; and (e) the reviewer
totals the accumulated variance removed by **every** re-baseline in the period, partial and whole, and
confirms that the aggregate was approved at the authority the delegation requires for that total rather
than only at the authority each part required on its own. A report measured against a new baseline
before the approval date is a failure of this test, and the test needs no judgement to reach it: the
two dates are either in that order or they are not. A series of partial re-baselines whose aggregate
exceeds the authority that approved them is a failure of this test even where each part was approved.

**22. Breach indicators.** Variance that resets to zero across control accounts in one period; a
re-baseline effective date that precedes its approval date; prior-period reports reissued showing
different variances; the outgoing baseline no longer retrievable; a re-baseline immediately preceding
a governance review or an incentive measurement date.

**23. Consequence within PCI authority.** Correction required and the reissued reports withheld;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of
appeal.

**24. Examination application.** Ethical dilemma: the candidate is asked to re-baseline before a board
meeting so that "the variance conversation can be about the future". Evidence selection: identifying
which document proves a re-baseline was authorised, from a set including an email chain, a minute and
a signed approval.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · New law — the eighteen-field set treated re-baselining only as a
clause inside PCL-LAW-03-02. **Stage 9 amendment:** the red-team found that the law could be complied
with in full by re-baselining one control account at a time, each within the lowest approval band, so
that no authority ever saw the reset the series achieved; `PR-05` and limb (e) of element 21 close that
route by aggregating partial and successive re-baselines for banding.

---

### PCI LAW PCI-PCL-LAW-03.04 — Completeness of the Estimate at Completion

**1. Normative requirement.** A credential holder must construct the estimate at completion so that it
includes every known cost effect on the remaining work at the **cut-off**.

**2. Purpose.** Controls the most common professional failure in project controls: a forecast that is
wrong not because the arithmetic is wrong but because something known was left out — a signed change
not yet in the ledger, a trend everyone discusses and nobody quantifies, a committed rate increase, an
exposure carried in the risk register and nowhere else. Each omission is defensible alone; together
they are the reason forecasts move in one direction.

**3. Scope.** All candidates and credential holders who prepare, review, approve or give assurance
over an estimate at completion, an estimate to complete, a cost forecast, a cash-flow forecast or a
funding request, on any project and under any contract type.

**4. Defined terms.** *cut-off*, *commitment*, *accrual*, *approved change*, *trend*, *material*,
*evidence*, *escalation threshold*, *reproducible*.

**5. Required actions.** The professional must assemble the estimate at completion from stated
components and must show that each component was considered, whether or not it produced a value.

- **PCI-PCL-LAW-03.04-PR-01 — Written basis of the forecast.** Every issued forecast must be
  accompanied by a written basis stating the method used, the assumptions relied on, the items
  deliberately excluded, the known uncertainties, and the name of the preparer; a forecast issued
  without that basis must not be relied on for a decision.
- **PCI-PCL-LAW-03.04-PR-02 — Estimate to complete built from remaining work.** The estimate to
  complete must be built from the remaining quantities, resources, durations and rates required to
  finish the work; deriving it only by subtracting actual cost from a target is prohibited, and where
  a formula-based estimate to complete is used it must be reconciled to the bottom-up figure with the
  difference explained.
- **PCI-PCL-LAW-03.04-PR-03 — Trend inclusion.** Every entry in the trend register that is not yet an
  approved change and not yet an actual cost must be either included in the forecast at a stated value
  or listed as excluded with the reason; a trend register that exists but does not reconcile to the
  forecast is a breach of this process requirement.
- **PCI-PCL-LAW-03.04-PR-04 — Approved-change inclusion.** Every approved change with a cost effect
  must be reflected in the forecast in the period in which it was approved, and the forecast must
  reconcile to the approved-change register in total.
- **PCI-PCL-LAW-03.04-PR-05 — Risk and contingency treatment stated.** The forecast must state
  whether risk exposure and contingency are included, and if included, at what basis; risk-adjusted
  and unadjusted figures must not be presented as though they were the same number, and contingency
  must be shown as a separate line rather than distributed into control account forecasts.
- **PCI-PCL-LAW-03.04-PR-06 — Schedule alignment.** The forecast must use the same schedule status,
  completion dates and duration assumptions as the current approved schedule status report; where the
  forecast assumes dates different from the schedule, the difference and its cost effect must be
  stated in the basis.

**6. Prohibited actions.** Issuing a forecast that omits a known cost effect; presenting an estimate to
complete derived only by subtraction as though it were built from remaining work; excluding a trend
because it is not yet approved; distributing contingency into control account forecasts so that it
absorbs overspend invisibly; forecasting to a target; issuing a forecast with no basis.

**7. Required evidence.** The written basis of the forecast; the reconciliation of the forecast to
actual cost, commitments, accruals, the approved-change register, the trend register and the risk
position; the bottom-up estimate to complete; the schedule status report used; the approver's identity
and date.

**8. Responsible role.** The **cost engineer** prepares the control account forecast; the **control
account owner** confirms the remaining work assumptions; the **project controls lead** assembles and
issues the project forecast.

**9. Approval authority.** The **decision owner** for the cost position approves the issued forecast,
after the challenge required by `PCI-PCL-LAW-03.05`.

**10. Independence requirement.** Preparation may be performed by the cost engineer who owns the
control account. The challenge under `PCI-PCL-LAW-03.05` must be **independent**; this law does not
require independent preparation, because a forecast prepared by someone without knowledge of the
remaining work is worse, not better.

**11. Materiality or threshold.** Every known cost effect must be considered, with no value threshold
for consideration — the threshold applies to *quantification*: an effect below the **materiality
rule** may be recorded as considered and immaterial rather than separately valued, and that record is
itself required. *Scaling:* on a USD 2 million refurbishment the trend register may hold ten items,
each individually quantified; on a USD 5 billion programme trends are quantified by control account
with a stated aggregation rule, and the professional must record that rule. In both cases an item may
be excluded only with a written reason.

**12. Exception and waiver.** No exception is permitted to inclusion of an approved change or of a
known cost effect. Where an effect is known but cannot be quantified at the cut-off, it must be
disclosed in the basis as an unquantified known effect with the reason and the date by which it will
be quantified — disclosure is the only permitted treatment, and omission is never one.

**13. Escalation trigger.** An instruction to exclude a known cost effect from the forecast; discovery
that an approved change is absent from the forecast and the omission is **material**; a forecast
issued to a target set before the analysis.

**14. AI application.** AI may aggregate cost, commitment and accrual data; propose estimate-to-
complete values from remaining quantities and historical rates; identify trends in performance data;
cross-check the forecast against the change and risk registers; and draft the basis document.

**15. AI prohibition.** AI must not decide that a forecast is complete, set the value of a judgemental
forecast item, decide to exclude a trend or a risk, approve a forecast, or generate a forecast
narrative that is presented as the professional's own reasoning.

**16. AI verification.** Independent recomputation plus reconciliation: the professional must
recompute the total estimate at completion from its components without the tool, must reconcile it to
actual cost plus commitments plus accruals plus the bottom-up estimate to complete plus stated
contingency, and must confirm line by line that every approved change and every open trend appears or
is explained. Where AI has proposed values from historical rates, the professional must test the
sensitivity of the total to the two largest such values and record the result.

**17. External reference.**

- **AACE International — *Total Cost Management Framework*.** Cited for the forecasting step of the
  cost-control cycle. Edition not asserted — unverified. Nature: Manual §6 category 5, professional
  framework. Register EXT-064. Persuasive only, on adoption.
- **Project Management Institute — *A Guide to the Project Management Body of Knowledge (PMBOK
  Guide)*.** Cited for the concept of an estimate at completion. Edition deliberately not asserted.
  Nature: Manual §6 category 5, professional framework; not regulatory authority. Register EXT-060,
  checked 2026-08-03. Persuasive only.

**18. Jurisdictional caution.** Where a forecast is used in statutory financial reporting — for
example in measuring progress towards satisfaction of a performance obligation, or in assessing an
onerous contract — the accounting determination belongs to the reporting entity under its applicable
framework and requires qualified accounting advice. This law governs the controls forecast only.

**19. Related PCI Laws.** `PCI-FND-LAW-05` (transparent assumptions) governs; this law adds the
enumerated completeness components, which the foundational duty of transparency does not supply. See
also `PCI-PCL-LAW-03.05`, `PCI-PCL-LAW-05.01`, `PCI-PCL-LAW-06.04`, `PCI-PCL-LAW-12.02`,
`PCI-PCL-LAW-13.02`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 3 · KA 3.4 Forecasting; KA 3.5 Cash-flow
forecasting. Also Domain 6 · KA 6.3 Forecasting with EVM: the EAC family; Domain 12 · KA 12.3
Contingency and management reserve.

**21. Compliance test.** Compliance is demonstrated when the approved forecast can be reconciled,
without unexplained difference, to: the current actual cost position, the open **commitment**
register, the **accrual** schedule, the approved-change register, the trend register, the stated
contingency position and the current approved schedule status — and when the written basis names the
method, the assumptions, the exclusions and the preparer. Any reconciling item without an explanation
is a failure of this test. Two reviewers performing this reconciliation from the same six sources reach the same
list of unexplained differences.

**22. Breach indicators.** A forecast equal to the budget for several consecutive periods followed by
a step increase; an estimate to complete that equals budget minus actual cost in every control
account; a trend register whose items never appear in the forecast; contingency falling exactly as
overspend rises; a forecast completion cost that assumes schedule dates the schedule does not support;
a basis document that has not been updated since the first issue.

**23. Consequence within PCI authority.** Correction required and the forecast withheld until
corrected; additional review; escalation; failure of the associated examination competency; ethics
review; certification investigation, suspension or withdrawal — each subject to due process and a
right of appeal.

**24. Examination application.** Calculation review: given actual cost, commitments, accruals, an
approved-change register and a trend register, the candidate identifies which items the draft forecast
omits and restates the estimate at completion. Scenario judgement: a forecast that matches the funding
limit exactly, with a trend register that would exceed it.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-03-03 *Forecast Honesty*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-03.05 — Independent Challenge and Approval of the Forecast

**1. Normative requirement.** A credential holder must not issue a forecast for a decision until a
person **independent** of its preparation has challenged it and the challenge has been recorded.

**2. Purpose.** Controls optimism that survives every completeness check. A forecast can include every
component required by `PCI-PCL-LAW-03.04` and still be wrong in the same direction every period,
because the person who prepared it is the person whose performance it describes. Independent challenge
is the only control that reaches that failure, and it works only if it is recorded and its outcome is
visible.

**3. Scope.** All candidates and credential holders who prepare, challenge, approve or give assurance
over a forecast that will be used to take or support a decision — a funding decision, a governance
report, a lender or client submission, an incentive assessment or an accounting input — on any
project.

**4. Defined terms.** *independent*, *competent reviewer*, *decision owner*, *approved*, *material*,
*evidence*, *escalation threshold*, *verified*.

**5. Required actions.** The professional must subject the forecast to a challenge that tests named
things, and must record what the challenge found.

- **PCI-PCL-LAW-03.05-PR-01 — Scope of the challenge.** The challenge must test, at minimum: the
  basis document against the forecast; the estimate to complete against remaining work; the treatment
  of trends and approved changes; the contingency position; the schedule assumptions; and the
  direction and size of the previous three periods' forecast movements.
- **PCI-PCL-LAW-03.05-PR-02 — Challenge record.** The challenge must be recorded with the challenger's
  name, the date, each question raised, the response, and whether the forecast changed as a result;
  and the record must be retained with the forecast.
- **PCI-PCL-LAW-03.05-PR-03 — Unresolved challenge disclosed.** Where a challenge point is not
  resolved before issue, it must be stated in the forecast's basis document, with the challenger's
  position and the preparer's position, so that the **decision owner** sees the disagreement rather
  than its average.

**6. Prohibited actions.** Issuing a forecast for a decision with no recorded challenge; treating a
review by the preparer's line manager who directed the outcome as an independent challenge; recording
a challenge that asked no question; resolving a challenge point by removing it from the record;
presenting a challenged and unresolved figure as agreed.

**7. Required evidence.** The challenge record with questions and responses; the challenger's identity
and the basis on which they are independent; the version of the forecast challenged; the approval
record; the basis document showing any unresolved points.

**8. Responsible role.** The **project controls lead** for arranging and recording the challenge; the
named **competent reviewer** for performing it; the **decision owner** for approving the forecast
after it.

**9. Approval authority.** The **decision owner** for the cost position approves the forecast. The
challenger has no approval authority and must not be asked to sign as approver — the two roles are
separate, and merging them removes the control.

**10. Independence requirement.** The challenger must satisfy all four facts in the definition of
**independent**, including having no benefit that turns on the forecast outcome and not having
configured the AI tool used in preparation. On a project with no independent person available, the
challenge is performed from outside the project; a challenge performed by the preparer's direct
report is never independent.

**11. Materiality or threshold.** Challenge is required for every forecast issued for a decision. The
adopting organisation's governance sets the depth of challenge by forecast significance, using
criteria it records — for example the value of the decision, the exposure of external parties, and
whether the previous forecast moved materially. Where no such criteria exist, the professional applies
and records their own. *Scaling:* on a USD 2 million refurbishment the challenge may be one competent
reviewer for one hour with a recorded question list; on a USD 5 billion programme it is a standing
review with named challengers per control account. The record required is identical.

**12. Exception and waiver.** An exception may be approved by the **decision owner**, in writing, where
a forecast is needed before a challenge can be completed — for example in an emergency funding
request. The forecast must then be marked as unchallenged on its face, the challenge must be completed
within the period the decision owner records at the time of approval and in any event before the next
issue of the forecast, and any change it produces must be reported to every recipient of the
unchallenged version. An unchallenged forecast that is not marked as such is a breach, not an
exception. **This exception does not displace `PCI-FND-LAW-03`**, whose element 12 makes no waiver of
independent verification available where the item supports an irreversible commitment, a payment to a
third party, an external regulatory or financial report, or a decision bearing on the safety of a
person: an unchallenged forecast must not be used for any of those, and an emergency funding request
that commits funds irreversibly is one of them.

**13. Escalation trigger.** A challenge point that is material and unresolved at issue; a pattern of
forecast movements in one direction across three or more consecutive periods; an instruction not to
record a challenge or to remove a challenge point from the record.

**14. AI application.** AI may prepare the challenge pack, compute the forecast movement history,
compare the forecast against prior periods and against comparable projects, and propose challenge
questions.

**15. AI prohibition.** AI must not perform the challenge, be recorded as the challenger, decide that
a challenge point is resolved, or approve a forecast. An AI review is never an independent review,
because the tool holds none of the four facts that constitute independence and cannot be accountable
for the conclusion.

**16. AI verification.** Named human judgement recorded with its reasoning: where AI has proposed
challenge questions, the human challenger must record which they adopted, which they rejected and why,
and must add at least the questions required by PR-01 that the tool did not raise. The challenge
record must name a person, never a tool.

**17. External reference.** Not applicable — this obligation is a PCI certification requirement about
who challenges a forecast, and no external instrument in the register imposes or displaces it; naming
one would create a false impression of external authority.

**18. Jurisdictional caution.** Where a forecast supports a regulated disclosure, a listed entity's
reporting, or a lender submission, additional review, approval and independence requirements may apply
under securities law, listing rules or finance documents; those are legal questions for qualified
advisers and are not satisfied by compliance with this law.

**19. Related PCI Laws.** `PCI-FND-LAW-03` (independent verification) governs; this law adds the
enumerated challenge scope, the record, and the disclosure of unresolved disagreement — none of which
the foundational law specifies. See also `PCI-PCL-LAW-03.04`, `PCI-PCL-LAW-04.02`,
`PCI-PCL-LAW-06.04`, `PCI-PCL-LAW-12.02`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 3 · KA 3.4 Forecasting. Also Domain 4 ·
KA 4.1 Performance management principles; Domain 6 · KA 6.3 Forecasting with EVM: the EAC family.

**21. Compliance test.** Compliance is demonstrated when, for the forecast under review, a retained
challenge record names a challenger, states the date, lists questions covering each of the six matters
in PR-01, records a response to each, and states whether the forecast changed; and when the
challenger's independence can be established from the four facts without asking them. A challenge
record with no question, or a challenger who prepared any part of the forecast, is a failure of this test. Two
reviewers applying the four independence facts to the same challenger reach the same answer.

**22. Breach indicators.** Challenge records identical in wording between periods; challengers drawn
from the preparer's own reporting line; forecasts that never change after challenge; unresolved points
that disappear between the challenge record and the issued basis; challenge dated after the forecast
was issued.

**23. Consequence within PCI authority.** Correction required and the forecast withheld from the
decision until challenged; additional review; escalation; failure of the associated examination
competency; ethics review; certification investigation, suspension or withdrawal — each subject to due
process and a right of appeal.

**24. Examination application.** Scenario judgement: the candidate is offered four candidate
challengers and must identify which are independent and why. Escalation decision: a material challenge
point the preparer refuses to record.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · New law — the eighteen-field set required independent review only
as a field within other laws and never defined its scope or record. **Stage 9 amendment:** element 12
gave the unchallenged-forecast exception an open-ended clock and offered an emergency funding request
as its example, which is precisely the case in which `PCI-FND-LAW-03` element 12 makes no waiver
available; the period is now fixed and the foundational carve-out is stated.

---
## Domain 4 — Performance Management, Variance Analysis & Reporting

### PCI LAW PCI-PCL-LAW-04.01 — Reconciliation of the Performance Report to Source Records

**1. Normative requirement.** A credential holder must not issue a performance report unless every
figure in it reconciles to the **source record** it derives from at the report's stated **cut-off**.

**2. Purpose.** Controls the failure in which a report is assembled from a spreadsheet that was once
right. Figures diverge from their systems quietly — a manual override left in, a filter changed, a
stale extract, a rounding that became a rule — and the report continues to look coherent because its
internal totals still add. Reconciliation to source is the only check that reaches this, and it is the
first thing dropped under deadline pressure.

**3. Scope.** All candidates and credential holders who prepare, review, approve or give assurance
over a periodic or ad hoc performance report, dashboard, governance pack, client report or lender
report containing cost, schedule, earned value, change, risk or commercial figures, on any project.

**4. Defined terms.** *source record*, *cut-off*, *current*, *material*, *reproducible*, *approved*,
*evidence*, *decision owner*.

**5. Required actions.** The professional must reconcile before issue, and must retain the
reconciliation as part of the report record.

- **PCI-PCL-LAW-04.01-PR-01 — Stated cut-off and extract identity.** Every report must state its
  cut-off date and time, and must record for each source system the extract identifier or timestamp
  used; figures drawn from different cut-offs must be labelled with the cut-off that applies to each.
- **PCI-PCL-LAW-04.01-PR-02 — Reconciliation record.** A reconciliation from each reported total to
  its source system total must be retained with the report, itemising and explaining every difference,
  including timing differences, exclusions and manual adjustments.
- **PCI-PCL-LAW-04.01-PR-03 — Manual adjustment register.** Every manual adjustment made between the
  source extract and the reported figure must be recorded with its value, its reason, its preparer and
  its approver, and must be visible to the reviewer as an adjustment rather than absorbed into a total.
- **PCI-PCL-LAW-04.01-PR-04 — Named report approval before issue.** Each report must carry the name of
  the individual who approved its issue and the date of that approval; a report issued without that
  name must not be relied on for a decision.

**6. Prohibited actions.** Issuing a report with no stated cut-off; drawing figures from extracts of
different dates without labelling them; making a manual adjustment that is not recorded; reporting a
figure that cannot be traced to a source system; carrying forward a prior-period figure as current;
issuing a report with no named approver.

**7. Required evidence.** The reconciliation record per reported total; the extract identifiers and
timestamps; the manual adjustment register; the approval record; the retained copy of the report as
issued.

**8. Responsible role.** The **project controls lead** for the report as issued; the **cost engineer**
and **planner** for the reconciliation of their own figures; the **decision owner** for reliance.

**9. Approval authority.** The **project controls lead** approves the report for issue. A manual
adjustment that is **material** must be approved by the **decision owner** for the report, recorded
before issue.

**10. Independence requirement.** Not required for preparation. The reconciliation under PR-02 must be
capable of re-performance by a **competent reviewer** who did not prepare the report; where the
adopting organisation's governance requires assurance over reporting, that assurance must be
**independent** of report preparation.

**11. Materiality or threshold.** Every reported figure must reconcile; the **materiality rule** does
not license an unreconciled figure, it decides only which differences must be corrected before issue
rather than explained in the reconciliation, and which manual adjustments need the second approval in
element 9. *Scaling:* on a USD 2 million refurbishment reconciliation is a short schedule of totals
per source; on a USD 5 billion programme it is a standing control-account-level reconciliation with
exception thresholds set by the adopting organisation's governance. The obligation to explain every
difference does not change with size — the level at which differences are aggregated does, and that
level must be recorded.

**12. Exception and waiver.** Where a source system is unavailable at the cut-off, the report may be
issued using the last **current** extract, provided the extract date is stated on the face of the
report next to the affected figures, the reason is recorded, and the reconciliation is completed and
reissued within a stated period. No exception is permitted to PR-01 or PR-04.

**13. Escalation trigger.** An unreconciled difference that is **material** and cannot be explained
before issue; an instruction to issue a report without completing the reconciliation; discovery that a
manual adjustment was made without record.

**14. AI application.** AI may extract and normalise data from source systems, produce the
reconciliation schedule, detect differences and unusual movements between periods, and draft the
report's factual sections.

**15. AI prohibition.** AI must not approve a report for issue, decide that a difference is
immaterial, make an unrecorded adjustment, or be named as the approver.

**16. AI verification.** Reconciliation plus independent recomputation: the professional must confirm
that each AI-produced reported total agrees to the source system total by re-extracting or
re-computing at least the largest total and a stated sample of the remainder, must confirm that every
difference the tool suppressed or auto-matched is itemised, and must record the sample basis. An
AI-generated reconciliation that shows no differences must be tested against a manual check of at
least one total, because a reconciliation that always balances is more often a defect than a result.

**17. External reference.**

- **COSO (Committee of Sponsoring Organizations of the Treadway Commission) — *Internal Control —
  Integrated Framework*.** Cited for the concept of control activities over information used in
  reporting. Edition: 2013 recorded in the register. Nature: Manual §6 category 5, professional
  framework published by a private-sector committee of professional bodies; not regulatory authority
  in itself, though widely imported by regulators. Checked 2026-08-03 (register EXT-084).
  Applicability: voluntary unless an organisation or a regulator adopts it.

**18. Jurisdictional caution.** Where a report feeds statutory financial reporting, a regulated
disclosure or a listed entity's controls certification, the applicable legal and accounting
requirements govern that use and may impose materially stricter obligations; they are matters for
qualified local advisers.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) and `PCI-FND-LAW-06` (source
and version integrity) govern. This law adds the specific reconciliation and manual-adjustment
obligations for periodic performance reporting. See also `PCI-PCL-LAW-04.02`, `PCI-PCL-LAW-04.03`,
`PCI-PCL-LAW-05.01`, `PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-11.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 4 · KA 4.3 Management reporting; KA 4.4
Data visualisation and storytelling for controls. Also Domain 2 · KA 2.5 Management reporting versus
statutory reporting.

**21. Compliance test.** Compliance is demonstrated when a reviewer, given the issued report and the
retained extracts, can agree each reported total to its source system total using only the retained
reconciliation, with every difference itemised and explained; when the report states one cut-off per
figure; when every manual adjustment appears in the register with a reason and an approver; and when
the report names its approver and the approval predates issue. A figure that cannot be agreed is an
exception. Two reviewers performing this on the same report and extracts produce the same exception
list.

**22. Breach indicators.** Reports whose totals differ from the system when queried afterwards; extract
timestamps missing or later than the stated cut-off; manual adjustments described as "alignment";
recurring round-number differences; a reconciliation prepared after the report was issued; the same
figure appearing differently in two reports of the same period.

**23. Consequence within PCI authority.** Correction required and the report withheld or reissued;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of
appeal.

**24. Examination application.** Evidence selection: given a report and three system extracts, the
candidate identifies the figure that does not reconcile and names the record needed to resolve it.
Scenario judgement: a request to issue with the reconciliation "to follow".

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-04-01 *Reporting Integrity*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-04.02 — Explanation of Material Variance

**1. Normative requirement.** A credential holder must explain each **material** variance by stating
its cause, evidenced from the **source record**.

**2. Purpose.** Controls the report that describes rather than explains. "Cost variance driven by
higher than planned expenditure" restates the number; it names no cause, supports no decision, and
conceals whether anyone knows why. The failure is not laziness — it is that a narrative that says
nothing cannot be contradicted, and a report full of such narrative can be issued every period without
anyone ever being wrong.

**3. Scope.** All candidates and credential holders who prepare, review, approve or give assurance
over variance analysis or performance commentary in any report, on any project. It covers cost,
schedule, earned value, quantity, productivity and commercial variances.

**4. Defined terms.** *material*, *source record*, *evidence*, *decision owner*, *escalation
threshold*, *objective evidence of progress*, *approved*.

**5. Required actions.** The professional must give each material variance a cause, a consequence and
a response, each attributable.

- **PCI-PCL-LAW-04.02-PR-01 — Cause stated to a source.** Each material variance explanation must name
  the specific cause and the record that evidences it — the change, the productivity record, the rate,
  the quantity, the event or the schedule delay — and must not restate the variance in words.
- **PCI-PCL-LAW-04.02-PR-02 — Decision content.** Each material variance explanation must state the
  expected effect on the forecast completion cost and date, the action proposed or taken, the named
  individual accountable for that action, and the date by which the effect will be known.
- **PCI-PCL-LAW-04.02-PR-03 — Separation of fact from assessment.** Statements of fact, statements of
  assumption and statements of opinion within performance commentary must be distinguishable, so that
  a reader can tell what is recorded from what is expected.
- **PCI-PCL-LAW-04.02-PR-04 — Consistency with the forecast.** A variance explanation must not assert
  a recovery, a mitigation or an offsetting benefit that is not reflected in the issued forecast; where
  the assertion is not yet in the forecast, that fact must be stated.

**6. Prohibited actions.** Explaining a variance by restating it; attributing a variance to an
unevidenced cause; asserting recovery without a plan behind it; issuing commentary that mixes fact and
expectation without distinction; omitting a material adverse variance while explaining a favourable
one; recycling the previous period's commentary unchanged when the underlying position has moved.

**7. Required evidence.** The variance analysis with cause, source reference, effect, action and owner
per material variance; the records referenced; the link from each asserted recovery to the forecast or
the statement that it is not yet included; the approver's identity and date.

**8. Responsible role.** The **control account owner** for the cause of variances in their control
account; the **project controls lead** for the analysis as issued; the **decision owner** for the
decisions the analysis supports.

**9. Approval authority.** The **project controls lead** approves the variance analysis for issue. The
**decision owner** approves any action commitment recorded in it.

**10. Independence requirement.** Not required for preparation — the cause of a variance is best known
to those close to the work. Where a variance is material and the explanation attributes it to a party
whose performance the preparer is accountable for, the explanation must be reviewed by a person
**independent** of that accountability before issue.

**11. Materiality or threshold.** Explanation is required for every variance that meets the
**materiality rule** applied to the report, and for every variance below it that forms part of a
recurring pattern in the same direction across three or more periods — because a pattern of
individually immaterial variances is a material fact about the project. The adopting organisation's
governance sets the quantum and the basis (value, percentage of control account budget, or effect on
completion cost or date); where it sets none, the professional records the rule applied. *Scaling:* on
a USD 2 million refurbishment a variance rule expressed as a percentage of a small control account
generates too many items to be useful, so a value floor is normally added; on a USD 5 billion
programme a value-only rule generates none at the control-account tier, so a percentage is normally
added. The professional must record which basis was used and why — this is the threshold most often
set without thought, and it is where a small project and a megaproject genuinely diverge.

**12. Exception and waiver.** No exception is permitted to explaining a material variance. Where the
cause is not yet known at the cut-off, the explanation must state that the cause is under
investigation, name the individual investigating and the date by which the cause will be reported —
"under investigation" without those two elements is not an explanation.

**13. Escalation trigger.** A material variance whose cause remains unknown after one further
reporting period; an instruction to omit or soften a material adverse variance explanation; discovery
that an asserted recovery has no plan behind it.

**14. AI application.** AI may compute variances, rank them by size and by movement, correlate them
with change, risk, productivity and schedule data to propose candidate causes, detect recurring
sub-threshold patterns, and draft commentary.

**15. AI prohibition.** AI must not state a cause as fact, attribute a variance to a party, assert a
recovery, approve commentary for issue, or be the source of an explanation that the professional has
not evidenced.

**16. AI verification.** Source tracing plus clause-to-summary comparison: for every AI-proposed cause,
the professional must open the record the tool relied on and confirm that the record supports the
cause and the magnitude; must confirm that no cause is asserted for which the tool cited nothing; and
must rewrite or delete any generated sentence whose evidential basis cannot be produced. Fluency is
not evidence, and an AI narrative that reads well is the most likely to be issued unverified.

**17. External reference.**

- **COSO — *Internal Control — Integrated Framework*.** Cited for the concept of quality information
  supporting internal control. Edition: 2013 per register. Nature: Manual §6 category 5, professional
  framework; not regulatory authority. Checked 2026-08-03 (EXT-084). Voluntary unless adopted.
- **AACE International — *Total Cost Management Framework*.** Cited for the analysis step in the
  cost-control cycle. Edition not asserted — unverified. Nature: Manual §6 category 5, professional
  framework. Register EXT-064. Persuasive only, on adoption.

**18. Jurisdictional caution.** A variance explanation that attributes cause to a contracting party
can affect contractual position, notice obligations and later dispute. Whether such a statement is
prudent, and how it should be worded, is a matter for the contract administrator and qualified
counsel — see `PCI-PCL-LAW-07.01`.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) governs; this law adds the
required content of an explanation, which the foundational law does not enumerate. See also
`PCI-PCL-LAW-04.01`, `PCI-PCL-LAW-04.03`, `PCI-PCL-LAW-03.04`, `PCI-PCL-LAW-06.03`,
`PCI-PCL-LAW-13.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 4 · KA 4.2 Variance analysis; KA 4.3
Management reporting. Also Domain 6 · KA 6.2 Variances and performance indices.

**21. Compliance test.** Compliance is demonstrated when, for every variance meeting the stated
materiality rule, the analysis names a cause, cites a record, states the effect on completion cost and
date, names an accountable individual and a date, and — where a recovery is asserted — either shows it
in the forecast or states that it is not yet included. An explanation containing no noun that is not
already in the variance table is a failure of this test. A reviewer must be able to request any three cited
records and receive them; failure to produce one is a failure of this test. Two reviewers applying this to the
same report identify the same unsupported explanations.

**22. Breach indicators.** Commentary that repeats the variance figures in prose; the same explanation
appearing in consecutive periods with only the numbers changed; recoveries asserted every period and
never realised; adverse variances explained in less detail than favourable ones; causes attributed to
"market conditions" or "productivity" with no record cited; actions with no named owner.

**23. Consequence within PCI authority.** Correction required and the analysis withheld or reissued;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of
appeal.

**24. Examination application.** Scenario judgement: the candidate receives four variance explanations
and must identify which state a cause and which restate the number. Evidence selection: naming the
record that would substantiate a claimed productivity cause.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-04-01 *Reporting Integrity*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-04.03 — Correction and Restatement of a Reported Error

**1. Normative requirement.** A credential holder who becomes aware that a figure already reported was
**material** and wrong must correct it in a **restatement** issued to every recipient of the original.

**2. Purpose.** Controls the failure of the quiet fix. An error discovered after issue is corrected in
the next report's opening position, the movement is absorbed into a variance, and nobody who acted on
the original figure learns that it was wrong. The correction is real; the accountability is not. This
law converts correction into an act that is visible to the people it affected.

**3. Scope.** All candidates and credential holders who discover, assess, correct, approve or give
assurance over a correction to a figure that has already been issued to a **decision owner**, a
client, a lender, an auditor, an assessor or a governance body, on any project.

**4. Defined terms.** *restatement*, *material*, *decision owner*, *approved*, *evidence*, *escalation
threshold*, *source record*.

**5. Required actions.** The professional must assess, correct and disclose, in that order, and must
not let the sequence stop at the first step.

- **PCI-PCL-LAW-04.03-PR-01 — Assessment on discovery.** On discovering a possible error the
  professional must, within the current reporting cycle, quantify its effect on the figures reported,
  determine whether it meets the materiality rule, and record that assessment with its date, whatever
  the conclusion.
- **PCI-PCL-LAW-04.03-PR-02 — Restatement content.** A restatement must show the figure as originally
  reported, the corrected figure, the difference, the cause of the error, the periods affected and the
  decisions known to have relied on the original figure.
- **PCI-PCL-LAW-04.03-PR-03 — Distribution to original recipients.** The restatement must be issued to
  every recipient of the original figure, and where a recipient is outside the professional's control,
  the issue must be requested in writing of the person who can make it and that request recorded.
- **PCI-PCL-LAW-04.03-PR-04 — Cause recorded and control addressed.** Each restatement must record the
  control weakness that allowed the error, and the corrective action taken, so that a repeated error
  is visible as a repeat.

**6. Prohibited actions.** Absorbing a material correction into the next period's variance without
disclosure; describing a correction as a change, a movement, a realignment or a refinement; delaying a
restatement until a favourable movement offsets it; restating without telling the recipients of the
original; correcting a figure and leaving the narrative that relied on it unchanged.

**7. Required evidence.** The assessment record with its date and conclusion; the restatement as
issued; the distribution record; the cause-and-control record; the approval of the restatement.

**8. Responsible role.** The individual who discovers the error must record and raise it; the
**project controls lead** is accountable for preparing the restatement; the **decision owner** for the
original report is accountable for its distribution.

**9. Approval authority.** The **decision owner** for the original report approves the restatement and
its distribution. Where the error affects a report issued outside the organisation, the approval
authority named in the adopting organisation's governance for external communications approves the
issue.

**10. Independence requirement.** The assessment under PR-01 may be performed by the preparer. Where
the error arose in work the preparer performed and is **material**, the assessment must be reviewed by
a person **independent** of that work before the conclusion "not material" is accepted — because that
is the conclusion with the strongest incentive behind it.

**11. Materiality or threshold.** The **materiality rule** applied to the original report decides
whether restatement is required; an error below it must still be corrected and recorded, but may be
corrected in the ordinary course. Two errors below the rule that share a cause, or that accumulate
above it, require restatement. *Scaling:* the same rule serves a USD 2 million refurbishment and a USD
5 billion programme because it is expressed against the report the figure appeared in, not against the
project total — an error material to a control account report is restated in that report even where it
is immaterial to the programme.

**12. Exception and waiver.** No exception is permitted to the duty to assess and record. The
**decision owner** may approve deferring the *issue* of a restatement to the next scheduled report
where no decision will be taken in the interim, provided the deferral, its reason and the interim
decisions checked are recorded. A deferral must never exceed one reporting cycle.

**13. Escalation trigger.** A material error already relied on for a decision; an instruction not to
restate; a second error in the same figure or from the same cause; a restatement approved but not
distributed.

**14. AI application.** AI may detect anomalies suggesting an error, quantify the effect across
affected periods, identify which reports and recipients contained the figure, and draft the
restatement schedule.

**15. AI prohibition.** AI must not decide that an error is immaterial, decide that restatement is
unnecessary, approve or issue a restatement, or draft an explanation of cause that the professional
has not evidenced.

**16. AI verification.** Independent recomputation and source tracing: the professional must recompute
the corrected figure from the **source record** without the tool, must confirm the list of affected
periods and recipients against the retained distribution records, and must confirm the stated cause
against the record that shows it.

**17. External reference.** Not applicable — the duty to correct a reported figure is a PCI
professional requirement; where an entity's financial statements are affected, the applicable
financial-reporting framework governs that treatment separately and is addressed in element 18 rather
than cited here as authority for this law.

**18. Jurisdictional caution.** Correction of an error in statutory financial statements is governed by
the entity's applicable accounting framework and, where the entity is regulated or listed, by
disclosure obligations that may be immediate. Those determinations require qualified accounting and
legal advice, and they are not satisfied by a controls restatement.

**19. Related PCI Laws.** `PCI-FND-LAW-15` (correction duty) governs; this law adds the restatement
content, the distribution obligation and the control-cause record, none of which the foundational duty
specifies. See also `PCI-PCL-LAW-04.01`, `PCI-PCL-LAW-04.02`, `PCI-PCL-LAW-03.03`,
`PCI-PCL-LAW-11.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 4 · KA 4.3 Management reporting. Also
Domain 2 · KA 2.5 Management reporting versus statutory reporting; Domain 11 · KA 11.3 Internal
control and segregation of duties.

**21. Compliance test.** Compliance is demonstrated when, for each error discovered in the period: (a)
a dated assessment record exists stating the quantified effect and the materiality conclusion; (b)
where material, a restatement exists showing original figure, corrected figure, difference, cause and
periods affected; (c) a distribution record shows it reaching every recipient of the original; and (d)
the control cause is recorded. An error corrected only inside the next period's opening position, with
no assessment record, is a failure of this test. Two reviewers comparing the retained reports before and after
the correction reach the same conclusion about whether a restatement was issued.

**22. Breach indicators.** Prior-period figures that change between reports without a restatement;
opening positions that do not equal the previous closing position; corrections described as
refinements; a discovered error with no dated assessment; the same error recurring with no recorded
control action; restatements approved but with no distribution record.

**23. Consequence within PCI authority.** Correction required and the restatement issued; output
withheld pending correction; additional review; escalation; failure of the associated examination
competency; ethics review; certification investigation, suspension or withdrawal — each subject to due
process and a right of appeal.

**24. Examination application.** Ethical dilemma: an error discovered after a funding decision, with a
suggestion to "pick it up in the next report". Evidence selection: identifying which records establish
who relied on the original figure.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · New law — the eighteen-field set contained no correction or
restatement requirement.

---
## Domain 5 — Cost Management, Cost Control & Change Control

### PCI LAW PCI-PCL-LAW-05.01 — Completeness and Reconciliation of the Recorded Cost Position

**1. Normative requirement.** A credential holder must reconcile the reported actual cost position to
the books of account and to the **commitment** and **accrual** records at every reporting **cut-off**.

**2. Purpose.** Controls the divergence between the cost system a project manages by and the ledger
the organisation is accountable for. Once the two are not reconciled, both are defensible and neither
is true: the project reports performance against a cost base the finance function does not recognise,
duplicate and missing costs are invisible, and the difference is discovered at year end when it can no
longer be explained.

**3. Scope.** All candidates and credential holders who prepare, review, approve or give assurance
over the actual cost position, the commitment position, or the actual-cost feed into earned value and
forecasting, on any project and under any accounting system. It applies to preparation, review,
approval and assurance.

**4. Defined terms.** *commitment*, *accrual*, *cut-off*, *duplicate cost*, *source record*,
*material*, *evidence*, *reproducible*, *approved*.

**5. Required actions.** The professional must reconcile in total and by control account, and must
demonstrate that the cost position is complete as well as agreed.

- **PCI-PCL-LAW-05.01-PR-01 — Commitment completeness.** The commitment register must include every
  executed contract, purchase order, subcontract, call-off and other instrument with financial effect
  that is open at the cut-off, must be reconciled to the procurement or contract system, and must show
  for each instrument its total value, the value received or performed to date and the value remaining.
- **PCI-PCL-LAW-05.01-PR-02 — Duplicate-cost detection.** Before issue, the cost position must be
  tested for **duplicate cost** — at minimum by matching invoices to the accruals and commitments they
  discharge, by testing for repeated supplier, value and date combinations, and by testing for one cost
  event coded to two cost codes — and the test, its method and its findings must be recorded.
- **PCI-PCL-LAW-05.01-PR-03 — Remaining-cost evidence.** The remaining value of each open commitment
  must be supported by the contract or order value less the value received or performed, evidenced
  from the source record, and not by a residual derived from a budget or a forecast.
- **PCI-PCL-LAW-05.01-PR-04 — Reconciliation to the books of account.** The reported actual cost must
  be reconciled to the ledger for the same period, with every difference itemised by type — timing,
  scope of the project cost object, accrual treatment, intercompany allocation, currency — and
  explained.

**6. Prohibited actions.** Reporting an actual cost that has never been reconciled to the ledger;
maintaining a commitment register that excludes instruments known to exist; deriving remaining
commitment value from budget rather than from the instrument; leaving a duplicate in the position once
identified; explaining a reconciling difference as "system timing" without identifying the
transactions; suppressing a reconciling difference by adjusting the reported figure.

**7. Required evidence.** The reconciliation from the reported cost position to the ledger, in total
and by control account; the commitment register with the reconciliation to the source system; the
duplicate-cost test record; the evidence supporting remaining commitment values; the approver's
identity and date.

**8. Responsible role.** The **cost engineer** prepares; the **project controls lead** issues; the
responsible finance controller owns the ledger against which the reconciliation is performed and is
not displaced by this law.

**9. Approval authority.** The **project controls lead** approves the reconciled cost position. An
unreconciled difference that is **material** may be carried only with the recorded approval of the
**decision owner** for the cost position and only with a stated resolution date.

**10. Independence requirement.** The reconciliation must be re-performable by a **competent
reviewer** who did not prepare it. Where a material difference persists for more than two consecutive
periods, its investigation must be performed by a person **independent** of the preparer.

**11. Materiality or threshold.** Reconciliation is required in every period without threshold; the
**materiality rule** decides only which differences must be resolved before issue and which may be
carried with an approved resolution date. Duplicate-cost testing has no value threshold, because
duplicates cluster at low values where nobody looks. *Scaling:* on a USD 2 million refurbishment
reconciliation runs at project total and by control account in one schedule; on a USD 5 billion
programme it runs by control account with roll-up, and the professional must record the tier at which
differences are aggregated. The obligation to itemise a material difference by transaction does not
change with scale.

**12. Exception and waiver.** Where the ledger for the period is not closed at the controls cut-off, the
reconciliation may be performed against the last closed period plus the movement, provided the method
is recorded and the full reconciliation is completed within the period stated in the adopting
organisation's procedure. No exception is permitted to PR-01 or PR-02.

**13. Escalation trigger.** A material unreconciled difference at two consecutive cut-offs; discovery
of a duplicate cost that has already been reported or paid; discovery of a commitment that was
excluded from the register; an instruction to report a cost position that has not been reconciled.

**14. AI application.** AI may match invoices, receipts, commitments and accruals; identify probable
duplicates including near matches; classify reconciling differences by type; and draft the
reconciliation schedule.

**15. AI prohibition.** AI must not clear a reconciling difference, write off a difference as
immaterial, approve the cost position, or determine that the commitment register is complete.

**16. AI verification.** Reconciliation plus sampling on a stated basis: the professional must agree
the AI-produced totals to the ledger and to the source systems independently of the tool; must
personally examine every difference the tool classified as immaterial above a level recorded in the
verification record; and must test AI-identified duplicates and AI-cleared near matches against the
underlying documents, recording the sample basis and the error rate. An absence of flagged duplicates
must be tested by an independent duplicate query, because a tool cannot evidence an absence.

**17. External reference.**

- **COSO — *Internal Control — Integrated Framework*.** Cited for reconciliation as a control activity.
  Edition: 2013 per register. Nature: Manual §6 category 5, professional framework; not regulatory
  authority. Checked 2026-08-03 (EXT-084). Voluntary unless adopted.
- **AACE International — *Total Cost Management Framework*.** Cited for the measurement step of the
  cost-control cycle. Edition not asserted — unverified. Nature: Manual §6 category 5, professional
  framework. Register EXT-064. Persuasive only, on adoption.

**18. Jurisdictional caution.** The ledger, the cost object and the allocation of shared or
intercompany cost are determined by the entity's accounting policies and local law. Reconciling
differences arising from those determinations require the finance function's and, where relevant, a
qualified adviser's input.

**19. Related PCI Laws.** `PCI-FND-LAW-07` (data lineage) governs; this law adds the specific
reconciliation to the books of account and the duplicate-cost obligation. See also
`PCI-PCL-LAW-01.01`, `PCI-PCL-LAW-01.02`, `PCI-PCL-LAW-01.03`, `PCI-PCL-LAW-03.04`,
`PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-07.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 5 · KA 5.2 The cost control cycle; KA 5.1
The cost management framework. Also Domain 11 · KA 11.2 Procure-to-Pay; Domain 1 · KA 1.3 Accrual
accounting and the matching concept.

**21. Compliance test.** Compliance is demonstrated when the reported actual cost agrees to the ledger
for the period with every difference itemised by type and explained; when the commitment register
agrees to the procurement or contract system with every open instrument present; when a duplicate-cost
test record exists stating the method used and the items examined; and when the remaining value of a
sample of open commitments can each be derived from the instrument and the receipted value. An open
instrument absent from the register is a failure of this test. Two reviewers performing the ledger reconciliation
from the same extracts reach the same difference list.

**22. Breach indicators.** A persistent difference between the cost system and the ledger described the
same way each period; a commitment register whose total never changes when orders are placed; remaining
commitment values equal to budget less actual; suppliers appearing twice with similar names; credit
notes without matching original invoices; a reconciliation prepared only at year end.

**23. Consequence within PCI authority.** Correction required and the cost position withheld until
reconciled; additional review; escalation; failure of the associated examination competency; ethics
review; certification investigation, suspension or withdrawal — each subject to due process and a right
of appeal.

**24. Examination application.** Calculation review: given a cost ledger extract, a commitment report
and a draft cost position, the candidate identifies the duplicate, the missing commitment and the
unexplained difference. Scenario judgement: a difference explained as "timing" for a fourth
consecutive period.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · New law — the eighteen-field set had no actual-cost reconciliation
or commitment-completeness requirement.

---

### PCI LAW PCI-PCL-LAW-05.02 — Identification and Registration of Change

**1. Normative requirement.** A credential holder must enter every potential **change** in the change
register in the reporting cycle in which it becomes known.

**2. Purpose.** Controls the failure that precedes every change-control failure: the change that was
never raised. Work is instructed in a meeting, absorbed as "clarification", performed as a goodwill
gesture, or deferred as "we will pick it up at the end". Registration is what converts an event into
something the project can assess, price, approve or reject — and an unregistered change can do none of
those things while still consuming cost and time.

**3. Scope.** All candidates and credential holders who identify, record, review, approve or give
assurance over changes to scope, budget or schedule — including client instructions, design
development, site instructions, variations, compensation events, scope transfers between parties, and
changes arising from risk materialising — on any project and under any contract form.

**4. Defined terms.** *change*, *approved*, *trend*, *material*, *evidence*, *cut-off*, *escalation
threshold*, *source record*.

**5. Required actions.** The professional must register first and assess afterwards, and must never
allow the assessment to become a condition of registration.

- **PCI-PCL-LAW-05.02-PR-01 — Registration regardless of merit.** A potential change must be
  registered even where its entitlement is disputed, its value is unknown, its approval is unlikely or
  its originator is the professional's own organisation; a register entry is a record of an event, not
  an admission about it.
- **PCI-PCL-LAW-05.02-PR-02 — Minimum register content.** Each entry must carry a unique identifier,
  the date the change became known, the source or instructing document, a description of the work
  affected, the originator, the current status and the individual accountable for progressing it.
- **PCI-PCL-LAW-05.02-PR-03 — Cumulative and related-change view.** The register must show, at each
  cut-off, the cumulative value and cumulative schedule effect of all approved changes and of all
  pending changes, and must group changes that share a cause so that their combined effect is visible
  rather than only their individual effects.
- **PCI-PCL-LAW-05.02-PR-04 — Reconciliation to the trend register and the forecast.** Every open
  register entry must be reflected in the trend position and in the forecast under
  `PCI-PCL-LAW-03.04`, or be listed with the reason it is excluded.

**6. Prohibited actions.** Performing changed work before the change is registered; leaving a change
unregistered because entitlement is doubtful; splitting one change into several to keep each below an
approval threshold; registering a change with a value only and no description; allowing pending
changes to accumulate outside the register; recording a change with an effective date earlier than the
date it became known in order to reset a notice period.

**7. Required evidence.** The change register with the PR-02 content per entry; the instructing or
source documents; the cumulative position at each cut-off; the reconciliation to the trend register and
the forecast; the record of changes grouped by common cause.

**8. Responsible role.** Any credential holder who becomes aware of a potential change must raise it;
the **project controls lead** is accountable for the register's completeness; the **commercial lead**
is accountable for the contractual characterisation of an entry.

**9. Approval authority.** No approval is required to *register* a change, and no person may refuse
registration. Approval of the change itself is governed by `PCI-PCL-LAW-05.04`.

**10. Independence requirement.** Not applicable to registration — requiring an independent person to
register a change would delay the record and defeat the law's purpose. Independence attaches to
approval, under `PCI-PCL-LAW-05.04`.

**11. Materiality or threshold.** Every potential change is registered, with no value threshold,
because the value is often unknown at the moment of identification and because splitting is the
standard method of evading a threshold. The **materiality rule** governs only the depth of assessment
under `PCI-PCL-LAW-05.03` and the level of approval under `PCI-PCL-LAW-05.04`. *Scaling:* on a USD 2
million refurbishment a single register with tens of entries serves; on a USD 5 billion programme
registers are held per contract or per package with a consolidated cumulative view, and the
consolidation rule must be recorded. Neither may operate a de minimis for registration.

**12. Exception and waiver.** No exception is permitted. Where an instruction must be executed
immediately for safety or to protect the works, the work may proceed before registration, provided the
change is registered at the first opportunity and no later than the current reporting cycle, with the
reason for the sequence recorded.

**13. Escalation trigger.** Changed work performed without a register entry; an instruction not to
register a change; a pattern of changes valued just below an approval threshold; the cumulative pending
value reaching a level defined by the adopting organisation's governance as requiring notification.

**14. AI application.** AI may scan correspondence, minutes, site records, requests for information and
drawings for language indicating instructed or implied change; propose register entries; group entries
by common cause; and compute cumulative positions.

**15. AI prohibition.** AI must not decide that an event is not a change, close a register entry, set
the contractual characterisation of a change, or determine entitlement.

**16. AI verification.** Clause-to-summary comparison plus source tracing: for each AI-proposed entry
the professional must open the source document and confirm that it says what the tool reports; for
each event the tool assessed as not a change, the professional must apply their own judgement, because
an AI conclusion of "no change" is exactly the conclusion this law exists to prevent being reached
casually.

**17. External reference.**

- **FIDIC (International Federation of Consulting Engineers) — FIDIC suite of conditions of contract.**
  Cited generically for the existence of variation and claim mechanisms with time-bound procedures.
  **No clause number, book or edition asserted.** Nature: Manual §6 category 4, contract framework.
  Checked 2026-08-03 (register EXT-050). Applicability: binds only the parties to a contract that
  adopts it, and only on its own terms.
- **NEC (Thomas Telford / ICE) — NEC4 suite of contracts.** Cited generically for the
  compensation-event mechanism as an example of early notification. No clause number asserted. Nature:
  Manual §6 category 4, contract framework. Checked 2026-08-03 (register EXT-051). Binds only adopting
  parties.

**18. Jurisdictional caution.** Whether an event is a variation, a compensation event, a claim or none
of these, and what notice it requires and by when, are questions of the governing contract and the
governing law. A register entry is a controls record and neither creates nor waives entitlement; the
contractual position requires the contract administrator and, where contested, qualified counsel.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) governs. This law adds the
obligation to record the event before its merit is settled — a domain-specific requirement the
foundational law does not contain. See also `PCI-PCL-LAW-05.03`, `PCI-PCL-LAW-05.04`,
`PCI-PCL-LAW-03.04`, `PCI-PCL-LAW-07.02`, `PCI-PCL-LAW-10.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 5 · KA 5.4 Change control and cost impact.
Also Domain 7 · KA 7.2 Contract management; Domain 8 · KA 8.4 Monitoring & Controlling.

**21. Compliance test.** Compliance is demonstrated when every instructing document, minute recording
an instruction, and site record of changed work in the period can be matched to a register entry dated
in the same reporting cycle; when every entry carries the PR-02 content; and when the register's
cumulative pending and approved positions at the cut-off agree to the forecast or are reconciled to it
with reasons. An instruction with no register entry is a failure of this test. Two reviewers testing the same
correspondence sample against the same register produce the same unmatched list.

**22. Breach indicators.** Register entries created in batches at period end; changes with a
registration date long after the instruction date; several changes of similar value just below an
approval threshold; work visible on site that appears in no register; pending changes with no
accountable individual; a cumulative pending value that never moves.

**23. Consequence within PCI authority.** Correction required and the register completed; additional
review; escalation; failure of the associated examination competency; ethics review; certification
investigation, suspension or withdrawal — each subject to due process and a right of appeal.

**24. Examination application.** Scenario judgement: a set of minutes, emails and site records from
which the candidate must identify the events requiring registration and the one that does not.
Escalation decision: an instruction to hold a change off the register until entitlement is agreed.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-05-01 *Change Control*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-05.03 — Completeness of Change Impact Assessment

**1. Normative requirement.** A credential holder must assess the full effect of a proposed **change**
on cost, schedule, risk and interfacing work before it is submitted for approval.

**2. Purpose.** Controls the change approved on a direct cost alone. The disruption, the acceleration
needed to hold the date, the effect on adjacent work, the additional risk exposure and the knock-on to
other contracts arrive later and are absorbed as performance variance, so the change appears cheap and
the project appears badly run. This is the single most common route by which an approved change
becomes an unapproved overrun.

**3. Scope.** All candidates and credential holders who prepare, review, approve or give assurance
over the assessment of a change, on any project and under any contract form. It covers changes
originated by any party, including omissions and de-scopes, where the same analysis applies in reverse.

**4. Defined terms.** *change*, *approved*, *material*, *evidence*, *trend*, *decision owner*,
*escalation threshold*, *objective evidence of progress*.

**5. Required actions.** The professional must assess named categories of effect and must state the
result of each, including where the effect is nil.

- **PCI-PCL-LAW-05.03-PR-01 — Direct and indirect cost.** The assessment must state the direct cost,
  the effect on preliminaries, time-related cost, supervision and plant, and the disruption or
  productivity effect on unchanged work; each stated with its basis, and a nil effect stated as nil
  rather than omitted.
- **PCI-PCL-LAW-05.03-PR-02 — Schedule effect through the network.** The schedule effect must be
  assessed by inserting or amending the affected activities in a copy of the current schedule and
  running the network, not by estimating a duration in isolation; the resulting effect on the critical
  path and on completion must be stated.
- **PCI-PCL-LAW-05.03-PR-03 — Risk and interface effect.** The assessment must state the change's
  effect on the risk register — risks created, removed or altered — and on interfacing packages,
  contracts, permits, approvals and third parties.
- **PCI-PCL-LAW-05.03-PR-04 — Cumulative effect.** The assessment must state the cumulative effect of
  this change together with all approved and pending changes on the completion cost, the completion
  date and the remaining contingency, so that the decision is taken on the position after the change
  rather than on the change alone.
- **PCI-PCL-LAW-05.03-PR-05 — Basis and exclusions.** The assessment must state its assumptions, its
  exclusions, the period for which its pricing is valid, and what will change if approval is delayed
  beyond that period.

**6. Prohibited actions.** Submitting a change for approval with direct cost only; assessing schedule
effect without running the network; stating a nil schedule effect that the network does not support;
omitting the cumulative position; pricing a change on an assumption of approval by a date without
saying so; assessing a change on scope the assessor has not read.

**7. Required evidence.** The assessment document with each PR category addressed; the schedule
fragment or scenario file used for PR-02 with its run date; the risk register entries created or
altered; the cumulative position at the assessment date; the assumptions and exclusions; the assessor's
identity and date.

**8. Responsible role.** The **cost engineer** for the cost assessment; the **planner** for the
schedule assessment; the **risk lead** for the risk effect; the **commercial lead** for the
contractual characterisation; the **project controls lead** for the assembled assessment.

**9. Approval authority.** The assessment is not itself approved; it is submitted to the **change
authority** under `PCI-PCL-LAW-05.04`. The **project controls lead** confirms the assessment is
complete before submission and records that confirmation.

**10. Independence requirement.** The person who prepares the assessment must not also hold the
**change authority** for that change — that separation is required by `PCI-PCL-LAW-05.04`. Where the
change arises from an error or omission by the assessor's own organisation, the cost and schedule
assessment must be reviewed by a person **independent** of that work before submission.

**11. Materiality or threshold.** Every change requires an assessment; the **materiality rule** and the
adopting organisation's governance set the *depth*, and that scaling must be recorded — for example a
banded approach in which changes below a stated value are assessed by a documented simplified method
and changes above it require the full PR-01 to PR-05 treatment. A simplified method must still state
schedule and cumulative effect, because those are the effects a simplified method most often loses.
*Scaling:* on a USD 2 million refurbishment the network run under PR-02 is a five-minute exercise on a
100-activity schedule; on a USD 5 billion programme it is a controlled scenario run on the affected
sub-network with the interface effects assessed separately. Neither is exempt from running the network.

**12. Exception and waiver.** Where a change must be instructed before assessment can be completed —
an emergency, a safety instruction, or an instruction the contract obliges the party to obey — the
**change authority** may approve on a partial assessment provided the missing categories are named,
the assessment is completed within a stated period, and the approval records that it was given on an
incomplete basis. No exception is permitted to completing the assessment afterwards.

**13. Escalation trigger.** A change presented for approval with a category of effect unassessed and
unnamed; an instruction to state a nil schedule effect that the network does not support; a cumulative
position that breaches the project's approved cost or date and has not been reported.

**14. AI application.** AI may extract the changed scope from drawings and specifications, propose
quantities and rates from historical data, identify interfacing packages and affected activities,
propose risk register effects, and compute the cumulative position.

**15. AI prohibition.** AI must not approve a change, decide that a category of effect is nil, price a
change without human verification of quantities and rates, or determine contractual entitlement or
characterisation.

**16. AI verification.** Independent recomputation, boundary testing and source tracing: the
professional must recompute the direct cost from the quantities and rates and confirm the quantities
against the changed scope documents; must re-run the schedule scenario and confirm the reported
completion effect against the network output; must test the assessment at its boundaries — approval
delayed, quantities at the upper end of the stated range — and record the sensitivity; and must confirm
each AI-proposed interface against the interface register.

**17. External reference.**

- **FIDIC — FIDIC suite of conditions of contract.** Cited generically for variation valuation and
  time-effect mechanisms. No clause number, book or edition asserted. Nature: Manual §6 category 4,
  contract framework. Checked 2026-08-03 (EXT-050). Binds only adopting parties.
- **NEC — NEC4 suite of contracts.** Cited generically for the assessment of compensation events on a
  forecast basis. No clause number asserted. Nature: Manual §6 category 4, contract framework. Checked
  2026-08-03 (EXT-051). Binds only adopting parties.
- **AACE International — Recommended Practices on risk analysis and contingency determination.** Cited
  as a class for the existence of recognised methods relevant to the risk effect of change. No
  numbered Recommended Practice asserted; not independently verified. Nature: Manual §6 category 5,
  professional framework. Register EXT-068. Persuasive only.

**18. Jurisdictional caution.** The valuation rules for a variation, the entitlement to time and to
time-related cost, and the effect of delay in instructing or approving are governed by the contract
and the governing law. This law requires the analysis; it does not determine entitlement, which
requires the contract administrator and, where contested, qualified counsel.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) and `PCI-FND-LAW-05`
(transparent assumptions) govern. This law adds the enumerated categories of effect and the cumulative
obligation. See also `PCI-PCL-LAW-05.02`, `PCI-PCL-LAW-05.04`, `PCI-PCL-LAW-03.04`,
`PCI-PCL-LAW-10.02`, `PCI-PCL-LAW-12.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 5 · KA 5.4 Change control and cost impact.
Also Domain 10 · KA 10.2 Network analysis and the Critical Path Method; Domain 12 · KA 12.2 The risk
process.

**21. Compliance test.** Compliance is demonstrated when the assessment document addresses each of the
five PR categories with a stated value or a stated nil; when a schedule scenario file or fragment exists
with a run date and the stated completion effect matches its output; when the cumulative position at
the assessment date is stated and agrees to the change register; and when the assumptions, exclusions
and pricing validity period are recorded. A stated schedule effect that the retained scenario does not
produce is a failure of this test. Two reviewers re-running the retained scenario obtain the same completion
effect.

**22. Breach indicators.** Changes assessed at nil schedule effect as a matter of routine; assessments
with no retained schedule scenario; cumulative positions that appear only when a threshold is
breached; disruption never assessed on any change; pricing validity periods absent while approvals take
months; the same assumptions copied between unrelated assessments.

**23. Consequence within PCI authority.** Correction required and the change withdrawn from approval
until assessed; additional review; escalation; failure of the associated examination competency; ethics
review; certification investigation, suspension or withdrawal — each subject to due process and a right
of appeal.

**24. Examination application.** Calculation review: given a change, a schedule and a change register,
the candidate assesses the completion effect and the cumulative position and identifies the missing
category. Scenario judgement: a change presented as "no time effect" on a critical-path activity.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-05-01 *Change Control*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-05.04 — Change Authority and Segregation of Preparation from Approval

**1. Normative requirement.** A credential holder must not approve a **change** they prepared,
assessed or priced.

**2. Purpose.** Controls self-approval, in the one process where self-approval is most tempting and
most damaging. Where the person who values a change also authorises it, no independent mind ever tests
the value, and the change-control process — which exists precisely to put a second mind between a cost
and the budget — becomes a formality that generates paperwork and prevents nothing.

**3. Scope.** All candidates and credential holders who prepare, assess, price, recommend, approve or
give assurance over changes, and all who hold or exercise a delegated change authority, on any project
and under any contract form.

**4. Defined terms.** *change*, *approved*, *change authority*, *independent*, *material*, *decision
owner*, *evidence*, *escalation threshold*.

**5. Required actions.** The professional must ensure that authority, preparation and approval are held
by different named people, and that the approval record shows it.

- **PCI-PCL-LAW-05.04-PR-01 — Recorded delegation of authority.** The value bands and the named holders
  of change authority must be recorded before they are used, and each approval must cite the band and
  the authority it was given under.
- **PCI-PCL-LAW-05.04-PR-02 — Approval record content.** Each approval must record the change
  identifier, the version of the assessment approved, the value approved, the schedule effect approved,
  the approver's name, the date, and any condition attached.
- **PCI-PCL-LAW-05.04-PR-03 — Baseline update only after approval.** The baseline must be updated to
  incorporate a change only after that change is approved, only for the value and schedule effect
  approved, and only through the version control required by `PCI-PCL-LAW-03.02`.
- **PCI-PCL-LAW-05.04-PR-04 — Audit trail from instruction to baseline.** For each approved change,
  the trail from the instructing document to the assessment, the approval and the baseline movement
  must be retained and traversable in both directions.
- **PCI-PCL-LAW-05.04-PR-05 — Aggregation rule against splitting.** Changes arising from one cause,
  one instruction or one scope item must be aggregated for the purpose of determining the approval
  band, and the aggregation applied must be recorded.

**6. Prohibited actions.** Approving one's own assessment; approving above one's recorded band;
splitting a change to bring it within a band; updating the baseline before approval or for a value
other than the one approved; approving retrospectively an instruction already executed without
recording that it was retrospective; approving a change whose assessment version is not identified.

**7. Required evidence.** The recorded delegation of authority; the approval records with PR-02
content; the retained assessment versions; the baseline movement referencing the approval; the
aggregation record where changes share a cause.

**8. Responsible role.** The **change authority** for the approval decision; the **project controls
lead** for the integrity of the trail; the **commercial lead** where the change is a contractual
variation or compensation event.

**9. Approval authority.** As recorded in the delegation of authority under PR-01, and only as
recorded. Where a change exceeds every recorded band, it must be escalated to the **decision owner**
for the project and, if the delegation so provides, above them.

**10. Independence requirement.** The approver must be **independent** of the preparation, assessment
and pricing of the change under review. Where a project is too small for a separate approver, the
change authority is exercised from outside the project — a functional line, a sponsor or a parent
entity — and the arrangement is recorded before it is used, not improvised when the first change
arises.

**11. Materiality or threshold.** Segregation applies to every change of every value; there is no
threshold below which self-approval is permitted, because a threshold on segregation is an instruction
to split. Value bands govern *which* authority approves, not *whether* segregation applies. The bands
are set by the adopting organisation's governance against its own delegation of authority. *Scaling:*
on a USD 2 million refurbishment there may be two bands and a single external approver; on a USD 5
billion programme there are typically four or five bands with a change board at the top. The law is
identical in both; only the number of bands differs.

**12. Exception and waiver.** No exception is permitted to element 1. Where an urgent instruction must
be executed before approval, the **decision owner** may authorise execution in writing, recording that
approval of the change itself remains outstanding; the change must then be assessed and approved
through the ordinary route within a stated period, and it must not be treated as approved in the
interim.

**13. Escalation trigger.** A change approved by its preparer; an approval outside a recorded band;
changes from one cause approved separately in a pattern that avoids a band; a baseline updated for a
value other than that approved.

**14. AI application.** AI may route changes to the correct authority band, detect potential splitting
by clustering changes with common causes or dates, check approval records for completeness, and
reconcile approved changes to baseline movements.

**15. AI prohibition.** AI must not approve a change, hold or exercise a change authority, be recorded
as an approver, or determine the band a change falls into where the determination requires judgement
about aggregation.

**16. AI verification.** Reconciliation plus named human approval: the professional must reconcile the
population of approved changes to the baseline movements and to the change register in total and by
control account; must confirm that every approval names a human being; and must personally review every
cluster the tool flags as possible splitting, recording the conclusion for each.

**17. External reference.**

- **COSO — *Internal Control — Integrated Framework*.** Cited for segregation of duties and
  authorisation as control activities. Edition: 2013 per register. Nature: Manual §6 category 5,
  professional framework; not regulatory authority. Checked 2026-08-03 (EXT-084). Voluntary unless
  adopted.
- **ISO — ISO 21502 *Project, programme and portfolio management — Guidance on project management*.**
  Cited for the existence of governance and control expectations around change. Edition: 2020 per
  register; no clause asserted. Nature: Manual §6 category 3, international voluntary standard.
  Checked 2026-08-03 (EXT-028). Voluntary unless adopted by regulation or contract.

**18. Jurisdictional caution.** Corporate authority to commit expenditure is determined by the entity's
constitution, its delegation of authority and applicable company law; contractual authority to instruct
a variation is determined by the contract. A PCI change authority is a controls role and confers no
corporate or contractual authority.

**19. Related PCI Laws.** `PCI-FND-LAW-04` (human decision authority) governs; this law adds the
specific segregation rule, the delegation record and the anti-splitting aggregation obligation. See
also `PCI-PCL-LAW-05.02`, `PCI-PCL-LAW-05.03`, `PCI-PCL-LAW-03.02`, `PCI-PCL-LAW-11.01`,
`PCI-PCL-LAW-12.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 5 · KA 5.4 Change control and cost impact.
Also Domain 11 · KA 11.3 Internal control and segregation of duties; Domain 8 · KA 8.4 Monitoring &
Controlling.

**21. Compliance test.** Compliance is demonstrated when, for every approved change in the period, the
approver named in the approval record is a different individual from every name on the assessment; the
approval cites a band in the recorded delegation and the value falls within it; the baseline movement
equals the approved value; and changes sharing a cause were aggregated for banding. An approval whose
approver appears anywhere on the assessment is a failure of this test, and the test requires no judgement — the
names either differ or they do not. Two reviewers comparing the same approval and assessment records
reach the same result.

**22. Breach indicators.** Approvals clustered just below a band boundary; several
changes approved on the same day from one instruction; approvers who are also the assessment's author;
baseline movements larger than approved values; conditions attached to approvals that are never tracked;
retrospective approvals with no record that they were retrospective.

**23. Consequence within PCI authority.** Correction required and the change reprocessed through the
correct authority; the affected baseline movement reversed; additional review; escalation; failure of
the associated examination competency; ethics review; certification investigation, suspension or
withdrawal — each subject to due process and a right of appeal.

**24. Examination application.** Ethical dilemma: a change the candidate priced, presented for their own
signature because "the approver is on leave and the works cannot wait". Evidence selection: identifying
which record proves the approval was within delegated authority.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-05-01 *Change Control* and
PCL-LAW-11-01 *Segregation of Duties*; both identifiers are retired and are not reused.

---
## Domain 6 — Earned Value Management & Forecasting

### PCI LAW PCI-PCL-LAW-06.01 — Earned Value Measurement Rules Fixed Before Performance

**1. Normative requirement.** A credential holder must fix and record the earned value measurement
method for each work package before performance of that work package begins.

**2. Purpose.** Controls the failure that makes earned value unfalsifiable. Where the measurement
method is chosen after the work has started — or changed once the result is known — the earned value
becomes a description of the answer wanted rather than of the work done. Fixing the rule in advance is
the only point at which the choice is made honestly, because at that point nobody knows which method
will flatter the result.

**3. Scope.** All candidates and credential holders who define, apply, review, approve or give
assurance over earned value measurement methods, on any project applying earned value in any form,
including adaptive delivery where earned value is derived from accepted increments.

**4. Defined terms.** *performance measurement baseline*, *control account*, *approved*, *objective
evidence of progress*, *material*, *evidence*, *change*, *escalation threshold*.

**5. Required actions.** The professional must record a method per work package, apply it consistently,
and change it only through change control.

- **PCI-PCL-LAW-06.01-PR-01 — Method recorded per work package.** Each work package must carry a
  recorded measurement method — for example units complete, milestone weighting, fixed-formula start
  and finish proportions, level of effort, or apportioned effort — together with the reason the method
  suits the work and, where the method uses milestones or units, the specific milestones or units and
  their weights.
- **PCI-PCL-LAW-06.01-PR-02 — Limits on level of effort.** Work packages measured as level of effort
  must be identified separately, their total budget stated as a proportion of the baseline, and level
  of effort must not be applied to work with a discrete, measurable output.
- **PCI-PCL-LAW-06.01-PR-03 — Change of method through change control.** A measurement method may be
  changed only before the work package starts, or through an approved change that records the reason,
  the effect on reported earned value to date and the treatment of the transition; a method must never
  be changed to alter a reported index.
- **PCI-PCL-LAW-06.01-PR-04 — Method consistent with the schedule.** The measurement method for each
  work package must be consistent with how progress is statused in the schedule, so that cost and
  schedule progress describe the same work.

**6. Prohibited actions.** Choosing a measurement method after performance has begun; changing a method
to move an index; applying level of effort to discrete work; measuring by expenditure ("cost spent
equals value earned") where an output measure exists; using different methods for the same work in cost
and schedule; recording a method that the progress data cannot support.

**7. Required evidence.** The measurement method record per work package with weights and milestones;
the level-of-effort register with its proportion of baseline; approved changes to any method with their
effect stated; the mapping between cost measurement and schedule statusing.

**8. Responsible role.** The **control account owner** for the method within their control account; the
**project controls lead** for the method set as a whole; the **planner** for its consistency with the
schedule.

**9. Approval authority.** The **baseline approval authority** approves the measurement method set as
part of the baseline. A change of method is approved by the **change authority** under
`PCI-PCL-LAW-05.04`.

**10. Independence requirement.** Not required for selection — the method is best chosen by those who
know the work. Approval of a *change* of method must be **independent** of the person whose reported
performance the change would affect.

**11. Materiality or threshold.** A method is required for every work package regardless of value. The
adopting organisation's governance sets the maximum proportion of the baseline that may be measured as
level of effort, on the basis of how much of its work genuinely lacks a discrete output; where it sets
none, the professional must state the proportion in use and the reason it is appropriate for the work.
*Scaling:* on a USD 2 million refurbishment a handful of work packages may be measured by milestones
with weights on one page; on a USD 5 billion programme methods are set by package type under a
recorded standard, with exceptions listed. Both must be able to answer, for any work package, "which
method, decided when, by whom".

**12. Exception and waiver.** Where work must start before its measurement method is recorded, the
**project controls lead** may authorise a provisional method in writing, provided it is confirmed or
replaced within one reporting cycle and no earned value is reported for that work package until it is.
No exception is permitted to PR-03.

**13. Escalation trigger.** A method changed after performance began without an approved change; a
proposal to move work to level of effort where a discrete output exists; earned value reported for a
work package with no recorded method.

**14. AI application.** AI may propose a measurement method from the work package's characteristics and
from historical practice, check method consistency across similar packages, detect level-of-effort
creep, and reconcile cost measurement methods to schedule statusing rules.

**15. AI prohibition.** AI must not set a measurement method as final, approve a change of method,
decide that level of effort is appropriate, or apply a method retrospectively to reported periods.

**16. AI verification.** Named human judgement recorded with reasoning, plus reconciliation: the
professional must record, for each AI-proposed method, whether it was adopted and why, and must
reconcile the adopted method set against the schedule statusing rules line by line before the baseline
is approved.

**17. External reference.**

- **SAE International (ANSI-accredited) — ANSI/EIA-748 *Earned Value Management Systems*.** Cited for
  the existence of recognised management-system expectations covering the definition of work and its
  measurement. **Edition and guideline count deliberately not asserted.** Nature: a **national
  standard**; Manual §6 **category 11, national standard**. Checked
  2026-08-03 (registers EXT-130 / EXT-090). Applicability: binding only where a contract or procurement
  regime imports it.
- **Project Management Institute — *The Standard for Earned Value Management*.** Cited for the
  existence of recognised measurement methods. Edition: not established — not independently verified.
  Nature: Manual §6 category 5, professional framework; not regulatory authority. Register EXT-061.
  Persuasive only.

**18. Jurisdictional caution.** Where a contract or procurement regime imports an earned value
management-system standard, compliance with that standard is a contractual or regulatory obligation
that may exceed this law, and its interpretation belongs to the contract administrator and qualified
counsel.

**19. Related PCI Laws.** `PCI-FND-LAW-13` (no silent override) governs; this law adds the
fix-in-advance obligation, which is the domain-specific form of preventing a silent override of the
measurement basis. See also `PCI-PCL-LAW-06.02`, `PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-03.01`,
`PCI-PCL-LAW-10.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 6 · KA 6.1 EVM fundamentals. Also Domain 3 ·
KA 3.3 The time-phased budget / cost baseline; Domain 9 · KA 9.5 Agile cost control, forecasting &
earned value.

**21. Compliance test.** Compliance is demonstrated when, for a sample of work packages selected on a
stated basis: (a) a measurement method is recorded and its record predates the work package's actual
start; (b) where the method uses milestones or units, the milestones or units and their weights are
recorded; (c) any change of method is supported by an approved change stating its effect; and (d) the
method matches the schedule statusing rule for the same work. A work package whose method record
postdates its actual start is a failure of this test — a date comparison, not a judgement. Two reviewers testing
the same sample reach the same exception list.

**22. Breach indicators.** Level of effort growing as a proportion of the baseline; methods recorded in
a single batch after the reporting period began; milestone weights adjusted between periods; work
packages whose cost progress and schedule progress diverge systematically; work packages measured by
expenditure; identical methods applied to plainly different work.

**23. Consequence within PCI authority.** Correction required and the affected earned value withheld
until the method basis is established; additional review; escalation; failure of the associated
examination competency; ethics review; certification investigation, suspension or withdrawal — each
subject to due process and a right of appeal.

**24. Examination application.** Scenario judgement: four work packages with proposed methods, one of
which is level of effort applied to discrete work. Calculation review: the effect on the reported cost
performance index of a mid-period change of milestone weights.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-06-01 *Earned Value Integrity*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-06.02 — Objective Evidence of Progress

**1. Normative requirement.** A credential holder must not report progress or earned value that is not
supported by **objective evidence of progress** at the reporting **cut-off**.

**2. Purpose.** Controls the most consequential single number in project controls. Progress asserted
without evidence drives earned value, the cost performance index, the estimate at completion, the
schedule status, the payment application and, on many contracts, the money that moves. An
unsubstantiated percentage is not an estimate — it is the point at which every downstream control
fails at once, silently, and in the same direction.

**3. Scope.** All candidates and credential holders who measure, record, review, approve or give
assurance over physical progress, earned value, milestone achievement, quantities installed or
percentage complete, on any project and under any delivery model, including agile increments assessed
as accepted.

**4. Defined terms.** *objective evidence of progress*, *evidence*, *cut-off*, *material*,
*independent*, *competent reviewer*, *source record*, *escalation threshold*.

**5. Required actions.** The professional must hold, for every progress claim, a record produced or
verifiable outside the claimant, and must be able to produce it on request.

- **PCI-PCL-LAW-06.02-PR-01 — Evidence identified per claim.** Each progress claim must identify the
  record that evidences it, by reference, so that the record can be retrieved without asking the
  claimant which one it was.
- **PCI-PCL-LAW-06.02-PR-02 — Verification of claims on a stated basis.** Progress claims must be
  verified before the earned value is reported, by physical or documentary verification of a population
  or a sample selected on a recorded basis that includes the highest-value claims and the claims that
  moved most since the previous period.
- **PCI-PCL-LAW-06.02-PR-03 — Prohibition on progress exceeding evidence.** Where the evidence supports
  less progress than claimed, the reported figure must be the evidenced figure, and the difference must
  be recorded and reported to the **control account owner**.
- **PCI-PCL-LAW-06.02-PR-04 — No negative or reversing progress without explanation.** Where reported
  progress falls between periods, the cause must be stated and the earlier over-report identified — a
  silent correction of prior over-claim is a breach of `PCI-PCL-LAW-04.03` as well as of this law.

**6. Prohibited actions.** Reporting progress supported only by the assertion of the person performing
or supervising the work; reporting progress against work not yet started; claiming a milestone whose
completion criteria are unmet; carrying forward the previous period's percentage; earning value for
materials delivered but not installed where the method does not permit it; adjusting progress to
achieve a target index or a payment position.

**7. Required evidence.** The progress record per work package with its evidencing record reference;
the verification record showing the population or sample, the basis of selection, the method and the
exceptions; the reconciliation from verified progress to reported earned value; the approver's identity
and date.

**8. Responsible role.** The **control account owner** for the progress claimed in their control
account; the **cost engineer** for its conversion into earned value; the **project controls lead** for
the reported position; the **planner** for the corresponding schedule status.

**9. Approval authority.** The **project controls lead** approves the earned value position for
reporting. Where a claim is contested between the claimant and the verifier, the **decision owner** for
the cost position decides and records the decision and its basis.

**10. Independence requirement.** Verification under PR-02 must be performed by a person **independent**
of the person claiming the progress. Where the work is performed by a contracting party, the verifier
must not be that party — a supplier's own progress return is a claim, never a verification of itself.

**11. Materiality or threshold.** Evidence is required for every claim, without threshold, because a
claim's value is often small while its effect on an index is not. The **sample** basis under PR-02 is
set by the adopting organisation's governance and recorded; where none is set, the professional records
the basis used, which must include all claims above the materiality rule and a stated random selection
below it. *Scaling:* on a USD 2 million refurbishment every claim can be physically verified in one
site visit and the population is the sample; on a USD 5 billion programme verification is layered —
package-level verification by the delivery team, sample re-verification by controls, and a stated
independent audit cycle. The rule that a claim must name its evidence is identical in both.

**12. Exception and waiver.** No exception is permitted to element 1. Where verification cannot be
performed before the cut-off — access denied, weather, remote location — the progress must be reported
as unverified, identified as such on the face of the report with the value affected, and verified in
the following period. Reporting it as verified is a breach, not an exception.

**13. Escalation trigger.** A material difference between claimed and evidenced progress; an instruction
to report progress that verification does not support; repeated unverified claims from the same source;
progress claimed for work the verifier found not started.

**14. AI application.** AI may compare claimed progress against installed quantities, delivery records,
timesheets, inspection records, photographs and site-scan data; flag claims inconsistent with resource
expenditure or with the schedule; and prioritise claims for physical verification.

**15. AI prohibition.** AI must not certify progress, approve a progress claim, replace physical or
documentary verification where the method requires it, or determine that a milestone's completion
criteria are met.

**16. AI verification.** Source tracing and sampling with a stated basis, and — where the AI output is
derived from imagery, sensor or model data — physical or documentary confirmation of a stated sample:
the professional must confirm the AI-assessed progress for the highest-value claims and for a random
selection of the remainder against the underlying record or the physical work, must record the sample
and the error rate, and must not report an AI-derived progress figure that no human has confirmed
against something outside the tool.

**17. External reference.**

- **SAE International (ANSI-accredited) — ANSI/EIA-748 *Earned Value Management Systems*.** Cited for
  the existence of recognised expectations that performance measurement rest on the work actually
  accomplished. Edition and guideline count deliberately not asserted. Nature: national standard,
  classified as Manual §6 **category 11, national standard**. Checked 2026-08-03
  (EXT-130 / EXT-090). Binding only where a contract or procurement regime imports it.
- **ISO — ISO 21508 *Earned value management in project and programme management*.** Cited for the
  international treatment of performance measurement. Edition: 2018 per register; second edition in
  development; no clause asserted. Nature: Manual §6 category 3, international voluntary standard.
  Checked 2026-08-03 (EXT-029). Voluntary unless adopted.

**18. Jurisdictional caution.** Where progress determines an entitlement to payment, the contract's
measurement, certification and payment provisions and any applicable construction-payment legislation
govern that entitlement. Certification under a contract is a contractual act, not a controls act, and
requires the certifier appointed under the contract.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) governs; this law adds the
definition of what counts as objective evidence of progress and the prohibition on reporting beyond it.
See also `PCI-PCL-LAW-06.01`, `PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-10.03`, `PCI-PCL-LAW-07.03`,
`PCI-PCL-LAW-04.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 6 · KA 6.1 EVM fundamentals. Also Domain 10
· KA 10.4 Progress measurement and schedule control; Domain 7 · KA 7.4 Invoicing and applications for
payment.

**21. Compliance test.** Compliance is demonstrated when, for a sample of progress claims selected on a
stated basis, the reviewer can name and retrieve for each the dated record produced by or verifiable
against a source other than the claimant, and that record supports the quantum claimed; and when the
verification record states the population or sample, the selection basis, the method and the exceptions
found. A claim for which the reviewer can retrieve no such record is a failure of this test. Two reviewers given
the same sample and the same evidence set reach the same exception list — which is the whole point of
defining objective evidence of progress rather than leaving "supported" undefined.

**22. Breach indicators.** Progress percentages ending in 0 or 5 across a whole control account;
progress that advances at a constant rate irrespective of events; claims that reach 90 per cent and
stop; earned value that tracks expenditure exactly; verification records with no exceptions in any
period; milestone claims with no completion criteria recorded; progress falling in a later period with
no explanation.

**23. Consequence within PCI authority.** Correction required and the earned value position withheld or
restated; additional review; escalation; failure of the associated examination competency; ethics
review; certification investigation, suspension or withdrawal — each subject to due process and a right
of appeal.

**24. Examination application.** Evidence selection: from five records, the candidate identifies which
constitute objective evidence of progress for a given claim and which do not. Ethical dilemma: a
supervisor's request to "round it up to 80 so the application clears".

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-06-01 *Earned Value Integrity* and
PCL-LAW-10-03 *Progress Measurement*; both identifiers are retired and are not reused.

---

### PCI LAW PCI-PCL-LAW-06.03 — Coherence of the Three Earned Value Data Points

**1. Normative requirement.** A credential holder must ensure that planned value, earned value and
actual cost are measured over the same scope, the same period and the same **cut-off** before any index
or variance derived from them is reported.

**2. Purpose.** Controls the arithmetic that is right and the answer that is wrong. A cost performance
index computed from earned value that excludes a scope which actual cost includes, or from an actual
cost that omits accruals the earned value assumes, is not a performance measure — it is a mismatch
expressed to two decimal places, and it is trusted precisely because it looks like a calculation.

**3. Scope.** All candidates and credential holders who compute, review, approve or give assurance over
earned value indices, variances, earned schedule measures or any derived performance metric, on any
project applying earned value.

**4. Defined terms.** *performance measurement baseline*, *cut-off*, *accrual*, *commitment*,
*material*, *reproducible*, *evidence*, *source record*.

**5. Required actions.** The professional must confirm the three data points are commensurable, and must
record that confirmation, before publishing any index.

- **PCI-PCL-LAW-06.03-PR-01 — Actual cost completeness for the period.** The actual cost used must
  include the **accrual** for work performed and not invoiced, must exclude amounts relating to work
  not yet performed, and must be reconciled to the cost position issued under `PCI-PCL-LAW-05.01`.
- **PCI-PCL-LAW-06.03-PR-02 — Planned value from the approved baseline only.** The planned value used
  must be taken from the current approved baseline version at the correct time phase, and the version
  used must be recorded with the calculation.
- **PCI-PCL-LAW-06.03-PR-03 — Index verification before publication.** Each published index and variance
  must be recomputed from its three inputs and agreed, and the inputs, the formula applied and the
  result must be retained so the figure is **reproducible**.
- **PCI-PCL-LAW-06.03-PR-04 — Scope coverage stated.** Where any part of the project is excluded from
  earned value measurement — a package not yet baselined, a level-of-effort element, a contract
  measured differently — the exclusion and its value must be stated with the index, so that the index's
  coverage is visible.
- **PCI-PCL-LAW-06.03-PR-05 — Cost and schedule statused to the same date.** The schedule status used
  for schedule performance measurement must carry the same status date as the cost cut-off, or the
  difference must be stated and its effect quantified.

**6. Prohibited actions.** Publishing an index whose inputs come from different cut-offs without saying
so; excluding accruals from actual cost while including their work in earned value; using a superseded
baseline for planned value; presenting a project-level index that covers only part of the project;
reporting an index without retaining its inputs; adjusting an input to produce an intended index.

**7. Required evidence.** The three inputs per control account with their sources and cut-offs; the
baseline version reference; the recomputation record; the statement of excluded scope and its value;
the reconciliation of actual cost to the issued cost position.

**8. Responsible role.** The **cost engineer** for the computation; the **project controls lead** for
the published indices; the **planner** for the schedule status used.

**9. Approval authority.** The **project controls lead** approves the published performance indices.

**10. Independence requirement.** The recomputation under PR-03 must be performable by a **competent
reviewer** from the retained inputs. Where indices are used in an incentive or contractual mechanism,
the recomputation must be performed by a person **independent** of the party whose reward they affect.

**11. Materiality or threshold.** Coherence is required absolutely — there is no threshold at which
mismatched inputs are acceptable, because the mismatch is not an error of size but an error of kind.
The **materiality rule** governs only whether a stated difference in cut-off dates under PR-05 must be
quantified in the report or merely noted. *Scaling:* on a USD 2 million refurbishment coherence is
demonstrated in one schedule of three columns; on a USD 5 billion programme it is demonstrated per
control account with roll-up, and the professional must record the level at which coherence was tested.

**12. Exception and waiver.** Where a source system's cut-off cannot be aligned, the index may be
published with the difference and its quantified effect stated on the face of the report, approved by
the **project controls lead**. No exception is permitted to PR-01 or PR-02.

**13. Escalation trigger.** An index published from mismatched inputs; discovery that actual cost used
in performance measurement differs materially from the reconciled cost position; use of a superseded
baseline in a published index.

**14. AI application.** AI may assemble the three data points from their systems, detect cut-off and
scope mismatches, recompute indices, and produce coverage statements.

**15. AI prohibition.** AI must not publish an index, decide that a mismatch is immaterial, substitute
an estimate for a missing input without that substitution being visible, or approve a performance
report.

**16. AI verification.** Independent recomputation: the professional must recompute at least the
project-level indices and the three largest control accounts' indices from the retained inputs without
using the tool that produced them, and must agree the results; any difference must be resolved before
publication rather than explained afterwards.

**17. External reference.**

- **SAE International (ANSI-accredited) — ANSI/EIA-748 *Earned Value Management Systems*.** Cited for
  the existence of recognised expectations that performance data be internally consistent. Edition and
  guideline count deliberately not asserted. Nature: **national standard** — Manual §6 category 11. Checked 2026-08-03 (EXT-130 / EXT-090). Binding only
  where imported by contract or procurement regime.
- **ISO — ISO 21508 *Earned value management in project and programme management*.** Cited for the
  international treatment of earned value data. Edition 2018 per register; no clause asserted. Nature:
  Manual §6 category 3, international voluntary standard. Checked 2026-08-03 (EXT-029). Voluntary
  unless adopted.

**18. Jurisdictional caution.** Where indices feed a contractual incentive, a payment mechanism or a
regulated report, the contract or the applicable regulation governs how they must be computed and
verified, and those requirements may exceed this law.

**19. Related PCI Laws.** `PCI-FND-LAW-06` (source and version integrity) governs; this law adds the
commensurability obligation specific to earned value arithmetic. See also `PCI-PCL-LAW-06.01`,
`PCI-PCL-LAW-06.02`, `PCI-PCL-LAW-06.04`, `PCI-PCL-LAW-05.01`, `PCI-PCL-LAW-03.02`,
`PCI-PCL-LAW-04.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 6 · KA 6.2 Variances and performance
indices; KA 6.4 Integrating cost & schedule; limitations; earned schedule. Also Domain 4 · KA 4.2
Variance analysis.

**21. Compliance test.** Compliance is demonstrated when a reviewer can take the three retained inputs
for any published index, recompute the index, and obtain the published figure; when the planned value
cites a baseline version that exists in the retained set; when the actual cost agrees to the reconciled
cost position for the same cut-off; and when the report states what scope the index covers and what it
excludes. A published index that cannot be recomputed from retained inputs is a failure of this test. Two
reviewers recomputing from the same retained inputs obtain the same figure — if they cannot, the law
has been breached whatever the report says.

**22. Breach indicators.** Indices that move smoothly while their inputs move erratically; actual cost
in the index differing from the reported cost position; a cost performance index of exactly 1.00 across
several control accounts; planned value that does not sum to the baseline; indices reported without
retained inputs; schedule status dates differing from the cost cut-off with no note.

**23. Consequence within PCI authority.** Correction required and the indices withheld or restated;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of
appeal.

**24. Examination application.** Calculation review: given three inputs with a deliberate cut-off
mismatch, the candidate identifies the mismatch, quantifies its effect on the cost performance index and
states the correct treatment.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · New law — the eighteen-field set assumed data coherence rather than
requiring it.

---

### PCI LAW PCI-PCL-LAW-06.04 — Selection and Disclosure of the Estimate-at-Completion Method

**1. Normative requirement.** A credential holder must disclose, with every earned-value-derived
estimate at completion, the method used to derive it and the reason that method suits the remaining
work.

**2. Purpose.** Controls method-shopping. The earned value family offers several formulae that produce
materially different answers from the same data; choosing among them without stating why, or presenting
one as *the* estimate at completion, converts a modelling choice into an apparent fact. The failure is
not using a formula — it is using it invisibly.

**3. Scope.** All candidates and credential holders who derive, review, approve or give assurance over
an estimate at completion, an independent estimate at completion, a to-complete performance index or
any other earned-value-derived forecast measure, on any project applying earned value.

**4. Defined terms.** *evidence*, *material*, *approved*, *decision owner*, *reproducible*, *trend*,
*escalation threshold*, *competent reviewer*.

**5. Required actions.** The professional must state the method, justify it against the remaining work,
and reconcile it to the bottom-up forecast.

- **PCI-PCL-LAW-06.04-PR-01 — Method disclosure and range.** Each earned-value-derived estimate at
  completion must state the formula applied, its inputs, and — where more than one recognised formula is
  applicable — the range the alternatives produce, so that the decision owner sees the spread rather
  than one point.
- **PCI-PCL-LAW-06.04-PR-02 — To-complete performance index interpreted, not merely reported.** Where a
  to-complete performance index is reported, it must be accompanied by a statement of what performance
  level it implies for the remaining work, and by a comparison with the performance achieved to date;
  a to-complete index materially above achieved performance must be identified as a target that the
  project has not yet demonstrated it can meet.
- **PCI-PCL-LAW-06.04-PR-03 — Reconciliation to the bottom-up forecast.** The earned-value-derived
  estimate at completion must be reconciled to the bottom-up estimate at completion prepared under
  `PCI-PCL-LAW-03.04`, and every material difference explained; where the two diverge materially and
  persistently, the divergence must be reported to the **decision owner** rather than resolved by
  choosing the preferred figure.
- **PCI-PCL-LAW-06.04-PR-04 — Method consistency between periods.** The method must not be changed
  between periods without stating the change, its reason and the effect on the reported estimate at
  completion.

**6. Prohibited actions.** Presenting an earned-value-derived estimate at completion without its method;
selecting the method that produces the preferred number; reporting a to-complete performance index
without interpreting it; changing method silently between periods; suppressing the bottom-up forecast
where it exceeds the formula-derived one; presenting a range as a single number.

**7. Required evidence.** The method statement with inputs and formula; the alternative-formula range
where applicable; the to-complete index interpretation; the reconciliation to the bottom-up forecast
with explanations; the record of any method change with its effect.

**8. Responsible role.** The **cost engineer** for the derivation; the **project controls lead** for the
disclosure as issued; the **decision owner** for the forecast relied on.

**9. Approval authority.** The **decision owner** for the cost position approves which estimate at
completion is adopted for decision-making, and that adoption must be recorded with its reason.

**10. Independence requirement.** Not required for derivation. Where the earned-value-derived and
bottom-up estimates diverge materially, the reconciliation under PR-03 must be reviewed by a person
**independent** of the preparer of both.

**11. Materiality or threshold.** Disclosure is required for every earned-value-derived estimate at
completion, without threshold. The **materiality rule** decides which divergence between methods must be
escalated rather than merely explained, and what counts as a to-complete index "materially above"
achieved performance under PR-02 — a judgement the professional must record, since a to-complete index
slightly above achieved performance may be routine on a short refurbishment and implausible on a
multi-year programme where achieved performance is well established over many periods. *Scaling:* the
smaller the completed proportion of work, the less reliable any earned-value-derived estimate is; on a
USD 2 million refurbishment early in delivery the professional must state that limitation, and on a USD
5 billion programme the same limitation applies control account by control account rather than in
total.

**12. Exception and waiver.** No exception is permitted to disclosure of the method. Where the data does
not support any earned-value-derived estimate — insufficient work complete, or performance not yet
stable — the professional must state that and rely on the bottom-up forecast, rather than publish a
figure with a caveat nobody reads.

**13. Escalation trigger.** A material and persistent divergence between the earned-value-derived and
bottom-up estimates; a to-complete performance index implying performance the project has never
achieved; a method changed in the period in which its result would otherwise have deteriorated.

**14. AI application.** AI may compute the full family of earned-value-derived estimates, produce the
range, compare methods against outturn on comparable completed projects, and draft the interpretation
of the to-complete index.

**15. AI prohibition.** AI must not select the estimate at completion adopted for decision-making,
decide which method is appropriate, suppress an alternative result, or approve the forecast.

**16. AI verification.** Independent recomputation plus sensitivity analysis: the professional must
recompute the adopted formula's result from its inputs without the tool, must confirm the reported range
by recomputing at least the highest and lowest alternatives, and must record the sensitivity of the
adopted figure to its principal input.

**17. External reference.**

- **Project Management Institute — *The Standard for Earned Value Management*.** Cited for the
  existence of a recognised family of estimate-at-completion methods. Edition not established — not
  independently verified. Nature: Manual §6 category 5, professional framework; not regulatory
  authority. Register EXT-061. Persuasive only.
- **AACE International — *Total Cost Management Framework*.** Cited for forecasting within the
  cost-control cycle. Edition not asserted — unverified. Nature: Manual §6 category 5, professional
  framework. Register EXT-064. Persuasive only, on adoption.

**18. Jurisdictional caution.** Where an estimate at completion is used in statutory reporting — for
example in assessing progress towards satisfying a performance obligation or the existence of an onerous
contract — the applicable accounting framework governs that use and requires qualified accounting
advice.

**19. Related PCI Laws.** `PCI-FND-LAW-05` (transparent assumptions) governs; this law adds the specific
disclosure of method, range and to-complete interpretation. See also `PCI-PCL-LAW-03.04`,
`PCI-PCL-LAW-03.05`, `PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-13.02`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 6 · KA 6.3 Forecasting with EVM: the EAC
family. Also Domain 3 · KA 3.4 Forecasting; Domain 4 · KA 4.2 Variance analysis.

**21. Compliance test.** Compliance is demonstrated when the issued forecast states the formula applied
and its inputs; when a reviewer recomputing from those inputs obtains the published figure; when the
alternative-formula range is shown or its absence explained; when any reported to-complete performance
index is accompanied by a comparison with achieved performance; and when a reconciliation to the
bottom-up estimate exists with material differences explained. An estimate at completion whose method is
not stated is a failure of this test on its face. Two reviewers recomputing from the stated inputs obtain the same
figure.

**22. Breach indicators.** The method changing in the period when the previous method's result
deteriorated; a single-point estimate at completion presented every period with no range; a to-complete
performance index reported without comment for several periods while achieved performance is far below
it; the bottom-up forecast appearing only when it is lower; inputs not retained.

**23. Consequence within PCI authority.** Correction required and the forecast withheld or reissued;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of
appeal.

**24. Examination application.** Calculation review: the candidate computes the estimate at completion
under two recognised methods, states the range, interprets the to-complete performance index and
identifies which figure the evidence supports. Scenario judgement: a method changed without disclosure.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · New law — the eighteen-field set required forecast honesty but did
not address method selection or disclosure.

---
## Domain 7 — Contracts, Commercial Management, BoQ, Invoicing & Revenue

### PCI LAW PCI-PCL-LAW-07.01 — Contract Source Verification

**1. Normative requirement.** A credential holder must verify every contractual term they rely on
against the executed contract and its executed amendments.

**2. Purpose.** Controls reliance on the wrong document. Commercial positions are routinely built on a
tender version, a draft that was never signed, a summary someone prepared, a term sheet, a
predecessor contract with the same counterparty, or — increasingly — an AI-generated extract. Each is
plausible and each has been wrong. A commercial analysis built on an unverified term is not
conservative or aggressive; it is unrelated to the agreement the parties actually made.

**3. Scope.** All candidates and credential holders who prepare, review, approve or give assurance
over any output that depends on a contractual term — payment terms, rates, milestones, liquidated
damages, notice periods, variation mechanisms, retention, indexation, caps, and the scope the contract
covers — on any project and under any contract form.

**4. Defined terms.** *source record*, *current*, *verified*, *evidence*, *material*, *competent
reviewer*, *escalation threshold*, *approved*.

**5. Required actions.** The professional must go to the executed document, record which document they
went to, and stay inside their competence when interpreting it.

- **PCI-PCL-LAW-07.01-PR-01 — Contract register with executed status.** A register of the project's
  contracts and amendments must record for each the parties, the date of execution, the document
  reference of the executed version, its location, and whether any amendment remains unexecuted.
- **PCI-PCL-LAW-07.01-PR-02 — Citation of the relied-on provision.** Any commercial analysis, claim
  assessment, payment assessment or report that turns on a contractual term must cite the document and
  the provision relied on, so that a reviewer can read the same words.
- **PCI-PCL-LAW-07.01-PR-03 — Boundary of interpretation.** Where the meaning or effect of a provision
  is uncertain, disputed, or determines legal entitlement, the professional must refer the question to
  the person authorised to obtain legal advice, must record the referral, and must not state a legal
  conclusion as though it were settled.
- **PCI-PCL-LAW-07.01-PR-04 — Unexecuted documents identified.** Where work proceeds under a letter of
  intent, an unexecuted amendment or an instruction outside the executed contract, that fact and the
  value exposed must be recorded and reported in the period in which it arises.

**6. Prohibited actions.** Relying on a draft, a tender document, a summary or an extract as the source
of a term; citing a provision the professional has not read in the executed document; stating a legal
conclusion on entitlement, liability or the effect of a notice; treating an unexecuted amendment as
binding; carrying a term forward from a previous contract with the same counterparty.

**7. Required evidence.** The contract register; the executed contract and amendments as retained; the
citation record for each relied-on provision; the referral record where interpretation was referred;
the register of work proceeding under unexecuted documents with its value.

**8. Responsible role.** The **commercial lead** for the contractual position; the **project controls
lead** for any controls output that depends on it; the person authorised by the adopting organisation
to obtain legal advice for questions referred under PR-03.

**9. Approval authority.** The **commercial lead** approves the contractual basis used in a controls
output. Only the person authorised under the adopting organisation's governance may commit the
organisation to a contractual position, and this law confers no such authority.

**10. Independence requirement.** Not required for verification against the executed document, which is
a matter of fact rather than judgement. Where a contractual position is material to a dispute, its
verification must be re-performed by a person **independent** of the party whose entitlement it
supports.

**11. Materiality or threshold.** Every relied-on term must be verified against the executed document,
without threshold — the cost of opening the contract is trivial and the cost of being wrong is not.
The **materiality rule** decides only which unexecuted-document exposures must be escalated rather than
recorded and reported. *Scaling:* on a USD 2 million refurbishment one contract and two amendments make
the register a single page; on a USD 5 billion programme with hundreds of contracts the register is a
system, and the professional must record how the executed version is identified within it. The
obligation to read the executed words before relying on them does not scale away.

**12. Exception and waiver.** No exception is permitted to element 1. Where the executed contract cannot
be located, the professional must record that fact, must state the assumption used in its place on the
face of the output, and must escalate — an output built on an assumed term that is not labelled as such
is a breach.

**13. Escalation trigger.** A material term that cannot be verified against an executed document; work
proceeding under an unexecuted document beyond the period stated in the adopting organisation's
governance; a request to state a legal conclusion the professional is not qualified to give; discovery
that a reported position rests on a superseded contract version.

**14. AI application.** AI may locate provisions across a contract set, extract candidate terms,
compare an executed version against a draft to identify differences, summarise obligations, and build
the contract register from document metadata.

**15. AI prohibition.** AI must not be the source of a contractual term relied on without human
confirmation against the executed document; must not determine entitlement; must not advise on the
meaning or legal effect of a provision; and must not be cited as the authority for a commercial
position.

**16. AI verification.** Clause-to-summary comparison: for every AI-extracted or AI-summarised term the
professional relies on, they must open the executed contract at that provision and read the words
themselves, and must record the document reference and provision identifier they read. An AI summary
that is consistent with the professional's expectation is the highest-risk case, because it is the one
least likely to be opened.

**17. External reference.**

- **FIDIC — FIDIC suite of conditions of contract.** Cited generically for the existence of standard
  forms whose provisions vary between books and between editions. **No clause number, book or edition
  asserted** — clause numbering has moved between editions, which is itself part of the reason for this
  law. Nature: Manual §6 category 4, contract framework. Checked 2026-08-03 (EXT-050). Binds only the
  parties to a contract that adopts it, on its own terms as amended by the parties.
- **NEC — NEC4 suite of contracts.** Cited generically for the existence of a standard form with
  time-bound notification mechanisms. No clause number asserted. Nature: Manual §6 category 4, contract
  framework. Checked 2026-08-03 (EXT-051). Binds only adopting parties.

**18. Jurisdictional caution.** The meaning and effect of a contract term, the validity of a notice, the
existence of an entitlement and the consequences of a breach are questions of the governing law and of
the contract as executed and amended. They require qualified legal advice in the governing
jurisdiction. Nothing produced under this law is legal advice, and a controls professional stating a
legal conclusion is outside their competence — see `PCI-FND-LAW-10`.

**19. Related PCI Laws.** `PCI-FND-LAW-06` (source and version integrity) and `PCI-FND-LAW-10`
(competence and limitation) govern. This law adds the executed-document rule and the interpretation
boundary specific to commercial project controls. See also `PCI-PCL-LAW-07.02`, `PCI-PCL-LAW-07.03`,
`PCI-PCL-LAW-05.02`, `PCI-PCL-LAW-13.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 7 · KA 7.1 Types of contract; KA 7.2 Contract
management. Also Domain 5 · KA 5.4 Change control and cost impact.

**21. Compliance test.** Compliance is demonstrated when, for a sample of commercial outputs selected on
a stated basis, each relied-on term cites a document and provision; the cited document is in the
contract register as executed; and reading that provision supports the use made of it. An output citing
no document, or citing a document the register does not record as executed, is a failure of this test. Two
reviewers reading the same cited provisions reach the same conclusion about whether the output's use of
the term is supported — and where they do not, the disagreement is itself the signal that PR-03 should
have been applied.

**22. Breach indicators.** Commercial analyses with no contract citations; payment terms in the cost
system that differ from the executed contract; a contract register with unexecuted amendments not
flagged; terms quoted identically across contracts with different counterparties; legal conclusions
stated by controls staff; letters of intent that have run for months with no record.

**23. Consequence within PCI authority.** Correction required and the output withheld until verified;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of
appeal.

**24. Examination application.** Evidence selection: given a tender document, a draft, an executed
contract and an AI-generated summary, the candidate identifies which may be relied on. Ethical dilemma:
pressure to state that a claim "is clearly entitled" without legal advice.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-07-01 *Commercial Traceability*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-07.02 — Traceability of Variations and Claims

**1. Normative requirement.** A credential holder must maintain, for each variation and each claim, a
traceable record from the originating event to the current commercial position.

**2. Purpose.** Controls the commercial position that nobody can reconstruct. Variations and claims
accumulate over years, through people who leave, in registers that diverge from the cost system and
from the correspondence. When the position is finally tested — at final account, in adjudication, in
audit — what matters is whether the chain from event to entitlement to value to recovery can be walked.
Where it cannot, a valid entitlement is lost and an invalid one is paid.

**3. Scope.** All candidates and credential holders who record, value, review, approve, report or give
assurance over variations, compensation events, claims, extensions of time, disruption claims,
counterclaims or contra charges, on any project and in either direction — as claimant or as recipient.

**4. Defined terms.** *change*, *approved*, *source record*, *evidence*, *material*, *current*,
*escalation threshold*, *cut-off*.

**5. Required actions.** The professional must keep the chain complete and the status honest.

- **PCI-PCL-LAW-07.02-PR-01 — Event-to-position chain.** Each variation and claim must carry a record
  linking the originating event, the instructing or notifying document, the assessment, the submission,
  the response, the agreement or determination, and the value recorded in the cost position.
- **PCI-PCL-LAW-07.02-PR-02 — Status stated to a defined vocabulary.** Each entry must carry a status
  from a defined status list — for example notified, submitted, under assessment, agreed in principle,
  agreed in value, rejected, in dispute, settled — each status defined in writing, and the date it was
  reached; "in progress" and "ongoing" are not statuses.
- **PCI-PCL-LAW-07.02-PR-03 — Notice-date integrity.** The date on which an event occurred, the date it
  became known, and the date any notice was given or received must each be recorded from the source
  document as they actually are, and must not be adjusted, back-dated or estimated to fit a
  contractual period.
- **PCI-PCL-LAW-07.02-PR-04 — Valuation basis and reasonableness stated.** The value recorded against
  each variation or claim must state the basis on which it was assessed and whether it represents the
  submitted value, the assessed value or the agreed value; a submitted value must never be reported as
  though it were agreed.
- **PCI-PCL-LAW-07.02-PR-05 — Reconciliation to the cost position and the forecast.** The register's
  totals by status must reconcile at each cut-off to the amounts included in the cost position and the
  forecast, with any difference explained.

**6. Prohibited actions.** Reporting a submitted claim value as an agreed value; recording a notice date
other than the actual date; assessing entitlement without reference to the executed contract;
maintaining a claims register that does not reconcile to the cost position; closing an entry without
recording the outcome; presenting an aggregate claim position without stating the mix of statuses
behind it.

**7. Required evidence.** The variation and claim register with the PR-01 chain per entry; the defined
status list; the source documents establishing event, knowledge and notice dates; the valuation basis
per entry; the reconciliation to the cost position and the forecast.

**8. Responsible role.** The **commercial lead** for the commercial position and the register; the
**project controls lead** for its reconciliation to the cost position and the forecast; the **planner**
for the schedule evidence supporting time-related entries.

**9. Approval authority.** The **commercial lead** approves the recorded commercial position. Agreement
of a value with a counterparty is a contractual act requiring the authority recorded in the adopting
organisation's delegation, which this law does not confer.

**10. Independence requirement.** Not required for maintaining the register. Where a claim position is
**material** to the reported result or to a dispute, its valuation must be reviewed by a person
**independent** of the person who prepared it and of any incentive that turns on its value.

**11. Materiality or threshold.** Traceability is required for every entry, of any value, because the
chain cannot be reconstructed later for entries that were never linked. The **materiality rule** decides
which valuations require independent review under element 10 and which reach the escalation threshold.
*Scaling:* on a USD 2 million refurbishment the register may hold a dozen entries maintained in one
file; on a USD 5 billion programme entries run to thousands across many contracts, and the professional
must record how the chain is held and how completeness is assured — sampling is acceptable for testing
the chain, never for building it.

**12. Exception and waiver.** No exception is permitted to PR-03. Where an event date cannot be
established from a document, the record must state that it is estimated, state the basis of the
estimate and identify who made it — an estimated date recorded as a fact is a breach.

**13. Escalation trigger.** Discovery that a notice date was recorded other than as the source document
shows; a material claim whose chain cannot be reconstructed; a claims position included in the reported
result at a value the counterparty has not agreed and which is not identified as unagreed; an approaching
contractual time bar on an unsubmitted entitlement.

**14. AI application.** AI may extract event, knowledge and notice dates from correspondence; assemble
candidate chains across documents; identify entries approaching a time bar; check register status
consistency; and reconcile register totals to the cost position.

**15. AI prohibition.** AI must not determine entitlement, decide a claim's status, set an agreed value,
assert that a notice was valid, or generate a claim narrative presented as the professional's assessment.

**16. AI verification.** Source tracing plus reconciliation: for every AI-extracted date the professional
must open the source document and confirm the date; for every AI-assembled chain that is material the
professional must confirm each link exists; and the professional must reconcile the register to the cost
position independently of the tool. Dates are the highest-consequence extraction in commercial work,
which is why PR-03 admits no sampling for material entries.

**17. External reference.**

- **FIDIC — FIDIC suite of conditions of contract.** Cited generically for the existence of time-bound
  notice and claim procedures whose operation depends on dates. No clause number, book or edition
  asserted. Nature: Manual §6 category 4, contract framework. Checked 2026-08-03 (EXT-050). Binds only
  adopting parties.
- **NEC — NEC4 suite of contracts.** Cited generically for the compensation-event notification
  mechanism. No clause number asserted. Nature: Manual §6 category 4, contract framework. Checked
  2026-08-03 (EXT-051). Binds only adopting parties.
- **AACE International — Recommended Practice 29R-03 *Forensic Schedule Analysis*.** Cited for the
  existence of recognised delay-analysis methods relevant to time-related claims. Not independently
  verified; no method prescribed here. Nature: Manual §6 category 5, professional framework. Register
  EXT-067. Applicability: persuasive only; the acceptability of any method differs between forums and
  jurisdictions.

**18. Jurisdictional caution.** Time bars, notice validity, the admissibility of a delay-analysis method
and the existence of an entitlement are questions of the governing law and the executed contract.
Missing a contractual time bar can extinguish a valid claim. These are matters for qualified counsel and
the contract administrator, and this law neither preserves nor defeats any entitlement.

**19. Related PCI Laws.** `PCI-FND-LAW-07` (data lineage) governs; this law adds the commercial chain,
the status vocabulary and the notice-date rule. See also `PCI-PCL-LAW-07.01`, `PCI-PCL-LAW-07.03`,
`PCI-PCL-LAW-05.02`, `PCI-PCL-LAW-10.02`, `PCI-PCL-LAW-11.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 7 · KA 7.2 Contract management; KA 7.3 Bills
of Quantities. Also Domain 5 · KA 5.4 Change control and cost impact; Domain 10 · KA 10.4 Progress
measurement and schedule control.

**21. Compliance test.** Compliance is demonstrated when, for a sample of entries selected on a stated
basis to include the highest values and the oldest open entries, the reviewer can walk the chain from
the originating document to the value in the cost position without asking the preparer; when every entry
carries a status from the defined list with its date; when every recorded event and notice date matches
the source document; and when the register's totals by status reconcile to the cost position and the
forecast. A recorded date that differs from the source document is a failure of this test. Two reviewers walking
the same chains reach the same conclusion about which are complete.

**22. Breach indicators.** Claim values in the forecast that exceed the assessed values in the register;
statuses that have not changed for many periods; notice dates that fall conveniently inside contractual
windows; entries closed with no outcome recorded; registers maintained separately by the commercial and
controls functions with different totals; aggregate claim recoveries reported without a status mix.

**23. Consequence within PCI authority.** Correction required and the commercial position restated;
output withheld; additional review; escalation; failure of the associated examination competency; ethics
review; certification investigation, suspension or withdrawal — each subject to due process and a right
of appeal.

**24. Examination application.** Scenario judgement: a register in which a submitted value has been
reported as agreed, and a notice date that does not match the correspondence. Escalation decision: an
entitlement approaching a time bar that nobody has submitted.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-07-01 *Commercial Traceability*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-07.03 — Support and Reconciliation of Applications for Payment

**1. Normative requirement.** A credential holder must not submit, certify or support an application for
payment for work that is not evidenced as performed or delivered at the application date.

**2. Purpose.** Controls the point where controls data becomes money. An application for payment
supported by a percentage nobody verified moves cash on the strength of an assertion; over-claiming
creates an exposure that unwinds at final account, and under-claiming starves the project of the cash it
needs. The failure is also the one most likely to be characterised, after the fact, as something other
than an error.

**3. Scope.** All candidates and credential holders who prepare, assess, certify, review, approve or
give assurance over applications for payment, valuations, interim certificates, subcontractor payment
assessments or the reconciliation between billing and revenue records, on any project and in either
direction.

**4. Defined terms.** *objective evidence of progress*, *evidence*, *cut-off*, *material*, *approved*,
*source record*, *commitment*, *escalation threshold*.

**5. Required actions.** The professional must tie every application line to evidenced performance and
must reconcile the billing position to the records that describe the same work.

- **PCI-PCL-LAW-07.03-PR-01 — Line-level support.** Each line of an application must be supported by
  the measurement, milestone certificate, delivery record, timesheet or other **objective evidence of
  progress** that establishes the quantity or the entitlement claimed, referenced from the application.
- **PCI-PCL-LAW-07.03-PR-02 — Reconciliation of billing to progress and to the cost position.** The
  cumulative applied and certified position must be reconciled at each cut-off to the reported physical
  progress and to the cost position, with every difference identified and explained.
- **PCI-PCL-LAW-07.03-PR-03 — Materials and unfixed goods identified.** Amounts claimed for materials on
  or off site, unfixed goods, advance payments and mobilisation must be identified separately from
  amounts for work performed, with the contractual basis stated.
- **PCI-PCL-LAW-07.03-PR-04 — Retention, set-off and contra charges recorded.** Retention held or
  released, set-offs, contra charges and liquidated damages applied must each be recorded with their
  basis and reconciled between the application, the certificate and the cost position.

**6. Prohibited actions.** Applying for payment for work not performed; front-loading rates so that
early activities recover more than their value; claiming materials as installed work; certifying
progress the certifier has not verified or caused to be verified; submitting an application that does
not reconcile to the reported progress position without explanation; applying a set-off with no recorded
basis.

**7. Required evidence.** The application with line-level references to supporting records; the
supporting records themselves; the reconciliation to physical progress and to the cost position; the
separate identification of materials, advances and retention; the certifier's assessment record; the
approver's identity and date.

**8. Responsible role.** The **commercial lead** for the application or the assessment; the **project
controls lead** for the reconciliation to progress and cost; the certifier appointed under the contract
for certification, which is a contractual role this law does not displace.

**9. Approval authority.** The authority named in the adopting organisation's delegation approves
submission of an application; certification is performed only by the person appointed under the contract.

**10. Independence requirement.** Assessment of a counterparty's application must be performed by a
person **independent** of that counterparty. Verification of progress supporting an application must
satisfy the independence requirement in `PCI-PCL-LAW-06.02` — the application is a claim, and a claim
cannot verify itself.

**11. Materiality or threshold.** Every line requires support, without threshold; the **materiality
rule** decides which unsupported lines must be removed before submission rather than corrected in the
next application, and which differences in the PR-02 reconciliation must be escalated. *Scaling:* on a
USD 2 million refurbishment support is a measurement sheet and a photograph set per line; on a USD 5
billion programme it is a measurement system with sampled re-measurement, and the professional must
record the sampling basis. In both, an application line that cannot name its support does not go in.

**12. Exception and waiver.** No exception is permitted to element 1. Where the contract permits payment
in advance of performance — an advance payment, a mobilisation payment, materials off site — that is not
an exception to this law but a contractual entitlement, and it must be identified under PR-03 with the
provision relied on cited under `PCI-PCL-LAW-07.01`.

**13. Escalation trigger.** An instruction to apply for or certify payment for work not evidenced as
performed; a material and unexplained difference between the cumulative certified position and reported
progress; discovery that a previous application over-claimed and has been paid.

**14. AI application.** AI may assemble applications from measurement and progress data; check
arithmetic and rate application; reconcile cumulative positions; identify lines lacking support; and
compare applications against contract rates and schedules of prices.

**15. AI prohibition.** AI must not certify payment, approve an application, determine entitlement to
payment, assess a set-off, or generate supporting narrative for a line that has no underlying record.

**16. AI verification.** Independent recomputation plus source tracing on a stated sample: the
professional must recompute the application total and the cumulative position without the tool; must
trace every line above the materiality rule, and a stated random selection below it, to its supporting
record; and must confirm rates against the executed contract's schedule of prices under
`PCI-PCL-LAW-07.01`.

**17. External reference.**

- **IFRS Foundation / IASB — IFRS 15 *Revenue from Contracts with Customers*.** Cited to mark the
  boundary this law respects: billing and payment are not revenue, and the reconciliation required by
  PR-02 must be capable of supporting the entity's revenue determination without purporting to make it.
  Edition: issued May 2014, in force; no clause asserted. Nature: Manual §6 category 2, authoritative
  financial-reporting standard. Checked 2026-08-03 (register EXT-001). Applicability: mandatory only for
  entities applying IFRS Accounting Standards in a jurisdiction that has adopted them; it creates no
  obligation through this law.
- **FIDIC — FIDIC suite of conditions of contract.** Cited generically for the existence of interim
  payment and certification mechanisms. No clause number, book or edition asserted. Nature: Manual §6
  category 4, contract framework. Checked 2026-08-03 (EXT-050). Binds only adopting parties.

**18. Jurisdictional caution.** Payment entitlement, certification, payment notices, withholding and
adjudication are governed by the contract and, in several jurisdictions, by construction-payment
legislation with strict statutory timescales whose breach has immediate financial consequences. Revenue
recognition is an accounting determination for the reporting entity. Both require qualified local advice
and neither is decided by this law.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) governs; this law adds the
line-level support and billing-to-progress reconciliation obligations specific to payment. See also
`PCI-PCL-LAW-06.02`, `PCI-PCL-LAW-07.01`, `PCI-PCL-LAW-07.02`, `PCI-PCL-LAW-05.01`,
`PCI-PCL-LAW-11.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 7 · KA 7.4 Invoicing and applications for
payment; KA 7.5 Revenue recognition in the commercial cycle. Also Domain 11 · KA 11.1 Order-to-Cash;
Domain 2 · KA 2.2 IFRS 15 Revenue from Contracts with Customers.

**21. Compliance test.** Compliance is demonstrated when, for a sample of application lines selected on
a stated basis, each names a supporting record that the reviewer can retrieve and that supports the
quantity or entitlement claimed; when the cumulative applied and certified position reconciles to
reported progress and to the cost position with differences explained; and when materials, advances,
retention and set-offs are separately identified with their contractual basis cited. A line whose
support cannot be retrieved is a failure of this test. Two reviewers testing the same sample reach the same
exception list.

**22. Breach indicators.** Applications that always equal the amount needed to meet a cash target;
cumulative certified value exceeding evidenced progress; early activities recovering disproportionate
value; materials claimed repeatedly without conversion to installed work; set-offs with no basis
recorded; a reconciliation between billing and progress that is prepared only when queried.

**23. Consequence within PCI authority.** Correction required and the application withdrawn or
corrected; output withheld; additional review; escalation; failure of the associated examination
competency; ethics review; certification investigation, suspension or withdrawal — each subject to due
process and a right of appeal.

**24. Examination application.** Calculation review: given a measurement record, a schedule of prices
and a draft application, the candidate identifies the over-claimed line and the misclassified materials
line. Ethical dilemma: an instruction to include an unsupported line "because it will be certified down
anyway".

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · New law — the eighteen-field set addressed commercial traceability
but contained no payment-application requirement.

---
## Domain 10 — Project Scheduling

### PCI LAW PCI-PCL-LAW-10.01 — Schedule Network Integrity

**1. Normative requirement.** A credential holder must not issue a schedule for planning, reporting or
decision-making that contains an **open end**.

**2. Purpose.** Controls the schedule that computes but does not model. An open end means an activity
whose movement is invisible to the rest of the network: it can slip without moving anything, or be
driven by nothing. Every downstream analysis — critical path, float, forecast completion, delay
assessment, resource profile — is then computed on a network that does not represent how the work
actually connects, and the resulting dates are arithmetic without meaning.

**3. Scope.** All candidates and credential holders who build, status, review, approve or give assurance
over a schedule used for planning, progress reporting, forecasting, resource planning, payment
assessment or delay analysis, on any project of any size using a network-based schedule.

**4. Defined terms.** *open end*, *approved*, *evidence*, *material*, *current*, *competent reviewer*,
*escalation threshold*, *performance measurement baseline*.

**5. Required actions.** The professional must build the network so that logic, constraints and
calendars each represent something real, and must record the tests that show it.

- **PCI-PCL-LAW-10.01-PR-01 — Logic completeness test before issue.** Before issue, the schedule must be
  tested for open ends, and every open end found must be closed by logic that represents a real
  dependency or explained in an exceptions record naming the activity and the reason.
- **PCI-PCL-LAW-10.01-PR-02 — Constraint register.** Every date constraint in the schedule must be
  recorded in a constraint register with the activity, the constraint type, the reason it exists, the
  authority for it and the date it will next be reviewed; constraints that remove float or override
  logic must be identified separately.
- **PCI-PCL-LAW-10.01-PR-03 — Calendar governance.** Every calendar in the schedule must be recorded
  with its working pattern, its non-working periods and the activities assigned to it; a calendar must
  not be altered to change a completion date in place of a logic or duration change, and any calendar
  change between issues must be recorded with its effect on the completion date.
- **PCI-PCL-LAW-10.01-PR-04 — Logic that represents real dependency.** Relationships must represent a
  genuine physical, contractual or resource dependency; logic added solely to satisfy a metric, to
  remove an open end without reflecting a real dependency, or to change a computed date is prohibited,
  and lags and leads must each carry a recorded reason.
- **PCI-PCL-LAW-10.01-PR-05 — Test record retained.** The output of the tests under PR-01 to PR-03 must
  be retained with the issued schedule, with the date it was run and the person who ran it.

**6. Prohibited actions.** Issuing a schedule with unexplained open ends; using a date constraint in
place of logic; changing a calendar to recover a date; adding a relationship that models no real
dependency; using negative lag to conceal an overlap that the logic does not support; issuing a
schedule with no record of the integrity tests run.

**7. Required evidence.** The issued schedule file; the open-end test output with exceptions; the
constraint register; the calendar register and change record; the record of lags and their reasons; the
test record with date and runner.

**8. Responsible role.** The **planner** for the schedule as built and issued; the **project controls
lead** for its integrity as reported; each **control account owner** for confirming that the logic
represents how their work will actually be executed.

**9. Approval authority.** The **project controls lead** approves the schedule for issue. The **baseline
approval authority** approves a schedule that becomes part of the **performance measurement baseline**.

**10. Independence requirement.** Not required for building the schedule. The integrity tests under PR-01
to PR-03 must be re-performable by a **competent reviewer** from the issued file; where the schedule
supports a claim, a delay analysis or a contractual entitlement, the tests must be re-performed by a
person **independent** of the party the entitlement favours.

**11. Materiality or threshold.** Open ends are prohibited without a value or count threshold, because a
single open end on a critical activity invalidates the analysis while a hundred on trivial activities may
not — a count-based tolerance would get this exactly backwards. Constraints and lags are permitted where
recorded and justified; the adopting organisation's governance may set a maximum proportion or require
review at a stated frequency, and where it does, that rule is recorded. *Scaling:* on a USD 2 million
refurbishment with 150 activities the test is a single query and the exceptions are read individually;
on a USD 5 billion programme with 100,000 activities across many contracts it is the same query run per
schedule with the exceptions triaged by whether the activity is on or near the longest path. Because the
test is a query rather than a manual pass, it costs the same at both scales — which is why this law
tolerates no exceptions on the ground of size.

**12. Exception and waiver.** An open end may be permitted where it genuinely represents the boundary of
the schedule's scope — an interface with a schedule held by another party — provided it is recorded in
the exceptions record with the interfacing schedule identified and the interface managed under a
recorded interface agreement. No exception is permitted to PR-02 or PR-05.

**13. Escalation trigger.** An open end on an activity on or near the longest path that cannot be closed;
a constraint applied without recorded authority; a calendar change that alters a reported completion date;
an instruction to change logic to produce a required date.

**14. AI application.** AI may run integrity tests, propose logic for open ends from activity
descriptions and historical networks, detect implausible durations, identify redundant or contradictory
logic, and draft the constraint and calendar registers.

**15. AI prohibition.** AI must not insert logic into an issued schedule without human confirmation that
the dependency is real; must not remove or alter a constraint; must not approve a schedule; and must not
close an open end by adding a relationship the planner has not accepted.

**16. AI verification.** Independent recomputation and named human judgement: for every AI-proposed
relationship the **planner** must record whether the dependency is real and why, and must re-run the
network and confirm the resulting dates; the professional must also re-run the integrity tests after any
AI-assisted edit, because closing an open end can create a new one.

**17. External reference.**

- **U.S. Government Accountability Office — *GAO Schedule Assessment Guide: Best Practices for Project
  Schedules*.** Cited for the existence of a public audit institution's published expectations on
  schedule quality. Edition: GAO-16-89G, issued 22 December 2015, per the register; no practice text
  reproduced. Nature: Manual §6 category 5, professional framework published by a public audit
  institution; not a regulation. Checked 2026-08-03 (register EXT-069). Applicability: persuasive; it
  binds only where a client or a procurement regime requires its use.
- **Defense Contract Management Agency (US) — *DCMA 14-Point Schedule Assessment*.** Cited for the
  existence of a widely used metric set that includes logic and constraint checks. Not independently
  verified; **no metric value or tolerance is asserted or adopted here** — this law's tests are its own.
  Nature: Manual §6 category 7, industry guidance. Register EXT-091. Applicability: no standard-setter's
  authority; used only where an organisation or a client adopts it.
- **Project Management Institute — *Practice Standard for Scheduling*.** Cited for the existence of a
  recognised treatment of schedule construction. Edition: third edition per the register; no clause
  asserted. Nature: Manual §6 category 5, professional framework; not regulatory authority. Checked
  2026-08-03 (EXT-062). Persuasive only.

**18. Jurisdictional caution.** Where a contract prescribes a scheduling specification — permitted
constraint types, maximum lag, required submission and acceptance procedures — that specification is a
contractual obligation that may exceed this law, and its interpretation belongs to the contract
administrator.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) governs; this law adds the network
integrity obligations, which no foundational law reaches. See also `PCI-PCL-LAW-10.02`,
`PCI-PCL-LAW-10.03`, `PCI-PCL-LAW-03.01`, `PCI-PCL-LAW-05.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 10 · KA 10.1 Schedule development; KA 10.2
Network analysis and the Critical Path Method. Also Domain 8 · KA 8.2 Planning.

**21. Compliance test.** Compliance is demonstrated when a query run against the issued schedule file
returns: zero activities without a predecessor other than the single start milestone; zero activities
without a successor other than the single finish milestone; zero activities whose only predecessor is a
start-to-start relationship or whose only successor is a finish-to-finish relationship — in each case
excepting only activities listed in the retained exceptions record with a stated reason; and, for every
constraint present in the file, a matching entry in the constraint register with reason, authority and
review date. The query, its date, its author and its output are retained. Two reviewers running the same
query against the same file obtain the same counts, which is why this test is expressed as a query
rather than as a judgement about whether the logic is "adequate".

**22. Breach indicators.** Constraints appearing shortly before a reporting deadline; calendars amended
between issues with no record; a completion date that does not move when a critical activity slips;
large numbers of lags with no reasons; open-end counts that fall to zero immediately after a review with
no logic review recorded; activities with durations longer than the reporting cycle and no interim
milestones.

**23. Consequence within PCI authority.** Correction required and the schedule withheld until the
integrity tests pass or the exceptions are recorded; additional review; escalation; failure of the
associated examination competency; ethics review; certification investigation, suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Scenario judgement: a network extract in which the candidate identifies
the open ends, the constraint used in place of logic and the negative lag concealing an overlap.
Calculation review: the effect on completion of removing an unjustified constraint.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Supersedes PCL-LAW-10-01 *Schedule Logic*; that identifier is
retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-10.02 — Critical-Path Verification Before Reliance

**1. Normative requirement.** A credential holder must verify that the reported critical path is the
network's true longest path before relying on it for a decision, a report or a delay assessment.

**2. Purpose.** Controls reliance on a tool's answer. Scheduling software reports a critical path from
total float, and total float is distorted by constraints, multiple calendars, resource levelling and
out-of-sequence progress. The reported path can therefore differ from the actual longest path through
the network — and every acceleration decision, delay analysis and mitigation plan built on the wrong
path spends money on work that does not drive completion.

**3. Scope.** All candidates and credential holders who report, review, approve, rely on or give
assurance over a critical path, a longest path, float values, a delay analysis or an acceleration or
mitigation plan, on any project using a network schedule.

**4. Defined terms.** *verified*, *open end*, *current*, *evidence*, *material*, *independent*,
*competent reviewer*, *escalation threshold*.

**5. Required actions.** The professional must test the reported path rather than accept it, and must
record the test.

- **PCI-PCL-LAW-10.02-PR-01 — Longest-path confirmation.** The reported critical path must be confirmed
  as the longest path through the network to the completion milestone, and where the software's
  float-based critical path and the longest path differ, both must be reported with the difference
  explained.
- **PCI-PCL-LAW-10.02-PR-02 — Distorting features identified.** Before reliance, the professional must
  identify the constraints, calendars, resource levelling and out-of-sequence progress present in the
  schedule and state their effect on float and on the reported path.
- **PCI-PCL-LAW-10.02-PR-03 — Near-critical paths reported.** Paths within a float range stated by the
  adopting organisation's governance must be reported alongside the critical path, so that a decision is
  not taken on a single path that a small change would displace.
- **PCI-PCL-LAW-10.02-PR-04 — Delay analysis method stated.** Where the schedule is used to assess
  delay, the analysis method used must be named, its data sources stated, and its limitations disclosed;
  a method must not be selected because it produces a preferred result.

**6. Prohibited actions.** Reporting the software's critical path as verified without testing it;
relying on float values without stating the constraints and calendars that shape them; presenting a
single critical path where near-critical paths would displace it; selecting a delay-analysis method for
its outcome; asserting a critical path from a schedule that has failed the integrity tests in
`PCI-PCL-LAW-10.01`.

**7. Required evidence.** The verification record naming the longest path and the method used to confirm
it; the list of distorting features and their effect; the near-critical path report with the float range
used; the delay-analysis method statement with sources and limitations; the schedule file version tested.

**8. Responsible role.** The **planner** for the verification; the **project controls lead** for the
reported path as issued; the **commercial lead** where the path supports a claim or a defence.

**9. Approval authority.** The **project controls lead** approves the reported critical path for
reporting. Where the analysis supports a contractual position, the **commercial lead** approves its use
for that purpose.

**10. Independence requirement.** Where the critical path or a delay analysis supports a claim, a
defence, an extension of time or an entitlement, the verification must be re-performed by a person
**independent** of the party the outcome favours. For internal reporting, re-performability by a
**competent reviewer** is sufficient.

**11. Materiality or threshold.** Verification is required before every reliance, without threshold. The
near-critical float range under PR-03 is set by the adopting organisation's governance on the basis of
the project's duration, its reporting cycle and the volatility of its work — and it must be recorded,
because a range set without a basis is arbitrary. *Scaling:* on a USD 2 million refurbishment of ten
months a near-critical range of a few days is meaningful and a range of a month would capture the entire
schedule; on a USD 5 billion programme of eight years the reverse is true. This is one of the few
thresholds in this set that genuinely must differ between the two, and the law therefore fixes the
obligation to record the basis rather than fixing the number.

**12. Exception and waiver.** No exception is permitted to element 1. Where a schedule cannot be verified
— for example a counterparty's schedule supplied without its logic — any reliance on its critical path
must be stated as reliance on an unverified schedule, with the limitation disclosed at the point of use.

**13. Escalation trigger.** A reported critical path that differs materially from the verified longest
path; an acceleration or mitigation decision proposed on an unverified path; a delay-analysis method
changed after an unfavourable result; reliance on a schedule that failed the integrity tests.

**14. AI application.** AI may compute the longest path independently of the software's float
calculation, identify distorting features, list near-critical paths, compare successive schedule versions
to show path movement, and draft the limitations statement.

**15. AI prohibition.** AI must not determine the critical path relied on for a decision without human
verification; must not select a delay-analysis method; must not attribute delay to a party; and must not
approve a schedule analysis.

**16. AI verification.** Independent recomputation plus boundary testing: the professional must confirm
the AI-identified longest path by tracing it activity by activity through the network from completion
back to the current status date, and must test the path's stability by re-running with the two largest
distorting features removed, recording the result. Tracing the path by hand is the only method that
detects a path that the tool has computed correctly from logic that is wrong.

**17. External reference.**

- **U.S. Government Accountability Office — *GAO Schedule Assessment Guide: Best Practices for Project
  Schedules*.** Cited for the existence of published expectations on critical-path validity. Edition:
  GAO-16-89G, issued 22 December 2015; no practice text reproduced. Nature: Manual §6 category 5,
  professional framework published by a public audit institution; not a regulation. Checked 2026-08-03
  (EXT-069). Persuasive; binding only where a client or procurement regime requires it.
- **AACE International — Recommended Practice 29R-03 *Forensic Schedule Analysis*.** Cited for the
  existence of recognised delay-analysis methods. Not independently verified; no method prescribed or
  preferred here. Nature: Manual §6 category 5, professional framework. Register EXT-067.
  Applicability: persuasive only; acceptability differs between forums and jurisdictions.
- **Project Management Institute — *Practice Standard for Scheduling*.** Cited for the recognised
  treatment of network analysis. Edition: third edition per register; no clause asserted. Nature: Manual
  §6 category 5, professional framework. Checked 2026-08-03 (EXT-062). Persuasive only.

**18. Jurisdictional caution.** Which delay-analysis method a tribunal, adjudicator, arbitrator or court
will accept differs between forums and jurisdictions, and the entitlement that follows from a delay is a
question of the contract and the governing law. Both require qualified counsel, and neither is settled by
this law.

**19. Related PCI Laws.** `PCI-FND-LAW-03` (independent verification) governs; this law adds the
longest-path verification method and the distorting-feature disclosure. See also `PCI-PCL-LAW-10.01`,
`PCI-PCL-LAW-10.03`, `PCI-PCL-LAW-05.03`, `PCI-PCL-LAW-07.02`, `PCI-PCL-LAW-13.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 10 · KA 10.2 Network analysis and the Critical
Path Method; KA 10.3 Schedule compression and resourcing. Also Domain 7 · KA 7.2 Contract management.

**21. Compliance test.** Compliance is demonstrated when a verification record exists naming the
activities on the longest path in sequence, stating the method used to confirm it, listing the
constraints, calendars, levelling and out-of-sequence progress present and their effect, and identifying
the paths within the recorded near-critical range. A reviewer tracing the named path through the retained
schedule file from completion back to the status date must arrive at the same activity list. Where the
software's float-based path differs from the longest path, both must appear in the record. Two reviewers
tracing the same file reach the same path.

**22. Breach indicators.** A critical path that runs through activities with date constraints; a critical
path that does not reach the completion milestone; float values that change without a logic or duration
change; near-critical paths never reported; a delay analysis whose method differs from the previous
analysis on the same project with no reason stated; acceleration spent on activities that later prove not
to have been driving.

**23. Consequence within PCI authority.** Correction required and the analysis withheld until verified;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of appeal.

**24. Examination application.** Calculation review: given a small network with a constraint and two
calendars, the candidate identifies the true longest path and explains why the software's reported path
differs. Scenario judgement: an acceleration proposal aimed at a path that is not driving completion.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Supersedes PCL-LAW-10-02 *Critical Path Verification*; that
identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-10.03 — Status Date and Actual-Date Integrity

**1. Normative requirement.** A credential holder must status a schedule to a single stated status date
using actual dates taken from the record of when the work actually started and finished.

**2. Purpose.** Controls the update that is a wish. Actual dates entered as planned dates, progress
statused to a date nobody stated, remaining durations left untouched while time passes, forecast dates
that no logic produces — each converts the schedule from a model of the work into a restatement of the
plan, and the project loses the only instrument that would have shown the drift while it was still small.

**3. Scope.** All candidates and credential holders who status, review, approve or give assurance over
schedule progress, actual dates, remaining durations or forecast dates, on any project using a schedule
for reporting or decision-making.

**4. Defined terms.** *cut-off*, *objective evidence of progress*, *evidence*, *current*, *material*,
*source record*, *approved*, *escalation threshold*.

**5. Required actions.** The professional must record what happened, when it happened, and what remains —
each from a record, and each visible in the issued file.

- **PCI-PCL-LAW-10.03-PR-01 — Actual dates from records.** Every actual start and actual finish entered
  must be taken from a dated record of the event, and no actual date may fall after the status date;
  where an actual date is estimated because no record exists, it must be marked as estimated with the
  basis and the person who estimated it.
- **PCI-PCL-LAW-10.03-PR-02 — Remaining duration re-assessed, not defaulted.** The remaining duration of
  every in-progress activity must be re-assessed against the work remaining at the status date and must
  not be left at its original value or derived only by subtracting elapsed time; forecast dates must be
  those the network computes from the statused position, and any manually entered forecast date must be
  identified with its reason.
- **PCI-PCL-LAW-10.03-PR-03 — Status date stated and single.** Each issued schedule must state one status
  date; where any data within it is as at a different date, that data and its date must be identified.
- **PCI-PCL-LAW-10.03-PR-04 — Baseline dates untouched by the update.** A progress update must not alter
  baseline dates, baseline durations or the baseline logic; any such alteration is a change to the
  baseline and is governed by `PCI-PCL-LAW-03.02`.
- **PCI-PCL-LAW-10.03-PR-05 — Out-of-sequence progress resolved.** Where progress has occurred out of the
  planned sequence, the professional must record it, state how the schedule has been adjusted to
  represent the actual sequence, and state the effect on the computed dates.

**6. Prohibited actions.** Entering planned dates as actual dates; statusing to a date that is not stated;
leaving remaining durations unchanged as time passes; entering a forecast date the network does not
produce without identifying it; altering baseline dates during an update; suppressing out-of-sequence
progress by allowing the software to resolve it silently.

**7. Required evidence.** The issued schedule file with its status date; the records supporting actual
dates for a sample of activities; the remaining-duration re-assessment record; the list of manually
entered forecast dates with reasons; the out-of-sequence record; the comparison confirming baseline dates
unchanged.

**8. Responsible role.** The **planner** for the statused schedule; each **control account owner** for
the accuracy of the progress and remaining duration for their work; the **project controls lead** for the
issued position.

**9. Approval authority.** The **project controls lead** approves the statused schedule for issue.

**10. Independence requirement.** Not required for statusing, which depends on knowledge of the work.
Verification of actual dates supporting a claim, a delay analysis or a payment must be performed by a
person **independent** of the party the outcome favours, consistent with `PCI-PCL-LAW-06.02`.

**11. Materiality or threshold.** Every actual date must come from a record, without threshold, because a
single wrong actual date on a driving activity moves the completion date. Sampling is permitted for
*verification*, never for *entry*, and the sample basis must be recorded and must include the activities
on and near the longest path. *Scaling:* on a USD 2 million refurbishment every actual date can be
checked against the site diary; on a USD 5 billion programme verification is sampled by contract with the
longest-path activities checked in full. The rule that entry comes from a record is identical.

**12. Exception and waiver.** Where no record of an actual date exists, the date may be estimated under
PR-01 provided it is marked as estimated. No exception is permitted to PR-03 or PR-04. Where a status
date must be moved after issue, the schedule must be reissued rather than amended in place.

**13. Escalation trigger.** Actual dates that cannot be supported by any record on activities that are
material to completion; a statused schedule whose baseline dates have moved; a forecast completion date
entered manually to match a contractual date; out-of-sequence progress that reverses the intended
construction sequence.

**14. AI application.** AI may extract actual dates from site records, timesheets, delivery notes and
inspection systems; flag activities whose remaining duration is inconsistent with reported progress;
detect out-of-sequence progress; and compare successive statused schedules to report movement.

**15. AI prohibition.** AI must not set an actual date without human confirmation against the record; must
not set a remaining duration for an activity whose remaining work it cannot observe; must not approve a
statused schedule; and must not resolve out-of-sequence progress by adopting a software default without a
recorded human decision.

**16. AI verification.** Source tracing on a stated sample plus independent recomputation: the
professional must confirm AI-extracted actual dates against the underlying records for all longest-path
and near-critical activities and for a recorded random sample of the remainder; must confirm that the
computed dates in the issued file follow from the statused position by re-running the network; and must
review every AI-proposed remaining duration against the **control account owner**'s assessment of the work
remaining.

**17. External reference.**

- **U.S. Government Accountability Office — *GAO Schedule Assessment Guide: Best Practices for Project
  Schedules*.** Cited for the existence of published expectations on schedule updating and status.
  Edition: GAO-16-89G, 22 December 2015; no practice text reproduced. Nature: Manual §6 category 5,
  professional framework published by a public audit institution; not a regulation. Checked 2026-08-03
  (EXT-069). Persuasive; binding only where required by a client or procurement regime.
- **ISO — ISO 21508 *Earned value management in project and programme management*.** Cited for the
  alignment of schedule status with performance measurement. Edition: 2018 per register; no clause
  asserted. Nature: Manual §6 category 3, international voluntary standard. Checked 2026-08-03 (EXT-029).
  Voluntary unless adopted.

**18. Jurisdictional caution.** Where a contract prescribes the form, frequency and content of schedule
updates and their submission and acceptance, those requirements are contractual obligations that may
exceed this law. Actual dates recorded in a schedule may later be relied on as evidence in a dispute;
their treatment is a matter for the contract administrator and qualified counsel.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (evidence before assertion) governs; this law adds the
status-date, actual-date and remaining-duration obligations. See also `PCI-PCL-LAW-10.01`,
`PCI-PCL-LAW-10.02`, `PCI-PCL-LAW-06.02`, `PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-03.02`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 10 · KA 10.4 Progress measurement and schedule
control. Also Domain 6 · KA 6.4 Integrating cost & schedule; Domain 8 · KA 8.4 Monitoring & Controlling.

**21. Compliance test.** Compliance is demonstrated when the issued schedule states one status date; when
no actual date in the file falls after that status date; when, for the longest-path and near-critical
activities and a recorded random sample of the remainder, each actual date matches a dated record or is
marked as estimated with a basis; when every in-progress activity's remaining duration has a recorded
re-assessment; when manually entered forecast dates are identified; and when a comparison against the
approved baseline shows baseline dates unchanged. An actual date after the status date is a failure of this test
detectable by query, and so is a baseline date that has moved. Two reviewers running the same queries on
the same file obtain the same results.

**22. Breach indicators.** Actual dates identical to baseline dates across many activities; remaining
durations that fall by exactly the elapsed period each cycle; forecast completion unchanged while
activities slip; status dates that vary between the schedule and the cost report; activities showing
progress before their predecessors started; baseline dates that differ from the last approved baseline.

**23. Consequence within PCI authority.** Correction required and the schedule withheld or reissued;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of appeal.

**24. Examination application.** Evidence selection: given a site diary, a delivery note and a timesheet,
the candidate selects the record that establishes an actual start. Scenario judgement: a statused schedule
whose completion date has not moved despite a month of slippage on the driving path.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-10-03 *Progress Measurement*; that
identifier is retired and is not reused.

---
## Domain 11 — Business Process Cycles & the Control Environment

### PCI LAW PCI-PCL-LAW-11.01 — Reproducibility of the Reported Controls Position

**1. Normative requirement.** A credential holder must retain, with each issued **project controls
deliverable**, the records that make every figure in it **reproducible** by a **competent reviewer**.

**2. Purpose.** Controls the position that was true when issued and cannot be demonstrated afterwards.
Systems are upgraded, extracts are overwritten, spreadsheets are edited, staff move on, and a figure
that was properly derived becomes indistinguishable from one that was invented. This is the law that
makes every other law in this set testable — the compliance tests above assume that the records still
exist, and this is the obligation that makes that assumption safe.

**3. Scope.** All candidates and credential holders who issue, approve or give assurance over a project
controls deliverable, on any project. It applies for the retention period set by the adopting
organisation's governance, by the contract, or by applicable law — whichever is longest.

**4. Defined terms.** *project controls deliverable*, *reproducible*, *competent reviewer*, *evidence*,
*source record*, *current*, *cut-off*, *tool configuration record*, *material*.

**5. Required actions.** The professional must retain what a later reviewer needs, in a form that has not
changed since issue.

- **PCI-PCL-LAW-11.01-PR-01 — Issue set retained.** For each deliverable, the retained set must include
  the deliverable as issued, the source extracts used with their timestamps, the working calculation, the
  version identifiers of any baseline or schedule relied on, and the approval record.
- **PCI-PCL-LAW-11.01-PR-02 — Method recorded, not only result.** Where a figure is derived by a method
  that is not evident from the retained data — an allocation, an apportionment, a weighting, a
  statistical or simulation model — the method and its parameters must be recorded with the figure.
- **PCI-PCL-LAW-11.01-PR-03 — Tool and configuration recorded.** Where an AI tool contributed to a
  figure or an analysis, the **tool configuration record** must be retained with the deliverable.
- **PCI-PCL-LAW-11.01-PR-04 — Retained records not alterable in place.** Retained records must be held so
  that a later edit is either impossible or detectable, and the retention location must be known to
  someone other than the preparer.
- **PCI-PCL-LAW-11.01-PR-05 — Retention through handover.** On handover, project closure or the
  preparer's departure, the retained set must be transferred with a record of what was transferred and
  to whom, and must not be left in personal storage.

**6. Prohibited actions.** Issuing a deliverable whose figures cannot be reproduced from retained
records; overwriting a source extract used in an issued deliverable; retaining a result without the
method that produced it; holding the only copy of project records in personal storage; deleting
superseded working papers that support an issued figure; retaining records in a form that can be edited
without trace.

**7. Required evidence.** The retained issue set per deliverable; the method records; the tool
configuration records; the retention register or index; the handover record.

**8. Responsible role.** The **project controls lead** for the retention of controls deliverables; each
preparer for the completeness of their own issue set; the **decision owner** for ensuring retention
arrangements exist before the deliverable is relied on.

**9. Approval authority.** The **project controls lead** approves the retention arrangement. Destruction
of records before the end of the retention period may be approved only by the authority named in the
adopting organisation's records policy.

**10. Independence requirement.** Reproduction must be achievable by a **competent reviewer** who is
**independent** of the preparer — that is the test, and it is why records held only in the preparer's
head or personal storage fail it, however complete they feel to the preparer.

**11. Materiality or threshold.** Every issued deliverable must be reproducible. The **materiality rule**
decides the granularity at which working calculations are retained: figures above it are retained at the
level that allows recomputation of the individual figure; below it, retention at the aggregate level with
the method recorded is sufficient. The retention period is set by the adopting organisation's governance,
the contract or applicable law, whichever is longest — this law sets no period of its own, because any
number would be wrong in some jurisdiction. *Scaling:* on a USD 2 million refurbishment the retained set
may be one folder per month; on a USD 5 billion programme it is a records system with an index. The test —
can an independent competent reviewer reproduce the figure — does not change.

**12. Exception and waiver.** Where a source system cannot produce a retainable extract, the professional
must retain the query, the parameters and the date so the extract can be reproduced, and must record the
limitation. No exception is permitted to PR-01 or PR-05.

**13. Escalation trigger.** Discovery that an issued figure cannot be reproduced; loss, overwriting or
deletion of records supporting an issued deliverable; a departure or handover completed without transfer
of the retained set; a request to destroy records before the retention period ends.

**14. AI application.** AI may index retained records, detect missing components of an issue set, check
that every issued deliverable has a retained extract, and assemble the retention register.

**15. AI prohibition.** AI must not delete or archive records outside a recorded human decision; must not
determine that a record is no longer required; and must not substitute a generated reconstruction for a
retained record.

**16. AI verification.** Reconciliation plus sampling on a stated basis: the professional must confirm
against the retention register that each issued deliverable in the period has a complete issue set, and
must test reproduction of a recorded sample of figures from the retained records alone. A retention
register that reports completeness is not evidence of completeness until a sample has actually been
reproduced.

**17. External reference.**

- **ISO — ISO 15489-1 *Information and documentation — Records management — Part 1: Concepts and
  principles*.** Cited for the existence of an international treatment of records management concepts.
  Edition: 2016 per the register; no clause asserted. Nature: Manual §6 category 3, international
  voluntary standard. Checked 2026-08-03 (register EXT-025). Applicability: voluntary unless adopted by
  regulation or contract.
- **COSO — *Internal Control — Integrated Framework*.** Cited for the concept of retaining information
  supporting internal control. Edition: 2013 per register. Nature: Manual §6 category 5, professional
  framework; not regulatory authority. Checked 2026-08-03 (EXT-084). Voluntary unless adopted.

**18. Jurisdictional caution.** Statutory retention periods, data-protection obligations governing what
may be retained and for how long, litigation and audit holds, and cross-border restrictions on where
records may be stored are matters of local law that vary widely and can conflict with one another.
Qualified local advice is required, and this law's retention obligation never overrides a legal
requirement to delete personal data.

**19. Related PCI Laws.** `PCI-FND-LAW-12` (record integrity) governs; this law adds the specific
reproducibility test and the retained-issue-set content for controls deliverables. See also
`PCI-PCL-LAW-04.01`, `PCI-PCL-LAW-05.04`, `PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-07.02`,
`PCI-PCL-LAW-13.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 11 · KA 11.3 Internal control and segregation
of duties. Also Domain 4 · KA 4.3 Management reporting; Domain 13 · KA 13.2 Data: the fuel.

**21. Compliance test.** Compliance is demonstrated when a **competent reviewer**, given only the retained
records and denied access to the preparer, can reproduce a recorded sample of figures from an issued
deliverable and obtain the same values; when each issued deliverable has a retained issue set containing
the components in PR-01; and when any AI-assisted figure has a retained tool configuration record. A
figure the reviewer cannot reproduce without asking the preparer is a failure of this test — and the test is
deliberately constructed so that it fails when the only route to the answer is a conversation.

**22. Breach indicators.** Working papers held in personal drives; extracts with no timestamps; issued
figures whose calculation exists only in a spreadsheet that has since been edited; deliverables issued
after a preparer's departure with no transferred records; retention registers that list documents nobody
can locate; AI-assisted analyses with no record of which tool or model produced them.

**23. Consequence within PCI authority.** Correction required and the deliverable withdrawn until its
basis is retained; additional review; escalation; failure of the associated examination competency;
ethics review; certification investigation, suspension or withdrawal — each subject to due process and a
right of appeal.

**24. Examination application.** Scenario judgement: a figure queried nine months after issue, with three
candidate record sets of which only one permits reproduction. Evidence selection: identifying which
components of an issue set are missing.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Supersedes PCL-LAW-11-02 *The Audit Trail*; that identifier is
retired and is not reused.

---

## Domain 12 — Risk Management for Project Controls

### PCI LAW PCI-PCL-LAW-12.01 — Risk Statement Quality and Named Ownership

**1. Normative requirement.** A credential holder must record each risk as a cause, an uncertain event
and an effect on a stated project objective, with one named individual accountable for managing it.

**2. Purpose.** Controls the register that cannot be managed. "Bad weather", "supply chain" and
"stakeholder issues" are topics, not risks: they cannot be assessed, priced, mitigated or closed, and
they cannot be owned. A register of topics generates activity and no control, and — because it looks
like risk management — it displaces the analysis that would have produced one.

**3. Scope.** All candidates and credential holders who identify, record, assess, review, approve or give
assurance over project risks and opportunities, on any project of any size and any delivery model.

**4. Defined terms.** *evidence*, *material*, *approved*, *decision owner*, *competent reviewer*,
*escalation threshold*, *trend*, *change*.

**5. Required actions.** The professional must write each risk so that it can be tested, and must attach
it to a person.

- **PCI-PCL-LAW-12.01-PR-01 — Three-part statement.** Each register entry must state the cause (a fact
  that exists), the event (an uncertainty that may occur) and the effect (a consequence for cost,
  schedule, scope, quality, safety or another stated objective); an entry that states only one or two of
  the three must not be assessed or quantified until it is completed.
- **PCI-PCL-LAW-12.01-PR-02 — One named owner.** Each risk must name one individual accountable for its
  management, with the authority and the budget to act; a function, a team or a role title is not an
  owner, and a risk with no owner must not be recorded as managed.
- **PCI-PCL-LAW-12.01-PR-03 — Separation of risk from issue and from change.** An event that has already
  occurred must be recorded as an issue, not a risk; an event whose cost consequence has become certain
  must be moved to the forecast or the change register under `PCI-PCL-LAW-03.04` and removed from the
  risk exposure, so that it is not counted twice.
- **PCI-PCL-LAW-12.01-PR-04 — Response with an owner and a date.** Each response must state the action,
  the individual accountable for it, the date by which it will be done, and the effect it is expected to
  have on likelihood or impact; a response recorded as "monitor" must state what will be monitored, by
  whom and against what trigger.

**6. Prohibited actions.** Recording a risk as a topic; quantifying an entry that does not state cause,
event and effect; assigning ownership to a function or a team; leaving a materialised risk in the register
as a risk; counting the same exposure in both the risk position and the forecast; recording a response
that no one is accountable for.

**7. Required evidence.** The risk register with the three-part statement per entry; the named owner per
entry; the response plan with owners and dates; the record of transfers between the risk, issue and
change registers; the review record with dates.

**8. Responsible role.** The **risk lead** for the register's quality; each named risk owner for their
risk; the **project controls lead** for the consistency between the risk position, the forecast and the
change register.

**9. Approval authority.** The **decision owner** for the project approves the risk register at the
frequency set by the adopting organisation's governance, and approves the ownership assignments.

**10. Independence requirement.** Not required for identification, which depends on knowledge of the work.
The review of register quality against PR-01 and PR-02 must be performed by a person **independent** of
the risk owners, because an owner is the least likely person to record their own risk as unmanaged.

**11. Materiality or threshold.** Every recorded risk must satisfy PR-01 and PR-02, without threshold — a
poorly written low-value risk is as useless as a poorly written high-value one and costs the same to
write properly. The **materiality rule** decides which risks require quantification under
`PCI-PCL-LAW-12.02` and which reach the escalation threshold. *Scaling:* on a USD 2 million refurbishment
a register of fifteen well-formed risks is a complete risk position; on a USD 5 billion programme risks
are held at multiple tiers with escalation criteria between them, and the professional must record the
tier at which each risk is managed. The three-part statement rule is identical at every tier.

**12. Exception and waiver.** No exception is permitted to PR-01 or PR-02. A newly identified risk may be
recorded in outline for one reporting cycle while its statement is completed, provided it is marked as
incomplete and is not quantified or reported as assessed during that period.

**13. Escalation trigger.** A **material** risk with no owner, or whose owner lacks authority to act; a
risk that has materialised and remains in the register as a risk; a risk exposure counted in both the
risk position and the forecast; a response whose date has passed with no action and no revised date.

**14. AI application.** AI may propose risks from project documents, historical registers and comparable
projects; test statements for the three-part structure; detect duplicates and near-duplicates; identify
entries that have materialised; and draft response options.

**15. AI prohibition.** AI must not own a risk, decide that a risk is closed, determine that a risk is
immaterial, or replace the judgement of the named owner about likelihood, impact or response.

**16. AI verification.** Named human judgement recorded with reasoning, plus source tracing: the risk
owner must record, for each AI-proposed risk, whether it is accepted and why; AI-proposed causes must be
confirmed against the document or record the tool relied on; and AI-identified duplicates must be
confirmed by the **risk lead** before merging, because merging two genuinely different risks conceals one
of them.

**17. External reference.**

- **ISO — ISO 31000 *Risk management — Guidelines*.** Cited for the existence of an internationally
  recognised treatment of risk-management principles. Edition: ISO 31000:2018, second edition, reviewed
  and confirmed 2023 per the register; no clause asserted. Nature: Manual §6 category 3, international
  voluntary standard — **guidance, and expressly not a certifiable requirements standard**. Checked
  2026-08-03 (register EXT-020). Applicability: voluntary unless adopted by regulation or contract; this
  law does not restate its process and does not require conformity with it.
- **Project Management Institute — *A Guide to the Project Management Body of Knowledge (PMBOK Guide)*.**
  Cited for the recognised concept of a risk register. Edition deliberately not asserted. Nature: Manual
  §6 category 5, professional framework; not regulatory authority. Checked 2026-08-03 (EXT-060).
  Persuasive only.

**18. Jurisdictional caution.** Risks with safety, environmental, employment or regulatory consequences
may carry legal duties of assessment, notification and control under local law that are independent of,
and stricter than, this law. Those duties require qualified local advice and are not discharged by a
register entry.

**19. Related PCI Laws.** `PCI-FND-LAW-01` (professional accountability) governs; this law adds the
statement-quality and single-owner obligations specific to a project risk register. See also
`PCI-PCL-LAW-12.02`, `PCI-PCL-LAW-12.03`, `PCI-PCL-LAW-03.04`, `PCI-PCL-LAW-05.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 12 · KA 12.1 The risk framework; KA 12.2 The
risk process. Also Domain 5 · KA 5.4 Change control and cost impact.

**21. Compliance test.** Compliance is demonstrated when every entry in the register states a cause that
is a present fact, an event that is uncertain, and an effect on a stated objective; when every entry names
one individual; when no entry describes an event that has already occurred; and when the sum of exposure
in the risk position and the amounts for the same events in the forecast contains no double count. An
entry whose "cause" is itself uncertain, or whose "event" has already happened, is a failure of this test. Two
reviewers applying the three-part test to the same register produce the same exception list — the test
turns on the grammar of the entry, not on the reviewer's view of the risk.

**22. Breach indicators.** Register entries of two or three words; owners recorded as departments; the
same risks carried unchanged for many periods; responses that all read "monitor"; risks closed with no
outcome recorded; a risk exposure that does not fall as the work completes; materialised risks still
carried as risks alongside their cost in the forecast.

**23. Consequence within PCI authority.** Correction required and the risk position withheld from
quantification until entries are well-formed; additional review; escalation; failure of the associated
examination competency; ethics review; certification investigation, suspension or withdrawal — each
subject to due process and a right of appeal.

**24. Examination application.** Scenario judgement: five register entries of which the candidate must
identify those that are topics, those that are issues and those that are properly formed risks. Evidence
selection: identifying the record that establishes a stated cause as a present fact.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-12-01 *Risk and Contingency Governance*;
that identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-12.02 — Basis and Confidence Level of Quantified Contingency

**1. Normative requirement.** A credential holder must state, with every contingency figure, the method
by which it was derived, the risks it covers and the confidence level it represents.

**2. Purpose.** Controls the number with no meaning. A contingency stated as a percentage of the estimate,
or as a simulation output whose confidence level is unstated, cannot be defended, cannot be tested against
the exposure it is meant to cover, and cannot be drawn down accountably. Worse, an unstated confidence
level lets the same figure be described as prudent to one audience and as tight to another.

**3. Scope.** All candidates and credential holders who derive, review, approve or give assurance over
contingency, risk allowance, management reserve or a risk-adjusted estimate, on any project of any size.

**4. Defined terms.** *material*, *evidence*, *approved*, *decision owner*, *independent*, *reproducible*,
*escalation threshold*, *trend*.

**5. Required actions.** The professional must derive contingency from the assessed exposure, and must
disclose enough for a reader to test it.

- **PCI-PCL-LAW-12.02-PR-01 — Confidence level stated.** Where contingency is derived from a probabilistic
  analysis, the confidence level adopted must be stated, together with the figures at the levels either
  side of it, so that the choice is visible as a choice; where it is derived by another method, the method
  and its basis must be stated instead.
- **PCI-PCL-LAW-12.02-PR-02 — Correlation and dependency treated.** A probabilistic analysis must state
  the correlation assumptions applied between risks and between cost elements, and must state the effect
  on the result of assuming independence; treating correlated risks as independent without saying so is
  prohibited, because it understates the tail that contingency exists to cover.
- **PCI-PCL-LAW-12.02-PR-03 — Coverage stated.** The contingency statement must identify which risks the
  contingency covers and which it does not — in particular whether it covers scope growth, escalation,
  currency movement, force majeure, and risks retained by another party — so that a gap between the
  exposure and the cover is visible rather than assumed away.
- **PCI-PCL-LAW-12.02-PR-04 — Contingency separate from estimate.** Contingency must be held and reported
  as a separate line, must not be distributed into control account budgets or forecasts, and must not be
  used to fund approved scope changes, which are funded through `PCI-PCL-LAW-05.04`.
- **PCI-PCL-LAW-12.02-PR-05 — Reproducible analysis.** The model, its inputs, its distributions, its
  correlation matrix and its random seed or equivalent must be retained so that the analysis is
  **reproducible** under `PCI-PCL-LAW-11.01`.

**6. Prohibited actions.** Stating a contingency without its method; adopting a confidence level without
stating it; assuming independence between plainly correlated risks without disclosure; setting
contingency by reference to what is affordable and describing it as assessed; distributing contingency
into control accounts; using contingency to fund approved changes.

**7. Required evidence.** The contingency derivation with method, inputs and outputs; the confidence-level
statement with adjacent levels; the correlation assumptions and their effect; the coverage statement; the
retained model; the approval record.

**8. Responsible role.** The **risk lead** for the quantification; the **project controls lead** for its
integration into the cost position; the **decision owner** for the level adopted.

**9. Approval authority.** The **decision owner** for the project approves the contingency held and the
confidence level adopted, and that approval must record the reason for the level chosen. Management
reserve, where held above the project, is approved by the authority named in the adopting organisation's
governance.

**10. Independence requirement.** Where contingency is **material** to a funding decision or an external
submission, the quantification must be reviewed by a person **independent** of the estimator and of the
party seeking the funding.

**11. Materiality or threshold.** **No confidence level is prescribed by this law.** A percentile is a
governance choice that depends on the organisation's risk appetite, the project's role in its portfolio,
who bears the overrun and what other reserves exist — and a number invented here would be applied
mechanically to projects it does not fit. The obligation is therefore to state the level, to state the
figures either side of it, and to record who chose it and why. Where the adopting organisation's
governance prescribes a level, that level applies and its source is cited. *Scaling:* on a USD 2 million
refurbishment a full probabilistic model may cost more than the contingency it sizes, so a documented
risk-by-risk expected-value build-up with a stated basis satisfies this law; on a USD 5 billion programme
a probabilistic analysis with correlation is expected and its absence must be justified. The disclosure
obligations are identical in both cases; only the method differs, and the method must be stated either
way.

**12. Exception and waiver.** No exception is permitted to element 1. Where a client, funder or contract
prescribes both the contingency and its method, the professional must state the prescribed basis and, if
their own analysis differs materially, must record that difference and report it to the **decision
owner** — accepting an imposed figure without recording the difference is a breach.

**13. Escalation trigger.** A contingency materially below the assessed exposure at the stated confidence
level; a confidence level changed between periods without disclosure; correlation assumptions altered to
reduce the result; contingency drawn into control accounts; an imposed contingency that the professional's
analysis does not support.

**14. AI application.** AI may fit distributions to historical data, run and re-run simulations, test
sensitivity to correlation assumptions, benchmark contingency against comparable projects, and draft the
disclosure statement.

**15. AI prohibition.** AI must not select the confidence level; must not set correlation assumptions
without human review; must not approve contingency; and must not present a simulation output as an
assessed contingency without the human derivation this law requires.

**16. AI verification.** Independent recomputation, sensitivity analysis and boundary testing: the
professional must re-run the model with a different seed and confirm the result is stable; must
recompute the expected value of the largest contributing risks by hand and compare; must test the result
at the confidence levels either side of the adopted one; and must test the effect of the independence
assumption. A single simulation run accepted without these tests is not verification.

**17. External reference.**

- **ISO — ISO 31000 *Risk management — Guidelines*.** Cited for internationally recognised risk-management
  principles. Edition: 2018, confirmed 2023 per register; no clause asserted. Nature: Manual §6 category
  3, international voluntary standard — guidance, not a certifiable requirements standard. Checked
  2026-08-03 (EXT-020). Voluntary unless adopted; this law does not restate it.
- **AACE International — Recommended Practices on risk analysis and contingency determination.** Cited as
  a class for the existence of recognised contingency-determination methods. **No numbered Recommended
  Practice, accuracy range or class table asserted or reproduced**; not independently verified. Nature:
  Manual §6 category 5, professional framework. Register EXT-068. Persuasive only.

**18. Jurisdictional caution.** Where contingency or reserve is reflected in statutory financial
statements — as a provision, an onerous-contract charge or otherwise — the applicable accounting
framework governs that treatment and requires qualified accounting advice. A controls contingency is not
a provision.

**19. Related PCI Laws.** `PCI-FND-LAW-05` (transparent assumptions) governs; this law adds the
confidence-level, correlation and coverage disclosures specific to quantified contingency. See also
`PCI-PCL-LAW-12.01`, `PCI-PCL-LAW-12.03`, `PCI-PCL-LAW-03.04`, `PCI-PCL-LAW-11.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 12 · KA 12.2 The risk process — quantitative
analysis; KA 12.3 Contingency and management reserve. Also Domain 3 · KA 3.2 Cost estimation.

**21. Compliance test.** Compliance is demonstrated when the contingency statement names the method; where
probabilistic, states the confidence level adopted and the figures at the levels either side; states the
correlation assumptions and the effect of assuming independence; states which risks are covered and which
are not; and when the retained model can be re-run by a **competent reviewer** to reproduce the stated
figure. A contingency figure whose confidence level is not stated is a failure of this test on its face. Two
reviewers re-running the retained model with the retained inputs obtain the same distribution.

**22. Breach indicators.** Contingency stated as a round percentage of the estimate; the same percentage
across projects of different risk profiles; a confidence level that appears only when it is favourable;
contingency equal to the gap between the estimate and the funding limit; correlation assumptions absent
from the record; contingency falling in step with overspend rather than with risk closure.

**23. Consequence within PCI authority.** Correction required and the contingency figure withheld until
its basis is stated; additional review; escalation; failure of the associated examination competency;
ethics review; certification investigation, suspension or withdrawal — each subject to due process and a
right of appeal.

**24. Examination application.** Calculation review: given a risk register and a simulation output, the
candidate states the contingency at two confidence levels and explains what the choice between them
means. Scenario judgement: a contingency reduced by changing a correlation assumption.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-12-01 *Risk and Contingency Governance*;
that identifier is retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-12.03 — Contingency Drawdown Authority and Re-assessment of Remaining Exposure

**1. Normative requirement.** A credential holder must not apply contingency to a cost without the
recorded approval of the authority holding it.

**2. Purpose.** Controls the reserve that drains without a decision. Where contingency is absorbed into
control accounts as overspend arises, nobody ever decides to spend it, nobody knows how much remains
against what exposure, and the first visible signal is the moment it runs out — typically late in
delivery, when the remaining risk is highest and no reserve is left to meet it.

**3. Scope.** All candidates and credential holders who request, approve, record, report or give assurance
over the drawdown, transfer, release or replenishment of contingency or management reserve, on any
project.

**4. Defined terms.** *approved*, *change*, *material*, *decision owner*, *evidence*, *independent*,
*escalation threshold*, *trend*.

**5. Required actions.** The professional must make each drawdown a decision with a reason, and must
re-test what remains against what is left to happen.

- **PCI-PCL-LAW-12.03-PR-01 — Drawdown request tied to a risk.** Each drawdown request must identify the
  risk or risks from the register whose materialisation it funds, the amount, the evidence that the event
  has occurred, and the residual exposure for those risks after the drawdown.
- **PCI-PCL-LAW-12.03-PR-02 — Remaining-exposure re-assessment.** At each reporting cut-off the remaining
  contingency must be compared with the assessed exposure of the risks still open, on the basis stated
  under `PCI-PCL-LAW-12.02`, and the comparison reported; a shortfall must be reported in the period in
  which it is identified.
- **PCI-PCL-LAW-12.03-PR-03 — Separation of contingency from management reserve.** Contingency held for
  identified risks within the baseline and management reserve held outside it must be recorded, drawn and
  reported separately, each with its own approval authority, and a transfer between them must be approved
  as a decision rather than performed as an adjustment.
- **PCI-PCL-LAW-12.03-PR-04 — Release recorded.** Where a risk closes without materialising, the
  contingency held for it must be identified and either released with a recorded decision or retained
  against a stated exposure; silent retention of contingency for closed risks is prohibited, as is silent
  release into performance.

**6. Prohibited actions.** Applying contingency without approval; drawing contingency for a risk not in the
register; using contingency to fund an approved scope change; transferring between contingency and
management reserve as an adjustment; reporting contingency remaining without reporting the exposure it
faces; allowing contingency to be absorbed into control account performance.

**7. Required evidence.** The drawdown requests with their risk references and evidence; the approvals with
approver and date; the period comparison of remaining contingency against remaining exposure; the record of
transfers and releases; the contingency register showing opening balance, drawdowns, releases and closing
balance per period.

**8. Responsible role.** The **risk lead** for the exposure assessment; the **project controls lead** for
the contingency register and the reporting; the **decision owner** for the drawdown decision.

**9. Approval authority.** The **decision owner** for the project approves drawdown of project contingency
within the bands recorded in the adopting organisation's delegation of authority. Management reserve is
approved by the authority named in that delegation, which must be above the project.

**10. Independence requirement.** The approver of a drawdown must be **independent** of the control account
that receives it — a control account owner must not approve contingency into their own control account,
which is the single most common route by which reserve disappears without a decision.

**11. Materiality or threshold.** Every drawdown requires approval, of any value, because contingency
drains in small amounts. Value bands in the delegation of authority determine **which** authority approves,
never **whether** approval is required, and drawdowns for one risk must be aggregated for banding.
*Scaling:* on a USD 2 million refurbishment the sponsor approves every drawdown personally; on a USD 5
billion programme approval is tiered and the exposure comparison is performed per portfolio segment. The
prohibition on self-approval into one's own control account applies identically.

**12. Exception and waiver.** No exception is permitted to element 1. Where cost must be incurred
immediately to protect life, the works or the environment, the cost may be incurred before approval
provided the drawdown request is submitted at the first opportunity and no later than the current reporting
cycle, with the reason for the sequence recorded.

**13. Escalation trigger.** Remaining contingency below the assessed remaining exposure; a drawdown made
without approval; contingency drawn for a risk not in the register; a transfer between contingency and
management reserve performed without a recorded decision; contingency exhausted before a stated proportion
of the work is complete.

**14. AI application.** AI may match drawdown requests to register entries, track the contingency balance,
compare remaining contingency with remaining exposure, model the depletion trajectory against the remaining
programme, and flag drawdowns without approvals.

**15. AI prohibition.** AI must not approve a drawdown, decide that a risk has materialised, release
contingency, or determine that remaining contingency is sufficient.

**16. AI verification.** Reconciliation plus independent recomputation: the professional must reconcile the
contingency register's movements to the approved drawdowns and releases with no residual, must recompute
the remaining exposure independently of the tool, and must confirm every AI-matched drawdown against the
register entry and the evidence that the event occurred.

**17. External reference.**

- **ISO — ISO 31000 *Risk management — Guidelines*.** Cited for recognised risk-management principles
  including monitoring and review. Edition: 2018, confirmed 2023; no clause asserted. Nature: Manual §6
  category 3, international voluntary standard — guidance, not certifiable. Checked 2026-08-03 (EXT-020).
  Voluntary unless adopted.
- **COSO — *Internal Control — Integrated Framework*.** Cited for authorisation as a control activity over
  the use of reserves. Edition: 2013 per register. Nature: Manual §6 category 5, professional framework;
  not regulatory authority. Checked 2026-08-03 (EXT-084). Voluntary unless adopted.

**18. Jurisdictional caution.** Where contingency or reserve is held under a funding agreement, a public
funding regime or a contract, the conditions on its use are contractual or regulatory obligations that may
exceed this law. Their interpretation requires the contract administrator and qualified counsel.

**19. Related PCI Laws.** `PCI-FND-LAW-04` (human decision authority) governs; this law adds the drawdown
approval, the exposure re-assessment and the reserve separation obligations. See also
`PCI-PCL-LAW-12.01`, `PCI-PCL-LAW-12.02`, `PCI-PCL-LAW-05.04`, `PCI-PCL-LAW-03.03`,
`PCI-PCL-LAW-03.04`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 12 · KA 12.3 Contingency and management reserve
— drawing down and re-baselining. Also Domain 5 · KA 5.4 Change control and cost impact.

**21. Compliance test.** Compliance is demonstrated when the contingency register reconciles for the period —
opening balance, plus approved additions, less approved drawdowns and releases, equals closing balance, with
no residual; when every drawdown cites a register risk, carries evidence that the event occurred, and names
an approver who does not own the receiving control account; and when the period report states remaining
contingency alongside the assessed remaining exposure. A movement in the contingency balance with no
approval is a failure of this test, and it is found by arithmetic rather than by judgement. Two reviewers performing
the reconciliation on the same records obtain the same residual.

**22. Breach indicators.** Contingency falling in exactly the amount of each period's overspend; drawdowns
approved after the cost was incurred as a matter of routine; remaining contingency reported without the
exposure it faces; contingency exhausted early in delivery; transfers between reserve categories described
as reclassifications; risks closed with their contingency neither released nor reassigned.

**23. Consequence within PCI authority.** Correction required and the drawdown reversed or regularised;
output withheld; additional review; escalation; failure of the associated examination competency; ethics
review; certification investigation, suspension or withdrawal — each subject to due process and a right of
appeal.

**24. Examination application.** Calculation review: the candidate reconciles a contingency register,
identifies the unapproved movement and computes the shortfall against remaining exposure. Ethical dilemma:
a control account owner asking to approve a drawdown into their own account "to keep the report clean".

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-12-01 *Risk and Contingency Governance*;
that identifier is retired and is not reused.

---
## Domain 13 — AI for Project Controls & Project Management

### PCI LAW PCI-PCL-LAW-13.01 — Approved Tools, Recorded Configuration and Protected Project Data

**1. Normative requirement.** A credential holder must not use an **AI tool** on project data outside the
tool-and-data boundary approved for that class of work by the adopting organisation.

**2. Purpose.** Controls two failures that arrive together. Project controls data is among the most
commercially sensitive information a project holds — unit rates, claim positions, contingency, contract
terms, counterparty performance — and a tool chosen personally by a practitioner may retain it, train on
it, or route it through a jurisdiction the project never agreed to. At the same time, an output produced
by an unrecorded tool at an unrecorded setting cannot be reproduced, so the analysis cannot be defended
even when it was right.

**3. Scope.** All candidates and credential holders who use, configure, procure, approve or give assurance
over AI tools applied to project cost, schedule, commercial, risk or performance data, on any project.
It covers standalone assistants and AI features embedded in controls, scheduling, commercial and
accounting applications alike — an embedded feature is not exempt because it arrived with the software.

**4. Defined terms.** *AI tool*, *AI assistance*, *tool configuration record*, *approved*, *material*,
*evidence*, *decision owner*, *escalation threshold*, *reproducible*.

**5. Required actions.** The professional must know what the tool does with the data before it sees it,
and must record what produced each output.

- **PCI-PCL-LAW-13.01-PR-01 — Approved-use record per class of work.** Before first use, the classes of
  project-controls work for which a tool is approved, and the classes for which it is not, must be
  recorded by the authority named in element 9; use outside the recorded classes is prohibited.
- **PCI-PCL-LAW-13.01-PR-02 — Data-handling determination before first use.** Before project data is
  placed in a tool, it must be determined and recorded whether the tool retains the data, uses it for
  training, discloses it to a third party, or processes it outside an agreed jurisdiction; where the
  answer cannot be established, the tool must be treated as retaining and disclosing.
- **PCI-PCL-LAW-13.01-PR-03 — Data classes barred by default.** Contract terms and prices, unit rates,
  claim and dispute positions, contingency and risk quantification, counterparty performance data,
  personal data and anything the project holds under a confidentiality obligation must not be placed in a
  tool that has not been approved for that data class under PR-01 and PR-02.
- **PCI-PCL-LAW-13.01-PR-04 — Configuration recorded with the output.** Every AI-assisted output relied
  on must carry a **tool configuration record** — the tool, the model or version, and the material
  settings, prompts or data sources — retained under `PCI-PCL-LAW-11.01`.
- **PCI-PCL-LAW-13.01-PR-05 — Change of tool or model reported.** A change of tool, model or material
  configuration that could alter the output of a recurring analysis must be recorded and reported with the
  first deliverable produced after it, so that a movement in results is not read as a movement in the
  project.

**6. Prohibited actions.** Using a personally chosen tool on project data outside the approved boundary;
placing barred data classes into an unapproved tool; relying on an output whose tool and configuration
were not recorded; assuming an embedded AI feature is approved because the host application is; changing
model or configuration silently between periods; circumventing a data restriction by paraphrasing or
partially redacting confidential material.

**7. Required evidence.** The approved-use record per tool and class of work; the data-handling
determination per tool with its date and its basis; the tool configuration records retained with
deliverables; the record of tool, model or configuration changes and the deliverables affected.

**8. Responsible role.** The credential holder who uses the tool, for staying inside the boundary; the
**project controls lead** for the recorded boundary on the project; the adopting organisation's named
technology or information-security authority for the determination under PR-02.

**9. Approval authority.** The authority named in the adopting organisation's governance for approving
technology and information handling approves a tool for a class of work and a class of data. Where the
organisation names none, the **decision owner** for the affected deliverable approves, records the basis,
and escalates the absence of a governance position.

**10. Independence requirement.** The person who selects or configures a tool must not be the person who
performs the independent verification of its output under `PCI-PCL-LAW-13.02` or `PCI-PCL-LAW-13.03` —
this is the fourth fact in the definition of **independent**, and it exists because a configurer verifies
their own configuration, not the output.

**11. Materiality or threshold.** The tool-and-data boundary applies to all use, of any size, because a
single prompt can disclose an entire commercial position. The **materiality rule** decides only which
configuration changes must be reported under PR-05 rather than merely recorded. *Scaling:* on a USD 2
million refurbishment the approved-use record may be a single page naming two tools and their permitted
data classes; on a USD 5 billion programme it is a managed catalogue with per-tool determinations. The
prohibition on placing barred data in an unapproved tool is identical, and it is not relaxed because a
small organisation has no technology function — where none exists, element 9's fallback applies and the
absence is escalated.

**12. Exception and waiver.** An exception may be approved by the authority in element 9, in writing,
naming the tool, the data class, the purpose, the duration and the compensating controls — for example
de-identification, a segregated environment, or an agreement barring retention and training. An exception
must not exceed the period stated in it, and it must be reported to the **decision owner** for the
affected deliverables. No exception is permitted to PR-04.

**13. Escalation trigger.** Project data placed in a tool outside the approved boundary; discovery that a
tool in use retains or trains on project data contrary to the determination; an output relied on whose
tool and configuration cannot be established; a model change that materially altered a recurring result
and was not reported.

**14. AI application.** AI may assist with the whole of project controls within the approved boundary, and
this set of laws is not a restriction on its use — the verification laws that follow are what make wide
use safe.

**15. AI prohibition.** An AI tool must not approve its own use, determine its own data handling, authorise
an exception, or be recorded as the authority for any of these decisions.

**16. AI verification.** Source tracing and named approval: the data-handling determination under PR-02
must be made from the tool provider's own published terms and any contract with the provider, recorded
with the date checked and the version of the terms relied on — and never from the tool's own answer about
itself, which is generated text and not evidence of the provider's obligations.

**17. External reference.**

- **ISO/IEC — ISO/IEC 42001 *Information technology — Artificial intelligence — Management system*.**
  Cited for the existence of an AI management-system standard against which an organisation's tool
  governance can be organised. Edition: ISO/IEC 42001:2023, first edition, per the register; no clause
  asserted. Nature: Manual §6 category 3, international voluntary standard; certifiable, but certification
  is a third party's opinion and not a substitute for this law. Checked 2026-08-03 (register EXT-021).
  Voluntary unless adopted by regulation or contract.
- **ISO/IEC — ISO/IEC 27001 *Information security management systems — Requirements*.** Cited for the
  existence of an information-security management standard relevant to the data-handling determination.
  Edition: ISO/IEC 27001:2022 with Amd 1:2024 per the register; no clause asserted. Nature: Manual §6
  category 3, international voluntary standard. Checked 2026-08-03 (EXT-023). Voluntary unless adopted.
- **NIST (US Department of Commerce) — *Artificial Intelligence Risk Management Framework (AI RMF 1.0)*.**
  Cited for the existence of a voluntary framework organising AI risk into govern, map, measure and
  manage functions. Edition: AI RMF 1.0, January 2023, per the register. Nature: a voluntary framework
  from a national standards institute; Classified as Manual §6 category 7, industry guidance: it has a single authoritative
  publisher but no standard-setter's binding force. Checked 2026-08-03 (register EXT-080). Applicability:
  expressly voluntary and non-regulatory.

**18. Jurisdictional caution.** Data protection, cross-border transfer, confidentiality obligations owed
to counterparties, and AI-specific legislation differ by jurisdiction and are changing. In the European
Union, Regulation (EU) 2024/1689 (the AI Act) is binding legislation applying in phases to those it
addresses, and the General Data Protection Regulation governs personal data; other jurisdictions differ.
Whether and how any of these applies to a given use is a legal question for qualified local counsel, and
it is not answered by this law.

**19. Related PCI Laws.** `PCI-FND-LAW-09` (confidentiality and approved technology) and `PCI-FND-LAW-14`
(responsible AI) govern. This law adds the per-class approval, the data-handling determination, the barred
data classes and the configuration record — none of which the foundational duties specify. See also
`PCI-PCL-LAW-13.02`, `PCI-PCL-LAW-13.03`, `PCI-PCL-LAW-13.04`, `PCI-PCL-LAW-11.01`,
`PCI-PCL-LAW-07.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 13 · KA 13.2 Data: the fuel — privacy,
confidentiality and preparing data; KA 13.4 AI tool categories for project controls & PM; KA 13.6
Governance, ethics, risk & assurance of AI.

**21. Compliance test.** Compliance is demonstrated when, for each tool in use on the project: (a) an
approved-use record names the classes of work and data it is approved for, dated before first use; (b) a
data-handling determination records whether the tool retains, trains on, discloses or exports the data,
with the date and the source of the answer; and (c) for a sample of AI-assisted deliverables selected on a
stated basis, each carries a tool configuration record naming tool, model and material settings. A
deliverable whose producing tool cannot be identified is a failure of this test. Two reviewers testing the same
sample reach the same list, because each element of the test is the presence or absence of a dated record.

**22. Breach indicators.** Outputs whose style or structure changes between periods with no recorded model
change; practitioners describing tools the approved-use record does not contain; determinations dated after
first use; contract text or rates appearing in prompts; the same analysis producing different results in
successive periods with no project change; configuration records absent from otherwise complete issue sets.

**23. Consequence within PCI authority.** Correction required and the affected outputs withheld until their
basis is established; additional review; escalation; failure of the associated examination competency;
ethics review; certification investigation, suspension or withdrawal — each subject to due process and a
right of appeal.

**24. Examination application.** AI-verification case: the candidate is given four proposed uses of a tool
and must identify which fall inside a stated approved boundary and which do not, and what record would
settle the question. Ethical dilemma: a deadline met by pasting a confidential rate schedule into an
unapproved assistant.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-13-01 *Data Lineage*; that identifier is
retired and is not reused.

---

### PCI LAW PCI-PCL-LAW-13.02 — Verification of AI-Generated Quantitative Controls Output

**1. Normative requirement.** A credential holder must independently recompute an AI-generated
quantitative controls figure before it is used in a **project controls deliverable**.

**2. Purpose.** Controls the most likely failure of AI in project controls: an output that is coherent,
well-formatted, confidently expressed and wrong. Forecasts, indices, allocations and reconciliations
produced by a tool arrive without the friction that used to expose errors — no arithmetic to check, no
intermediate working to query — and a plausible number is adopted precisely because nothing about it
invites doubt.

**3. Scope.** All candidates and credential holders who produce, review, approve or give assurance over
AI-generated or AI-assisted quantitative output — estimates at completion, estimates to complete, earned
value figures and indices, cost allocations, cost coding, accrual proposals, reconciliations, cash-flow
profiles, quantities and rates — on any project.

**4. Defined terms.** *verified*, *material*, *material AI assistance*, *independent*, *competent
reviewer*, *reproducible*, *evidence*, *tool configuration record*, *decision owner*.

**5. Required actions.** The professional must reproduce the number by a route that does not pass through
the tool, and must record the reproduction.

- **PCI-PCL-LAW-13.02-PR-01 — Recomputation of forecast output.** An AI-generated estimate at completion,
  estimate to complete or cash-flow profile must be recomputed from its components by the professional —
  actual cost, commitments, accruals, remaining work, approved changes, trends and stated contingency —
  and the two results reconciled before issue, with any difference resolved rather than averaged.
- **PCI-PCL-LAW-13.02-PR-02 — Recomputation of indices and derived measures.** AI-generated earned value
  figures, indices and variances must be recomputed from their retained inputs under
  `PCI-PCL-LAW-06.03` before publication.
- **PCI-PCL-LAW-13.02-PR-03 — Sampling for classification and reconciliation output.** Where AI has
  classified transactions, assigned cost codes, matched invoices to commitments or proposed accruals, the
  professional must test a sample selected on a recorded basis that includes the highest values, the
  lowest-confidence assignments and a random selection of the remainder; must record the sample, the
  method and the error rate; and must re-process the whole affected population where an error of principle
  is found.
- **PCI-PCL-LAW-13.02-PR-04 — Named human approval before release.** Every AI-assisted quantitative
  deliverable must carry the name of the individual who performed the verification and the name of the
  individual who approved its release, with the date; a deliverable naming only a tool must not be
  released.
- **PCI-PCL-LAW-13.02-PR-05 — Verification record.** The verification must record the method used, what
  was tested, what was found and how each difference was resolved; a record stating only that the output
  was reviewed does not satisfy this process requirement.

**6. Prohibited actions.** Issuing an AI-generated figure that no person has recomputed; averaging a tool's
result with the professional's own to resolve a difference; accepting a tool's output because it agrees
with expectation; testing only the items the tool flagged; recording verification that was not performed;
releasing a deliverable whose only named producer is a tool.

**7. Required evidence.** The verification record with method, scope, findings and resolutions; the
independent recomputation working; the sample basis and error rate for classification output; the tool
configuration record; the names and dates of the verifier and the approver.

**8. Responsible role.** The credential holder who issues the figure, for its verification; the **project
controls lead** for the deliverable as released; the **decision owner** for reliance on it.

**9. Approval authority.** The **project controls lead** approves release of an AI-assisted quantitative
deliverable; where the figure is material to a decision, the **decision owner** for that decision approves
its use.

**10. Independence requirement.** The verifier must be **independent** of the configuration of the tool
under the fourth fact in the definition. Where the AI-generated figure is material to an external
submission or an incentive, the recomputation must also be independent of the party the figure favours.
**The recomputation this law requires is additional to, and does not displace, the verification
`PCI-FND-LAW-03` requires**: a material figure must also be verified by a person independent of its
preparation before any person relies on it, and a recomputation performed by the professional who
produced or issued the figure satisfies this law's element 1 but does not satisfy that one. Where the
same individual performs both, the working record must say so, and the foundational verification
remains outstanding until an independent person performs it or a waiver under `PCI-FND-LAW-03`
element 12 is recorded.

**11. Materiality or threshold.** Recomputation is required for every AI-generated figure that is
**material** to the deliverable, and for the components that drive it. Below the materiality rule,
verification may be by sampling under PR-03 on a recorded basis — but the sample must never be drawn only
from what the tool flagged, because that is the tool verifying itself. *Scaling:* on a USD 2 million
refurbishment recomputation of the whole forecast is a short exercise and the sample is the population; on
a USD 5 billion programme recomputation is performed at control account level for material accounts and by
recorded sampling below, and the professional must record the level. What never scales away is that a
human recomputes the number that drives the decision.

**12. Exception and waiver.** No exception is permitted to element 1 for a material figure. Where an
AI-generated figure must be issued before verification can be completed, it must be labelled as unverified
on its face with the value affected stated, must not be used to take an irreversible decision, and must be
verified within a stated period, with any change reported to every recipient.

**13. Escalation trigger.** A material difference between the AI output and the recomputation that cannot
be explained; discovery that a material figure was issued unverified; an error of principle found in a
sample; pressure to accept a tool's output because verification would miss a deadline.

**14. AI application.** AI may produce the quantitative output, assemble its inputs, prepare the working
for the human recomputation, propose reconciliations and identify its own low-confidence items — this law
constrains reliance, not use.

**15. AI prohibition.** AI must not verify its own output; must not be recorded as the verifier or the
approver; must not resolve a difference between its output and a human recomputation; and must not
determine that a figure is immaterial and therefore exempt from recomputation.

**16. AI verification.** Independent recomputation is the required method for material figures, with
sampling on a stated basis for populations; boundary testing and sensitivity analysis must additionally be
applied where the output depends on assumptions the tool selected — the professional must test the result
at the boundaries of those assumptions and record the effect. Where a second AI tool is used to check the
first, that is a cross-check and not a verification, and it must be recorded as such.

**17. External reference.**

- **ISO/IEC — ISO/IEC 42001 *Artificial intelligence — Management system*.** Cited for the existence of an
  AI management-system standard covering oversight of AI outputs. Edition: 2023 per register; no clause
  asserted. Nature: Manual §6 category 3, international voluntary standard. Checked 2026-08-03 (EXT-021).
  Voluntary unless adopted.
- **ISO/IEC — ISO/IEC 23894 *Artificial intelligence — Guidance on risk management*.** Cited for the
  existence of guidance on AI risk. Edition: 2023 per register; guidance, not requirements; no clause
  asserted. Nature: Manual §6 category 3, international voluntary standard. Checked 2026-08-03 (EXT-024).
  Voluntary unless adopted.
- **NIST — *AI Risk Management Framework (AI RMF 1.0)*.** Cited for the existence of a voluntary framework
  addressing measurement and management of AI risk. Edition: 1.0, January 2023. Nature: voluntary
  framework from a national standards institute; classified as Manual §6 category 7, industry guidance. Checked 2026-08-03 (EXT-080). Expressly voluntary and
  non-regulatory.

**18. Jurisdictional caution.** Where AI is used in a context that applicable legislation classifies as
high-risk, or where a regulator imposes model-governance expectations on the entity, those requirements
apply in addition to this law and are matters for qualified local counsel and the entity's compliance
function.

**19. Related PCI Laws.** `PCI-FND-LAW-03` (independent verification) and `PCI-FND-LAW-14` (responsible AI)
govern. This law adds the recomputation method, the sampling rule and the named-approval requirement for
quantitative controls output. See also `PCI-PCL-LAW-13.01`, `PCI-PCL-LAW-13.03`, `PCI-PCL-LAW-13.04`,
`PCI-PCL-LAW-03.04`, `PCI-PCL-LAW-06.03`, `PCI-PCL-LAW-01.03`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 13 · KA 13.5 AI applied across the
project-controls lifecycle — forecasting & EVM/EAC, cost control & extraction; KA 13.3 Prompting and
working with generative AI — iterative refinement and verification. Also Domain 6 · KA 6.3 Forecasting
with EVM.

**21. Compliance test.** Compliance is demonstrated when, for each material AI-generated figure in the
deliverable, a retained verification record shows an independent recomputation, its result, and the
resolution of any difference; when a reviewer re-performing that recomputation from the retained inputs
obtains the issued figure; when classification output carries a recorded sample basis and error rate; and
when the deliverable names a human verifier and a human approver with dates. A verification record that
states only "reviewed" is a failure of this test. Two reviewers re-performing the recomputation from the same
retained inputs obtain the same figure — which is the point: an output nobody can independently reproduce
was never verified, whatever the record says.

**22. Breach indicators.** Verification records identical in wording across deliverables; verification
completed in less time than a recomputation takes; samples drawn only from tool-flagged items; differences
resolved by adopting the tool's figure with no reason; deliverables naming a tool as author; error rates
never recorded; forecasts that change materially when the model version changes.

**23. Consequence within PCI authority.** Correction required and the output withheld until verified;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of appeal.

**24. Examination application.** AI-verification case: the candidate is given an AI-generated estimate at
completion, its inputs and a deliberate error, and must recompute, identify the error and state the
verification record required. Calculation review: recomputing an index the tool reported.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-13-02 *AI Verification* and
PCL-LAW-13-03 *Human Sign-Off*; both identifiers are retired and are not reused. **Stage 9 amendment:**
element 10 required the verifier to be independent only of the tool's configuration, which permitted the
professional who produced an AI-generated figure to recompute it and record the recomputation as its
verification; element 10 now states that this law's recomputation is additional to, and does not
displace, the independent-person verification `PCI-FND-LAW-03` requires before reliance.

---

### PCI LAW PCI-PCL-LAW-13.03 — Verification of AI-Generated Schedule, Risk and Extraction Output

**1. Normative requirement.** A credential holder must verify an AI-generated schedule, risk or
document-extraction output by tracing it to the underlying network, register or source document before it
is used in a **project controls deliverable**.

**2. Purpose.** Controls the AI failure that recomputation cannot reach. A critical path, a risk
assessment, a delay narrative and a contract summary are not arithmetic; they cannot be checked by
recomputing a total, and their errors are structural — a dependency that does not exist, a risk whose
stated cause is not in the document, a summary that omits the proviso that reverses its meaning. These
outputs are also the most persuasive, because they arrive as prose.

**3. Scope.** All candidates and credential holders who produce, review, approve or give assurance over
AI-generated schedule analysis, critical-path or float analysis, delay narratives, risk identification or
assessment, contract and correspondence extraction, or narrative commentary used in controls deliverables,
on any project.

**4. Defined terms.** *verified*, *evidence*, *source record*, *material*, *material AI assistance*,
*independent*, *competent reviewer*, *open end*, *escalation threshold*.

**5. Required actions.** The professional must go to the underlying artefact — the network, the register,
the document — and confirm the output against it.

- **PCI-PCL-LAW-13.03-PR-01 — Schedule output traced through the network.** An AI-identified critical or
  longest path must be traced activity by activity through the retained schedule file, and the network
  re-run, before the path is reported; an AI-proposed logic change must be accepted only where the
  **planner** records that the dependency is real, under `PCI-PCL-LAW-10.01`.
- **PCI-PCL-LAW-13.03-PR-02 — Risk output traced to cause and register.** An AI-proposed risk, likelihood,
  impact or response must be confirmed by the named risk owner against the document or record the tool
  relied on, and must satisfy the three-part statement requirement of `PCI-PCL-LAW-12.01` before it is
  quantified.
- **PCI-PCL-LAW-13.03-PR-03 — Extraction confirmed against the source document.** Every AI-extracted term,
  date, quantity or obligation relied on must be confirmed by reading the provision in the source
  document, and the document and provision recorded; extraction from contracts and notices must be
  confirmed in full for material items rather than sampled.
- **PCI-PCL-LAW-13.03-PR-04 — Narrative supported or removed.** Every assertion in AI-generated commentary
  must be supported by a record the professional can produce, or be removed before issue; a generated
  sentence retained because it reads well and offends nobody is a breach of this process requirement.
- **PCI-PCL-LAW-13.03-PR-05 — Absence claims tested separately.** Where a tool reports that nothing was
  found — no open ends, no missing risks, no unmatched clauses — the professional must test that
  conclusion by an independent query or manual check, because a tool cannot evidence an absence.

**6. Prohibited actions.** Reporting an AI-identified critical path without tracing it; quantifying an
AI-proposed risk the owner has not confirmed; relying on an AI contract summary without reading the
provision; issuing generated narrative whose basis cannot be produced; treating an AI report of "no issues
found" as assurance; presenting an AI cross-check as an independent verification.

**7. Required evidence.** The trace record for schedule output naming the activities confirmed; the risk
owner's confirmation per accepted AI-proposed risk; the document and provision references for extractions;
the record of narrative assertions and their supporting records; the independent test of any absence claim;
the tool configuration record.

**8. Responsible role.** The **planner** for schedule output; the **risk lead** and the named risk owner
for risk output; the **commercial lead** for contract extraction; the **project controls lead** for
narrative issued in a controls deliverable.

**9. Approval authority.** The **project controls lead** approves release of the deliverable containing the
output; the **commercial lead** approves any commercial position derived from AI extraction, under
`PCI-PCL-LAW-07.01`.

**10. Independence requirement.** The verifier must be **independent** of the tool's configuration. Where
the output supports a claim, a defence or an entitlement, the verification must additionally be performed
by a person **independent** of the party the outcome favours, consistent with `PCI-PCL-LAW-10.02`.
**The tracing this law requires is additional to, and does not displace, the verification
`PCI-FND-LAW-03` requires** of a material output before any person relies on it; tracing performed by
the professional who produced the output satisfies this law's element 1 but does not satisfy that one.

**11. Materiality or threshold.** Tracing is required in full for every material output and for every
extraction that affects entitlement, a date, a rate or a quantity — sampling is not permitted for those,
because the consequence of a single wrong date or clause is not proportionate to its share of the
population. For non-material narrative and for bulk extraction that feeds no entitlement, verification may
be by sampling on a recorded basis. *Scaling:* on a USD 2 million refurbishment full tracing of material
items is a short exercise; on a USD 5 billion programme it is the reason material items must be identified
before extraction begins rather than after. The professional must record which items were treated as
material and why.

**12. Exception and waiver.** No exception is permitted to PR-03 for material items. Where verification of
non-material output cannot be completed before issue, the output must be labelled as unverified with the
scope stated, and must not support a decision until verified.

**13. Escalation trigger.** An AI-generated output that materially misstates a network, a register or a
document; discovery that a commercial or schedule position rests on an unverified extraction; a tool's
absence claim contradicted by an independent check; narrative issued whose basis cannot be produced when
requested.

**14. AI application.** AI may analyse networks, propose logic, identify risks, extract terms and dates,
summarise correspondence, draft narrative and prioritise items for human verification — its value in these
tasks is high, which is exactly why the verification method must be specified rather than assumed.

**15. AI prohibition.** AI must not determine a critical path relied on for a decision; must not own,
assess or close a risk; must not determine entitlement or the meaning of a contract provision; must not be
the sole basis of a narrative assertion; and must not be recorded as the verifier.

**16. AI verification.** Source tracing is the required method — activity-by-activity through the network,
entry-by-entry against the register, provision-by-provision in the source document — supplemented by
clause-to-summary comparison for extraction and by an independent query for any absence claim. Reading the
output and finding it plausible is expressly not verification, and a plausible output is the case this
requirement exists for.

**17. External reference.**

- **ISO/IEC — ISO/IEC 42001 *Artificial intelligence — Management system*.** Cited for the existence of an
  AI management-system standard covering human oversight of AI output. Edition: 2023; no clause asserted.
  Nature: Manual §6 category 3, international voluntary standard. Checked 2026-08-03 (EXT-021). Voluntary
  unless adopted.
- **ISO/IEC — ISO/IEC 23894 *Artificial intelligence — Guidance on risk management*.** Cited for guidance
  on AI-specific risk. Edition: 2023; guidance, not requirements. Nature: Manual §6 category 3,
  international voluntary standard. Checked 2026-08-03 (EXT-024). Voluntary unless adopted.
- **U.S. Government Accountability Office — *GAO Schedule Assessment Guide*.** Cited for published
  expectations on schedule quality against which an AI-generated schedule analysis can be tested. Edition:
  GAO-16-89G, 22 December 2015; no practice text reproduced. Nature: Manual §6 category 5, professional
  framework from a public audit institution; not a regulation. Checked 2026-08-03 (EXT-069). Persuasive
  only.

**18. Jurisdictional caution.** An AI-generated contract summary, delay narrative or entitlement assessment
is not legal advice and carries no professional privilege. Where the output touches entitlement, notice
validity or liability, qualified counsel in the governing jurisdiction is required, and local rules may
also govern the use of AI-generated material in proceedings.

**19. Related PCI Laws.** `PCI-FND-LAW-03` (independent verification) and `PCI-FND-LAW-14` (responsible AI)
govern. This law adds the tracing method matched to non-quantitative output and the rule on absence claims.
See also `PCI-PCL-LAW-13.01`, `PCI-PCL-LAW-13.02`, `PCI-PCL-LAW-10.01`, `PCI-PCL-LAW-10.02`,
`PCI-PCL-LAW-12.01`, `PCI-PCL-LAW-07.01`, `PCI-PCL-LAW-04.02`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 13 · KA 13.5 AI applied across the
project-controls lifecycle — scheduling, contracts & commercial, reporting & performance; KA 13.1
Strengths and hard limits. Also Domain 10 · KA 10.2 Network analysis and the Critical Path Method;
Domain 12 · KA 12.2 The risk process.

**21. Compliance test.** Compliance is demonstrated when: (a) for AI-generated schedule output, a trace
record names the activities on the reported path in sequence and a reviewer following that list through the
retained file arrives at the same path; (b) for AI-proposed risks, each accepted entry carries the named
owner's confirmation and satisfies the three-part test; (c) for each material extraction, the record names
the document and provision, and reading it supports the use made of it; (d) each narrative assertion has a
producible supporting record; and (e) every absence claim relied on has an independent test recorded. An
assertion whose supporting record cannot be produced is a failure of this test. Two reviewers repeating (a) and (c)
on the same artefacts reach the same result.

**22. Breach indicators.** Schedule analyses issued minutes after the file was produced; risk registers that
grow by dozens of entries in one cycle with no owner confirmations; contract summaries whose wording does
not appear in any contract; commentary that is fluent, general and unattributable; tool reports of "no
issues" accepted in consecutive periods; extraction errors found later in items that were never traced.

**23. Consequence within PCI authority.** Correction required and the output withheld until traced;
additional review; escalation; failure of the associated examination competency; ethics review;
certification investigation, suspension or withdrawal — each subject to due process and a right of appeal.

**24. Examination application.** AI-verification case: an AI-generated critical path that omits a driving
activity, and an AI contract summary that drops a condition precedent — the candidate must identify both
and state the verification each required. Ethical dilemma: issuing a generated narrative whose basis cannot
be produced.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-13-02 *AI Verification*; that identifier
is retired and is not reused. **Stage 9 amendment:** element 10 now states that the tracing this law
requires is additional to, and does not displace, the independent-person verification
`PCI-FND-LAW-03` requires before reliance.

---

### PCI LAW PCI-PCL-LAW-13.04 — Disclosure of AI Assistance in a Controls Deliverable

**1. Normative requirement.** A credential holder must disclose **material AI assistance** within the
deliverable it affected, identifying what the AI contributed.

**2. Purpose.** Controls the invisible contribution. A recipient who does not know that a figure, a
classification or a narrative was AI-generated cannot calibrate their reliance on it, cannot ask the right
question, and cannot know what to re-check if the tool is later found to be flawed. Disclosure at the
level of the organisation — "we use AI" — does none of that; disclosure must attach to the artefact so that
it travels with the number.

**3. Scope.** All candidates and credential holders who issue, approve or give assurance over a project
controls deliverable produced with AI assistance, on any project. It applies to internal and external
deliverables alike, and to assessment submissions to PCI.

**4. Defined terms.** *material AI assistance*, *AI tool*, *tool configuration record*, *project controls
deliverable*, *approved*, *decision owner*, *evidence*, *material*.

**5. Required actions.** The professional must say what the AI did, where, and what was done to check it.

- **PCI-PCL-LAW-13.04-PR-01 — Disclosure located in the deliverable.** The disclosure must appear in the
  deliverable itself — in its basis, assumptions or notes section — and not only in a policy, a footer or a
  separate register.
- **PCI-PCL-LAW-13.04-PR-02 — Content of the disclosure.** The disclosure must state which figures,
  analyses or sections the AI contributed to, the class of tool used, and the verification performed under
  `PCI-PCL-LAW-13.02` or `PCI-PCL-LAW-13.03`; naming the tool's vendor is optional, naming what it touched
  is not.
- **PCI-PCL-LAW-13.04-PR-03 — Accountability unaffected.** The disclosure must not qualify, share or
  transfer the professional's accountability for the deliverable, and must not be worded so as to suggest
  the reader should direct any question about the output to the tool or its supplier.
- **PCI-PCL-LAW-13.04-PR-04 — Disclosure to PCI in assessment.** Where AI assistance is used in work
  submitted to PCI for assessment, certification or continuing professional development, it must be
  disclosed to PCI on the terms PCI publishes for that assessment.

**6. Prohibited actions.** Issuing a deliverable with material AI assistance and no disclosure; disclosing
in general terms that give no indication of what the AI touched; wording a disclosure so as to shift
accountability; presenting AI-generated analysis as the professional's own reasoning; omitting disclosure
because verification was performed — verification is not a substitute for disclosure, and disclosure is not
a substitute for verification.

**7. Required evidence.** The deliverable containing the disclosure; the tool configuration record; the
verification records referenced by the disclosure; the assessment submission record where PR-04 applies.

**8. Responsible role.** The credential holder who issues the deliverable; the **project controls lead**
where the deliverable is issued in the project's name.

**9. Approval authority.** The **project controls lead** approves the deliverable including its disclosure.
Where the adopting organisation's governance prescribes disclosure wording, that wording applies, provided
it satisfies PR-02.

**10. Independence requirement.** Not applicable — disclosure is a statement of fact by the issuer about
their own deliverable, and there is nothing for an independent party to verify beyond the underlying
verification records, which are governed by `PCI-PCL-LAW-13.02` and `PCI-PCL-LAW-13.03`.

**11. Materiality or threshold.** Disclosure is required where the assistance is **material** as defined —
where removing the AI contribution would change a figure by more than the materiality rule applied, or
would change a recommendation, a classification affecting entitlement or coding, or a stated conclusion.
Routine use that does not meet that test — spell-checking, formatting, transcription, template population —
need not be disclosed, and requiring it would produce a disclosure on every document and inform nobody.
*Scaling:* the test is identical on a USD 2 million refurbishment and a USD 5 billion programme because it
is expressed against the deliverable's own materiality rule, not against a project value.

**12. Exception and waiver.** No exception is permitted to PR-04. An exception to PR-01 may be approved by
the **decision owner** where the deliverable's format genuinely cannot carry a disclosure — a system-
generated data feed, for example — provided the disclosure is made in the accompanying document and the
arrangement is recorded.

**13. Escalation trigger.** A material AI contribution issued without disclosure; a disclosure worded to
transfer accountability; discovery that a recipient took a decision on an AI-assisted figure they believed
to be human-derived; undisclosed AI assistance in work submitted to PCI.

**14. AI application.** AI may draft the disclosure text and maintain the register of which deliverables
carried AI assistance.

**15. AI prohibition.** AI must not decide whether its own contribution was material, must not author a
disclosure that is issued without human review, and must not be presented as the accountable author of any
deliverable.

**16. AI verification.** Named human judgement recorded with reasoning: the professional must record, for
each deliverable, whether the AI contribution met the materiality test and why, and must confirm that the
disclosure names the figures or sections actually affected — a disclosure that is accurate about the tool
and vague about the artefact fails this test.

**17. External reference.**

- **OECD — *Recommendation of the Council on Artificial Intelligence* (the OECD AI Principles),
  OECD/LEGAL/0449.** Cited for internationally agreed principles including transparency about AI use.
  Adopted 2019; revised May 2024, per the register. Nature: Manual §6 category 7, industry guidance in the
  Manual's vocabulary — an OECD Council **Recommendation**, and expressly **not legislation**, binding on
  no one, including adherents. Checked 2026-08-03 (register EXT-081). Applicability: none of its own; it
  reaches a practitioner only through instruments a jurisdiction or an organisation adopts.
- **ISO/IEC — ISO/IEC 42001 *Artificial intelligence — Management system*.** Cited for the existence of a
  management-system standard addressing transparency of AI use. Edition: 2023; no clause asserted. Nature:
  Manual §6 category 3, international voluntary standard. Checked 2026-08-03 (EXT-021). Voluntary unless
  adopted.

**18. Jurisdictional caution.** Disclosure obligations concerning AI use are emerging and differ by
jurisdiction and by sector; in the European Union the AI Act imposes transparency obligations on certain
actors and uses. Whether a legal disclosure obligation applies to a given deliverable is a question for
qualified local counsel, and satisfying this law does not satisfy such an obligation.

**19. Related PCI Laws.** `PCI-FND-LAW-14` (responsible AI) and `PCI-FND-LAW-01` (professional
accountability) govern. This law adds the requirement that disclosure attach to the artefact and name what
the AI touched — the foundational duty requires disclosure, not its location or its specificity. See also
`PCI-PCL-LAW-13.01`, `PCI-PCL-LAW-13.02`, `PCI-PCL-LAW-13.03`, `PCI-PCL-LAW-11.01`.

**20. Related Body of Knowledge content.** PCL-AI · Domain 13 · KA 13.6 Governance, ethics, risk &
assurance of AI. Also KA 13.7 Building an AI-augmented project-controls capability; Domain 4 · KA 4.3
Management reporting.

**21. Compliance test.** Compliance is demonstrated when, for each deliverable with material AI assistance,
the deliverable itself contains a disclosure naming the figures, analyses or sections the AI contributed
to, the class of tool, and the verification performed; and when the tool configuration record and
verification records referenced can be produced. A deliverable whose AI contribution is established from
the configuration records but which carries no disclosure is a failure of this test; so is a disclosure that names no
figure or section. Two reviewers comparing the disclosure against the configuration and verification
records reach the same conclusion.

**22. Breach indicators.** Identical disclosure wording on every deliverable regardless of the assistance
given; disclosures that name a tool but no artefact; deliverables with configuration records and no
disclosure; disclosure language that invites the reader to treat the output as the tool's responsibility;
AI-assisted assessment submissions with no declaration to PCI.

**23. Consequence within PCI authority.** Correction required and the deliverable reissued with disclosure;
output withheld; additional review; escalation; failure of the associated examination competency; ethics
review; certification investigation, suspension or withdrawal — each subject to due process and a right of
appeal. Undisclosed AI assistance in work submitted to PCI for assessment is treated under PCI's assessment
and conduct processes.

**24. Examination application.** Ethical dilemma: a report whose forecast section was AI-generated and
verified, issued with no disclosure, and a recipient who later asks how the figure was produced.
AI-verification case: judging which of four disclosure statements satisfies PR-02.

**25. Version and status.** Version 2.0 · Charter §5 stage reached: 3 · Approval date: not yet
approved · Effective: on approval · Partially supersedes PCL-LAW-13-04 *Professional Accountability*; that
identifier is retired and is not reused.

---
## Audit-question findings

The Manual §9 questions were worked across the whole set before issue. The table records, for each
question, the laws it changed and what changed — not merely that the question was asked. Findings that
produced no change say so, and say why.

| # | Question | Laws affected | What changed |
|---|---|---|---|
| 1 | What exact failure does this law prevent? | All 33 | Element 2 was rewritten in every law to name an observed professional failure and its mechanism, replacing the virtue statements of the superseded set ("to ensure integrity"). Three drafts that could not name a failure — a general "data quality" law, a general "professional judgement" law and a general "governance" law — were dropped rather than published. |
| 2 | Mandatory or only recommended? | All 33 | Every obligation is `must` or `must not`. No `should` carries an obligation anywhere in the set, and no Recommended Practices are issued in this edition, so nothing mandatory can hide at Charter Level 5. |
| 3 | Can a professional know whether it applies? | All 33 | Element 3 now names the *acts* governed (prepare · review · approve · give assurance) and the artefacts, rather than a job family. `PCI-PCL-LAW-13.01` was amended to state expressly that an embedded AI feature is in scope, after a reviewer read the draft as covering standalone tools only. |
| 4 | Is the responsible person identifiable? | All 33 | A defined role vocabulary was added to the Definitions, each role defined by function rather than title, with the rule that a person holding two roles on a small project never removes an independence requirement. Every element 8 names a role from that list; "the team" and "management" appear nowhere. |
| 5 | Is the required action observable? | All 33 | 145 process requirements were created, each stating an act that leaves a record. Bundled obligations in the superseded set (for example one clause requiring identification, assessment, approval and baseline update of change) were split across `PCI-PCL-LAW-05.02`, `05.03` and `05.04`. |
| 6 | Is compliance provable? | All 33 | Element 21 was written as a performable test in every law — the element the superseded eighteen-field set lacked entirely. Each test states the population, the method and what constitutes a failure, and closes with the condition that two reviewers applying it reach the same answer. **Stage 9 correction:** twenty-eight of those tests originally closed by saying that the defective condition "is an exception", in the audit sense of a finding. In this corpus *exception* is a Charter §8 term meaning an **approved departure**, and Charter §8 states expressly that an undocumented departure is a breach and not an exception. Every one of the twenty-eight now reads "is a failure of this test". |
| 7 | Is the required evidence proportionate? | 01.01 · 01.02 · 05.01 · 06.02 · 10.03 · 13.02 | Evidence was limited to records the process already produces. Four drafting proposals were removed as disproportionate: a standing log of every cost-code decision, a signed certificate per progress claim, a full re-performance of every AI output, and a separate schedule-quality report per issue. Sampling on a recorded basis replaced them, with full testing retained only where a single item carries the consequence — dates, contract terms, longest-path activities. |
| 8 | Can the law be audited? | All 33 | Every element 21 names what the auditor examines and where it is retained, and `PCI-PCL-LAW-11.01` was added to make retention itself an obligation, because the other 32 tests assume the records still exist. |
| 9 | Can the law be examined through a scenario? | All 33 | Element 24 names the item type — scenario judgement, evidence selection, calculation review, escalation decision, ethical dilemma or AI-verification case. No law is examinable only by recalling its number. |
| 10 | Can a professional technically comply while defeating its purpose? | 03.02 · 05.02 · 05.04 · 06.02 · 10.01 · 10.03 · 12.02 · 13.02 · 13.03 | **The most productive question in the audit.** Eight specific defeats were identified and closed by adding prohibitions or process requirements: adding meaningless logic to clear an open-end count (`10.01-PR-04`); splitting a change to stay below an approval band (`05.02` element 6; `05.04-PR-05`); assembling a material baseline edit from immaterial ones (`03.02` element 11 removes any threshold); claiming progress within a verification sample's blind spot (`06.02-PR-02` requires the highest-value and most-moved claims to be in the sample); leaving remaining durations to fall by elapsed time (`10.03-PR-02`); choosing a confidence level after seeing the answer (`12.02-PR-01` requires the levels either side to be shown); drawing the verification sample only from items the tool flagged (`13.02-PR-03`); and accepting "no issues found" from a tool as assurance (`13.03-PR-05`). |
| 11 | Does it conflict with another PCI law? | 03.02 ↔ 10.03 · 04.01 ↔ 11.01 · 05.04 ↔ 12.03 · 03.04 ↔ 06.04 | Four overlaps were found and scoped rather than left to interpretation. Schedule baseline protection sits in `03.02-PR-04` and is cross-referred from `10.03-PR-04` rather than restated. `04.01` governs reconciliation at issue; `11.01` governs reproduction afterwards. Change funding sits in `05.04`; contingency drawdown in `12.03`, with an express prohibition on using one to do the other. Forecast completeness sits in `03.04`; method disclosure in `06.04`, reconciled by `06.04-PR-03`. |
| 12 | Does it duplicate an external standard unnecessarily? | 12.01 · 12.02 · 03.02 · 06.01 · 06.02 · 06.03 | ISO 31000's risk process is **not** restated: the two risk laws impose statement quality, ownership and disclosure obligations the standard does not contain, and say expressly that they do not require conformity with it. The earned value laws cite ANSI/EIA-748 for the existence of management-system expectations only and impose PCI's own tests. All citations of the IFRS *Conceptual Framework* carried by the superseded set were **removed**: Manual §6 forbids sourcing a requirement to it, and it was doing no work. |
| 13 | Does it misrepresent external authority? | 03.02 · 06.01 · 06.02 · 06.03 · 13.01 · 13.02 · 13.04 · 10.01 | ANSI/EIA-748 is now described as a **national standard binding only where a contract or procurement regime imports it**, with its **edition and guideline count deliberately not asserted** — the count changed at the most recent revision, and the superseded set's silence on it was correct and is preserved. Manual §6 has since added a *national standard* category (11) and a *supervisory guidance* category (12); ANSI/EIA-748 is now classified under category 11, and the NIST AI RMF under category 7 with its voluntary status and national origin stated at each use. The EU AI Act and the GDPR were moved out of element 17 into element 18, because the corpus uses them as jurisdictional cautions and not as authority. The OECD AI Principles are marked expressly as a Council Recommendation and not legislation. No clause number, article or judicial decision is asserted anywhere in the set. |
| 14 | Does it require legal or jurisdiction-specific advice? | All 33, materially 01.01 · 01.02 · 05.02 · 07.01 · 07.02 · 07.03 · 10.02 · 11.01 · 12.02 · 13.01 | Element 18 in every law. `07.01-PR-03` was added to make the competence boundary an obligation rather than a caution: a controls professional must refer questions of contractual meaning and entitlement and must not state a legal conclusion. `11.01` element 18 records that a retention obligation never overrides a legal duty to delete personal data. |
| 15 | Does it define the relevant materiality threshold? | All 33 | Element 11 in every law, and a single defined **materiality rule** in the Definitions that is configurable by the adopting organisation's governance and, failing that, recorded by the professional. **No percentage is invented anywhere in the set.** Where a number would have been arbitrary — the contingency confidence level, the near-critical float range, the variance reporting basis — the law requires the basis and the decision-maker to be recorded instead. |
| 16 | Does it cover AI use? | All 33 | Elements 14, 15 and 16 appear in every law, not only in Domain 13, and element 16 names a method in each — recomputation, source tracing, reconciliation, sampling on a stated basis, boundary testing, sensitivity analysis, clause-to-summary comparison or named human judgement recorded with reasoning. "Review the AI output" appears nowhere. |
| 17 | Does it preserve human accountability? | All 33, materially 13.02 · 13.03 · 13.04 | Element 15 in every law prohibits AI from approving, certifying or deciding. `13.02-PR-04` requires a named human verifier and a named human approver on the deliverable. `13.04-PR-03` prohibits disclosure wording that shifts accountability to a tool or its supplier. |
| 18 | Does it contain an exception process? | All 33 | Element 12 in every law. Twelve laws permit a bounded exception with an approver, a justification, a duration and a compensating control; the remainder state that no exception is permitted, and several permit an exception to one process requirement while refusing one to the principal obligation. |
| 19 | Does it define escalation? | All 33 | Element 13 in every law names the triggering event, and the Definitions fix **escalation threshold** so that the duty does not depend on the professional's expectation of the recipient's reaction. |
| 20 | Is every important term defined? | All 33 | A Definitions section defines the fourteen terms the brief requires plus twelve subject-matter terms, four AI terms and nine roles. Definitions are non-circular and each states a test a reader can apply. Undefined judgement words carrying obligations — *appropriate*, *adequate*, *reasonable*, *timely*, *sufficient* — were removed and replaced with stated tests. |
| 21 | Is the language concrete and modern? | All 33 | The legislative requirement verb prohibited by Manual §1 was eliminated. It had entered the superseded set through an earlier red-team pass; this set contains **zero** occurrences of it, in any field, and the ISO mapping in *How to read these laws* is written so that the convention is explained without using the word. |
| 22 | Does it impose an impossible or excessive burden? | 06.02 · 10.01 · 10.03 · 12.02 · 13.02 · 13.03 | Six burdens were reduced. Schedule integrity is tested by a **query** rather than a manual review, so it costs the same at any scale. Progress and classification verification are sampled on a recorded basis. Contingency quantification expressly accepts a documented expected-value build-up where a probabilistic model would cost more than the contingency it sizes. AI verification is full only for material figures and for items where one error carries the whole consequence. One proposed law — a standing independent quality review of every controls deliverable — was dropped as disproportionate to any failure it would prevent. |
| 23 | Can it operate on both small projects and megaprojects? | All 33 | Every element 11 carries an explicit scaling paragraph tested against a USD 2 million refurbishment and a USD 5 billion programme. Three laws changed as a result: `03.05` and `05.04` now provide that independence and change authority are obtained **from outside the project** where the project cannot supply them, so a two-person team is not placed in automatic breach; `04.02` records that a percentage-only variance rule generates noise on a small project and nothing at all on a megaproject, and requires the basis chosen to be recorded; and `10.02` fixes the obligation to record the basis of the near-critical range rather than fixing the range, because it genuinely must differ between the two. |
| 24 | Can it operate internationally? | All 33 | No law depends on a single jurisdiction's requirements. Accounting treatment, tax, contractual entitlement, payment legislation, records retention and data protection are all left to element 18 and to local advisers. External instruments are cited with their applicability limitation stated at each use. |
| 25 | Is there a clear consequence within PCI's authority? | All 33 | Element 23 draws only on the Charter §9 list and states in each law that PCI can impose no fine, no civil or criminal liability and no other consequence. Every consequence is subject to due process and a right of appeal. |

### How this edition was produced — Charter §5 record

Charter §5 requires a law's file to record honestly which stages were performed and by whom, including
where a stage was performed with AI assistance rather than by a named human. For this edition:

- **Stages 1–3** (problem definition, drafting instruction, initial draft in the mandatory structure)
  were performed **with AI assistance**, from the superseded twenty-law set, the PCL-AI Body of Knowledge
  and the suite external-reference register, under human direction.
- **Stage 9** (red-team challenge) was performed in part, as the audit table above records — the question
  10 findings are its principal product. A **second red-team pass** was subsequently run across the whole
  four-file corpus; its findings and their disposition are recorded in
  [`LAW_RED_TEAM_REPORT.md`](LAW_RED_TEAM_REPORT.md), and the amendments it produced in this file are
  noted in the element 25 of each law changed and in the question 6 row above.
- **Definitions reconciliation.** The red team's structural finding **P-1** — that no PCI Law
  Definitions Register existed, so each volume built its own and seven compliance-deciding terms
  diverged — has since been closed. The register is published at
  [`PCI_LAW_DEFINITIONS_REGISTER.md`](PCI_LAW_DEFINITIONS_REGISTER.md), and §A above was reconciled to
  it. In this file that changed the wording of *material*, *independent*, *verified*, *current*,
  *competent reviewer*, *decision owner*, *evidence*, *approved*, *escalation threshold* and *material
  AI assistance*; **no obligation changed**, because each now states in one place the reading the
  wider-obligation rule at the head of these Definitions already produced. Two substantive corrections
  are worth naming. First, *competent reviewer* no longer folds independence into competence — where a
  law requires an independent reviewer, its element 10 says so and is tested separately, which is what
  makes `PCI-FND-LAW-10` element 12's supervised-acquisition exception usable again. Second, a
  **circular definition** was removed: *evidence* was defined by reference to a *competent reviewer*,
  *competent reviewer* by the ability to perform *the verification method*, and *verified* by
  application of a method to *evidence* — a closed loop in which none of the three could be applied
  without one of the others. *Evidence* is now defined by properties of the record itself.
- **Stages 4 to 8 and 10 to 13** — technical review, standards and legal-characterisation review,
  practitioner consultation, impact assessment, scenario testing, revision, approval, publication and
  post-implementation review — **have not been performed.** This set is therefore a draft for approval and
  not an approved law set, and every law says so in its element 25.
- No external body has reviewed, approved, endorsed or accredited this set.

---

## Index of PCL-AI Professional Laws

External-reference categories are Manual §6 numbers: **2** authoritative financial-reporting standard ·
**3** international voluntary standard · **4** contract framework · **5** professional framework ·
**7** industry guidance. A national standard is classified under category **11**, added to the Manual for exactly this case
in the law and in audit finding 13. "—" means the law cites no external authority and says why.

| ID | Official title | Anchor domain | Principal obligation | Ext. ref. categories |
|---|---|---|---|---|
| `PCI-PCL-LAW-01.01` | Cost Cut-Off Integrity | 01 — Foundations of Accounting for Project Controls | Record each cost in the period its underlying work or receipt falls in, by a written cut-off rule. | 2, 5 |
| `PCI-PCL-LAW-01.02` | Accrual Completeness and Basis | 01 — Foundations of Accounting | Accrue everything received or performed and not invoiced at the cut-off, from evidence. | 2 |
| `PCI-PCL-LAW-01.03` | Cost Classification and Cost-Code Integrity | 01 — Foundations of Accounting | Code each cost to the work it was incurred on, never to where budget remains. | 5, 7 |
| `PCI-PCL-LAW-03.01` | Scope Completeness of the Performance Measurement Baseline | 03 — Budgeting & Forecasting | Put all authorised scope, and only authorised scope, in the baseline. | 5 |
| `PCI-PCL-LAW-03.02` | Baseline Approval, Version Control and the Change Prohibition | 03 — Budgeting & Forecasting | Do not alter an approved baseline except by approved change or authorised re-baseline. | 3 |
| `PCI-PCL-LAW-03.03` | Authority to Re-baseline | 03 — Budgeting & Forecasting | Do not measure against a re-baseline until the approval authority has approved it in writing. | 3 |
| `PCI-PCL-LAW-03.04` | Completeness of the Estimate at Completion | 03 — Budgeting & Forecasting | Include every known cost effect on the remaining work in the forecast. | 5 |
| `PCI-PCL-LAW-03.05` | Independent Challenge and Approval of the Forecast | 03 — Budgeting & Forecasting | Do not issue a forecast for a decision until an independent person has challenged it on the record. | — |
| `PCI-PCL-LAW-04.01` | Reconciliation of the Performance Report to Source Records | 04 — Performance, Variance & Reporting | Do not issue a report whose figures do not reconcile to their source records at a stated cut-off. | 5 |
| `PCI-PCL-LAW-04.02` | Explanation of Material Variance | 04 — Performance, Variance & Reporting | Explain each material variance by a cause evidenced from a source record. | 5 |
| `PCI-PCL-LAW-04.03` | Correction and Restatement of a Reported Error | 04 — Performance, Variance & Reporting | Restate a material reported error to every recipient of the original. | — |
| `PCI-PCL-LAW-05.01` | Completeness and Reconciliation of the Recorded Cost Position | 05 — Cost Management & Control | Reconcile actual cost to the books of account and to commitments and accruals every period. | 5 |
| `PCI-PCL-LAW-05.02` | Identification and Registration of Change | 05 — Cost Management & Control | Register every potential change in the cycle it becomes known, whatever its merit. | 4 |
| `PCI-PCL-LAW-05.03` | Completeness of Change Impact Assessment | 05 — Cost Management & Control | Assess a change's full cost, schedule, risk and interface effect before it goes for approval. | 4, 5 |
| `PCI-PCL-LAW-05.04` | Change Authority and Segregation of Preparation from Approval | 05 — Cost Management & Control | Do not approve a change you prepared, assessed or priced. | 3, 5 |
| `PCI-PCL-LAW-06.01` | Earned Value Measurement Rules Fixed Before Performance | 06 — EVM & Forecasting | Fix and record each work package's measurement method before its work begins. | 3, 5 |
| `PCI-PCL-LAW-06.02` | Objective Evidence of Progress | 06 — EVM & Forecasting | Do not report progress that objective evidence does not support at the cut-off. | 3 |
| `PCI-PCL-LAW-06.03` | Coherence of the Three Earned Value Data Points | 06 — EVM & Forecasting | Measure planned value, earned value and actual cost over the same scope, period and cut-off. | 3 |
| `PCI-PCL-LAW-06.04` | Selection and Disclosure of the Estimate-at-Completion Method | 06 — EVM & Forecasting | Disclose the method behind every earned-value-derived estimate at completion and why it suits the work. | 5 |
| `PCI-PCL-LAW-07.01` | Contract Source Verification | 07 — Contracts & Commercial | Verify every contractual term relied on against the executed contract and its executed amendments. | 4 |
| `PCI-PCL-LAW-07.02` | Traceability of Variations and Claims | 07 — Contracts & Commercial | Keep a traceable record from originating event to current commercial position for every variation and claim. | 4, 5 |
| `PCI-PCL-LAW-07.03` | Support and Reconciliation of Applications for Payment | 07 — Contracts & Commercial | Do not submit, certify or support payment for work not evidenced as performed or delivered. | 2, 4 |
| `PCI-PCL-LAW-10.01` | Schedule Network Integrity | 10 — Project Scheduling | Do not issue a schedule containing an open end. | 5, 7 |
| `PCI-PCL-LAW-10.02` | Critical-Path Verification Before Reliance | 10 — Project Scheduling | Verify that the reported critical path is the network's true longest path before relying on it. | 5 |
| `PCI-PCL-LAW-10.03` | Status Date and Actual-Date Integrity | 10 — Project Scheduling | Status to one stated date, using actual dates taken from records of what happened. | 3, 5 |
| `PCI-PCL-LAW-11.01` | Reproducibility of the Reported Controls Position | 11 — Process Cycles & Control Environment | Retain, with each deliverable, the records that let an independent competent reviewer reproduce every figure. | 3, 5 |
| `PCI-PCL-LAW-12.01` | Risk Statement Quality and Named Ownership | 12 — Risk Management | Record each risk as cause, uncertain event and effect, with one named individual accountable. | 3, 5 |
| `PCI-PCL-LAW-12.02` | Basis and Confidence Level of Quantified Contingency | 12 — Risk Management | State the method, the coverage and the confidence level behind every contingency figure. | 3, 5 |
| `PCI-PCL-LAW-12.03` | Contingency Drawdown Authority and Re-assessment of Remaining Exposure | 12 — Risk Management | Do not apply contingency without the recorded approval of the authority holding it. | 3, 5 |
| `PCI-PCL-LAW-13.01` | Approved Tools, Recorded Configuration and Protected Project Data | 13 — AI for Project Controls | Do not use an AI tool on project data outside the approved tool-and-data boundary. | 3, 7 |
| `PCI-PCL-LAW-13.02` | Verification of AI-Generated Quantitative Controls Output | 13 — AI for Project Controls | Independently recompute an AI-generated quantitative figure before it is used. | 3, 7 |
| `PCI-PCL-LAW-13.03` | Verification of AI-Generated Schedule, Risk and Extraction Output | 13 — AI for Project Controls | Trace AI-generated schedule, risk and extraction output to the network, register or source document before use. | 3, 5 |
| `PCI-PCL-LAW-13.04` | Disclosure of AI Assistance in a Controls Deliverable | 13 — AI for Project Controls | Disclose material AI assistance inside the deliverable it affected, naming what the AI contributed. | 3, 7 |

**Thirty-three laws · one hundred and forty-five process requirements · ten of the thirteen PCL-AI
domains carrying an anchored law.** Domains 2, 8 and 9 carry no law of their own in this
edition: their subject matter is reached through the laws anchored elsewhere and listed in element 20 —
financial reporting through `01.01`, `01.02` and `07.03`; the project lifecycle through `03.01`, `05.02`
and `10.03`; adaptive delivery through `06.01`, whose measurement rules apply to accepted increments.
That is a deliberate choice recorded here so that a reader does not read the gaps as an oversight.

> **AI proposes; the professional verifies, decides and remains accountable.**
