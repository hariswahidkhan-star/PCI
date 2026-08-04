# PCL-AI Body of Knowledge — implementation record for the 18 open completeness findings

**Date:** 4 August 2026
**Scope of edits:** `docs/bok/` build inputs only — `00-conventions.md`, the flat `domain-NN-*.md` files and
`appendices.md`. The numbered subdirectories (`01-foundations-accounting/` etc.) are stale working copies and
were not touched.
**Worklist:** 18 findings, PCL-03 to PCL-20.
**Arithmetic gate:** `docs/books/_build/verify_formulas.py` — `✓ all golden answers verified`, 0 failures,
before and after. **No existing number was altered and no new number was introduced** (see §3).
**Legal sweep:** preserved in full. No caution, referral or jurisdictional qualification was removed; twelve
new ones were added (§4).

---

## 1. Finding by finding

### PCL-03 — the PCI Standards were invisible to the reader *(major)*

The worklist referred to the instruments by their withdrawn names and identifiers (`PCL_AI_LAWS.md`,
`PCL-LAW-DD-NN`, `PCI-LAW-F-NN`). Those are superseded. Every citation written here uses the live forms
`PCI-FND-STD-NN` and `PCI-PCL-STD-DD.NN`, re-pointed **by subject** rather than by number, using
`laws/STANDARDS_CONCORDANCE.md` §3.1 — where the old fourteen-standard foundational scheme maps
many-to-many onto the new fifteen, so a one-for-one renumbering would have been wrong.

- **`00-conventions.md` §11 (new)** — "The PCI Standards — the companion instrument": what the Standards are,
  that they are private professional requirements and not legislation, that a stricter legal, regulatory,
  contractual or authoritative professional requirement governs over them, the two identifier forms with a
  worked example of each, the anchoring convention, and why some domains anchor none.
- **A "PCI Standards engaged by this domain" paragraph added to all thirteen domain summaries.** Each lists
  the certification standards anchored in that domain by ID and title, plus the seven foundational standards
  that run throughout. Domains 2, 8 and 9 anchor none; each says so explicitly, explains that this is the
  design, and points at the standards anchored elsewhere that a professional engages while doing that
  domain's work — so the absence reads as complete rather than truncated.
- **Appendix C preamble** gains "Not in this table: PCI's own instrument", so a PCI requirement is never
  mistaken for an external standard or the reverse.

**Deviation from the proposed correction:** the finding scoped the domain blocks to Domains 1–7. They were
added to all thirteen, because Domains 10–13 anchor standards of their own and several of the other findings
in this batch cite them.

### PCL-04 — the universal "neither changes project cost" tax claim *(major)*

`domain-03` KA 3.5.2. The unqualified sentence is replaced by a conditional: recoverable input VAT/GST and
creditable or grossed-up withholding are timing; **irrecoverable** input tax and **non-creditable,
non-grossed-up** withholding are project cost and are budgeted as cost. Common irrecoverable situations are
named at category level. A new paragraph requires the position to be confirmed with the entity's tax function
— and with qualified tax advisers where the answer turns on the contract or a cross-border structure —
**before the forecast is issued**, with the assumption actually used recorded in the basis of estimate
(Toolkit 3.T.1). Two key terms added (*irrecoverable input tax*, *gross-up*); domain summary updated. The
pre-existing **Tax caution** block was left untouched.

### PCL-05 — change-control decision rights asserted but never operationalised *(major)*

`domain-05` KA 5.4.

- 5.4.3's "The professional owns the impact assessment **and the approval**" → "owns the impact assessment and
  the **recommendation**; the change authority approves".
- New **change-authority ladder** (three-tier table: PM within delegation / project board / sponsor and
  counterparty), the aggregation rule against splitting, and the rule that no change is executed before
  authorisation.
- New **minimum change-record evidence** list, and the **escalation trigger** (assessed cost exceeding the
  remaining reserve of its funding source escalates *before* approval).
