# PFL-AI Professional Laws — PCI AI Project Finance Leader

**Status:** Certification Laws for the **PCI AI Project Finance Leader** credential (PFL-AI).
Version 2.0 — a complete reconstruction of the twenty-four-law v1.0 set onto the twenty-five-element
structure required by the **PCI Law Drafting Manual** §5. **Thirty-three laws** and **one hundred and
one process requirements**, anchored to the sixteen-domain PFL-AI Body of Knowledge
(`../pfl-ai/`).

**Governing instruments:** the [`PCI Professional Laws Charter`](PCI_PROFESSIONAL_LAWS_CHARTER.md)
(what a law is) and the [`PCI Law Drafting Manual`](PCI_LAW_DRAFTING_MANUAL.md) (how a law is
written). A law that does not conform to both does not pass gate. Where this volume and either
instrument differ, the instrument prevails and the difference is a defect in this volume.

---

## Legal and professional status

The following statement is required by Charter §2 in every publication that contains or cites a PCI
Law, and is reproduced here in full:

> **PCI Professional Laws are private professional certification requirements established by Project
> Controls Institute Global. They are not legislation, government regulation, legal advice or
> substitutes for applicable laws, contractual obligations, regulatory requirements or authoritative
> professional standards. Where an applicable legal, regulatory, contractual or authoritative
> requirement imposes a higher or different obligation, that requirement prevails.**

PCI's authority is confined to its own processes: examination, certification, quality and conduct.
No law in this volume asserts or implies that PCI can impose a fine, create criminal or civil
liability, or exercise any governmental enforcement power. **Nothing in this volume is legal, tax,
accounting, insurance or investment advice**, and no law here determines any question of religious
law (see `PCI-PFL-LAW-09.02`).

---

## How to read these laws

### The requirement form, and the ISO mapping

PCI uses **modern must-drafting**, exclusively (Manual §1).

| Word | Force in a PCI Law |
|---|---|
| **must** | Mandatory PCI professional requirement — the obligation itself |
| **must not** | Prohibited practice; doing it is a breach |
| **should** | Recommended practice only; a justified, recorded alternative may be acceptable |
| **may** | Permission |
| **can** | Capability or possibility — never permission |

**The ISO mapping, for readers who work to ISO/IEC drafting conventions.** Those conventions reserve
a particular modal auxiliary to mark a requirement, and a reader trained on them may misread **must**
as an external constraint rather than as the requirement itself. That reading is wrong here. **In a
PCI Law the requirement form is `must`, and it carries exactly the force an ISO/IEC document places
on its own requirement auxiliary.** `should` marks recommendation in both systems; `may` marks
permission in both; `can` marks capability in both and permission in neither.

PCI does not use the ISO requirement auxiliary anywhere in a law, in any element, including in
quotations of PCI's own earlier drafts — it has been read as both obligation and futurity, and a
draft containing it fails gate (Manual §1). **That word is therefore deliberately not printed
anywhere in this volume**, including in this paragraph, which is why the mapping above is expressed
by description rather than by example. Version 1.0 of this set used it in twenty-two of its
twenty-four laws, and one law — `PFL-LAW-06-05`, Model Change Governance — carried four of its five
sub-obligations on it after a red-team revision. Every one of those obligations is re-expressed on
`must` in `PCI-PFL-LAW-06.05` below.

### Identifiers

| Instrument | Form | Example |
|---|---|---|
| Certification / Domain Law | `PCI-PFL-LAW-DD.NN` | `PCI-PFL-LAW-10.03` |
| Process Requirement (mandatory, Charter Level 4) | `<parent>-PR-NN` | `PCI-PFL-LAW-10.03-PR-02` |
| Recommended Practice (not mandatory, Charter Level 5) | `<parent>-RP-NN` | `PCI-PFL-LAW-15.02-RP-01` |

`DD` is the two-digit PFL-AI Body of Knowledge domain of primary anchorage; `NN` is a two-digit
sequence within that domain. Cite by identifier, never by page. **Every identifier in this volume is
new**: v1.0 used the form `PFL-LAW-DD-NN`, which the Charter and Manual superseded, and the whole set
is renumbered. The v1.0 identifier each law supersedes is recorded in its element 25. A withdrawn
identifier is never reused.

**Process requirements are mandatory.** They sit at Charter Level 4, are breached independently of
their parent law, and are assessed independently. They exist so that each law's element 1 carries
**one** principal obligation and every remaining obligation still has an identifier, a subject, an
action, an object and a test of its own (Manual §2).

### These laws sit under the Foundational Laws

The foundational set binds every PCI credential holder, including every holder of the PCI AI Project
Finance Leader credential. The Charter and Manual give it the identifier form `PCI-FND-LAW-NN`; the
published file [`PCI_FOUNDATIONAL_LAWS.md`](PCI_FOUNDATIONAL_LAWS.md) still carries the earlier form
`PCI-LAW-F-NN`. **This volume cites the Charter form and publishes the mapping**, so that every
cross-reference resolves to a law that actually exists:

| Charter form (cited here) | Published form | Title |
|---|---|---|
| `PCI-FND-LAW-01` | `PCI-LAW-F-01` | Professional Accountability and the Suite Principle |
| `PCI-FND-LAW-02` | `PCI-LAW-F-02` | Verification of AI Output Before Professional Use |
| `PCI-FND-LAW-03` | `PCI-LAW-F-03` | Human Decision Authority |
| `PCI-FND-LAW-04` | `PCI-LAW-F-04` | Disclosure of Material AI Assistance |
| `PCI-FND-LAW-05` | `PCI-LAW-F-05` | Evidence and the Audit Trail |
| `PCI-FND-LAW-06` | `PCI-LAW-F-06` | Data Lineage and Integrity |
| `PCI-FND-LAW-07` | `PCI-LAW-F-07` | Honesty in Reporting and Forecasting |
| `PCI-FND-LAW-08` | `PCI-LAW-F-08` | Competence Boundaries and Referral |
| `PCI-FND-LAW-09` | `PCI-LAW-F-09` | Confidentiality and Information Protection |
| `PCI-FND-LAW-10` | `PCI-LAW-F-10` | Conflict-of-Interest Disclosure |
| `PCI-FND-LAW-11` | `PCI-LAW-F-11` | Duty to Escalate |
| `PCI-FND-LAW-12` | `PCI-LAW-F-12` | Record Retention |
| `PCI-FND-LAW-13` | `PCI-LAW-F-13` | Ethical Conduct Toward Candidates, Employers and the Public |
| `PCI-FND-LAW-14` | `PCI-LAW-F-14` | No Misrepresentation of PCI Credentials or Accreditation Status |
| `PCI-FND-LAW-15` | — | **Unallocated at this revision.** |

**`PCI-FND-LAW-15` is not cited anywhere in this volume**, because the foundational set contains
fourteen laws and citing an identifier with no law behind it would be a false cross-reference. The
identifier is reserved, not used.

**A certification law must add.** Every law below carries, in element 19, an explicit statement of
what it requires that its foundational parent does not. A certification law that merely restates a
foundational law is a drafting defect and is withdrawn, not published.

### External references

Every external instrument is **named and characterised in PCI's own words, never reproduced**, and
classified as exactly one of the ten categories in Manual §6. Each reference records the issuing
organisation, the title, the subject, what was checked and when, the nature of the authority, and the
limitation on its applicability. **No clause number, article, edition, effective date or judicial
decision is asserted unless it was verified**, and where a precise provision was not verified the
instrument is cited by name only. Reference rows are cross-keyed to the suite register
[`../registries/EXTERNAL_AUTHORITIES.md`](../registries/EXTERNAL_AUTHORITIES.md) by its `EXT-` ID, so
a reader can audit the classification against the register's own entry.

**No endorsement, affiliation or accreditation is claimed or implied.** Naming an instrument means
only that it exists and is relevant to the subject under discussion. No standards body, professional
institute, government, supervisory authority, export credit agency, Shariah supervisory board,
verifier, second-party opinion provider or financial institution has reviewed, approved, endorsed or
accredited these laws, the PCI AI Project Finance Leader credential or PCI Global.

### Charter §5 due-process record for this edition

Charter §5 requires each law's file to record honestly which of the thirteen stages were performed
and by whom, **including where a stage was performed with AI assistance rather than by a named
human**. For this edition, across the whole set:

| Stage | Performed? | By whom |
|---|---|---|
| 1 Problem definition · 2 Drafting instruction | Yes | PCI drafting brief, human-authored |
| 3 Initial draft | Yes | **AI-assisted drafting** against the brief, the Charter and the Manual |
| 4 Technical review | Partial — internal consistency and arithmetic-discipline review only | AI-assisted; **no named subject specialist has signed** |
| 5 Standards and legal-characterisation review | Partial — every reference checked against the suite register | AI-assisted; **no qualified counsel has reviewed** |
| 6 Practitioner consultation | **Not performed** | — |
| 7 Impact assessment | Partial — burden and small-organisation impact considered per law (element 11) | AI-assisted |
| 8 Scenario testing | Yes — each law tested against a small municipal project and a multi-billion cross-border financing | AI-assisted |
| 9 Red-team challenge | Yes — the twenty-five audit questions, recorded in the findings table below | AI-assisted |
| 10 Revision | Yes | AI-assisted |
| 11 Approval · 12 Publication · 13 Post-implementation review | **Not performed** | — |

**This edition is therefore a draft for approval, not an approved law set**, and every element 25
says so. Charter §5 forbids publishing a law solely because it was drafted and appeared reasonable,
including where it was drafted with AI assistance; that prohibition governs this volume.

### The suite principle

> **AI proposes; the professional verifies, decides and remains accountable.**

---

## Definitions

Every term below could alter whether a professional has complied, so each is defined by a test a
reader can apply — what makes it so, measured against what, decided by whom (Manual §4). **No
definition below is circular.** Terms are used in these senses throughout, and a law that needs a
narrower sense defines it in its own element 4.

### Core compliance-deciding terms

**material** — A difference, item, omission or fact is *material* when, applied to the output in
question, it would change a decision that a reader of that output is entitled to make on it. The
test is stated **before** the work begins, in the transaction's own metric, and is recorded in the
engagement's materiality statement: for a coverage-ratio output, a stated movement in the ratio; for
a funding output, a stated cash amount against the funding requirement; for a claim, whether the
supporting evidence would still support it. **PCI sets no percentage.** The *decision owner* sets the
figure, the finance documents override it wherever they state their own, and a materiality statement
that names no figure and no metric is not a materiality statement.

**independent** — Of a person, in relation to a defined piece of work: that person (a) did not
prepare the work or any part of it, (b) is not in the preparer's reporting line and does not report
to a person whose performance is measured by the work's conclusion, (c) receives no fee, bonus,
continuing mandate, success payment or other benefit that varies with the conclusion reached, and
(d) holds no financial interest in the transaction or in a party to it. All four limbs are required.
Independence is a fact about a relationship, not a state of mind, and asserting objectivity does not
establish it.

**verified** — Of a figure, statement, extraction or machine output: a named human has checked it
against the source that determines it, by a **named method** — independent recomputation, source
tracing, clause-to-output comparison, reconciliation, sampling on a stated basis, boundary testing,
or sensitivity analysis — and has recorded the method, the source, the result and the date. Reading
an output and finding it plausible is not verification. (*Collision flag:* the suite uses *verify* in
this AI-assurance sense throughout, not in the engineering verification-and-validation sense recorded
in `../registries/TERMINOLOGY_AUDIT.md` §2.2.)

**evidence** — A dated, retrievable record, attributable to an identified author or issuing party,
which a person other than its author can retrieve and use to reach the same conclusion. An
unattributed file, an undated extract, a recollection and a screenshot with no source are not
evidence for the purposes of these laws.

**decision owner** — The single named individual accountable for the decision that an output
supports. Accountability is held by one person, is not delegable, and is recorded before the output
is relied upon. "The team", "management", "the sponsor", "the lenders" and "the organisation" do not
name a decision owner.

**competent reviewer** — A named individual who satisfies all three of: demonstrated experience in
the specific subject matter under review, recorded before the review; *independence* as defined
above in relation to the work reviewed; and written authorisation from the engaging organisation to
record a review conclusion on that subject. Seniority alone does not make a competent reviewer, and
neither does availability.

**escalation threshold** — The stated condition, recorded before the work begins, on whose
occurrence a matter must be passed to a named higher authority. Thresholds are set by the finance
documents where those documents state them, and otherwise by the adopting organisation's governance.
PCI does not set them; PCI requires that they exist, are written down, name the authority, and are
applied.

### Transaction terms

**finance documents** — The executed agreements that govern the financing and the security for it,
together with their schedules, side letters and amendments. Before execution, the term means the
agreed final-form documents, and a law that turns on the finance documents applies to the agreed
final form with the fact of non-execution stated.

**CFADS** *(cash flow available for debt service)* — The cash amount that the finance documents
define as available to meet *debt service* in a period. **CFADS is a defined term of a transaction,
not a market constant**: two financings of the same asset can define it differently and both be
right. Where no finance documents yet exist, the modelled definition must be written out in full,
item by item, and labelled as the modeller's definition.

**debt service** — The amounts the finance documents require to be paid to the finance parties in a
period, comprising the items those documents name — typically scheduled principal and interest, and,
where the documents include them, fees, hedging settlements and equivalent amounts. The documents'
list governs; a conventional list does not.

**coverage ratio** — A ratio of *CFADS* (or of its present value) to *debt service* (or to debt
outstanding), computed on the definitions the finance documents state. `DSCR` is the period test;
`LLCR` is the loan-horizon test; `PLCR` extends to the end of project life. (*Collision flag:*
`coverage` also carries a risk-identification sense in the suite — the share of an estimated risk
population actually identified. This volume uses the credit sense only, and writes *coverage ratio*
in full wherever ambiguity is possible.)

**reserve account** — An account the finance documents require to hold a stated balance or to be
funded to a stated level, whose balance is available only for the purposes and in the order those
documents specify.

**distribution** — Any transfer of value out of the borrower group to a shareholder or a
shareholder's affiliate, in any legal form — dividend, return of capital, redemption, payment of
shareholder-loan interest or principal, management or development fee, or payment for goods or
services outside the ordinary course on arm's-length terms — that the finance documents treat as a
restricted payment.

**lock-up** — The state in which the finance documents prevent a *distribution* because a stated
test has not been met at a stated date, without an event of default necessarily having occurred.
Lock-up is a trap on cash, not a default, and the two must not be reported as the same event.

**conditions precedent** — The deliverables and states of affairs that the finance documents require
to exist, in the form and to the satisfaction those documents specify, before a stated event may
occur — effectiveness, first drawing, a subsequent drawing, a release from a reserve, or completion.

**waiver** — A finance party's consent, given in the form the finance documents require, to
disregard a specific breach or unsatisfied condition, on stated terms and for a stated period.

**amendment** — A change to the terms of the finance documents, executed in the form those documents
require. A waiver is not an amendment: it leaves the term in place.

**cost-to-complete (`CTC`)** — The cash cost expected to be incurred from a stated date to complete
the works and reach the completion test the finance documents specify, computed from certified
progress, committed and uncommitted scope, notified and assessed claims, escalation and the
remaining programme. (*Symbol discipline:* this volume writes `CTC` and never `EAC`.
`../registries/TERMINOLOGY_AUDIT.md` Issue 1 records that `EAC` carries two unrelated formulas inside
the PFL-AI corpus — estimate at completion, and equivalent annual cost — so this volume uses neither
`EAC` nor bare `PV`; present value is written in words or as `PV(x)`.)

**sources and uses** — The statement setting the project's total funding requirement against the
committed funding that meets it, whose two totals are equal by construction and whose inequality is
therefore always an error or an unfunded amount.

**funds flow** — The statement of every payment to be made at, or in connection with, a closing or a
drawing, showing payer, payee, account, amount, purpose and order, reconciled to the *sources and
uses*.

**base case** — The agreed model case that the finance documents identify as the case against which
the transaction was sized and agreed. (*This volume does not use the word* baseline, *which collides
across two senses in the suite — see* `../registries/TERMINOLOGY_AUDIT.md` §2.3.)

### Modelling and governance terms

**financial model** — A computational representation of a project's cash flows, financing and
returns, used to inform, support or evidence a decision.

**model owner** — The single named individual accountable for a financial model's integrity, its
version control and its release. A tool, a team, a mailbox and a folder cannot be a model owner.

**authoritative version** — The one identified file or record of a model, register or document set
that the parties are entitled to rely on at a stated date, bearing a version identifier, held under
the *model owner's* control, and distinguishable from every other copy by that identifier. Where two
files bearing the same identifier differ in content, neither is the authoritative version until the
model owner determines which is and records the determination.

**decision-grade** — Of an output: presented to, or relied on by, any person for a financing,
investment, credit, certification, distribution, reporting or approval decision. An output that is
labelled indicative but is in fact relied upon for such a decision is decision-grade.

**source line** — A citation attached to a figure that identifies the document determining it, that
document's version or date, and the party that issued it. A source line that names a file path, a
person's recollection or another model is not a source line.

**AI-assisted work** — Any work in which an artificial-intelligence system generated, extracted,
drafted, computed, summarised, classified, translated or checked content that is carried into an
output, whether or not a human edited it afterwards.

### Terms in the two areas of highest misdescription risk

**Shariah compliance determination** — A ruling on whether a structure, instrument or transaction
conforms to Islamic law, issued by the scholar, Shariah supervisory board or equivalent body that the
relevant institution, market or jurisdiction recognises as competent to issue it. (*Transliteration:*
this volume writes **Shariah**, following the PFL-AI manuscript; the form *Sharia* is the same word.)
**A determination is a matter of religious law, and PCI has no authority of any kind over it** — PCI
does not make, review, endorse, accredit or overrule such a determination, and no PCI law, credential,
examination or process confers competence to make one.

**sustainability claim** — A statement that an instrument, an asset, a project, an expenditure or a
performance metric qualifies under a named environmental, social, climate or sustainability label,
taxonomy, framework, principle set or target — including a green, social or sustainability-linked
label, a taxonomy-alignment assertion, and any environmental, social or governance metric quoted
outside the model that produced it.

**voluntary framework** — A framework whose whole force is that an organisation chooses to adopt it;
an organisation may lawfully decline to adopt it. **A voluntary framework must never be described as
legislation, regulation or law** (Charter §2; Manual §6). Where a jurisdiction has enacted a
taxonomy, labelling or disclosure regime, that regime is legislation *in that jurisdiction only*, its
applicability to a given entity is a question for qualified local counsel, and it does not make the
voluntary framework it resembles into law anywhere.

---
## Domain 1 — Foundations of project finance leadership

### PCI LAW PCI-PFL-LAW-01.01 — Cash-Flow Integrity in Financial Judgement

**1. Normative requirement.** A credential holder must not present an accounting result, an earnings
measure, an averaged ratio or a period-aggregated figure as evidence that a project can meet an
obligation on the date it falls due.

**2. Purpose.** A financing fails on the day the cash is not in the account, whatever the reported
profit. Accrual results, non-cash charges, revaluations, annualised averages and horizon choices can
each present a comfortable picture over an account that will be empty on the payment date, and the
reader of the output usually cannot see which of them is doing the work.

**3. Scope.** Every credential holder who prepares, reviews, recommends, approves or provides
assurance on an appraisal, forecast, funding plan, coverage assessment, liquidity statement, lender
report, credit submission or board paper, on any asset class and any financing structure, from
screening to handback. It applies to preparation, review, recommendation, approval and assurance
alike.

**4. Defined terms.** *material*, *evidence*, *decision owner*, *CFADS*, *debt service*, *coverage
ratio*, *decision-grade*, *verified*.

**5. Required actions.**

- **PCI-PFL-LAW-01.01-PR-01 — Dated obligation schedule.** The preparer must produce, for every
  decision-grade output that speaks to ability to pay, a schedule of each obligation against the date
  it falls due, sourced to the document that creates it.
- **PCI-PFL-LAW-01.01-PR-02 — Accrual-to-cash reconciliation.** The preparer must reconcile the
  accounting result to the cash line used, and must present the non-cash adjustments as a separate,
  itemised schedule rather than as a net figure.
- **PCI-PFL-LAW-01.01-PR-03 — Payment-date liquidity test.** The preparer must test liquidity at each
  payment date in the tested horizon, and must identify the binding date — the earliest date at which
  available cash is least in excess of the obligation then due.
- **PCI-PFL-LAW-01.01-PR-04 — Presentation-dependency disclosure.** The preparer must disclose, on
  the face of the output, every respect in which a favourable presentation depends on a non-cash
  item, a timing convention, a horizon choice or an averaging basis.

**6. Prohibited actions.** Offering an earnings measure or accounting profit as evidence of ability
to pay on a date; netting a dated cash shortfall against an unrealised or non-cash gain; re-phasing
or smoothing timing to remove a shortfall from view; describing an accrued or contracted receipt as
available cash; quoting an average where a period test governs.

**7. Required evidence.** The dated cash-flow forecast with each obligation on its due date; the
accrual-to-cash reconciliation; the itemised non-cash schedule; the payment-date liquidity test with
the binding date identified; the presentation-dependency disclosure; the *decision owner's* recorded
approval.

**8. Responsible role.** The project finance leader accountable for the funding plan prepares and
signs; the *decision owner* for the decision the output supports accepts it.

**9. Approval authority.** The decision owner approves the output for use. No exception to this law
may be approved below the level of the person who would have approved the decision it supports.

**10. Independence requirement.** Independence of preparation is required for the review under
element 21 wherever the output supports sanction, financial close, a drawing, a *distribution*, a
covenant certificate or a waiver request. Elsewhere, review by a *competent reviewer* who did not
prepare the schedule is sufficient.

**11. Materiality or threshold.** The threshold is a cash amount and a date, not a percentage. A
shortfall is *material* when available cash at a payment date is less than the obligation then due by
any amount, because a payment either is made or is not; a *headroom* disclosure threshold is set by
the *decision owner* in the engagement's materiality statement, in the transaction's own metric.
*Scale test:* on a small municipal project with one annual instalment and a single account, the
required schedule is one page and the test is arithmetic; on a multi-billion cross-border financing
with multiple currencies, tranches and payment dates, the same test is applied per currency and per
account, and the binding date is identified per obligation group.

**12. Exception and waiver.** No exception is permitted to the prohibition in element 1. A departure
from the *form* of any process requirement — for example, presenting the reconciliation in an
appendix rather than on the face — may be approved in writing by the *decision owner*, for one named
output, for a stated period not exceeding the life of that output, on condition that the substance is
delivered in full and the departure is recorded in the output. An undocumented departure is a breach.

**13. Escalation trigger.** A forecast showing an obligation that cannot be met from available cash at
its due date, on any case run; an instruction to answer a solvency, funding or coverage question on
an accounting basis in place of a cash basis; discovery that a presented figure depended on an
undisclosed timing or averaging convention.

**14. AI application.** AI may build and re-phase cash-flow projections, reconcile accrual results to
cash, detect timing mismatches between obligations and receipts, generate liquidity stress runs, and
draft the presentation-dependency disclosure for human confirmation.

**15. AI prohibition.** AI must not conclude that a project is solvent, adequately funded or able to
meet an obligation; must not approve a funding plan or a liquidity position; and must not be recorded
as the author or approver of any schedule required by this law.

**16. AI verification.** Independent recomputation of the cash available at the binding date and at
one other tested date, from *source lines*, by a named human; source tracing of every obligation date
to the document that creates it; and reconciliation of the AI-produced accrual-to-cash bridge to the
*authoritative version* of the model and to the financial statements. Each check is recorded with its
method and date.

**17. External reference.**

- **IFRS Foundation — *Conceptual Framework for Financial Reporting*.** Issuing organisation: IFRS
  Foundation / IASB. Subject: the accrual basis and faithful representation, which is precisely what
  makes an accounting result a different statement from a cash fact. Checked: named without clause,
  edition or date (register `EXT-011`, verified 2026-08-03). Nature: **authoritative
  financial-reporting material that is expressly not a standard** — the IASB states that nothing in it
  overrides a Standard. Manual §6 records it separately for that reason. Applicability limitation:
  **no requirement in this law is sourced to it**; it is named to explain a distinction, and it
  creates no obligation for anyone.
- **IAS 7 *Statement of Cash Flows*.** Issuing organisation: IFRS Foundation / IASB. Subject: the
  presentation and classification of cash movements against which a modelled cash line is reconciled.
  Checked: current, by name only, no clause asserted (register `EXT-120`, verified 2026-08-03).
  Nature: Manual §6 category 2 — authoritative financial-reporting standard. Applicability
  limitation: mandatory only for entities applying IFRS Accounting Standards in a jurisdiction that
  has adopted them; it defines no *coverage ratio* and no liquidity test. Verify current requirements.

