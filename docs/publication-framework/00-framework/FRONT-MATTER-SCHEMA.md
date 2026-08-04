# Front-Matter Schema

Every manuscript in this framework opens with a YAML front-matter block delimited by `---`. The block is
the machine-readable half of the document: it drives the registry, the publishing calendar and the
Downloads Centre import.

---

## 1. The block

```yaml
---
id: BPG-08
series: S09
series_name: Best Practice Guides
title: Earned Value in Practice
subtitle: Making EV mean something on a real control account
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 14
summary: >
  One-sentence-to-one-paragraph plain-language statement of what the reader gets.
linkedin:
  format: article
  hook: >
    The single sentence that has to earn the click, and that must be true on its own.
  tags: [ProjectControls, EarnedValue, CostEngineering, ProjectManagement]
  asset: carousel-8
gated: false
related: [BPG-06, BPG-09, TPL-07]
bok_domains: [6]
sources: []
placeholders: 0
---
```

---

## 2. Field reference

| Field | Type | Required | Rules |
|---|---|---|---|
| `id` | string | yes | `PREFIX-NN` from the registry. Immutable once assigned — it is the document's permanent citation key. |
| `series` | string | yes | `S01`–`S10` |
| `series_name` | string | yes | Must match the registry exactly |
| `title` | string | yes | Sentence case, no trailing full stop, no marketing adjectives |
| `subtitle` | string | no | A clarifying second line; not a slogan |
| `version` | string | yes | `MAJOR.MINOR`. Minor for corrections and clarifications; major for a change that alters guidance. |
| `status` | enum | yes | `draft` · `in-review` · `approved` · `published` · `retired` |
| `date` | ISO date | yes | Date of the current version |
| `authors` | list | yes | `[PCI Editorial]` unless a named subject-matter expert has genuinely authored it. Never attribute AI-assisted text to a named expert who did not write it. |
| `audience` | list | yes | Any of `student` · `practitioner` · `manager` · `executive` · `employer` · `academic` |
| `level` | enum | yes | `foundation` · `practitioner` · `professional` · `leader` — matches the S03 competency levels |
| `reading_time_min` | integer | yes | Word count ÷ 220, rounded up |
| `summary` | string | yes | Plain language. Reused verbatim as the Downloads Centre description and the LinkedIn document blurb. |
| `linkedin.format` | enum | yes | `post` · `carousel` · `article` · `newsletter` · `document` |
| `linkedin.hook` | string | yes | Must be true standing alone, with no caveat withheld |
| `linkedin.tags` | list | yes | 3–5 tags, no spaces, PascalCase |
| `linkedin.asset` | string | no | Companion asset to produce, e.g. `carousel-8`, `one-pager`, `checklist-pdf` |
| `gated` | boolean | yes | `false` = open download. `true` = email capture. Templates and teaching are ungated by default; only candidate packs and the annual report are gated. |
| `related` | list | yes | Real IDs from the registry. Minimum two, except where a document genuinely stands alone. |
| `bok_domains` | list | no | Domain numbers 1–13 from `docs/bok/` that this document draws on |
| `sources` | list | no | Named, dated, verifiable sources. Empty list is honest; a fabricated entry is a rejection defect. |
| `placeholders` | integer | yes | Count of unresolved `[CONFIRM: …]` markers. Must be `0` before `status: approved`. |

---

## 3. Validation rules

1. `id` is unique across the framework and matches its directory's series prefix.
2. `status: approved` requires `placeholders: 0`.
3. `status: published` requires an entry in `PUBLISHING-CALENDAR.md`.
4. Every ID in `related` resolves to a real document in `ASSET-REGISTRY.md`.
5. `reading_time_min` is within ±20 % of the actual word count ÷ 220.
6. `linkedin.tags` has 3–5 entries.
7. `sources` entries are objects or strings that name the source and its date — never a bare URL with no
   context, never a plausible-looking citation that was not consulted.

---

## 4. Why the placeholder count is a field

Because it makes an unfinished document *visibly* unfinished at a glance and in a grep, rather than
relying on a reviewer to spot a bracket buried on page four. `grep -c 'CONFIRM:'` across the framework is
the release gate — see `GOVERNANCE-AND-REVIEW.md` §5.
