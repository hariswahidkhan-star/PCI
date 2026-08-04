# PFL-AI Professional Laws — PCI AI Project Finance Leader

**Status:** Certification Laws for the **PCI AI Project Finance Leader** credential (PFL-AI).
Version 2.0 — a complete reconstruction of the twenty-four-law v1.0 set onto the twenty-five-element
structure required by the **PCI Law Drafting Manual** §5. **Thirty-three laws** and **one hundred and
fifty-five process requirements**, anchored to eleven of the sixteen domains of the PFL-AI Body of
Knowledge (`../pfl-ai/`).

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
- **PCI-PFL-LAW-01.01-PR-05 — Stated and internally consistent appraisal basis.** The preparer must
  state, for every appraisal or return measure presented, the inflation basis and the currency basis of
  the cash flows **and** of the discount rate, the horizon and the perspective from which the measure is
  taken; must keep the cash flows and the discount rate on the same inflation and currency basis; and
  must not re-base, re-cut or re-select a measure after a result is known in order to produce a preferred
  conclusion.

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
prepare the schedule discharges this element.

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
presentation-dependency disclosure. (e) For each appraisal or return measure presented, confirms the
stated basis is complete and that the cash flows and the discount rate share one inflation and one
currency basis, by recomputing the measure on the stated basis. Compliance is demonstrated when all five
steps complete; failure of any one is a breach.

**22. Breach indicators.** A real discount rate applied to nominal cash flows, or a measure whose
horizon changed between drafts; a coverage or liquidity conclusion with no dated obligation schedule
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
**Reasonable party** — the test applied under this law: a party to the financing, acting with the
information ordinarily available to that party, who is weighing whether to rely on the credential
holder's judgement. The test is applied by the credential holder, and where doubt exists it is resolved
by disclosing rather than by concluding. **Connected person** — for this law, a person whose financial
position a *reasonable party* would treat as affecting the credential holder's judgement: a spouse or
partner, a dependant, a person sharing a household, an entity the credential holder or any of them
controls or in which any of them holds a financial interest, and the credential holder's employer and
its group.

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
size, and the test is the *reasonable party* test defined at element 4. A *de
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
- **PCI-PFL-LAW-06.03-PR-05 — Revenue characterised by its actual basis.** The model owner must record,
  for every revenue assumption, whether the amount is contracted, regulated, availability-based or
  forecast, whether it is indexed or fixed, and the credit standing of the party that pays — and must
  not present a forecast or merchant revenue as though it were contracted.

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
assumption value, decide that a recorded *basis* meets PR-02, or re-confirm an expired assumption.

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
re-confirmation record exists. (e) For each revenue assumption, confirms the register records its actual
basis — contracted, regulated, availability-based or forecast — and that any amount shown as contracted
is traced to an executed agreement under `PCI-PFL-LAW-12.01`. Compliance is demonstrated when all five
complete; an unowned or unsourced *material* assumption is a breach, and so is a forecast revenue
presented as contracted.

**22. Breach indicators.** A merchant price curve shown in the same column as a contracted tariff; a
register with a "source" column reading "internal"; the same escalation
rate typed on three sheets; a model released without its register; an assumption dated two years before
the close it supports; a basis that cites the previous transaction; an owner column populated with a
team name.

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Evidence selection: from a register extract, the candidate identifies
which assumptions must not be used in a decision-grade case and why. Scenario judgement: a stale price
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
close and the candidate must state what they may say, what they must not say, and what they must obtain. Evidence
selection: identifying which of four documents constitutes advice the relying party may rely upon. No
live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-12-01` *The Advice Boundary* (v1.0). Amendment note: restructured onto
the twenty-five-element form; *qualified adviser* and *treatment* defined; the boundary-question log,
the modelling-to-the-limits rule and the fresh-advice-on-change rule made process requirements; the
OECD Model Tax Convention characterisation tightened to state that it is not law in any jurisdiction.

---
## Domain 13 — Due diligence and financial close

### PCI LAW PCI-PFL-LAW-13.01 — Independent Model Review

**1. Normative requirement.** A model audit, diligence review or adviser's report relied upon by any
party other than the reviewer's own team must be performed by a person *independent* of the work
reviewed.

**2. Purpose.** A review is worth exactly what its independence is worth. A reviewer who built the
model, who reports to the person who built it, or whose continuing mandate depends on a clean
conclusion, produces a document that reads like assurance and functions as advocacy — and the parties
relying on it cannot tell the difference from the outside.

**3. Scope.** Every model audit, financial-model review, technical or financial diligence report,
lender's adviser report, second review of a *base case*, and every internal review whose conclusion is
communicated outside the preparing team. Applies to the reviewer, to the person engaging the reviewer,
and to the credential holder who relies on the report.

**4. Defined terms.** *independent*, *competent reviewer*, *verified*, *material*, *evidence*,
*decision owner*, *finance documents*, *authoritative version*, *model owner*. **Review scope
statement** — a written statement, issued before the review begins, of what is reviewed, what is
excluded, the *materiality* figure applied, the procedures performed and the limitations on reliance.
**Finding** — a difference, error, omission or limitation identified by the review, recorded with its
quantified effect where it can be quantified.

**5. Required actions.**

- **PCI-PFL-LAW-13.01-PR-01 — Independence recorded before engagement.** The reviewer must record, and
  the engaging party must obtain, a statement addressing each of the four limbs of *independent* before
  the engagement begins, and must update it if circumstances change.
- **PCI-PFL-LAW-13.01-PR-02 — Scope statement issued first.** The reviewer must issue the *review scope
  statement* before the review begins, and must not extend, narrow or re-date it after the findings are
  known without recording the change and its reason.
- **PCI-PFL-LAW-13.01-PR-03 — Findings reported with quantified effect.** The reviewer must report
  every *finding*, with its quantified effect where it can be quantified and with a statement that it
  cannot where it cannot, and must not suppress, aggregate or reclassify a finding to reach a cleaner
  conclusion.
- **PCI-PFL-LAW-13.01-PR-04 — Version identified.** The reviewer must identify the *authoritative
  version* reviewed and must state expressly that the report does not extend to any later version.
- **PCI-PFL-LAW-13.01-PR-05 — No self-review.** A credential holder must not review their own work, and
  must not accept a review engagement whose fee, continuing mandate or other benefit varies with the
  conclusion reached.

**6. Prohibited actions.** Reviewing one's own work; accepting a fee or mandate contingent on the
conclusion; issuing a report without a scope statement; re-dating a scope statement after findings are
known; describing a review as independent where any limb of *independent* fails; presenting a review of
one version as covering another; agreeing a finding's wording with the party reviewed in exchange for
its removal.

**7. Required evidence.** The independence statement addressing all four limbs, dated before
engagement; the review scope statement as first issued and any recorded amendment; the findings log
with quantified effects; the version identification; the engagement terms showing the fee basis; the
reviewer's working papers supporting each finding.

**8. Responsible role.** The named individual reviewer who signs the report, personally. The engaging
party's *decision owner* for engaging a reviewer who satisfies element 10.

**9. Approval authority.** The reviewer alone determines the findings and the conclusion. The engaging
party may determine the scope **before** the review begins; it must not determine a finding. A dispute
about a finding is recorded, not resolved by the engaging party.

**10. Independence requirement.** All four limbs of *independent* apply, in relation to the work
reviewed and to the transaction: the reviewer did not prepare the work; is not in the preparer's
reporting line and does not report to a person whose performance is measured by the conclusion; receives
no benefit varying with the conclusion; and holds no financial interest in the transaction or a party
to it. **Where any limb fails, the work may still be useful, but it must not be described as
independent**, and the failing limb must be stated in the report.

**11. Materiality or threshold.** The *materiality* figure applied is stated in the review scope
statement, in the transaction's own metric — for example a stated movement in the minimum *coverage
ratio*, which is how model audits in this market ordinarily express it. **PCI sets no figure**, because
a defensible figure depends on the transaction's size, its headroom and the decision the review
supports; the *decision owner* and the reviewer agree it before the review begins, and it is not
changed after findings are known. *Scale test:* on a small municipal project a single reviewer, a short
scope statement and a materiality expressed in ratio units are proportionate; on a multi-billion
cross-border financing the scope statement is partitioned by workstream and tranche, each partition
carries its own materiality, and PR-04 matters most because the model moves daily during close.

**12. Exception and waiver.** No exception is permitted to PR-05 or to element 10's description rule. A
review performed by a person who fails a limb of independence may be issued as a **review that is not
independent**, expressly so labelled, with the failing limb stated, approved in writing by the engaging
party's *decision owner*, and reported to every party to whom it is provided. It must never be
described, cited or relied upon as an independent review.

**13. Escalation trigger.** Discovery that a limb of independence fails after engagement; pressure to
remove, soften or aggregate a finding; a scope narrowed after findings are known; a *material* finding
that the engaging party declines to disclose to a party relying on the report; a report cited in
support of a later model version.

**14. AI application.** AI may recompute a model, run regression and check-block suites, compare two
versions, test formula consistency, extract contractual terms for the reviewer's clause-level reading,
and draft findings for the reviewer's confirmation.

**15. AI prohibition.** AI must not perform the review; must not sign, issue or approve a report; must
not decide that a difference is not a finding; must not determine *materiality*; and must not be
described as independent, because independence is a property of a relationship and a tool has none.

**16. AI verification.** Independent recomputation by the named reviewer of every *material* AI-derived
finding; clause-to-output comparison of AI-extracted contractual terms against the executed clause under
`PCI-PFL-LAW-12.01`; boundary testing of any AI-run check suite by seeding a known error; and the
reviewer's personal sign-off on each finding. **The reviewer's name on the report attests to the
reviewer's own work, not to the tool's.**

**17. External reference.**

- **IESBA / IFAC — *International Code of Ethics for Professional Accountants (including International
  Independence Standards)*.** Issuing organisation: the International Ethics Standards Board for
  Accountants, under IFAC. Subject: ethics and independence for professional accountants. Checked:
  current, by name only; no section or date asserted (register `EXT-127`, verified 2026-08-03). Nature:
  Manual §6 category 6 — ethical code. Applicability limitation: **binding only where a professional
  body, regulator or engagement has adopted it. A PCI credential holder who is not subject to it is not
  made subject to it by a PCI law**; it is named because its independence concepts are the reference
  discipline this law's four limbs express in PCI's own words.
- **ISO/IEC 17024 *Conformity assessment — General requirements for bodies operating certification of
  persons*.** Issuing organisation: ISO/IEC. Subject: impartiality in certification of persons.
  Checked: a **2026 edition has been published, superseding the 2012 edition** (register `EXT-022`,
  verified 2026-08-03). Nature: Manual §6 category 3 — international voluntary standard. Applicability
  limitation: voluntary unless imported by law or contract; addresses certification bodies, not
  reviewers of financial models. **No PCI accreditation is claimed through this reference.**
- **The Equator Principles.** Issuing organisation: the Equator Principles Association. Subject: the
  independent-review expectations adopting institutions apply to environmental and social assessment.
  Checked: EP4, adopted 18 November 2019, effective 1 October 2020 (register `EXT-082`, verified
  2026-08-03). Nature: Manual §6 category 8 — voluntary environmental or social framework.
  Applicability limitation: **voluntary; never legislation**; applies to adopting institutions' own
  transactions.

**18. Jurisdictional caution.** Whether a reviewer owes a duty to a third party relying on a report,
the effect of a reliance letter or a hold-harmless, the enforceability of a liability cap in an
engagement letter, and any statutory or professional restriction on providing both advisory and
assurance services to the same client, are jurisdiction- and profession-specific. Obtain qualified
legal advice on reliance and liability before a report is provided to a party outside the engagement.

**19. Related PCI Laws.** `PCI-FND-LAW-10`; `PCI-FND-LAW-08`; `PCI-PFL-LAW-01.02`;
`PCI-PFL-LAW-06.05`; `PCI-PFL-LAW-13.02`; `PCI-PFL-LAW-16.03`. **Increment over the foundational
parent:** `PCI-FND-LAW-10` requires conflicts to be disclosed; this law goes further for a review by
making independence a four-limb factual test recorded *before* engagement, forbidding self-review
outright, fixing the scope and materiality before findings are known, and requiring a non-independent
review to be labelled as one rather than merely disclosed.

**20. Related Body of Knowledge content.** PFL-AI · Domain 13 — Due diligence and financial close ·
KA 13.2 Model audit · including the materiality threshold expressed in the transaction's own metric.
Also KA 13.1 (the diligence streams) and Domain 6 KA 6.4 (model audit and its economics).

**21. Compliance test.** A reviewer of the review takes the report, the engagement file and the model,
and performs five steps. (a) Confirms an independence statement addressing all four limbs is dated
before the engagement began. (b) Confirms the fee basis in the engagement terms does not vary with the
conclusion. (c) Compares the scope statement as first issued with the one in the report and confirms
either that they match or that a recorded, reasoned amendment exists dated before the findings. (d)
Confirms every finding in the working papers appears in the report, with its quantified effect. (e)
Confirms the report identifies the model version reviewed and disclaims later versions. Compliance is
demonstrated when all five complete; a finding in the working papers absent from the report is a
breach.

**22. Breach indicators.** A report describing itself as independent with no independence statement; a
success fee in the engagement letter; a scope statement dated after the fieldwork; findings graded
"observation" that carry *material* effects; a report cited at close against a model version issued
afterwards; a reviewer employed by the sponsor reviewing the sponsor's own model.

**23. Consequence within PCI authority.** Correction required and the affected report withheld from
reliance; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Ethical dilemma: a reviewer is asked to downgrade a finding in exchange
for the next mandate. Evidence selection: from an engagement file, the candidate identifies which
documents establish independence and which do not. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-13-01` *Independence of Review* (v1.0). Amendment note: restructured onto
the twenty-five-element form; *review scope statement* and *finding* defined; the four-limb independence
test brought in from the Definitions; the labelled non-independent review route added at element 12,
which v1.0 lacked and which practitioners resolved by calling such reviews independent anyway.

---

### PCI LAW PCI-PFL-LAW-13.02 — Adviser Independence

**1. Normative requirement.** A credential holder must not describe themselves, their firm, their
advice or their output as *independent* while any limb of the definition of *independent* fails.

**2. Purpose.** *Independent* is the most load-bearing word an adviser uses, and the cheapest to
misuse. It is not a description of character, effort or good intentions; it is a factual claim about
four specific relationships — authorship, reporting line, remuneration and financial interest — and a
party pricing risk on the strength of the word is entitled to have all four be true.

**3. Scope.** Every credential holder acting as, or engaged as, an adviser, reviewer, expert, modeller,
monitor, technical adviser, market consultant or assurance provider on a project financing, and every
credential holder who commissions, engages or relies upon such a person. Covers descriptions in
engagement letters, reports, presentations, marketing material, credentials pages and oral statements.

**4. Defined terms.** *independent*, *competent reviewer*, *decision owner*, *material*, *evidence*,
*verified*. **Contingent benefit** — any fee, bonus, commission, carried interest, continuing mandate,
future engagement or other benefit whose existence or amount varies with the conclusion reached or with
whether the transaction proceeds. **Role separation** — the arrangement by which the individual advising
on a matter is not the individual reviewing or approving it.

**5. Required actions.**

- **PCI-PFL-LAW-13.02-PR-01 — Four-limb test recorded.** The credential holder must record, against
  each of the four limbs of *independent*, whether it is satisfied, before using the word in relation
  to themselves or their output.
- **PCI-PFL-LAW-13.02-PR-02 — Contingent benefits disclosed.** The credential holder must disclose in
  writing every *contingent benefit* to every party relying on the output, before the output is relied
  upon.
- **PCI-PFL-LAW-13.02-PR-03 — Role separation within a firm.** Where one firm both advises and reviews,
  the firm must record the *role separation*, name the individuals on each side, and must not describe
  the review as independent of the firm — only, where the four limbs hold at individual level, as
  independent of the advising team.
- **PCI-PFL-LAW-13.02-PR-04 — Correction of description.** The credential holder must correct, in
  writing and to every recipient, any description of themselves or their output as independent that has
  become inaccurate.

**6. Prohibited actions.** Using *independent* where any limb fails; describing a firm as independent
where the firm holds a *contingent benefit*, even if the individual does not; presenting *objective*,
*impartial* or *arm's-length* as equivalent to independent where the four limbs do not hold; accepting a
success fee on an assurance engagement; allowing a marketing description to outlive the arrangement it
described.

**7. Required evidence.** The four-limb record, dated; the written disclosure of every contingent
benefit with its recipients; the role-separation record naming individuals; engagement letters and fee
schedules; the corrections issued under PR-04 with their distribution.

**8. Responsible role.** The credential holder, personally, for descriptions of themselves and their
output. The engaging organisation's responsible partner or officer for firm-level descriptions.

**9. Approval authority.** No one may approve the use of the word where a limb fails — **the word is a
statement of fact, and a fact is not within anyone's authority to approve.** The engaging party's
*decision owner* approves the engagement on the disclosed basis.

**10. Independence requirement.** This law *is* the independence requirement for adviser roles.
Independence is assessed at the level of the individual signing and, separately, at the level of the
firm, and both assessments are recorded because they can differ.

**11. Materiality or threshold.** No materiality applies to the word: a limb either holds or it does
not. Materiality governs the *disclosure* of a benefit that does not defeat independence — the engaging
organisation's governance records the value or class of ordinary-course benefit that need not be
disclosed, and that record must exclude any benefit connected to the transaction or its parties. **PCI
sets no figure.** *Scale test:* on a small municipal project the adviser is often a single person and
the test is answered in four lines; on a multi-billion cross-border financing with a firm holding
several roles across the same transaction and its parties, PR-03 does the work, and the honest answer
is frequently "independent of the advising team, not of the firm" — which this law requires to be said
in exactly those terms.

**12. Exception and waiver.** No exception is permitted. The available route is accurate description:
an adviser who fails a limb may act, and may be useful, provided the output is not described as
independent and the failing limb is stated. That route requires no approval because it is compliance.

**13. Escalation trigger.** A limb failing after the word has been used; a contingent benefit introduced
mid-engagement; a firm accepting a second role on the same transaction; discovery that marketing
material describes an engagement as independent when it is not; a party relying on the word who has not
received the disclosures.

**14. AI application.** AI may screen a firm's engagement and fee data against the four limbs, surface
other engagements with parties to the transaction, monitor for newly arising contingent benefits, and
draft disclosure wording for review.

**15. AI prohibition.** AI must not determine that a person or firm is independent, decide that a
contingent benefit is immaterial, approve a description, or be described as independent itself.

**16. AI verification.** Source tracing of each AI-surfaced engagement or benefit to the record that
evidences it; the credential holder's own recorded confirmation against personal knowledge that no limb
fails; and clause-to-output comparison of each AI-drafted disclosure against the fee schedule it
describes. A clean machine screen is not a finding of independence.

**17. External reference.**

- **IESBA / IFAC — *International Code of Ethics for Professional Accountants (including International
  Independence Standards)*.** Issuing organisation: IESBA, under IFAC. Subject: independence in fact and
  in appearance, and threats to it. Checked: current, by name only (register `EXT-127`, verified
  2026-08-03). Nature: Manual §6 category 6 — ethical code. Applicability limitation: **binding only
  where a professional body, regulator or engagement has adopted it; a PCI credential holder who is not
  subject to it is not made subject to it by a PCI law.** Named as the reference discipline; this law's
  four limbs are PCI's own formulation.
- **G20/OECD *Principles of Corporate Governance*.** Issuing organisation: OECD, with the G20. Subject:
  disclosure and the management of conflicts in governance. Checked: 2023 revision, OECD/LEGAL/0413
  (register `EXT-128`, verified 2026-08-03). Nature: Manual §6 category 5 — professional framework;
  specifically an **OECD Council Recommendation — intergovernmental, non-binding, not legislation
  anywhere.** Applicability limitation: creates no obligation for a credential holder.

**18. Jurisdictional caution.** Statutory and professional restrictions on combining advisory and
assurance roles, rules on adviser registration and authorisation, restrictions on success fees for
certain services, and the legal effect of describing an output as independent differ by jurisdiction and
by profession — and a description lawful in one place can be a regulated statement in another. Obtain
qualified legal and professional-body advice before using the word in a regulated context.

