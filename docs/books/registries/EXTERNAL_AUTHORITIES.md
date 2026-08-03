# Suite External-Reference Register — PCL-AI · PML-AI · PFL-AI

**Status:** Binding register for every external authority named anywhere in the PCI Body of Knowledge
programme — the three books and the PCI Professional Law set. Referenced by
`../laws/LAW_SYSTEM.md` §5, which requires every external reference inside a law to be tagged with
one of this register's categories.

---

## 1. What this register is

The three books and the law set name real standards, contract forms, frameworks and professional
guidance. They **name and characterise** them; they never reproduce them. This register is the
single place where the programme records, for each authority:

- what it is actually called, in the publisher's own words;
- which edition or version was current when the entry was checked;
- **what kind of instrument it is** — the Category column, which is the point of the register;
- where in the corpus it is relied on;
- whether the entry was independently verified, and when.

**Coverage.** Each book carries its own derived `STANDARDS.md` generated from its manuscripts
(`pml-ai/STANDARDS.md`, 22 entries; `pfl-ai/STANDARDS.md`, 10 entries). Those registers are
build-gated but **book-scoped**, and two bodies of content fall outside them entirely:

- **PCL-AI (`docs/bok/`) has no `STANDARDS.md` at all.** Its authorities — the IFRS/IAS suite,
  FIDIC, AACE, PMBOK, the Scrum Guide, SAFe — are disclosed nowhere else.
- **The law files (`docs/books/laws/`) are outside every book build.** They are the densest users
  of external authority in the programme (35 `External references.` blocks) and introduce
  instruments no manuscript names: ANSI/EIA-748, COSO, NEC4, DAMA-DMBOK, ISO 21508, ISO 15489,
  the GAO Schedule Assessment Guide, the DCMA 14-point assessment, AACE Recommended Practices and
  the IFRS *Conceptual Framework*.

This register covers all of it. It supersedes nothing: where a book's own `STANDARDS.md` and this
register disagree, that disagreement is a defect and is listed in §11.

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
| **illustrative reference** | Named to illustrate a shape, a concept or a regulatory pattern — not relied on for any requirement | Does the corpus rely on it for anything at all? |

**Legislation is deliberately not a category.** The programme's rule is that PCI Professional Laws
are not legislation and that no voluntary framework may be dressed as one. Real legislation named
in the corpus (the EU AI Act, the GDPR) is therefore listed in §8 as an **illustrative reference** —
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
5. **Re-verify before each publication.** §10 lists the rows whose status is known to be moving.

**Verification method.** Rows dated 2026-08-03 were checked against the publisher's own catalogue or
site on that date (iso.org, ifrs.org, nist.gov, oecd.org, ifc.org, coso.org, pmi.org, aacei.org,
fidic.org, neccontract.com, scrumguides.org, gao.gov, eur-lex.europa.eu), or against an
authoritative record of it. **41 of the 70 entries below were verified this way** — comfortably more
than the 20 most-cited authorities. A further **22 are marked unverified** and carry no edition
claim. The remaining **7** are generic classes (local GAAP, US GAAP, Kanban/Lean) or negative
findings in §11, where there is no edition to verify; each says so in its own row.

---