**18. Jurisdictional caution.** Solvency, going-concern, distributable-reserve, wrongful-trading and
insolvency tests are matters of local company and insolvency law and are **not** established by a
cash forecast. The consequences for directors and officers of continuing to trade, or of making a
payment, differ by jurisdiction and can be personal. Obtain local legal and accounting advice before
any statement about an entity's ability to continue or to pay.

**19. Related PCI Laws.** `PCI-FND-LAW-07` (honesty in reporting and forecasting);
`PCI-FND-LAW-05`; `PCI-PFL-LAW-10.01`; `PCI-PFL-LAW-10.03`; `PCI-PFL-LAW-14.02`;
`PCI-PFL-LAW-15.01`. **Increment over the foundational parent:** `PCI-FND-LAW-07` requires honest
reporting generally; this law fixes *what honesty means in a financing* — the obligation is dated, the
test is at the payment date, the reconciliation is itemised, and the averaging convention is
disclosed on the face of the output.

**20. Related Body of Knowledge content.** PFL-AI · Domain 1 — Foundations of project finance
leadership · KA 1.2 Value, cash and risk · topic 1.2.2 cash as the binding constraint. Also Domain 2
KA 2.1–2.2 (the accrual model; the three statements and their articulation) and Domain 10 KA 10.1
(debt capacity and sizing).

**21. Compliance test.** A reviewer takes the output, the model *authoritative version*, the
obligation documents and the financial statements, and performs four steps. (a) Recomputes available
cash at the binding date from the model's own source lines and obtains the figure stated in the
output, without unexplained difference. (b) Traces every obligation date in the schedule to the
document that creates it, with no date unsourced. (c) Adds the itemised non-cash adjustments to the
cash line and reaches the accounting result stated, without unexplained difference. (d) Confirms that
every averaging, horizon or timing convention on which a favourable figure depends appears in the
presentation-dependency disclosure. Compliance is demonstrated when all four steps complete; failure
of any one is a breach.

**22. Breach indicators.** A coverage or liquidity conclusion with no dated obligation schedule
behind it; a reconciliation presented as a single net "non-cash" line; an annual average quoted where
the finance documents test a period; a forecast whose worst date is not identified anywhere; a
favourable figure whose horizon changed between drafts without a recorded reason.

**23. Consequence within PCI authority.** Correction required and the affected output withheld from
use until corrected; additional independent review; escalation to the decision owner; failure of the
associated examination competency; ethics review; certification investigation; suspension or
withdrawal of the credential. Each is subject to due process and a right of appeal (Charter §9). PCI
claims no other consequence.

**24. Examination application.** Scenario judgement: a profitable-looking forecast conceals a dated
liquidity shortfall and the candidate must identify the binding date and the compliant presentation.
Calculation review: a candidate is given a cash line and an accrual result and must produce the
reconciliation. Evidence selection: from a document set, the candidate selects what proves ability to
pay on a date. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 (stage record in the
front matter; Stages 6, 11, 12 and 13 not performed) · effective on approval · supersedes
`PFL-LAW-01-01` *Cash Before Appearance* (v1.0). Amendment note: restructured onto the
twenty-five-element form; the single bundled rule split into one principal obligation and four
process requirements; the compliance test made performable; threshold restated as a cash-and-date
test rather than a judgement phrase.

---

### PCI LAW PCI-PFL-LAW-01.02 — Conflict Disclosure and the Two-Hat Rule

**1. Normative requirement.** A credential holder must disclose in writing, before advising,
modelling, negotiating, reviewing or approving on a financing, every interest of theirs or of a
connected person that a reasonable party to that financing would want to know when weighing their
judgement.

**2. Purpose.** Project finance concentrates advice: the same individual or firm can sit close to
sponsor, lender, grantor, contractor and offtaker on one transaction. An undisclosed interest converts
advice into advocacy while preserving the appearance of objectivity, and the parties relying on it
cannot see that the change has happened.

**3. Scope.** Every credential holder advising, modelling, structuring, reviewing, lending,
approving, certifying or negotiating on a project financing, in-house or as an external adviser,
including their personal financial interests and those of connected persons where these relate to the
transaction, its parties or its competitors. Applies from first contact through to the end of the
engagement and to any later re-engagement.

**4. Defined terms.** *independent*, *evidence*, *decision owner*, *material*, *finance documents*.
**Connected person** — for this law, a person whose financial position a reasonable party would treat
as affecting the credential holder's judgement: a spouse or partner, a dependent, a person sharing a
household, an entity the credential holder or any of them controls or in which any of them holds a
financial interest, and the credential holder's employer and its group.

**5. Required actions.**

- **PCI-PFL-LAW-01.02-PR-01 — Standing conflicts register.** The credential holder must maintain a
  standing register of their interests and those of connected persons, dated, and must update it
  whenever an interest arises, changes or ends.
- **PCI-PFL-LAW-01.02-PR-02 — Screening before acceptance.** The credential holder must screen every
  new instruction against the register and against the parties to the transaction before accepting it,
  and must record the screen and its outcome.
- **PCI-PFL-LAW-01.02-PR-03 — Written consent or declinature.** Where an interest is disclosed, the
  credential holder must obtain the informed written consent of every affected party before beginning
  or continuing the work, or must decline the engagement; consent obtained on an incomplete
  disclosure is not consent.
- **PCI-PFL-LAW-01.02-PR-04 — Information-barrier testing.** Where a conflict is managed by an
  information barrier rather than by declining, the credential holder must record the barrier's terms
  and must have its operation tested at a stated interval by a person *independent* of both sides,
  with the test result recorded.

**6. Prohibited actions.** Acting for more than one side of the same question without informed
written consent from every affected party; accepting an undisclosed fee, commission, referral payment
or success-linked benefit from a party other than the client; using transaction information for
personal or connected benefit; describing advice, a review or a model as *independent* while holding
an undisclosed interest; delaying a disclosure until after a decision is taken.

**7. Required evidence.** The conflicts register with dates and outcomes; the screening record for
each instruction; written disclosures and the consents obtained; engagement terms recording role and
side; information-barrier terms and test results; the record of engagements declined for conflict.

**8. Responsible role.** The credential holder, personally, for making the disclosure. The engaging
organisation's responsible partner or officer for deciding whether the engagement may proceed and on
what terms.

**9. Approval authority.** The affected parties, by written consent, for whether the credential holder
may act. The engaging organisation's responsible partner or officer for whether the firm accepts the
engagement. Neither can consent on behalf of a party that has not been told.

**10. Independence requirement.** The information-barrier test under PR-04 must be performed by a
person *independent* of both sides of the barrier. Where the credential holder or their firm acts for
more than one party to the same transaction, an independent review of the arrangement is required
before work begins.

**11. Materiality or threshold.** Disclosure is triggered by the existence of an interest, not by its
size: the test is whether a reasonable party to the financing would want to know it when weighing the
judgement, applied by the credential holder and, where doubt exists, resolved by disclosing. A *de
minimis* register threshold for holdings in widely held listed entities may be set by the engaging
organisation's governance, must be written down, and must not apply to any party to the transaction.
*Scale test:* on a small municipal project, the register is a single sheet and the parties are few;
on a multi-billion cross-border financing with dozens of counterparties across several jurisdictions,
the same register is maintained by entity group and the screen is run against the full party list at
each accession, syndication and transfer.

**12. Exception and waiver.** No exception to disclosure is permitted. The *consequence* of a
disclosed conflict may be waived only by the informed written consent of every affected party, for
the stated scope of work, for a stated period; the consent must state what was disclosed. Compensating
controls — an information barrier, a change of team, an independent review of the output — must be
recorded with the consent. All consents are reportable to the engaging organisation's responsible
officer.

**13. Escalation trigger.** Discovery of an undisclosed interest; an instruction to withhold, soften
or delay a disclosure; a consent that appears to have been given on incomplete information; a mandate
that would require advising both sides on the same question; a fee arrangement that varies with a
conclusion the credential holder is being asked to reach.

**14. AI application.** AI may screen instructions against the register and entity-relationship data,
surface related-party and common-control links, monitor for newly arising interests, and draft
disclosure wording for human review.

**15. AI prohibition.** AI must not determine whether a conflict exists, decide that a conflict is
manageable, waive or defer a disclosure, certify a person or firm as *independent*, or be recorded as
the author of a disclosure.

**16. AI verification.** Clause-to-output comparison of each AI-drafted disclosure against the
register entry it reports; source tracing of each surfaced relationship to the record that evidences
it; and the credential holder's own recorded confirmation, against personal knowledge, that the screen
is complete. **A negative machine screen is not a finding of no conflict** and must never be recorded
as one.

**17. External reference.**

- **ISO/IEC 17024 *Conformity assessment — General requirements for bodies operating certification of
  persons*.** Issuing organisation: ISO/IEC. Subject: impartiality and the management of conflicts in
  the certification of persons. Checked: a **2026 edition has been published, superseding the 2012
  edition** (register `EXT-022`, verified 2026-08-03); no clause is cited here. Nature: Manual §6
  category 3 — international voluntary standard. Applicability limitation: voluntary unless a law or
  contract imports it; it addresses certification bodies, not project finance advisers, and is named
  here as the reference discipline for conflict management in assessment roles. **No accreditation to,
  or certification against, this standard is claimed by PCI through this reference.**
- **G20/OECD *Principles of Corporate Governance*.** Issuing organisation: OECD, with the G20.
  Subject: disclosure, related-party transactions and the management of conflicts in governance.
  Checked: 2023 revision, OECD/LEGAL/0413 (register `EXT-128`, verified 2026-08-03). Nature: Manual §6
  category 5 — professional framework; specifically an **OECD Council Recommendation, which is
  intergovernmental, non-binding and not legislation anywhere**. Applicability limitation: creates no
  obligation for any credential holder; named for the governance context only.

**18. Jurisdictional caution.** Fiduciary duty, agency law, the enforceability of consent, secret-
commission and bribery offences, financial-promotion rules and adviser-registration requirements
determine the *legal* consequences of an undisclosed interest, and they differ by jurisdiction and by
role. Consent is not a universal cure. Obtain local legal advice before relying on consent, and note
that anti-bribery obligations can be extraterritorial.

**19. Related PCI Laws.** `PCI-FND-LAW-10` (conflict-of-interest disclosure); `PCI-FND-LAW-13`;
`PCI-PFL-LAW-13.02`; `PCI-PFL-LAW-16.03`. **Increment over the foundational parent:**
`PCI-FND-LAW-10` requires disclosure of conflicts; this law adds the multi-party structure specific
to a financing — screening against the full party list at each accession and transfer, the two-hat
prohibition on advising both sides of one question, tested information barriers, and the rule that
consent obtained on incomplete disclosure is not consent.

**20. Related Body of Knowledge content.** PFL-AI · Domain 1 — Foundations of project finance
leadership · KA 1.3 Ethics, fiduciary awareness and responsible AI · topics 1.3.1 fiduciary and
professional obligations, 1.3.3 conflicts and independence. Also Domain 13 KA 13.1 (the diligence
streams).

**21. Compliance test.** A reviewer takes the engagement file and the conflicts register and performs
four steps. (a) Confirms a screening record exists dated on or before the date work began. (b) For
each interest on the register at that date that touches a party to the transaction, locates the
written disclosure and the consent, and confirms the consent post-dates the disclosure. (c) Confirms
the engagement terms state the role and the side. (d) Where an information barrier is relied upon,
locates a test result within the stated interval, performed by a person independent of both sides.
Compliance is demonstrated when all four steps complete for every interest; an interest on the
register with no disclosure, or a consent pre-dating its disclosure, is a breach.

**22. Breach indicators.** A register with no entries on a transaction with many counterparties; a
consent dated the same day as, or before, the disclosure it relies on; an engagement letter silent on
which side the adviser acts for; an information barrier that has never been tested; a success fee
payable on a conclusion the credential holder is asked to reach; a review described as independent by
a person in the preparer's reporting line.

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Ethical dilemma: an adviser is offered a second role, a success fee
or a referral payment mid-transaction, and the candidate must decide whether to disclose, decline or
proceed and on what record. Evidence selection: from an engagement file, the candidate identifies what
is missing before work may begin. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-01-02` *Conflict Disclosure and the Two-Hat Rule* (v1.0). Amendment
note: restructured onto the twenty-five-element form; *connected person* defined for the first time;
information-barrier testing raised from a minimum action to an identified process requirement;
compliance test made performable; the ISO/IEC 17024 reference updated to record the 2026 edition.

---

## Domain 5 — Project development and bankability

### PCI LAW PCI-PFL-LAW-05.01 — The Bankability Statement

**1. Normative requirement.** A credential holder must not describe a project as bankable, or as
capable of being financed on limited recourse, unless every condition on which that conclusion depends
is stated with its status, its owner and its resolution path.

**2. Purpose.** Bankability is a conjunction, not a score: revenue and payment mechanism, offtake or
grantor covenant, permits, land and consents, technology and track record, construction and
operational readiness, and an allocated risk position must all hold together. A summary word hides
which limb is unresolved, and the reader assumes all of them are.

**3. Scope.** Every credential holder who prepares, reviews, recommends, approves or provides
assurance on a feasibility conclusion, an investment committee paper, an information memorandum, a
mandate letter, a lender's preliminary view or any statement that a project is financeable. Applies
from screening to financial close, and again on any material restructuring.

**4. Defined terms.** *material*, *evidence*, *decision owner*, *finance documents*, *decision-grade*,
*competent reviewer*. **Bankability condition** — for this law, a fact or instrument whose absence
would prevent a lender from advancing on limited recourse: the revenue mechanism, the payment
counterparty's covenant, each permit and consent, land and access rights, technology and its track
record, construction and operational readiness, and the risk allocation position under
`PCI-PFL-LAW-11.01`.

**5. Required actions.**

- **PCI-PFL-LAW-05.01-PR-01 — The conditions schedule.** The preparer must produce a schedule listing
  every *bankability condition*, its status at a stated date, the named party who owns it, the step
  required to resolve it, and the expected date of resolution.
- **PCI-PFL-LAW-05.01-PR-02 — Status honesty.** The preparer must record each condition's status as
  one of *satisfied*, *in progress*, *not started* or *at risk*, evidenced, and must not record a
  condition as satisfied on the strength of an expectation, an indication or a draft.
- **PCI-PFL-LAW-05.01-PR-03 — Conjunction statement.** Wherever the word *bankable* or an equivalent
  appears in a decision-grade output, the preparer must state on the same page that the conclusion
  holds only while every condition in the schedule holds, and must identify the unresolved conditions
  by name.
- **PCI-PFL-LAW-05.01-PR-04 — Re-testing on change.** The preparer must re-run the schedule and
  re-issue the conclusion whenever a condition's status changes materially, and must notify every
  party known to be relying on the earlier conclusion.

**6. Prohibited actions.** Describing a project as bankable while a condition is unresolved and
unstated; recording a condition as satisfied on a draft, an indicative term sheet or an oral
assurance; presenting a lender's expression of interest as a commitment; aggregating conditions into a
score or a colour that conceals which limb fails; carrying forward a superseded bankability conclusion
into a later document without re-testing.

**7. Required evidence.** The dated conditions schedule with owners and resolution paths; the evidence
supporting each *satisfied* status; the conjunction statement as it appeared in the output; the
re-test records; the distribution list used for notification under PR-04.

**8. Responsible role.** The project finance leader accountable for the development or financing plan
prepares and signs the schedule. The *decision owner* for the sanction, mandate or submission accepts
it.

**9. Approval authority.** The decision owner approves the bankability conclusion for use. A change of
a condition's status from *at risk* or *in progress* to *satisfied* may be approved only by the named
owner of that condition, on evidence.

**10. Independence requirement.** A *competent reviewer* independent of the development team must
review the schedule before the conclusion is used to support a sanction decision, a mandate or an
information memorandum, because the development team's commercial benefit runs to the project
proceeding.

**11. Materiality or threshold.** A condition is *material* when its non-resolution would cause a
lender, on the risk allocation as it stands, to decline or to reprice; the *decision owner* records
the test in the engagement's materiality statement and the lenders' own credit criteria override it
wherever those are known. **PCI sets no completeness percentage**, because a conjunction admits none:
one unresolved condition defeats the conclusion regardless of how many others hold. *Scale test:* on a
small municipal project the schedule may contain a dozen conditions and be maintained in one table; on
a multi-billion cross-border financing it is maintained per jurisdiction, per tranche and per lender
group, and the conjunction statement identifies the unresolved conditions per group.

**12. Exception and waiver.** No exception is permitted to element 1. A *decision owner* may approve
in writing the use of a conclusion with a named unresolved condition where that condition is stated on
the face of the output with its owner and resolution path — which is compliance, not exception. A
waiver of PR-04 re-testing is not available.

**13. Escalation trigger.** A condition moving to *at risk* whose failure would prevent financing; a
request to describe a project as bankable while a condition is unresolved; discovery that a *satisfied*
status rests on a draft or an indication; a party relying on a superseded conclusion.

**14. AI application.** AI may assemble the conditions schedule from the document set, track status
changes, identify conditions present in comparable transactions and absent here, draft the conjunction
statement, and flag documents whose dates have passed.

**15. AI prohibition.** AI must not conclude that a project is bankable, set a condition's status to
*satisfied*, decide that an unresolved condition is immaterial, or approve an information memorandum,
mandate or credit paper.

**16. AI verification.** Source tracing of every *satisfied* status to the executed instrument or
issued permit that establishes it; clause-to-output comparison of each AI-drafted condition entry
against that instrument; and a named human's recorded confirmation that no condition known to the team
is missing from the schedule. Completeness is a human judgement and must be recorded as one.

**17. External reference.**

- **The Equator Principles.** Issuing organisation: the Equator Principles Association. Subject: a
  risk-management framework under which participating financial institutions apply agreed
  environmental and social requirements to project finance. Checked: EP4, adopted 18 November 2019,
  effective 1 October 2020 (register `EXT-082`, verified 2026-08-03). Nature: Manual §6 category 8 —
  **voluntary environmental or social framework; not legislation, and not regulation**. Applicability
  limitation: applies only to transactions of adopting institutions, and only as those institutions
  apply it; adoption is the whole of its force.
- **IFC *Performance Standards on Environmental and Social Sustainability*.** Issuing organisation:
  International Finance Corporation, World Bank Group. Subject: environmental and social performance
  expectations widely used as a reference benchmark in project finance. Checked: 2012 edition,
  effective 1 January 2012; a Sustainability Framework update is in progress (register `EXT-083`,
  verified 2026-08-03). Nature: Manual §6 category 8 — voluntary environmental or social framework.
  Applicability limitation: binding on IFC clients by contract; on others only where adopted, including
  through EP4. **Status is moving — verify the current position.**
- **ISO 31000 *Risk management — Guidelines*.** Issuing organisation: ISO. Subject: principles and a
  process for managing risk. Checked: ISO 31000:2018, 2nd edition, reviewed and confirmed 2023
  (register `EXT-020`, verified 2026-08-03). Nature: Manual §6 category 3 — international voluntary
  standard, and **guidance rather than a certifiable requirements standard; nothing can be certified
  against it**. Applicability limitation: voluntary unless a law or contract imports it.

**18. Jurisdictional caution.** Permit and consent regimes, land tenure and expropriation rules,
grid- or network-access rights, foreign-investment screening, and the enforceability of a grantor's
covenant against a public body are all jurisdiction-specific, and several of them can change with a
change of administration. A condition satisfied under one jurisdiction's regime says nothing about
another's. Obtain local legal advice on each permit, each land right and the grantor's capacity to
contract.

**19. Related PCI Laws.** `PCI-FND-LAW-07`; `PCI-FND-LAW-11`; `PCI-PFL-LAW-09.01`;
`PCI-PFL-LAW-11.01`; `PCI-PFL-LAW-12.01`; `PCI-PFL-LAW-13.03`. **Increment over the foundational
parent:** `PCI-FND-LAW-07` requires honest reporting; this law converts a single summary adjective
into an evidenced conjunction with a named owner and a resolution path for each limb, and forbids the
aggregation that hides a failing limb.

**20. Related Body of Knowledge content.** PFL-AI · Domain 5 — Project development and bankability ·
KA 5.3 The bankability test · and KA 5.4 Construction and operational readiness. Also Domain 1 KA 1.2
(the risk–return–bankability triangle) and Domain 12 KA 12.4 (risk allocation, claims and change).

**21. Compliance test.** A reviewer takes the output containing the bankability conclusion and the
conditions schedule, and performs four steps. (a) Confirms every *bankability condition* in element 4
appears in the schedule with a status, an owner and a resolution path. (b) For each condition marked
*satisfied*, locates the executed instrument, issued permit or equivalent record and confirms it is
final rather than draft. (c) Confirms the conjunction statement appears on the same page as the
conclusion and names each unresolved condition. (d) Confirms the schedule date is not earlier than the
output date. Compliance is demonstrated when all four complete; a *satisfied* status supported by a
draft, or a missing condition, is a breach.

**22. Breach indicators.** A bankability conclusion with no schedule; a status column of colours with
no evidence column; a condition owned by "the project"; a schedule dated months before the memorandum
that relies on it; an information memorandum describing an indicative term sheet as a commitment; a
condition that disappeared between versions without a resolution record.

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Scenario judgement: a candidate is given a memorandum describing a
project as bankable and a document set in which one permit is a draft, and must identify the defect
and the compliant wording. Evidence selection: choosing which document proves a condition satisfied.
No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-05-01` *The Bankability Test* (v1.0). Amendment note: restructured onto
the twenty-five-element form; *bankability condition* defined; the re-test and notification duty added
as PR-04; threshold rewritten to state expressly that a conjunction admits no completeness percentage.

---
## Domain 6 — Financial modelling

### PCI LAW PCI-PFL-LAW-06.01 — Financial-Model Architecture

**1. Normative requirement.** A *financial model* used for a *decision-grade* output must separate
inputs, calculations and outputs into distinct, identifiable regions, so that no cell serves two of
those roles.

**2. Purpose.** A model in which an input sits inside a formula, or an output is typed over a
calculation, cannot be reviewed, cannot be scenario-switched reliably and cannot be handed on. The
architecture is what makes every other modelling law in this domain testable.

**3. Scope.** Every *financial model* a credential holder builds, materially edits, reviews, relies
upon or presents for a decision — screening, base case, bank case, financial-close, operating, budget,
restructuring and refinancing models — on any platform, including spreadsheet, code and
vendor-hosted forms. Applies to preparation, review, recommendation, approval and assurance.

**4. Defined terms.** *financial model*, *model owner*, *authoritative version*, *decision-grade*,
*material*, *evidence*, *competent reviewer*, *verified*, *base case*. **Check block** — a named,
visible region of the model that computes the model's own arithmetic invariants as differences that
resolve to nil, each with a pass/fail state visible without navigation. **Invariant** — an identity
that must hold in a correct model, such as balance-sheet balance, the equality of *sources and uses*
totals, cash-flow articulation, and the closure of every debt and reserve schedule.

**5. Required actions.**

- **PCI-PFL-LAW-06.01-PR-01 — Region separation.** The *model owner* must place every input in an
  input region, every calculation in a calculation region and every reported figure in an output
  region, and must not permit a cell to hold both a typed value and a formula.
- **PCI-PFL-LAW-06.01-PR-02 — One timeline.** The model owner must build the model on one declared
  timeline with a single stated periodicity, date convention and first-period definition, and must
  drive every schedule from it.
- **PCI-PFL-LAW-06.01-PR-03 — The check block.** The model owner must maintain a *check block*
  covering, as a minimum, balance-sheet balance, *sources and uses* equality, cash-flow articulation,
  the closure of each debt schedule and each *reserve account*, and the non-negativity of every account
  balance the *finance documents* require to be non-negative.
- **PCI-PFL-LAW-06.01-PR-04 — Failing checks stop circulation.** No person may quote, circulate,
  submit or rely upon a model output while any check in the check block is failing; the model owner
  must record each failure, its cause and its resolution.
- **PCI-PFL-LAW-06.01-PR-05 — Scenario switch integrity.** The model owner must implement every case
  as a switch over the input region alone, and must not implement a case by editing a calculation.

**6. Prohibited actions.** Placing a typed value inside a calculation region; running more than one
timeline in one model without a declared reconciliation; circulating a model or a figure from it while
a check is failing; deleting or suppressing a failing check rather than resolving it; implementing a
scenario by editing formulas; presenting an output region figure that no calculation produces.

**7. Required evidence.** The model file identified by its *authoritative version* identifier; the
visible check block with its state at the date of the output; the check-failure log with causes and
resolutions; the timeline declaration; the case switch and its input mapping; the model owner's
release record.

