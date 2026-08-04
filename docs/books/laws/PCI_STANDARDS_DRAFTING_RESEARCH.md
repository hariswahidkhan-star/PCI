# PCI Standards Drafting Research Report

**Programme:** Reconstruction of the PCI Standards (PCL-AI, PFL-AI, PML-AI)
**Stage:** 2 — establishing how high-quality normative requirements are actually written
**Verification date:** 2026-08-04
**Status:** Research input. This document is not itself a law, a rule or a drafting standard. It
informs the revision of `SUPERSEDED_LAW_SYSTEM_v0.md` and the drafting of the law sets.
**Language:** British English.

---

## 1. Why this report exists

PCI Standards are intended to become testable, enforceable, evidence-based professional
requirements. Before drafting any law, the programme needs a defensible answer to a prior question:
*how do the bodies that write normative requirements well actually do it?*

This report extracts **drafting principles, structures and governance methods** from the official
guidance of bodies whose entire business is writing rules that other people must comply with. It
extracts method, not text. Nothing protected is reproduced (see §3 and §9).

Three things this report deliberately does **not** do:

1. It does not propose law wording. That is Stage 3.
2. It does not assert any clause number, edition, effective date or requirement that could not be
   corroborated on 2026-08-04. Where a detail could not be corroborated it is marked
   **UNVERIFIED** in place, not guessed.
3. It does not claim PCI is, or should represent itself as, equivalent to any of the bodies studied.

---

## 2. Verification method — and an honest statement of its limits

### 2.1 What happened

The research plan was to fetch each issuing body's own published page and record what was actually
on it. **This was not possible.** Every outbound HTTPS request from this session was refused at the
egress proxy with `403` (organisation policy denial), including:

| Host attempted | Result |
|---|---|
| `www.iso.org` | 403 CONNECT rejected — policy denial |
| `www.iec.ch` | 403 CONNECT rejected — policy denial |
| `share.ansi.org` | 403 CONNECT rejected — policy denial |
| `www.ifrs.org` | 403 CONNECT rejected — policy denial |
| `www.gov.uk` | 403 CONNECT rejected — policy denial |
| `metanorma.github.io` | 403 CONNECT rejected — policy denial |
| `en.wikipedia.org`, `example.com` (control tests) | 403 CONNECT rejected — policy denial |

The proxy's own diagnostics recorded these as `connect_rejected — gateway answered 403 to CONNECT
(policy denial or upstream failure)`. Per the environment's operating rules, policy denials were
reported rather than routed around. **No page was fetched directly in this session.**

Web search remained available and returns content extracted from indexed pages, including the
issuing bodies' own pages. All findings below therefore rest on that channel.

### 2.2 Verification tiers used throughout this report

Every source and every row in the main table carries one of these tiers. They are not
interchangeable and the distinction is load-bearing.

| Tier | Meaning |
|---|---|
| **F — Fetched** | The issuing body's own page was retrieved and read in this session. **No source in this report reaches tier F.** |
| **S — Search-corroborated** | The issuing body's own page (or its own hosted PDF) was returned by web search and its content reported by the search tool, consistent across two or more independently phrased queries. This is the highest tier achieved here. |
| **S¹ — Single-pass search** | Reported by the search tool from an official page but corroborated by only one query. Treat as indicative. |
| **U — Unverified** | Could not be corroborated on 2026-08-04. Recorded as an open item, never asserted as fact. |

**Consequence for the programme:** everything in this report should be re-verified against the
issuing bodies' own pages before any law text relies on it, and before any PCI publication cites it.
A re-verification checklist is at §10.

### 2.3 A note on precision

Search-tool summaries are paraphrases. They are good evidence of *what a source says in substance*
and weak evidence of *exact wording, clause numbering and current edition*. This report therefore
states substance and marks numbering and editions as UNVERIFIED wherever they were not
independently corroborated. That restraint is deliberate: PCI's own drafting rule already forbids
inventing clause numbers, editions and effective dates, and this report is held to the same rule.

---

## 3. Copyright position adopted for this exercise

The bodies studied publish under different terms. The position taken throughout is the strictest one
consistent with useful research:

- **ISO/IEC Directives Part 2, ISO/IEC 17024, ISO/IEC 17021-1, ISO guidance publications** —
  © ISO/IEC. Copyright-protected, sold or licensed. **Method and structure may be described in PCI's
  own words. Clause text, the verbal-forms tables, definitions and any table layout must not be
  reproduced, in whole or in substance, in any PCI publication.** PCI may name an instrument and
  characterise its relevance; the official publication governs.
- **IFRS Foundation Due Process Handbook and IFRS material** — © IFRS Foundation, all rights
  reserved. **Process design may be described in PCI's own words. Handbook paragraphs, defined
  terms and Basis-for-Conclusions text must not be reproduced.** IFRS, IASB, ISSB and the hexagon
  device are trade marks; PCI must not use them in any way implying endorsement, affiliation or
  process equivalence.
- **UK Office of the Parliamentary Counsel (OPC) *Drafting Guidance*** — published on GOV.UK. GOV.UK
  material is normally issued under the Open Government Licence v3.0, which permits copying and
  adaptation with attribution. **The licence footer of the specific document could not be verified on
  2026-08-04 (UNVERIFIED).** Until it is, treat the guidance as restate-in-own-words and do not
  reproduce passages.
- **FCA Handbook and FCA publications** — © Financial Conduct Authority. Licence terms not verified
  on 2026-08-04 (UNVERIFIED). Restate in own words; do not reproduce provisions or the Reader's
  Guide text.

A single rule covers all of them and should be carried into PCI's drafting convention:

> **PCI reproduces no protected text. PCI names the instrument, states in its own words why the
> instrument matters, and directs the reader to the official publication, which governs.**

This is already PCI's stated position in `SUPERSEDED_LAW_SYSTEM_v0.md` §5. This report confirms it is the right
one and adds that it must apply to *method* documents (the Directives, the Handbook, the Drafting
Guidance) exactly as it applies to *subject-matter* standards (IFRS 15, ISO 21500 and the like).

---

## 4. Source register

Five primary sources plus two supporting sources were used. All are the issuing bodies' own
publications. None reached tier F because of the egress block described in §2.1.

### Source 1 — ISO/IEC Directives, Part 2

*Principles and rules for the structure and drafting of ISO and IEC documents*

| Field | Detail |
|---|---|
| Issuing bodies | International Organization for Standardization (ISO) and International Electrotechnical Commission (IEC) |
| Official pages identified | `https://www.iso.org/sites/directives/current/part2/index.xhtml`; `https://www.iec.ch/standards-development/isoiec-directives-part-2`; IEC drafting-guidance pages under `https://www.iec.ch/standardsdev/resources/draftingpublications/directives/` (verbal forms, scope, terms and definitions, normative references, annexes) |
| Verification tier | **S** — content reported by search across multiple independent queries; both official domains blocked for direct fetch |
| Edition | **UNVERIFIED.** Search returned document titles indicating a *Ninth edition, 2021* / *Edition 9.0 2021-05*, but also a reference to an eighth edition and to later amendment activity. The current edition as at 2026-08-04 was not established. **PCI must not cite an edition of the Directives until this is confirmed against ISO or IEC directly.** |
| Why it matters to PCI | It is the most developed public treatment anywhere of the distinction between a requirement, a recommendation, a permission and a statement of capability — the exact distinction PCI's law sets must make on every page. |

### Source 2 — IFRS Foundation *Due Process Handbook*

| Field | Detail |
|---|---|
| Issuing body | IFRS Foundation (Trustees; Due Process Oversight Committee) |
| Official pages identified | `https://www.ifrs.org/groups/due-process-oversight-committee/due-process-handbook/`; `https://www.ifrs.org/about-us/our-due-process/`; `https://www.ifrs.org/about-us/how-we-set-ifrs-standards/`; handbook PDFs under `https://www.ifrs.org/content/dam/ifrs/about-us/legal-and-governance/constitution-docs/` |
| Verification tier | **S** |
| Edition | A revised Due Process Handbook dated **April 2026** was corroborated (search returned an ifrs.org PDF titled *April 2026 Due Process Handbook* and an ifrs.org news item announcing publication of the revised standard-setting handbook in April 2026; a publication date of 30 April 2026 was reported). The revision followed an Exposure Draft published by the Trustees in December 2024. **The precise date and the paragraph structure are S, not F.** Earlier editions (June 2016; a 2020 revision) were also corroborated. |
| Why it matters to PCI | It is a published, auditable specification of *how a rule-making body must behave* — the governance half of the problem, which drafting rules alone do not solve. |

