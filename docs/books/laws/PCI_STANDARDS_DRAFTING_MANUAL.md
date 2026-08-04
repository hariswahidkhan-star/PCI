# PCI Standards Drafting Manual

**Status:** Binding drafting specification for every PCI Standard. Version 1.0.  
**Companion:** the **PCI Standards Charter** (status, hierarchy, priority, due process,
interpretation, amendment). This manual governs how a standard is *written*; the Charter governs what
a standard *is*. A standard must conform to both.

---

## 1. The normative language system

PCI uses **modern must-drafting**. This is a deliberate choice and it is exclusive: PCI does not use
ISO-style `shall` drafting, and the two systems are never mixed.

| Word | Meaning in a PCI Standard | Use it for |
|---|---|---|
| **must** | Mandatory PCI professional requirement | The obligation itself |
| **must not** | Prohibited practice | Conduct that constitutes a breach |
| **should** | Recommended practice; a justified alternative may be acceptable | Level 5 recommendations only — see §1.3 |
| **may** | Permission | Something the professional is allowed to do |
| **can** | Capability or possibility — never permission | Describing what a person, tool or method is able to do |

**`shall` is not used as a normative form anywhere in a PCI Standard**, in any field, including quotations
of PCI's own earlier drafts. Legislative `shall` is ambiguous — it has been read as both obligation
and futurity — and modern drafting practice in the UK and elsewhere has moved to `must` for precisely
that reason. A draft that uses `shall` to impose, permit or recommend anything fails gate.

**The ban is on the form, not on the word.** Explanatory material *about* the convention — the ISO
mapping in §1.0, this paragraph, a supersession note recording that an earlier draft used the word —
may name `shall` in order to say that PCI does not use it. Refusing to print the word while trying to
explain the mapping produces a circumlocution the reader cannot follow, which defeats the purpose of
stating the mapping at all. The gate check is therefore: zero occurrences inside any standard's
twenty-five elements or any process requirement; occurrences permitted only in front matter that explains the
convention, and only in that explanatory sense.

### 1.0 The `must` inversion — read this before drafting anything

PCI's choice of `must` is **the exact inverse of ISO's use of that same word**, and this is the single
most dangerous ambiguity in the whole system.

In ISO/IEC drafting convention, `shall` expresses the document's own requirement, and **`must` is
reserved for a constraint or obligation defined *outside* the document, stated for information** —
using it does not make that external constraint a requirement of the document. So an ISO-literate
reader meeting a PCI Standard that says *"the professional must independently verify…"* may read it as
*"somebody else's rule, mentioned in passing"* — the precise opposite of what PCI intends.

Three consequences follow, and none is optional:

1. **Every PCI Standards publication must carry an explicit disclaimer of ISO verbal-form
   conventions**, in the "How to read these standards" note, in this form or wording to the same
   effect:

   > These standards do not follow ISO/IEC verbal-form conventions. In a PCI Standard, **`must`
   > states PCI's own mandatory requirement** — the role that an ISO document gives to `shall`. PCI
   > does not use `shall` at all, and does not use `must` for external constraints.

2. **PCI needs a different device for signalling an external constraint**, because `must` is no
   longer available for it. See §1.2 — external constraints are stated with `may impose` and are
   confined to element 17 (External reference) and element 18 (Jurisdictional caution). An external
   obligation is never expressed as a PCI `must`.

3. **A legal or statutory obligation is never restated as a PCI requirement.** Voluntary-standard
   practice excludes restating law as the document's own requirement, and PCI follows it: legal
   obligations appear only as external constraints or jurisdictional cautions, never as a PCI standard.

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

External constraints live only in element 17 (External reference) and element 18 (Jurisdictional
caution). They never appear in element 1 (Normative requirement), element 5 (Required actions) or
element 6 (Prohibited actions), because those elements state what **PCI** requires.

### 1.3 `should` — recommendation, not a disguised requirement

A recommendation permits a justified alternative. It does **not** become a requirement because a
reason must be recorded for departing from it.

If PCI wants to say *"do this, or record why you did something else"*, that is a distinct device — a
requirement to record a departure — and it must be written as one:

> **Recommended practice.** The review should include sensitivity testing proportionate to the
> materiality of the decision.
>
> **Associated requirement.** Where the review omits sensitivity testing, the reviewer must record
> the reason for the omission in the review record.

Written that way, the recommendation stays a recommendation and the recording obligation is a real,
testable requirement with its own compliance test. Written the other way — "should … and a reason
must be recorded for any departure" bundled into one clause — it is neither: it reads as a
recommendation, binds like a requirement, and cannot be assessed as either.

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
| Foundational Standard | `PCI-FND-STD-NN` | `PCI-FND-STD-04` |
| Certification / Domain Standard | `PCI-<CRED>-STD-DD.NN` | `PCI-PCL-STD-06.03` |
| Process Requirement | `<parent>-PR-NN` | `PCI-PCL-STD-06.03-PR-01` |
| Recommended Practice | `<parent>-RP-NN` | `PCI-PFL-STD-10.01-RP-02` |

