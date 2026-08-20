# Stage A — continuation contract for ledger rows 51–500

Deliverable 7 of the Master Prompt's Stage A: the exact token and format a writer (human or AI)
uses to extend `MASTER_LEDGER.csv` without changing column order or duplicating intent.

## Continuation token

```
CONTINUE LEDGER v1 | file=docs/marketing/authority-engine/MASTER_LEDGER.csv | next=A-051 | through=A-500 | columns=32 | schema=frozen
```

Issue it together with the Stage B command from the Master Prompt ("Continue Stage B. Preserve the
approved dossier and ledger schema…"). The receiving writer must:

1. Re-read `FACT_DOSSIER.md`, `CLUSTER_ALLOCATION.md` and the existing ledger rows first.
2. Append rows `A-051`…`A-500` in ID order, **exactly** the 32 columns below, comma-separated,
   one row per line, double-quoted cells whenever a cell contains a comma.
3. Keep cluster totals equal to `CLUSTER_ALLOCATION.md`; tag quotas are tracked in the
   `Content type` column.
4. Run the intent-deduplication pass across ALL rows (1–500), merge collisions, and report final
   cluster totals before any drafting.
5. Never renumber or reorder existing rows; corrections happen in place with `Status` updated.

## Frozen column order (32)

```
ID | Status | Cluster | Funnel stage | Search intent | Audience | Country/region | Certification/property | Working title | Primary keyword | Supporting keyword cluster | Semantic entities | Real question answered | Content type | Pillar/spoke | Canonical target | Slug | Primary PCI domain | Verified deep URL | Internal links | External primary sources | Comparison bodies | Word range | CTA | Schema | Repurposing package | Image concept | Risk/approval flags | Owner | Verification date | Publication date | Performance notes
```

Column conventions:

- `ID`: `A-001`…`A-500`, zero-padded, never reused.
- `Status`: `Planned` → `Briefed` → `Drafted` → `Judged` → `Approved` → `Published` → `Refreshed`
  (or `Merged-into:<ID>` after deduplication, `Blocked:<reason>`).
- `Content type` carries the quota tags: `comparison`, `faq`, `career`, `applied`, `trust`,
  `pricing-route`, `asset` (original research/calculator/template), or `standard`.
- `Verified deep URL`: only URLs from the dossier's URL inventory with `Verified` status; a
  homepage + `[DEEP URL PENDING]` otherwise (Master Prompt §2: never invent deep URLs).
- `Risk/approval flags`: any of `[LEGAL STATUS — PCI APPROVAL REQUIRED]`,
  `[VERIFY CERTUVO ACCESS TERM]`, `[PRICE UNVERIFIED]`, `[LIVE-SITE VERIFICATION PENDING]`,
  `[COMPETITOR FACTS PENDING]`, or `none`.
- `Repurposing package`: `full` (all §11 derivatives), `social-core` (LinkedIn/X/newsletter), or
  `none` — assigned at briefing, produced at drafting.