### Source 3 — UK Office of the Parliamentary Counsel, *Drafting Guidance*, and the Good Law initiative

| Field | Detail |
|---|---|
| Issuing body | Office of the Parliamentary Counsel (Cabinet Office), UK |
| Official pages identified | `https://www.gov.uk/government/publications/drafting-bills-for-parliament`; `https://www.gov.uk/government/publications/drafting-bills-for-parliament/2024-03-19-drafting-guidance`; PDF at `https://assets.publishing.service.gov.uk/media/660407d091a320001a82b06b/2024.03.19.Drafting-guidance.pdf`; Good Law guidance at `https://www.gov.uk/guidance/good-law`; *When Laws Become Too Complex* at `https://assets.publishing.service.gov.uk/media/5a7a2ce9e5274a34770e4c80/GoodLaw_report_8April_AP.pdf` |
| Verification tier | **S** |
| Version | A version dated **19 March 2024** was corroborated from the GOV.UK page title and PDF filename. Whether a later version exists as at 2026-08-04 is **UNVERIFIED**. |
| Why it matters to PCI | It is the working guidance of a drafting office that has consciously abandoned the legislative "shall" — the exact transition PCI has decided to make. |

### Source 4 (chosen) — ISO/IEC 17024, personnel certification

*Conformity assessment — General requirements for bodies operating certification of persons*

| Field | Detail |
|---|---|
| Issuing bodies | ISO and IEC |
| Official page identified | `https://www.iso.org/standard/17024` (also `https://www.iso.org/standard/52993.html` for the 2012 edition and `https://www.iso.org/standard/29346.html` for 2003); ISO guidance publication *How to develop schemes for the certification of persons* at `https://www.iso.org/publication/PUB100384.html` |
| Verification tier | **S** for substance; edition **S¹** |
| Edition | Search consistently indicated an **ISO/IEC 17024:2026** edition superseding ISO/IEC 17024:2012, following a revision opened in 2023. Corroborated by ISO and CEN catalogue entries returned in search. **Not confirmed against iso.org directly — treat the edition as S¹ and re-verify before citing.** |
| Why chosen | It supplies what the first three sources lack entirely: how a *certification body* must govern a scheme, run defensible examinations, and give candidates appeal rights. It is the closest published analogue to what PCI actually is. |

### Source 5 (chosen) — FCA Handbook (Reader's Guide, GEN 2, SUP 8, SUP 9)

| Field | Detail |
|---|---|
| Issuing body | Financial Conduct Authority (UK) |
| Official pages identified | `https://handbook.fca.org.uk/handbook`; `https://handbook.fca.org.uk/handbook-readers-guide`; `https://www.fca.org.uk/publication/handbook/readers-guide_0.pdf`; `https://www.handbook.fca.org.uk/handbook/GEN/2/2.html`; `https://handbook.fca.org.uk/handbook/sup8`; `https://www.handbook.fca.org.uk/handbook/SUP/9/4.html`; `https://www.fca.org.uk/publication/corporate/statement-policy-cba.pdf` |
| Verification tier | **S** |
| Why chosen | It is the best publicly readable demonstration of a rulebook that *labels the legal status of every single provision on the face of the text*, and that provides formal machinery for exceptions (waivers), interpretation (individual guidance) and graded evidential weight. This is precisely the machinery PCI's law sets currently lack. |

### Supporting source A — ISO/IEC 17021-1 (nonconformity classification)

`https://www.iso.org/obp/ui/en/#!iso:std:61651:en`. Tier **S**. Used only for how non-fulfilment of a
requirement is defined and graded. Edition indicated as 2015 — **S¹**, not confirmed.

### Supporting source B — ISO, *Conformity assessment for standards writers — Do's and don'ts*

`https://www.iso.org/publication/PUB100303.html` (PDF at `https://www.iso.org/iso/PUB100303.pdf`).
Tier **S**. Used for the relationship between how a requirement is written and whether conformity
can be assessed at all.

---

## 5. The twenty questions, answered

Each answer states the mechanism, the source that answers it, and what it means for PCI. Tiers are
as defined in §2.2.

**1. How is a mandatory requirement distinguished from guidance?**
By a fixed vocabulary, applied without exception, and — in the best rulebooks — by a visible status
marker on the provision itself. ISO/IEC Directives Part 2 reserves one auxiliary for requirements
and different auxiliaries for recommendations, permissions and statements of capability, precisely
so that a reader can identify what must be satisfied in order to claim compliance (S). The FCA goes
further and marks the legal character of every provision with a status letter carried in the margin
or heading, so status is never inferred from tone (S). *PCI implication:* vocabulary alone is not
enough; PCI should carry both a controlled vocabulary and a visible status label, which its existing
box system (`PCI STANDARD` / `PCI PRACTICE GUIDANCE` / `EXTERNAL REFERENCE` / `CAUTION`) already
half-implements.

**2. How is scope defined?**
As a discrete, mandatory, normative opening element that delimits the subject and the limits of
applicability, worded as statements of fact, appearing once, and containing no requirements,
permissions or recommendations (ISO/IEC Directives Part 2, S). *PCI implication:* PCI's `Scope`
field must state *who and what* is covered and must not smuggle in obligations — the obligations
belong in the Rule and the Minimum professional requirement.

**3. How are defined terms controlled?**
By a normative terms clause, the substitution principle (a definition must be capable of replacing
the term in context), a prohibition on circular definitions, a prohibition on definitions taking the
form of or containing a requirement, and formal rules on form (ISO/IEC Directives Part 2, S). The
FCA controls defined terms differently but as strictly: defined expressions appear in italics
wherever they occur, so a reader always knows a special meaning is in play (S). *PCI implication:*
one glossary, one meaning per term, one term per meaning, substitution-tested, and typographically
signalled in the law text.

**4. How is applicability established?**
Separately from scope and separately from the obligation. ISO/IEC Directives Part 2 requires
applicability statements in the Scope, introduced by set wording (S). The FCA structures most
modules with an explicit *Application and purpose* section stating who the module binds before any
substantive provision appears (S). ISO/IEC 17024 fixes applicability through the certification
scheme, which defines the population of persons a requirement bears on (S). *PCI implication:*
every PCI law needs an unambiguous answer to "does this bind me, today, in my role?" — currently
carried by `Scope`, which should be split into *applies to whom* and *applies to what work*.

**5. How are exceptions written?**
Two distinct patterns exist and PCI needs both. (i) *Drafted-in exceptions* — legislative practice
(OPC, S) handles conditions and carve-outs within the provision's own structure, using modern
conditional forms rather than archaic provisos. (ii) *Granted exceptions* — the FCA operates a
formal waiver and modification regime with published statutory conditions: the regulator may not
grant relief unless satisfied that compliance would be unduly burdensome or would not achieve the
purpose for which the rules were made, and the waiver would not adversely affect its objectives;
waivers are published unless publication is inappropriate or unnecessary (S). *PCI implication:*
PCI should stop treating exceptions as prose hedges inside the Rule and create a named, conditioned,
recorded relief mechanism with published criteria.

**6. How is responsibility assigned?**
By naming the actor who must act, not by describing a desirable state. ISO/IEC Directives Part 2
distinguishes instructions (imperative mood, addressed to whoever performs the step) from
requirements (auxiliary form, with the bearer identified), and prohibits drifting between them
(S). ISO/IEC 17024 assigns responsibility structurally — the certification body, the scheme
committee/scheme owner, the examiner — and requires the certification decision to be made
independently of training (S). *PCI implication:* PCI's `Decision owner` field is the right
instrument; it must name a role that exists, and every Rule must be traceable to exactly one
accountable role.

