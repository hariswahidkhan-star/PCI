# PCI Law Drafting Manual

**Status:** Binding drafting specification for every PCI Professional Law. Version 1.0.  
**Companion:** the **PCI Professional Laws Charter** (status, hierarchy, priority, due process,
interpretation, amendment). This manual governs how a law is *written*; the Charter governs what a
law *is*. A law must conform to both.

---

## 1. The normative language system

PCI uses **modern must-drafting**. This is a deliberate choice and it is exclusive: PCI does not use
ISO-style `shall` drafting, and the two systems are never mixed.

| Word | Meaning in a PCI Law | Use it for |
|---|---|---|
| **must** | Mandatory PCI professional requirement | The obligation itself |
| **must not** | Prohibited practice | Conduct that constitutes a breach |
| **should** | Recommended practice; a justified alternative may be acceptable | Level 5 recommendations only |
| **may** | Permission | Something the professional is allowed to do |
| **can** | Capability or possibility — never permission | Describing what a person, tool or method is able to do |

**`shall` is not used anywhere in a PCI Law**, in any field, including quotations of PCI's own
earlier drafts. Legislative `shall` is ambiguous — it has been read as both obligation and futurity —
and modern drafting practice in the UK and elsewhere has moved to `must` for precisely that reason.
A draft containing `shall` fails gate.

**The ISO mapping must be stated wherever these laws meet an ISO-literate audience.** A reader who
works to ISO/IEC drafting conventions will expect `shall` to mark a requirement and may read `must`
as an external constraint rather than a requirement. PCI's convention is therefore stated explicitly
in every law publication: in a PCI Law, **`must` is the requirement form**, and it corresponds to
what an ISO document would express with `shall`.

### 1.1 Worked contrasts

| Wrong | Right | Why |
|---|---|---|
| The professional shall verify the forecast. | The professional must verify the forecast. | `shall` is not used. |
| The professional should verify every material calculation. | The professional must independently verify every material calculation before approving the output. | A mandatory obligation written as a recommendation is unenforceable. |
| The professional may not approve an unverified output. | The professional must not approve an unverified output. | `may not` is ambiguous between prohibition and absence of permission. |
| A competent reviewer can approve the baseline. | A competent reviewer may approve the baseline. | This is permission, not capability. |
| The model can be updated by the modeller. | The modeller may update the model. | `can` describes capability; permission needs `may`, and the actor must be the subject. |

### 1.2 External obligations

Where an obligation originates outside PCI, use this form and do not dress it as a PCI requirement:

> Applicable law, regulation, contract or authoritative professional requirements may impose
> additional obligations.

## 2. One requirement per clause

Each mandatory clause expresses **one principal obligation**. A clause bundling several obligations
cannot be tested, cannot be breached precisely, and cannot be assessed.

**Wrong:**

> The professional must validate the model, inform management, review the contract, maintain records
> and escalate every variance.

**Right — five independently testable requirements:**

> - The professional must validate the model.
> - The professional must document the validation.
> - The professional must communicate material limitations.
> - The professional must escalate defined exceptions.
> - The professional must retain the supporting evidence.

Every requirement has a unique identifier, a single principal obligation, a defined **subject** (who),
a defined **action** (what they must do), a defined **object** (to what), and a **compliance test**.

**Every requirement must be verifiable.** If no one can determine whether it has been met, it is not
a requirement — it is an aspiration, and it belongs in Recommended Practice or in commentary.

## 3. Identifiers

| Instrument | Form | Example |
|---|---|---|
| Foundational Law | `PCI-FND-LAW-NN` | `PCI-FND-LAW-04` |
| Certification / Domain Law | `PCI-<CRED>-LAW-DD.NN` | `PCI-PCL-LAW-06.03` |
| Process Requirement | `<parent>-PR-NN` | `PCI-PCL-LAW-06.03-PR-01` |
| Recommended Practice | `<parent>-RP-NN` | `PCI-PFL-LAW-10.01-RP-02` |

`<CRED>` is `PCL`, `PFL` or `PML`. `DD` is the two-digit Body of Knowledge domain of primary
anchorage. `NN` is a two-digit sequence within that domain.

Identifiers are **stable**. All internal citation is by identifier — never by page number, because
pagination changes. A withdrawn law's identifier is never reused.

## 4. Defined terms

Any term that could alter whether a professional has complied must be defined in the law that uses
it, or in the **PCI Law Definitions Register**, and used consistently.

Terms that always need definition when they appear in an obligation: *material*, *independent*,
*approved*, *verified*, *current*, *competent reviewer*, *evidence*, *decision owner*, *exception*,
*escalation threshold*, *promptly*, *appropriate*, *adequate*, *reasonable*.

