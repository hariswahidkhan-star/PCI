# PCI Professional Law Concordance

**Status:** Machine-checkable reference index for the PCI Professional Law system. It carries **no
obligation**: every requirement lives in an identified law or process requirement (Charter §3), and
nothing stated here creates, narrows or waives one. Where this file and a published law differ, the
law governs and the difference is a defect in this file.

**What it is for.** The four law files are rebuilt independently. A citation that resolves to the
wrong law is more dangerous than one that does not resolve at all, because every automated check
passes while the reader is sent to a different obligation. This file records, in one place, which
certification law cites which foundational law, so that a change on either side is visible. It is
verified by [`check_laws.py`](check_laws.py), which regenerates the table below from the law files
themselves and fails if the published table has drifted.

**Sources.** [`PCI_FOUNDATIONAL_LAWS.md`](PCI_FOUNDATIONAL_LAWS.md) ·
[`PCL_AI_LAWS.md`](PCL_AI_LAWS.md) · [`PFL_AI_LAWS.md`](PFL_AI_LAWS.md) ·
[`PML_AI_LAWS.md`](PML_AI_LAWS.md). Governing instruments:
[`PCI_PROFESSIONAL_LAWS_CHARTER.md`](PCI_PROFESSIONAL_LAWS_CHARTER.md) and
[`PCI_LAW_DRAFTING_MANUAL.md`](PCI_LAW_DRAFTING_MANUAL.md).

---

## 1. The foundational set

Fifteen laws, `PCI-FND-LAW-01` to `PCI-FND-LAW-15`. **A citation is correct only when the number and
the subject agree**; the subject is the authority, the number is only its address.

| ID | Subject | Principal obligation, in one line |
|---|---|---|
| `PCI-FND-LAW-01` | Professional Accountability | Record your own acceptance of personal accountability for a material output before it is issued, whoever or whatever prepared it. |
| `PCI-FND-LAW-02` | Evidence Before Assertion | Do not present a material claim unless retrievable evidence for it is identified in the working record. |
| `PCI-FND-LAW-03` | Independent Verification | Have a material calculation, model output or automated conclusion verified by an independent competent reviewer before anyone relies on it. |
| `PCI-FND-LAW-04` | Human Decision Authority | Record every material decision as taken by a named, authorised, competent human decision owner. |
| `PCI-FND-LAW-05` | Transparent Assumptions | Do not issue a forecast, model, appraisal or recommendation unless the document carrying the conclusion also carries its material assumptions. |
| `PCI-FND-LAW-06` | Source and Version Integrity | Do not rely on a source without a dated check that it is the version in force for the matter. |
| `PCI-FND-LAW-07` | Data Lineage | Make every material output reproducible from its sources by a competent reviewer without the preparer's help. |
| `PCI-FND-LAW-08` | Conflict Disclosure | Disclose every conflict capable of affecting your independent judgement, in writing, before acting in the matter. |
| `PCI-FND-LAW-09` | Confidentiality and Approved Technology | Do not enter protected information into a system that is not authorised for it, including any AI system. |
| `PCI-FND-LAW-10` | Competence and Limitation | Do not accept, continue or issue work requiring competence you do not hold. |
| `PCI-FND-LAW-11` | Escalation of Material Misstatement | Escalate a known or suspected material error, omission, unsupported assumption or misleading presentation promptly to the level accountable for remedying it. |
| `PCI-FND-LAW-12` | Record Integrity | Keep records so that authorship and creation time are fixed and any later alteration is detectable and attributable. |
| `PCI-FND-LAW-13` | No Silent Override | Do not override a control result on a material matter without an authorised decision owner's recorded decision, made before reliance. |
| `PCI-FND-LAW-14` | Responsible AI | Never permit an AI system to decide, approve, certify, sign, waive or authorise a material matter, or to be represented as having independently verified one. |
| `PCI-FND-LAW-15` | Correction Duty | Communicate a material error in work you issued promptly to everyone known to be relying on it. |

---

## 2. Which certification laws cite which foundational law

Counts are of **citing laws**, not of citation occurrences: a law that names its foundational parent
three times inside its own text is counted once.