**8. Responsible role.** The *model owner*, named, for the architecture and the check block. The
*decision owner* for the decision taken on the model's output.

**9. Approval authority.** The model owner approves a release of the model. Only the *decision owner*,
in writing, may approve the use of an output produced while a check was failing, and only with the
failing check, its cause and its effect stated on the face of the output — see element 12.

**10. Independence requirement.** Independence is not required to build the model. It is required for
the review that supports financial close, a *distribution*, a covenant certificate or a lender
submission, which is governed by `PCI-PFL-LAW-13.01`.

**11. Materiality or threshold.** A check either resolves to nil or it does not; the *model owner*
records a nil tolerance for each check in the model's own units, set to the rounding precision of the
underlying quantity and no wider. **PCI sets no tolerance figure**, because the defensible tolerance
is a function of the model's currency units and rounding, which differ between transactions. *Scale
test:* on a small municipal project with one tranche and annual periods, the check block is a handful
of rows; on a multi-billion cross-border financing with monthly periods, several currencies and a
dozen tranches, the same invariants are checked per currency and per tranche, and the block carries a
single roll-up state so a failure anywhere is visible without navigation.

**12. Exception and waiver.** No exception is permitted to PR-01, PR-02 or PR-03. PR-04 may be
departed from only by written approval of the *decision owner*, for one named output, for a period not
exceeding fourteen days, where the failing check is stated on the face of the output together with its
cause and its effect on the figures quoted, and where a *competent reviewer* has confirmed that the
failure does not affect the quantities relied upon. The approval is reported to the model owner and
recorded in the check-failure log. An unrecorded departure is a breach.

**13. Escalation trigger.** A check that fails and cannot be resolved before the output is needed; a
check that has been removed, disabled or hard-coded to pass; discovery of a typed value inside a
calculation region in a released model; two live timelines in one model; an output figure with no
calculation behind it.

**14. AI application.** AI may propose an architecture, generate the timeline and schedule skeletons,
build and extend the check block, scan a model for typed values inside calculation regions and for
inconsistent formulas across a row, and produce a structural map of a model it has been given.

**15. AI prohibition.** AI must not release a model, decide that a failing check may be ignored, set
or widen a check tolerance, approve an architecture for use, or be recorded as the *model owner*.

**16. AI verification.** Independent recomputation by a named human of at least one figure from each
of the model's principal schedules, from source lines; boundary testing of the check block by
introducing a known error and confirming that the relevant check fails; and source tracing of the
timeline declaration to the *finance documents* or the agreed term sheet. The boundary test is
recorded with its date and its result.

**17. External reference.**

- **ICAEW *Financial Modelling Code*.** Issuing organisation: the Institute of Chartered Accountants
  in England and Wales. Subject: principles for building, documenting and controlling financial
  models. Checked: current, by name only, no clause or edition asserted (register `EXT-125`, verified
  2026-08-03). Nature: Manual §6 category 5 — professional framework. Applicability limitation:
  principles-based guidance published by a professional body; **not a compliance standard, not
  certifiable, and binding on no one unless a body, regulator or engagement adopts it.**
- **The FAST Standard.** Issuing organisation: the FAST Standard Organisation. Subject: a structural
  convention for spreadsheet financial models. Checked: current, by name only, no edition asserted
  (register `EXT-126`, verified 2026-08-03). Nature: Manual §6 category 5 — professional framework;
  **voluntary, adopted by choice, and imposing no obligation of its own**. Applicability limitation:
  this law requires the separation of inputs, calculations and outputs; it does **not** require
  conformity with this or any other named modelling convention, and adopting one does not by itself
  satisfy this law.

**18. Jurisdictional caution.** Where a model is a contractual deliverable, a condition of financing
or a regulated submission, the governing law of the *finance documents* determines which version
governs, what representations attach to it and what liability follows from an error. Model-liability
positions differ by jurisdiction and by engagement. Obtain qualified legal advice on the contractual
status of the model before releasing it outside the preparing organisation.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-06`; `PCI-PFL-LAW-06.02`;
`PCI-PFL-LAW-06.03`; `PCI-PFL-LAW-06.05`; `PCI-PFL-LAW-13.01`; `PCI-PFL-LAW-16.01`. **Increment over
the foundational parent:** `PCI-FND-LAW-06` requires data lineage and integrity; this law adds the
structural preconditions that make lineage checkable in a financing model — role separation per cell,
one declared timeline, a visible check block of named invariants, and a circulation stop while any
check fails.

**20. Related Body of Knowledge content.** PFL-AI · Domain 6 — Financial modelling · KA 6.1 Model
architecture · topics: the inputs–calculations–outputs separation and the model timeline. Also KA 6.2
(construction and operating models; articulation) and KA 6.4 (checks, sensitivity, model audit and AI
controls).

**21. Compliance test.** A reviewer opens the *authoritative version* and performs five steps. (a)
Samples twenty cells from the calculation region on a stated basis and confirms that none holds a typed
value. (b) Confirms one declared timeline drives every schedule, by changing the first-period date and
observing that all schedules move together. (c) Confirms the check block covers each invariant listed
in PR-03 and that every check reads nil. (d) Introduces a known error into one input, confirms the
relevant check fails, and reverses the change. (e) Switches the case and confirms that no formula
changed. Compliance is demonstrated when all five complete; a check that does not detect the seeded
error is a failed check block and a breach.

**22. Breach indicators.** A model whose totals row is typed; a check block that is always green
because its tolerance is wide; a scenario implemented by a second copy of the calculation sheet; two
date columns with different conventions; an output figure that cannot be traced to any formula; a
model circulated with a note that "the balance sheet is out by a small amount".

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Calculation review: a candidate is given a model extract with a
seeded architecture defect and must find it and state the compliant structure. Scenario judgement: a
close deadline against a failing check, testing whether the candidate stops circulation or invokes the
element 12 route correctly. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-06-01` *Model Integrity* (v1.0). Amendment note: restructured onto the
twenty-five-element form; *check block* and *invariant* defined; scenario-switch integrity added as
PR-05; the element 12 route for a failing check made explicit, bounded and reportable; boundary
testing of the check block added to element 16.

---

### PCI LAW PCI-PFL-LAW-06.02 — Formula Consistency

**1. Normative requirement.** A credential holder must not present as calculated any figure that was
typed, pasted or overridden.

**2. Purpose.** A hard-coded constant inside a calculation is invisible to a reviewer, survives every
scenario switch, and quietly makes the model answer a question nobody asked. Formula inconsistency
across a row does the same thing intermittently, which is worse, because the model is right in the
periods the reviewer samples.

**3. Scope.** Every calculation in a *financial model* used for a *decision-grade* output, and every
figure quoted from such a model in a lender-facing, board-facing, grantor-facing or investor-facing
document. Applies to preparation, review and approval.

**4. Defined terms.** *financial model*, *model owner*, *decision-grade*, *material*, *verified*,
*source line*, *authoritative version*. **Override** — a typed value placed over a formula, whether
permanently or for a case. **Row consistency** — the property that one formula, copied without
variation, computes every period of a calculation row.

**5. Required actions.**

- **PCI-PFL-LAW-06.02-PR-01 — Declared inputs only.** The *model owner* must express every
  calculation as a formula referencing a declared input or another calculation, and must not embed a
  numeric constant in a calculation other than a dimensionless mathematical constant.
- **PCI-PFL-LAW-06.02-PR-02 — Row consistency.** The model owner must maintain *row consistency*
  across every calculation row, and where a period genuinely requires a different formula must place
  the difference in a declared switch or flag driven from the input region.
- **PCI-PFL-LAW-06.02-PR-03 — Override register.** Where an *override* is unavoidable, the model owner
  must record it in an override register stating the cell, the value, the reason, the author, the date
  and the date by which it will be removed, and must make the override visible in the model.
- **PCI-PFL-LAW-06.02-PR-04 — Override clearance before release.** The model owner must clear every
  override from the *authoritative version* before release, or must report each remaining override on
  the face of every output produced from that version.

**6. Prohibited actions.** Embedding a constant, a rate, a factor, a date or an escalation inside a
calculation; typing over a formula without registering the override; describing a typed figure as
calculated, modelled or derived; varying a formula mid-row without a declared switch; hiding an
override by formatting, by column width or by placing it off the visible sheet.

**7. Required evidence.** The model *authoritative version*; the override register with the six fields
in PR-03; the row-consistency scan output for each calculation block, dated; the release record showing
overrides cleared or reported; the outputs that carried an override disclosure.

**8. Responsible role.** The *model owner* for the model's formulas and its override register. The
person who quotes a figure externally for the accuracy of that quotation.

**9. Approval authority.** The model owner approves an entry in the override register. The *decision
owner* approves the release of a model that still carries an override, and only with the disclosure
required by PR-04.

**10. Independence requirement.** Not required for compliance with this law by the preparer, because
the test is mechanical and repeatable. The row-consistency scan supporting a model audit conclusion
must be performed or reperformed by a person *independent* of preparation, under
`PCI-PFL-LAW-13.01`.

**11. Materiality or threshold.** No materiality threshold applies to the existence of a hard-coded
constant: any constant embedded in a calculation is a defect and is registered or removed. Materiality
governs only *escalation*: the *decision owner* records, in the engagement's materiality statement, the
movement in the transaction's own metric at which a discovered override becomes reportable beyond the
model owner. **PCI sets no figure**, because the metric differs by transaction. *Scale test:* on a
small municipal project the scan is run over a few hundred formula cells and takes minutes; on a
multi-billion cross-border financing the scan is run per sheet and per tranche, and the register is
maintained by workstream owner so that clearance before release is assignable.

**12. Exception and waiver.** No exception is permitted to element 1 or to PR-01. An override may
persist to release only under PR-04, with disclosure on every output, approved by the *decision owner*,
for a stated period not exceeding the next model release, and with a named person accountable for its
removal. Reported to the *model owner* and recorded in the register.

**13. Escalation trigger.** Discovery of an unregistered override in a released model; an override
whose removal date has passed; a formula that varies mid-row without a switch; a figure quoted
externally that the model does not compute; an override that changes a *coverage ratio*, a debt
quantum or a *distribution* result.

**14. AI application.** AI may scan a model for constants embedded in calculations, detect
row-inconsistency, list overrides with their cell references, propose the declared-input replacement
for a discovered constant, and draft override-register entries for confirmation.

**15. AI prohibition.** AI must not create an override, decide that an override is acceptable, clear
an override from the register, approve a release, or be recorded as the author of an override.

**16. AI verification.** Independent recomputation of the affected calculation after each
AI-proposed replacement, by a named human, against the source that determines the constant; and a
sample of the AI scan re-run manually on a stated basis — no fewer than one calculation block per
model sheet — to confirm the scan's coverage. A clean machine scan is not evidence of consistency
until the sample confirms the scan looked where it claimed to.

**17. External reference.**

- **The FAST Standard.** Issuing organisation: the FAST Standard Organisation. Subject: structural
  and formula conventions for spreadsheet models, including consistency across a row. Checked: current,
  by name only (register `EXT-126`, verified 2026-08-03). Nature: Manual §6 category 5 — professional
  framework; **voluntary**. Applicability limitation: this law requires *row consistency* and the
  absence of embedded constants; it does not require conformity with this convention, and conformity
  with it does not by itself satisfy this law.
- **ICAEW *Financial Modelling Code*.** Issuing organisation: ICAEW. Subject: transparency and
  documentation of model calculations. Checked: current, by name only (register `EXT-125`, verified
  2026-08-03). Nature: Manual §6 category 5 — professional framework. Applicability limitation:
  guidance published by a professional body; not certifiable; binding only where adopted.

**18. Jurisdictional caution.** Where a model or a figure derived from it is included in an offering
document, a listing particular, a regulated disclosure or a submission to a public authority, the
liability regime for a misstatement is jurisdiction-specific and can extend personally to the person
who prepared or signed it. Obtain qualified legal advice before a model figure is quoted in any such
document.

**19. Related PCI Laws.** `PCI-FND-LAW-06`; `PCI-FND-LAW-07`; `PCI-PFL-LAW-06.01`;
`PCI-PFL-LAW-06.03`; `PCI-PFL-LAW-06.04`; `PCI-PFL-LAW-16.01`. **Increment over the foundational
parent:** `PCI-FND-LAW-06` requires that data can be traced to its origin; this law adds the
calculation-level discipline that makes tracing possible at all — no constant inside a formula, one
formula per row, and every unavoidable override registered, visible, dated and owned.

**20. Related Body of Knowledge content.** PFL-AI · Domain 6 — Financial modelling · KA 6.1 Model
architecture and KA 6.4 Checks, sensitivity, model audit and AI controls. Also Domain 13 KA 13.2
(model audit).

**21. Compliance test.** A reviewer takes the *authoritative version* and performs four steps. (a)
Runs a formula-consistency scan across every calculation row and confirms that each row is computed by
one formula, or that each break is explained by a declared switch. (b) Extracts every numeric constant
appearing inside a calculation and confirms the list is empty but for dimensionless mathematical
constants. (c) Reconciles the override register to the model, confirming that every registered override
is present and every present override is registered. (d) Confirms that each override in a released
version appears on the face of the outputs produced from it. Compliance is demonstrated when all four
complete; one unregistered override is a breach.

**22. Breach indicators.** A growth rate typed inside a revenue formula; a row whose final period
differs from every other; a "plug" cell; a figure in a credit paper that the model does not produce; an
override register with entries but no removal dates; a model that reconciles only after a manual
adjustment nobody can locate.

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Calculation review: a candidate is given a model row with a seeded
constant and must locate it and state its effect across the case set. Evidence selection: choosing
which record proves an override was authorised. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-06-02` *Formula Transparency* (v1.0). Amendment note: restructured onto
the twenty-five-element form; *override* and *row consistency* defined; the override register given six
mandatory fields and a removal date; the compliance test converted into a repeatable scan-and-reconcile
procedure.

---

### PCI LAW PCI-PFL-LAW-06.03 — Input and Assumption Traceability

**1. Normative requirement.** Every assumption used in a *decision-grade* model must be entered once,
in the input region, and recorded in an assumption register that travels with the model.

**2. Purpose.** Assumptions are where a model's judgement lives. An assumption entered twice diverges;
an assumption with no recorded owner cannot be challenged, refreshed or defended; and a model whose
register does not travel with it becomes unreadable the moment its author leaves the transaction.

**3. Scope.** Every assumption in a *financial model* used for a *decision-grade* output — macro,
price, volume, cost, capital-expenditure phasing, operating cost, tax, financing, escalation, currency,
availability and terminal-value assumptions alike — at every stage from screening to restructuring.

**4. Defined terms.** *financial model*, *model owner*, *decision-grade*, *evidence*, *material*,
*source line*, *authoritative version*, *verified*. **Assumption register** — a record travelling with
the model that states, for each assumption, its value, its unit, its basis, its *source line*, its date
and its named owner. **Basis** — the reasoning or derivation by which the value was arrived at, stated
in enough detail that a *competent reviewer* could reach the same value from the same source.

**5. Required actions.**

- **PCI-PFL-LAW-06.03-PR-01 — Single entry.** The *model owner* must ensure each assumption is entered
  once and referenced everywhere else, and must not permit the same quantity to be typed in two places.
- **PCI-PFL-LAW-06.03-PR-02 — Register completeness.** The model owner must record every assumption in
  the *assumption register* with all six fields — value, unit, *basis*, *source line*, date, named
  owner — and must not use in a decision-grade case any assumption whose owner or basis is absent.
- **PCI-PFL-LAW-06.03-PR-03 — Register travels with the model.** The model owner must release the
  register with every release of the model, bearing the same *authoritative version* identifier.
- **PCI-PFL-LAW-06.03-PR-04 — Currency of assumptions.** The model owner must record a review date for
  each assumption and must re-confirm or replace any assumption whose review date has passed before the
  model is used for a further decision.

**6. Prohibited actions.** Entering the same assumption in two places; using an assumption with no
named owner in a decision-grade case; recording "management estimate" or "market practice" as a
*basis* without stating the derivation; releasing a model without its register; carrying an expired
assumption into a new decision without re-confirmation; changing an assumption value without changing
its date.

**7. Required evidence.** The assumption register with all six fields per assumption; the release
record pairing register and model version; the re-confirmation record for each assumption past its
review date; the *source line* evidence behind each externally sourced assumption.

**8. Responsible role.** The *model owner* for the register's existence, completeness and release. The
named assumption owner, individually, for the value and *basis* of their own assumptions.

**9. Approval authority.** The named assumption owner approves a change to their assumption's value or
basis. The *decision owner* approves the assumption set as a whole for a decision-grade case.

**10. Independence requirement.** Not required for entry or maintenance. Independent challenge of the
assumption set is required where the model supports financial close, a lender submission, a
*distribution* or a restructuring proposal, and is discharged under `PCI-PFL-LAW-13.01`.

**11. Materiality or threshold.** Every assumption is registered regardless of size; materiality
governs the *depth of basis* required and the review interval, both set by the *decision owner* in the
engagement's materiality statement, in the transaction's own metric — for example the movement in the
minimum *coverage ratio* produced by a stated proportional change in the assumption. **PCI sets no
figure.** *Scale test:* on a small municipal project a register of thirty to sixty assumptions is a
single table maintained by one person; on a multi-billion cross-border financing the register runs to
several hundred entries, is partitioned by workstream with an owner per partition, and the review-date
discipline is what keeps a two-year development from carrying a stale price curve into close.

**12. Exception and waiver.** No exception is permitted to PR-02 for a decision-grade case. For a
clearly labelled indicative or screening case, the *model owner* may approve in writing the use of an
assumption whose *basis* is recorded as *placeholder*, provided the placeholder is visible in the
register and on the output, and provided the case is not used for a decision. Duration: until the case
is next reissued. Compensating control: a standing list of placeholders reviewed at each release.

**13. Escalation trigger.** A decision-grade case containing an assumption with no owner or no basis;
an assumption whose value changed without an owner's approval; two entries of the same quantity with
different values; an assumption past its review date being used for a new decision; a *material*
assumption whose only basis is another model.

**14. AI application.** AI may extract candidate assumptions and their sources from documents into a
draft register, detect duplicate entry of the same quantity, flag assumptions past their review date,
compare an assumption set against a previous version, and draft basis statements for confirmation.

**15. AI prohibition.** AI must not be the recorded owner of an assumption, set or approve an
assumption value, decide that a basis is sufficient, or re-confirm an expired assumption.

**16. AI verification.** Source tracing of every AI-extracted assumption to the document, version and
issuing party stated in its *source line*, by a named human; clause-to-output comparison where the
assumption derives from a contractual term; and independent recomputation of any assumption the AI
derived by calculation. Verification is recorded per assumption, not per register.

**17. External reference.**

- **ICAEW *Financial Modelling Code*.** Issuing organisation: ICAEW. Subject: documentation of model
  inputs and assumptions. Checked: current, by name only (register `EXT-125`, verified 2026-08-03).
  Nature: Manual §6 category 5 — professional framework. Applicability limitation: guidance; not
  certifiable; binding only where adopted.
- **ISO 8000 (data-quality series).** Issuing organisation: ISO. Subject: data quality, including the
  provenance and completeness of data used in decisions. Checked: a **multi-part series** — Part 1
  (Overview) is ISO 8000-1:2022, with further parts issued separately; cited generically, no part
  relied upon (register `EXT-026`, verified 2026-08-03). Nature: Manual §6 category 3 — international
  voluntary standard. Applicability limitation: voluntary unless a law or contract imports it; the
  series addresses data quality generally and imposes nothing on a project financing of its own force.

**18. Jurisdictional caution.** Assumptions on tax rates, allowances, depreciation, withholding,
indexation, tariff regulation and permitted return are jurisdiction-specific and change with
legislation and with regulatory determinations. An assumption *basis* that cites another jurisdiction's
regime, or an out-of-date determination, is a defect that a reviewer cannot detect from the model
alone. Obtain qualified local tax and regulatory advice on each such assumption — see
`PCI-PFL-LAW-12.02`.

**19. Related PCI Laws.** `PCI-FND-LAW-06`; `PCI-FND-LAW-05`; `PCI-PFL-LAW-06.01`;
`PCI-PFL-LAW-06.02`; `PCI-PFL-LAW-06.04`; `PCI-PFL-LAW-16.01`. **Increment over the foundational
parent:** `PCI-FND-LAW-06` requires lineage from a figure to its origin; this law adds the register
that makes lineage survive a change of personnel — six mandatory fields, a named human owner per
assumption, single entry, and a review date that expires the assumption rather than letting it drift.

**20. Related Body of Knowledge content.** PFL-AI · Domain 6 — Financial modelling · KA 6.1 Model
architecture and KA 6.2 Construction-period and operating models. Also Domain 8 KA 8.1 (cost estimate
classes) and Domain 7 KA 7.3 (price escalation and volume risk).

**21. Compliance test.** A reviewer takes the released model and its register and performs four steps.
(a) Selects every assumption whose proportional change of a stated size moves the minimum *coverage
ratio* by more than the recorded materiality figure, and confirms each has all six register fields
populated. (b) Traces each of those *source lines* to the document named, at the version named, issued
by the party named. (c) Searches the input region for duplicate entry of any registered quantity and
finds none. (d) Confirms every assumption's review date is later than the output date, or that a
re-confirmation record exists. Compliance is demonstrated when all four complete; an unowned or
unsourced *material* assumption is a breach.

**22. Breach indicators.** A register with a "source" column reading "internal"; the same escalation
rate typed on three sheets; a model released without its register; an assumption dated two years before
the close it supports; a basis that cites the previous transaction; an owner column populated with a
team name.

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Evidence selection: from a register extract, the candidate identifies
which assumptions may not be used in a decision-grade case and why. Scenario judgement: a stale price
assumption is carried into a close, and the candidate must state the required action. No live
examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-06-03` *Assumption Traceability* (v1.0). Amendment note: restructured
onto the twenty-five-element form; *assumption register* and *basis* defined; the review-date and
re-confirmation duty added as PR-04; the compliance test made performable by tying the sample to a
recorded materiality figure rather than to reviewer taste.

---

### PCI LAW PCI-PFL-LAW-06.04 — The Source Line

**1. Normative requirement.** A credential holder must withdraw from a *decision-grade* document any
figure whose *source line* cannot be produced on request.

**2. Purpose.** A figure without a traceable origin cannot be checked, cannot be refreshed and cannot
be defended when it is challenged — and in a financing it is challenged, under time pressure, by
someone entitled to an answer. The source line is what converts a number into a statement someone
made.

**3. Scope.** Every input to a *decision-grade* model and every figure quoted from one in a
lender-facing, board-facing, grantor-facing, investor-facing, rating-agency-facing or public document,
including figures in presentations, term sheets, information memoranda, credit papers, compliance
certificates and disclosure documents.

**4. Defined terms.** *source line*, *decision-grade*, *evidence*, *material*, *authoritative version*,
*verified*, *model owner*. **Producible on request** — retrievable and deliverable, in the form
recorded, within the response time the engaging organisation's governance states, by a person other
than the figure's author.

**5. Required actions.**

- **PCI-PFL-LAW-06.04-PR-01 — Source line on every input.** The preparer must attach to every model
  input a *source line* naming the document, its version or date and the issuing party.
- **PCI-PFL-LAW-06.04-PR-02 — Source line on every quoted figure.** The preparer must be able to
  produce, for every figure quoted in a decision-grade document, the source line and the underlying
  record.
- **PCI-PFL-LAW-06.04-PR-03 — Retention with the output.** The preparer must retain the underlying
  record with the output for the retention period the engaging organisation's governance states, and
  not merely a reference to a location that may change.
- **PCI-PFL-LAW-06.04-PR-04 — Withdrawal on failure.** Where a source line cannot be produced, the
  preparer must withdraw the figure from the document, notify every recipient of the document, and
  record the withdrawal.

**6. Prohibited actions.** Citing another model, a file path, a mailbox, a person's recollection or "as
previously advised" as a source line; quoting a figure whose origin is unknown; re-using a figure from
a superseded document without re-sourcing it; describing an estimate as sourced; leaving a withdrawn
figure in circulation.

**7. Required evidence.** The source line attached to each input and quoted figure; the retained
underlying records; the retention record; the withdrawal notices issued under PR-04 with their
distribution lists.

**8. Responsible role.** The preparer of the document for the figures it contains. The *model owner*
for the source lines on model inputs.

**9. Approval authority.** The *decision owner* approves the issue of a decision-grade document. No one
may approve the quotation of a figure whose source line does not exist.

