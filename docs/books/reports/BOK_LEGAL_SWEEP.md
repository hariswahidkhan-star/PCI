# Legal-risk sweep of the three Body of Knowledge volumes

**Date:** 4 August 2026
**Scope:** `docs/bok/` (PCL-AI), `docs/books/pml-ai/`, `docs/books/pfl-ai/` — manuscripts,
appendices, glossaries, capstones, question banks, and the two generator scripts that derive
`STANDARDS.md`, `GLOSSARY.md` and `QUESTION_BANK.md` from the manuscripts.
**Out of scope, untouched:** `docs/books/laws/` (owned by another agent).
**Instruction applied:** maximum risk aversion — where there was doubt, the passage was changed.
**Verification:** `docs/books/_build/verify_formulas.py` — `✓ all golden answers verified`, 0
failures, before and after. No number, worked example or golden answer was altered.

---

## 1. Method

Targeted search rather than full reading. Patterns swept across all in-scope Markdown:

`is an offence` · `criminal` · `unlawful` · `illegal` · `prohibited by law` · `the law requires` ·
`legally required` · `statutory duty` · `must by law` · `required by law` · `shall be liable` ·
`liable` · `prosecut` · `penalt` · `whistleblow` · `tipping off` · `mandatory report` ·
`sanction` · `money launder` · `bribery` · `facilitation payment` · `(IFRS|IAS|ISO|NEC|FIDIC|
PMBOK|GDPR|AI Act|SOX)\s.*\b(requires|mandates|prohibits|permits|obliges)\b` · `Article \d` ·
`s\.\d` · `§\s*\d` · `paragraph \d+ of` · `Clause \d+(\.\d+)+ of` · `Reg \(EU\) \d{4}/\d+` ·
`:20\d\d` (edition years) · insolvency / securities / employment / tax / data-protection
vocabulary · `accredit|endorse|affiliat|approved by|recognised by` (implied authority).

Every hit was read in context before a decision. Three volumes, 49 manuscript files plus
appendices and derived files.

**Note on derived files.** `QUESTION_BANK.md`, `GLOSSARY.md` and `STANDARDS.md` in both
`pml-ai/` and `pfl-ai/` are generated from the manuscripts and from `_build/make_standards.py`.
Manuscript edits were made first, then the three generators re-run. Their diffs contain nothing
beyond the intended changes (8, 8, 6 and 2 lines respectively) and all three report zero defects.

---

## 2. Changes by volume

### 2.1 PFL-AI — Domain 1, KA 1.3.2 (financial crime) — the largest concentration

`books/pfl-ai/manuscript/domain-01-foundations.md`. This topic was the single highest-risk
passage in the corpus (see §4). It asserted criminal liability as fact, framed the four
families of conduct as offences, and named prosecution as a consequence. Every professional
control in it was preserved; only the characterisation changed.