**7. How is evidence of compliance specified?**
By making verifiability a condition of the requirement's existence. ISO/IEC Directives Part 2 admits
only requirements that convey objectively verifiable criteria, excludes subjective formulations, and
excludes properties for which no test method can verify the claim in a reasonable time (S). ISO's
guidance for standards writers makes the corollary explicit: a document containing no requirements
is not intended to be used for conformity assessment at all (S). ISO/IEC 17024 requires assessment
against competence criteria derived from a job or practice analysis (S). *PCI implication:* PCI's
`Required evidence` field is the load-bearing field of the whole template. If a law's evidence field
cannot name something a reviewer could actually inspect, the law is not a law yet.

**8. How is independent review required?**
By structural separation rather than by exhortation. ISO/IEC 17024 requires impartiality, separation
of certification decisions from training interests, and management of conflicts of interest (S). The
IFRS Foundation places oversight of process compliance with the Trustees acting through the Due
Process Oversight Committee — a body distinct from the standard-setters it oversees (S). *PCI
implication:* PCI's `Independent review` field must specify *independent of what*, and PCI needs at
least one reviewer role that is structurally separate from the person who produced the work.

**9. How are prohibitions stated?**
As a negative form of the mandatory auxiliary, in the same controlled vocabulary as the positive
requirement, so that prohibition is not a matter of tone (ISO/IEC Directives Part 2, S; OPC modern
drafting, S). *PCI implication:* PCI's `Prohibited practice` field must use the negative of PCI's
chosen mandatory auxiliary and nothing else — never "avoid", "refrain from where possible" or
"should not".

**10. How are permissions stated?**
With a dedicated auxiliary distinct from both the mandatory and the recommendatory forms, and
distinct again from statements of possibility or capability (ISO/IEC Directives Part 2, S). *PCI
implication:* PCI's `AI application` field currently reads as a permission and must be written as
one, explicitly, or it will be read as a requirement to use AI.

**11. How are external constraints referenced?**
Three mechanisms, all relevant. (i) ISO/IEC Directives Part 2 reserves a separate verbal form for
constraints or obligations defined *outside* the document, given for the user's information, and
states that using it does not make the external constraint a requirement of the document (S).
(ii) It excludes contractual and legal or statutory requirements from the document's own
requirements (S). (iii) It controls incorporation by reference: only references whose content
constitutes requirements go in the normative references list; for dated references only the cited
edition applies, and for undated references the latest edition including amendments applies (S).
*PCI implication:* this is the single most directly transferable finding in the report — see §7(a)
and §7(d).

**12. How are conflicts between requirements resolved?**
By an express precedence rule and by drafting consistency. ISO/IEC Directives Part 2 attacks the
problem preventively: consistency within and across documents, identical wording for identical
provisions, one terminology, no synonyms (S). The IFRS architecture resolves status conflicts by
making the accompanying material subordinate — a Basis for Conclusions accompanies but is not part
of the Standard (S), and explanatory material in an Interpretations Committee agenda decision
derives its authority from the Standards themselves (S). *PCI implication:* PCI needs a written
precedence order (external law > authoritative standard > PCI law > PCI rule > guidance > example)
stated as a foundational law, not merely as front matter.

**13. How is non-compliance classified?**
Graded, against the requirement, on stated criteria. ISO/IEC 17021-1 defines a nonconformity as
non-fulfilment of a requirement and separates major from minor by whether the failure affects the
capability of the system to achieve intended results, treating a pattern of minor failures against
the same requirement as evidence of systemic failure; findings must be identified, classified and
recorded so that an informed decision can be made, and a nonconformity must not be recorded as
merely an opportunity for improvement (S). The FCA supplies a second gradation: guidance is not
binding and departure from it raises no presumption of breach, whereas evidential provisions —
though not binding in their own right — may be relied on as tending to establish contravention of
the underlying rule (S). *PCI implication:* PCI's `Consequences of breach` field should classify,
not narrate; and PCI needs to decide whether departure from a `should` is evidentially relevant
(see §7(a)).

**14. How is appeal or interpretation handled?**
As separate machinery from the rule itself. ISO/IEC 17024 requires accessible, fair and impartial
complaints and appeals procedures for candidates and certificants (S). The FCA publishes individual
guidance and states the effect of relying on it, and treats guidance as the FCA's view that does not
bind the courts (S). The IFRS Foundation routes application questions to the Interpretations
Committee, which may publish an agenda decision with explanatory material after public consultation,
or conclude that an amendment is needed instead (S). *PCI implication:* PCI needs a named
interpretation route and a named appeal route, both published, both with time limits, and both
separate from the law text.

**15. How are requirements amended?**
Through the same process that created them, with the change recorded. The IFRS Due Process Handbook
governs amendments to Standards, and the Handbook itself was amended through public consultation —
an Exposure Draft in December 2024 leading to a revised Handbook in April 2026 (S). The FCA records
Handbook changes through published instruments and Handbook Notices, and the Handbook is presented
as a dated, versioned text (S). ISO/IEC Directives Part 2 governs revision and amendment of
documents and controls how references to amended documents behave (S). *PCI implication:* PCI needs
a law amendment register, dated versions of each law, and a rule that no law changes silently.

**16. How are stakeholders consulted?**
On published minimum terms. The IFRS Foundation's due process rests on transparency, full and fair
consultation, and accountability; technical discussions are held in public, meeting papers are
published, comment letters are published, and an Exposure Draft normally carries a minimum comment
period of 120 days, with a shorter minimum (reported as 60 days) available for narrow re-exposure
(S). Priorities are set through a five-yearly agenda consultation (S). The FCA consults through
consultation papers that must explain the purpose of the proposed rules, invite representations
within a stated period, and be followed by feedback (S). *PCI implication:* PCI can adopt the
*shape* of this honestly at a fraction of the scale — see §7(b).

**17. How is implementation impact assessed?**
Before the requirement is made, and again after. The IFRS Foundation analyses the likely effects,
costs and benefits, of proposals, at all stages of standard-setting, and explains the rationale for
decisions (S). The FCA is required to publish a cost benefit analysis of proposed rules, with a
narrow exemption where cost increases are absent or of minimal significance, and has established a
CBA Panel and a published Statement of Policy on how it conducts such analysis (S). *PCI
implication:* PCI must ask, for every law, what it will cost a candidate or credential holder to
comply and to evidence compliance — and record the answer.

**18. How is conformity or compliance tested?**
By writing the requirement so that conformity is assessable in the first place, and then keeping the
assessment method neutral. ISO's guidance for standards writers records that a document without
requirements is not intended for conformity assessment, that a preference for one type of assessment
over another should not be stated, and that sector-specific conformity-assessment provisions should
be separated from technical requirements so they can be applied independently (S). ISO/IEC 17024
requires assessment methods to be valid, reliable, objective and periodically reviewed (S). *PCI
implication:* PCI's `Verification requirement` field must state the check, not the aspiration, and
must not presuppose that only PCI can perform it.

**19. How are requirements converted into examination scenarios?**
Through a documented chain from practice to competence to assessment. ISO/IEC 17024 requires the
certification scheme to rest on a job or practice analysis that identifies the tasks required for
successful performance, the competence required for each task, any prerequisites, and the assessment
mechanisms and examination content; the examination then measures those competences using objective
criteria and scoring, with the whole scheme subject to validation (S). *PCI implication:* PCI's
`Examination relevance` field should be the visible end of an auditable chain
(law → competence → scenario → scoring rubric), not a free-text note. Nothing in that chain may
expose live examination content.

**20. How is an audit trail maintained?**
By publishing the reasoning and preserving the record. The IFRS Foundation publishes meeting papers,
comment letters, feedback statements and a Basis for Conclusions explaining why the body decided as
it did (S), and its Trustees monitor process compliance through the Due Process Oversight Committee,
which also handles complaints about due process (S). ISO/IEC 17024 requires records across the
certification lifecycle — application, assessment, examination, decision, certification,
recertification, suspension and withdrawal, appeals and complaints (S). ISO/IEC 17021-1 requires
findings to be recorded so an informed decision can be made (S). The FCA maintains a dated,
versioned Handbook with published change instruments (S). *PCI implication:* PCI already has an
`audit_logs` mechanism in its platform; the law programme needs the documentary equivalent — a
decision record per law, retained, and a published change history.

---

## 6. Extracted drafting principles

One row per principle. **Tier** is as defined in §2.2. Nothing in the "Drafting principle
identified" column reproduces protected text; each is a restatement in PCI's own words.