**10. Independence requirement.** Not required for attachment. A sample of source lines must be
re-traced by a person *independent* of preparation as part of any model audit or diligence review under
`PCI-PFL-LAW-13.01`.

**11. Materiality or threshold.** Every input carries a source line regardless of size. Materiality
governs the *sampling density* of the independent re-trace and the *urgency* of a withdrawal: the
*decision owner* records both in the engagement's materiality statement, in the transaction's own
metric. **PCI sets no sampling percentage**, because defensible density depends on population size and
homogeneity, which differ by transaction. *Scale test:* on a small municipal project the underlying
records are few enough to retain in full alongside the model; on a multi-billion cross-border financing
with a virtual data room of tens of thousands of documents, PR-03 is satisfied by retaining the
specific record with an immutable identifier rather than a data-room link that expires at close.

**12. Exception and waiver.** No exception is permitted to element 1. Where a source is confidential
and cannot be delivered to a particular recipient, the figure may still be quoted if the source line is
recorded in full internally and the recipient is told that the source exists and is withheld, and only
with the written approval of the *decision owner* — the figure is never presented as unsourced. Any
confidentiality restriction is applied under `PCI-FND-LAW-09`.

**13. Escalation trigger.** A challenged figure whose source cannot be produced within the stated
response time; a source line pointing to a document that does not exist at the version stated; a figure
appearing in an external document that is absent from the model; discovery that a quoted figure came
from a superseded draft.

**14. AI application.** AI may attach and format source lines from a document set, detect figures in a
document that no source line covers, compare a quoted figure to the *authoritative version* of the
model, and check that each cited document exists at the version cited.

**15. AI prohibition.** AI must not create, infer or reconstruct a source line for a figure whose
origin it has not read; must not approve the issue of a document; and must not decide that a missing
source is immaterial. **An AI-generated citation that has not been traced to the document by a human is
not a source line.**

**16. AI verification.** Source tracing by a named human of every AI-attached source line for a
*material* figure, opening the cited document at the cited version and confirming the figure and the
issuing party; plus a sample of non-material source lines on the stated basis. Recorded per figure with
the date and the tracer's name.

**17. External reference.**

- **ISO 8000 (data-quality series).** Issuing organisation: ISO. Subject: data provenance and quality.
  Checked: multi-part series, Part 1 is ISO 8000-1:2022; cited generically (register `EXT-026`,
  verified 2026-08-03). Nature: Manual §6 category 3 — international voluntary standard. Applicability
  limitation: voluntary unless imported by law or contract.
- **ISO 15489-1 *Information and documentation — Records management — Part 1: Concepts and
  principles*.** Issuing organisation: ISO. Subject: the characteristics that make a record reliable
  and retrievable. Checked: ISO 15489-1:2016 (register `EXT-025`, verified 2026-08-03). Nature: Manual
  §6 category 3 — international voluntary standard. Applicability limitation: voluntary unless imported
  by law or contract; it sets no retention period for a project financing, which is a matter for the
  *finance documents*, local law and the engaging organisation's governance.

**18. Jurisdictional caution.** Retention periods, the admissibility and evidential weight of an
electronic record, data-localisation requirements and rules on retaining personal data are
jurisdiction-specific and can conflict with one another in a cross-border financing. A retention
policy lawful in one jurisdiction can breach another's. Obtain local legal advice on retention and on
cross-border transfer before designing the retention arrangement.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-06`; `PCI-FND-LAW-12`; `PCI-PFL-LAW-06.03`;
`PCI-PFL-LAW-13.04`; `PCI-PFL-LAW-16.02`. **Increment over the foundational parent:**
`PCI-FND-LAW-05` requires an audit trail; this law states what the trail must contain for a financing
figure — document, version, issuing party — makes production on request the test, retains the record
rather than a link, and imposes a positive duty to withdraw and notify when production fails.

**20. Related Body of Knowledge content.** PFL-AI · Domain 6 — Financial modelling · KA 6.1 Model
architecture. Also Domain 13 KA 13.1 (the diligence streams) and KA 13.3 (conditions precedent and
documentation).

**21. Compliance test.** A reviewer selects, from a decision-grade document, every figure that is
*material* on the recorded test plus a sample of the remainder on the stated basis, and for each: asks
the preparer to produce the source line and the underlying record within the stated response time;
opens the record; and confirms that it is the document, at the version, from the party named, and that
it contains the figure. Compliance is demonstrated when every selected figure is produced and matches.
A figure that cannot be produced within the stated time, or that does not match its record, is a breach
and must be withdrawn under PR-04.

**22. Breach indicators.** A source column reading "model"; a data-room hyperlink that no longer
resolves; a figure in a board pack that the model does not contain; a term sheet quoting a
counterparty's own earlier estimate as a fact; a document reissued with figures unchanged after the
underlying report was superseded.

**23. Consequence within PCI authority.** Correction required and the affected figure or output
withheld; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Evidence selection: given a figure and four candidate records, the
candidate selects the one that constitutes a source line. Scenario judgement: a lender challenges a
figure at close and the source cannot be produced — the candidate must state the required action and
its sequence. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-06-04` *The Source Line* (v1.0). Amendment note: restructured onto the
twenty-five-element form; *producible on request* defined with a response time; retention of the record
rather than a link added as PR-03; withdrawal and notification made an identified process requirement;
the AI-citation prohibition made explicit.

---

### PCI LAW PCI-PFL-LAW-06.05 — Model Version Control

**1. Normative requirement.** A *financial model* that has been used for a decision must not be
changed except under version control, so that exactly one *authoritative version* exists at any time
and every change to it is recorded before it is relied upon.

**2. Purpose.** A transaction model is edited continuously, by several hands, under time pressure.
Without version control the parties stop looking at the same project, and nobody can say when a number
changed, why, or on whose authority. This is also the law that v1.0 got wrong: its obligations were
carried on the ISO requirement auxiliary rather than on **must**, and they are restated here in PCI's
own drafting form without any change of substance.

**3. Scope.** Every model used for a decision, a report or a certification — screening, base case,
bank case, financial-close, operating, budget, restructuring and refinancing models — and every edit
to them, whether made by a human or by an AI system.

**4. Defined terms.** *financial model*, *model owner*, *authoritative version*, *base case*,
*decision-grade*, *evidence*, *material*, *verified*, *competent reviewer*. **Change log** — a record
in which each change to a model is entered with its author, date, reason and effect, before the changed
version is relied upon. **Regression suite** — a stated set of independently *verified* figures
recomputed after every edit, whose movements must each be explained before the version is accepted.

**5. Required actions.**

- **PCI-PFL-LAW-06.05-PR-01 — One authoritative version.** The *model owner* must identify one
  *authoritative version* at any time, must give it a version identifier, and must distribute
  controlled copies rather than permitting uncontrolled local copies to circulate.
- **PCI-PFL-LAW-06.05-PR-02 — Every change logged under a named human author.** The model owner must
  record every change in the *change log* with author, date, reason and effect, and must not permit any
  change to reach the authoritative version without a change-log entry naming a human author.
- **PCI-PFL-LAW-06.05-PR-03 — Check block and regression re-run.** The model owner must re-run the
  *check block* required by `PCI-PFL-LAW-06.01-PR-03` and the *regression suite* after every edit,
  whether made by a human or by an AI system.
- **PCI-PFL-LAW-06.05-PR-04 — Every movement explained before acceptance.** The model owner must
  explain every movement in the regression suite, in writing, before accepting the version, and must
  not accept a version carrying an unexplained movement.
- **PCI-PFL-LAW-06.05-PR-05 — Locking the agreed case.** The model owner must lock the agreed
  *base case* at financial close with an integrity control, and must treat every later change as a new
  version rather than as an edit to the locked case.

**6. Prohibited actions.** Editing an agreed or locked model without logging the change; distributing
an unlabelled copy; accepting an unexplained regression movement; recording a tool, a script or an AI
system as the author of a change; representing an updated model as the version agreed at close;
allowing two files bearing the same version identifier to differ in content.

**7. Required evidence.** The version register; change-log entries with author, date, reason and
effect; regression results per version with the written explanation of each movement; the locked close
model with its version identifier and integrity control; the controlled-distribution record.

**8. Responsible role.** The *model owner* for the model and its version control. The lender's or
sponsor's named approver for any change to an agreed or locked *base case*.

**9. Approval authority.** The model owner approves a release. A change to a locked base case, or to a
definition on which debt sizing, a *coverage ratio* or the waterfall depends, requires the written
approval of the party entitled to it under the *finance documents* — commonly the agent or the
lenders' technical or model adviser.

**10. Independence requirement.** A *competent reviewer* independent of the model's development must
review any change to a locked *base case*, any change affecting a covenant definition, debt sizing or
the waterfall, and each revalidation of a high-consequence model at the interval its model-inventory
entry states.

**11. Materiality or threshold.** All changes are logged regardless of size; materiality governs
whether a change requires *independent review* and *external approval*, and the threshold is the one
the *finance documents* state for a change to the agreed case — for example a stated movement in the
minimum *coverage ratio*, in the debt quantum or in a defined term. **PCI sets none of those figures:
they are creatures of the finance documents**, and where the documents are silent the *decision owner*
records the figure in the engagement's materiality statement. *Scale test:* on a small municipal
project the version register is a table in the model and the regression suite a dozen figures; on a
multi-billion cross-border financing the register is a controlled system of record, the regression
suite covers each tranche, currency and *reserve account*, and the locked close model is held with a
cryptographic integrity control so any later difference is detectable.

**12. Exception and waiver.** No exception is permitted to PR-01, PR-02 or PR-05. A departure from the
*timing* of PR-03 and PR-04 — running the regression suite immediately after acceptance rather than
before, during a live closing — may be approved in writing by the *decision owner*, once, for a period
not exceeding forty-eight hours, on condition that the version is not distributed outside the closing
team, that a *competent reviewer* is named to complete the run, and that the departure is reported to
the lenders' adviser and recorded in the change log.

**13. Escalation trigger.** A change to a locked model without the approval element 9 requires; an
unexplained movement in a regression figure; two versions in circulation bearing the same identifier;
an AI-made edit discovered in the authoritative version with no change-log entry; a difference between
the model at close and the model in the closing bible.

**14. AI application.** AI may produce version diffs as human-readable change lists, run the regression
suite, draft change-log entries for confirmation, identify every cell that moved between versions, and
detect that two circulating files bearing one identifier differ.

**15. AI prohibition.** AI must not approve a change, be recorded as a change's author, decide that a
regression difference is acceptable, release a version, or lift a lock on a *base case*.

**16. AI verification.** A named human must confirm, item by item, that every entry in the AI-produced
diff corresponds to an intended change; must write the explanation for every regression movement rather
than accept a machine-generated one; and must sign the version as released before it is used or
distributed. Method: clause-to-output comparison of the diff against the change log, plus independent
recomputation of any regression figure whose movement is *material*.

**17. External reference.**

- **ICAEW *Financial Modelling Code*.** Issuing organisation: ICAEW. Subject: version control and
  documentation of financial models. Checked: current, by name only (register `EXT-125`, verified
  2026-08-03). Nature: Manual §6 category 5 — professional framework. Applicability limitation:
  guidance; not certifiable; binding only where adopted.
- **ISO/IEC 42001 *Information technology — Artificial intelligence — Management system*.** Issuing
  organisation: ISO/IEC. Subject: organisational management of AI systems, including lifecycle and
  change controls. Checked: ISO/IEC 42001:2023, 1st edition (register `EXT-021`, verified 2026-08-03).
  Nature: Manual §6 category 3 — international voluntary standard. Applicability limitation: voluntary
  unless imported by law or contract; it addresses an organisation's AI management system, not a
  project's model, and imposes nothing on a financing of its own force.
- ***Supervisory Guidance on Model Risk Management* (SR 11-7 / OCC 2011-12).** Issuing organisation:
  United States banking supervisors. Subject: expectations for model development, implementation, use,
  validation and governance in supervised institutions. Checked: **not independently verified — verify
  current requirements** (register `EXT-102`). Nature: Manual §6 category 10 — illustrative practice;
  specifically **supervisory guidance, jurisdiction-specific, addressed to supervised institutions and
  not to project sponsors or advisers**. Applicability limitation: named for context only; **no
  requirement in this law is sourced to it**, and it is not law anywhere outside the supervisory
  relationship it governs.

**18. Jurisdictional caution.** Where the model is a contractual deliverable or a condition of
financing, the governing law of the *finance documents* determines which version governs, which changes
require consent, and what representation attaches to the locked case. Whether a cryptographic integrity
control constitutes evidence of a document's state, and whether an electronic signature on a release is
effective, are also jurisdiction-specific. Obtain qualified legal advice on the contractual status of
the model before amending an agreed version.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-12`; `PCI-FND-LAW-04`; `PCI-PFL-LAW-06.01`;
`PCI-PFL-LAW-13.04`; `PCI-PFL-LAW-16.01`. **Increment over the foundational parent:**
`PCI-FND-LAW-05` requires an audit trail and `PCI-FND-LAW-12` requires retention; this law adds what a
*transaction* model needs on top — a single authoritative version among many circulating copies, a
regression suite whose every movement is explained *before* acceptance, a human author on every change
including AI-made ones, and a locked close case that later work cannot silently overwrite.

**20. Related Body of Knowledge content.** PFL-AI · Domain 6 — Financial modelling · KA 6.4 Checks,
sensitivity, model audit and AI controls · topics: model audit and governance; AI-assisted modelling
controls. Also Domain 16 KA 16.3 (inventory, tiering and revalidation) and Domain 13 KA 13.4
(financial close).

**21. Compliance test.** A reviewer takes the version register, the change log, the regression results
and two consecutive released versions, and performs five steps. (a) Confirms exactly one version is
identified as authoritative at the output date. (b) Produces a diff between the two versions and
confirms every difference has a change-log entry with a named human author, a date, a reason and an
effect. (c) Confirms the change log contains no entry naming a tool or an AI system as author. (d)
Confirms a regression result exists for the later version and that every movement carries a written
explanation. (e) For a closed transaction, recomputes the integrity control on the locked *base case*
and confirms it matches the value recorded at close. Compliance is demonstrated when all five complete;
one undiffed change, or one unexplained movement, is a breach.

**22. Breach indicators.** Two files named "final"; a change log whose author column reads "system" or
"model update"; a regression movement annotated "rounding" with no figure; a locked close model whose
file date is later than close; a version identifier that repeats; an AI-generated rebuild of a sheet
with no log entry.

**23. Consequence within PCI authority.** Correction required and the affected version withdrawn from
use; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Scenario judgement: a locked close model has moved and the candidate
must state what may still be quoted, to whom, and what must be escalated. AI-verification case: an AI
system has rebuilt a schedule overnight and the candidate must state the log, regression and approval
steps required before the version is used. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-06-05` *Model Change Governance* (v1.0). Amendment note: **the v1.0
red-team revision expressed four of this law's five sub-obligations on the ISO requirement auxiliary;
all four are restated here on `must`, with no change of substance**, and each now has its own process
requirement identifier. *Change log* and *regression suite* defined; the locked-case rule separated as
PR-05; a bounded, reported element 12 route added for the live-closing timing case, which v1.0 left
unaddressed and which practitioners resolved by ignoring the rule.

---
## Domain 9 — Funding structure and sources of capital

### PCI LAW PCI-PFL-LAW-09.01 — The Capital-Structure Decision Basis

**1. Normative requirement.** A credential holder must not propose, recommend or accept a capital
structure whose feasibility depends on a future refinancing, a further contribution or a market
condition that has not been stated as an assumption with its owner and its consequence if it does not
occur.

**2. Purpose.** Gearing, tranching, tenor, amortisation profile and hedging are chosen together, and
the choice is usually defended on a return figure. A structure that only works if something else
happens later transfers a risk to whoever is holding it when the assumption fails, and the transfer is
invisible unless the assumption is written down.

**3. Scope.** Every credential holder who proposes, structures, models, reviews, recommends, approves
or provides assurance on a funding plan or capital structure — including gearing, tranche composition,
seniority, tenor, amortisation and sculpting profile, hedging strategy, and the cost of each
instrument — at screening, at sanction, at close, and on any restructuring or refinancing.

**4. Defined terms.** *CFADS*, *debt service*, *coverage ratio*, *finance documents*, *base case*,
*material*, *decision owner*, *evidence*, *decision-grade*, *competent reviewer*. **Structural
dependency** — an event outside the transaction's committed documents on which the structure's
feasibility relies: a refinancing at or before a stated date, a further equity or sponsor contribution
not yet committed, a rate or price level not hedged or contracted, or an approval not yet obtained.

**5. Required actions.**

- **PCI-PFL-LAW-09.01-PR-01 — Dependency schedule.** The preparer must schedule every *structural
  dependency*, stating the event, the date by which it must occur, the party who owns it, and the
  consequence for the structure if it does not occur.
- **PCI-PFL-LAW-09.01-PR-02 — Structure tested against cash, risk and coverage.** The preparer must
  demonstrate that the proposed structure is supported by the project's *CFADS* profile, by the risk
  allocation recorded under `PCI-PFL-LAW-11.01`, and by the *coverage ratios* the *finance documents*
  require, each shown separately.
- **PCI-PFL-LAW-09.01-PR-03 — Instrument-by-instrument cost on a comparable basis.** The preparer must
  state the all-in effective cost of each instrument, solved from that instrument's own cash-flow
  stream against its net proceeds, and must not compare instruments by headline rate or by adding fees
  to a rate.
- **PCI-PFL-LAW-09.01-PR-04 — Failure case.** The preparer must model and present the case in which
  each *material* structural dependency does not occur, and must state who bears the consequence.

**6. Prohibited actions.** Presenting a structure as feasible while a dependency is unstated; comparing
tranches on headline rates or on rate-plus-fees; treating an uncommitted sponsor contribution as
committed funding; presenting a capitalised premium or fee as free because no cash moves at the time;
sizing to a *coverage ratio* the finance documents do not use; describing a refinancing as an
expectation rather than as a dependency.

**7. Required evidence.** The dependency schedule; the *CFADS*, risk-allocation and coverage tests as
presented; the all-in effective cost calculation per instrument with its cash-flow stream; the failure
cases with their consequence statements; the *decision owner's* recorded approval of the structure.

**8. Responsible role.** The project finance leader accountable for the funding plan. The *decision
owner* for the sanction, mandate or credit decision that adopts the structure.

**9. Approval authority.** The decision owner approves the structure. Where the structure depends on a
party's future act, only that party can commit it, and no PCI law makes an expectation into a
commitment.

**10. Independence requirement.** A *competent reviewer* independent of the arranging, advisory or
sponsor benefit must review the dependency schedule and the failure cases before the structure is used
to support a sanction or a credit decision, because the arranging benefit runs to the transaction
proceeding.

**11. Materiality or threshold.** A dependency is *material* where its non-occurrence would breach a
*coverage ratio* the finance documents test, would leave a funding requirement unmet, or would require
a further contribution. The **coverage levels used are those the finance documents or the credit
approval state, never a figure invented by PCI or borrowed from another transaction**; where no
documents yet exist, the target used is the credit approver's stated target and it is labelled as
such. *Scale test:* on a small municipal project the structure may be one senior tranche and a grant,
and the schedule fits on one page; on a multi-billion cross-border financing with export credit,
development finance, bond and bank tranches across currencies, the schedule is maintained per tranche
and the failure case is run per dependency, because the tranches fail at different points.

**12. Exception and waiver.** No exception is permitted to element 1. A *decision owner* may approve
in writing the presentation of a structure carrying an unresolved dependency **provided the dependency
is on the face of the output with its owner, its date and its consequence** — which is compliance, not
exception. Where a failure case cannot be modelled because the consequence is not quantifiable, the
preparer must state that in words and the decision owner must record acceptance of an unquantified
exposure.

**13. Escalation trigger.** A structure whose feasibility rests on a refinancing within the tenor; a
dependency whose date passes unresolved; a sponsor contribution assumed but not documented; a hedging
assumption that the market no longer supports; a credit approval conditioned on a coverage level the
*base case* does not meet.

**14. AI application.** AI may generate structuring alternatives, solve all-in effective costs across
instruments, run gearing and tenor sensitivities, compute the coverage effect of each tranche mix,
and draft the dependency schedule for confirmation.

**15. AI prohibition.** AI must not select the capital structure, decide that a dependency is
acceptable, conclude that a refinancing is available, or approve a funding plan.

**16. AI verification.** Independent recomputation by a named human of the all-in effective cost of
each *material* tranche from its own cash-flow stream; sensitivity analysis over each dependency to
confirm the failure case the AI produced; and source tracing of every committed amount to the executed
or agreed-final-form document that commits it. An uncommitted amount traced only to a term sheet is
recorded as uncommitted.

**17. External reference.**

- **The Basel Framework.** Issuing organisation: the Basel Committee on Banking Supervision. Subject:
  the supervisory context in which lenders assess and hold project exposures. Checked: the consolidated
  framework as maintained by the BCBS; no standard, paragraph or date asserted (register `EXT-110`,
  verified 2026-08-03). Nature: Manual §6 category 10 — illustrative practice; specifically
  **internationally agreed supervisory standards with no legal force of their own**. Applicability
  limitation: the Committee has no supranational authority; its standards reach a bank only as a
  national authority transposes them, and they **never apply directly to a project, a sponsor or an
  adviser**. Named for context; **no requirement in this law is sourced to it**.
- **OECD *Arrangement on Officially Supported Export Credits*.** Issuing organisation: OECD. Subject:
  constraints participating agencies apply to officially supported export credits, including minimum
  premium rates, maximum repayment terms and starting points. Checked: **not independently verified —
  verify current requirements** (register `EXT-085`). Nature: Manual §6 category 10 — illustrative
  practice; specifically an **inter-governmental understanding, not a treaty and not legislation**.
  Applicability limitation: its terms are revised periodically and vary by sector, so they must be
  checked as at the transaction date rather than assumed; it binds participating agencies through their
  own practice, not the project.

**18. Jurisdictional caution.** Thin-capitalisation and interest-limitation rules, withholding tax on
interest and on profit distributions, stamp and registration duties on security, exchange controls,
the enforceability of subordination and intercreditor arrangements, and the insolvency treatment of
each instrument are all jurisdiction-specific and can determine which structure is available at all.
Sanctions and financial-crime obligations may also restrict which capital providers may participate.
Obtain qualified local tax and legal advice on each instrument and each jurisdiction in the structure
before the structure is recommended — see `PCI-PFL-LAW-12.02`.

**19. Related PCI Laws.** `PCI-FND-LAW-07`; `PCI-FND-LAW-08`; `PCI-PFL-LAW-05.01`;
`PCI-PFL-LAW-10.02`; `PCI-PFL-LAW-11.01`; `PCI-PFL-LAW-15.02`. **Increment over the foundational
parent:** `PCI-FND-LAW-07` forbids a misleading forecast; this law addresses the specific way a
funding plan misleads — by resting on an event nobody has committed to — and requires the dependency
to be scheduled, owned, dated, and modelled in failure.

**20. Related Body of Knowledge content.** PFL-AI · Domain 9 — Funding structure and sources of
capital · KA 9.1 Equity and shareholder instruments, KA 9.2 Senior, subordinated and mezzanine debt and
bonds, KA 9.3 Islamic finance concepts, export credit and development finance, KA 9.4 Government
support, grants, sustainable finance and refinancing. Also Domain 10 KA 10.1 (debt capacity and
sizing).

**21. Compliance test.** A reviewer takes the funding plan, the model and the committed-document set,
and performs four steps. (a) Lists every funding source in the plan and confirms each is traced to a
committed document, or appears on the dependency schedule. (b) Confirms each *material* dependency on
the schedule has an event, a date, a named owner and a consequence. (c) Recomputes the all-in effective
cost of two tranches from their own streams and obtains the figures stated, without unexplained
difference. (d) Runs each material dependency's failure case and confirms the consequence stated in the
plan. Compliance is demonstrated when all four complete; a funding source that is neither committed nor
scheduled as a dependency is a breach.

