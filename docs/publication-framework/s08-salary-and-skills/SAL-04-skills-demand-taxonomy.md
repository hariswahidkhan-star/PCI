---
id: SAL-04
series: S08
series_name: Salary and Skills Report
title: The skills demand taxonomy
subtitle: A controlled vocabulary for skills, and an honest definition of what "demand" can mean
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [manager, employer, academic, practitioner]
level: professional
reading_time_min: 14
summary: >
  The structured vocabulary the Institute's salary and skills report uses to classify skills across five
  facets — technical method, tool category, domain, data and AI, and behavioural — with a permanent code
  for each, the rules for coding free text into it, and a precise definition of what the report may call
  "demand". The short version of that definition: what respondents were asked for is a reported
  requirement, not market demand, and the report says so in those words.
linkedin:
  format: article
  hook: >
    "The most in-demand skill in project controls" is almost always a count of what a self-selected group
    said they were asked for. That is a real measurement — but it is not demand, and calling it demand is
    where skills research stops being research.
  tags: [ProjectControls, SkillsTaxonomy, WorkforceData, ResponsibleAI]
  asset: carousel-8
gated: false
related: [SAL-02, SAL-03, SAL-05, CMP-08, AIG-12]
bok_domains: [13, 4, 6]
sources: []
placeholders: 3
---

# The skills demand taxonomy

> The controlled vocabulary behind the skills questions, the coding rules, and the limits on what the resulting numbers may be called.

**In one paragraph.** This document defines the structured vocabulary the Institute's salary and skills
report uses to classify skills across five facets — technical method, tool category, domain, data and AI,
and behavioural — with a permanent code for each, the rules for coding free text into it, and a precise
definition of what the report may call "demand". The short version of that definition: what respondents
were asked for is a reported requirement, not market demand, and the report says so in those words.

**Who this is for.** Analysts coding survey responses; heads of function and capability leads who need a
skills vocabulary that does not change every time a tool is renamed; and anyone assessing whether a
published skills statistic means what its headline says.

---

## 1. What goes wrong when skills are counted without a vocabulary

Ask a hundred practitioners what skills their job requires and you get a hundred phrasings of perhaps
thirty things. "Forecasting", "EAC", "estimate at completion", "predicting the outturn" and "cost
projection" are one skill written five ways. "Data skills" is between four and nine different skills
depending on who wrote it. "Excel" is a tool, a proxy for modelling capability, and sometimes a complaint.

Left uncoded, that free text produces a ranked list whose ordering is an artefact of vocabulary rather
than of anything in the world — the skill with the most common single phrasing wins. Coded carelessly, it
produces something worse: a list that looks authoritative and is unreproducible, because the person who
made the coding decisions did not write them down.

A controlled vocabulary fixes both, and costs something in return. It cannot represent a skill nobody
anticipated, and it imposes boundaries that will occasionally be wrong. The compensating controls are in
§5: unmatched text is coded as unmatched and reported rather than forced, and the codebook has a defined
review point where new codes are added — deliberately not continuously, because a vocabulary that changes
mid-cycle cannot be compared with itself.

## 2. Five facets

Skills are classified on five facets. A facet is not a level of importance; it is a *kind* of skill, and
each kind behaves differently over time and needs different treatment in analysis.

| Facet | Code | What it holds | Why it is separate |
|---|---|---|---|
| Technical method | `T` | The discipline itself — how the work is done, independent of any tool | The slowest-moving facet, and the one the Body of Knowledge maps to directly |
| Tool category | `L` | Categories of software and platform, never named products | Products churn; categories persist. Naming products would date within a year and function as endorsement |
| Domain | `D` | Sector, contracting model and standards context | Portable-looking skills are often domain-bound, and this facet is what reveals it |
| Data and AI | `A` | Data handling, analysis, automation and the responsible use of AI tooling | The fastest-moving facet; kept separate so its volatility does not contaminate the others |
| Behavioural | `B` | Professional behaviour and judgement | Measurable only through anchored self-report, and reported with heavier caveats than the rest |

**Code structure:** `FACET-GROUP-SKILL`, for example `T-EVM-EAC`. Codes are permanent. A retired code is
never reused for a different skill, for the same reason a document ID is never reused: it is a citation
key, and anyone comparing two cycles must be able to trust that the same code means the same thing.

## 3. The vocabulary

Each entry gives the code, the skill, what it means in one line, and what evidence of it looks like — the
evidence column is what makes the code applicable by two different analysts to the same free text.

### 3.1 Facet T — technical method