| # | Research area | Authoritative source | Drafting principle identified | Application to PCI | Copyright restriction |
|---|---|---|---|---|---|
| 1 | Normative vocabulary | ISO/IEC Directives Part 2 (S) | A rule document must operate a closed set of auxiliaries, each carrying exactly one normative meaning, with a single preferred form per provision type and no synonyms, so that obligation, recommendation, permission and capability are never inferred from tone | PCI adopts a closed verbal-form set, publishes it as a Conventions clause at the front of every law volume, and bans synonyms for the mandatory form ("is required to", "is obliged to", "has a duty to") | Principle restatable. The Directives' verbal-forms tables, their wording and their equivalent-expression lists must not be reproduced or reformatted into a PCI table |
| 2 | Provision-type mapping | ISO/IEC Directives Part 2 (S) | Requirements are carried by one dedicated auxiliary; recommendations by a second; permissions by a third; possibility and capability by a fourth | PCI maps: obligation → `must`; recommendation → `should`; permission → `may`; capability → `can`. PCI must state this mapping expressly because it differs from ISO's | Mapping is PCI's own. Do not reproduce ISO's tables or state that PCI follows the Directives |
| 3 | The `must` collision | ISO/IEC Directives Part 2 (S) | In ISO/IEC drafting, `must` is reserved for constraints or obligations defined **outside** the document, given for information; using it does not make the external constraint a requirement of the document | **Critical.** PCI's chosen `must` = its own mandatory requirement is the inverse of ISO's use of the same word. PCI must publish an explicit disclaimer of ISO verbal-form conventions | Restatable as method. Do not quote the Directives' text on external constraints |
| 4 | What a requirement is | ISO/IEC Directives Part 2; ISO conformity-assessment guidance for standards writers (S) | A requirement conveys objectively verifiable criteria from which no deviation is permitted if conformance is to be claimed | PCI adopts a gate: no statement enters a Rule unless a reviewer could determine fulfilment from evidence | Concept restatable in own words. The ISO definition sentence must not be reproduced |
| 5 | Verifiability filter | ISO/IEC Directives Part 2 (S) | Only requirements capable of verification may be included; subjective formulations of the "sufficiently strong" kind are prohibited; and a property must not be specified where no method can verify the claim within a reasonable period | PCI bans undefined judgement adjectives in Rule text (material, reasonable, significant, appropriate, adequate, timely, robust) and must not write laws whose compliance can only be judged after the project ends | Restatable. Do not reproduce the Directives' example phrases |
| 6 | What a voluntary document excludes | ISO/IEC Directives Part 2 (S) | Guarantee, commercial and contractual conditions are excluded from a document's requirements, and legal or statutory requirements are not restated as the document's own requirements | PCI laws bind professional conduct and method only. They never set commercial terms and never restate law as PCI law; legal obligations appear only as external constraints or jurisdictional cautions | Restatable. This principle underwrites PCI's legal-status disclaimer |
| 7 | Scope as a normative element | ISO/IEC Directives Part 2 (S) | Scope is the mandatory opening element delimiting the subject and the limits of applicability, with applicability statements introduced by set wording so they are found in one predictable place | PCI's `Scope` field becomes structurally mandatory, is drafted before the Rule, and opens with one fixed pattern of PCI's own devising | Restatable. Do not adopt ISO's specific opening phrases verbatim |
| 8 | Scope contains no obligations | ISO/IEC Directives Part 2 (S) | The scope statement must contain no requirements, permissions or recommendations, and is worded as statements of fact | PCI gate check: any `must`/`should`/`may` appearing in a `Scope` field is a drafting defect | Restatable |
| 9 | Normative vs informative | ISO/IEC Directives Part 2 (S) | Each element of a document is classified as normative or informative, and the classification is made visible to the reader | PCI must classify each of the fields in its law template as normative or informative and print the classification. See §7(d) | Restatable. Do not reproduce the Directives' typographic conventions table |
| 10 | Examples are never normative | ISO/IEC Directives Part 2 (S) | Examples must not contain requirements, instructions, recommendations or permissions, and are written as statements of fact | PCI's worked examples and `AI application` illustrations must be scrubbed of obligation language | Restatable. Do not reproduce the Directives' rule text |
| 11 | Notes are constrained | ISO/IEC Directives Part 2 (S) | Notes integrated in the text follow different rules from notes attached to a terminological entry; the two must not be conflated | PCI adopts one note type only — informative, never obligation-bearing — to remove the ambiguity entirely | Restatable. The distinction is UNVERIFIED as to detail; PCI's simplification avoids reliance on it |
| 12 | Terms clause is normative | ISO/IEC Directives Part 2 (S) | The terms and definitions clause is a normative element of the document, not a reader's convenience | PCI's glossary becomes part of the law set, versioned with it, not an appendix | Restatable |
| 13 | Substitution principle | ISO/IEC Directives Part 2 (S) | A definition must be written so that it can replace the term in context | PCI adds a mechanical substitution test to the law gate: substitute every defined term into every Rule that uses it and confirm the sentence still reads correctly | Restatable |
| 14 | Definitions carry no requirements | ISO/IEC Directives Part 2 (S) | A definition must not take the form of, or contain, a requirement | PCI gate check: no `must` in the glossary. Obligations belong in laws, not in definitions | Restatable |
| 15 | Definition construction | ISO/IEC Directives Part 2 (S) | Definitions must not repeat the term being defined, and follow fixed formal conventions of construction | PCI gate check: automated detection of the defined term appearing inside its own definition and of two-term definitional loops; PCI publishes its own house rules for definition form | Restatable. Do not copy ISO's specific formatting rules as PCI rules |
| 16 | Consistency across a suite | ISO/IEC Directives Part 2 (S) | Identical provisions are expressed in identical words, with one terminology and no synonyms; related documents share structure and numbering so a reader can navigate one from knowledge of another | Across PCL-AI, PFL-AI and PML-AI an identical obligation must be worded identically; the common domain-numbered ID scheme and common field order in `SUPERSEDED_LAW_SYSTEM_v0.md` are validated | Restatable |
| 17 | Normative references | ISO/IEC Directives Part 2 (S) | Only documents whose content constitutes requirements of the document are listed as normative references | PCI separates *references that create obligation* from *references cited for orientation*. The current `External references` field conflates them | Restatable |
| 18 | Dated vs undated references | ISO/IEC Directives Part 2 (S) | For a dated reference only the cited edition applies; for an undated reference the latest edition including amendments applies; normative reference to draft documents is strongly discouraged because the text can still change | Given PCI's ban on inventing editions and clause numbers, PCI references undated by instrument name with "verify current requirements", and never anchors a law to an exposure or consultation draft | Restatable. This is a rule about referencing, not reproducible text |
| 19 | Instructions vs requirements | ISO/IEC Directives Part 2 (S) | Direct procedural steps are expressed in the imperative mood; requirements use the mandatory auxiliary; the two forms are not interchangeable | PCI's `Minimum professional requirement` field is procedural and may use imperative steps; the `Rule` field must always use the mandatory auxiliary | Restatable |
| 20 | Declared aim of the rules | ISO/IEC Directives Part 2 (S) | The stated aim of the drafting rules is documents that are clear, precise and unambiguous | PCI adopts the same declared aim and makes it the tie-breaker in editorial disputes | Restatable |
| 21 | No requirements, no conformity | ISO, *Conformity assessment for standards writers* (S) | A document containing no requirements is not intended to be used for conformity assessment | If a PCI "law" contains no obligation, it is guidance and must be relabelled. This is the test that removes slogans | Restatable. ISO publication text must not be reproduced |
| 22 | Assessment neutrality and separation | ISO, *Conformity assessment for standards writers* (S) | A requirement should not state a preference for who assesses it — first, second or third party — and conformity-assessment provisions are kept separate from technical requirements so each can be applied independently | PCI laws state *what* must be verified and by what evidence, not that only PCI may verify it; PCI separates the law (obligation) from the scheme documents (how PCI examines and audits) | Restatable |
| 23 | Process principles | IFRS Foundation Due Process Handbook (S) | Rule-making rests on three published principles: transparency, full and fair consultation, and accountability | PCI publishes a three-principle process statement in its own words and at its own scale | Principles are ideas, not protected expression; the Handbook's articulation must not be reproduced |
| 24 | Process as a written rulebook | IFRS Foundation Due Process Handbook (S) | The process itself is documented as a set of requirements the rule-making body must meet, and the process document is amended through the same public process it prescribes | PCI writes a Law Development Procedure that binds PCI, holds itself to it, and changes it only by the route it sets out | Restatable |
| 25 | Public record of reasoning and input | IFRS Foundation (S) | Meeting papers and material considered are made public, and comment letters received are published rather than merely summarised by the body itself | PCI publishes a short decision record per law and publishes the comments it receives, or states honestly that consultation was internal | Restatable |
| 26 | Minimum consultation period | IFRS Foundation Due Process Handbook (S) | A minimum comment period is fixed in advance (reported as 120 days normally for an exposure draft, with a shorter minimum, reported as 60 days, for narrow re-exposure), and materially changed proposals are re-exposed rather than pushed through | PCI sets its own published minimum, never shortens it silently, adopts a materiality trigger for re-consultation, and must not imply its period is equivalent to IFRS's | Durations are facts, not protected expression, but are **S** and must be re-verified before citation |
| 27 | Feedback statement | IFRS Foundation (S) | The body publishes what it heard and how it responded, closing the consultation loop | PCI publishes a short feedback note with each law release | Restatable |
| 28 | Reasoned basis document | IFRS Foundation (S) | A Basis for Conclusions accompanies but is not part of the Standard — it explains the reasoning without adding requirements | PCI publishes a **Basis for Decision** (its own term) that is expressly non-normative. PCI should avoid the phrase "Basis for Conclusions" to prevent implied IFRS equivalence | Format and function restatable. IFRS Basis for Conclusions text must not be reproduced; avoid IFRS terms of art and marks |
| 29 | Effects analysis | IFRS Foundation (S) | The likely effects — costs and benefits — of a proposed requirement are analysed and explained | PCI records, per law, the compliance and evidence burden it imposes on a candidate or credential holder | Restatable |
| 30 | Post-implementation review | IFRS Foundation (S) | A review after implementation is mandatory, assesses whether the requirement achieved what was intended, runs in phases, may involve a formal request for information, and is timed to allow real implementation experience to accumulate | PCI schedules a proportionate review of each law set at a fixed interval after entry into effect and publishes the outcome | Restatable. Reported timings are **S**; do not cite IFRS timings as PCI's justification |
| 31 | Effective dates and transition | IFRS Foundation (S); FCA (S) | Requirements carry a stated effective date, and those affected are allowed sufficient time to implement before compliance is expected | Every PCI law carries an "applies from" date and, where behaviour must change, a transition period. Laws never apply retrospectively to work already completed | Restatable |
| 32 | Priority-setting consultation | IFRS Foundation (S) | The forward agenda is itself consulted on, at a fixed interval (reported as five-yearly) | PCI consults on which laws to write next, not only on the text of laws already drafted | Restatable; interval is **S** |
| 33 | Interpretation route | IFRS Foundation (S) | Application questions are routed to a standing body that may publish explanatory material after public consultation, or conclude that amendment is required instead | PCI creates a named interpretation route with two possible outcomes: published clarification, or an amendment proposal | Restatable |
| 34 | Authority of explanatory material | IFRS Foundation (S) | Explanatory material derives its authority from the requirements it explains; it does not create new requirements | PCI clarifications may explain a law but must never extend it. A clarification that adds obligation is an amendment and follows the amendment process | Restatable |
| 35 | Independent process oversight | IFRS Foundation (S) | Compliance with the process is monitored by a body separate from the one that writes the requirements, which also handles complaints about the process | PCI must either create a genuinely separate reviewer of its law process, or state plainly that it has not. See §7(b) | Restatable |
| 36 | Avoiding the legislative "shall" | UK OPC *Drafting Guidance* (S) | Office policy is to avoid the legislative "shall"; obligations are imposed using "must". Limited exceptions exist, principally where text is inserted into an existing instrument that already uses the older form | PCI bans `shall` outright. PCI has no legacy instrument to amend, so the OPC exception does not arise. `SUPERSEDED_LAW_SYSTEM_v0.md` §3 currently permits `shall` and must be corrected | Restatable. Do not reproduce OPC passages until the licence position is confirmed (**UNVERIFIED**) |
| 37 | Modern standard English | UK OPC *Drafting Guidance* (S) | Draft in modern, standard English using vocabulary reflecting ordinary general usage; avoid archaisms and expressions likely to cause difficulty | PCI bans hereinafter, aforesaid, notwithstanding, thereto, pursuant to, save that, in the event that | Restatable |
| 38 | Conditional structure | UK OPC *Drafting Guidance* (S) | Conditions are set out before the obligation they qualify, in a structured conditional form, rather than trailed after it in provisos | PCI's `Rule` is a single sentence; conditions precede the obligation or move to `Scope` | Restatable |
| 39 | Definitions discipline | UK OPC *Drafting Guidance*; UK tax-definitions review (S) | Definitions should be used sparingly, should not surprise the reader, and negative definitions should be rare | PCI defines only terms it actually uses normatively; no term may be defined contrary to its ordinary professional meaning without a visible flag | Restatable |
| 40 | Good law criteria | UK Good Law initiative (S) | Good law is necessary, effective, clear, coherent and accessible — and this covers content, architecture, language and accessibility together | PCI adopts these five as the acceptance criteria for its law gate, in its own formulation | The five-word formulation is short and attributable; attribute it to the Good Law initiative rather than presenting it as PCI's invention |
| 41 | Complexity has a cost | UK OPC, *When Laws Become Too Complex* (S) | Volume, piecemeal structure, excessive detail and frequent amendment make rules hard to understand and hard to comply with | PCI caps law count per domain and prefers one clear law to three overlapping ones. Every added law must justify its marginal complexity | Restatable |
| 42 | Status marking on the face of the text | FCA Handbook (S) | Every provision carries a status marker identifying its legal character — rule, guidance, evidential provision, direction and others — and defined expressions are typographically distinguished wherever they appear | PCI marks every block with a status label (**LAW**, **RULE**, **GUIDANCE**, **EXTERNAL**, **EXAMPLE**) and flags defined terms in law text, linking them to the glossary. Colour must never be the only carrier — consistent with PCI's existing accessibility rule | Restatable. Do not copy the FCA's letters or their published meanings verbatim |
| 43 | Guidance is not binding | FCA Handbook (S) | Guidance does not bind, and there is no presumption that departing from it indicates a breach of the underlying rule | PCI's `should` provisions must be expressly non-binding. `SUPERSEDED_LAW_SYSTEM_v0.md` currently requires a recorded reason for departure — that makes `should` quasi-evidential and must be stated as such | Restatable |
| 44 | Evidential provisions | FCA Handbook (S) | A middle category exists: provisions not binding in their own right, contravention of which may be relied on as tending to establish contravention of the binding rule | Directly useful to PCI. It gives a principled home for "recommended, but departure is evidentially relevant" without pretending such provisions are laws | Restatable. Do not reproduce the FCA's formulation or statutory references |
| 45 | Safe-harbour effect | FCA Handbook (S) | Where a firm follows guidance indicating a way to comply, the regulator proceeds on the basis that the rule has been complied with | PCI can offer a safe harbour: following the stated method is accepted as compliance, without making that method the only route | Restatable |
| 46 | Application and purpose sections | FCA Handbook (S) | Modules open with an explicit statement of who they apply to and what they are for, before any substantive provision | Validates PCI's `Scope` + `Purpose` fields; confirms they must come before, and be readable independently of, the Rule | Restatable |
| 47 | Precise citation architecture | FCA Handbook (S) | Provisions are addressed by a stable module.chapter.section.paragraph reference carrying the status marker, so any provision can be cited exactly | Validates PCI's stable-ID rule. PCI should add the status marker into the citation form so a citation reveals whether it is law or guidance | Restatable |
| 48 | Formal exception machinery | FCA Handbook, SUP 8 (S) | Relief from a rule is granted only against published conditions — that compliance would be unduly burdensome or would not achieve the purpose of the rule, and that relief would not undermine the body's objectives — and is published unless publication is inappropriate | PCI creates a written, conditioned, recorded exemption route with published criteria, replacing ad hoc prose hedges inside Rules | Restatable as method. Do not reproduce FCA text or the statutory tests verbatim |
| 49 | Individual interpretation | FCA Handbook, SUP 9 (S) | A regulated person may obtain guidance on their own situation, with a published statement of what reliance on it achieves | PCI offers a candidate interpretation request route with a stated effect on later assessment | Restatable |
| 50 | Consultation before rule-making | FCA (S) | Proposed rules are consulted on with a published explanation of purpose, a compatibility statement against the body's objectives, and an invitation for representations within a stated period | PCI's law consultation notice states purpose, alignment with PCI's certification objectives, and a response deadline | Restatable |
| 51 | Cost benefit analysis | FCA (S) | A cost benefit analysis of proposed rules is published, with a narrow exemption where cost increases are absent or of minimal significance, supported by a published statement of policy on how such analysis is done | PCI applies a proportionate version: what does compliance and evidence cost the credential holder, and is it justified by the risk addressed | Restatable |
| 52 | Versioned rulebook | FCA (S) | The rulebook is dated and versioned, with changes made by published instruments and recorded in change notices | PCI publishes each law set with a version, a date and a change log; superseded text remains retrievable | Restatable |
| 53 | Scheme built on job analysis | ISO/IEC 17024 (S) | A personnel certification scheme rests on a job or practice analysis identifying tasks, the competence each requires, prerequisites, and the assessment mechanisms and examination content | PCI's laws must trace to identified professional tasks, and the examination must trace to the laws. This closes the law → competence → scenario chain | Standard is copyright; may be named and characterised only. Do not reproduce clause text. **PCI must not claim conformity with ISO/IEC 17024 unless accredited** |
| 54 | Valid and reliable assessment | ISO/IEC 17024 (S) | Assessment methods must be valid, reliable and objective, and reviewed periodically to remain current with the profession | PCI's examination items derived from laws must be reviewed on a stated cycle, with evidence of validity retained | As above |
| 55 | Impartiality and separation | ISO/IEC 17024 (S) | The certification body must manage conflicts of interest and separate certification decisions from training and other commercial interests | PCI must state how its examination and certification decisions are separated from its training and publishing activities | As above |
| 56 | Appeals and complaints | ISO/IEC 17024 (S) | Complaints and appeals procedures must be accessible, fair and handled impartially, giving candidates genuine recourse | PCI's `Consequences of breach` field must point to a published appeal route with a decision-maker who was not part of the original decision | As above |
| 57 | Lifecycle and scheme governance | ISO/IEC 17024 (S) | The scheme governs the whole lifecycle — application, assessment, examination, decision, certification, recertification, surveillance, suspension and withdrawal, appeals and complaints — and scheme development and maintenance is a governed function with defined ownership, capable of sitting outside the certification body | PCI laws about credential holder conduct must connect to defined lifecycle states so a breach has a defined effect; PCI names an accountable owner for the law set and separates law authorship from law approval | As above |
| 58 | Nonconformity defined | ISO/IEC 17021-1 (S) | Non-compliance is defined as non-fulfilment of a requirement — a gap between stated criteria and observed evidence | PCI defines breach against the law's own `Required evidence`, not against a general impression of poor practice | As above |
| 59 | Graded classification and recording | ISO/IEC 17021-1 (S) | Non-compliance is graded by whether it defeats the intended result, a pattern of small failures against one requirement can amount to a systemic failure, and findings must be identified, classified and recorded so an informed decision can be made — a genuine non-compliance must never be downgraded to a mere improvement suggestion | PCI grades breaches as minor, material or systemic, escalates on repetition against one law, and records every finding with its classification. A breach is never quietly logged as feedback | As above |

