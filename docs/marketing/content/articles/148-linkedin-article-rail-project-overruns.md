---
platform:      LinkedIn Article
type:          data-study
title:         Rail project overruns: separating price from performance
meta:          Rail project overruns mix inflation, scope change and real cost growth. How to separate them, with rebasing, P50 versus P80 and possession arithmetic.
primary_kw:    rail project overruns
secondary_kw:  price base rebasing, optimism bias, P50 and P80, possession productivity
pillar:        Project controls fundamentals
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1816
hashtags:      #ProjectControls #CostEngineering #Scheduling #RiskManagement
ab_id:         AB-03341
---

# Rail project overruns: separating price from performance

Rail project overruns are three different things reported as one number: inflation between price bases, scope that changed after the first announcement, and genuine cost growth. Separate the three before comparing anything. The figure first put into the public domain is often a funding envelope, not an estimate.

## What are rail project overruns actually measured against?

Against one of five different figures, all of which get called "the budget". A headline can compare any one of them with any other, so settle which two are in play before quoting a percentage.

| The number | What it actually is | Who sets it | Why it moves |
|---|---|---|---|
| Announcement figure | A political or strategic commitment, often before design | Sponsor or government | Not produced as an estimate and rarely carries a stated price base |
| Funding envelope | The money made available, sometimes including a separate contingency | Funder | Set for affordability as much as for cost |
| Control estimate | The cost engineer's estimate at a stated price base and scope | Project | Rises with design definition, falls with descoping |
| Target price | A contractual figure with a pain and gain mechanism | Client and contractor | Reflects risk transfer and market conditions at award |
| Outturn | Cash actually spent, in the money of the day | Everyone, afterwards | Includes inflation, all approved change and often land and consents |

PCI publishes no industry overrun percentage for rail, because these five are not measuring a common thing and no published series reconciles them.

Compare an outturn in the money of the day against an announcement made a decade earlier in that year's prices and you will produce a large percentage that is mostly arithmetic. Say which two numbers you are comparing, or say nothing.

## How much of the increase is just prices?

Often the majority of it, on a scheme that runs for a decade. Rebasing takes ten minutes and changes the conversation.

Take a control estimate of **£5.6bn** at 2016 prices, with construction cost inflation running at **4.1% a year** for eight years to the spend period.

The index factor is 1.041^8 = **1.379**. The same scope in 2024 money is 5.6 × 1.379 = **£7.72bn**, with no scope change and no performance failure whatsoever.

Now suppose the outturn is **£8.9bn**. The headline is 8.9 ÷ 5.6 = **58.9% over budget**.

Rebased, the real growth is 8.9 ÷ 7.72 = **1.153**, or **15.3%**. In cash, £2.12bn of the £3.30bn increase is price movement, so nearly two-thirds of the reported overrun is inflation.

Those figures are illustrative arithmetic, not project data. The discipline they demonstrate is not optional: every estimate should carry its price base on the same line as its value, and every comparison should rebase before subtracting.

## What do P50 and P80 have to do with an overrun headline?

Everything, because a scheme funded at its P50 is expected to exceed that figure about half the time. That is the definition of a P50, not a failure of the team.

Suppose quantified risk analysis returns a **P50 of £7.7bn** and a **P80 of £8.9bn**. The contingency between them is **£1.2bn**, which is **15.6%** of the P50.

Fund at P50 and report against P50, and roughly one scheme in two produces an overrun story. Fund at P80 and hold the difference as a sponsor-level reserve, and the same schemes report inside budget while spending the same money.

Public appraisal guidance in several countries, including the UK Treasury's Green Book, requires an explicit adjustment for the tendency of sponsors to underestimate cost and duration early on. Stripping that adjustment out to make a business case work is the most reliable predictor of a later headline there is.

State the confidence level next to the number. "£7.7bn (P50, 2024 prices, scope as at gate 3)" is a sentence nobody can misquote.

## Why does access, not construction, set the rail programme?

Because on an operational railway you cannot build when the trains are running, so the schedule is a function of possessions rather than resources. Adding labour does not add hours if there is no track to stand on.

Take **60 weekend possessions** planned at **30 usable hours** each: 60 × 30 = **1,800 productive hours** in the plan.

Realised handback discipline, safety briefings, walking time and isolation delays reduce the usable window to **24.5 hours**: 60 × 24.5 = **1,470 hours**, a shortfall of **330 hours**.

Recovering 330 hours needs 330 ÷ 24.5 = **13.5**, so **14 further possessions**. Possessions come round once a week, so that is **14 weeks** added to that path.

If the path carried **6 weeks** of total float, the completion date moves by **8 weeks**. Critical path arithmetic is unforgiving here: float absorbs the first part of the loss silently, which is exactly why the effect is usually noticed late.

Model usable hours per possession as a range rather than a point and you get [a P50 and a P80 completion date](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) instead of one optimistic figure that nobody can defend at the next gate.

The same logic applies to signalling commissioning windows, station closures and third-party consents. Model them as activities with named owners, not as assumptions in the estimate's basis.

## Which causes actually recur on rail schemes?

The recurring causes are structural rather than exotic, and most of them are decided before construction starts.

