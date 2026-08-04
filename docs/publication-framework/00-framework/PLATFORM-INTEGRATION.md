# Platform Integration

How the 100 documents reach readers through the PCI platform, and — critically — how they sit alongside
the document libraries the platform **already ships**.

This file exists because the framework was written after the platform, not before it. Ignoring what is
already seeded would produce a second, competing library and a confused Downloads Centre.

---

## 1. What the platform already publishes

Three seeded libraries already populate `public_documents`:

| Seed | What it ships | Category |
|---|---|---|
| `backend/Data/PublicDocsSeed.cs` | The governance and candidate document set (policies, handbooks, routes, privacy, fees) | `policies`, `certification-governance`, `candidate-handbooks`, `application-routes`, `exams`, … |
| `backend/Data/TemplatesLibrarySeed.cs` | **25 CSV templates** mirroring the Simulation Lab engines | `templates` |
| `docs/downloads/` (built via `build_docs.py`) | Candidate handbook, examination blueprint, code of professional conduct, glossary, formula sheet, sample questions, study guide | — |

**None of these are superseded by this framework.** The relationship is defined in §3 and §4.

---

## 2. The `public_documents` model

Every document is **version-chained**: each version is its own row sharing a stable `doc_group` — the
public identifier. `is_current` flags the live version, and `supersedes_id` points at the row it replaced.
A public request only ever sees rows with `status='published'` **and** `visibility='public'`, so drafts,
internal documents, withdrawn and archived versions are never distributed.

That model maps onto this framework almost exactly, which is convenient rather than accidental — both were
built around the idea that a published document's history must survive its corrections.

| Framework front matter | `public_documents` column | Note |
|---|---|---|
| `id` (e.g. `BPG-08`) | `doc_group` = `pub-bpg-08` | The ID is the permanent citation key; the `doc_group` is its public form. Never reused, even after retirement. |
| `title` | `title` | |
| `summary` | `description` | Written once, used in the Downloads Centre card and the LinkedIn blurb |
| `version` | `version` | A new version inserts a new row; it never overwrites bytes — see §5 |
| `status` | `status` | Mapped in §2.1 |
| `series` | `category` | Mapped in §2.2 |
| `related` | `related_groups` | Comma-separated `doc_group` values |
| `gated: true` | — | Gating is an application concern, not a document column — see §6 |

### 2.1 Status mapping

| Framework | Platform `status` | Platform `legal_review_status` |
|---|---|---|
| `draft` | `draft` | `draft` |
| `in-review` | `under_review` | `internal_review_completed` |
| `approved` | `approved` | `external_legal_review_completed` |
| `published` | `published` | `approved_for_publication` |
| `retired` | `withdrawn` or `superseded` | unchanged |

Note the platform's `legal_review_required` state. Series S04 (ethics), S05 (certification handbook) and
S06 (blueprint) carry contractual and quasi-regulatory weight; they route through
`legal_review_required` before `approved`, and the framework's own gate 7
(`GOVERNANCE-AND-REVIEW.md` §3) is not a substitute for that.

### 2.2 Category mapping

| Series | `category` |
|---|---|
| S01 `PCB` Body of Knowledge summary | `certification-governance` |
| S02 `AIG` AI guide | `general` |
| S03 `CMP` Competency frameworks | `certification-governance` |
| S04 `ETH` Code of ethics | `policies` |
| S05 `CER` Certification handbook | `candidate-handbooks` |
| S06 `EXB` Exam blueprint | `exams` |
| S07 `CAR` Career roadmap | `general` |
| S08 `SAL` Salary and skills | `general` |
| S09 `BPG` Best practice guides | `general` |
| S10 `TPL` Free templates | `templates` |

---

## 3. S10 and the existing CSV template library — the important one

The platform already publishes **25 CSV templates**. Series S10 publishes **16 template documents**. These
are not duplicates, and the distinction must be preserved or both become worse.

**The CSVs are instruments. The S10 documents are the method.**

The CSV library deliberately ships **without embedded formulas** — every field is neutralised against
spreadsheet-formula injection, so a template carrying live formulas would be indistinguishable from an
attack payload. Each CSV therefore carries columns, worked example rows and a short "how to compute" note.

That is exactly the gap S10 fills: the formula stated in words *and* as a spreadsheet expression, the
completion instructions field by field, a worked fragment, the common mistakes, and the rules for adapting
the instrument safely. A CSV cannot carry any of that.