**Row count: 59.**

---

## 7. Recommendations for PCI

### (a) Verbal forms: the consequences of modern must-drafting

**The decision is already taken:** PCI drafts with modern `must`. This section records what follows
from it, not whether to do it.

The choice is well supported. The UK Office of the Parliamentary Counsel's stated policy is to avoid
the legislative `shall` and to impose obligations with `must`, on the ground that modern, ordinary
vocabulary is clearer than archaic legislative usage (S). For a document read by working
professionals rather than by lawyers, that is the right register.

But the choice has four consequences PCI must handle explicitly.

**Consequence 1 — the `must` inversion, and it is the serious one.** In ISO/IEC drafting, `must` is
*not* the mandatory auxiliary. It is reserved for constraints or obligations defined **outside** the
document, recorded for the reader's information, and the Directives state that using it does not
make the external constraint a requirement of the document. `shall` is the document's own
requirement (S). PCI's convention is therefore not merely different from ISO's — for the specific
word `must` it is the **exact inverse**. A reader trained on ISO documents, of whom PCI's audience
will contain many, will read a PCI `must` as "someone else's rule, mentioned for information", which
is the precise opposite of what PCI intends.

PCI must therefore publish, in the front matter of every law volume and every law web page, a
Conventions statement that does all of the following in PCI's own words:

- states that `must` and `must not` express PCI's own mandatory requirements;
- states that `should` expresses a PCI recommendation, `may` a permission, and `can` a possibility
  or capability;
- states expressly that **PCI does not follow the ISO/IEC Directives verbal-form conventions**, and
  that readers familiar with them must not map PCI's `must` onto ISO's `must`;
- states how PCI signals an external constraint instead — see Consequence 4.

Without that statement, PCI's most fundamental drafting decision is ambiguous to a large part of its
own readership. With it, the decision is safe.

**Consequence 2 — the `should` mapping is compatible, but PCI's version is stronger than it looks.**
ISO, the OPC and the FCA all treat the recommendatory form as non-binding, and the FCA goes further:
guidance does not bind, and departure from it raises no presumption of breach (S). PCI's current
`SUPERSEDED_LAW_SYSTEM_v0.md` says departures from `should` "need a recorded reason". That is not plain
recommendation; it is closer to the FCA's *evidential provision* — not binding in itself, but
capable of being relied on as tending to establish a breach of the binding rule (S). PCI must
choose, and say which:

- **Option A (recommended):** `should` is genuinely non-binding, and departure is not evidence of
  breach. Simple, honest, easy to defend.
- **Option B:** `should` is evidential — departure requires a recorded reason and the absence of one
  may be relied on when assessing compliance with the related `must`. Also defensible, but it must be
  labelled as its own category with its own status marker, not left looking like ordinary advice.

What PCI must not do is keep the current position, in which `should` is described as a
recommendation but carries an unstated obligation to justify departure.

**Consequence 3 — `shall` must disappear, and it currently has not.** `SUPERSEDED_LAW_SYSTEM_v0.md` §3 presently
permits `must / must not / shall / shall not` interchangeably for mandatory rules. That directly
contradicts the programme decision and must be corrected before drafting begins. Concretely:

- amend `SUPERSEDED_LAW_SYSTEM_v0.md` §3 to remove `shall` and `shall not`;
- add a mechanical gate check that fails any law set containing `shall` in any field;
- extend the check to the whole corpus, since imported or adapted text is the usual route by which
  `shall` re-enters a document — the OPC itself notes that the main reason to use the old form is
  insertion into an instrument that already uses it (S), and PCI has no such instrument.

**Consequence 4 — PCI needs a replacement device for external constraints.** Because `must` is now
spent on PCI's own obligations, PCI cannot use it for "the law requires X". PCI already has the
right containers — the `EXTERNAL REFERENCE` block and the `CAUTION` block — but needs a fixed
formula. Recommended pattern, in PCI's own words and marked as external:

> *External constraint — not a PCI requirement.* Applicable law or the named standard may impose
> requirements on this activity. PCI does not impose them and does not restate them. The official
> publication governs. Verify current requirements in your jurisdiction.

Paired with the precedence rule (external law and stricter authoritative standards govern), this
gives PCI a complete and unambiguous system without borrowing ISO's vocabulary.

**Recommended closed verbal-form set for PCI** (PCI's own construction, not reproduced from any
source):