| Code | Skill | What it means | What evidence looks like | BoK |
|---|---|---|---|---|
| `T-PLN-DEV` | Schedule development | Building a schedule that represents how the work will actually be done | A network with defensible logic and a written schedule basis | 10 |
| `T-PLN-INT` | Network and logic integrity | Keeping the schedule's logic sound as it changes | Diagnosed and corrected logic defects; documented open ends and constraints | 10 |
| `T-PLN-PRG` | Progress measurement | Establishing how progress is claimed and verified | Rules of credit applied consistently across a scope | 6 |
| `T-PLN-CPA` | Critical path and float analysis | Reading what the network says about completion and slack | Float analysis that changed a decision | 10 |
| `T-PLN-TIA` | Time impact analysis | Isolating the schedule effect of a discrete event | A modelled impact that survived challenge | 10, 7 |
| `T-CST-CBS` | Cost structure design | Designing a cost breakdown that aligns to scope and to the accounts | A cost breakdown structure mapped to both the work breakdown structure and the code of accounts | 5 |
| `T-CST-BUD` | Budgeting and control accounts | Distributing a budget into controllable units | Control accounts with named owners | 3, 5 |
| `T-CST-ACT` | Commitment and actual capture | Getting real cost into the report completely and on time | Committed, incurred and paid tracked distinctly | 5, 11 |
| `T-CST-ACC` | Accruals and cut-off | Reporting the period's cost in the period | An accrual schedule that reconciles to finance | 2, 11 |
| `T-CST-VAR` | Variance analysis | Explaining a difference in terms someone can act on | Variance narrative naming cause, not restating the number | 4, 5 |
| `T-EST-QTO` | Quantity take-off | Deriving quantities from a design of stated maturity | Take-off with a stated source and maturity | 3 |
| `T-EST-MTH` | Estimate method and class | Choosing a method appropriate to definition and stating its class | An estimate labelled by class with an accuracy range stated as a range concept | 3 |
| `T-EST-BEN` | Benchmarking and norms | Using comparable data critically, including rejecting it | A norm applied with a documented adjustment | 3 |
| `T-EST-BOE` | Basis of estimate | Writing down what the estimate assumes, includes and excludes | A basis document a stranger could audit | 3 |
| `T-EVM-TEC` | Earned value technique selection | Choosing a measurement method suited to the work type | Technique chosen per work package, not per project | 6 |
| `T-EVM-IND` | Performance indices | Computing and, more importantly, interpreting indices | An index read alongside its data quality, not in isolation | 6 |
| `T-FCT-EAC` | Estimate at completion | Selecting, computing and defending a forecast method | Multiple methods compared and one chosen with a reason | 6, 3 |
| `T-FCT-CSH` | Cash flow forecasting | Forecasting the timing of money, not only its total | A profile reconciled to schedule and to commercial terms | 3, 7 |
| `T-RSK-IDN` | Risk identification | Surfacing what could happen, in usable form | Risks written as cause-event-effect, not as concerns | 12 |
| `T-RSK-QNT` | Quantitative risk analysis | Modelling uncertainty in cost or schedule | A simulation with defensible ranges and correlation treatment | 12 |
| `T-RSK-CTG` | Contingency derivation and drawdown | Deriving contingency from analysis and controlling its release | Contingency traced to risks and drawn down against them | 12, 3 |
| `T-COM-CHG` | Change identification and valuation | Recognising a change and pricing it | A change register where the entry date matches the event | 7 |
| `T-COM-ENT` | Entitlement and notice | Managing the records that make a position defensible | Notices issued in time, with contemporaneous records | 7 |
| `T-COM-CLM` | Claims and extension of time | Constructing a substantiated narrative from records | A claim narrative supported by its own evidence trail | 7 |
| `T-GOV-BLC` | Baseline control | Keeping the baseline meaningful under change | A change-controlled baseline with a traceable history | 4, 8 |
| `T-GOV-PCE` | Controls execution planning | Writing down how controls will be run before running it | A project controls execution plan in use, not filed | 8 |
| `T-GOV-ASR` | Assurance and health check | Testing whether reported performance is credible | A health check that produced corrective actions | 4 |
| `T-RPT-MGT` | Management reporting | Producing reporting that changes a decision | A report whose recommendations were acted on | 4 |

### 3.2 Facet L — tool category

