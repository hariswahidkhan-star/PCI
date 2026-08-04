---
id: PCB-05
series: S01
series_name: Body of Knowledge — Executive Summary
title: How the Body of Knowledge was built
subtitle: The authoring order, the evidence rule, the arithmetic discipline and the gates a chapter must survive
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, academic, employer]
level: professional
reading_time_min: 7
summary: >
  A body of knowledge is only worth as much as its method. This document sets out how the PCL-AI Body of
  Knowledge was authored: the style spine that fixed notation before content, the sequence in which the
  thirteen domains were written and why, the per-knowledge-area completion checklist, the four-way
  classification of every claim, the arithmetic verification rule, and the review gates a chapter must
  pass — including what is still unfinished.
linkedin:
  format: article
  hook: >
    We fixed the symbol table, the worked-example format and the citation rules before a single domain was
    written — because thirteen chapters authored in parallel without a spine become thirteen different
    books. Here is the whole method, including what has not yet been reviewed.
  tags: [BodyOfKnowledge, Standards, ProfessionalCertification, EditorialGovernance, ProjectControls]
  asset: one-pager
gated: false
related: [PCB-01, PCB-02, PCB-04, EXB-04, EXB-06]
bok_domains: [1, 3, 6, 9, 13]
sources:
  - "The Style Spine — Conventions of This Reference (docs/bok/00-style-spine.md), §§1–11, August 2026"
  - "PCL-AI Body of Knowledge README and domain drafts (docs/bok/), August 2026"
  - "PCI Editorial Charter (docs/books/EDITORIAL_CHARTER.md), §§3–10, August 2026"
  - "Examination Blueprint (docs/downloads/examination-blueprint.md), §5, August 2026"
placeholders: 2
---

# How the Body of Knowledge was built

> The authoring order, the evidence rule, the arithmetic discipline and the gates a chapter must survive.

**In one paragraph.** A body of knowledge is only worth as much as its method. This document sets out how
the PCL-AI Body of Knowledge was authored: the style spine that fixed notation before content, the sequence
in which the thirteen domains were written and why, the per-knowledge-area completion checklist, the
four-way classification of every claim, the arithmetic verification rule, and the review gates a chapter
must pass — including what is still unfinished. A reader can use it to judge how much weight the framework
deserves, and to hold the Institute to its own standard.

**Who this is for.** Practitioners deciding how seriously to take the framework, subject-matter experts
considering reviewing it, and anyone building a body of knowledge of their own who would rather not
rediscover the failure modes.

---

## 1. The problem a body of knowledge has to solve

Authority is the whole problem. A framework that will decide whether someone passes an examination cannot
borrow its authority from the confidence of its prose. It has to be built so that a hostile expert can find
the joins: where a claim is a standard's requirement, where it is the Institute's recommendation, where
competent practitioners legitimately differ, and where a number came from.

A second problem sits underneath it. A reference of this size cannot be written in one pass by one person,
and material written in parallel drifts — the same symbol means two things by chapter nine, the same concept
is defined three times in slightly different words, and the worked examples stop looking like they came from
the same book. That drift is not a cosmetic failure. A candidate who meets `PV` as Planned Value in one
domain and present value in another has been set up to fail an item that tests neither.

Both problems are solved before writing, not after.

## 2. The style spine — fixing the conventions before the content

The first document authored was not a domain. It was the style spine, which fixes seven things and binds
every domain to them.

**Terminology.** A term is defined once and used identically everywhere; silent redefinition in a later
chapter is a defect, not an inconsistency. Terms introduced by a domain are added to the shared glossary in
a consolidation pass rather than living locally.

**The symbol table.** One table of symbols with units, seeded up front even for notation first used deeply in
later domains — so that the classical and agile treatments of earned value share one notation rather than
two. Where a genuine clash exists it is resolved by an explicit rule: `PV` denotes Planned Value in
earned-value contexts and present value in discounting contexts, and discounting contexts must write
"present value" in words or `PV(x)` so the two never collide on a page.