| Foundational law | Subject | PCL-AI (42) | PFL-AI (74) | PML-AI (97) |
|---|---|---|---|---|
| `PCI-FND-LAW-01` | Professional Accountability | `PCI-PCL-LAW-12.01`<br>`PCI-PCL-LAW-13.04` | `PCI-PFL-LAW-16.01`<br>`PCI-PFL-LAW-16.03` | `PCI-PML-LAW-01.01`<br>`PCI-PML-LAW-01.03`<br>`PCI-PML-LAW-02.02`<br>`PCI-PML-LAW-03.01`<br>`PCI-PML-LAW-03.04`<br>`PCI-PML-LAW-07.02`<br>`PCI-PML-LAW-10.01`<br>`PCI-PML-LAW-12.01`<br>`PCI-PML-LAW-12.02`<br>`PCI-PML-LAW-13.02`<br>`PCI-PML-LAW-14.02` |
| `PCI-FND-LAW-02` | Evidence Before Assertion | `PCI-PCL-LAW-01.01`<br>`PCI-PCL-LAW-01.02`<br>`PCI-PCL-LAW-03.01`<br>`PCI-PCL-LAW-04.01`<br>`PCI-PCL-LAW-04.02`<br>`PCI-PCL-LAW-05.02`<br>`PCI-PCL-LAW-05.03`<br>`PCI-PCL-LAW-06.02`<br>`PCI-PCL-LAW-07.03`<br>`PCI-PCL-LAW-10.01`<br>`PCI-PCL-LAW-10.03` | `PCI-PFL-LAW-01.01`<br>`PCI-PFL-LAW-06.01`<br>`PCI-PFL-LAW-06.03`<br>`PCI-PFL-LAW-06.04`<br>`PCI-PFL-LAW-06.05`<br>`PCI-PFL-LAW-09.02`<br>`PCI-PFL-LAW-09.03`<br>`PCI-PFL-LAW-10.05`<br>`PCI-PFL-LAW-12.01`<br>`PCI-PFL-LAW-13.02`<br>`PCI-PFL-LAW-13.03`<br>`PCI-PFL-LAW-13.04`<br>`PCI-PFL-LAW-14.01`<br>`PCI-PFL-LAW-14.02`<br>`PCI-PFL-LAW-14.04`<br>`PCI-PFL-LAW-15.01` | `PCI-PML-LAW-01.01`<br>`PCI-PML-LAW-02.01`<br>`PCI-PML-LAW-02.02`<br>`PCI-PML-LAW-03.03`<br>`PCI-PML-LAW-05.01`<br>`PCI-PML-LAW-06.01`<br>`PCI-PML-LAW-07.01`<br>`PCI-PML-LAW-08.01`<br>`PCI-PML-LAW-09.01`<br>`PCI-PML-LAW-09.02`<br>`PCI-PML-LAW-11.01`<br>`PCI-PML-LAW-13.01`<br>`PCI-PML-LAW-15.02`<br>`PCI-PML-LAW-16.01`<br>`PCI-PML-LAW-16.02`<br>`PCI-PML-LAW-16.03` |
| `PCI-FND-LAW-03` | Independent Verification | `PCI-PCL-LAW-03.05`<br>`PCI-PCL-LAW-10.02`<br>`PCI-PCL-LAW-13.02`<br>`PCI-PCL-LAW-13.03` | `PCI-PFL-LAW-16.01`<br>`PCI-PFL-LAW-16.02`<br>`PCI-PFL-LAW-16.03` | `PCI-PML-LAW-01.02`<br>`PCI-PML-LAW-14.02` |
| `PCI-FND-LAW-04` | Human Decision Authority | `PCI-PCL-LAW-05.04`<br>`PCI-PCL-LAW-12.03` | `PCI-PFL-LAW-14.04`<br>`PCI-PFL-LAW-16.03` | `PCI-PML-LAW-01.01`<br>`PCI-PML-LAW-01.02`<br>`PCI-PML-LAW-03.02`<br>`PCI-PML-LAW-13.01`<br>`PCI-PML-LAW-13.02`<br>`PCI-PML-LAW-14.02` |
| `PCI-FND-LAW-05` | Transparent Assumptions | `PCI-PCL-LAW-03.04`<br>`PCI-PCL-LAW-05.03`<br>`PCI-PCL-LAW-06.04`<br>`PCI-PCL-LAW-12.02` | `PCI-PFL-LAW-01.01`<br>`PCI-PFL-LAW-05.01`<br>`PCI-PFL-LAW-06.02`<br>`PCI-PFL-LAW-09.01`<br>`PCI-PFL-LAW-10.01`<br>`PCI-PFL-LAW-10.02`<br>`PCI-PFL-LAW-10.03`<br>`PCI-PFL-LAW-11.01`<br>`PCI-PFL-LAW-13.03`<br>`PCI-PFL-LAW-14.02`<br>`PCI-PFL-LAW-14.03`<br>`PCI-PFL-LAW-15.02` | `PCI-PML-LAW-02.01`<br>`PCI-PML-LAW-06.01`<br>`PCI-PML-LAW-07.01`<br>`PCI-PML-LAW-08.01`<br>`PCI-PML-LAW-15.01`<br>`PCI-PML-LAW-15.02` |
| `PCI-FND-LAW-06` | Source and Version Integrity | `PCI-PCL-LAW-01.01`<br>`PCI-PCL-LAW-03.02`<br>`PCI-PCL-LAW-04.01`<br>`PCI-PCL-LAW-06.03`<br>`PCI-PCL-LAW-07.01` | `PCI-PFL-LAW-06.04`<br>`PCI-PFL-LAW-12.01`<br>`PCI-PFL-LAW-14.01`<br>`PCI-PFL-LAW-16.02` | `PCI-PML-LAW-03.03`<br>`PCI-PML-LAW-04.01`<br>`PCI-PML-LAW-05.02`<br>`PCI-PML-LAW-16.03` |
| `PCI-FND-LAW-07` | Data Lineage | `PCI-PCL-LAW-01.03`<br>`PCI-PCL-LAW-05.01`<br>`PCI-PCL-LAW-07.02` | `PCI-PFL-LAW-06.01`<br>`PCI-PFL-LAW-06.02`<br>`PCI-PFL-LAW-06.03`<br>`PCI-PFL-LAW-10.01`<br>`PCI-PFL-LAW-16.01` | `PCI-PML-LAW-05.02`<br>`PCI-PML-LAW-07.01`<br>`PCI-PML-LAW-14.01` |
| `PCI-FND-LAW-08` | Conflict Disclosure | — | `PCI-PFL-LAW-01.02`<br>`PCI-PFL-LAW-13.01`<br>`PCI-PFL-LAW-13.02`<br>`PCI-PFL-LAW-15.02` | `PCI-PML-LAW-01.03`<br>`PCI-PML-LAW-10.01` |
| `PCI-FND-LAW-09` | Confidentiality and Approved Technology | `PCI-PCL-LAW-13.01` | `PCI-PFL-LAW-06.04`<br>`PCI-PFL-LAW-16.01` | `PCI-PML-LAW-10.01`<br>`PCI-PML-LAW-12.02`<br>`PCI-PML-LAW-14.01` |
| `PCI-FND-LAW-10` | Competence and Limitation | `PCI-PCL-LAW-07.01` | `PCI-PFL-LAW-09.01`<br>`PCI-PFL-LAW-09.02`<br>`PCI-PFL-LAW-10.04`<br>`PCI-PFL-LAW-12.02`<br>`PCI-PFL-LAW-13.01` | `PCI-PML-LAW-01.03`<br>`PCI-PML-LAW-12.01` |
| `PCI-FND-LAW-11` | Escalation of Material Misstatement | — | `PCI-PFL-LAW-05.01`<br>`PCI-PFL-LAW-09.03`<br>`PCI-PFL-LAW-10.04`<br>`PCI-PFL-LAW-10.05`<br>`PCI-PFL-LAW-11.01`<br>`PCI-PFL-LAW-12.02`<br>`PCI-PFL-LAW-13.03`<br>`PCI-PFL-LAW-14.03`<br>`PCI-PFL-LAW-14.04`<br>`PCI-PFL-LAW-15.01`<br>`PCI-PFL-LAW-15.03` | `PCI-PML-LAW-02.01`<br>`PCI-PML-LAW-03.02`<br>`PCI-PML-LAW-03.04`<br>`PCI-PML-LAW-07.02`<br>`PCI-PML-LAW-08.01`<br>`PCI-PML-LAW-08.02`<br>`PCI-PML-LAW-11.01`<br>`PCI-PML-LAW-12.01`<br>`PCI-PML-LAW-12.02`<br>`PCI-PML-LAW-15.01`<br>`PCI-PML-LAW-15.02`<br>`PCI-PML-LAW-16.01` |
| `PCI-FND-LAW-12` | Record Integrity | `PCI-PCL-LAW-11.01` | `PCI-PFL-LAW-06.04`<br>`PCI-PFL-LAW-06.05`<br>`PCI-PFL-LAW-13.04`<br>`PCI-PFL-LAW-15.03` | `PCI-PML-LAW-01.01`<br>`PCI-PML-LAW-02.02`<br>`PCI-PML-LAW-03.01`<br>`PCI-PML-LAW-03.02`<br>`PCI-PML-LAW-03.03`<br>`PCI-PML-LAW-04.01`<br>`PCI-PML-LAW-05.01`<br>`PCI-PML-LAW-05.02`<br>`PCI-PML-LAW-08.02`<br>`PCI-PML-LAW-09.01`<br>`PCI-PML-LAW-09.02`<br>`PCI-PML-LAW-13.01`<br>`PCI-PML-LAW-13.02`<br>`PCI-PML-LAW-14.01`<br>`PCI-PML-LAW-15.01`<br>`PCI-PML-LAW-16.02`<br>`PCI-PML-LAW-16.03` |
| `PCI-FND-LAW-13` | No Silent Override | `PCI-PCL-LAW-03.02`<br>`PCI-PCL-LAW-03.03`<br>`PCI-PCL-LAW-06.01` | — | `PCI-PML-LAW-01.02`<br>`PCI-PML-LAW-03.02`<br>`PCI-PML-LAW-04.01`<br>`PCI-PML-LAW-05.01`<br>`PCI-PML-LAW-06.01`<br>`PCI-PML-LAW-12.01`<br>`PCI-PML-LAW-16.01` |
| `PCI-FND-LAW-14` | Responsible AI | `PCI-PCL-LAW-13.01`<br>`PCI-PCL-LAW-13.02`<br>`PCI-PCL-LAW-13.03`<br>`PCI-PCL-LAW-13.04` | `PCI-PFL-LAW-06.05`<br>`PCI-PFL-LAW-16.01`<br>`PCI-PFL-LAW-16.02`<br>`PCI-PFL-LAW-16.03` | `PCI-PML-LAW-01.02`<br>`PCI-PML-LAW-11.01`<br>`PCI-PML-LAW-14.02` |
| `PCI-FND-LAW-15` | Correction Duty | `PCI-PCL-LAW-04.03` | — | `PCI-PML-LAW-01.01`<br>`PCI-PML-LAW-08.02`<br>`PCI-PML-LAW-11.01` |