**19. Related PCI Laws.** `PCI-FND-LAW-10`; `PCI-FND-LAW-13`; `PCI-FND-LAW-14`; `PCI-PFL-LAW-01.02`;
`PCI-PFL-LAW-13.01`; `PCI-PFL-LAW-09.03`. **Increment over the foundational parent:**
`PCI-FND-LAW-10` requires conflicts to be disclosed and `PCI-FND-LAW-14` forbids misrepresenting a PCI
credential; this law addresses a different misrepresentation — the word *independent* applied to an
adviser or an output — and converts it from a self-description into a recorded four-limb factual test
assessed at both individual and firm level.

**20. Related Body of Knowledge content.** PFL-AI · Domain 13 — Due diligence and financial close ·
KA 13.1 The diligence streams. Also Domain 1 KA 1.3 (conflicts and independence) and Domain 13 KA 13.2
(model audit).

**21. Compliance test.** A reviewer takes every document in which the credential holder or their output
is described as independent and performs four steps. (a) Locates the four-limb record and confirms it
is dated before first use of the word. (b) Tests each limb against the engagement letter, the fee
schedule, the organisation chart and the firm's engagement register. (c) Confirms every *contingent
benefit* found is in a written disclosure to each relying party, dated before reliance. (d) Where one
firm holds both roles, confirms the description is qualified to independence of the advising team
rather than of the firm. Compliance is demonstrated when all four complete; the word used with a failing
limb is a breach, whether or not the advice was in fact objective.

**22. Breach indicators.** A report headed "independent" with a success fee in the appendix; a firm
describing itself as independent while holding an arranging mandate; a four-limb record created after
the report; a marketing page describing a lapsed arrangement; a reviewer inside the preparer's reporting
line; the words *objective* and *independent* used interchangeably.

**23. Consequence within PCI authority.** Correction required and the affected description withdrawn;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Scenario judgement: a firm advising the sponsor is asked to provide the
lenders' model audit, and the candidate must state what may be described how. Ethical dilemma: a
proposed fee uplift payable on financial close, offered to an assurance provider. No live examination
content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law** — v1.0 addressed independence only inside `PFL-LAW-13-01`, and only for reviews.
Amendment note: *contingent benefit* and *role separation* defined; firm-level and individual-level
assessment separated; the correction duty added; the honest-description route made explicit so that
useful non-independent work is not driven into misdescription.

---

### PCI LAW PCI-PFL-LAW-13.03 — Conditions Precedent

**1. Normative requirement.** A credential holder must treat a *condition precedent* as satisfied only
when the *evidence* the *finance documents* require has been delivered and accepted by the party
entitled to accept it.

**2. Purpose.** Conditions precedent are the mechanism by which lenders control when money moves. A
condition recorded as satisfied on the strength of an expectation, a draft, an oral acceptance or a
substantially complete deliverable moves the drawing forward and the protection backward — and the
record shows a clean close.

**3. Scope.** Every credential holder who tracks, prepares, certifies, reviews, accepts or reports on
conditions precedent to effectiveness, first drawing, a subsequent drawing, a release, a completion
test or any other event the finance documents condition. Applies at close and throughout the
availability period.

**4. Defined terms.** *conditions precedent*, *finance documents*, *evidence*, *waiver*, *material*,
*decision owner*, *competent reviewer*, *verified*, *source line*. **CP register** — a record listing
each condition, the clause requiring it, the deliverable, the party entitled to accept it, the
acceptance status, the date, and any *waiver* or deferral with its terms. **Acceptance** — the
confirmation, in the form the finance documents require, by the party entitled to give it, that the
deliverable satisfies the condition.

**5. Required actions.**

- **PCI-PFL-LAW-13.03-PR-01 — CP register maintained.** The credential holder must maintain a *CP
  register* with all seven fields per condition, sourced to the clause requiring it.
- **PCI-PFL-LAW-13.03-PR-02 — Status recorded truthfully.** The credential holder must record each
  condition as *satisfied*, *outstanding*, *waived* or *deferred*, and must not record a waived or
  deferred condition as satisfied.
- **PCI-PFL-LAW-13.03-PR-03 — Acceptance evidenced.** The credential holder must hold, for each
  satisfied condition, the deliverable and the *acceptance* by the party entitled to accept it, in the
  form the documents require.
- **PCI-PFL-LAW-13.03-PR-04 — Waiver and deferral terms carried.** The credential holder must record,
  for each waived or deferred condition, who waived or deferred it, on what terms, until when, and what
  must then happen — and must track it to closure.
- **PCI-PFL-LAW-13.03-PR-05 — No certification while any condition is outstanding.** The credential
  holder must not certify or report that conditions are met while any remains outstanding, and must
  identify the outstanding conditions by name in any status report.

**6. Prohibited actions.** Recording a condition as satisfied on a draft, an expectation, an oral
assurance or a substantially complete deliverable; recording a waived or deferred condition as
satisfied; certifying a complete condition set while one is outstanding; accepting a deliverable on
behalf of a party not entitled to accept it; allowing a deferral to lapse untracked; back-dating an
acceptance.

**7. Required evidence.** The CP register with clause references; each deliverable with its *source
line*; each acceptance in the required form; the waiver and deferral instruments with their terms and
long-stop dates; the closure record for each deferral; the status reports issued.

**8. Responsible role.** The credential holder maintaining the register and reporting status. The
lenders' agent, or the party the documents name, for acceptance.

**9. Approval authority.** Only the party the *finance documents* entitle may accept a deliverable,
waive a condition or grant a deferral, in the form those documents require. No credential holder, and
no PCI law, can substitute for that party.

**10. Independence requirement.** A *competent reviewer* independent of the transaction team must
reconcile the CP register to the documents and to the deliverables before financial close and before
first drawing, because the transaction team's benefit runs to close occurring.

**11. Materiality or threshold.** No materiality applies to satisfaction: a condition is satisfied or it
is not, and the *finance documents* set the standard, not PCI. Materiality governs *escalation timing*
— the *escalation threshold* is the number of days before the target close or drawdown date at which an
outstanding condition must be escalated, set by the adopting organisation's governance or by the
transaction timetable. *Scale test:* on a small municipal project the register may hold twenty
conditions and one accepting party, and reconciliation takes an hour; on a multi-billion cross-border
financing it holds several hundred across facilities with different accepting parties and different
forms of acceptance, the register is maintained per facility, and PR-04 does the heaviest work because
deferrals accumulate through a long close and each carries its own long-stop.

**12. Exception and waiver.** No exception to element 1 is permitted, because the law's whole subject is
the difference between satisfaction and its absence. A condition may be *waived* or *deferred* by the
party entitled to do so, in the form the documents require — that is the documents' own exception
mechanism, and this law requires only that it be recorded as what it is and tracked to closure.

**13. Escalation trigger.** A condition outstanding within the escalation window; a deliverable
accepted informally or by a party not entitled to accept; a deferral whose long-stop is approaching or
has passed; a waiver granted without terms; a condition satisfied by a document that has since been
amended.

**14. AI application.** AI may build a draft CP register from the finance documents, match deliverables
in the data room to conditions, track status and long-stop dates, alert on approaching deadlines, and
draft the status report for review.

**15. AI prohibition.** AI must not accept a deliverable, determine that a condition is satisfied,
waive or defer a condition, certify a condition set, or record an acceptance.

**16. AI verification.** Clause-to-output comparison, by a named human, of every AI-built register entry
against the clause requiring the condition; source tracing of each satisfied status to the deliverable
**and** to the acceptance instrument; and confirmation that the accepting party is the one the documents
entitle. An AI match between a data-room document and a condition is a candidate, not a satisfaction.

**17. External reference.**

- **ISO 15489-1 *Information and documentation — Records management — Part 1: Concepts and
  principles*.** Issuing organisation: ISO. Subject: what makes a record reliable, authentic and
  retrievable — the properties a condition-satisfaction record needs. Checked: ISO 15489-1:2016
  (register `EXT-025`, verified 2026-08-03). Nature: Manual §6 category 3 — international voluntary
  standard. Applicability limitation: voluntary unless imported by law or contract; it sets no condition
  and no retention period for a financing.
- **The Equator Principles.** Issuing organisation: the Equator Principles Association. Subject:
  environmental and social conditions that adopting institutions commonly make conditions to
  effectiveness or drawing. Checked: EP4, adopted 18 November 2019, effective 1 October 2020 (register
  `EXT-082`, verified 2026-08-03). Nature: Manual §6 category 8 — voluntary environmental or social
  framework. Applicability limitation: **voluntary; never legislation.** Where an adopting institution
  makes it a condition, the obligation comes from the finance documents, not from the framework.

**18. Jurisdictional caution.** Whether a security interest is validly created and perfected, whether a
legal opinion is capable of being relied upon, whether an authorisation is final or appealable, whether
notarisation, registration, stamping or translation is required, and what constitutes valid execution
are all jurisdiction-specific — and in a multi-jurisdictional financing each condition may be governed
by a different law. Obtain qualified local legal advice on each condition's satisfaction in its own
jurisdiction; a condition satisfied under one law is not thereby satisfied under another.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-07`; `PCI-FND-LAW-11`;
`PCI-PFL-LAW-12.01`; `PCI-PFL-LAW-13.04`; `PCI-PFL-LAW-14.02`; `PCI-PFL-LAW-15.03`. **Increment over
the foundational parent:** `PCI-FND-LAW-05` requires an evidenced trail; this law states what the trail
must contain for a condition — the deliverable *and* the acceptance by the entitled party in the
required form — and forbids the specific substitution that a close under time pressure invites, namely
recording a waiver or a deferral as satisfaction.

**20. Related Body of Knowledge content.** PFL-AI · Domain 13 — Due diligence and financial close ·
KA 13.3 Conditions precedent and documentation · including the CP chain and its duration. Also KA 13.4
(syndication and financial close) and Domain 14 KA 14.1 (draw requests and conditions).

**21. Compliance test.** A reviewer takes the finance documents, the CP register and the deliverable
file, and performs five steps. (a) Confirms every condition in the documents appears in the register
with its clause. (b) For each condition marked *satisfied*, locates the deliverable and the acceptance,
and confirms the acceptance is by the entitled party in the required form. (c) Confirms no waived or
deferred condition is marked satisfied. (d) Confirms each deferral has terms, a long-stop and a
tracking entry. (e) Confirms any status report issued while a condition was outstanding named it.
Compliance is demonstrated when all five complete; a satisfied status with a deliverable but no
acceptance is a breach.

**22. Breach indicators.** A register with a status column and no acceptance column; a condition
satisfied by a draft opinion; a waiver recorded as "agreed"; a deferral with no long-stop; a certificate
of satisfaction issued the day before the last deliverable arrived; an acceptance signed by the
borrower's own counsel where the documents require the agent's.

**23. Consequence within PCI authority.** Correction required and the affected certificate or report
withheld; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Escalation decision: two conditions remain outstanding on the morning
of close and the candidate must state what may be certified, by whom, and what must be escalated.
Evidence selection: distinguishing a deliverable from an acceptance. No live examination content is
exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-13-02` *Conditions Precedent* (v1.0). Amendment note: restructured onto
the twenty-five-element form; *CP register* and *acceptance* defined; the seven register fields
specified; waiver and deferral tracking to closure added as PR-04; the compliance test made performable
by separating deliverable from acceptance.

---

### PCI LAW PCI-PFL-LAW-13.04 — Financial-Close Readiness

**1. Normative requirement.** A credential holder must not treat a transaction as closed until the
complete closing record — the executed document set, the locked *base case*, the final *sources and
uses*, the condition-satisfaction record and the *funds flow* — has been captured, identified and
retained as one record.

**2. Purpose.** Close is the point at which a transaction stops being negotiable and starts being
administered, often by people who were not there. Everything the project will be measured against for
twenty years is fixed on that day, and an incomplete record means that in three years nobody can say
what was agreed, at what version, on what numbers.

**3. Scope.** Every credential holder involved in financial close, first drawing, a syndication or
accession, an amendment-and-restatement, or a refinancing close. Applies to preparation, review,
certification and retention of the closing record.

**4. Defined terms.** *finance documents*, *base case*, *sources and uses*, *funds flow*, *authoritative
version*, *evidence*, *decision owner*, *competent reviewer*, *verified*, *conditions precedent*.
**Closing record** — the single, identified, retained set comprising the executed *finance documents*
and all ancillary executed documents, the locked *base case* with its integrity control, the final
*sources and uses*, the *CP register* with its deliverables and acceptances, the *funds flow* as
executed, and the register of *waivers* and deferrals outstanding at close. **Closed** — the state in
which the finance documents have become effective in accordance with their terms.

**5. Required actions.**

- **PCI-PFL-LAW-13.04-PR-01 — Assemble the closing record.** The credential holder must assemble the
  *closing record* in full, with an index identifying each component and its version.
- **PCI-PFL-LAW-13.04-PR-02 — Lock and control the base case.** The credential holder must ensure the
  *base case* is locked at close under `PCI-PFL-LAW-06.05-PR-05`, with an integrity control recorded in
  the index.
- **PCI-PFL-LAW-13.04-PR-03 — Reconcile the closing numbers.** The credential holder must reconcile the
  final *sources and uses* to the *funds flow* as executed and to the *base case*, and must explain
  every difference.
- **PCI-PFL-LAW-13.04-PR-04 — Carry the open items forward.** The credential holder must produce, as
  part of the record, a schedule of every condition waived or deferred at close and every post-close
  obligation, with its owner and its long-stop date, and must hand it to the party who will administer
  it.
- **PCI-PFL-LAW-13.04-PR-05 — Retain and make retrievable.** The credential holder must retain the
  closing record for the period the *finance documents*, applicable law and the engaging organisation's
  governance require, and must ensure it is retrievable by a person who was not present at close.

**6. Prohibited actions.** Treating a transaction as closed on an incomplete record; describing a
transaction as closed before the documents are effective in accordance with their terms; retaining a
closing record only as links to a data room that will be decommissioned; omitting the waived and
deferred items from the record; permitting the locked base case to be superseded in place; issuing a
closing certificate whose numbers do not reconcile to the funds flow.

**7. Required evidence.** The closing-record index with a version per component; the executed document
set; the locked base case with its integrity control value; the final sources and uses; the funds flow
as executed; the CP register with deliverables and acceptances; the open-items schedule with owners and
long-stops; the retention and retrievability record.

**8. Responsible role.** The project finance leader accountable for close. The *decision owner* for the
organisation's acceptance that the transaction has closed.

**9. Approval authority.** The parties the *finance documents* name determine when the documents become
effective. The *decision owner* approves the closing record as complete. **No credential holder may
declare a transaction closed on the strength of a timetable.**

**10. Independence requirement.** A *competent reviewer* independent of the closing team must confirm
the completeness of the closing record against the index and the reconciliation under PR-03, within a
period the engaging organisation's governance states after close, because the closing team disperses.

**11. Materiality or threshold.** Completeness is not a percentage: each component in the definition of
*closing record* is present or the record is incomplete. Materiality governs the *reconciliation
tolerance* under PR-03 — expressed in the transaction's currency at the rounding precision of the funds
flow, recorded by the *decision owner*. **PCI sets no figure.** *Scale test:* on a small municipal
project the closing record may be a single indexed folder assembled in a day; on a multi-billion
cross-border financing with several facilities, jurisdictions and currencies, the index is maintained
per facility, the funds flow reconciliation is performed per currency, and PR-05 is the binding
constraint because the record must outlive the data room, the arranger's mandate and the individuals.

**12. Exception and waiver.** No exception is permitted to element 1. Where a component genuinely cannot
be captured at close — a document in transit for notarisation, for example — the *decision owner* may
approve in writing a record marked incomplete, naming the missing component, its owner and the date by
which it will be added, and the record must be completed and re-indexed by that date. The incompleteness
is reported to the party that will administer the financing.

**13. Escalation trigger.** A component missing from the record after close; a reconciliation difference
between the funds flow and the sources and uses that cannot be explained; a locked base case whose
integrity control does not match; an open item with no owner; a retention arrangement that will not
outlive the data room.

**14. AI application.** AI may assemble and index the closing record, check the index against a template
for missing components, reconcile the sources and uses to the funds flow, extract post-close obligations
and long-stop dates into the open-items schedule, and verify that every executed document in the index
is present.

**15. AI prohibition.** AI must not determine that a transaction has closed, certify the record as
complete, approve a reconciliation difference, or be the sole holder of the record.

**16. AI verification.** Reconciliation, by a named human, of the AI-produced index against the executed
document list and against the CP register; independent recomputation of the sources-and-uses to
funds-flow reconciliation; and recomputation of the integrity control on the locked base case. Recorded
in the index with the reviewer's name and the date.

**17. External reference.**

- **ISO 15489-1 *Information and documentation — Records management — Part 1: Concepts and
  principles*.** Issuing organisation: ISO. Subject: authenticity, reliability, integrity and usability
  of records — the four properties a closing record must have to be usable years later. Checked: ISO
  15489-1:2016 (register `EXT-025`, verified 2026-08-03). Nature: Manual §6 category 3 — international
  voluntary standard. Applicability limitation: voluntary unless imported by law or contract; it sets no
  retention period for a project financing.
- **ISO/IEC 27001 *Information security, cybersecurity and privacy protection — Information security
  management systems — Requirements*.** Issuing organisation: ISO/IEC. Subject: information-security
  management, relevant to the integrity and availability of a retained closing record. Checked: ISO/IEC
  27001:2022, 3rd edition, **plus Amendment 1:2024** (register `EXT-023`, verified 2026-08-03). Nature:
  Manual §6 category 3 — international voluntary standard; certifiable. Applicability limitation:
  voluntary unless imported by law or contract; certification is a third party's opinion about a
  management system at a point in time.

**18. Jurisdictional caution.** Statutory retention periods, the evidential status of an electronic
original, execution and notarisation formalities, registration and stamping deadlines that run from
close, data-localisation rules, and personal-data retention limits are all jurisdiction-specific and can
conflict across the jurisdictions in one financing. A missed post-close registration deadline can
invalidate security. Obtain qualified local legal advice on execution formalities, post-close filings
and retention before the record is designed.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-12`; `PCI-PFL-LAW-06.05`;
`PCI-PFL-LAW-13.03`; `PCI-PFL-LAW-14.01`; `PCI-PFL-LAW-14.04`. **Increment over the foundational
parent:** `PCI-FND-LAW-12` requires records to be retained; this law names the components a *financing*
record must contain, requires them to be one identified set with an index and a version per component,
requires the closing numbers to reconcile, and requires the open items to be handed to the person who
will administer them.

**20. Related Body of Knowledge content.** PFL-AI · Domain 13 — Due diligence and financial close ·
KA 13.4 Syndication and financial close · including the funds-flow statement reconciled to the
close-cost budget. Also KA 13.3 (conditions precedent and documentation) and Domain 6 KA 6.2 (sources
and uses).

**21. Compliance test.** A reviewer takes the closing-record index and performs five steps. (a) Confirms
every component in the definition of *closing record* appears in the index with a version. (b) Retrieves
three components at random, as a person who was not present at close, and confirms each is available in
the form indexed. (c) Recomputes the integrity control on the locked *base case* and compares it to the
index value. (d) Reconciles the final sources and uses to the funds flow as executed, within the
recorded tolerance. (e) Confirms the open-items schedule lists every waived and deferred condition from
the CP register, each with an owner and a long-stop. Compliance is demonstrated when all five complete;
a component available only through a decommissioned data room fails step (b).

**22. Breach indicators.** A closing bible that is a folder of links; a base case in the record whose
file date is later than close; a funds flow that does not tie to the sources and uses; deferred
conditions that appear nowhere after close; an index with no version column; nobody able to produce the
record two years later.

**23. Consequence within PCI authority.** Correction required and any closing certification withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Evidence selection: from a list of documents, the candidate assembles
the minimum complete closing record. Scenario judgement: a transaction is announced as closed while one
facility's documents are not yet effective. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-13-03` *Financial-Close Evidence* (v1.0). Amendment note: restructured
onto the twenty-five-element form; *closing record* and *closed* defined; the open-items handover added
as PR-04; retrievability by a person who was not present at close made the operative test of retention,
replacing an unverifiable "retained" obligation.

---
## Domain 14 — Construction monitoring and drawdown

### PCI LAW PCI-PFL-LAW-14.01 — Sources-and-Uses Integrity

**1. Normative requirement.** A credential holder must present a *sources and uses* statement whose two
totals are equal, and must not close the gap with an uncommitted source, an unallocated balancing item
or a figure that no other statement supports.