### 3.1 Where an S10 document has a CSV counterpart

Pair them: the Downloads Centre card links both, and the S10 document names the CSV in its
*When to use this* section.

| S10 document | Existing CSV `doc_group` |
|---|---|
| `TPL-02` WBS and dictionary | `template-wbs-dictionary` |
| `TPL-03` CBS and code of accounts | `template-cost-breakdown` |
| `TPL-05` Progress measurement sheet | `template-progress-measurement` |
| `TPL-07` EVM calculation sheet | `template-evm-tracker` |
| `TPL-09` Cash flow forecast | `template-cash-flow` |
| `TPL-10` Risk register | `template-risk-register` |
| `TPL-11` QSRA input sheet | `template-three-point-estimate` |
| `TPL-12` Change order log | `template-change-log` |
| `TPL-16` Lessons learned register | `template-lessons-learned` |

### 3.2 Where an S10 document has no counterpart

These are genuinely new, and are documents rather than data tables — a form, a plan structure, a narrative
skeleton or an assessment instrument, none of which reduces to a CSV grid:

`TPL-01` controls execution plan · `TPL-04` baseline change request · `TPL-06` monthly report ·
`TPL-08` EAC scenario comparison · `TPL-13` claim and EOT narrative structure ·
`TPL-14` schedule quality review checklist · `TPL-15` project controls health check

### 3.3 Where the CSV library has no S10 counterpart

Sixteen CSVs stand alone, including resource histogram, bill of quantities, productivity log, schedule
network, earned schedule, procurement log, decision matrix, portfolio scorecard, data quality check, RAID
log, milestone tracker, contingency drawdown, variance analysis, status report, S-curve data and
stakeholder register.

**These are candidates for a future S11**, not gaps in the current hundred. Do not quietly add them to
S10 — the registry count is a contract.

---

## 4. S05, S06 and the existing candidate documents

`docs/downloads/candidate-handbook.md`, `examination-blueprint.md` and `code-of-professional-conduct.md`
are live documents. S05, S06 and S04 expand and eventually supersede them.

**Sequence matters.** The existing documents stay live and unchanged until their replacements are
`approved`. At switchover the old document is marked `superseded` (never deleted, never byte-swapped) and
the new version is inserted into the same `doc_group` so the public identifier and every historical link
survive. The platform enforces this: once a version has left the pre-publication pipeline, replacing its
bytes in place returns `409 published_file_immutable`.

That 409 is a feature. It is the same principle as `GOVERNANCE-AND-REVIEW.md` §6: a published document is
corrected in the open, with a new version, not quietly overwritten.

---

## 5. Publishing a document

1. Author in this framework; pass the gates in `GOVERNANCE-AND-REVIEW.md` §3.
2. Resolve every `[CONFIRM: …]` placeholder. `placeholders: 0`.
3. Render to PDF for the Downloads Centre; keep the Markdown as the editable source of record.
4. Insert a new `public_documents` row with the mapped `doc_group`, category and status.
5. For a revision, insert a **new row** in the same `doc_group`, set `supersedes_id`, move `is_current`.
   Never edit published bytes.
6. Schedule the LinkedIn slot from `PUBLISHING-CALENDAR.md`.

---

## 6. Gating

Six documents are gated (`gated: true`): `CER-01`, `SAL-05`, `TPL-06`, `TPL-14`, `TPL-15`.

Gating is an application concern — an email-capture step in front of the download — not a
`public_documents` column. `visibility` is `public` versus `internal`, which is a different question
entirely: an `internal` document is never distributed at all.

**Do not implement gating by marking a document `internal`.** That would remove it from the public
catalogue rather than putting a form in front of it, and it would break the download-count reporting that
makes the gate worth having.

The default is ungated, and teaching is never gated — `LINKEDIN-PLAYBOOK.md` §6.

---

## 7. What this framework does not touch

The ~216 server-rendered marketing pages, the content-injection system (`Core/PageContent.cs`,
`CertCatalogue`, `ListSections`, `PriceTags`) and the `site_content` / `page_blocks` tables are a separate
mechanism with its own editing surface. A document in this framework may be *linked from* a page, but it
never becomes page content, and nothing here requires a schema change or a redeploy.
