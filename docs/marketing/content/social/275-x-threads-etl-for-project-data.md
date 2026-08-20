---
platform:      X / Threads
type:          thread
title:         ETL for project data, and the CPI it quietly breaks
meta:          The report said CPI 0.98. The job was running at 0.91. Five posts on extract, transform and load, and the 900 activities that never reached a cost account.
primary_kw:    ETL for project data *
secondary_kw:  master data management, XER import, data pipeline, cost breakdown structure
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    312
hashtags:      #ProjectControls #PMO
ab_id:         AB-01381
---

# ETL for project data, and the CPI it quietly breaks

*X / Threads thread — 5 posts, each under 280 characters and each able to stand alone. The link sits in the final post. Character counts are for production; X counts any URL as 23 characters, so the live figures run lower.*

**Post 1/5 — the hook** (181 characters)
The board pack said CPI 0.98. The job was running at 0.91.

The difference was 900 activities that never mapped to a cost account, and almost all of the overrun was sitting in them.

**Post 2/5 — what ETL actually means** (261 characters)
Extract, transform, load.

Extract the XER, the cost ledger and the timesheets. Transform them onto one code, one calendar and one cut-off. Load a snapshot.

Every claim a project report makes rests on the middle step, and the middle step is where nobody looks.

**Post 3/5 — the arithmetic** (248 characters)
Worked case. Budget £48.0m across 6,800 activities. 5,900 map to a cost account, so £41.8m is visible and £6.2m is not.

Mapped only: EV 20.9 ÷ AC 21.3 = CPI 0.98
All work: EV 22.4 ÷ AC 24.6 = CPI 0.91

The report was accurate about 87% of the job.

**Post 4/5 — four transform rules** (253 characters)
Map every activity to exactly one cost account or fail the load.
Convert to a single calendar before any date is compared.
Key on activity ID plus snapshot date, never activity ID alone.
Print mapping coverage on the front page of the pack, next to CPI.

**Post 5/5 — the load rule** (250 characters)
A snapshot is written once and never edited. If last month's CPI moves when you re-run the report, you cannot defend a forecast, and neither can the accountant who booked a number off it.
https://pciai.org/ai-in-project-controls
#ProjectControls #PMO

---

*Figures are a worked example, not a benchmark.*

*Internal links: the final post carries the only link and points at [AI in project controls](https://pciai.org/ai-in-project-controls) with that anchor. Reply posts should use [project performance management](https://projectcontrolsinstitute.org/project-performance-management) and [month-end close for projects](https://projectcontrolsinstitute.org/month-end-close-for-projects) with those anchors.*