**2. Purpose.** The sources-and-uses statement is the transaction's single arithmetic promise: this is
what the project costs, and this is the money that pays for it. A balancing item, an assumed
contribution or a source that is not yet committed converts that promise into an assertion, and the
project discovers the difference during construction, when the options are worst.

**3. Scope.** Every credential holder who prepares, reviews, certifies, approves or relies upon a
sources-and-uses statement — at screening, at sanction, at close, at each drawing, on each variation,
and on any restructuring or refinancing.

**4. Defined terms.** *sources and uses*, *finance documents*, *cost-to-complete*, *material*,
*evidence*, *decision owner*, *competent reviewer*, *verified*, *source line*, *authoritative version*.
**Committed source** — funding that a party is contractually obliged to provide, evidenced by an
executed document, subject only to conditions that are within the project's control or already
satisfied. **Balancing item** — any line whose value is derived from the difference between the two
totals rather than from its own evidence.

**5. Required actions.**

- **PCI-PFL-LAW-14.01-PR-01 — Every source classified.** The preparer must classify each source as
  *committed* or *uncommitted*, with the *source line* of the document that commits it, and must show
  the two classes separately.
- **PCI-PFL-LAW-14.01-PR-02 — No balancing item.** The preparer must derive every line from its own
  evidence, and must not include a *balancing item*; where the totals do not agree, the difference is
  reported as an unfunded amount or an unallocated surplus, named as such.
- **PCI-PFL-LAW-14.01-PR-03 — Uses reconciled to the cost base.** The preparer must reconcile the uses
  to the capital-cost estimate, the financing costs, the interest during construction, the fees, the
  working capital and the reserves, and must reconcile the total to the *cost-to-complete* at each
  update.
- **PCI-PFL-LAW-14.01-PR-04 — Contingency shown, not absorbed.** The preparer must show contingency and
  any management reserve as separate uses with their release authority, and must not absorb a cost
  overrun into contingency without recording the draw.

**6. Prohibited actions.** Balancing the statement with a plug; presenting an uncommitted source as
committed; netting an unfunded amount against an unallocated surplus; omitting interest during
construction, fees or working capital from uses; absorbing an overrun into contingency silently;
carrying a superseded statement into a drawdown certificate.

**7. Required evidence.** The statement with sources classified and *source lines* attached; the
executed documents committing each committed source; the reconciliation of uses to the cost base and to
the *cost-to-complete*; the contingency schedule with draws and release authority; the version history
of the statement against the model *authoritative version*.

**8. Responsible role.** The project finance leader accountable for the funding plan. The *decision
owner* for the sanction, close or drawing that relies on the statement.

**9. Approval authority.** The decision owner approves the statement for use; the lenders' agent or
technical adviser approves it where the *finance documents* make it a condition of drawing. A source
becomes committed only when the committing party executes the document — no approval can make it so.

**10. Independence requirement.** A *competent reviewer* independent of the project team must verify
the classification of sources and the reconciliation to the cost base before financial close and before
any drawing that depends on an in-balance test.

**11. Materiality or threshold.** The totals are equal or the statement is wrong; there is no tolerance
on the identity itself beyond the rounding precision recorded by the *decision owner*. Materiality
governs escalation of an unfunded amount — the figure, in the transaction's currency or as a proportion
of remaining *cost-to-complete*, is recorded in the engagement's materiality statement, and where the
*finance documents* define an in-balance test **that documented test governs and is the one applied**.
*Scale test:* on a small municipal project the statement is a dozen lines and one grant; on a
multi-billion cross-border financing it is maintained per currency and per facility, with a separate
statement for each drawdown currency, because a statement that balances in aggregate can be out of
balance in a currency the project cannot convert.

**12. Exception and waiver.** No exception is permitted to element 1 or to PR-02. A statement showing an
**unfunded amount**, named and quantified, with the party expected to fund it and the date, is
compliance and needs no exception — it is the balancing item, not the shortfall, that this law forbids.

**13. Escalation trigger.** An unfunded amount at any update; a source moving from committed to
uncommitted; a contingency draw that takes remaining contingency below the *cost-to-complete* risk
allowance; a use omitted from the statement and discovered later; a difference between the statement and
the *funds flow* at a drawing.

**14. AI application.** AI may assemble the statement from the model and the cost base, classify sources
against the executed document set for confirmation, reconcile uses to the cost-to-complete, detect a
balancing item by tracing each line to its evidence, and produce the per-currency views.

**15. AI prohibition.** AI must not classify a source as committed, decide that an unfunded amount is
acceptable, approve a statement, or create a line whose value it has derived from the difference between
the totals.

**16. AI verification.** Independent recomputation of both totals by a named human; source tracing of
every committed source to the executed document that commits it; and reconciliation of the uses to the
cost base and to the *cost-to-complete*. A line the human cannot trace to its own evidence is treated as
a *balancing item* and removed.

**17. External reference.**

- **The FIDIC suite of conditions of contract.** Issuing organisation: FIDIC. Subject: the payment and
  certification mechanics that determine when a construction cost becomes a use. Checked: characterised
  generically; **no book, clause number or edition asserted** (register `EXT-050`, verified 2026-08-03).
  Nature: Manual §6 category 4 — contract framework. Applicability limitation: binds only the parties
  who adopt it, through the contract they sign; not legislation, and commonly amended in a project
  contract.
- **ISO 15489-1 *Records management — Part 1*.** Issuing organisation: ISO. Subject: reliability and
  retrievability of the records evidencing each source and use. Checked: ISO 15489-1:2016 (register
  `EXT-025`, verified 2026-08-03). Nature: Manual §6 category 3 — international voluntary standard.
  Applicability limitation: voluntary unless imported by law or contract.

**18. Jurisdictional caution.** Whether a grant or subsidy is committed and irrevocable, the tax
treatment of grants and of capitalised interest and fees, indirect tax on construction costs and its
recoverability and timing, exchange controls affecting a source in another currency, and the insolvency
treatment of an undrawn commitment are all jurisdiction-specific and can change the statement. Obtain
qualified local legal and tax advice on each source and on the treatment of financing costs — see
`PCI-PFL-LAW-12.02`.

**19. Related PCI Laws.** `PCI-FND-LAW-06`; `PCI-FND-LAW-07`; `PCI-PFL-LAW-09.01`;
`PCI-PFL-LAW-14.02`; `PCI-PFL-LAW-14.03`; `PCI-PFL-LAW-14.04`; `PCI-PFL-LAW-13.04`. **Increment over
the foundational parent:** `PCI-FND-LAW-07` forbids a misleading presentation; this law names the
mechanism specific to a funding plan — a plug that makes an out-of-balance project look funded — and
requires each source to be classified against an executed document and each line to stand on its own
evidence.

**20. Related Body of Knowledge content.** PFL-AI · Domain 14 — Construction monitoring and drawdown ·
KA 14.1 Sources and uses, draw requests and conditions. Also Domain 6 KA 6.2 (sources and uses; the
identity by construction) and Domain 8 KA 8.1–8.3 (cost, escalation, contingency and management
reserve).

**21. Compliance test.** A reviewer takes the statement, the executed document set and the cost base,
and performs four steps. (a) Adds both columns independently and confirms equality within the recorded
rounding precision. (b) Traces each source classified *committed* to an executed document and confirms
the conditions remaining are within the project's control or satisfied. (c) Traces every use line to its
own evidence and confirms none is derived from the difference between the totals. (d) Reconciles total
uses to the current *cost-to-complete* plus costs incurred, and explains each difference. Compliance is
demonstrated when all four complete; a line that cannot be traced to its own evidence is a breach.

**22. Breach indicators.** A line called "sponsor support" with no commitment letter; a use line whose
value changes whenever another line changes; contingency falling without a recorded draw; a statement
that balances only after a rounding line; a per-currency view that has never been produced on a
multi-currency deal; a statement dated before the last variation order.

**23. Consequence within PCI authority.** Correction required and the affected statement withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Calculation review: a statement balances only through a plug, and the
candidate must locate it, name the unfunded amount and restate the statement. Evidence selection: which
document makes a source committed. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law — no v1.0 predecessor.** Amendment note: v1.0 mentioned the sources-and-uses
statement inside the close and drawdown laws but imposed no obligation on its integrity. *Committed
source* and *balancing item* defined; per-currency treatment required at scale; the unfunded-amount
route made explicit so that honesty is available without an exception.

---

### PCI LAW PCI-PFL-LAW-14.02 — Drawdown Control

**1. Normative requirement.** A credential holder must not request, certify or approve a drawing for
work not performed, for costs not incurred or not evidenced, or while the project is out of balance
without that fact being disclosed in the request.

**2. Purpose.** The drawdown is where the financing meets the site. Money drawn against work not done,
or drawn while the remaining funding no longer covers the remaining cost, removes the lenders' principal
protection at the point they are most exposed and least able to withdraw.

**3. Scope.** Every credential holder who prepares, certifies, reviews, approves or relies upon a draw
request, an application for payment, a drawdown certificate or a lender's monitoring report during the
availability period, on any facility.

**4. Defined terms.** *cost-to-complete*, *sources and uses*, *finance documents*, *conditions
precedent*, *evidence*, *material*, *decision owner*, *competent reviewer*, *verified*, *source line*.
**In balance** — the state in which committed and available funding at least equals the remaining
*cost-to-complete* plus the amounts the *finance documents* require to be held, tested on the basis those
documents state. **Certified value** — the value of work performed as certified by the person the
construction contract entitles to certify it.

**5. Required actions.**

- **PCI-PFL-LAW-14.02-PR-01 — Evidence of value.** The preparer must support each draw request with the
  *certified value* of work performed, or with evidence of cost incurred, for every amount requested.
- **PCI-PFL-LAW-14.02-PR-02 — Current cost-to-complete.** The preparer must support each draw request
  with a *cost-to-complete* current at the request date, prepared under `PCI-PFL-LAW-14.03`.
- **PCI-PFL-LAW-14.02-PR-03 — In-balance test on the documented basis.** The preparer must test whether
  the project is *in balance* on the basis the *finance documents* state, and must disclose the result
  in the request, including where it is negative.
- **PCI-PFL-LAW-14.02-PR-04 — Conditions to the drawing confirmed.** The preparer must confirm that
  every condition to that drawing is satisfied under `PCI-PFL-LAW-13.03`, and must identify any waived
  or deferred condition in the request.
- **PCI-PFL-LAW-14.02-PR-05 — Advance and retention treatment.** The preparer must show advance
  payments, retention and their recovery separately, and must not present an advance payment as value
  earned.

**6. Prohibited actions.** Requesting or certifying against uncertified work; drawing for a cost not
yet incurred where the documents require incurrence; presenting an advance payment as value earned;
suppressing a negative in-balance result; drawing while a condition is unsatisfied without disclosing
it; front-loading a drawdown to build a cash cushion outside the documented purpose.

**7. Required evidence.** The draw request with its supporting certificates and invoices; the
*cost-to-complete* current at the request date; the in-balance test with its documented basis and its
result; the conditions confirmation; the advance-payment and retention schedule; the approvals obtained.

**8. Responsible role.** The project finance leader or finance director who signs the request. The
lenders' technical adviser or agent for the lenders' confirmation, where the documents require it.

**9. Approval authority.** The party the *finance documents* name approves the drawing. The *decision
owner* approves the borrower's request. A negative in-balance result can be cured only by additional
committed funding or by a *waiver* under `PCI-PFL-LAW-15.03` — never by a re-presentation.

**10. Independence requirement.** The *certified value* must come from the person the construction
contract entitles to certify it, who is *independent* of the party requesting payment; where the
finance documents require a lenders' technical adviser, that adviser's confirmation must satisfy the
four limbs of *independent* in relation to the borrower and the contractor.

**11. Materiality or threshold.** **The in-balance test is the test in the finance documents — its
basis, its inclusions and its required headroom — and PCI states none of them.** Where the documents are
silent, the preparer applies committed and available funding against remaining *cost-to-complete* plus
required reserve balances, states that basis expressly, and labels it as the preparer's basis.
Materiality governs escalation of a shortfall, recorded by the *decision owner* in the transaction's own
metric. *Scale test:* on a small municipal project with monthly certificates from a single contract, the
request is a short pack and the in-balance test one line; on a multi-billion cross-border financing with
several facilities drawing in different currencies against multiple contracts, the in-balance test is
performed on the documented consolidated basis **and** per facility, because a facility can be out of
balance while the project is not.

**12. Exception and waiver.** No exception is permitted to element 1. A drawing while out of balance
requires a *waiver* from the party the documents entitle, obtained before the drawing, on stated terms
— and the request must disclose the position whether or not a waiver is expected. Disclosure is never
waivable.

**13. Escalation trigger.** A negative in-balance result; a certificate withheld, qualified or reduced
by the certifier; a *material* movement in *cost-to-complete* between requests; a request that would
draw the last of a contingency; an unsatisfied condition discovered after a request is submitted; a
retention recovery that does not reconcile.

**14. AI application.** AI may assemble the draw pack, reconcile requested amounts to certificates and
invoices, recompute the in-balance test, detect duplicate or previously funded invoices, track advance
recovery and retention, and flag conditions not yet confirmed.

**15. AI prohibition.** AI must not certify value, approve a drawing, decide that a project is in
balance, waive a condition, or sign a draw request.

**16. AI verification.** Reconciliation, by a named human, of every requested amount to a *certified
value* or an evidenced cost; independent recomputation of the in-balance test on the documented basis;
and source tracing of each certificate to the certifier entitled to issue it. A machine match between an
invoice and a request line is not evidence that the work was performed.

**17. External reference.**

- **The FIDIC suite of conditions of contract.** Issuing organisation: FIDIC. Subject: the interim
  payment and certification mechanism through which value becomes payable. Checked: characterised
  generically; **no book, clause number or edition asserted** (register `EXT-050`, verified 2026-08-03).
  Nature: Manual §6 category 4 — contract framework. Applicability limitation: binds only the parties
  who adopt it, through the contract they sign; project contracts commonly amend it, so the standard
  form is not a safe source for the mechanism actually in force — read the executed contract under
  `PCI-PFL-LAW-12.01`.
- **ISO 15489-1 *Records management — Part 1*.** Issuing organisation: ISO. Subject: the reliability of
  the certification and payment record. Checked: ISO 15489-1:2016 (register `EXT-025`, verified
  2026-08-03). Nature: Manual §6 category 3 — international voluntary standard. Applicability
  limitation: voluntary unless imported by law or contract.

**18. Jurisdictional caution.** Statutory payment regimes and pay-when-certified rules, construction
liens and their priority over lenders' security, retention-trust requirements, indirect tax points on
certified value and on advance payments, and the treatment of an advance-payment bond on insolvency are
all jurisdiction-specific and can override the contract's own mechanics. Obtain qualified local legal
advice on the payment regime and on lien risk before a drawdown structure is relied upon.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-07`; `PCI-PFL-LAW-13.03`;
`PCI-PFL-LAW-14.01`; `PCI-PFL-LAW-14.03`; `PCI-PFL-LAW-14.04`. **Increment over the foundational
parent:** `PCI-FND-LAW-05` requires evidence behind a claim; this law states what evidence a *drawing*
needs — certified value or evidenced cost, a current cost-to-complete, and the documented in-balance
test — and makes disclosure of a negative result unwaivable even where the drawing itself is waived.

**20. Related Body of Knowledge content.** PFL-AI · Domain 14 — Construction monitoring and drawdown ·
KA 14.1 Sources and uses, draw requests and conditions · and KA 14.3 Progress certification and change
control, including advance payments and bonds. Also Domain 8 KA 8.2 (schedule-driven cash flow).

**21. Compliance test.** A reviewer takes a draw request pack and performs five steps. (a) Traces every
requested amount to a *certified value* or an evidenced incurred cost. (b) Confirms the *cost-to-complete*
used is dated on or after the request date's cut-off and was prepared under `PCI-PFL-LAW-14.03`. (c)
Recomputes the in-balance test on the documented basis and obtains the disclosed result. (d) Confirms
each condition to the drawing is confirmed satisfied, and that any waived or deferred condition is named
in the request. (e) Confirms advance payments and retention are shown separately and that recovery
reconciles. Compliance is demonstrated when all five complete; a requested amount with no certificate or
evidence of cost is a breach.

**22. Breach indicators.** A request whose total matches the monthly budget rather than the
certificates; a cost-to-complete reused from the previous month; an in-balance test that is always
positive by the same margin; an advance payment included in progress; retention recovered twice; a
condition confirmed by the borrower where the documents require the agent.

**23. Consequence within PCI authority.** Correction required and the affected request or certificate
withheld; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Scenario judgement: a contractor's application exceeds the certified
value and the deadline is today — the candidate must state what may be requested and what must be
disclosed. Calculation review: performing the in-balance test on a documented basis. No live examination
content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-14-01` *Drawdown Integrity* (v1.0). Amendment note: restructured onto the
twenty-five-element form; *in balance* and *certified value* defined; the advance-payment and retention
rule added as PR-05; the in-balance basis tied expressly to the finance documents; disclosure of a
negative result made unwaivable.

---

### PCI LAW PCI-PFL-LAW-14.03 — Cost-to-Complete

**1. Normative requirement.** A credential holder must prepare the *cost-to-complete* from certified
progress, committed and uncommitted remaining scope, assessed claim exposure, escalation and the
remaining programme, and must not derive it by deducting costs incurred from the original budget.

**2. Purpose.** The cost-to-complete is the number that says whether the project is still funded.
Deriving it from the budget assumes the budget was right and that nothing has changed — which is the one
assumption the number exists to test. A cost-to-complete that cannot rise is not a forecast; it is the
budget wearing a different label.

**3. Scope.** Every credential holder who prepares, reviews, certifies, challenges, approves or relies
upon a cost-to-complete, a contingency-adequacy assessment or a funding-adequacy conclusion during
construction, and on any delay, variation or restructuring.

**4. Defined terms.** *cost-to-complete*, *sources and uses*, *finance documents*, *material*,
*evidence*, *decision owner*, *competent reviewer*, *verified*, *escalation threshold*. **Assessed claim
exposure** — the project's own judgement of the probable settled cost of notified but unagreed claims,
recorded with its basis. **Remaining scope** — the work not yet performed, separated into scope
committed under contract and scope not yet committed.

**5. Required actions.**

- **PCI-PFL-LAW-14.03-PR-01 — Build up from components.** The preparer must build the cost-to-complete
  from *remaining scope* separated into committed and uncommitted, *assessed claim exposure*, escalation
  on uncommitted scope, and the cost of the remaining programme, each stated separately.
- **PCI-PFL-LAW-14.03-PR-02 — Claims assessed, not ignored.** The preparer must include an *assessed
  claim exposure* for every notified but unagreed claim, with its basis, and must not record it as nil
  merely because it is disputed.
- **PCI-PFL-LAW-14.03-PR-03 — Programme linkage.** The preparer must state the programme on which the
  cost-to-complete is based, must include the time-related cost of the remaining programme, and must
  re-prepare the cost-to-complete when the programme changes *materially*.
- **PCI-PFL-LAW-14.03-PR-04 — Contingency adequacy stated.** The preparer must state the remaining
  contingency against the remaining risk, and must not present contingency as available funding for a
  known cost.
- **PCI-PFL-LAW-14.03-PR-05 — Movements explained.** The preparer must reconcile each cost-to-complete
  to the previous one and explain every movement by component.

**6. Prohibited actions.** Deriving the cost-to-complete as budget less costs incurred; recording a
notified claim at nil because it is disputed; omitting escalation on uncommitted scope; using a
superseded programme; presenting contingency as funding for a cost already known; smoothing a movement
across periods; presenting a cost-to-complete that has never risen as evidence of control.

**7. Required evidence.** The component build-up with its separate lines; the claims schedule with each
assessed exposure and its basis; the programme relied on, identified by version and date; the
contingency-adequacy statement; the period-on-period reconciliation with movements explained by
component; the certified-progress record it starts from.

**8. Responsible role.** The project finance leader or project controls lead who prepares it. The
*decision owner* for the funding conclusion drawn from it.

**9. Approval authority.** The decision owner approves the cost-to-complete for use in a drawing or a
report. Where the *finance documents* require the lenders' technical adviser to confirm it, that
confirmation is required in addition and cannot be given by the borrower.

**10. Independence requirement.** A *competent reviewer* independent of the project delivery team must
review the cost-to-complete before it supports a drawing that depends on an in-balance test, before any
contingency draw, and before any statement that the project remains funded — because the delivery team's
benefit runs to the number staying flat.