**Foundational laws with no citation from a credential are not a defect.** A certification law exists
only where the credential's subject matter adds something the foundational law does not reach; where
it adds nothing, no law is published (Manual §9 Q12). `PCI-FND-LAW-08` and `PCI-FND-LAW-11` carry no
PCL-AI law, and `PCI-FND-LAW-13` and `PCI-FND-LAW-15` carry no PFL-AI law, for that reason.

---

## 3. Superseded identifiers

**Nothing below is a live citation.** These identifiers are recorded so that the history stays
traceable; Charter §10 forbids reusing a withdrawn identifier, and no published law cites any of
them.

### 3.1 The withdrawn fourteen-law foundational scheme `PCI-LAW-F-NN`

Withdrawn and replaced by the fifteen-law set in §1. The reason was structural — an eighteen-field
template, two mixed normative-language systems, and several obligations bundled into single clauses.
The mapping is reproduced from the supersession record in `PCI_FOUNDATIONAL_LAWS.md`, which governs.

| Superseded | Subject | Carried forward into |
|---|---|---|
| `PCI-LAW-F-01` | Professional Accountability and the Suite Principle | `PCI-FND-LAW-01` |
| `PCI-LAW-F-02` | Verification of AI Output Before Professional Use | `PCI-FND-LAW-03` (verification substance) and `PCI-FND-LAW-14` (boundary substance) |
| `PCI-LAW-F-03` | Human Decision Authority | `PCI-FND-LAW-04` |
| `PCI-LAW-F-04` | Disclosure of Material AI Assistance | `PCI-FND-LAW-14-PR-01` to `-PR-03` |
| `PCI-LAW-F-05` | Evidence and the Audit Trail | `PCI-FND-LAW-02`, `PCI-FND-LAW-07`, `PCI-FND-LAW-12` |
| `PCI-LAW-F-06` | Data Lineage and Integrity | `PCI-FND-LAW-07` |
| `PCI-LAW-F-07` | Honesty in Reporting and Forecasting | `PCI-FND-LAW-05`, `PCI-FND-LAW-11`, `PCI-FND-LAW-15` (correction substance) |
| `PCI-LAW-F-08` | Competence Boundaries and Referral | `PCI-FND-LAW-10` |
| `PCI-LAW-F-09` | Confidentiality and Information Protection | `PCI-FND-LAW-09` |
| `PCI-LAW-F-10` | Conflict-of-Interest Disclosure | `PCI-FND-LAW-08` |
| `PCI-LAW-F-11` | Duty to Escalate | `PCI-FND-LAW-11` |
| `PCI-LAW-F-12` | Record Retention | `PCI-FND-LAW-12` |
| `PCI-LAW-F-13` | Ethical Conduct Toward Candidates, Employers and the Public | **Not carried forward at Level 1.** Specific duties sit in `PCI-FND-LAW-08`, `PCI-FND-LAW-11-PR-05` and `PCI-FND-LAW-14`; general conduct is a certification-conditions matter |
| `PCI-LAW-F-14` | No Misrepresentation of PCI Credentials or Accreditation Status | **Not carried forward at Level 1.** Credential-claim rules govern the PCI-to-holder relationship, not the conduct of professional work |