`<CRED>` is `PCL`, `PFL` or `PML`. `DD` is the two-digit Body of Knowledge domain of primary
anchorage. `NN` is a two-digit sequence within that domain.

Identifiers are **stable**. All internal citation is by identifier — never by page number, because
pagination changes. A withdrawn standard's identifier is never reused.

## 4. Defined terms

Any term that could alter whether a professional has complied must be defined in the standard that uses
it, or in the **PCI Standards Definitions Register**, and used consistently.

Terms that always need definition when they appear in an obligation: *material*, *independent*,
*approved*, *verified*, *current*, *competent reviewer*, *evidence*, *decision owner*, *exception*,
*escalation threshold*, *promptly*, *appropriate*, *adequate*, *reasonable*.

**No circular definitions.** "A material variance is a variance that is material" defines nothing.
Define by the test a reader can apply: what makes it material, measured against what, decided by whom.

**Undefined judgement words are a drafting defect.** *Appropriate*, *adequate*, *reasonable*,
*relevant*, *timely* and *sufficient* must either be defined, replaced with a stated test, or removed.

## 5. The mandatory structure of a standard

Every PCI Standard carries all twenty-five elements below, in this order. No element may be omitted. Where
an element is genuinely inapplicable, it states "Not applicable" **and gives the reason in one
clause** — a bare "None." is a defect.

The heading is `## PCI STANDARD <identifier> — <official title>`, and the twenty-five elements
follow it in this order:

| # | Element | What it carries |
|---|---|---|
| 1 | **Normative requirement** | One precise mandatory statement. One principal obligation. |
| 2 | **Purpose** | The professional risk this standard controls. |
| 3 | **Scope** | Who is governed · which decisions · which projects, programmes or transactions · whether it applies to preparation, review, recommendation, approval or assurance. |
| 4 | **Defined terms** | Every term in this standard that could alter compliance. |
| 5 | **Required actions** | The minimum actions necessary for compliance. |
| 6 | **Prohibited actions** | The practices that constitute a breach. |
| 7 | **Required evidence** | What must exist to prove compliance. |
| 8 | **Responsible role** | The role accountable for performance. Never "the team", "management", "relevant people", "appropriate personnel" or "the organisation" unless that term is formally defined. |
| 9 | **Approval authority** | Who may approve, reject, waive or escalate. |
| 10 | **Independence requirement** | Whether the reviewer must be independent of preparation, commercial benefit, model development, contract administration, project sponsorship or AI-tool configuration. |
| 11 | **Materiality or threshold** | Numerical, risk, authority, cumulative or trigger-event threshold, or the professional-judgement criteria that stand in for one. |
| 12 | **Exception and waiver** | Whether an exception is permitted; who approves; what justification; how long it lasts; compensating controls; reporting. Or: no exception is permitted. |
| 13 | **Escalation trigger** | The event that requires escalation. |
| 14 | **AI application** | What AI may assist with. |
| 15 | **AI prohibition** | What AI must not decide, approve, certify, sign, waive, authorise or represent as independently verified. |
| 16 | **AI verification** | The specific human verification method. "Review the AI output" is insufficient. |
| 17 | **External reference** | Each authority listed separately with issuing organisation, title, subject, edition or effective date checked, nature of authority, verification date, applicability limitation. |
| 18 | **Jurisdictional caution** | What requires local legal, tax, accounting or regulatory advice. |
| 19 | **Related PCI Standards** | Stable identifiers. |
| 20 | **Related Body of Knowledge content** | Certification · domain · Knowledge Area · topic. |
| 21 | **Compliance test** | A test an auditor, assessor or reviewer can actually perform. |
| 22 | **Breach indicators** | Observable indicators that the standard may have been breached. |
| 23 | **Consequence within PCI authority** | From the Charter §9 list only. |
| 24 | **Examination application** | How the standard can be tested by scenario judgement, evidence selection, escalation decision, calculation review, ethical dilemma or AI-verification case — not by memorising standard numbers. |
| 25 | **Version and status** | Version · approval date · effective date · amendment note. ``` |
### 5.0 Which elements are normative

A standard's twenty-five elements do not all carry obligation, and the structure must say which do.
Without this declaration a reader cannot tell whether a sentence in, say, element 22 binds them —
and an assessor cannot tell what to assess.

| Elements | Status | Effect |
|---|---|---|
| 1 Normative requirement · 5 Required actions · 6 Prohibited actions · 7 Required evidence · 12 Exception and waiver · 13 Escalation trigger · 15 AI prohibition · 16 AI verification | **Normative** | These bind. A breach of any of them is a breach of the standard. |
| 3 Scope · 4 Defined terms · 8 Responsible role · 9 Approval authority · 10 Independence requirement · 11 Materiality or threshold | **Normative — determinative** | These do not add obligation; they determine to whom, and in what circumstances, the normative elements apply. Getting one wrong changes what the standard requires. |
| 14 AI application | **Permissive** | States what is allowed. Creates no obligation. |
| 2 Purpose · 17 External reference · 18 Jurisdictional caution · 19 Related PCI Standards · 20 Related BoK content · 21 Compliance test · 22 Breach indicators · 23 Consequence · 24 Examination application · 25 Version and status | **Informative** | Explain, locate, test or administer the standard. **They must not contain an obligation.** |

Two consequences worth stating because both are easy to get wrong:

- **The compliance test (21) is informative.** It describes how to test the obligation stated in the
  normative elements; it must never be the only place an obligation appears. If a reviewer can find
  a requirement in element 21 that is not in element 1, 5, 6 or 7, that is a drafting defect.
- **Breach indicators (22) are informative.** An indicator suggests the standard may have been
  breached; it is not itself the breach, and it must not read as one.

### 5.1 On the compliance test (element 21)

This element is what separates a standard from a slogan. It must be written so that a reviewer can perform
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
| 9 | PCI internal professional standard | This system |
| 10 | Illustrative practice | Named to illustrate, not relied on |
| 11 | **National standard** | A published standard of one country's standards body (e.g. an ANSI-accredited US standard). Binds only where a contract or procurement regime imports it. Not an international standard, not industry guidance |
| 12 | **Supervisory guidance** | A supervisor's published expectations, or an internationally agreed supervisory framework. **No legal force of its own**; applies only as a national authority transposes it or a supervised firm is subject to it. Never tag it as regulation |

> **Note on categories 11 and 12.** The programme brief specified ten categories. Two more were added
> because the ten had no accurate home for instruments the corpus actually cites, and forcing them
> into an existing category would have been exactly the misclassification the scheme exists to
> prevent: ANSI/EIA-748 is a *national* standard, not an international one and not industry practice;
> the Basel framework and a banking supervisor's model-risk expectations are *supervisory*, and
> tagging either as regulation would state that they bind directly, which they do not. Category 3
> ("international voluntary standard") also does not fit a non-international voluntary framework such
> as the NIST AI RMF or the OECD AI Principles — classify those under category 7 or 8 by their
> subject, and state in the reference itself that they are voluntary and where they originate.
> These two additions are recorded here as a deviation from the brief, with the reason, rather than
> made silently.

**Specific rules that have already been breached once in this corpus and must not be again:**

- IFRS and IAS are financial-reporting standards, never PCI Standards, and are mandatory only for entities
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

**The category describes the instrument, not the use PCI makes of it.** An instrument has one
category everywhere it appears; how a particular standard relies on it is a separate statement.
Writing "category 10 — illustrative practice" for the EU AI Act because *this* standard only cites it
to illustrate a shape gives the same instrument two categories across the corpus, which is the
inconsistency the classification exists to prevent. Legislation cited illustratively is still
legislation: give it category 1 and say in the same bullet that it is relied on for no requirement in
this standard. The pattern to follow is: **`category` (what it is) · `relied on for` (what this
standard does with it) · `limitation` (where and on whom it bites).**

**Never invent** a clause number, article, edition, effective date, judicial decision or requirement.
Where a precise provision is not verified, cite the instrument by name only. Every reference records
the date its currency was checked, and adds "verify current requirements" where the authority can
change.

## 7. Prohibited drafting patterns

| Pattern | Why it fails |
|---|---|
| A requirement inside a note, example, case study, figure caption or rationale | Charter §3 — obligations exist only in identified standards and process requirements |
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
| Mandatory standard | `PCI PROFESSIONAL STANDARD` | Dark PCI Standard Red `#9B1C1C` | `#FDECEC` | § | solid left |
| External authority | `EXTERNAL STANDARD OR FRAMEWORK` | Standards Blue `#1D4ED8` | `#EEF4FF` | ⬢ | double |
| Recommended practice | `PCI RECOMMENDED PRACTICE` | Guidance Teal `#0F766E` | `#ECFDF5` | ✦ | dashed |
| Jurisdictional caution | `JURISDICTIONAL CAUTION` | Amber `#B45309` | `#FEF3C7` | ⚠ | dotted |

Body copy inside a call-out is black on the light tint. Colour carries the heading, border and label
only; large blocks of body text are never set in bright red.

## 9. The twenty-five audit questions

Before any standard is approved, every question below must be answered. **A standard failing one or
more must be revised before approval** — the failure and its resolution are recorded in the
standard's file.

1. What exact failure does this standard prevent?
2. Is the requirement mandatory or only recommended?
3. Can a professional know whether it applies to them?
4. Is the responsible person identifiable?
5. Is the required action observable?
6. Is compliance provable?
7. Is the required evidence proportionate?
8. Can the standard be audited?
9. Can the standard be examined through a scenario?
10. Can a professional technically comply while defeating its purpose?
11. Does it conflict with another PCI Standard?
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