**No circular definitions.** "A material variance is a variance that is material" defines nothing.
Define by the test a reader can apply: what makes it material, measured against what, decided by whom.

**Undefined judgement words are a drafting defect.** *Appropriate*, *adequate*, *reasonable*,
*relevant*, *timely* and *sufficient* must either be defined, replaced with a stated test, or removed.

## 5. The mandatory structure of a law

Every PCI Law carries all twenty-five elements below, in this order. No element may be omitted. Where
an element is genuinely inapplicable, it states "Not applicable" **and gives the reason in one
clause** — a bare "None." is a defect.

```
## PCI LAW <identifier> — <official title>

**1. Normative requirement.**   One precise mandatory statement. One principal obligation.

**2. Purpose.**                 The professional risk this law controls.

**3. Scope.**                   Who is governed · which decisions · which projects, programmes or
                                transactions · whether it applies to preparation, review,
                                recommendation, approval or assurance.

**4. Defined terms.**           Every term in this law that could alter compliance.

**5. Required actions.**        The minimum actions necessary for compliance.

**6. Prohibited actions.**      The practices that constitute a breach.

**7. Required evidence.**       What must exist to prove compliance.

**8. Responsible role.**        The role accountable for performance. Never "the team",
                                "management", "relevant people", "appropriate personnel" or
                                "the organisation" unless that term is formally defined.

**9. Approval authority.**      Who may approve, reject, waive or escalate.

**10. Independence requirement.** Whether the reviewer must be independent of preparation,
                                commercial benefit, model development, contract administration,
                                project sponsorship or AI-tool configuration.

**11. Materiality or threshold.** Numerical, risk, authority, cumulative or trigger-event threshold,
                                or the professional-judgement criteria that stand in for one.

**12. Exception and waiver.**   Whether an exception is permitted; who approves; what justification;
                                how long it lasts; compensating controls; reporting. Or: no exception
                                is permitted.

**13. Escalation trigger.**     The event that requires escalation.

**14. AI application.**         What AI may assist with.

**15. AI prohibition.**         What AI must not decide, approve, certify, sign, waive, authorise or
                                represent as independently verified.

**16. AI verification.**        The specific human verification method. "Review the AI output" is
                                insufficient.

**17. External reference.**     Each authority listed separately with issuing organisation, title,
                                subject, edition or effective date checked, nature of authority,
                                verification date, applicability limitation.

**18. Jurisdictional caution.** What requires local legal, tax, accounting or regulatory advice.

**19. Related PCI Laws.**       Stable identifiers.

**20. Related Body of Knowledge content.** Certification · domain · Knowledge Area · topic.

**21. Compliance test.**        A test an auditor, assessor or reviewer can actually perform.

**22. Breach indicators.**      Observable indicators that the law may have been breached.

**23. Consequence within PCI authority.** From the Charter §9 list only.

**24. Examination application.** How the law can be tested by scenario judgement, evidence
                                selection, escalation decision, calculation review, ethical dilemma
                                or AI-verification case — not by memorising law numbers.

**25. Version and status.**     Version · approval date · effective date · amendment note.
```

### 5.1 On the compliance test (element 21)

This element is what separates a law from a slogan. It must be written so that a reviewer can perform
it and reach the same answer as another reviewer.

**Weak:** Compliance is demonstrated when the forecast has been properly reviewed.

**Strong:** Compliance is demonstrated when the approved forecast can be reconciled without
unexplained differences to the current cost ledger, the commitment register, the accrual schedule,
the approved change register, the risk-adjusted estimate and the authorised schedule status date.

### 5.2 On AI verification (element 16)

Name the method. Verification must specify whichever of these applies: independent recomputation,
source tracing, clause-to-summary comparison, sampling with a stated basis, reconciliation, boundary
testing, sensitivity analysis, expert judgement, or named approval.

### 5.3 On thresholds (element 11)

Do not invent arbitrary percentages. A threshold must be supported, explained, configurable by the
adopting organisation's governance, and tested for practical use on both a small project and a
megaproject. Where no defensible number exists, state the professional-judgement criteria instead and
say who applies them.

## 6. External reference classification

Every external reference is classified as exactly one of these, and the categories are never combined:

| # | Category | Notes |
|---|---|---|
| 1 | Applicable legislation or regulation | Only where jurisdiction and applicability are known |
| 2 | Authoritative financial-reporting standard | e.g. IFRS, IAS — mandatory only for entities applying that framework |
| 3 | International voluntary standard | e.g. ISO, IEC — voluntary unless adopted by regulation or contract |
| 4 | Contract framework | e.g. FIDIC, NEC — binds only parties who adopt it |
| 5 | Professional framework | e.g. PMBOK Guide, AACE TCM — not regulatory authority |
| 6 | Ethical code | Binding only where a body, regulator or engagement has adopted it |
| 7 | Industry guidance | No single authoritative publisher |
| 8 | Voluntary environmental or social framework | e.g. Equator Principles — voluntary, not legislation |
| 9 | PCI internal professional law | This system |
| 10 | Illustrative practice | Named to illustrate, not relied on |

