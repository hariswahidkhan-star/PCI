---
platform:      Zenodo (CERN / OpenAIRE open repository) — record description field for a DOI deposit
type:          repository-record
title:         "The PCI Standards: 113 mandatory Standards and 532 process requirements for project controls, project finance and governed AI"
when_to_post:  "Not on a posting clock. A published Zenodo record registers its DOI with DataCite at the moment you press publish, the files become permanent, and there is no unpublishing — so the deposit waits until the requirement text is final, not until launch week wants a link. Sequence: reserve the DOI in the deposit form first (Zenodo issues it before publication), give the reserved DOI to the PM World Journal manuscript and the own-site Body of Knowledge page so both carry a real identifier on the day they appear, publish the record once the hub page is live and indexed, then check the DOI actually resolves before any asset announces it. Registration and OpenAIRE harvesting are not instantaneous; a launch tweet pointing at a DOI that 404s for an hour is worse than no DOI. Publish at least a week ahead of the journal submission deadline you are aiming at, because a reference list with a resolving DOI is checked once and a reference list without one is queried."
word_count:    305 words / 2,001 characters (the description field only, from "The PCI Standards are" to "forms no part of this deposit"; the deposit-form sheet and the notes are working material and are not deposited)
hashtags:      None, and none may be added. Zenodo has no hashtags; it has a Keywords field that feeds DataCite, OpenAIRE and every downstream index, and a hashtag pasted into it is a dead term nobody searches. Enter these, one per line, exactly — project controls; earned value management; cost engineering; project finance; accrual accounting; revenue cut-off; forecasting; AI governance; professional certification; body of knowledge.
cta_link:      https://projectcontrolsinstitute.org/body-of-knowledge
credential:    suite — PCL-AI, PFL-AI and PML-AI all named, because the deposit is the framework all three rest on
target_domain: projectcontrolsinstitute.org
canonical:     original deposit — the Zenodo record is the citable copy of record; the own-site Standards and Body of Knowledge pages remain the readable copy and are not canonicalised to Zenodo
schema:        "Publication / Standard — the DataCite resource type this record is deposited as; Publication / Report only where the menu offers no Standard"
ab_id:         n/a — flagship launch asset, not an Article Bank brief