| Code | Category | Scope of the code |
|---|---|---|
| `L-SCH` | Planning and scheduling software | Any critical-path scheduling platform |
| `L-CST` | Cost management or control system | Dedicated project cost control platforms |
| `L-ERP` | Enterprise resource planning system | Finance and procurement systems as they touch project cost |
| `L-RSK` | Risk analysis and simulation tools | Quantitative cost and schedule risk platforms |
| `L-BI` | Business intelligence and dashboarding | Reporting and visualisation platforms |
| `L-SPR` | Spreadsheet as a primary control tool | Including structured modelling, not incidental use |
| `L-DOC` | Document control and correspondence systems | Including the controls-relevant record trail |
| `L-COD` | Programming or scripting | Any language used to manipulate controls data |
| `L-AIT` | AI assistant or large language model tooling | General-purpose and embedded assistants |
| `L-INT` | System integration and interfacing | Moving data between the above without losing its meaning |

**Named products are never published.** They are collected as free text at Q42 (`SAL-02`) and used only to
assign the category. Publishing a ranked list of products would function as an endorsement the Institute
has no basis to give, would age within a year, and would distract from the finding that matters: which
*categories* of capability a role is expected to hold.
`[CONFIRM: whether an annex lists the named products encountered, in alphabetical order with no counts and
an explicit non-endorsement statement, or whether product names are discarded entirely after coding]`

### 3.3 Facet D — domain

| Code | Domain skill | What it means |
|---|---|---|
| `D-SEC-HVY` | Heavy industrial and energy delivery | Controls practice under the constraints of large capital plant and site delivery |
| `D-SEC-INF` | Infrastructure and transport | Long linear or networked assets, often publicly scrutinised |
| `D-SEC-BLD` | Buildings and property | Trade-package structures and shorter cycles |
| `D-SEC-REG` | Highly regulated environments | Nuclear, pharmaceutical, defence and similar: qualification and evidence regimes that change how controls is done |
| `D-SEC-CHG` | Technology and business-change portfolios | Iterative delivery where scope is discovered rather than specified |
| `D-CON-LSM` | Lump sum and fixed price | Controls under fixed-price commercial pressure |
| `D-CON-REI` | Reimbursable and target cost | Open-book regimes, pain-gain mechanisms, client audit |
| `D-CON-ALL` | Alliance and integrated delivery | Shared-risk arrangements and their reporting demands |
| `D-CON-PPP` | Concessions and public-private arrangements | Long-horizon financing structures as they constrain controls |
| `D-STD-EVM` | Earned value guidance and its conventions | Working to a recognised earned value management framework |
| `D-STD-RSK` | Risk management standards | Applying recognised risk principles, such as those in ISO 31000 |
| `D-STD-TCM` | Total cost management practice | Working within the AACE International total cost management body of practice |
| `D-STD-PMB` | Project management frameworks | PMBOK and comparable frameworks as they meet controls |
| `D-STD-AGL` | Adaptive delivery frameworks | Scrum and comparable approaches, and their controls implications |

### 3.4 Facet A — data and AI

| Code | Skill | What it means | BoK |
|---|---|---|---|
| `A-DQL` | Data quality and structure | Knowing whether the data can bear the analysis about to be done to it | 13, 4 |
| `A-MDL` | Data modelling for controls | Structuring controls data so cost, schedule and change can be joined | 13 |
| `A-ANL` | Analysis and applied statistics | Trend, distribution and comparison, used correctly | 13, 4 |
| `A-VIS` | Visualisation and metric definition | Defining a metric precisely and displaying it without distorting it | 4 |
| `A-AUT` | Automation of recurring work | Removing manual steps from a reporting cycle reliably | 13 |
| `A-PRD` | Predictive and machine-assisted forecasting | Using algorithmic methods to support a forecast a human still owns | 13, 6 |
| `A-PRM` | Task framing for AI tools | Specifying a task to an AI tool precisely enough to get usable output | 13 |
| `A-VAL` | Validation of AI output | Checking what a tool produced before anyone relies on it | 13 |
| `A-EXP` | Explainability and bias awareness | Understanding what a model did and where it can be systematically wrong | 13 |
| `A-GOV` | AI governance and permissible use | Knowing what is allowed, recording what was used, and keeping the audit trail | 13 |

`A-VAL` and `A-GOV` carry the Institute's position directly: AI proposes; the professional disposes. They
are separate codes rather than an aspect of the others because they are separately absent — a respondent
can be fluent with the tools and have no validation practice at all, and that combination is precisely
what a skills report should be able to see.

### 3.5 Facet B — behavioural