| Cause | Where it originates | What it does to the number | Leading indicator |
|---|---|---|---|
| Design maturity at the funding decision | Sponsor pressure for an early figure | Sets a control estimate the design cannot support | Design deliverables approved against plan |
| Scope drift through stakeholder commitments | Consents, planning conditions, local agreements | Adds scope that never appears as a change | Commitments register growth against the baseline |
| Utilities diversions | Third-party assets discovered or mis-recorded | Adds cost and, worse, delays enabling works | Diversion agreements signed against diversions required |
| Ground conditions and existing structures | Old infrastructure with incomplete records | Rework, redesign and temporary works | Ground investigation coverage of the alignment |
| Access and possession productivity | Operational railway constraints | Extends duration at a fixed burn rate | Usable hours per possession, measured every weekend |
| Systems integration and commissioning | Multiple suppliers, one railway | Concentrates risk at the end, where float is gone | Interface issues closed against issues raised |
| Interface with the operator's requirements | The people who must accept the asset | Late change with high consequence | Acceptance criteria agreed against systems in build |

The rows nobody funds properly are the last two. Cost curves are flat by then, so the cost report goes quiet at exactly the point where the schedule risk peaks.

## What happens to the same slip in the accounts?

It moves between two very different reporting worlds, and the people in each rarely read the other's output.

For the sponsor, expenditure on an asset under construction sits on the balance sheet while it is being built, and capitalisation of borrowing costs stops when the asset is ready for use. Extending the programme therefore changes both the capitalised total and the date depreciation begins.

For the contractor, revenue on a long-term contract is usually recognised over time by measuring progress, commonly cost incurred against total forecast cost. Raising the forecast cost lowers the measured percentage complete, so a delivery decision made on site becomes a correction to revenue already reported.

The two worlds train different people. A planner's formation covers float, possessions and progress measurement and stops short of cut-off, capitalisation and the contract asset; a reporting accountant's covers recognition and disclosure and stops short of a possession productivity rate.

Rail programmes lose money in the space between the two, which is why the PCI AI Project Finance Leader (PFL-AI) credential, at 16 domains and 61 knowledge areas, examines both sides rather than one.

The Institute's own requirements sit alongside that as 113 mandatory PCI Standards carrying 532 process requirements. They are certification requirements established by the Institute, not law, and they exist so that a process claim can be tested rather than asserted.

## How should a rail cost report be presented so it cannot be misread?

Put the basis on the face of the report, not in an appendix nobody opens. State the price base with every value, the confidence level with every total, the scope reference, and one data date used for cost and schedule alike.

Then split the movement since the last report into three lines: inflation, approved scope change, and performance. A report showing a single movement figure invites the reader to attribute all of it to the third line.

That version is harder to write and far harder to argue with. It also makes the honest case for more funding, because it separates the part of the increase the team could have controlled from the part it could not.

## Frequently asked questions

**Is an inflation-driven increase an overrun?**
Not against a real-terms baseline, and not if the estimate's price base was stated. It is a genuine increase in the cash the funder must provide, so it matters to affordability. The distinction is between a cost control failure and a funding requirement, and conflating the two damages trust in both directions. Report both lines separately every period.

**Why not simply publish the P80 as the budget?**
Some sponsors do, and reported performance improves while the up-front funding requirement rises. Money held as contingency is money not available for other schemes, so the affordability case weakens. The workable compromise is to fund at P50 at project level and hold the P50-to-P80 difference as a portfolio reserve with a published release process.

**How is scope change tracked when it never goes through change control?**
Through a commitments register maintained alongside the change register. Planning conditions, stakeholder undertakings and consent obligations create real scope without ever presenting as a variation. Reviewing the commitments register against the baseline scope every quarter is the only reliable way to catch it before it turns up as a cost.

**What single measure predicts a rail overrun earliest?**
Design deliverables approved against plan, in the year before main works start. Late design pushes construction into the wrong season, compresses commissioning and forces work into fewer access windows, and every one of those effects is expensive. It is available long before any cost variance appears and it is rarely reported to the board.

---

*PCI publishes certification requirements. Nothing here is legal, tax or accounting advice. Every figure in the worked examples above is illustrative arithmetic, not project data, and PCI publishes no industry overrun statistic.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Linking note: one cross-estate link now sits in the body. The hub link to quantitative schedule risk analysis sits after the possession arithmetic, because ranging usable hours is what turns a single date into a P50 and a P80, and this piece stops at the point estimate. The pciglobal.ai link to the UK certification page has been removed. It sat after the credential paragraph and was justified by the regional domain's territory rather than by a question the sentence asked — the sentence itself conceded that market recognition was "a separate question" and then linked anyway. Nothing in a piece about price bases, possession productivity and capitalisation raises which qualifications a British market recognises, so the link has gone rather than been moved to a sentence written to host it. The target domain moved with it: this piece works rebasing, P50 and P80, possession arithmetic and capitalisation, which is hub territory, and it now carries no regional framing to justify filing it against pciglobal.ai. The proposed second hub link to total float was dropped: only one link per domain is allowed per piece, and the six weeks of float in the possession example is worked here rather than elsewhere.*