notes: |
  ================= DECISION REQUIRED BEFORE DEPOSIT: THE LICENCE =================
  A person must choose this, not a writer. The choice is one-way in the direction that matters: a
  licence can be loosened at version 2 and never tightened, because version 1.0 keeps whatever you
  choose today and so does every copy anybody took under it. Zenodo will not let you retract that.
  The options table is in the deposit sheet below, with what each one costs the Institute rather
  than what each one is called. The short version of the argument, for whoever decides:
  the property a normative document needs is that a modified requirement cannot circulate as a PCI
  Standard, and only the ND licences preserve it. The property a young certifying body needs is
  citation, and ND is not an open licence under the Open Definition, so some reuse policies and
  aggregation pipelines discount it. Those two properties are in direct conflict and no wording
  fixes that. CC BY-ND 4.0 is the recommendation if integrity wins; CC BY 4.0 if reach wins; and
  the recommendation is worth less than ten minutes of the decision being made deliberately.
  Note also that Zenodo's access right and its licence are separate fields. An ND record is still
  Open Access in Zenodo's own terms. Do not let the two questions be answered as one.

  RESOURCE TYPE — SETTLED, AND HERE IS THE REASONING. This is a document, so it deposits as
  "Publication → Standard" where the menu offers it, and "Publication → Report" where it does not.
  The front matter now carries that one value rather than a choice, and the filename says deposit
  rather than dataset for the same reason. "Dataset" is correct only if the requirements are
  uploaded as structured, machine-readable rows rather than as a document; if that is ever what
  ships, the type and the front matter change together. The type governs how OpenAIRE and every
  aggregator file the record, and a document filed as a dataset is found by nobody looking for either.

  CREATORS — THIRD DECISION, AND THE ONE MOST LIKELY TO BE FUDGED. Zenodo creators are people or
  organisations with identifiers. Enter the named authors of the framework, or enter Project
  Controls Institute Global as a corporate creator with its ROR identifier if it has one. Do not
  invent an ORCID, do not attach an ORCID belonging to someone who did not write this, and do not
  register a ROR record in a rush to fill a field. A fabricated identifier in a DataCite record is
  permanent, machine-readable and trivially checked, which makes it the single worst place in this
  entire launch to guess.

  FILE MANIFEST GAP, DELIBERATE. The description describes contents, not files, because the file
  set is not decided here. If the deposit is split — requirement register, mapping to the three
  Bodies of Knowledge, a changelog — add one sentence listing the files immediately before the
  scope-note paragraph and recount to stay inside 300 words. Do not list a file that is not in the
  upload. The first sentence commits to depositing the framework "in full", and a DOI is permanent:
  if what is actually uploaded is ever less than the whole requirement set, "in full" comes out of
  the description in the same edit. That phrase is the only claim in this record that a reader can
  falsify by downloading the files.

  REGISTER: repository metadata. Third person, no second person, no imperative, no adjective a
  cataloguer would strike. Zenodo descriptions are harvested verbatim by OpenAIRE and reproduced by
  aggregators with no surrounding site, so every sentence has to be true standing entirely alone —
  which is also exactly what makes a block quotable to a model. Zenodo metadata is itself CC0, so
  this text will be copied whatever the file licence says. Write it as though it will be read
  without the Institute's name above it, because it will be.
  THE ASK IS DELIBERATELY NOT A CALL TO ACTION. _STORY.md §5 asks the reader to read the syllabus
  and test it against their own work. In a repository description an imperative is the fastest way
  to be filed as promotional and ignored by the librarians and the indexes that make a DOI worth
  having. So the ask is rendered as an availability statement — the framework is published in full
  at the hub, and the phrase "cited and checked rather than described" carries the invitation. That
  is the same ask in the only register this platform accepts.
  Hook B (contrast first) per _STORY.md §2, unblended, and placed second rather than first because a
  repository record must open by saying what the record is. The contrast then lands as the scope
  rationale, which is what a cataloguer reads that paragraph for anyway.

  LINKS — ONE URL IN THE RECORD, ONE RELATION HELD BACK, AND WHY. One link, one domain, per
  _LINK_ARCHITECTURE.md §2. The hub Body of Knowledge URL sits in the final paragraph, in the
  sentence that raises the question it answers (where is the full text). It appears once. The
  Related identifiers field carries that same URL as `IsDocumentedBy`, which is a metadata
  assertion in the deposit form rather than a second link, so the form sheet below names the page
  instead of restating the address. credentialfinder.org is NOT in this record at all: the
  verification guide that would have carried the relation does not exist on that domain, and a
  related identifier pointing at a page nobody has published is a false statement in structured
  metadata that anyone can check by following it. When a PCI page does cite the DOI, add
  `IsReferencedBy` to the hub's own verification page at /verify.html, and add it the week that
  citation goes live, not before. That leaves two hub URLs asserted in the metadata and one in the
  description, which is deliberate and is stated here so it stays a decision. §2 caps the links a
  piece places; a DataCite relation is a structured assertion that a named page cites this DOI, it
  is created by the citing page rather than by this record, and it is false if it is dropped for
  tidiness. So nothing is dropped at that point: `IsDocumentedBy` still points at the page the
  description names, and the description itself still carries exactly one URL.
  No pciai.org, pciworld.org or pciglobal.ai link. This record raises no AI-tooling, career or
  regional question, and a repository deposit is the last place an estate should look like an
  estate.

  NUMBERS AUDIT. 113 Standards and 532 process requirements (register). 13/61, 16/61, 16/63 with
  full credential names on first mention (register, _STORY.md §6 Fact 1). 15,613 in a sentence that
  names PFL-AI and PML-AI and states that PCL-AI has no equivalent suite — the scope and the figure
  are inseparable and are never split across sentences. The worked example uses 2,200,000 /
  1,850,000 / 240,000 / 2,090,000 / 1.19 / 1.05 and nothing else, with no currency, no client, no
  sector, no date, and an explicit line saying the figures are illustrative arithmetic. No
  frequency claim: there is no researched figure for how often an accrual is missed, so the
  description does not imply one. No examination weighting anywhere; 40/40/20 is not used in this
  asset at all, which removes the commonest way to get it wrong. No pass rate, no student number,
  no accreditation, no endorsement, no partnership.
  The scope-note paragraph is not defensive boilerplate. In a repository record it is the paragraph
  that establishes rights clearance and prevents the framework being catalogued as a national or
  international standard by an indexer working from the title alone, which is a real risk with the
  word "Standards" in the title and a real problem to unwind afterwards.

  ONE THING TO GET RIGHT ON THE FORM. The en dash in "PCI Project Management Leader – AI" is part of
  the name. Zenodo's form will not correct it and a hyphen there creates a second entity name in
  every index that harvests this record. Paste, do not retype.
---

# Zenodo deposit — description field

Paste everything between the rules into the Description field. Nothing else goes in it.

---

The PCI Standards are the certification requirements of the Project Controls Institute Global: 113 mandatory Standards carrying 532 process requirements. This record deposits the framework in full, so the requirements a project controls credential rests on can be cited and checked rather than described.

Accountancy examinations rarely test float; engineering examinations rarely test cut-off. Requirements on recognition, accrual and cut-off sit alongside progress measurement, schedule integrity, float and forecasting, and a further group governs where an AI system may act on a project record and where it may not.