**Four of the fifteen have no predecessor in the withdrawn set** — `PCI-FND-LAW-06` (source and
version integrity) and `PCI-FND-LAW-13` (no silent override) are new laws, and `PCI-FND-LAW-02` and
`PCI-FND-LAW-15` are new laws carrying part of a withdrawn one forward. A concordance that maps the
new numbers one-for-one onto the old is therefore wrong on its face, whatever it says.

### 3.2 The corrected PFL-AI concordance

`PFL_AI_LAWS.md` v2.0 published a table equating each `PCI-FND-LAW-NN` identifier with the subject of
`PCI-LAW-F-NN` — it inferred its mapping from the superseded fourteen-law file. Every identifier
resolved, so no dangling-reference check could see it, but many resolved to a different law than the
citing sentence intended: `PCI-FND-LAW-03` was equated with *Human Decision Authority*, which is law
04's subject, and `PCI-FND-LAW-02` with *Verification of AI Output Before Professional Use*, which is
no longer a foundational subject at all. **That table is deleted.** Every foundational citation in
that volume has been re-pointed by subject, and the three subjects with no successor were handled as
follows.

| Subject cited by PFL v2.0 | Successor used | Where none fitted |
|---|---|---|
| *Honesty in reporting and forecasting* (`PCI-LAW-F-07`) | `PCI-FND-LAW-05` for presentation honesty; `PCI-FND-LAW-02` where the sentence turns on evidence for a claim; `PCI-FND-LAW-11` where it turns on raising a known problem | — |
| *Ethical conduct toward candidates, employers and the public* (`PCI-LAW-F-13`) | `PCI-FND-LAW-11` in `PCI-PFL-LAW-09.03` | Dropped in `PCI-PFL-LAW-01.02` and `PCI-PFL-LAW-13.02`, where the surviving duty is `PCI-FND-LAW-08`, already cited |
| *No misrepresentation of PCI credentials or accreditation status* (`PCI-LAW-F-14`) | `PCI-FND-LAW-02` in `PCI-PFL-LAW-13.02`; `PCI-FND-LAW-14` in `PCI-PFL-LAW-16.02` | Dropped in `PCI-PFL-LAW-09.02`, `PCI-PFL-LAW-09.03` and `PCI-PFL-LAW-12.02`, where the surviving duty is `PCI-FND-LAW-10` or `PCI-FND-LAW-02`, already cited |