| Code | Skill | What it means |
|---|---|---|
| `B-COM` | Explaining a number to a non-specialist | Making a forecast or variance land with someone who does not do this job |
| `B-POS` | Holding a position under pressure | Maintaining a defensible number when it is unwelcome |
| `B-STK` | Stakeholder management | Working across client, contractor and function boundaries |
| `B-COL` | Cross-discipline collaboration | Working effectively with delivery, commercial and finance |
| `B-JUD` | Judgement under incomplete data | Deciding when the data will not settle it, and saying what is assumed |
| `B-LRN` | Method improvement | Improving how the work is done, not only doing it |
| `B-DEV` | Developing others | Building capability in a team |
| `B-ETH` | Professional conduct | Recognising and escalating a reporting integrity problem |

Behavioural codes are reported with the heaviest caveats in the report, because they rest entirely on
anchored self-report. `SAL-02` Q40 uses behavioural anchors precisely to make this facet less
self-flattering, and the limitation is stated wherever a behavioural finding appears.

## 4. Skill entries — what a code carries

Every code in the master codebook carries seven fields, and a code without all seven is not fielded:
the code itself; the facet; the display label used in the instrument; the one-line definition; the
evidence descriptor; the mapped Body of Knowledge domain or domains; and the cycle in which the code was
introduced. Retired codes additionally carry the cycle of retirement and the reason.

## 5. Coding free text into the vocabulary

Free text arrives from three places: the "other" fields at Q07 and Q25, the product names at Q42, and the
open question at Q52.

**The procedure.**

1. **Screen for identifying detail first.** Employer names, project names, colleagues' names and anything
   else identifying is removed — removed, not paraphrased — before any coding begins.
2. **Code to the vocabulary, or to `UNC`.** A segment of text is assigned one or more codes. Where no code
   fits, it is coded `UNC` (unmatched) with the text retained. `UNC` is reported as a count. It is never
   distributed across neighbouring codes to make a table look complete.
3. **Code segments, not responses.** One open answer may carry three skills; forcing one code per
   respondent silently discards two-thirds of what was said.
4. **Double-code independently.** A defined share of the free text is coded by two analysts working
   separately, and their agreement is measured with a named statistic — Cohen's kappa for two coders on
   nominal codes, or Krippendorff's alpha where more than two coders or partial agreement are involved.
   The statistic and the minimum acceptable value are fixed before coding begins
   `[CONFIRM: which agreement statistic is used and the minimum acceptable value below which coding is
   redone rather than reconciled]`.
5. **Resolve disagreements by rule, not by seniority.** A third analyst adjudicates; the adjudication is
   recorded; if the same boundary is adjudicated repeatedly, that is a codebook defect and goes to the
   review point rather than being absorbed by the analysts.
6. **Do not add codes mid-cycle.** New codes are added only at the codebook review, between cycles. A
   vocabulary that grows during coding cannot be compared with itself, and the first responses coded were
   coded under a different scheme from the last.
7. **Publish the codebook version** used for each cycle, with the change log
   `[CONFIRM: codebook owner, version numbering scheme and the review point at which new codes may be
   introduced]`.

## 6. What "demand" may mean here

This is the section that decides whether the skills half of the report is research or decoration. The
instrument measures three distinct constructs, and the report never merges them or lets one borrow the
other's name.

**Construct 1 — reported requirement.** From Q41: a respondent states that a skill was explicitly required
of them, in a documented form (job specification, objective, appraisal or interview), within the twelve
months to the reference date. What it measures: what these respondents were asked for. What it does not
measure: what employers who did not respond want, what unfilled vacancies specify, or what anyone will
want next year.

**Construct 2 — held capability.** From Q40: a respondent's anchored self-assessment of what they can do.
What it measures: claimed capability among respondents. What it does not measure: assessed competence —
that requires assessment, which is what the Institute's certification does and this survey does not.

**Construct 3 — capability gap.** Computed *within* a respondent: a skill explicitly required at Q41 and
held at the two lowest anchors at Q40. Computed per person and then counted, never inferred by comparing
two aggregate distributions — comparing a required-skills distribution with a held-skills distribution
across different people produces a "gap" that may not exist for any individual.

Economists mean something specific by demand: a schedule of quantity against price, evidenced by
vacancies, time-to-fill and wage movement. The Institute's survey collects none of that. It therefore
never uses the word without qualification.