**22. Breach indicators.** A plan whose final tranche is labelled "refinancing"; a comparison table of
headline rates; a sponsor contribution shown as a source with no commitment letter; a capitalised
premium omitted from the cost comparison; a structure sized to a coverage target that appears in no
document; a failure case absent from the pack because "it does not arise".

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Calculation review: two tranches with different fee structures, where
the candidate must compare on all-in effective cost and explain why the headline comparison reverses.
Scenario judgement: a structure that only closes if a refinancing occurs in year seven — the candidate
must identify the dependency and state the required disclosure. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-09-01` *Capital-Structure Justification* (v1.0). Amendment note:
restructured onto the twenty-five-element form; *structural dependency* defined; the all-in
effective-cost discipline raised from prose to PR-03; the failure case made a process requirement; the
Basel characterisation tightened so that no requirement is sourced to it.

---

### PCI LAW PCI-PFL-LAW-09.02 — Accuracy in Describing Islamic-Finance Structures

**1. Normative requirement.** A credential holder must not state, imply, record or repeat that a
structure, instrument or transaction is Shariah-compliant unless a *Shariah compliance determination*
covering that specific structure has been issued by the body competent to issue it, and the credential
holder holds that determination and can produce it.

**2. Purpose.** A project finance leader must be able to price, model and compare structures used in
Islamic finance markets — that is a professional competence. **Whether a structure conforms to Islamic
law is not a modelling question and not a PCI question.** It is a determination of religious law made
by competent scholarly authority, and a professional who asserts it, implies it by labelling, or
carries it across from another transaction is making a representation they have no standing to make.
The reputational and commercial consequences of that misstatement fall on the institution, the
investors and the communities who relied on the label.

**3. Scope.** Every credential holder who describes, models, compares, structures, reviews, markets,
reports on or approves any structure used in Islamic finance markets — including istisna'a, ijara,
forward lease, murabaha, wakala, mudaraba, musharaka and sukuk forms — in any document, model,
presentation or communication, internal or external.

**4. Defined terms.** *Shariah compliance determination*, *evidence*, *material*, *decision owner*,
*finance documents*, *verified*, *decision-grade*, *source line*. **Competent body** — the scholar,
Shariah supervisory board or equivalent authority that the relevant institution, market or jurisdiction
recognises as competent to issue a *Shariah compliance determination*. **Economic description** — a
description of a structure by its cash flows, its asset and ownership arrangements, its risk allocation
and its all-in effective cost, containing no assertion about religious-law conformity.

**5. Required actions.**

- **PCI-PFL-LAW-09.02-PR-01 — Economic description first.** The credential holder must describe every
  such structure by its *economic description*, and must make that description sufficient to price,
  model and compare the structure without reference to any compliance assertion.
- **PCI-PFL-LAW-09.02-PR-02 — Determination record.** Where a *Shariah compliance determination* is
  relied upon, the credential holder must record its issuing *competent body*, its date, the exact
  structure and documents it covers, any conditions attached to it, and its stated limitations, and
  must attach that record as the figure's *source line*.
- **PCI-PFL-LAW-09.02-PR-03 — No extrapolation.** The credential holder must treat a determination as
  covering only the structure and documents it names, and must not extend it to a different structure,
  a different transaction, an amended document set or a later version without a further determination.
- **PCI-PFL-LAW-09.02-PR-04 — Pending status stated.** Where a determination has been sought and not
  yet issued, the credential holder must record the status as *pending* wherever the structure is
  described, must name the body from which it is sought, and must not describe the structure as
  compliant, expected to be compliant or market-standard.
- **PCI-PFL-LAW-09.02-PR-05 — Ownership and intercreditor consequences modelled.** The credential
  holder must model and disclose the economic consequences that follow from the structure's ownership
  arrangements — including any residual obligation retained by the lessor or issuer, the effect of a
  service-agency pass-back, and the ranking and enforcement arrangements between Islamic and
  conventional tranches — rather than assuming economic equivalence with a conventional tranche.

**6. Prohibited actions.** Describing a structure as Shariah-compliant, Islamic, halal or equivalent on
the strength of its form, its name, market practice, another transaction's determination or an
adviser's expectation; presenting a pending determination as obtained; presenting a determination as
covering documents it does not name; asserting that a structure would be approved; offering an opinion
on a question of religious law; presenting an Islamic tranche as economically identical to a
conventional one without modelling the ownership consequences; **describing PCI, a PCI credential or a
PCI law as evidence of, or as conferring competence over, Shariah compliance**.

**7. Required evidence.** The *economic description* as issued; the determination record under PR-02
with the determination itself; the record of any pending status and the body approached; the model
treatment of ownership obligations, service-agency pass-back and intercreditor ranking; the
withdrawal record where a determination is refused, withdrawn or conditioned.

**8. Responsible role.** The credential holder who prepares or issues the description, personally, for
the accuracy of what is stated. The *decision owner* for any decision taken on it.

**9. Approval authority.** **The *Shariah compliance determination* is outside PCI's authority
entirely.** PCI does not make, review, endorse, accredit, register or overrule such a determination,
and no PCI law, credential, examination, syllabus or process confers competence to make one. Only the
*competent body* determines compliance. The *decision owner* approves the credential holder's
description of the structure; the decision owner cannot approve a compliance assertion, because the
authority to make one is not theirs to give.

**10. Independence requirement.** Not applicable to the determination — **the reason being that the
determination is made by an external authority whose composition, independence and standing are
governed by the recognising institution, market or jurisdiction, not by PCI**. Independence *is*
required for the review of the credential holder's *economic description* and model treatment where
those support a lender submission or an offering document, and is discharged under
`PCI-PFL-LAW-13.01`.

**11. Materiality or threshold.** No materiality threshold applies to element 1: a compliance assertion
is either supported by a producible determination or it is not, and there is no size below which an
unsupported assertion is acceptable. Materiality governs only the *depth* of the PR-05 modelling — the
*decision owner* records, in the engagement's materiality statement, the movement in the transaction's
own metric at which an ownership or pass-back consequence must be separately quantified rather than
described. *Scale test:* on a small municipal project with a single ijara facility, PR-05 is a short
schedule of the lessor's retained obligations and their annual cost; on a multi-billion cross-border
financing combining a sukuk, a bank ijara and conventional tranches across jurisdictions, PR-05 covers
the intercreditor ranking, the enforcement waterfall and the treatment of each tranche on acceleration,
and each is modelled separately because the tranches do not fail together.

**12. Exception and waiver.** **No exception is permitted, and none may be granted by anyone.** PCI has
no authority to waive a requirement whose subject matter is a determination it does not make. A
structure described only by its *economic description*, with no compliance assertion, needs no
exception — it is already compliant with this law.

**13. Escalation trigger.** A request to describe a structure as compliant before a determination is
issued; a determination that is refused, withdrawn, made conditional or limited in scope; a document
amendment after a determination was issued; discovery that a marketing document carries a compliance
label the determination does not support; a determination whose issuing body the counterparty does not
recognise as competent.

**14. AI application.** AI may explain the economic mechanics of these structures, build and compare
their cash-flow streams, solve all-in effective costs, extract the ownership and pass-back obligations
from a document set for human confirmation, and draft the *economic description*.

**15. AI prohibition.** **AI must not determine, predict, assert, imply or certify Shariah compliance,
in any form or with any hedge.** AI must not draft a compliance assertion, must not summarise a
determination in a way that widens its scope, and must not be recorded as a source for a compliance
statement. A machine's account of what is or is not compliant carries no authority whatever and must
not be repeated in any output.

**16. AI verification.** Clause-to-output comparison, by a named human, of every AI-produced statement
about a determination against the determination document itself — its issuer, its date, the structure
and documents named, and its conditions; independent recomputation of every AI-produced all-in
effective cost from its own stream; and source tracing of each ownership obligation to the executed
document that creates it. **Any AI output containing a compliance assertion is deleted rather than
corrected**, so that it cannot be recovered from a draft.

**17. External reference.**

- **Standards issued by the Accounting and Auditing Organisation for Islamic Financial Institutions
  (AAOIFI).** Issuing organisation: AAOIFI. Subject: accounting, auditing, governance and Shariah
  standards used in Islamic finance markets. Checked: **named only; no standard, number, edition or
  date is asserted, and none was verified.** Nature: Manual §6 category 5 — professional framework.
  Applicability limitation: **a determination for a specific structure is made by the *competent body*,
  not by a standard**; AAOIFI standards are mandatory only where a jurisdiction's regulator or an
  institution's own constitution has adopted them, and whether they apply to a given transaction is a
  question for that regulator, that institution and qualified local counsel. **Not currently registered
  in `../registries/EXTERNAL_AUTHORITIES.md`** — recorded as an open item in the audit findings below.
- **ISO 37001 *Anti-bribery management systems — Requirements with guidance for use*.** Issuing
  organisation: ISO. Subject: anti-bribery management systems, named here because multi-source Islamic
  and export-credit structures commonly involve intermediaries and agency arrangements. Checked: ISO
  37001:2025, superseding ISO 37001:2016 (register `EXT-133`, verified 2026-08-04). Nature: Manual §6
  category 3 — international voluntary standard. Applicability limitation: adoption is voluntary unless
  a contract or regulator requires it, and **certification against it is a third party's opinion about
  a management system at a point in time, not a legal defence**.

**18. Jurisdictional caution.** Whether a determination is required at all, which body is recognised as
competent, whether that body's rulings have legal effect, how an Islamic structure is characterised for
tax, whether ownership under an ijara or sukuk creates registrable title or a security interest, and
how such a structure is treated on insolvency or enforcement, are **all jurisdiction-specific and
differ sharply even between jurisdictions with substantial Islamic finance markets**. Some
jurisdictions have adopted a national standard-setting body; others leave the question to each
institution's own board. Obtain qualified local legal, tax and Shariah advice for the specific
structure, the specific documents and the specific jurisdiction, and do not carry an answer across a
border.

**19. Related PCI Laws.** `PCI-FND-LAW-08` (competence boundaries and referral); `PCI-FND-LAW-07`;
`PCI-FND-LAW-14`; `PCI-PFL-LAW-06.04`; `PCI-PFL-LAW-09.01`; `PCI-PFL-LAW-12.02`. **Increment over the
foundational parent:** `PCI-FND-LAW-08` requires a professional to work within competence and to refer
beyond it; this law identifies one specific boundary that a finance professional crosses easily and
almost always inadvertently — a label — and converts the referral duty into a producible determination,
a no-extrapolation rule, a pending-status rule and a standing statement that PCI has no authority over
the determination at all.

**20. Related Body of Knowledge content.** PFL-AI · Domain 9 — Funding structure and sources of
capital · KA 9.3 Islamic finance concepts, export credit and development finance · topic 9.3.1 Islamic
finance structures in economic terms, whose scope statement records that compliance is a determination
for the relevant Shariah supervisory board and is outside the book's scope. Also Domain 12 KA 12.3
(guarantees, direct agreements and the security package).

**21. Compliance test.** A reviewer takes every document in which the structure is described and
performs four steps. (a) Searches for every compliance assertion — the words *Shariah*, *Sharia*,
*Islamic*, *halal* and every equivalent label used as a qualifier of the structure — and lists each
occurrence. (b) For each occurrence, requires production of the *Shariah compliance determination* and
confirms that the document produced is issued by the named *competent body*, is dated, and names the
structure and the document set actually used. (c) Confirms that no determination is relied upon for a
structure, transaction or document version it does not name. (d) Confirms the *economic description*
alone is sufficient to price and model the structure — by pricing it from that description and
obtaining the figure in the output. Compliance is demonstrated when all four complete; **one
unsupported assertion is a breach**, and a pending determination presented as obtained is a breach of
element 6.

**22. Breach indicators.** A term sheet headed with a compliance label and no determination in the
file; a determination dated before the last document amendment; a determination for a sister project
in the file for this one; a model comparing an ijara tranche to a conventional tranche on rate alone; a
presentation describing a structure as "market-standard Shariah-compliant"; an AI-drafted paragraph
explaining why a structure is compliant; a service-agency pass-back described but never costed.

**23. Consequence within PCI authority.** Correction required and the affected description or output
withheld from use until corrected; additional independent review; escalation to the decision owner;
failure of the associated examination competency; ethics review; certification investigation;
suspension or withdrawal of the credential. Each subject to due process and a right of appeal (Charter
§9). **PCI claims no other consequence, and specifically claims no authority to rule on, validate or
invalidate any question of religious law.**

**24. Examination application.** Scenario judgement: a candidate is asked to approve marketing material
carrying a compliance label where the file contains a determination for an earlier document version,
and must identify the defect and the required action. Calculation review: comparing an ijara tranche
and a conventional tranche on all-in effective cost, where the headline ranking reverses.
Ethical dilemma: a client asks the candidate to confirm that a structure "would obviously be approved".
No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law — no v1.0 predecessor.** Amendment note: v1.0 of this set contained no law on this
subject, and the Body of Knowledge carried the boundary in a chapter scope statement only, where under
Charter §3 it could not create an obligation. This law places the obligation where it can be assessed
and breached.

---

### PCI LAW PCI-PFL-LAW-09.03 — Sustainable-Finance Claims

**1. Normative requirement.** A credential holder must not make, model, certify, publish or repeat a
*sustainability claim* at a strength that the identified supporting *evidence* does not support.

**2. Purpose.** A label is cheap to attach and expensive to defend. A green, social or
sustainability-linked designation, a taxonomy-alignment assertion or an environmental, social or
governance metric quoted outside the model that produced it can each outrun its evidence in one
sentence — and the gap between the claim and the evidence is precisely what a greenwashing allegation,
a regulatory enquiry or an investor claim is about. The professional risk is not scepticism about
sustainability; it is a claim nobody can now substantiate.

**3. Scope.** Every credential holder who prepares, models, reviews, recommends, approves, publishes or
provides assurance on a *sustainability claim* in connection with a financing — in a term sheet,
information memorandum, framework document, allocation or impact report, covenant certificate,
marketing material, press release or investor communication — and every credential holder who repeats
such a claim in their own output.

**4. Defined terms.** *sustainability claim*, *voluntary framework*, *evidence*, *verified*,
*material*, *decision owner*, *finance documents*, *competent reviewer*, *source line*, *independent*.
**Claim strength** — what a claim asserts and how firmly: the difference between *aligned with*,
*assessed against*, *intended to meet* and *certified as meeting* is a difference in strength, and each
requires different evidence. **Second-party opinion** — an external provider's opinion on a framework
or an instrument, whose scope, method and limitations are stated in the opinion itself.

**5. Required actions.**

- **PCI-PFL-LAW-09.03-PR-01 — Claim register.** The preparer must record every *sustainability claim*
  made in a decision-grade output, with its exact wording, its *claim strength*, the framework, label,
  taxonomy or metric relied on, the *evidence* relied on, the verifier where one exists, and the date.
- **PCI-PFL-LAW-09.03-PR-02 — Strength matched to evidence.** The preparer must state each claim at the
  strength the evidence supports, and must not use a certification word where the evidence is an
  assessment, or an assessment word where the evidence is an intention.
- **PCI-PFL-LAW-09.03-PR-03 — Voluntary status stated.** The preparer must identify each framework,
  label or principle set relied on as voluntary where it is voluntary, and **must not present any
  voluntary framework as legislation, regulation or law**; where a jurisdiction's enacted taxonomy,
  labelling or disclosure regime is relied on, the preparer must name the jurisdiction, must state that
  the regime is legislation in that jurisdiction only, and must not extend it to another.
- **PCI-PFL-LAW-09.03-PR-04 — Metric definition and measurement.** The preparer must record, for every
  performance metric or key performance indicator relied on, its definition, its measurement method,
  its measurement boundary, its reference point, its assurance arrangement and who performs it — and
  must not quote a metric outside the model, report or measurement system that produced it without
  those particulars.
- **PCI-PFL-LAW-09.03-PR-05 — Economics of the label disclosed.** Where a claim attaches to a
  sustainability-linked instrument, the preparer must present the margin effect and the cost of the
  measurement, assurance and reporting apparatus over the life of the facility on the same basis, and
  must state whether the ratchet is symmetric.
- **PCI-PFL-LAW-09.03-PR-06 — Withdrawal on lapse.** The preparer must withdraw or restate a claim
  where its evidence lapses, is superseded, is withdrawn by its verifier, or ceases to support the
  strength stated, and must notify every party known to be relying on it.

**6. Prohibited actions.** Describing a *voluntary framework* as legislation, regulation or law;
presenting one jurisdiction's taxonomy as universal; using *certified*, *compliant* or *verified* where
the evidence is an assessment, an opinion or an intention; quoting a metric with no definition,
boundary or reference point; presenting a *second-party opinion* as assurance over performance when it
is an opinion on a framework; selecting a key performance indicator the project would meet without
change and presenting it as a target; presenting a label as the reason for a pricing benefit that the
evidence attributes elsewhere; carrying a claim forward after its evidence has lapsed.

**7. Required evidence.** The claim register with all seven fields; the underlying evidence for each
claim, retained under `PCI-PFL-LAW-06.04-PR-03`; each *second-party opinion*, external review,
assurance report or certificate, with its stated scope and limitations; the metric definitions and
measurement records; the PR-05 economics; the withdrawal and notification records.

**8. Responsible role.** The credential holder who prepares or issues the claim, personally. The
*decision owner* for the decision taken on it, and for the issue of any external communication carrying
it.

**9. Approval authority.** The decision owner approves the issue of a claim. **No credential holder may
approve a claim about their own performance against a metric they measure and report on** without the
independent element required by element 10. PCI approves no claim and certifies no instrument.

**10. Independence requirement.** Where a claim asserts performance against a metric, the measurement
or the assurance over it must be performed by a person or firm *independent* of the party whose
performance is measured — the *finance documents* commonly require this, and where they do not, the
independence requirement in this law still applies to the assurance step. A *second-party opinion*
provider is independent of the issuer but is **not** thereby an assurer of performance, and its opinion
must not be presented as one.

**11. Materiality or threshold.** The threshold is the **evidence**, not a percentage: a claim is
compliant when the evidence identified in the register supports the strength stated, and no proportion
of eligible assets, no share of proceeds and no metric level set by PCI enters the test. Where the
*finance documents*, a framework or a jurisdiction's regime state an eligibility proportion, a
reporting threshold or a target level, **that documented figure is the one used and tested**, and it is
recorded with its source. *Scale test:* on a small municipal project a single grant-funded efficiency
measure may support one narrow claim, and the register is a few rows — the law's burden is proportionate
because the number of claims is small. On a multi-billion cross-border financing with a green framework,
a second-party opinion, several jurisdictional taxonomies and a sustainability-linked margin ratchet,
the register is maintained per claim and per jurisdiction, and PR-03 does the heaviest work because the
regimes differ and none of them is global.

**12. Exception and waiver.** No exception is permitted to element 1 or to PR-03. A *decision owner*
may approve in writing a claim stated at a **lower** strength than the evidence would support — that is
always available and needs no exception. Where a claim must be made before its verification is
complete, the claim must state that verification is outstanding, name the verifier and the expected
date, and be withdrawn if verification is not obtained; that approval is recorded, is limited to the
named output, and lasts only until the expected date.

**13. Escalation trigger.** A verifier declining, qualifying or withdrawing an opinion; a metric that
cannot be measured on the recorded boundary; a target met without any change in the project; a
jurisdiction's regime changing so that an alignment claim no longer holds; an allegation of
greenwashing; a request to state a claim more firmly than the register supports; a ratchet whose
symmetric downside has become likely.

**14. AI application.** AI may map project attributes against a named framework's eligibility criteria
for human review, assemble the claim register, compute the PR-05 economics across cases, compare a
draft claim's wording against the evidence recorded for it, and flag claims whose evidence dates have
passed.

**15. AI prohibition.** AI must not determine that a project, asset or expenditure qualifies under a
label, framework or taxonomy; must not decide a claim's *claim strength*; must not certify, verify or
assure a metric; must not produce a *second-party opinion* or anything presented as one; and must not
be recorded as the source of any sustainability claim.

**16. AI verification.** Clause-to-output comparison, by a named human, of each AI-drafted claim against
the framework text or regime it cites and against the evidence in the register; source tracing of every
metric value to the measurement record that produced it; independent recomputation of the PR-05
economics; and a named human's recorded confirmation that the strength word used matches the evidence
class. **An AI-generated eligibility conclusion is a hypothesis for human checking and must never be
recorded as an eligibility determination.**

**17. External reference.**

- **Voluntary market principles for green, social and sustainability-linked instruments.** Issuing
  organisations: international market associations. Subject: voluntary process guidelines for
  use-of-proceeds and sustainability-linked instruments, covering use of proceeds, project evaluation,
  management of proceeds, reporting and, for linked instruments, key performance indicator selection
  and target calibration. Checked: **named generically; no publisher, title, edition or date is
  asserted, and none was verified** — consistent with the PFL-AI manuscript, which also names them
  generically. Nature: Manual §6 category 8 — **voluntary environmental or social framework**.
  Applicability limitation: **voluntary; adoption is the whole of their force; none of them is a global
  standard and none is legislation anywhere.**
- **Jurisdictional taxonomy, labelling and disclosure regimes.** Issuing organisations: national and
  supranational legislatures and regulators. Subject: criteria for describing an economic activity,
  instrument or fund as environmentally or socially qualifying, and the disclosures required.
  Checked: **named generically; no regime, article, threshold or date is asserted, and none was
  verified.** Nature: Manual §6 category 1 — applicable legislation or regulation, **but only within
  the enacting jurisdiction and only where the entity is in scope**. Applicability limitation: these
  regimes differ materially between jurisdictions and change frequently; **treating one jurisdiction's
  taxonomy as universal is the characteristic error in this area**. Whether a given regime applies to a
  given entity or instrument is a question for qualified local counsel.
- **IFC *Performance Standards on Environmental and Social Sustainability*.** Issuing organisation:
  IFC, World Bank Group. Subject: environmental and social performance expectations used as a reference
  benchmark. Checked: 2012 edition; Sustainability Framework update in progress (register `EXT-083`,
  verified 2026-08-03). Nature: Manual §6 category 8 — voluntary environmental or social framework.
  Applicability limitation: binding on IFC clients by contract; on others only where adopted. **Status
  is moving — verify the current position.**
- **The Equator Principles.** Issuing organisation: the Equator Principles Association. Subject: a
  voluntary framework under which adopting financial institutions apply agreed environmental and social
  requirements. Checked: EP4, adopted 18 November 2019, effective 1 October 2020 (register `EXT-082`,
  verified 2026-08-03). Nature: Manual §6 category 8 — voluntary environmental or social framework.
  Applicability limitation: **voluntary; never legislation**; applies only to transactions of adopting
  institutions, as those institutions apply it.

**18. Jurisdictional caution.** Taxonomy, labelling, fund-naming, disclosure and anti-greenwashing
rules are jurisdiction-specific, are being introduced and amended rapidly, and can attach liability to
a statement that was accurate when made. Whether a claim constitutes a misleading commercial practice,
a misrepresentation, a prospectus liability or a consumer-protection breach is a question of local law,
and the answer differs by jurisdiction, by instrument and by investor type. Obtain qualified local
legal advice before any sustainability claim is published, and re-take that advice before the same
claim is repeated in another jurisdiction.

**19. Related PCI Laws.** `PCI-FND-LAW-07` (honesty in reporting); `PCI-FND-LAW-13`;
`PCI-FND-LAW-14`; `PCI-PFL-LAW-06.04`; `PCI-PFL-LAW-09.01`; `PCI-PFL-LAW-10.04`. **Increment over the
foundational parent:** `PCI-FND-LAW-07` requires honest reporting; this law addresses the specific
mechanism by which a sustainability statement becomes dishonest without anyone lying — strength drift
between *intended*, *assessed*, *aligned* and *certified* — and adds the register, the voluntary-status
rule, the metric particulars, the ratchet economics and a positive duty to withdraw when evidence
lapses.

**20. Related Body of Knowledge content.** PFL-AI · Domain 9 — Funding structure and sources of
capital · KA 9.4 Government support, grants, sustainable finance and refinancing · topic 9.4.3 green
and sustainability-linked finance. Also Domain 5 KA 5.3 (the bankability test) and Domain 11 KA 11.4
(environmental and social, technology, cybersecurity and AI model risk).

**21. Compliance test.** A reviewer takes every external document carrying a claim and the claim
register, and performs five steps. (a) Lists every *sustainability claim* in the documents and confirms
each appears in the register with its exact wording. (b) For each, retrieves the evidence named and
confirms it is current, in scope, and of a class that supports the strength word used — an opinion for
*assessed*, a certificate within its validity for *certified*, a measurement record for a performance
statement. (c) Confirms every framework named is labelled voluntary where it is voluntary, and that no
document describes a voluntary framework as legislation. (d) For each metric, confirms the definition,
boundary, reference point and assurance arrangement are recorded and that the quoted value reconciles
to the measurement record. (e) Confirms the PR-05 economics were presented where a ratchet exists.
Compliance is demonstrated when all five complete; **a claim whose evidence is of a weaker class than
its strength word is a breach**, as is any description of a voluntary framework as law.

**22. Breach indicators.** The word *compliant* attached to a voluntary framework; a taxonomy named
without its jurisdiction; a second-party opinion cited as assurance over outcomes; a key performance
indicator with no measurement boundary; an impact figure quoted with no measurement record behind it; a
ratchet presented as the commercial case with no cost of the apparatus alongside; a claim repeated in a
second jurisdiction with no fresh advice; a claim whose supporting review expired last year.

**23. Consequence within PCI authority.** Correction required and the affected claim or output withheld
from use until corrected; additional independent review; escalation; failure of the associated
examination competency; ethics review; certification investigation; suspension or withdrawal of the
credential. Each subject to due process and a right of appeal (Charter §9). PCI claims no other
consequence, and PCI neither certifies nor endorses any instrument, label or framework.

**24. Examination application.** Scenario judgement: a draft press release says an instrument is
"certified as taxonomy-aligned" where the file holds a second-party opinion on a framework — the
candidate must identify the strength defect and restate the claim. Calculation review: the ratchet
economics of a sustainability-linked facility against the cost of its verification apparatus. Ethical
dilemma: an arranger asks for a firmer word than the evidence supports. No live examination content is
exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law — no v1.0 predecessor.** Amendment note: v1.0 contained no law on sustainability
claims, which left the corpus's most exposed representation area governed only by a general honesty
obligation. *Claim strength* and *second-party opinion* defined; the voluntary-versus-legislation rule
made an identified process requirement rather than a caution; external references named generically so
that no publisher, edition or clause is asserted without verification.

---
## Domain 10 — Debt sizing, covenants and credit metrics

### PCI LAW PCI-PFL-LAW-10.01 — The CFADS Definition

**1. Normative requirement.** A credential holder must compute *CFADS* on the definition the *finance
documents* state, item by item, and must not substitute a market convention, a textbook formulation or
another transaction's definition.

**2. Purpose.** Every coverage number, every sizing calculation, every *distribution* test and every
lock-up trigger is built on one defined quantity. Two financings of the same asset can define that
quantity differently and both be right — so a definition borrowed from anywhere but the documents can
flatter or breach a covenant that was never tested, and the error is invisible in the ratio.

**3. Scope.** Every credential holder who computes, reviews, certifies, reports or relies upon *CFADS*
or any quantity derived from it, at sizing, at close, in operation, at *distribution* testing, in
restructuring and in refinancing. Applies to preparation, review, certification and assurance.

**4. Defined terms.** *CFADS*, *debt service*, *coverage ratio*, *finance documents*, *authoritative
version*, *source line*, *material*, *evidence*, *decision owner*, *verified*, *competent reviewer*.
**Definition schedule** — an item-by-item statement of what is included in, and excluded from, *CFADS*
for this transaction, each item carrying the *source line* of the document and defined term that puts
it there.

**5. Required actions.**

- **PCI-PFL-LAW-10.01-PR-01 — Definition schedule.** The preparer must maintain a *definition schedule*
  for *CFADS* and for *debt service*, each item sourced to the defined term in the *finance documents*
  that establishes it.
- **PCI-PFL-LAW-10.01-PR-02 — Model line matches schedule.** The preparer must build the model's
  *CFADS* line to the definition schedule, item for item, and must reconcile the model line to the
  schedule at each release.
- **PCI-PFL-LAW-10.01-PR-03 — Reconciliation to the statements.** The preparer must reconcile the
  *CFADS* line to the financial statements for every period in which both exist, and must explain each
  reconciling item.
- **PCI-PFL-LAW-10.01-PR-04 — Pre-document definitions labelled.** Where no finance documents yet
  exist, the preparer must write out the modelled definition in full and label it as the modeller's
  definition, and must not describe it as the transaction's definition.
- **PCI-PFL-LAW-10.01-PR-05 — Re-derivation on amendment.** The preparer must re-derive the definition
  schedule and re-reconcile the model line whenever a defined term is amended or waived.

**6. Prohibited actions.** Including in *CFADS* an item the definition excludes, or excluding one it
includes; adopting a market or textbook definition where the documents state one; carrying a definition
across from another transaction; netting an excluded item against an included one; presenting a
modeller's definition as the transaction's; leaving a definition unchanged after an amendment changes
it.

**7. Required evidence.** The definition schedule with a *source line* per item; the model-to-schedule
reconciliation at each release; the *CFADS*-to-statements reconciliation with reconciling items
explained; the re-derivation records following each amendment or waiver.

**8. Responsible role.** The project finance leader or finance director who prepares or signs the
calculation. The *decision owner* for the sizing, certificate or report that relies on it.

**9. Approval authority.** The decision owner approves the calculation for use. **Only the parties to
the finance documents can change the definition**, by amendment in the form those documents require; no
PCI law and no professional judgement can change it.

**10. Independence requirement.** A *competent reviewer* independent of preparation must confirm the
definition schedule against the executed documents before financial close, before any certificate on
which a *lock-up* or default depends, and at model audit.

**11. Materiality or threshold.** The definition is applied in full regardless of an item's size:
there is no *de minimis* under which an excluded item may be included. Materiality governs *escalation*
and *re-review*: the *decision owner* records, in the engagement's materiality statement, the movement
in the minimum *coverage ratio* at which a discovered definitional difference must be escalated beyond
the preparer. **PCI sets no figure, and PCI sets no definition** — both belong to the finance documents
and to the parties. *Scale test:* on a small municipal project the schedule may run to a dozen items on
one page; on a multi-billion cross-border financing it is maintained per facility and per currency,
because tranches routinely define the same words differently and a single consolidated line would be
wrong for at least one of them.

**12. Exception and waiver.** No exception is permitted. Where the documents are genuinely ambiguous,
the preparer must record the ambiguity, compute on each reading, present both, and refer the
construction question to qualified counsel under `PCI-PFL-LAW-12.02` — the preparer must not resolve it
by choosing.

**13. Escalation trigger.** A definitional difference between the model and the documents; an amendment
or waiver that changes a defined term; a reconciling item between *CFADS* and the statements that
cannot be explained; an adjustment that improves a ratio and has not previously been applied; two
facilities whose definitions of the same term conflict.

**14. AI application.** AI may extract defined terms and their component references from the document
set into a draft definition schedule, compare the model line to that schedule item by item, identify
differences between two facilities' definitions, and draft the reconciliation for confirmation.

**15. AI prohibition.** AI must not interpret a defined term, decide whether an item falls inside a
definition, resolve an ambiguity, certify a *CFADS* figure, or approve a definition schedule.

**16. AI verification.** Clause-to-output comparison, by a named human, of every AI-extracted item
against the defined term in the executed document — reading the defining clause, not a summary of it;
independent recomputation of the *CFADS* line for the binding period and one other period from source
lines; and reconciliation of the recomputed line to the financial statements. Recorded per period with
the reviewer's name and date.

**17. External reference.**

- **IAS 7 *Statement of Cash Flows*.** Issuing organisation: IFRS Foundation / IASB. Subject: the
  presentation and classification of cash movements against which a modelled cash line is reconciled.
  Checked: current, by name only (register `EXT-120`, verified 2026-08-03). Nature: Manual §6 category
  2 — authoritative financial-reporting standard. Applicability limitation: mandatory only for entities
  applying IFRS Accounting Standards in a jurisdiction that has adopted them. **It defines no *CFADS*
  and no *coverage ratio*: those are creatures of the finance documents.** Verify current requirements.

**18. Jurisdictional caution.** The construction of a defined term is a question of the governing law
of the *finance documents*, and the same words can be read differently under different governing laws.
Cash tax, withholding, group relief and permitted deductions — all of which sit inside most *CFADS*
definitions — are jurisdiction-specific and change with legislation. Obtain qualified legal advice on
construction and qualified local tax advice on the tax components, and never resolve a construction
question inside the model.

**19. Related PCI Laws.** `PCI-FND-LAW-06`; `PCI-FND-LAW-07`; `PCI-PFL-LAW-01.01`;
`PCI-PFL-LAW-10.02`; `PCI-PFL-LAW-10.03`; `PCI-PFL-LAW-10.04`; `PCI-PFL-LAW-15.01`. **Increment over
the foundational parent:** `PCI-FND-LAW-06` requires data integrity and lineage; this law names the one
quantity in a financing whose *definition* — not its data — is the usual point of failure, and requires
an item-by-item schedule sourced to the defining clause, reconciled to the model and to the statements,
and re-derived on every amendment.

**20. Related Body of Knowledge content.** PFL-AI · Domain 10 — Debt sizing, covenants and credit
metrics · KA 10.1 Debt capacity and sizing · topic 10.1.1 cash available for debt service. Also Domain
2 KA 2.3 (project-relevant treatments), Domain 6 KA 6.2 (the *CFADS* tie) and Domain 15 KA 15.1
(operational monitoring and covenant testing).

**21. Compliance test.** A reviewer takes the executed documents, the definition schedule and the model
*authoritative version*, and performs four steps. (a) Reads the defined terms for *CFADS* and *debt
service* in the documents and confirms every component appears in the schedule, and that the schedule
contains nothing the documents do not. (b) Traces each schedule item to the model line and confirms it
is included or excluded as scheduled. (c) Recomputes *CFADS* for two periods from source lines and
obtains the model figures without unexplained difference. (d) Reconciles those figures to the financial
statements, with every reconciling item explained. Compliance is demonstrated when all four complete;
one item in the model that the definition excludes is a breach, whatever its size.

**22. Breach indicators.** A *CFADS* line built from a template; a definition schedule with no clause
references; an adjustment appearing for the first time in the period that would otherwise fail; two
facilities reported on one consolidated *CFADS*; a reconciling item labelled "timing"; a definition
unchanged after an amendment that redefined a component.

**23. Consequence within PCI authority.** Correction required and the affected calculation withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Calculation review: a candidate is given a defined term and a model
line that differ by one item and must find it and quantify its effect on the minimum ratio. Evidence
selection: choosing the document that establishes a component of the definition. No live examination
content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law** — the subject was carried inside `PFL-LAW-10-01` (v1.0), which bundled the
definition, the calculation and the reporting into one rule. Amendment note: separated so that the
definition has its own obligation, its own evidence and its own test; *definition schedule* defined;
the pre-document labelling rule and the re-derivation-on-amendment rule added.

---

### PCI LAW PCI-PFL-LAW-10.02 — Debt Sizing

**1. Normative requirement.** A credential holder must size debt to the coverage level, the tenor and
the amortisation profile that the *finance documents* or the credit approval state, and must not size
to a level chosen because it produces a preferred quantum.

**2. Purpose.** Debt capacity is a function of cash, coverage, rate and tenor, and a small movement in
the target coverage level moves the quantum a long way. Sizing to a level nobody has approved transfers
risk to the lender who later discovers the basis, and to the sponsor who has built a funding plan on a
quantum that will not survive credit.

**3. Scope.** Every credential holder who sizes, reviews, recommends, approves or provides assurance on
a debt quantum, a sculpting profile, a repayment schedule or a debt-capacity conclusion, at screening,
sanction, close, resizing, restructuring and refinancing.

**4. Defined terms.** *CFADS*, *debt service*, *coverage ratio*, *finance documents*, *base case*,
*material*, *decision owner*, *evidence*, *competent reviewer*, *verified*, *authoritative version*.
**Sizing basis** — the recorded set comprising the target *coverage ratio* and its source, the tenor,
the amortisation or sculpting convention, the assumed rate and hedging, the tax treatment fed back into
*CFADS*, and the case from which the *CFADS* profile is taken.

**5. Required actions.**

- **PCI-PFL-LAW-10.02-PR-01 — Record the sizing basis.** The preparer must record the *sizing basis*
  in full before the quantum is quoted, and must attach the *source line* of the document or approval
  that establishes the target coverage level.
- **PCI-PFL-LAW-10.02-PR-02 — Size from the documented case.** The preparer must take the *CFADS*
  profile from the case the *finance documents* or credit approval identify, and must not size from a
  more favourable case.
- **PCI-PFL-LAW-10.02-PR-03 — Sensitivity of the quantum.** The preparer must present the movement in
  the debt quantum produced by a stated change in each of the target coverage level, the rate, the
  tenor and the principal *CFADS* drivers, so that the quantum's fragility is visible.
- **PCI-PFL-LAW-10.02-PR-04 — Sculpting disclosed.** Where the profile is sculpted, the preparer must
  state the sculpting convention, the coverage level held, the treatment of the interest tax shield in
  the feedback loop, and the resulting profile's sensitivity to the rate.

**6. Prohibited actions.** Sizing to a coverage level that appears in no document or approval; taking
the *CFADS* profile from an upside case; solving for a quantum first and reporting the coverage that
results as the basis; presenting a sculpted profile without its convention; omitting a required
deduction from *debt service* in the sizing loop; quoting a debt capacity with no tenor or rate stated.

**7. Required evidence.** The recorded *sizing basis* with its source lines; the sizing calculation
traceable to the model *authoritative version*; the sensitivity presentation under PR-03; the sculpting
statement under PR-04; the credit approval or term sheet establishing the target level; the *decision
owner's* recorded approval of the quantum.

**8. Responsible role.** The project finance leader accountable for the sizing. The credit approver, on
the lender side, for the quantum adopted.

**9. Approval authority.** The credit approver for the lender's quantum; the *decision owner* for the
sponsor's funding plan. A change to the target coverage level requires the approval of the party whose
document or approval established it.

**10. Independence requirement.** A *competent reviewer* independent of the arranging or sponsor
benefit must review the *sizing basis* and the sensitivity presentation before the quantum is used to
support a credit decision or a funding plan.

**11. Materiality or threshold.** **The coverage level used is the one the finance documents or the
credit approval state — PCI does not set it, does not recommend one, and does not publish a range.**
Where no document yet states a level, the preparer records the credit approver's stated target and
labels it as an assumption under `PCI-PFL-LAW-09.01-PR-01`. Materiality governs escalation: the
*decision owner* records the movement in quantum, in the transaction's own units, at which a change of
basis must be escalated. *Scale test:* on a small municipal project a single target level, one tranche
and a level-repayment profile make the sizing basis a short paragraph; on a multi-billion cross-border
financing each tranche can carry its own target level, tenor and convention, and the basis is recorded
per tranche because a consolidated statement would be true of none of them.

**12. Exception and waiver.** No exception is permitted to element 1. A quantum may be quoted on an
assumed target level before any document exists, provided the level is labelled as an assumption, its
owner is named, and the sensitivity under PR-03 is presented with it; that is compliance. A waiver of
PR-02 is not available.

**13. Escalation trigger.** A quantum that requires a target level lower than the credit approval
states; a sizing case that differs from the documented case; a sensitivity showing the quantum moving
*materially* on a small change in the target level; a sculpting convention that produces a profile the
documents do not permit; a rate assumption no longer supported by the hedging market.

**14. AI application.** AI may solve the sizing loop including the tax feedback, generate sculpted
profiles, run the PR-03 sensitivities across a grid, compare the resulting profile against the
documented amortisation constraints, and draft the *sizing basis* record for confirmation.

**15. AI prohibition.** AI must not determine debt capacity, select the target coverage level, choose
the sizing case, approve a profile, or certify a quantum.

**16. AI verification.** Independent recomputation by a named human of the quantum at the recorded
basis, from the *CFADS* profile and the annuity or sculpting arithmetic; boundary testing at the
extremes of the sensitivity grid to confirm the AI's solution behaves correctly; and source tracing of
the target coverage level to the document or approval that states it. Recorded with method and date.

**17. External reference.**

- **The Basel Framework.** Issuing organisation: the Basel Committee on Banking Supervision. Subject:
  the supervisory context of lenders' credit assessment. Checked: consolidated framework as maintained
  by the BCBS; no standard, paragraph or date asserted (register `EXT-110`, verified 2026-08-03).
  Nature: Manual §6 category 10 — illustrative practice; **internationally agreed supervisory standards
  with no legal force of their own**, reaching a bank only as a national authority transposes them and
  **never applying directly to a project or its sponsors**. Applicability limitation: named for context;
  **no requirement in this law is sourced to it, and it sets no coverage level for any transaction.**
- **IAS 7 *Statement of Cash Flows*.** Issuing organisation: IFRS Foundation / IASB. Subject: cash-flow
  presentation against which the sizing cash line is reconciled. Checked: current, by name only
  (register `EXT-120`, verified 2026-08-03). Nature: Manual §6 category 2 — authoritative
  financial-reporting standard. Applicability limitation: mandatory only for entities applying IFRS
  Accounting Standards in an adopting jurisdiction; it defines no coverage ratio and no sizing method.

**18. Jurisdictional caution.** Interest-limitation and thin-capitalisation rules, the deductibility of
interest and of hedging costs, withholding tax on interest, and regulatory limits on lending to a
project or a sector can each cap the quantum independently of the coverage arithmetic, and all are
jurisdiction-specific. Obtain qualified local tax and regulatory advice before a quantum is relied
upon — see `PCI-PFL-LAW-12.02`.

**19. Related PCI Laws.** `PCI-FND-LAW-07`; `PCI-PFL-LAW-09.01`; `PCI-PFL-LAW-10.01`;
`PCI-PFL-LAW-10.03`; `PCI-PFL-LAW-10.05`; `PCI-PFL-LAW-06.03`. **Increment over the foundational
parent:** `PCI-FND-LAW-07` forbids a forecast that misleads; this law names the specific reverse-
engineering that produces a misleading quantum — choosing the coverage level to fit the answer — and
requires the basis to be recorded, sourced and stress-tested before the quantum is quoted.

**20. Related Body of Knowledge content.** PFL-AI · Domain 10 — Debt sizing, covenants and credit
metrics · KA 10.1 Debt capacity and sizing · topics: sizing from coverage, sculpting, and the effective
sculpting rate. Also Domain 3 KA 3.2 (annuities and loan schedules) and Domain 9 KA 9.1–9.2.

**21. Compliance test.** A reviewer takes the sizing output, the model and the credit approval, and
performs four steps. (a) Confirms the recorded *sizing basis* contains all six components and that the
target coverage level carries a *source line* to a document or approval. (b) Confirms the *CFADS*
profile used is the documented case, by comparing it to that case in the model. (c) Recomputes the
quantum from the recorded basis and obtains the figure quoted, without unexplained difference. (d)
Reproduces two points on the PR-03 sensitivity and obtains the stated quanta. Compliance is
demonstrated when all four complete; a target level with no source is a breach.

**22. Breach indicators.** A sizing note stating a coverage level and no source; a quantum that matches
the funding requirement exactly; a sizing case that appears nowhere else in the model; a sculpted
profile with no convention stated; a sensitivity table omitting the coverage level; a resize after
credit that changed the level rather than the quantum.

**23. Consequence within PCI authority.** Correction required and the affected sizing withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Calculation review: sizing from a given *CFADS* profile at a stated
target level and tenor, then re-sizing at a different level to expose the quantum's sensitivity.
Scenario judgement: a sponsor asks for the quantum to be raised by moving the target level. No live
examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law** — the subject was implicit in `PFL-LAW-10-01` (v1.0), which addressed reporting
rather than sizing. Amendment note: *sizing basis* defined with six components; the quantum's
sensitivity made a process requirement; the threshold rewritten to state expressly that PCI publishes
no coverage level.

---

### PCI LAW PCI-PFL-LAW-10.03 — Coverage-Ratio Calculation and Reporting

**1. Normative requirement.** A credential holder must report a *coverage ratio* with its definition,
its period basis and its minimum over the tested horizon, and must not present an average, a
period-aggregate or a single point as the coverage position.

**2. Purpose.** Coverage is the number on which debt is sized, *distributions* are permitted and
default is declared. An average conceals the period that binds; a horizon choice conceals the tail; and
a single quoted figure tells the reader nothing about which of those is happening. A number without its
definition and its minimum is not a coverage statement.

**3. Scope.** Every credential holder who computes, reviews, certifies, submits or relies upon `DSCR`,
`LLCR`, `PLCR`, interest cover or any other *coverage ratio*, in sizing, compliance certificates, credit
submissions, *distribution* tests, refinancing analyses, restructuring proposals and lender reports.

**4. Defined terms.** *coverage ratio*, *CFADS*, *debt service*, *finance documents*, *lock-up*,
*material*, *decision owner*, *competent reviewer*, *verified*, *authoritative version*, *evidence*.
**Binding period** — the period in the tested horizon at which the ratio is lowest. **Period basis** —
the periodicity, the calculation dates, and whether the test is historic, forward-looking or both, as
the finance documents state.

**5. Required actions.**

- **PCI-PFL-LAW-10.03-PR-01 — Report the minimum and identify the binding period.** The preparer must
  report the minimum ratio over the tested horizon and identify the *binding period*, alongside any
  average presented.
- **PCI-PFL-LAW-10.03-PR-02 — State the definition and period basis.** The preparer must state, with
  every reported ratio, the definition applied under `PCI-PFL-LAW-10.01`, the *period basis*, and whose
  projection the forward-looking element uses.
- **PCI-PFL-LAW-10.03-PR-03 — Publish the series.** The preparer must make the period-by-period ratio
  series available with any summary figure, so that a reader can see the shape rather than a point.
- **PCI-PFL-LAW-10.03-PR-04 — Disclose every adjustment.** The preparer must disclose every adjustment
  made in arriving at the reported ratio, and must state whether that adjustment has been applied in
  previous periods.
- **PCI-PFL-LAW-10.03-PR-05 — Compare against the documented level.** The preparer must present the
  reported ratio against the level the *finance documents* state for the test in question — sizing,
  *distribution*, *lock-up* or default — and must not compare it against a level from another test or
  another transaction.

**6. Prohibited actions.** Quoting an average where a period test governs; annualising to obscure a
seasonal or maintenance-year shortfall; moving cost out of a tested period; including in the cash line
an item the definition excludes; presenting a textbook ratio as the covenant ratio; reporting a ratio
without the level it is tested against; introducing a new adjustment in the period it is needed without
saying so.

**7. Required evidence.** The ratio calculation traceable to the model *authoritative version* and to
source lines; the definition applied, with its document reference; the period-by-period series; the
adjustment disclosure with prior-period application stated; the compliance certificate and its
workings; the reviewer's recomputation record.

**8. Responsible role.** The finance director or project finance leader who signs the certificate,
submission or report. The credit approver for the sizing adopted on it.

**9. Approval authority.** The signatory the *finance documents* name for a compliance certificate; the
*decision owner* for a credit submission or board report. No one may approve the omission of the
minimum.

**10. Independence requirement.** A *competent reviewer* independent of preparation must recompute the
ratio before financial close, before any certificate on which *lock-up*, a *distribution* or default
depends, and at model audit.

**11. Materiality or threshold.** **The level tested against is the level in the finance documents, and
PCI neither sets nor recommends one.** Where several levels apply — sizing, distribution, lock-up,
default — each is reported against its own level under PR-05. Materiality governs escalation only: the
*decision owner* records the headroom, in ratio units, at which a position must be escalated before the
test date. *Scale test:* on a small municipal project with one annual test, the series is a single row
and the minimum is obvious; on a multi-billion cross-border financing with quarterly tests, several
tranches, a backward and a forward test and different definitions per facility, the minimum is reported
per test and per facility, because the binding period is rarely the same for all of them.

**12. Exception and waiver.** No exception is permitted to element 1. Where the *finance documents*
require a specific certificate form that omits the minimum, the preparer must complete that form as
required **and** provide the minimum and the series alongside it; the documents' form governs the
certificate, and this law governs the professional's own reporting. A *waiver* of a covenant by the
finance parties is governed by `PCI-PFL-LAW-15.03` and does not waive this reporting obligation.

**13. Escalation trigger.** A computed ratio at or below any documented level in any period of the
tested horizon; a definitional difference between the model and the documents; an adjustment that
*materially* improves a ratio and has not previously been applied; a forward-looking test that fails on
the borrower's own projection; a movement in the *binding period* between reports.

**14. AI application.** AI may compute ratio series across cases, identify the *binding period*, test
sizing and sculpting arithmetic, generate stress runs, reconcile the cash line to the statements, and
draft the certificate workings for review.

**15. AI prohibition.** AI must not interpret a covenant definition, decide whether an item belongs in
the cash line, certify compliance, determine debt capacity, or sign a compliance certificate.

**16. AI verification.** Independent recomputation, by a named human, of the ratio for the *binding
period* and for at least one other period, from source lines; clause-to-output comparison of the
definition applied against the executed document; and reconciliation of the cash line to the financial
statements before certifying or submitting. Each recorded with the reviewer's name and the date.

**17. External reference.**

- **IAS 7 *Statement of Cash Flows*.** Issuing organisation: IFRS Foundation / IASB. Subject: the
  classification of cash flows against which the modelled cash line is reconciled. Checked: current, by
  name only (register `EXT-120`, verified 2026-08-03). Nature: Manual §6 category 2 — authoritative
  financial-reporting standard. Applicability limitation: entities applying IFRS Accounting Standards
  in an adopting jurisdiction only; **it defines no coverage ratio.**
- **The Basel Framework.** Issuing organisation: the Basel Committee on Banking Supervision. Subject:
  the supervisory context of lenders' credit assessment and monitoring. Checked: consolidated framework;
  no standard or date asserted (register `EXT-110`, verified 2026-08-03). Nature: Manual §6 category 10
  — illustrative practice; **internationally agreed supervisory standards with no legal force of their
  own.** Applicability limitation: named for context; **no requirement in this law is sourced to it.**

**18. Jurisdictional caution.** Coverage definitions and levels are contractual, not statutory. Their
construction, the effect of a breach, notice requirements and the enforceability of cure rights are
matters for the governing law of the *finance documents* and for qualified counsel — see
`PCI-PFL-LAW-10.04` and `PCI-PFL-LAW-12.02`. Where a ratio is also disclosed in financial statements,
the classification consequences of a breach are a reporting question in the entity's own framework and
jurisdiction.

**19. Related PCI Laws.** `PCI-FND-LAW-07`; `PCI-PFL-LAW-01.01`; `PCI-PFL-LAW-10.01`;
`PCI-PFL-LAW-10.02`; `PCI-PFL-LAW-10.04`; `PCI-PFL-LAW-15.01`. **Increment over the foundational
parent:** `PCI-FND-LAW-07` requires honest reporting; this law specifies the three things a coverage
number needs before it is honest — its definition, its period basis and its minimum — and requires the
series and every adjustment to travel with it.

**20. Related Body of Knowledge content.** PFL-AI · Domain 10 — Debt sizing, covenants and credit
metrics · KA 10.2 The coverage ratios · topics: the period test, the horizon tests, and reading a ratio
set together. Also Domain 15 KA 15.1 (backward, rolling and forward-looking tests).

**21. Compliance test.** A reviewer takes the reported ratio, the model and the finance documents, and
performs five steps. (a) Confirms the report states the definition, the *period basis* and the minimum,
and identifies the *binding period*. (b) Recomputes the ratio for the binding period and one other from
source lines and obtains the reported figures without unexplained difference. (c) Confirms the cash line
reconciles to the financial statements. (d) Confirms every adjustment is disclosed and that its
prior-period application is stated. (e) Confirms the ratio is compared against the level in the finance
documents for that test. Compliance is demonstrated when all five complete; a reported ratio with no
minimum, or an undisclosed adjustment, is a breach.

**22. Breach indicators.** A certificate quoting one annual figure; an average described as "the DSCR";
a series that is not in the pack; a new adjustment appearing in the tightest period; a ratio compared
against a level from the sizing test when the distribution test governs; a forward-looking figure with
no statement of whose projection it uses.

**23. Consequence within PCI authority.** Correction required and the affected report or certificate
withheld; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Calculation review: an average coverage figure conceals a failing
period, and the candidate must locate the binding period and restate the report compliantly. Evidence
selection: which workings a compliance certificate needs behind it. No live examination content is
exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes the reporting limb of `PFL-LAW-10-01` *Debt-Service Coverage Truth* (v1.0).
Amendment note: the v1.0 rule is split across `PCI-PFL-LAW-10.01` (definition),
`PCI-PFL-LAW-10.02` (sizing) and this law (calculation and reporting); *binding period* and *period
basis* defined; the comparison-against-the-documented-level rule added as PR-05; the element 12 conflict
between a prescribed certificate form and this law resolved expressly.

---

### PCI LAW PCI-PFL-LAW-10.04 — Covenant Interpretation

**1. Normative requirement.** A credential holder must not state a legal conclusion on the meaning, the
breach or the consequence of a covenant; that construction belongs to qualified counsel.

**2. Purpose.** Covenants are how lenders keep a project fixable while options remain. The credential
holder's duty is to compute and report faithfully against the documented terms — and to recognise the
point at which computation becomes legal construction, which is exactly the point at which a confident
professional does the most damage.

**3. Scope.** Every credential holder who monitors, models, certifies, negotiates, reports on or
advises upon financial, information, positive and negative covenants, *distribution* conditions,
*lock-up* triggers, events of default, cure rights and their consequences.

**4. Defined terms.** *finance documents*, *lock-up*, *distribution*, *coverage ratio*, *waiver*,
*amendment*, *material*, *decision owner*, *competent reviewer*, *escalation threshold*, *evidence*.
**Covenant register** — a record stating, for each covenant, the document and clause in which it
appears, its definition set, its test dates, its levels, its cure rights and its consequences.
**Interpretive question** — a question whose answer depends on the construction of language rather than
on arithmetic.

**5. Required actions.**

- **PCI-PFL-LAW-10.04-PR-01 — Maintain the covenant register.** The credential holder must maintain a
  *covenant register* with all six fields per covenant, sourced to the executed documents.
- **PCI-PFL-LAW-10.04-PR-02 — Test on the defined dates with the defined inputs.** The credential
  holder must test each covenant on its defined dates using its defined inputs, and must not substitute
  a market convention, a textbook definition or another transaction's terms.
- **PCI-PFL-LAW-10.04-PR-03 — Refer, do not resolve.** The credential holder must record every
  *interpretive question* and refer it to qualified counsel, and must not resolve it inside the model or
  the certificate.
- **PCI-PFL-LAW-10.04-PR-04 — Register maintained through waivers and amendments.** The credential
  holder must record every *waiver* and *amendment* as a change to both the register and the model, and
  must re-confirm the register after each.
- **PCI-PFL-LAW-10.04-PR-05 — Report a lock-up as a lock-up.** The credential holder must report a
  *lock-up* as a trap on cash and a default as a default, and must not describe either as the other.

**6. Prohibited actions.** Computing a covenant on a definition other than the documented one; giving
an opinion on whether a breach has occurred, whether notice is required, or whether a cure is
effective; presenting a market convention as the transaction's requirement; delaying notification of a
likely breach to preserve a reporting position; treating a waived condition or covenant as satisfied;
describing a lock-up as a default or a default as a lock-up.

**7. Required evidence.** The covenant register with document and clause references; compliance
certificates and their workings; the log of *interpretive questions* referred and the advice received;
the *waiver* and *amendment* log with its effect on the register; the covenant dashboard version
history.

**8. Responsible role.** The finance director or project finance leader for compliance reporting; the
lenders' agent for the lenders' position; **qualified counsel for interpretation**.

**9. Approval authority.** The signatory the finance documents name for each certificate. Only the
finance parties, in the form the documents require, may waive or amend a covenant. Counsel, not the
credential holder, resolves construction.

**10. Independence requirement.** A *competent reviewer* independent of preparation must review the
covenant register and the position whenever a covenant approaches its level, before any *waiver* or
*amendment* request, and whenever a definition changes.

**11. Materiality or threshold.** **Every level is the documented level; PCI sets none.** The
*escalation threshold* is the headroom — in ratio units, cash or days — at which a position must be
escalated before the test date, and it is set by the adopting organisation's governance where the
finance documents do not state one. *Scale test:* on a small municipal project the register may hold a
dozen covenants tested annually; on a multi-billion cross-border financing it holds several hundred
across facilities, with different test dates, definitions and cure regimes per facility, and the
dashboard must show the earliest binding test across the whole set rather than a consolidated position
that exists in no document.

**12. Exception and waiver.** No exception is permitted to element 1 or to PR-03. A *waiver* of a
covenant itself is a matter for the finance parties under the documents and is governed by
`PCI-PFL-LAW-15.03`; it does not permit the credential holder to state a legal conclusion.

**13. Escalation trigger.** A position at or approaching a *lock-up* or default level; a definitional
ambiguity that changes the result; a proposed *amendment* whose effect on the register has not been
assessed; an event that may constitute a default; a cure whose mechanics the model cannot reproduce.

**14. AI application.** AI may extract covenant terms and definitions into a draft register for human
confirmation, monitor positions against levels, generate early-warning alerts, model cure mechanics,
and draft compliance certificates for review.

**15. AI prohibition.** AI must not interpret a covenant, determine that a breach has or has not
occurred, certify compliance, advise on cure, *waiver* or *amendment*, or characterise an event as a
default.

**16. AI verification.** Clause-to-output comparison, by a named human, of every AI-extracted covenant
term against the executed document, reading the clause itself; re-confirmation of the whole register
after every amendment; and referral rather than resolution of any question the comparison leaves open.
An AI-produced answer to an *interpretive question* is deleted, not filed.

**17. External reference.**

- **IAS 1 *Presentation of Financial Statements*.** Issuing organisation: IFRS Foundation / IASB.
  Subject: the presentation of financial statements, including the basis on which liabilities are
  classified as current or non-current — the point at which a covenant position meets the reported
  position. Checked: in force for periods beginning before 1 January 2027; **IFRS 18 *Presentation and
  Disclosure in Financial Statements* replaces IAS 1 for annual reporting periods beginning on or after
  1 January 2027, earlier application permitted** (register `EXT-004` / `EXT-003`, verified
  2026-08-03). Nature: Manual §6 category 2 — authoritative financial-reporting standard. Applicability
  limitation: entities applying IFRS Accounting Standards in an adopting jurisdiction only; **confirm
  which instrument applies to the period being reported**, and note that reporting classification is
  not covenant compliance — only the finance documents determine that.
- **The Basel Framework.** Issuing organisation: the Basel Committee on Banking Supervision. Subject:
  the supervisory context of lenders' covenant monitoring. Checked: consolidated framework; no standard
  or date asserted (register `EXT-110`, verified 2026-08-03). Nature: Manual §6 category 10 —
  illustrative practice; **internationally agreed supervisory standards with no legal force of their
  own**. Applicability limitation: named for context; **no requirement in this law is sourced to it.**

**18. Jurisdictional caution.** The construction of covenant language, the effect of a breach, notice
requirements, grace periods, the enforceability of cure rights and of acceleration, the availability of
injunctive relief, and the interaction with insolvency law are all determined by the governing law of
the *finance documents* and by the law of the place of enforcement, which need not be the same. Obtain
qualified legal advice, and **never act on a modelled interpretation**.

**19. Related PCI Laws.** `PCI-FND-LAW-08` (competence boundaries and referral); `PCI-FND-LAW-11`;
`PCI-PFL-LAW-10.01`; `PCI-PFL-LAW-10.03`; `PCI-PFL-LAW-12.02`; `PCI-PFL-LAW-15.01`;
`PCI-PFL-LAW-15.03`. **Increment over the foundational parent:** `PCI-FND-LAW-08` requires referral
beyond competence; this law draws the boundary precisely for a financing — arithmetic on defined inputs
is the professional's; the construction of the words is counsel's — and adds the register, the referral
log and the rule that a lock-up and a default are reported as different events.

**20. Related Body of Knowledge content.** PFL-AI · Domain 10 — Debt sizing, covenants and credit
metrics · KA 10.4 Covenants, default and cure · topics: covenant types, distribution lock-up, events of
default and cure rights, living with covenants. Also Domain 15 KA 15.3 (refinancing, waivers and
amendments).

**21. Compliance test.** A reviewer takes the register, the executed documents and the last two
certificates, and performs five steps. (a) Selects five covenants and confirms each register entry
matches the clause, definition set, test dates, level, cure right and consequence in the document. (b)
Confirms each certificate was computed on the defined dates with the defined inputs. (c) Confirms every
*interpretive question* in the log was referred and that no answer to one appears in the model or the
certificate. (d) Confirms the register was re-confirmed after each waiver and amendment in the period.
(e) Confirms any lock-up in the period is reported as a lock-up and not as a default. Compliance is
demonstrated when all five complete; a legal conclusion stated by the credential holder is a breach
regardless of whether it was right.

**22. Breach indicators.** A register without clause references; a certificate whose inputs differ from
the defined ones; an email from the modeller stating that a breach "has not been triggered"; a waiver
recorded in the model but not the register; a dashboard showing a consolidated covenant position across
facilities with different definitions; a lock-up reported to a board as a default.

**23. Consequence within PCI authority.** Correction required and the affected report withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Escalation decision: a position moves within the escalation threshold
before a test date and the candidate must state who is told, when, and on what record. Ethical dilemma:
a sponsor asks the candidate to confirm that no default has occurred. No live examination content is
exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-10-02` *Covenant Interpretation* (v1.0). Amendment note: restructured
onto the twenty-five-element form; the single bundled rule reduced to one principal obligation with
five process requirements; *covenant register* and *interpretive question* defined; the lock-up/default
reporting distinction added as PR-05; the IAS 1 reference carries the IFRS 18 supersession date.