**11. Materiality or threshold.** The *escalation threshold* is a movement in the cost-to-complete, or
in remaining contingency, of a size recorded by the *decision owner* in the engagement's materiality
statement, expressed in the transaction's own metric — for example a stated proportion of remaining
contingency, or a stated movement in remaining funding headroom. **PCI sets no figure**, and where the
*finance documents* define a reporting or in-balance trigger, **that documented trigger governs**.
*Scale test:* on a small municipal project the build-up is a single table refreshed monthly against one
contract; on a multi-billion cross-border financing it is prepared per contract package and consolidated,
with claim exposure assessed per package, because packages fail at different times and a consolidated
movement can conceal two offsetting ones.

**12. Exception and waiver.** No exception is permitted to element 1. Where a component genuinely cannot
be quantified — an unassessed claim of unknown scope, for example — the preparer must state it in words,
with its potential direction and the date by which it will be assessed, and the *decision owner* must
record acceptance of an unquantified exposure. It must not be recorded as nil.

**13. Escalation trigger.** A cost-to-complete movement beyond the *escalation threshold*; remaining
contingency falling below the remaining assessed risk; a claim assessed materially above its previous
exposure; a programme change that adds time-related cost; a cost-to-complete that would take the project
out of balance at the next drawing.

**14. AI application.** AI may build the component schedule from the commitment register and the
programme, compute escalation on uncommitted scope, track claim exposures against their assessment
dates, reconcile period-on-period movements by component, and flag components that have not moved when
the programme has.

**15. AI prohibition.** AI must not assess a claim exposure, decide that remaining contingency
covers the remaining assessed risk, certify a cost-to-complete, approve a contingency draw, or conclude
that a project remains funded.

**16. AI verification.** Independent recomputation, by a named human, of the escalation and time-related
components; source tracing of each committed-scope figure to the executed commitment; expert judgement,
recorded, on every *assessed claim exposure*, which must be a named human's assessment and never a
machine's; and reconciliation of the movement explanation to the underlying changes.

**17. External reference.**

- **The FIDIC suite of conditions of contract.** Issuing organisation: FIDIC. Subject: variation, claim
  and extension-of-time mechanisms that generate the exposures a cost-to-complete must carry. Checked:
  characterised generically; **no book, clause number or edition asserted** (register `EXT-050`,
  verified 2026-08-03). Nature: Manual §6 category 4 — contract framework. Applicability limitation:
  binds only the parties who adopt it, through the contract they sign; commonly amended.
- **IAS 37 *Provisions, Contingent Liabilities and Contingent Assets*.** Issuing organisation: IFRS
  Foundation / IASB. Subject: the boundary between a provision and a contingent liability, which is
  where a claim exposure meets the reported position. Checked: in force, by name only (register
  `EXT-006`, verified 2026-08-03). Nature: Manual §6 category 2 — authoritative financial-reporting
  standard. Applicability limitation: entities applying IFRS Accounting Standards in an adopting
  jurisdiction only; **it governs reporting, not the funding assessment**, and an exposure excluded from
  a provision is still included in a cost-to-complete.

**18. Jurisdictional caution.** The validity of a claim, the effect of a contractual time bar, the
availability of global claims, statutory adjudication and its enforceability, and the treatment of
disputed amounts on insolvency are jurisdiction-specific and materially affect assessed exposure — and a
time bar that is enforceable in one jurisdiction might not be enforceable in another. Obtain qualified local legal
advice on claim validity and time bars before an exposure is assessed as low.

**19. Related PCI Laws.** `PCI-FND-LAW-07`; `PCI-FND-LAW-11`; `PCI-PFL-LAW-14.01`;
`PCI-PFL-LAW-14.02`; `PCI-PFL-LAW-11.01`; `PCI-PFL-LAW-06.03`. **Increment over the foundational
parent:** `PCI-FND-LAW-07` requires an honest forecast; this law fixes the construction-phase method —
a build-up from components rather than a deduction from budget, an assessed exposure for every notified
claim, a named programme, and a movement explanation by component — which is what makes the forecast
capable of rising.

**20. Related Body of Knowledge content.** PFL-AI · Domain 14 — Construction monitoring and drawdown ·
KA 14.2 Cost-to-complete and contingency draw · including assessed claim exposure. Also Domain 8 KA 8.3
(contingency and management reserve) and KA 8.4 (delay impact, cost-to-complete and the interface with
project controls). *Symbol note:* this volume writes `CTC` and never `EAC` — see the Definitions.

**21. Compliance test.** A reviewer takes two consecutive cost-to-complete reports and the supporting
records, and performs five steps. (a) Confirms the build-up shows committed scope, uncommitted scope,
assessed claim exposure, escalation and time-related cost as separate lines. (b) Confirms every notified
claim in the claims register appears with an assessed exposure and a basis, and that none is recorded at
nil solely because it is disputed. (c) Confirms the programme relied on is identified by version and is
the current one. (d) Reconciles the two reports and confirms every movement is explained by component.
(e) Confirms the contingency-adequacy statement compares remaining contingency to remaining assessed
risk. Compliance is demonstrated when all five complete; a cost-to-complete equal to budget less
incurred is a breach on its face.

**22. Breach indicators.** A cost-to-complete that has never moved; a claims line reading nil against an
active claims register; escalation applied only to committed scope; a programme reference with no
version; a movement explained as "reforecast"; contingency described as available funding for a known
variation.

**23. Consequence within PCI authority.** Correction required and the affected report or drawing
withheld; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Calculation review: building a cost-to-complete from components and
comparing it with the budget-less-incurred figure to expose the difference. Scenario judgement: a
notified claim is recorded at nil and the candidate must state the required treatment. No live
examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law** — v1.0 required a "current cost-to-complete" inside `PFL-LAW-14-01` but never
said how one is built, which left the commonest error uncontrolled. Amendment note: *assessed claim
exposure* and *remaining scope* defined; the component build-up, programme linkage, contingency adequacy
and movement explanation each made a process requirement.

---

### PCI LAW PCI-PFL-LAW-14.04 — Funds-Flow Approval

**1. Normative requirement.** Project funds must move only through the accounts, in the order and for
the purposes the *finance documents* specify, on instructions authorised by named signatories under
segregation of duties.

**2. Purpose.** The waterfall and the account structure are the mechanism by which every priority in
the transaction is actually enforced. A payment made outside them defeats the ranking, the reserves and
the *lock-up* simultaneously — and payment fraud in project finance is overwhelmingly a control failure
at the instruction step, not a documentation failure.

**3. Scope.** Every credential holder who prepares, authorises, reviews, approves, executes or reconciles
a payment instruction, a *funds flow*, a waterfall application or an account transfer in connection with
a project financing, at close, at each drawing, and throughout operation.

**4. Defined terms.** *funds flow*, *finance documents*, *reserve account*, *distribution*, *lock-up*,
*evidence*, *decision owner*, *competent reviewer*, *verified*, *material*. **Segregation of duties** —
the arrangement in which the person who prepares an instruction, the person who authorises it and the
person who reconciles the account afterwards are three different named individuals, none of whom can
perform another's step. **Authorised signatory** — an individual named in a mandate lodged with the
account bank, within the limits that mandate states.

**5. Required actions.**

- **PCI-PFL-LAW-14.04-PR-01 — Payments follow the documented order.** The preparer must apply each
  payment to the account, purpose and waterfall position the *finance documents* specify, and must not
  net, reorder or combine payments across tiers.
- **PCI-PFL-LAW-14.04-PR-02 — Segregation of duties.** The organisation must operate *segregation of
  duties* on every payment instruction, with the three roles held by three named individuals, and must
  record who performed each.
- **PCI-PFL-LAW-14.04-PR-03 — Instruction authorised within mandate.** The preparer must confirm that
  every instruction is authorised by an *authorised signatory* within the mandate's limits, and must not
  execute an instruction outside them.
- **PCI-PFL-LAW-14.04-PR-04 — Payee and account verification.** The preparer must verify each payee's
  account details against a source independent of the payment request itself before first payment and
  after any change of details, and must record the verification.
- **PCI-PFL-LAW-14.04-PR-05 — Reconciliation after execution.** The organisation must reconcile executed
  payments to the approved *funds flow* and to the account statements, by a person who neither prepared
  nor authorised them, and must report every difference.

**6. Prohibited actions.** Initiating, approving or concealing a payment outside the documented funds
flow; netting payments across waterfall tiers; executing an instruction outside a signatory's mandate;
changing payee details on the strength of an emailed request alone; permitting one individual to prepare
and authorise; making a *distribution* during a *lock-up*; delaying a reconciliation until after the
next payment run.

**7. Required evidence.** The approved funds flow; each payment instruction with its preparer,
authoriser and reconciler recorded; the bank mandate showing signatories and limits; the payee
verification records with their independent source; the post-execution reconciliation with differences
reported; the account statements.

**8. Responsible role.** The finance director or treasurer accountable for the accounts and the mandate.
The *decision owner* for the approval of the funds flow itself.

**9. Approval authority.** The *authorised signatories* named in the mandate, within their limits, for
each instruction. The party the finance documents name — commonly the agent or security trustee — for
applications from controlled accounts. No professional judgement can authorise a payment outside the
mandate.

**10. Independence requirement.** The reconciler under PR-05 must be *independent* of the preparer and
the authoriser in respect of that payment run. Payee verification under PR-04 must use a source
independent of the payment request, and independence here means a channel not controlled by the
requester.

**11. Materiality or threshold.** Segregation of duties applies to every payment regardless of size;
there is no *de minimis* under which one person may both prepare and authorise. Approval limits are
those the bank mandate and the *finance documents* state, and **PCI sets none**. Materiality governs
escalation and the depth of payee re-verification, recorded by the *decision owner*. *Scale test:* on a
small municipal project with three people in the finance function, segregation is achieved by naming the
third role outside the function — a board member or an external administrator — and this law expects
that arrangement to be recorded rather than the rule to be relaxed; on a multi-billion cross-border
financing with controlled accounts in several jurisdictions, the mandate, the limits and the roles are
maintained per account, and PR-01 does the heaviest work because a single waterfall in the documents can
be operated through many accounts.

**12. Exception and waiver.** **No exception to PR-02 or PR-03 is permitted**, including in an emergency:
an urgent payment is made by using an alternative named authoriser within the mandate, never by
collapsing the roles. A payment outside the documented order requires a *waiver* from the party entitled
to give one, obtained before the payment, under `PCI-PFL-LAW-15.03`.

**13. Escalation trigger.** A payment made outside the documented funds flow; an instruction presented
outside a signatory's mandate; a change of payee details; a reconciliation difference; a request to
release a payment urgently without the second authoriser; a payment that would breach a *lock-up*.

**14. AI application.** AI may generate the funds flow from the waterfall and the approved uses,
reconcile executed payments to instructions and statements, detect duplicate payments and unusual payees,
monitor the waterfall order, and flag instructions outside mandate limits.

**15. AI prohibition.** **AI must not authorise, initiate, release or approve a payment**, must not
verify payee details as the sole check, must not be an *authorised signatory*, and must not hold two of
the three segregated roles. An automated identity able to prepare, authorise and reconcile is a
prohibited configuration.

**16. AI verification.** Independent human confirmation of every AI-generated instruction against the
approved funds flow before authorisation; source tracing of each payee's account details to the
independent verification record; and human reconciliation of the AI-produced reconciliation to the
account statements. Detection is where AI is useful here; authorisation is where it is prohibited.

**17. External reference.**

- **COSO — *Internal Control — Integrated Framework*.** Issuing organisation: the Committee of
  Sponsoring Organizations of the Treadway Commission. Subject: internal control, including segregation
  of duties and control activities over payments. Checked: 2013 framework, revising the 1992 original;
  seventeen principles across five components (register `EXT-084`, verified 2026-08-03). Nature: Manual
  §6 category 5 — professional framework; **voluntary in itself**, although widely imported by
  regulators and internal-control regimes. Applicability limitation: adoption is voluntary unless a law,
  regulator or contract imports it; it creates no obligation for a project of its own force.
- **ISO/IEC 27001 *Information security, cybersecurity and privacy protection — Information security
  management systems — Requirements*.** Issuing organisation: ISO/IEC. Subject: access control,
  authorisation and the integrity of instruction channels. Checked: ISO/IEC 27001:2022, 3rd edition,
  **plus Amendment 1:2024** (register `EXT-023`, verified 2026-08-03). Nature: Manual §6 category 3 —
  international voluntary standard; certifiable. Applicability limitation: voluntary unless imported by
  law or contract; certification is an opinion about a management system at a point in time.

**18. Jurisdictional caution.** Account-control and security arrangements, the account bank's set-off
rights, exchange controls and repatriation restrictions, payment-services and authorised-push-payment
rules, sanctions screening obligations, and anti-money-laundering and counter-terrorist-financing
requirements are jurisdiction-specific and bind the paying institution, the payer and sometimes the
individual authoriser personally. **A payment that is contractually correct can still be unlawful.**
Obtain qualified local legal advice on the payment perimeter, and treat sanctions and financial-crime
screening as a separate obligation owed to the institution's own compliance function.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-03`; `PCI-FND-LAW-11`;
`PCI-PFL-LAW-10.05`; `PCI-PFL-LAW-13.04`; `PCI-PFL-LAW-14.02`; `PCI-PFL-LAW-15.01`;
`PCI-PFL-LAW-16.03`. **Increment over the foundational parent:** `PCI-FND-LAW-03` reserves decision
authority to humans; this law applies that to the one place in a financing where an automated pipeline
can move money — three named humans in three segregated roles, authorisation within a lodged mandate,
payee verification through an independent channel, and an express prohibition on an automated identity
holding more than one role.

**20. Related Body of Knowledge content.** PFL-AI · Domain 14 — Construction monitoring and drawdown ·
KA 14.1 Sources and uses, draw requests and conditions. Also Domain 13 KA 13.4 (the funds-flow statement
reconciled to the close-cost budget), Domain 15 KA 15.2 (the cash waterfall in operation) and Domain 16
KA 16.4 (privacy, cybersecurity, human approval and AI governance; authority accumulation).

**21. Compliance test.** A reviewer takes a payment run and performs five steps. (a) Traces each payment
to a line in the approved *funds flow* and confirms the account, purpose and waterfall position match
the finance documents. (b) Confirms three different named individuals performed the prepare, authorise
and reconcile roles. (c) Confirms each authoriser is on the bank mandate and within their limit. (d) For
each new or changed payee, locates the verification record and confirms the source was independent of
the request. (e) Confirms the post-execution reconciliation was performed by someone who neither prepared
nor authorised, and that every difference was reported. Compliance is demonstrated when all five
complete; two roles held by one individual is a breach regardless of the payment's size or correctness.

**22. Breach indicators.** A payment run authorised by the person who prepared it; a payee change
actioned from an email; a payment applied to a lower waterfall tier ahead of a higher one; a
reconciliation performed by the payments team; an automated integration with both instruction and
approval rights; a distribution paid while a lock-up test was failing.

**23. Consequence within PCI authority.** Correction required and the affected process suspended pending
correction; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence — in particular, PCI has
no authority over any criminal or regulatory consequence of a payment, which belongs to the relevant
authorities.

**24. Examination application.** Scenario judgement: an urgent payment must be released and the second
authoriser is unavailable — the candidate must state the compliant route. AI-verification case: an
automation is proposed that would prepare, approve and reconcile payments, and the candidate must
identify the prohibited configuration. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-14-02` *Funds-Flow Control* (v1.0). Amendment note: restructured onto the
twenty-five-element form; *segregation of duties* and *authorised signatory* defined; payee verification
through an independent channel added as PR-04; post-execution reconciliation by a third person added as
PR-05; the emergency route closed expressly, since collapsing roles under time pressure is the observed
failure mode.

---
## Domain 15 — Operations, performance and restructuring

### PCI LAW PCI-PFL-LAW-15.01 — Distribution Testing

**1. Normative requirement.** A credential holder must not permit, certify or make a *distribution*
unless every distribution condition in the *finance documents* is satisfied at the test date on the
documented basis.

**2. Purpose.** A *distribution* is irreversible in practice. Cash that leaves the structure ahead of a
prior claim, or during a *lock-up*, cannot usually be recovered, and every protection the lenders
negotiated depends on this one test being applied honestly at one moment.

**3. Scope.** Every credential holder who computes, certifies, approves, instructs or reports on a
distribution, a shareholder-loan payment, a subordinated-debt payment, a management or development fee,
or any other restricted payment, whether in construction or in operation.

**4. Defined terms.** *distribution*, *lock-up*, *finance documents*, *coverage ratio*, *reserve
account*, *conditions precedent*, *evidence*, *material*, *decision owner*, *competent reviewer*,
*verified*. **Distribution condition** — each condition the finance documents impose before a
distribution may be made, typically comprising a backward-looking and a forward-looking *coverage
ratio*, fully funded *reserve accounts*, no default or event of default continuing, the passing of a
stated date or milestone, and any additional condition the documents state. **Test date** — the date
the finance documents specify for testing, which is not necessarily the payment date.

**5. Required actions.**

- **PCI-PFL-LAW-15.01-PR-01 — Test every condition, at the test date.** The preparer must test every
  *distribution condition* at the documented *test date* on the documented basis, and must not test a
  subset or test at a more convenient date.
- **PCI-PFL-LAW-15.01-PR-02 — Waterfall position confirmed.** The preparer must confirm that every
  prior claim in the waterfall for that period has been paid or provided for before any cash is released
  to shareholders.
- **PCI-PFL-LAW-15.01-PR-03 — Forward-looking test on the documented projection.** Where a
  forward-looking condition applies, the preparer must use the projection the documents specify, must
  state whose projection it is and its approval status, and must not substitute a more favourable case.
- **PCI-PFL-LAW-15.01-PR-04 — Lock-up applied and reported.** Where a condition fails, the preparer must
  apply the *lock-up* the documents provide, must place the trapped cash where the documents require,
  and must report the lock-up as a lock-up.
- **PCI-PFL-LAW-15.01-PR-05 — Certificate evidenced.** The preparer must retain, with the distribution
  certificate, the calculation for each condition, the *reserve account* confirmations and the
  no-default confirmation.

**6. Prohibited actions.** Releasing cash to shareholders ahead of a prior claim or during a lock-up;
testing at a date other than the documented test date; using a more favourable projection for a
forward-looking test; releasing a *reserve account* in order to pass a distribution test; treating a
failed condition as cured by a payment made after the test date; characterising a distribution as an
operating payment to avoid the test.

**7. Required evidence.** The distribution certificate with the calculation for each condition; the
reserve confirmations at the test date, from an *independent* source; the no-default confirmation; the
waterfall application for the period; the projection used for any forward-looking test with its approval
status; the lock-up record where applicable.

**8. Responsible role.** The finance director or project finance leader who signs the distribution
certificate. The board of the distributing entity for the distribution decision itself.

**9. Approval authority.** The signatory the *finance documents* name for the certificate; the agent or
security trustee where the documents require confirmation; the board for the corporate act. **A
distribution that fails a condition can be permitted only by a *waiver* from the party entitled to give
one**, under `PCI-PFL-LAW-15.03`.

**10. Independence requirement.** *Reserve account* balances and the no-default confirmation must be
evidenced from sources *independent* of the distributing entity. A *competent reviewer* independent of
preparation must recompute the coverage conditions before any certificate on which a distribution
depends.

**11. Materiality or threshold.** **Every level, every test date and every condition is the one in the
finance documents. PCI sets no distribution level, no lock-up level and no headroom requirement**, and a
level from another transaction or from market convention must not be used. Materiality governs
escalation timing only — the headroom, in ratio units or cash, at which an approaching failure must be
escalated before the test date, set by the adopting organisation's governance. *Scale test:* on a small
municipal project a single annual test with one coverage condition and one reserve is a one-page
certificate; on a multi-billion cross-border financing with quarterly tests, several facilities each
with its own conditions, and a distribution-block account, each facility's conditions are tested and
certified separately, because passing on one facility does not permit a payment blocked by another.

**12. Exception and waiver.** No exception is permitted. The only route to a distribution that fails a
condition is a *waiver* in the form the documents require, from the party entitled to give it, obtained
**before** the distribution. A waiver obtained afterwards does not make the earlier payment compliant; it
addresses the consequence, and the breach is still recorded.

**13. Escalation trigger.** A condition failing or approaching failure at a test date; a proposal to
release a reserve to enable a distribution; a forward-looking test that fails on the documented
projection; a payment to a shareholder characterised as something else; a default that may be continuing
at a test date; a distribution instructed before the certificate is signed.

**14. AI application.** AI may compute each condition across the test dates, project the forward-looking
test, reconcile reserve balances to required levels, monitor the waterfall application, alert on
approaching failures, and draft the certificate for review.

**15. AI prohibition.** AI must not certify that a distribution condition is satisfied, decide that a
default is not continuing, approve or instruct a distribution, or release a *lock-up*.

**16. AI verification.** Independent recomputation, by a named human, of every condition at the test
date from source lines; source tracing of each reserve balance to an independent confirmation;
clause-to-output comparison of the conditions tested against the clause listing them, to confirm none is
omitted; and the signatory's own confirmation of no default. Recorded on the certificate.

**17. External reference.**

- **IAS 1 *Presentation of Financial Statements*.** Issuing organisation: IFRS Foundation / IASB.
  Subject: presentation of financial statements, including equity and the classification of liabilities,
  which is where a distribution meets the reported position. Checked: in force for periods beginning
  before 1 January 2027; **IFRS 18 replaces IAS 1 for annual reporting periods beginning on or after 1
  January 2027, earlier application permitted** (register `EXT-004` / `EXT-003`, verified 2026-08-03).
  Nature: Manual §6 category 2 — authoritative financial-reporting standard. Applicability limitation:
  entities applying IFRS Accounting Standards in an adopting jurisdiction only; **it does not determine
  whether a distribution is permitted**, which is a matter for the finance documents and for company
  law. Confirm which instrument applies to the period.
- **G20/OECD *Principles of Corporate Governance*.** Issuing organisation: OECD, with the G20. Subject:
  the governance of decisions affecting shareholders and creditors. Checked: 2023 revision,
  OECD/LEGAL/0413 (register `EXT-128`, verified 2026-08-03). Nature: Manual §6 category 5 — professional
  framework; an **OECD Council Recommendation — non-binding, not legislation**. Applicability
  limitation: creates no obligation for a credential holder.

**18. Jurisdictional caution.** **Whether a distribution is lawful is a question of company law, not of
the finance documents.** Distributable-reserve tests, solvency and net-asset tests, directors' duties on
the eve of insolvency, clawback and transactions-at-undervalue rules, withholding tax on dividends and
on shareholder-loan interest, thin-capitalisation limits and exchange controls are all
jurisdiction-specific, and a distribution permitted by the finance documents may still be unlawful or
recoverable. Personal liability for directors is possible. Obtain qualified local legal, accounting and
tax advice before every distribution — see `PCI-PFL-LAW-12.02`.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-11`; `PCI-PFL-LAW-01.01`;
`PCI-PFL-LAW-10.03`; `PCI-PFL-LAW-10.04`; `PCI-PFL-LAW-10.05`; `PCI-PFL-LAW-14.04`;
`PCI-PFL-LAW-15.03`. **Increment over the foundational parent:** `PCI-FND-LAW-11` requires escalation of
a defined exception; this law defines the exception precisely for the one irreversible act in a
financing, requires every condition to be tested at the documented date on the documented basis, forbids
releasing a reserve to pass the test, and separates the contractual question from the company-law
question that no finance document answers.