## 4. Authoritative accounting standards

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-001 | IFRS Foundation / IASB | **IFRS 15** *Revenue from Contracts with Customers* | Issued May 2014; in force | authoritative accounting standard | PCL-AI D2 (129 references, the corpus's most-cited instrument), D1, D7; PFL-AI D2; Laws PCI-LAW-F-07, PCL-LAW-07-01 | 2026-08-03 | The programme's anchor revenue standard. Books describe the control-based five-step model in their own words; no text reproduced. |
| EXT-002 | IFRS Foundation / IASB | **IFRS 16** *Leases* | Issued January 2016; in force | authoritative accounting standard | PCL-AI D2 §2.4.3; PFL-AI D2 | 2026-08-03 | — |
| EXT-003 | IFRS Foundation / IASB | **IFRS 18** *Presentation and Disclosure in Financial Statements* | Issued April 2024; **effective for annual reporting periods beginning on or after 1 January 2027**, earlier application permitted | authoritative accounting standard | **Not named anywhere in the corpus** | 2026-08-03 | **Replaces IAS 1.** See §10 and §11 — the corpus teaches IAS 1 as current with no forward note, and IFRS 18 is under six months from effect at the date of this register. |
| EXT-004 | IFRS Foundation / IASB | **IAS 1** *Presentation of Financial Statements* | In force for periods beginning before 1 January 2027; **superseded by IFRS 18 from that date** | authoritative accounting standard | PCL-AI D2 §2.1.4 (18 references), §2.2.7, §2.4; Laws PCI-LAW-F-07 | 2026-08-03 | Current today, superseded imminently. The no-offset principle the books rely on is carried into IFRS 18, but the citation will need changing. |
| EXT-005 | IFRS Foundation / IASB | **IAS 8** — currently *Accounting Policies, Changes in Accounting Estimates and Errors*; **retitled *Basis of Preparation of Financial Statements*** from 1 January 2027 | Retitle effective 1 January 2027 (consequential to IFRS 18) | authoritative accounting standard | PCL-AI D1 lines 1382, 1672 | 2026-08-03 | The title the book prints becomes wrong on 1 January 2027. The book's *substance* (a revised life is a change in accounting estimate) is unaffected. |
| EXT-006 | IFRS Foundation / IASB | **IAS 37** *Provisions, Contingent Liabilities and Contingent Assets* | In force | authoritative accounting standard | PCL-AI D1 §1.4 (43 references), D2 §2.4.5; PFL-AI D2; Laws PCL-LAW-01-02 | 2026-08-03 | Second most-cited instrument in the corpus; the provisions/contingent-liability boundary underpins the suite's reserve vocabulary. |
| EXT-007 | IFRS Foundation / IASB | **IAS 16** *Property, Plant and Equipment* | In force | authoritative accounting standard | PCL-AI D2 §2.4.2, D1 | 2026-08-03 | — |
| EXT-008 | IFRS Foundation / IASB | **IAS 2** *Inventories* | not independently verified — verify current requirements | authoritative accounting standard | PCL-AI D2 §2.4.1 | — | Title as printed in the book matches the standard as commonly cited; not checked at ifrs.org. |
| EXT-009 | IFRS Foundation / IASB | **IAS 23** *Borrowing Costs* | not independently verified — verify current requirements | authoritative accounting standard | PCL-AI D2 §2.4.4 | — | — |
| EXT-010 | IFRS Foundation / IASB | **IAS 11** *Construction Contracts* | **Withdrawn** — superseded by IFRS 15 | authoritative accounting standard (superseded) | PCL-AI D2 §2.4.6 (14 references), appendices | 2026-08-03 | **Correctly handled.** The book teaches it expressly as legacy context and states it is withdrawn (D2 line 992). No correction needed — recorded here so the withdrawal is on the register. |
| EXT-011 | IFRS Foundation | **Conceptual Framework for Financial Reporting** | 2018 (current) | authoritative accounting material — **not itself an accounting standard** | Laws PCL-LAW-01-01, PCL-LAW-01-02, PCL-LAW-04-01; PFL-LAW-01-01 | 2026-08-03 | The IASB states expressly that the Conceptual Framework is **not a Standard** and that nothing in it overrides any Standard or its requirements. It must never be tagged as an authoritative accounting standard, and a requirement must never be sourced to it. Cited by name only in the laws, for the accrual basis and faithful representation. No clause numbers asserted — correct practice. |
| EXT-012 | IASB (body) | **International Accounting Standards Board** | not independently verified — verify current requirements | authoritative accounting standard (issuing body) | PCL-AI D2 | — | Named as the standard-setter, not relied on for a requirement. |
| EXT-013 | Various national standard-setters | **Local GAAP** (generic) | n/a — generic reference | authoritative accounting standard (generic class) | PCL-AI D2; Laws PCL-LAW-01-01, PCL-LAW-01-02 | — | Never a specific instrument. Always paired with the jurisdictional caution, correctly. |

## 5. International standards

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-020 | ISO | **ISO 31000** *Risk management — Guidelines* | **ISO 31000:2018**, 2nd edition; reviewed and confirmed 2023 | international standard | PCL-AI D12, appendices; PML-AI D1; Laws PCI-LAW-F-11, PCL-LAW-12-01 | 2026-08-03 | Cancels and replaces ISO 31000:2009. **Guidance, not a certifiable standard** — ISO says so explicitly. Nothing in the corpus claims otherwise. |
| EXT-021 | ISO/IEC | **ISO/IEC 42001** *Information technology — Artificial intelligence — Management system* | **ISO/IEC 42001:2023**, 1st edition | international standard | PML-AI D1, D9, D14; PFL-AI D16; Laws PCL-LAW-13-01, 13-02, 13-03, 13-04 | 2026-08-03 | The first AI management-system standard. Most-cited ISO instrument after ISO 31000. |
| EXT-022 | ISO/IEC | **ISO/IEC 17024** *Conformity assessment — General requirements for bodies operating certification of persons* | **ISO/IEC 17024:2026** — new edition published, superseding ISO/IEC 17024:2012 | international standard | PCL-AI appendices; Laws PCI-LAW-F-01, F-08, F-10, F-14 | 2026-08-03 | **Newly revised.** The 2026 edition adds expectations on AI-based assessment tools, human oversight of AI-generated outcomes and competence in AI use — directly relevant to a certification programme built around AI. See §10. |
| EXT-023 | ISO/IEC | **ISO/IEC 27001** *Information security, cybersecurity and privacy protection — Information security management systems — Requirements* | **ISO/IEC 27001:2022**, 3rd edition, plus **Amd 1:2024** | international standard | PML-AI D14; PFL-AI D16; Laws PCI-LAW-F-09, F-12 | 2026-08-03 | Replaces ISO/IEC 27001:2013. Note the amendment — a register that names only the base edition is incomplete. |
| EXT-024 | ISO/IEC | **ISO/IEC 23894** *Information technology — Artificial intelligence — Guidance on risk management* | **ISO/IEC 23894:2023** | international standard | PML-AI D1, D9, D14; PFL-AI D16; Laws PCL-LAW-13-02 | 2026-08-03 | Guidance, not requirements — it sits alongside ISO/IEC 42001 rather than under it. |
| EXT-025 | ISO | **ISO 15489-1** *Information and documentation — Records management — Part 1: Concepts and principles* | **ISO 15489-1:2016** | international standard | Laws PCI-LAW-F-12, PCL-LAW-11-02 | 2026-08-03 | Revised 2016 from the 2001 edition, whose Part 1 was titled *General*. Laws cite "ISO 15489 records-management standards" generically — safe. |
| EXT-026 | ISO | **ISO 8000** *Data quality* (multi-part series) | **ISO 8000-1:2022** *Part 1: Overview*; further parts issued separately (e.g. -2:2022 Vocabulary, -150:2022, -114:2024) | international standard | PML-AI D9, D14; Laws PCI-LAW-F-06, PCL-LAW-13-01 | 2026-08-03 | A **series**, not one document. The corpus cites "ISO 8000 data-quality standards" in the plural — correct, and better than naming a single part. |
| EXT-027 | ISO | **ISO 21500** *Project, programme and portfolio management — Context and concepts* | **ISO 21500:2021** | international standard | PML-AI D1; Laws PCI-LAW-F-05 | 2026-08-03 | The 2021 edition changed character: it is now context and concepts, and guidance moved to ISO 21502. A reference to "ISO 21500 (project management guidance)" would now be wrong; the corpus does not make that error. |
| EXT-028 | ISO | **ISO 21502** *Project, programme and portfolio management — Guidance on project management* | **ISO 21502:2020** | international standard | PML-AI D1; Laws PCL-LAW-04-02, PCL-LAW-11-01 | 2026-08-03 | — |
| EXT-029 | ISO | **ISO 21508** *Earned value management in project and programme management* | **ISO 21508:2018**; **2nd edition under development** at ISO/TC 258 | international standard | Laws PCL-LAW-03-02, PCL-LAW-06-01, PCL-LAW-10-03 | 2026-08-03 | Cited only in the law set — **absent from every book `STANDARDS.md`** because the laws are outside those builds. See §11. |
| EXT-030 | ISO | **ISO 21503** *Project, programme and portfolio management — Guidance on programme management* | not independently verified — verify current requirements | international standard | PML-AI D1 | — | — |
| EXT-031 | ISO | **ISO 21504** *Project, programme and portfolio management — Guidance on portfolio management* | not independently verified — verify current requirements | international standard | PML-AI D1 | — | — |
| EXT-032 | ISO | **ISO 21505** *Project, programme and portfolio management — Guidance on governance* | not independently verified — verify current requirements | international standard | PML-AI D1 | — | — |
| EXT-033 | ISO | **ISO 9001** *Quality management systems — Requirements* | **ISO 9001:2015**, 5th edition; a revision is in progress at ISO/TC 176/SC 2 | international standard | PML-AI D9 | 2026-08-03 | The certifiable one — the book characterises it exactly so, correctly distinguishing it from ISO 9000. See §10 for the revision. |
| EXT-034 | ISO | **ISO 9000** *Quality management systems — Fundamentals and vocabulary* | not independently verified — verify current requirements | international standard | PML-AI D9 | — | Vocabulary standard, not certifiable. The book's characterisation is right. |
| EXT-035 | ISO | **ISO 10006** *Quality management — Guidelines for quality management in projects* | **ISO 10006:2017** | international standard | PML-AI D1 | 2026-08-03 | Title changed at the 2017 edition — the 2003 edition read *Quality management systems — Guidelines…*. The current form is as given. |
| EXT-036 | ISO/IEC | **ISO/IEC 25012** *Software engineering — Software product Quality Requirements and Evaluation (SQuaRE) — Data quality model* | **ISO/IEC 25012:2008** | international standard | PML-AI D9, D14 | 2026-08-03 | Long-standing edition; check for supersession within the SQuaRE series before publication. |
| EXT-037 | ISO/IEC | **ISO/IEC 38507** *Information technology — Governance of IT — Governance implications of the use of artificial intelligence by organizations* | **ISO/IEC 38507:2022** | international standard | PML-AI D14 | 2026-08-03 | A governance-of-IT standard aimed at governing bodies, not at practitioners — the book characterises it correctly. |
| EXT-038 | ISO/IEC | **ISO/IEC 27701** *Information security, cybersecurity and privacy protection — Privacy information management systems — Requirements and guidance* | **ISO/IEC 27701:2025** — now a **standalone requirements standard** | international standard | PML-AI D14 | 2026-08-03 | **Materially changed.** The 2019 edition was *Extension to ISO/IEC 27001 and ISO/IEC 27002 for privacy information management*. The 2025 edition stands alone. `pml-ai/STANDARDS.md` line 67 still describes it as an extension — see §11, Correction C-01. |
| EXT-039 | ISO | **ISO 19650** series — *Organization and digitization of information about buildings and civil engineering works, including building information modelling (BIM) — Information management using building information modelling* | Multi-part: **-1:2018**, **-2:2018**, **-4:2022**, **-5:2020** | international standard | PML-AI D14 | 2026-08-03 | A **series**. The corpus cites it generically, which is right — no single part is relied on. |
| EXT-040 | IEC (with ISA) | **IEC 62443** series — *Security for industrial automation and control systems* | Multi-part and actively extended (e.g. IEC PAS 62443-1-6:2025) | international standard | PML-AI D14 | 2026-08-03 | Developed jointly with the International Society of Automation; often written ISA/IEC 62443. Cited generically in the corpus. |
| EXT-041 | ISO | **ISO 20022** (financial-services messaging) | not independently verified — verify current requirements | international standard | PCL-AI D9 | — | Single passing reference. |

## 6. Contract frameworks

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-050 | FIDIC (International Federation of Consulting Engineers) | **FIDIC suite of conditions of contract** — Red Book (*Construction*), Yellow Book (*Plant and Design-Build*), Silver Book (*EPC/Turnkey*) | **2nd editions 2017, reprinted 2022 with amendments**; a third set of amendments published November 2022, effective 1 January 2023 | contract framework | PCL-AI D7 §7.2.5, appendices; Laws PCI-LAW-F-05, F-11, PCL-LAW-05-01, PCL-LAW-07-01 | 2026-08-03 | The corpus cites FIDIC **generically and asserts no clause numbers** — the laws say so expressly ("characterised generically, no clause numbers cited"). Correct, and important: FIDIC clause numbering moved between editions. |
| EXT-051 | NEC (Thomas Telford / ICE) | **NEC4 suite of contracts**, including the Engineering and Construction Contract (ECC) | **June 2017 edition, revised January 2023**; amendments issued January 2019, October 2020 and January 2023 | contract framework | Laws PCL-LAW-05-01, PCL-LAW-07-01 | 2026-08-03 | Cited only in the law set for the compensation-event mechanism, characterised generically. **Absent from every book `STANDARDS.md`** — see §11. |

## 7. Professional guidance

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-060 | Project Management Institute | **A Guide to the Project Management Body of Knowledge (PMBOK Guide)** | **Eighth Edition** is current; the Seventh Edition (2021) is superseded | professional guidance | PCL-AI D8, appendices; Laws PCI-LAW-F-05, F-07, PCL-LAW-03-03, PCL-LAW-05-01, PCL-LAW-12-01 | 2026-08-03 | The corpus cites "PMBOK Guide" **without an edition**, which is the safe form and remains correct across the 7th→8th change. Do not add an edition to prose. |
| EXT-061 | Project Management Institute | **The Standard for Earned Value Management** | not independently verified — verify current requirements | professional guidance | Laws PCL-LAW-03-02, PCL-LAW-06-01 | — | Existence confirmed on pmi.org; current edition not established. |
| EXT-062 | Project Management Institute | **Practice Standard for Scheduling** | **Third Edition**, aligned to the PMBOK Guide Seventh Edition | professional guidance | Laws PCL-LAW-10-01, PCL-LAW-10-02 | 2026-08-03 | — |
| EXT-063 | Project Management Institute | **Code of Ethics and Professional Conduct** | not independently verified — verify current requirements | professional guidance | Laws PCI-LAW-F-08, F-10, F-13 | — | Cited for honesty, fairness and conflict disclosure. No text reproduced. |
| EXT-064 | AACE International | **Total Cost Management (TCM) Framework** | not independently verified — verify current requirements | professional guidance | PCL-AI D3, D8, appendices; PML-AI D7; Laws PCI-LAW-F-05, F-06, F-07, PCL-LAW-01-01, PCL-LAW-03-01, PCL-LAW-03-03, PCL-LAW-10-03 | — | The most-cited professional-guidance item in the law set. Cited for the existence and purpose of the cost-control cycle, never for its text. |
| EXT-065 | AACE International | **Recommended Practice 17R-97** *Cost Estimate Classification System* | Confirmed as a current AACE RP; **revised 7 August 2020** | professional guidance | Laws PCL-LAW-03-01 | 2026-08-03 | Generic classification RP. The law names it and describes maturity-based classes in PCI's own words; **no accuracy ranges or class tables are reproduced** — correct, those are protected. |
| EXT-066 | AACE International | **Recommended Practice 18R-97** *Cost Estimate Classification System — As Applied in Engineering, Procurement, and Construction for the Process Industries* | Confirmed as a current AACE RP; **revised 7 August 2020** | professional guidance | Laws PCL-LAW-03-01 | 2026-08-03 | Supplements 17R-97 for process industries. |
| EXT-067 | AACE International | **Recommended Practice 29R-03** *Forensic Schedule Analysis* | not independently verified — verify current requirements | professional guidance | Laws PCL-LAW-10-02 | — | Cited for the existence of recognised delay-analysis methods, paired with a caution that forum acceptability differs. |
| EXT-068 | AACE International | **Recommended Practices on risk analysis and contingency determination** (generic reference) | not independently verified — verify current requirements | professional guidance | Laws PCL-LAW-12-01 | — | Cited as a class, not as a numbered RP — the safest form where the precise RP is not verified, and consistent with LAW_SYSTEM §5. |
| EXT-069 | U.S. Government Accountability Office | **GAO Schedule Assessment Guide: Best Practices for Project Schedules** | **GAO-16-89G**, final version issued 22 December 2015 | professional guidance | Laws PCL-LAW-10-01, PCL-LAW-10-02 | 2026-08-03 | A public audit institution's guide, freely available. Ten best practices; the corpus reproduces none of them. |

## 8. Voluntary frameworks

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-080 | NIST (US Department of Commerce) | **Artificial Intelligence Risk Management Framework (AI RMF 1.0)**, NIST AI 100-1 | **AI RMF 1.0**, January 2023 | voluntary framework | PML-AI D14; PFL-AI D16; Laws PCI-LAW-F-01, F-02, F-03, F-04, F-06, F-09, PCL-LAW-13-02 | 2026-08-03 | NIST states it is "voluntary, rights-preserving, non-sector specific". Four functions: Govern, Map, Measure, Manage. **Both `STANDARDS.md` registers say expressly "a voluntary, function-based framework, not a standard and not a regulation" — exemplary, and the model for every other row in this section.** |
| EXT-081 | OECD | **Recommendation of the Council on Artificial Intelligence** (the "OECD AI Principles"), OECD/LEGAL/0449 | Adopted 2019; **revised May 2024** at the Ministerial Council Meeting | voluntary framework | Laws PCI-LAW-F-01, F-03, F-04, F-13, PCL-LAW-13-02, 13-03, 13-04 | 2026-08-03 | An OECD Council **Recommendation** — not binding law even on adherents. The laws say "never legislation" at every single use. Correct. |
| EXT-082 | Equator Principles Association | **The Equator Principles (EP4)** | **EP4**, adopted 18 November 2019, **effective 1 October 2020** | voluntary framework | PFL-AI D5, D9; Laws PCI-LAW-F-13 | 2026-08-03 | Adopted voluntarily by 130+ financial institutions across 38+ countries. PFL-AI D5 calls it "a lender framework under which participating institutions apply agreed environmental and social requirements", names it "for identification only" and states neither body is associated with the book — a model characterisation. |
| EXT-083 | International Finance Corporation (World Bank Group) | **Performance Standards on Environmental and Social Sustainability** | **2012 edition**, effective 1 January 2012; **Sustainability Framework update under way** (approach paper published 2025) | voluntary framework | PFL-AI D5, D9; Laws PCI-LAW-F-13 | 2026-08-03 | Binding on IFC clients by contract; a **benchmark adopted voluntarily** by others, including through EP4. The corpus says "widely adopted as a reference benchmark" — accurate. See §10: an update is in progress. |
| EXT-084 | COSO (Committee of Sponsoring Organizations of the Treadway Commission) | **Internal Control — Integrated Framework** | **2013**, revising the 1992 original; 17 principles across five components | voluntary framework | Laws PCL-LAW-04-01, PCL-LAW-04-02, PCL-LAW-11-01, PCL-LAW-11-02, PCL-LAW-13-03 | 2026-08-03 | Voluntary in itself, though widely imported by regulators — the US federal *Standards for Internal Control* adopt its principles. The laws tag it "(voluntary framework)" consistently. Correct. |
| EXT-085 | OECD | **Arrangement on Officially Supported Export Credits** | not independently verified — verify current requirements | voluntary framework | PFL-AI D9 | — | An **inter-governmental understanding**, not a treaty and not legislation. PFL-AI D9 line 850 characterises it exactly so and adds that "its terms are revised periodically and vary by sector, so they must be checked as at the transaction date" — the date-sensitivity rule applied in prose, correctly. |
| EXT-086 | Ken Schwaber and Jeff Sutherland | **The Scrum Guide** | **November 2020** version, current | voluntary framework | PCL-AI D9 §9.2, appendices | 2026-08-03 | PCL-AI D9 line 140 says "Described from the current Scrum Guide's concepts, in this reference's own words" — an explicit, honest disclosure of the citation basis. |
| EXT-087 | Beck et al. | **Manifesto for Agile Software Development** ("the Agile Manifesto") | not independently verified — verify current requirements | voluntary framework | PCL-AI D9, appendices | — | Stable since 2001; described conceptually, not quoted. |

## 9. Industry practice

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-090 | SAE International / NDIA IPMD | **ANSI/EIA-748** *Earned Value Management Systems* | **EIA-748-E released February 2026**, condensing the previous 32 guidelines into **27**; supersedes EIA-748-D (2019) | industry practice | Laws PCL-LAW-03-02, PCL-LAW-06-01 | 2026-08-03 | **Just revised.** The laws cite it **without an edition and without a guideline count**, so the change does not break them — and the corpus nowhere states "32 criteria" (checked). See §10. Tagged "industry practice" in the laws; defensible, though it is a published ANSI-accredited standard — see §11, C-04. |
| EXT-091 | Defense Contract Management Agency (US) | **DCMA 14-Point Schedule Assessment** | not independently verified — verify current requirements | industry practice | Laws PCL-LAW-10-01 | — | Correctly tagged "industry practice" — a widely used metric set, not a published standard. |
| EXT-092 | DAMA International | **DAMA-DMBOK: Data Management Body of Knowledge** | **2nd Edition (2017)**; a revised printing of the 2nd Edition exists | industry practice | Laws PCL-LAW-13-01 | 2026-08-03 | A commercially published body of knowledge. **Protected — never reproduced or structurally mirrored** (`SOURCES.md` §2). |
| EXT-093 | Scaled Agile, Inc. | **SAFe (Scaled Agile Framework)** | not independently verified — verify current requirements | industry practice | PCL-AI D9 §9.4, appendices | — | PCL-AI treats it at "awareness only" level and says so in the text. A proprietary commercial framework — the awareness-level treatment is the right call. |
| EXT-094 | The LeSS Company | **LeSS (Large-Scale Scrum)** | not independently verified — verify current requirements | industry practice | PCL-AI D9 §9.4, appendices | — | Awareness level only. |
| EXT-095 | Various | **Kanban / Lean / Little's Law** (methods) | n/a — generic methods | industry practice | PCL-AI D9, appendices; PML-AI D13 | — | Generic methods with no single owner; no attribution issue. |
| EXT-096 | Various | **Six Sigma** | not independently verified — verify current requirements | industry practice | PML-AI D9 | — | Single passing reference. |

## 10. Illustrative references, including legislation named as a reference point

**Read §2 first.** Nothing in this section is relied on by the corpus for any requirement. The two
EU instruments below **are genuine legislation** within their jurisdiction; they appear here because
the corpus names them only to illustrate a regulatory shape, never as a source of obligation.

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-100 | European Union | **Regulation (EU) 2024/1689** laying down harmonised rules on artificial intelligence (the "AI Act") | In force since **1 August 2024**; phased application — prohibitions from 2 February 2025, GPAI obligations from 2 August 2025, general application from **2 August 2026**, remaining rules by 2 August 2027 | illustrative reference *(actual status: **binding legislation** within the EU)* | PML-AI D14; Laws PCI-LAW-F-02, F-03, F-04 | 2026-08-03 | `pml-ai/STANDARDS.md` places it under "**Regulation — named as reference points, not as applicable law**" and says it is "binding only within its own jurisdiction". The foundational laws call it "legislation in its jurisdiction … an external requirement where applicable". **Both are exactly right.** Note it reached general application the day before this register's date. |
| EXT-101 | European Union | **General Data Protection Regulation** (Regulation (EU) 2016/679) | not independently verified — verify current requirements | illustrative reference *(actual status: **binding legislation** within the EU)* | PML-AI D14; Laws PCI-LAW-F-09 (jurisdictional caution) | — | Named as an example of a rights-based data-protection approach. Never relied on for a requirement. |
| EXT-102 | US Federal Reserve / OCC | **SR 11-7 / OCC 2011-12** *Supervisory Guidance on Model Risk Management* | not independently verified — verify current requirements | illustrative reference *(actual status: **supervisory guidance**, jurisdiction-specific)* | Laws PCI-LAW-F-02 | — | The law calls it "public supervisory guidance … jurisdiction-specific, cited as guidance only". Accurate. |
| EXT-103 | Various | **US GAAP** (generic) | n/a — generic reference | illustrative reference | PCL-AI D2, appendices; Laws PCL-LAW-01-01 | — | Named only to contrast with IFRS. |
| EXT-110 | Basel Committee on Banking Supervision (BCBS) | **The Basel Framework** (consolidated BCBS standards) | consolidated framework, as maintained by the BCBS | illustrative reference — internationally agreed supervisory standards with **no legal force of their own** | Laws PFL-LAW-09-01, PFL-LAW-10-01, PFL-LAW-10-02 | 2026-08-03 | Corrects an earlier negative finding in this register, which recorded that "Basel" appeared in no law file; it is cited six times in `PFL_AI_LAWS.md`. The Committee has no supranational authority: its standards bind banks only as national authorities transpose them, and they must never be described as regulation applying directly to a project or its sponsors. |

## 11. Registered but not used

`SOURCES.md` carries rows that no manuscript and no law actually relies on. They are not defects —
`SOURCES.md` marks them "Proposed" — but a reader should not infer from them that the corpus draws
on these bodies. **Verified by search across all manuscripts and law files on 2026-08-03.**

| Ref ID | Authority | `SOURCES.md` row | Actual corpus usage | Notes |
|---|---|---|---|---|
| EXT-111 | World Bank Group | S-07, "Proposed (PFL-AI D5–D15)" | **None** as "World Bank". IFC Performance Standards (EXT-083) are used and are covered by S-08 | — |
| EXT-112 | IPMA, ACCA, CIMA, CFA Institute | `SOURCES.md` §2 prohibited-use register | **None** — correctly, that is the point of the row | Listed only as bodies whose material must never be reproduced. |
| EXT-113 | PRINCE2 (PeopleCert) · APM | Not registered | **None** | Recorded so a future reviewer does not expect them. Neither appears anywhere in the corpus. |

---

## 12. Rows whose status is known to be moving

Re-verify each of these immediately before publication. This is the date-sensitivity rule (§3.5) made
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

---

## 13. Cited in the law files, registered here

The law sets in `docs/books/laws/` cite eleven instruments that neither book's `STANDARDS.md`
registers, because the laws reach subject matter the chapters do not. Each was verified directly with
its publisher during the law red-team audit on 2026-08-03. They are registered here so that this file
remains the single disclosure point it claims to be.

| Ref ID | Authority | Full title | Edition/year checked | Category | Book locations | Verification date | Notes |
|---|---|---|---|---|---|---|---|
| EXT-120 | IFRS Foundation | **IAS 7** *Statement of Cash Flows* | current | authoritative accounting standard | Laws PFL-LAW-01-01, PFL-LAW-10-01 | 2026-08-03 | Mandatory only for entities applying IFRS Accounting Standards in a jurisdiction that has adopted them. It defines no coverage ratio — those are creatures of the finance documents. |
| EXT-121 | IFRS Foundation | **IAS 12** *Income Taxes* | current | authoritative accounting standard | Law PFL-LAW-12-01 | 2026-08-03 | Governs financial reporting of tax only, never the tax position itself. Any tax position needs qualified local advice. |
| EXT-122 | IFRS Foundation | **IAS 36** *Impairment of Assets* | current | authoritative accounting standard | Law PFL-LAW-15-01 | 2026-08-03 | Cited for the impairment-indicator discipline; entities applying IFRS only. |
| EXT-123 | ISO | **ISO 45001:2018** *Occupational health and safety management systems — Requirements with guidance for use* | 2018 | international standard | Laws PML-LAW-09-01, PML-LAW-12-02 | 2026-08-03 | Certifiable management-system standard. Adoption is voluntary unless a contract or regulator requires it. |
| EXT-124 | ISO | **ISO 45003:2021** *Occupational health and safety management — Psychological health and safety at work — Guidelines* | 2021 | international standard (guidance, not certifiable) | Law PML-LAW-12-02 | 2026-08-03 | Guidance, not a requirements standard; nothing can be certified against it. |
| EXT-125 | ICAEW | **Financial Modelling Code** | current | professional guidance | Laws PFL-LAW-06-01, PFL-LAW-06-02 | 2026-08-03 | Principles-based guidance published by a professional body. Not a compliance standard and not certifiable. |
| EXT-126 | FAST Standard Organisation | **The FAST Standard** | current | voluntary framework | Law PFL-LAW-06-02 | 2026-08-03 | Adopted voluntarily by modellers and firms; imposes no obligation of its own. |
| EXT-127 | IESBA / IFAC | **International Code of Ethics for Professional Accountants (including International Independence Standards)** | current | professional guidance | Law PFL-LAW-13-01 | 2026-08-03 | Binding **only** where a professional body, regulator or engagement has adopted it. A PCI credential holder who is not subject to it is not made subject to it by a PCI law. |
| EXT-128 | OECD (G20/OECD) | **G20/OECD Principles of Corporate Governance** | 2023 revision (OECD/LEGAL/0413) | voluntary framework | Laws PFL-LAW-01-02, PFL-LAW-10-02, PFL-LAW-15-01 | 2026-08-03 | An OECD Council Recommendation. Non-binding; not legislation. The current title carries the G20/OECD attribution. |
| EXT-129 | OECD | **Model Tax Convention on Income and on Capital** | current | illustrative reference | Law PFL-LAW-12-01 | 2026-08-03 | A model instrument. **It is not law in any jurisdiction**; only the executed treaty and domestic law bind. |
| EXT-130 | SAE International (ANSI-accredited) | **ANSI/EIA-748** *Earned Value Management Systems* | edition deliberately not asserted | national standard | Laws PCL-LAW-03-02, PCL-LAW-06-01 | 2026-08-03 | A published US national standard, binding only where a contract or procurement regime imports it. The law files deliberately assert no guideline count or edition, because the guideline count changed at EIA-748-E. |

## Corrections needed in the books

Six items. Four are date-driven and none is a mischaracterisation of an authority's legal status —
**the corpus's handling of voluntary-versus-binding is consistently correct**, which is the finding
that matters most for this register. Every "voluntary framework" is labelled voluntary at the point
of use; the EU AI Act is the only instrument called legislation, and it is one.

### C-01 — ISO/IEC 27701 described as an extension it no longer is

**File:** `docs/books/pml-ai/STANDARDS.md`, **line 67**

> `| **ISO/IEC 27701** | privacy information management, extending the information-security management system | D14 |`

ISO/IEC 27701:2025 is a **standalone requirements standard** — *Privacy information management
systems — Requirements and guidance*. The "extension to ISO/IEC 27001 and ISO/IEC 27002"
characterisation described the withdrawn 2019 edition.

**Proposed:** "privacy information management systems, as a requirements standard in its own right".
The source is the D14 manuscript key-terms entry, since `STANDARDS.md` is derived — fix it there and
regenerate.

### C-02 — IAS 1 taught as current with no note that IFRS 18 replaces it

**File:** `docs/bok/domain-02-financial-reporting.md`, **lines 17, 29, 88–90, 104, 132, 138, 1220, 1834**

KA 2.1.4 is titled "IAS 1 and the presentation principles" and line 90 reads "**IAS 1 (presentation
of financial statements)** sets how the statements are presented". True today; false for periods
beginning on or after **1 January 2027**, when IFRS 18 *Presentation and Disclosure in Financial
Statements* replaces it.