| # | Nature | Was | Now |
|---|---|---|---|
| 1 | Legal obligation (§1) | Domain opener: "the financial-crime duties **whose breach is criminal**" | "the financial-crime duties **the profession treats as absolute**" |
| 2 | Legal obligation (§1) | KA 1.3.1 lattice: "**statutory duties** (companies law, anti-bribery…)" | "**duties arising under law**" + which of them reach a given person/entity/transaction varies by jurisdiction and is a question for qualified counsel |
| 3 | Criminality (§1) | "this is the one whose breach **is a criminal offence** — for the organisation and, in many regimes, for the individual…" | "this is the one **the profession treats as absolute** — no commercial justification reopens it, and the professional standard applies to the organisation and to the individual…" |
| 4 | Named legal consequence (§1) | "priced in **prosecutions**, debarment from public procurement…" | "priced in consequences of a different order — the loss of standing to bid public work, an event of default… — and, in many jurisdictions, in the attention of authorities whose powers a leader should assume are severe and should never try to gauge unaided" |
| 5 | Statement of law (§1) | Heading "**The prohibited acts**… stated as a leader needs to recognise them rather than as any statute frames them" | "**The conduct a leader must recognise**…" + explicit "This is a professional taxonomy, not a statement of the law of any jurisdiction: what conduct is caught, by whom and where, is for qualified counsel" |
| 6 | Offence definition (§1) | "**Bribery** *is* offering… Both sides of the transaction **are caught**… the offer completes the act" | "**Bribery**, *as this book uses the term*, is offering… The professional standard treats both sides alike… *for professional purposes* the offer completes the act" |
| 7 | Offence definition (§1) | "Bribery of a foreign public official **is a distinct and usually wider offence**… the improper-performance element **may not be required at all**" | "is **treated as** a distinct and usually stricter **category**… a leader **should not assume** that an absence of improper performance puts an advantage outside the category" |
| 8 | Statement of law (§1) | "**A facilitation payment is a bribe in most regimes**… **is prohibited under most anti-bribery regimes**" | "**Treat a facilitation payment as a bribe**… falls inside **the professional prohibition in this book**… Anti-bribery regimes differ" (the written-counsel-advice-before-payment rule preserved unchanged) |
| 9 | Offence + defence (§1) | "Several regimes **make it an offence**… **not knowing what the intermediary did is not a defence**… **Where a defence exists at all it is a procedures defence**" | "Several jurisdictions **are understood to operate regimes** under which an organisation answers…; the professional lesson **which holds whatever the local position turns out to be** is that a leader **should not assume** that not knowing will protect the organisation… Where such regimes admit a defence it is **generally described as** a procedures defence… **Whether any such regime applies… is a question for qualified counsel**" |
| 10 | Offence (§1) | "**tipping off** — **is itself an offence in many regimes**" | "is treated by this book as a **serious professional failure in its own right**, and restrictions on it **are understood to exist** in many jurisdictions, though their scope and consequences vary and are a matter for qualified counsel" |
| 11 | Offence (§1) | Escalation bullet: "alerting the subject **may be a criminal offence in its own right**" | "may expose the individual and the organisation to consequences of a kind this book does not attempt to describe — which is precisely why the judgement is not the leader's to make" |
| 12 | Named legal consequence (§1) | "what a lender's reviewer, an auditor or a **prosecutor** can be shown" | "…or an **investigator** can be shown" |
| 13 | **Data protection (§4) — ADDED** | Evidence list instructed recording beneficial owners, PEPs, named intermediaries and concerns about individuals, with no data-protection caution | New paragraph before the list: such records are typically personal data attracting accuracy / proportionality / retention / subject-access obligations that vary and need the data-protection function and qualified counsel **before the file is built**; keeping the record remains non-optional |
| 14 | Legal effect of instruments (§2) | OECD Convention "its obligations **bind** the states Party… **takes effect only through** each signatory's own **domestic criminal law**… **not itself law in any jurisdiction**"; FATF "**not legislation anywhere and confer no obligation directly**"; ISO 37001 "certification **is not a legal defence**" | Treaty "**addressed to** the states Party rather than to a project company; a leader should not read it as applying directly, and should **ask counsel what does**"; FATF "addressed to countries, reaching a project in practice through the supervised institutions"; ISO 37001 "**should never be offered or accepted as a defence**" |
| 15 | **Non-affiliation (§5) — STRENGTHENED** | "none of the issuing bodies is associated with this book" | "…no description of any of them here is authoritative, and none of the issuing bodies **is associated with, endorses or has reviewed** this book" |
| 16 | Statement of law (§1) | Standing caution "**Which offence applies**…" | "**Which regimes apply**…" + added "nothing in it is authoritative about any regime it mentions" (whole caution otherwise preserved verbatim) |
| 17 | Offence (§1) | Walkthrough: "the second **may be an offence**"; "Where a failure-to-prevent **offence** applies"; "whether **any offence is engaged**"; "whether a report **is mandatory** and to whom" | "the second **is the failure this topic warns about most sharply**"; "failure-to-prevent **regime**"; "whether **any such regime** is engaged"; "whether **any report is expected of it**, and to whom" |
| 18 | Offence (§1) | Key-terms table: "Facilitation payment … a bribe **in most regimes**"; "**Failure-to-prevent offence** — An organisation's **offence** of failing to prevent…"; "Associated person … through whom its **criminal exposure** runs"; "Tipping off … **an offence in many regimes**" | Reframed to professional standard + "understood to exist in several jurisdictions" + "Local application is a question for counsel"; "criminal exposure" → "exposure"; "**a serious professional failure, restricted in many jurisdictions**" |
| 19 | Offence (§1) | MCQ 1.3-I rationale "does nothing about **an offence**"; MCQ 1.3-J rationale "**may be a criminal offence in its own right**" | "does nothing about **the underlying conduct**"; "may carry consequences… **that are not the accountant's to weigh**" |
| 20 | Legal defence (§3) | MCQ 1.3-K option A "…**is a defence to the offence**"; correct option B "it **is not a legal defence**" | A "…**answers the concern**"; B "it **should never be relied on as a defence**" (answer key unchanged, distractor still wrong for the same reason) |
| 21 | Offence (§1) | Self-check 8; domain summary "the only ones in the lattice **whose breach is criminal** and where a facilitation payment **is a bribe**"; exam-pitfall "as a defence **to an offence**" | "failure-to-prevent **regime**"; "the only ones the profession **treats as admitting no commercial justification**, where a facilitation payment **is treated as** a bribe, an intermediary's conduct **is assumed to be** the organisation's exposure"; "as a defence" |