- 5.4.1 amended to name the authority; KA topic list, learning objectives and domain summary updated.
- Toolkit 5.T.2 columns aligned: *Assessed by*, *Approved by (authority band)* and *Evidence pack complete*
  added, with a usage note explaining why the first two are separate columns, plus a retention line.
- Two key terms, two MCQs (5.4-F, 5.4-G) and two self-check items added.
- Cited to `PCI-PCL-STD-05.03`, `PCI-PCL-STD-05.04`, `PCI-FND-STD-04`.

### PCL-06 — no duty to escalate anywhere in the seven domains *(major — the largest gap)*

New topic **4.3.7 "When the report is contested — the duty to escalate"**, written as the standing treatment
for the whole reference: the trigger (and what distinguishes it from ordinary professional disagreement), the
required act (in writing, with the evidence, before issue), the route with a working definition of an
"inadequate" response, the record, and four prohibitions (do not sign; do not let a changed analysis be
attributed to you; do not participate in suppression; do not go silent). Closes with an explicit statement
that these are PCI's professional obligations and **not** a statement of anyone's legal rights — protected
disclosure, employment protection and any external reporting obligation vary, change, and are for qualified
counsel taken before any external step.

Cross-referenced from the five places the finding identified plus two more: `domain-02` executive perspective
(the onerous test deferred), `domain-03` Advanced 3.A.5 (the ratchet maintained deliberately), `domain-04`
4.2.6 (fade held back), `domain-06` "The governance conversation" (the report edited on its way up) and
Advanced 6.A.3 (a green status against a sliding trend), and the Domain 3, 6, 7 and 8 summary cross-reference
lines.

Apparatus: two key terms, two MCQs (4.3-E, 4.3-F), two self-check items, KA topic list, learning objectives
and domain summary. A **data-protection caution** was added because the topic directs the recording of
information about identified individuals, carrying Domain 11's "record the observation, not a conclusion
about the person" rule across.

Cited to `PCI-FND-STD-11`, `-05`, `-15`, `-01`, `PCI-PCL-STD-04.02`, `PCI-PCL-STD-04.03`.

### PCL-07 — the EVM flagship carried no external anchor *(major)*

`domain-06` Advanced 6.A.2: new "Where the formal apparatus comes from" naming **ANSI/EIA-748** (national
standard, voluntary in itself, reaching a programme only where a contract or procurement regime imports it —
and the source of the "EVMS compliance", surveillance and over-target-baseline vocabulary), **ISO 21508**
(international standard, guidance, not certifiable) and **professional-body practice guidance on earned value
management**. States plainly that none is legislation, that force comes from the contract, and that the
honest answer to "what is an EVMS assessed against?" is "the instrument the contract names, on the terms the
contract sets". 6.A.1's OTB paragraph now points at those anchors. Four Appendix C rows added (ANSI/EIA-748,
ISO 21508, PMI EVM practice guidance, plus a **national standard** category definition).

**Deviation:** the finding asked for edition-level identification. No edition year, revision date or clause
number was used anywhere, per the legal sweep's standing policy.

### PCL-08 — allowable cost never defined *(major)*

`domain-07` 7.1.3: new "What makes a cost reimbursable — allowable cost" defining allowable/defined cost as a
contract-defined subset, listing the recurring exclusions, and stating the two duties (code and segregate at
source — Domain 5, KA 5.2.3; keep records audit-ready for as long as the client's inspection and audit rights
run) plus the forecasting consequence (forecast on allowable cost, not total cost). A "note on the base" was
added to worked example 7.1.3 and to 7.1.4b. Three key terms, two MCQs (7.1-F, 7.1-G), two self-check items,
a Toolkit 7.T.1 row ("Basis of actual cost — allowable-cost definition and audit rights"), learning
objectives and summary.

### PCL-09 — notices, time bars and the statutory overlay *(major)*

