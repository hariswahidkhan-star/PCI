# Suite External-Reference Register — PCL-AI · PML-AI · PFL-AI

**Status:** Binding register for every external authority named anywhere in the PCI Body of Knowledge
programme — the three books and the PCI Standard set. Referenced by
[`../laws/PCI_STANDARDS_DRAFTING_MANUAL.md`](../laws/PCI_STANDARDS_DRAFTING_MANUAL.md) **section 6**, which
requires every external reference inside a PCI Standard to be tagged with one of this register's
categories. (The superseded first specification,
[`../laws/SUPERSEDED_LAW_SYSTEM_v0.md`](../laws/SUPERSEDED_LAW_SYSTEM_v0.md) section 5, carried the same rule
and is retained for history only.)

---

## 1. What this register is

The three books and the PCI Standard set name real standards, contract forms, frameworks and
professional guidance. They **name and characterise** them; they never reproduce them. This register
is the single place where the programme records, for each authority:

- what it is actually called, in the publisher's own words;
- which edition or version was current when the entry was checked;
- **what kind of instrument it is** — the Category column, which is the point of the register;
- where in the corpus it is relied on;
- whether the entry was independently verified, and when.

**Coverage.** Each book carries its own derived `STANDARDS.md` generated from its manuscripts
(`pml-ai/STANDARDS.md`, 33 documents; `pfl-ai/STANDARDS.md`, 14 documents). Those registers are
build-gated but **book-scoped**, and two bodies of content fall outside them entirely:

- **PCL-AI (`docs/bok/`) has no `STANDARDS.md` at all.** Its authorities — the IFRS/IAS suite,
  FIDIC, AACE, PMBOK, ANSI/EIA-748, ISO 21508, the Scrum Guide, SAFe — are disclosed nowhere else.
- **The PCI Standard files (`docs/books/laws/`) are outside every book build.** They are the densest
  users of external authority in the programme — **113 standards, each carrying an element 17
  external-reference block** — and they introduce instruments no manuscript names: COSO, NEC4,
  DAMA-DMBOK, the GAO Schedule Assessment Guide, the DCMA 14-point assessment and the IFRS
  *Conceptual Framework*.

This register covers all of it. It supersedes nothing: where a book's own `STANDARDS.md` and this
register disagree, that disagreement is a defect and is listed in section 15.

### 1.1 How to read the Book locations column

- **Book locations** name a domain (`PCL-AI D6`), plus the Knowledge Area or section heading where
  one was verified. Domain numbers refer to the manuscripts: `docs/bok/domain-*.md` (PCL-AI),
  `docs/books/pfl-ai/manuscript/domain-*.md` and `docs/books/pml-ai/manuscript/domain-*.md`.
- **Standards** names every current PCI Standard whose **element 17** (*external reference*) cites the
  instrument, taken from the standard files themselves, which carry this register's `EXT-` identifiers.
  Where the corpus instead names an instrument in **element 18** (*jurisdictional caution*), that is
  said expressly — EXT-013, EXT-100 and EXT-101 are the cases.
- Within a cell, an identifier written as `-DD.NN` continues the series of the last full identifier:
  `` `PCI-PCL-STD-05.02`, `-05.03` `` means `PCI-PCL-STD-05.02` and `PCI-PCL-STD-05.03`.
- **Withdrawn identifiers appear in the Notes column only, never as a location.** The instruments
  formerly called *Professional Laws* are now **PCI Standards**, and their identifiers migrated twice —
  `PCL-LAW-03-01` → `PCI-PCL-LAW-03.01` → `PCI-PCL-STD-03.01`. This register's Book locations column
  cited the **v1.0** forms, which no longer resolve; every one has been re-pointed **by subject** to the
  standard that actually carries the reliance, or removed where none does. A withdrawn identifier that
  still appears below is quoted in a Note to explain a mapping, and is not a citation. The
  authoritative history is
  [`../laws/STANDARDS_CONCORDANCE.md`](../laws/STANDARDS_CONCORDANCE.md) section 3.

> **Why by subject and not by number.** Both migrations renumbered whole sets. `PCL-LAW-03-01`
> *Estimate Basis* has no successor at all, while `PCI-PCL-STD-03.01` is *Scope Completeness of the
> Performance Measurement Baseline* — a different subject that a number-for-number mapping would have
> silently substituted. `PCI-LAW-F-08` and `PCI-LAW-F-10` swapped numbers on the way to
> `PCI-FND-STD-10` and `PCI-FND-STD-08`. A citation that resolves to the wrong obligation passes every
> mechanical check there is, and is worse than one that dangles.

## 2. Category definitions

Category is not decoration. It is the control that stops a voluntary framework being read as law,
and it is the reason the register exists. Exactly one of the following applies to each entry.

| Category | Means | Test |
|---|---|---|
| **authoritative accounting standard** | An accounting standard issued by a recognised standard-setter, mandatory for entities that apply that framework in a jurisdiction that has adopted it | Would a statutory auditor test compliance with it? |
| **international standard** | A published standard of ISO, IEC or ISO/IEC JTC 1, adopted by international consensus. Adoption is voluntary unless a law or contract imports it | Does it carry an ISO/IEC number? |
| **contract framework** | A published suite of standard contract forms. It binds only the parties who adopt it, and only through the contract they sign | Does it become binding by signature rather than by enactment? |
| **professional guidance** | Guidance published by a professional body or a public audit institution. Persuasive, not binding, unless imported by contract or mandate | Is it published by a body that certifies or audits, as guidance? |
| **voluntary framework** | A framework organisations choose to adopt. Adoption is the whole of its force. **Never described as legislation** | Can an organisation simply decline to adopt it, lawfully? |
| **industry practice** | A widely used method, guideline set or body of knowledge that has no standard-setter's authority behind it, or whose authority is sectoral | Is it followed because it works, rather than because anyone issued it? |
| **national standard** | A published standard of a national standards body or accreditation process. Real, and not international; it binds only where a contract or procurement regime imports it | Was it published through one country's standards process, not ISO/IEC? |
| **supervisory guidance** | A supervisor's or internationally agreed supervisory expectation. It has **no legal force of its own** and reaches anyone only as a national authority transposes it or a supervised firm is subject to it | Would it apply to a firm only through its own supervisor? |
| **illustrative reference** | Named to illustrate a shape, a concept or a regulatory pattern — not relied on for any requirement | Does the corpus rely on it for anything at all? |

**Two categories were added after this register was first compiled.** *National standard* and
*supervisory guidance* are in the live vocabulary — Drafting Manual section 6 categories 11 and 12. The
current Standard set uses category 11 for ANSI/EIA-748; **category 12 is defined but not yet used by
any standard**, and SR 11-7 is still tagged category 10 (*illustrative practice*) in the foundational
set. That is recorded as an open item in section 15 (C-07), not silently reclassified here. This register had
been using *national standard* in EXT-130 without defining it, and carrying ANSI/EIA-748 twice under
two different categories; both are corrected — the vocabulary is stated above, and EXT-090 is aligned
to EXT-130.

**Legislation is deliberately not a category.** The programme's rule is that PCI Standards
are not legislation and that no voluntary framework may be dressed as one. Real legislation named
in the corpus (the EU AI Act, the GDPR) is therefore listed in section 10 as an **illustrative reference** —
because that is genuinely how the books use it, as an example of a regulatory shape — with its
actual legal status stated in the Notes column. That is a description of the corpus's usage, not a
demotion of the instrument.

## 3. The date-sensitivity rule

**Every row in this register is a snapshot, not a warranty.**

Standards are revised, withdrawn and superseded on their publishers' schedules and not on ours. A
citation to a superseded edition is worse than no citation, because it reads as authoritative while
being wrong.

1. **The official publication always governs.** This register records what was found; it never
   substitutes for the source.
2. **The books state no editions in running prose.** That is the deliberate policy of both
   `STANDARDS.md` registers and it is correct. Editions live here, dated, and nowhere else.
3. **A row marked "not independently verified — verify current requirements" has not been checked
   against the publisher.** It may name a current instrument, a superseded one or a retitled one.
   Treat it as an open item.
4. **No edition, clause number, requirement or effective date in this register was inferred.** Where
   a value was not found at the publisher or an authoritative secondary source, the cell says so.
5. **Re-verify before each publication.** section 12 lists the rows whose status is known to be moving.

### 3.6 What this register may state as a date — and what it may not

There is a real conflict between two correct policies, and it is resolved here rather than left to be
discovered row by row.

Each volume's `STANDARDS.md` states, as deliberate policy, that **no entry carries an edition year or
revision date**, because a citation to a superseded edition reads as authoritative while being wrong.
That policy is right, and section 3.2 above is its other half: **editions live in this register, dated, and
nowhere else.** `PML_AI_STANDARDS.md` puts it exactly so — no standard asserts an edition or effective
date, and "editions are held in the suite register with their verification status". Deleting dates from
this register would therefore not make the programme safer; it would leave the corpus with nowhere to
hold them.

What the programme owner's instruction — remove anything doubtful — actually reaches is a narrower
class, and the test is **whether a reader could ever check the value**:

- **(a) Edition or version designations are kept.** `ISO 31000:2018`, `Amd 1:2024`, `EP4`,
  `GAO-16-89G`, `EIA-748-E`, `Regulation (EU) 2024/1689`, `2nd editions, 2017` — these are the
  publisher's own labels and they are *how the right document is retrieved*. A wrong one is visible to
  anyone who looks it up.
- **(b) Dates the corpus depends on are kept, with the dependency named.** IFRS 18 replacing IAS 1 on
  1 January 2027 drives EXT-003, EXT-004, the section 12 watch list and corrections C-02 and C-03, and
  `PCI-FND-STD-06` uses that supersession as its worked example. The EU AI Act's phasing is the same
  case. Remove these and the corrections that depend on them lose their reason.
- **(c) A bare revision, amendment or reprint date on an instrument that carries no edition
  designation is removed.** It cannot be retrieved from the instrument, it cannot be re-verified, it
  ages silently, and nothing in the corpus turns on it. Where such a value was previously asserted, the
  row falls back to this register's existing machinery — it says what was confirmed and stops there.

**Applied at this revision:** EXT-065 and EXT-066 lose "revised 7 August 2020" — AACE Recommended
Practices carry no edition designation, both are cited generically wherever the corpus names them, and
after this repair **no PCI Standard cites either**, so nothing depends on the date. EXT-050 (FIDIC) and
EXT-051 (NEC4) lose their amendment chronologies for the same reason, keeping the edition designations
that identify the documents. No verification date is changed by this rule: a verification date records
when *this register* looked, which is a fact about the register and not a claim about the instrument.

**Why no value could simply be refreshed instead.** Direct verification against the publishers is not
available from this environment: the egress proxy refuses connections to standards-body sites as an
organisation policy denial. Every value below therefore stands or falls on the 2026-08-03 and
2026-08-04 checks recorded in the Verification date column. That is a further reason to hold class (c)
values at nothing rather than at a number nobody can currently confirm.

**Verification method.** Rows dated 2026-08-03 were checked against the publisher's own catalogue or
site on that date (iso.org, ifrs.org, nist.gov, oecd.org, ifc.org, coso.org, pmi.org, aacei.org,
fidic.org, neccontract.com, scrumguides.org, gao.gov, eur-lex.europa.eu), or against an
authoritative record of it. **41 of the 70 entries below were verified this way** — comfortably more
than the 20 most-cited authorities. A further **22 are marked unverified** and carry no edition
claim. The remaining **7** are generic classes (local GAAP, US GAAP, Kanban/Lean) or negative
findings in section 15, where there is no edition to verify; each says so in its own row.

**Rows dated 2026-08-04** were added after that compilation and were checked on that date against
the publisher's own site or an authoritative record of it (iso.org, fatf-gafi.org,
legalinstruments.oecd.org). They are listed in section 14.

### 3.7 Locate by text, not by line number

Line numbers in manuscripts move whenever a paragraph is added above them, and several of this
register's line references had silently drifted onto unrelated sentences by the time they were
re-checked — EXT-005's "D1 lines 1382, 1672" had become lines 1388 and 1678, and corrections C-02,
C-03 and C-04 quoted lines that no longer held the text quoted. A stale line number is the same defect
as a stale identifier: it resolves, and it resolves to the wrong thing. Locations are therefore given
by **domain, Knowledge Area or section heading**, with the quoted text carried alongside so it can be
found by search. Where a line number is still useful it is given as "at this revision", and it is the
quoted text that governs.

---

