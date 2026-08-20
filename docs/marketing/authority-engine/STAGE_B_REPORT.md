# Stage B report — ledger complete, deduplicated, totals reconciled

Produced by `_build/build_ledger.py` (rerun any time; rows A-001–A-050 are never rewritten).
`MASTER_LEDGER.csv` now holds **500 rows × 32 columns**, IDs `A-001`…`A-500`.

## 1. Deduplication and cannibalisation

| Check | Result |
|---|---|
| Duplicate primary keyword | **0** |
| Duplicate slug | **0** |
| Duplicate working title | **0** |
| Duplicate "real question answered" | **0** |

Two exact collisions were found on the first build and resolved at source rather than by deleting a
brief: `project assurance review` was claimed by both A-121 (PML-AI delivery assurance) and A-290
(governance assurance reviews) — the PML-AI row moved to `delivery assurance review`; and "What
trips candidates up?" was shared by the PFL-AI and PML-AI mistake articles, now specialised per
credential.

**Shared keyword heads reviewed, kept, and why** (a shared head is not cannibalisation when a
modifier separates the intent — master prompt §9 deduplicates by *intent*, not wording):

| Cluster | Head | Rows | Why they do not compete |
|---|---|---|---|
| 22 | `project controls jobs` | 20 (A-407–A-426) | Each carries a country modifier and must be written with genuinely local evidence; the `[LOCAL EVIDENCE REQUIRED]` flag blocks any thin geo-doorway page from passing the gate |
| 3 | `project controls certification` | 3 (A-087–A-089) | Industry modifiers: construction / energy / defence-and-regulated |
| 6 | `project controls to` | 2 (A-125, A-126) | Distinct destinations: → project finance, → leadership |
| 25 | `is pci certification` | 2 (A-047, A-477) | A-047 answers legitimacy-and-how-to-check; A-477 answers the accreditation position specifically |

## 2. Cluster totals — all exact

Every cluster matches `CLUSTER_ALLOCATION.md` (16/20/20/20/20/18/16/20/20/18/20/20/20/18/20/20/20/16/14/20/20/20/28/27/29), summing to **500**. 25 pillar pages, one per cluster.

## 3. Quota tags — all minimums met

| Tag | Actual | Minimum |
|---|---|---|
| comparison | 86 | 75 |
| faq | 76 | 75 |
| career | 71 | 60 |
| applied | 105 | 45 |
| trust | 32 | 30 |
| pricing-route | 25 | 25 |
| asset (research/calculator/checklist/template/tool) | 36 | 15 |

Retagging was done at source in the topic corpus, not by relabelling finished work: the
"what is examined" rows in clusters 3–5 and the navigational MyPCI rows are genuine direct-student-
question articles, and the eligibility/validity/access rows in cluster 2 are genuine route-and-access
articles.

## 4. Certification balance

PCL-AI 21 · PFL-AI 22 · PML-AI 20 dedicated rows, plus the shared clusters (2, 6, 23–25) that treat
all three. No credential is over- or under-served.

## 5. Risk flags carried into Stage C

Every row inherits the dossier's constraints. Counts across the 500:

- `[LIVE-SITE VERIFICATION PENDING]` — all PCI-property rows (clusters 1–10)
- `[COMPETITOR FACTS PENDING]` — every comparison row; **no comparison article may be drafted to completion until `COMPETITOR_RESEARCH_PLAN.md` has been executed**
- `[PRICE UNVERIFIED]`, `[VERIFY CERTUVO ACCESS TERM]`, `[LEGAL STATUS — PCI APPROVAL REQUIRED]`,
  `[STANDARD SOURCE REQUIRED]` (cluster 19), `[LOCAL EVIDENCE REQUIRED]` (cluster 22)

## 6. What Stage B did not do

It did not draft articles (correct — that is Stage C), and it did not resolve any Stage A blocker.
The workbook and live-verification gaps (`MISSING_INFO_APPROVALS.md` M1/M2) remain open, so the
ledger's verification-date column is empty by design.