### 2.2 PFL-AI — other domains

| File | Nature | Change |
|---|---|---|
| `domain-02-accounting-foundations.md` (distributable reserves) | Lawfulness of a company's act (§3) | "what the consequences of an **unlawful distribution** are for the directors who declared it" → "what follows for the directors who declared **a distribution later challenged**"; added "on which nothing here should be relied upon". The surrounding "this is a legal question before it is an accounting one… matters for counsel in the jurisdiction of incorporation" was already present and is preserved. |
| `STANDARDS.md` (derived) | §2, §5 | Regenerated from the corrected `make_standards.py` characterisations of the OECD Convention, FATF and ISO 37001. |
| `GLOSSARY.md`, `QUESTION_BANK.md` (derived) | §1, §3 | Regenerated; carry the Domain 1 corrections above. |

### 2.3 PML-AI

| File | Nature | Change |
|---|---|---|
| `domain-01-profession.md` | Lawfulness of an instruction (§3) | "It does not apply where **the instruction is unlawful**" → "where **following the instruction would require the leader to act outside the law as they understand it**". The following sentences — that protections, protected-disclosure routes and any external-reporting obligation are jurisdiction- and sector-specific and that advice must be taken first — were already exemplary and are preserved verbatim. |
| `domain-01-profession.md` (×3) | Statement of legal obligation (§1) | "Where a **statutory duty**, an operating licence or patient safety is engaged" → "Where a **duty arising under law or regulation**…" in all three places (KA 1.4.3 limit, self-check 6, and the verification-card rule). The substantive point — that expected-value arithmetic must never decide such a case — is untouched. |
| `domain-10-procurement-contracts-supply.md` | Statement of law (§1) | Treating a screening hit as a finding is "both unfair and, in some jurisdictions, **unlawful**" → "…**restricted — take advice before any such inference is acted on**". Also "a **sanctions** or adverse-media hit" → "a **screening** or adverse-media hit". |
| `domain-11-stakeholders-communication-influence.md` | Legal obligation (§1) | Omitted stakeholders "whose consent **may be legally required**" → "whose consent **may have to be obtained under obligations that vary by jurisdiction**". |
| `domain-12-leadership-teams-behaviour.md` (×2) | Statement of law (§1, §6 employment) | Behavioural-telemetry inference "frequently **unlawful** depending on jurisdiction" → "**restricted or prohibited in many jurisdictions — a question for qualified advice before any such tool is even considered**"; individual-level communication analysis "restricted **or unlawful** in many jurisdictions" → "restricted in many jurisdictions". Both prohibitions themselves are preserved in full. The domain's existing "**nothing in this domain should be read as advice on any jurisdiction's employment law**" is preserved. |
| `domain-14-digital-data-responsible-ai.md` | Named legal consequence (§1) | "where the consequence is a **statutory penalty**, a licence condition…" → "a **penalty imposed by an authority**…". |
| `domain-16-transition-closeout-benefits.md` (×4) | Legal obligation (§1) | Same statutory-penalty correction; "**statutory notifications made**" → "**notifications required of the organisation made**" (3 occurrences, gate block + 2 summaries); "the **statutory** limitation periods on latent defects" → "the limitation periods…"; "closure reporting is frequently **statutory**" → "frequently **required**", "retention period is **set by public-records legislation**" → "**commonly set by public-records requirements**". The domain's own "Nothing in this domain states a legal position" and "nothing here states a legal minimum or maximum" are preserved. |
| `STANDARDS.md` (derived) | §2 edition citation | ISO/IEC 27701 entry cited "**The 2019 edition**… **the 2025 edition**…" — in direct contradiction of the register's own stated policy that no entry carries an edition year. Rewritten to "Its relationship to an ISO/IEC 27001 management system, and whether it can be certified in its own right, **differ between editions**; establish which edition any claim of conformity refers to before relying on it." Fixed at source in `_build/make_standards.py`. |