**20. Related Body of Knowledge content.** PFL-AI · Domain 15 — Operations, performance and
restructuring · KA 15.2 The cash waterfall in operation, reserves and distributions · including the
distributable amount, the distribution-block account and the distribution drought. Also Domain 10 KA
10.4 (distribution lock-up).

**21. Compliance test.** A reviewer takes the distribution certificate, the finance documents and the
accounts, and performs five steps. (a) Lists every *distribution condition* in the clause and confirms
each appears in the certificate. (b) Recomputes each coverage condition at the documented *test date*
from source lines. (c) Confirms reserve balances at the test date from independent confirmations. (d)
Confirms the waterfall for the period paid or provided for every prior claim before the release. (e)
Confirms the forward-looking test, where applicable, used the documented projection at its stated
approval status. Compliance is demonstrated when all five complete; a condition omitted from the
certificate is a breach even if it would have passed.

**22. Breach indicators.** A certificate testing coverage and nothing else; a reserve released days
before a distribution; a distribution paid before the certificate date; a forward-looking test using an
unapproved case; a management fee paid during a lock-up; a distribution certificate signed by the person
who prepared the calculation with no independent recomputation.

**23. Consequence within PCI authority.** Correction required and any certification withheld; additional
independent review; escalation; failure of the associated examination competency; ethics review;
certification investigation; suspension or withdrawal of the credential. Each subject to due process and
a right of appeal (Charter §9). PCI claims no other consequence, and has no authority over the corporate
or legal consequences of an unlawful distribution.

