# PCI Authority Engine — Stages A–D

Execution of the master prompt for the 500-article authority programme, produced against the
platform repository as source of truth. **Stages A, B, C and D are complete. Stage E (production
batches) must not begin until the approval items below are cleared** — six of the ten pilots hold a
flag only an operator can lift.

## Stage A — research and strategy foundation

| § | Deliverable | File |
|---|---|---|
| 1 | PCI fact dossier (10 tables incl. claims register) | `FACT_DOSSIER.md` |
| 2 | Five-domain URL inventory | `FACT_DOSSIER.md` §2 |
| 3 | Missing-information and approval list | `MISSING_INFO_APPROVALS.md` |
| 4 | Competitor primary-source research plan | `COMPETITOR_RESEARCH_PLAN.md` |
| 5 | 25-cluster allocation totalling exactly 500 | `CLUSTER_ALLOCATION.md` |
| 6 | Master ledger, rows A-001–A-050 (32-column schema; 10 Stage C pilots flagged) | `MASTER_LEDGER.csv` |
| 7 | Continuation token/format for rows 51–500 | `CONTINUATION.md` |

## Stage B — the complete ledger

`MASTER_LEDGER.csv` now holds **all 500 rows × 32 columns**, built from a curated topic corpus by
`_build/build_ledger.py` (rerunnable; rows 1–50 are never rewritten). Audit in `STAGE_B_REPORT.md`:
zero duplicate keywords, slugs, titles or questions; every cluster total exact; all seven quota
minimums met; balanced across the three credentials; 25 pillars.

## Stage C — the ten pilot articles

In `pilots/`, each carrying the full 22-point output contract, the §11 platform repurposing package
(15 platform variants) and the §13 carousel/image system:

| ID | Article | State |
|---|---|---|
| A-001 | What is the Project Controls Institute? | Draft — hold for legal (M5) |
| A-011 | PCI PCL-AI explained | Draft — hold for price/live verification |
| A-016 | PCI PFL-AI explained | Draft — hold for price/live verification |
| A-020 | PCI PML-AI explained | Draft — hold for price/live verification |
| A-026 | Certuvo and PCI: who does what | **Blocked** — access-trigger contradiction (M4) |
| A-029 | Guided labs and simulation labs | Draft — hold for live verification |
| A-032 | PCI World and the Passport | Draft — hold for live verification |
| A-040 | PCL-AI vs PMP | **Blocked** — competitor facts pending |
| A-043 | PFL-AI vs CMA | **Blocked** — competitor facts pending |
| A-046 | Which PCI certification first? | Draft — hold for price verification |

Contract item 21 (copy-ready HTML) is **generated**, not hand-maintained: `_build/md_to_html.py`
emits `pilots/html/*.html` and refuses any article marked `PUBLICATION BLOCKED`.

## Stage D — independent Judge

`STAGE_D_JUDGE.md`: no automatic failures in the batch; 8 of 10 meet the pass bar (avg 4.88–4.94);
2 correctly blocked and unscoreable until competitor research runs. Two defects were found and fixed
in the revision round rather than deferred.

## Read this first — where the master prompt itself was wrong

The fact sweep corrected four assumptions baked into the brief (details + citations in the
dossier): **PML-AI's title is "PCI Project Management Leader – AI"** (suffix form, not
"PCI AI Project Management Leader"); the route story is **Standard / Founding / Honorary** (no
"experience"/"recognition" routes); the honorary phrase is **"apply for the board's
consideration"** (not "invitation to be considered" — "invitation" is Founding-route vocabulary);
and **Certuvo access follows an active membership**, not the exam fee, per the platform's own
provisioning rule. Two of the five claimed domains (`pciai.org`, `pciglobal.ai`) have **no
evidence in the platform** and must not be linked until PCI confirms them.

## What must happen before Stage E (production)

In priority order — the first two block roughly a sixth of the whole programme:

1. **Execute the competitor research** (`COMPETITOR_RESEARCH_PLAN.md`). The ledger contains **86
   comparison articles**; every one inherits A-040/A-043's blocker. Do this *before* Stage E starts,
   not alongside it — the Judge's standing note is that the temptation to fill a table from memory
   peaks when a batch deadline is due.
2. **Resolve the Certuvo access trigger** (M4) — membership or exam fee. A-026 and every
   pricing/access article depend on it.
3. **Run the live-verification pass** (M2): stamp `verified on` dates against every fee, syllabus and
   policy fact; lift `[PRICE UNVERIFIED]` / `[LIVE-SITE VERIFICATION PENDING]`.
4. **Confirm or drop `pciai.org` and `pciglobal.ai`** (M3) so the ecosystem block can be completed.
5. **Legal decision on the nonprofit wording** (M5), and correct the platform's JSON-LD to match.
6. **Supply the workbook** (M1) and reconcile it against the dossier.

Then issue the continuation command from `CONTINUATION.md` for Stage E, in batches of ten, with the
Judge run at the end of each batch.