### 2.4 PCL-AI (`docs/bok/`)

| File | Nature | Change |
|---|---|---|
| `00-conventions.md` §9 | §1, §2, §5, §6 — **ADDED** | Three additions to the citation-practice section: (a) explicit "no clause, article, paragraph or edition is cited as authority anywhere in this book"; (b) a new **Named, never relied upon** bullet with a volume-level non-affiliation statement — no standards body, regulator, government, professional association or issuing body named anywhere is associated with, has accredited, endorses, has approved or has reviewed the book, the designation or the programme; (c) a new **Not legal, tax or accounting advice** bullet covering law, regulation, tax, employment, insolvency, securities, sanctions and data protection, referring decisions to qualified counsel and advisers. The worked-principle example was also updated from "under IAS 37 a provision is recognised when…" to "the principle IAS 37 addresses is…". |
| `domain-01-foundations-of-accounting.md` (×3) | §2 standard-requires | "IAS 37 **does not permit** a provision for future operating losses" → "the principle **IAS 37 addresses does not extend to** a provision for…"; "IAS 37 **requires** it to be disclosed as a contingent liability" → "the principle IAS 37 addresses **treats it as** disclosed…"; MCQ rationale "IAS 37 **prohibits** provisioning for future operating losses" → "**the provisioning principle does not reach** future operating losses". |
| `domain-02-financial-reporting.md` (×6) | §2 standard-requires | "IAS 1 **prohibits** offsetting unless a standard **requires/permits** it" → "the presentation principle **IAS 1 addresses is that** assets and liabilities are not offset unless the framework provides for it"; "IFRS 15 **requires** disclosures that…" → "the disclosure principle **IFRS 15 addresses is that users should be able to understand**…"; "**IAS 2 requires** inventories to be measured at…" → "the measurement principle **IAS 2 addresses is that** inventories are carried at…"; same pattern for **IAS 16** and **IAS 23**; self-check "What does IAS 23 **require** for interest…" → "How does **the borrowing-cost principle treat** interest…". All measurement content, numbers and answer keys unchanged. |
| `domain-03-budgeting-forecasting.md` | §6 tax — **ADDED** | New **Tax caution** block after the VAT/withholding cash-forecast passage: the rates are illustrative and are not any jurisdiction's; which indirect taxes apply, at what rate and base, with what registration, invoicing and remittance timing, whether a withholding applies to a cross-border payment, and whether any relief or treaty position is available are jurisdiction-specific and change; nothing states a tax position; the professional owns the cash-timing mechanics, the treatment comes from the entity's finance function and qualified tax advisers, and the forecast records whose advice it rests on. |
| `domain-07-contracts-commercial.md` | §1, §3 — **ADDED** | New **Standing caution for this knowledge area** at the head of KA 7.2: the material describes the management discipline of contract administration, does not state the law of any jurisdiction and is not legal advice; whether a clause means what a party thinks, whether an entitlement arises, whether an instrument is effective as drafted and what any of it obliges are questions for the contract and applicable law and belong to qualified counsel. |
| `domain-07-contracts-commercial.md` (7.2.3, MCQ 7.2-B) | §3 lawfulness (enforceability) | "In common-law jurisdictions LDs **must be** set as a genuine pre-estimate — a punitive penalty **is unenforceable**; many civil-law systems instead **enforce** penalty clauses subject to judicial adjustment" → "**Professional practice sets** the rate as a genuine pre-estimate rather than as a punishment, and that is the discipline this book teaches. Legal systems differ… Which position applies… turns on the governing law and is a question for qualified counsel; the professional obligation is to have it checked rather than assumed, and to record the answer." MCQ stem "Liquidated damages **are enforceable when** they represent" → "**In professional practice a liquidated-damages rate is set to** represent"; rationale reframed. Correct answer and all distractors preserved. Mirrored in the appendix MCQ bank (PCL-MCQ-07-07). |
| `domain-07-contracts-commercial.md` (7.4 ladder) | §1 legal effect | Adjudication "gives a decision that is binding at least temporarily: **the parties must comply** while…" → "a decision **typically expressed as** binding at least temporarily, the design intent being that the parties act on it… **Whether, and with what effect, such a decision binds in a particular case turns on the contract and on the applicable law, and is a question for counsel rather than for this book.**" |
| `domain-10-scheduling.md` (10.A.6) | §1, §3 — **ADDED** | New **A caution on what this topic is** closing 10.A.6: everything above is *method*; whether an extension of time is due, whether delay is concurrent and what follows, what notice provisions mean and what any of it entitles either party to are legal questions determined by the contract and applicable law; the word "entitlement" in the worked example names the output of an analytical method, not a legal conclusion; advice is taken before a claim or rejection is issued. **The worked example's numbers (120 / 10 / 127 / 7 / 3 days) were not touched.** |
| `domain-11-process-cycles.md` (11.A.2) | §1 criminality, mandatory reporting, whistleblowing | "Alerting a suspect ('tipping off') **is a criminal offence in some jurisdictions**" → "is treated by this book as a **serious professional failure in its own right**; restrictions on it **are understood to exist** in many jurisdictions, they vary, and their consequences are a question for qualified counsel rather than for the controls professional at the moment of discovery." Bullet "Duties may run outside the organisation… a suspicion **triggers a mandatory external report**… a person who raises a concern in good faith **may attract whistleblower protection**" → "**Obligations** may run outside the organisation. Regimes requiring an external report **are understood to exist in a number of jurisdictions**… arrangements protecting a person who raises a concern **are understood to exist in others**. **Nothing here states whether any of them applies… and none of it should be relied on**… The professional obligation this book does impose is narrower and unconditional — **raise the question with counsel promptly, and record that you did.**" |
| `domain-11-process-cycles.md` (11.A.2) | §4 data protection — **ADDED** | New **A caution on the records this protocol creates**: red flags, escalation notes and preservation holds are almost always information about identified living people; typically personal data with accuracy, proportionality, retention and subject-access obligations that vary; keeping the record remains the non-optional professional control; wording, holding, duration and access are for the data-protection function and counsel *before* the file is built. Adds the operative rule: **record the observation, not a conclusion about the person** — "raised and approved by the same user ID on 14 March" is a fact; "X is defrauding the company" is an allegation the controls professional is not entitled to make. |
| `domain-13-ai-for-project-controls.md` | §1, §2, §5 | "may only be stricter than **what the law requires**" → "stricter than **any applicable legal or regulatory obligation**"; edition/version citations stripped (see §3); "**Regulation (EU) 2024/1689, the EU AI Act** — the only instrument that **is legislation**. It is **binding within the European Union and nowhere else of its own force**, applying in phases with **general application from 2 August 2026**" → "**The European Union's AI Act** — the only instrument **understood to be** legislation. **Whether, when and how it reaches a given organisation, activity or system… is a question for legal and compliance and not one this book answers; no commencement date, scope or obligation should be taken from these pages.**"; OECD AI Principles "**not binding, even on the countries that adhere to it**" → "**a statement of principle rather than legislation**"; **added** "None of the issuing bodies is associated with, endorses or has reviewed this book, and no description here is authoritative." |
| `domain-13-ai-for-project-controls.md` (13.A.2) | §2 citation, §1 legal effect | Model-risk guidance citation "**(SR 11-7 / OCC 2011-12)**" removed; "**it binds only the firms those supervisors supervise**, and **its requirements** are neither reproduced nor summarised" → "**it is addressed to** the firms those supervisors supervise **rather than to the world**, and **its content** is neither reproduced nor summarised". |
| `appendices.md` Appendix C | §2, §5 | Preamble gains "they are not authoritative, and no requirement should be inferred from them" plus a bold non-affiliation statement covering every issuing body in the table. Four rows rewritten (see §3). Category definitions: "*Authoritative accounting standard* — issued by a standard-setter and **mandatory for** entities reporting under that framework" → "**applied by** entities…"; "One entry — the EU AI Act — **is genuine legislation, binding within the European Union and nowhere else of its own force**" → "**is understood to be legislation**… and **its reach is a question for qualified advice**". |
| `appendices.md` (IFRS 18 note) | §2 effective-date assertion | "IFRS 18 **replaces IAS 1 for annual reporting periods beginning on or after 1 January 2027**, and consequentially retitles IAS 8… **Candidates and practitioners working to periods from 2027 onward should read those topics against IFRS 18**" → "A further standard, IFRS 18 …, **has been issued and is expected to change** how financial statements are presented… **Its scope, its effective date and what it means for any particular entity are matters to confirm with the issuing body and with the entity's auditors; nothing is stated here.**" |

