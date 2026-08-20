---
platform:      LinkedIn post
type:          linkedin-post
title:         Why your XER imports slowly, and what POBS has to do with it
meta:          An XER is a text dump of every table P6 exports, whether you need it or not. One awk line counts the rows per table, and POBS is usually the answer.
primary_kw:    XER import performance
secondary_kw:  POBS table, P6 data pipeline, schedule data ingestion, staging database
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    324
hashtags:      #ProjectControls #Primavera #Scheduling #AIGovernance
ab_id:         AB-00189
---

# Why your XER imports slowly, and what POBS has to do with it

**Post body (1,782 characters):**

An XER is not a file format so much as a text dump of every table P6 felt like exporting. The parser reads all of them, in order, whether your import needs them or not.

So before you blame the server, count the rows. An XER marks each table with %T, its fields with %F and each record with %R, which means one line tells you where the weight is:

awk '/^%T/{t=$2} /^%R/{c[t]++} END{for(k in c) print c[k], k}' project.xer | sort -rn | head

Run that on a large export and the top line is often not ACTVCODE or TASKPRED. It is POBS, an internal P6 table that rides along in the export and that almost nothing downstream reads. Open the file in a text editor and look for yourself before you take my word for it.

Illustrative shape: 12,400 activities, 31,800 relationships, 1,240,000 POBS rows. The file is 214 MB and 80% of it is a table nobody in the room can name. Strip that block from a copy and the same import that took 52 minutes takes 6.

Three habits follow from that.

Measure before you optimise. Row counts per table take one second and point straight at the cause of the performance problem, which is usually one table rather than a big schedule.

Import into a staging database first, then promote. An import into a live database with baselines attached and global changes running is doing several jobs at once and only one of them is yours.

If you are shipping XERs nightly into a data platform for reporting or model work, you are paying to store and re-parse those rows every single night. Filter at ingestion, keep the tables your pipeline actually joins on, and record which tables you dropped so the next person knows.

Data engineering is the unglamorous half of AI in project controls, and it is the half that decides whether anything downstream runs at all.

#ProjectControls #Primavera #Scheduling #AIGovernance

**First comment:** Where AI genuinely helps in project controls, and the data plumbing each use case needs first: https://pciai.org/ai-in-project-controls

---

*Every figure above is illustrative arithmetic, not project data. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and profile featured section): [AI in project controls](https://pciai.org/ai-in-project-controls) with the anchor "the data conditions AI in project controls needs", and [build a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with the anchor "the P6 build that keeps the export clean".*