Earned value of 2,200,000 against invoiced cost of 1,850,000 gives a cost performance index of 1.19. Accrue 240,000 of work performed and not yet invoiced: cost is 2,090,000 and the index 1.05. The error is accounting. The damage is delivery. The figures are illustrative arithmetic, not a project record.

Three credentials rest on the framework: the PCI AI Project Controls Leader (PCL-AI), 13 domains and 61 knowledge areas; the PCI AI Project Finance Leader (PFL-AI), 16 domains and 61 knowledge areas; and the PCI Project Management Leader – AI (PML-AI), 16 domains and 63 knowledge areas. 15,613 machine calculation checks run against PFL-AI and PML-AI, all passing; PCL-AI has no equivalent suite.

The PCI Standards are the Institute's own certification requirements. They are not law, not a national or international standard, and nothing here is legal, tax or accounting advice. No ISO, IFRS, IAS, PMI or AACE text, table or figure is reproduced; those documents are named, not quoted.

Revisions are deposited as new versions under the same concept DOI: cite the version DOI where exact wording matters, the concept DOI where it does not. The Bodies of Knowledge built on these Standards are published in full at https://projectcontrolsinstitute.org/body-of-knowledge, the readable copy. The examination blueprint is an open decision and forms no part of this deposit.

---

## Deposit form — the remaining fields

Working material. None of this is the description.

| Field | Value |
|---|---|
| Resource type | Publication → Standard if the menu offers it; Publication → Report otherwise. Dataset only if the requirements are uploaded as machine-readable rows. |
| Title | The PCI Standards: 113 mandatory Standards and 532 process requirements for project controls, project finance and governed AI |
| Creators | Named authors with ORCIDs, or Project Controls Institute Global as corporate creator with its ROR identifier. Nothing invented. |
| Publication date | The date the requirement text was finalised, not the date of the upload, if those differ. |
| Version | 1.0 |
| Language | English |
| Access right | Open Access |
| Licence | **Decision required — see the table below.** |
| Keywords | project controls; earned value management; cost engineering; project finance; accrual accounting; revenue cut-off; forecasting; AI governance; professional certification; body of knowledge |
| Related identifiers | `IsDocumentedBy` → the hub Body of Knowledge page, using the same URL the description already gives. Add `IsReferencedBy` → the hub's credential verification page at `/verify.html` only once that page cites the DOI. No verification page exists on credentialfinder.org, so no relation to that domain is asserted here. |
| Communities | Search for an open community in standards, project management or engineering education and request to join. Do not create one for a single record. |

## The licence decision

Zenodo requires a licence and applies it per version. Version 1.0 keeps whatever is chosen now.

| Licence | What it permits | What it costs the Institute |
|---|---|---|
| CC BY 4.0 | Redistribution and adaptation, commercial included, with attribution | A competing scheme may build a derivative requirement set on this wording and need only credit it |
| CC BY-SA 4.0 | Adaptation, with derivatives carrying the same licence | Derivatives stay open but still exist; share-alike deters anyone embedding a requirement in an internal procedure |
| CC BY-ND 4.0 | Verbatim redistribution, commercial included; no derivatives | A modified requirement cannot circulate as a PCI Standard, which is the integrity property; but ND is not an open licence under the Open Definition and some reuse policies discount it |
| CC BY-NC-ND 4.0 | Verbatim, non-commercial only | Blocks a training vendor reprinting the set, and also blocks most legitimate readers, whose employers are commercial; NC is ambiguous in practice |
| CC0 1.0 | Everything waived | A Standard that may be altered and reissued with no attribution stops being checkable, which defeats the reason for depositing it |
| Restricted | Metadata public, files on request | An uncitable record earns nothing; the DOI exists and no one can use it |

**Linking note.** One external link, to one domain, because only one sentence in this record raises a question another page answers. The hub Body of Knowledge page sits in the description's closing paragraph, in the sentence asking where the full requirement text is published, named as the readable copy rather than by the primary keyword. It appears once: the Related identifiers field reuses that same address as `IsDocumentedBy`, which is a form field rather than a second placement, so the deposit sheet names the page instead of repeating the URL. No credentialfinder.org link is used anywhere: the verification guide it would have pointed at was never authored, and a related identifier aimed at a page that does not exist is a false assertion in machine-readable metadata. No reciprocal link is asked for. If the hub's verification page later cites the DOI, `IsReferencedBy` records that fact and nothing is dropped to make room for it: a DataCite relation states that a named page cites this deposit, it is created by the citing page rather than placed by this record, and removing a true relation to hold a link count would put a false picture in structured metadata. The description keeps its single URL either way.