**The worked-example format.** Five lines, every time: setup with the given data, the formula with each
variable named and its unit, the substitution shown explicitly, the result with its unit and rounding, and an
interpretation of what the number means for the decision. Where a computation is inherently tabular — a
network forward and backward pass, a multi-scenario comparison — the table replaces the first four lines.
The interpretation line is never optional, because a number a reader cannot act on has not been taught.

**Figure specifications.** Every figure carries a number on the same Domain.KA pattern, a caption, the
underlying data so it is reproducible, and a render-ready description. A figure whose data is not stated
cannot be checked, and an unpublishable figure specification is usually a sign the point was not clear.

**The item format.** Sample items are four options, one key, with a rationale that says why the key is right
*and* why each plausible distractor is wrong, tagged with the topic number and cognitive level. Distractors
are the results of common errors, never filler. Sample items are maintained separately from any live
examination content.

**Citation rules and language conventions.** Treated in §4 and §5 below.

## 3. The authoring order, and why it is not domain one to thirteen

The domains were written in a deliberate sequence dictated by dependency, not by numbering.

The four finance domains were authored first, to lock the notation and the worked-example style on material
where precision is least forgiving. The project management domains inherited that style. The agile domain
was authored **after** earned value, because agile cost forecasting reuses the earned-value machinery on
variable scope and would otherwise have invented a parallel notation for the same arithmetic. The AI domain
was authored **last**, so that its workflows could cross-reference the domains they actually operate on
rather than describing AI in the abstract.

That ordering is why Domain 13's largest knowledge area is a systematic pass across the earlier domains
rather than a survey of tools. It could not have been written first.

## 4. The evidence rule — four kinds of statement, each marked

Every claim in the framework is one of four things, and the reader is told which.

A **fact** is verifiable and, where it matters, attributed to a named framework. A **recommended practice**
is what the Institute advises, stated as advice. A **professional judgement** is a point where competent
practitioners legitimately differ — said so explicitly, with the grounds for choosing given rather than
withheld. A **PCI interpretation** is the Institute's position, labelled as the Institute's.

Two absolute rules govern sources. **Name real frameworks; never fabricate.** IFRS 15, IAS 1, IAS 2, IAS 16,
IAS 23, IAS 37 and IFRS 16; the PMBOK Guide and AACE's Total Cost Management framework; ISO 31000 and
ISO/IEC 17024; the Agile Manifesto, the Scrum Guide, Kanban and Lean are named where they are relevant. No
clause number, page reference or quotation is invented. **Never reproduce protected text.** A standard's
principle is explained in the framework's own words — "under IAS 37 a provision is recognised when…" — and
its wording, tables, diagrams and question banks are never pasted. Every example, table, figure and item is
original.

A third rule governs the AI material specifically: describe real capabilities *and their limits* —
hallucination, data quality, bias, confidentiality, auditability — mark evolving capabilities as evolving,
and never hype. The governing principle is that AI proposes and the professional disposes.

The rule that makes the others enforceable is the prohibition on fabrication of any kind: no invented
quotations, case studies, company names, project data, survey results or statistics. Cases are original or
properly anonymised. A plausible-looking citation that was not consulted is treated as a conduct matter, not
a style error.

## 5. Arithmetic, jurisdiction and language

**Every calculation is independently recomputed by someone other than its author**, and numerical items get
two reviewers. Units, currency, period and rounding are stated; the substitution is shown, not only the
result; and where an answer depends on an assumption, the assumption is named in the same breath as the
answer. Rounding is fixed by convention — money to the whole currency unit unless the topic needs cents,
ratios and indices to two decimal places, percentages to one — and stated wherever it affects the answer.
A chapter containing an unverified calculation is rejected at gate.

**No jurisdiction's legal, tax or accounting treatment is presented as universal.** Where treatment varies,
the framework says that it varies and teaches the principle. Where an example needs a legal frame, the frame
is stated as an assumption of the example. Examples are currency-explicit; no live or precise exchange rate
is implied.

**British English throughout**, with acronyms expanded on first use and complete sentences rather than
bulleted fragments where prose carries the meaning better.

## 6. The per-knowledge-area completion checklist