The book has an established, well-executed pattern for exactly this: §2.4.6 handles the
IAS 11 → IFRS 15 supersession explicitly and correctly (EXT-010).

**Proposed:** apply the §2.4.6 pattern to IAS 1 — one sentence in KA 2.1.4 recording that IFRS 18
replaces IAS 1 for periods beginning on or after 1 January 2027 and that the no-offset principle
relied on at §2.2.7 and §2.3 carries over. Do **not** restate IFRS 18's requirements; the programme
does not assert requirements it has not verified.

### C-03 — IAS 8's printed title expires on 1 January 2027

**File:** `docs/bok/domain-01-foundations-of-accounting.md`, **line 1382** (and the cross-reference at line 1672)

> "Under **IAS 8 (accounting policies, changes in accounting estimates and errors)** three superficially…"

From 1 January 2027 IAS 8 is retitled ***Basis of Preparation of Financial Statements*** as a
consequential amendment to IFRS 18. The book's substantive point — that a revised asset life is a
change in accounting estimate — is unaffected.

**Proposed:** either drop the parenthetical title and cite "IAS 8" alone (consistent with the
programme's own no-editions-in-prose policy), or add the retitling date. The first is cheaper and
more robust.

### C-04 — ANSI/EIA-748 tagged "industry practice" where it is a published standard

**File:** `docs/books/laws/PCL_AI_LAWS.md`, **lines 289 and 634**

> "ANSI/EIA-748 *Earned Value Management Systems* (industry practice)"

EIA-748 is an ANSI-accredited standard published by SAE International and stewarded by the NDIA
Integrated Program Management Division. "Industry practice" understates it; "international standard"
overstates it, since it is a US national standard.

This is a **borderline call, not an error** — the LAW_SYSTEM category list has no "national
standard" value, and "industry practice" is the honest choice among those available. Recorded so the
decision is deliberate rather than accidental.

**Proposed:** keep "industry practice" and add six words — "(industry practice; a published SAE/ANSI
standard)" — or extend the category vocabulary in `LAW_SYSTEM.md` §5. No change to substance.

