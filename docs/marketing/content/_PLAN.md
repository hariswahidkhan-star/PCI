# Content run allocation — 300 pieces

Working table for the 300-piece run. It is the binding allocation: one row per piece, numbered
001–300. Read `_BRIEF.md` first — this file says *what* to write and *where it goes*; the brief
says how to write it and what may not be claimed.

## How to read the table

| Column | Meaning |
|---|---|
| `num` | Piece number. Filenames are `NNN-platform-slug.md`, zero-padded. |
| `platform` | Where it publishes. Own-site rows name the domain that hosts the original. |
| `type` | Format. Article-Bank formats (`comparison`, `faq`, `template`, `process-guide`, `practice`) are used alongside the brief's list; `forum-post` is the one addition, for Reddit. |
| `title` | Working title. Own-site and LinkedIn/Substack titles must land at 50–60 characters in the final draft; trim there, not here. |
| `primary_kw` | The one primary keyword. A trailing `*` marks a long-tail extension of a Keyword Plan cluster rather than a verbatim plan keyword — see the keyword rule below. |
| `pillar` | One of the seven. |
| `credential` | PCL-AI, PFL-AI, PML-AI or suite. |
| `target_domain` | Which of the five domains the piece links to. Off-estate links do not ship. |
| `canonical` | `original`, or `canonical -> #NNN` naming the own-site piece the republish derives from. |
| `ab_id` | Article Bank ID where the brief supplied the title. `—` means no brief matched and the title is new. |

## Rules this table already enforces

**Keyword cannibalisation.** The Keyword Plan holds 76 targetable keywords. All 76 are assigned,
each to exactly one own-site original, so no two originals compete for the same term. The
remaining 24 own-site originals carry a documented long-tail extension of a plan cluster, marked
`*`, each also unique. Off-estate originals (LinkedIn, Substack, Quora) mostly carry long-tails for
the same reason; where a Quora answer reuses a plan keyword it is deliberate SERP interception —
the answer and our page can both rank, and Quora links are nofollow either way. On social rows the
`primary_kw` column records the theme the piece amplifies, not a ranking target, so a repeat there
costs nothing.

**Publish order.** Every `canonical -> #NNN` row waits for its original to be live and indexed
(roughly 2–10 days) before it goes out. Republishing first hands the platform the credit.
Medium, DEV and Hashnode all support a canonical field and it is set on every one of the 45
republishing rows. LinkedIn Articles, Substack and Quora support none, so every row on those three
platforms is an original — never a rewrite of a page we want to rank.

**Nofollow, honestly.** Medium and Quora links pass no equity. They are in the plan for qualified
traffic and for being read by models, not as backlinks. Nothing here should be counted as a link
win.

**The finance/delivery overlap.** 116 of 300 pieces (1 in 2.6) make the overlap thesis explicit —
above the one-in-three floor. Their numbers are listed at the end of this file.

## Allocation

| Platform | Count |
|---|---:|
| Own site — projectcontrolsinstitute.org | 55 |
| Own site — pciai.org | 15 |
| Own site — pciglobal.ai | 10 |
| Own site — pciworld.org | 10 |
| Own site — credentialfinder.org | 10 |
| Medium | 25 |
| LinkedIn Article | 25 |
| Substack | 15 |
| DEV Community | 10 |
| Hashnode | 10 |
| Quora | 15 |
| LinkedIn post | 40 |
| LinkedIn carousel | 20 |
| X / Threads | 15 |
| Instagram / Facebook | 15 |
| Reddit / forum | 10 |
| **Total** | **300** |

Articles 001–200: 100 own-site originals, 25 Medium, 25 LinkedIn Articles, 15 Substack, 10 DEV,
10 Hashnode, 15 Quora. Social 201–300: 40 LinkedIn posts, 20 LinkedIn carousels, 15 X/Threads,
15 Instagram/Facebook, 10 Reddit/forum.

## Pillar spread

Proportional to the 260 briefs, within a point on every pillar.

| Pillar | Briefs | Share | Pieces | Share |
|---|---:|---:|---:|---:|
| Certification and careers | 48 | 18.5% | 54 | 18.0% |
| Project controls fundamentals | 42 | 16.2% | 49 | 16.3% |
| AI in project controls | 42 | 16.2% | 48 | 16.0% |
| Cost control and estimating | 37 | 14.2% | 43 | 14.3% |
| Planning and scheduling | 35 | 13.5% | 41 | 13.7% |
| Earned value management | 31 | 11.9% | 36 | 12.0% |
| Risk management | 25 | 9.6% | 29 | 9.7% |

## The table