---

## 3. Clause, article, edition and instrument citations removed

**Eight distinct citations, across eleven locations. None was replaced with a different citation;
in every case the instrument is still *named*, only the authority-shaped identifier is gone.**

| # | Citation removed | Locations |
|---|---|---|
| 1 | `Regulation (EU) 2024/1689` (legal instrument number) | `bok/domain-13`, `bok/appendices.md` Appendix C |
| 2 | `SR 11-7 / OCC 2011-12` (supervisory-guidance document numbers) | `bok/domain-13` 13.A.2, `bok/appendices.md` Appendix C |
| 3 | `ISO/IEC 42001:2023` → `ISO/IEC 42001` (edition year) | `bok/domain-13` |
| 4 | `ISO/IEC 23894:2023` → `ISO/IEC 23894` (edition year) | `bok/domain-13` |
| 5 | `NIST AI RMF 1.0` → framework named without version | `bok/domain-13`, `bok/appendices.md` Appendix C |
| 6 | ISO/IEC 27701 "**2019 edition** / **2025 edition**" (edition years) | `_build/make_standards.py` → `pml-ai/STANDARDS.md` |
| 7 | EU AI Act general application "**2 August 2026**" (commencement date) | `bok/domain-13` |
| 8 | IFRS 18 effective date "**1 January 2027**" and the replacement assertion | `bok/appendices.md` |

