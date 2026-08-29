---
platform:      Instagram / Facebook carousel
type:          carousel
title:         Power BI vs Excel dashboards: the year-one arithmetic
meta:          The manual cycle costs 162 hours a year. The modelled one costs 164 in year one and 74 after that. Nine slides on choosing between them honestly.
primary_kw:    Power BI vs Excel dashboards
secondary_kw:  project reporting automation, single source of truth, row-level security, data model
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    908
hashtags:      #ProjectControls #PMO #AIGovernance #CostEngineering #ProjectManagement #Scheduling
ab_id:         AB-01378
---

# Power BI vs Excel dashboards: the year-one arithmetic

*Instagram and Facebook carousel — 9 slides, 1080 × 1350. Instagram captions carry no clickable link, so the link goes in the bio; on Facebook it goes in the post.*

**Caption (the first 125 characters have to earn the swipe):**

The manual reporting cycle costs 162 hours a year. The modelled one costs 164 in year one. Then 74.

Every dashboard business case skips the build hours and the maintenance hours. Nine slides: the full arithmetic on slide 3, where Excel genuinely wins and you should not move, the silent failure that cost one project 2.9 per cent of its actual cost, and the rule that ends the argument.

Save it before the next tooling decision.

---

**Slide 1 — The question is not which tool is better**

Both tools will produce a page with numbers on it. The difference is what happens when someone asks where a number came from, and what happens when the person who built it leaves.

Answer those two questions first and the tool choice usually makes itself.

**Slide 2 — What each is actually for**

Excel is a model you can see. Every step is on the sheet, anyone can open it, and changing something takes seconds.

Power BI is a model you can govern. The transformation steps are recorded, the relationships are declared, the refresh is scheduled, and permissions are enforced in the model rather than by who has the file.

**Slide 3 — The arithmetic**

Fourteen contractor returns, consolidated monthly.

| | Manual in Excel | Modelled and refreshed |
|---|---:|---:|
| Consolidation | 14 × 45 min = 10.5 h | Refresh 12 min = 0.2 h |
| Rework and chasing errors | 3.0 h | Exception handling 2.0 h |
| **Per month** | **13.5 h** | **2.2 h** |
| **Per year** | **162 h** | **26 h** |

Now the part the business case leaves out. Build: **90 h**. Maintenance: 4 h a month = **48 h a year**.

Year one: 90 + 48 + 26 = **164 hours** against 162. **No saving at all.**
Year two onwards: 48 + 26 = **74 hours**, saving **88 hours a year**.

Build it because year two matters, or do not build it. Do not build it on a year-one promise.

**Slide 4 — Where they genuinely differ**

| | Excel | Power BI |
|---|---|---|
| Who can maintain it | Almost anyone | Someone who knows the model |
| Version control | Filenames and hope | One published model |
| Refresh | Manual, per file | Scheduled, from source |
| Audit trail | The formula bar | Recorded transformation steps |
| Permissions | Whoever has the file | Row-level security in the model |
| Failure mode | Silent wrong number | Refresh fails loudly |
| Scale limit | Rows and open time | The model design |

The last row is the honest one. Excel fails silently and Power BI fails visibly, and visible failure is worth real money on a project report.

**Slide 5 — Where Excel wins and you should not move**

One-off analysis. Anything you will run twice and throw away. Estimate build-ups, where the reviewer needs to see every rate and change one.

Also anything a client insists on receiving as a file. Rebuilding a required Excel deliverable inside a reporting tool is work with no reader.

**Slide 6 — How Excel fails at project scale**

A consolidation matched contractor returns on WBS code. One contractor renamed **ME-04** to **ME-4**. One hundred and eighteen rows stopped matching, carrying **£1.4m** of actual cost.

Cumulative actual cost should have been **£48.2m**. The report said **£46.8m** — **2.9% low**, which is well inside the range where a number still looks plausible. It ran for two months before a ledger reconciliation caught it.

A modelled refresh would have thrown an unmatched-key error on the first run. That is the difference, in one line.

**Slide 7 — What moving actually costs**

Beyond the hours: someone has to own the data, someone has to own the metric definitions, and someone has to be reachable when a refresh fails at 07:00 on reporting day.

If those three roles are not named, the model becomes a different single point of failure with better graphics.

**Slide 8 — The rule that ends the argument**

Model in one place, present in the other. Do the joining, the cleaning and the metric definitions in a governed model. Let people export to Excel for their own analysis, from a number that has already been agreed.

The failure worth avoiding is not "someone used Excel". It is two people computing per cent complete two different ways and the meeting becoming an argument about the tool.

**Slide 9 — The governance line**

Whichever tool you choose: one definition per metric, written down. The data date on every page. Any figure traceable to a ledger entry and a progress record in under a minute. Access reviewed whenever the sharing list changes.

Governed AI and governed data are 20% of every PCI Body of Knowledge, alongside 40% finance and reporting and 40% project management. The PCI AI Project Controls Leader (PCL-AI) examines that across 13 domains and 61 knowledge areas, because a reporting model that nobody can audit is a liability whatever it was built in.

---

#ProjectControls #PMO #AIGovernance #CostEngineering #ProjectManagement #Scheduling

**Link (bio on Instagram, in-post on Facebook):** what a project controls model in Power BI should look like — https://pciai.org/ai-in-project-controls

---

*Every figure above is illustrative arithmetic, not project data or a benchmark. Microsoft Excel and Microsoft Power BI are named as tools in common use; PCI claims no affiliation with or endorsement by Microsoft. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (bio link and first comment): [AI in project controls](https://pciai.org/ai-in-project-controls) with that anchor, [generative AI for project reporting](https://pciai.org/generative-ai-project-reporting) with the anchor "speed without losing the audit trail", and [the earned value management pillar](https://projectcontrolsinstitute.org/earned-value-management) with the anchor "the formulas the dashboard is reporting".*