**Five citations were dropped rather than re-pointed**, because pointing them at a law that does not
carry the subject would reproduce the defect in a new form.

**Two citations kept their number and changed their subject**, which is the reverse trap and is
recorded so it is not mistaken for an untouched line: `PCI-FND-LAW-06` in `PCI-PFL-LAW-06.04`,
`PCI-PFL-LAW-12.01`, `PCI-PFL-LAW-14.01` and `PCI-PFL-LAW-16.02` now cites *source and version
integrity* rather than the withdrawn *data lineage and integrity*; `PCI-FND-LAW-14` in
`PCI-PFL-LAW-16.02` now cites *responsible AI* rather than the withdrawn credential-claims law.

### 3.3 Withdrawn certification-law identifier schemes

| Credential | v1.0 form | Current form | Note |
|---|---|---|---|
| PCL-AI | `PCL-LAW-DD-NN` | `PCI-PCL-LAW-DD.NN` | Whole set renumbered; each successor records its predecessor in element 25 |
| PFL-AI | `PFL-LAW-DD-NN` | `PCI-PFL-LAW-DD.NN` | Whole set renumbered; twenty-two of twenty-four v1.0 laws have a named successor |
| PML-AI | `PML-LAW-DD-NN` | `PCI-PML-LAW-DD.NN` | Whole set renumbered |