**No clause number was invented, and none was retained.** The only surviving `clause 14.3`
references (`bok/domain-13` MCQ 13.6-F ×2, `bok/appendices.md` PCL-MCQ-13, `pfl-ai/domain-13`
§13.2.1) point to a **fictional contract inside the book's own worked scenario** — the MCQ is
precisely about an AI citing a clause whose stated value does not match. These are pedagogically
load-bearing and are not citations of any external instrument. Retained deliberately.

---

## 4. Highest-risk passage found

**PFL-AI, `manuscript/domain-01-foundations.md`, KA 1.3.2 "Financial crime: bribery, sanctions
and the money-laundering perimeter"** — roughly 90 lines that, before this sweep, told the
reader in the book's own voice that:

- breach of this duty **"is a criminal offence"** for the organisation *and the individual*;
- the exposure is **"priced in prosecutions"** and debarment;
- **"a facilitation payment is a bribe in most regimes"** and **"is prohibited under most
  anti-bribery regimes"**;
- **"not knowing what the intermediary did is not a defence"**, stated flatly;
- **"tipping off … is itself an offence in many regimes"**, twice, plus once more in the
  key-terms table and twice in MCQ rationales.

Aggravating factors: the passage names four families of conduct in the shape of statutory
offence definitions; it names a specific treaty, an intergovernmental standard and a
certifiable ISO standard and characterises each one's legal effect; and it instructs the reader
to build a file of allegations and diligence findings about **named natural persons** — PEPs,
beneficial owners, intermediaries — with no data-protection caution anywhere. A reader in an
unfamiliar jurisdiction acting on any one of those sentences, from a book published by a private
certification body, is the exposure the owner's instruction is aimed at.