**24. Examination application.** Escalation decision: a forward-looking condition fails while the
backward-looking one passes, and the candidate must state what may be certified and what must be
escalated. Ethical dilemma: a sponsor proposes releasing a reserve to pass the test. No live examination
content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-15-01` *Distribution Restriction* (v1.0). Amendment note: restructured
onto the twenty-five-element form; *distribution condition* and *test date* defined; the waterfall-
position confirmation and the lock-up-reporting rule made process requirements; element 18 strengthened
to separate the contractual question from the company-law question, which v1.0 addressed only briefly.

---

### PCI LAW PCI-PFL-LAW-15.02 — Refinancing Assessment

**1. Normative requirement.** A credential holder must assess a proposed refinancing on the present
value of the change in terms net of every cost of achieving it, and must not present a headline margin
or tenor improvement as the gain.

**2. Purpose.** A refinancing replaces a facility priced for a risk that has since retired, and the
value comes from margin, tenor and covenant reset together. Break costs, hedge unwinds, fees, tax
consequences, new-lender conditions and the loss of an existing package routinely consume most of the
headline saving — and the transaction is usually recommended by someone whose fee depends on it
happening.

**3. Scope.** Every credential holder who originates, models, reviews, recommends, approves or provides
assurance on a refinancing, a repricing, an amendment-and-extension or a capital-markets take-out.

**4. Defined terms.** *finance documents*, *coverage ratio*, *distribution*, *material*, *evidence*,
*decision owner*, *competent reviewer*, *independent*, *verified*, *base case*. **Refinancing gain** —
the present value of the improvement in terms over the remaining life, net of every cost of achieving
it, computed on one stated discount basis. **All-in cost of achievement** — break costs, hedge
termination or novation costs, prepayment premia, arrangement and agency fees, adviser and rating costs,
security re-taking and registration costs, tax consequences, and the value of any protection given up.

**5. Required actions.**

- **PCI-PFL-LAW-15.02-PR-01 — Net present-value assessment.** The preparer must compute the *refinancing
  gain* on one stated discount basis, showing the improvement and the *all-in cost of achievement*
  separately.
- **PCI-PFL-LAW-15.02-PR-02 — Decompose the gain.** The preparer must decompose the gain into its
  margin, tenor, structural and covenant components, so that the source of the value is visible.
- **PCI-PFL-LAW-15.02-PR-03 — Protections given up stated.** The preparer must state every protection,
  flexibility or entitlement surrendered in the new terms — covenant headroom, cure rights, reserve
  releases, *distribution* conditions, security, hedging arrangements — and must value it where it can
  be valued.
- **PCI-PFL-LAW-15.02-PR-04 — Execution risk and the do-nothing case.** The preparer must present the
  case in which the refinancing does not complete, including the position at the existing maturity, and
  must state who bears the cost of a failed process.
- **PCI-PFL-LAW-15.02-PR-05 — Adviser interest disclosed.** The preparer must disclose every fee or
  benefit accruing to any adviser, arranger or connected person that depends on the refinancing
  proceeding, under `PCI-PFL-LAW-01.02` and `PCI-PFL-LAW-13.02`.

**6. Prohibited actions.** Presenting a margin reduction as the gain; omitting break, hedge-unwind or
prepayment costs; discounting on a basis chosen to flatter; presenting a tenor extension without its
effect on total interest paid; ignoring the protections surrendered; omitting the do-nothing case;
recommending a refinancing while an undisclosed fee depends on it.

**7. Required evidence.** The gain computation with its discount basis and both components; the
decomposition by source; the schedule of protections surrendered with valuations or a statement that
they cannot be valued; the failed-process case; the fee and benefit disclosures; the *decision owner's*
recorded approval.

**8. Responsible role.** The project finance leader accountable for the recommendation. The *decision
owner* for the decision to proceed.

**9. Approval authority.** The *decision owner* and, on the lender side, the credit approver. Existing
lenders' consents and any prepayment mechanics are governed by the existing *finance documents*.

**10. Independence requirement.** A *competent reviewer* independent of the arranging benefit and of any
fee contingent on completion must review the gain computation and the surrendered-protection schedule
before the recommendation is put to a decision owner. Where the preparer holds a contingent benefit,
element 10 of `PCI-PFL-LAW-13.02` governs how their work may be described.

**11. Materiality or threshold.** The discount basis, the horizon and the treatment of tax are stated
before the computation and are not changed afterwards; **PCI prescribes no discount rate, no minimum
gain and no payback period.** Where the *finance documents* or the organisation's governance state a
hurdle for a refinancing decision, **that documented hurdle is the one applied and tested**. Materiality
governs which surrendered protections must be valued rather than described, recorded by the *decision
owner*. *Scale test:* on a small municipal project the computation is a single discounted stream and the
cost of achievement is dominated by break costs; on a multi-billion cross-border financing with hedging,
several facilities and cross-border security, PR-03 is the largest item and the honest answer is often
that a surrendered protection cannot be valued — which must be stated rather than omitted.

**12. Exception and waiver.** No exception is permitted to element 1. A refinancing may be recommended
on grounds other than present value — releasing a constraint, changing a lender group, extending beyond
a concession event — provided the *refinancing gain* is computed and presented alongside the other
grounds, and the *decision owner* records the basis of the decision. Suppressing the computation because
the decision rests elsewhere is a breach.

**13. Escalation trigger.** A gain that turns negative on the stated basis; a break or hedge-unwind cost
materially above the estimate; a new covenant package materially tighter than the existing one; a
failed process with committed costs; discovery of an undisclosed contingent fee; a required consent that
will not be given.

**14. AI application.** AI may compute the gain across scenarios and discount bases, decompose it by
source, model break and unwind costs, compare covenant packages clause by clause for human confirmation,
and produce the failed-process case.

**15. AI prohibition.** AI must not recommend a refinancing, decide that a surrendered protection is
immaterial, conclude that the market will accept a structure, or approve a transaction.

**16. AI verification.** Independent recomputation by a named human of the gain on the stated basis;
sensitivity analysis over the discount basis and the break-cost assumption to confirm the conclusion's
robustness; and clause-to-output comparison of the AI covenant comparison against both document sets
under `PCI-PFL-LAW-12.01`.

**17. External reference.**

- **IAS 36 *Impairment of Assets*.** Issuing organisation: IFRS Foundation / IASB. Subject: the
  impairment-indicator discipline, relevant where a refinancing signals a change in the asset's expected
  cash flows. Checked: current, by name only (register `EXT-122`, verified 2026-08-03). Nature: Manual
  §6 category 2 — authoritative financial-reporting standard. Applicability limitation: entities
  applying IFRS Accounting Standards in an adopting jurisdiction only; it governs reporting, not the
  refinancing decision.
- **The Basel Framework.** Issuing organisation: the Basel Committee on Banking Supervision. Subject:
  the supervisory context that shapes lender appetite, pricing and tenor. Checked: consolidated
  framework; no standard or date asserted (register `EXT-110`, verified 2026-08-03). Nature: Manual §6
  category 10 — illustrative practice; **internationally agreed supervisory standards with no legal
  force of their own**, reaching a bank only as a national authority transposes them. Applicability
  limitation: named for context; **no requirement in this law is sourced to it.**

**18. Jurisdictional caution.** Prepayment penalties and their enforceability, withholding tax on the
new facility, the tax treatment of break costs and of hedge terminations, stamp and registration duty on
re-taken security, the priority consequences of releasing and re-granting security, and any
change-of-control or refinancing-gain-sharing obligation to a public grantor are all
jurisdiction-specific and can reverse a refinancing case. Obtain qualified local legal and tax advice
before a refinancing is recommended — see `PCI-PFL-LAW-12.02`.

**19. Related PCI Laws.** `PCI-FND-LAW-07`; `PCI-FND-LAW-10`; `PCI-PFL-LAW-09.01`;
`PCI-PFL-LAW-13.02`; `PCI-PFL-LAW-15.03`; `PCI-PFL-LAW-10.04`. **Increment over the foundational
parent:** `PCI-FND-LAW-07` requires honest reporting and `PCI-FND-LAW-10` requires conflicts to be
disclosed; this law joins them at the point where they interact worst — a transaction recommended by
someone paid for it happening — and requires a net present-value computation, a decomposition, a
surrendered-protection schedule and a do-nothing case before the recommendation is made.

**20. Related Body of Knowledge content.** PFL-AI · Domain 15 — Operations, performance and
restructuring · KA 15.3 Refinancing, waivers and amendments. Also Domain 9 KA 9.4 (refinancing gain,
decomposed into rate and extension components) and Domain 10 KA 10.4 (covenants and cure).

**21. Compliance test.** A reviewer takes the refinancing paper and the two document sets, and performs
five steps. (a) Confirms the *refinancing gain* is computed on one stated discount basis with the
improvement and the *all-in cost of achievement* shown separately. (b) Recomputes the gain and obtains
the stated figure without unexplained difference. (c) Confirms the decomposition accounts for the whole
gain, with no unexplained residual. (d) Compares the covenant and security packages and confirms every
surrendered protection appears in the PR-03 schedule. (e) Confirms the failed-process case and the fee
disclosures are present. Compliance is demonstrated when all five complete; a surrendered protection
absent from the schedule is a breach.

**22. Breach indicators.** A paper headed with a margin saving; break costs in a footnote; a discount
basis that differs from the one used elsewhere in the transaction; a covenant comparison with no
security comparison; no do-nothing case; an arranger fee disclosed only after approval; a gain that
depends entirely on a tenor extension whose total interest cost is not shown.

**23. Consequence within PCI authority.** Correction required and the recommendation withheld; additional
independent review; escalation; failure of the associated examination competency; ethics review;
certification investigation; suspension or withdrawal of the credential. Each subject to due process and
a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Calculation review: computing a refinancing gain net of break and
hedge-unwind costs, where the headline saving reverses. Ethical dilemma: an adviser whose fee depends on
completion is asked whether the refinancing is worth doing. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law — no v1.0 predecessor.** Amendment note: v1.0 addressed refinancing only as a
prohibited dependency inside the capital-structure law. *Refinancing gain* and *all-in cost of
achievement* defined; the surrendered-protection schedule, the do-nothing case and the adviser-interest
disclosure each made a process requirement.

---

### PCI LAW PCI-PFL-LAW-15.03 — Waivers and Amendments

**1. Normative requirement.** A credential holder must treat a *waiver* or an *amendment* as effective
only when it has been granted by the party entitled to grant it, in the form the *finance documents*
require, and must record its full effect before relying on it.

**2. Purpose.** Waivers and amendments are granted quickly, under pressure, often by email in principle
and by deed later. A waiver assumed, a condition attached and forgotten, or an amendment whose knock-on
effect on other definitions is never traced, leaves a transaction being administered against terms that
no longer exist — usually discovered at the next test date.

**3. Scope.** Every credential holder who requests, negotiates, records, models, monitors, certifies or
relies upon a waiver, consent, amendment, restatement, standstill or forbearance under a project
financing.

**4. Defined terms.** *waiver*, *amendment*, *finance documents*, *covenant register*, *lock-up*,
*evidence*, *material*, *decision owner*, *competent reviewer*, *verified*, *escalation threshold*.
**Entitled party** — the person or group whose consent the finance documents require for the waiver or
amendment in question, at the majority or unanimity those documents specify. **Knock-on effect** — the
consequence of a change to one defined term or clause on every other term, ratio, test, reserve or
certificate that uses it.

**5. Required actions.**

- **PCI-PFL-LAW-15.03-PR-01 — Entitlement confirmed.** The credential holder must confirm that the
  consent obtained is from the *entitled party* at the required threshold, and must not treat a lender's
  individual indication as the group's consent.
- **PCI-PFL-LAW-15.03-PR-02 — Form confirmed.** The credential holder must confirm the waiver or
  amendment is in the form the documents require, executed as required, and must not rely on an
  agreement in principle.
- **PCI-PFL-LAW-15.03-PR-03 — Conditions and duration recorded.** The credential holder must record
  every condition attached to the waiver, its duration or long-stop, any fee, and what happens on
  expiry, and must track it to closure.
- **PCI-PFL-LAW-15.03-PR-04 — Knock-on effects traced.** The credential holder must trace the *knock-on
  effect* of every amendment through the *covenant register*, the model, the reserve schedules and the
  certificates, and must re-confirm each before the next test date.
- **PCI-PFL-LAW-15.03-PR-05 — Waived is not satisfied.** The credential holder must record a waived
  condition or covenant as waived, never as satisfied, and must report the underlying position alongside
  the waiver.

**6. Prohibited actions.** Relying on an agreement in principle; treating one lender's consent as the
group's; recording a waived covenant as satisfied; omitting a waiver's attached conditions from the
register; allowing a waiver to expire untracked; implementing an amendment in the model before it is
executed; presenting a waived breach as though it had not occurred.

**7. Required evidence.** The executed waiver or amendment instrument; the confirmation of entitlement
and threshold; the register entry recording conditions, duration, fee and expiry consequence; the
knock-on-effect trace with the items changed; the closure record; the reporting that carried the
underlying position alongside the waiver.

**8. Responsible role.** The finance director or project finance leader who requests and records it. The
agent, for the lenders' side, in accordance with the documents.

**9. Approval authority.** Only the *entitled party*, at the required threshold, in the required form.
**No credential holder, and no PCI law, can grant, imply or assume a waiver or an amendment.**

**10. Independence requirement.** A *competent reviewer* independent of the transaction team must review
the knock-on-effect trace of any amendment affecting a covenant definition, a *CFADS* component, a
reserve level or a *distribution* condition, before the next test date.

**11. Materiality or threshold.** **The consent threshold is the one the finance documents state** —
majority, super-majority, affected class or all lenders — and PCI states none. Materiality governs the
depth of the knock-on trace: every amendment is traced, and the *decision owner* records the movement, in
the transaction's own metric, at which the trace must be independently reviewed. *Scale test:* on a small
municipal project a waiver is one letter from one lender and the trace is short; on a multi-billion
cross-border financing with intercreditor arrangements, several classes and different thresholds per
matter, PR-01 is the hardest requirement, because an amendment consented at the wrong threshold is
ineffective however many signatures it carries.

**12. Exception and waiver.** No exception is permitted. This law governs the transaction's own exception
mechanism; it does not create a further one. A waiver obtained after the act it waives does not make the
earlier act compliant — the breach is recorded, and the waiver addresses its consequence.

**13. Escalation trigger.** A waiver relied on before execution; a consent obtained at the wrong
threshold; a waiver condition not met within its period; a waiver approaching expiry with the underlying
position unresolved; an amendment whose knock-on effects have not been traced before a test date; a
pattern of repeated waivers of the same covenant.

**14. AI application.** AI may compare document versions to identify every change, trace a changed
defined term through the document set and the model, maintain the waiver register with its dates and
conditions, alert on approaching expiries, and draft the knock-on-effect trace for confirmation.

**15. AI prohibition.** AI must not grant, assume, interpret or confirm a waiver or amendment; must not
decide that a consent threshold is met; must not determine that a knock-on effect is immaterial; and must
not update the *covenant register* without a named human's confirmation.

**16. AI verification.** Clause-to-output comparison, by a named human, of the AI-produced change list
against the executed instrument; independent recomputation of every ratio, reserve level and certificate
affected by the change; and confirmation against the documents that the consenting parties constitute
the *entitled party* at the required threshold. The threshold confirmation is a human reading of the
clause, never a machine count.

**17. External reference.**

- **ISO 15489-1 *Records management — Part 1*.** Issuing organisation: ISO. Subject: the authenticity
  and integrity of the amended document record. Checked: ISO 15489-1:2016 (register `EXT-025`, verified
  2026-08-03). Nature: Manual §6 category 3 — international voluntary standard. Applicability limitation:
  voluntary unless imported by law or contract.
- **IAS 1 *Presentation of Financial Statements*.** Issuing organisation: IFRS Foundation / IASB.
  Subject: the classification of liabilities as current or non-current, which a waiver's timing and
  duration can change. Checked: in force for periods beginning before 1 January 2027; **IFRS 18 replaces
  IAS 1 for annual reporting periods beginning on or after 1 January 2027** (register `EXT-004` /
  `EXT-003`, verified 2026-08-03). Nature: Manual §6 category 2 — authoritative financial-reporting
  standard. Applicability limitation: entities applying IFRS Accounting Standards in an adopting
  jurisdiction only; **the reporting consequence of a waiver is not the same question as its contractual
  effect.** Confirm which instrument applies to the period.

**18. Jurisdictional caution.** Whether a waiver must be in writing or by deed, whether consideration is
required, the effect of a course of dealing or of estoppel on a repeatedly waived covenant, the validity
of an agent's authority to bind a lender group, and the treatment of a standstill or forbearance on
insolvency are all questions of the governing law of the *finance documents*. **A pattern of informal
waivers can change the parties' rights in ways the documents do not describe.** Obtain qualified legal
advice on form, authority and the effect of past conduct.

**19. Related PCI Laws.** `PCI-FND-LAW-05`; `PCI-FND-LAW-11`; `PCI-PFL-LAW-10.04`;
`PCI-PFL-LAW-13.03`; `PCI-PFL-LAW-15.01`; `PCI-PFL-LAW-15.02`; `PCI-PFL-LAW-12.02`. **Increment over
the foundational parent:** `PCI-FND-LAW-05` requires the record to show what happened; this law states
what a *financing's* waiver record must show — entitlement at the right threshold, execution in the
required form, conditions and expiry tracked to closure, knock-on effects traced through every dependent
term, and the underlying position reported alongside the waiver rather than replaced by it.

**20. Related Body of Knowledge content.** PFL-AI · Domain 15 — Operations, performance and
restructuring · KA 15.3 Refinancing, waivers and amendments. Also Domain 10 KA 10.4 (waiver and
amendment; consented departure from terms) and Domain 13 KA 13.3 (conditions precedent).

**21. Compliance test.** A reviewer takes each waiver and amendment in the period and performs five
steps. (a) Confirms an executed instrument exists in the required form. (b) Confirms the consenting
parties constitute the *entitled party* at the threshold the documents require for that matter, by
reading the clause. (c) Confirms every attached condition, the duration and any fee are in the register,
with a closure record or an open tracking entry. (d) Traces one changed defined term through the covenant
register, the model and the certificates, and confirms each dependent item was updated. (e) Confirms the
period's reporting records the item as waived, not satisfied, with the underlying position shown.
Compliance is demonstrated when all five complete; reliance on an unexecuted agreement in principle is a
breach.

**22. Breach indicators.** A model updated on the strength of an email; a covenant recorded as compliant
with a waiver reference in a footnote; a waiver register with no expiry column; an amendment whose
knock-on effect on a reserve level was never traced; the same covenant waived four periods running with
no escalation; a consent from the agent where the documents require all lenders.

**23. Consequence within PCI authority.** Correction required and the affected reporting withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** Escalation decision: a waiver expires before the underlying position is
cured, and the candidate must state what is reported and to whom. Evidence selection: distinguishing an
agreement in principle from an effective waiver. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law — no v1.0 predecessor.** Amendment note: v1.0 required waivers to be logged inside
the covenant law but imposed no obligation on entitlement, form, conditions, expiry or knock-on effects.
*Entitled party* and *knock-on effect* defined; the waived-is-not-satisfied rule restated here as a
process requirement so that it can be breached and assessed independently.

---
## Domain 16 — Data, automation and responsible AI in finance

### PCI LAW PCI-PFL-LAW-16.01 — AI-Assisted Financial Modelling

**1. Normative requirement.** Where an AI system builds, edits, checks, extracts for or interprets any
part of a *financial model* or its outputs, the professional relying on that work must *verify* it
before it is used.

**2. Purpose.** Machine assistance in modelling is fast, fluent and confidently wrong in ways that look
like competence — a rebuilt schedule that reconciles, an extracted term that omits a proviso, a check
that passes because it was written from the same misunderstanding as the calculation. Machine checking
is additive to professional review and never substitutive, and the moment it becomes substitutive the
whole control system in Domain 6 stops working.

**3. Scope.** Every credential holder who uses, relies upon, commissions or approves *AI-assisted work*
touching a financial model or its outputs — model construction, formula generation, schedule building,
data extraction, reconciliation, check design, scenario generation, anomaly detection, commentary and
summarisation — at any stage of a transaction.

**4. Defined terms.** *AI-assisted work*, *financial model*, *model owner*, *authoritative version*,
*verified*, *material*, *evidence*, *decision owner*, *competent reviewer*, *source line*,
*decision-grade*. **AI contribution record** — the record of what the AI system was asked to do, what it
produced, which human verified it, by what method, and when. **Authorised tool** — an AI system that the
engaging organisation has approved for the classification of information involved, recorded in its
governance.

**5. Required actions.**

- **PCI-PFL-LAW-16.01-PR-01 — Record the contribution.** The relying professional must maintain an *AI
  contribution record* for every *material* AI contribution to a decision-grade model or output.
- **PCI-PFL-LAW-16.01-PR-02 — Verify by a named method.** The relying professional must verify every
  material AI contribution by recomputation, by regression against independently *verified* figures, or
  by tracing to source, and must record which method was used.
- **PCI-PFL-LAW-16.01-PR-03 — Machine checks do not replace human review.** The relying professional
  must treat an AI-run check, reconciliation or anomaly scan as additional to the review required by
  `PCI-PFL-LAW-06.01` and `PCI-PFL-LAW-13.01`, and must not reduce human review because a machine check
  passed.
- **PCI-PFL-LAW-16.01-PR-04 — Authorised tools and information classification.** The credential holder
  must place transaction, counterparty and personal information only into an *authorised tool* for that
  classification of information, and must not place such information into an AI system, external service
  or storage location that has not been authorised for it.
- **PCI-PFL-LAW-16.01-PR-05 — Seeded-error testing of AI checks.** Where an AI system designs or
  operates a check, the *model owner* must confirm the check detects a seeded known error before relying
  on it, and must record the test.

**6. Prohibited actions.** Relying on an AI contribution that has not been verified by a named method;
recording an AI system as the author of a model, a change or a check; reducing human review because a
machine check passed; placing confidential transaction or personal information into an unauthorised
tool; presenting AI-generated commentary as analysis performed by a named professional; using an
AI-produced figure that carries no *source line*.

**7. Required evidence.** The AI contribution records; the verification records with methods and dates;
the seeded-error test results for AI-operated checks; the record of tool authorisation for each
classification of information; the review records showing human review was not reduced; the change-log
entries under `PCI-PFL-LAW-06.05-PR-02` naming human authors.

**8. Responsible role.** The professional relying on the AI contribution, personally, for its
verification. The *model owner* for the model as a whole. The engaging organisation's named officer for
tool authorisation.

**9. Approval authority.** The *decision owner* approves the output. **No AI system holds approval
authority of any kind under this law**, and none can be delegated to one.

**10. Independence requirement.** Verification under PR-02 may be performed by the relying professional;
it does not require independence, because it is a check of substance rather than an assurance opinion.
Independence is required where the verification is relied upon by another party, in which case
`PCI-PFL-LAW-13.01` governs. **An AI system is never *independent*: independence is a property of a
relationship, and a tool has none.**

**11. Materiality or threshold.** An AI contribution is *material* where the figures, terms or
conclusions it affects are material on the recorded test in the transaction's own metric; the *decision
owner* records that figure in the engagement's materiality statement. **PCI sets no figure**, and PCI
sets no minimum proportion of AI output to be sampled — the *decision owner* records the sampling basis
for non-material contributions. *Scale test:* on a small municipal project with one modeller using a
single assistant, the contribution record can be a column in the change log and the burden is small; on
a multi-billion cross-border financing with several teams and several tools, the record is maintained per
workstream, and PR-04 becomes the binding constraint because the temptation to paste a document set into
an unapproved service rises with the volume of documents.

**12. Exception and waiver.** No exception is permitted to element 1 or to PR-04. The *decision owner*
may approve in writing a reduced sampling basis for non-material contributions, for a named engagement,
for a stated period, where a *competent reviewer* confirms the basis is defensible and the basis is
recorded. Reduced sampling must never extend to a material contribution.

**13. Escalation trigger.** An AI contribution discovered in a released model with no contribution
record; a verification that fails; a seeded-error test that a check does not detect; confidential
information found in an unauthorised tool; an AI-produced figure quoted externally without a source line;
a proposal to reduce human review on the strength of machine checking.

**14. AI application.** AI may build and edit model regions, generate formulas and schedules, extract
data and terms, reconcile, design and run checks, generate scenarios, detect anomalies, produce version
diffs and draft commentary — all as **proposals for human verification**.

**15. AI prohibition.** AI must not approve, release, certify or sign any model, output or check; must
not be recorded as an author or an owner; must not decide that a difference is acceptable; must not
determine materiality; and must not be represented as having independently verified anything.

**16. AI verification.** The method is named per contribution and is one or more of: **independent
recomputation** of the affected figures from source; **regression** against a suite of independently
verified figures under `PCI-PFL-LAW-06.05-PR-03`; **source tracing** to the document, version and issuing
party under `PCI-PFL-LAW-06.04`; **clause-to-output comparison** for any extraction from a contract under
`PCI-PFL-LAW-12.01`; **boundary testing** by seeding a known error under PR-05; and **sensitivity
analysis** where the contribution affects a conclusion rather than a figure. "Reviewed the AI output" is
not a method and does not satisfy this element.

**17. External reference.**

- **ISO/IEC 42001 *Information technology — Artificial intelligence — Management system*.** Issuing
  organisation: ISO/IEC. Subject: organisational management of AI systems across their lifecycle.
  Checked: ISO/IEC 42001:2023, 1st edition (register `EXT-021`, verified 2026-08-03). Nature: Manual §6
  category 3 — international voluntary standard. Applicability limitation: voluntary unless imported by
  law or contract; it addresses an organisation's management system, not a project's model.
- **ISO/IEC 23894 *Information technology — Artificial intelligence — Guidance on risk management*.**
  Issuing organisation: ISO/IEC. Subject: guidance on managing AI-related risk. Checked: ISO/IEC
  23894:2023 (register `EXT-024`, verified 2026-08-03). Nature: Manual §6 category 3 — international
  voluntary standard; **guidance, not requirements — it sits alongside ISO/IEC 42001, not under it.**
  Applicability limitation: voluntary unless imported.
- **NIST *Artificial Intelligence Risk Management Framework* (AI RMF 1.0), NIST AI 100-1.** Issuing
  organisation: NIST, US Department of Commerce. Subject: a function-based approach to AI risk — govern,
  map, measure, manage. Checked: AI RMF 1.0, January 2023 (register `EXT-080`, verified 2026-08-03).
  Nature: Manual §6 category 7 — industry guidance; NIST states it is **voluntary, rights-preserving and
  non-sector-specific — not a standard and not a regulation.** Applicability limitation: creates no
  obligation for anyone.
- ***Supervisory Guidance on Model Risk Management* (SR 11-7 / OCC 2011-12).** Issuing organisation:
  United States banking supervisors. Subject: model development, validation and governance expectations
  in supervised institutions. Checked: **not independently verified — verify current requirements**
  (register `EXT-102`). Nature: Manual §6 category 10 — illustrative practice; **supervisory guidance,
  jurisdiction-specific, addressed to supervised institutions.** Applicability limitation: named for
  context only; **no requirement in this law is sourced to it.**
- **Regulation (EU) 2024/1689 (the EU AI Act).** Issuing organisation: the European Union. Subject:
  harmonised rules on artificial intelligence, including obligations that turn on a system's risk
  classification. Checked: in force since 1 August 2024, with phased application — general application
  from 2 August 2026 and remaining rules by 2 August 2027 (register `EXT-100`, verified 2026-08-03).
  Nature: **binding legislation within the European Union**; Manual §6 category 1 where an entity is in
  scope. Applicability limitation: **binding only within its own jurisdiction and only on entities and
  systems within its scope**; whether it applies to a given deployment is a question for qualified
  counsel. It is named here as an external requirement where applicable, never as the source of a PCI
  obligation.

**18. Jurisdictional caution.** Whether a given AI deployment falls within an AI regulation, what
disclosure or human-oversight obligations attach, data-protection and cross-border transfer rules
governing the information placed into a tool, professional-privilege risks in submitting draft documents
to an external service, and any contractual restriction in the *finance documents* or a non-disclosure
agreement on processing transaction information are all jurisdiction- and engagement-specific and are
changing quickly. Obtain qualified legal advice before deploying an AI tool on transaction information —
see `PCI-FND-LAW-09` and `PCI-PFL-LAW-12.02`.

**19. Related PCI Laws.** `PCI-FND-LAW-02` (verification of AI output before professional use);
`PCI-FND-LAW-01`; `PCI-FND-LAW-04`; `PCI-FND-LAW-06`; `PCI-FND-LAW-09`; `PCI-PFL-LAW-06.01`;
`PCI-PFL-LAW-06.05`; `PCI-PFL-LAW-13.01`; `PCI-PFL-LAW-16.03`. **Increment over the foundational
parent:** `PCI-FND-LAW-02` requires AI output to be verified before use; this law names the verification
methods that count in *financial modelling*, requires the method to be recorded per contribution,
requires an AI-operated check to be proved against a seeded error before it is relied upon, forbids
reducing human review because a machine check passed, and ties tool authorisation to the classification
of transaction information.

**20. Related Body of Knowledge content.** PFL-AI · Domain 16 — Data, automation and responsible AI in
finance · KA 16.2 Scenario generation, document review and model assistance · KA 16.3 Explainability,
validation, bias and model risk. Also Domain 6 KA 6.4 (AI-assisted modelling controls) and Domain 11 KA
11.4 (AI model risk).

**21. Compliance test.** A reviewer takes the model, the change log and the AI contribution records, and
performs five steps. (a) For each *material* AI contribution, confirms a contribution record exists
naming the task, the output, the verifier, the method and the date. (b) Re-performs one recorded
verification by the same method and reaches the same conclusion. (c) Confirms no change-log entry names
an AI system as author. (d) Seeds a known error and confirms every AI-operated check that should detect
it does. (e) Confirms, from the tool-authorisation record, that each tool used was authorised for the
classification of information placed into it. Compliance is demonstrated when all five complete; a
material AI contribution with no recorded verification method is a breach.

**22. Breach indicators.** A schedule rebuilt overnight with no contribution record; a verification
recorded as "checked"; a model whose only check suite was written by the same tool that wrote the
calculations; an extraction relied upon with no clause reading; confidential documents in a consumer
service; a review plan shortened because "the tool reconciles it"; commentary in a lender report that no
named professional wrote.

**23. Consequence within PCI authority.** Correction required and the affected output withheld;
additional independent review; escalation; failure of the associated examination competency; ethics
review; certification investigation; suspension or withdrawal of the credential. Each subject to due
process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** AI-verification case: an AI system has rebuilt a debt schedule and the
candidate must state the verification method, the record required and what must not be relied upon.
Scenario judgement: a machine check passes and a human review is proposed to be dropped. No live
examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-16-02` *AI-Assisted Model Verification* (v1.0), and absorbs the
transaction-information limb of `PFL-LAW-16-01` *Transaction Confidentiality* (v1.0) as PR-04, the
general confidentiality obligation remaining with `PCI-FND-LAW-09`. Amendment note: *AI contribution
record* and *authorised tool* defined; seeded-error testing of AI-operated checks added as PR-05; the
prohibition on reducing human review made a process requirement in its own right; the EU AI Act
characterised as legislation binding only within its own jurisdiction and only on systems in scope.

---

### PCI LAW PCI-PFL-LAW-16.02 — AI Precedent and Market-Term Research

**1. Normative requirement.** A credential holder must not use, cite or repeat an AI-produced statement
about a precedent transaction, a market term, a legal position, a standard or a published authority
until it has been traced to the primary source and confirmed against it by a named human.

**2. Purpose.** Research is where machine assistance is most useful and most dangerous. A fabricated
precedent, an invented clause number, a plausible but wrong market range, a superseded edition quoted as
current, a standard characterised as mandatory when it is voluntary — each is fluent, checkable in
seconds, and routinely not checked. In a financing these statements become term-sheet positions and
credit-paper assertions within hours.

**3. Scope.** Every credential holder who uses AI to research or summarise precedent transactions,
market terms and pricing, comparable structures, contractual provisions, standards, frameworks,
regulation, tax positions or published authority, for any internal or external output.

**4. Defined terms.** *AI-assisted work*, *source line*, *evidence*, *verified*, *material*, *decision
owner*, *competent reviewer*, *voluntary framework*. **Primary source** — the document, dataset or
publication that itself establishes the fact asserted: the executed agreement, the publisher's own
current text, the regulator's own instrument, or the transaction record — never a summary, a secondary
description or another model's output. **Characterisation** — the statement of what an external
instrument *is*: who issues it, whether it is legislation, a standard, a voluntary framework, guidance or
a model instrument, and on whom it is binding.

**5. Required actions.**

- **PCI-PFL-LAW-16.02-PR-01 — Trace to the primary source.** The credential holder must obtain the
  *primary source* for every AI-produced factual statement before using it, and must attach it as the
  statement's *source line*.
- **PCI-PFL-LAW-16.02-PR-02 — Confirm currency.** The credential holder must confirm the source is
  current at the date of use — the edition, version or amendment in force — and must record the date the
  currency was checked.
- **PCI-PFL-LAW-16.02-PR-03 — Characterise correctly.** The credential holder must state the
  *characterisation* of every external instrument named in an output, must not describe a *voluntary
  framework*, a model instrument, an intergovernmental understanding or supervisory guidance as
  legislation or regulation, and must not describe a standard as mandatory except where a law, regulator
  or contract makes it so for the entity in question.
- **PCI-PFL-LAW-16.02-PR-04 — Assert no unverified particulars.** The credential holder must not state a
  clause number, an article, an edition, an effective date, a judicial decision or a numeric market range
  that has not been verified against the primary source; where the particular is unverified, the
  instrument or comparable is cited by name only.
- **PCI-PFL-LAW-16.02-PR-05 — Precedent comparability stated.** Where a precedent transaction or market
  comparable is relied upon, the credential holder must state its date, jurisdiction, sector, structure
  and the respects in which it differs from the transaction at hand, and must not present a comparable
  as a market standard.

**6. Prohibited actions.** Citing a precedent, clause, edition, date, decision or market range produced
by an AI system without tracing it to the primary source; describing a voluntary framework as
legislation; presenting a model instrument as law; quoting a superseded edition as current; presenting a
single comparable as the market; attributing a market range to a source that does not publish one;
allowing an AI-produced citation to survive into an external document unverified.

**7. Required evidence.** The primary source for each researched statement, retained; the source line
with the currency-check date; the characterisation as stated in the output; the comparability statement
for each precedent used; the record of statements discarded because no primary source could be found.

**8. Responsible role.** The credential holder who uses or repeats the statement, personally. The
*decision owner* for the output that relies on it.

**9. Approval authority.** The decision owner approves the output. No one may approve the use of an
unverified particular; **the absence of verification is a fact, not a matter for discretion.**

**10. Independence requirement.** Not required for tracing, which is mechanical and repeatable. A
*competent reviewer* independent of preparation must re-trace a sample of researched statements, on a
stated basis, where the output is a diligence report, an information memorandum, an offering document or
any document a third party will rely on.

**11. Materiality or threshold.** Every particular asserted — clause, edition, date, decision, numeric
range — is traced before use, regardless of size, because an invented particular is not a small error but
a false statement. Materiality governs the *sampling density* of the independent re-trace and the depth
of the comparability statement, recorded by the *decision owner*. **PCI sets no sampling percentage.**
*Scale test:* on a small municipal project the research base may be a handful of comparables and one or
two standards, and full tracing is trivial; on a multi-billion cross-border financing with hundreds of
researched statements across jurisdictions, full tracing applies to every particular asserted and stated
sampling to descriptive statements, with the sampling basis recorded before the sample is drawn.

**12. Exception and waiver.** No exception is permitted to PR-01, PR-03 or PR-04. Where a primary source
cannot be obtained, the compliant route is to cite the instrument or comparable **by name only**, with no
particulars, and to say that the particular was not verified — that is compliance, not exception, and it
is the practice the suite's own external-reference register follows.