| Form | Meaning in PCI law | Where permitted |
|---|---|---|
| `must` / `must not` | PCI mandatory requirement / prohibition | Rule, Minimum professional requirement, Required evidence, Independent review, Escalation trigger, Prohibited practice, AI restriction, Verification requirement |
| `should` / `should not` | PCI recommendation (status per Option A or B above) | Practice guidance blocks only |
| `may` | PCI permission — a genuine discretion | Any normative field, sparingly |
| `can` / `cannot` | Possibility or capability — a statement of fact, never a permission | Anywhere |
| `is` / `are` | Statement of fact | Scope, Purpose, examples |

**Banned in all law text:** `shall`, `shall not`, `ought to`, `is required to`, `is obliged to`,
`has a duty to`, `will` used as an obligation, `may not` (ambiguous between prohibition and absence
of permission — use `must not` or "is not permitted"), `endeavour to`, `use best efforts`, `as
appropriate`, `where practicable`, `wherever possible` — the last three unless the condition is
defined.

### (b) Which IFRS due-process elements PCI can honestly adopt — and which it cannot

The governing constraint: **PCI is a small private certifier. It must not represent its process as
equivalent to IFRS standard-setting, and must not use IFRS Foundation, IASB or ISSB names or marks
in a way suggesting endorsement, affiliation or equivalence.** The value of the Handbook to PCI is
as a demonstration that a rule-making process can be written down, published and audited — not as a
badge.

**Can be adopted honestly, at PCI's scale:**

| Element | PCI form |
|---|---|
| A written process the body binds itself to | A published *PCI Law Development Procedure* — short, real, and actually followed |
| Three stated principles | Transparency, consultation and accountability, stated in PCI's own words and scaled to a private certifier |
| Consultation with a fixed minimum period | PCI sets and publishes its own minimum. It must be a period PCI will genuinely honour, and must never be described as comparable to IFRS's |
| Publishing what was heard | A short feedback note per law release, listing themes raised and PCI's response |
| Reasoned basis, expressly non-normative | A **Basis for Decision** per law or per release. Use PCI's own term; avoid "Basis for Conclusions" |
| Effective dates and transition | Every law carries an "applies from" date; behavioural changes get a transition period; no retrospective application |
| Amendment through the same process | Laws change only through the published procedure, with a change log |
| Review after implementation | A proportionate review of each law set at a fixed interval after it takes effect, with a published outcome |
| Forward agenda consultation | Publish what PCI intends to draft next and invite comment |
| A complaints route about the process | A named route for "PCI did not follow its own procedure", distinct from an appeal against a decision |
| Clarifications cannot add obligation | A clarification that would extend a law is an amendment and follows the amendment route |

**Cannot honestly be adopted, or must not be claimed:**

| Element | Why not | What PCI says instead |
|---|---|---|
| Independent trustee-level oversight of process compliance | PCI has no equivalent body. Asserting one that does not exist, or is not genuinely independent, is worse than having none | Either constitute a genuinely separate reviewer with a published remit, or state plainly that process oversight is internal |
| Public broadcast meetings and full papers publication | Not proportionate, and PCI cannot sustain it | Publish decisions and reasoning, not proceedings |
| Global multi-stakeholder consultation | PCI does not have the reach; claiming it would be false | Describe the actual consultation population honestly — for example practitioners, employers, subject reviewers |
| Formal effects analysis of the IFRS kind | Requires data and economic analysis PCI does not have | A proportionate compliance-burden note per law: what evidence must be produced, by whom, and at what cost |
| Post-implementation review at IFRS depth and timing | Requires large-scale implementation evidence | A scaled review with a stated, smaller evidence base — and say that it is scaled |
| Any statement of equivalence | Materially misleading | "PCI's process is informed by published standard-setting practice. It is PCI's own process and is not equivalent to, endorsed by, or affiliated with any standard-setting body." |

**The single most important lesson from the Handbook is not any stage of the process. It is that the
process is written down and the body is held to it.** A published PCI procedure that PCI does not
follow is a worse position than no published procedure at all, because it converts a gap into a
verifiable failure. PCI should publish only what it will actually do.

### (c) Keeping PCI laws clearly distinguishable from legislation

PCI has chosen the word "law" for its rules. That is a defensible branding choice for a professional
body, but it carries a permanent obligation to prevent misunderstanding. The following are
recommended as binding editorial constraints.

**1. The status disclaimer travels with the text, not just the volume.** `SUPERSEDED_LAW_SYSTEM_v0.md` already
mandates a legal-status disclaimer. Extend it: the disclaimer, or a short form of it, must appear in
the running footer of every printed page of a law volume, in the header of every law web page, and
in any exported extract or single-law PDF. Front matter is not read; footers are.

**2. Make the precedence rule a numbered foundational law, not a preface.** The rule that applicable
law, regulation, contract or a stricter authoritative standard governs should be `PCI-LAW-F-01` or
equivalent — a law that cannot be extracted away from the set.