---

### PCI LAW PCI-PFL-LAW-10.05 — Reserve-Account Governance

**1. Normative requirement.** A credential holder must fund, apply and release a *reserve account* only
in the amounts, for the purposes and in the order the *finance documents* specify.

**2. Purpose.** A reserve buys payment continuity and time; it does not buy compliance. Reserves are
the easiest balances in a financing to borrow against informally — funded late, applied to the wrong
purpose, released early to permit a *distribution* — and each of those makes the structure weaker at
exactly the moment it was built to be strong.

**3. Scope.** Every credential holder who models, funds, monitors, certifies, applies, releases or
reports on a debt-service reserve, a maintenance reserve, a lifecycle or major-maintenance reserve, a
tax reserve, a decommissioning reserve, a distribution-block account or any other account the *finance
documents* require to hold a balance.

**4. Defined terms.** *reserve account*, *finance documents*, *distribution*, *lock-up*, *debt
service*, *CFADS*, *material*, *decision owner*, *competent reviewer*, *evidence*, *verified*.
**Required balance** — the balance the finance documents require the account to hold at a stated date,
computed on the basis those documents state. **Permitted application** — a use of a reserve balance
that the finance documents expressly allow, in the order they specify.

**5. Required actions.**

- **PCI-PFL-LAW-10.05-PR-01 — Required-balance schedule.** The preparer must maintain a schedule of
  each reserve's *required balance* by date, computed on the documented basis and sourced to the clause
  that establishes it.
