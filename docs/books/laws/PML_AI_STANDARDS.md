# PML-AI Professional Standards — PCI Project Management Leader – AI

**Status:** Certification Standard set for the **PML-AI** credential (PCI Project Management Leader – AI).
Version 2.0, drafted under the **PCI Standards Charter** (what a standard is) and the **PCI Standard
Drafting Manual** (how a standard is written). A standard that does not conform to both does not pass gate.
**Thirty-two standards and one hundred and forty-eight process requirements**, anchored across all sixteen
domains of the PML-AI Body of Knowledge (`../pml-ai/`).

> **PCI Standards are private professional certification requirements established by Project
> Controls Institute Global. They are not legislation, government regulation, legal advice or
> substitutes for applicable laws, contractual obligations, regulatory requirements or authoritative
> professional standards. Where an applicable legal, regulatory, contractual or authoritative
> requirement imposes a higher or different obligation, that requirement prevails.**

---

## How to read these standards

### The two governing instruments

The **Charter** governs status, hierarchy, priority, due process, interpretation, amendment,
exceptions and consequence. The **Drafting Manual** governs normative language, one-obligation
clauses, identifiers, defined terms, the twenty-five-element structure, thresholds, external-reference
classification, prohibited patterns and the twenty-five audit questions. Where any doubt arises, those
two documents prevail over anything below.

### Identifiers

Each standard is cited as **`PCI-PML-STD-DD.NN`**, where `DD` is the two-digit Body-of-Knowledge domain of
primary anchorage and `NN` a two-digit sequence within that domain. Each process requirement is cited
as **`PCI-PML-STD-DD.NN-PR-NN`**. Identifiers are stable and are the only citation form; page numbers
are never used, because pagination changes. A withdrawn identifier is never reused.

**This edition renumbers the whole set.** The previous edition used `PML-LAW-DD-NN`. Element 25 of
each standard records the identifier it supersedes, so a citation made under the old scheme can be
resolved. The old `PML-LAW-01-02` carried the title *Human Decision Authority in Delivery*, which
collided with the foundational standard **Human Decision Authority**; it is republished here as
`PCI-PML-STD-01.02` under the distinguishing title **Reserved Delivery Decisions and the Named
Human Decider**, and its obligation is narrowed to what a delivery leader must do that the
foundational standard does not already require.

### The normative language, and the ISO mapping

PCI uses **modern must-drafting**, exclusively.

| Word | Meaning in a PCI Standard |
|---|---|
| **must** | Mandatory PCI professional requirement |
| **must not** | Prohibited practice; a breach |
| **should** | Recommended practice; a justified alternative may be acceptable |
| **may** | Permission |
| **can** | Capability or possibility — never permission |

**The ISO mapping, stated for readers who work to ISO/IEC drafting conventions.** In an ISO or IEC
deliverable, requirements are expressed with the auxiliary verb that ISO/IEC Directives Part 2
reserves for requirements, and a reader trained on those conventions may misread **must** as an
external constraint rather than as the requirement itself. In a PCI Standard, **must** *is* the
requirement form, and it occupies exactly the place that ISO/IEC drafting gives to its requirement
verb. That verb is deliberately not printed anywhere in this file, in any field, including in
quotations of PCI's own earlier drafts, because it has been read as both obligation and futurity and
PCI's convention excludes it. A draft containing it fails gate. This file was mechanically checked
for it before publication; the check is recorded in the audit table.

**Consequence for the reader of the previous edition:** every rule in the 2026 v1.0 set that was
drafted with that verb has been re-expressed here with **must**. No obligation was weakened in the
translation; several were strengthened, and each such change is noted in element 25 of the standard
concerned.

### One obligation per requirement

Element 1 of every standard states **one** principal obligation. Everything else the standard requires appears
as a numbered **process requirement** under element 5. Process requirements are Level 4 instruments
in the Charter's hierarchy: **they are mandatory**, they are breached independently of one another,
and they are cited independently. Nothing in a note, a rationale, a bracket or a commentary sentence
in this file creates an obligation; if such a sentence reads as one, that is a drafting defect and
the Interpretation Panel corrects it.

### The relationship to the Foundational Standards

Every standard below sits under the **PCI Foundational Standards**, which bind every credential holder in all
three credentials. This file cites them in the Charter section 3 identifier form `PCI-FND-STD-NN`. The
foundational set is a **fifteen-standard** set, and its subjects are listed below. This is the same
concordance the rebuilt PCL-AI Certification Standard set uses, so that `PCI-FND-STD-NN` denotes the same
standard in every credential's standard file:

| ID | Subject | ID | Subject |
|---|---|---|---|
| `PCI-FND-STD-01` | Professional accountability | `PCI-FND-STD-09` | Confidentiality and approved technology |
| `PCI-FND-STD-02` | Evidence before assertion | `PCI-FND-STD-10` | Competence and limitation |
| `PCI-FND-STD-03` | Independent verification | `PCI-FND-STD-11` | Escalation of material misstatement |
| `PCI-FND-STD-04` | Human decision authority | `PCI-FND-STD-12` | Record integrity |
| `PCI-FND-STD-05` | Transparent assumptions | `PCI-FND-STD-13` | No silent override |
| `PCI-FND-STD-06` | Source and version integrity | `PCI-FND-STD-14` | Responsible AI |
| `PCI-FND-STD-07` | Data lineage | `PCI-FND-STD-15` | Correction duty |
| `PCI-FND-STD-08` | Conflict disclosure | | |

**The published foundational file carries these fifteen standards under these identifiers.** Every
`PCI-FND-STD-NN` citation below therefore resolves directly against
[`PCI_FOUNDATIONAL_STANDARDS.md`](PCI_FOUNDATIONAL_STANDARDS.md), and the subject it carries there is the
subject intended here. The superseded `PCI-LAW-F-NN` identifiers are recorded, for historical
traceability only, in [`STANDARDS_CONCORDANCE.md`](STANDARDS_CONCORDANCE.md); no live citation uses them.

**A certification standard must add.** Where a standard below governs the same subject as a foundational standard, it
states what leading projects, programmes and portfolios specifically requires that the foundational
standard does not — a named artefact, a named authority, a named record, a named test. A certification standard
that only restates its parent is a defect and is listed as such in the audit table.

### External references

Every external reference is classified as exactly **one** Drafting Manual section 6 category, and the
categories are never combined:

| # | Category | Used in this file for |
|---|---|---|
| 1 | Applicable legislation or regulation | *(not used — applicability is unknown for a global readership)* |
| 2 | Authoritative financial-reporting standard | *(not used — this credential states no accounting treatment)* |
| 3 | International voluntary standard | ISO, IEC and ISO/IEC deliverables |
| 4 | Contract framework | FIDIC, NEC |
| 5 | Professional framework | PMBOK Guide, AACE material, the Scrum Guide |
| 6 | Ethical code | PMI Code of Ethics and Professional Conduct |
| 7 | Industry guidance | *(not used)* |
| 8 | Voluntary environmental or social framework | *(not used)* |
| 9 | PCI internal professional standard | the Foundational Standards and this set |
| 10 | Illustrative practice | the EU AI Act, the GDPR, the NIST AI RMF, the OECD AI Principles, the G20/OECD Principles of Corporate Governance |

Four rules govern every entry, and each of them has been breached once somewhere in this corpus:

- **ISO 21500, ISO 21502, ISO 21503, ISO 21504, ISO 21505, ISO 31000, ISO 10006 and ISO 9000 are
  voluntary international standards, and each of them is guidance.** Nothing can be certified against
  any of them. **ISO 9001, ISO/IEC 42001, ISO/IEC 27001 and ISO 45001 are certifiable requirements
  standards**, and are described as such where they appear. **ISO 45003 is guidance, not a
  requirements standard**, and nothing can be certified against it — a point that matters in
  `PCI-PML-STD-12.02`, where the temptation to dress guidance as a compliance obligation is greatest.
- **The Scrum Guide is a voluntary framework. Adoption is the whole of its force.** It binds a team
  that has adopted it and nobody else, and it is not a standard.
- **The PMBOK Guide and AACE material are professional frameworks, never regulatory authority.**
- **The EU AI Act and the GDPR are real legislation binding within their own jurisdictions.** They are
  named here to illustrate a regulatory shape and are relied on for no requirement in this set; the
  applicable position for any given project is a question for qualified local counsel.

**No clause number, article, edition or effective date is asserted in this file.** Editions live in
the suite External-Reference Register (`../registries/EXTERNAL_AUTHORITIES.md`), dated, with their
verification status; each element 17 entry gives the register reference (`EXT-0NN`) and the date the
currency was checked. Where the register records an entry as not independently verified, the standard says
so. **A reader relying on any instrument named below must obtain the current edition from its
publisher.** Nothing here is reproduced from any of them.

**The two taxonomies.** The suite register uses its own seven-category scheme; the Drafting Manual
uses ten. Where the two differ — most visibly for the Scrum Guide, which the register calls a
voluntary framework and the Manual's scheme reaches through category 5 — **Manual section 6 governs this
file**, and the register reference is given so a reader can see both.

### No endorsement

Naming an external instrument means only that the instrument exists and bears on the topic under
discussion. No standards
body, professional institute, government, supervisory authority or employer has reviewed, approved,
endorsed or accredited these standards, the PML-AI credential or PCI Global.

### British English and the volume's forms

*Organisation, organisational, realisation, prioritisation, judgement, programme.* Spelling follows
the suite editorial charter (`../registries/TERMINOLOGY.md`).

### The due-process record, stated honestly

Charter section 5 requires thirteen stages and requires the file to record which were performed and by whom,
**including where a stage was performed with AI assistance rather than by a named human.** For this
edition:

| Stage | Performed | By |
|---|---|---|
| 1 Problem definition · 2 Drafting instruction | Yes | Named human commissioner; the drafting brief is the reconstruction specification for this edition |
| 3 Initial draft | Yes | AI-assisted drafting against the Manual's structure |
| 5 Standards and legal-characterisation review | Partial | Checked against the suite External-Reference Register; **no external publisher was contacted for this edition** |
| 8 Scenario testing | Partial | Every standard tested against a six-person internal project and a multi-partner national programme (element 11); other scenario classes untested |
| 9 Red-team challenge | Partial | The technical-compliance and unverifiable-requirement classes were worked; the full red-team is outstanding |
| 10 Revision | Yes | Findings recorded in the audit table at the end of this file |
| 4 Technical review · 6 Practitioner consultation · 7 Impact assessment · 11 Approval · 12 Publication · 13 Post-implementation review | **No** | Outstanding |

**This set is therefore a draft pending approval.** Element 25 of every standard says so. PCI is a small
private certifier and this process must never be represented as equivalent in scale, independence or
authority to that of a public standard-setter.

### The suite principle

> **AI proposes; the professional verifies, decides and remains accountable.**

---

## Definitions

These definitions decide compliance. Every one is written as a test a reader can apply — what makes
the thing what it is, measured against what, decided by whom — and none defines a term by itself.
Where the suite terminology audit (`../registries/TERMINOLOGY_AUDIT.md`) records that a term carries
genuinely different professional meanings across the three books, **both senses survive here and are
flagged**; collapsing them would make the standards wrong.

**Where a term is also defined in the Foundational Standards.** *material*, *independent*, *evidence*,
*competent reviewer*, *decision owner* and *escalation threshold* are also defined, at `D-01` to
`D-30`, in [`PCI_FOUNDATIONAL_STANDARDS.md`](PCI_FOUNDATIONAL_STANDARDS.md). **They now carry the same wording in
both places.** Each was reconciled to the canonical definition recorded in
[`PCI_STANDARDS_DEFINITIONS_REGISTER.md`](PCI_STANDARDS_DEFINITIONS_REGISTER.md), which also records what this
volume previously said and why the change was made. The terms this set flags as carrying two genuine
professional senses — *baseline*, *sponsor*, *verification*, *governance* — are **not** collapsed by
that reconciliation; the register carries both senses of each, with the context each belongs to,
because collapsing a real collision would make this volume wrong.

Three reading rules remain, and they still matter for any term this reconciliation did not reach.
First, **where a foundational standard states its own defined term by its `D-NN` number, that definition
governs that foundational obligation**, and nothing here narrows it. Second, **where a definition here
and a foundational definition both bear on the same act, the one producing the wider obligation
governs** — Charter section 4 states that a PCI Standard never lowers an obligation. Third, a term defined here and
not there is a PML-AI term and carries only the sense given here.

**How element 21 samples are drawn.** Where a standard's element 21 tests "a stated sample", the sample is
selected by the reviewer performing the test, not by the credential holder whose work is under review,
and the reviewer records the basis of selection.

### Terms that decide compliance

**acceptance** — A named authority's recorded decision that a deliverable meets the acceptance
criteria that were set and version-identified before the deliverable was produced. An acceptance
record states what was tested, against which version of the criteria, what did not conform, and which
nonconformities are carried forward with an owner and a date. **Silence, use of the deliverable,
payment against it and the expiry of a review period are not acceptance.**

**baseline** — *Context flag: two senses, both used in this set, and both are correct professional
usage.*
- **Control baseline** — the approved, version-controlled plan (scope, schedule or cost) against which
  performance is measured. This is the sense inherited binding from the suite style spine and is the
  sense meant wherever this set writes *baseline* unqualified in a planning, cost or change context.
- **Benefits baseline** — the measured pre-change position from which a benefit is measured, captured
  before the change and never reconstructed after it.

Where both senses could be meant in one sentence, this set writes the qualified form. A standard that says
*baseline* in a benefits context and means the plan is a defect.

**benefit** — A measurable improvement resulting from an outcome, owned by a named individual outside
the delivery team, measured against a benefits baseline. A benefit is what the organisation gets;
*value* is the worth it attaches to that. **Neither is the Earned Value of a work package**, which is
a budgeted money amount and a measure of work performed, not of worth delivered.

**competent reviewer** — *(Canonical — `D-04`.)* A named individual who, in relation to a particular
item, satisfies all of: (a) their competence in the subject matter is evidenced by a qualification, an
assessed competence record or documented experience of comparable work, **recorded for that class of
review before the review begins**; (b) they are able to state what would make the item wrong and which
method would detect that error; and (c) they hold the technical knowledge and delivery experience to
perform the verification method the standard requires and to reach a conclusion on the matter **without
relying on the preparer's explanation of it**. Competence is recorded against the class of work, not
against seniority, job title or availability. **Independence is not a limb of competence:** the
requirement that the reviewer did not prepare, direct, specify or approve the thing being reviewed is
*independence*, defined below, and it is imposed by each standard's element 10 rather than by this
definition. The two were previously folded together here, which made `PCI-FND-STD-10` element 12's
supervised-acquisition exception unusable, because that exception needs a reviewer who is competent and
is precisely not independent — a supervisor. No standard loses an independence requirement by the change:
every element 10 that required one still states it.

**decision owner** — *(Canonical — `D-08`.)* The single named individual holding authority, under the
applicable governance arrangement or the organisation's documented delegation schedule, to approve,
reject, amend, defer or withhold a specified decision; who bears its consequence; and who answers for
it afterwards. The accountability is held by one person, is not delegable, and is recorded before the
output is relied upon. **A committee is not a decision owner.** Where a body decides collectively, the
decision owner is the named chair or the named authority the delegation schedule assigns, and the
record names that person. A decision owner is always a natural person: "the team", "management", "the
business", "the sponsor", "the lenders", "the organisation", a role held by no named individual, a
system, a model and a vendor are never decision owners.

**detriment** — Any of the following acts occurring to a person after they raised a concern: removal
from, or reduction of, a role, scope, responsibility, grade or pay; an adverse change to a performance
rating, reference, bonus or renewal decision; exclusion from meetings, distributions or work they
previously took part in; a reassignment they did not request; non-renewal or early termination of an
assignment, secondment or contract; or a disciplinary step. **Detriment is defined by these observable
acts, and not by how the person felt about them** — which is what makes an allegation of detriment
capable of being investigated and either upheld or dismissed on evidence.

**evidence** — *(Canonical — `D-11`.)* A dated record that exists independently of the assertion it
supports, attributable to an identified author or issuing system, version-identified where it has a
version, and retained so that a person other than the author of the assertion — a person who was not
present — can retrieve it, examine it and reach the same conclusion **without asking that author**,
determining what was known, by whom, and when. The following are **not** evidence for the purposes of
this set: an output of an AI system that does not identify the source of what it asserts; an
AI-generated summary of a record, as evidence of the underlying fact — the underlying record is; a
statement that a system, model or tool produced a figure, unaccompanied by the inputs and the method; a
restatement of the assertion in a second document by the same author; a preparer's own statement
offered in support of their own assertion; a recollection; an undated summary or extract; an
unattributed file; an unversioned extract or working copy; a screenshot with no source reference; and a
dashboard state that cannot be reproduced.

**escalation threshold** — *(Canonical — `D-10`.)* The escalation threshold for a matter is reached at
the earliest moment any of the following becomes true: the matter is *material*; it creates a risk to
the safety of a person; it would change, or would have changed, a decision already taken or about to be
taken; it affects an output already issued outside the credential holder's own organisation; it affects
a contractual, regulatory, tax or financial-reporting position; or a documented value, condition or
event stated in the organisation's delegation schedule is met. On reaching it the matter must be passed
to a named higher authority within the time the threshold states. **Any documented value in the
delegation schedule is additional to those six triggers and never in place of them**; a matter that
reaches the threshold requires escalation under `PCI-FND-STD-11` whether or not the delegation schedule
enumerated it. A threshold with no stated destination, or with no stated time, is an incomplete
threshold and it is an aspiration until it is completed — but **the absence of a documented threshold
does not remove an escalation duty**: where the delegation schedule names no destination, the matter
goes to the next authority above the *decision owner* for it, and where it states no time, the
foundational period at `D-20` applies — one working day where the matter creates a risk to the safety
of a person or an ongoing financial loss, five working days otherwise, running from the moment the
credential holder first knows or suspects the matter. Reaching the threshold starts that period.

**gate** — A continuation decision taken by a named authority at a defined point in the delivery
lifecycle, against criteria published before the evidence supporting the decision was assembled, and
carrying the power to stop, hold, redirect or authorise the next commitment. A review that cannot stop
the work is not a gate.

**independent** — *(Canonical — `D-12`.)* Of a person or function, **in relation to a specified
matter**, where all of the following are true: (a) they did not perform the act, prepare the item or
any part of it, or direct, specify or approve it; (b) they did not select, build or configure the
tool, model or AI system that produced it; (c) they hold no conflict of interest in its outcome and no
financial interest in the matter or in a party to it; (d) they receive no fee, bonus, continuing
mandate, success payment or other benefit that varies with the conclusion reached, and their
remuneration, appraisal or continuation is not determined by the outcome the item supports; (e) they
are not accountable for the outcome it reports on; and (f) they are not in the reporting line of the
person accountable for it *for the purpose of that matter*, and do not report to the preparer in
respect of the work under review. Reporting to the preparer's line manager on unrelated work does not
by itself defeat independence; being appraised on the outcome does. **Independence is a property of a
relationship to a specific matter, not a job title** — which is why a permanently titled "independent
assurance" function loses independence over any artefact it helped produce.

**mandatory precondition** — A transition, gate or release condition recorded **met or not met**, with
its approving authority named and dated, closed by the authority that owns it rather than by the
project. A mandatory precondition carries no probability, no weighting, no score and no admission to
economic trade against the cost of delay.

**material** — *(Canonical — `D-15`.)* A matter, item, error, omission, variance or difference is
*material* if any of the following is true: (a) had the decision-maker known it at the time, it could
have changed a decision within the scope of the work — its substance, its timing, its conditions, or
the authority at which it had to be taken; (b) it changes a reported figure by more than the quantified
tolerance published for that figure; (c) it affects a contractual, regulatory, tax or
financial-reporting position; (d) it affects the safety of a person; (e) it affects a party's reliance;
or (f) it meets the organisation's published materiality criteria. Materiality is assessed **against
the decision**, not against a fixed sum, and it is judged twice: on the item alone, and on the
accumulation of items of the same kind since the test was last applied. **In PML-AI work, limb (b) is
measured by the documented threshold**: where the organisation's governance sets a documented
financial, schedule or exposure threshold for a class of decision, that threshold applies to matters
that are purely financial, schedule or exposure in character. **A matter bearing on safety, legality, a
licence or permission, a statutory duty, or the truth of a statement made to a decision-maker is
material irrespective of size**, and no documented threshold reduces it. **PCI sets no percentage.**

**sponsor** — *Context flag: the delivery and governance sense.* The single named individual
accountable for the project's business outcome and mandate, who owns the business case, holds the
authority reserved above the project leader, and answers for the benefits the project exists to
enable. **The project-finance sense of "sponsor" — an equity investor promoting a project — belongs to
the PFL-AI volume and is not used anywhere in this set.**

**approved** — *(Canonical — `D-29`.)* A decision, document, figure or version is *approved* where the
person holding authority for that decision under the applicable governance arrangement or the
documented delegation schedule has given assent identifiably, recording the date, the version assented
to and the scope of the assent. Silence, absence of objection, unrecorded verbal assent, assent by a
person outside their recorded authority, and assent recorded after the item was used are not approval.
Distinguish from *acceptance* above, which is a named authority's recorded decision that a deliverable
meets version-identified criteria.

**current** — *(Canonical — `D-30`.)* A record, figure, extract, document or version is *current*
where it is the latest version issued by the system or authority that owns it as at the artefact's
stated cut-off or, where it states none, as at its issue date — and where its version identifier and
its extraction date and time are recorded on or with the artefact. A record whose version cannot be
identified is not current, whatever its age.

**material AI assistance** — *(Canonical — carried to the whole corpus by
[`PCI_STANDARDS_DEFINITIONS_REGISTER.md`](PCI_STANDARDS_DEFINITIONS_REGISTER.md).)* AI assistance in producing an
artefact is *material* where removing the AI-generated contribution would change a figure in the
artefact by more than the applicable materiality measurement, or would change a recommendation, a
classification that affects entitlement, coding, ranking or eligibility, or a stated conclusion.
*Material AI contribution* means the same thing. Volume of use, licence cost and whether a human edited
the artefact afterwards are irrelevant to the test. This set made compliance turn on the term at
`PCI-PML-STD-14.02` element 21 without defining it; the definition is supplied here so the test can be
applied.

### Supporting terms used in obligations

**abstention** — The interested party taking no part in the discussion or the decision on an item,
minuted with the name of whoever decided in their place. An abstention that is not minuted is
indistinguishable from participation when the file is read later.

**assurance** — Independent examination providing confidence that a control, process, plan or
deliverable is what it is claimed to be. Distinguish from *verification* below.

**assurance capture** — An assurance function providing assurance on work it prepared, directed or
specified, and therefore unable to challenge it. Capture is the least visible assurance failure,
because the product still looks independent.

**concern** — A statement by any person working on or for a project that something in its delivery,
conduct, safety, legality or reporting is wrong, is going wrong, or is being misrepresented. A concern
is a concern whether or not it turns out to be correct; the correctness is what the response
determines.

**concern route** — The named, published mechanism by which a concern reaches a named recipient. A
route is a **bypass route** in relation to a person if that person is neither its recipient, nor able
to see the concern before the recipient does, nor able to determine what happens to it.

**conflict of interest** — *(Canonical — `D-05`, where the term is written* conflict *and means the
same thing.)* A relationship, interest or duty of any of the kinds `D-05` lists, held by the person or
by a member of their household or a connected person, which bears on a matter they are being asked to
decide, advise on or assure — a financial stake or interest in a party or in the outcome; employment,
office, directorship or partnership with a party; a personal, family or household connection; a gift,
hospitality or benefit above the recorded threshold; a past or prospective engagement with a party;
authorship of, or a personal interest in, the thing being assured; a future role that depends on the
outcome; and an interest in a tool, model, vendor or AI system being selected, priced, assessed or
relied upon. An **interest** is anything capable of affecting, or of appearing to affect, that person's
judgement on the matter. **The appearance is part of the definition, not a softening of it.** A
conflict of interest is a structural fact about a role, not a lapse of character. The list is closed
for the purposes of the disclosure duty: every conflict within it must be disclosed and there is no
threshold.

**decision record** — The versioned, attributable log entry that converts a decision into an
institutional fact. Its fields are the decision, the named decision owner, the date, the options
considered, the reasoning, any recorded dissent, the declared interests or the nil return, and a
**versioned reference to the information relied on**.

**delegation schedule** — The organisation's documented statement of which named role may take which
class of decision up to which documented limit, what aggregates across related decisions, and where a
decision above the limit goes.

**dependency** — A commitment by one party to give another party a defined thing by a defined date,
recorded with the giver, the receiver, the thing, the date, the confidence and the consequence of a
breach. **A dependency with no named owner on the giving side is not a dependency.**

**discretionary condition** — A transition or gate condition that genuinely admits degree at the
decision date, is genuinely uncertain, and is genuinely tradeable against the cost of waiting. Its
assessment carries a probability. It is the complement of a mandatory precondition, and the two never
appear in the same block.

**gate block** — The set of mandatory preconditions sitting above a readiness assessment. While any
item in it is recorded not met, the only decision available is hold.

**governance** — *Context flag.* **Project governance** (this set's default sense): the decision
rights, accountabilities and information flows through which an organisation directs and controls a
project, programme or portfolio. **Data governance**: ownership, definitions, access and traceability
of data. This set writes *data governance* in full, always.

**nil return** — A positive record that a person had nothing to declare on a matter, so that silence
in the file is evidence rather than an absence of evidence.

**product owner** — The single individual accountable for the order of a backlog within a value
envelope set by the sponsor. Where the ordering right is exercised elsewhere, that other body is the
product owner in fact and the record must say so.

**structural interest** — An interest touching the substance of a role rather than a single item.
It is managed by changing the role or moving the work, never by a declaration followed by the decision
being taken anyway.

**traceability** — A maintained, bidirectional link from each approved requirement to its source, to
the design or backlog item that satisfies it, to the test or acceptance criterion that proves it, and
to the deliverable that carries it — such that a reader can start at any one of those and reach the
others without asking the author.

**verification** — *Context flag: two senses, both used in this set.*
- **V&V verification** — the check that what was built is what was specified. (Its partner,
  *validation*, asks whether what was built produces the outcome that was needed.)
- **AI verification** — a named human's check of machine output against source before reliance on it.
  This is the sense carried by the suite principle and by element 16 of every standard below. **The test is
  the canonical *verified* at `D-26`**: a named *competent reviewer* applies one of the eight
  admissible methods — independent recomputation, source tracing, clause-to-summary comparison,
  sampling on a stated basis, reconciliation, boundary testing, sensitivity analysis, or named expert
  judgement recorded with its reasoning — against *evidence*, and records the method, the source or
  population tested and its selection basis where a sample was used, the inputs, the scope, the date,
  the result, and every difference found with its resolution. Reading an output and finding it
  plausible is not verification.

Where a sentence could carry either sense, this set writes the qualified form. **The two senses are a
declared collision, not a defect**: collapsing V&V verification into AI verification would make this
set's delivery-assurance standards wrong. Both are recorded at
[`PCI_STANDARDS_DEFINITIONS_REGISTER.md`](PCI_STANDARDS_DEFINITIONS_REGISTER.md) section 4 with the context each belongs
to, and at `../registries/TERMINOLOGY_AUDIT.md` Issue 9.

---
## Domain 1 — The Project Leadership Profession

### PCI STANDARD PCI-PML-STD-01.01 — Leadership Accountability for Delivery Decisions

**1. Normative requirement.** A credential holder who leads a project, programme or portfolio must
remain personally accountable for every delivery decision taken under their authority, including a
decision informed, drafted, modelled or recommended by an AI system, an adviser, a supplier or a
committee.

**2. Purpose.** Delivery leadership is an accountable role, not a coordinating one. Every other standard in
this set assumes there is one identifiable person who answers for the decision. Where accountability
is diffused across a governance diagram, a delivery partner and an analytics platform, nobody answers
and the project drifts unowned until it fails visibly.

**3. Scope.** Every credential holder exercising project, programme or portfolio leadership, in every
delivery model — predictive, adaptive or hybrid — whether the work is delivered in-house, through
partners or through an automated toolchain. It applies to preparation, recommendation, approval and
assurance of delivery decisions, and to decisions taken in the leader's name by others.

**4. Defined terms.** *decision owner* · *decision record* · *delegation schedule* · *material* ·
*evidence* · *sponsor*. Additionally, **delivery decision** means a decision that changes what is
built, when it is delivered, what it costs, what risk is accepted, who does the work, or what is
represented to a decision-maker about any of those.

**5. Required actions — process requirements.** Each is mandatory and is breached independently.

- **`PCI-PML-STD-01.01-PR-01` — Named acceptance.** The credential holder must accept the leadership
  accountability in a written instrument — charter, appointment letter or terms of reference — that
  names the individual, and must not begin exercising leadership authority before that instrument
  exists.
- **`PCI-PML-STD-01.01-PR-02` — Known reservation boundary.** The credential holder must hold a
  current, dated statement of which decisions are personally reserved to them and which sit above
  them, drawn from the delegation schedule, and must re-read it at each gate.
- **`PCI-PML-STD-01.01-PR-03` — Decision recording.** The credential holder must ensure that every
  material delivery decision taken under their authority produces a decision record carrying all of
  its defined fields, including the versioned reference to the information relied on.
- **`PCI-PML-STD-01.01-PR-04` — Correction on discovery.** On discovering that a decision issued under
  their authority rested on a material error, the credential holder must record the error, notify
  every person who relied on the decision, and state what the corrected position is, within the
  notification time the delegation schedule sets for that decision class.

**6. Prohibited actions.** Presenting a committee, a model, a methodology, a framework or a supplier as
the accountable party for a decision. Accepting a leadership title without the corresponding decision
rights while allowing others to believe accountability is discharged. Signing a charter one does not
intend to honour. Permitting a decision to be issued in one's name without one's knowledge and
allowing the record to stand uncorrected once that is known.

**7. Required evidence.** The signed charter or appointment naming the accountable leader; the current
delegation schedule; decision-record entries carrying the named owner, options, reasoning, recorded
dissent and versioned information reference; correction and notification records for decisions later
found defective.

**8. Responsible role.** The named credential holder appointed as project, programme or portfolio
leader. Not "the team", not "the PMO", not "management".

**9. Approval authority.** The sponsor approves the leader's appointment and the reservation boundary.
The governing body approves the delegation schedule the boundary is drawn from. Neither may be
approved by the leader.

**10. Independence requirement.** Not applicable to the decision itself, because leadership
accountability is by definition non-independent of delivery; independence attaches instead to the
assurance of these records, which `PCI-PML-STD-01.03-PR-06` governs.

**11. Materiality or threshold.** This standard sets no number. The materiality of a delivery decision is
decided by the definition of *material* above, applied against the organisation's documented
delegation schedule. The credential holder must apply the documented thresholds; where the
organisation has none for a decision class, the credential holder must record that gap and treat every
decision in that class as reserved until a threshold is documented.
*Six-person internal project:* the charter is one page, the delegation schedule is a five-row table,
and the decision record is a shared log — the obligation is satisfied in minutes a week.
*Multi-partner national programme:* the same obligations attach at each accountable tier, and the
schedule states the aggregation rule across partners so a decision cannot be split below a limit.

**12. Exception and waiver.** No exception is permitted to the principal obligation: accountability
cannot be waived, transferred or bounded by contract, disclaimer or tool configuration. An exception to
`PR-01` — beginning to act before the written instrument exists — may be approved by the sponsor for a
stated period not exceeding the next gate, on written justification, with the instrument's absence
recorded as an open item and reported to the governing body.

**13. Escalation trigger.** Any material delivery decision with no identifiable accountable person; any
instruction to act beyond or outside the recorded authority; discovery that a decision was issued in
the leader's name without their knowledge or review.

**14. AI application.** AI may assemble decision packages, summarise options and trade-offs, maintain
and age the decision record, flag entries missing an owner, a date, a reasoning field or a versioned
information reference, and cluster the log to detect decisions being taken twice.

**15. AI prohibition.** An AI system must not hold, share or discharge accountability, be recorded as
the decision maker or approver, be cited as the reason a decision was taken, or author a decision
record entry wholesale. No configuration, disclaimer, procurement term or contract removes the
credential holder's accountability.

**16. AI verification.** **Source tracing plus named approval.** Before a decision is taken on
AI-assembled material, the credential holder must trace each figure and each stated constraint in the
package to its cited source document and version, and must record their own name against the decision.
Quarterly, the credential holder must apply **sampling with a stated basis** to the decision record —
a stated number of entries drawn across the period — and confirm for each sampled entry that a named
human owner, a reasoning field and a versioned information reference are present.

**17. External reference.**
- **ISO** · *ISO 21502, Project, programme and portfolio management — Guidance on project management* ·
  relied on for: the existence of a distribution of accountabilities between a governing body, a
  sponsor and a project manager · edition not asserted here; recorded at **EXT-028** in the suite
  External-Reference Register · **Manual section 6 category 3 — international voluntary standard** · currency
  checked 2026-08-03 · limitation: guidance only, voluntary unless imported by contract or regulation;
  nothing is certifiable against it; it is not the source of this standard's obligation.
- **ISO** · *ISO 21505, Project, programme and portfolio management — Guidance on governance* · relied
  on for: the location of delivery accountability inside an organisation's governance arrangements ·
  **EXT-032**, recorded as **not independently verified — verify current requirements** · **Manual section 6
  category 3** · limitation: as above, and the register's verification status is open.
- **ISO/IEC** · *ISO/IEC 42001, Information technology — Artificial intelligence — Management system* ·
  relied on for: the existence of an expectation that organisational roles and responsibilities for AI
  use are assigned · **EXT-021** · **Manual section 6 category 3** · currency checked 2026-08-03 · limitation:
  a certifiable management-system standard, voluntary unless adopted; certification concerns a
  management system, not any individual decision.
- **OECD** · *Recommendation of the Council on Artificial Intelligence (the OECD AI Principles)* ·
  relied on for: the proposition that human actors remain accountable for AI-influenced outcomes ·
  **EXT-081** · **Manual section 6 category 10 — illustrative practice** · currency checked 2026-08-03 ·
  limitation: a Council Recommendation, not binding law even on adherents; never legislation.

**18. Jurisdictional caution.** Directors' duties, statutory delivery, safety, planning and licensing
roles, employment law and the law of professional negligence determine **legal** responsibility
independently of this professional requirement, and can place it on a person other than the credential
holder. Obtain local legal advice on the statutory roles attaching to a specific project and entity.

**19. Related PCI Standards.** `PCI-FND-STD-01`; `PCI-FND-STD-02`; `PCI-FND-STD-04`; `PCI-FND-STD-12`;
`PCI-FND-STD-15`; `PCI-PML-STD-01.02`; `PCI-PML-STD-03.02`; `PCI-PML-STD-03.04`.

**20. Related Body of Knowledge content.** PML-AI · Domain 1 · KA 1.2 The project leader's
accountability · topics 1.2.1 accountability and responsibility, 1.2.2 the obligation set, 1.2.3 the
professional standard of care; KA 1.4 topic 1.4.3 the leader's AI accountability. Also Domain 3 KA 3.2;
Domain 14 KA 14.4.

**21. Compliance test.** Compliance is demonstrated when a reviewer can, for a stated sample of
material delivery decisions taken in the period: (a) name the accountable individual from the charter
or appointment instrument, without asking anyone; (b) find that individual's name in the decision
record entry as owner; (c) open the versioned information reference in that entry and retrieve the
document version cited; (d) match the decision's value or exposure against the delegation schedule and
confirm it was taken at or below the authority the schedule assigns to that named individual; and
(e) for any decision later corrected, find a correction record and a notification record dated within
the schedule's notification time. A sample in which any entry fails (a), (b) or (d) fails the test.

**22. Breach indicators.** Decision entries whose owner field names a committee, a function or a tool.
A charter with no named individual. Two people each stating the other decided. Corrections issued
silently. A leader unable, when asked, to state the basis of a decision issued in their name within
the previous quarter. A delegation schedule dated before the current organisational structure.

**23. Consequence within PCI authority.** Correction required; additional review; escalation;
examination failure on the associated competency; ethics review; certification investigation;
suspension or withdrawal of the PML-AI credential — each subject to due process and a right of appeal.

**24. Examination application.** Scenario judgement: a delivery decision goes wrong and responsibility
is contested between a leader, a steering committee, a delivery partner and an analytics tool; the
candidate locates accountability and states the next required act. Evidence selection: choosing, from
five artefacts, the two that would establish who decided. Escalation decision: an instruction to act
outside recorded authority.

**25. Version and status.** Version 2.0 · **not yet approved** (Charter section 5 Stage 11 outstanding) ·
effective on approval · supersedes `PML-LAW-01-01` v1.0. Amendment note: renumbered; restructured to
the twenty-five-element form; legislative drafting removed; the former single rule split into one
principal obligation and four process requirements; compliance test replaced with a performable
five-part test.

---

### PCI STANDARD PCI-PML-STD-01.02 — Reserved Delivery Decisions and the Named Human Decider

**1. Normative requirement.** A credential holder must ensure that every delivery decision in a class
the organisation has reserved to a human decider is taken by the named individual holding that
authority, on evidence that individual has examined, and must not permit such a decision to be
executed on the output of an automated or AI system without that examination.

**2. Purpose.** `PCI-FND-STD-04` establishes that a human decides. This standard addresses what is specific
to delivery: automation in delivery does not usually announce itself as a decision. A prioritisation
engine reorders a backlog, a scheduling tool re-sequences a network, a resource optimiser reassigns
people, a risk model closes a risk — and each of those is a decision that has already taken effect by
the time a human sees it. The failure this standard prevents is **decision by default**: nobody decided,
the system acted, and the record shows an approval that was never an examination.

**3. Scope.** Every credential holder in a delivery leadership or delivery governance role, for every
decision class the organisation reserves to a human. It applies to decisions executed by scheduling,
prioritisation, resource-allocation, risk-scoring, procurement-scoring and reporting automation, and to
adaptive delivery where the ordering right sits with a product owner. It governs approval and
execution, not analysis.

**4. Defined terms.** *decision owner* · *material* · *evidence* · *delegation schedule* ·
*AI verification*. Additionally, **reserved decision class** means a class of decision the
organisation's delegation schedule states must be taken by a named human; **executed automatically**
means taking effect in a system of record without a named human having recorded an examination first.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-01.02-PR-01` — The reserved-class list exists.** The credential holder must maintain a
  dated list of the decision classes reserved to a human decider on their project, approved by the
  sponsor, covering at minimum: baseline change, scope acceptance, risk acceptance, contingency
  release, supplier selection and award, release or go-live, and any decision affecting safety, a
  licence, a permission or a statutory duty.
- **`PCI-PML-STD-01.02-PR-02` — Automation inventory against the list.** The credential holder must
  maintain a dated inventory of the automated and AI systems in use on the project that are capable of
  changing anything in a reserved class, and must record for each whether its output takes effect
  automatically or on a named human's recorded action.
- **`PCI-PML-STD-01.02-PR-03` — No automatic effect in a reserved class.** Where the inventory shows a
  system capable of taking effect automatically in a reserved class, the credential holder must have
  that capability disabled or gated, and must record the date it was disabled or gated and by whom.
- **`PCI-PML-STD-01.02-PR-04` — Examination before decision.** For each reserved decision taken on
  AI-assisted material, the named decider must record what they examined, which version of it, and
  what they changed or confirmed — a record distinct from the approval itself.

**6. Prohibited actions.** Recording an approval that was a click on a system-generated recommendation
with no examination record. Configuring or accepting a tool that changes a reserved-class item without
a named human action. Describing an automated re-sequencing, re-prioritisation or reallocation as "the
tool's output" when it has taken effect. Delegating a reserved decision to a supplier's system because
the supplier holds the tool.

**7. Required evidence.** The approved reserved-class list with its date; the automation inventory with
its per-system effect column; configuration records or screenshots with source references showing
automatic effect disabled or gated, dated and attributed; examination records paired with approval
records for a sample of reserved decisions.

**8. Responsible role.** The named credential holder leading the project, for maintaining `PR-01` to
`PR-03`. The named decider recorded in the delegation schedule for the class, for `PR-04`.

**9. Approval authority.** The sponsor approves the reserved-class list. The governing body approves
any addition to, or removal from, that list. A supplier or tool vendor must not approve either.

**10. Independence requirement.** The named decider must be independent of the configuration of the AI
or automation system whose output they are examining, in the sense that they did not set its
parameters, weights or thresholds for the decision in question. Where the same person unavoidably does
both on a small project, `PCI-PML-STD-01.03-PR-05` applies and the arrangement is recorded and reported
to the sponsor.

**11. Materiality or threshold.** This standard sets no number. The reserved-class list *is* the threshold
instrument, and this standard requires that it exists, is approved, is dated, and is applied. The listed
minimum classes above are the floor; the organisation's governance sets everything else.
*Six-person internal project:* the list is seven lines and the inventory is three rows — most such
projects run one scheduling tool, one issue tracker and one spreadsheet with a model in it.
*Multi-partner national programme:* each partner maintains an inventory for the systems it operates,
the programme maintains the consolidated list, and the interface is a stated obligation in the partner
agreement rather than an assumption.

**12. Exception and waiver.** An exception permitting automatic effect in a reserved class may be
approved only by the governing body, only for a named system, only for a stated period not exceeding
one gate interval, on a written justification stating the compensating control (at minimum: a daily
exception report to the named decider naming every change the system made). Every such exception is
reported to the sponsor and to assurance.

**13. Escalation trigger.** Discovery that a reserved-class item changed with no named human action.
Any request to enable automatic effect in a reserved class. A supplier's refusal or inability to
disclose whether its system takes effect automatically.

**14. AI application.** AI may generate options, rank them against stated criteria, model consequences,
prepare the decision package, and detect and flag reserved-class changes that occurred with no paired
examination record.

**15. AI prohibition.** An AI system must not take, approve, sign, certify, waive or authorise a
reserved decision; must not be recorded as the decider; and must not be given effect in a reserved
class by default configuration. An AI system's confidence score must not stand in for a human
examination.

**16. AI verification.** **Boundary testing plus independent recomputation.** For each reserved
decision class exposed to an AI or optimisation system, the named decider must test the system's
behaviour at the boundary of the class — submit an input just inside and just outside the reserved
threshold and confirm the system stops at the boundary — and must record the test date and result at
least once per gate interval and after any configuration change. Where the system produces a number
that carries the decision (a rank, a score, a date, a cost), the decider must recompute at least one
such number by an independent method and record the comparison.

**17. External reference.**
- **ISO/IEC** · *ISO/IEC 38507, Governance implications of the use of artificial intelligence by
  organizations* · relied on for: the existence of governing-body-level questions about where AI is
  permitted to act · **EXT-037** · **Manual section 6 category 3 — international voluntary standard** ·
  currency checked 2026-08-03 · limitation: guidance aimed at governing bodies, not at practitioners;
  voluntary; not certifiable.
- **NIST (US Department of Commerce)** · *Artificial Intelligence Risk Management Framework
  (AI RMF 1.0)* · relied on for: the existence of a govern-map-measure-manage shape for AI risk ·
  **EXT-080** · **Manual section 6 category 10 — illustrative practice** · currency checked 2026-08-03 ·
  limitation: NIST states it is voluntary, rights-preserving and non-sector-specific; **it is not a
  standard and not a regulation**.
- **European Union** · *Regulation (EU) 2024/1689 (the AI Act)* · relied on for: illustrating a
  risk-tiered regulatory shape that treats human oversight as a design question · **EXT-100** ·
  **Manual section 6 category 1 — applicable legislation or regulation** · currency checked 2026-08-03 · limitation: **binding
  legislation within the European Union only**, named here only to illustrate the shape; it is
  relied on for no requirement in this standard, and whether it applies to a given project is a question for
  qualified local counsel.

**18. Jurisdictional caution.** Whether a particular automated decision is lawful, whether it triggers
a transparency, human-review or impact-assessment duty, and what disclosure is owed to an affected
person are jurisdiction-specific questions. Obtain local legal advice before deploying automation that
affects people's work allocation, pay, assessment or employment.

**19. Related PCI Standards.** `PCI-FND-STD-04`; `PCI-FND-STD-03`; `PCI-FND-STD-14`; `PCI-FND-STD-13`;
`PCI-PML-STD-01.01`; `PCI-PML-STD-03.02`; `PCI-PML-STD-13.02`; `PCI-PML-STD-14.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 1 · KA 1.4 Ethics and the responsible use of
AI · topics 1.4.2 the PCI responsible-AI principle, 1.4.3 the leader's AI accountability. Also Domain 14
KA 14.3 and KA 14.4; Domain 13 KA 13.1 topic 13.1.2 product ownership as a decision right.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) obtain the dated,
sponsor-approved reserved-class list and confirm it contains the seven minimum classes; (b) obtain the
automation inventory and confirm every system on the project capable of changing a reserved-class item
appears on it; (c) for each such system, find either a dated record that automatic effect is disabled
or gated, or a governing-body exception within its stated period; (d) draw a stated sample of reserved
decisions from the period and find, for each, an examination record naming the version examined and
dated **at or before** the approval record; and (e) find a boundary-test record for each exposed class
dated within the current gate interval. Any reserved-class change in the systems of record with no
paired named-human action fails the test outright.

**22. Breach indicators.** Approval timestamps that cluster within seconds of recommendation
timestamps. An automation inventory that has not changed while three new tools were procured. A
scheduling tool whose re-sequencing appears in the published plan with no change record. Suppliers
reporting "the system decided". A reserved-class list that names no owner and no date.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure on the associated competency; ethics review; certification
investigation; suspension or withdrawal — each subject to due process and a right of appeal.

**24. Examination application.** AI-verification case: a resource optimiser has reassigned four people
across two workstreams overnight and the plan now shows a new critical path; the candidate states
whether a decision has been taken, by whom, and what must happen next. Evidence selection: which
artefact proves a human examined the recommendation rather than approved it.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-01-02` v1.0. Amendment note: **retitled** to remove the collision with the foundational standard
*Human Decision Authority*; obligation narrowed to what delivery leadership adds — the reserved-class
list, the automation inventory, the prohibition on automatic effect and the examination record — so the
standard adds rather than restates; renumbered and restructured; legislative drafting removed.

---

### PCI STANDARD PCI-PML-STD-01.03 — Interests, Abstention and Assurance Independence

**1. Normative requirement.** A credential holder must not take, or participate in taking, a decision,
recommendation, evaluation or assurance opinion on a matter in which they hold a conflict of interest.

**2. Purpose.** Until the current edition of the PML-AI manuscript set, the phrase *conflict of
interest* appeared nowhere in it. The volume taught gates, steering committees, weighted scoring,
supplier selection and three lines of assurance, and none of that machinery carried a rule about **who
is barred from deciding**. That gap is now closed in the manuscript at Domain 1, KA 1.2.2a; this standard is its
enforceable counterpart. The failure it prevents is the one the machinery cannot see: a decision that
is procedurally perfect — criteria published, scores recorded, minutes taken — and taken by a person
whose judgement was engaged elsewhere. Its hardest case is **assurance capture**, where the second line
assures a plan it helped write and the product still looks independent.

**3. Scope.** Every credential holder, on every matter they decide, advise on, evaluate, score or
assure: gate decisions, steering and change-board decisions, option appraisals and scoring models,
tender evaluation and supplier selection and award, claims and disputes, assurance reviews and
opinions, recruitment and assignment of people to roles they will later assess, and the setting or
approval of thresholds. It applies to preparation, review, recommendation, approval and assurance
alike.

**4. Defined terms.** *conflict of interest* · *structural interest* · *abstention* · *nil return* ·
*independent* · *assurance* · *assurance capture* · *decision record* · *competent reviewer* ·
*material*.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-01.03-PR-01` — Identify at appointment and at each convening.** The credential holder
  must ask themselves, in writing, at appointment to any role and again as each gate, steering meeting,
  evaluation panel, change board or assurance review is convened, whether they hold an interest in what
  is about to be decided. A single question at induction does not satisfy this requirement, because
  interests are acquired after induction.
- **`PCI-PML-STD-01.03-PR-02` — Declare in writing before the item is taken.** A declaration must reach
  the chair in writing **before** the item is opened. A declaration made during or after the discussion
  does not satisfy this requirement; it is an explanation.
- **`PCI-PML-STD-01.03-PR-03` — Record, including nil returns.** The decision record for every gate,
  steering, change-board, evaluation-panel and assurance decision must carry an interests field, and
  that field must carry either the declarations made or a positive **nil return** for each participant.
  A blank field does not satisfy this requirement.
- **`PCI-PML-STD-01.03-PR-04` — Abstain, and name who decided instead.** The interested party must take
  no part in the discussion or the decision, and the minute must name the individual who decided in
  their place. An abstention that is not minuted does not satisfy this requirement.
- **`PCI-PML-STD-01.03-PR-05` — Structural interests change the role.** Where an interest touches the
  substance of a role rather than a single item, the credential holder must move the person or move the
  work, and must record in the governance file that this was done and by whom. Managing a structural
  interest by declaration followed by the decision being taken anyway does not satisfy this requirement.
- **`PCI-PML-STD-01.03-PR-06` — Assurance independence, tested by name.** A person or function must not
  provide an assurance opinion on a plan, control, estimate, schedule, business case or deliverable
  that the same person or function prepared, directed, specified or approved. The assurance record must
  name the assurer and must state, positively, that the assurer's name does not appear in the
  authorship or approval record of the artefact assured.
- **`PCI-PML-STD-01.03-PR-07` — Evaluation-panel exclusion.** A credential holder who holds an interest
  in any bidder, tenderer or candidate must not be a member of the evaluation panel for that
  procurement or appointment, must not see other evaluators' scores or comments, and must not
  moderate, chair or ratify the evaluation.

**6. Prohibited actions.** Deciding a matter in which one holds an interest. Declaring an interest and
then participating "for continuity". Assuring one's own work, or one's own function's work. Chairing
the moderation of scores one influenced. Recording an abstention with no named substitute decider.
Leaving an interests field blank rather than recording a nil return. Treating an interest as
extinguished because it was declared once, in a previous year, to a different body.

**7. Required evidence.** The interests register with entry dates; the written declarations with their
timestamps relative to the meeting agenda; minutes carrying the interests field, the abstention and the
substitute decider's name; role-change or work-transfer records for structural interests; assurance
opinions carrying the assurer's name and the positive independence statement; evaluation-panel
membership records with declarations against each member.

**8. Responsible role.** Each credential holder, for their own identification, declaration and
abstention. The named chair of the body, for ensuring the interests field is completed and the
abstention minuted. The named accountable owner of the assurance function, for `PR-06`.

**9. Approval authority.** The chair of the body determines whether a declared interest requires
abstention on an item. The sponsor determines a structural interest touching a project role. The
governing body determines a structural interest touching the sponsor, the assurance function or the
chair. **The interested party never determines their own case.**

**10. Independence requirement.** This standard *is* the independence requirement for the set. Its `PR-06`
test is applied by matching names, not by relying on titles: a permanently titled independent function
is not independent of an artefact whose authorship record carries its name.

**11. Materiality or threshold.** This standard sets no financial threshold, and one would defeat it — a
personal connection has no value in currency. The test is the definition of *conflict of interest*
above, applied by the chair, together with the daylight test the manuscript states at KA 1.4.1: would
every party seeing the full picture still regard the decision as impartial? Where an organisation's
governance sets a monetary de-minimis for **declarable financial** interests, that documented figure
applies to financial interests only, and applies to declaration, never to abstention.
*Six-person internal project:* three of the six may hold interests in the same supplier because the
population is small; the answer is a named substitute decider borrowed from elsewhere in the
organisation, recorded in the minute, not a decision taken anyway.
*Multi-partner national programme:* each partner maintains its own register, the programme holds the
consolidated view for shared decision bodies, and a partner's refusal to disclose is itself the
escalation trigger below.

**12. Exception and waiver.** **No exception is permitted** to the principal obligation or to `PR-06`.
Where abstention would leave a decision with no competent decider — a genuine constraint on very small
projects and in narrow specialisms — the matter must be escalated to the next authority in the
delegation schedule and decided there; it must not be decided by the interested party with a
disclaimer attached.

**13. Escalation trigger.** An interest that cannot be managed by abstention. A chair who declines to
minute a declared interest or an abstention. An assurance opinion whose assurer appears in the
authorship record. A party who refuses to state whether they hold an interest. Discovery that a
completed decision, evaluation or assurance opinion was taken by an interested party.

**14. AI application.** AI may maintain the interests register, prompt the standing question at each
convening, cross-match declared interests against a supplier or entity list, flag decision records with
an empty interests field, and cross-match assurance-opinion signatories against document authorship and
approval metadata to surface candidate capture cases.

**15. AI prohibition.** An AI system must not determine whether an interest exists, whether it is
material, whether abstention is required, or whether an assurance arrangement is independent. It must
not clear a conflict, and its failure to flag one is not evidence that none existed.

**16. AI verification.** **Reconciliation plus sampling with a stated basis.** Every AI-produced
capture candidate must be confirmed against the artefact's own version history by a named human before
it is recorded as a finding. Each quarter, a competent reviewer must reconcile the interests register
against the membership lists of every decision body in the period and confirm that each member appears
with either a declaration or a nil return, and must draw a stated sample of assurance opinions and
trace each assurer's name against the assured artefact's authorship and approval record.

**17. External reference.**
- **Project Management Institute** · *Code of Ethics and Professional Conduct* · relied on for: the
  existence of a professional expectation of conflict disclosure and impartiality · **EXT-063**,
  recorded as **not independently verified — verify current requirements** · **Manual section 6 category 6 —
  ethical code** · limitation: binding only where a body, regulator or engagement has adopted it; a PCI
  credential holder not subject to it is not made subject to it by this standard. No text is reproduced.
- **ISO/IEC** · *ISO/IEC 17024, Conformity assessment — General requirements for bodies operating
  certification of persons* · relied on for: the existence of impartiality requirements on bodies that
  certify people, which is why PCI holds itself to this standard as well · **EXT-022** · **Manual section 6
  category 3 — international voluntary standard** · currency checked 2026-08-03 · limitation: it binds
  certification bodies, not credential holders; voluntary unless imported.
- **OECD (G20/OECD)** · *G20/OECD Principles of Corporate Governance* · relied on for: the existence of
  a governance expectation that conflicted parties do not decide · **EXT-128** · **Manual section 6 category
  10 — illustrative practice** · currency checked 2026-08-03 · limitation: a Council Recommendation;
  non-binding; **not legislation**.

**18. Jurisdictional caution.** Public-procurement law, anti-bribery law, company law, charity law and
sector regulation impose their own conflict rules, registration duties and criminal consequences, and
they differ by jurisdiction and by the nature of the buying entity. Compliance with this standard is not
compliance with any of them. Obtain local legal advice before designing a declaration or evaluation
process for a publicly funded procurement.

**19. Related PCI Standards.** `PCI-FND-STD-08` (the parent disclosure obligation); `PCI-FND-STD-01`;
`PCI-FND-STD-10`; `PCI-PML-STD-03.03`; `PCI-PML-STD-09.01`; `PCI-PML-STD-10.01`;
`PCI-PML-STD-12.02`.
**What this standard adds to `PCI-FND-STD-08`:** the foundational standard requires disclosure of a conflict.
This standard requires
**abstention with a named substitute decider**, requires a **nil return** so silence becomes evidence,
requires a **role change** for structural interests, and applies a **name-matching test to assurance
independence** — none of which the foundational standard states.

**20. Related Body of Knowledge content.** PML-AI · Domain 1 · KA 1.2 · topic 1.2.2a Interests,
competence and confidentiality. Also Domain 3 KA 3.3 topics 3.3.2 assurance lines and 3.3.4 the decision
record; Domain 2 KA 2.2 topic 2.2.3 selection and prioritisation models; Domain 10 KA 10.2 tendering and
evaluation; Domain 9 KA 9.2 assurance and control.

**21. Compliance test.** Compliance is demonstrated when a reviewer, taking a stated sample of decision
records drawn from **each** of gate decisions, steering decisions, evaluation panels and assurance
opinions in the period, finds that: (a) every record carries a populated interests field, with a
declaration or a nil return against every participant named in the attendance list; (b) for every
declared interest, the minute records the abstention and **names the individual who decided instead**;
(c) for every assurance opinion, the assurer's name does **not** appear in the authorship or approval
record of the artefact assured, and the opinion carries the positive statement to that effect; (d) for
every structural interest recorded in the period, a dated role-change or work-transfer record exists;
and (e) no evaluation-panel membership list contains a member with a declared interest in a bidder. Any
sampled assurance opinion failing (c) fails the test outright, and the failure is reported as assurance
capture rather than as a documentation defect.

**22. Breach indicators.** Interests fields blank rather than nil-returned. Declarations timestamped
after the meeting. Abstentions minuted with no substitute decider. The same function appearing as
author and assurer of a plan. A steering committee whose interests register has no entries across a
year in a market where several members previously worked for the incumbent supplier. Evaluation
moderation chaired by the sponsor of the winning option. A structural interest managed by an annual
declaration.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure on the associated competency; ethics review; certification
investigation; suspension or withdrawal of the PML-AI credential — each subject to due process and a
right of appeal.

**24. Examination application.** Ethical dilemma: an assurance lead is asked to review the delivery plan
their own team drafted for a project six weeks from a gate, and the alternative reviewer is unavailable
for a month. Evidence selection: which two artefacts would show whether an award decision was taken by
an interested party. Scenario judgement: a steering member's former employer is one of three bidders and
the member offers to "stay in the room but not vote".

**25. Version and status.** Version 1.0 · **not yet approved** · effective on approval · **new standard** —
there is no predecessor in the v1.0 set, and its absence was the completeness finding this standard answers.
Amendment note: none.

---

## Domain 2 — Strategy, Selection and Business Alignment

### PCI STANDARD PCI-PML-STD-02.01 — Business-Case Integrity

**1. Normative requirement.** A credential holder must not present, endorse or rely on a business case
whose stated benefits, costs, assumptions, dependencies or option comparison they know, or on the
evidence available to them ought to know, to be materially misstated.

**2. Purpose.** The business case is the instrument that authorises everything downstream, and it is
authored by the party that benefits from approval. Optimism is not the failure; **asymmetric optimism**
is — a benefit case built on a best-case adoption rate against a cost case built on a mid-case
contingency, compared to a do-nothing option deliberately drawn badly. The result survives approval,
fails delivery, and cannot be corrected later because the approval it obtained has already consumed the
funds.

**3. Scope.** Every credential holder preparing, reviewing, recommending, approving or assuring a
business case, an option appraisal, a selection score or a re-approval, at initial approval and at every
subsequent gate at which the case is restated. It applies to the case for continuing as much as to the
case for starting.

**4. Defined terms.** *material* · *benefit* · *benefits baseline* · *evidence* · *sponsor* ·
*competent reviewer* · *decision owner*. Additionally, **option comparison** means the set of options
appraised on a common basis, including the do-minimum option; **asymmetric treatment** means applying a
different level of optimism, contingency, discount or evidence to one option or one side of the case
than to another.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-02.01-PR-01` — Symmetric treatment.** The credential holder must apply the same
  estimating basis, the same contingency approach, the same optimism adjustment and the same evidence
  standard to every option and to both sides of the case, and must state in the case that this was done
  and where it was not.
- **`PCI-PML-STD-02.01-PR-02` — Assumptions carry owner, source and date.** Every assumption on which a
  benefit or a cost materially depends must be recorded with a named owner, a source reference and the
  date it was last tested. An assumption with no owner is recorded as an open risk, not as an
  assumption.
- **`PCI-PML-STD-02.01-PR-03` — The do-minimum option is drawn honestly.** The do-minimum or
  do-nothing option must be appraised on the same basis as the preferred option, and its stated
  consequences must be sourced, not asserted.
- **`PCI-PML-STD-02.01-PR-04` — Restatement at each gate.** At each gate, the credential holder must
  restate the case against actual costs incurred, actual benefits evidence available and the current
  assumption set, and must state whether the case still holds. A case carried forward unchanged from a
  previous gate, with no restatement, does not satisfy this requirement.
- **`PCI-PML-STD-02.01-PR-05` — Kill criteria are stated before approval.** The case must state, before
  approval, the conditions under which the project would be stopped, and each such condition must be
  measurable against something the project will actually measure.

**6. Prohibited actions.** Presenting a benefit whose measurement method does not exist. Applying
contingency to one option and not to its comparators. Restating a case by moving the benefit date
rather than reducing the benefit. Presenting a sunk cost as a reason to continue. Removing a kill
criterion once it is approaching. Suppressing a material adverse assumption change between gates
because the next gate is close.

**7. Required evidence.** The versioned business case; the assumption register with owners, sources and
test dates; the option appraisal showing a common basis; the gate-by-gate restatement record; the
approved kill criteria and the measurement each is tested against; the record of every material change
to an assumption between gates and its notification.

**8. Responsible role.** The **sponsor** owns the business case and answers for its integrity. The
credential holder leading delivery answers for the accuracy of the delivery-side inputs — cost,
schedule, risk, resource and dependency — and for stating any known material misstatement to the
sponsor and, where the sponsor does not act, to the governing body.

**9. Approval authority.** The governing body approves the case at the authority level the delegation
schedule sets for its value and exposure. The sponsor approves restatements within the tolerance the
governing body sets. Neither the author of the case nor the delivery supplier may approve it.

**10. Independence requirement.** The gate restatement at `PR-04` must be reviewed by a competent
reviewer independent of the case's preparation and independent of the delivery organisation, at the
authority level the delegation schedule sets. On a project too small to supply such a reviewer
internally, independence may be met by a named reviewer from another part of the organisation who did
not contribute to the case, and the arrangement is recorded.

**11. Materiality or threshold.** This standard sets no percentage. A misstatement is material when it meets
the definition of *material* above — it could reasonably have changed the approval, its conditions or
the authority at which it was taken. The organisation's governance sets the documented approval
thresholds and the documented tolerance for restatement, and this standard requires that those documented
figures exist, are approved, and are applied. Where an organisation has no documented tolerance, the
credential holder must escalate every restatement.
*Six-person internal project:* the case is four pages, the assumption register is eight rows and the
kill criteria are two lines — and the restatement at each of two gates costs an afternoon.
*Multi-partner national programme:* the same obligations apply per component and at the programme
level, and the aggregation rule states which component restatements roll up to which programme
decision, so that a portfolio of restated-but-still-green components cannot conceal an amber programme.

**12. Exception and waiver.** An exception to `PR-04` — carrying a case forward without restatement —
may be approved only by the governing body, only where the interval since the last restatement is
shorter than the organisation's documented minimum restatement interval, and only with a recorded
statement that no material assumption has changed, signed by the sponsor. No exception is permitted to
the principal obligation.

**13. Escalation trigger.** A benefit whose owner declines to accept it. An assumption that has become
untrue and has not been reflected. A material adverse change with a gate more than the escalation time
away. Instruction to remove or weaken a kill criterion. Discovery that the option comparison was not
prepared on a common basis.

**14. AI application.** AI may test a case for internal arithmetic consistency, cross-check figures
between the narrative and the model, extract the assumption set and flag assumptions with no owner or
no source, compare the treatment of options for asymmetry, and track assumption changes between
versions.

**15. AI prohibition.** An AI system must not approve a business case, decide whether a benefit is
achievable, determine materiality, generate a benefit figure that is then presented as an estimate
without a named human's derivation, or author the kill criteria.

**16. AI verification.** **Independent recomputation plus source tracing.** A competent reviewer must
recompute the case's headline financial result by an independent method and reconcile it to the model
without unexplained difference, and must trace every benefit figure and every cost figure that carries
more than the documented materiality threshold to the source document and version it came from. Where
AI produced the option comparison, the reviewer must additionally apply **sensitivity analysis**: vary
each of the three assumptions with the largest effect and confirm the option ranking's stated
robustness.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of a business
  case as a governance instrument reviewed through the lifecycle · **EXT-028** · **Manual section 6 category
  3 — international voluntary standard** · currency checked 2026-08-03 · limitation: guidance;
  voluntary; not certifiable; not the source of this obligation.
- **ISO** · *ISO 21504, Guidance on portfolio management* · relied on for: the existence of a
  portfolio-level test that a component still merits its funding · **EXT-031**, recorded as **not
  independently verified — verify current requirements** · **Manual section 6 category 3** · limitation: as
  above, with an open verification status.
- **AACE International** · *Total Cost Management (TCM) Framework* · relied on for: the existence and
  purpose of a maturity-based progression in estimate reliability · **EXT-064**, recorded as **not
  independently verified — verify current requirements** · **Manual section 6 category 5 — professional
  framework** · limitation: **professional framework, never regulatory authority**; no accuracy range,
  class table or recommended-practice text is reproduced.

**18. Jurisdictional caution.** Public-sector appraisal rules, subsidy control, state-aid regimes,
listed-company disclosure duties and grant conditions impose their own appraisal, approval and
disclosure requirements, and misstating a case to obtain public funds can engage criminal law. Obtain
local legal advice where a case supports a regulated funding application or a market disclosure.

**19. Related PCI Standards.** `PCI-FND-STD-05`; `PCI-FND-STD-02`; `PCI-FND-STD-11`; `PCI-PML-STD-02.02`;
`PCI-PML-STD-03.03`; `PCI-PML-STD-15.02`; `PCI-PML-STD-16.03`.
**What this standard adds to `PCI-FND-STD-05`:** the foundational standard requires assumptions to be
transparent. This standard requires **symmetric treatment across options**, an **owned and dated
assumption
register**, an **honestly drawn do-minimum**, **restatement at every gate**, and **kill criteria fixed
before approval** — the specific mechanics by which a delivery business case goes wrong.

**20. Related Body of Knowledge content.** PML-AI · Domain 2 · KA 2.2 The business case and selection ·
topics 2.2.1 the business case as decision instrument, 2.2.2 options and appraisal, 2.2.3 selection and
prioritisation models; KA 2.3 topic 2.3.4 assumption and dependency management; KA 2.4 Strategic
termination · topics 2.4.2 sunk cost and escalation of commitment, 2.4.3 kill criteria and honest gates.

**21. Compliance test.** Compliance is demonstrated when the approved business case can be reconciled,
without unexplained difference, to: the current cost estimate and its basis, the approved risk and
contingency position, the benefits register with its named owners and its benefits baselines, the
assumption register with owners and test dates, and the current schedule's stated benefit start dates —
and when the gate file shows, for each gate held in the period, a dated restatement, a named
independent reviewer, and an explicit statement that the case does or does not still hold. A case that
cannot be reconciled to the benefits register's owners fails the test.

**22. Breach indicators.** Benefits with no named owner. Assumptions with no date. A do-minimum option
half a page long against a preferred option of twelve. Contingency present in one option only. A
benefit whose start date has moved at every gate while its value has not. Kill criteria removed between
versions. A restatement produced after the gate date.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Calculation review: a candidate is given an option comparison in which
one option carries contingency and another does not, and states the effect on the ranking. Scenario
judgement: an assumption underpinning 60 per cent of the benefit has become untrue four weeks before a
gate. Ethical dilemma: a sponsor asks for the kill criteria to be "softened" because the project is
close to one.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-02-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; five
process requirements separated out; symmetric treatment and honest do-minimum added as express
obligations; compliance test replaced with a reconciliation test.

---

### PCI STANDARD PCI-PML-STD-02.02 — Benefits Ownership

**1. Normative requirement.** A credential holder must not allow a benefit to be claimed in a business
case, a plan or a report unless a named individual outside the delivery team has accepted, in writing,
accountability for realising it.

**2. Purpose.** A benefit the delivery team owns is a benefit nobody owns, because the delivery team
disbands at handover and the benefit arrives afterwards. The failure this prevents is the standard one:
a business case full of benefits, a project delivered on time and to budget, and no measurable
improvement anywhere — with every party able to say truthfully that they did their part.

**3. Scope.** Every credential holder preparing, reviewing or approving a business case, a benefits
register, a benefits map, a benefits profile or a benefits realisation plan, on projects, programmes
and portfolios, at approval and at every gate. It applies to enabling outputs as much as to end
benefits.

**4. Defined terms.** *benefit* · *benefits baseline* · *sponsor* · *acceptance* · *evidence* ·
*material*. Additionally, **benefits owner** means the named individual, accountable in the receiving
or operating organisation, who accepts that the improvement will be realised and measured in their area
of accountability.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-02.02-PR-01` — Named written acceptance.** Every benefit in the register must carry
  the name and role of its benefits owner and a dated written acceptance by that individual. A name
  entered by the project on the owner's behalf does not satisfy this requirement.
- **`PCI-PML-STD-02.02-PR-02` — A measurable definition before approval.** Every benefit must carry,
  before the case is approved, the measure that will evidence it, the source system or method that will
  produce the measure, the benefits baseline value, and the date the baseline was measured.
- **`PCI-PML-STD-02.02-PR-03` — Baseline measured before the change.** The benefits baseline must be
  measured before the change is made. A baseline reconstructed after go-live must be labelled as
  reconstructed, with the method stated, and must not be presented as a measurement.
- **`PCI-PML-STD-02.02-PR-04` — Refusal is recorded, not absorbed.** Where a proposed benefits owner
  declines to accept a benefit, the credential holder must record the refusal and its stated reason and
  escalate it, and must not transfer the benefit to the delivery team, to the sponsor by default, or to
  an unnamed function.

**6. Prohibited actions.** Listing a benefit with the delivery team, the project, the PMO or "the
business" as its owner. Claiming a benefit whose measure does not yet exist. Reconstructing a baseline
after the change and presenting it as measured. Double-counting the same improvement across two
components of a programme. Presenting an enabling output — a system delivered, a process documented —
as a benefit.

**7. Required evidence.** The benefits register with, per benefit: owner name and role, dated written
acceptance, measure, source system, benefits baseline value and its measurement date, target value and
target date. The refusal record and its escalation for any declined benefit. The double-count check for
programmes.

**8. Responsible role.** The **sponsor** is accountable for securing a benefits owner for every claimed
benefit. The credential holder leading delivery is accountable for refusing to carry an unowned benefit
in any document they issue, and for escalating any that remains unowned.

**9. Approval authority.** The governing body approves the benefits register at case approval. The
sponsor approves changes to a benefit's target value or date within the documented tolerance; changes
beyond it go to the governing body. The benefits owner alone accepts ownership; nobody may accept on
their behalf.

**10. Independence requirement.** The benefits owner must be **independent of the delivery team** in the
sense defined above: not accountable for delivering the output, and answering in the receiving or
operating organisation. Measurement of realised benefits must be produced by, or verified by, a party
independent of the delivery team — see `PCI-PML-STD-16.03`.

**11. Materiality or threshold.** This standard sets no threshold, because ownership is binary: a benefit is
owned or it is not. The organisation's governance sets which benefits are individually tracked and
which are aggregated, and this standard requires that documented rule to exist and to be applied. A benefit
below the documented individual-tracking threshold must still carry a named owner for its aggregate.
*Six-person internal project:* one benefit, one owner, one measure already produced monthly by an
existing report — the whole obligation is three cells in a table.
*Multi-partner national programme:* the register is held at programme level, benefits owners sit in the
operating organisations rather than in any partner, and the double-count check across components is a
standing programme obligation because components have an incentive to claim the same improvement.

**12. Exception and waiver.** An exception permitting a benefit to be carried without an accepted owner
may be approved only by the governing body, only until the next gate, and only where the case records
that the benefit is excluded from the approval arithmetic while unowned. No exception permits an unowned
benefit to count towards an approval.

**13. Escalation trigger.** A proposed benefits owner declining acceptance. A benefit whose measure
cannot be produced by any existing or planned system. A benefit still unowned at the gate at which the
case is approved. Discovery that the same improvement is claimed by two components.

**14. AI application.** AI may build a benefits register from case documents, flag benefits with no
owner, no measure, no baseline or no date, detect candidate double-counts across a programme by
clustering benefit descriptions, and check that each stated measure maps to a named source system.

**15. AI prohibition.** An AI system must not accept ownership of a benefit, assign an owner, decide
whether a benefit is realisable, or generate a baseline value.

**16. AI verification.** **Source tracing plus named approval.** Every baseline value produced with AI
assistance must be traced by a competent reviewer to the source system extract, with the extract date
and the query or filter recorded, and the benefits owner must confirm the value in writing before it is
recorded as the benefits baseline. Every AI-flagged double-count must be confirmed by a named human
against both components' registers.

**17. External reference.**
- **ISO** · *ISO 21503, Guidance on programme management* · relied on for: the existence of a
  programme-level concern with benefits that no single component delivers · **EXT-030**, recorded as
  **not independently verified — verify current requirements** · **Manual section 6 category 3 — international
  voluntary standard** · limitation: guidance; voluntary; not certifiable; open verification status.
- **ISO** · *ISO 21504, Guidance on portfolio management* · relied on for: the existence of a portfolio
  concern with the benefits a component is funded to produce · **EXT-031**, **not independently
  verified** · **Manual section 6 category 3** · limitation: as above.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of benefit
  identification and realisation as lifecycle activities · **EXT-028** · **Manual section 6 category 3** ·
  currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.

**18. Jurisdictional caution.** Where benefits are claimed to a grant funder, a regulator, a market or a
tax authority, the accuracy of the claim can engage funding-condition, disclosure or fraud law. Obtain
local legal advice before a benefits statement is made outside the organisation.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-01`; `PCI-FND-STD-12`; `PCI-PML-STD-02.01`;
`PCI-PML-STD-15.02`; `PCI-PML-STD-16.02`; `PCI-PML-STD-16.03`.

**20. Related Body of Knowledge content.** PML-AI · Domain 2 · KA 2.3 Benefits, value and sustainability
· topics 2.3.1 benefits mapping, 2.3.2 measures and baselines. Also Domain 15 KA 15.2 benefits and
portfolio balancing; Domain 16 KA 16.4 benefits measurement.

**21. Compliance test.** Compliance is demonstrated when a reviewer can open the benefits register and
find, for **every** benefit carried in the approved case: a named individual and their role; a written
acceptance dated on or before the approval date; a stated measure; a named source system that already
produces or is planned to produce it; a benefits baseline value with the date it was measured; and a
target with a date. The reviewer must then contact a stated sample of named owners and confirm each is
aware of and accepts the accountability recorded against them. **A register in which any sampled owner
does not recognise their own entry fails the test**, regardless of the paperwork's completeness.

**22. Breach indicators.** Owner cells containing a team, a function or a job family rather than a
person. Acceptance dates after the approval date. Baselines dated after go-live. The same improvement
in two components' registers. A benefit whose measure is "to be defined". Owners who, when asked, say
the project told them they were the owner.

**23. Consequence within PCI authority.** Correction required; additional review; escalation;
examination failure; ethics review; certification investigation; suspension or withdrawal — each
subject to due process and a right of appeal.

**24. Examination application.** Evidence selection: which artefact establishes that a benefit is owned.
Scenario judgement: the proposed benefits owner refuses, four days before the approval gate, and the
sponsor proposes to record the PMO as owner. Calculation review: a programme's benefits total exceeds
the sum of its components' distinct improvements.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-02-02` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the
benefits-baseline sense of *baseline* separated from the control sense per the suite terminology audit;
the compliance test strengthened to require confirmation with sampled owners, which is what makes the
standard provable rather than documentary.

---

## Domain 3 — Governance, Organisation and Decision Rights

### PCI STANDARD PCI-PML-STD-03.01 — Governance Authority Before Commitment

**1. Normative requirement.** A credential holder must not commit an organisation's funds, contractual
obligations or people to delivery before the governance arrangements for that delivery — the named
sponsor, the named decision bodies, the delegation schedule and the gate points — are documented and
approved.

**2. Purpose.** Governance retrofitted after commitment is governance that cannot say no, because the
money is spent, the contract is signed and the team is hired. The failure this prevents is the project
that reaches its first gate with nothing the gate can decide.

**3. Scope.** Every credential holder in a position to commit or to recommend commitment — funds,
contracts, orders, hires or secondments — on projects, programmes and portfolios, in every delivery
model. It applies to the initial commitment and to any commitment following a material change in the
delivery structure, including the addition of a delivery partner.

**4. Defined terms.** *governance* (project sense) · *sponsor* · *delegation schedule* · *gate* ·
*decision owner* · *material*. Additionally, **commitment** means an act that creates a legal, financial
or employment obligation the organisation cannot withdraw from without cost.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-03.01-PR-01` — The four artefacts exist and are dated.** Before commitment, the
  credential holder must hold: a named sponsor with a written appointment; the decision bodies with
  their named members and their terms of reference; the delegation schedule; and the gate points with
  their criteria. Each artefact must carry a date and an approver.
- **`PCI-PML-STD-03.01-PR-02` — The gate can stop the work.** The credential holder must confirm, in
  writing, that each defined gate carries the power to stop, hold or redirect, and must record any gate
  that does not, because a review that cannot stop the work is not a gate and must not be described as
  one.
- **`PCI-PML-STD-03.01-PR-03` — Governance proportionate and recorded as such.** The credential holder
  must record the tailoring decision — which governance elements apply at what depth for this project's
  size and exposure — with the sponsor's approval, so that light governance is a decision rather than an
  omission.
- **`PCI-PML-STD-03.01-PR-04` — Re-approval on structural change.** Where the delivery structure changes
  materially — a partner is added, a component is transferred, the sponsor changes, or the funding route
  changes — the credential holder must obtain re-approval of the four artefacts before further
  commitment.

**6. Prohibited actions.** Committing funds or contracts against a governance structure that is drafted
but not approved. Describing a review with no stopping power as a gate. Operating with an unnamed
sponsor or a sponsor who has not accepted the appointment. Allowing a delivery partner's internal
governance to substitute for the client organisation's, without an approved instrument saying so.

**7. Required evidence.** The dated sponsor appointment; the terms of reference of each decision body
with its membership; the approved delegation schedule; the gate definitions with criteria and stopping
powers; the tailoring decision and its approval; re-approval records at each structural change.

**8. Responsible role.** The named credential holder leading the project, for holding and applying the
artefacts. The **sponsor**, for securing their approval. The governing body, for approving them.

**9. Approval authority.** The governing body approves the delegation schedule and the gate points. The
sponsor approves the tailoring decision within the governing body's stated limits. The project leader
approves nothing in this standard.

**10. Independence requirement.** The approval of the governance arrangements must be taken by a body
independent of the delivery organisation in the sense defined above. Where a delivery partner sits on
the approving body, that partner's members must abstain from the approval of the delegation schedule
under `PCI-PML-STD-01.03-PR-04`.

**11. Materiality or threshold.** This standard sets no financial trigger for "commitment", because
organisations define commitment differently — order, contract signature, purchase requisition, hire.
The credential holder must apply the organisation's documented definition of commitment and its
documented approval thresholds; where none is documented, the credential holder must escalate before
any commitment.
*Six-person internal project:* the four artefacts are one page in total — a named sponsor, a fortnightly
review with the power to stop, a three-row delegation table and two gates. The tailoring decision is one
sentence.
*Multi-partner national programme:* the artefacts exist at programme and component level, the delegation
schedule states the aggregation rule across partners, and each partner's own governance is mapped to
the programme's rather than substituted for it.

**12. Exception and waiver.** An exception permitting commitment before the artefacts are approved may
be approved only by the governing body, only for a stated and capped amount, only for a stated period,
on written justification, with the compensating control that every commitment made under the exception
is individually reported to the governing body within its stated reporting time.

**13. Escalation trigger.** Pressure to commit before governance is approved. A gate defined without
stopping power. A sponsor who has not accepted the appointment. A structural change made without
re-approval. A delivery partner asserting that its own governance suffices.

**14. AI application.** AI may check a governance pack for missing artefacts, missing dates and missing
approvers; test gate criteria for assessability and flag the unmeasurable ones; check a delegation
schedule for decision classes with no accountable role or with more than one; and model the latency a
proposed governance cadence produces.

**15. AI prohibition.** An AI system must not approve governance arrangements, appoint a sponsor,
determine tailoring, or decide that a gate's criteria are assessable.

**16. AI verification.** **Clause-to-summary comparison plus named approval.** Where AI has summarised or
assembled the governance pack, a competent reviewer must compare each summarised item against the
source instrument it claims to summarise — the appointment letter, the terms of reference, the schedule
— and confirm the summary states the same authority, the same limits and the same names. The sponsor
must then approve the pack by name.

**17. External reference.**
- **ISO** · *ISO 21505, Guidance on governance of projects, programmes and portfolios* · relied on for:
  the existence of a governance framework distinct from management · **EXT-032**, **not independently
  verified — verify current requirements** · **Manual section 6 category 3 — international voluntary standard**
  · limitation: guidance; voluntary; not certifiable; open verification status.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of pre-delivery
  governance establishment · **EXT-028** · **Manual section 6 category 3** · currency checked 2026-08-03 ·
  limitation: guidance; voluntary; not certifiable.
- **OECD (G20/OECD)** · *G20/OECD Principles of Corporate Governance* · relied on for: the existence of
  a board-level expectation that authority is defined before it is exercised · **EXT-128** · **Manual section 6
  category 10 — illustrative practice** · currency checked 2026-08-03 · limitation: a Council
  Recommendation; non-binding; **not legislation**.

**18. Jurisdictional caution.** Company law, public-procurement law, financial regulation, delegated
authority under statute and grant conditions determine who may lawfully bind an entity, and a
professionally correct delegation schedule can still be legally ineffective. Obtain local legal advice
on authority to contract for the specific entity and funding route.

**19. Related PCI Standards.** `PCI-FND-STD-01`; `PCI-FND-STD-12`; `PCI-PML-STD-03.02`;
`PCI-PML-STD-03.03`; `PCI-PML-STD-03.04`; `PCI-PML-STD-13.01`.

**20. Related Body of Knowledge content.** PML-AI · Domain 3 · KA 3.1 Governance models · topics 3.1.1
what governance is for, 3.1.2 structures across organisational forms, 3.1.3 governance in agile and
hybrid environments; KA 3.3 topic 3.3.1 stage gates. Also Domain 4 KA 4.1 charter and management plans.

**21. Compliance test.** Compliance is demonstrated when a reviewer, taking the date of the earliest
commitment recorded in the project's financial or contractual system, finds that the sponsor
appointment, the decision-body terms of reference, the delegation schedule and the gate definitions are
each dated **on or before** that date and each carry a named approver; and when each gate definition
states, in terms, the power to stop, hold or redirect. A commitment date earlier than any of the four
artefact dates fails the test unless a governing-body exception covering that commitment exists.

**22. Breach indicators.** Governance packs dated after the first invoice. Gates described as
"checkpoints" with no stated power. A sponsor named in a slide and nowhere else. A delegation schedule
with decision classes carrying two accountable roles or none. Partner governance cited in place of the
client's. Terms of reference with no membership list.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; certification investigation; suspension or withdrawal — each subject to
due process and a right of appeal.

**24. Examination application.** Scenario judgement: a leader is asked to place a long-lead order two
weeks before the governance pack is due for approval. Evidence selection: which three artefacts must
predate the first commitment. Escalation decision: a gate has been defined with no power to stop.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-03-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed;
the "gate can stop the work" obligation and the re-approval-on-structural-change obligation added as
express process requirements; compliance test replaced with a date-ordering test that a reviewer can
perform from two systems.

---

### PCI STANDARD PCI-PML-STD-03.02 — Decision Rights and Delegated Authority

**1. Normative requirement.** A credential holder must take every decision at the authority level the
organisation's documented delegation schedule assigns to it, and must not take, split, defer or
aggregate a decision so as to avoid the authority the schedule requires.

**2. Purpose.** Delegation schedules fail in two directions, and both are common. Upward: everything
goes to the steering committee, the committee becomes a queue, and the escalation that matters arrives
behind forty that do not. Downward: a decision above the limit is split into three below it, or taken
under an urgency provision that becomes ordinary practice. This standard addresses the second, which is the
one that produces exposure nobody approved.

**3. Scope.** Every credential holder taking, recommending, ratifying or recording a delivery decision,
on projects, programmes and portfolios. It applies to financial, contractual, scope, schedule, risk,
resource, quality and release decisions alike, and to urgent decisions taken outside the ordinary
cadence.

**4. Defined terms.** *delegation schedule* · *decision owner* · *escalation threshold* · *material* ·
*decision record*. Additionally, **aggregation rule** means the schedule's provision that related
decisions summing above a stated value within a stated period require the authority the schedule assigns
to the aggregate; **relatedness class** means the set over which that rule sums.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-03.02-PR-01` — Single accountable role per class.** The credential holder must confirm,
  before relying on the delegation schedule, that each decision class in it carries exactly one
  accountable role, and must escalate any class carrying two or none.
- **`PCI-PML-STD-03.02-PR-02` — Universal registration.** Every decision in a class covered by the
  schedule must generate a decision-record entry **regardless of its value**, so that the aggregation
  rule has inputs. A rule that sums decisions it cannot see is not a control.
- **`PCI-PML-STD-03.02-PR-03` — Aggregation applied by what the change touches.** The credential holder
  must apply the schedule's aggregation rule using its documented relatedness class, and where the
  organisation defines relatedness by what the changes touch — the same deliverable, the same assured
  control, the same interface — must not substitute a broader class such as requester or budget line.
- **`PCI-PML-STD-03.02-PR-04` — Urgency provisions are time-boxed and reported.** A decision taken under
  an urgency or out-of-cycle provision must be recorded as such, with the reason, and must be reported
  to the authority that would ordinarily have taken it within the time the schedule states.
- **`PCI-PML-STD-03.02-PR-05` — Escalation carries a destination and a time.** The credential holder
  must not rely on an escalation threshold that states no named destination and no time; where one is
  encountered, they must escalate the defect itself.

**6. Prohibited actions.** Splitting a decision to bring each part below a limit. Recording a decision
at a lower value than it carries. Using an urgency provision as ordinary practice. Ratifying, after the
event, a decision taken without authority, without recording that it was taken without authority.
Widening a relatedness class so that the aggregation rule trips constantly and is then disapplied.

**7. Required evidence.** The approved delegation schedule with its aggregation rule and relatedness
class; the decision record showing universal registration; aggregation calculations for any class that
approached or crossed the rule; urgency-decision records with reasons and their reporting; escalation
records showing destination and elapsed time against the stated time.

**8. Responsible role.** The named credential holder for decisions within their authority; the named
role the schedule assigns for each other class. The governing body owns the schedule itself.

**9. Approval authority.** The governing body approves the delegation schedule, its thresholds, its
aggregation rule and its relatedness class. The sponsor approves nothing in this standard that would raise
their own authority.

**10. Independence requirement.** Not applicable to the taking of a decision within delegated authority,
because delegation exists precisely so that the accountable party decides; independence attaches to the
periodic testing of the schedule's operation, which must be performed by a competent reviewer
independent of the decisions tested.

**11. Materiality or threshold.** **This standard invents no figure and no percentage**, and doing so would be
wrong: delegated authority levels are set by an organisation's own governance against its own balance
sheet, risk appetite and decision rate. What this standard requires is that documented thresholds, a
documented aggregation rule, a documented relatedness class and a documented period exist, and are
applied. Where a threshold has been set without reference to the observed decision rate, so that it
either never trips or trips on ordinary traffic, the credential holder must record that finding and
escalate it — a rule that cannot fire and a rule that always fires are equally useless, and both look
like controls.
*Six-person internal project:* the schedule is three rows; the aggregation rule sums changes touching
the same deliverable within a month; universal registration is the issue tracker they already use.
*Multi-partner national programme:* the schedule exists at each tier, and the aggregation rule states
explicitly whether it sums across partners — the case in which the exposure the rule exists to catch
arrives as one coherent change split between four organisations.

**12. Exception and waiver.** An exception permitting a decision above the delegated level may be
approved only by the authority the schedule assigns to that level or above, in writing, before the
decision, and never retrospectively by the person who took it. A decision already taken without
authority is a breach, is recorded as one, and is then ratified or reversed by the proper authority —
ratification does not convert it into a compliant decision.

**13. Escalation trigger.** A decision class with two accountable roles or none. A proposal to split a
decision. An urgency provision used more than the number of times the schedule permits. An aggregation
that has crossed the rule. A threshold that has not fired in a period in which the observed decision
rate says it should have.

**14. AI application.** AI may extract a decision register from minutes and flag entries missing an
owner, a date or a versioned information reference; run integrity checks across a decision-rights matrix
for classes with two accountable roles or none; compute aggregations across a relatedness class;
identify decisions being re-decided by clustering decision text across a long log; and model the latency
a governance cadence produces.

**15. AI prohibition.** An AI system must not take a delegated decision, set or change a threshold,
determine a relatedness class, or ratify a decision taken without authority.

**16. AI verification.** **Independent recomputation plus sampling with a stated basis.** Every
aggregation computed with AI assistance must be recomputed by hand by a competent reviewer before it is
relied on to escalate or not to escalate — the arithmetic is a handful of operations and none of it
should be taken on trust. Each quarter, a competent reviewer must draw a stated sample of decision
entries and confirm against source minutes that the recorded value, class and authority match.

**17. External reference.**
- **ISO** · *ISO 21505, Guidance on governance of projects, programmes and portfolios* · relied on for:
  the existence of defined decision authority within governance · **EXT-032**, **not independently
  verified** · **Manual section 6 category 3 — international voluntary standard** · limitation: guidance;
  voluntary; not certifiable; open verification status.
- **Project Management Institute** · *A Guide to the Project Management Body of Knowledge (PMBOK
  Guide)* · relied on for: the existence of change and decision authority structures in professional
  practice · **EXT-060** · **Manual section 6 category 5 — professional framework** · currency checked
  2026-08-03 · limitation: **a professional framework, never regulatory authority**; no edition is
  asserted and no text is reproduced.
- **COSO** · *Internal Control — Integrated Framework* · relied on for: the existence of an
  authorisation control concept · **EXT-084** · **Manual section 6 category 10 — illustrative practice** ·
  currency checked 2026-08-03 · limitation: voluntary in itself, though widely imported by regulators;
  it is not legislation and this standard does not rely on it for any requirement.

**18. Jurisdictional caution.** Whether a person has legal authority to bind an entity is determined by
company law, delegated statutory authority, procurement regulation and the entity's own constitution,
not by a delegation schedule. A decision within delegated professional authority can still be legally
ineffective. Obtain local legal advice on binding authority.

**19. Related PCI Standards.** `PCI-FND-STD-04`; `PCI-FND-STD-11`; `PCI-FND-STD-12`; `PCI-FND-STD-13`;
`PCI-PML-STD-01.01`; `PCI-PML-STD-03.01`; `PCI-PML-STD-04.01`; `PCI-PML-STD-08.01`.

**20. Related Body of Knowledge content.** PML-AI · Domain 3 · KA 3.2 Sponsorship and steering · topic
3.2.3 decision authorities and thresholds; KA 3.3 · topics 3.3.3 escalation design, 3.3.4 auditability
and the decision record. Also Domain 4 KA 4.3 topic 4.3.3 baseline maintenance and KA 4.4 the change
board.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) obtain the approved
delegation schedule and confirm every decision class carries exactly one accountable role; (b) extract
every decision in the period from the decision record and confirm that each was taken at or below the
authority the schedule assigns to its class and value; (c) recompute the aggregation over the documented
relatedness class and period and confirm that every aggregate crossing the rule was taken at the
aggregate authority; (d) confirm that decisions below the individual threshold were nonetheless
registered, by comparing the decision-record count against the change, order or issue counts in the
operational systems; and (e) confirm every urgency decision was reported to the ordinary authority
within the stated time. A material gap between (d)'s two counts fails the test, because an aggregation
rule without universal registration cannot see its own inputs.

**22. Breach indicators.** Three change requests to the same deliverable in one month, each just below
a limit. A decision register materially shorter than the change log. Urgency decisions clustering in the
week before each steering meeting. An aggregation rule that has never fired. A relatedness class widened
shortly after the rule first fired. Ratifications recorded as approvals.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Calculation review: seven related changes across a quarter, three of
them below the individual limit, and the candidate determines the authority required. Scenario judgement:
a supplier proposes to raise two orders rather than one. Escalation decision: a decision class is found
to carry two accountable roles a week before the gate.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-03-02` v1.0. Amendment note: renumbered and restructured; legislative drafting removed;
universal registration, the relatedness-class discipline and the time-boxing of urgency provisions added
as express process requirements; element 11 rewritten to require documented thresholds rather than to
state any.

---

### PCI STANDARD PCI-PML-STD-03.03 — Gate Evidence and the Gate Decision

**1. Normative requirement.** A credential holder must not recommend or take a gate decision except on
evidence that is dated, attributable, version-identified and assessed against criteria published before
the evidence was assembled.

**2. Purpose.** A gate whose criteria are written after the evidence is a gate that cannot fail, and a
gate that cannot fail is a governance cost with no governance product. The failure this prevents is the
retrospectively satisfiable gate: criteria adjusted to the evidence available, conditions attached with
no owner and no date, and a "conditional pass" that is a pass.

**3. Scope.** Every credential holder preparing, reviewing, chairing, recommending, deciding or assuring
a gate decision at any stage of a project, programme or portfolio lifecycle, including release gates in
adaptive delivery and investment gates in a portfolio.

**4. Defined terms.** *gate* · *evidence* · *decision owner* · *material* · *competent reviewer* ·
*independent* · *acceptance*. Additionally, **conditional pass** means a continuation decision taken
subject to stated conditions; **condition-closure** means the verified completion of such a condition.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-03.03-PR-01` — Criteria published first.** The gate criteria must be published, dated
  and version-identified before the evidence supporting the gate is assembled, and the credential holder
  must not amend them after assembly without the approving authority's recorded decision and a statement
  of what changed and why.
- **`PCI-PML-STD-03.03-PR-02` — Criteria are assessable.** Each criterion must be expressed so that two
  competent reviewers assessing the same evidence reach the same answer. The credential holder must
  identify any criterion that cannot be assessed and must have it replaced or removed before the gate.
- **`PCI-PML-STD-03.03-PR-03` — Evidence carries its provenance.** Every item in the gate pack must
  carry its date, its author or source system, and its version. An undated extract, an unversioned
  model output and a dashboard state that cannot be reproduced must not be admitted as evidence.
- **`PCI-PML-STD-03.03-PR-04` — Conditions carry owner, date and consequence.** Every condition attached
  to a conditional pass must carry a named owner, a due date and the stated consequence of non-closure,
  and the credential holder must report condition-closure status at the following gate.
- **`PCI-PML-STD-03.03-PR-05` — Dissent is recorded.** Where any member of the deciding body disagrees
  with the decision, the disagreement and its reason must be recorded in the decision record. A decision
  recorded as unanimous when it was not is a false record.
- **`PCI-PML-STD-03.03-PR-06` — The criteria are set by the authority, not by the project.** The gate
  criteria must be approved, before publication, by the authority that holds the gate decision or by the
  authority the delegation schedule assigns to set them, and the approval must name that authority and
  its date. **The project being gated must not be the approver of its own gate criteria.** Where the
  project drafts them, the record must show that fact and the approving authority's assent to them as
  published.

**6. Prohibited actions.** Writing or altering criteria to fit the evidence. Admitting evidence with no
date, no author and no version. Attaching a condition with no owner. Passing a gate on a promise that
the evidence will follow. Recording a conditional pass and then never reporting closure. Presenting the
absence of an assurance finding as an assurance opinion.

**7. Required evidence.** The published criteria with their version and publication date, and the
approval of those criteria by the authority required by `PR-06`; the gate pack with per-item
provenance; the decision record with the named decision owner, the decision, the conditions with owners
and dates, and any recorded dissent; the condition-closure report at the following gate; the
independent reviewer's opinion where one is required.

**8. Responsible role.** The named chair of the gate body decides and answers for the decision. The
named credential holder leading the project answers for the completeness and provenance of the pack and
for stating any material matter the pack does not cover.

**9. Approval authority.** The gate body's named chair, at the authority level the delegation schedule
assigns. A condition may be waived only by the authority that imposed it. The project must not close its
own gate condition where the condition concerns an assurance, safety, licence or acceptance matter owned
elsewhere.

**10. Independence requirement.** Where the delegation schedule requires assurance at the gate, the
assurance opinion must be given by a competent reviewer independent of the pack's preparation, tested by
the name-matching rule in `PCI-PML-STD-01.03-PR-06`. **An assurance function must not opine on a plan it
helped produce**, and the opinion must state positively that it did not.

**11. Materiality or threshold.** This standard sets no number. Which gates require independent assurance,
what evidence depth each gate requires, and what tolerance triggers a hold are set by the organisation's
governance and must be documented and applied. The credential holder must apply the documented depth;
where a gate's evidence requirement is not documented, they must record the gap and escalate it.
*Six-person internal project:* two gates, criteria of five lines each published at kick-off, a pack of
six artefacts each carrying a date and a version, and assurance provided by a named colleague from
another team.
*Multi-partner national programme:* gates at component and programme level, criteria published against
a common template so component evidence is comparable, and an assurance opinion at the programme gate
independent of every partner rather than of one.

**12. Exception and waiver.** An exception permitting a gate to proceed on incomplete evidence may be
approved only by the gate body's chair, only where the missing evidence is identified item by item, only
with a named owner and a date for each missing item, and only where the decision record states what the
decision would have been had the missing evidence been adverse. No exception permits criteria to be
written after the evidence.

**13. Escalation trigger.** A request to alter criteria after evidence assembly. A criterion that cannot
be assessed. Evidence offered without provenance. A condition from a previous gate still open at the
next. An assurance opinion whose author appears in the pack's authorship record. Pressure to record a
decision as unanimous when dissent was expressed.

**14. AI application.** AI may test criteria for assessability and flag the unmeasurable ones; check a
gate pack for items missing a date, author or version; reconcile figures between pack documents;
track condition closure and age open conditions; and detect criteria that changed between the published
version and the version used.

**15. AI prohibition.** An AI system must not take a gate decision, give an assurance opinion, close a
condition, or author a decision record entry wholesale — the record must show that a person applied
judgement, and a wholly generated record cannot evidence that.

**16. AI verification.** **Reconciliation plus sampling with a stated basis.** Every AI-produced gate
reconciliation must be reproduced by hand for the figures that carry the decision. Every AI-flagged
defect must be confirmed by a named human before it is reported as a finding. Where AI summarised a
source document into the pack, a competent reviewer must perform a **clause-to-summary comparison**
against the source and confirm the summary states the same position, including its qualifications.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of stage-gate
  style continuation decisions in project governance · **EXT-028** · **Manual section 6 category 3 —
  international voluntary standard** · currency checked 2026-08-03 · limitation: guidance; voluntary;
  not certifiable.
- **ISO** · *ISO 9001, Quality management systems — Requirements* · relied on for: the existence of a
  certifiable requirement for documented information and its control · **EXT-033** · **Manual section 6
  category 3 — international voluntary standard** · currency checked 2026-08-03 · limitation: **this is
  the certifiable one in the ISO 9000 family**, but certification is voluntary and concerns a management
  system; a project is not certified against it and this standard does not import its requirements.
- **ISO** · *ISO 21505, Guidance on governance* · relied on for: the existence of assurance lines within
  governance · **EXT-032**, **not independently verified** · **Manual section 6 category 3** · limitation: as
  above, with an open verification status.

**18. Jurisdictional caution.** Where a gate decision concerns a safety case, a licence, a permission, a
consent or a statutory notification, the effective decision belongs to the body empowered to make it and
a professional gate decision does not substitute for it. Obtain local legal advice on which approvals a
specific project requires and who may grant them.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-06`; `PCI-FND-STD-12`; `PCI-PML-STD-01.03`;
`PCI-PML-STD-03.01`; `PCI-PML-STD-09.01`; `PCI-PML-STD-16.01`.

**20. Related Body of Knowledge content.** PML-AI · Domain 3 · KA 3.3 Assurance, gates and escalation ·
topics 3.3.1 stage gates, 3.3.2 assurance lines, 3.3.4 auditability and the decision record. Also
Domain 9 KA 9.2 assurance and control; Domain 16 KA 16.1 readiness.

**21. Compliance test.** Compliance is demonstrated when a reviewer can, for each gate held in the
period: (a) obtain the criteria and confirm their publication date and version precede the earliest date
on any evidence item in the pack; (b) confirm every pack item carries a date, an author or source system
and a version; (c) take any two criteria at random, apply them to the pack, and reach the same answer
the gate body recorded; (d) find every condition attached to the decision carrying a named owner, a date
and a consequence; (e) find, in the following gate's pack, a closure status for each of those
conditions; and (f) find, on the criteria themselves, the approval required by `PR-06`, naming an
approving authority who is neither the project being gated nor a person reporting to it for the purpose
of that gate. A gate whose criteria post-date its evidence fails the test outright, and so does a gate
whose criteria the project set for itself — **criteria published in time but written by the party they
judge produce the same unfailable gate as criteria written late.**

**22. Breach indicators.** Criteria version dates later than the pack. Criteria phrased as
"satisfactory", "adequate" or "on track" with no stated test. Conditions with no owner. Conditional
passes never revisited. Gate packs assembled by the same person who signs the assurance opinion.
Decisions recorded as unanimous in bodies where dissent is known to have been expressed. Evidence items
that are screenshots with no source reference.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Evidence selection: from a pack of nine items, the candidate identifies
the three that are not evidence and states why. Scenario judgement: the chair proposes to amend a
criterion the pack does not meet. Escalation decision: a condition from two gates ago has never been
closed and the project is at its final gate.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-03-03` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the
criteria-before-evidence ordering, criterion assessability, per-item provenance, condition fields and
recorded dissent separated into five process requirements; compliance test replaced with a date-ordering
and re-application test two reviewers can independently perform. **Stage 9 amendment:** the standard fixed
*when* the criteria are published but not *who* sets them, so a project could publish its own
trivially satisfiable criteria before assembling evidence and comply in full while producing the
unfailable gate element 2 describes; `PR-06` and step (f) of element 21 require the criteria to be
approved by the authority that holds the gate decision.

---

### PCI STANDARD PCI-PML-STD-03.04 — Sponsor Accountability

**1. Normative requirement.** A credential holder must not lead delivery on a project, programme or
portfolio that has no named individual sponsor who has accepted the sponsorship accountability in
writing.

**2. Purpose.** The sponsor is the only role that can resolve the questions delivery cannot: whether the
business case still holds, whether a benefit is still wanted, whether to stop. A vacant, nominal or
rotating sponsorship transfers those questions to the delivery leader, who has neither the authority nor
the standing to answer them, and the project continues by default long after the reason for it has gone.

**3. Scope.** Every credential holder in a delivery leadership role, from appointment to closure,
including periods in which a sponsor is absent, has changed, or is acting. It governs the credential
holder's obligation to secure, use and escalate about sponsorship; it does not purport to govern the
sponsor, who might not be a credential holder.

**4. Defined terms.** *sponsor* (delivery sense) · *decision owner* · *benefit* · *material* ·
*escalation threshold* · *governance*. Additionally, **reserved sponsor decision** means a decision the
delegation schedule assigns to the sponsor and to nobody below them.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-03.04-PR-01` — Written acceptance held.** The credential holder must hold the sponsor's
  dated written acceptance of the sponsorship accountability, naming the individual, before delivery
  commitment, and must escalate where it does not exist.
- **`PCI-PML-STD-03.04-PR-02` — The reserved-decision list is agreed and used.** The credential holder
  must agree with the sponsor, in writing, the list of reserved sponsor decisions drawn from the
  delegation schedule, and must route each such decision to the sponsor rather than deciding it.
- **`PCI-PML-STD-03.04-PR-03` — A stated decision service level.** The credential holder must record the
  time within which the sponsor undertakes to decide a reserved matter, and must escalate to the
  governing body where a reserved decision exceeds that time by the margin the escalation threshold
  states.
- **`PCI-PML-STD-03.04-PR-04` — Sponsor change is a governance event.** On a change of sponsor, the
  credential holder must obtain the incoming sponsor's written acceptance, record a handover of the open
  reserved decisions and the current case position, and must not treat the vacancy period as a period in
  which reserved decisions may be taken below the sponsor.

**6. Prohibited actions.** Taking a reserved sponsor decision because the sponsor is unavailable.
Recording a committee as the sponsor. Continuing delivery through an unfilled sponsorship without
escalating. Accepting a sponsor's verbal instruction on a reserved matter and recording it as a decision
without written confirmation. Allowing a delivery supplier to supply the sponsor.

**7. Required evidence.** The sponsor's dated written acceptance; the agreed reserved-decision list; the
decision-service-level record and the log of reserved decisions with elapsed times; escalations raised
on breach; sponsor-change handover records.

**8. Responsible role.** The named credential holder leading delivery, for every obligation in this standard.
The governing body appoints the sponsor and answers for the appointment.

**9. Approval authority.** The governing body appoints and replaces the sponsor and approves the
reserved-decision list. The sponsor approves the decision service level. The credential holder approves
nothing here.

**10. Independence requirement.** The sponsor must be independent of the delivery supplier: not employed
by, contracted to, or remunerated by reference to the performance of the organisation delivering the
work. Where the organisation is delivering to itself, the sponsor must sit outside the delivery
reporting line for the purpose of the project.

**11. Materiality or threshold.** This standard sets no number. The reserved-decision list and the decision
service level are set by the organisation's governance, and this standard requires that both are documented
and applied. Where the organisation states no service level, the credential holder must propose one in
writing and record the sponsor's response.
*Six-person internal project:* the sponsor is a line director, the reserved list is four items, and the
service level is "within one week" recorded in an email.
*Multi-partner national programme:* there is one accountable senior responsible owner at programme level
and a sponsor per component, the reserved-decision lists state which decisions rise from component to
programme, and a partner-supplied sponsor is prohibited by element 10 rather than discouraged.

**12. Exception and waiver.** An exception permitting delivery to continue during a sponsorship vacancy
may be approved only by the governing body, only for a stated period, only with an acting sponsor named
in writing, and only where the reserved decisions that fall due in the period are listed and either
taken by the acting sponsor or deferred with their consequences stated.

**13. Escalation trigger.** No written sponsor acceptance. A reserved decision exceeding the service
level by the stated margin. A sponsor who declines to decide a reserved matter. A sponsor change with no
handover. A sponsor supplied by the delivery organisation. A sponsor whose stated position conflicts with
the governing body's approved case.

**14. AI application.** AI may age the reserved-decision log against the service level and flag
overdue items, maintain the reserved-decision list against the delegation schedule and flag divergence,
and prepare the sponsor handover pack from the decision and case records.

**15. AI prohibition.** An AI system must not act as sponsor, take a reserved sponsor decision, accept
sponsorship accountability, or be recorded as the approver of a reserved matter.

**16. AI verification.** **Reconciliation plus named approval.** The credential holder must reconcile the
AI-maintained reserved-decision list against the approved delegation schedule at each gate and confirm
that every reserved class appears; and the sponsor must confirm by name, at each gate, that the reserved
list the project is operating is the list they accept.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of a sponsor role
  distinct from the project manager · **EXT-028** · **Manual section 6 category 3 — international voluntary
  standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **ISO** · *ISO 21505, Guidance on governance* · relied on for: the existence of an accountable owner
  within governance arrangements · **EXT-032**, **not independently verified** · **Manual section 6 category
  3** · limitation: as above, with an open verification status.
- **OECD (G20/OECD)** · *G20/OECD Principles of Corporate Governance* · relied on for: the existence of
  a board expectation that an accountable executive owner exists for major commitments · **EXT-128** ·
  **Manual section 6 category 10 — illustrative practice** · currency checked 2026-08-03 · limitation: a
  Council Recommendation; non-binding; **not legislation**.

**18. Jurisdictional caution.** In some jurisdictions and sectors the sponsor or senior responsible owner
carries statutory or regulatory duties — for safety, for public funds, for data protection — that are
distinct from this professional requirement and cannot be delegated. Obtain local legal advice on the
statutory roles attaching to the specific project and entity.

**19. Related PCI Standards.** `PCI-FND-STD-01`; `PCI-FND-STD-11`; `PCI-PML-STD-01.01`;
`PCI-PML-STD-02.01`; `PCI-PML-STD-02.02`; `PCI-PML-STD-03.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 3 · KA 3.2 Sponsorship and steering · topics
3.2.1 the sponsor role, 3.2.2 steering committees that work, 3.2.3 decision authorities and thresholds.
Also Domain 1 KA 1.2 the leader's obligations; Domain 15 KA 15.4 transformation governance.

**21. Compliance test.** Compliance is demonstrated when a reviewer can (a) produce the sponsor's dated
written acceptance naming an individual; (b) produce the agreed reserved-decision list and confirm that
no decision in a reserved class appears in the decision record with a decider other than the sponsor or
a governing-body-approved acting sponsor; (c) compute, from the reserved-decision log, the elapsed time
of each reserved decision and confirm that every breach of the service level beyond the stated margin
produced an escalation record; and (d) for any sponsor change in the period, produce the incoming
sponsor's acceptance and the handover record. Any reserved-class decision taken below the sponsor fails
the test.

**22. Breach indicators.** A sponsor named only in a slide pack. Reserved decisions decided by the
project leader "in the interim". A reserved-decision log with no elapsed-time column. A sponsor employed
by the delivery partner. Three sponsors in a year with no handover records. A steering committee minuted
as "the sponsor".

**23. Consequence within PCI authority.** Correction required; additional review; escalation;
examination failure; ethics review; certification investigation; suspension or withdrawal — each subject
to due process and a right of appeal.

**24. Examination application.** Scenario judgement: the sponsor has been unreachable for five weeks and
a reserved contingency release is due. Evidence selection: which artefact establishes that sponsorship
was accepted. Escalation decision: the incoming sponsor is a director of the delivery partner.

**25. Version and status.** Version 1.0 · **not yet approved** · effective on approval · **new standard** —
sponsorship was addressed in the v1.0 set only through the *Related PCI standards* fields of other standards, which
created no obligation. Amendment note: none.

---
## Domain 4 — Integration and Delivery Architecture

### PCI STANDARD PCI-PML-STD-04.01 — Change Authority and Integrated Change Control

**1. Normative requirement.** A credential holder must not permit a change to an approved control
baseline to take effect before the change has been assessed for its integrated effect on scope,
schedule, cost, risk, quality, benefits and dependencies, and approved by the authority the delegation
schedule assigns to it.

**2. Purpose.** Baselines do not drift; they are moved, one uncontrolled change at a time, each of which
looked small. The failure this prevents is the project whose plan is current, whose baseline is
historical, and whose variance is therefore meaningless — after which nothing downstream, including
every forecast and every gate, is measuring anything.

**3. Scope.** Every credential holder preparing, assessing, recommending, approving, implementing or
assuring a change to an approved scope, schedule or cost baseline, in predictive, adaptive and hybrid
delivery, including changes arising from a supplier, from a regulator and from a decision taken
elsewhere in a programme.

**4. Defined terms.** *baseline* (control sense) · *delegation schedule* · *decision owner* · *material*
· *evidence* · *dependency* · *benefit*. Additionally, **integrated effect** means the effect of a
change across all seven dimensions listed in element 1, assessed together rather than separately;
**take effect** means being reflected in a system of record that others plan or report from.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-04.01-PR-01` — One flow, no side doors.** Every change to an approved control baseline
  must enter through the defined change flow, whatever its origin, and the credential holder must not
  accept a change instructed directly into a delivery system, a supplier instruction or a meeting minute.
- **`PCI-PML-STD-04.01-PR-02` — Seven-dimension assessment.** Each change must carry a written
  assessment of its effect on scope, schedule, cost, risk, quality, benefits and dependencies, with
  "none" stated positively where a dimension is unaffected. A blank dimension does not satisfy this
  requirement.
- **`PCI-PML-STD-04.01-PR-03` — Baseline version integrity.** On approval, the credential holder must
  re-issue the affected baseline with a new version identifier and a change reference, and must retain
  the superseded version. Performance reported after the change must state which baseline version it is
  measured against.
- **`PCI-PML-STD-04.01-PR-04` — Rejected and withdrawn changes retained.** Rejected, withdrawn and
  deferred changes must be retained in the register with their reasons, because the pattern of what was
  refused is evidence at the next dispute and at the next gate.

**6. Prohibited actions.** Implementing a change before approval and regularising it afterwards.
Assessing cost effect without schedule effect. Re-baselining to remove a variance rather than to reflect
an approved change. Reporting performance against a baseline version that no longer exists. Accepting a
supplier's implemented change because the work is already done. Deleting rejected changes from the
register.

**7. Required evidence.** The change register with every entry's origin, seven-dimension assessment,
recommendation, decision, decider, date and authority level; superseded and current baseline versions
with change references; performance reports naming the baseline version; the rejected and withdrawn
entries with reasons.

**8. Responsible role.** The named credential holder leading the project, for operating the flow and for
the completeness of each assessment. The named change authority in the delegation schedule, for the
decision.

**9. Approval authority.** The change authority the delegation schedule assigns, by value and by class.
Changes crossing the aggregation rule go to the aggregate authority under `PCI-PML-STD-03.02-PR-03`.
A supplier may never approve a change to the client's baseline.

**10. Independence requirement.** The assessment of a change proposed by a supplier must be reviewed by
a competent reviewer independent of that supplier before the decision. Where the change originates with
the project leader, the assessment must be reviewed by a competent reviewer who did not prepare it.

**11. Materiality or threshold.** This standard states no figure. The change authority levels, the aggregation
rule and the class of change requiring independent assessment are set by the organisation's governance;
this standard requires that they are documented and applied, and that **every** change is registered
regardless of value so the aggregation rule can see its inputs.
*Six-person internal project:* one register, one weekly change slot, and a seven-column assessment that
is usually six "none" entries and one line of substance.
*Multi-partner national programme:* one flow per contract plus a programme-level flow for changes
crossing an interface, with the interface owner named on both sides so a change cannot be approved by
one partner into another partner's baseline.

**12. Exception and waiver.** An emergency change may be implemented before approval only where delay
would cause injury, loss of life, breach of a statutory duty or irreversible loss, only on the
credential holder's recorded decision naming the ground, and only if it is submitted through the flow
within the time the delegation schedule states. The emergency provision must not be used for commercial
convenience, and its use is reported to the change authority each period.

**13. Escalation trigger.** A change implemented without approval. A supplier instruction that changes
the baseline. A change whose assessment cannot be completed because another party will not supply the
dependency effect. Re-baselining proposed with no approved change behind it. An aggregation crossing the
delegation rule.

**14. AI application.** AI may draft the impact assessment across the seven dimensions for human
completion, detect changes that appeared in a delivery system without a register entry by reconciling
system logs against the register, cluster related changes to test the aggregation rule, and check that
every issued report names a baseline version that exists.

**15. AI prohibition.** An AI system must not approve a change, determine its integrated effect as a
final position, re-baseline a plan, or close a change record.

**16. AI verification.** **Reconciliation plus independent recomputation.** Each period, a competent
reviewer must reconcile the change register against the schedule and cost systems' change logs and
account for every difference. Any schedule or cost effect produced with AI assistance must be
recomputed by an independent method before it is presented to the change authority, and the two results
recorded.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of integrated
  change control as a lifecycle practice · **EXT-028** · **Manual section 6 category 3 — international
  voluntary standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **FIDIC** · *FIDIC suite of conditions of contract* · relied on for: the existence of contractual
  variation and claim mechanisms that run in parallel with, and do not replace, internal change control
  · **EXT-050** · **Manual section 6 category 4 — contract framework** · currency checked 2026-08-03 ·
  limitation: **binds only the parties who adopt it, through the contract they sign**; characterised
  generically, no clause numbers cited, no text reproduced.
- **NEC** · *NEC4 suite of contracts* · relied on for: the existence of a compensation-event mechanism
  with its own notification timescales · **EXT-051** · **Manual section 6 category 4 — contract framework** ·
  currency checked 2026-08-03 · limitation: as above; characterised generically.
- **Project Management Institute** · *PMBOK Guide* · relied on for: the existence of change control as
  professional practice · **EXT-060** · **Manual section 6 category 5 — professional framework** · currency
  checked 2026-08-03 · limitation: **a professional framework, never regulatory authority**; no edition
  asserted, no text reproduced.

**18. Jurisdictional caution.** Contractual change and claim mechanisms carry notification periods,
condition-precedent provisions and time bars whose effect is determined by the governing law of the
contract, and a change correctly processed internally can still be barred contractually. Obtain legal
advice on the applicable contract's notice requirements before relying on an internal change record.

**19. Related PCI Standards.** `PCI-FND-STD-06`; `PCI-FND-STD-12`; `PCI-FND-STD-13`; `PCI-PML-STD-03.02`;
`PCI-PML-STD-05.01`; `PCI-PML-STD-06.01`; `PCI-PML-STD-07.01`; `PCI-PML-STD-10.01`.

**20. Related Body of Knowledge content.** PML-AI · Domain 4 · KA 4.3 Integrated baselines · topics
4.3.1 scope-schedule-cost integration, 4.3.2 configuration management, 4.3.3 baseline maintenance;
KA 4.4 Integrated change control · topics 4.4.1 change flow, 4.4.2 impact assessment, 4.4.3 the change
board and the decision log.

**21. Compliance test.** Compliance is demonstrated when a reviewer can reconcile, without unexplained
difference: the current control baselines' version identifiers to the change register's approved
entries; the change register's entry count to the change logs of the schedule and cost systems; each
approved change's seven-dimension assessment to a completed field for every dimension; and each
performance report issued in the period to a baseline version that exists in the version store. Any
change present in a delivery system and absent from the register fails the test.

**22. Breach indicators.** Baseline version identifiers that do not increment. Reports citing a baseline
"as at" a date rather than a version. Assessment fields blank in the schedule or dependency columns.
Rejected changes absent from the register. Emergency provisions invoked more than a handful of times a
year. Supplier progress that exceeds any approved scope.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Calculation review: a change approved on cost grounds alone is shown to
consume all remaining float. Scenario judgement: a supplier has built to a verbal instruction. Evidence
selection: which artefacts establish which baseline a variance was measured against.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-04-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the
seven-dimension assessment, baseline version integrity and retention of rejected changes separated into
process requirements; compliance test replaced with a four-way reconciliation.

---

## Domain 5 — Scope, Requirements and Value Definition

### PCI STANDARD PCI-PML-STD-05.01 — Scope Integrity

**1. Normative requirement.** A credential holder must not allow work to be performed, or a deliverable
to be produced, outside the approved scope baseline without an approved change.

**2. Purpose.** Scope creep is rarely a decision; it is an accumulation of accommodations, each helpful,
each unpriced. The failure this prevents is the project that delivers more than it was funded for,
later than it promised, and cannot show which of the two facts caused the other.

**3. Scope.** Every credential holder defining, approving, delivering, verifying or accepting scope, in
predictive, adaptive and hybrid delivery. In adaptive delivery it applies to the scope envelope and the
value envelope rather than to a fixed feature list — see `PCI-PML-STD-13.01`.

**4. Defined terms.** *baseline* (control sense) · *acceptance* · *material* · *decision owner* ·
*evidence* · *traceability*. Additionally, **scope envelope** means the bounded set of outcomes within
which an adaptive team may vary the specific work without a change; **out-of-scope work** means work
performed that no approved scope item or scope envelope covers.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-05.01-PR-01` — Exclusions stated, not implied.** The approved scope baseline must state
  what is excluded as well as what is included, because a dispute about scope is almost always about
  something nobody wrote down either way.
- **`PCI-PML-STD-05.01-PR-02` — Acceptance criteria before production.** Every scope item must carry
  acceptance criteria, version-identified, agreed before the item is produced. An item whose criteria
  are written after it is built does not satisfy this requirement.
- **`PCI-PML-STD-05.01-PR-03` — Out-of-scope work is refused or changed.** On identifying work outside
  the approved scope, the credential holder must either stop it or route it through the change flow, and
  must record which was done. Continuing it while "keeping an eye on it" does not satisfy this
  requirement.
- **`PCI-PML-STD-05.01-PR-04` — Periodic scope reconciliation.** At an interval the governance sets and
  at every gate, the credential holder must reconcile work in progress and completed deliverables
  against the approved scope baseline and report any item with no approved scope reference.

**6. Prohibited actions.** Absorbing an addition because refusing it would be awkward. Delivering a
deliverable with no acceptance criteria. Recording out-of-scope work as "clarification". Allowing a
supplier to expand scope in exchange for time. Writing acceptance criteria to match what was built.
Treating a stakeholder's expectation as scope without an approved change.

**7. Required evidence.** The approved scope baseline with inclusions and exclusions; acceptance criteria
per item with their version and agreement date; the scope reconciliation at each interval and gate; the
change records for every addition; records of work stopped as out of scope.

**8. Responsible role.** The named credential holder leading the project. The **sponsor** approves scope
change beyond the credential holder's delegated authority.

**9. Approval authority.** The change authority the delegation schedule assigns for scope change. The
named acceptance authority for acceptance criteria. A supplier must not approve either.

**10. Independence requirement.** Acceptance of a deliverable must be recorded by a person independent
of its production, applying `PCI-PML-STD-09.01`. The scope reconciliation itself may be prepared by the
project and must be reviewed at gates by a competent reviewer independent of the delivery organisation
where the delegation schedule requires assurance at that gate.

**11. Materiality or threshold.** This standard sets no percentage of scope. The organisation's governance
sets the change-authority thresholds and the reconciliation interval, and this standard requires that both are
documented and applied. Every item of out-of-scope work is registered regardless of size, because the
aggregation rule in `PCI-PML-STD-03.02` cannot operate on unregistered items.
*Six-person internal project:* the baseline is a one-page inclusion and exclusion list, the criteria are
two lines per deliverable, and the reconciliation is a fortnightly ten-minute check of the task board
against that page.
*Multi-partner national programme:* each contract carries its own scope baseline, the programme holds the
interface scope explicitly so that a gap between two partners' baselines is visible as a gap rather than
assumed to be someone's, and the reconciliation runs at both levels.

**12. Exception and waiver.** An exception permitting work to proceed outside the approved baseline may
be approved only by the change authority, only for a stated scope and period, and only where the cost
and schedule effect is recorded at the time of approval rather than reconciled afterwards. No exception
permits acceptance criteria to be written after production.

**13. Escalation trigger.** Work identified outside the baseline that the requesting party declines to
route through change. A deliverable presented for acceptance with no criteria. A supplier's claim that a
scope item was "always included" where the baseline is silent. Reconciliation showing a material
quantity of unreferenced work.

**14. AI application.** AI may compare delivered artefacts and work-in-progress items against the scope
baseline and flag those with no reference, draft acceptance criteria for human agreement, detect scope
language drift between document versions, and maintain the exclusion list against change records.

**15. AI prohibition.** An AI system must not approve scope, accept a deliverable, decide that an item is
within the envelope, or agree acceptance criteria.

**16. AI verification.** **Source tracing plus sampling with a stated basis.** Every AI-flagged
unreferenced item must be traced by a named human to the scope baseline or to a change record before it
is reported. Each reconciliation cycle, a competent reviewer must draw a stated sample of delivered items
and confirm, item by item, that an approved scope reference and version-identified acceptance criteria
exist and pre-date production.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of scope definition
  and control as lifecycle practices · **EXT-028** · **Manual section 6 category 3 — international voluntary
  standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **ISO** · *ISO 21500, Project, programme and portfolio management — Context and concepts* · relied on
  for: the vocabulary of project, programme and portfolio context · **EXT-027** · **Manual section 6 category
  3** · currency checked 2026-08-03 · limitation: since its current edition this document carries context
  and concepts, **not project-management guidance**, which moved to ISO 21502; it is cited here for
  concepts only.
- **Project Management Institute** · *PMBOK Guide* · relied on for: the existence of scope baselines and
  breakdown structures in professional practice · **EXT-060** · **Manual section 6 category 5 — professional
  framework** · currency checked 2026-08-03 · limitation: **never regulatory authority**; no edition
  asserted; no text reproduced.

**18. Jurisdictional caution.** Whether unpriced additional work is recoverable, and whether a variation
was validly instructed, are contractual questions determined by the governing law and the contract's own
mechanisms. Obtain legal advice before relying on an internal scope record in a commercial claim.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-12`; `PCI-FND-STD-13`; `PCI-PML-STD-04.01`;
`PCI-PML-STD-05.02`; `PCI-PML-STD-09.01`; `PCI-PML-STD-13.01`.

**20. Related Body of Knowledge content.** PML-AI · Domain 5 · KA 5.1 Scope definition and the scope
baseline; KA 5.4 Scope change, creep and verification/acceptance. Also Domain 4 KA 4.2 breakdown
structures; Domain 9 KA 9.3 acceptance and nonconformance.

**21. Compliance test.** Compliance is demonstrated when a reviewer can take the list of deliverables
produced and work packages in progress at a stated date and match **every one** to either an item in the
approved scope baseline or an approved change record — with no residue — and can confirm, for a stated
sample, that version-identified acceptance criteria exist with an agreement date **earlier than** the
production start date recorded for that item. Any unmatched item, or any criteria dated after production
began, fails the test.

**22. Breach indicators.** Deliverables the baseline does not mention. Acceptance criteria whose version
date follows the delivery date. An exclusion list that has never been updated. Supplier progress reports
describing work no change record covers. Reconciliations that report "no exceptions" every period on a
project with an active change register.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; certification investigation; suspension or withdrawal — each subject to
due process and a right of appeal.

**24. Examination application.** Scenario judgement: a stakeholder's "small addition" has been in progress
for three weeks. Evidence selection: which artefact establishes that a deliverable was in scope.
Calculation review: reconciling a deliverable list against a baseline and identifying the residue.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-05-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed;
exclusions, criteria-before-production, the stop-or-change obligation and periodic reconciliation
separated into process requirements; compliance test replaced with a no-residue matching test.

---

### PCI STANDARD PCI-PML-STD-05.02 — Requirements Traceability

**1. Normative requirement.** A credential holder must maintain traceability from every approved
requirement to its source, to the work that satisfies it and to the test or acceptance criterion that
proves it, for the life of the project.

**2. Purpose.** Without traceability nobody can answer three questions that decide whether a project
succeeded: why is this requirement here, who asked for it, and is it still wanted; is anything being
built that no requirement asked for; and is any requirement unproven at acceptance. Each unanswered
question produces a distinct and expensive failure, and all three are cheap to prevent and impossible to
reconstruct.

**3. Scope.** Every credential holder eliciting, approving, delivering, testing or accepting
requirements, in predictive, adaptive and hybrid delivery, including requirements arising from
regulation, from safety analysis and from interfaces with other projects.

**4. Defined terms.** *traceability* · *acceptance* · *evidence* · *material* · *baseline* (control
sense) · *dependency*. Additionally, **orphan requirement** means an approved requirement with no work
item satisfying it; **orphan deliverable** means delivered work satisfying no approved requirement.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-05.02-PR-01` — Every requirement carries a source.** Each approved requirement must
  record who raised it, the document or decision it derives from, and its date, so that its continued
  relevance can be tested rather than assumed.
- **`PCI-PML-STD-05.02-PR-02` — Bidirectional links maintained.** The credential holder must maintain
  links in both directions — requirement to work item to test, and back — and must update them when a
  change is approved rather than at the end.
- **`PCI-PML-STD-05.02-PR-03` — Orphans reported, not tidied.** At each reporting cycle and at each gate,
  the credential holder must report the count and identity of orphan requirements and orphan
  deliverables. Resolving an orphan by deleting the requirement, without a change record, does not
  satisfy this requirement.
- **`PCI-PML-STD-05.02-PR-04` — Regulatory and safety requirements marked and never silently dropped.**
  Requirements deriving from a statutory duty, a licence condition, a safety analysis or a consent must
  be marked as such in the register, and may be removed only by an approved change that names the
  authority permitting the removal.

**6. Prohibited actions.** Building work that satisfies no approved requirement and recording it as
delivery. Closing a requirement as met with no test or acceptance evidence. Deleting an inconvenient
requirement without a change record. Merging two requirements to hide that one is unmet. Marking a
regulatory requirement as descoped on the project's own authority.

**7. Required evidence.** The requirements register with source, raiser, date and status per
requirement; the traceability links to work items and to tests or acceptance criteria; the orphan report
at each cycle and gate; change records for every requirement added, altered or removed; the marked
subset of regulatory and safety requirements with their authority references.

**8. Responsible role.** The named credential holder leading the project, for maintaining traceability.
The named requirement owner for each requirement's continued validity. The named acceptance authority
for closure.

**9. Approval authority.** The change authority approves the addition, alteration or removal of an
approved requirement. Only the authority that owns a regulatory, licence or safety requirement may
permit its removal, and never the project.

**10. Independence requirement.** The confirmation that a requirement is proven must be given by a person
independent of the work that satisfies it, under `PCI-PML-STD-09.01`. The traceability record itself is
maintained by the project and reviewed at gates by a competent reviewer independent of the delivery
organisation where the delegation schedule requires assurance.

**11. Materiality or threshold.** This standard sets no number of requirements and no coverage percentage,
because a coverage percentage is exactly the figure that gets managed rather than met. What it requires is
that **the orphan counts are reported**, that the trend is visible, and that the organisation's governance
sets the tolerance at which an orphan count triggers escalation.
*Six-person internal project:* the traceability matrix is a spreadsheet of forty rows with four columns,
and the orphan report is a filter on it.
*Multi-partner national programme:* requirements are held per partner with a programme-level register for
cross-partner and interface requirements; the orphan report runs at both levels, and an interface
requirement that is nobody's orphan is precisely the failure the programme-level report exists to catch.

**12. Exception and waiver.** An exception permitting a requirement to be accepted without a completed
test may be approved only by the acceptance authority, only where the residual risk is recorded and
owned, only for a stated period, and never for a requirement marked regulatory or safety-derived.

**13. Escalation trigger.** An orphan count exceeding the documented tolerance. A regulatory requirement
proposed for removal. A requirement closed with no test evidence. A supplier declining to supply
traceability contractually required. Discovery that traceability has not been maintained since a named
date.

**14. AI application.** AI may build and maintain a traceability matrix from requirement, backlog and
test artefacts; detect orphans in both directions; detect duplicate and contradictory requirements;
propose candidate links for human confirmation; and flag requirements whose source reference does not
resolve.

**15. AI prohibition.** An AI system must not approve a requirement, close one as met, decide that a test
proves a requirement, or delete a link.

**16. AI verification.** **Sampling with a stated basis plus source tracing.** Every AI-proposed link
must be confirmed by a named human before it is recorded as a trace. Each gate, a competent reviewer must
draw a stated sample of requirements and trace each by hand from source document to work item to test
evidence, and must additionally sample the **regulatory and safety** subset in full rather than by
sample, because its population is small and its failure mode is severe.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of requirements
  management within delivery practice · **EXT-028** · **Manual section 6 category 3 — international voluntary
  standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **ISO** · *ISO 9001, Quality management systems — Requirements* · relied on for: the existence of a
  certifiable requirement that product requirements are determined, reviewed and verified · **EXT-033** ·
  **Manual section 6 category 3** · currency checked 2026-08-03 · limitation: voluntary unless imported;
  certification concerns a management system, not a project; this standard imports none of its requirements.
- **Project Management Institute** · *PMBOK Guide* · relied on for: the existence of a requirements
  traceability matrix as professional practice · **EXT-060** · **Manual section 6 category 5 — professional
  framework** · currency checked 2026-08-03 · limitation: **never regulatory authority**; no edition
  asserted; no text reproduced.

**18. Jurisdictional caution.** Requirements deriving from statute, licence conditions, building or
safety regulation, accessibility law or data-protection law are determined by those instruments and by
the regulator, not by the project's register. Obtain local legal and regulatory advice on which
requirements are non-removable for the specific project.

**19. Related PCI Standards.** `PCI-FND-STD-06`; `PCI-FND-STD-07`; `PCI-FND-STD-12`; `PCI-PML-STD-05.01`;
`PCI-PML-STD-09.01`; `PCI-PML-STD-13.02`; `PCI-PML-STD-16.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 5 · KA 5.2 Requirements elicitation, analysis
and traceability; KA 5.3 value definition and prioritisation. Also Domain 9 KA 9.3 acceptance and
nonconformance; Domain 13 KA 13.2 backlogs.

**21. Compliance test.** Compliance is demonstrated when a reviewer can select any approved requirement at
random and, without asking its author, reach its source document, the work item that satisfies it and the
test or acceptance evidence that proves it — and can perform the same traverse in reverse from any
delivered item — and when the orphan report for the current cycle can be reproduced from the register by
an independent query returning the same counts. Any requirement marked regulatory or safety-derived that
cannot be traversed in both directions fails the test outright.

**22. Breach indicators.** Traceability last updated at the previous gate. Orphan counts reported as zero
in every period. Requirements closed on the same date they were raised. A test suite whose case count is
unrelated to the requirement count. Requirements removed between register versions with no change
reference. Source fields containing a person's name and no document.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; certification investigation; suspension or withdrawal — each subject to
due process and a right of appeal.

**24. Examination application.** Evidence selection: which artefacts prove a requirement was met.
Scenario judgement: a safety-derived requirement is proposed for deferral to a later release. Calculation
review: reconciling requirement, work-item and test counts to locate the orphans.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-05-02` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the
regulatory and safety marking obligation added; orphan reporting made an express process requirement;
compliance test replaced with a bidirectional traverse a reviewer performs unaided.

---

## Domain 6 — Planning, Scheduling and Delivery Flow

### PCI STANDARD PCI-PML-STD-06.01 — Schedule Credibility

**1. Normative requirement.** A credential holder must not issue, endorse or rely on a schedule whose
logic, durations, constraints or status they know, or ought to know, to misrepresent the achievable
completion of the work.

**2. Purpose.** A schedule is a statement to other people about when they can rely on something. The
failure this prevents is the schedule that meets the required date by construction — constraints imposed
in place of logic, durations compressed without a resource or method change, progress claimed on
activities that have not started — which is believed for exactly as long as it takes for the first
dependent party to plan against it.

**3. Scope.** Every credential holder producing, reviewing, approving, statusing, forecasting or assuring
a schedule at any level, in predictive, adaptive and hybrid delivery, including supplier schedules
incorporated into a client plan and programme-level integrated schedules.

**4. Defined terms.** *baseline* (control sense) · *evidence* · *material* · *dependency* · *competent
reviewer* · *escalation threshold*. Additionally, **hard constraint** means a date imposed on an activity
that overrides its logic; **status date** means the date to which progress is stated; **negative float**
means the condition in which the logic cannot achieve an imposed date.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-06.01-PR-01` — Logic, not constraint, carries the dates.** Completion dates must be
  derived from activity logic and durations. Every hard constraint must be listed with the reason it
  exists and the authority that imposed it, and the credential holder must state the completion date the
  logic produces without it.
- **`PCI-PML-STD-06.01-PR-02` — Compression states its mechanism.** Where a duration has been reduced,
  the credential holder must state the mechanism — added resource, changed method, changed scope,
  increased overlap — and its cost and risk consequence. A duration reduced with no stated mechanism is
  a target, not a duration, and must be labelled as one.
- **`PCI-PML-STD-06.01-PR-03` — Progress is evidenced, not asserted.** Physical or output-based progress
  must be supported by evidence of the work performed. Progress recorded against an activity with no
  evidence of commencement does not satisfy this requirement.
- **`PCI-PML-STD-06.01-PR-04` — Negative float is reported, never absorbed.** Negative float must be
  reported to the authority the escalation threshold names, with its cause and the options, and must not
  be removed by re-imposing constraints, deleting logic, or extending the imposed date without an
  approved change.
- **`PCI-PML-STD-06.01-PR-05` — Status date and baseline version stated on issue.** Every issued schedule
  must carry its status date and the control baseline version it is measured against.

**6. Prohibited actions.** Imposing a constraint to produce a required date and presenting the result as
a forecast. Recording progress on activities not started. Deleting or reversing logic to remove negative
float. Reporting a completion date the logic does not produce. Incorporating a supplier's schedule
without reconciling its interfaces. Re-baselining to eliminate a variance with no approved change.

**7. Required evidence.** The schedule file with its logic, its constraint list and its status date; the
constraint register with reasons and imposing authorities; the compression record with mechanisms and
consequences; progress evidence per statused activity for a stated sample; the negative-float reports and
their escalations; the baseline version reference on each issue.

**8. Responsible role.** The named credential holder leading the project, for the schedule issued in the
project's name. The named planner or scheduler, for its technical preparation. Neither may be recorded as
the other.

**9. Approval authority.** The change authority the delegation schedule assigns approves a change to the
schedule baseline. The sponsor approves a change to a committed external date within their authority;
above it, the governing body. A supplier must not approve a change to the client's schedule baseline.

**10. Independence requirement.** At each gate at which the delegation schedule requires assurance, the
schedule must be reviewed by a competent reviewer independent of its preparation. Where the schedule
supports a contractual claim or a committed external date, the review must also be independent of the
party that benefits from the date.

**11. Materiality or threshold.** **This standard states no float threshold, no density figure and no
percentage**, and inventing one would be indefensible: what counts as an acceptable constraint count or
float profile depends on the contract, the sector and the planning method. The organisation's governance
must document the schedule-quality criteria it applies, the tolerance at which a variance is escalated,
and the sample size for progress evidence; this standard requires that they exist and are applied. Where the
organisation applies a published schedule-assessment method, the credential holder must record which
method and which criteria, so that a reported score can be interpreted.
*Six-person internal project:* forty activities in a shared tool, three constraints each with a stated
reason, and progress evidenced by the same artefacts the team produces anyway.
*Multi-partner national programme:* an integrated schedule assembled from partner schedules, with the
interface milestones owned on both sides by name, and constraint and progress rules stated in the partner
agreement so that a partner's internal convention cannot silently change the programme's dates.

**12. Exception and waiver.** An exception permitting a schedule to be issued with an unreconciled
interface or an unsupported progress claim may be approved only by the sponsor, only for one reporting
cycle, only where the affected activities are identified and the issued schedule states the limitation on
its face. No exception permits a completion date to be presented as achievable when the logic does not
produce it.

**13. Escalation trigger.** Negative float against a committed date. A required completion date that the
logic cannot produce. An instruction to record progress not performed. A supplier schedule that cannot be
reconciled at its interfaces. A constraint imposed with no stated authority. A compression with no
mechanism.

**14. AI application.** AI may run schedule-quality checks, identify open ends, redundant logic,
excessive lags and constraint concentrations, compare the current schedule against the baseline and
summarise the differences, generate scenario schedules for human evaluation, and detect progress claims
inconsistent with cost or delivery data.

**15. AI prohibition.** An AI system must not approve a schedule, decide a duration that is then issued
without human derivation, remove a constraint, status an activity, or determine that a date is
achievable.

**16. AI verification.** **Independent recomputation plus boundary testing.** A competent reviewer must
recompute the critical path and the float on the issued schedule using a second method or tool and
reconcile any difference before issue. Where AI generated or compressed durations, the reviewer must test
the boundaries — the shortest and longest credible duration for the sampled activities — and record
whether the issued value sits inside the range the mechanism supports. Progress statused with AI
assistance must be confirmed by **sampling with a stated basis** against physical evidence.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of schedule
  development and control as lifecycle practices · **EXT-028** · **Manual section 6 category 3 — international
  voluntary standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **Project Management Institute** · *Practice Standard for Scheduling* · relied on for: the existence of
  recognised schedule-quality attributes in professional practice · **EXT-062** · **Manual section 6 category 5
  — professional framework** · currency checked 2026-08-03 · limitation: **never regulatory authority**;
  no metric, threshold or checklist from it is reproduced or relied on.
- **AACE International** · *Total Cost Management (TCM) Framework* · relied on for: the existence of a
  planning and scheduling process within a cost-management framework · **EXT-064**, **not independently
  verified — verify current requirements** · **Manual section 6 category 5 — professional framework** ·
  limitation: as above; no recommended-practice text is reproduced.

**18. Jurisdictional caution.** Where a schedule supports an extension-of-time claim, a delay analysis or
a liquidated-damages position, the acceptable method and the evidential standard are determined by the
contract and by the forum, and they differ between jurisdictions and between tribunals. Obtain legal
advice before a schedule is relied on in a claim.

**19. Related PCI Standards.** `PCI-FND-STD-05`; `PCI-FND-STD-02`; `PCI-FND-STD-13`; `PCI-PML-STD-04.01`;
`PCI-PML-STD-07.01`; `PCI-PML-STD-08.01`; `PCI-PML-STD-15.01`.

**20. Related Body of Knowledge content.** PML-AI · Domain 6 · KA 6.1 Planning levels and logic networks;
KA 6.2 The critical path and float; KA 6.3 Resources, constraints, milestones and rolling wave; KA 6.4
Delivery flow, recovery and forecasting. Also Domain 15 KA 15.1 dependency arithmetic.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) open the issued schedule and
find its status date and baseline version on the face of it; (b) list every hard constraint and find, for
each, a reason and an imposing authority in the constraint register; (c) remove the constraints in a copy
of the schedule, recompute, and reconcile the resulting completion date to the date the credential holder
stated under `PR-01`; (d) draw a stated sample of statused activities and find, for each, evidence of the
work performed; and (e) confirm that every period in which negative float existed produced an escalation
record. A completion date that cannot be reproduced from the logic fails the test.

**22. Breach indicators.** Constraint counts rising as the committed date approaches. Float distributions
with a large spike at zero. Activities at 90 per cent for several periods. Negative float that appears
and disappears without a change record. Durations that changed with no mechanism recorded. Schedules
issued with no status date. A supplier schedule whose interface dates differ from the client's by more
than the reporting cycle.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Calculation review: a forward and backward pass on a small network with
an imposed date, identifying the negative float and its cause. Scenario judgement: a sponsor asks for the
completion date to be "brought back" without a scope or resource change. Evidence selection: which
artefacts support a progress claim.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-06-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the
constraint-transparency, compression-mechanism, progress-evidence, negative-float and status-date
obligations separated into five process requirements; element 11 rewritten to require documented
schedule-quality criteria rather than to assert any; compliance test replaced with a
remove-the-constraints reproduction test.

---

## Domain 7 — Cost, Resources and Commercial Awareness

### PCI STANDARD PCI-PML-STD-07.01 — Cost Stewardship

**1. Normative requirement.** A credential holder must report the project's cost position — committed,
incurred, accrued and forecast — on a basis that is complete, current and reconcilable to the
organisation's financial records.

**2. Purpose.** The two failures this prevents are opposite and equally common: the position that omits
commitments and accruals, so the project looks affordable until the invoices arrive; and the forecast
that is a target restated, so the overrun is discovered when the budget is exhausted rather than when it
became inevitable. Both are failures of completeness, not of arithmetic.

**3. Scope.** Every credential holder preparing, reviewing, approving, reporting or assuring a project,
programme or portfolio cost position or forecast, including contingency and reserve positions, in every
delivery model.

**4. Defined terms.** *baseline* (control sense) · *material* · *evidence* · *competent reviewer* ·
*escalation threshold* · *decision owner*. Additionally, **committed** means value contractually
committed but not yet incurred; **accrued** means value of work performed but not yet invoiced;
**contingency** means provision for identified risk held inside the control baseline; **management
reserve** means provision for unidentified risk and scope change held outside it and released by the
authority the governance plan names.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-07.01-PR-01` — Four elements always present.** Every issued cost position must state
  committed, incurred, accrued and forecast values, and must not omit an element because it is
  incomplete; where an element is estimated, the basis is stated.
- **`PCI-PML-STD-07.01-PR-02` — Reconciliation to the ledger.** At the interval the governance sets, the
  credential holder must reconcile the project cost position to the organisation's financial ledger and
  account for every difference, retaining the reconciliation.
- **`PCI-PML-STD-07.01-PR-03` — Forecast method stated and consistent.** Each forecast must state the
  method used and the assumptions it rests on, and the method must not be changed between periods without
  recording the change, its reason and its effect on the reported figure.
- **`PCI-PML-STD-07.01-PR-04` — Contingency and reserve reported separately.** Contingency and management
  reserve must be reported separately from the base cost and from each other, with drawdown recorded
  against the specific risk or change that justified it and the authority that released it.
- **`PCI-PML-STD-07.01-PR-05` — Adverse movement reported in the period it is known.** A material adverse
  movement must be reported in the reporting cycle in which it becomes known, and must not be held for a
  later cycle, netted against an unrealised saving, or absorbed into contingency without a release
  decision.

**6. Prohibited actions.** Reporting incurred cost as the position while commitments are material and
unstated. Drawing contingency to conceal a base-cost overrun. Presenting a forecast equal to the budget
with no supporting change. Netting an adverse movement against an optimistic one. Changing forecast
method to produce a better figure. Recording a management-reserve drawdown as a base-cost saving.

**7. Required evidence.** The cost report showing the four elements; the ledger reconciliation with its
differences explained; the forecast with its stated method and assumptions and the record of any method
change; the contingency and reserve registers with drawdowns, their justifying risk or change, and the
releasing authority; the record of adverse movements with the date known and the date reported.

**8. Responsible role.** The named credential holder leading the project, for the position issued in the
project's name. The named cost or finance role, for its preparation and for the ledger reconciliation.

**9. Approval authority.** The authority the governance plan names releases contingency; the authority it
names — typically the sponsor or change authority, and this standard does not assume which — releases
management reserve, through change control. Neither is released by the person who prepares the forecast.

**10. Independence requirement.** The ledger reconciliation must be prepared or verified by a person
independent of the project's cost reporting. At each gate at which the delegation schedule requires
assurance, the forecast must be reviewed by a competent reviewer independent of its preparation.

**11. Materiality or threshold.** This standard states no percentage. The organisation's governance sets the
materiality threshold for adverse movement, the reconciliation interval, the contingency release
authority and the forecast tolerance; this standard requires that each is documented and applied. Where the
governance plan does not name the authority that releases management reserve, the credential holder must
escalate that gap rather than assume the answer — the suite terminology audit records that different
books name different roles, and assuming one is how a reserve is released by someone who could not
release it.
*Six-person internal project:* one cost line per person plus three purchase orders; the reconciliation is
a monthly comparison against the finance extract and takes under an hour.
*Multi-partner national programme:* the four elements are reported per partner and consolidated, the
consolidation states which partner's accrual basis differs and by how much, and management reserve is held
and released at programme level so that a component cannot fund its own overrun.

**12. Exception and waiver.** An exception permitting a cost position to be issued without a completed
reconciliation may be approved by the sponsor for one cycle, only where the report states the limitation
on its face and identifies the unreconciled amount. No exception permits a known material adverse movement
to go unreported in the cycle in which it became known.

**13. Escalation trigger.** A forecast that exceeds the approved budget or the documented tolerance. A
reconciliation difference that cannot be explained. Contingency drawdown against risks that were not in
the register. Instruction to delay reporting an adverse movement. A supplier's commitment position that
cannot be obtained.

**14. AI application.** AI may reconcile cost data across systems and surface unexplained differences,
detect commitments absent from the position, produce several forecast methods for human selection, flag
contingency drawdowns with no matching register entry, and detect forecast-method changes between
periods.

**15. AI prohibition.** An AI system must not approve a cost position, select the forecast method that is
issued, release contingency or reserve, or determine that a difference is immaterial.

**16. AI verification.** **Independent recomputation plus reconciliation.** The credential holder must
recompute the headline forecast by an independent method and reconcile the two results before issue,
recording both. Every AI-produced reconciliation must be checked by a competent reviewer against the
ledger extract itself, with the extract date and filter recorded, and every AI-surfaced difference
confirmed by a named human before it is reported as resolved.

**17. External reference.**
- **AACE International** · *Total Cost Management (TCM) Framework* · relied on for: the existence and
  purpose of a cost-control cycle and of maturity-based estimate classification · **EXT-064**, **not
  independently verified — verify current requirements** · **Manual section 6 category 5 — professional
  framework** · limitation: **a professional framework, never regulatory authority**; no class table,
  accuracy range or recommended-practice text is reproduced.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of cost planning
  and control as lifecycle practices · **EXT-028** · **Manual section 6 category 3 — international voluntary
  standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **Project Management Institute** · *PMBOK Guide* · relied on for: the existence of cost baselines,
  reserves and forecasting in professional practice · **EXT-060** · **Manual section 6 category 5** · currency
  checked 2026-08-03 · limitation: **never regulatory authority**; no edition asserted; no text
  reproduced.

**18. Jurisdictional caution.** How costs are recognised, when a liability is recorded, how contract
assets and provisions are measured and what must be disclosed are determined by the financial-reporting
framework the entity applies and by its auditors, not by a project cost report. **This standard states no
accounting treatment.** Obtain qualified accounting advice for the specific entity and framework.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-05`; `PCI-FND-STD-07`; `PCI-PML-STD-04.01`;
`PCI-PML-STD-06.01`; `PCI-PML-STD-07.02`; `PCI-PML-STD-08.01`.

**20. Related Body of Knowledge content.** PML-AI · Domain 7 · KA 7.1 Estimating and budgeting; KA 7.2
The cost baseline, actuals and forecasting; KA 7.3 Earned value: measurement, variances and forecasting.
Also Domain 8 KA 8.3 responses, reserves and governance.

**21. Compliance test.** Compliance is demonstrated when the issued cost position can be reconciled,
without unexplained difference, to: the financial ledger extract at the stated date, the commitment
register, the accrual schedule, the approved change register, the contingency and reserve registers with
their release authorities, and the approved control baseline version named on the report — and when the
forecast's stated method, applied to the same inputs by a second preparer, reproduces the reported figure
within the tolerance the method itself implies. A position that omits commitments, or a forecast whose
method cannot be reproduced, fails the test.

**22. Breach indicators.** Positions reporting incurred cost only. Forecasts equal to budget for
consecutive periods on a project with an active risk register. Contingency drawdowns with no risk
reference. Reconciliations with a persistent unexplained residual. A forecast method that changes in the
period an overrun would otherwise appear. Adverse movements reported one cycle after the supplier
notified them.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Calculation review: an earned-value suite in which the candidate selects
and justifies a forecasting method and identifies why another is unsupportable. Scenario judgement:
contingency is proposed to cover a base-cost overrun four weeks before a gate. Evidence selection: which
artefacts establish the completeness of a reported position.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-07-01` v1.0, whose subject matter is split between this standard and `PCI-PML-STD-07.02`. Amendment
note: renumbered and restructured; legislative drafting removed; resource obligations moved out to their
own standard so that one standard carries one principal obligation; the four-element completeness rule, ledger
reconciliation, method consistency, separate reserve reporting and same-cycle adverse reporting separated
into five process requirements.

---

### PCI STANDARD PCI-PML-STD-07.02 — Resource Decisions and the Commitment of People

**1. Normative requirement.** A credential holder must not commit a person, a team or a shared resource to
a project plan without the agreement of the named individual who controls that resource's availability.

**2. Purpose.** The most common cause of a plan that cannot be delivered is a resource assumption nobody
agreed to. The failure this prevents is the plan built on named people who are already committed
elsewhere — a plan that is arithmetically sound, professionally presented, and false from the day it is
issued, with the consequence landing on the individuals concerned rather than on the plan's author.

**3. Scope.** Every credential holder planning, committing, reallocating or reporting on human and shared
resources — internal staff, seconded staff, supplier staff, specialist equipment and shared environments
— on projects, programmes and portfolios, in every delivery model.

**4. Defined terms.** *decision owner* · *material* · *evidence* · *dependency* · *escalation threshold* ·
*baseline* (control sense). Additionally, **resource owner** means the named individual who controls
whether a person or shared resource is available to a project; **committed availability** means the
proportion of a resource's capacity that the resource owner has agreed to this project for a stated
period.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-07.02-PR-01` — Named agreement before the plan is issued.** Each resource assumption
  material to the plan must carry the resource owner's name and a dated agreement to the committed
  availability, obtained before the plan is issued.
- **`PCI-PML-STD-07.02-PR-02` — Aggregate demand tested against capacity.** The credential holder must
  test the plan's total demand on each shared resource against that resource's total committed
  availability across all claims on it, and must report any over-commitment rather than assuming
  resolution.
- **`PCI-PML-STD-07.02-PR-03` — Withdrawal is a change, not an adjustment.** Where a committed resource
  is withdrawn or reduced, the credential holder must record it, assess the effect through
  `PCI-PML-STD-04.01`, and report the consequence, rather than re-planning silently around it.
- **`PCI-PML-STD-07.02-PR-04` — Sustained overload is reported.** Where delivery depends on individuals
  working beyond the hours or duration the organisation's own policy permits, the credential holder must
  report that dependency to the sponsor as a delivery risk with its cause, and must not present the plan
  as achievable without stating it.

**6. Prohibited actions.** Naming people in a plan without their resource owner's agreement. Planning to
an availability the resource owner has refused. Concealing over-commitment by planning at an aggregate
level that hides it. Presenting sustained overtime as capacity. Reallocating a person committed to
another project without that project's resource owner. Recording a supplier's named key personnel as
committed when the contract does not secure them.

**7. Required evidence.** The resource plan with, per material resource, the owner's name and dated
agreement; the aggregate demand-against-capacity analysis; withdrawal records and their change
assessments; the reports of sustained overload with their causes; the contractual basis for any
supplier key personnel recorded as committed.

**8. Responsible role.** The named credential holder leading the project, for obtaining and recording
agreements and for reporting over-commitment. The named resource owner, for the availability they agree.

**9. Approval authority.** The resource owner agrees availability. The sponsor resolves competing claims
within their authority; above it, the portfolio or programme authority the delegation schedule names.
The project leader must not resolve a competing claim on another owner's resource.

**10. Independence requirement.** Not applicable to the agreement itself, because the resource owner's
consent is by definition an interested party's consent to their own resource; independence attaches to
the portfolio-level capacity test in `PCI-PML-STD-15.02`, which must be performed by a function
independent of any single competing project.

**11. Materiality or threshold.** This standard sets no percentage of a person's time. The organisation's
governance sets the threshold at which a resource assumption is material enough to require a recorded
agreement, the limits on working hours and duration, and the escalation route for competing claims; this
standard requires that these exist and are applied. Where no threshold is documented, every named individual
in the plan requires a recorded agreement.
*Six-person internal project:* six agreements, one line each, in an email thread the leader retains — and
the aggregate test is a single check that no one appears on two plans at once.
*Multi-partner national programme:* resource owners sit in several organisations, the aggregate test runs
at portfolio level under `PCI-PML-STD-15.02`, and supplier key personnel are secured contractually rather
than assumed, because a name in a tender is not committed availability.

**12. Exception and waiver.** An exception permitting a plan to be issued with an unagreed material
resource assumption may be approved by the sponsor for a stated period not exceeding one reporting cycle,
only where the plan states the unagreed assumption on its face and identifies the affected activities. No
exception permits an individual to be named in an issued plan against their resource owner's recorded
refusal.

**13. Escalation trigger.** A resource owner's refusal or withdrawal of a committed resource. Aggregate
demand exceeding committed availability on a shared resource. Delivery dependent on hours beyond policy.
A supplier substituting named key personnel. A competing claim the sponsor cannot resolve.

**14. AI application.** AI may aggregate demand across plans and surface over-commitment, model the effect
of alternative allocations, flag plan entries with no recorded owner agreement, and detect individuals
whose aggregate allocation exceeds their committed availability.

**15. AI prohibition.** An AI system must not commit a person to a plan, agree availability, resolve a
competing claim, or reallocate people between projects. Where an optimiser produces an allocation, it has
produced a recommendation, and the decision is a reserved class under `PCI-PML-STD-01.02-PR-01`.

**16. AI verification.** **Reconciliation plus named approval.** Every AI-produced allocation must be
reconciled against the recorded owner agreements before it is issued, and each affected resource owner
must confirm by name. Every AI-surfaced over-commitment must be confirmed by a named human against the
competing plans before it is escalated.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of resource
  planning and of the negotiation of resources with those who control them · **EXT-028** · **Manual section 6
  category 3 — international voluntary standard** · currency checked 2026-08-03 · limitation: guidance;
  voluntary; not certifiable.
- **ISO** · *ISO 45001, Occupational health and safety management systems — Requirements with guidance
  for use* · relied on for: the existence of a certifiable management-system standard within which
  workload and fatigue are addressed as health and safety matters · **EXT-123** · **Manual section 6 category 3**
  · currency checked 2026-08-03 · limitation: certifiable, but adoption is voluntary unless a contract or
  regulator requires it; this standard imports none of its requirements and states no fatigue rule of its own.
- **AACE International** · *Total Cost Management (TCM) Framework* · relied on for: the existence of
  resource economics within a cost-management framework · **EXT-064**, **not independently verified** ·
  **Manual section 6 category 5 — professional framework** · limitation: **never regulatory authority**; no text
  reproduced.

**18. Jurisdictional caution.** Working-time law, collective agreements, works-council consultation
rights, secondment and agency-worker rules, and health-and-safety duties determine what may lawfully be
required of people, and they differ sharply by jurisdiction. Obtain local legal and human-resources
advice before planning on extended hours, cross-border deployment or supplier staff substitution.

**19. Related PCI Standards.** `PCI-FND-STD-01`; `PCI-FND-STD-11`; `PCI-PML-STD-06.01`;
`PCI-PML-STD-07.01`; `PCI-PML-STD-12.01`; `PCI-PML-STD-15.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 7 · KA 7.4 Resource economics, procurement
strategy and cash. Also Domain 6 KA 6.3 resources, constraints, milestones and rolling wave; Domain 12
KA 12.2 team formation and retention; Domain 15 KA 15.3 capacity and enterprise PMOs.

**21. Compliance test.** Compliance is demonstrated when a reviewer can take every named individual and
shared resource in the issued plan whose contribution exceeds the documented materiality threshold and
find, for each, a dated agreement from the named resource owner covering the committed availability the
plan assumes; and can sum the plan's demand on each shared resource against all other current claims on
it and reproduce the over-commitment figure the project reported. Any material resource assumption with no
recorded owner agreement fails the test.

**22. Breach indicators.** Plans naming individuals whose owners have not been asked. Allocation
percentages summing above 100 across concurrent plans. Delivery dates that depend on weekend working
described nowhere. Supplier key personnel named in a plan and absent from the contract. Resource
withdrawals resolved by re-planning with no change record. Resource owners who first learn of a
commitment from the published plan.

**23. Consequence within PCI authority.** Correction required; additional review; escalation; examination
failure; ethics review; certification investigation; suspension or withdrawal — each subject to due
process and a right of appeal.

**24. Examination application.** Calculation review: aggregate allocation across three concurrent plans
showing a specialist committed at 160 per cent. Scenario judgement: a resource owner withdraws a key
specialist a fortnight before a milestone. Ethical dilemma: a plan can only be met by sustained overtime
the leader has not disclosed.

**25. Version and status.** Version 1.0 · **not yet approved** · effective on approval · **new standard**,
carrying the resource obligations formerly bundled inside `PML-LAW-07-01` v1.0, which stated cost and
resource obligations in one rule and could therefore be breached in two unrelated ways at once. Amendment
note: separated so that each standard carries one principal obligation.

---

## Domain 8 — Risk, Uncertainty and Resilience

### PCI STANDARD PCI-PML-STD-08.01 — Risk Escalation

**1. Normative requirement.** A credential holder must escalate a risk whose exposure exceeds the
escalation threshold the delegation schedule sets, to the named authority that threshold identifies,
within the time it states.

**2. Purpose.** The value of an escalation is mostly in its timing: the same warning is worth a great deal
early and almost nothing late, because what it buys is the option set. The failure this prevents is the
escalation that arrives after the decision it was supposed to inform — technically compliant, entirely
useless — and the one that never arrives because the person holding the risk hoped to resolve it first.

**3. Scope.** Every credential holder identifying, assessing, owning, responding to, reporting or assuring
risk on a project, programme or portfolio, in every delivery model, including risks arising in a
supplier's scope and risks transferred by contract.

**4. Defined terms.** *escalation threshold* · *material* · *decision owner* · *evidence* · *dependency* ·
*contingency* (as defined in `PCI-PML-STD-07.01`). Additionally, **exposure** means the assessed
consequence of a risk combined with its assessed likelihood, on the basis the organisation's risk
framework states; **decision action window** means the remaining duration less the escalation latency,
expressed as a proportion of the remaining duration.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-08.01-PR-01` — Every risk has one named owner.** Each risk must carry exactly one named
  owner with the authority to act on it; a risk owned by a team, a function or a supplier organisation
  rather than a person does not satisfy this requirement.
- **`PCI-PML-STD-08.01-PR-02` — Escalate on threshold, not on comfort.** The credential holder must
  escalate when the documented threshold is met, whether or not they expect to resolve the risk, and must
  record the date the threshold was met alongside the date of escalation.
- **`PCI-PML-STD-08.01-PR-03` — Escalate with options.** Every escalation must carry the options open to
  the receiving authority, their consequences, and the date after which each option closes. An escalation
  that reports a problem with no option set does not satisfy this requirement.
- **`PCI-PML-STD-08.01-PR-04` — Test the action window.** Before escalating, the credential holder must
  state whether the receiving authority can still act in time, and where the window is negative must say
  so on the face of the escalation and route it out of cycle.
- **`PCI-PML-STD-08.01-PR-05` — Contingency is drawn against a registered risk.** Contingency must be
  drawn only against a risk recorded in the register before the drawdown, with the releasing authority
  named.

**6. Prohibited actions.** Holding a risk above the threshold because a fix is expected. Reducing an
assessed exposure without a recorded change to the underlying facts. Escalating a risk with no options.
Recording a risk as closed because it stopped being discussed. Transferring a risk to a supplier by
contract and removing it from the register while the consequence still lands on the client. Drawing
contingency against a risk added to the register afterwards.

**7. Required evidence.** The risk register with one named owner per risk, assessed exposure and its
basis, and status history; escalation records with the date the threshold was met, the date escalated,
the options and the action window; out-of-cycle routing records; contingency drawdown records with the
justifying register entry and the releasing authority.

**8. Responsible role.** The named risk owner for each risk. The named credential holder leading the
project, for operating the escalation process and for escalating where the risk owner does not.

**9. Approval authority.** The authority the escalation threshold names receives and decides. The
authority the governance plan names releases contingency. A risk may be closed only by its owner, with
the reason recorded.

**10. Independence requirement.** At each gate at which the delegation schedule requires assurance, the
risk register and the escalation record must be reviewed by a competent reviewer independent of the
delivery organisation, whose review includes testing whether risks that met the threshold were escalated.
That reviewer must not be the function that maintains the register.

**11. Materiality or threshold.** **This standard sets no exposure figure, no matrix and no score**, because
risk appetite is an organisational decision and a number imported from elsewhere is a number nobody owns.
It requires that the organisation documents its escalation thresholds, their named destinations and their
times, and that the credential holder applies them. Where the organisation documents no threshold for a
risk class, the credential holder must escalate every risk in that class until one exists.
*Six-person internal project:* one register of a dozen risks, a single threshold — anything that could
delay the committed date or exceed the remaining contingency — and escalation by email to the sponsor
within two working days.
*Multi-partner national programme:* thresholds at component and programme level with a stated rule for
which component risks rise, escalation destinations named as roles in the programme rather than
organisations, and an explicit provision for risks that are below every component's threshold but
correlated across components — which is the exposure a tiered threshold structure is worst at seeing.

**12. Exception and waiver.** An exception permitting an escalation to be deferred may be approved only by
the authority that would have received it, in writing, before the escalation time expires, for a stated
period, with the reason recorded. **A risk bearing on safety, a statutory duty, a licence condition or
harm to a person must not be deferred**, and no exception is available for it.

**13. Escalation trigger.** Exposure meeting the documented threshold. A risk owner who declines to act
or to escalate. A negative decision action window. A supplier risk whose consequence lands on the client.
Correlated risks whose combined exposure exceeds a threshold that none of them individually meets. An
instruction not to escalate.

**14. AI application.** AI may scan delivery, cost, schedule and issue data for early risk signals and
propose candidate register entries, compute exposures on the organisation's stated basis, age unescalated
risks against thresholds and flag them, compute the decision action window, and detect correlations
between risks held in different registers.

**15. AI prohibition.** An AI system must not decide that a risk need not be escalated, close a risk,
release contingency, own a risk, or assign a likelihood or consequence that is then reported without a
named human's derivation.

**16. AI verification.** **Independent recomputation plus source tracing.** Every exposure computed with
AI assistance must be recomputed by hand by the risk owner before it is used to decide escalation, and
every AI-proposed risk must be traced by a named human to the underlying data before it enters the
register. Where AI has aged risks against thresholds, a competent reviewer must confirm by
**sampling with a stated basis** that no risk meeting a threshold was omitted from the flagged set.

**17. External reference.**
- **ISO** · *ISO 31000, Risk management — Guidelines* · relied on for: the existence of a risk-management
  process concept including escalation and review · **EXT-020** · **Manual section 6 category 3 — international
  voluntary standard** · currency checked 2026-08-03 · limitation: **guidance, and ISO states expressly
  that it is not a certifiable standard**; voluntary; this standard relies on it for no requirement.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of risk management
  within delivery practice · **EXT-028** · **Manual section 6 category 3** · currency checked 2026-08-03 ·
  limitation: guidance; voluntary; not certifiable.
- **ISO/IEC** · *ISO/IEC 23894, Guidance on risk management for artificial intelligence* · relied on for:
  the existence of AI-specific risk guidance sitting alongside the AI management-system standard ·
  **EXT-024** · **Manual section 6 category 3** · currency checked 2026-08-03 · limitation: **guidance, not
  requirements**; voluntary; not certifiable.

**18. Jurisdictional caution.** Duties to report certain risks to a regulator, a safety authority, an
auditor or a market are set by law and by licence conditions and carry their own timescales, which can be
shorter than any internal threshold. Obtain local legal advice on mandatory reporting duties attaching to
the specific project and sector.

**19. Related PCI Standards.** `PCI-FND-STD-11` (the parent duty to escalate); `PCI-FND-STD-05`;
`PCI-FND-STD-02`; `PCI-PML-STD-03.02`; `PCI-PML-STD-07.01`; `PCI-PML-STD-08.02`;
`PCI-PML-STD-12.02`; `PCI-PML-STD-15.01`.
**What this standard adds to `PCI-FND-STD-11`:** the foundational standard creates the duty to escalate a
material misstatement. This standard
requires **one named risk owner**, escalation **on threshold rather than on expectation of resolution**,
an **option set with closing dates**, an explicit **action-window test**, and **contingency drawn only
against a pre-registered risk**.

**20. Related Body of Knowledge content.** PML-AI · Domain 8 · KA 8.1 Threats, opportunities and
identification; KA 8.2 Analysis: from screening to quantification; KA 8.3 Responses, reserves and
governance; KA 8.4 Resilience, bias and crisis leadership. Also Domain 3 KA 3.3 topic 3.3.3 escalation
design.

**21. Compliance test.** Compliance is demonstrated when a reviewer can, for every risk in the period whose
recorded exposure met the documented threshold: (a) find an escalation record naming the authority the
threshold identifies; (b) compute the elapsed time between the date the threshold was met and the date
escalated and confirm it is within the stated time; (c) find an option set with consequences and closing
dates on the face of the escalation; and (d) find the receiving authority's recorded decision. The
reviewer must additionally confirm that every contingency drawdown in the period references a risk whose
register entry pre-dates the drawdown. A risk that met the threshold with no escalation record fails the
test, and the absence of a later problem is not a defence.

**22. Breach indicators.** Exposures reassessed downward in the period before a gate with no change in
the underlying facts. Escalation dates clustering immediately before steering meetings. Escalations with
no options. Risks closed with the reason "no longer relevant". Contingency drawdowns whose risk entry was
created the same day. A shortening trend in escalation lead time. Risks whose owner is a supplier
organisation.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Escalation decision: a risk meets the threshold six weeks before a
committed date and the leader believes it can be resolved in four. Calculation review: computing a
decision action window and showing the escalation is ceremonial. Ethical dilemma: a sponsor instructs
that a risk be held below the threshold by splitting it.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-08-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; single
named ownership, escalation-on-threshold, the option set, the action-window test and the contingency rule
separated into five process requirements; compliance test replaced with an elapsed-time test performed
against two recorded dates.

---

### PCI STANDARD PCI-PML-STD-08.02 — Issue Management

**1. Normative requirement.** A credential holder must record every issue affecting delivery with a single
named owner, a required resolution date and a stated consequence of non-resolution, and must not close an
issue without recording how it was resolved.

**2. Purpose.** An issue is a risk that has already occurred: it is managed, not analysed. The failure this
prevents is the issue log used as a repository rather than a control — hundreds of open items, no owners,
no dates, and the two that matter indistinguishable from the rest. Where that happens the organisation
discovers its issues through their consequences.

**3. Scope.** Every credential holder identifying, owning, resolving, reporting or assuring issues on a
project, programme or portfolio, including issues raised by suppliers, by users and by assurance, and
including issues escalated from a concern under `PCI-PML-STD-12.02`.

**4. Defined terms.** *material* · *decision owner* · *escalation threshold* · *evidence* · *dependency* ·
*acceptance*. Additionally, **issue** means a risk that has occurred, or a present condition adversely
affecting delivery; **resolution** means the recorded action that removed the issue or, where it was
accepted, the recorded acceptance of its consequence by the authority entitled to accept it.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-08.02-PR-01` — Owner, date and consequence on entry.** Every issue must carry, at the
  point of recording, one named owner, a required resolution date and the stated consequence if it is not
  resolved by that date.
- **`PCI-PML-STD-08.02-PR-02` — Ageing drives escalation.** Issues must be aged against their required
  resolution dates, and an issue past its date by the margin the escalation threshold states must be
  escalated to the named authority.
- **`PCI-PML-STD-08.02-PR-03` — Closure states the resolution.** An issue may be closed only with a
  recorded statement of how it was resolved or, where the consequence was accepted, by whom and on what
  basis. Closure with no resolution statement does not satisfy this requirement.
- **`PCI-PML-STD-08.02-PR-04` — Recurrence is analysed, not re-logged.** Where the same issue recurs
  within the period the governance sets, the credential holder must record it as a recurrence and route
  it to root-cause analysis under `PCI-PML-STD-09.02`, rather than opening an unconnected entry.

**6. Prohibited actions.** Closing an issue because it went quiet. Recording an issue with no owner or no
date "pending assignment" beyond the period the governance allows. Re-logging a recurring issue as new.
Accepting an issue's consequence on behalf of a party entitled to decide it. Downgrading an issue to a
risk after it has occurred. Reporting an issue count without the ageing profile that gives it meaning.

**7. Required evidence.** The issue register with owner, raised date, required date, consequence, status
and closure statement per entry; the ageing report per period; escalation records for overdue issues; the
recurrence records and their routing to root-cause analysis; acceptance records for consequences accepted.

**8. Responsible role.** The named issue owner for each issue. The named credential holder leading the
project, for operating the register, the ageing and the escalations.

**9. Approval authority.** The authority the escalation threshold names receives escalated issues. The
consequence of an unresolved issue may be accepted only by the party that bears it, or by the authority
the delegation schedule assigns; the project must not accept a consequence borne by an operating
organisation or a user.

**10. Independence requirement.** Not applicable to issue ownership, which by design sits with the party
able to act; independence attaches to the periodic review of closures, which must be sampled at each gate
by a competent reviewer independent of the delivery organisation where the delegation schedule requires
assurance.

**11. Materiality or threshold.** This standard states no issue count and no age limit. The organisation's
governance sets the ageing margin that triggers escalation, the recurrence window and the closure review
sample; this standard requires that they exist and are applied. Every issue is recorded regardless of size,
because the ageing profile and the recurrence analysis both depend on a complete population.
*Six-person internal project:* the register is the team's issue tracker with three extra fields, and the
ageing report is a saved filter reviewed at the weekly meeting.
*Multi-partner national programme:* each partner runs its own register, the programme holds the
cross-partner issues explicitly with an owner named on the giving side, and the ageing profile is reported
per partner so a single partner's backlog cannot be averaged away.

**12. Exception and waiver.** An exception permitting an issue to be closed without a resolution statement
may be approved only by the authority the escalation threshold names, only where the issue is recorded as
superseded by a specified change or decision, and only with that change or decision referenced. No
exception permits closure by silence.

**13. Escalation trigger.** An issue past its required date by the documented margin. An issue whose owner
declines ownership. A recurring issue at its second recurrence within the documented window. An issue
whose consequence falls on a party that has not accepted it. An issue arising from a concern raised under
`PCI-PML-STD-12.02` that has not been actioned.

**14. AI application.** AI may cluster issue text to detect recurrence, age the register and flag overdue
items, propose owners from the routing history for human confirmation, detect issues with no
consequence stated, and reconcile the register against defect, incident and change systems.

**15. AI prohibition.** An AI system must not own an issue, close one, accept a consequence, or determine
that an issue is a recurrence for the purposes of `PR-04` — that determination is a judgement with a
consequence, and a named human makes it.

**16. AI verification.** **Sampling with a stated basis plus source tracing.** Every AI-proposed
recurrence cluster must be confirmed by a named human against both entries before it is treated as a
recurrence. Each gate, a competent reviewer must draw a stated sample of closed issues and confirm the
resolution statement against the evidence it cites.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of issue
  management as a lifecycle practice distinct from risk management · **EXT-028** · **Manual section 6 category
  3 — international voluntary standard** · currency checked 2026-08-03 · limitation: guidance; voluntary;
  not certifiable.
- **ISO** · *ISO 31000, Risk management — Guidelines* · relied on for: the distinction between an
  uncertain event and one that has occurred · **EXT-020** · **Manual section 6 category 3** · currency checked
  2026-08-03 · limitation: **guidance, expressly not certifiable**; voluntary.
- **ISO** · *ISO 9001, Quality management systems — Requirements* · relied on for: the existence of a
  certifiable requirement to control nonconformity and to take corrective action · **EXT-033** ·
  **Manual section 6 category 3** · currency checked 2026-08-03 · limitation: voluntary unless imported;
  certification concerns a management system; this standard imports none of its requirements.

**18. Jurisdictional caution.** Some issues carry statutory reporting duties — safety incidents, data
breaches, environmental events, regulatory breaches — with their own recipients and timescales that
override any internal register. Obtain local legal advice on mandatory incident reporting for the sector
and jurisdiction.

**19. Related PCI Standards.** `PCI-FND-STD-11`; `PCI-FND-STD-12`; `PCI-FND-STD-15`; `PCI-PML-STD-08.01`;
`PCI-PML-STD-09.01`; `PCI-PML-STD-09.02`; `PCI-PML-STD-12.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 8 · KA 8.1 topic on issues as occurred risks;
KA 8.4 crisis leadership. Also Domain 9 KA 9.3 nonconformance and root-cause analysis; Domain 4 KA 4.4
the decision log.

**21. Compliance test.** Compliance is demonstrated when a reviewer can extract the issue register and
confirm that: (a) every open entry carries one named owner, a required resolution date and a stated
consequence; (b) the ageing profile reported to governance in each period can be reproduced from the
register by an independent query returning the same counts; (c) every entry past its required date by the
documented margin has a matching escalation record; (d) every closed entry carries a resolution statement
whose cited evidence can be retrieved; and (e) for a stated sample of recurring issues, a root-cause
record exists. An open entry with no owner, or a closed entry with no resolution statement, fails the
test.

**22. Breach indicators.** A register whose open count never changes. Closures clustering at period end.
Owners recorded as functions. Issues with required dates in the past and no escalation. The same
description appearing under four unconnected entries. An ageing profile reported as an average rather than
as a distribution. Issues raised by users closed faster than issues raised by assurance.

**23. Consequence within PCI authority.** Correction required; additional review; escalation; examination
failure; certification investigation; suspension or withdrawal — each subject to due process and a right
of appeal.

**24. Examination application.** Evidence selection: which fields make an issue register a control rather
than a list. Scenario judgement: an issue has been open past its date for three cycles and the owner has
left the organisation. Calculation review: reproducing an ageing profile and identifying the two entries
that carry the exposure.

**25. Version and status.** Version 1.0 · **not yet approved** · effective on approval · **new standard** — the
v1.0 set addressed risk and left issues to be inferred from it, which left the occurred-risk population
ungoverned. Amendment note: none.

---
## Domain 9 — Quality, Assurance and Continuous Improvement

### PCI STANDARD PCI-PML-STD-09.01 — Quality Acceptance

**1. Normative requirement.** A credential holder must not record a deliverable as accepted unless a
named acceptance authority has decided, against the version-identified acceptance criteria set before
the deliverable was produced, that it conforms.

**2. Purpose.** Acceptance is the point at which risk transfers — from the producer to the organisation,
and often from the project to an operating team who were not in the room. The failure this prevents is
acceptance by default: the deliverable used, invoiced, or simply not objected to within a review window,
and thereafter treated as accepted by everyone except the people who have to live with it.

**3. Scope.** Every credential holder producing, reviewing, testing, accepting or assuring a deliverable
on a project, programme or portfolio, in every delivery model, including deliverables produced by
suppliers and deliverables accepted on behalf of an operating organisation.

**4. Defined terms.** *acceptance* · *evidence* · *competent reviewer* · *independent* · *material* ·
*decision owner*. Additionally, **nonconformity** means a failure to meet an acceptance criterion;
**carried nonconformity** means a nonconformity accepted with the deliverable, recorded with an owner and
a date.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-09.01-PR-01` — Criteria version-identified and pre-dated.** The acceptance criteria
  applied must carry a version identifier and an agreement date earlier than the deliverable's production
  start, and the acceptance record must cite that version.
- **`PCI-PML-STD-09.01-PR-02` — Acceptance is a positive act.** Acceptance must be recorded as a dated
  decision by the named acceptance authority. Use of the deliverable, payment against it, and the expiry
  of a review period must not be recorded as acceptance.
- **`PCI-PML-STD-09.01-PR-03` — Nonconformities carried, not lost.** Every nonconformity known at
  acceptance must be recorded with a named owner, a due date and the consequence of non-closure, and must
  be reported until closed.
- **`PCI-PML-STD-09.01-PR-04` — The receiving party is in the decision.** Where a deliverable will be
  operated, maintained or used by an organisation other than the project, a named representative of that
  organisation must be recorded in the acceptance decision.
- **`PCI-PML-STD-09.01-PR-05` — Acceptance evidence retrievable.** The test, inspection or review
  evidence cited in an acceptance record must be retained and retrievable for the retention period the
  organisation sets, and the acceptance record must reference it specifically enough to retrieve it.

**6. Prohibited actions.** Accepting against criteria written after production. Recording acceptance
because a review window expired. Accepting on behalf of an operating organisation that was not consulted.
Closing a nonconformity by removing the criterion. Accepting a deliverable whose test evidence cannot be
produced. Splitting a deliverable so each part falls below the acceptance authority's threshold.

**7. Required evidence.** The version-identified acceptance criteria with their agreement date; the
acceptance record naming the authority, the date, the criteria version and the outcome; the
nonconformity register with owners, dates and consequences; the receiving organisation's named
representative in the record; the retained test, inspection or review evidence.

**8. Responsible role.** The named acceptance authority decides. The named credential holder leading the
project answers for the completeness of the acceptance pack and for refusing to record an acceptance that
was not decided.

**9. Approval authority.** The acceptance authority the delegation schedule names, by deliverable class.
A carried nonconformity may be accepted only by the authority that bears its consequence. A supplier may
never accept its own deliverable.

**10. Independence requirement.** The acceptance authority must be **independent of the production** of
the deliverable in the sense defined above. Where the project both produces and accepts — common on small
internal projects — the acceptance authority must sit outside the producing team and the arrangement must
be recorded; and where the deliverable will be operated by another organisation, `PR-04` supplies the
independence that a small structure cannot.

**11. Materiality or threshold.** This standard states no defect count and no pass rate. The organisation's
governance sets the acceptance authority by deliverable class, the classes requiring independent test,
the retention period and the tolerance for carried nonconformities; this standard requires that these exist
and are applied. A nonconformity bearing on safety, a licence condition or a statutory duty is not
carried under any tolerance — it is a mandatory precondition under `PCI-PML-STD-16.01`.
*Six-person internal project:* the criteria are five lines per deliverable agreed at kick-off, acceptance
is a dated line in the log signed by the receiving manager, and the evidence is the test output the team
produced anyway.
*Multi-partner national programme:* acceptance runs at component and integrated levels, the integrated
acceptance names a representative of each receiving organisation, and the criteria are held against a
common template so that a partner cannot accept against its own weaker set.

**12. Exception and waiver.** A deliverable may be accepted with an open nonconformity only where the
nonconformity is recorded with an owner, a date and a consequence, and only by the authority bearing that
consequence. No exception permits acceptance against criteria written after production, and no exception
permits acceptance by expiry of a review window.

**13. Escalation trigger.** A deliverable presented with no pre-dated criteria. A receiving organisation
that declines to be named in the acceptance. A nonconformity the project proposes to close by amending
the criterion. Acceptance evidence that cannot be retrieved. Pressure to accept to release a payment
milestone.

**14. AI application.** AI may check acceptance packs for missing criteria versions, missing evidence
references and missing authorities; compare a deliverable against stated criteria and produce a candidate
conformity assessment for human decision; age carried nonconformities; and detect acceptances recorded
with no paired decision.

**15. AI prohibition.** An AI system must not accept a deliverable, decide conformity, close a
nonconformity, or be recorded as the acceptance authority.

**16. AI verification.** **Independent recomputation or re-test plus sampling with a stated basis.** Where
AI assessed conformity, a competent reviewer must independently re-perform the assessment on a stated
sample of criteria and reconcile any difference before acceptance. Every AI-flagged nonconformity must be
confirmed by a named human before it is recorded, and every AI-cleared criterion in the safety, licence or
statutory subset must be re-checked in full rather than by sample.

**17. External reference.**
- **ISO** · *ISO 9001, Quality management systems — Requirements* · relied on for: the existence of a
  certifiable requirement to verify that outputs meet requirements before release · **EXT-033** ·
  **Manual section 6 category 3 — international voluntary standard** · currency checked 2026-08-03 ·
  limitation: **the certifiable member of its family**, but adoption is voluntary unless a contract or
  regulator requires it; a project is not certified against it; this standard imports none of its
  requirements and reproduces none of its text.
- **ISO** · *ISO 9000, Quality management systems — Fundamentals and vocabulary* · relied on for: the
  existence of an agreed vocabulary distinguishing conformity from nonconformity · **EXT-034**, **not
  independently verified — verify current requirements** · **Manual section 6 category 3** · limitation: a
  vocabulary standard; **not certifiable**; voluntary.
- **ISO** · *ISO 10006, Quality management — Guidelines for quality management in projects* · relied on
  for: the existence of project-specific quality guidance · **EXT-035** · **Manual section 6 category 3** ·
  currency checked 2026-08-03 · limitation: **guidelines, not requirements**; voluntary; not certifiable.

**18. Jurisdictional caution.** Whether acceptance has occurred, what it transfers, and what remedies
survive it are contractual and statutory questions determined by the governing law — and in some sectors
acceptance is regulated separately by a safety, health or licensing authority. Obtain legal advice before
relying on an internal acceptance record in a commercial or regulatory position.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-12`; `PCI-PML-STD-01.03`;
`PCI-PML-STD-05.01`; `PCI-PML-STD-05.02`; `PCI-PML-STD-16.01`; `PCI-PML-STD-16.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 9 · KA 9.1 Quality planning; KA 9.2 Assurance
and control; KA 9.3 Acceptance, nonconformance and root-cause analysis. Also Domain 5 KA 5.4
verification and acceptance; Domain 16 KA 16.1 handover and commissioning.

**21. Compliance test.** Compliance is demonstrated when a reviewer can take a stated sample of
deliverables recorded as accepted and, for each: (a) retrieve the criteria version the acceptance record
cites and confirm its agreement date precedes the production start date; (b) find a dated decision by the
named acceptance authority, who does not appear in the deliverable's authorship or production record;
(c) retrieve the test, inspection or review evidence from the reference in the record; (d) find a named
representative of the receiving organisation where one operates the deliverable; and (e) find every
nonconformity known at acceptance in the register with an owner, a date and a consequence. An acceptance
recorded with no decision, or with criteria post-dating production, fails the test.

**22. Breach indicators.** Acceptance dates identical to payment dates. Criteria versions issued the week
of acceptance. Acceptance authorities who also appear as deliverable authors. Nonconformity registers that
empty at handover. Receiving organisations first seeing a deliverable after acceptance. Evidence
references that name a folder rather than a document.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Evidence selection: which artefacts establish that acceptance occurred.
Scenario judgement: a supplier claims acceptance because the client used the deliverable for six weeks.
Ethical dilemma: a leader is asked to accept a deliverable with an open safety nonconformity to release a
payment milestone.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-09-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the
positive-act rule, the receiving-party rule and the evidence-retrievability rule separated into process
requirements; compliance test replaced with a five-part retrieval test.

---

### PCI STANDARD PCI-PML-STD-09.02 — Lessons Learned and Organisational Retention

**1. Normative requirement.** A credential holder must convert each lesson accepted from a review into a
change to a named standing artefact — a process, a checklist, a template, an estimating basis, a
criterion or a training item — with a named owner and a date.

**2. Purpose.** Lessons-learned processes are near-universal and largely ineffective, and the reason is
structural rather than cultural: a lesson recorded in a repository nobody queries at the moment of
decision has no route into behaviour. The failure this prevents is the well-run review whose output
changes nothing, repeated on the next project by people who could not have found it.

**3. Scope.** Every credential holder conducting, contributing to, approving or assuring a retrospective,
a post-implementation review, a post-project review or a root-cause analysis, on projects, programmes and
portfolios, in every delivery model.

**4. Defined terms.** *evidence* · *decision owner* · *material* · *acceptance* · *competent reviewer*.
Additionally, **standing artefact** means a document, model, template, checklist, criterion set or
training item used on future work; **accepted lesson** means a lesson the review's approving authority has
accepted as valid.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-09.02-PR-01` — Each accepted lesson names its artefact.** Every accepted lesson must
  name the standing artefact it changes, the owner of that artefact and the date by which the change is
  made. A lesson recorded with no target artefact does not satisfy this requirement.
- **`PCI-PML-STD-09.02-PR-02` — Review separated from assessment.** A post-project or post-implementation
  review must be conducted separately from any individual performance assessment, and its record must not
  be used as evidence in one. Where the two are combined, the review's output is not evidence of
  compliance with this standard.
- **`PCI-PML-STD-09.02-PR-03` — Root cause where a defect recurs.** Where a defect, issue or
  nonconformity recurs within the window the governance sets, the credential holder must commission a
  root-cause analysis and must record its conclusion and the artefact change it produces.
- **`PCI-PML-STD-09.02-PR-04` — Retrieval at the point of decision.** The credential holder must record
  where each changed artefact is held and how a future project reaches it at the moment it makes the
  corresponding decision. A lesson retrievable only by searching a repository does not satisfy this
  requirement.

**6. Prohibited actions.** Closing a review with a list of observations and no artefact changes. Using a
review to attribute blame. Recording a lesson as "implemented" when only the repository entry changed.
Deferring root-cause analysis of a recurrence to the next project. Reporting a lesson count as evidence of
learning.

**7. Required evidence.** The review record with attendance, method and accepted lessons; the artefact
change record per accepted lesson with owner and date; the changed artefacts themselves, versioned; the
root-cause analyses for recurrences with their conclusions; the retrieval route recorded for each changed
artefact.

**8. Responsible role.** The named credential holder leading the project, for commissioning the review and
for routing accepted lessons to artefacts. The named artefact owner, for making the change. The function
that owns the standing artefact set, for its retrievability.

**9. Approval authority.** The review's approving authority accepts or rejects each lesson, with reasons.
The artefact owner approves the change to their artefact. Neither may be the person whose work the lesson
concerns, where the lesson attributes a cause to that work.

**10. Independence requirement.** A root-cause analysis under `PR-03` must be led by a competent reviewer
independent of the work in which the defect arose. A retrospective within an adaptive team may be
facilitated internally, and independence attaches instead to the periodic sampling of artefact changes at
gates.

**11. Materiality or threshold.** This standard states no lesson count and no reuse rate. The organisation's
governance sets the recurrence window that triggers root-cause analysis, the review points in the
lifecycle and the retention period for review records; this standard requires that they exist and are applied.
Where an organisation measures the reuse of lessons, the credential holder must record the basis of the
measure so that it is interpretable rather than decorative.
*Six-person internal project:* one retrospective per iteration and one at closure; accepted lessons change
the team's own checklist, which is the standing artefact, and retrieval is that the checklist is opened at
the start of the next piece of work.
*Multi-partner national programme:* reviews run per component with a programme-level synthesis; artefact
owners sit in the enterprise function rather than in a partner, because a lesson lodged only in a
supplier's method leaves with the supplier.

**12. Exception and waiver.** An exception permitting an accepted lesson to be recorded with no artefact
change may be approved by the review's approving authority only where the lesson is recorded as
project-specific and incapable of generalisation, with that reason stated. No exception permits a
recurrence to go without root-cause analysis where the governance's window has been met.

**13. Escalation trigger.** A recurrence meeting the documented window. An artefact owner who declines the
change. A lesson concerning a safety, licence or statutory matter. A review that cannot be held because
the team has dispersed — which is itself a planning failure and is escalated as one.

**14. AI application.** AI may cluster a large lessons repository into recurring themes, propose candidate
artefact targets for human decision, detect recurrences across issue and defect data, draft the change to
a checklist or template for the artefact owner's approval, and test whether a proposed artefact change
would have been retrievable at the decision point it targets.

**15. AI prohibition.** An AI system must not conduct the review, decide which lessons enter the standing
process, accept or reject a lesson, or approve a change to a standing artefact. A review is partly an act
of collective sense-making by the people who were there, and a generated summary is not that.

**16. AI verification.** **Source tracing plus named approval.** Every AI-produced theme must be traced by
a named human to the underlying review entries before it is accepted as a lesson, and the artefact owner
must approve the resulting change by name. Every AI-detected recurrence must be confirmed by a named human
against both underlying records.

**17. External reference.**
- **ISO** · *ISO 9001, Quality management systems — Requirements* · relied on for: the existence of a
  certifiable requirement for corrective action addressing causes · **EXT-033** · **Manual section 6 category 3
  — international voluntary standard** · currency checked 2026-08-03 · limitation: voluntary unless
  imported; certification concerns a management system; no requirement is imported here.
- **ISO** · *ISO 10006, Guidelines for quality management in projects* · relied on for: the existence of
  project-level improvement guidance · **EXT-035** · **Manual section 6 category 3** · currency checked
  2026-08-03 · limitation: **guidelines, not requirements**; voluntary; not certifiable.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of lessons capture
  as a lifecycle practice · **EXT-028** · **Manual section 6 category 3** · currency checked 2026-08-03 ·
  limitation: guidance; voluntary; not certifiable.

**18. Jurisdictional caution.** Review records can be disclosable in litigation, arbitration, a regulatory
investigation or a public inquiry, and privilege rules differ by jurisdiction. Obtain legal advice before
conducting a review into an event with legal or regulatory exposure, and never let that advice become a
reason to hold no review.

**19. Related PCI Standards.** `PCI-FND-STD-12`; `PCI-FND-STD-02`; `PCI-PML-STD-08.02`;
`PCI-PML-STD-09.01`; `PCI-PML-STD-12.02`; `PCI-PML-STD-16.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 9 · KA 9.4 Lessons learned, continuous
improvement, data quality and AI-output quality · topic 9.4.1 lessons that change behaviour. Also
Domain 16 KA 16.3 knowledge transfer and the post-project review.

**21. Compliance test.** Compliance is demonstrated when a reviewer can take the accepted lessons from
every review held in the period and, for each: (a) name the standing artefact it targets; (b) open that
artefact and find the change, in a version dated on or before the recorded due date; (c) follow the
recorded retrieval route and reach the changed artefact from the decision point it targets, without
searching a repository; and (d) for every recurrence meeting the documented window, find a root-cause
analysis led by someone independent of the work concerned. Accepted lessons with no artefact change fail
the test, however many were recorded.

**22. Breach indicators.** Lesson counts reported with no artefact-change counts. Review outputs that are
lists of observations. The same lesson recorded on consecutive projects. Root-cause analyses led by the
team that produced the defect. Artefact changes dated long after their due dates. Review records
referenced in appraisal documents.

**23. Consequence within PCI authority.** Correction required; additional review; escalation; examination
failure; certification investigation; suspension or withdrawal — each subject to due process and a right
of appeal.

**24. Examination application.** Scenario judgement: a review produces eleven observations and the sponsor
asks which of them are lessons. Evidence selection: which artefact proves a lesson changed behaviour.
Calculation review: the reuse economics of a review, and where its value is actually lost.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-09-02` v1.0 (*Organisational Learning*). Amendment note: renumbered, retitled and restructured;
legislative drafting removed; the principal obligation changed from conducting reviews to **converting
accepted lessons into named artefact changes**, which is the act that can be verified; retrieval at the
point of decision added as an express process requirement.

---

## Domain 10 — Procurement, Contracts and Supply Networks

### PCI STANDARD PCI-PML-STD-10.01 — Procurement Fairness

**1. Normative requirement.** A credential holder must evaluate every tender, bid or proposal against the
evaluation criteria and weightings published to bidders before submissions were received, and must not
apply any criterion, weighting or consideration that was not published.

**2. Purpose.** Procurement is where delivery meets money, incumbency and relationships, and it is the
process most exposed to the interest that decides quietly. The failure this prevents is the evaluation
that reaches the intended answer through a criterion introduced afterwards, a weighting adjusted at
moderation, or a clarification granted to one bidder and not to others.

**3. Scope.** Every credential holder specifying, running, evaluating, moderating, recommending, awarding
or assuring a procurement, and every credential holder administering a contract in a way that affects
competition — including framework call-offs, single-source justifications, extensions and variations that
displace competition.

**4. Defined terms.** *conflict of interest* · *independent* · *evidence* · *decision owner* · *material*
· *acceptance*. Additionally, **published criteria** means the evaluation criteria and weightings issued
to all bidders before the submission deadline; **moderation** means the reconciliation of evaluators'
individual scores into a panel position.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-10.01-PR-01` — Criteria and weightings published before submission.** The evaluation
  criteria and their weightings must be published to all bidders before the submission deadline, and must
  not be changed after it except by an addendum issued to all bidders with an extended deadline.
- **`PCI-PML-STD-10.01-PR-02` — Individual scores recorded before moderation.** Each evaluator must
  record their own score and its written rationale against each published criterion before seeing any
  other evaluator's score, and those individual records must be retained.
- **`PCI-PML-STD-10.01-PR-03` — Moderation changes are reasoned and retained.** Where moderation changes
  a score, the change and its reason must be recorded against the individual score it replaced. A
  moderated score with no recorded reason does not satisfy this requirement.
- **`PCI-PML-STD-10.01-PR-04` — Information symmetry.** Any clarification, correction or additional
  information given to one bidder that could affect a submission must be given to all bidders, and the
  record must show what was issued, to whom and when.
- **`PCI-PML-STD-10.01-PR-05` — Panel interests cleared before evaluation.** Every panel member must have
  a declaration or a nil return recorded under `PCI-PML-STD-01.03-PR-03` before evaluation begins, and a
  member with an interest in a bidder must be excluded under `PCI-PML-STD-01.03-PR-07`.

**6. Prohibited actions.** Introducing an unpublished criterion, including "fit", "confidence" or
"deliverability", unless it was published. Adjusting weightings after submissions. Moderating to a
predetermined outcome. Giving one bidder information not given to others. Allowing an evaluator with an
interest to score, moderate, chair or ratify. Accepting hospitality, employment discussion or any benefit
from a bidder during a live procurement. Splitting a requirement to avoid a competition threshold.

**7. Required evidence.** The published criteria and weightings with their issue date; the individual
evaluator score sheets with rationales and timestamps; the moderation record showing each change and its
reason; the clarification log showing symmetric issue; the panel interests record; the award recommendation
and the award decision with its authority; the debrief records issued to unsuccessful bidders where
applicable.

**8. Responsible role.** The named credential holder running the procurement, for the process. Each named
evaluator, for their own scores and declarations. The named award authority, for the award.

**9. Approval authority.** The award authority the delegation schedule assigns, applying the aggregation
rule where a series of call-offs or extensions aggregates. A single-source or direct-award justification
must be approved by the authority the schedule names for the aggregate value, never by the requesting
project alone.

**10. Independence requirement.** Every evaluator must be independent of every bidder in the sense defined
above. The moderation chair must be independent of the requesting project where the delegation schedule
requires it. Assurance of the procurement must be provided by a competent reviewer independent of the
panel and of the requesting project, and must satisfy the name-matching test in
`PCI-PML-STD-01.03-PR-06`.

**11. Materiality or threshold.** This standard states no value threshold and no minimum bidder count. The
organisation's governance — and, in the public sector, the applicable procurement regime — sets the
competition thresholds, the award authorities, the aggregation rule and the standstill or debrief
obligations; this standard requires that the documented rules exist and are applied and that no requirement is
split to fall below one.
*Six-person internal project:* a single quotation exercise with three suppliers, criteria of four lines
sent with the request, individual scores recorded in a shared sheet before discussion, and the leader's
own declaration recorded before scoring begins.
*Multi-partner national programme:* multiple concurrent competitions with evaluators drawn from several
organisations; the interests register runs across all of them, the aggregation rule states whether
call-offs across partners aggregate, and the assurance reviewer is independent of every partner rather
than of one.

**12. Exception and waiver.** A departure from competition — single source, direct award, extension —
may be approved only by the authority the delegation schedule assigns to the aggregate value, only on a
written justification stating the ground, the alternatives considered and the period, and only where the
justification is retained and reported. **No exception is permitted** to `PR-01`, `PR-02` or `PR-05`:
criteria published after submissions, scores recorded after moderation, and undeclared panel interests are
breaches in every case.

**13. Escalation trigger.** A proposal to change criteria or weightings after submissions. An evaluator's
interest disclosed after scoring has begun. Information given to one bidder only. A moderation outcome
that reverses the individual scores with no recorded reason. A bidder's approach to an evaluator outside
the process. A series of call-offs aggregating above the award authority.

**14. AI application.** AI may check submissions for completeness against the published requirements,
extract stated commitments for evaluator verification, compute weighted scores from evaluator inputs,
detect divergence between individual and moderated scores, and check the clarification log for asymmetric
issue.

**15. AI prohibition.** An AI system must not score a submission on a qualitative criterion in place of a
named evaluator, decide an award, moderate scores, or determine that a bidder is non-compliant. Where AI
produces a ranking, it has produced a recommendation, and the award is a reserved decision class under
`PCI-PML-STD-01.02-PR-01`.

**16. AI verification.** **Independent recomputation plus source tracing.** Every weighted score computed
with AI assistance must be recomputed by hand before the recommendation is issued. Every extracted
commitment must be traced by a named evaluator to the page and version of the submission it came from
before it is scored. Where AI screened submissions for compliance, a competent reviewer must re-check in
full every submission the system would exclude.

**17. External reference.**
- **FIDIC** · *FIDIC suite of conditions of contract* · relied on for: the existence of standard-form
  contractual structures a procurement may adopt · **EXT-050** · **Manual section 6 category 4 — contract
  framework** · currency checked 2026-08-03 · limitation: **binds only the parties who adopt it, by
  signature**; characterised generically, no clause numbers cited, no text reproduced; it is not
  legislation.
- **NEC** · *NEC4 suite of contracts* · relied on for: the existence of an alternative standard-form
  structure with its own change and notification mechanics · **EXT-051** · **Manual section 6 category 4** ·
  currency checked 2026-08-03 · limitation: as above.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of procurement
  within delivery practice · **EXT-028** · **Manual section 6 category 3 — international voluntary standard** ·
  currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **Project Management Institute** · *Code of Ethics and Professional Conduct* · relied on for: the
  existence of a professional expectation of fairness and impartiality in supplier dealings ·
  **EXT-063**, **not independently verified — verify current requirements** · **Manual section 6 category 6 —
  ethical code** · limitation: binding only where a body, regulator or engagement has adopted it.

**18. Jurisdictional caution.** Public-procurement law, subsidy control, competition law, anti-bribery
law and sector regulation impose binding, jurisdiction-specific rules on advertising, thresholds,
standstill periods, debriefs, remedies and record-keeping, and breach can void an award or engage criminal
liability. **This standard does not state any of those rules and compliance with it is not compliance with
them.** Obtain local legal advice before running a regulated procurement.

**19. Related PCI Standards.** `PCI-FND-STD-08`; `PCI-FND-STD-09`; `PCI-FND-STD-01`; `PCI-PML-STD-01.03`;
`PCI-PML-STD-03.02`; `PCI-PML-STD-04.01`; `PCI-PML-STD-09.01`.

**20. Related Body of Knowledge content.** PML-AI · Domain 10 · KA 10.1 Make-or-buy and the procurement
lifecycle; KA 10.2 Tendering and evaluation; KA 10.3 Contract strategy and supplier governance; KA 10.4
Claims, change and disputes; ethical sourcing; supply resilience.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) retrieve the published
criteria and weightings and confirm their issue date precedes the submission deadline; (b) retrieve every
evaluator's individual score sheet and confirm each carries a rationale and a timestamp preceding the
moderation meeting; (c) recompute the moderated result from the individual scores and the published
weightings, and account for every difference by a recorded moderation reason; (d) confirm every
clarification in the log was issued to all bidders; and (e) confirm every panel member has a declaration
or nil return dated before evaluation began. A criterion in the evaluation that does not appear in the
published set fails the test outright, whatever the merits of the winning bid.

**22. Breach indicators.** Moderated scores that reverse the individual ranking with brief or absent
reasons. Criteria documents with version dates after the deadline. Clarifications issued to one bidder.
Panel interests registers with no entries in a market with a long-standing incumbent. Award values
clustering just below a competition threshold. Evaluators who scored and then joined the winning bidder.
Debriefs that describe criteria the bidders were never given.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal of
the PML-AI credential — each subject to due process and a right of appeal. **PCI cannot set aside an
award, impose a fine or create any legal liability**; those consequences, where they exist, arise under
the applicable procurement or criminal law and not under this standard.

**24. Examination application.** Ethical dilemma: at moderation the chair proposes to add a
"deliverability" adjustment that was not published. Evidence selection: which artefacts would establish
that an evaluation was fair. Scenario judgement: an evaluator discloses, after scoring, that they have
been approached about a role with a bidder.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-10-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed;
individual-scores-before-moderation, reasoned moderation changes, information symmetry and panel-interest
clearance separated into process requirements; the link to `PCI-PML-STD-01.03` made explicit, which is the
change that gives this standard an enforceable rule about who is barred from scoring; consequence field
corrected to
state expressly what PCI cannot do.

---

## Domain 11 — Stakeholders, Communication and Influence

### PCI STANDARD PCI-PML-STD-11.01 — Stakeholder Transparency

**1. Normative requirement.** A credential holder must not issue, endorse or allow to stand a report or
communication about delivery status that omits a material adverse fact known to them at the time of
issue.

**2. Purpose.** Reporting is where every other control either works or is neutralised, because a control
that detects a problem and a report that omits it produce the same outcome as no control at all. The
failure this prevents is the technically true report: every figure correct, every omission deliberate, and
the reader left with a false picture they could not have detected.

**3. Scope.** Every credential holder preparing, approving, presenting or assuring a status report,
dashboard, board paper, briefing or external communication about a project, programme or portfolio, and
every credential holder who becomes aware that an issued report is materially misleading.

**4. Defined terms.** *material* · *evidence* · *decision owner* · *sponsor* · *escalation threshold* ·
*baseline* (control sense). Additionally, **materially misleading** means capable of causing a
recipient reading
it in the ordinary course of their role to form a view of status, risk or forecast that the known facts
do not support; **status
report** means any recurring or ad hoc statement of delivery position issued to a decision-maker.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-11.01-PR-01` — Adverse facts appear in the same document as the good ones.** A material
  adverse fact must appear in the report itself, not only in an appendix, a footnote, a verbal briefing or
  a separate document sent to a narrower list.
- **`PCI-PML-STD-11.01-PR-02` — Status colours are defined and derived.** Where a report uses a rating,
  colour or index, its derivation rule must be documented and the reported value must follow it. A rating
  set by judgement against a documented rule must record the override, its reason and its author.
- **`PCI-PML-STD-11.01-PR-03` — One version of status.** The credential holder must not issue different
  status positions to different audiences for the same period; where audiences require different depth,
  the underlying position must be identical and the difference must be one of detail only.
- **`PCI-PML-STD-11.01-PR-04` — Correction of an issued report.** On discovering that an issued report was
  materially misleading, the credential holder must issue a correction to every recipient of the original,
  within the time the escalation threshold states.
- **`PCI-PML-STD-11.01-PR-05` — Refusal is recorded.** Where the credential holder is instructed to issue
  a report they consider materially misleading, they must record the instruction, record their objection
  in writing, and escalate under `PCI-FND-STD-11`; they must not issue it in their own name.

**6. Prohibited actions.** Removing an adverse item because a decision on it is expected shortly. Reporting
a rating that the documented rule does not produce, without recording the override. Briefing the sponsor
on a position the board paper does not contain. Presenting a forecast as a range whose stated bounds
exclude the outcome currently most likely. Attributing an omission to space. Allowing an issued report
known to be misleading to stand uncorrected.

**7. Required evidence.** Issued reports with their dates, versions and distribution lists; the rating
derivation rule and the override records; the correction notices with their distribution; the record of
any instruction to report otherwise and the objection raised; the escalation record.

**8. Responsible role.** The named credential holder who issues or endorses the report. The **sponsor**,
for the position communicated to the governing body. Neither may attribute the report to "the project".

**9. Approval authority.** The report's named approver, as the governance arrangements set. A rating
override may be approved only by the named authority the derivation rule identifies. Nobody may approve
the omission of a material adverse fact.

**10. Independence requirement.** Not applicable to routine status reporting, which is properly prepared
by the delivery organisation; independence attaches at gates, where a competent reviewer independent of
the delivery organisation must be able to reach a status position from the underlying data and compare it
with the reported one.

**11. Materiality or threshold.** This standard states no variance percentage. Materiality is decided by the
definition above, applied against the decisions the report informs, and the organisation's governance sets
the reporting tolerances, the rating derivation rule and the correction time; this standard requires that these
exist and are applied. **A fact bearing on safety, legality, a licence or a statutory duty is material at
any size** and no tolerance excludes it.
*Six-person internal project:* a weekly half-page with a stated rating rule of three lines and one
distribution list, corrected by replying to the same thread.
*Multi-partner national programme:* a consolidated report whose derivation rule states how component
ratings combine — and states explicitly that a component's adverse item is not averaged away by others —
with the same underlying position issued at every level and the depth varying, not the substance.

**12. Exception and waiver.** An adverse fact may be withheld from a wider distribution only where its
disclosure would breach a legal duty of confidence, prejudice a legal position on written legal advice, or
disclose personal data unlawfully — and in every such case it must still be disclosed to the sponsor and
to the governing body, with the ground for the restriction recorded. **Commercial embarrassment, an
expected fix and the imminence of a decision are not grounds.**

**13. Escalation trigger.** An instruction to omit or soften a material adverse fact. A rating override
that the derivation rule does not support. Discovery that an issued report is materially misleading. A
divergence between the position briefed verbally and the position reported. A supplier's refusal to supply
status data needed for a complete report.

**14. AI application.** AI may draft status narrative from underlying data, reconcile the narrative to the
data and flag statements the data does not support, compute ratings from the documented derivation rule,
compare successive reports for items that disappeared, and check distribution consistency across audiences.

**15. AI prohibition.** An AI system must not approve or issue a report, decide whether a fact is
material, set a rating that overrides the documented rule, or generate a status narrative that is issued
without a named human's verification against the underlying data.

**16. AI verification.** **Reconciliation plus clause-to-summary comparison.** Before issue, the credential
holder must reconcile every figure in an AI-drafted report to its source system extract, with the extract
date recorded, and must compare each summarised statement against the source it summarises to confirm it
carries the same position **including its qualifications** — the failure mode of generated narrative being
the quiet loss of a caveat. Ratings computed with AI assistance must be recomputed against the derivation
rule by hand.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of reporting and
  information management within delivery practice · **EXT-028** · **Manual section 6 category 3 — international
  voluntary standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **ISO** · *ISO 21505, Guidance on governance* · relied on for: the existence of an information flow from
  delivery to the governing body · **EXT-032**, **not independently verified** · **Manual section 6 category 3**
  · limitation: as above, with an open verification status.
- **Project Management Institute** · *PMBOK Guide* · relied on for: the existence of performance reporting
  in professional practice · **EXT-060** · **Manual section 6 category 5 — professional framework** · currency
  checked 2026-08-03 · limitation: **never regulatory authority**; no edition asserted; no text reproduced.

**18. Jurisdictional caution.** Statements about a project can engage listed-company disclosure duties,
prospectus and market-abuse rules, grant-condition reporting, public-inquiry duties and fraud law, and the
threshold for a legally material statement is not the same as the professional one used here. Obtain legal
advice before any statement about delivery status is made outside the organisation.

**19. Related PCI Standards.** `PCI-FND-STD-11` (the parent escalation obligation); `PCI-FND-STD-02`;
`PCI-FND-STD-15`; `PCI-FND-STD-14`; `PCI-PML-STD-06.01`; `PCI-PML-STD-07.01`; `PCI-PML-STD-08.01`;
`PCI-PML-STD-12.01`.
**What this standard adds to `PCI-FND-STD-11`:** the foundational standard requires a material misstatement to
be escalated. This standard adds the delivery-specific mechanics by which honest figures produce a
dishonest
picture — **same-document disclosure**, a **documented rating derivation with recorded overrides**, **one
version of status across audiences**, a **correction duty to the original distribution**, and a **recorded
refusal**.

**20. Related Body of Knowledge content.** PML-AI · Domain 11 · KA 11.1 Stakeholder systems and engagement
strategies; KA 11.2 Executive communication and reporting; KA 11.4 Public and community stakeholders,
cross-cultural communication and AI-generated communication risk. Also Domain 3 KA 3.3 the decision record.

**21. Compliance test.** Compliance is demonstrated when a reviewer can, for a stated sample of reporting
periods: (a) rebuild the reported rating from the documented derivation rule and the period's underlying
data, and account for every difference by a recorded override with a named author; (b) list the material
adverse facts recorded in the risk, issue, change and cost records in that period and find each one in the
report itself rather than in a separate document; (c) compare every version of the period's report issued
to different audiences and confirm that the underlying position is identical; and (d) for every correction
issued, confirm its distribution matched the original's. A material adverse fact present in the underlying
records and absent from the report fails the test.

**22. Breach indicators.** Ratings that stay green until the period before a gate. Adverse items appearing
first in an appendix. Two decks for the same period with different messages. Corrections issued to a
narrower list than the original. Overrides recorded with no author. Reports whose figures cannot be
reproduced from the source systems. A pattern of items disappearing between draft and issue.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Ethical dilemma: the sponsor asks for an adverse item to be held for one
cycle because a fix is expected. Evidence selection: which artefacts establish that a report was complete.
Calculation review: rebuilding a status rating from its derivation rule and identifying the unrecorded
override.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-11-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the
same-document rule, the rating derivation rule, the one-version rule, the correction duty and the recorded
refusal separated into five process requirements; compliance test replaced with a rebuild-and-compare test
performed from source records.

---

## Domain 12 — Leadership, Teams and Organisational Behaviour

### PCI STANDARD PCI-PML-STD-12.01 — Leadership Conduct

**1. Normative requirement.** A credential holder must not use their authority to cause another person to
prepare, sign, present or withhold a professional statement that the person has told them, or has
recorded, is inaccurate or incomplete.

**2. Purpose.** Leadership conduct standards fail when they are written as character requirements, because
character cannot be audited and an unverifiable requirement is not a requirement. This standard is therefore
drafted around the single leadership act that is both observable and gravely damaging: **the use of
positional authority to override another professional's judgement about their own work.** Every reporting,
estimating, scheduling and assurance control in this set depends on the person nearest the facts being
able to state them; this standard protects that, and leaves inspiration to the Body of Knowledge.

**3. Scope.** Every credential holder exercising authority over another person's professional output —
line, project, matrix, contractual or client-side — including over supplier staff, seconded staff and
assurance staff, and including instructions given verbally.

**4. Defined terms.** *material* · *evidence* · *decision owner* · *competent reviewer* · *detriment* ·
*independent*. Additionally, **professional statement** means an estimate, forecast, schedule, assessment,
opinion, test result, assurance conclusion or status report issued by a person in their professional
capacity; **override** means a leader's decision to issue a different position from the one the preparer
holds.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-12.01-PR-01` — Overrides are recorded in writing.** Where a credential holder issues a
  professional statement different from the preparer's position, they must record the change, their
  reason and their own name, and must not present the result as the preparer's position.
- **`PCI-PML-STD-12.01-PR-02` — The preparer's position travels with the output.** The preparer must be
  able to record their own position, and that record must accompany the output to the decision-maker.
  Removing, summarising away or declining to transmit a preparer's recorded position does not satisfy
  this requirement.
- **`PCI-PML-STD-12.01-PR-03` — Attribution is accurate.** Analysis, drafting and professional
  conclusions must be attributed to the person who produced them, and a credential holder must not sign as
  preparer, author or reviewer of work they did not perform.
- **`PCI-PML-STD-12.01-PR-04` — Assessment against criteria set in advance.** Where a credential holder
  contributes to a performance assessment of a team member, the assessment must be recorded against
  criteria set before the period assessed. An assessment altered after the person raised a concern is
  subject to `PCI-PML-STD-12.02-PR-04`.
- **`PCI-PML-STD-12.01-PR-05` — First adverse report acknowledged in writing.** A material adverse fact
  reported to the credential holder by a team member must be acknowledged in writing within the time the
  governance sets, with a recorded statement of what will be done about it and by whom.

**6. Prohibited actions.** Instructing a person to change a professional statement they have recorded as
accurate, without recording the override in one's own name. Removing a preparer's recorded position from
a pack. Signing as author or reviewer of work one did not do. Attributing a team member's analysis to
oneself. Assessing a person against criteria introduced after the period. Responding to a first report of
bad news by questioning the reporter's competence in place of addressing the fact.

**7. Required evidence.** Override records with the changed position, the reason and the leader's name;
preparers' recorded positions as transmitted to decision-makers; authorship and sign-off records; the
performance-assessment criteria with their date of setting; the written acknowledgements of adverse
reports with their response statements and dates.

**8. Responsible role.** The named credential holder exercising the authority. Where the instruction comes
from above the credential holder, the credential holder's obligation is to record it and escalate under
`PCI-FND-STD-11`, not to transmit it.

**9. Approval authority.** A credential holder may override a professional statement within their own
authority and must record it. They must not approve the removal of the preparer's recorded position; only
the receiving decision-maker may decide that they do not need it, and that decision is itself recorded.

**10. Independence requirement.** Where an override concerns an assurance conclusion, a test result or a
safety assessment, the override must be reviewed by a competent reviewer independent of both the leader
and the preparer before the statement is issued. For estimates, forecasts and schedules, independence
attaches at gates under the standard governing that artefact.

**11. Materiality or threshold.** This standard sets no number. The organisation's governance sets the time for
acknowledging an adverse report and the classes of statement whose override requires independent review;
this standard requires that both exist and are applied. Every override of a professional statement is recorded
regardless of size, because the pattern of overrides is the evidence, and a threshold would remove the
population that reveals it.
*Six-person internal project:* the override record is a line in the same document with the leader's name
and a sentence of reason; the acknowledgement is a reply within one working day.
*Multi-partner national programme:* the same obligation attaches at each tier and across organisational
boundaries, and the partner agreement states that a client-side leader's override of a supplier's
professional statement is recorded in the client's record — the case where an override is otherwise
invisible because it crossed a contract.

**12. Exception and waiver.** **No exception is permitted** to the principal obligation. An exception to
`PR-02` — not transmitting the preparer's recorded position — may be granted only by the receiving
decision-maker, in writing, and the fact that a position exists and was not transmitted must still be
disclosed to them.

**13. Escalation trigger.** An instruction from any level to change a professional statement the preparer
holds to be accurate. A preparer's position removed from a pack. A request to sign as author of work one
did not do. An assessment altered after a concern was raised. An adverse report unacknowledged beyond the
documented time.

**14. AI application.** AI may compare successive document versions and surface changes to professional
conclusions, check that authorship metadata matches recorded sign-offs, age adverse reports against the
acknowledgement time, and flag assessments whose criteria post-date the period assessed.

**15. AI prohibition.** An AI system must not override a professional statement, decide that an override
is justified, author a professional statement attributed to a named person, or determine whether an
adverse report requires action.

**16. AI verification.** **Clause-to-summary comparison plus named approval.** Where AI produced a summary
of a professional statement for a decision pack, a named human must compare the summary against the
statement and confirm it carries the same position and the same qualifications; and the preparer must
confirm by name that the transmitted summary is theirs before it is issued as such.

**17. External reference.**
- **Project Management Institute** · *Code of Ethics and Professional Conduct* · relied on for: the
  existence of professional expectations of honesty, responsibility, respect and fairness · **EXT-063**,
  **not independently verified — verify current requirements** · **Manual section 6 category 6 — ethical code** ·
  limitation: binding only where a body, regulator or engagement has adopted it; a PCI credential holder
  not subject to it is not made subject to it by this standard; no text is reproduced.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of leadership and
  team-development activity within delivery practice · **EXT-028** · **Manual section 6 category 3 —
  international voluntary standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not
  certifiable.
- **ISO** · *ISO 45001, Occupational health and safety management systems — Requirements* · relied on for:
  the existence of a certifiable management-system standard within which worker consultation and
  participation are addressed · **EXT-123** · **Manual section 6 category 3** · currency checked 2026-08-03 ·
  limitation: certifiable, but voluntary unless required by contract or regulator; this standard imports none
  of its requirements.

**18. Jurisdictional caution.** Employment law, collective agreements, professional-body rules and, in
regulated professions, statutory duties govern what may be required of a professional and what protection
they have when they refuse. Obtain local legal and human-resources advice before acting on an override
dispute involving an employee, a contractor or a regulated professional.

**19. Related PCI Standards.** `PCI-FND-STD-13`; `PCI-FND-STD-01`; `PCI-FND-STD-10`; `PCI-FND-STD-11`;
`PCI-PML-STD-11.01`; `PCI-PML-STD-12.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 12 · KA 12.1 Leadership theories in practice and
emotional intelligence; KA 12.3 Delegation, coaching and difficult conversations; KA 12.4 Remote and
multicultural teams, ethical leadership. Also Domain 1 KA 1.2 the professional standard of care;
Domain 11 KA 11.3 negotiation and conflict.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) take a stated sample of
professional statements issued in the period and, for each whose final position differs from the
preparer's earlier version in the document history, find an override record carrying the leader's name and
a reason; (b) confirm that the preparer's recorded position accompanied the output to the decision-maker,
or that a written exception from the decision-maker exists disclosing that a position was withheld;
(c) match authorship and sign-off records and find no statement signed by a person who did not perform the
work; (d) confirm every performance-assessment criteria set in the period is dated before the period it
assesses; and (e) compute, for a stated sample of adverse reports from team members, the elapsed time to
written acknowledgement and confirm it is within the documented time. A changed professional conclusion
with no override record fails the test.

**22. Breach indicators.** Document histories in which a preparer's figures change with no override
record. Packs in which the preparer's caveats appear in early versions and not in the issued one.
Sign-offs by people with no editing history on the document. Assessment criteria dated at the end of the
period. Adverse reports with no acknowledgement. A pattern of estimates revised downward in the version
immediately before issue.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal of
the PML-AI credential — each subject to due process and a right of appeal.

**24. Examination application.** Ethical dilemma: a leader is instructed by a director to reissue a
supplier's schedule with a completion date the supplier's planner has recorded as unachievable. Evidence
selection: which artefacts would establish that an override occurred. Scenario judgement: a preparer asks
for their dissent to be recorded and the leader considers it unhelpful to the decision.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-12-01` v1.0. Amendment note: renumbered, restructured and **substantially rewritten**. The v1.0
standard stated general conduct expectations that could not be audited — audit questions 5 and 6 both failed —
and the principal obligation has been replaced with the single observable act of overriding another
person's professional statement, supported by five process requirements each of which leaves a record.

---

### PCI STANDARD PCI-PML-STD-12.02 — Route to Raise a Concern and Freedom from Detriment

**1. Normative requirement.** A credential holder leading delivery must establish, publish and operate a
route by which any person working on or for the project may raise a concern to a named recipient who is
neither the subject of the concern nor in the subject's reporting line.

**2. Purpose.** Every control in this set consumes voluntary upward information. Risk identification,
nonconformance reporting, interface issues and escalation lead time all fail silently in the same way when
people learn that raising a problem is penalised, and the leader becomes the last person on the project to
know. **This standard does not require a state of mind and does not require a leader to produce a feeling in
another person.** A state of mind cannot be observed, cannot be audited and cannot be breached, so a standard
written around one would be an aspiration wearing a standard's clothing — the exact defect the Drafting Manual
prohibits. What this standard requires instead is three things that are entirely observable: **a route that
goes around the person the concern is about**, **the absence of detriment to the person who used it**, and
**a record showing the concern was answered.** Those are the conditions under which candour is possible;
whether candour then occurs is a matter of leadership and is taught, not legislated.

**3. Scope.** Every credential holder leading a project, programme or portfolio team, including
distributed, multicultural, seconded, contracted and supplier teams, and including concerns raised
anonymously or through a third party. It applies to concerns about delivery, conduct, safety, legality and
the accuracy of reporting. It does not displace an employer's statutory whistleblowing, grievance or
safety-reporting arrangements, which run in parallel and take precedence where they apply.

**4. Defined terms.** *concern* · *concern route* · *detriment* · *evidence* · *material* · *independent*
· *decision owner*. Additionally, **subject of a concern** means the person or organisation whose act,
omission or decision the concern is about; **lookback window** means the period after a recorded concern
during which an act meeting the definition of detriment against the person who raised it requires the
review in `PR-04`.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-12.02-PR-01` — A published route with named recipients.** The credential holder must
  publish, to everyone working on or for the project, a route naming **at least two** recipients, the
  method of reaching each, what happens after a concern is raised, and the time within which a response is
  given. A route that names a role with no person, or that exists only in a policy nobody on the project
  has been given, does not satisfy this requirement.
- **`PCI-PML-STD-12.02-PR-02` — The bypass property.** For every person working on the project, at least
  one published recipient must be a person who is neither their line manager, nor in their reporting line,
  nor — where the concern is about the project leader — the project leader. The credential holder must be
  able to demonstrate this property person by person, and must add a recipient where it fails.
- **`PCI-PML-STD-12.02-PR-03` — A concern register with mandatory fields.** Every concern raised through
  the route must be recorded with: the date raised; the route used; whether the subject of the concern was
  a recipient of it (which must be recorded, and must be "no"); the substantive response given; the
  decision taken; the date closed; and the name of the person who responded. An entry missing any field is
  not a compliant record.
- **`PCI-PML-STD-12.02-PR-04` — No detriment, and any act resembling one is justified and reviewed.** The
  credential holder must not subject a person to detriment for raising a concern. Where an act meeting the
  definition of *detriment* falls on a person who raised a concern within the documented lookback window,
  the act must not take effect until: a written justification exists stating the grounds and the evidence;
  the decision is taken by a person who is **not** the subject of the concern; and a competent reviewer
  independent of both parties has reviewed the justification and recorded their conclusion.
- **`PCI-PML-STD-12.02-PR-05` — Anonymity is not defeated.** The credential holder must not attempt, and
  must not permit any person or system to attempt, to identify the author of an anonymous concern, and
  must not use monitoring, communications analysis or people analytics to identify or profile people who
  raise concerns.
- **`PCI-PML-STD-12.02-PR-06` — The countable indicator is reported.** Each reporting period the credential
  holder must report the proportion of issues in the period that were **discovered by assurance, testing or
  audit** rather than **reported by the team**, and must treat a sustained rise in that proportion as an
  issue under `PCI-PML-STD-08.02` with a named owner. This is a count drawn from records the project
  already produces, and it is the one indicator of reporting health that does not depend on anybody's
  self-report.

**6. Prohibited actions.** Requiring concerns to be raised only through the person they are about, or only
through a line manager. Responding to a concern by assessing the person who raised it. Subjecting a person
to detriment for raising a concern. Attempting to identify an anonymous reporter. Closing a concern with
no substantive response. Recording a concern only where it turned out to be correct. Treating dissent
recorded in a review as disloyalty. Using a survey result as a substitute for operating the route.

**7. Required evidence.** The published route, with evidence of its distribution to everyone working on
the project and the date; the bypass demonstration mapping each person to at least one qualifying
recipient; the concern register with all seven fields per entry; the justification, decision and
independent review record for any act meeting the definition of detriment inside the lookback window; the
period-by-period indicator under `PR-06` and its trend.

**8. Responsible role.** The named credential holder leading the project, for establishing, publishing and
operating the route and for the register. The named recipients, for responding. The employing
organisation's designated function, for statutory whistleblowing and grievance matters, which this standard
does not displace.

**9. Approval authority.** The sponsor approves the route and its recipients, and approves any change to
them. The governing body approves the route where the project leader is a possible subject. **No act
falling within `PR-04` may be approved by the subject of the concern**, and no approval by the credential
holder cures that.

**10. Independence requirement.** At least one published recipient must be independent of the project's
delivery line. The reviewer of a justification under `PR-04` must be independent of both the person who
raised the concern and its subject, and must not be the person who decided the act. Where an allegation of
detriment is made, the investigation must be conducted by a person independent of the project.

**11. Materiality or threshold.** This standard states no number of concerns, no target and no score, and any
of those would be actively harmful: a concern target invites manufactured concerns and a survey score
invites managed answers. **A survey result is not evidence of compliance with this standard, and a low score is
not evidence of breach** — the evidence is the route, the register and the detriment review. The
organisation's governance sets the response time, the lookback window and the reporting period; this standard
requires that they exist and are applied. Where the organisation sets no lookback window, the credential
holder must apply the period to the end of the project or twelve months, whichever is shorter, and record
that they did so in the absence of a documented window. **That period is also the floor.** A documented
lookback window shorter than it does not satisfy this standard, because a window an organisation can set to a
fortnight is a protection the organisation can switch off; where the documented window is shorter, the
credential holder applies the floor and records that they did so.
*Six-person internal project:* two named recipients — the project leader and one named manager outside the
team — a one-page route sent to all six on day one, and a register that may hold no entries for months. The
bypass demonstration is six lines. **A register with no entries is not a breach**; a register with no
entries alongside a rising `PR-06` indicator is the thing to investigate.
*Multi-partner national programme:* recipients named in each employing organisation plus one at programme
level who is independent of every partner, because a supplier's engineer with a concern about the client's
schedule has no usable route inside their own employer; the register is held at programme level with
partner-level extracts; and the `PR-06` indicator is reported per partner, because averaging conceals the
one partner whose team has stopped reporting.

**12. Exception and waiver.** **No exception is permitted** to `PR-02`, `PR-04` or `PR-05`. An exception to
the response time in `PR-01` may be granted by the sponsor where an investigation genuinely requires
longer, provided the person who raised the concern is told, in writing, that it is being handled and when
they will hear. Confidentiality of an investigation is not a ground for giving no response at all.

**13. Escalation trigger.** Any act meeting the definition of detriment against a person who raised a
concern. Any attempt to identify an anonymous reporter. A concern about the project leader, the sponsor or
a governing-body member — which is escalated outside the project on receipt. A concern that discloses a
safety, legal or regulatory matter. A sustained rise in the `PR-06` indicator. A concern unanswered beyond
the documented time.

**14. AI application.** AI may age the concern register against the response time and flag overdue entries,
check entries for missing mandatory fields, compute and trend the `PR-06` indicator from issue and
assurance data, and summarise the themes of closed concerns for governance in aggregate form.

**15. AI prohibition.** An AI system must not identify or attempt to identify the author of an anonymous
concern, score or profile individuals on attitude, engagement, sentiment or dissent, determine the outcome
of a concern, decide whether an act constitutes detriment, or triage concerns so that some receive no
human response. Sentiment or communications analysis must not be used on project communications for the
purpose of locating dissent.

**16. AI verification.** **Sampling with a stated basis plus independent recomputation.** Each reporting
period, a competent reviewer must draw a stated sample of concern entries and confirm against the
underlying correspondence that the recorded response was given, was substantive, and came from the named
responder; and must recompute the `PR-06` indicator by hand from the issue and assurance records for one
period and reconcile it to the reported figure. Where any people-analytics tool is in use anywhere on the
project, the credential holder must obtain and retain a written confirmation of its data sources and
verify that concern-route data is not among them.

**17. External reference.**
- **ISO** · *ISO 45003, Occupational health and safety management — Psychological health and safety at
  work — Guidelines* · relied on for: the existence of psychosocial risk as a recognised subject within
  occupational health and safety management · **EXT-124** · **Manual section 6 category 3 — international
  voluntary standard** · currency checked 2026-08-03 · limitation: **guidance, not a requirements
  standard — nothing can be certified against it**, and no obligation in this standard derives from it. It is
  named because a reader will expect it to be addressed, and the correct treatment is to say what it is.
- **ISO** · *ISO 45001, Occupational health and safety management systems — Requirements with guidance for
  use* · relied on for: the existence of a certifiable management-system standard addressing worker
  consultation, participation and protection from reprisal · **EXT-123** · **Manual section 6 category 3** ·
  currency checked 2026-08-03 · limitation: certifiable, but adoption is voluntary unless a contract or
  regulator requires it; this standard imports none of its requirements.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of team development
  and working environment within delivery practice · **EXT-028** · **Manual section 6 category 3** · currency
  checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.

**18. Jurisdictional caution.** Whistleblower protection, mandatory internal reporting channels, works-
council consultation, employee monitoring, data-protection and anti-retaliation law differ sharply by
jurisdiction and frequently impose obligations stricter and more specific than this standard — including on
who may receive a report, how it must be handled, what records may be kept and for how long. **Obtain
local legal advice before establishing a concern route, an anonymous channel, a register or any monitoring
arrangement**, and where the applicable regime imposes a higher obligation, that regime governs.

**19. Related PCI Standards.** `PCI-FND-STD-11` (the parent duty to escalate); `PCI-FND-STD-01`;
`PCI-FND-STD-09`; `PCI-PML-STD-08.02`; `PCI-PML-STD-09.02`; `PCI-PML-STD-12.01`;
`PCI-PML-STD-14.01`.
**What this standard adds to `PCI-FND-STD-11`:** the foundational standard creates a duty to escalate, which
binds the person who holds the information. This standard creates the **conditions under which they can
discharge it** —
a named recipient outside the line of the person concerned, protection from detriment tested by an
independent review, and a record that the concern was answered.

**20. Related Body of Knowledge content.** PML-AI · Domain 12 · KA 12.2 Motivation, team formation and
psychological safety · topic 12.2.3 psychological safety as a design variable, including its observable
presence and absence signals and its stated evidential limits; KA 12.3 difficult conversations. Also
Domain 3 KA 3.3 escalation design; Domain 8 KA 8.4 bias and crisis leadership; Domain 9 KA 9.4 lessons.

**21. Compliance test.** Compliance is demonstrated when a reviewer can perform all six of the following:
(a) obtain the published route and the evidence of its distribution, and confirm it names at least two
recipients with a stated method and response time; (b) take the project's current people list and confirm,
person by person, that at least one named recipient is neither their line manager nor in their reporting
line — and, for the project leader's own reports, that a recipient exists who is not the project leader;
(c) open the concern register and confirm every entry carries all seven mandatory fields, and that the
"subject was a recipient" field reads "no" in every case; (d) draw a stated sample of closed entries and
confirm from the underlying correspondence that a substantive response was given by the named responder
within the documented time; (e) cross-match the concern register against the project's assignment,
role-change and assessment records for the lookback window and confirm that **every** act meeting the
definition of *detriment* falling on a person who raised a concern carries a written justification, a
decision taken by someone other than the subject of the concern, and an independent reviewer's recorded
conclusion dated before the act took effect; and (f) recompute the `PR-06` indicator for one period from
the issue and assurance records and reconcile it to the reported figure. **Test (e) is the operative one.**
An act of detriment inside the lookback window with no independent review dated before it took effect
fails the test, and no survey score, culture statement or leadership testimonial cures it.

**22. Breach indicators.** A route naming one recipient, who is the project leader. A route that exists in
a corporate policy and has never been sent to the project. Register entries with the response field
containing "noted". Concerns closed the day they were raised. A person removed from a workstream within
weeks of raising a concern, with no justification record. Anonymous concerns followed by an inquiry into
who wrote them. A `PR-06` indicator rising for three periods with no owner. A team whose register is empty
while assurance is finding everything.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure on the associated competency; ethics review; certification investigation;
suspension or withdrawal of the PML-AI credential — each subject to due process and a right of appeal.
**PCI cannot compensate a person subjected to detriment, order reinstatement or impose any penalty on an
employer**; those remedies, where they exist, arise under the applicable employment or whistleblowing law.

**24. Examination application.** Scenario judgement: a supplier's engineer raises a safety concern about
the client's sequence and the only published recipient is their own project manager, who set the sequence.
Evidence selection: which artefacts establish compliance, and why a staff-survey score is not among them.
Ethical dilemma: a leader is asked to remove from a workstream a person who raised a concern three weeks
earlier, for reasons the leader believes are genuine and unrelated. Escalation decision: an anonymous
concern names the sponsor.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-12-02` v1.0 (*Psychological Safety*). Amendment note: renumbered, **retitled** and rewritten from
the ground up. The v1.0 standard required a leader to "maintain conditions in which any team member can report
bad news" — a state of affairs no reviewer could test, which failed audit questions 5 and 6. The principal
obligation is now the **establishment and operation of a bypass route**, the protections are expressed as
observable acts through a defined term for *detriment*, and the compliance test cross-matches two record
sets that already exist. The concept of psychological safety remains the reason for the standard and is taught
in Domain 12; it is not the requirement, because it is not observable. **Stage 9 amendment:** the
lookback window was set wholly by the organisation, so the protection in `PR-04` could be switched off
by documenting a fortnight; element 11 now makes the standard's own default the floor.

---

## Domain 13 — Agile, Adaptive and Hybrid Delivery

### PCI STANDARD PCI-PML-STD-13.01 — Governance of Adaptive Delivery

**1. Normative requirement.** A credential holder using an adaptive or hybrid delivery approach must
operate governance that produces the same decision rights, evidence and accountability as a predictive
approach, expressed through the adaptive method's own artefacts.

**2. Purpose.** Adaptive delivery does not remove governance; it relocates it, from documents produced for
gates to decisions taken continuously. The failure this prevents runs in both directions: the team that
treats an iterative method as an exemption from evidence and authority, and the organisation that bolts a
predictive gate structure onto an adaptive team and destroys the feedback the method exists to produce.

**3. Scope.** Every credential holder leading, governing, assuring or reporting on delivery using an
adaptive or hybrid approach, at team, programme and portfolio level, including scaled arrangements and
including contracts written for adaptive delivery.

**4. Defined terms.** *governance* (project sense) · *decision owner* · *delegation schedule* · *evidence*
· *gate* · *acceptance* · *product owner*. Additionally, **value envelope** means the bounded authority
within which a product owner may commit capacity without escalation; **release decision** means the
decision to make an increment available to users.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-13.01-PR-01` — Decision rights mapped to adaptive artefacts.** The credential holder must
  record which adaptive artefact or event carries each decision class in the delegation schedule, so that
  every class has a location. A class with no location is governed by the predictive route until one is
  recorded.
- **`PCI-PML-STD-13.01-PR-02` — The value envelope is stated.** The product owner's value envelope must be
  documented and approved by the sponsor, and decisions exceeding it must escalate within the latency the
  delegation schedule states.
- **`PCI-PML-STD-13.01-PR-03` — Release decisions carry acceptance and evidence.** A release decision must
  satisfy `PCI-PML-STD-09.01`, and the increment's acceptance evidence must be retained. Iterative
  delivery does not reduce the evidence requirement; it changes its cadence.
- **`PCI-PML-STD-13.01-PR-04` — Adaptive metrics are defined and not gamed.** Where flow, throughput,
  cycle-time or velocity measures are reported to governance, their definition must be documented, applied
  consistently, and reported with the work-in-progress position, so that a rising throughput achieved by
  starting more work is visible as such.
- **`PCI-PML-STD-13.01-PR-05` — Framework adoption is a recorded choice.** Where a published adaptive
  framework is adopted, the credential holder must record which framework, which parts of it are adopted,
  and which are not — because adoption is the whole of such a framework's force and an unrecorded partial
  adoption is unauditable.

**6. Prohibited actions.** Treating an adaptive method as removing the need for acceptance evidence,
decision records or escalation. Reporting velocity as progress against a commitment. Increasing throughput
by increasing work in progress and reporting the first without the second. Describing a team as following a
named framework while omitting the parts that constrain it. Placing the ordering right with a committee
while naming an individual as product owner.

**7. Required evidence.** The decision-rights map from classes to adaptive artefacts; the documented value
envelope with its approval; release decisions with their acceptance records and retained evidence; the
metric definitions and the reported series including work in progress; the framework-adoption record.

**8. Responsible role.** The named credential holder leading delivery, for the governance mapping and the
reporting. The named **product owner**, for ordering decisions within the envelope. The named release
authority, for release decisions.

**9. Approval authority.** The sponsor approves the value envelope. The delegation schedule's named
authority approves decisions above it. The release authority approves releases. A committee must not be
recorded as the product owner; where a committee holds the ordering right in fact, the record must say so
and the product owner is named as a proxy.

**10. Independence requirement.** Acceptance of an increment must be recorded by a person independent of
its production under `PCI-PML-STD-09.01`. Assurance of an adaptive delivery must be provided by a competent
reviewer independent of the team, and must satisfy `PCI-PML-STD-01.03-PR-06` where that reviewer has
contributed to the team's ways of working.

**11. Materiality or threshold.** This standard states no envelope value, no iteration length and no
work-in-progress limit. The organisation's governance sets the value envelope, the escalation latency and
the release authority; this standard requires that they are documented and applied. Where a team operates a
work-in-progress limit, its basis must be recorded so that a change to it is visible as a decision.
*Six-person internal project:* one team, one product owner with an envelope stated in a sentence, releases
accepted by a named user representative, and a metric definition of two lines. The decision-rights map is
a five-row table.
*Multi-partner national programme:* several teams across organisations with a stated rule for which
decisions rise from team to programme, a programme-level release authority for anything crossing an
interface, and metric definitions held at programme level so that partners' velocity figures are not added
together as though they were comparable — which they are not.

**12. Exception and waiver.** An exception permitting a release without complete acceptance evidence may be
approved only by the release authority, only where the missing evidence is identified item by item with
owners and dates, and only where the release is reversible or the residual risk is accepted by the party
bearing it. No exception permits a decision class to have no recorded location. **This exception does not
reach a mandatory precondition.** Where the release is a release into operational use, `PCI-PML-STD-16.01`
applies to it, and its element 12 permits no exception, waiver, dispensation or deferral in respect of a
gate-block item — an iterative delivery model changes the cadence of the release, not the status of a
safety case, a licence or a statutory notification.

**13. Escalation trigger.** A decision exceeding the value envelope. A decision class with no recorded
location. A release proposed without acceptance evidence. A product owner who cannot exercise the ordering
right. A throughput improvement accompanied by a rising work-in-progress position. A framework described
as adopted whose constraining practices are not in use.

**14. AI application.** AI may reconcile the decision-rights map against the delegation schedule and flag
unlocated classes, compute flow metrics with their work-in-progress position, detect backlog items that
have never been reordered, and check release records for missing acceptance references.

**15. AI prohibition.** An AI system must not order a backlog as the operative decision, approve a release,
set a value envelope, or act as product owner. A model that produces an order has produced a
recommendation; the ordering right belongs to a named person.

**16. AI verification.** **Independent recomputation plus named approval.** Flow and throughput metrics
produced with AI assistance must be recomputed by hand for at least one period per quarter and reconciled.
Any AI-produced backlog order must be reviewed and adopted, altered or rejected by the named product owner,
who records which they did.

**17. External reference.**
- **Ken Schwaber and Jeff Sutherland** · *The Scrum Guide* · relied on for: the existence of a defined
  set of accountabilities, events and artefacts in one widely used adaptive framework · **EXT-086** ·
  **Manual section 6 category 5 — professional framework** · currency checked 2026-08-03 · limitation: **a
  voluntary framework whose adoption is the whole of its force.** It binds a team that has adopted it and
  nobody else; it is **not a standard**, nothing can be certified against it as a requirements document,
  and no PCI obligation derives from it. Its concepts are described here in PCI's own words; no text is
  reproduced. The suite register classifies it as a voluntary framework; Manual section 6 governs this file and
  places it at category 5.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of tailoring and of
  iterative life cycles within delivery practice · **EXT-028** · **Manual section 6 category 3 — international
  voluntary standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **Project Management Institute** · *PMBOK Guide* · relied on for: the existence of adaptive and hybrid
  approaches in professional practice · **EXT-060** · **Manual section 6 category 5 — professional framework** ·
  currency checked 2026-08-03 · limitation: **never regulatory authority**; no edition asserted; no text
  reproduced.

**18. Jurisdictional caution.** Contracts written for adaptive delivery raise questions of what has been
agreed, when payment is due and what has been accepted that are determined by the governing law of the
contract. Obtain legal advice before adopting an outcome-based or capacity-based contract structure.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-12`; `PCI-FND-STD-04`; `PCI-PML-STD-03.01`;
`PCI-PML-STD-03.02`; `PCI-PML-STD-05.01`; `PCI-PML-STD-09.01`; `PCI-PML-STD-13.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 13 · KA 13.1 Agile principles and product
ownership; KA 13.2 Backlogs, iteration planning, flow and Kanban; KA 13.3 Scaling considerations and hybrid
governance; KA 13.4 Contracting for adaptive delivery, metrics and anti-patterns. Also Domain 3 KA 3.1
topic 3.1.3 governance in agile and hybrid environments.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) take every decision class in
the delegation schedule and find, in the decision-rights map, the adaptive artefact or event that carries
it; (b) retrieve the approved value envelope and confirm that every commitment above it in the period has
an escalation record; (c) take a stated sample of releases and retrieve, for each, an acceptance record and
the evidence it cites; (d) recompute one period's reported flow metric from the raw item data using the
documented definition and reconcile it to the reported figure, and confirm the work-in-progress position
was reported alongside it; and (e) compare the framework-adoption record against observed practice for the
constraining elements it claims to adopt. An unlocated decision class, or a release with no acceptance
record, fails the test.

**22. Breach indicators.** Velocity reported as a commitment. Throughput rising while work in progress
rises faster. Backlogs from which nothing is ever removed. A product owner whose decisions are routinely
overturned. Releases with no acceptance record. A named framework in the delivery approach and none of its
constraining events in the calendar. Assurance provided by the coach who designed the ways of working.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; certification investigation; suspension or withdrawal — each subject to
due process and a right of appeal.

**24. Examination application.** Calculation review: throughput and work-in-progress series showing an
apparent improvement that is a queue. Scenario judgement: a steering committee requires a stage gate on an
adaptive team six weeks into a quarterly cycle. Evidence selection: which artefacts show that an adaptive
delivery is governed.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-13-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the
decision-rights mapping, value envelope, release evidence, metric integrity and framework-adoption record
separated into five process requirements; the Scrum Guide's characterisation corrected to state expressly
that adoption is the whole of its force. **Stage 9 amendment:** element 12's release exception overlapped
`PCI-PML-STD-16.01`, whose element 12 admits no exception at all in respect of a mandatory precondition;
element 12 now states that the exception does not reach one.

---

### PCI STANDARD PCI-PML-STD-13.02 — Product and Project Accountability

**1. Normative requirement.** A credential holder must ensure that, for every product or service being
delivered, exactly one named individual holds the ordering right and exactly one named individual holds
delivery accountability, and that the record names both.

**2. Purpose.** Where product and project accountability blur, two failures follow and they are opposite.
Either nobody can order the work, so the backlog is arbitrated by whoever escalates hardest; or the product
role is nominal, held by someone who must obtain approval for every decision, in which case the authority
sits with the approver and the record is false. The professional content of the role is the **decision
right**, and this standard protects it.

**3. Scope.** Every credential holder in a delivery leadership, product ownership or governance role, in
adaptive, hybrid and product-organised delivery, including arrangements where a product outlives the
project that created it and arrangements where a supplier supplies the product role.

**4. Defined terms.** *product owner* · *decision owner* · *accountability* · *value envelope* ·
*evidence* · *material*. Additionally, **ordering right** means the authority to determine the sequence in
which work is done and what is not done; **proxy** means a person recorded as holding a right that is in
fact exercised elsewhere.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-13.02-PR-01` — Both roles named, and distinguished.** The record must name the individual
  holding the ordering right and the individual holding delivery accountability, and must state what each
  decides. Naming one person for both is permitted only where the record says so expressly and the sponsor
  has approved it.
- **`PCI-PML-STD-13.02-PR-02` — Proxy arrangements are disclosed.** Where the ordering right is in fact
  exercised by a committee or by a person other than the named product owner, the credential holder must
  record that the named holder is a proxy and identify where the authority actually sits.
- **`PCI-PML-STD-13.02-PR-03` — Refusals are recorded.** The product owner's decisions not to do work must
  be recorded, because a backlog from which nothing is ever removed is evidence that the ordering right is
  not being exercised.
- **`PCI-PML-STD-13.02-PR-04` — Continuity across the project boundary.** Where a product continues beyond
  the project, the credential holder must record the transfer of the ordering right and of delivery
  accountability to named individuals in the receiving organisation before closure, under
  `PCI-PML-STD-16.02`.

**6. Prohibited actions.** Recording a committee as product owner. Naming a product owner who must obtain
approval for every ordering decision without recording them as a proxy. Allowing a supplier to hold the
ordering right for a client's product without an approved instrument. Closing a project leaving a live
product with no named ordering right. Presenting a backlog with no refusals as evidence of prioritisation.

**7. Required evidence.** The record naming both individuals and their decision scopes; the sponsor's
approval where one person holds both; the proxy disclosure where applicable; the backlog history showing
items removed or refused with dates; the transfer record at closure.

**8. Responsible role.** The named credential holder leading delivery, for maintaining the record and for
escalating where a role is vacant or nominal. The **sponsor**, for appointing the ordering right and
setting the value envelope.

**9. Approval authority.** The sponsor appoints the product owner and approves the combination of both
roles in one person. The governing body approves a supplier holding the ordering right for a client
product. The credential holder approves neither.

**10. Independence requirement.** Not applicable to the ordering right itself, which is a delivery decision
right and is not intended to be independent; independence attaches to the acceptance of increments under
`PCI-PML-STD-09.01` and to the assurance of the arrangement under `PCI-PML-STD-01.03-PR-06`.

**11. Materiality or threshold.** This standard states no number. The value envelope and the escalation latency
are set by the organisation's governance under `PCI-PML-STD-13.01-PR-02`; this standard requires that the roles
are named and that the record is true. The test of nominality is not a percentage but the observable one:
whether decisions within the stated envelope are taken and stand.
*Six-person internal project:* one person may hold both roles, and that is entirely proper — the obligation
is that the record says so and the sponsor approved it, so that nobody later assumes a separation that
never existed.
*Multi-partner national programme:* the ordering right sits with the client organisation for each product,
partners supply delivery accountability within their own scope, and the boundary is recorded per product
rather than per contract, because a product delivered by three partners has one order and three delivery
accountabilities.

**12. Exception and waiver.** An exception permitting the ordering right to be held collectively may be
approved by the governing body for a stated period, only where the collective body is named, its chair is
recorded as the decision owner under the definition above, and its decision latency is stated. No exception
permits a product to run with no recorded ordering right.

**13. Escalation trigger.** A product with no named ordering right. A named product owner whose decisions
are routinely overturned. A supplier exercising the ordering right without an approved instrument. Project
closure approaching with a live product and no receiving owner. A backlog with no refusals across a period
the governance considers material.

**14. AI application.** AI may detect backlog items that have never been reordered or refused, measure the
decision wait between an item being raised and being ordered, flag products with no recorded owner, and
compare recorded ordering decisions against the value envelope.

**15. AI prohibition.** An AI system must not hold the ordering right, order a backlog as the operative
decision, or be recorded as a product owner or as delivery-accountable.

**16. AI verification.** **Named approval plus sampling with a stated basis.** Every AI-proposed ordering
must be adopted, altered or rejected by the named product owner with a record of which. Each quarter, a
competent reviewer must sample ordering decisions and confirm from the record that the named holder took
them and that they stood.

**17. External reference.**
- **Ken Schwaber and Jeff Sutherland** · *The Scrum Guide* · relied on for: the existence of a single
  accountable ordering role in one widely used adaptive framework · **EXT-086** · **Manual section 6 category 5 —
  professional framework** · currency checked 2026-08-03 · limitation: **a voluntary framework; adoption
  is the whole of its force**; it is not a standard and nothing can be certified against it as a
  requirements document; described in PCI's own words, no text reproduced. This standard's obligation is PCI's
  own and does not derive from it.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of defined roles and
  responsibilities within delivery · **EXT-028** · **Manual section 6 category 3 — international voluntary
  standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **ISO** · *ISO 21505, Guidance on governance* · relied on for: the existence of accountability
  definition within governance arrangements · **EXT-032**, **not independently verified** · **Manual section 6
  category 3** · limitation: as above, with an open verification status.

**18. Jurisdictional caution.** Where a supplier holds decision rights over a client's product, the
allocation of liability, intellectual property and data controllership is a contractual and regulatory
question. Obtain legal advice before placing an ordering right outside the owning organisation.

**19. Related PCI Standards.** `PCI-FND-STD-01`; `PCI-FND-STD-04`; `PCI-FND-STD-12`; `PCI-PML-STD-01.01`;
`PCI-PML-STD-01.02`; `PCI-PML-STD-13.01`; `PCI-PML-STD-16.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 13 · KA 13.1 topic 13.1.2 product ownership as a
decision right; KA 13.4 metrics and anti-patterns, including the proxy product owner. Also Domain 1 KA 1.2
accountability and responsibility; Domain 16 KA 16.2 operational transition.

**21. Compliance test.** Compliance is demonstrated when a reviewer can, for every product in delivery:
(a) name the holder of the ordering right and the holder of delivery accountability from the record;
(b) take a stated sample of ordering decisions in the period and confirm from the record that the named
holder took them; (c) confirm that decisions within the stated value envelope were not subsequently
overturned by another body without an escalation record; (d) find refusals or removals in the backlog
history; and (e) where one person holds both roles, find the sponsor's recorded approval. A product whose
ordering decisions are routinely retaken elsewhere, with the named holder unchanged and no proxy
disclosure, fails the test.

**22. Breach indicators.** A product owner named in an organisation chart and absent from every ordering
decision. A backlog that only grows. Median ordering decision waits of the same order as the escalation
cycle. Products with two people each believing they hold the order. A project closing with a live product
and an unnamed owner. Sponsors ordering the backlog directly while a product owner is named.

**23. Consequence within PCI authority.** Correction required; additional review; escalation; examination
failure; certification investigation; suspension or withdrawal — each subject to due process and a right of
appeal.

**24. Examination application.** Scenario judgement: a named product owner must obtain steering-committee
approval for every ordering decision, and the candidate states what the record must show. Evidence
selection: which artefact establishes that the ordering right is exercised. Calculation review: a decision
wait series showing a proxy arrangement.

**25. Version and status.** Version 1.0 · **not yet approved** · effective on approval · **new standard** — the
v1.0 set governed adaptive delivery in one standard and left the product/project accountability boundary
unstated, which is the boundary most often blurred in practice. Amendment note: none.

---
## Domain 14 — Digital Delivery, Data and Responsible AI

### PCI STANDARD PCI-PML-STD-14.01 — Responsible Data Use in Delivery

**1. Normative requirement.** A credential holder must not collect, use, share, retain or expose data on a
project beyond the purpose, the recipients and the retention period recorded for it.

**2. Purpose.** Delivery accumulates data faster than it governs it: common data environments, dashboards,
supplier portals, personal data about workers and users, safety records, commercially confidential
tenders. The failure this prevents is the quiet expansion of purpose — data collected for one reason,
copied into a second system for a second reason, and left there — which is simultaneously a professional
failure, a security exposure and, in many jurisdictions, unlawful.

**3. Scope.** Every credential holder specifying, commissioning, operating, sharing or closing a project
data environment, dashboard, model, register or archive, including supplier-hosted environments and
including data about people working on the project.

**4. Defined terms.** *evidence* · *material* · *decision owner* · *independent* · *detriment* ·
*data governance* (written in full, always, and distinct from project *governance*). Additionally,
**purpose record** means the recorded statement of why a data set is held, who may see it and for how long;
**common data environment** means the shared repository through which project information is exchanged.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-14.01-PR-01` — A purpose record per data set.** Every project data set must carry a
  recorded purpose, a named owner, the classes of recipient permitted, and a retention period. A data set
  with no purpose record must not be created or, where inherited, must be recorded and escalated.
- **`PCI-PML-STD-14.01-PR-02` — Access matches the record.** Access rights in each system must be
  reconciled against the purpose record at the interval the governance sets, and every access not
  supported by the record must be removed and the removal recorded.
- **`PCI-PML-STD-14.01-PR-03` — Personal data is identified and minimised.** Data about identifiable people
  must be identified as such in the record, must be limited to what the recorded purpose requires, and must
  not be used to assess, rank or profile individuals except where the purpose record states that use and
  the applicable legal basis has been confirmed by the organisation's competent function.
- **`PCI-PML-STD-14.01-PR-04` — Lineage for figures that carry decisions.** Any figure reported to a
  decision-maker must be traceable to its source system, its extract date and its transformation, and the
  credential holder must not report a figure whose lineage cannot be stated.
- **`PCI-PML-STD-14.01-PR-05` — Closure disposes or transfers deliberately.** At project closure, every
  data set must be transferred to a named receiving owner, archived under the recorded retention period,
  or securely disposed of, with the decision recorded per data set.

**6. Prohibited actions.** Copying a data set into a second environment without extending the purpose
record. Granting standing access "for convenience". Using worker or user data to profile individuals
outside the recorded purpose. Reporting a figure whose source cannot be stated. Leaving a project
environment live and unowned after closure. Sharing commercially confidential tender data with a party
outside the evaluation. Exporting personal data to a supplier tool without the organisation's competent
function confirming the basis.

**7. Required evidence.** The data register with purpose, owner, recipient classes and retention per set;
the periodic access reconciliation with removals; the identification of personal data and the confirmed
legal basis where used to assess individuals; lineage records for reported figures; the closure disposition
record per data set.

**8. Responsible role.** The named credential holder leading the project, for the register, the
reconciliation and the closure disposition. The named data-set owner, for each set. The organisation's
competent data-protection or information-governance function, for the legal basis — **not** the project.

**9. Approval authority.** The organisation's information-governance authority approves a new purpose, a
new recipient class and any extension of retention. The sponsor approves the closure disposition plan. The
project must not approve its own extension of purpose.

**10. Independence requirement.** The access reconciliation must be performed or verified by a person
independent of the team whose access is being reconciled. The confirmation of a legal basis must come from
a function independent of the project.

**11. Materiality or threshold.** This standard states no data volume and no retention period, because retention
is set by law, by contract and by the organisation's policy, and a number invented here would conflict with
all three. It requires that the purpose record exists per data set, that the reconciliation interval is
documented and met, and that the retention period recorded is the one the organisation's policy or the
applicable law sets.
*Six-person internal project:* six data sets, one register page, access reconciled quarterly by a named
manager outside the team, and a closure decision of one line per set.
*Multi-partner national programme:* a common data environment per partner plus a shared one, a register
that records which partner is the owner and which are recipients for each set, and a closure disposition
agreed contractually in advance — because the moment to agree what happens to a partner's data at exit is
before the partner is exiting.

**12. Exception and waiver.** An exception permitting use beyond the recorded purpose may be approved only
by the organisation's information-governance authority, in writing, before the use, for a stated period and
recipient set, with the compensating control stated. **No exception may be approved by the project alone**,
and no exception permits use of personal data for an assessment purpose without a confirmed legal basis.

**13. Escalation trigger.** A data set found with no purpose record. Access that the record does not
support. A proposal to use worker or user data to assess individuals. A figure reported whose lineage
cannot be stated. A supplier tool found to be processing project personal data outside the record. A
suspected or actual data breach — which is escalated immediately and outside the ordinary cycle.

**14. AI application.** AI may build the data register from system metadata, reconcile access lists against
the register and flag divergence, detect probable personal data in unclassified stores, trace lineage
across systems and flag figures with broken lineage, and produce the closure disposition list.

**15. AI prohibition.** An AI system must not determine a legal basis, approve a purpose extension, decide
that data may be shared, profile individuals, or be the sole determinant that a data set contains no
personal data.

**16. AI verification.** **Source tracing plus sampling with a stated basis.** Every AI-detected
classification must be confirmed by a named human before it is acted on, and every AI-produced lineage must
be traced by a competent reviewer to the source system and extract for a stated sample of reported figures.
Where AI screened stores for personal data, a named human must review in full any store the system reports
as clean but which the purpose record indicates is likely to contain personal data.

**17. External reference.**
- **ISO/IEC** · *ISO/IEC 27001, Information security management systems — Requirements* · relied on for:
  the existence of a certifiable requirement set for information-security management · **EXT-023** ·
  **Manual section 6 category 3 — international voluntary standard** · currency checked 2026-08-03 · limitation:
  certifiable, but adoption is voluntary unless required by contract or regulator; this standard imports none of
  its controls.
- **ISO/IEC** · *ISO/IEC 27701, Privacy information management systems — Requirements and guidance* ·
  relied on for: the existence of a privacy management-system standard · **EXT-038** · **Manual section 6
  category 3** · currency checked 2026-08-03 · limitation: **materially changed between editions** — the
  earlier edition was an extension to an information-security management system and the current one is a
  standalone standard certifiable in its own right. **Verify which edition any claim of conformity refers
  to.** Voluntary unless required.
- **ISO** · *ISO 8000 data-quality standards* (a multi-part series) · relied on for: the existence of a
  standardised treatment of data quality · **EXT-026** · **Manual section 6 category 3** · currency checked
  2026-08-03 · limitation: **a series, not one document**; voluntary; cited generically and relied on for
  no requirement.
- **ISO/IEC** · *ISO/IEC 25012, Data quality model* · relied on for: the existence of a defined set of
  data-quality characteristics · **EXT-036** · **Manual section 6 category 3** · currency checked 2026-08-03 ·
  limitation: voluntary; a model, not a requirements standard for a project; check for supersession within
  its series.
- **ISO** · *ISO 19650 series, Information management using building information modelling* · relied on
  for: the existence of a common data environment concept in the built environment · **EXT-039** ·
  **Manual section 6 category 3** · currency checked 2026-08-03 · limitation: **a series**; voluntary; sector-
  specific; cited generically.
- **European Union** · *General Data Protection Regulation* · relied on for: illustrating a rights-based
  data-protection approach · **EXT-101**, **not independently verified — verify current requirements** ·
  **Manual section 6 category 10 — illustrative practice** · limitation: **this is binding legislation within the
  European Union**, named here only to illustrate the shape of such a regime; it is relied on for no
  requirement in this standard, and whether it or any other data-protection regime applies is a question for
  qualified local counsel.

**18. Jurisdictional caution.** Data-protection law, employee-monitoring rules, works-council consultation,
data-localisation requirements, sector confidentiality duties, breach-notification timescales and
cross-border transfer restrictions are jurisdiction-specific and change frequently. **Compliance with this
standard is not compliance with any of them.** Obtain local legal advice before collecting, sharing, exporting
or retaining personal or confidential project data.

**19. Related PCI Standards.** `PCI-FND-STD-07`; `PCI-FND-STD-09`; `PCI-FND-STD-12`; `PCI-PML-STD-11.01`;
`PCI-PML-STD-12.02`; `PCI-PML-STD-14.02`; `PCI-PML-STD-16.03`.

**20. Related Body of Knowledge content.** PML-AI · Domain 14 · KA 14.1 Digital project environments, data
governance and the common data environment; KA 14.4 Explainability, bias, human accountability,
cybersecurity and privacy. Also Domain 9 KA 9.4 data quality; Domain 16 KA 16.4 responsible archive and
model/data retention.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) list the project's data sets
from the systems in use and match each to a purpose record with an owner, recipient classes and a retention
period, with no residue; (b) extract the current access list from each system and reconcile it to the
recipient classes in the record, with every difference explained by a dated removal or a recorded approval;
(c) select a stated sample of figures from reports issued in the period and trace each to its source system,
extract date and transformation; (d) confirm that every use of data about identifiable people to assess or
rank them carries a legal basis confirmed by the organisation's competent function; and (e) for a closed
project, find a disposition decision for every data set. Any data set with no purpose record fails the test.

**22. Breach indicators.** Environments with more users than the project has people. Copies of a register in
personal drives. Dashboards whose figures cannot be traced. Retention periods recorded as "indefinite".
Personal data in a supplier's tool with no record. Access lists unchanged since project start. Closed
projects with live environments.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal. **PCI cannot impose a data-protection penalty**; that
consequence, where it exists, arises under the applicable law.

**24. Examination application.** Scenario judgement: a supplier proposes to copy the project's issue data
into its own analytics platform to improve its service. Evidence selection: which artefacts establish that
a reported figure is traceable. Ethical dilemma: a sponsor asks for individual productivity rankings from
the delivery tool's activity data.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-14-01` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the purpose
record, access reconciliation, personal-data minimisation, lineage and closure disposition separated into
five process requirements; the GDPR's characterisation corrected to state expressly that it is binding
legislation within its own jurisdiction and is relied on here for nothing.

---

### PCI STANDARD PCI-PML-STD-14.02 — Responsible AI in Delivery

**1. Normative requirement.** A credential holder must not rely on an AI system's output for a delivery
decision, a report or a deliverable until a named human has verified it by a method recorded for that class
of output.

**2. Purpose.** `PCI-FND-STD-03` requires verification before professional use. What delivery adds is that
AI output arrives inside artefacts other people then rely on without knowing its origin: a risk register
populated by a model, a schedule narrative generated from data, a supplier evaluation summary, a lessons
theme. The failure this prevents is **unattributed reliance** — an output nobody checked because everybody
assumed somebody had, embedded in an artefact that carries the organisation's name.

**3. Scope.** Every credential holder using, commissioning, configuring, governing or assuring an AI system
in delivery — including supplier-operated and embedded features in delivery tools — for any output that
informs a decision, a report or a deliverable.

**4. Defined terms.** *AI verification* (the sense carried by the suite principle) · *evidence* ·
*material* · *competent reviewer* · *decision owner* · *independent*. Additionally, **AI use record** means
the record of where AI is used on the project, for what, by whom, and with which verification method;
**verification method** means one of the named methods in element 16, not a general instruction to review.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-14.02-PR-01` — An AI use record exists.** The credential holder must maintain a dated
  record of every AI use on the project: the system, the output class, the person accountable for it, and
  the verification method applied. Embedded features in delivery tools must be included; an unlisted
  embedded feature is the most common gap.
- **`PCI-PML-STD-14.02-PR-02` — A named method per output class, recorded before use.** Each output class
  must have a verification method recorded before the output is relied on, chosen from: independent
  recomputation, source tracing, clause-to-summary comparison, sampling with a stated basis,
  reconciliation, boundary testing, sensitivity analysis, expert judgement, or named approval. **"Review
  the AI output" is not a method** and does not satisfy this requirement.
- **`PCI-PML-STD-14.02-PR-03` — Material AI assistance is disclosed.** Where AI assistance is material to
  a deliverable or a report, the credential holder must disclose it in the artefact, under
  `PCI-FND-STD-14`, in the form the organisation's governance sets.
- **`PCI-PML-STD-14.02-PR-04` — Verification proportionate to reliance.** The depth of verification must
  be matched to the consequence of the output being wrong, and the credential holder must record the basis
  on which the depth was chosen for each output class.
- **`PCI-PML-STD-14.02-PR-05` — Failures are recorded and fed back.** Where verification finds a material
  error in an AI output, the credential holder must record it, correct anything already issued in reliance
  on it under `PCI-PML-STD-11.01-PR-04`, and route the pattern to `PCI-PML-STD-09.02`.

**6. Prohibited actions.** Relying on an output because it is plausible and well-formed. Recording "AI
reviewed" as a verification. Presenting AI-generated analysis as a named person's professional judgement.
Using an AI system for an output class with no recorded method. Treating a supplier's assurance about its
own model as verification of the output. Suppressing a verification failure because the output was already
issued.

**7. Required evidence.** The AI use record with output classes, accountable people and methods; the
verification records per output class showing the method applied and by whom; disclosure statements in
artefacts; the recorded basis for verification depth; the verification-failure log with corrections issued
and the routing to lessons.

**8. Responsible role.** The named credential holder leading the project, for the AI use record and for the
sufficiency of methods. The named person accountable for each output class, for performing the
verification. Neither may be a supplier by default; where a supplier verifies, the credential holder
records who in the supplier is accountable and retains the verification evidence.

**9. Approval authority.** The sponsor approves the AI use record and the verification methods. The
governing body approves any use of AI in a reserved decision class under `PCI-PML-STD-01.02`. A tool vendor
approves nothing.

**10. Independence requirement.** The person verifying an AI output must be independent of the configuration
of the system that produced it, in the sense that they did not set the parameters, prompts, weights or
thresholds for that output. Where that is impossible on a small project, the arrangement must be recorded
and a second named person must perform the verification for the highest-consequence output class.
**Independence of configuration is the minimum this standard adds; it does not displace `PCI-FND-STD-03`**,
which requires a *material* calculation, model output or automated conclusion to be verified by a person
independent of its preparation before any person relies on it. Where the verifier under this standard is also
the preparer of the output, this standard is satisfied and the foundational verification is not.

**11. Materiality or threshold.** This standard states no accuracy figure and no confidence threshold. **A
model's own confidence score is not a verification and must not be used as one.** The organisation's
governance sets the output classes, the materiality at which AI assistance is disclosed, and the
verification depth by consequence; this standard requires that these exist and are applied.
*Six-person internal project:* three AI uses — drafting, summarising and a scheduling assistant — one line
each in the record, with source tracing for figures, clause-to-summary comparison for summaries and named
approval for anything issued.
*Multi-partner national programme:* the record is consolidated across partners because a partner's embedded
tool produces outputs that enter the programme's reports; verification methods are stated in the partner
agreement; and the highest-consequence classes carry independent recomputation performed outside the partner
that produced the output.

**12. Exception and waiver.** An exception permitting reliance on an unverified output may be approved only
by the sponsor, only where the decision cannot wait, only where the artefact states on its face that the
output is unverified, and only with a named owner and a date for the verification to follow. **No exception
is permitted** where the output bears on safety, a licence condition, a statutory duty or a person's rights.

**13. Escalation trigger.** An AI output relied on with no recorded method. A material verification failure.
An embedded feature discovered producing outputs into project artefacts. A supplier declining to disclose
whether AI produced a deliverable. Any proposal to use AI in a reserved decision class. An output that
cannot be verified by any available method — which is a reason not to use it.

**14. AI application.** AI may assist across delivery — drafting, summarising, reconciling, classifying,
detecting anomalies, modelling scenarios, checking documents for completeness — and every such use is
permitted, recorded and verified rather than prohibited. AI may also maintain the AI use record itself and
flag outputs relied on with no verification record.

**15. AI prohibition.** An AI system must not decide, approve, certify, sign, waive, authorise, accept,
assure or be recorded as verifying anything. It must not verify another AI system's output for the purposes
of this standard. It must not be represented as having independently verified anything, and no supplier statement
converts its output into verified information.

**16. AI verification.** **The method is named per output class, from this list, and recorded before use:**
independent recomputation; source tracing; clause-to-summary comparison; sampling with a stated basis;
reconciliation; boundary testing; sensitivity analysis; expert judgement; named approval. For each class,
the record states the method, who performs it, the sample size where sampling is used, and the tolerance at
which a difference becomes a failure. Verification is performed **before** reliance, not after issue.

**17. External reference.**
- **ISO/IEC** · *ISO/IEC 42001, Artificial intelligence management system* · relied on for: the existence
  of a management-system standard for AI governance, including assignment of roles · **EXT-021** ·
  **Manual section 6 category 3 — international voluntary standard** · currency checked 2026-08-03 · limitation:
  certifiable as a management system, but adoption is voluntary; certification says nothing about any
  individual output; no requirement is imported here.
- **ISO/IEC** · *ISO/IEC 23894, Guidance on risk management for artificial intelligence* · relied on for:
  the existence of AI-specific risk guidance · **EXT-024** · **Manual section 6 category 3** · currency checked
  2026-08-03 · limitation: **guidance, not requirements**; voluntary; not certifiable.
- **ISO/IEC** · *ISO/IEC 38507, Governance implications of the use of artificial intelligence by
  organizations* · relied on for: the existence of governing-body-level questions about AI use ·
  **EXT-037** · **Manual section 6 category 3** · currency checked 2026-08-03 · limitation: guidance for governing
  bodies; voluntary; not certifiable.
- **NIST (US Department of Commerce)** · *Artificial Intelligence Risk Management Framework (AI RMF 1.0)* ·
  relied on for: the existence of a voluntary function-based framework for AI risk · **EXT-080** ·
  **Manual section 6 category 10 — illustrative practice** · currency checked 2026-08-03 · limitation: **voluntary,
  rights-preserving and non-sector-specific by NIST's own description; not a standard and not a
  regulation**.
- **OECD** · *Recommendation of the Council on Artificial Intelligence (the OECD AI Principles)* · relied
  on for: the existence of an international expectation of human accountability for AI-influenced outcomes
  · **EXT-081** · **Manual section 6 category 10 — illustrative practice** · currency checked 2026-08-03 ·
  limitation: a Council Recommendation; **never legislation**; non-binding even on adherents.
- **European Union** · *Regulation (EU) 2024/1689 (the AI Act)* · relied on for: illustrating a risk-tiered
  regulatory approach · **EXT-100** · **Manual section 6 category 1 — applicable legislation or regulation** · currency checked
  2026-08-03 · limitation: **binding legislation within the European Union only**, named here to illustrate the
  shape only; relied on for no requirement; applicability to any given project is a question for qualified
  local counsel.

**18. Jurisdictional caution.** Obligations on AI transparency, human oversight, impact assessment,
automated decision-making, intellectual property in generated output and liability for AI-influenced harm
are jurisdiction-specific and are changing quickly. Obtain local legal advice before deploying AI in a
process affecting people's rights, employment, safety, or access to a service.

**19. Related PCI Standards.** `PCI-FND-STD-03` (the parent verification obligation); `PCI-FND-STD-14`;
`PCI-FND-STD-04`; `PCI-FND-STD-01`; `PCI-PML-STD-01.02`; `PCI-PML-STD-11.01`; `PCI-PML-STD-14.01`.
**What this standard adds to `PCI-FND-STD-03`:** the foundational standard requires independent verification
before reliance. This standard requires an **AI use record covering embedded features**, a **named method
per output class
recorded before use**, **depth matched to consequence with the basis recorded**, and a **failure log routed
into the organisation's standing artefacts**.

**20. Related Body of Knowledge content.** PML-AI · Domain 14 · KA 14.2 Dashboards, analytics, digital twins
and automation; KA 14.3 AI use across the lifecycle, prompting and verification; KA 14.4 Explainability,
bias, human accountability, cybersecurity and privacy. Also Domain 1 KA 1.4 the leader's AI accountability;
Domain 9 KA 9.4 AI-output quality.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) list the AI systems and
embedded AI features actually in use on the project, by inspecting the tools rather than by asking, and
match each to an entry in the AI use record; (b) for each output class, retrieve the recorded verification
method and confirm it is one of the named methods with its parameters stated; (c) draw a stated sample of
outputs relied on in the period and find, for each, a verification record naming the method, the person and
the date, **dated before** the date of reliance; (d) confirm that every artefact with material AI assistance
carries the disclosure; and (e) confirm every recorded verification failure produced a correction to
anything already issued. An AI system in use and absent from the record fails the test, and an output relied
on before its verification date fails it.

**22. Breach indicators.** An AI use record listing only the tools procured centrally. Verification fields
containing "reviewed". Verification dates after issue dates. Identical verification records across
dissimilar output classes. A supplier deliverable whose style changes abruptly with no disclosure. A model
confidence score cited as the verification. Verification failures with no corrections.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure on the associated competency; ethics review; certification investigation;
suspension or withdrawal — each subject to due process and a right of appeal.

**24. Examination application.** AI-verification case: a generated risk register of forty entries is offered
for the gate pack, and the candidate states the method, the sample and what is done with the entries that
fail. Ethical dilemma: an output cannot be verified by any available method and the decision is due.
Evidence selection: which artefacts establish that verification preceded reliance.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-14-02` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the AI use
record, the named-method rule, proportionate depth and the failure log separated into five process
requirements; element 16 rewritten to name the permitted methods explicitly, because "review the AI output"
was the defect the previous edition shared with most of the corpus. **Stage 9 amendment:** element 10
required independence only of the system's configuration, which allowed the preparer of a material
output to be its verifier; element 10 now states that this is the minimum this standard adds and does not
displace the independent-person verification `PCI-FND-STD-03` requires before reliance.

---

## Domain 15 — Programmes, Portfolios and Enterprise Delivery

### PCI STANDARD PCI-PML-STD-15.01 — Programme Integration and Dependency Ownership

**1. Normative requirement.** A credential holder leading or governing a programme must ensure that every
dependency between components is recorded with a named giver, a named receiver, the thing to be given, the
date, and the consequence of a breach.

**2. Purpose.** Programmes fail at their joints. A dependency with no named owner on the giving side is not
a dependency but a hope, and a milestone that requires six of them holds only as their joint probability,
which is a great deal lower than any of them individually and lower than every dashboard suggests. The
failure this prevents is the programme whose components are each green and whose integrated milestone is
not achievable.

**3. Scope.** Every credential holder leading, integrating, governing, reporting on or assuring a programme
or a multi-component delivery, including dependencies on parties outside the programme and dependencies
between the programme and business-as-usual operations.

**4. Defined terms.** *dependency* · *decision owner* · *material* · *evidence* · *escalation threshold* ·
*baseline* (control sense). Additionally, **giver** means the named individual accountable for supplying
the dependency; **integrated milestone** means a milestone whose achievement requires more than one
component's contribution.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-15.01-PR-01` — Named on both sides.** Every dependency must name an individual on the
  giving side and an individual on the receiving side, and the giver must have confirmed the commitment.
  A dependency naming organisations rather than people does not satisfy this requirement.
- **`PCI-PML-STD-15.01-PR-02` — Integrated milestone confidence is computed as a conjunction.** Where an
  integrated milestone depends on several dependencies, its confidence must be reported as the joint
  probability of the dependencies holding, not as an average, a weighted average or a proportion of green
  items — and the correlation assumption used must be stated.
- **`PCI-PML-STD-15.01-PR-03` — Decoupling is considered before improvement.** Where an integrated
  milestone's confidence is below the tolerance the governance sets, the credential holder must record
  whether a dependency can be removed or deferred, because removing one dependency changes the arithmetic
  by more than improving any of them.
- **`PCI-PML-STD-15.01-PR-04` — Breach of a dependency is an issue on the day it is known.** A dependency
  that will not be met must be raised as an issue under `PCI-PML-STD-08.02` on the day the giver knows,
  not on the date it was due.

**6. Prohibited actions.** Recording a dependency with an organisation as giver. Reporting an integrated
milestone's confidence as an average of its dependencies. Accepting a receiver-side entry with no giver
confirmation. Rolling up component RAG ratings into a programme rating with no stated rule. Reporting a
dependency as met when a partial or conditional delivery was made. Holding a known dependency breach until
the due date.

**7. Required evidence.** The dependency register with giver, receiver, thing, date, confidence and breach
consequence per entry, and the giver's confirmation; the integrated milestone confidence computation with
its stated correlation assumption; the decoupling analysis where tolerance was breached; the issue records
for dependency breaches with the date known and the date raised.

**8. Responsible role.** The named credential holder leading the programme, for the register and the
integrated computation. The named giver, for the commitment. The named receiver, for raising a breach.

**9. Approval authority.** The programme's governing body approves the dependency tolerance and any
decoupling that changes a component's scope. A component must not release itself from a dependency.

**10. Independence requirement.** At each programme gate at which the delegation schedule requires
assurance, the dependency register and the integrated confidence computation must be reviewed by a
competent reviewer independent of every component, not merely of one — because a reviewer inside one
component has an interest in where the dependency risk is recorded.

**11. Materiality or threshold.** This standard states no confidence figure. The organisation's governance sets
the confidence tolerance for an integrated milestone, the escalation threshold for a dependency at risk and
the reporting cadence; this standard requires that they are documented and applied, and that the **arithmetic is
the conjunction** rather than an average — which is a method requirement, not a threshold.
*Six-person internal project:* the standard applies only where the project is a component of a programme, and
then its obligation is to name a giver and a receiver for each of its handful of external dependencies and
to raise a breach the day it is known.
*Multi-partner national programme:* the register is held at programme level with per-partner extracts,
givers are named individuals inside partner organisations with the commitment reflected in the partner
agreement, and the integrated confidence is computed and published at programme level so that no partner
computes its own.

**12. Exception and waiver.** An exception permitting an integrated milestone to be reported on a basis
other than the conjunction may be approved only by the programme's governing body, only where the
alternative basis and its assumption are stated on the face of the report, and only for a stated period. No
exception permits a dependency with no named giver.

**13. Escalation trigger.** A dependency with no giver confirmation. An integrated milestone whose computed
confidence is below tolerance. A dependency the giver states will not be met. A component proposing to
change a date on which another component depends. A correlation between components' dependencies that the
stated assumption does not cover.

**14. AI application.** AI may build and reconcile a dependency register from component schedules, detect
dependencies present in one component's plan and absent from another's, compute integrated confidences and
their sensitivities, identify which single dependency change most improves a milestone, and detect
dependency entries that have not been confirmed by their giver.

**15. AI prohibition.** An AI system must not commit a dependency on behalf of a giver, decide that a
dependency is met, set a confidence value that is reported without a named human's derivation, or approve a
decoupling.

**16. AI verification.** **Independent recomputation plus named approval.** Every integrated confidence
computed with AI assistance must be recomputed by hand before it is reported — it is a product of a handful
of numbers and taking it on trust is indefensible — and every AI-proposed dependency must be confirmed by
the named giver before it enters the register as a commitment.

**17. External reference.**
- **ISO** · *ISO 21503, Guidance on programme management* · relied on for: the existence of programme-level
  integration and dependency management · **EXT-030**, **not independently verified — verify current
  requirements** · **Manual section 6 category 3 — international voluntary standard** · limitation: guidance;
  voluntary; not certifiable; open verification status.
- **ISO** · *ISO 21505, Guidance on governance* · relied on for: the existence of governance across
  multiple components · **EXT-032**, **not independently verified** · **Manual section 6 category 3** ·
  limitation: as above.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of interface and
  dependency management within delivery · **EXT-028** · **Manual section 6 category 3** · currency checked
  2026-08-03 · limitation: guidance; voluntary; not certifiable.

**18. Jurisdictional caution.** Where a dependency crosses a contractual boundary, whether it is
enforceable, what notice is required and what remedy exists are contractual questions determined by the
governing law. Obtain legal advice before relying on a dependency register in a commercial position.

**19. Related PCI Standards.** `PCI-FND-STD-05`; `PCI-FND-STD-11`; `PCI-FND-STD-12`; `PCI-PML-STD-06.01`;
`PCI-PML-STD-08.01`; `PCI-PML-STD-08.02`; `PCI-PML-STD-11.01`; `PCI-PML-STD-15.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 15 · KA 15.1 Programme architecture and
dependency management, including dependency arithmetic at programme scale and decoupling as the primary
response. Also Domain 2 KA 2.3 topic 2.3.4 assumption and dependency management; Domain 6 KA 6.1 logic
networks.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) take every entry in the
dependency register and find a named individual on each side and a recorded confirmation from the giver;
(b) take a stated integrated milestone, extract its dependencies' confidences and recompute the joint
probability by hand, and reconcile it to the reported figure; (c) confirm the correlation assumption is
stated on the report; (d) find, for every milestone below tolerance, a recorded decoupling analysis; and
(e) for a stated sample of breached dependencies, compare the date the giver knew with the date the issue
was raised. A reported integrated confidence that is an average rather than a product fails the test.

**22. Breach indicators.** Dependency registers whose giver column contains organisation names. Integrated
milestones reported at the average of their dependencies. Programme ratings greener than every component
computation supports. Dependency breaches raised on the due date. Registers where receivers created every
entry and no giver confirmed one. The same dependency recorded differently in two components' plans.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; certification investigation; suspension or withdrawal — each subject to due
process and a right of appeal.

**24. Examination application.** Calculation review: six dependencies at stated confidences, and the
candidate computes the milestone's joint probability and compares it with the dashboard's average.
Scenario judgement: a component offers to improve one dependency from 0.85 to 0.92 and the candidate
evaluates it against removing a different dependency. Evidence selection: what makes a dependency register
a control.

**25. Version and status.** Version 1.0 · **not yet approved** · effective on approval · **new standard** — the
v1.0 set reached Domain 15 only through *Related book content* fields and therefore created no
programme-level obligation. Amendment note: none.

---

### PCI STANDARD PCI-PML-STD-15.02 — Portfolio Prioritisation and Capacity Truth

**1. Normative requirement.** A credential holder advising or deciding a portfolio's composition must not
allow the portfolio to hold more concurrent work than the organisation's assessed delivery capacity
supports.

**2. Purpose.** A portfolio that is over-committed does not deliver less; it delivers everything later, and
the delay is invisible because each component reports its own slippage as a local problem. The failure this
prevents is the standing organisational condition in which every project is late for the same reason and no
project can see it.

**3. Scope.** Every credential holder preparing, recommending, approving, reporting on or assuring a
portfolio's composition, prioritisation, capacity plan or intake, including enterprise portfolio management
offices and including intake outside the formal portfolio process.

**4. Defined terms.** *material* · *decision owner* · *benefit* · *evidence* · *independent* · *dependency*
· *sponsor*. Additionally, **assessed capacity** means the organisation's recorded judgement of how much
concurrent work each constrained resource pool can carry; **intake** means the admission of new work into
the portfolio.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-15.02-PR-01` — Capacity is assessed and recorded per constrained pool.** The credential
  holder must hold a dated assessment of capacity for each constrained resource pool the portfolio depends
  on, with the basis of the assessment stated.
- **`PCI-PML-STD-15.02-PR-02` — Intake is tested against capacity before approval.** No component may be
  approved into the portfolio without a recorded test of its demand against assessed capacity, and the
  result of that test must be presented to the approving authority.
- **`PCI-PML-STD-15.02-PR-03` — Over-commitment is reported, not distributed.** Where demand exceeds
  assessed capacity, the credential holder must report the over-commitment and the components affected,
  and must not resolve it by reducing every component's assumption proportionally.
- **`PCI-PML-STD-15.02-PR-04` — Prioritisation criteria are published and applied.** The criteria by which
  components are ranked must be documented and published to the sponsors whose components are ranked, and
  the ranking must follow them; any departure must be recorded with its reason and its author.

**6. Prohibited actions.** Approving intake with no capacity test. Resolving over-commitment by assuming
higher productivity. Ranking components against undisclosed criteria. Admitting work outside the portfolio
process and reporting portfolio capacity as though it did not exist. Presenting a capacity plan whose
protective capacity has been removed without stating that it was.

**7. Required evidence.** The dated capacity assessment per constrained pool with its basis; the intake
test per approved component with its result; the over-commitment reports with affected components; the
published prioritisation criteria and the ranking that applied them; the record of any departure from the
criteria.

**8. Responsible role.** The named credential holder accountable for the portfolio process — typically the
head of the portfolio or enterprise delivery function. The portfolio's governing body decides composition.
Individual sponsors are not accountable for the portfolio's aggregate position.

**9. Approval authority.** The portfolio's governing body approves intake, composition and the capacity
assessment. A component's sponsor must not approve their own component's admission. A departure from the
published criteria may be approved only by the governing body, with the reason recorded.

**10. Independence requirement.** The capacity assessment and the intake test must be prepared by a function
independent of any single competing component, and the credential holder performing them must not be the
sponsor or leader of a component being ranked. Where the same person unavoidably holds both roles, they
must abstain from the ranking of their own component under `PCI-PML-STD-01.03-PR-04`.

**11. Materiality or threshold.** This standard states no utilisation figure and no work-in-progress limit,
because both are organisation-specific and a borrowed number is worse than none. The organisation's
governance sets the capacity basis, the intake threshold and the concurrency limits it applies; this standard
requires that these are documented, that the assessment is dated, and that intake is tested against them.
Where protective capacity is held, its size and its basis must be recorded so that removing it is a visible
decision.
*Six-person internal project:* the standard applies to the organisation that funds it rather than to the project;
for a small organisation the capacity assessment may be a single table of six people and their committed
availability, which is enough to make an intake test real.
*Multi-partner national programme:* capacity is assessed per pool per organisation and consolidated, the
intake test states which pools are constrained, and prioritisation criteria are published to every sponsor
including those in partner organisations — because criteria disclosed only inside the client organisation
produce exactly the disputes the criteria exist to prevent.

**12. Exception and waiver.** An exception permitting intake above assessed capacity may be approved only by
the portfolio's governing body, only where the components that will be delayed are identified by name in the
approval, and only where their sponsors are informed. **An over-commitment approved without naming who is
delayed is not an exception; it is an undisclosed decision.**

**13. Escalation trigger.** Demand exceeding assessed capacity on any constrained pool. Intake approved with
no capacity test. A ranking that departs from the published criteria with no recorded reason. Work admitted
outside the portfolio process. A capacity assessment older than the period the governance sets.

**14. AI application.** AI may aggregate demand across components, model portfolio composition under a
capacity constraint, compute the deferral cost of excess concurrency, test a ranking against the published
criteria and flag departures, and detect work in delivery systems that has no portfolio record.

**15. AI prohibition.** An AI system must not decide portfolio composition, rank components as the operative
decision, approve intake, or set the capacity assessment. An optimiser's allocation is a recommendation, and
composition is a reserved decision class under `PCI-PML-STD-01.02-PR-01`.

**16. AI verification.** **Independent recomputation plus sensitivity analysis.** Any AI-produced portfolio
allocation must be recomputed against the recorded capacity assessment by a competent reviewer, and the
reviewer must vary the two most influential capacity assumptions and record whether the recommended
composition survives — because a composition that is optimal only at the assessment's point estimate is not
a plan.

**17. External reference.**
- **ISO** · *ISO 21504, Guidance on portfolio management* · relied on for: the existence of portfolio
  balancing against capacity and strategy · **EXT-031**, **not independently verified — verify current
  requirements** · **Manual section 6 category 3 — international voluntary standard** · limitation: guidance;
  voluntary; not certifiable; open verification status.
- **ISO** · *ISO 21503, Guidance on programme management* · relied on for: the existence of a programme
  layer between portfolio and project · **EXT-030**, **not independently verified** · **Manual section 6 category
  3** · limitation: as above.
- **ISO** · *ISO 21500, Context and concepts* · relied on for: the vocabulary distinguishing project,
  programme and portfolio · **EXT-027** · **Manual section 6 category 3** · currency checked 2026-08-03 ·
  limitation: **context and concepts only** since its current edition; guidance moved to ISO 21502.

**18. Jurisdictional caution.** Portfolio decisions that change employment, close operations, or alter a
regulated service can engage employment consultation, regulatory notification and public-law duties. Obtain
local legal advice before a portfolio decision that stops work with those consequences.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-05`; `PCI-FND-STD-11`; `PCI-PML-STD-02.01`;
`PCI-PML-STD-02.02`; `PCI-PML-STD-07.02`; `PCI-PML-STD-15.01`; `PCI-PML-STD-16.03`.

**20. Related Body of Knowledge content.** PML-AI · Domain 15 · KA 15.2 Benefits and portfolio balancing;
KA 15.3 Capacity and enterprise PMOs; KA 15.4 Transformation governance and strategic reporting. Also
Domain 2 KA 2.2 topic 2.2.3 selection and prioritisation models; Domain 13 KA 13.2 flow.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) retrieve a dated capacity
assessment for every constrained pool the portfolio depends on; (b) take every component admitted in the
period and find a recorded intake test with its result presented to the approving authority; (c) sum the
portfolio's current demand on each constrained pool and compare it with the assessed capacity, reproducing
the over-commitment position the portfolio reported; (d) retrieve the published prioritisation criteria,
re-rank a stated sample of components against them, and account for every difference from the published
ranking by a recorded departure with a reason and an author; and (e) reconcile the portfolio record against
the delivery systems and find no material work in progress with no portfolio entry. An intake approved with
no capacity test fails the test.

**22. Breach indicators.** Capacity assessments older than the last reorganisation. Intake approvals with no
test attached. Portfolio dashboards showing full capacity utilisation and no deferrals. Rankings that
correlate with sponsor seniority rather than with the published criteria. Work in delivery systems absent
from the portfolio. Protective capacity removed between versions with no decision record.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Calculation review: aggregate demand against a constrained pool, and the
deferral the excess produces. Scenario judgement: a new component is directed into the portfolio by an
executive with no capacity test. Ethical dilemma: a ranking is adjusted after publication to move one
sponsor's component up.

**25. Version and status.** Version 1.0 · **not yet approved** · effective on approval · **new standard** — the
v1.0 set contained no portfolio obligation. Amendment note: none.

---

## Domain 16 — Transition, Closeout and Benefits Realisation

### PCI STANDARD PCI-PML-STD-16.01 — Transition Readiness and the Gate Block

**1. Normative requirement.** A credential holder must not permit a transition, go-live or release into
operational use to proceed while any mandatory precondition for it is recorded as not met.

**2. Purpose.** Transition is the decision that is irreversible for the people who did not take it. Some of
its conditions are not assessments of likelihood at all — a safety case closed by the authority that owns
it, a licence or regulatory approval granted, a privacy or data-protection assessment signed, statutory
notifications made, takeover certificates issued. The failure this prevents is the one a single readiness
template invites: **every condition entered as a probability**, at which point a forbidden thing has been
silently converted into a chance that it is permissible, and the resulting number can be raised by improving
something else entirely.

**3. Scope.** Every credential holder preparing, assessing, recommending, deciding or assuring a transition,
go-live, commissioning, handover or operational release, in every delivery model and at every scale,
including phased and pilot releases.

**4. Defined terms.** *mandatory precondition* · *gate block* · *discretionary condition* · *acceptance* ·
*evidence* · *decision owner* · *independent* · *material*. Additionally, **readiness assessment** means the
record of whether the conditions for transition are satisfied; **go/hold economics** means the comparison of
the cost of proceeding against the cost of waiting.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-16.01-PR-01` — Two blocks, separated before assessment.** The readiness assessment must
  separate a gate block of mandatory preconditions from a block of discretionary conditions, and the
  separation must be made and approved **before** the assessment is populated. A single undifferentiated
  condition list does not satisfy this requirement.
- **`PCI-PML-STD-16.01-PR-02` — Gate-block items are binary, authority-named and dated.** Each mandatory
  precondition must be recorded met or not met, with the approving authority named and the date given, and
  must be closed by the authority that owns it rather than by the project.
- **`PCI-PML-STD-16.01-PR-03` — No probability, no weight, no score on a gate-block item.** A mandatory
  precondition must not be expressed as a probability, a percentage, a confidence, a weighting, a
  red-amber-green rating or any other quantity admitting degree, and must not appear among the
  discretionary conditions' probabilities or in any aggregate computed from them.
- **`PCI-PML-STD-16.01-PR-04` — No gate-block item in the go/hold economics.** A mandatory precondition must
  not be entered into the go/hold economics, priced against the cost of delay, or traded against any other
  condition. **Expressing it as a cost concedes that some cost of delay would be large enough to buy it,
  and that is precisely what the item exists to forbid.**
- **`PCI-PML-STD-16.01-PR-05` — Discretionary readiness is a conjunction, and its assumption is stated.**
  The probability of a clean transition across the discretionary conditions must be reported as their joint
  probability, not as an average, a weighted average or the proportion of conditions rated green, and the
  correlation assumption used must be stated on the face of the assessment.

**6. Prohibited actions.** Entering a safety case, a licence approval, a privacy assessment, a statutory
notification or a takeover certificate as a probability, a percentage or a rating. Including a mandatory
precondition in a weighted readiness score. Pricing a mandatory precondition against the cost of delay.
Producing an option set while the gate block is open. The project recording a gate-block item as met on its
own authority. Reporting a readiness average and describing it as the probability of a clean transition.

**7. Required evidence.** The readiness assessment in two blocks, with the separation approved and dated
before population; the gate-block record with each item's met/not-met status, approving authority and date,
and the underlying certificate, approval, signature or notification; the discretionary block with its
probabilities, its joint computation and its stated correlation assumption; the go/hold economics showing
only discretionary conditions; the transition decision with its named decision owner.

**8. Responsible role.** The named credential holder leading the transition, for producing the assessment in
the required form and for refusing to proceed while the gate block is open. The named transition decision
owner, for the decision. The **authority that owns each mandatory precondition**, for closing it.

**9. Approval authority.** The transition decision owner the delegation schedule names decides, and may
decide only *hold* while any gate-block item is not met. Each mandatory precondition is closed by its owning
authority — clinical or safety governance, the regulator or licensing body, the accountable privacy
authority, the notifying authority, the certifying party — and **by nobody else**.

**10. Independence requirement.** Every mandatory precondition must be closed by an authority independent of
the project in the sense defined above: not the preparer of the thing being approved, not accountable for
the delivery date, and holding no interest in the transition proceeding. The readiness assessment itself
must be reviewed at the transition gate by a competent reviewer independent of the delivery organisation,
who must satisfy `PCI-PML-STD-01.03-PR-06`.

**11. Materiality or threshold.** **This standard states no readiness percentage, and any such figure would be
the defect it exists to prevent.** The gate block is binary and admits no threshold at all. For the
discretionary block, the organisation's governance sets the confidence tolerance at which transition may be
recommended and the escalation route below it; this standard requires that the tolerance is documented, that the
computation is the conjunction, and that the assumption is stated. Which items belong in the gate block is
set by law, licence, contract and the organisation's governance — not by this standard and not by the project —
and the credential holder's obligation is to obtain that list from the owning authorities and record it.
*Six-person internal project:* the gate block may contain a single item — an information-security sign-off,
say — recorded met with the name of the person who signed it and the date; the discretionary block may hold
four conditions whose product is computed in one line. The obligation costs almost nothing and removes the
failure entirely.
*Multi-partner national programme:* the gate block is assembled from every jurisdiction, regulator and
operating organisation the transition touches, each item names its own owning authority, and no partner may
close another's item. The discretionary conjunction is computed at the level at which the transition
actually happens — per site, per region — rather than nationally, because a national average conceals the
site that is not ready.

**12. Exception and waiver.** **No exception, waiver, dispensation or deferral is permitted** in respect of
a mandatory precondition. There is no authority within PCI's system that may approve a transition while a
gate-block item is not met, and a credential holder must not seek, recommend or record one. Where the
organisation believes an item has been wrongly classified as mandatory, the remedy is to have the **owning
authority** reclassify it, in writing, before the assessment — not to trade it at the gate.

**The emergency case, stated so that the standard survives it.** Where continuing without the transition is
itself the greater danger — a failed system that cannot be restored, an asset that must be taken out of
service — the answer is still not a trade at the gate. It is that **the authority that owns the item
exercises its own emergency instrument**: a regulator's derogation, a safety authority's temporary
approval, a licensing body's dispensation, an interim certificate. An item closed or varied through the
owning authority's own emergency process is a **met** item, recorded under `PR-02` with that authority's
instrument, its scope and its expiry, and reported at the next governance meeting. What a credential
holder must not do is seek such an instrument from anyone other than the owning authority, treat its
absence as a probability, or permit the transition while the item stands not met. Where the owning
authority declines or cannot be reached, the decision remains **hold**, and the consequences of holding
are escalated under `PCI-FND-STD-11` rather than resolved at the gate.

**13. Escalation trigger.** Any gate-block item not met as the transition date approaches. Any proposal to
express a mandatory precondition as a probability, a rating or a cost. Any request for a dispensation
against a gate-block item. Discovery that a gate-block item was closed by the project rather than by its
owning authority. A discretionary conjunction below the documented tolerance. A readiness dashboard
reporting an average.

**14. AI application.** AI may assemble the readiness assessment from source records, check that every
gate-block item carries a named authority and a date, flag any gate-block item that has acquired a numeric
value, compute the discretionary conjunction and its sensitivities, and identify which discretionary
condition most improves the joint probability.

**15. AI prohibition.** An AI system must not decide readiness, close a mandatory precondition, assign a
probability to one, recommend transition, or produce a readiness figure that is reported without a named
human's derivation. **A model asked to score readiness across an undifferentiated condition list will
produce exactly the defect this standard prohibits**, and the credential holder must not use one in that form.

**16. AI verification.** **Independent recomputation plus boundary testing.** The discretionary conjunction
must be recomputed by hand before the transition decision and reconciled to the reported figure. The
credential holder must additionally test the assessment's structure by boundary case: set one gate-block
item to not met and confirm that the assessment's output is *hold* and that no option set or economic
comparison is produced. An assessment that returns a number in that test is structurally non-compliant and
must be corrected before use.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of transition and
  handover as lifecycle activities · **EXT-028** · **Manual section 6 category 3 — international voluntary
  standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **ISO** · *ISO 9001, Quality management systems — Requirements* · relied on for: the existence of a
  certifiable requirement that outputs are verified before release · **EXT-033** · **Manual section 6 category 3**
  · currency checked 2026-08-03 · limitation: voluntary unless imported; certification concerns a management
  system; no requirement is imported here.
- **ISO** · *ISO 10006, Guidelines for quality management in projects* · relied on for: the existence of
  project-specific quality guidance covering closure · **EXT-035** · **Manual section 6 category 3** · currency
  checked 2026-08-03 · limitation: **guidelines, not requirements**; voluntary; not certifiable.
- **ISO** · *ISO 45001, Occupational health and safety management systems — Requirements* · relied on for:
  the existence of a certifiable management-system standard within which operational readiness for safety
  is addressed · **EXT-123** · **Manual section 6 category 3** · currency checked 2026-08-03 · limitation:
  certifiable, but voluntary unless required; **it does not determine which items are mandatory
  preconditions for any given transition** — law, licence and contract do.

**18. Jurisdictional caution.** Which approvals, certificates, notifications and assessments are legally
required before an asset, system or service enters use — and which body may grant each — are determined by
statute, licence conditions, sector regulation and contract, and they differ by jurisdiction and by sector.
**This standard does not state which items are mandatory for any project, and a professional must not infer the
list from the examples given.** Obtain local legal and regulatory advice to establish the gate block for the
specific transition.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-11`; `PCI-FND-STD-13`; `PCI-PML-STD-03.03`;
`PCI-PML-STD-09.01`; `PCI-PML-STD-15.01`; `PCI-PML-STD-16.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 16 · KA 16.1 Handover, commissioning and
readiness — the two-block readiness model, the gate block of mandatory preconditions, the conjunction across
discretionary conditions and its contrast with the averaged dashboard. Also Domain 14 KA 14.4, which states
the same boundary for security and privacy controls; Domain 9, which states it for quality, where the
economic optimum is taken among compliant options rather than across them.

**21. Compliance test.** Compliance is demonstrated when a reviewer can: (a) obtain the readiness assessment
and confirm it is in two blocks, with the separation approved and dated before the assessment was populated;
(b) confirm that **every** gate-block item carries a met/not-met value, a named approving authority and a
date, and carries **no** numeric value of any kind; (c) confirm that no gate-block item appears in the
discretionary probability list, in any weighted score, or in the go/hold economics; (d) recompute the
discretionary joint probability by hand from the stated conditions and reconcile it to the reported figure,
and find the correlation assumption stated; and (e) for any period in which a gate-block item was not met,
confirm that the recorded decision was **hold** and that no option set or economic comparison was produced.
**A probability, percentage, weighting or rating attached to any mandatory precondition fails this test
outright**, whatever the transition's outcome, and so does an option set produced while the gate block was
open.

**22. Breach indicators.** A single readiness template with one probability column. A safety case shown at
"95 per cent". A dashboard reporting readiness as a weighted average. A cost-of-delay analysis that includes
a licence approval among its variables. A gate-block item signed by the project's own transition manager. A
readiness figure that improved without any condition changing. Requests for a "dispensation" against a
safety or licence item.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure on the associated competency; ethics review; certification investigation;
suspension or withdrawal of the PML-AI credential — each subject to due process and a right of appeal.
**PCI cannot authorise a transition, grant an approval, or override any regulator or safety authority**, and
no PCI process substitutes for the approvals this standard requires to be obtained from their owning authorities.

**24. Examination application.** Calculation review: seven discretionary conditions whose product is
computed and compared with the dashboard's average, with the candidate stating why the two differ and in
which direction. Scenario judgement: a safety case is not yet closed, the cost of delay is large, and the
candidate states the option set — which is *hold*, and there is no option set below the gate block. Ethical
dilemma: an executive asks for the safety case to be shown as a probability so that the readiness figure
"reflects reality".

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-16-01` v1.0. Amendment note: renumbered, restructured and **substantially strengthened** to match
the current Domain 16 readiness model. The v1.0 standard required readiness to be assessed and evidenced but did
not distinguish mandatory preconditions from discretionary conditions, and therefore did not prohibit the
central failure — a forbidden item entered as a probability. The two-block separation, the binary
authority-named recording, the prohibitions on probability and on economic trade, and the conjunction rule
are new, and element 12 now states expressly that no exception exists. **Stage 9 amendment:** the
red-team's emergency case — the transition that must happen because continuing is the greater danger —
had no compliant route, and a standard with no route in the case that will arise is a standard that gets broken;
element 12 now states the route without conceding the duty, by directing the credential holder to the
owning authority's own derogation or temporary-approval instrument, requiring it to be recorded under
`PR-02` with its scope and expiry, and leaving *hold* as the decision where that authority declines or
cannot be reached.

---

### PCI STANDARD PCI-PML-STD-16.02 — Operational Acceptance and Handover

**1. Normative requirement.** A credential holder must not close a project until the receiving organisation
has recorded, by a named individual, that it accepts operational responsibility for what has been
transferred.

**2. Purpose.** Projects end on their own schedule; operations begin on theirs. The failure this prevents is
the closure that transfers an asset, a system or a service to an organisation that has not agreed to take
it, has not been given what it needs to run it, and discovers both facts after the project team has
dispersed — at which point reconstructing what the team knew costs several times what recording it would
have cost.

**3. Scope.** Every credential holder closing a project, phase, contract or component that transfers
something to be operated, maintained, supported or further developed by another organisation or team,
including transfers to a supplier's operations, to business-as-usual, and to a successor project.

**4. Defined terms.** *acceptance* · *evidence* · *decision owner* · *material* · *independent* ·
*mandatory precondition*. Additionally, **receiving organisation** means the team or organisation that will
operate, maintain or support what is transferred; **operational responsibility** means accountability for
running it, including for its faults.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-16.02-PR-01` — A named acceptance from the receiver.** Operational acceptance must be
  recorded by a named individual in the receiving organisation, with the date, and must state what is
  accepted. Acceptance by the project on the receiver's behalf does not satisfy this requirement.
- **`PCI-PML-STD-16.02-PR-02` — The handover set is defined and delivered.** The credential holder must
  agree with the receiving organisation, before closure, what must be handed over — as-built
  documentation, configuration baselines, runbooks, licences, credentials, contracts, open defects, known
  workarounds, contacts — and must record delivery of each item.
- **`PCI-PML-STD-16.02-PR-03` — Open items transfer with owners and dates.** Every open defect,
  nonconformity, warranty item, contractual obligation and carried risk must transfer with a named owner in
  the receiving organisation and a date, and must not be closed at project closure for the reason that the
  project is closing.
- **`PCI-PML-STD-16.02-PR-04` — Capability is transferred, not just documents.** The credential holder must
  record how the receiving organisation was made able to operate what it receives — training delivered,
  procedures rehearsed, a period of supported operation — and the receiving organisation's confirmation
  that it was.

**6. Prohibited actions.** Closing a project with an unaccepted transfer. Recording acceptance signed by the
project. Closing open defects at closure to clear the register. Handing over documentation in place of
capability. Transferring a live product with no named ordering right under `PCI-PML-STD-13.02-PR-04`.
Retaining credentials, licences or contracts the receiver needs. Treating the disbanding of the team as the
closure event.

**7. Required evidence.** The operational acceptance record with the named receiver individual and the date;
the agreed handover set with delivery confirmation per item; the transferred open-item register with
receiving owners and dates; the capability-transfer record with the receiver's confirmation; the closure
decision with its named decision owner.

**8. Responsible role.** The named credential holder leading the project, for securing acceptance and
delivering the handover set. The named individual in the receiving organisation, for the acceptance. The
sponsor, for resolving a refusal.

**9. Approval authority.** The receiving organisation's named acceptance authority accepts. The sponsor
approves closure. The governing body approves closure where operational acceptance has not been obtained —
and that approval must state what is being transferred unaccepted and to whom.

**10. Independence requirement.** The acceptance authority must be independent of the project in the sense
defined above: accountable within the receiving organisation, not the project's own representative, and not
remunerated by reference to the project's closure. Where the project and the receiver sit in the same small
organisation, the acceptance authority must at least sit outside the project team and the arrangement must
be recorded.

**11. Materiality or threshold.** This standard states no number. The organisation's governance sets the closure
criteria, the handover set for each class of transfer, the supported-operation period and the tolerance for
open items transferred; this standard requires that they exist and are applied. An open item that is a mandatory
precondition under `PCI-PML-STD-16.01` does not transfer — it is closed before transition or the transition
does not happen.
*Six-person internal project:* the handover set is six items, acceptance is a dated line signed by the
receiving manager, and capability transfer is a two-hour walkthrough recorded as delivered.
*Multi-partner national programme:* acceptance is recorded per receiving organisation rather than once,
because a national transfer has many receivers, and the open-item register is split by receiver so that no
organisation inherits items it never saw.

**12. Exception and waiver.** Closure without operational acceptance may be approved only by the governing
body, only where the unaccepted transfer is described item by item, only where the receiving organisation is
notified in writing, and only where a named owner outside the project holds the residual responsibility
until acceptance. No exception permits an open safety, licence or statutory item to transfer unaccepted.

**13. Escalation trigger.** A receiving organisation that declines acceptance. A handover item the project
cannot supply. An open safety or regulatory item approaching closure. A receiver with no named individual
able to accept. Closure proposed while the team that holds the knowledge has already dispersed.

**14. AI application.** AI may assemble the handover set from project records and flag missing items, extract
open items from the defect, risk and issue registers for transfer, draft runbooks from configuration and
test records for human verification, and check that every transferred item has a receiving owner.

**15. AI prohibition.** An AI system must not accept operational responsibility, decide that a handover is
complete, close an open item, or be recorded as the receiving party.

**16. AI verification.** **Source tracing plus named approval.** Every AI-drafted runbook or as-built record
must be traced by a competent reviewer to the configuration baseline and test evidence it claims to reflect,
and the receiving organisation must confirm by name that each handover item is what it needs and that it
received it.

**17. External reference.**
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of transition and
  closure activities within delivery practice · **EXT-028** · **Manual section 6 category 3 — international
  voluntary standard** · currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **ISO** · *ISO 9001, Quality management systems — Requirements* · relied on for: the existence of a
  certifiable requirement to control documented information and to release outputs · **EXT-033** ·
  **Manual section 6 category 3** · currency checked 2026-08-03 · limitation: voluntary unless imported; no
  requirement is imported here.
- **ISO** · *ISO 15489-1, Records management — Concepts and principles* · relied on for: the existence of
  records-management principles governing the retention and transfer of records · **EXT-025** · **Manual section 6
  category 3** · currency checked 2026-08-03 · limitation: voluntary; guidance on principles; not a
  certifiable requirement for a project.

**18. Jurisdictional caution.** Whether operational responsibility, statutory duties, licences, safety
duties, data controllership and warranties transfer — and when — is determined by law, licence and contract,
not by a handover record. Obtain legal advice on what transfers and what remains, particularly for regulated
assets and for personal data.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-12`; `PCI-PML-STD-09.01`;
`PCI-PML-STD-13.02`; `PCI-PML-STD-14.01`; `PCI-PML-STD-16.01`; `PCI-PML-STD-16.03`.

**20. Related Body of Knowledge content.** PML-AI · Domain 16 · KA 16.2 Operational transition and contract
closeout; KA 16.3 Knowledge transfer and post-project review, including the cost of reconstructing as-built
knowledge after the team disperses. Also Domain 9 KA 9.3 acceptance; Domain 13 KA 13.1 product ownership.

**21. Compliance test.** Compliance is demonstrated when a reviewer can, for a closed project: (a) produce
an operational acceptance record naming an individual in the receiving organisation, dated on or before the
closure date; (b) take the agreed handover set and confirm a delivery record for every item; (c) take the
project's open defect, risk, nonconformity and warranty registers at closure and find every open item in the
receiving organisation's registers with a named owner and a date, with no item closed on the closure date
for that reason; (d) find the capability-transfer record and the receiver's confirmation; and (e) confirm no
mandatory precondition under `PCI-PML-STD-16.01` transferred open. Closure with no receiver acceptance and
no governing-body exception fails the test.

**22. Breach indicators.** Acceptance records signed by project staff. Registers that empty on the closure
date. Handover sets consisting only of documents. Receiving teams raising defects the project had recorded
and closed. Credentials still held by former project staff months after closure. Closure dates that precede
the receiver's first involvement.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; certification investigation; suspension or withdrawal — each subject to due
process and a right of appeal.

**24. Examination application.** Scenario judgement: the receiving operations manager refuses acceptance a
week before the project's funding ends. Evidence selection: which artefacts establish that capability, not
just documentation, was transferred. Calculation review: the cost of reconstructing as-built knowledge
against the cost of recording it.

**25. Version and status.** Version 1.0 · **not yet approved** · effective on approval · **new standard** — the
v1.0 set covered transition readiness and benefits measurement and left the acceptance of operational
responsibility between them unstated. Amendment note: none.

---

### PCI STANDARD PCI-PML-STD-16.03 — Benefits Measurement

**1. Normative requirement.** A credential holder must not state that a benefit has been realised except on
a measurement taken by the method, from the source and against the benefits baseline recorded for that
benefit before the change was made.

**2. Purpose.** Benefits reporting is the least policed statement an organisation makes about itself, and it
is made after the people who could contradict it have moved on. The failure this prevents is the realised
benefit that was measured differently from the way it was defined, against a baseline reconstructed
afterwards, by the party whose case depended on it.

**3. Scope.** Every credential holder measuring, reporting, reviewing or assuring benefits realisation on a
project, programme or portfolio, at closure and through the post-closure measurement period, including
benefits reported to funders, boards and the public.

**4. Defined terms.** *benefit* · *benefits baseline* · *evidence* · *material* · *independent* ·
*decision owner* · *sponsor*. Additionally, **realisation statement** means an assertion that a benefit has
been achieved in whole or in part; **measurement period** means the period after transition over which the
benefit is measured.

**5. Required actions — process requirements.**

- **`PCI-PML-STD-16.03-PR-01` — Measure by the recorded method and source.** Each realisation statement must
  use the measure, the source system and the calculation recorded for that benefit under
  `PCI-PML-STD-02.02-PR-02`, and any change to any of them must be recorded with its reason and its effect
  on the reported figure.
- **`PCI-PML-STD-16.03-PR-02` — Compare against the pre-change baseline.** The comparison must be against
  the benefits baseline measured before the change. Where a baseline was reconstructed, the statement must
  say so and state the reconstruction method.
- **`PCI-PML-STD-16.03-PR-03` — Attribution is stated, not assumed.** The statement must record what else
  changed in the measurement period that could have produced the movement, and must not attribute the whole
  movement to the project where other causes are known.
- **`PCI-PML-STD-16.03-PR-04` — Shortfalls are reported with the same prominence as achievements.** A
  benefit measured below its target must be reported to the same audiences, in the same document, and in the
  same period as the benefits that met theirs.

**6. Prohibited actions.** Changing a measure to one that shows a better result. Comparing against a
reconstructed baseline without saying so. Attributing an improvement with several causes wholly to the
project. Reporting only the benefits that were achieved. Extending a measurement period until a target is
met, without recording the extension. Reporting a proxy measure as the defined benefit.

**7. Required evidence.** The benefits register entries with method, source, baseline and target; the source
system extracts with dates and filters; the realisation statements with their comparisons; the record of any
method, source or period change with reasons; the attribution statement; the reporting record showing
shortfalls issued to the same audiences.

**8. Responsible role.** The named **benefits owner** for each benefit, for the measurement and the
statement. The **sponsor**, for the aggregate position reported to the governing body. The credential holder
who prepared the measurement, for its integrity.

**9. Approval authority.** The governing body approves the closure of a benefit as realised or unrealised.
The sponsor approves a change to a measurement period or method within their documented authority; above it,
the governing body. The delivery organisation approves nothing here.

**10. Independence requirement.** The measurement must be produced by, or verified by, a party independent
of the delivery organisation and independent of the benefits owner where the owner's own performance is
assessed on the benefit. Where an organisation is too small to supply that, the measurement must be taken
directly from an unmodified source system extract and the extract retained.

**11. Materiality or threshold.** This standard states no tolerance for a shortfall. The organisation's governance
sets the measurement period, the tolerance at which a shortfall is escalated, and the point at which a
benefit is closed as unrealised; this standard requires that these exist and are applied and that a shortfall
inside tolerance is still reported.
*Six-person internal project:* one benefit, measured from a report the operating team already produces,
compared with a baseline captured before go-live, reported once at three months and once at twelve.
*Multi-partner national programme:* benefits are measured by the operating organisations rather than by any
partner, the double-count check from `PCI-PML-STD-02.02` runs again at measurement, and the attribution
statement is required per benefit because a national programme's measurement period contains many other
causes.

**12. Exception and waiver.** An exception permitting a benefit to be reported on a method or source other
than the recorded one may be approved only by the governing body, only where the change and its effect on
the figure are stated in the report, and only where the original method's result is reported alongside it
where it can still be produced. No exception permits a shortfall to go unreported.

**13. Escalation trigger.** A benefit measured materially below target. A source system that no longer
produces the recorded measure. A proposal to change a method or extend a measurement period. A benefits
owner who declines to report a shortfall. Discovery that a baseline was reconstructed after the change.

**14. AI application.** AI may extract measurements from source systems on the recorded basis, detect
changes of method or source between periods, compute the comparison against baseline, identify concurrent
changes in the measurement period as candidate alternative causes for human assessment, and check that every
benefit in the register has a statement for the period.

**15. AI prohibition.** An AI system must not state that a benefit has been realised, decide attribution,
select the measure or source, or determine that a shortfall is immaterial.

**16. AI verification.** **Independent recomputation plus source tracing.** Every reported realisation figure
produced with AI assistance must be recomputed from the source extract by a competent reviewer, with the
extract date and filter recorded, and traced to the register entry that defines the method. Where AI proposed
alternative causes for attribution, a named human must assess each and record the conclusion.

**17. External reference.**
- **ISO** · *ISO 21504, Guidance on portfolio management* · relied on for: the existence of benefits
  tracking at portfolio level · **EXT-031**, **not independently verified — verify current requirements** ·
  **Manual section 6 category 3 — international voluntary standard** · limitation: guidance; voluntary; not
  certifiable; open verification status.
- **ISO** · *ISO 21503, Guidance on programme management* · relied on for: the existence of benefits
  realisation as a programme concern · **EXT-030**, **not independently verified** · **Manual section 6 category
  3** · limitation: as above.
- **ISO** · *ISO 21502, Guidance on project management* · relied on for: the existence of benefits
  identification and realisation within delivery practice · **EXT-028** · **Manual section 6 category 3** ·
  currency checked 2026-08-03 · limitation: guidance; voluntary; not certifiable.
- **ISO** · *ISO 15489-1, Records management — Concepts and principles* · relied on for: the existence of
  records-management principles governing retention of the evidence a measurement rests on · **EXT-025** ·
  **Manual section 6 category 3** · currency checked 2026-08-03 · limitation: voluntary; principles guidance.

**18. Jurisdictional caution.** Benefits statements made to funders, regulators, markets or the public can
engage grant-condition, disclosure, procurement and fraud law, and the legal standard for such a statement is
not the professional one used here. Obtain legal advice before a benefits claim is made outside the
organisation.

**19. Related PCI Standards.** `PCI-FND-STD-02`; `PCI-FND-STD-06`; `PCI-FND-STD-12`; `PCI-PML-STD-02.01`;
`PCI-PML-STD-02.02`; `PCI-PML-STD-11.01`; `PCI-PML-STD-15.02`; `PCI-PML-STD-16.02`.

**20. Related Body of Knowledge content.** PML-AI · Domain 16 · KA 16.4 Benefits measurement, responsible
archive and model/data retention. Also Domain 2 KA 2.3 topics 2.3.1 benefits mapping and 2.3.2 measures and
baselines; Domain 15 KA 15.2 benefits and portfolio balancing.

**21. Compliance test.** Compliance is demonstrated when a reviewer can, for every benefit reported as
realised in the period: (a) retrieve the register entry defining its measure, source, baseline and target,
and confirm the reported figure used that measure and that source; (b) reproduce the reported figure from
the source system extract, using the recorded calculation, and reconcile any difference; (c) confirm the
comparison is against a baseline whose measurement date precedes the change date, or that the report states
the baseline was reconstructed and how; (d) find the attribution statement listing concurrent changes; and
(e) confirm that every benefit measured below target in the period appears in the same report, to the same
distribution, as those that met theirs. A realisation figure that cannot be reproduced from the recorded
source fails the test.

**22. Breach indicators.** Measures that changed between the case and the measurement. Baselines dated after
go-live. Reports containing only achieved benefits. Measurement periods extended without a decision record.
Attribution statements absent from programmes with large concurrent change. Benefits reported as realised
whose owners cannot produce the extract.

**23. Consequence within PCI authority.** Correction required; output withheld; additional review;
escalation; examination failure; ethics review; certification investigation; suspension or withdrawal —
each subject to due process and a right of appeal.

**24. Examination application.** Calculation review: a reported improvement recomputed from source, showing
a method change between the case and the measurement. Ethical dilemma: a sponsor asks for the measurement
period to be extended by a quarter because the trend is improving. Evidence selection: which artefacts
establish that a benefit was realised.

**25. Version and status.** Version 2.0 · **not yet approved** · effective on approval · supersedes
`PML-LAW-16-02` v1.0. Amendment note: renumbered and restructured; legislative drafting removed; the
method-and-source rule, the pre-change baseline rule, the attribution statement and the equal-prominence
rule for shortfalls separated into four process requirements; the benefits-baseline sense of *baseline*
separated from the control sense per the suite terminology audit.

---
## Audit findings — the twenty-five questions worked over the set

Drafting Manual section 9 requires that every question below is answered before approval, and that a standard
failing one is revised, with the failure and its resolution recorded. This table records the working
for the set as a whole, names the standards that failed a question in draft, and states what was changed.
**Every failure listed was resolved before this edition was assembled**; the questions with an open
finding are marked and carried into the outstanding due-process stages.

The front-matter stage record above notes that the Stage 9 red-team was only partly worked for this
edition. It has since been completed across the whole four-file corpus, and its findings and their
disposition are recorded in [`STANDARDS_RED_TEAM_REPORT.md`](STANDARDS_RED_TEAM_REPORT.md). The amendments that pass
produced in this volume are noted in the element 25 of each standard changed, and the definitional reading
rules it added sit at the head of the Definitions above.

| # | Question | Finding across the set | Action taken |
|---|---|---|---|
| 1 | What exact failure does this standard prevent? | Answered in element 2 of all 32 standards. Three drafts stated a purpose that was really a benefit of compliance rather than a failure — `12.01`, `12.02`, `13.02`. | Element 2 rewritten in those three to name the failure: authority used to override professional judgement; information that stops flowing upward; the ordering right exercised by someone other than its named holder. |
| 2 | Is the requirement mandatory or only recommended? | All 32 principal obligations and all 148 process requirements are expressed with **must** or **must not**. No `should` carries an obligation anywhere in the set; no Recommended Practice is published in this edition. | None needed. Checked mechanically. |
| 3 | Can a professional know whether it applies to them? | Element 3 of each standard names the roles, the decisions and the delivery models. `15.01` and `15.02` initially read as though they bound every credential holder, when they bind those in programme and portfolio roles. | Element 3 of both narrowed, and element 11 of each states expressly what the obligation amounts to for a small internal project. |
| 4 | Is the responsible person identifiable? | Element 8 of all 32 names an individual role. No standard uses "the team", "management", "relevant people" or "the organisation" as a responsible role. Two drafts used "the PMO" — `03.03` and `09.02`. | Replaced with the named credential holder and the named artefact owner respectively. |
| 5 | **Is the required action observable?** | **The set's hardest question, and the one the previous edition failed.** `12.01` required conduct ("act with integrity, model the behaviour") and `12.02` required a state of affairs ("maintain conditions in which any team member can report bad news"). Neither can be observed and therefore neither could be required. | `12.01` rebuilt around **one observable act** — overriding another person's professional statement — with five process requirements that each leave a record. `12.02` rebuilt around **three observable things**: a published route with named recipients, the bypass property demonstrated person by person, and a concern register with seven mandatory fields. Both now pass. |
| 6 | **Is compliance provable?** | Element 21 of every standard is now a test a reviewer performs against records that exist. The behavioural standards were the exposure: a leadership standard provable only by testimony proves nothing. | `12.02` element 21 test (e) **cross-matches two record sets that already exist** — the concern register against assignment, role-change and assessment records — so that detriment is found by matching dates and names rather than by asking anybody how they felt. `12.01` element 21 test (a) uses **document version history**, which is produced automatically by every document system in use. |
| 7 | Is the required evidence proportionate? | Element 7 was tested against a six-person project throughout. Two standards required evidence a small project cannot economically produce — an earlier `05.02` required full traceability tooling and an earlier `14.02` required a verification log per output instance. | `05.02` now requires a maintained matrix in any form and full traversal only for the regulatory and safety subset; `14.02` requires a method per **output class**, not per output. |
| 8 | Can the standard be audited? | Yes for all 32. Each element 21 names the artefacts to obtain and the comparison to make. | None needed. |
| 9 | Can the standard be examined through a scenario? | Element 24 of every standard gives at least two examination modes drawn from the Manual's list. No standard is examinable only by recalling its number. | None needed. |
| 10 | **Can a professional technically comply while defeating its purpose?** | **Six standards were vulnerable and were changed.** `03.02` — register only decisions above the limit, so the aggregation rule sees nothing. `05.02` — resolve orphans by deleting requirements. `08.01` — reassess exposure downward instead of escalating. `13.01` — raise throughput by starting more work. `16.01` — enter a mandatory precondition as a high probability so the readiness figure stays true-looking. `12.02` — publish a route whose only recipient is the person concerns are about. | `03.02-PR-02` universal registration; `05.02-PR-03` orphans reported, not tidied; `08.01-PR-02` escalate on threshold rather than on expectation of resolution, with **the date the threshold was met** recorded beside the date escalated; `13.01-PR-04` metrics reported with the work-in-progress position; `16.01-PR-03` and `-PR-04` prohibit probability and prohibit economic trade, and element 16 adds a **boundary test of the assessment's structure**; `12.02-PR-02` the bypass property, demonstrated person by person. |
| 11 | Does it conflict with another PCI standard? | No conflict found. Two overlaps were resolved by narrowing: `01.02` against `PCI-FND-STD-04`, and `05.01` against `04.01` where a scope change is also a baseline change. | `01.02` retitled and narrowed to the reserved-class list, the automation inventory and the examination record. `05.01-PR-03` routes scope additions **into** `04.01` rather than duplicating its assessment. |
| 12 | Does it duplicate an external standard unnecessarily? | No standard imports a requirement from an external instrument. Every element 17 entry states what the instrument is relied on **for** — usually the existence of a concept — and states that the obligation is PCI's own. | The phrase "it is not the source of this standard's obligation" or its equivalent appears in the limitation column throughout. |
| 13 | Does it misrepresent external authority? | Four risks were identified and closed. ISO 21500 is **context and concepts** since its current edition, not project-management guidance. ISO 45003 is **guidance and nothing can be certified against it**. The Scrum Guide is a **voluntary framework whose adoption is the whole of its force**, not a standard. The EU AI Act and the GDPR are **binding legislation in their own jurisdictions**, named here to illustrate a shape and relied on for nothing. | All four stated expressly at the point of use. No clause number, article, edition or effective date is asserted anywhere; editions are held in the suite register with their verification status, and rows recorded there as **not independently verified** are marked as such in the standard that cites them. |
| 14 | Does it require legal or jurisdiction-specific advice? | Element 18 of all 32 states what needs local advice. The heaviest are `10.01` (procurement regimes), `12.02` (whistleblowing, monitoring, works councils), `14.01` (data protection) and `16.01` (which approvals are legally required, and who may grant them). | `16.01` element 18 states expressly that **the professional must not infer the gate-block list from the examples given** — the examples are the manuscript's, and the list is the owning authorities'. |
| 15 | Does it define the relevant materiality threshold? | Yes, and **no standard in this set invents a percentage.** Element 11 of every standard requires that the organisation's own documented threshold exists and is applied, and states the professional-judgement criteria and the applying role where no number is defensible. Three standards state expressly that a number would be harmful: `12.02` (a concern target invites manufactured concerns), `15.02` (a borrowed utilisation figure is worse than none) and `16.01` (the gate block is binary and admits no threshold at all). | Element 11 of every standard carries a *six-person internal project* line and a *multi-partner national programme* line, which is where the thresholds were tested. |
| 16 | Does it cover AI use? | Elements 14, 15 and 16 appear in all 32 standards. | None needed. |
| 17 | Does it preserve human accountability? | Element 15 of every standard prohibits an AI system from deciding, approving, certifying, signing, waiving, authorising or being recorded as verifying. `01.01`, `01.02` and `14.02` carry the load. | `14.02` element 15 adds that **an AI system must not verify another AI system's output** for the purposes of this set. |
| 18 | Does it contain an exception process? | Element 12 of all 32. **Four standards state that no exception is permitted**, and each gives the reason: `01.03` (accountability for a conflicted decision cannot be waived; escalate instead), `12.01` (the principal obligation), `12.02` (`PR-02`, `PR-04`, `PR-05`) and `16.01` (no exception exists in respect of a mandatory precondition, and none may be sought). | `16.01` element 12 also states the correct remedy where a classification is disputed: have the **owning authority** reclassify it in writing before the assessment. |
| 19 | Does it define escalation? | Element 13 of all 32. The defined term *escalation threshold* requires a named destination **and** a time; `03.02-PR-05` makes encountering one without them an escalable defect in itself. | None needed. |
| 20 | Is every important term defined? | Thirteen compliance-deciding terms are defined at the head of the file, plus nineteen supporting terms. Terms the suite terminology audit records as carrying legitimately different meanings across the three books are **kept apart and flagged**, not collapsed: *sponsor* (delivery sense used; the project-finance sense is PFL-AI's), *baseline* (control sense and benefits sense, both defined), *verification* (V&V sense and AI-assurance sense, both defined), *governance* (project sense and data governance, written in full), and *benefit* against *value* against Earned Value. | The definition of *material* was rewritten twice: it now states that safety, legality, licence and truthfulness matters are material **irrespective of size** and that no documented threshold reduces them. |
| 21 | **Is the language concrete and modern?** | **Zero occurrences of the legislative auxiliary verb** in the file, in any field, checked mechanically. No `may not` is used for a prohibition. No undefined judgement word carries an obligation: *appropriate*, *adequate*, *reasonable*, *timely* and *sufficient* were removed from every obligation or replaced with a stated test. | The Manual section 1 requirement to state the ISO mapping was met **without printing the prohibited word**: the mapping is stated by reference to the auxiliary that ISO/IEC Directives Part 2 reserves for requirements. This is recorded here because it is a deliberate resolution of a tension between two Manual provisions, and the Interpretation Panel should confirm it. |
| 22 | **Does it impose an impossible or excessive burden?** | Tested standard by standard against a six-person internal project. Three drafts were excessive: an earlier `03.01` required four separate approved artefacts on any project of any size; an earlier `10.01` required a three-person panel; an earlier `16.03` required independent measurement in all cases. | `03.01-PR-03` makes proportionate tailoring an express, recorded decision so light governance is a choice rather than an omission; `10.01` element 11 scales to a three-supplier quotation exercise; `16.03` element 10 permits, for an organisation too small to supply independence, measurement taken directly from an unmodified source extract that is retained. **The residual burden is honestly stated:** `01.03`, `12.02` and `16.01` each add real recording work, and each was retained because the failure it prevents is severe. |
| 23 | Can it operate on both small projects and megaprojects? | Yes — element 11 of every standard states both cases explicitly, and this is where the set was most often rewritten. | Recorded in element 11 throughout. |
| 24 | Can it operate internationally? | No standard depends on a single jurisdiction's legal concepts. Element 18 of every standard states what is jurisdiction-specific, and no standard states a legal position. | `12.02` element 18 states expressly that where a national whistleblowing or monitoring regime imposes a higher or more specific obligation, **that regime governs**. |
| 25 | Is there a clear consequence within PCI's authority? | Element 23 of all 32 draws only from Charter section 9. Three standards state expressly what PCI **cannot** do, because those are the three where a reader might otherwise assume a wider power: `10.01` (PCI cannot set aside an award or impose a fine), `12.02` (PCI cannot compensate, order reinstatement or penalise an employer) and `16.01` (PCI cannot authorise a transition or override a regulator). | Added to those three; the remainder rely on the Charter section 9 list, which is stated in full in each. |

### Definitions reconciliation

The red team's structural finding **P-1** — no PCI Standards Definitions Register, so each volume built its
own and seven compliance-deciding terms diverged — has since been closed. The register is published at
[`PCI_STANDARDS_DEFINITIONS_REGISTER.md`](PCI_STANDARDS_DEFINITIONS_REGISTER.md) and *Terms that decide compliance*
above were reconciled to it: *material*, *independent*, *evidence*, *competent reviewer*, *decision
owner*, *escalation threshold* and *conflict of interest* now carry the canonical wording, and
*approved*, *current* and *material AI assistance* were added. **No obligation changed.** Three notes:

- **The four collisions this set flags are preserved, not collapsed** — *baseline* (control versus
  benefits), *sponsor* (delivery versus project-finance), *verification* (V&V versus AI) and
  *governance* (project versus data). Each is carried in the register with both senses and the context
  each belongs to. Forcing one definition on any of them would make this volume wrong, which is worse
  than the divergence.
- ***Competent reviewer* no longer folds independence into competence.** The limb requiring that the
  reviewer did not prepare, direct, specify or approve the thing reviewed is *independence*, imposed by
  each standard's element 10 and tested separately. That separation is what makes `PCI-FND-STD-10` element
  12's supervised-acquisition exception usable, and no element 10 loses an independence requirement.
- ***Material AI assistance* was undefined here** while `PCI-PML-STD-14.02` element 21(d) made
  compliance turn on it. The canonical definition is supplied so the test can be applied; the
  Interpretation Panel should confirm it, because it decides which artefacts must carry the disclosure.

### Open findings carried forward

| Finding | Why it is open | Where it goes |
|---|---|---|
| **Six external-reference rows are recorded as "not independently verified" in the suite register** — ISO 21503, ISO 21504, ISO 21505, ISO 9000, the PMBOK Guide's companion *Code of Ethics and Professional Conduct*, and the AACE TCM Framework. | Charter section 5 Stage 5 was performed against the register, not against the publishers. | Verification before publication; each citing standard states the open status in element 17. |
| ~~**The published foundational file lags the concordance.**~~ **Closed.** | `PCI_FOUNDATIONAL_STANDARDS.md` now carries fifteen standards under the Charter section 3 form `PCI-FND-STD-01` to `PCI-FND-STD-15`, with the subjects listed in *How to read these standards* above. Every citation in this set resolves against the published file itself. | Closed on the foundational rebuild. The superseded `PCI-LAW-F-NN` identifiers survive only as history, in [`STANDARDS_CONCORDANCE.md`](STANDARDS_CONCORDANCE.md). |
| **Charter section 5 Stages 4, 6, 7, 11, 12 and 13 have not been performed.** | Technical review, practitioner consultation, impact assessment, approval, publication and post-implementation review are outstanding. | Element 25 of every standard records the set as **not yet approved**, with the effective date on approval. |
| **The ISO-mapping statement resolves a tension between two Manual provisions.** | Manual section 1 both prohibits the legislative auxiliary everywhere and requires the ISO mapping to be stated. This file states the mapping by describing the auxiliary rather than printing it. | Interpretation Panel confirmation under Charter section 6. |

---

## Index of PML-AI Professional Standards

**Thirty-two standards · one hundred and forty-eight process requirements · sixteen anchor domains.**
External-reference categories are Drafting Manual section 6 categories: **3** international voluntary standard ·
**4** contract framework · **5** professional framework · **6** ethical code · **10** illustrative practice.
Every standard also relates to category **9**, PCI internal professional standard, through its element 19, and that
category is not repeated in the table.

| ID | Official title | Anchor domain | Principal obligation | External reference categories |
|---|---|---|---|---|
| `PCI-PML-STD-01.01` | Leadership Accountability for Delivery Decisions | D1 — The project leadership profession | Remain personally accountable for every delivery decision taken under one's authority, including AI-informed ones | 3, 10 |
| `PCI-PML-STD-01.02` | Reserved Delivery Decisions and the Named Human Decider | D1 | Ensure a decision in a reserved class is taken by the named human on evidence they examined | 3, 10 |
| `PCI-PML-STD-01.03` | Interests, Abstention and Assurance Independence | D1 | Do not decide, advise on, evaluate or assure a matter in which one holds a conflict of interest | 3, 6, 10 |
| `PCI-PML-STD-02.01` | Business-Case Integrity | D2 — Strategy, selection and business alignment | Do not present, endorse or rely on a materially misstated business case | 3, 5 |
| `PCI-PML-STD-02.02` | Benefits Ownership | D2 | Do not allow a benefit to be claimed unless a named individual outside delivery has accepted it in writing | 3 |
| `PCI-PML-STD-03.01` | Governance Authority Before Commitment | D3 — Governance, organisation and decision rights | Do not commit funds, contracts or people before governance is documented and approved | 3, 10 |
| `PCI-PML-STD-03.02` | Decision Rights and Delegated Authority | D3 | Take every decision at the authority the delegation schedule assigns; do not split, defer or aggregate to avoid it | 3, 5, 10 |
| `PCI-PML-STD-03.03` | Gate Evidence and the Gate Decision | D3 | Do not take a gate decision except on dated, attributable, versioned evidence against criteria published first | 3 |
| `PCI-PML-STD-03.04` | Sponsor Accountability | D3 | Do not lead delivery with no named individual sponsor who has accepted the accountability in writing | 3, 10 |
| `PCI-PML-STD-04.01` | Change Authority and Integrated Change Control | D4 — Integration and delivery architecture | Do not let a baseline change take effect before integrated assessment and approval at the assigned authority | 3, 4, 5 |
| `PCI-PML-STD-05.01` | Scope Integrity | D5 — Scope, requirements and value definition | Do not allow work outside the approved scope baseline without an approved change | 3, 5 |
| `PCI-PML-STD-05.02` | Requirements Traceability | D5 | Maintain traceability from every approved requirement to source, to satisfying work, and to proving test | 3, 5 |
| `PCI-PML-STD-06.01` | Schedule Credibility | D6 — Planning, scheduling and delivery flow | Do not issue or rely on a schedule that misrepresents achievable completion | 3, 5 |
| `PCI-PML-STD-07.01` | Cost Stewardship | D7 — Cost, resources and commercial awareness | Report the cost position — committed, incurred, accrued, forecast — completely, currently and reconcilably | 3, 5 |
| `PCI-PML-STD-07.02` | Resource Decisions and the Commitment of People | D7 | Do not commit a person or shared resource to a plan without the named resource owner's agreement | 3, 5 |
| `PCI-PML-STD-08.01` | Risk Escalation | D8 — Risk, uncertainty and resilience | Escalate a risk meeting the documented threshold, to the named authority, within the stated time | 3 |
| `PCI-PML-STD-08.02` | Issue Management | D8 | Record every issue with one named owner, a required date and a stated consequence; close only with a resolution | 3 |
| `PCI-PML-STD-09.01` | Quality Acceptance | D9 — Quality, assurance and continuous improvement | Do not record acceptance unless a named authority decided conformity against pre-dated criteria | 3 |
| `PCI-PML-STD-09.02` | Lessons Learned and Organisational Retention | D9 | Convert each accepted lesson into a change to a named standing artefact, with an owner and a date | 3 |
| `PCI-PML-STD-10.01` | Procurement Fairness | D10 — Procurement, contracts and supply networks | Evaluate only against criteria and weightings published to bidders before submission | 3, 4, 6 |
| `PCI-PML-STD-11.01` | Stakeholder Transparency | D11 — Stakeholders, communication and influence | Do not issue or let stand a delivery report that omits a material adverse fact known at issue | 3, 5 |
| `PCI-PML-STD-12.01` | Leadership Conduct | D12 — Leadership, teams and organisational behaviour | Do not use authority to cause another person to state, or withhold, what they have recorded as inaccurate | 3, 6 |
| `PCI-PML-STD-12.02` | Route to Raise a Concern and Freedom from Detriment | D12 | Establish, publish and operate a concern route to a named recipient outside the subject's line | 3 |
| `PCI-PML-STD-13.01` | Governance of Adaptive Delivery | D13 — Agile, adaptive and hybrid delivery | Operate governance producing the same decision rights, evidence and accountability through adaptive artefacts | 3, 5 |
| `PCI-PML-STD-13.02` | Product and Project Accountability | D13 | Ensure exactly one named holder of the ordering right and one of delivery accountability, both recorded | 3, 5 |
| `PCI-PML-STD-14.01` | Responsible Data Use in Delivery | D14 — Digital delivery, data and responsible AI | Do not collect, use, share, retain or expose data beyond its recorded purpose, recipients and retention | 3, 10 |
| `PCI-PML-STD-14.02` | Responsible AI in Delivery | D14 | Do not rely on an AI output until a named human has verified it by a method recorded for that output class | 3, 10 |
| `PCI-PML-STD-15.01` | Programme Integration and Dependency Ownership | D15 — Programmes, portfolios and enterprise delivery | Record every inter-component dependency with a named giver, receiver, thing, date and breach consequence | 3 |
| `PCI-PML-STD-15.02` | Portfolio Prioritisation and Capacity Truth | D15 | Do not let the portfolio hold more concurrent work than assessed delivery capacity supports | 3 |
| `PCI-PML-STD-16.01` | Transition Readiness and the Gate Block | D16 — Transition, closeout and benefits realisation | Do not permit transition while any mandatory precondition is recorded not met | 3 |
| `PCI-PML-STD-16.02` | Operational Acceptance and Handover | D16 | Do not close a project until a named individual in the receiving organisation has accepted operational responsibility | 3 |
| `PCI-PML-STD-16.03` | Benefits Measurement | D16 | Do not state a benefit is realised except on the recorded method, source and pre-change benefits baseline | 3 |

### Distribution

Every one of the sixteen PML-AI domains carries at least one standard. Domain 3 carries four, because
governance is where decision rights, gate evidence and sponsorship are taught; Domain 16 carries three,
because transition, operational acceptance and benefits measurement are three separate decisions that
fail in three separate ways; Domains 1, 2, 5, 7, 8, 9, 12, 13, 14 and 15 carry two or three; Domains 4,
6, 10 and 11 carry the single standard their material makes mandatory.

**Eight standards are new in this edition** — `01.03`, `03.04`, `07.02`, `08.02`, `13.02`, `15.01`, `15.02`
and `16.02` — and two were rebuilt because they could not be verified: `12.01` and `12.02`. `01.02` was
retitled to end a collision with the foundational standard *Human Decision Authority*, and `09.02` was
retitled and given a new principal obligation. The remainder were renumbered, restructured to the
twenty-five-element form,
re-drafted in must-form, split into one principal obligation plus process requirements, and given a
compliance test that can actually be performed.

Every standard in this set operates under the Foundational Standards `PCI-FND-STD-01` to `PCI-FND-STD-15`, under
the **PCI Standards Charter** and the **PCI Standards Drafting Manual**, and under one principle:

> **AI proposes; the professional verifies, decides and remains accountable.**