## 4. Authoritative accounting standards

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-001 | IFRS Foundation / IASB | **IFRS 15** *Revenue from Contracts with Customers* | Issued May 2014; in force | authoritative accounting standard | PCL-AI D2 (129 references, the corpus's most-cited instrument), D1, D7; PFL-AI D2; Standards `PCI-PCL-STD-07.03` | 2026-08-03 | The programme's anchor revenue standard. Books describe the control-based five-step model in their own words; no text reproduced. **Re-pointed by subject, not by number:** the withdrawn `PCL-LAW-07-01` *Commercial Traceability* became `PCI-PCL-STD-07.01` and `-07.02`, and neither relies on IFRS 15; the reliance now sits in `PCI-PCL-STD-07.03` (applications for payment). `PCI-LAW-F-07` was split across three foundational standards and none of them cites IFRS 15. |
| EXT-002 | IFRS Foundation / IASB | **IFRS 16** *Leases* | Issued January 2016; in force | authoritative accounting standard | PCL-AI D2 section 2.4.3; PFL-AI D2 | 2026-08-03 | Named in the books only. **No current PCI Standard cites it** — recorded so the empty Standards column is read as a checked finding, not an omission. |
| EXT-003 | IFRS Foundation / IASB | **IFRS 18** *Presentation and Disclosure in Financial Statements* | Issued April 2024; **effective for annual reporting periods beginning on or after 1 January 2027**, earlier application permitted | authoritative accounting standard | Standards `PCI-FND-STD-06`, `PCI-PFL-STD-10.04`, `PCI-PFL-STD-15.01`, `PCI-PFL-STD-15.03` | 2026-08-03 | **Replaces IAS 1.** The earlier finding that IFRS 18 is "not named anywhere in the corpus" is superseded: the Standard set now names it in four places, and `PCI-FND-STD-06` (source and version integrity) uses the IAS 1 → IFRS 18 supersession as its worked example of a source changing version. The books are unchanged — see section 12 and section 15, where PCL-AI D2 still teaches IAS 1 as current with no forward note. |
| EXT-004 | IFRS Foundation / IASB | **IAS 1** *Presentation of Financial Statements* | In force for periods beginning before 1 January 2027; **superseded by IFRS 18 from that date** | authoritative accounting standard | PCL-AI D2 section 2.1.4 (18 references), section 2.2.7, section 2.4; Standards `PCI-FND-STD-06`, `PCI-PFL-STD-10.04`, `PCI-PFL-STD-15.01`, `PCI-PFL-STD-15.03` | 2026-08-03 | Current today, superseded imminently. The no-offset principle the books rely on is carried into IFRS 18, but the citation will need changing. **Re-pointed by subject, not by number:** `PCI-LAW-F-07` *Honesty in Reporting and Forecasting* was carried into `PCI-FND-STD-05`, `-11` and `-15`, and none of those cites IAS 1. The standard that relies on it is `PCI-FND-STD-06`, which has **no predecessor at all** in the withdrawn foundational set. |
| EXT-005 | IFRS Foundation / IASB | **IAS 8** — currently *Accounting Policies, Changes in Accounting Estimates and Errors*; **retitled *Basis of Preparation of Financial Statements*** from 1 January 2027 | Retitle effective 1 January 2027 (consequential to IFRS 18) | authoritative accounting standard | PCL-AI D1 — *Advanced topics* (IAS 8 named with its current title) and *Case study B* (cross-reference at 1.A.3); Standards `PCI-FND-STD-15` | 2026-08-03 | The title the book prints becomes wrong on 1 January 2027. The book's *substance* (a revised life is a change in accounting estimate) is unaffected. Located by section rather than by line: the line numbers this register carried (1382, 1672) had drifted to 1388 and 1678 — see section 3.7. |
| EXT-006 | IFRS Foundation / IASB | **IAS 37** *Provisions, Contingent Liabilities and Contingent Assets* | In force | authoritative accounting standard | PCL-AI D1 section 1.4 (43 references), D2 section 2.4.5; PFL-AI D2; Standards `PCI-PCL-STD-01.01`, `-01.02`; `PCI-PFL-STD-14.03` | 2026-08-03 | Second most-cited instrument in the corpus; the provisions/contingent-liability boundary underpins the suite's reserve vocabulary. |
| EXT-007 | IFRS Foundation / IASB | **IAS 16** *Property, Plant and Equipment* | In force | authoritative accounting standard | PCL-AI D2 section 2.4.2, D1 | 2026-08-03 | Named in the books only. **No current PCI Standard cites it.** |
| EXT-008 | IFRS Foundation / IASB | **IAS 2** *Inventories* | not independently verified — verify current requirements | authoritative accounting standard | PCL-AI D2 section 2.4.1 | — | Title as printed in the book matches the standard as commonly cited; not checked at ifrs.org. Named in the books only — **no current PCI Standard cites it.** |
| EXT-009 | IFRS Foundation / IASB | **IAS 23** *Borrowing Costs* | not independently verified — verify current requirements | authoritative accounting standard | PCL-AI D2 section 2.4.4 | — | Named in the books only. **No current PCI Standard cites it.** |
| EXT-010 | IFRS Foundation / IASB | **IAS 11** *Construction Contracts* | **Withdrawn** — superseded by IFRS 15 | authoritative accounting standard (superseded) | PCL-AI D2 section 2.4.6 (14 references), appendices | 2026-08-03 | **Correctly handled.** The book teaches it expressly as legacy context and states it is withdrawn (D2 line 992). No correction needed — recorded here so the withdrawal is on the register. |
| EXT-011 | IFRS Foundation | **Conceptual Framework for Financial Reporting** | 2018 (current) | authoritative accounting material — **not itself an accounting standard** | Standards `PCI-PFL-STD-01.01` | 2026-08-03 | The IASB states expressly that the Conceptual Framework is **not a Standard** and that nothing in it overrides any Standard or its requirements. It must never be tagged as an authoritative accounting standard, and a requirement must never be sourced to it. **Three citations removed, not re-pointed.** The foundational set now **excludes it entirely** (Drafting Manual section 6 forbids sourcing a requirement to it) and the PCL-AI set removed every citation it inherited, so the withdrawn `PCL-LAW-01-01`, `PCL-LAW-01-02` and `PCL-LAW-04-01` citations have no successor. Only `PCI-PFL-STD-01.01` still names it, for the accrual basis, with no clause number asserted — correct practice. |
| EXT-012 | IASB (body) | **International Accounting Standards Board** | not independently verified — verify current requirements | authoritative accounting standard (issuing body) | PCL-AI D2 | — | Named as the standard-setter, not relied on for a requirement. |
| EXT-013 | Various national standard-setters | **Local GAAP** (generic) | n/a — generic reference | authoritative accounting standard (generic class) | PCL-AI D2; Standards `PCI-PCL-STD-01.01` (element 18, jurisdictional caution) | — | Never a specific instrument. Always paired with the jurisdictional caution, correctly. The Standard set names it in a jurisdictional caution rather than as an external reference, which is the right element for it. |

## 5. International standards

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-020 | ISO | **ISO 31000** *Risk management — Guidelines* | **ISO 31000:2018**, 2nd edition; reviewed and confirmed 2023 | international standard | PCL-AI D12, appendices; PML-AI D1, D8; Standards `PCI-FND-STD-05`, `PCI-FND-STD-11`; `PCI-PCL-STD-12.01`, `-12.02`, `-12.03`; `PCI-PFL-STD-05.01`, `-11.01`; `PCI-PML-STD-08.01`, `-08.02` | 2026-08-03 | Cancels and replaces ISO 31000:2009. **Guidance, not a certifiable standard** — ISO says so explicitly. Nothing in the corpus claims otherwise. |
| EXT-021 | ISO/IEC | **ISO/IEC 42001** *Information technology — Artificial intelligence — Management system* | **ISO/IEC 42001:2023**, 1st edition | international standard | PCL-AI D13, appendices; PML-AI D1, D9, D14; PFL-AI D16; Standards `PCI-FND-STD-04`, `PCI-FND-STD-14`; `PCI-PCL-STD-13.01`, `-13.02`, `-13.03`, `-13.04`; `PCI-PFL-STD-06.05`, `-16.01`, `-16.03`; `PCI-PML-STD-01.01`, `-14.02` | 2026-08-03 | The first AI management-system standard. Most-cited ISO instrument after ISO 31000. |
| EXT-022 | ISO/IEC | **ISO/IEC 17024** *Conformity assessment — General requirements for bodies operating certification of persons* | **ISO/IEC 17024:2026** — new edition published, superseding ISO/IEC 17024:2012 | international standard | PCL-AI appendices; Standards `PCI-FND-STD-01`, `PCI-FND-STD-10`; `PCI-PFL-STD-01.02`, `-12.02`, `-13.01`; `PCI-PML-STD-01.03` | 2026-08-03 | **Newly revised.** The 2026 edition adds expectations on AI-based assessment tools, human oversight of AI-generated outcomes and competence in AI use — directly relevant to a certification programme built around AI. See section 12. **Two citations removed:** `PCI-LAW-F-10` (conflict of interest) became `PCI-FND-STD-08`, which does not cite this standard, and `PCI-LAW-F-14` (credential claims) was **not carried forward at all**. The foundational set now also states expressly that ISO/IEC 17024 addresses **certification bodies, not individual credential holders** — the withdrawn set cited it four times without that limitation. |
| EXT-023 | ISO/IEC | **ISO/IEC 27001** *Information security, cybersecurity and privacy protection — Information security management systems — Requirements* | **ISO/IEC 27001:2022**, 3rd edition, plus **Amd 1:2024** | international standard | PML-AI D14; PFL-AI D16; Standards `PCI-FND-STD-09`, `PCI-FND-STD-12`; `PCI-PCL-STD-13.01`; `PCI-PFL-STD-13.04`, `-14.04`; `PCI-PML-STD-14.01` | 2026-08-03 | Replaces ISO/IEC 27001:2013. Note the amendment — a register that names only the base edition is incomplete. |
| EXT-024 | ISO/IEC | **ISO/IEC 23894** *Information technology — Artificial intelligence — Guidance on risk management* | **ISO/IEC 23894:2023** | international standard | PCL-AI D13, appendices; PML-AI D1, D9, D14; PFL-AI D16; Standards `PCI-FND-STD-14`; `PCI-PCL-STD-13.02`, `-13.03`; `PCI-PFL-STD-16.01`; `PCI-PML-STD-08.01`, `-14.02` | 2026-08-03 | Guidance, not requirements — it sits alongside ISO/IEC 42001 rather than under it. |
| EXT-025 | ISO | **ISO 15489-1** *Information and documentation — Records management — Part 1: Concepts and principles* | **ISO 15489-1:2016** | international standard | PML-AI D16 (*Why this domain exists*); Standards `PCI-FND-STD-12`; `PCI-PCL-STD-11.01`; `PCI-PFL-STD-06.04`, `-12.01`, `-13.03`, `-13.04`, `-14.01`, `-14.02`, `-15.03`; `PCI-PML-STD-16.02`, `-16.03` | 2026-08-03 | Revised 2016 from the 2001 edition, whose Part 1 was titled *General*. The corpus cites "ISO 15489 records-management standards" generically — safe. **Correction to this register:** section 1 previously listed ISO 15489 among instruments "no manuscript names". PML-AI D16 names it. `PCI-LAW-F-12` *Record Retention* → `PCI-FND-STD-12`, and `PCL-LAW-11-02` *The Audit Trail* → `PCI-PCL-STD-11.01` — the number moved, the subject did not. |
| EXT-026 | ISO | **ISO 8000** *Data quality* (multi-part series) | **ISO 8000-1:2022** *Part 1: Overview*; further parts issued separately (e.g. -2:2022 Vocabulary, -150:2022, -114:2024) | international standard | PML-AI D9, D14; Standards `PCI-FND-STD-07`; `PCI-PFL-STD-06.03`, `-06.04`, `-16.02`; `PCI-PML-STD-14.01` | 2026-08-03 | A **series**, not one document. The corpus cites "ISO 8000 data-quality standards" in the plural — correct, and better than naming a single part. **Re-pointed by subject, not by number:** `PCI-LAW-F-06` *Data Lineage and Integrity* became `PCI-FND-STD-07`, not `-06`; `PCI-FND-STD-06` is *source and version integrity*, a different subject. The withdrawn `PCL-LAW-13-01` *Data Lineage* citation is **removed**: its successor `PCI-PCL-STD-13.01` is *Approved Tools, Recorded Configuration and Protected Project Data* and does not rely on ISO 8000. |
| EXT-027 | ISO | **ISO 21500** *Project, programme and portfolio management — Context and concepts* | **ISO 21500:2021** | international standard | PML-AI D1, D2, D15; Standards `PCI-PML-STD-05.01`, `-15.02` | 2026-08-03 | The 2021 edition changed character: it is now context and concepts, and guidance moved to ISO 21502. A reference to "ISO 21500 (project management guidance)" would now be wrong; the corpus does not make that error. **Re-pointed by subject:** `PCI-LAW-F-05` *Evidence and the Audit Trail* was split across `PCI-FND-STD-02`, `-07` and `-12`, none of which cites ISO 21500; the reliance is entirely in the PML-AI set. |
| EXT-028 | ISO | **ISO 21502** *Project, programme and portfolio management — Guidance on project management* | **ISO 21502:2020** | international standard | PML-AI D1, D4, D11; Standards `PCI-PCL-STD-05.04`; `PCI-PML-STD-01.01`, `-02.01`, `-02.02`, `-03.01`, `-03.03`, `-03.04`, `-04.01`, `-05.01`, `-05.02`, `-06.01`, `-07.01`, `-07.02`, `-08.01`, `-08.02`, `-09.02`, `-10.01`, `-11.01`, `-12.01`, `-12.02`, `-13.01`, `-13.02`, `-15.01`, `-16.01`, `-16.02`, `-16.03` (26 standards) | 2026-08-03 | The most widely cited external instrument in the Standard set. **One citation removed:** `PCL-LAW-04-02` *Escalation* was withdrawn and records **no successor** in the PCL-AI set (escalation is now a foundational subject, `PCI-FND-STD-11`, which does not cite ISO 21502), so it is not replaced. `PCL-LAW-11-01` *Segregation of Duties* → `PCI-PCL-STD-05.04`, which does cite it. |
| EXT-029 | ISO | **ISO 21508** *Earned value management in project and programme management* | **ISO 21508:2018**; **2nd edition under development** at ISO/TC 258 | international standard | PCL-AI D6, appendices; Standards `PCI-PCL-STD-03.02`, `-03.03`, `-06.02`, `-06.03`, `-10.03` | 2026-08-03 | **Correction to this register:** the earlier finding that ISO 21508 is "cited only in the law set" and "absent from every book" is **wrong** — PCL-AI D6 names and characterises it, and PCL-AI's appendices carry it. It remains absent from every book `STANDARDS.md`, because PCL-AI has none; see section 15 C-05. |
| EXT-030 | ISO | **ISO 21503** *Project, programme and portfolio management — Guidance on programme management* | not independently verified — verify current requirements | international standard | PML-AI D1, D15; Standards `PCI-PML-STD-02.02`, `-15.01`, `-15.02`, `-16.03` | — | — |
| EXT-031 | ISO | **ISO 21504** *Project, programme and portfolio management — Guidance on portfolio management* | not independently verified — verify current requirements | international standard | PML-AI D1, D2, D15; Standards `PCI-PML-STD-02.01`, `-02.02`, `-15.02`, `-16.03` | — | — |
| EXT-032 | ISO | **ISO 21505** *Project, programme and portfolio management — Guidance on governance* | not independently verified — verify current requirements | international standard | PML-AI D1, D3, D15; Standards `PCI-PML-STD-01.01`, `-03.01`, `-03.02`, `-03.03`, `-03.04`, `-11.01`, `-13.02`, `-15.01` | — | — |
| EXT-033 | ISO | **ISO 9001** *Quality management systems — Requirements* | **ISO 9001:2015**, 5th edition; a revision is in progress at ISO/TC 176/SC 2 | international standard | PML-AI D9; Standards `PCI-FND-STD-13`, `PCI-FND-STD-15`; `PCI-PML-STD-03.03`, `-05.02`, `-08.02`, `-09.01`, `-09.02`, `-16.01`, `-16.02` | 2026-08-03 | The certifiable one — the book characterises it exactly so, correctly distinguishing it from ISO 9000. See section 12 for the revision. |
| EXT-034 | ISO | **ISO 9000** *Quality management systems — Fundamentals and vocabulary* | not independently verified — verify current requirements | international standard | PML-AI D9; Standards `PCI-PML-STD-09.01` | — | Vocabulary standard, not certifiable. The book's characterisation is right. |
| EXT-035 | ISO | **ISO 10006** *Quality management — Guidelines for quality management in projects* | **ISO 10006:2017** | international standard | PML-AI D1; Standards `PCI-PML-STD-09.01`, `-09.02`, `-16.01` | 2026-08-03 | Title changed at the 2017 edition — the 2003 edition read *Quality management systems — Guidelines…*. The current form is as given. |
| EXT-036 | ISO/IEC | **ISO/IEC 25012** *Software engineering — Software product Quality Requirements and Evaluation (SQuaRE) — Data quality model* | **ISO/IEC 25012:2008** | international standard | PML-AI D9, D14; Standards `PCI-FND-STD-07`; `PCI-PML-STD-14.01` | 2026-08-03 | Long-standing edition; check for supersession within the SQuaRE series before publication. |
| EXT-037 | ISO/IEC | **ISO/IEC 38507** *Information technology — Governance of IT — Governance implications of the use of artificial intelligence by organizations* | **ISO/IEC 38507:2022** | international standard | PML-AI D14; Standards `PCI-FND-STD-04`; `PCI-PML-STD-01.02`, `-14.02` | 2026-08-03 | A governance-of-IT standard aimed at governing bodies, not at practitioners — the book characterises it correctly. |
| EXT-038 | ISO/IEC | **ISO/IEC 27701** *Information security, cybersecurity and privacy protection — Privacy information management systems — Requirements and guidance* | **ISO/IEC 27701:2025** — now a **standalone requirements standard** | international standard | PML-AI D14; Standards `PCI-FND-STD-09`; `PCI-PML-STD-14.01` | 2026-08-03 | **Materially changed.** The 2019 edition was *Extension to ISO/IEC 27001 and ISO/IEC 27002 for privacy information management*. The 2025 edition stands alone. `pml-ai/STANDARDS.md` **no longer** carries the "extension" wording — see section 15, Correction C-01, now closed. |
| EXT-039 | ISO | **ISO 19650** series — *Organization and digitization of information about buildings and civil engineering works, including building information modelling (BIM) — Information management using building information modelling* | Multi-part: **-1:2018**, **-2:2018**, **-4:2022**, **-5:2020** | international standard | PML-AI D14; Standards `PCI-FND-STD-12`; `PCI-PML-STD-14.01` | 2026-08-03 | A **series**. The corpus cites it generically, which is right — no single part is relied on. |
| EXT-040 | IEC (with ISA) | **IEC 62443** series — *Security for industrial automation and control systems* | Multi-part and actively extended (e.g. IEC PAS 62443-1-6:2025) | international standard | PML-AI D14 | 2026-08-03 | Developed jointly with the International Society of Automation; often written ISA/IEC 62443. Cited generically in the corpus. **No current PCI Standard cites it.** |
| EXT-041 | ISO | **ISO 20022** (financial-services messaging) | not independently verified — verify current requirements | international standard | PCL-AI D9 | — | Single passing reference. **No current PCI Standard cites it.** |

## 6. Contract frameworks

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-050 | FIDIC (International Federation of Consulting Engineers) | **FIDIC suite of conditions of contract** — Red Book (*Construction*), Yellow Book (*Plant and Design-Build*), Silver Book (*EPC/Turnkey*) | **2nd editions, 2017**; amendments have been issued since (revision dates deliberately not asserted — see section 3.6) | contract framework | PCL-AI D7 section 7.2.5, appendices; PML-AI D10; Standards `PCI-FND-STD-06`, `PCI-FND-STD-11`; `PCI-PCL-STD-05.02`, `-05.03`, `-07.01`, `-07.02`, `-07.03`; `PCI-PFL-STD-11.01`, `-12.01`, `-14.01`, `-14.02`, `-14.03`; `PCI-PML-STD-04.01`, `-10.01` | 2026-08-03 | The corpus cites FIDIC **generically and asserts no clause numbers** — the Standard set says so expressly at each use. Correct, and important: FIDIC clause numbering moved between editions. **Re-pointed by subject:** `PCI-LAW-F-05` was split three ways and none of its successors cites FIDIC; the foundational reliance is now `PCI-FND-STD-06`, a standard with no predecessor. |
| EXT-051 | NEC (Thomas Telford / ICE) | **NEC4 suite of contracts**, including the Engineering and Construction Contract (ECC) | **NEC4, June 2017 edition**; amendments have been issued since (revision dates deliberately not asserted — see section 3.6) | contract framework | Standards `PCI-PCL-STD-05.02`, `-05.03`, `-07.01`, `-07.02`; `PCI-PML-STD-04.01`, `-10.01` | 2026-08-03 | Cited only in the Standard set, for the compensation-event mechanism, characterised generically. **Absent from every book manuscript and every book `STANDARDS.md`** — verified 2026-08-04. See section 15 C-05. |

## 7. Professional guidance

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-060 | Project Management Institute | **A Guide to the Project Management Body of Knowledge (PMBOK Guide)** | **Eighth Edition** is current; the Seventh Edition (2021) is superseded | professional guidance | PCL-AI D8, appendices; Standards `PCI-FND-STD-11`; `PCI-PCL-STD-03.01`, `-03.04`, `-12.01`; `PCI-PML-STD-03.02`, `-04.01`, `-05.01`, `-05.02`, `-07.01`, `-11.01`, `-13.01` | 2026-08-03 | The corpus cites "PMBOK Guide" **without an edition**, which is the safe form and remains correct across the 7th→8th change. Do not add an edition to prose. |
| EXT-061 | Project Management Institute | **The Standard for Earned Value Management** | not independently verified — verify current requirements | professional guidance | Standards `PCI-PCL-STD-06.01`, `-06.04` | — | Existence confirmed on pmi.org; current edition not established. **Re-pointed by subject:** the withdrawn `PCL-LAW-03-02` *Baseline Integrity* became `PCI-PCL-STD-03.01` to `-03.03`, none of which cites this standard; the reliance sits with the earned-value standards. |
| EXT-062 | Project Management Institute | **Practice Standard for Scheduling** | **Third Edition**, aligned to the PMBOK Guide Seventh Edition | professional guidance | Standards `PCI-PCL-STD-10.01`, `-10.02`; `PCI-PML-STD-06.01` | 2026-08-03 | — |
| EXT-063 | Project Management Institute | **Code of Ethics and Professional Conduct** | not independently verified — verify current requirements | professional guidance | Standards `PCI-FND-STD-01`, `PCI-FND-STD-08`, `PCI-FND-STD-10`; `PCI-PML-STD-01.03`, `-10.01`, `-12.01` | — | Cited for honesty, fairness and conflict disclosure. No text reproduced. **The clearest case of number and subject disagreeing:** `PCI-LAW-F-08` *Competence Boundaries* became `PCI-FND-STD-10`, and `PCI-LAW-F-10` *Conflict-of-Interest Disclosure* became `PCI-FND-STD-08` — the two numbers swapped. `PCI-LAW-F-13` *Ethical Conduct* was **not carried forward at Level 1**; its citation is removed rather than re-pointed. |
| EXT-064 | AACE International | **Total Cost Management (TCM) Framework** | not independently verified — verify current requirements | professional guidance | PCL-AI D8 (*Why this domain exists*), appendices; PML-AI D7 KA 7.1; Standards `PCI-FND-STD-02`, `PCI-FND-STD-05`; `PCI-PCL-STD-01.01`, `-01.03`, `-03.01`, `-03.04`, `-04.02`, `-05.01`, `-06.04`; `PCI-PML-STD-02.01`, `-06.01`, `-07.01`, `-07.02` | — | The most-cited professional-guidance item in the Standard set. Cited for the existence and purpose of the cost-control cycle, never for its text. **"PCL-AI D3" removed as a mis-attribution:** D3 names AACE's *estimate-classification* framework, which is EXT-065/EXT-066's subject, not the TCM Framework. The withdrawn `PCL-LAW-03-01` *Estimate Basis* has no successor (see EXT-065). |
| EXT-065 | AACE International | **Recommended Practice 17R-97** *Cost Estimate Classification System* | Confirmed as a current AACE Recommended Practice at the verification date; **no revision date asserted — see section 3.6** | professional guidance | PCL-AI D3 KA 3.2, appendices (classification framework named generically, **no RP number**); PFL-AI D8; **no current PCI Standard cites it** | 2026-08-03 | Generic classification RP. The corpus names the framework and describes maturity-based classes in PCI's own words; **no accuracy ranges or class tables are reproduced** — correct, those are protected. **Citation removed, not re-pointed.** The v1.0 law that cited it, `PCL-LAW-03-01` *Estimate Basis*, was withdrawn and records **no successor** in the PCL-AI set: no current standard governs the basis of estimate as a subject, and none names 17R-97. Inventing a target would reproduce the defect this repair exists to remove. **The revision date this row carried ("revised 7 August 2020") has been removed** — see section 3.6 for the rule and the reasoning. |
| EXT-066 | AACE International | **Recommended Practice 18R-97** *Cost Estimate Classification System — As Applied in Engineering, Procurement, and Construction for the Process Industries* | Confirmed as a current AACE Recommended Practice at the verification date; **no revision date asserted — see section 3.6** | professional guidance | PCL-AI D3 KA 3.2, appendices (classification framework named generically, **no RP number**); PFL-AI D8; **no current PCI Standard cites it** | 2026-08-03 | Supplements 17R-97 for process industries. PFL-AI D8 names "AACE International's Recommended Practices on cost-estimate classification" as a class, without RP numbers. **Citation removed, not re-pointed**, for the same reason as EXT-065; **revision date removed** under section 3.6. |
| EXT-067 | AACE International | **Recommended Practice 29R-03** *Forensic Schedule Analysis* | not independently verified — verify current requirements | professional guidance | PCL-AI D10 *Advanced topics*, appendices (RP named generically, **no RP number**); Standards `PCI-PCL-STD-07.02`, `-10.02` | — | Cited for the existence of recognised delay-analysis methods, paired with a caution that forum acceptability differs. |
| EXT-068 | AACE International | **Recommended Practices on risk analysis and contingency determination** (generic reference) | not independently verified — verify current requirements | professional guidance | Standards `PCI-PCL-STD-05.03`, `-12.02` | — | Cited as a class, not as a numbered RP — the safest form where the precise RP is not verified, and consistent with the Drafting Manual section 6. |
| EXT-069 | U.S. Government Accountability Office | **GAO Schedule Assessment Guide: Best Practices for Project Schedules** | **GAO-16-89G**, final version issued 22 December 2015 | professional guidance | Standards `PCI-PCL-STD-10.01`, `-10.02`, `-10.03`, `-13.03` | 2026-08-03 | A public audit institution's guide, freely available. Ten best practices; the corpus reproduces none of them. |

## 8. Voluntary frameworks

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-080 | NIST (US Department of Commerce) | **Artificial Intelligence Risk Management Framework (AI RMF 1.0)**, NIST AI 100-1 | **AI RMF 1.0**, January 2023 | voluntary framework | PCL-AI D13, appendices; PML-AI D14; PFL-AI D16; Standards `PCI-FND-STD-14`; `PCI-PCL-STD-13.01`, `-13.02`; `PCI-PFL-STD-16.01`, `-16.02`; `PCI-PML-STD-01.02`, `-14.02` | 2026-08-03 | NIST states it is "voluntary, rights-preserving, non-sector specific". Four functions: Govern, Map, Measure, Manage. **Both `STANDARDS.md` registers say expressly "a voluntary, function-based framework, not a standard and not a regulation" — exemplary, and the model for every other row in this section.** |
| EXT-081 | OECD | **Recommendation of the Council on Artificial Intelligence** (the "OECD AI Principles"), OECD/LEGAL/0449 | Adopted 2019; **revised May 2024** at the Ministerial Council Meeting | voluntary framework | PCL-AI D13, appendices; Standards `PCI-FND-STD-01`, `PCI-FND-STD-14`; `PCI-PCL-STD-13.04`; `PCI-PFL-STD-16.03`; `PCI-PML-STD-01.01`, `-14.02` | 2026-08-03 | An OECD Council **Recommendation** — not binding law even on adherents. The Standard set says "never legislation" at every single use. Correct. `PCI-LAW-F-13`'s citation is removed: that law was not carried forward. |
| EXT-082 | Equator Principles Association | **The Equator Principles (EP4)** | **EP4**, adopted 18 November 2019, **effective 1 October 2020** | voluntary framework | PFL-AI D5, D9; Standards `PCI-PFL-STD-05.01`, `-09.03`, `-13.01`, `-13.03` | 2026-08-03 | Adopted voluntarily by 130+ financial institutions across 38+ countries. PFL-AI D5 calls it "a lender framework under which participating institutions apply agreed environmental and social requirements", names it "for identification only" and states neither body is associated with the book — a model characterisation. `PCI-LAW-F-13` was not carried forward; the reliance is now entirely in the PFL-AI set. |
| EXT-083 | International Finance Corporation (World Bank Group) | **Performance Standards on Environmental and Social Sustainability** | **2012 edition**, effective 1 January 2012; **Sustainability Framework update under way** (approach paper published 2025) | voluntary framework | PFL-AI D5, D9; Standards `PCI-PFL-STD-05.01`, `-09.03`, `-11.01` | 2026-08-03 | Binding on IFC clients by contract; a **benchmark adopted voluntarily** by others, including through EP4. The corpus says "widely adopted as a reference benchmark" — accurate. See section 12: an update is in progress. |
| EXT-084 | COSO (Committee of Sponsoring Organizations of the Treadway Commission) | **Internal Control — Integrated Framework** | **2013**, revising the 1992 original; 17 principles across five components | voluntary framework | PCL-AI D11, appendices; Standards `PCI-FND-STD-13`; `PCI-PCL-STD-04.01`, `-04.02`, `-05.01`, `-05.04`, `-11.01`, `-12.03`; `PCI-PFL-STD-14.04`, `-16.03`; `PCI-PML-STD-03.02` | 2026-08-03 | Voluntary in itself, though widely imported by regulators — the US federal *Standards for Internal Control* adopt its principles. The Standard set tags it "(voluntary framework)" consistently. Correct. `PCL-LAW-04-02` *Escalation* and `PCL-LAW-13-03` *Human Sign-Off* have no COSO-citing successor; those two citations are removed. |
| EXT-085 | OECD | **Arrangement on Officially Supported Export Credits** | not independently verified — verify current requirements | voluntary framework | PFL-AI D9 (line 850); Standards `PCI-PFL-STD-09.01` | — | An **inter-governmental understanding**, not a treaty and not legislation. PFL-AI D9 line 850 characterises it exactly so and adds that "its terms are revised periodically and vary by sector, so they must be checked as at the transaction date" — the date-sensitivity rule applied in prose, correctly. |
| EXT-086 | Ken Schwaber and Jeff Sutherland | **The Scrum Guide** | **November 2020** version, current | voluntary framework | PCL-AI D9 section 9.2, appendices; Standards `PCI-PML-STD-13.01`, `-13.02` | 2026-08-03 | PCL-AI D9 line 140 says "Described from the current Scrum Guide's concepts, in this reference's own words" — an explicit, honest disclosure of the citation basis. |
| EXT-087 | Beck et al. | **Manifesto for Agile Software Development** ("the Agile Manifesto") | not independently verified — verify current requirements | voluntary framework | PCL-AI D9, appendices | — | Stable since 2001; described conceptually, not quoted. **No current PCI Standard cites it.** |

## 9. Industry practice

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-090 | SAE International / NDIA IPMD | **ANSI/EIA-748** *Earned Value Management Systems* | **EIA-748-E released February 2026**, condensing the previous 32 guidelines into **27**; supersedes EIA-748-D (2019) | national standard *(see EXT-130 — one instrument, one category)* | PCL-AI D6, appendices; Standards `PCI-PCL-STD-03.02`, `-06.01`, `-06.02`, `-06.03` | 2026-08-03 | **Just revised.** The Standard set cites it **without an edition and without a guideline count**, so the change does not break it — and the corpus nowhere states "32 criteria" (checked). See section 12. **Correction to this register:** this row and EXT-130 are the **same instrument carried twice under two different categories**, which section 2 forbids. The category here is aligned to EXT-130 (*national standard*), which is also how the current Standard set describes it — see section 15 C-04, now closed. Section 1's earlier statement that no manuscript names ANSI/EIA-748 is **wrong**: PCL-AI D6 names and characterises it. |
| EXT-091 | Defense Contract Management Agency (US) | **DCMA 14-Point Schedule Assessment** | not independently verified — verify current requirements | industry practice | Standards `PCI-PCL-STD-10.01` | — | Correctly tagged "industry practice" — a widely used metric set, not a published standard. |
| EXT-092 | DAMA International | **DAMA-DMBOK: Data Management Body of Knowledge** | **2nd Edition (2017)**; a revised printing of the 2nd Edition exists | industry practice | Standards `PCI-FND-STD-07`; `PCI-PCL-STD-01.03` | 2026-08-03 | A commercially published body of knowledge. **Protected — never reproduced or structurally mirrored** (`SOURCES.md` section 2). **Re-pointed by subject, not by number:** `PCL-LAW-13-01` *Data Lineage* became `PCI-PCL-STD-13.01` *Approved Tools, Recorded Configuration and Protected Project Data*, which does not cite DAMA-DMBOK; the reliance moved to `PCI-FND-STD-07` (data lineage) and `PCI-PCL-STD-01.03` (cost-code integrity). |
| EXT-093 | Scaled Agile, Inc. | **SAFe (Scaled Agile Framework)** | not independently verified — verify current requirements | industry practice | PCL-AI D9 section 9.4, appendices | — | PCL-AI treats it at "awareness only" level and says so in the text. A proprietary commercial framework — the awareness-level treatment is the right call. **No current PCI Standard cites it.** |
| EXT-094 | The LeSS Company | **LeSS (Large-Scale Scrum)** | not independently verified — verify current requirements | industry practice | PCL-AI D9 section 9.4, appendices | — | Awareness level only. **No current PCI Standard cites it.** |
| EXT-095 | Various | **Kanban / Lean / Little's Law** (methods) | n/a — generic methods | industry practice | PCL-AI D9, appendices; PML-AI D13, D15, appendices | — | Generic methods with no single owner; no attribution issue. Named in `PCI-PML-STD-13.01` element 20 (*related Body of Knowledge content*), which is not an external-reference citation. |
| EXT-096 | Various | **Six Sigma** | not independently verified — verify current requirements | industry practice | PML-AI D9 | — | Single passing reference. **No current PCI Standard cites it.** |

## 10. Illustrative references, including legislation named as a reference point

**Read section 2 first.** Nothing in this section is relied on by the corpus for any requirement. The two
EU instruments below **are genuine legislation** within their jurisdiction; they appear here because
the corpus names them only to illustrate a regulatory shape, never as a source of obligation.

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-100 | European Union | **Regulation (EU) 2024/1689** laying down harmonised rules on artificial intelligence (the "AI Act") | In force since **1 August 2024**; phased application — prohibitions from 2 February 2025, GPAI obligations from 2 August 2025, general application from **2 August 2026**, remaining rules by 2 August 2027 | illustrative reference *(actual status: **binding legislation** within the EU)* | PCL-AI D13, appendices; PML-AI D14; Standards `PCI-FND-STD-04`, `PCI-FND-STD-14`; `PCI-PFL-STD-16.01`, `-16.03`; `PCI-PML-STD-01.02`, `-14.02`; and in element 18 (jurisdictional caution) of `PCI-PCL-STD-13.01` and `-13.04` | 2026-08-03 | `pml-ai/STANDARDS.md` places it under "**Regulation — named as reference points, not as applicable law**" and says it is "binding only within its own jurisdiction". The foundational set calls it "legislation in its jurisdiction … an external requirement where applicable". **Both are exactly right.** The PCL-AI set deliberately moved the AI Act and the GDPR **out of element 17 into element 18**, because it uses them as jurisdictional cautions and not as authority — recorded here so the split location is not read as a missing citation. |
| EXT-101 | European Union | **General Data Protection Regulation** (Regulation (EU) 2016/679) | not independently verified — verify current requirements | illustrative reference *(actual status: **binding legislation** within the EU)* | PML-AI D14; Standards `PCI-FND-STD-09`; `PCI-PML-STD-14.01`; and in element 18 (jurisdictional caution) of `PCI-PCL-STD-13.01` | — | Named as an example of a rights-based data-protection approach. Never relied on for a requirement. |
| EXT-102 | US Federal Reserve / OCC | **SR 11-7 / OCC 2011-12** *Supervisory Guidance on Model Risk Management* | not independently verified — verify current requirements | illustrative reference *(actual status: **supervisory guidance**, jurisdiction-specific)* | Standards `PCI-FND-STD-03`, `PCI-FND-STD-13`; `PCI-PFL-STD-06.05`, `-16.01` | — | The Standard set calls it "public supervisory guidance … jurisdiction-specific, cited as guidance only". Accurate. `PCI-LAW-F-02` *Verification of AI Output* was split into `PCI-FND-STD-03` (verification) and `PCI-FND-STD-14` (boundary); the SR 11-7 reliance followed the verification half. |
| EXT-103 | Various | **US GAAP** (generic) | n/a — generic reference | illustrative reference | PCL-AI D9, appendices | — | Named only to contrast with IFRS. **Two corrections:** the book location was recorded as PCL-AI D2 and is in fact **D9** (with an ASC 606 row in the appendices); and `PCL-LAW-01-01`'s citation is removed — **no current PCI Standard names US GAAP.** |
| EXT-110 | Basel Committee on Banking Supervision (BCBS) | **The Basel Framework** (consolidated BCBS standards) | consolidated framework, as maintained by the BCBS | illustrative reference — internationally agreed supervisory standards with **no legal force of their own** | Standards `PCI-PFL-STD-09.01`, `-10.02`, `-10.03`, `-10.04`, `-15.02` | 2026-08-03 | Corrects an earlier negative finding in this register, which recorded that "Basel" appeared in no Standard file; it is named in five PFL-AI standards (`PFL_AI_STANDARDS.md`). The Committee has no supranational authority: its standards bind banks only as national authorities transpose them, and they must never be described as regulation applying directly to a project or its sponsors. **Re-pointed by subject:** `PFL-LAW-10-01` became `PCI-PFL-STD-10.01` to `-10.03`, but `-10.01` (the CFADS definition) does not cite Basel; `PFL-LAW-10-02` became `PCI-PFL-STD-10.04`. Named in no book manuscript — verified 2026-08-04. |

## 11. Registered but not used

`SOURCES.md` carries rows that no manuscript and no PCI Standard actually relies on. They are not
defects — `SOURCES.md` marks them "Proposed" — but a reader should not infer from them that the corpus
draws on these bodies. **Verified by search across all manuscripts and Standard files on 2026-08-03
and re-verified on 2026-08-04.**

| Ref ID | Authority | `SOURCES.md` row | Actual corpus usage | Notes |
|---|---|---|---|---|
| EXT-111 | World Bank Group | S-07, "Proposed (PFL-AI D5–D15)" | **None** as "World Bank". IFC Performance Standards (EXT-083) are used and are covered by S-08 | — |
| EXT-112 | IPMA, ACCA, CIMA, CFA Institute | `SOURCES.md` section 2 prohibited-use register | **None** — correctly, that is the point of the row | Listed only as bodies whose material must never be reproduced. |
| EXT-113 | PRINCE2 (PeopleCert) · APM | Not registered | **None** | Recorded so a future reviewer does not expect them. Neither appears anywhere in the corpus. |

---

## 12. Rows whose status is known to be moving

Re-verify each of these immediately before publication. This is the date-sensitivity rule (section 3.5) made
operational.

| Ref ID | Authority | What is moving | Deadline that matters |
|---|---|---|---|
| EXT-003 / EXT-004 | IFRS 18 / IAS 1 | IFRS 18 **replaces IAS 1** | **1 January 2027** — under six months away |
| EXT-005 | IAS 8 | **Retitled** *Basis of Preparation of Financial Statements* | **1 January 2027** |
| EXT-022 | ISO/IEC 17024 | **2026 edition published**, superseding 2012; adds AI-assessment expectations | Already effective |
| EXT-038 | ISO/IEC 27701 | **2025 edition is standalone**, no longer an ISO/IEC 27001 extension | Already effective |
| EXT-090 | ANSI/EIA-748 | **Revision E published February 2026**; 32 guidelines → 27 | Already effective |
| EXT-029 | ISO 21508 | **2nd edition in development** at ISO/TC 258 | Watch |
| EXT-033 | ISO 9001 | Revision in progress at ISO/TC 176/SC 2 | Watch |
| EXT-083 | IFC Performance Standards | Sustainability Framework **update in progress** | Watch |
| EXT-060 | PMBOK Guide | **Eighth Edition** now current | Already effective; corpus cites no edition, so unaffected |
| EXT-133 | ISO 37001 | **2025 edition published**, superseding ISO 37001:2016 | Already effective; the book states no edition, so the prose is unaffected |
| EXT-132 | FATF Recommendations | **Updated periodically** by plenary decision, so any edition claim ages within months | Continuous — re-verify at each publication; the book cites no Recommendation by number |

---

## 13. Cited in the PCI Standard set, registered here

The PCI Standard files in `docs/books/laws/` cite eleven instruments that neither book's
`STANDARDS.md` registers, because the standards reach subject matter the chapters do not. Each was
verified directly with its publisher during the red-team audit on 2026-08-03. They are registered here
so that this file remains the single disclosure point it claims to be.

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-120 | IFRS Foundation | **IAS 7** *Statement of Cash Flows* | current | authoritative accounting standard | Standards `PCI-PFL-STD-01.01`, `-10.01`, `-10.02`, `-10.03`, `-10.05` | 2026-08-03 | Mandatory only for entities applying IFRS Accounting Standards in a jurisdiction that has adopted them. It defines no coverage ratio — those are creatures of the finance documents. |
| EXT-121 | IFRS Foundation | **IAS 12** *Income Taxes* | current | authoritative accounting standard | Standards `PCI-PFL-STD-12.02` | 2026-08-03 | Governs financial reporting of tax only, never the tax position itself. Any tax position needs qualified local advice. **The number moved:** `PFL-LAW-12-01` became `PCI-PFL-STD-12.02`, not `-12.01`. |
| EXT-122 | IFRS Foundation | **IAS 36** *Impairment of Assets* | current | authoritative accounting standard | Standards `PCI-PFL-STD-15.02` | 2026-08-03 | Cited for the impairment-indicator discipline; entities applying IFRS only. **Re-pointed by subject, not by number:** `PFL-LAW-15-01` became `PCI-PFL-STD-15.01` *Distribution Testing*, which does not cite IAS 36; the impairment discipline sits in `PCI-PFL-STD-15.02` *Refinancing Assessment*. Citing `-15.01` would have resolved perfectly and pointed at the wrong obligation. |
| EXT-123 | ISO | **ISO 45001:2018** *Occupational health and safety management systems — Requirements with guidance for use* | 2018 | international standard | Standards `PCI-PML-STD-07.02`, `-12.01`, `-12.02`, `-16.01` | 2026-08-03 | Certifiable management-system standard. Adoption is voluntary unless a contract or regulator requires it. **Re-pointed by subject:** `PML-LAW-09-01` became `PCI-PML-STD-09.01` *Quality Acceptance*, which does not cite ISO 45001; the occupational-health reliance is in the leadership, resource and transition standards. |
| EXT-124 | ISO | **ISO 45003:2021** *Occupational health and safety management — Psychological health and safety at work — Guidelines* | 2021 | international standard (guidance, not certifiable) | Standards `PCI-PML-STD-12.02` | 2026-08-03 | Guidance, not a requirements standard; nothing can be certified against it. |
| EXT-125 | ICAEW | **Financial Modelling Code** | current | professional guidance | Standards `PCI-FND-STD-02`, `PCI-FND-STD-03`, `PCI-FND-STD-05`; `PCI-PFL-STD-06.01`, `-06.02`, `-06.03`, `-06.05` | 2026-08-03 | Principles-based guidance published by a professional body. Not a compliance standard and not certifiable. |
| EXT-126 | FAST Standard Organisation | **The FAST Standard** | current | voluntary framework | Standards `PCI-FND-STD-03`; `PCI-PFL-STD-06.01`, `-06.02` | 2026-08-03 | Adopted voluntarily by modellers and firms; imposes no obligation of its own. |
| EXT-127 | IESBA / IFAC | **International Code of Ethics for Professional Accountants (including International Independence Standards)** | current | professional guidance | Standards `PCI-FND-STD-08`, `PCI-FND-STD-10`, `PCI-FND-STD-15`; `PCI-PFL-STD-13.01`, `-13.02` | 2026-08-03 | Binding **only** where a professional body, regulator or engagement has adopted it. A PCI credential holder who is not subject to it is not made subject to it by a PCI Standard. |
| EXT-128 | OECD (G20/OECD) | **G20/OECD Principles of Corporate Governance** | 2023 revision (OECD/LEGAL/0413) | voluntary framework | Standards `PCI-PFL-STD-01.02`, `-13.02`, `-15.01`; `PCI-PML-STD-01.03`, `-03.01`, `-03.04` | 2026-08-03 | An OECD Council Recommendation. Non-binding; not legislation. The current title carries the G20/OECD attribution. **Re-pointed by subject:** `PFL-LAW-10-02` became `PCI-PFL-STD-10.04` *Covenant Interpretation*, which does not cite it; the governance reliance moved to `PCI-PFL-STD-13.02` *Adviser Independence*. |
| EXT-129 | OECD | **Model Tax Convention on Income and on Capital** | current | illustrative reference | Standards `PCI-PFL-STD-12.02` | 2026-08-03 | A model instrument. **It is not law in any jurisdiction**; only the executed treaty and domestic law bind. |
| EXT-130 | SAE International (ANSI-accredited) | **ANSI/EIA-748** *Earned Value Management Systems* | edition deliberately not asserted | national standard | PCL-AI D6, appendices; Standards `PCI-PCL-STD-03.02`, `-06.01`, `-06.02`, `-06.03` | 2026-08-03 | A published US national standard, binding only where a contract or procurement regime imports it. The Standard set deliberately asserts no guideline count or edition, because the guideline count changed at the most recent revision. **This row and EXT-090 are the same instrument**; EXT-090's category has been aligned to this one. |

## 14. Financial crime, anti-bribery and sanctions

Three instruments registered on 2026-08-04, when PFL-AI D1 KA 1.3 gained a topic on bribery,
sanctions and the money-laundering perimeter (**1.3.2**). They are grouped here because they are the
three most often misdescribed instruments in this subject matter, and because the category column is
the entire point of registering them: **a treaty that is not law in any jurisdiction, an
intergovernmental standard that is not legislation, and a voluntary standard whose certification is
not a defence.** The book names and characterises each and relies on none of them for a requirement;
the applicable offence, its extraterritorial reach and any adequate-procedures defence are stated in
the text to be jurisdiction-specific questions for qualified counsel.

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-131 | OECD | **Convention on Combating Bribery of Foreign Public Officials in International Business Transactions** (the "OECD Anti-Bribery Convention"), OECD/LEGAL/0293 | Adopted 21 November 1997; **entered into force February 1999**; **46 Parties** at the date checked | illustrative reference *(actual status: **a treaty**, binding on the states party to it and taking effect only through each signatory's own domestic criminal law)* | PFL-AI D1 section 1.3.2 | 2026-08-04 | Registered as an illustrative reference on the section 2 rule and the EXT-100 precedent: the corpus names it to characterise a regulatory shape and relies on it for no requirement. **It is not itself law anywhere** — the signatories implemented it differently, so what applies to a transaction is the domestic offence. Monitored by peer review through the OECD Working Group on Bribery. The book states no article, no offence definition and no penalty. |
| EXT-132 | Financial Action Task Force (FATF) | **International Standards on Combating Money Laundering and the Financing of Terrorism & Proliferation — the FATF Recommendations** | Adopted February 2012; **updated periodically**, with updates recorded by the publisher through 2026 (Recommendation 16 revised at the June 2025 plenary) | illustrative reference — intergovernmental standards with **no legal force of their own** | PFL-AI D1 section 1.3.2 | 2026-08-04 | The same treatment as the Basel Framework (EXT-110), and for the same reason: the Recommendations are addressed to **countries**, and they reach a project only through national legislation and the supervised institutions that implement it. They must never be described as regulation applying directly to a project, a sponsor or an adviser. The book cites no Recommendation by number. **Moving — see section 12.** |
| EXT-133 | ISO | **ISO 37001** *Anti-bribery management systems — Requirements with guidance for use* | **ISO 37001:2025**, superseding ISO 37001:2016 | international standard | PFL-AI D1 section 1.3.2; Standards `PCI-FND-STD-08`, `PCI-PFL-STD-09.02` | 2026-08-04 | Certifiable, and that is exactly where it is misused: **certification is not a legal defence**, it is a third party's opinion about a management system at a point in time. The book says so at the point of use and again in `pfl-ai/STANDARDS.md`. Adoption is voluntary unless a contract or regulator requires it. **Newly revised — see section 12.** |

## 15. Corrections needed in the books and in this register

**Seven items; two of them are now closed.** None is a mischaracterisation of an authority's legal
status — **the corpus's handling of voluntary-versus-binding is consistently correct**, which is the
finding that matters most for this register. Every "voluntary framework" is labelled voluntary at the
point of use; the EU AI Act is the only instrument called legislation, and it is one.

### C-01 — ISO/IEC 27701 described as an extension it no longer is — **CLOSED**

**File:** `docs/books/pml-ai/STANDARDS.md` — the ISO/IEC 27701 row

The row previously read "privacy information management, **extending the information-security
management system**", which described the withdrawn 2019 edition. It now reads:

> "privacy information management. Its relationship to an ISO/IEC 27001 information-security
> management system, and whether it can be certified in its own right, differ between editions;
> establish which edition any claim of conformity refers to before relying on it"

That is better than the wording this register proposed, because it states the *edition sensitivity*
rather than asserting the 2025 position in a register that carries no editions. **No action
outstanding.** (The correction previously cited "line 67"; the row is at line 79 at this revision —
see section 3.7.)

### C-02 — IAS 1 taught as current with no note that IFRS 18 replaces it — **open**

**File:** `docs/bok/domain-02-financial-reporting.md` — **KA 2.1.4**, "IAS 1 and the presentation
principles", and the offsetting material at section 2.2.7 and the worked example that follows it

> "**IAS 1 (presentation of financial statements)** sets how the statements are presented"

True today; false for periods beginning on or after **1 January 2027**, when IFRS 18 *Presentation and
Disclosure in Financial Statements* replaces it. The Standard set has already moved: `PCI-FND-STD-06`,
`PCI-PFL-STD-10.04`, `-15.01` and `-15.03` all name the supersession (EXT-003). **The book is now the
only place in the corpus that teaches IAS 1 as current with no forward note.**

The book has an established, well-executed pattern for exactly this: section 2.4.6 handles the
IAS 11 → IFRS 15 supersession explicitly and correctly (EXT-010).

**Proposed:** apply the section 2.4.6 pattern to IAS 1 — one sentence in KA 2.1.4 recording that IFRS 18
replaces IAS 1 for periods beginning on or after 1 January 2027 and that the no-offset principle
relied on at section 2.2.7 and section 2.3 carries over. Do **not** restate IFRS 18's requirements; the programme
does not assert requirements it has not verified.

### C-03 — IAS 8's printed title expires on 1 January 2027 — **open**

**File:** `docs/bok/domain-01-foundations-of-accounting.md` — *Advanced topics — Domain 1*, and the
cross-reference in *Case study B*

> "Under **IAS 8 (accounting policies, changes in accounting estimates and errors)** three superficially…"

From 1 January 2027 IAS 8 is retitled ***Basis of Preparation of Financial Statements*** as a
consequential amendment to IFRS 18. The book's substantive point — that a revised asset life is a
change in accounting estimate — is unaffected.

**Proposed:** either drop the parenthetical title and cite "IAS 8" alone (consistent with the
programme's own no-editions-in-prose policy), or add the retitling date. The first is cheaper and
more robust. (The line numbers this correction previously carried, 1382 and 1672, had drifted to 1388
and 1678 — see section 3.7.)

### C-04 — ANSI/EIA-748 tagged "industry practice" where it is a published standard — **CLOSED**

**File:** `docs/books/laws/PCL_AI_STANDARDS.md`

The quoted text — "ANSI/EIA-748 *Earned Value Management Systems* (industry practice)" — **no longer
exists** in that file. The Drafting Manual section 6 added a *national standard* category (11) and a
*supervisory guidance* category (12) after this correction was raised, and the current Standard set
records ANSI/EIA-748 under category 11, "a national standard binding only where a contract or
procurement regime imports it", with its edition and guideline count deliberately not asserted. The
front matter of `PCL_AI_STANDARDS.md` states the reclassification expressly.

**No action outstanding in the books.** The corresponding defect *in this register* — EXT-090 and
EXT-130 carrying the same instrument under two different categories, which section 2 forbids — is fixed at
this revision: EXT-090 is aligned to *national standard*, and section 2 now defines the two added categories.

### C-05 — the Standard set's authorities sit outside every build gate — **open**

**Files:** `docs/books/laws/*.md` (113 standards), `docs/bok/domain-*.md`

Both new books' `STANDARDS.md` open with: "A standard referenced in a chapter and missing from this
register fails the build, so a reference cannot enter the corpus without being disclosed here."

That gate covers `pml-ai/manuscript/` and `pfl-ai/manuscript/` only. It does not cover the PCI
Standard files, and **PCL-AI (`docs/bok/`) has no `STANDARDS.md` at all**. Six instruments appear in
the corpus with no derived register behind them — COSO, NEC4, DAMA-DMBOK, the GAO Schedule Assessment
Guide, the DCMA 14-point assessment and the IFRS *Conceptual Framework* — plus PCL-AI's own FIDIC,
AACE, PMBOK, ANSI/EIA-748, ISO 21508, Scrum Guide and SAFe references.

*(The earlier count of nine included ISO 21508, ISO 15489 and ANSI/EIA-748, which **are** named in
manuscripts — PCL-AI D6 and PML-AI D16. The list above is corrected; the gap itself is unchanged.)*

**Proposed:** extend `_build/make_standards.py` to harvest `docs/books/laws/*.md` and
`docs/bok/domain-*.md`, emitting either a PCL-AI `STANDARDS.md` and a Standard-set register, or a
single suite-wide derived appendix that this file is checked against. A cheap first step is already
available: every element 17 in the Standard set carries this register's `EXT-` identifier, so a
harvester can check both directions mechanically.

### C-06 — `SOURCES.md` implies reliance on bodies the corpus never cites — **open**

**File:** `docs/books/registries/SOURCES.md`, **rows S-06 and S-07** (lines 23–24 at this revision)

S-06 registers "Basel/IOSCO & central-bank model-risk guidance" and S-07 "World Bank/IFC PPP
reference material". Of these, the Basel framework **is** cited — in five PFL-AI standards
(see EXT-110) — alongside SR 11-7 (from S-06) and the IFC Performance Standards (covered by S-08).
IOSCO and the World Bank are named nowhere in any manuscript or Standard file.

Both rows are marked "Proposed", so this is register hygiene rather than a false claim.

**Proposed:** narrow S-06 to "Central-bank and supervisory model-risk guidance (SR 11-7)" and either
retire S-07 or mark it "Proposed — unused at this revision", so no reader infers a multilateral
evidence base the books do not have.

### C-07 — the *supervisory guidance* category is defined and unused — **open**

**Files:** `docs/books/laws/PCI_STANDARDS_DRAFTING_MANUAL.md` section 6 (category 12);
`docs/books/laws/PCI_FOUNDATIONAL_STANDARDS.md` (SR 11-7); `docs/books/laws/PFL_AI_STANDARDS.md`
(the Basel Framework)

Category 12 exists precisely for an instrument with no legal force of its own that reaches a firm only
through its supervisor. SR 11-7 and the Basel Framework are the two clearest instances in the corpus,
and both are currently tagged **category 10, *illustrative practice***. That is not wrong — the corpus
does use them illustratively — but it puts a supervisory expectation in the same box as an example,
which is the flattening the category system exists to prevent.

**Not resolved here.** Re-categorising an instrument is a change to the Standard files, which are out
of this register's scope, and the register's own section 10 rows (EXT-102, EXT-110) already state the actual
status in words. Raised so the decision is deliberate rather than accidental.

---

*Register compiled 2026-08-03. **70 entries** — 41 independently verified against the publisher on
that date, 22 marked "not independently verified — verify current requirements" and carrying no
edition claim, 7 generic classes or negative findings with no edition to verify. **6 corrections
raised.** British English throughout. The official publication always governs.*

*Revised 2026-08-04: section 14 registers three financial-crime authorities — EXT-131 to EXT-133 — each
verified on that date, together with the two section 12 rows they add. The counts in the paragraph above
describe the 2026-08-03 compilation and have deliberately **not** been restated, because a
recompilation is a separate exercise from an addition and this register does not assert a total it
has not recounted.*

*Revised 2026-08-04 — **cross-reference repair**. Every citation in the Book locations column that
named a v1.0 identifier (`PCL-LAW-DD-NN`, `PFL-LAW-DD-NN`, `PML-LAW-DD-NN`, `PCI-LAW-F-NN`) has been
re-pointed **by subject** to the current PCI Standard that carries the reliance, or removed where none
does. All **114 stale citation instances — 46 distinct withdrawn identifiers across 50 rows** — are
resolved: **47 rows now cite the current standards that carry the reliance**, and **3 cite none**
(EXT-065, EXT-066, EXT-103), because no current standard does. **Seventeen individual withdrawn-identifier
citations were dropped rather than re-pointed** — in EXT-011, EXT-022, EXT-026, EXT-028, EXT-063,
EXT-064, EXT-065, EXT-066, EXT-081, EXT-082, EXT-083, EXT-084 and EXT-103 — and each is named, with its
reason, in the row that carried it. The mapping was
taken from the standard files themselves — every element 17 carries this register's `EXT-` identifier,
so the correspondence is machine-checkable in both directions — cross-checked against the supersession
notes in element 25 and against
[`../laws/STANDARDS_CONCORDANCE.md`](../laws/STANDARDS_CONCORDANCE.md) section 3. Book-domain citations were
re-verified against `docs/bok/domain-*.md`, `docs/books/pfl-ai/manuscript/domain-*.md` and
`docs/books/pml-ai/manuscript/domain-*.md`; four were wrong and are corrected (EXT-025, EXT-029,
EXT-064, EXT-090/EXT-103), and EXT-003's "not named anywhere in the corpus" is superseded. Section 3.6 states
the date policy and applies it; section 3.7 states why locations are given by section rather than by line.
**No verification date, edition designation or clause number was added**, and nothing in this revision
asserts a value the register did not already hold.*