- **PCI-PFL-LAW-10.05-PR-02 — Funding on time and in form.** The preparer must confirm that each
  reserve is funded to its required balance on the required date and in the permitted form — cash or an
  acceptable instrument — and must report a shortfall as a shortfall.
- **PCI-PFL-LAW-10.05-PR-03 — Application only as permitted.** The preparer must confirm before each
  application that it is a *permitted application* and that the order of application is as specified,
  and must not apply a reserve to a purpose the documents do not allow.
- **PCI-PFL-LAW-10.05-PR-04 — Replenishment tracked.** The preparer must track the replenishment
  obligation following any application, and must report the position until the required balance is
  restored.
- **PCI-PFL-LAW-10.05-PR-05 — Release only on the documented condition.** The preparer must confirm
  that every release condition is satisfied before a release, and must not release a balance in order
  to permit a *distribution*.

**6. Prohibited actions.** Funding a reserve after its required date and reporting it as funded on
time; substituting an instrument the documents do not permit; applying a reserve to a purpose outside
the permitted list or out of order; netting a reserve shortfall against a surplus in another account;
releasing a reserve early; presenting a reserve balance as *CFADS*; counting an undrawn facility as a
funded reserve where the documents require cash.

**7. Required evidence.** The required-balance schedule with clause references; account statements or
custodian confirmations at each test date; the permitted-application confirmation for each application;
the replenishment tracking record; the release conditions and the evidence they were satisfied; the
reporting to the agent or lenders.

**8. Responsible role.** The finance director or project finance leader accountable for the accounts.
The account bank, agent or security trustee for the operation of the accounts under the documents.

**9. Approval authority.** The party the finance documents name — commonly the agent or security
trustee — for an application or a release. The *decision owner* for the borrower's request. No
professional judgement can substitute for the documented condition.

**10. Independence requirement.** Confirmation of balances must come from a source *independent* of the
borrower — an account bank statement, a custodian confirmation or the agent's record — and not from the
borrower's own ledger alone, before any certificate on which a *distribution* or a *lock-up* depends.

**11. Materiality or threshold.** **The required balance is the documented balance; PCI sets no reserve
level and no coverage of months.** A shortfall of any size is a shortfall and is reported as one.
Materiality governs escalation timing only, and the *escalation threshold* — in days before the
required date, or in the proportion of the balance unfunded — is set by the adopting organisation's
governance where the documents are silent. *Scale test:* on a small municipal project a single
debt-service reserve funded in cash makes the schedule one line and the confirmation one statement; on
a multi-billion cross-border financing with reserves per facility, per currency and per purpose, some
funded by letter of credit and some in cash, the schedule is maintained per account and the permitted
form is confirmed per account, because an instrument acceptable to one facility is often not acceptable
to another.

**12. Exception and waiver.** No exception is permitted. A shortfall, a late funding, a non-permitted
application or an early release can be cured only by a *waiver* from the party entitled to give one,
under `PCI-PFL-LAW-15.03`; until that waiver is given in the form the documents require, the position
is reported as non-compliant.

**13. Escalation trigger.** A reserve that will not be funded to its required balance on its required
date; an application proposed that is not on the permitted list; a replenishment obligation not met
within its period; a release requested where a condition is unsatisfied; a permitted instrument that
has been downgraded, has expired or is about to.

**14. AI application.** AI may compute required balances across the schedule, reconcile account
statements to required balances, alert on approaching funding dates and instrument expiries, model the
replenishment path, and draft the reporting for review.

**15. AI prohibition.** AI must not authorise a funding, an application or a release; must not decide
that an application is permitted; must not confirm a balance; and must not certify that a release
condition is satisfied.

**16. AI verification.** Reconciliation, by a named human, of every AI-computed required balance to the
clause that establishes it; source tracing of every balance to an independent account confirmation
rather than to the borrower's ledger; and clause-to-output comparison of each proposed application
against the permitted-application list before it is made.

**17. External reference.**

- **IAS 7 *Statement of Cash Flows*.** Issuing organisation: IFRS Foundation / IASB. Subject: the
  classification of cash and of restricted balances in the statement of cash flows, which is where a
  reserve balance meets the reported position. Checked: current, by name only (register `EXT-120`,
  verified 2026-08-03). Nature: Manual §6 category 2 — authoritative financial-reporting standard.
  Applicability limitation: entities applying IFRS Accounting Standards in an adopting jurisdiction
  only; **it does not determine whether a reserve is adequately funded — the finance documents do.**

**18. Jurisdictional caution.** The effectiveness of an account charge or account-control arrangement,
the perfection of security over a reserve balance, set-off rights of the account bank, the treatment of
a restricted balance on the borrower's or the bank's insolvency, and any exchange control over an
offshore reserve are all jurisdiction-specific — and in a cross-border financing the account's
jurisdiction, the borrower's and the governing law's may differ. Obtain qualified local legal advice on
each account before relying on a reserve as security or as available cash.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-11`; `PCI-PFL-LAW-10.01`;
`PCI-PFL-LAW-10.03`; `PCI-PFL-LAW-14.04`; `PCI-PFL-LAW-15.01`. **Increment over the foundational
parent:** `PCI-FND-LAW-05` requires an evidenced trail; this law states what the trail must prove for a
reserve — the required balance sourced to its clause, funding on the date and in the permitted form,
each application checked against the permitted list before it is made, replenishment tracked to
restoration, and independent confirmation of the balance itself.

**20. Related Body of Knowledge content.** PFL-AI · Domain 10 — Debt sizing, covenants and credit
metrics · KA 10.3 Reserve accounts and the debt-service schedule · including reserve tolerance. Also
Domain 15 KA 15.2 (the cash waterfall in operation, reserves and distributions).

**21. Compliance test.** A reviewer takes the required-balance schedule, the account confirmations and
the application records, and performs five steps. (a) Confirms each required balance in the schedule
matches the clause cited, recomputing it on the documented basis. (b) Compares the independent account
confirmation at each test date to the required balance. (c) For each application in the period, confirms
it appears on the permitted list and was made in the specified order. (d) Confirms every replenishment
obligation was met within its period, or is being reported. (e) Confirms every release was preceded by
evidence that each condition was satisfied. Compliance is demonstrated when all five complete; a
balance confirmed only by the borrower's own ledger is a failed step.

**22. Breach indicators.** A reserve funded on the day after its required date and reported as compliant;
a letter of credit from an institution the documents do not permit; a reserve applied to an operating
cost outside the permitted list; a release immediately preceding a distribution; a replenishment
obligation that has been open for several periods without report; a reserve balance included in *CFADS*.

**23. Consequence within PCI authority.** Correction required and the affected certificate or report
withheld; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Scenario judgement: a release is requested to enable a distribution
and one condition is unsatisfied — the candidate must state the required action. Calculation review:
computing a required balance on a documented basis and reconciling it to an account confirmation. No
live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law — no v1.0 predecessor.** Amendment note: v1.0 addressed reserves only obliquely,
through the distribution law. *Required balance* and *permitted application* defined; independent
confirmation of balances made an element 10 requirement; replenishment tracking added as its own
process requirement.

---
## Domain 11 — Risk identification and allocation

### PCI LAW PCI-PFL-LAW-11.01 — Risk-Allocation Honesty

**1. Normative requirement.** A credential holder must record where each *material* risk actually
lands once the contracts, caps, exclusions, deductibles, insurance and counterparty credit are read
together, and must not represent a risk as transferred where that reading leaves it with the project.

**2. Purpose.** Risk allocation is written across many documents and is usually summarised in one
matrix. A cap, an exclusion, a liability basket, an uninsured peril or a counterparty that cannot pay
each returns a risk to the project silently, and the matrix keeps saying it was transferred. Lenders,
sponsors and grantors then price something that does not exist.

**3. Scope.** Every credential holder who identifies, allocates, models, reviews, reports on or
approves project risk — construction and completion, market, demand, price, supply, operations,
counterparty, political and regulatory, currency, interest rate, force majeure, environmental and
social, technology, cybersecurity and AI model risk — at development, close, in operation and on
restructuring.

**4. Defined terms.** *material*, *evidence*, *decision owner*, *finance documents*, *competent
reviewer*, *verified*, *decision-grade*, *escalation threshold*. **Allocation matrix** — a record
stating, for each risk: the bearing party, the clause that places it there, the financial limit on that
party's liability, the instrument backing it, and the residual position with the project.
**Residual position** — the part of a risk that remains with the project after every transfer
mechanism is read at its limits.

**5. Required actions.**

- **PCI-PFL-LAW-11.01-PR-01 — Allocation matrix with limits.** The preparer must maintain an
  *allocation matrix* containing all five fields for every *material* risk, sourced to the clauses that
  create the allocation.
- **PCI-PFL-LAW-11.01-PR-02 — Read the transfer at its limit.** The preparer must state, for each
  transferred risk, the cap, exclusion, deductible, time bar and condition on which the transfer
  depends, and must compute the *residual position* on the basis that each of those operates.
- **PCI-PFL-LAW-11.01-PR-03 — Counterparty capacity.** The preparer must record the credit standing of
  each party bearing a *material* risk and the instrument supporting it, and must treat a risk
  transferred to a party that cannot meet it as retained to the extent of the shortfall.
- **PCI-PFL-LAW-11.01-PR-04 — Orphan and double-cover identification.** The preparer must identify
  every risk that no party bears and every risk two mechanisms purport to cover, and must report both.

**6. Prohibited actions.** Presenting a risk as transferred without stating the cap or the exclusion;
describing insurance as covering a peril the policy excludes; treating a parent guarantee as unlimited
where it is capped; assuming a counterparty's performance without recording its credit standing;
netting an orphan risk against a doubly covered one; carrying a matrix forward after the contracts
change.

**7. Required evidence.** The allocation matrix with clause references and limits; the residual-position
calculation for each *material* transferred risk; the counterparty credit records and supporting
instruments; the orphan and double-cover report; the version history of the matrix against the contract
set.

**8. Responsible role.** The project finance leader accountable for the risk position. The *decision
owner* for the sanction, credit or investment decision taken on it.

**9. Approval authority.** The decision owner approves the risk position for use. Only the parties can
change an allocation, by amending the contracts.

**10. Independence requirement.** A *competent reviewer* independent of the negotiating team must
review the matrix and the residual positions before financial close and before any restructuring
proposal, because the negotiating benefit runs to the allocation being presented as complete.

**11. Materiality or threshold.** A risk is *material* where its *residual position*, quantified,
exceeds the figure recorded in the engagement's materiality statement in the transaction's own metric —
for example a stated movement in the minimum *coverage ratio* or a stated cash amount against
available headroom and contingency. **PCI sets no figure**, and where a risk cannot be quantified the
preparer records that expressly rather than treating it as immaterial. *Scale test:* on a small
municipal project the matrix may hold twenty risks and one contractor, and PR-03 is a single credit
check; on a multi-billion cross-border financing with dozens of counterparties, layered insurance and
political-risk cover across jurisdictions, the matrix is maintained per contract package and PR-04 does
the heaviest work, because orphan risks emerge at the interfaces between packages rather than inside
them.

**12. Exception and waiver.** No exception is permitted to element 1. A risk may be recorded as
*retained and accepted* by the project, which is compliance, provided the retention is on the face of
the matrix, quantified where it can be quantified, and accepted in writing by the *decision owner*.

**13. Escalation trigger.** A *material* residual position that was previously reported as transferred;
a counterparty downgrade or failure affecting a risk-bearing obligation; an orphan risk discovered
after the contracts are signed; an insurance renewal that narrows cover; a cap consumed by a claim.

**14. AI application.** AI may extract allocation clauses, caps, exclusions and time bars from the
contract set into a draft matrix, identify risks named in one document and absent from another, compare
insurance schedules against the risk list, and model the residual position under stated limits.

**15. AI prohibition.** AI must not conclude that a risk is transferred, decide that a residual position
is acceptable, assess a counterparty's ability to pay, or approve a risk matrix.

**16. AI verification.** Clause-to-output comparison, by a named human, of every AI-extracted allocation
against the clause itself, including its proviso and its exclusions; independent recomputation of each
*material* residual position; and source tracing of every counterparty credit statement to the record
that evidences it. An AI matrix that has not been clause-checked is a draft, not a matrix.

**17. External reference.**

- **ISO 31000 *Risk management — Guidelines*.** Issuing organisation: ISO. Subject: principles and a
  process for managing risk. Checked: ISO 31000:2018, 2nd edition, reviewed and confirmed 2023 (register
  `EXT-020`, verified 2026-08-03). Nature: Manual §6 category 3 — international voluntary standard;
  **guidance, not a certifiable requirements standard — nothing can be certified against it.**
  Applicability limitation: voluntary unless a law or contract imports it.
- **The FIDIC suite of conditions of contract.** Issuing organisation: FIDIC (International Federation
  of Consulting Engineers). Subject: standard forms allocating construction risk between employer and
  contractor. Checked: **characterised generically; no book, clause number or edition is asserted**
  (register `EXT-050`, verified 2026-08-03 — note that clause numbering moved between editions).
  Nature: Manual §6 category 4 — contract framework. Applicability limitation: **binds only the parties
  who adopt it, and only through the contract they sign; it is not generally applicable legislation.**
- **IFC *Performance Standards on Environmental and Social Sustainability*.** Issuing organisation:
  IFC, World Bank Group. Subject: environmental and social risk expectations. Checked: 2012 edition;
  Sustainability Framework update in progress (register `EXT-083`, verified 2026-08-03). Nature: Manual
  §6 category 8 — voluntary environmental or social framework. Applicability limitation: binding on IFC
  clients by contract; on others only where adopted. **Status is moving — verify the current position.**

**18. Jurisdictional caution.** The enforceability of a liability cap, an exclusion clause, a liquidated
damages provision, an indemnity and a parent guarantee differs by jurisdiction, and some are
unenforceable in some places; the availability and scope of political-risk and force-majeure cover is
jurisdiction- and insurer-specific; and sanctions may make a counterparty unusable irrespective of its
credit. Obtain qualified local legal advice on the enforceability of each transfer mechanism before
relying on it — a cap that is void does not cap.

**19. Related PCI Laws.** `PCI-FND-LAW-07`; `PCI-FND-LAW-11`; `PCI-PFL-LAW-05.01`;
`PCI-PFL-LAW-09.01`; `PCI-PFL-LAW-12.01`; `PCI-PFL-LAW-12.02`. **Increment over the foundational
parent:** `PCI-FND-LAW-07` requires honest reporting; this law names the mechanism by which a risk
matrix becomes dishonest without a false statement in it — reading a transfer at its headline rather
than at its limit — and requires caps, exclusions, counterparty capacity, orphans and double cover all
to be on the face of the record.

**20. Related Body of Knowledge content.** PFL-AI · Domain 11 — Risk identification and allocation ·
KA 11.1–11.4. Also Domain 12 KA 12.4 (risk allocation, claims and change) and Domain 5 KA 5.3 (the
bankability test).

**21. Compliance test.** A reviewer takes the matrix and the contract and insurance set, and performs
four steps. (a) Selects the five largest *material* risks and confirms each matrix row's clause
reference matches the clause, including its provisos. (b) For each, reads the cap, exclusion,
deductible and time bar and recomputes the *residual position*, obtaining the figure in the matrix. (c)
Confirms each risk-bearing counterparty's credit record is present and dated. (d) Confirms the orphan
and double-cover report covers the interfaces between contract packages. Compliance is demonstrated
when all four complete; a risk shown as transferred whose recomputed residual position is *material*
and unreported is a breach.

**22. Breach indicators.** A matrix with a "bearing party" column and no limit column; insurance listed
against a peril its policy excludes; a guarantee shown without its cap; a matrix dated before the last
contract amendment; no orphan report at all; a counterparty whose credit was assessed at bid and never
since.

**23. Consequence within PCI authority.** Correction required and the affected matrix or report
withheld; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Scenario judgement: a matrix shows a completion risk as transferred and
the contract caps damages well below the funded cost of delay — the candidate must quantify the
residual and restate the row. Evidence selection: which document establishes where a risk lands. No
live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-11-01` *Risk Allocation Honesty* (v1.0). Amendment note: restructured
onto the twenty-five-element form; *allocation matrix* and *residual position* defined; counterparty
capacity and the orphan/double-cover report raised to process requirements; FIDIC characterisation
tightened to record that clause numbering moved between editions and that none is asserted.