Mitigating and preserved: the topic already ended with a genuinely good **Standing caution**
referring the jurisdictional questions to qualified counsel, and already stated that the OECD
Convention is not itself law. Neither was removed; both were strengthened.

Runner-up: **PCL-AI Domain 11, Advanced 11.A.2** — "tipping off is a criminal offence", "a
suspicion triggers a mandatory external report", "may attract whistleblower protection", again
with an instruction to record suspicions about identified individuals and no data-protection
caution.

---

## 5. Borderline calls — examined and deliberately left

| Passage | Why it was left |
|---|---|
| PFL-AI D4 "A cross-sector caution on tax" | Already model-grade: states the arithmetic and **refuses to state the treatment**, requires after-tax flows "computed on written advice for the specific jurisdiction and structure", with the advice referenced in the assumption register. Nothing to fix. |
| PFL-AI D5 SPV limits (withholding, thin capitalisation) | Already "all for qualified tax counsel and all capable of changing after-tax cash". Left. |
| PFL-AI D5 security instruments, D12 termination compensation | Already "whether a given security instrument is enforceable as drafted is jurisdiction-specific and belongs to counsel"; "matters of local law and public policy that vary fundamentally between jurisdictions… must be confirmed by qualified local counsel". Left. |
| PFL-AI D16 §16.4.1 personal data | "protected by law that differs materially between jurisdictions; **this book does not state any jurisdiction's requirements**". Left — it is the model the rest of the corpus should follow. |
| PML-AI D12 §12.3.4 performance management | Carries "**nothing in this domain should be read as advice on any jurisdiction's employment law**" and routes formal consequences to HR and legal. The phrase "lawful termination" survives inside an explicit list of things that *differ by country* — it is not an assertion. Left. |
| PML-AI D14 §14.4.1 explainability | "Explainability obligations in law and regulation are **jurisdiction-specific and moving**, and **nothing in this book should be read as stating them**", then names AI Act / GDPR / ISO / NIST as reference points "each applying on its own terms, in its own territory". Model treatment. Left. |
| PML-AI D16 retention economics | "Retention periods for personal and health data are set by law and by contract in each jurisdiction and by data class… **nothing here states a legal minimum or maximum**". Left. |
| PCL-AI, uses of "statutory reporting", "statutory accounts", "statutory rate" | These are the ordinary accounting terms for externally audited reporting and the headline tax rate, not assertions about legal duty. Changing them would damage comprehension for no risk reduction. Left throughout (BoK D2, D5, appendices; PFL-AI D6, D15). |
| PCL-AI D7 concurrent delay (7.A.1) and global claims (7.A.2) | Explicitly "**described neutrally**… different contracts and forums resolve it differently — this reference stays at concept level". Approaches are attributed to practice, not asserted as law. Left; now additionally covered by the new KA 7.2 standing caution. |
| PCL-AI D12 ISO 31000, D9 Scrum Guide / Agile Manifesto | "ISO 31000 **describes**…", "Described from the current Scrum Guide's concepts, **in this reference's own words**". No requires-assertions found. Left. |
| PCL-AI D3 "statutory project bank accounts" | Names a funding-structure type used in some public frameworks; descriptive, not an obligation. Left, though a reviewer may prefer "project bank accounts mandated under some public frameworks" softened further. |
| "Liable / liability" throughout D7, D16, PFL D12 | Overwhelmingly contractual vocabulary — liquidated damages, defects liability period, limitation of liability, liability caps. Not legal-consequence assertions. Left. |
| `clause 14.3` (5 occurrences) | Fictional contract inside the book's own worked scenario; the point of the MCQ. Left — see §3. |

---

## 6. Removed material a subject-matter reviewer may want restored in another form