`domain-07`: new topic **7.2.6 "Notices, time bars and the statutory overlay"** — the condition-precedent
point moved from a toolkit aside into the teaching text; the five-step discipline (identify, find the window
and its trigger, diarise, serve, *then* substantiate) and the rule of thumb "notify early, price later"; a
**qualitative decision walkthrough** of a meritorious claim defeated on time bar (a five-stage table, no
arithmetic); and the statutory overlay written generically in the sweep's own register — regimes "understood
to exist in a number of jurisdictions", nothing stated anywhere, governing law and the contract both to be
read, local advice before a payment position, withholding, suspension or referral. Cross-referenced from
7.4.2 (new paragraph) and 7.A.6, whose opening now describes the ladder as set by the contract **and** by any
applicable statutory regime, with the adjudication rung corrected. Three key terms, two MCQs (7.2-F, 7.2-G),
two self-check items, Toolkit 7.T.1 rows for notice provisions and governing law, learning objectives and
summary.

### PCL-10 — AI blocks carried capability without the confidentiality limit *(major)*

A confidentiality clause was added to every AI block whose described input is confidential, personal or
potentially privileged, in the priority order the finding set: **Domain 7 KA 7.5** (with the additional note
that a claim file may attract legal privilege and that legal confirms before any external tool is used),
**Domain 1 KA 1.3** (payroll/timesheets), **Domain 5 KA 5.2** (supplier and cost-ledger data), **Domain 2 KA
2.2** (contract extraction and pre-disclosure figures), and **Domain 4 KA 4.3** — which already had one, so
the clause went into the new Domain 7 KA 7.2 block instead, alongside new blocks in Domain 2 KA 2.1 and KA
2.4 and Domain 5 KAs 5.1 and 5.3. Each cites 13.2.5, 13.3.4 and `PCI-FND-STD-09`.

### PCL-11 — no retention expectation on any toolkit *(major)*

Retention paragraphs added to **Toolkit 7.T.2** (contract records/audit provisions and the applicable
limitation or prescription period, whichever is longer; held beyond either where a dispute is live or
foreseeable; period confirmed locally because it varies by jurisdiction and by how the contract was
executed — a question for counsel), **Toolkit 1.T.1**, **Toolkit 1.T.2** (with a data-protection note on
timesheet support), **Toolkit 6.T.1** (baseline change and reserve logs for the life of the programme plus
the audit period) and **Toolkit 5.T.2**. 7.T.2 also gains two checklist lines (notice register; retention
period with named custodian and tested retrieval). All cite `PCI-FND-STD-12`.

### PCL-12 — integrated change control asserted but not operationalised *(major)*

`domain-08` 8.4.2 expanded from one paragraph into five: decision rights with a CCB and a four-tier
authority table, the assessor-is-never-the-approver and aggregation rules, minimum change-request evidence
(including the **benefit** dimension), the prohibition on executing before authorisation with an honest
emergency route requiring ratification and logging as such, four escalation triggers, and retention. Three
key terms, two MCQs (8.4-E, 8.4-F), two self-check items, learning objectives and summary. Case B's
USD 9,600,000 change now names both the **assessor** and the **approver with the authority band**.

### PCL-13 — an accounting-policy judgement assigned to the wrong role *(major)*

`domain-09` 9.5.4: the closing sentence rewritten to allocate decision rights — the controls professional
owns the forecast, the scope-change transparency and the reconciliation of the three progress views; the
**measure of progress used for revenue recognition is finance's accounting-policy judgement**, applied
consistently and tested by external audit, and the controls professional supplies and evidences the inputs
without selecting or changing the basis. Added: a statement that story-point percentage is a proxy and is not
itself an acceptable measure of progress, and a jurisdictional note that IFRS preparers apply IFRS 15 and
US-GAAP preparers the equivalent ASC 606 model, with a "nothing here states what any framework requires"
qualification. MCQ 9.5-C's option A and rationale rewritten so the professional reconciles, evidences and
refers rather than recognising; distractor C reworked. Key term and self-check item added; ASC 606 added to
Appendix C. The AI block was amended to match.

### PCL-14 — delay analysis named no authority and carried no governing-law caveat *(major)*