**3. Ban legislative register.** Prohibited in all PCI law text: *enacted*, *in force*, *statutory*,
*statute*, *regulation* (of PCI's own rules), *Act*, *section*/`s.5(2)(a)` citation style,
*sub-section*, *comes into force*, *commencement*, *offence*, *penalty*, *fine*, *sanction* in its
punitive legal sense, *shall come into effect*, *by order*, *competent authority*. Use instead:
*PCI Law*, *issued*, *applies from*, *effective from*, *PCI requirement*, *breach*, *consequence*.

**4. Keep the PCI identifier scheme visibly non-legislative.** `PCL-LAW-06-03` cannot be mistaken
for a statutory citation. Retain it, and never cite a PCI law in a form resembling
"section 6(3)".

**5. Consequences stay inside PCI's actual authority.** Breach consequences may only concern the
examination, the certification, PCI's conduct and quality processes, and PCI's own record-keeping.
No PCI law may state or imply a financial penalty, a legal liability, a professional disqualification
beyond PCI's own credential, or any consequence PCI cannot itself deliver.

**6. Say who is bound and why.** PCI laws bind those who hold or seek a PCI credential, by agreement,
as a condition of certification. State that in the foundational laws. It is the honest basis of the
whole system and it is also the clearest possible statement that these are not laws of general
application.

**7. Avoid legislative visual signals.** No crown or state devices, no imitation of official
statutory typography, no "Queen's/King's Printer" style layout. PCI's existing colour-and-icon system
already differentiates; keep it and keep it distinctive.

**8. Do not restate law as PCI law.** This is also required by the ISO principle that a voluntary
document excludes legal and statutory requirements (S). Where a legal obligation matters, it appears
as an external constraint or a jurisdictional caution, never as a PCI `must`.

### (d) The specific traps to avoid

**Trap 1 — requirements hidden in notes, examples and soft fields.**
ISO's rule is unambiguous: examples must not contain requirements, instructions, recommendations or
permissions (S). PCI's law template has eighteen fields and does not currently declare which of them
are normative. That is the highest-probability source of defect in the entire programme, because a
reader cannot comply with an obligation they cannot identify.

*Action:* declare the status of every field, print the declaration in the Conventions clause, and
gate-check it. A recommended classification, for the programme to confirm:

| Field | Proposed status |
|---|---|
| Rule | **Normative — mandatory** |
| Minimum professional requirement | **Normative — mandatory** |
| Required evidence | **Normative — mandatory** |
| Decision owner | **Normative — mandatory** |
| Independent review | **Normative — mandatory** |
| Escalation trigger | **Normative — mandatory** |
| Prohibited practice | **Normative — mandatory** |
| AI restriction | **Normative — mandatory** |
| Verification requirement | **Normative — mandatory** |
| Scope | Normative — delimiting; contains no obligations |
| AI application | **Normative — permission.** Must be written as permission, or it will be read as a requirement to use AI |
| Purpose | Informative |
| External references | Informative — external constraints only |
| Jurisdictional caution | Informative — external constraints only |
| Related PCI laws / Related book content / Examination relevance | Informative |
| Consequences of breach | Normative as to classification; must not introduce new obligations |

**Trap 2 — unverifiable requirements.**
ISO admits only requirements conveying objectively verifiable criteria and excludes subjective
formulations (S); its conformity-assessment guidance adds that a document with no requirements
cannot be used for conformity assessment at all (S).

*Action:* apply a two-part gate to every Rule. (i) *Evidence test* — the `Required evidence` field
must name an artefact a reviewer could actually inspect (a calculation, a signed approval, a dated
record, a register entry). "Professional judgement was applied" is not evidence. (ii) *Falsifiability
test* — it must be possible to describe, in one sentence, what a breach would look like. If neither
can be done, the statement is guidance and must be relabelled. **This is the specific test that
removes slogans**, which is the stated purpose of the reconstruction.

**Trap 3 — circular and defective definitions.**
ISO prohibits circular definitions, prohibits definitions that take the form of or contain a
requirement, and requires the substitution principle (S).

*Action:* three mechanical checks in the law gate — (i) the defined term does not appear inside its
own definition, and no two-term definitional loop exists; (ii) no `must` appears anywhere in the
glossary; (iii) substituting each definition into each Rule that uses the term produces a sentence
that still reads correctly. Also: one glossary for the whole suite, one meaning per term, one term
per meaning — the consistency principle (S) means PCL-AI, PFL-AI and PML-AI cannot define the same
word differently.

**Trap 4 — undefined judgement terms.**
This is the same defect as Trap 2 but it hides better, because judgement words read as rigorous.
*Material*, *reasonable*, *significant*, *appropriate*, *adequate*, *timely*, *robust*, *sufficient*,
*proportionate*, *best practice*, *industry standard*, *as required*, *where necessary*.

*Action:* ban them in Rule text unless the law does one of three things: (i) gives a threshold;
(ii) gives a decision procedure; or (iii) names the `Decision owner` who determines the question and
the criteria they apply. Option (iii) is usually the right answer for professional judgement, and it
has the advantage of being verifiable — you can check who decided and on what basis, even where you
cannot check the judgement itself.

**Trap 5 — duplicating an external standard without adding anything.**
A PCI law that restates what IFRS 15, ISO 21500 or a FIDIC form already requires adds no obligation,
creates copyright and misstatement risk, and becomes wrong the moment the external instrument
changes. ISO's own rule excludes legal and statutory requirements from a document's requirements
(S), and its referencing rules exist precisely so that documents do not carry stale copies of one
another (S).

*Action:* apply a **"what does PCI add?" test** to every law. Each law must state, in the Basis for
Decision, the obligation it creates that the external instrument does not already create — typically
one of: *evidence* (PCI requires the reasoning to be recorded in a specific form), *independence*
(PCI requires a second person to check), *accountability* (PCI names the decision owner),
*escalation* (PCI fixes the trigger), or *AI limitation* (PCI states what a model must not decide).
If a law cannot answer the test, delete it and reference the external instrument instead.

*Related discipline:* reference external instruments **undated, by name**, with "verify current
requirements", so that PCI never carries a stale edition or clause number. This follows from ISO's
dated/undated distinction (S) and from PCI's existing ban on inventing clause numbers and editions.

**Trap 6 — `shall` leakage.** Add it to the gate as a hard failure (see §7(a), Consequence 3).
Imported and adapted text is how it returns.

**Trap 7 — `may not`.** Ambiguous between "is prohibited from" and "is not required to". Ban it;
use `must not` or "is not required to" as intended.

---

## 8. Immediate actions arising

| # | Action | Owner | Depends on |
|---|---|---|---|
| 1 | Amend `SUPERSEDED_LAW_SYSTEM_v0.md` §3 to remove `shall`/`shall not` and adopt the closed verbal-form set in §7(a) | Law programme | — |
| 2 | Add a Conventions clause covering verbal forms, the express disclaimer of ISO conventions, and field status | Law programme | 1 |
| 3 | Classify every template field as normative or informative and print the classification | Law programme | 2 |
| 4 | Decide `should` as Option A or Option B (§7(a), Consequence 2) and state it | Law programme | 2 |
| 5 | Add gate checks: no `shall`; no `may not`; no banned judgement terms in Rule text; no `must` in glossary; substitution test; circularity test; evidence-artefact test | Build/checks (`docs/books/_build/checks`) | 1–4 |
| 6 | Add the "what does PCI add?" test to the law gate | Law programme | — |
| 7 | Draft the PCI Law Development Procedure covering only what PCI will actually do (§7(b)) | PCI governance | — |
| 8 | Move the legal-status disclaimer into running headers/footers and every export path | Publishing/typesetting | — |
| 9 | Create the exemption route and the interpretation route, with published criteria | PCI governance | 7 |
| 10 | Re-verify every source in §10 against the issuing bodies' own pages before any PCI publication cites them | Law programme | Network access |

---

## 9. Copyright compliance statement for this report

- No text from the ISO/IEC Directives Part 2, ISO/IEC 17024, ISO/IEC 17021-1, ISO guidance
  publications, the IFRS Foundation Due Process Handbook, the OPC *Drafting Guidance* or the FCA
  Handbook has been reproduced in this report.
- Every principle is stated as a restatement of method in PCI's own words. Tables in this report are
  PCI's own constructions and do not reproduce the layout, sequence or content of any source table —
  in particular, the ISO/IEC verbal-forms tables are described in function only and are nowhere
  reconstructed.
- Source names, document titles and URLs are used for identification and attribution. Trade marks
  remain those of their owners; no endorsement or affiliation is implied.
- The verbal-form table in §7(a) is PCI's own convention, constructed for PCI's purposes. It is not
  derived from, and must not be presented as consistent with, the ISO/IEC verbal-form system.
- Where a source's licence position could not be verified (OPC *Drafting Guidance*, FCA Handbook),
  the stricter restate-only position has been applied.

---

## 10. Open items requiring re-verification

Every item below is **UNVERIFIED** as at 2026-08-04 and must be confirmed against the issuing body's
own page before any PCI publication relies on it.

| # | Item | Why it matters | Where to verify |
|---|---|---|---|
| 1 | Current edition and date of ISO/IEC Directives, Part 2 | PCI must never cite an edition it has not confirmed. Search evidence was inconsistent (titles indicating a ninth edition dated 2021, a reference to an eighth edition, and later amendment activity) | `https://www.iso.org/sites/directives/current/part2/index.xhtml`; `https://www.iec.ch/standards-development/isoiec-directives-part-2` |
| 2 | Exact contents and numbering of the verbal-forms tables | PCI does not need the contents — it must not reproduce them — but must confirm the substance of the `must`/external-constraint rule before publishing its disclaimer | As above |
| 3 | Exact publication date and structure of the April 2026 IFRS Due Process Handbook | Cited in §4 and §5 as S | `https://www.ifrs.org/groups/due-process-oversight-committee/due-process-handbook/` |
| 4 | IFRS minimum comment periods (reported as 120 days; 60 days for narrow re-exposure) and the five-yearly agenda consultation interval | PCI should not cite these figures unless confirmed; PCI sets its own regardless | As above |
| 5 | Whether an OPC *Drafting Guidance* version later than 19 March 2024 exists | PCI cites the guidance as the basis for its `must` decision | `https://www.gov.uk/government/publications/drafting-bills-for-parliament` |
| 6 | Licence terms of the OPC *Drafting Guidance* PDF (OGL v3.0 assumed but not confirmed) | Determines whether PCI may quote at all | As above |
| 7 | Current edition of ISO/IEC 17024 (a 2026 edition was indicated but not confirmed against ISO) | PCI must not cite a superseded or non-existent edition | `https://www.iso.org/standard/17024` |
| 8 | Current edition of ISO/IEC 17021-1 (2015 indicated) | Used for nonconformity classification | `https://www.iso.org/obp/ui/en/#!iso:std:61651:en` |
| 9 | FCA Handbook copyright and reuse terms | Determines whether PCI may quote | `https://www.fca.org.uk/` legal pages |
| 10 | FCA Reader's Guide current version and the full set of status letters | PCI cites the status-marking method, not the letters | `https://handbook.fca.org.uk/handbook-readers-guide` |
| 11 | Whether the ISO rule on notes to terminological entries differs from notes in text as reported | PCI's recommendation (one informative note type only) is deliberately designed not to depend on this | Directives Part 2 terms-and-definitions provisions |

**Re-verification is a prerequisite for publication, not for drafting.** Stage 3 drafting can
proceed on the method findings in §6 and §7, because those are principles of construction that PCI
adopts as its own. Only citation of the sources requires the checks above.

---

*End of report. Verification date 2026-08-04. No source in this report reached direct-fetch
verification; see §2.1.*