Nothing was deleted outright. Four items lost *specificity* and a reviewer with the right
qualifications may wish to restore them in a form that carries proper legal sign-off:

1. **The EU AI Act commencement date (2 August 2026)** and its phased-application framing.
   Genuinely useful to a practitioner planning a programme. If restored, it should sit behind an
   explicit "as at the date of writing; confirm the current position" and should not appear in a
   book that will outlive the date. *Currently: the phasing is mentioned, the date is not.*
2. **The IFRS 18 effective date (periods beginning on or after 1 January 2027)** and the
   statement that it replaces IAS 1 and retitles IAS 8. Materially useful to candidates. Same
   treatment: restore only with a dated "confirm with the issuing body" wrapper, or leave the
   forward pointer as it now stands.
3. **`SR 11-7 / OCC 2011-12`.** The document numbers are how a practitioner actually finds this
   guidance. If a reviewer judges the citation safe (it is a public supervisory document, named
   not summarised), it could return as a "further reading" pointer clearly outside the
   book's assertion voice, rather than inline in the text.
4. **The ISO/IEC 27701 2019-vs-2025 edition distinction.** The substantive warning — that a
   conformity claim means different things depending on edition — is preserved, but the reader
   can no longer tell *which* editions. Restoring the years would contradict the register's own
   no-editions policy; the alternative is to relax that policy deliberately for entries where
   the edition materially changes the claim. A reviewer's call, not an editor's.

Additionally, two phrasings were *weakened* rather than removed and a subject-matter reviewer
may consider them now under-stated:

- **"not knowing what the intermediary did is not a defence"** → "a leader should not assume
  that not knowing will protect the organisation". The professional lesson survives and is
  arguably clearer, but the original was more arresting.
- **"tipping off is an offence"** → "a serious professional failure… restrictions are understood
  to exist". In the two places where this drives behaviour (do not confront, do not warn) the
  instruction itself is unchanged and unconditional, so the control is intact; only the reason
  given is softer.

---

## 7. Preserved, as instructed

- **Every** pre-existing disclaimer, jurisdictional caution and "seek qualified advice"
  statement. The full diff was audited for deletions of caution text: the nine deleted lines
  matching `counsel|advice|caution|jurisdiction-specific|varies` were each replaced in place
  by an equal or stronger formulation. **Net cautions added: 5 new blocks** (BoK D3 tax, BoK D7
  KA 7.2 standing caution, BoK D10 delay-analysis caution, BoK D11 personal-data caution,
  PFL-AI D1 personal-data caution) plus 3 new bullets in `bok/00-conventions.md` §9.
- The AI-drafting disclosure and review-status statements in the front matter
  (`books/README.md`, `pml-ai/TOC.md`, `pfl-ai/TOC.md`) — not touched.
- Every professional obligation. **No duty was deleted anywhere in this sweep.** Escalation
  routes, stop rules, preservation-before-enquiry, do-not-investigate, decision rights, the
  evidence-that-must-exist lists, the gate-block rule and the red-flag tables are all intact
  and in several places now carry an added unconditional obligation ("raise the question with
  counsel promptly, and record that you did").
- All worked examples, arithmetic and verified numbers. **No number was changed.** Two places
  where a number-adjacent fix was tempting were handled by adding a caution instead of editing:
  BoK D10's 120/127/7-day EOT example and BoK D3's illustrative 15 % VAT / 5 % withholding.

---

## 8. Verification

```
cd /home/user/PCI/docs/books/_build && python3 verify_formulas.py
…
check modules loaded: 58
TALLY PFL-AI:0=905;…;PML-AI:17=324
✓ all golden answers verified
```

Run twice — once after the first edit batch and once after the last. Exit code 0, zero `FAIL`
lines, zero `✗`. Tallies identical to the pre-sweep baseline. No edit had to be reverted.

Derived-file generators also re-run clean:
`make_question_bank.py` — 363 + 453 items, **0 defects**;
`make_glossary.py` — 591 + 455 terms, **0 defects**;
`make_standards.py` — 22 + 13 references, **0 defects**.

Nothing was committed.