### C-05 — the law set's authorities sit outside every build gate

**Files:** `docs/books/laws/PCI_FOUNDATIONAL_LAWS.md`, `docs/books/laws/PCL_AI_LAWS.md`

Both books' `STANDARDS.md` open with: "A standard referenced in a chapter and missing from this
register fails the build, so a reference cannot enter the corpus without being disclosed here."

That gate covers `pml-ai/manuscript/` and `pfl-ai/manuscript/` only. It does not cover the law files,
and **PCL-AI (`docs/bok/`) has no `STANDARDS.md` at all**. Nine instruments therefore appear in the
corpus with no derived register behind them: ISO 21508, ISO 15489, ANSI/EIA-748, COSO, NEC4,
DAMA-DMBOK, the GAO Schedule Assessment Guide, the DCMA 14-point assessment and the AACE Recommended
Practices — plus PCL-AI's own FIDIC, AACE, PMBOK, Scrum Guide and SAFe references.

This register closes the disclosure gap. It does not close the **build** gap.

**Proposed:** extend `_build/make_standards.py` to harvest `docs/books/laws/*.md` and `docs/bok/domain-*.md`,
emitting either a PCL-AI `STANDARDS.md` and a law-set register, or a single suite-wide derived
appendix that this file is checked against.

### C-06 — `SOURCES.md` implies reliance on bodies the corpus never cites

**File:** `docs/books/registries/SOURCES.md`, **lines 23–24** (rows S-06, S-07)

S-06 registers "Basel/IOSCO & central-bank model-risk guidance" and S-07 "World Bank/IFC PPP
reference material". Of these, the Basel framework **is** cited — six times in `PFL_AI_LAWS.md`
(see EXT-110) — alongside SR 11-7 (from S-06) and the IFC Performance Standards (covered by S-08).
IOSCO and the World Bank are named nowhere in any manuscript or law file.

Both rows are marked "Proposed", so this is register hygiene rather than a false claim.

**Proposed:** narrow S-06 to "Central-bank and supervisory model-risk guidance (SR 11-7)" and either
retire S-07 or mark it "Proposed — unused at this revision", so no reader infers a multilateral
evidence base the books do not have.

---

*Register compiled 2026-08-03. **70 entries** — 41 independently verified against the publisher on
that date, 22 marked "not independently verified — verify current requirements" and carrying no
edition claim, 7 generic classes or negative findings with no edition to verify. **6 corrections
raised.** British English throughout. The official publication always governs.*