| Never written | Written instead |
|---|---|
| "The most in-demand skill in project controls" | "The skill most often reported as explicitly required by respondents in this cell" |
| "Demand for X grew" | "A larger share of respondents reported X as explicitly required than in the previous cycle" — and only where §7 permits a comparison at all |
| "There is a shortage of X" | "A capability gap for X was reported by respondents in this cell" |
| "Employers want X" | "Respondents reported that X was required of them" |
| "X is the future of the profession" | Nothing. This is not a measurement |

Every one of the permitted forms is longer and duller than the phrase it replaces. That is the cost of
saying only what was measured, and it is the whole reason to read the report rather than a headline about
it.

## 7. Reporting rules for skills findings

1. **Cell thresholds apply identically.** The minimum response counts and suppression rules in `SAL-01` §6
   govern skills findings exactly as they govern pay findings. A skills table is not exempt because it
   contains no money.
2. **Item-level denominators.** Skills matrices lose respondents partway through. Every skills figure
   carries the count of respondents who answered *that item*, not the survey total.
3. **Ranks are bands.** Where the difference between adjacent skills is within the reporting precision,
   they are shown as a tied band. A ranked list with no tie rule invites readers to treat noise as order.
4. **No top-ten lists without the whole list.** Publishing only the top of a ranking hides how flat the
   ranking is. The full ordering is published in an annex whenever any part of it is quoted.
5. **No cross-cycle skills comparison** unless the item wording, the anchors and the code were all
   unchanged, and both cycles meet the cell threshold. Adding a skill to the vocabulary resets that
   skill's series to its first cycle.
6. **Facet A findings carry a currency warning.** The data and AI facet moves fastest; a finding from it is
   labelled with its reference period wherever it appears, because a twelve-month-old statement about AI
   tool use is a historical statement.
7. **Behavioural findings carry the self-report caveat** in the same paragraph, not in a footnote.

## 8. How this goes wrong

**The vocabulary becomes a wish list.** Someone adds the skills the Institute would like the profession to
value. The vocabulary must describe what exists, including the parts nobody is proud of — the spreadsheet
category exists for exactly that reason.

**Tool codes swallow method codes.** A respondent who names a scheduling product is coded as having
schedule development capability. They are different things, and the facet separation exists to keep them
apart.

**Coding drift.** Over a long coding run, a boundary case is decided one way early and the other way late.
Double-coding a sample at the start, middle and end of the run catches it; a single agreement check at the
start does not.

**`UNC` gets tidied away.** Unmatched text is the taxonomy's error signal. Distributing it into the nearest
codes destroys the only evidence that the vocabulary is incomplete.

**The gap that exists only in aggregate.** Two distributions are compared, a difference appears, and it is
reported as a skills gap. Construct 3 is computed per respondent for this reason.

**"Demand" leaks back in.** The report is careful and the promotional post is not. The language table in §6
applies to the headline, the social copy and the conference slide, not only to the body text.

## 9. Checklist — before a skills finding is published

- [ ] Every code used is in the published codebook version for this cycle
- [ ] Identifying detail removed from free text before coding, not after
- [ ] Double-coded sample drawn from across the whole coding run, and agreement reported
- [ ] `UNC` count reported, and no `UNC` text redistributed
- [ ] No code added mid-cycle
- [ ] Item-level response count shown on every skills figure
- [ ] Ranked lists carry a tie rule; nothing quoted from a ranking without the full ordering available
- [ ] Construct named explicitly — reported requirement, held capability or capability gap
- [ ] The word "demand" checked against the language table in §6, in the report and in every derived post
- [ ] Facet A findings labelled with their reference period; facet B findings carry the self-report caveat
- [ ] No product name published in any ranked or counted form

---

## Related

- `SAL-02 — The survey instrument` — the skills matrices, tool question and open question this vocabulary codes
- `SAL-03 — Role taxonomy and levelling` — the roles these skills are reported against
- `SAL-05 — Report template and data tables` — the skills tables and their standing caveats
- `CMP-08 — Data, digital and AI competencies in depth` — what competence in the data and AI facet actually requires
- `AIG-12 — The AI-literate controls professional` — the Institute's position on the capability facet A measures

## Sources and standards

No external source is cited. Standards named in facet D — ISO 31000 risk management principles, the AACE
International total cost management practice, PMBOK, the Scrum Guide, and recognised earned value
management guidance — are named as objects of the domain codes and are not reproduced, summarised or
represented here; where the report describes them it does so in our own words. Body of Knowledge domain
numbers refer to the thirteen domains in `docs/bok/`. The inter-coder agreement statistics named in §5,
Cohen's kappa and Krippendorff's alpha, are standard content-analysis measures described here in our own
words.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
