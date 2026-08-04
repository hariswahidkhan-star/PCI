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
<!-- LAWS -->