**13. Escalation trigger.** A cited authority, precedent or decision that cannot be located; a clause
number or edition that does not match the primary source; an instrument found to have been superseded; a
market range with no publisher behind it; discovery that an unverified particular has reached an external
document; a characterisation error found in an issued output.

**14. AI application.** AI may search, summarise, translate and organise research; propose candidate
sources; draft comparisons between a precedent and the transaction at hand; and flag where a cited
instrument may have been revised — all as **leads for human tracing**.

**15. AI prohibition.** **AI must not be the source of any factual statement about a precedent, a market
term, a standard, an authority or the law**, and must not be cited as one. AI must not characterise an
instrument's legal status for use in an output, must not confirm that a source is current, and must not
supply a clause number, edition, date or decision.

**16. AI verification.** Source tracing by a named human to the *primary source* for every particular,
opening the source and reading the passage; clause-to-output comparison of the AI summary against that
passage; and a recorded currency check against the publisher's own current catalogue or the regulator's
own instrument. **An AI citation that cannot be traced is deleted, not softened**, so that it cannot
survive in a draft.

**17. External reference.**

- **NIST *Artificial Intelligence Risk Management Framework* (AI RMF 1.0), NIST AI 100-1.** Issuing
  organisation: NIST, US Department of Commerce. Subject: identifying and managing AI risks including
  output reliability. Checked: AI RMF 1.0, January 2023 (register `EXT-080`, verified 2026-08-03).
  Nature: Manual §6 category 7 — industry guidance; **voluntary; not a standard and not a regulation**.
  Applicability limitation: creates no obligation for anyone.
- **ISO 8000 (data-quality series).** Issuing organisation: ISO. Subject: data quality and provenance.
  Checked: multi-part series; Part 1 is ISO 8000-1:2022; cited generically, no part relied upon
  (register `EXT-026`, verified 2026-08-03). Nature: Manual §6 category 3 — international voluntary
  standard. Applicability limitation: voluntary unless imported by law or contract.
- **The suite external-reference register** — `../registries/EXTERNAL_AUTHORITIES.md`. Issuing
  organisation: PCI. Subject: the classification, edition status and verification date of every external
  authority named anywhere in the PCI Body of Knowledge programme. Checked: compiled 2026-08-03, revised
  2026-08-04. Nature: Manual §6 category 9 — PCI internal professional law and its supporting registry.
  Applicability limitation: **it records what was found on a date; it never substitutes for the official
  publication, which always governs.**

**18. Jurisdictional caution.** Whether a standard, code or framework is mandatory depends on the law,
regulator, professional body or contract applying to the specific entity in the specific jurisdiction,
and the answer differs between jurisdictions for the same instrument. Copyright in standards and in
market data restricts reproduction and redistribution, and those restrictions are jurisdiction-specific.
Obtain qualified local legal advice before asserting that an instrument applies, and before reproducing
any part of a copyright work.

**19. Related PCI Laws.** `PCI-FND-LAW-02`; `PCI-FND-LAW-06`; `PCI-FND-LAW-14`;
`PCI-PFL-LAW-06.04`; `PCI-PFL-LAW-12.01`; `PCI-PFL-LAW-12.02`; `PCI-PFL-LAW-09.03`;
`PCI-PFL-LAW-16.01`. **Increment over the foundational parent:** `PCI-FND-LAW-02` requires AI output to
be verified before use; this law addresses the class of output that verification most often skips — a
statement about the outside world — and adds primary-source tracing, a currency check, a correct
characterisation of legal status, an absolute bar on asserting an unverified particular, and a
comparability statement for every precedent relied upon.

**20. Related Body of Knowledge content.** PFL-AI · Domain 16 — Data, automation and responsible AI in
finance · KA 16.2 Scenario generation, document review and model assistance · KA 16.3 Explainability,
validation, bias and model risk. Also Domain 12 KA 12.1–12.4 (contracts, where precedent language is
most often researched) and Domain 9 KA 9.4 (sustainable finance, where characterisation errors are most
consequential).

**21. Compliance test.** A reviewer takes an output containing researched statements and performs five
steps. (a) Lists every particular asserted — clause, article, edition, date, decision, numeric market
range. (b) For each, obtains the *primary source* and confirms the particular against it. (c) Confirms
each source line records the date its currency was checked, and that the version cited is the one in
force at the output date. (d) Confirms every external instrument is characterised correctly, and that no
voluntary framework, model instrument or supervisory guidance is described as legislation. (e) Confirms
each precedent carries its date, jurisdiction, sector, structure and points of difference. Compliance is
demonstrated when all five complete; **one particular that cannot be traced to a primary source is a
breach.**

**22. Breach indicators.** A clause number cited for a contract form the file does not contain; an
edition year in prose where the programme's own policy is to state none; "market standard is 1.30×" with
no source; a voluntary framework described as a requirement; a superseded standard cited as current; a
precedent list with no jurisdictions; a citation that no one can locate.

**23. Consequence within PCI authority.** Correction required and the affected statement withdrawn from
the output; additional independent review; escalation; failure of the associated examination competency;
ethics review; certification investigation; suspension or withdrawal of the credential. Each subject to
due process and a right of appeal (Charter §9). PCI claims no other consequence.

**24. Examination application.** AI-verification case: a research note cites a clause number and an
edition, and the candidate must state what to do before either may be used. Evidence selection:
distinguishing a primary source from a secondary description. Scenario judgement: a term sheet asserts a
market range the candidate cannot source. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · **new law — no v1.0 predecessor.** Amendment note: v1.0 covered AI in modelling but not in
research, leaving the fabricated-citation failure mode — the most frequently observed AI failure in
professional work — governed only by the general foundational verification duty. *Primary source* and
*characterisation* defined; the delete-rather-than-soften rule stated in element 16 so that an untraceable
citation cannot survive into a later draft.

---

### PCI LAW PCI-PFL-LAW-16.03 — Human Sign-Off

**1. Normative requirement.** Every financial decision, certification, representation and external report
within a credential holder's scope must be approved by a named, competent and authorised human being
before it takes effect.

**2. Purpose.** Accountability is held by a person, and it cannot be held by a pipeline. Where an output
becomes a payment, a certificate, a representation to a lender or a published figure without a human
approving it, there is no one to answer for it — and the suite principle, which is the foundation of
every PCI credential, has been defeated by an architecture rather than by a decision.

**3. Scope.** Every financial decision, certification, representation, compliance certificate, drawdown
request, payment instruction, *distribution* certificate, lender report, investor communication and
published figure within a credential holder's scope, and the design and operation of any process or
automation that produces one.

**4. Defined terms.** *decision owner*, *competent reviewer*, *evidence*, *verified*, *AI-assisted
work*, *material*, *authorised tool*, *escalation threshold*. **Sign-off** — a named individual's
recorded approval, given before the output takes effect, identifying what was approved, at which version,
on what date, and on what basis. **Automated pipeline** — any sequence in which an output is produced and
acted upon without an intervening human approval.

**5. Required actions.**

- **PCI-PFL-LAW-16.03-PR-01 — Named sign-off before effect.** The *decision owner* must record a
  *sign-off* before the output takes effect, identifying the output, its version, the date and the basis
  of approval.
- **PCI-PFL-LAW-16.03-PR-02 — Competence and authority.** The engaging organisation must confirm that
  the person signing holds the competence for the subject matter and the authority for the value or
  consequence involved, and must record both.
- **PCI-PFL-LAW-16.03-PR-03 — No unbroken automation.** No credential holder may design, operate or
  approve an *automated pipeline* in which an AI output becomes a payment, a certificate, a
  representation to a lender or a published figure without a human *sign-off*.
- **PCI-PFL-LAW-16.03-PR-04 — Disclosure of material AI assistance.** The signatory must disclose
  *material* AI assistance in the production of the output, in the manner `PCI-FND-LAW-04` requires, and
  must not present AI-assisted work as unassisted.
- **PCI-PFL-LAW-16.03-PR-05 — The signatory answers.** The signatory must be able to explain the output's
  basis, its principal assumptions and its limitations without reference to the tool that produced it,
  and must not sign an output they cannot explain.

**6. Prohibited actions.** Allowing an output to take effect without a recorded sign-off; signing an
output the signatory cannot explain; signing outside one's competence or authority; designing or
operating an automated pipeline that reaches an external effect without human approval; presenting
AI-assisted work as unassisted; recording an AI system, a service account or a mailbox as the approver;
back-dating a sign-off.

**7. Required evidence.** The sign-off record with signatory, output, version, date and basis; the
competence and authority record for each signatory; the process design showing where human approval sits
in each pipeline; the AI-assistance disclosures; the escalation records where a signatory declined to
sign.

**8. Responsible role.** The named signatory, personally. The engaging organisation's responsible officer
for the authority framework and for the design of every pipeline.

**9. Approval authority.** The named human signatory, within their recorded authority. **Approval
authority is never held by an AI system, a script, a service account, a shared mailbox or a role
inbox**, and it cannot be delegated to any of them.

**10. Independence requirement.** Independence is not required of the signatory, who is ordinarily
accountable rather than independent — that is the point of the sign-off. Where the output is an assurance
opinion or a review relied on by another party, `PCI-PFL-LAW-13.01` and `PCI-PFL-LAW-13.02` apply in
addition, and the independence they require is not satisfied by this law.

**11. Materiality or threshold.** Every output within scope requires a sign-off; materiality governs
**who** signs, through the authority framework, and the *escalation threshold* at which approval moves to
a higher authority. Both are set by the adopting organisation's governance and, where the *finance
documents* name a signatory or a threshold, **the documented requirement governs**. **PCI sets no
authority limits.** *Scale test:* on a small municipal project one or two named signatories may cover the
whole output set, and the framework is a single table; on a multi-billion cross-border financing the
framework is maintained per entity, per facility and per value band, and PR-03 is the binding constraint
because the pressure to automate reporting rises with volume.

**12. Exception and waiver.** No exception is permitted to element 1 or to PR-03. Where a signatory is
unavailable, the compliant route is an alternate named in the authority framework — never the omission of
the sign-off and never an automated release. A sign-off recorded after the output took effect is a
breach, and is recorded as one even where the output was correct.

**13. Escalation trigger.** An output that took effect without a sign-off; a proposed automation that
would reach an external effect without human approval; a signatory asked to sign outside their competence
or authority; a signatory unable to explain the output; a signatory who declines to sign; an approval
recorded against a service account or a shared mailbox.

**14. AI application.** AI may assemble the approval pack, check that every required approval is present
before release, route an output to the correct signatory under the authority framework, and maintain the
sign-off register.

**15. AI prohibition.** **AI must not approve, sign, certify, waive, authorise or release anything**, and
must not be recorded as an approver. AI must not determine which signatory is competent, must not
represent an output as independently verified, and must not be configured with both production and
release rights over the same output.

**16. AI verification.** Before signing, the signatory must satisfy themselves by a named method —
independent recomputation of the principal figures, source tracing of the principal inputs, or
reconciliation to the *authoritative version* — and must record the method with the sign-off. **The
signature attests to the signatory's own verification, not to the tool's**, and a signature given on the
strength of a machine's assurance does not satisfy this element.

**17. External reference.**

- **ISO/IEC 42001 *Information technology — Artificial intelligence — Management system*.** Issuing
  organisation: ISO/IEC. Subject: governance of AI systems including human oversight in an organisation's
  management system. Checked: ISO/IEC 42001:2023, 1st edition (register `EXT-021`, verified 2026-08-03).
  Nature: Manual §6 category 3 — international voluntary standard. Applicability limitation: voluntary
  unless imported by law or contract.
- **OECD *Recommendation of the Council on Artificial Intelligence* (the OECD AI Principles),
  OECD/LEGAL/0449.** Issuing organisation: OECD. Subject: principles for trustworthy AI including human
  agency and accountability. Checked: adopted 2019, **revised May 2024** (register `EXT-081`, verified
  2026-08-03). Nature: Manual §6 category 5 — professional framework; an **OECD Council Recommendation —
  not binding law even on adherents, and never legislation.** Applicability limitation: creates no
  obligation for a credential holder.
- **Regulation (EU) 2024/1689 (the EU AI Act).** Issuing organisation: the European Union. Subject:
  harmonised rules on artificial intelligence, including human-oversight obligations for certain systems.
  Checked: in force since 1 August 2024; phased application, general application from 2 August 2026
  (register `EXT-100`, verified 2026-08-03). Nature: **binding legislation within the European Union**;
  Manual §6 category 1 where an entity is in scope. Applicability limitation: **binding only within its
  own jurisdiction and only on entities and systems within its scope.** Named as an external requirement
  where applicable; **no PCI obligation is sourced to it**, and this law applies to every credential
  holder regardless of whether that Regulation does.
- **COSO — *Internal Control — Integrated Framework*.** Issuing organisation: COSO. Subject: control
  activities, authorisation and accountability. Checked: 2013 framework (register `EXT-084`, verified
  2026-08-03). Nature: Manual §6 category 5 — professional framework; **voluntary in itself**, though
  widely imported by regulators. Applicability limitation: creates no obligation of its own force.

**18. Jurisdictional caution.** Who may sign a certificate or a representation, the validity of electronic
signatures, the personal liability of a signatory for a misstatement, statutory or regulatory
human-oversight obligations for automated decision-making, and restrictions on automated processing that
produces legal effects on a person are all jurisdiction-specific. A sign-off arrangement lawful in one
jurisdiction might not satisfy another's requirements for the same document. Obtain qualified local legal
advice on signing authority and on automated decision-making before designing a release process.

**19. Related PCI Laws.** `PCI-FND-LAW-01` (professional accountability and the suite principle);
`PCI-FND-LAW-03` (human decision authority); `PCI-FND-LAW-04`; `PCI-FND-LAW-02`;
`PCI-PFL-LAW-14.04`; `PCI-PFL-LAW-16.01`; `PCI-PFL-LAW-16.02`; `PCI-PFL-LAW-13.01`. **Increment over
the foundational parent:** `PCI-FND-LAW-03` reserves decision authority to humans; this law makes that
enforceable in a financing by naming the outputs that require a sign-off before effect, requiring
competence and authority to be recorded for each signatory, prohibiting the unbroken pipeline as an
architecture rather than as a decision, and requiring the signatory to be able to explain the output
without the tool.

**20. Related Body of Knowledge content.** PFL-AI · Domain 16 — Data, automation and responsible AI in
finance · KA 16.4 Privacy, cybersecurity, human approval and AI governance · including authority
accumulation as the separation-of-duties failure. Also KA 16.3 (validation and model risk) and Domain 1
KA 1.3.4 (the PCI responsible-AI principle in finance).

**21. Compliance test.** A reviewer takes a sample of outputs that took effect in the period and performs
five steps. (a) Confirms each has a *sign-off* record dated before the output took effect, naming the
signatory, the output and its version. (b) Confirms each signatory appears in the authority framework
with the competence and authority for that output. (c) Traces each production pipeline from input to
external effect and confirms a human approval point exists before the effect. (d) Confirms *material* AI
assistance is disclosed. (e) Interviews one signatory and confirms they can explain the output's basis,
assumptions and limitations without reference to the tool. Compliance is demonstrated when all five
complete; an output that took effect before its sign-off is a breach, and so is a pipeline with no human
approval point even if no defective output has yet resulted.

**22. Breach indicators.** An approval log with entries after the release timestamp; a service account in
the approver column; a report published by a scheduled job; a signatory who refers every question to the
modeller; an authority framework that does not cover the output being signed; AI-assisted commentary
issued without disclosure; an automation with both production and release rights.

**23. Consequence within PCI authority.** Correction required and the affected output withheld or
withdrawn; the process suspended pending correction; additional independent review; escalation; failure
of the associated examination competency; ethics review; certification investigation; suspension or
withdrawal of the credential. Each subject to due process and a right of appeal (Charter §9). PCI claims
no other consequence.

**24. Examination application.** AI-verification case: an automation is proposed that would publish a
covenant certificate on a schedule, and the candidate must identify the prohibited configuration and the
compliant design. Ethical dilemma: a signatory is asked to sign an output they cannot explain because the
deadline has passed. No live examination content is exposed.

**25. Version and status.** Version 2.0 · **draft for approval** under Charter §5 · effective on
approval · supersedes `PFL-LAW-16-03` *Human Approval* (v1.0). Amendment note: restructured onto the
twenty-five-element form; *sign-off* and *automated pipeline* defined; the explain-without-the-tool test
added as PR-05, which converts an unverifiable "approval" into an observable one; the alternate-signatory
route stated expressly so that unavailability cannot be used to justify automated release.

---
## Withdrawal record — v1.0 laws with no successor

Charter §10 requires that a withdrawn law's withdrawal and its reason be published, and that the law is
not deleted from the record. Twenty-two of v1.0's twenty-four laws are superseded by a named law above,
recorded in each successor's element 25. **Two are withdrawn without a successor law**, and their
obligations are carried as process requirements instead, in the domain that already owns the surrounding
discipline.

| Withdrawn v1.0 law | Reason for withdrawal | Where the obligation now lives |
|---|---|---|
| `PFL-LAW-04-01` — Appraisal Discipline (D4) | Its rule bundled four obligations — a stated basis, internal consistency between cash flows and discount rate, one horizon and one perspective, and a prohibition on re-cutting a measure — into a single unenforceable sentence, and its subject is a *presentation* discipline rather than a distinct professional duty. It is not a certification law's worth of independent obligation once `PCI-PFL-LAW-01.01` governs how a financial judgement may be presented. | **`PCI-PFL-LAW-01.01-PR-05`**, whose parent law's scope already reaches appraisals. The basis must be stated in full, the cash flows and the discount rate must share one inflation and one currency basis, and re-basing a measure after a result is known is prohibited. Element 21 gained a fifth step that recomputes the measure on the stated basis. Domain 4 therefore anchors no law in this edition — recorded here so that its absence is deliberate. |
| `PFL-LAW-07-01` — Revenue Assumption Discipline (D7) | Its substance is an assumption-register requirement — record what a revenue actually is — and it duplicated `PCI-PFL-LAW-06.03` and `PCI-PFL-LAW-12.01` in every respect but one: the prohibition on presenting a forecast revenue as contracted. Manual §9 Q12 treats a law that adds nothing to its neighbours as a defect. | **`PCI-PFL-LAW-06.03-PR-05`**: every revenue assumption must record whether it is contracted, regulated, availability-based or forecast, whether it is indexed or fixed, and the payer's credit standing; and a forecast or merchant revenue must not be presented as contracted. Element 21 gained a fifth step tracing any contracted amount to an executed agreement. Domain 7 therefore anchors no law in this edition. |

**Five of the sixteen domains anchor no law**: Domain 2 (accounting foundations), Domain 3 (time value of
money), Domain 4 (investment appraisal), Domain 7 (revenue and commercial models) and Domain 8 (cost,
schedule and contingency). Domains 2 and 3 are taught rather than governed — an arithmetic or
presentation error there is caught by the laws that use the result. Domain 8's obligations sit in
`PCI-PFL-LAW-14.03`, which is where a cost forecast becomes a funding statement. **This distribution is a
deliberate scoping decision, not an oversight**, and it is recorded so that a later reviewer can
challenge it rather than assume it.

---

## Audit findings — the twenty-five questions of Manual §9

Manual §9 requires every question to be answered before a law is approved, and requires the failure and
its resolution to be recorded in the law's file. The table below records the working of Stage 9 (red-team
challenge) and Stage 10 (revision) over the whole set. **A finding is recorded whether or not it
produced a change**, because a question answered and found satisfactory is part of the record.