A knowledge area is not finished when it is long. It is finished when every applicable line of a published
checklist is satisfied: a precise definition and purpose for each topic with the real standard named; the
underlying principle; every formula stated with each variable and unit defined; at least one fully worked
example in the five-line format with the numbers re-checked; a second example or mini-case for any
non-trivial topic; at least one numbered figure specification; common pitfalls and misconceptions; a
treatment of how AI assists that area and where it must not be trusted; a key-terms box; three to six sample
items with rationales and level tags; two to four self-check questions with answers; and cross-references by
number to related topics.

The checklist is what makes depth auditable. "Does this knowledge area have a worked example whose numbers
add up?" is a question with an answer; "is this chapter deep enough?" is not.

## 7. The gates, and what fails at them

Every chapter passes a fixed sequence of reviews before it is treated as content: pattern conformance,
alignment to the competency framework, technical accuracy, calculation verification, source verification,
originality, style, legal and company-mention review, diagram and table review, accessibility,
cross-reference integrity, item validation, and final editorial approval. A reviewer never reviews their own
draft. One chapter has one owner and one file; two authors never edit the same manuscript concurrently.

Four things fail at gate regardless of quality elsewhere: a placeholder citation, an unresolved calculation,
duplicated prose, and generic filler. Length is explicitly not an acceptance criterion — the standing rule is
that page count is an output check, never a writing method, and that no phase may be skipped to meet one.
Content is never merged because it is long.

## 8. What is not yet done

Three things are outstanding, and the framework is published saying so.

**Subject-matter-expert review.** Every domain is a **first authored draft**. It has passed the internal
gates in §7; it has not yet been through independent subject-matter-expert review, and until it has, it is
not treated as final certification content. Panel composition: `[CONFIRM: the number of subject-matter
experts on the first-edition review panel and their disciplines]`. Completion: `[CONFIRM: date
subject-matter-expert review of the first edition completes]`.

**The job-task analysis.** The blueprint's sampling of the domains has not yet been validated against
practitioners' actual work. That study will confirm item counts — which is why no item count is published
anywhere — and will test the group weightings defended in `PCB-03 — Why 40/40/20`.

**Standard setting.** The pass mark configured on the platform is sixty-five per cent; the definitive
standard will be set by a modified-Angoff study, which is a documented judgement about what a minimally
competent professional must demonstrate.

Publishing a method with its gaps visible is a choice with a cost: it invites the objection that the
framework is unfinished, which it is. The alternative — publishing a confident framework and disclosing the
gaps after the first cohort has paid — is the reason certification bodies are distrusted. A reader who can
see exactly which parts are settled can challenge the parts that are not, and that challenge is worth more
to the Institute than the appearance of completeness.

---

## Related

- `PCB-01 — The Project Controls Body of Knowledge — executive summary` — what the material built this way claims
- `PCB-02 — The thirteen domains at a glance` — the domain and knowledge-area structure this method produced
- `PCB-04 — What a project controls professional must know` — the propositions the framework was built to support
- `EXB-04 — How items are written, reviewed and retired` — the item-development standard that continues from §2
- `EXB-06 — Job-task analysis: where the blueprint gets its authority` — the outstanding study named in §8

## Sources and standards

- The Style Spine — Conventions of This Reference (`docs/bok/00-style-spine.md`), August 2026: §1 scope and
  weighting, §3 the seed glossary, §4 the master symbol table and notation-clash rule, §5 the worked-example
  format, §6 figure specifications, §7 language and rounding conventions, §8 the item format, §9 citation
  rules, §10 the per-knowledge-area checklist.
- PCL-AI Body of Knowledge, first authored draft and its README (`docs/bok/`), August 2026: the authoring
  order and the per-domain draft status.
- PCI Editorial Charter (`docs/books/EDITORIAL_CHARTER.md`), August 2026: §3 the non-negotiable quality
  rules, §6 concurrent-authoring and file-ownership rules, §7 the chapter quality gates, §8 phase gates,
  §10 the definition of done.
- Examination Blueprint (`docs/downloads/examination-blueprint.md`), August 2026: §5, the job-task analysis
  and modified-Angoff commitments.

The standards named in §4 are named as frameworks the Body of Knowledge references and explains in its own
words. No edition, clause or wording is cited here, because none was verified for this document.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