**Specific rules that have already been breached once in this corpus and must not be again:**

- IFRS and IAS are financial-reporting standards, never PCI laws, and are mandatory only for entities
  applying IFRS Accounting Standards in a jurisdiction that has adopted them.
- The IFRS *Conceptual Framework* is **not a standard** — the IASB states so expressly, and nothing in
  it overrides a Standard. Never source a requirement to it.
- ISO references are voluntary international standards unless separately adopted by regulation or
  contract. Some ISO documents are certifiable requirements standards and some are guidance that
  nothing can be certified against; say which.
- FIDIC is a contract-form framework, not generally applicable legislation.
- PMI and AACE material must never be presented as regulatory authority.
- The Equator Principles are a voluntary financial-sector risk-management framework.
- Supervisory instruments (a banking supervisor's model-risk expectations, an internationally agreed
  supervisory framework) have no legal force of their own and apply only as a national authority
  transposes them or a supervised firm is subject to them.

**Never invent** a clause number, article, edition, effective date, judicial decision or requirement.
Where a precise provision is not verified, cite the instrument by name only. Every reference records
the date its currency was checked, and adds "verify current requirements" where the authority can
change.

## 7. Prohibited drafting patterns

| Pattern | Why it fails |
|---|---|
| A requirement inside a note, example, case study, figure caption or rationale | Charter §3 — obligations exist only in identified laws and process requirements |
| A requirement no one can verify | Unverifiable requirements cannot be audited, assessed or breached |
| A circular definition | Defines nothing |
| An undefined judgement word carrying the obligation | The requirement's content is unknown |
| Several obligations in one clause | Cannot be tested or breached precisely |
| `shall` | Not part of PCI's convention; ambiguous |
| `may not` for a prohibition | Ambiguous; use `must not` |
| A vague responsible role | Nobody is accountable |
| Duplicating an external standard without adding an obligation | Creates the impression PCI is restating law, and goes stale when the standard moves |
| Hedging inside the Normative requirement ("where practical", "should generally", "may wish to") | Qualifications belong in Scope, Threshold or Jurisdictional caution |
| Claiming a consequence outside PCI's authority | Charter §2 and §9 |

## 8. Visual presentation

Colour is never the only distinction. Every call-out carries a written label, an icon, a border
treatment and an identifier, so the categories survive greyscale printing, screen reading and
monochrome displays.

| Call-out | Written label | Colour | Tint | Icon | Border |
|---|---|---|---|---|---|
| Mandatory law | `PCI PROFESSIONAL LAW` | Dark PCI Law Red `#9B1C1C` | `#FDECEC` | § | solid left |
| External authority | `EXTERNAL STANDARD OR FRAMEWORK` | Standards Blue `#1D4ED8` | `#EEF4FF` | ⬢ | double |
| Recommended practice | `PCI RECOMMENDED PRACTICE` | Guidance Teal `#0F766E` | `#ECFDF5` | ✦ | dashed |
| Jurisdictional caution | `JURISDICTIONAL CAUTION` | Amber `#B45309` | `#FEF3C7` | ⚠ | dotted |

Body copy inside a call-out is black on the light tint. Colour carries the heading, border and label
only; large blocks of body text are never set in bright red.

## 9. The twenty-five audit questions

Before any law is approved, every question below must be answered. **A law failing one or more must
be revised before approval** — the failure and its resolution are recorded in the law's file.

1. What exact failure does this law prevent?
2. Is the requirement mandatory or only recommended?
3. Can a professional know whether it applies to them?
4. Is the responsible person identifiable?
5. Is the required action observable?
6. Is compliance provable?
7. Is the required evidence proportionate?
8. Can the law be audited?
9. Can the law be examined through a scenario?
10. Can a professional technically comply while defeating its purpose?
11. Does it conflict with another PCI law?
12. Does it duplicate an external standard unnecessarily?
13. Does it misrepresent external authority?
14. Does it require legal or jurisdiction-specific advice?
15. Does it define the relevant materiality threshold?
16. Does it cover AI use?
17. Does it preserve human accountability?
18. Does it contain an exception process?
19. Does it define escalation?
20. Is every important term defined?
21. Is the language concrete and modern?
22. Does it impose an impossible or excessive burden?
23. Can it operate on both small projects and megaprojects?
24. Can it operate internationally?
25. Is there a clear consequence within PCI's authority?

## 10. Governing principle

> **AI proposes; the professional verifies, decides and remains accountable.**