| # | Question | Laws affected | What changed |
|---|---|---|---|
| 1 | What exact failure does this law prevent? | All 33 | Element 2 rewritten in every law to name a specific observed failure rather than a general risk. Three laws whose v1.0 purpose was a restatement of the rule (`PFL-LAW-06-04`, `PFL-LAW-13-03`, `PFL-LAW-16-03`) now name the failure: an unproduceable figure, a closing record nobody can retrieve, an unbroken automated pipeline. |
| 2 | Mandatory or only recommended? | All 33 | `should` removed from every normative position. All obligations carry `must` / `must not`; sub-obligations became numbered process requirements at Charter Level 4, which are mandatory. No Recommended Practice (`-RP-NN`) is published in this edition, so nothing in the set is optional. |
| 3 | Can a professional know whether it applies to them? | All 33 | Element 3 now names the roles, the decisions, the transaction stages **and** whether the law reaches preparation, review, recommendation, approval or assurance — the last of which v1.0 omitted throughout. |
| 4 | Is the responsible person identifiable? | All 33 | Element 8 names a role, never "the team" or "the organisation". *Model owner*, *decision owner* and *authorised signatory* are defined as single named individuals; `PCI-PFL-LAW-16.03-PR-03` and `PCI-PFL-LAW-14.04` expressly exclude a service account, a shared mailbox or an automated identity. |
| 5 | Is the required action observable? | All 33 | Element 5 recast as numbered process requirements, each with a subject, an action and an object. Unobservable formulations such as "maintain integrity" or "ensure adequacy" were replaced — for example `PCI-PFL-LAW-14.03-PR-01`, which specifies the five components a *cost-to-complete* is built from. |
| 6 | Is compliance provable? | All 33 | Element 7 lists retained artefacts only. `PCI-PFL-LAW-13.04-PR-05` replaces "retained" with retrievability **by a person who was not present at close**, because retention without retrievability cannot be proved. |
| 7 | Is the required evidence proportionate? | 06.03 · 06.04 · 12.01 · 16.01 · 16.02 | Full treatment reserved for *material* items; stated-basis sampling introduced for the remainder, with the basis recorded before the sample is drawn so that "sampling" cannot become "skipping". |
| 8 | Can the law be audited? | All 33 | **This was v1.0's principal defect.** Every element 21 is now a numbered procedure a reviewer performs — recompute, trace, reconcile, seed an error, retrieve, interview — with a stated pass condition and a stated failure. Four tests require the reviewer to *do* something to the model rather than read it (`06.01`, `06.05`, `16.01`, `16.03`). |
| 9 | Can it be examined through a scenario? | All 33 | Element 24 now names the item type — scenario judgement, evidence selection, escalation decision, calculation review, ethical dilemma or AI-verification case — and describes a concrete situation. No law is examinable by recalling its number. |
| 10 | **Can a professional technically comply while defeating its purpose?** | 06.01 · 06.02 · 06.05 · 10.01 · 10.03 · 13.01 · 14.01 · 14.03 · 15.03 · 16.01 | **The highest-yield question in the set.** Ten circumventions were found and closed: a check block whose tolerance is wide enough never to fail (now seeded-error tested, `06.01-PR-03` + element 21(d)); an override register kept but never cleared (`06.02-PR-04`); a regression suite run *after* acceptance (`06.05-PR-04`, with a bounded 48-hour element 12 route so the rule is not simply ignored at a live close); a *CFADS* definition applied correctly but re-derived nowhere after an amendment (`10.01-PR-05`); a compliant minimum ratio reported against the wrong test's level (`10.03-PR-05`); a review whose scope was narrowed after findings were known (`13.01-PR-02`); a sources-and-uses statement balanced by a plug (`14.01-PR-02`); a cost-to-complete that arithmetically cannot rise (`14.03` element 1); a waiver recorded but its knock-on effects never traced (`15.03-PR-04`); and human review reduced because a machine check passed (`16.01-PR-03`). |
| 11 | Does it conflict with another PCI law? | 06.01 ↔ 06.05 · 10.01 ↔ 10.03 · 13.01 ↔ 13.02 · 16.01 ↔ 16.03 | Four overlaps resolved by allocation rather than by cross-reference: the check block is defined once in `06.01-PR-03` and re-run under `06.05-PR-03`; the *CFADS* definition sits in `10.01` and its reporting in `10.03`; review independence sits in `13.01` and adviser self-description in `13.02`; AI verification method sits in `16.01` and the sign-off in `16.03`, with `16.03` element 10 stating expressly that it does not satisfy `13.01`. |
| 12 | Does it duplicate an external standard unnecessarily? | 06.01 · 06.02 · 16.01 | The ICAEW Code and the FAST Standard are named as context, and both laws now state expressly that **conformity with either does not satisfy the PCI obligation** and that the PCI obligation does not require conformity with them. ISO/IEC 42001 is characterised as addressing an organisation's management system, not a project's model, so the laws add rather than restate. |
| 13 | **Does it misrepresent external authority?** | 09.01 · 09.02 · 09.03 · 10.02 · 10.03 · 10.04 · 12.02 · 15.02 · 16.01 · 16.03 | **The highest risk in this volume, and the question most work went into.** Findings: (a) the **Basel Framework** appears in four laws and each now states it is an internationally agreed supervisory standard with **no legal force of its own**, reaching a bank only as a national authority transposes it, never applying to a project or sponsor, and **not the source of any requirement**; (b) the **IFRS Conceptual Framework** is cited once, in `01.01`, expressly as **not a standard**, and no requirement is sourced to it; (c) the **OECD Model Tax Convention** is stated to be **not law in any jurisdiction**; (d) the **Equator Principles**, **IFC Performance Standards** and the market principles behind green and sustainability-linked instruments are all tagged voluntary at every use, and `09.03-PR-03` makes describing a voluntary framework as legislation a breach; (e) the **FAST Standard** and the **ICAEW Code** are stated to impose no obligation of their own; (f) **IESBA** is stated to bind **only where a body, regulator or engagement has adopted it**, and expressly not to be imported by a PCI law; (g) the **OECD Arrangement on Officially Supported Export Credits** is characterised as an inter-governmental understanding, not a treaty and not legislation; (h) the **EU AI Act** is the only instrument called legislation, and is stated to bind only within its jurisdiction and only on systems in scope; (i) **SR 11-7** is characterised as supervisory guidance addressed to supervised institutions, not to advisers. **No clause number, article, edition or effective date is asserted anywhere in this volume except three that were verified**: the IAS 1 → IFRS 18 replacement date of 1 January 2027, ISO/IEC 27001:2022 + Amd 1:2024, and the EU AI Act's phased application dates. `16.02` exists because this failure mode is now largely machine-generated. **Two open items are recorded below the table.** |
| 14 | **Does it require legal or jurisdiction-specific advice?** | All 33 | **No law in this set records "Not applicable" at element 18**, which was a deliberate target: tax, security interests, insolvency, distributions, sanctions and financial-crime obligations are jurisdiction-specific and this is the volume where a generic caution does most damage. Each element 18 names the specific exposures — for example enforceability of a liability cap (`11.01`), lien priority over lenders' security (`14.02`), distributable-reserve and clawback rules with possible personal liability for directors (`15.01`), estoppel arising from repeated informal waivers (`15.03`), and recognition of a competent body and the tax and insolvency characterisation of an Islamic structure (`09.02`). `12.02` is a jurisdictional caution in its entirety. |
| 15 | Does it define the relevant materiality threshold? | All 33 | Element 11 in every law states who sets the threshold, in what metric, and where it is recorded. **PCI publishes no percentage anywhere in this volume.** Where a threshold belongs to the transaction — every coverage level, every reserve balance, every consent threshold, every in-balance test — the law requires **the documented figure to be used and tested**, and says expressly that PCI sets none. |
| 16 | Does it cover AI use? | All 33 | Elements 14, 15 and 16 are populated in every law. Element 16 names a method from the Manual §5.2 list; "review the AI output" appears nowhere. Three laws add a deletion rule — an AI compliance assertion (`09.02`), an AI answer to a construction question (`10.04`) and an untraceable AI citation (`16.02`) are **deleted rather than corrected**, so they cannot survive in a draft. |
| 17 | Does it preserve human accountability? | All 33 | Every element 9 vests approval in a named human or in a party the finance documents name. `16.03-PR-03` prohibits the unbroken automated pipeline as an architecture; `14.04` element 15 makes an automated identity holding prepare, authorise and reconcile rights a prohibited configuration; `13.01` and `13.02` state that an AI system is never *independent*, because independence is a property of a relationship. |
| 18 | Does it contain an exception process? | All 33 | Element 12 states either the process — who approves, what justification, how long, what compensating control, to whom reported — or that no exception is permitted. **Nine laws permit no exception at all.** `09.02` records that PCI cannot grant one, because the subject matter is a determination PCI does not make. Three laws (`06.05`, `12.01`, `16.01`) gained a **bounded, reported** exception route in place of a rule that practice was quietly ignoring. |
| 19 | Does it define escalation? | All 33 | Element 13 lists observable trigger events, not a general duty. *escalation threshold* is defined once and is set by the finance documents or by the adopting organisation's governance, never by PCI. |
| 20 | Is every important term defined? | All 33 | Fourteen compliance-deciding terms are defined at the head of the volume as required, plus twenty-one transaction and modelling terms, and each law's element 4 defines any term local to it. Definitions were checked against `../registries/TERMINOLOGY_AUDIT.md` before drafting: `EAC` is **not used** and `CTC` is used instead (Issue 1 — one symbol, two formulas inside PFL-AI); bare `PV` is **not used** (Issue 2); *coverage*, *verification*, *sponsor* and *baseline* carry context flags or are avoided; *base case* is used in place of *baseline*. No definition is circular. |
| 21 | Is the language concrete and modern? | All 33 | **Zero occurrences of the ISO requirement auxiliary**, verified by search. `may not` is not used for any prohibition. Undefined judgement words carrying an obligation — *appropriate*, *adequate*, *reasonable*, *timely*, *sufficient* — were removed or replaced with a stated test. |
| 22 | **Does it impose an impossible or excessive burden?** | 06.03 · 06.04 · 12.01 · 13.01 · 14.04 · 16.01 · 16.02 | Six burdens were reduced and one deliberately kept. Reduced: full treatment reserved for *material* items with stated-basis sampling elsewhere (`06.03`, `06.04`, `12.01`, `16.01`, `16.02`); review scope and materiality agreed **before** work begins so the reviewer is not asked to test everything (`13.01`). **Kept, deliberately:** `14.04-PR-02` segregation of duties admits no *de minimis*, and a small finance function satisfies it by naming the third role outside the function — a board member or an external administrator — rather than by relaxing the rule. That is stated in element 11 so the small-organisation route is visible rather than left to be discovered. |
| 23 | **Can it operate on small projects and megaprojects?** | All 33 | Every element 11 carries a *Scale test* naming both a small municipal project and a multi-billion cross-border financing. The test changed six laws: `06.01`, `10.01`, `10.03`, `13.03`, `14.01` and `15.01` now require the relevant record or test **per facility, per tranche or per currency** at scale, because a consolidated position can be true of no facility in the transaction; `14.04` gained the small-organisation segregation route; `13.01` gained per-workstream materiality. |
| 24 | Can it operate internationally? | All 33 | No law depends on a single jurisdiction's institutions, forms or terminology. Every obligation that could be jurisdiction-bound is expressed as a duty to apply the **finance documents'** own terms and to obtain local advice. British English throughout; the transliteration *Shariah* is used with *Sharia* noted as the same word. |
| 25 | Is there a clear consequence within PCI authority? | All 33 | Element 23 draws only on the Charter §9 list and states in every law that PCI claims no other consequence. `09.02` adds that PCI claims no authority to rule on any question of religious law; `14.04` and `15.01` add that criminal, regulatory and corporate consequences belong to the relevant authorities and not to PCI. |

### Open items recorded against Q13

Two matters were found during the external-reference pass that this volume cannot close on its own, and
they are recorded rather than resolved, per Charter §5 Stage 5:

1. **AAOIFI is not registered.** `PCI-PFL-LAW-09.02` names the Accounting and Auditing Organisation for
   Islamic Financial Institutions, following the PFL-AI manuscript at KA 9.3.1. It has **no entry in
   `../registries/EXTERNAL_AUTHORITIES.md`**. It is cited by name only, with no standard, number,
   edition or date asserted, and no requirement is sourced to it. **Action required:** register it, with
   its category and the note that some jurisdictions have made its standards mandatory by regulation
   while others leave the question to each institution.
2. **Manual §6's category list has no value for a voluntary intergovernmental instrument.** The
   G20/OECD *Principles of Corporate Governance* and the OECD *AI Principles* are Council
   Recommendations; the OECD *Arrangement on Officially Supported Export Credits* is an
   inter-governmental understanding. None is an ISO/IEC standard (category 3), an environmental or
   social framework (category 8) or a professional body's guidance in the ordinary sense (category 5).
   This volume classifies them as **category 5** and states the instrument's true nature expressly in
   the applicability limitation, which is the honest choice among the available values. This is a
   **borderline call, recorded so that it is deliberate rather than accidental** — the same treatment
   `../registries/EXTERNAL_AUTHORITIES.md` correction C-04 applies to ANSI/EIA-748. **Action required:**
   extend Manual §6 with a category for voluntary intergovernmental instruments, or confirm the present
   classification.

---

## Index of PFL-AI Professional Laws

Thirty-three laws · one hundred and fifty-five process requirements · eleven anchor domains. External
reference categories are the Manual §6 numbers, given in full at the foot of the table.

| ID | Official title | Anchor domain | Principal obligation (element 1, in brief) | External reference categories |
|---|---|---|---|---|
| PCI-PFL-LAW-01.01 | Cash-Flow Integrity in Financial Judgement | D1 — Foundations of project finance leadership | Must not present an accounting, earnings or averaged figure as evidence of ability to pay on a date | 2 (and IFRS *Conceptual Framework*, expressly not a standard) |
| PCI-PFL-LAW-01.02 | Conflict Disclosure and the Two-Hat Rule | D1 — Foundations of project finance leadership | Must disclose in writing, before acting, every interest a reasonable party would want to know | 3 · 5 |
| PCI-PFL-LAW-05.01 | The Bankability Statement | D5 — Project development and bankability | Must not call a project bankable unless every condition is stated with status, owner and resolution path | 3 · 8 |
| PCI-PFL-LAW-06.01 | Financial-Model Architecture | D6 — Financial modelling | A decision-grade model must separate inputs, calculations and outputs, no cell serving two roles | 5 |
| PCI-PFL-LAW-06.02 | Formula Consistency | D6 — Financial modelling | Must not present as calculated any figure that was typed, pasted or overridden | 5 |
| PCI-PFL-LAW-06.03 | Input and Assumption Traceability | D6 — Financial modelling | Every assumption entered once, in the input region, and recorded in a register that travels with the model | 3 · 5 |
| PCI-PFL-LAW-06.04 | The Source Line | D6 — Financial modelling | Must withdraw any figure whose source line cannot be produced on request | 3 |
| PCI-PFL-LAW-06.05 | Model Version Control | D6 — Financial modelling | A model used for a decision must not change except under version control, one authoritative version at a time | 3 · 5 · 10 |
| PCI-PFL-LAW-09.01 | The Capital-Structure Decision Basis | D9 — Funding structure and sources of capital | Must not propose a structure resting on an uncommitted event unless it is stated as an assumption | 10 |
| PCI-PFL-LAW-09.02 | Accuracy in Describing Islamic-Finance Structures | D9 — Funding structure and sources of capital | Must not assert Shariah compliance without a producible determination from the competent body | 3 · 5 |
| PCI-PFL-LAW-09.03 | Sustainable-Finance Claims | D9 — Funding structure and sources of capital | Must not state a sustainability claim at a strength the identified evidence does not support | 1 · 8 |
| PCI-PFL-LAW-10.01 | The CFADS Definition | D10 — Debt sizing, covenants and credit metrics | Must compute CFADS on the finance documents' definition, item by item | 2 |
| PCI-PFL-LAW-10.02 | Debt Sizing | D10 — Debt sizing, covenants and credit metrics | Must size to the documented coverage level, tenor and profile, never to a preferred quantum | 2 · 10 |
| PCI-PFL-LAW-10.03 | Coverage-Ratio Calculation and Reporting | D10 — Debt sizing, covenants and credit metrics | Must report a coverage ratio with its definition, its period basis and its minimum | 2 · 10 |
| PCI-PFL-LAW-10.04 | Covenant Interpretation | D10 — Debt sizing, covenants and credit metrics | Must not state a legal conclusion on the meaning, breach or consequence of a covenant | 2 · 10 |
| PCI-PFL-LAW-10.05 | Reserve-Account Governance | D10 — Debt sizing, covenants and credit metrics | Must fund, apply and release a reserve only as the finance documents specify | 2 |
| PCI-PFL-LAW-11.01 | Risk-Allocation Honesty | D11 — Risk identification and allocation | Must record where each material risk lands once caps, exclusions, insurance and credit are read together | 3 · 4 · 8 |
| PCI-PFL-LAW-12.01 | Contract-Source Verification | D12 — Contracts and transaction structure | Must take every contractual term from the executed document, read at the clause | 3 · 4 |
| PCI-PFL-LAW-12.02 | The Tax and Legal Advice Boundary | D12 — Contracts and transaction structure | Must not give legal, tax, accounting, regulatory or insurance advice, and must obtain written advice before adoption | 2 · 3 · 10 |
| PCI-PFL-LAW-13.01 | Independent Model Review | D13 — Due diligence and financial close | A review relied on outside the reviewer's own team must be performed by a person independent of the work | 3 · 6 · 8 |
| PCI-PFL-LAW-13.02 | Adviser Independence | D13 — Due diligence and financial close | Must not describe self, firm, advice or output as independent while any limb of the definition fails | 5 · 6 |
| PCI-PFL-LAW-13.03 | Conditions Precedent | D13 — Due diligence and financial close | A condition is satisfied only when the required evidence is delivered **and** accepted by the entitled party | 3 · 8 |
| PCI-PFL-LAW-13.04 | Financial-Close Readiness | D13 — Due diligence and financial close | Must not treat a transaction as closed until the complete closing record is captured and retained as one record | 3 |
| PCI-PFL-LAW-14.01 | Sources-and-Uses Integrity | D14 — Construction monitoring and drawdown | Totals must be equal, and the gap must not be closed with a balancing item or an uncommitted source | 3 · 4 |
| PCI-PFL-LAW-14.02 | Drawdown Control | D14 — Construction monitoring and drawdown | Must not draw for work not performed, cost not evidenced, or while out of balance undisclosed | 3 · 4 |
| PCI-PFL-LAW-14.03 | Cost-to-Complete | D14 — Construction monitoring and drawdown | Must build the cost-to-complete from components, never as budget less costs incurred | 2 · 4 |
| PCI-PFL-LAW-14.04 | Funds-Flow Approval | D14 — Construction monitoring and drawdown | Funds must move only through the documented accounts and order, on instructions under segregation of duties | 3 · 5 |
| PCI-PFL-LAW-15.01 | Distribution Testing | D15 — Operations, performance and restructuring | Must not permit or make a distribution unless every documented condition is satisfied at the test date | 2 · 5 |
| PCI-PFL-LAW-15.02 | Refinancing Assessment | D15 — Operations, performance and restructuring | Must assess a refinancing on present value net of every cost of achieving it, not on a headline improvement | 2 · 10 |
| PCI-PFL-LAW-15.03 | Waivers and Amendments | D15 — Operations, performance and restructuring | A waiver or amendment is effective only from the entitled party, in the required form, with its full effect recorded | 2 · 3 |
| PCI-PFL-LAW-16.01 | AI-Assisted Financial Modelling | D16 — Data, automation and responsible AI in finance | AI work touching a model or its outputs must be verified by a named method before use | 1 · 3 · 7 · 10 |
| PCI-PFL-LAW-16.02 | AI Precedent and Market-Term Research | D16 — Data, automation and responsible AI in finance | Must not use an AI statement about the outside world until traced to the primary source and confirmed | 3 · 7 · 9 |
| PCI-PFL-LAW-16.03 | Human Sign-Off | D16 — Data, automation and responsible AI in finance | Every decision, certification, representation and external report must be approved by a named human before effect | 1 · 3 · 5 |

**Manual §6 external-reference categories used above:** 1 applicable legislation or regulation · 2
authoritative financial-reporting standard · 3 international voluntary standard · 4 contract framework ·
5 professional framework · 6 ethical code · 7 industry guidance · 8 voluntary environmental or social
framework · 9 PCI internal professional law · 10 illustrative practice. **A category is a statement
about what an instrument is, not about how important it is**, and no instrument in this volume is
relied upon for a requirement except where element 17 says so expressly.

---

*Version 2.0, compiled 2026-08-04. **Draft for approval under Charter §5** — Stages 6, 11, 12 and 13 not
performed, Stages 4, 5 and 7 partial; see the stage record in the front matter. Thirty-three laws · one
hundred and fifty-five process requirements · twenty-five elements in every law, none omitted · zero
occurrences of the ISO requirement auxiliary · all internal cross-references resolve · British English
throughout. The official publication of every external instrument always governs.*