`domain-10` Advanced 10.A.6: "Where the taxonomies come from" names the **SCL Delay and Disruption
Protocol** and **AACE International's recommended practice on forensic schedule analysis** as voluntary
professional guidance published by private bodies, binding on nobody unless a contract imports one. "The
answer depends on the governing law as much as on the method" identifies the three questions settled outside
the analysis — concurrency, float ownership, prospective versus retrospective — states that starting points
differ between legal traditions, jurisdictions and standard forms, and that a delay position is a legal
position before it is a schedule one, with counsel engaged **before a method is adopted**. A third paragraph
ties the methods to archived contemporaneous records (PCL-20). Both instruments added to Appendix C. The
sweep's existing "A caution on what this topic is" was preserved verbatim.

**Deviation:** the finding asked for "SCL … 2nd edition" and "AACE RP 29R-03". Neither identifier is used —
no edition year and no revision-numbered document identifier, per the sweep's policy. Both instruments are
named descriptively and are identifiable.

### PCL-15 — internal control defined with no framework anchor and no scoping statement *(major)*

`domain-11` 11.3.1: names the **COSO Internal Control — Integrated Framework** as the most widely used
articulation, characterised as a voluntary framework published by a private-sector body whose adoption is the
whole of its force. A caution block states that whether an entity must document, assess or have attested its
internal control over financial reporting turns on jurisdiction, listing status, size and sector, differs
materially between regimes, and is for its finance function, auditors and qualified advisers — with a note
that "compliance" in the objectives triad is used in its ordinary control-framework sense and asserts no
particular rule. Source-and-scope lines added to **Toolkit 11.T.1** and **Toolkit 11.T.2**, both framed as
good practice offered for adoption rather than compliance instruments. Key term added; COSO added to
Appendix C.

### PCL-16 — ISO 31000 framed as an instrument that can be breached *(major)*

`domain-12` 12.1.2: new paragraph characterising ISO 31000 as voluntary international guidance, not
requirements, not intended for certification, of no force unless adopted or imported — and drawing the
consequence that the domain is **principle-led rather than compliance-led**. Key-terms row rewritten to say
the same (glossary updated). MCQ 12.1-D restemmed from "chiefly breached" to "most clearly departs from",
option A and distractor C reworded, and the rationale now makes the departure-not-breach point explicitly and
names its consistency with MCQ 12.1-B. Domain summary reworded from "Working to ISO 31000 principles" to
"Working to the principles ISO 31000 articulates … voluntary guidance adopted rather than imposed".

### PCL-17 — contingency draw-down left unimplementable *(major)*

`domain-12` 12.3.3: new "Draw-down governance" — the draw-down request as the controlling artefact with its
five required contents; approval by the PM within a stated delegated limit and by the sponsor above it, with
the requester never the approver; simultaneous closure of the register entry and opening of an issue;
mandatory period reporting of remaining contingency against remaining exposure; the escalation trigger
(remaining contingency below remaining exposure at the stated P-level → management reserve → the change
authority of 5.4.3); and retention. **Toolkit 12.T.1** gains *Draw approved by (band)* and *Draw approval
date* columns with a usage note. The case study's USD 130,000 draw now runs through the governance — register
ID, evidence, substantiation, PM approval within delegation, register closed and issue opened. Two key terms,
one MCQ (12.3-G), two self-check items, domain summary. Cited to `PCI-PCL-STD-12.03` and `PCI-FND-STD-04`.

### PCL-18 — personal data invoked and never governed *(major)*