| num | platform | type | title | primary_kw | pillar | credential | target_domain | canonical | ab_id |
|---|---|---|---|---|---|---|---|---|---|
| 001 | Own site — projectcontrolsinstitute.org | pillar | Project controls certification: everything the search actually asks | project controls certification | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00004 |
| 002 | Own site — projectcontrolsinstitute.org | pillar | What is project controls: everything the search actually asks | what is project controls | Project controls fundamentals | suite | projectcontrolsinstitute.org | original | AB-00027 |
| 003 | Own site — projectcontrolsinstitute.org | pillar | Earned value management: the complete guide (EVM pillar) | what is earned value management | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00091 |
| 004 | Own site — projectcontrolsinstitute.org | pillar | Project controls training: everything the search actually asks | project controls training | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00005 |
| 005 | Own site — projectcontrolsinstitute.org | pillar | Project controls course: everything the search actually asks | project controls course | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00006 |
| 006 | Own site — projectcontrolsinstitute.org | pillar | Schedule risk analysis: everything the search actually asks | schedule risk analysis | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-00035 |
| 007 | Own site — projectcontrolsinstitute.org | pillar | IFRS for project controls: the standards every cost engineer must know | IFRS for project controls * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00092 |
| 008 | Own site — projectcontrolsinstitute.org | pillar | Project budgeting and forecasting: a practical end-to-end guide | project budgeting and forecasting * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00095 |
| 009 | Own site — projectcontrolsinstitute.org | pillar | What are capital projects? The complete beginner's guide | what are capital projects * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00207 |
| 010 | Own site — projectcontrolsinstitute.org | pillar | Why a certification that blends finance + project management + AI had to exist | finance and project management certification * | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00088 |
| 011 | Own site — projectcontrolsinstitute.org | pillar | Why PCI AI? Inside the Project Controls Institute and its AI-era credentials | project controls qualifications | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00084 |
| 012 | Own site — projectcontrolsinstitute.org | guide | Project controls institute: what it is, who it is for, and how to start | project controls institute | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00001 |
| 013 | Own site — projectcontrolsinstitute.org | guide | PCL-AI certification: what it is, who it is for, and how to start | PCL-AI certification | Certification and careers | PCL-AI | projectcontrolsinstitute.org | original | AB-00002 |
| 014 | Own site — projectcontrolsinstitute.org | guide | Certuvo: what it is, who it is for, and how to start | Certuvo | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00003 |
| 015 | Own site — projectcontrolsinstitute.org | guide | Project controls certification online: entry routes, fees and what you actually get | project controls certification online | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00007 |
| 016 | Own site — projectcontrolsinstitute.org | guide | Project controls course online: entry routes, fees and what you actually get | project controls course online | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00008 |
| 017 | Own site — projectcontrolsinstitute.org | faq | Project controls certification cost: the questions people really ask | project controls certification cost | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00010 |
| 018 | Own site — projectcontrolsinstitute.org | guide | Certified project controls professional: what the title actually proves | certified project controls professional | Certification and careers | suite | projectcontrolsinstitute.org | original | — |
| 019 | Own site — projectcontrolsinstitute.org | guide | Earned value management certification: what it covers, what it costs and who it suits | earned value management certification | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00029 |
| 020 | Own site — projectcontrolsinstitute.org | guide | Earned value management training: what it covers, what it costs and who it suits | earned value management training | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00030 |
| 021 | Own site — projectcontrolsinstitute.org | guide | Earned value management explained with a worked example | earned value worked example * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00080 |
| 022 | Own site — projectcontrolsinstitute.org | guide | Earned value formulas cheat sheet explained: a practitioner's guide | earned value formulas cheat sheet | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00032 |
| 023 | Own site — projectcontrolsinstitute.org | how-to | Using EVM to predict final project costs: the four EAC formulas | four EAC formulas * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00215 |
| 024 | Own site — projectcontrolsinstitute.org | guide | Project performance management: from metrics to decisions | project performance management * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00101 |
| 025 | Own site — projectcontrolsinstitute.org | guide | Earned value reporting thresholds: when variances must trigger action | earned value reporting thresholds * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00214 |
| 026 | Own site — projectcontrolsinstitute.org | template | Earned value practice questions: a free tool and how to use it | earned value practice questions | Certification and careers | PCL-AI | projectcontrolsinstitute.org | original | AB-00070 |
| 027 | Own site — projectcontrolsinstitute.org | guide | EAC accounting: how estimate at completion flows into the financial statements | EAC accounting * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00094 |
| 028 | Own site — projectcontrolsinstitute.org | process-guide | Month-end close for projects: the controls-to-finance handshake | month-end close for projects * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00141 |
| 029 | Own site — projectcontrolsinstitute.org | guide | IFRS 15 for construction: percentage of completion and performance obligations | IFRS 15 for construction * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00130 |
| 030 | Own site — projectcontrolsinstitute.org | guide | Project cash flow: modelling, S-curves and the forecast the CFO actually reads | project cash flow forecasting * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00096 |
| 031 | Own site — projectcontrolsinstitute.org | guide | Cost control methods that catch overruns early | cost control in construction | Cost control and estimating | PCL-AI | projectcontrolsinstitute.org | original | AB-00079 |
| 032 | Own site — projectcontrolsinstitute.org | comparison | Cost engineer certification: an honest comparison | cost engineer certification | Certification and careers | PFL-AI | projectcontrolsinstitute.org | original | AB-00018 |
| 033 | Own site — projectcontrolsinstitute.org | guide | Cost controller certification: what it covers and who it suits | cost controller certification | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | — |
| 034 | Own site — projectcontrolsinstitute.org | practice | Cost management practice questions: 25 exam-style problems solved | project controls exam questions | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-00171 |
| 035 | Own site — projectcontrolsinstitute.org | guide | Primavera P6 certification: what it covers, what it costs and who it suits | primavera p6 certification | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00033 |
| 036 | Own site — projectcontrolsinstitute.org | comparison | Is Primavera P6 certification worth it? Training options compared | primavera p6 online course | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-03488 |
| 037 | Own site — projectcontrolsinstitute.org | template | Primavera P6 practice test: a free tool and how to use it | primavera p6 practice test | Certification and careers | PCL-AI | projectcontrolsinstitute.org | original | AB-00069 |
| 038 | Own site — projectcontrolsinstitute.org | guide | How to build a realistic project schedule in Primavera P6 | realistic schedule in Primavera P6 * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00078 |
| 039 | Own site — projectcontrolsinstitute.org | glossary | What is critical path method (CPM)? Definition, examples and why it matters | critical path method definition * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00591 |
| 040 | Own site — projectcontrolsinstitute.org | glossary | What is total float? Definition, examples and why it matters | total float definition * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00601 |
| 041 | Own site — projectcontrolsinstitute.org | guide | Planning engineer certification: the routes compared | planning engineer certification | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | — |
| 042 | Own site — projectcontrolsinstitute.org | guide | Certified planning engineer: what the credential actually proves | certified planning engineer | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | — |
| 043 | Own site — projectcontrolsinstitute.org | guide | Project scheduler certification: scope, cost and who it suits | project scheduler certification | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | — |
| 044 | Own site — projectcontrolsinstitute.org | guide | Quantitative schedule risk analysis for beginners | quantitative schedule risk analysis QSRA | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-00081 |
| 045 | Own site — projectcontrolsinstitute.org | how-to | How to run a Monte Carlo cost simulation: a step-by-step guide | Monte Carlo cost simulation * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-01051 |
| 046 | Own site — projectcontrolsinstitute.org | how-to | How to build a risk register stakeholders actually use | risk register that gets used * | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-01037 |
| 047 | Own site — projectcontrolsinstitute.org | comparison | Project controls vs project management: an honest comparison | project controls vs project management | Project controls fundamentals | suite | projectcontrolsinstitute.org | original | AB-00028 |
| 048 | Own site — projectcontrolsinstitute.org | comparison | Choosing a delay analysis technique: impacted-as-planned, windows, TIA, as-built-but-for | delay analysis techniques * | Project controls fundamentals | PCL-AI | projectcontrolsinstitute.org | original | AB-00200 |
| 049 | Own site — projectcontrolsinstitute.org | process-guide | The capital project management process, step by step | capital project management process * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00208 |
| 050 | Own site — projectcontrolsinstitute.org | guide | PMO certification: what it covers and where it stops | PMO certification | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | — |
| 051 | Own site — projectcontrolsinstitute.org | how-to | How to become a project controls manager: qualifications, path and first role | how to become a project controls manager | Certification and careers | suite | projectcontrolsinstitute.org | original | AB-02592 |
| 052 | Own site — projectcontrolsinstitute.org | guide | Honorary fellowship in engineering: what it recognises | honorary fellowship engineering | Certification and careers | suite | projectcontrolsinstitute.org | original | — |
| 053 | Own site — projectcontrolsinstitute.org | how-to | How to become a fellow of a professional institution | how to become a fellow of a professional institution | Certification and careers | suite | projectcontrolsinstitute.org | original | — |
| 054 | Own site — projectcontrolsinstitute.org | guide | Professional fellowship application: what assessors look for | professional fellowship application | Certification and careers | suite | projectcontrolsinstitute.org | original | — |
| 055 | Own site — projectcontrolsinstitute.org | guide | Fellowship for project professionals: routes and evidence | fellowship for project professionals | Certification and careers | suite | projectcontrolsinstitute.org | original | — |
| 056 | Own site — pciai.org | pillar | AI in project controls: everything the search actually asks | AI in project controls | AI in project controls | PCL-AI | pciai.org | original | AB-00038 |
| 057 | Own site — pciai.org | guide | AI project controls certification: what it covers, what it costs and who it suits | AI project controls certification | AI in project controls | PCL-AI | pciai.org | original | AB-00039 |
| 058 | Own site — pciai.org | guide | AI for construction scheduling explained: a practitioner's guide | AI for construction scheduling | AI in project controls | PCL-AI | pciai.org | original | AB-00040 |
| 059 | Own site — pciai.org | guide | AI in construction project management explained: a practitioner's guide | AI in construction project management | AI in project controls | PML-AI | pciai.org | original | AB-00041 |
| 060 | Own site — pciai.org | faq | Will AI replace planning engineers: the questions people really ask | will AI replace planning engineers | AI in project controls | PCL-AI | pciai.org | original | AB-00042 |
| 061 | Own site — pciai.org | guide | Will AI replace project managers explained: a practitioner's guide | will AI replace project managers | AI in project controls | PML-AI | pciai.org | original | AB-00043 |
| 062 | Own site — pciai.org | comparison | AI project management certification: an honest comparison | AI project management certification | AI in project controls | PML-AI | pciai.org | original | AB-00044 |
| 063 | Own site — pciai.org | guide | Future of project controls explained: a practitioner's guide | future of project controls | AI in project controls | suite | pciai.org | original | AB-00045 |
| 064 | Own site — pciai.org | guide | AI for cost estimating in construction explained: a practitioner's guide | AI for cost estimating in construction | AI in project controls | PFL-AI | pciai.org | original | AB-00046 |
| 065 | Own site — pciai.org | comparison | Best AI construction scheduling software: an honest comparison | best AI construction scheduling software | AI in project controls | PCL-AI | pciai.org | original | AB-00047 |
| 066 | Own site — pciai.org | pillar | AI in project management: the 2026 state of play | AI in project management 2026 * | AI in project controls | PML-AI | pciai.org | original | AB-00099 |
| 067 | Own site — pciai.org | how-to | Prompt engineering for project professionals: a working introduction | prompt engineering for project professionals * | AI in project controls | suite | pciai.org | original | AB-00150 |
| 068 | Own site — pciai.org | how-to | Using large language models to review schedules: a practical protocol | LLM schedule review * | AI in project controls | PCL-AI | pciai.org | original | AB-00151 |
| 069 | Own site — pciai.org | template | Building an AI policy for a project controls team | AI policy for project controls * | AI in project controls | PCL-AI | pciai.org | original | AB-00154 |
| 070 | Own site — pciai.org | how-to | Generative AI for project reports: speed without losing the audit trail | generative AI project reporting * | AI in project controls | PFL-AI | pciai.org | original | AB-00157 |
| 071 | Own site — pciglobal.ai | guide | Project controls certification in the UK: what actually counts | project controls certification UK | Certification and careers | suite | pciglobal.ai | original | — |
| 072 | Own site — pciglobal.ai | guide | Project controls certification in the USA: what actually counts | project controls certification USA | Certification and careers | suite | pciglobal.ai | original | — |
| 073 | Own site — pciglobal.ai | comparison | Project controls course India: an honest comparison | project controls course India | Certification and careers | suite | pciglobal.ai | original | AB-00053 |
| 074 | Own site — pciglobal.ai | guide | Project controls courses in Dubai: how to choose one | project controls courses in Dubai | Certification and careers | suite | pciglobal.ai | original | — |
| 075 | Own site — pciglobal.ai | guide | Project controls training in the UAE: the 2026 options | project controls training UAE | Certification and careers | suite | pciglobal.ai | original | — |
| 076 | Own site — pciglobal.ai | guide | Planning engineer course in Saudi Arabia: the routes | planning engineer course in Saudi Arabia | Planning and scheduling | PCL-AI | pciglobal.ai | original | — |
| 077 | Own site — pciglobal.ai | guide | Project controls courses in Qatar: what to look for | project controls courses in Qatar | Certification and careers | suite | pciglobal.ai | original | — |
| 078 | Own site — pciglobal.ai | guide | Primavera P6 course in Dubai: how to choose one | primavera p6 course in Dubai | Planning and scheduling | PCL-AI | pciglobal.ai | original | — |
| 079 | Own site — pciglobal.ai | guide | Quantity surveyor certification: the global routes compared | quantity surveyor certification | Cost control and estimating | PFL-AI | pciglobal.ai | original | — |
| 080 | Own site — pciglobal.ai | guide | Quantity surveyor certification online: what you actually get | quantity surveyor certification online | Cost control and estimating | PFL-AI | pciglobal.ai | original | — |
| 081 | Own site — pciworld.org | template | Planning engineer salary: a free tool and how to use it | planning engineer salary | Certification and careers | suite | pciworld.org | original | AB-00016 |
| 082 | Own site — pciworld.org | template | Project controls salary: a free tool and how to use it | project controls salary | Certification and careers | suite | pciworld.org | original | AB-00026 |
| 083 | Own site — pciworld.org | data-study | Planning engineer salary in the UAE: what the market pays | planning engineer salary UAE | Certification and careers | suite | pciworld.org | original | — |
| 084 | Own site — pciworld.org | data-study | Planning engineer jobs in the UAE: where the demand sits | planning engineer jobs in UAE | Certification and careers | suite | pciworld.org | original | — |
| 085 | Own site — pciworld.org | data-study | The Gulf giga-projects hiring wave: what it means for planners and cost engineers | project controls jobs in Saudi Arabia | Certification and careers | suite | pciworld.org | original | AB-00119 |
| 086 | Own site — pciworld.org | how-to | How to become a planning engineer: qualifications, path and first role | how to become a planning engineer | Certification and careers | PCL-AI | pciworld.org | original | AB-02559 |
| 087 | Own site — pciworld.org | how-to | How to become a senior planning engineer: qualifications, path and first role | senior planning engineer career path | Certification and careers | PCL-AI | pciworld.org | original | AB-02570 |
| 088 | Own site — pciworld.org | guide | What does a project controls engineer do? A day in the role | what does a project controls engineer do | Project controls fundamentals | suite | pciworld.org | original | — |
| 089 | Own site — pciworld.org | qa-list | Planning engineer interview questions: 20 that actually come up | planning engineer interview questions | Planning and scheduling | PCL-AI | pciworld.org | original | — |
| 090 | Own site — pciworld.org | qa-list | Project controls interview questions and strong answers | project controls interview questions | Certification and careers | suite | pciworld.org | original | — |
| 091 | Own site — credentialfinder.org | comparison | Best project controls certification: an honest comparison | best project controls certification | Certification and careers | suite | credentialfinder.org | original | AB-00059 |
| 092 | Own site — credentialfinder.org | comparison | Best certification for planning engineers, compared | best certification for planning engineers | Planning and scheduling | PCL-AI | credentialfinder.org | original | — |
| 093 | Own site — credentialfinder.org | comparison | AACE CCP vs PMP: an honest comparison | AACE CCP vs PMP | Certification and careers | suite | credentialfinder.org | original | AB-00060 |
| 094 | Own site — credentialfinder.org | comparison | PMI-SP vs AACE PSP: an honest comparison | PMI-SP vs AACE PSP | Certification and careers | suite | credentialfinder.org | original | AB-00061 |
| 095 | Own site — credentialfinder.org | faq | PMI-SP worth it: the questions people really ask | PMI-SP worth it | Certification and careers | suite | credentialfinder.org | original | AB-00062 |
| 096 | Own site — credentialfinder.org | comparison | AACE certification cost: an honest comparison | AACE certification cost | Cost control and estimating | PFL-AI | credentialfinder.org | original | AB-00063 |
| 097 | Own site — credentialfinder.org | faq | CCP certification worth it? An honest assessment | CCP certification worth it | Cost control and estimating | PFL-AI | credentialfinder.org | original | — |
| 098 | Own site — credentialfinder.org | practice | CCP exam questions: what the paper actually tests | CCP exam questions | Cost control and estimating | PFL-AI | credentialfinder.org | original | — |
| 099 | Own site — credentialfinder.org | guide | PMI-SP exam prep: a realistic study plan | PMI-SP exam prep | Planning and scheduling | PCL-AI | credentialfinder.org | original | — |
| 100 | Own site — credentialfinder.org | guide | AACE PSP certification guide: scope, cost and fit | AACE PSP certification guide | Planning and scheduling | PCL-AI | credentialfinder.org | original | — |
| 101 | Medium | pillar | Earned value management: the complete guide (EVM pillar) | what is earned value management | Earned value management | PCL-AI | projectcontrolsinstitute.org | canonical -> #003 | AB-00091 |
| 102 | Medium | guide | Earned value management explained with a worked example | earned value worked example * | Earned value management | PCL-AI | projectcontrolsinstitute.org | canonical -> #021 | AB-00080 |
| 103 | Medium | how-to | Using EVM to predict final project costs: the four EAC formulas | four EAC formulas * | Earned value management | PFL-AI | projectcontrolsinstitute.org | canonical -> #023 | AB-00215 |
| 104 | Medium | guide | Earned value formulas cheat sheet explained: a practitioner's guide | earned value formulas cheat sheet | Earned value management | PCL-AI | projectcontrolsinstitute.org | canonical -> #022 | AB-00032 |
| 105 | Medium | guide | Earned value reporting thresholds: when variances must trigger action | earned value reporting thresholds * | Earned value management | PFL-AI | projectcontrolsinstitute.org | canonical -> #025 | AB-00214 |
| 106 | Medium | pillar | IFRS for project controls: the standards every cost engineer must know | IFRS for project controls * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | canonical -> #007 | AB-00092 |
| 107 | Medium | guide | EAC accounting: how estimate at completion flows into the financial statements | EAC accounting * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | canonical -> #027 | AB-00094 |
| 108 | Medium | process-guide | Month-end close for projects: the controls-to-finance handshake | month-end close for projects * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | canonical -> #028 | AB-00141 |
| 109 | Medium | guide | IFRS 15 for construction: percentage of completion and performance obligations | IFRS 15 for construction * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | canonical -> #029 | AB-00130 |
| 110 | Medium | guide | Project cash flow: modelling, S-curves and the forecast the CFO actually reads | project cash flow forecasting * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | canonical -> #030 | AB-00096 |
| 111 | Medium | pillar | AI in project controls: everything the search actually asks | AI in project controls | AI in project controls | PCL-AI | pciai.org | canonical -> #056 | AB-00038 |
| 112 | Medium | guide | AI for construction scheduling explained: a practitioner's guide | AI for construction scheduling | AI in project controls | PCL-AI | pciai.org | canonical -> #058 | AB-00040 |
| 113 | Medium | faq | Will AI replace planning engineers: the questions people really ask | will AI replace planning engineers | AI in project controls | PCL-AI | pciai.org | canonical -> #060 | AB-00042 |
| 114 | Medium | guide | Future of project controls explained: a practitioner's guide | future of project controls | AI in project controls | suite | pciai.org | canonical -> #063 | AB-00045 |
| 115 | Medium | guide | AI for cost estimating in construction explained: a practitioner's guide | AI for cost estimating in construction | AI in project controls | PFL-AI | pciai.org | canonical -> #064 | AB-00046 |
| 116 | Medium | guide | How to build a realistic project schedule in Primavera P6 | realistic schedule in Primavera P6 * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | canonical -> #038 | AB-00078 |
| 117 | Medium | glossary | What is critical path method (CPM)? Definition, examples and why it matters | critical path method definition * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | canonical -> #039 | AB-00591 |
| 118 | Medium | glossary | What is total float? Definition, examples and why it matters | total float definition * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | canonical -> #040 | AB-00601 |
| 119 | Medium | comparison | Is Primavera P6 certification worth it? Training options compared | primavera p6 online course | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | canonical -> #036 | AB-03488 |
| 120 | Medium | guide | Quantitative schedule risk analysis for beginners | quantitative schedule risk analysis QSRA | Risk management | PCL-AI | projectcontrolsinstitute.org | canonical -> #044 | AB-00081 |
| 121 | Medium | how-to | How to run a Monte Carlo cost simulation: a step-by-step guide | Monte Carlo cost simulation * | Risk management | PFL-AI | projectcontrolsinstitute.org | canonical -> #045 | AB-01051 |
| 122 | Medium | how-to | How to build a risk register stakeholders actually use | risk register that gets used * | Risk management | PCL-AI | projectcontrolsinstitute.org | canonical -> #046 | AB-01037 |
| 123 | Medium | pillar | What is project controls: everything the search actually asks | what is project controls | Project controls fundamentals | suite | projectcontrolsinstitute.org | canonical -> #002 | AB-00027 |
| 124 | Medium | comparison | Project controls vs project management: an honest comparison | project controls vs project management | Project controls fundamentals | suite | projectcontrolsinstitute.org | canonical -> #047 | AB-00028 |
| 125 | Medium | pillar | What are capital projects? The complete beginner's guide | what are capital projects * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | canonical -> #009 | AB-00207 |
| 126 | LinkedIn Article | comparison | IFRS 15 vs IAS 11: what actually changed for construction | IFRS 15 vs IAS 11 * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00253 |
| 127 | LinkedIn Article | comparison | Percentage of completion vs completed contract method | percentage of completion method * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00254 |
| 128 | LinkedIn Article | how-to | WIP journal entries on construction contracts: a walkthrough | WIP journal entries construction * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00257 |
| 129 | LinkedIn Article | guide | Project financing explained for project controls professionals | project financing for project controls * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00102 |
| 130 | LinkedIn Article | guide | EVM as management practice, not compliance theatre | earned value management practice * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00216 |
| 131 | LinkedIn Article | guide | Common misconceptions about earned value management | earned value misconceptions * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00217 |
| 132 | LinkedIn Article | faq | Will AI replace planning engineers? The evidence, not the hype | AI and planning engineer roles * | AI in project controls | PCL-AI | pciai.org | original | AB-00147 |
| 133 | LinkedIn Article | faq | Will AI replace cost engineers? Role change vs role loss | AI and cost engineer roles * | AI in project controls | PFL-AI | pciai.org | original | AB-00149 |
| 134 | LinkedIn Article | faq | Will AI replace quantity surveyors? What the data says | AI and quantity surveyor roles * | AI in project controls | PFL-AI | pciai.org | original | AB-00148 |
| 135 | LinkedIn Article | data-study | AI skills in demand: what employers now ask of project controls professionals | AI skills in project controls * | AI in project controls | suite | pciai.org | original | AB-00100 |
| 136 | LinkedIn Article | data-study | Two-thirds of Gulf professionals already use AI at work: the skills signal | AI adoption in the Gulf * | AI in project controls | suite | pciglobal.ai | original | AB-00302 |
| 137 | LinkedIn Article | faq | Is project controls a good career? An honest assessment | is project controls a good career * | Certification and careers | suite | pciworld.org | original | AB-00120 |
| 138 | LinkedIn Article | faq | How do you get into project controls with no experience? | project controls with no experience * | Certification and careers | suite | pciworld.org | original | AB-00259 |
| 139 | LinkedIn Article | faq | What industries pay project controls professionals the most? | highest paying project controls industries * | Certification and careers | suite | pciworld.org | original | AB-00258 |
| 140 | LinkedIn Article | data-study | Women in project controls: the state of the profession | women in project controls * | Certification and careers | suite | pciworld.org | original | AB-00127 |
| 141 | LinkedIn Article | comparison | Construction scheduling methods compared: CPM, LOB, takt, agile | construction scheduling methods * | Planning and scheduling | PML-AI | projectcontrolsinstitute.org | original | AB-00229 |
| 142 | LinkedIn Article | guide | What a Must Finish By date really does to your schedule | must finish by date P6 * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00184 |
| 143 | LinkedIn Article | how-to | Recovery schedules: when the engineer demands one and how to build it | how to build a recovery schedule * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00199 |
| 144 | LinkedIn Article | guide | Risk analysis as a cost probability distribution: reading the curve | cost probability distribution * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-00225 |
| 145 | LinkedIn Article | guide | QRA capability: building quantitative risk analysis into the organisation | QRA capability * | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-00226 |
| 146 | LinkedIn Article | data-study | Why oil and gas projects overrun: causes and the data | oil and gas project overruns * | Project controls fundamentals | PFL-AI | pciglobal.ai | original | AB-03325 |
| 147 | LinkedIn Article | data-study | Why LNG projects overrun: causes and the data | LNG project overruns * | Project controls fundamentals | PFL-AI | pciglobal.ai | original | AB-03333 |
| 148 | LinkedIn Article | data-study | Why rail projects overrun: causes and the data | rail project overruns * | Project controls fundamentals | PFL-AI | pciglobal.ai | original | AB-03341 |
| 149 | LinkedIn Article | faq | What is EPC? What planners need to know about contract delivery models | what is EPC contract * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00288 |
| 150 | LinkedIn Article | faq | What KPIs belong on a project dashboard? | project dashboard KPIs * | Project controls fundamentals | PCL-AI | projectcontrolsinstitute.org | original | AB-00282 |
| 151 | Substack | data-study | Global construction cost inflation: what the index actually shows | construction cost inflation * | Cost control and estimating | PFL-AI | pciglobal.ai | original | AB-00304 |
| 152 | Substack | data-study | Labour availability: now the number-one construction cost driver | construction labour cost driver * | Cost control and estimating | PFL-AI | pciglobal.ai | original | AB-00305 |
| 153 | Substack | data-study | The most expensive cities to build in: the cost-per-m2 league table | cost per square metre to build * | Cost control and estimating | PFL-AI | pciglobal.ai | original | AB-00306 |
| 154 | Substack | faq | What are the four EAC formulas and when does each apply? | when to use each EAC formula * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00267 |
| 155 | Substack | faq | What does a CPI below 1 mean (and what to do about it)? | CPI below 1 meaning * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00265 |
| 156 | Substack | faq | When should you use bottom-up ETC? | bottom-up ETC * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00266 |
| 157 | Substack | data-study | Cost benchmarks and metrics for oil and gas projects | oil and gas cost benchmarks * | Project controls fundamentals | PFL-AI | pciglobal.ai | original | AB-03327 |
| 158 | Substack | data-study | Cost benchmarks and metrics for LNG projects | LNG cost benchmarks * | Project controls fundamentals | PFL-AI | pciglobal.ai | original | AB-03335 |
| 159 | Substack | how-to | FIDIC's 84-day fully detailed claim (20.2.4): building the submission | FIDIC fully detailed claim * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00247 |
| 160 | Substack | faq | What is generative scheduling? | generative scheduling * | AI in project controls | PCL-AI | pciai.org | original | AB-00283 |
| 161 | Substack | faq | How accurate is AI construction cost estimating? | AI cost estimating accuracy * | AI in project controls | PFL-AI | pciai.org | original | AB-00284 |
| 162 | Substack | guide | Planned value vs earned value under different P6 settings | planned value vs earned value in P6 * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00187 |
| 163 | Substack | guide | WBS-level reporting in agile environments: making hybrid delivery measurable | WBS reporting in agile * | Planning and scheduling | PML-AI | projectcontrolsinstitute.org | original | AB-00105 |
| 164 | Substack | guide | AACE 42R-08: contingency by parametric estimating | contingency by parametric estimating * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-00234 |
| 165 | Substack | data-study | How many project controls jobs are there? The 2026 market, by region and role | project controls job market * | Certification and careers | suite | pciworld.org | original | AB-00087 |
| 166 | DEV Community | how-to | Using large language models to review schedules: a practical protocol | LLM schedule review * | AI in project controls | PCL-AI | pciai.org | canonical -> #068 | AB-00151 |
| 167 | DEV Community | how-to | Prompt engineering for project professionals: a working introduction | prompt engineering for project professionals * | AI in project controls | suite | pciai.org | canonical -> #067 | AB-00150 |
| 168 | DEV Community | how-to | Generative AI for project reports: speed without losing the audit trail | generative AI project reporting * | AI in project controls | PFL-AI | pciai.org | canonical -> #070 | AB-00157 |
| 169 | DEV Community | template | Building an AI policy for a project controls team | AI policy for project controls * | AI in project controls | PCL-AI | pciai.org | canonical -> #069 | AB-00154 |
| 170 | DEV Community | comparison | Best AI construction scheduling software: an honest comparison | best AI construction scheduling software | AI in project controls | PCL-AI | pciai.org | canonical -> #065 | AB-00047 |
| 171 | DEV Community | guide | AI in construction project management explained: a practitioner's guide | AI in construction project management | AI in project controls | PML-AI | pciai.org | canonical -> #059 | AB-00041 |
| 172 | DEV Community | how-to | How to run a Monte Carlo cost simulation: a step-by-step guide | Monte Carlo cost simulation * | Risk management | PFL-AI | projectcontrolsinstitute.org | canonical -> #045 | AB-01051 |
| 173 | DEV Community | glossary | What is critical path method (CPM)? Definition, examples and why it matters | critical path method definition * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | canonical -> #039 | AB-00591 |
| 174 | DEV Community | guide | Project cash flow: modelling, S-curves and the forecast the CFO actually reads | project cash flow forecasting * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | canonical -> #030 | AB-00096 |
| 175 | DEV Community | how-to | Using EVM to predict final project costs: the four EAC formulas | four EAC formulas * | Earned value management | PFL-AI | projectcontrolsinstitute.org | canonical -> #023 | AB-00215 |
| 176 | Hashnode | pillar | AI in project controls: everything the search actually asks | AI in project controls | AI in project controls | PCL-AI | pciai.org | canonical -> #056 | AB-00038 |
| 177 | Hashnode | guide | AI for cost estimating in construction explained: a practitioner's guide | AI for cost estimating in construction | AI in project controls | PFL-AI | pciai.org | canonical -> #064 | AB-00046 |
| 178 | Hashnode | guide | How to build a realistic project schedule in Primavera P6 | realistic schedule in Primavera P6 * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | canonical -> #038 | AB-00078 |
| 179 | Hashnode | glossary | What is total float? Definition, examples and why it matters | total float definition * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | canonical -> #040 | AB-00601 |
| 180 | Hashnode | guide | Quantitative schedule risk analysis for beginners | quantitative schedule risk analysis QSRA | Risk management | PCL-AI | projectcontrolsinstitute.org | canonical -> #044 | AB-00081 |
| 181 | Hashnode | how-to | How to build a risk register stakeholders actually use | risk register that gets used * | Risk management | PCL-AI | projectcontrolsinstitute.org | canonical -> #046 | AB-01037 |
| 182 | Hashnode | guide | Cost control methods that catch overruns early | cost control in construction | Cost control and estimating | PCL-AI | projectcontrolsinstitute.org | canonical -> #031 | AB-00079 |
| 183 | Hashnode | guide | EAC accounting: how estimate at completion flows into the financial statements | EAC accounting * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | canonical -> #027 | AB-00094 |
| 184 | Hashnode | guide | Earned value management explained with a worked example | earned value worked example * | Earned value management | PCL-AI | projectcontrolsinstitute.org | canonical -> #021 | AB-00080 |
| 185 | Hashnode | process-guide | The capital project management process, step by step | capital project management process * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | canonical -> #049 | AB-00208 |
| 186 | Quora | qa-list | Will AI replace planning engineers? | will AI replace planning engineers | AI in project controls | PCL-AI | pciai.org | original | AB-00042 |
| 187 | Quora | qa-list | Will AI replace project managers? | will AI replace project managers | AI in project controls | PML-AI | pciai.org | original | AB-00043 |
| 188 | Quora | qa-list | Is project controls in demand? | is project controls in demand * | Certification and careers | suite | pciworld.org | original | AB-03684 |
| 189 | Quora | qa-list | Is project controls a stressful job? | is project controls stressful * | Certification and careers | suite | pciworld.org | original | AB-03683 |
| 190 | Quora | qa-list | What is BCWS vs BCWP? (the old names for PV and EV) | BCWS vs BCWP * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00264 |
| 191 | Quora | qa-list | Control account vs work package: what is the difference? | control account vs work package * | Earned value management | PML-AI | projectcontrolsinstitute.org | original | AB-00272 |
| 192 | Quora | qa-list | What is three-point estimating? | three-point estimating * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00273 |
| 193 | Quora | qa-list | Cost overrun vs cost escalation: what is the difference? | cost overrun vs cost escalation * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00274 |
| 194 | Quora | qa-list | What is a schedule of values? | schedule of values * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00275 |
| 195 | Quora | qa-list | What is a construction draw schedule? | construction draw schedule * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00276 |
| 196 | Quora | qa-list | What is the 100% rule in WBS? | 100% rule WBS * | Planning and scheduling | PML-AI | projectcontrolsinstitute.org | original | AB-00269 |
| 197 | Quora | qa-list | WBS vs project schedule: what is the difference? | WBS vs project schedule * | Planning and scheduling | PML-AI | projectcontrolsinstitute.org | original | AB-00270 |
| 198 | Quora | qa-list | What is a recovery schedule? | what is a recovery schedule * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00289 |
| 199 | Quora | qa-list | How many levels should a WBS have? | how many WBS levels * | Planning and scheduling | PML-AI | projectcontrolsinstitute.org | original | AB-00271 |
| 200 | Quora | qa-list | How do you apply for an extension of time? | how to apply for an extension of time * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00290 |
| 201 | LinkedIn post | linkedin-post | The month-end close is where project controls goes to die | month-end close for projects * | Project controls fundamentals | PFL-AI | projectcontrolsinstitute.org | original | AB-00141 |
| 202 | LinkedIn post | linkedin-post | An accountant and a planner described the same month. Two answers. | finance and delivery overlap * | Project controls fundamentals | suite | projectcontrolsinstitute.org | original | AB-00088 |
| 203 | LinkedIn post | linkedin-post | What is contract administration, in one paragraph a lawyer would accept | contract administration * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-01108 |
| 204 | LinkedIn post | linkedin-post | NEC4 early warnings: the meeting nobody books until it is too late | NEC4 early warnings * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-01115 |
| 205 | LinkedIn post | linkedin-post | NEC4 compensation events, explained without the clause numbers | NEC4 compensation events * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-01112 |
| 206 | LinkedIn post | linkedin-post | Six NEC4 main options, and the one your cost report depends on | NEC4 main options * | Project controls fundamentals | PFL-AI | projectcontrolsinstitute.org | original | AB-01118 |
| 207 | LinkedIn post | linkedin-post | A time impact analysis is not a delay claim. Here is the difference. | time impact analysis * | Project controls fundamentals | PCL-AI | projectcontrolsinstitute.org | original | AB-01136 |
| 208 | LinkedIn post | linkedin-post | The FIDIC claims procedure, drawn as a clock | FIDIC claims procedure * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-01121 |
| 209 | LinkedIn post | linkedin-post | FIDIC 1999 vs 2017: the claims change most teams still miss | FIDIC 1999 vs 2017 claims * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00250 |
| 210 | LinkedIn post | linkedin-post | Standing up contract administration in the first 30 days | contract administration first 30 days * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-05532 |
| 211 | LinkedIn post | linkedin-post | Your dashboard has 22 KPIs. Four of them are decisions. | project dashboard KPIs * | Project controls fundamentals | PCL-AI | projectcontrolsinstitute.org | original | AB-00282 |
| 212 | LinkedIn post | linkedin-post | Earned value is a measurement of work, not of money spent | what is earned value * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00460 |
| 213 | LinkedIn post | linkedin-post | Planned value: the number everyone quotes and few can derive | planned value PV * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00455 |
| 214 | LinkedIn post | linkedin-post | Actual cost is where finance and delivery stop agreeing | actual cost AC * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00465 |
| 215 | LinkedIn post | linkedin-post | BAC is fixed. EAC is an argument. Know which you are quoting. | budget at completion BAC * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00470 |
| 216 | LinkedIn post | linkedin-post | Progress measurement: rules of credit beat percentage guesses | progress measurement rules of credit * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00585 |
| 217 | LinkedIn post | linkedin-post | The integrated baseline review nobody prepares for | integrated baseline review IBR * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00566 |
| 218 | LinkedIn post | linkedin-post | Class 5 to Class 1: what an estimate class actually promises | estimate classification * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00793 |
| 219 | LinkedIn post | linkedin-post | Parametric estimating works until the cost driver stops being the driver | parametric estimating * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00806 |
| 220 | LinkedIn post | linkedin-post | Analogous estimating: fast, cheap, and wrong in one predictable way | analogous estimating * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00810 |
| 221 | LinkedIn post | linkedin-post | Front-loaded cash flow is a signal, not a scheduling preference | front-loaded cash flow * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00277 |
| 222 | LinkedIn post | linkedin-post | Build the construction cash flow spreadsheet once. Use it for a decade. | construction cash flow spreadsheet * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00213 |
| 223 | LinkedIn post | linkedin-post | A risk register with 140 rows is a filing exercise | risk register * | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-01035 |
| 224 | LinkedIn post | linkedin-post | Qualitative risk analysis: the 5x5 matrix and its honest limits | qualitative risk analysis * | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-01042 |
| 225 | LinkedIn post | linkedin-post | The risk breakdown structure is the part teams skip and then need | risk breakdown structure * | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-01039 |
| 226 | LinkedIn post | linkedin-post | Contingency drawdown: the curve that tells you the truth early | contingency drawdown * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-05527 |
| 227 | LinkedIn post | linkedin-post | AACE 44R-08: contingency by expected value, done properly | contingency by expected value * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-00235 |
| 228 | LinkedIn post | linkedin-post | Your P80 is not a commitment unless someone owns the P50 gap | quantitative risk analysis QRA * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-01046 |
| 229 | LinkedIn post | linkedin-post | Negative float is a message about your constraints, not your team | negative float * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00182 |
| 230 | LinkedIn post | linkedin-post | Hard vs soft constraints in P6: one of them lies to you | P6 constraints * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00183 |
| 231 | LinkedIn post | linkedin-post | How total float is actually calculated in Primavera P6 | total float calculation P6 * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00185 |
| 232 | LinkedIn post | linkedin-post | Schedule percent complete in P6: three settings, three answers | schedule percent complete P6 * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00186 |
| 233 | LinkedIn post | linkedin-post | Re-baselining is a governance act, not a scheduling task | re-baselining * | Planning and scheduling | PML-AI | projectcontrolsinstitute.org | original | AB-00622 |
| 234 | LinkedIn post | linkedin-post | Prompting a model to review a schedule: the protocol we use | LLM schedule review * | AI in project controls | PCL-AI | pciai.org | original | AB-00151 |
| 235 | LinkedIn post | linkedin-post | Master data management is the unglamorous reason your AI pilot failed | master data management on projects * | AI in project controls | PCL-AI | pciai.org | original | AB-01397 |
| 236 | LinkedIn post | linkedin-post | Why your XER imports slowly, and what POBS has to do with it | XER import performance * | AI in project controls | PCL-AI | pciai.org | original | AB-00189 |
| 237 | LinkedIn post | linkedin-post | Generative AI wrote the report. Who signs the audit trail? | generative AI project reporting * | AI in project controls | PFL-AI | pciai.org | original | AB-00157 |
| 238 | LinkedIn post | linkedin-post | How to verify a PCI AI credential in under a minute | verify a PCI credential * | Certification and careers | suite | credentialfinder.org | original | AB-00114 |
| 239 | LinkedIn post | linkedin-post | Rate yourself against the project controls skills matrix | project controls skills matrix * | Certification and careers | suite | pciworld.org | original | AB-00128 |
| 240 | LinkedIn post | linkedin-post | Cost estimator jobs are shrinking. Openings are not. Read both numbers. | cost estimator jobs outlook * | Certification and careers | PFL-AI | pciworld.org | original | AB-00293 |
| 241 | LinkedIn carousel | carousel | The controls-to-finance handshake in 10 slides | month-end close for projects * | Project controls fundamentals | PFL-AI | projectcontrolsinstitute.org | original | AB-00141 |
| 242 | LinkedIn carousel | carousel | Capital project management, start to handover: 12 slides | capital project management process * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00208 |
| 243 | LinkedIn carousel | carousel | Extension of time: the evidence pack, slide by slide | extension of time evidence * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-01127 |
| 244 | LinkedIn carousel | carousel | As-planned vs as-built: reading the two bars side by side | as-planned vs as-built * | Project controls fundamentals | PCL-AI | projectcontrolsinstitute.org | original | AB-01141 |
| 245 | LinkedIn carousel | carousel | FIDIC Red vs Yellow Book 2017: who carries the design | FIDIC Red vs Yellow Book * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00249 |
| 246 | LinkedIn carousel | carousel | What is EPC? A 9-slide explainer for planners | what is EPC contract * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00288 |
| 247 | LinkedIn carousel | carousel | The four EAC formulas, one slide each, with the arithmetic on slide 3 | four EAC formulas * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00215 |
| 248 | LinkedIn carousel | carousel | CPI 0.92 on a GBP 40m job: what it costs you by completion | CPI below 1 meaning * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00265 |
| 249 | LinkedIn carousel | carousel | Variance analysis report: the structure that survives a board meeting | variance analysis report * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00569 |
| 250 | LinkedIn carousel | carousel | EVMS in 30 days: the standing-up checklist | earned value management system EVMS * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00562 |
| 251 | LinkedIn carousel | carousel | Monte Carlo in 11 slides: inputs, correlation, and the curve | Monte Carlo cost simulation * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-01052 |
| 252 | LinkedIn carousel | carousel | Building a risk register people actually update | risk register template * | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-01036 |
| 253 | LinkedIn carousel | carousel | Qualitative risk analysis: the workshop run sheet | qualitative risk analysis workshop * | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-01044 |
| 254 | LinkedIn carousel | carousel | QRA checklist: what good looks like, in 10 slides | QRA checklist * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-01048 |
| 255 | LinkedIn carousel | carousel | Cost estimating classes 5 to 1, with the accuracy ranges | estimate classification * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00796 |
| 256 | LinkedIn carousel | carousel | The S-curve, built from scratch in 10 slides | S-curve formula * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00212 |
| 257 | LinkedIn carousel | carousel | Ten P6 layouts every planner should own | P6 layouts * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00194 |
| 258 | LinkedIn carousel | carousel | The WBS dictionary: structure, example, and the 100% rule | WBS dictionary * | Planning and scheduling | PML-AI | projectcontrolsinstitute.org | original | AB-00632 |
| 259 | LinkedIn carousel | carousel | Power BI for project controls: what good looks like | Power BI for project controls * | AI in project controls | PCL-AI | pciai.org | original | AB-01376 |
| 260 | LinkedIn carousel | carousel | Three credentials, three shapes: PCL-AI, PFL-AI, PML-AI | PCI AI credentials compared * | Certification and careers | suite | credentialfinder.org | original | AB-00084 |
| 261 | X / Threads | thread | 7 posts: why the overlap between finance and delivery loses the money | finance and delivery overlap * | Project controls fundamentals | suite | projectcontrolsinstitute.org | original | AB-00088 |
| 262 | X / Threads | thread | 6 posts: what a compensation event does to your cost report | NEC4 compensation events * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-01112 |
| 263 | X / Threads | thread | 8 posts: reading an as-built programme like a forensic planner | as-planned vs as-built * | Project controls fundamentals | PCL-AI | projectcontrolsinstitute.org | original | AB-01141 |
| 264 | X / Threads | thread | 5 posts: what EPC actually transfers, and to whom | what is EPC contract * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00288 |
| 265 | X / Threads | thread | 7 posts: EV, PV, AC — the three numbers, one worked month | earned value worked example * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00461 |
| 266 | X / Threads | thread | 6 posts: when bottom-up ETC beats the formula | bottom-up ETC * | Earned value management | PFL-AI | projectcontrolsinstitute.org | original | AB-00266 |
| 267 | X / Threads | thread | 8 posts: a Monte Carlo run, from inputs to the P50/P80 split | Monte Carlo cost simulation * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-01047 |
| 268 | X / Threads | thread | 6 posts: contingency drawdown, month by month | contingency drawdown * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-05526 |
| 269 | X / Threads | thread | 5 posts: the risk breakdown structure in plain English | risk breakdown structure * | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-01039 |
| 270 | X / Threads | thread | 7 posts: total float, free float and the myth of slack | total float definition * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00598 |
| 271 | X / Threads | thread | 6 posts: schedule crashing without buying the same day twice | schedule crashing * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-05476 |
| 272 | X / Threads | thread | 6 posts: three-point estimating, and where PERT misleads | three-point estimating * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00273 |
| 273 | X / Threads | thread | 7 posts: what a schedule of values is really for | schedule of values * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00275 |
| 274 | X / Threads | thread | 6 posts: what generative scheduling can and cannot do yet | generative scheduling * | AI in project controls | PCL-AI | pciai.org | original | AB-00283 |
| 275 | X / Threads | thread | 5 posts: ETL for project data, explained for planners | ETL for project data * | AI in project controls | PCL-AI | pciai.org | original | AB-01381 |
| 276 | Instagram / Facebook | carousel | Caption + 10 slides: what project controls actually is | what is project controls | Project controls fundamentals | suite | projectcontrolsinstitute.org | original | AB-00027 |
| 277 | Instagram / Facebook | carousel | Caption + 9 slides: the capital project lifecycle | what are capital projects * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-00207 |
| 278 | Instagram / Facebook | carousel | Caption + 8 slides: extension of time in five steps | extension of time claim * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-01126 |
| 279 | Instagram / Facebook | carousel | Caption + 10 slides: what a contract administrator does all week | contract administration * | Project controls fundamentals | PML-AI | projectcontrolsinstitute.org | original | AB-01110 |
| 280 | Instagram / Facebook | carousel | Caption + 8 slides: five KPIs that change a decision | project dashboard KPIs * | Project controls fundamentals | PCL-AI | projectcontrolsinstitute.org | original | AB-00282 |
| 281 | Instagram / Facebook | carousel | Caption + 10 slides: earned value in one worked month | what is earned value management | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00451 |
| 282 | Instagram / Facebook | carousel | Caption + 9 slides: progress measurement without guesswork | progress measurement * | Earned value management | PCL-AI | projectcontrolsinstitute.org | original | AB-00585 |
| 283 | Instagram / Facebook | carousel | Caption + 10 slides: cost estimating classes at a glance | what is cost estimating * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00789 |
| 284 | Instagram / Facebook | carousel | Caption + 9 slides: reading a project cash flow curve | project cash flow forecasting * | Cost control and estimating | PFL-AI | projectcontrolsinstitute.org | original | AB-00213 |
| 285 | Instagram / Facebook | carousel | Caption + 10 slides: the critical path, drawn properly | critical path * | Planning and scheduling | PCL-AI | projectcontrolsinstitute.org | original | AB-00595 |
| 286 | Instagram / Facebook | carousel | Caption + 9 slides: five levels of a work breakdown structure | work breakdown structure levels * | Planning and scheduling | PML-AI | projectcontrolsinstitute.org | original | AB-00268 |
| 287 | Instagram / Facebook | carousel | Caption + 8 slides: what AI does to a planner's week | AI in project controls | AI in project controls | PCL-AI | pciai.org | original | AB-00038 |
| 288 | Instagram / Facebook | carousel | Caption + 9 slides: dashboards in Power BI vs Excel | Power BI vs Excel dashboards * | AI in project controls | PCL-AI | pciai.org | original | AB-01378 |
| 289 | Instagram / Facebook | carousel | Caption + 10 slides: how to become a planning engineer | how to become a planning engineer | Certification and careers | PCL-AI | pciworld.org | original | AB-02559 |
| 290 | Instagram / Facebook | carousel | Caption + 8 slides: the Gulf giga-project hiring wave | project controls jobs in Saudi Arabia | Certification and careers | suite | pciworld.org | original | AB-00119 |
| 291 | Reddit / forum | forum-post | How month-end close actually works on a capital project (r/projectmanagement) | month-end close for projects * | Project controls fundamentals | PFL-AI | projectcontrolsinstitute.org | original | AB-00141 |
| 292 | Reddit / forum | forum-post | Why oil and gas projects overrun: the causes, with the data (r/ConstructionManagers) | oil and gas project overruns * | Project controls fundamentals | PFL-AI | pciglobal.ai | original | AB-03325 |
| 293 | Reddit / forum | forum-post | Is project controls a stressful job? An honest answer (r/civilengineering) | is project controls stressful * | Project controls fundamentals | suite | pciworld.org | original | AB-03683 |
| 294 | Reddit / forum | forum-post | What a risk register is for, and why yours is ignored (r/projectmanagement) | risk register that gets used * | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-01033 |
| 295 | Reddit / forum | forum-post | Monte Carlo on a schedule: what it tells you and what it does not (r/PrimaveraP6) | quantitative schedule risk analysis QSRA | Risk management | PCL-AI | projectcontrolsinstitute.org | original | AB-01048 |
| 296 | Reddit / forum | forum-post | Contingency: expected value vs parametric, and when each breaks (r/CostEngineering) | contingency by expected value * | Risk management | PFL-AI | projectcontrolsinstitute.org | original | AB-00235 |
| 297 | Reddit / forum | forum-post | Python for planners: where it genuinely saves a day a week (r/dataengineering) | Python for planners * | AI in project controls | PCL-AI | pciai.org | original | AB-01401 |
| 298 | Reddit / forum | forum-post | Querying the P6 database directly: what to know first (r/PrimaveraP6) | P6 database queries * | AI in project controls | PCL-AI | pciai.org | original | AB-01385 |
| 299 | Reddit / forum | forum-post | Getting into project controls with no experience: what worked (r/careerguidance) | project controls with no experience * | Certification and careers | suite | pciworld.org | original | AB-00259 |
| 300 | Reddit / forum | forum-post | Which project controls certification is worth it in 2026? (r/projectmanagement) | best project controls certification | Certification and careers | suite | credentialfinder.org | original | AB-00059 |

## Finance/delivery overlap ladder

These 116 pieces state the overlap thesis explicitly — a chartered accountant is examined on
recognition and provisions, an engineer on float and progress measurement, and the money is lost
in between. The rest must still be *consistent* with it.

#007, #008, #010, #011, #013, #018, #023, #024, #025, #027, #028, #029, #030, #031, #032, #033, #045, #047, #057, #063, #064, #069, #070, #079, #080, #090, #091, #097, #103, #105, #106, #107, #108, #109, #110, #114, #115, #121, #124, #126, #127, #128, #129, #130, #133, #134, #143, #144, #146, #147, #148, #150, #151, #152, #154, #155, #156, #157, #158, #159, #161, #163, #164, #168, #169, #172, #174, #175, #177, #182, #183, #193, #194, #195, #200, #201, #202, #206, #211, #214, #215, #218, #221, #222, #226, #227, #228, #233, #237, #240, #241, #243, #247, #248, #249, #251, #254, #255, #256, #260, #261, #262, #266, #267, #268, #271, #272, #273, #278, #280, #283, #284, #291, #292, #296, #300