### 3.4 The two PFL-AI v1.0 laws withdrawn without a successor law

Recorded under Charter §10: a withdrawn law's withdrawal and its reason are published, and the law is
not deleted from the record. Neither obligation was abolished; each became a process requirement
under the law that already owned the surrounding discipline.

| Withdrawn v1.0 law | Reason | Where the obligation now lives |
|---|---|---|
| `PFL-LAW-04-01` — Appraisal Discipline (D4) | Bundled four obligations into one unenforceable sentence, and its subject is a presentation discipline rather than a distinct professional duty once `PCI-PFL-LAW-01.01` governs how a financial judgement may be presented | `PCI-PFL-LAW-01.01-PR-05`. Domain 4 anchors no law in this edition |
| `PFL-LAW-07-01` — Revenue Assumption Discipline (D7) | Duplicated `PCI-PFL-LAW-06.03` and `PCI-PFL-LAW-12.01` in every respect but the prohibition on presenting a forecast revenue as contracted; Manual §9 Q12 treats a law that adds nothing to its neighbours as a defect | `PCI-PFL-LAW-06.03-PR-05`. Domain 7 anchors no law in this edition |

---

## 4. What the validator checks

[`check_laws.py`](check_laws.py) runs over the four law files and exits non-zero on any of:

1. a `PCI-FND-LAW-NN` citation whose number does not exist in the foundational set;
2. a certification-law citation (`PCI-PCL-LAW-…`, `PCI-PFL-LAW-…`, `PCI-PML-LAW-…`) that does not
   resolve to a published law, in either direction;
3. a law missing any of the Manual §5 twenty-five elements, or carrying them out of order;
4. a duplicate law identifier;
5. `shall` inside any law element or process requirement — the Manual §1 ban is on the form, so the
   explanatory front matter that names the word in order to say PCI does not use it is permitted;
6. an anchor domain outside its credential's Body of Knowledge range — PCL-AI ≤ 13, PFL-AI ≤ 16,
   PML-AI ≤ 16;
7. drift between §2 of this file and the citations actually published in the law files.

**What it cannot check is the thing that caused the defect.** A citation whose number resolves but
whose subject is wrong passes every mechanical test there is. Only reading the sentence catches it,
which is why §2 exists in a form a human can scan.

---

*Compiled 2026-08-04 from the published law files. Draft for approval under
Charter §5 — this index has no status of its own beyond the laws it indexes. British English
throughout.*