`domain-13` 13.2.5 extended from a confidentiality rule into a data-protection rule: the distinction between
"is it confidential?" and "is it personal data?", the categories of personal data project controls actually
handles, and six questions settled **before** the data reaches a tool — basis, compatible purpose,
minimisation, privacy notice, whether an impact assessment is required, and what makes a cross-border
transfer permissible. A caution states that these obligations arise under data-protection law that differs by
jurisdiction (naming the EU General Data Protection Regulation once, purely to identify the kind of
instrument), that workforce uses attract additional constraints and in some places consultation duties, that
nothing is stated anywhere, and that the applicable law governs — while imposing the unconditional
professional sequence: ask first, record the answers, and never treat "we already hold it" as permission.
13.6.3 gains a matching personal-data risk bullet. **Case study B** gains a full privacy paragraph placed
before the cost-benefit case, ending on the general lesson — privacy questions answered and recorded before
the pilot, not after the saving is calculated. Domain 8's stakeholder register (8.1.3) gains a
data-protection caution with two drafting rules; Domain 9 KA 9.6.4 gains a personal-data pointer. Four key
terms, two self-check items, KA topic list; GDPR added to Appendix C.

### PCL-19 — uneven "AI in this KA" coverage *(moderate)*

New blocks written for the two the finding named — **KA 6.3** (predictive EAC: value in earliness and
breadth; limits — cannot see the critical path, cannot see the cause of a variance, cannot select among
methods (a)–(d), cannot own a forecast defended to a board; verification via the TCPI check of 6.2.3 and the
CPI-stability test of 6.A.5) and **KA 7.2** (contract analytics: value in coverage; limits — extraction is
not entitlement, and a model must not be relied on to confirm that a notice window is open or that a clause
is a condition precedent; verification by reading the cited clause in the executed contract) — plus the
audit the finding asked for: new blocks in **2.1**, **2.4**, **3.1**, **3.2**, **3.3**, **5.1** and **5.3**.
Nine blocks in total; every non-AI KA in Domains 1–7 now carries one.

### PCL-20 — record retention required and never specified *(moderate)*

`domain-08` 8.5.1 expanded into "Archiving is a requirement, not an activity" with four heads — **how long**
(the longest of the contract's records/audit provisions, the claim-limitation and defects-liability periods
under the governing law, accounting and tax retention, and any funder, regulator, insurer or auditor
condition; anything under a live or foreseeable dispute held until resolved; the period confirmed with
commercial, finance and legal and written into the closure plan, with a jurisdictional caveat), **whose
custody** (a named custodian in the permanent organisation), **in what form** (retrievable and readable,
retrieval tested rather than assumed) and **why it matters downstream**. Toolkit 8.T.2's single "Records
archived" tick became five auditable lines, with a usage note explaining the replacement. Cross-referenced
from Domain 10 Advanced 10.A.6 and Domain 9 KA 9.6.4, as the finding required. Two key terms, two MCQs
(8.5-D, 8.5-E), two self-check items, learning objectives and summary.

---

## 2. Appendices kept in step

`docs/bok/appendices.md` is hand-maintained for this volume (`_build/make_appendices.py` and
`make_standards.py` cover only the PML-AI and PFL-AI volumes).

- **Appendix B** — 25 new terms merged into the alphabetical glossary and the ISO 31000 entry rewritten;
  count 255 → **280**. Original sort key preserved, so no existing row moved.
- **Appendix C** — eight new rows (ASC 606, ANSI/EIA-748, ISO 21508, PMI EVM practice guidance, SCL Protocol,
  AACE forensic schedule analysis, COSO, EU GDPR); a **national standard** category definition added; the
  legislation sentence updated from one entry to two; and a new "Not in this table: PCI's own instrument"
  preamble paragraph.
- **Appendix E** — 17 new self-check answers; count 129 → **146**.
- **Appendix F** — 12 new items and 2 rewritten (PCL-MCQ-09-20, PCL-MCQ-12-04); count 309 → **321**; the
  per-domain table updated for Domains 4, 5, 7 and 8. **New items are appended at the end of their domain's
  block rather than inserted at chapter position**, so no existing bank number moved — a deliberate choice,
  explained in a new note in the appendix preamble, because external documents already cite these numbers.

---

## 3. Numbers