---

## Domain 12 — Contracts and transaction structure

### PCI LAW PCI-PFL-LAW-12.01 — Contract-Source Verification

**1. Normative requirement.** A credential holder must take every contractual term used in a model,
register, certificate or report from the executed document itself, read at the clause.

**2. Purpose.** A term sheet, a summary, a précis in a diligence report, a previous transaction's
schedule and a machine extraction are all easier to read than a signed contract, and all of them are
routinely wrong in the detail that matters — a proviso, a cross-reference, a defined term used in a
narrower sense. Everything downstream inherits the error, and the model reconciles perfectly to it.

**3. Scope.** Every credential holder who takes a term from a contract into any model, register,
certificate, dashboard, report or advice — including tariff and payment mechanics, indexation,
availability and deduction regimes, liquidated damages, caps and exclusions, termination and
compensation, security and direct-agreement terms, and every defined term used in a covenant or a
*CFADS* definition.

**4. Defined terms.** *finance documents*, *source line*, *evidence*, *verified*, *material*, *decision
owner*, *competent reviewer*, *authoritative version*. **Executed document** — the signed agreement in
its current form, including every amendment, side letter and variation in effect at the date used.
**Clause-level reading** — reading the operative clause together with its definitions, provisos,
cross-references and any schedule it invokes.

**5. Required actions.**

- **PCI-PFL-LAW-12.01-PR-01 — Read at the clause.** The preparer must perform a *clause-level reading*
  of the *executed document* for every *material* term before using it, and must not rely on a summary,
  a term sheet or a précis as the source.
- **PCI-PFL-LAW-12.01-PR-02 — Cite the clause.** The preparer must record, as the term's *source line*,
  the document, its date and execution status, and the clause and defined terms relied on.
- **PCI-PFL-LAW-12.01-PR-03 — Amendment currency.** The preparer must confirm that the version read is
  the version in effect at the date used, including all amendments and side letters, and must re-read
  the term after any amendment.
- **PCI-PFL-LAW-12.01-PR-04 — Discrepancy reporting.** The preparer must report any discrepancy between
  the executed document and a summary, diligence report or previous model, to the *decision owner* and
  to the party that produced the summary.

**6. Prohibited actions.** Taking a term from a term sheet, a summary, a diligence report, a prior
model or a machine extraction without reading the clause; using a term from a draft as though executed;
ignoring a side letter; treating a defined term in its ordinary sense where the contract defines it;
carrying a term across from a comparable transaction; leaving a discovered discrepancy unreported.

**7. Required evidence.** The *source line* for each *material* term, with document, date, execution
status and clause; the record of the clause-level reading, dated and attributed; the amendment-currency
confirmation; the discrepancy reports issued and their responses.

**8. Responsible role.** The preparer who takes the term into the output, personally. The *decision
owner* for the output that relies on it.

**9. Approval authority.** The decision owner approves the output. No one may approve the use of a term
that has not been read at the clause. Where the clause's *meaning* is in question, that is an
interpretive question for counsel under `PCI-PFL-LAW-10.04-PR-03` and `PCI-PFL-LAW-12.02`.

**10. Independence requirement.** A *competent reviewer* independent of preparation must re-read a
sample of *material* terms at the clause as part of any model audit or diligence review under
`PCI-PFL-LAW-13.01`; the sampling basis is stated before the sample is drawn.

**11. Materiality or threshold.** A term is *material* where a difference between the summary and the
clause would change a modelled output by more than the figure recorded in the engagement's materiality
statement, in the transaction's own metric, or would change a covenant, a *distribution* test or a
termination outcome at all. **PCI sets no figure.** *Scale test:* on a small municipal project the
contract set may be three documents and PR-01 is achievable in full for every term; on a multi-billion
cross-border financing with dozens of agreements in several languages, PR-01 is applied in full to
every material term and by stated sampling to the remainder, with the sampling basis recorded — and
where a governing text is in another language, the translation's status is recorded with the source
line.

**12. Exception and waiver.** No exception is permitted for a *material* term. For a non-material term
during a live negotiation, the *decision owner* may approve in writing the temporary use of an agreed
draft, provided the output states that the term is drawn from a draft, names the draft version, and is
re-based on the executed document before the output is used for a decision. Duration: until execution
or fourteen days, whichever is earlier.

**13. Escalation trigger.** A discrepancy between the executed document and a diligence summary; a side
letter discovered after a model was built; an amendment that changes a term already in use; a governing
text in a language nobody on the team reads; an executed document that cannot be located.

**14. AI application.** AI may locate candidate clauses across a large document set, extract terms and
their defined terms into a draft register, compare two versions of a document, detect that a summary
and a clause differ, and flag cross-references and provisos for human reading.

**15. AI prohibition.** AI must not be the source of a contractual term; must not decide that a
proviso does not apply; must not confirm that a document is the executed version; and must not resolve
a discrepancy between a summary and a clause. **An AI extraction is a pointer to a clause, never a
substitute for reading it.**

**16. AI verification.** Clause-to-output comparison by a named human for every AI-extracted *material*
term — opening the executed document, reading the operative clause with its definitions and provisos,
and confirming the extracted term against it; plus a stated sample of non-material extractions.
Recorded per term with the reader's name and the date.

**17. External reference.**

- **The FIDIC suite of conditions of contract.** Issuing organisation: FIDIC. Subject: standard forms
  of construction contract whose clause numbering and drafting differ between editions and amendment
  sets. Checked: **characterised generically; no book, clause number or edition asserted** (register
  `EXT-050`, verified 2026-08-03). Nature: Manual §6 category 4 — contract framework. Applicability
  limitation: **binds only the parties who adopt it, through the contract they sign — never
  legislation**; and a project's contract is commonly an amended form, so the standard form is not a
  safe source for a term.
- **ISO 15489-1 *Information and documentation — Records management — Part 1: Concepts and
  principles*.** Issuing organisation: ISO. Subject: the characteristics that make a record reliable and
  retrievable. Checked: ISO 15489-1:2016 (register `EXT-025`, verified 2026-08-03). Nature: Manual §6
  category 3 — international voluntary standard. Applicability limitation: voluntary unless imported by
  law or contract.

**18. Jurisdictional caution.** Which text governs where a contract exists in two languages, whether an
electronic execution is valid, whether a side letter varies the main agreement, the effect of an
entire-agreement clause, and the admissibility of pre-contractual material in construing a term are all
questions of the governing law and can differ between the contracts in a single financing. Obtain
qualified legal advice on the effective document set before relying on it — see `PCI-PFL-LAW-12.02`.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-06`; `PCI-PFL-LAW-06.04`;
`PCI-PFL-LAW-10.01`; `PCI-PFL-LAW-10.04`; `PCI-PFL-LAW-11.01`; `PCI-PFL-LAW-16.02`. **Increment over
the foundational parent:** `PCI-FND-LAW-06` requires lineage to a source; this law fixes what counts as
the source for a contractual term — the executed clause with its definitions and provisos, at the
version in effect — and adds a positive duty to report a discrepancy back to whoever produced the
summary that was wrong.

**20. Related Body of Knowledge content.** PFL-AI · Domain 12 — Contracts and transaction structure ·
KA 12.1 EPC and O&M, KA 12.2 offtake, concession, supply and interface agreements, KA 12.3 guarantees,
direct agreements and the security package, KA 12.4 risk allocation, claims and change. Also Domain 13
KA 13.1 (the diligence streams).

**21. Compliance test.** A reviewer selects the *material* terms used in an output and, for each: opens
the executed document at the clause cited; reads the operative clause with its definitions and
provisos; and confirms the term as used matches. The reviewer then (d) confirms the document version
read is the one in effect at the output date, including amendments and side letters. Compliance is
demonstrated when every selected term matches and the version is current; a term matching a summary but
not the clause is a breach, and so is a correct term with no clause citation.

**22. Breach indicators.** A model whose tariff sheet cites a diligence report; a term in use that
exists only in a superseded draft; a defined term used in its ordinary sense; a register built entirely
from a machine extraction with no reading record; an amendment in the data room that no register
reflects; a discrepancy found and quietly corrected without a report.

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Evidence selection: given a term sheet, a diligence summary and an
executed clause that differ, the candidate identifies the source that governs and the required action.
AI-verification case: a machine extraction omits a proviso that reverses the result. No live examination
content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law — no v1.0 predecessor.** Amendment note: v1.0 required the executed documents to be
applied in the covenant and conditions-precedent laws but never stated the sourcing discipline itself,
so the obligation existed only by implication. *Executed document* and *clause-level reading* defined;
discrepancy reporting made a process requirement.

---

### PCI LAW PCI-PFL-LAW-12.02 — The Tax and Legal Advice Boundary

**1. Normative requirement.** A credential holder must not give legal, tax, accounting, regulatory or
insurance advice, and must obtain written advice from an adviser qualified in the relevant jurisdiction
before any treatment, contractual position or structure is adopted or represented as correct.

**2. Purpose.** Project finance sits on top of tax structuring, security law, insolvency law,
regulation and insurance, and the finance professional is the person in the room most often asked what
the position is. Answering is easy, feels helpful, is usually approximately right, and is exactly how a
structure fails years later — when the approximation meets a tax authority, a security registry or an
administrator.

**3. Scope.** Every credential holder, in every role, on every transaction, whenever a question of law,
tax, accounting treatment, regulatory permission or insurance coverage arises. Applies to advice given
internally and externally, in writing and orally, and to the modelling of a treatment as much as to its
statement.

**4. Defined terms.** *material*, *evidence*, *decision owner*, *finance documents*, *verified*,
*escalation threshold*. **Qualified adviser** — a person or firm authorised to give advice on the
subject in the relevant jurisdiction, engaged for that purpose, and identifiable from the engagement
record. **Treatment** — a tax, accounting, legal or regulatory position adopted in a model, a document
or a decision.

**5. Required actions.**

- **PCI-PFL-LAW-12.02-PR-01 — Identify the boundary question.** The credential holder must identify
  every question in their work whose answer depends on law, tax, accounting treatment, regulation or
  insurance coverage, and must record it as a boundary question rather than answering it.
- **PCI-PFL-LAW-12.02-PR-02 — Obtain written advice before adoption.** The credential holder must
  obtain written advice from a *qualified adviser* in the relevant jurisdiction before a *treatment* is
  adopted in a decision-grade output or represented as correct.
- **PCI-PFL-LAW-12.02-PR-03 — Model to the advice, and to its limits.** The credential holder must
  model the treatment as the advice states it, must record the advice's stated assumptions,
  qualifications, scope and date as part of the assumption's *basis*, and must not extend the treatment
  beyond the entity, structure, period or jurisdiction the advice covers.
- **PCI-PFL-LAW-12.02-PR-04 — Re-take advice on change.** The credential holder must obtain fresh
  advice where the structure, the entity, the jurisdiction, the period or the law changes, and must not
  carry an earlier opinion across the change.

**6. Prohibited actions.** Stating a legal, tax, accounting, regulatory or insurance conclusion;
describing a treatment as correct, standard, market or safe on the strength of experience; modelling a
treatment with no advice behind it and presenting it as the position; extending an opinion to another
entity or jurisdiction; relying on an adviser's oral indication; presenting a *treatment* whose advice
is qualified without carrying the qualification; using an opinion after the law it addressed has
changed.

**7. Required evidence.** The boundary-question log with dates and dispositions; the written advice
with its scope, assumptions, qualifications and date; the record linking each modelled treatment to the
advice that supports it; the fresh-advice records following each change; the *decision owner's*
acceptance of any treatment adopted without advice, and the reason.

**8. Responsible role.** The credential holder, personally, for staying inside the boundary. The
*decision owner* for the decision to adopt a treatment and for engaging the adviser.

**9. Approval authority.** The *qualified adviser* determines the treatment. The decision owner adopts
it. **No PCI law and no credential confers authority to determine a legal, tax, accounting, regulatory
or insurance position**, and no PCI process validates one.

**10. Independence requirement.** The *qualified adviser* must be engaged to advise the party relying
on the advice, or the advice must be capable of being relied upon by that party on its stated terms.
Advice addressed to another party and not capable of being relied upon is recorded as such, and is not
treated as advice to the relying party.

**11. Materiality or threshold.** The boundary is not a matter of size: a legal or tax conclusion is
outside the credential holder's competence regardless of the amount at stake. Materiality governs the
*depth and formality* of the advice sought — the *decision owner* records, in the engagement's
materiality statement, the exposure at which a formal written opinion is required rather than a
confirming note from the adviser, in the transaction's own metric. **PCI sets no figure.** *Scale
test:* on a small municipal project a single confirming letter from local counsel and the entity's
auditor may cover the whole treatment set, and the burden is proportionate; on a multi-billion
cross-border financing the log runs to hundreds of questions across jurisdictions, is maintained per
jurisdiction and per entity, and PR-04 is what prevents a five-year-old structuring opinion being
relied upon after a tax reform.

**12. Exception and waiver.** No exception is permitted to element 1. Where a decision must be taken
before advice can be obtained, the *decision owner* may approve in writing the use of a provisional
treatment, provided it is labelled provisional on the face of the output, the advice is sought
immediately, the output is re-based when the advice arrives, and every recipient is notified. Duration:
until the advice is received. Compensating control: the provisional treatment is excluded from any
representation, certificate or public document.

**13. Escalation trigger.** A request for a legal, tax or accounting conclusion; a treatment adopted
with no advice; advice that is qualified in a way that changes the modelled result; a change of law,
structure, entity or jurisdiction after advice was given; an adviser declining to opine; a treatment
that depends on a filing or clearance not yet obtained.

**14. AI application.** AI may summarise advice received for human reading, identify questions in a
document set that are likely to be boundary questions, track which treatments have advice and which do
not, compare a modelled treatment against the advice's stated scope, and draft the boundary-question log.

**15. AI prohibition.** **AI must not give legal, tax, accounting, regulatory or insurance advice, and
must not be relied upon as a source for any *treatment*.** AI must not conclude that a treatment is
correct, must not confirm that an opinion covers a structure, and must not be recorded as the basis of
a treatment. An AI answer to a boundary question is deleted, not filed.

**16. AI verification.** Clause-to-output comparison, by a named human, of every AI summary of advice
against the advice document itself, including its assumptions and qualifications; and confirmation by
the *qualified adviser*, where the summary is to be relied upon, that the summary is accurate. A summary
of advice that the adviser has not confirmed is not advice.

**17. External reference.**

- **IAS 12 *Income Taxes*.** Issuing organisation: IFRS Foundation / IASB. Subject: the financial
  *reporting* of income taxes. Checked: current, by name only (register `EXT-121`, verified
  2026-08-03). Nature: Manual §6 category 2 — authoritative financial-reporting standard. Applicability
  limitation: entities applying IFRS Accounting Standards in an adopting jurisdiction only; **it
  governs the reporting of tax, never the tax position itself**, which is a matter of the applicable tax
  law and requires qualified local advice.
- **OECD *Model Tax Convention on Income and on Capital*.** Issuing organisation: OECD. Subject: a
  model text used in negotiating bilateral tax treaties. Checked: current, by name only; no article or
  date asserted (register `EXT-129`, verified 2026-08-03). Nature: Manual §6 category 10 — illustrative
  practice. Applicability limitation: **it is a model instrument and is not law in any jurisdiction.**
  Only an executed treaty between the relevant states, as applied under each state's domestic law,
  binds — and its terms commonly differ from the model. Named for context only.
- **ISO/IEC 17024 *Conformity assessment — General requirements for bodies operating certification of
  persons*.** Issuing organisation: ISO/IEC. Subject: the scope and limits of what a personnel
  certification attests. Checked: a **2026 edition has been published, superseding the 2012 edition**
  (register `EXT-022`, verified 2026-08-03). Nature: Manual §6 category 3 — international voluntary
  standard. Applicability limitation: voluntary unless imported by law or contract; **PCI claims no
  accreditation to it through this reference**, and a PCI credential attests competence in project
  finance, not authority to advise on law or tax.

**18. Jurisdictional caution.** This law is a jurisdictional caution in its entirety. Tax residence,
permanent establishment, withholding, transfer pricing, interest limitation, indirect tax on
construction and on tariffs, the creation and perfection of security interests, the priority of
creditors, insolvency and enforcement, exchange controls, regulatory licences, sanctions and
financial-crime obligations, and the availability and scope of insurance are **all** jurisdiction-
specific, frequently amended, and often interact across the several jurisdictions present in one
financing. An answer obtained for one entity, one structure, one period or one jurisdiction says nothing
about another.

**19. Related PCI Laws.** `PCI-FND-LAW-08` (competence boundaries and referral); `PCI-FND-LAW-11`;
`PCI-FND-LAW-14`; `PCI-PFL-LAW-06.03`; `PCI-PFL-LAW-09.02`; `PCI-PFL-LAW-10.04`;
`PCI-PFL-LAW-12.01`. **Increment over the foundational parent:** `PCI-FND-LAW-08` requires referral
beyond competence; this law makes the referral operational in a financing — a logged boundary question,
written advice before adoption, modelling to the advice's stated limits, and fresh advice on any change
of entity, structure, period or jurisdiction, which is the failure mode a general competence rule never
catches.

**20. Related Body of Knowledge content.** PFL-AI · Domain 12 — Contracts and transaction structure ·
KA 12.1–12.4, whose treatment is expressly commercial, with jurisdiction-specific matters referred to
qualified counsel. Also Domain 2 KA 2.3 (project-relevant treatments) and Domain 1 KA 1.3 (fiduciary
awareness; financial crime, bribery, sanctions and the money-laundering perimeter).

**21. Compliance test.** A reviewer takes the model, the outputs and the advice file, and performs four
steps. (a) Lists every *treatment* in the model — tax rate, allowances, depreciation, withholding,
indirect tax, security, accounting classification — and confirms each is linked to a written advice
document. (b) For each, confirms the advice covers the entity, the structure, the period and the
jurisdiction modelled, and that its qualifications are carried into the assumption's *basis*. (c)
Searches the outputs and correspondence for statements of legal, tax, accounting, regulatory or
insurance conclusion by the credential holder, and finds none. (d) Confirms that each change of
structure, entity, jurisdiction or law in the period was followed by fresh advice. Compliance is
demonstrated when all four complete; a modelled treatment with no advice is a breach.

**22. Breach indicators.** A tax rate in a model with "standard rate" as its basis; an email answering
"is this deductible?"; an opinion addressed to another party filed as this party's advice; a structure
extended to a new jurisdiction on the old opinion; an adviser's qualification stripped out of the model
note; a phrase such as "market practice is" used to settle a legal question.

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Ethical dilemma: a sponsor presses for a view on deductibility before
close and the candidate must state what they may and may not say, and what they must obtain. Evidence
selection: identifying which of four documents constitutes advice the relying party may rely upon. No
live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-12-01` *The Advice Boundary* (v1.0). Amendment note: restructured onto
the twenty-five-element form; *qualified adviser* and *treatment* defined; the boundary-question log,
the modelling-to-the-limits rule and the fresh-advice-on-change rule made process requirements; the
OECD Model Tax Convention characterisation tightened to state that it is not law in any jurisdiction.

---
<!-- LAWS -->