**None introduced.** Every addition is qualitative: decision walkthroughs, authority tables, checklists,
evidence lists and triage rules. Where a finding proposed a worked example (PCL-09's time-bar claim), it was
written as a five-stage decision table with no arithmetic, per the standing instruction that PCL-AI has no
golden-answer suite. Three incidental figures that crept into first drafts — an illustrative "USD 300,000" in
a Domain 4 caution, an "assessment version 3" in the Domain 8 Case B table, and "three years" in a Domain 8
MCQ stem — were removed before completion. `verify_formulas.py` passes identically before and after; no
existing number, worked example or golden answer was touched.

---

## 4. Legal sweep — preserved, and extended

No caution, referral, jurisdictional qualification or non-affiliation statement was removed or weakened.
**Twelve new cautions or caution-grade passages were added:**

| Where | Subject |
|---|---|
| `domain-03` 3.5.2 | Tax recoverability, gross-up and remittance timing are jurisdiction- and contract-specific; confirm with the tax function and qualified advisers before issue |
| `domain-04` 4.3.7 | Escalation records are personal data (accuracy, proportionality, retention, access) |
| `domain-04` 4.3.7 | The duty is PCI's, not a statement of anyone's legal rights; protected disclosure and external reporting vary and are for counsel |
| `domain-06` 6.A.2 | None of the EVM anchors is legislation; force comes from the contract |
| `domain-07` 7.1.3 | Ambiguity in the allowable-cost definition is settled by reading the contract with commercial and legal, and recorded |
| `domain-07` 7.2.6 | The statutory overlay — regimes understood to exist in some jurisdictions; nothing stated anywhere; local advice before a payment position, withholding, suspension or referral |
| `domain-07` 7.T.2 | Limitation and prescription periods vary and can turn on how a contract was executed — a question for counsel |
| `domain-08` 8.1.3 | Stakeholder registers are personal data; record the observable, not the character judgement |
| `domain-08` 8.5.1 | Retention periods differ by jurisdiction and contract form; nothing states a period for anywhere |
| `domain-10` 10.A.6 | The delay-analysis taxonomies are voluntary guidance; governing law decides concurrency, float ownership and the prospective/retrospective choice |
| `domain-11` 11.3.1 | Whether internal control must be documented, assessed or attested depends on jurisdiction, listing status, size and sector; nothing states the position anywhere |
| `domain-13` 13.2.5 | Data-protection obligations differ by jurisdiction; workforce uses attract additional constraints and in some places consultation duties; the applicable law governs |

Standing rules observed throughout: no statement of what the law requires; no statement of what any external
standard requires; no clause, article, edition year or effective date; no implied endorsement, accreditation
or affiliation; no characterisation of any organisation's or employer's act as lawful or unlawful.

---

## 5. Verification performed

| Check | Result |
|---|---|
| `python3 verify_formulas.py` | `✓ all golden answers verified`, 0 failures, before and after |
| Every `PCI-FND-STD-…` / `PCI-PCL-STD-…` citation resolves to a published standard | 42 distinct IDs, **0 unresolved** |
| Every new numbered cross-reference resolves to an existing KA, topic, advanced topic or toolkit | 66 checked, **0 missing** (2 corrected in the process — a Domain 6 and a Domain 10 KA number) |
| Banned strings — `Target: ~`, `Binds to`, `(draft)`, `SME`, repository paths | **none** |
| Retired names — `PCP-AI`, `PCL-LAW`, `PCI-LAW`, "Professional Laws" | **none** |
| Retired principle wording ("AI proposes, the professional disposes") | **none**; every AI block carries the approved formulation |
| New MCQ option sets and answer keys identical between domain file and Appendix F | 14 pairs, **all match** |
| New key terms present in both the domain key-terms table and Appendix B | 25 terms, **all present in both** |
| Markdown tables preceded by a blank line | **all** |
| British English | no American spellings introduced |

---

## 6. Nothing left incomplete

All 18 findings are implemented. Three were implemented differently from the proposed correction, for the
reasons given above: **PCL-03** (live standard identifiers instead of the withdrawn law IDs, and all thirteen
domains instead of seven), **PCL-07** and **PCL-14** (instruments named without edition years or
revision-numbered identifiers, to hold the sweep's citation policy). No finding was declined.

*Not committed, per instruction.*
