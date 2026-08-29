---
platform:      Own site — projectcontrolsinstitute.org
type:          pillar
title:         What is project controls, and who actually does it?
meta:          What is project controls? The functions, the monthly cycle, the arithmetic behind a defended forecast, and how it differs from project management.
primary_kw:    what is project controls
secondary_kw:  project controls functions, project controls team, cost and schedule integration
pillar:        Project controls fundamentals
credential:    suite
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,933
hashtags:      n/a (own site)
ab_id:         AB-00027
---

# What is project controls, and who actually does it?

Project controls is the discipline that measures where a project has got to, what it has cost, and what it will cost if nothing changes. The short answer to *what is project controls* is this: the measurement and forecasting function of a project, run independently of the people whose performance it measures.

It covers planning, scheduling, estimating, cost control, earned value, risk and reporting. Its job is to produce numbers a decision can be taken on, and to say honestly when they cannot be trusted.

## What is project controls responsible for, function by function

Each function produces a specific artefact. If you cannot name the artefact, the function is not being performed.

| Function | What it produces | What it answers | Common failure |
|---|---|---|---|
| Planning and scheduling | A logic-linked, resource-loaded programme | When will it finish, and what drives that date | Logic built to hit a date rather than to model the work |
| Estimating | A classed estimate with a basis of estimate | What should it cost, and how confident are we | The basis is never written down, so nobody can test the number |
| Cost control | Commitments, accruals, actuals and a control account structure | What have we spent and committed | Actuals lag two months, so the report describes history |
| Earned value and performance | EV, CPI, SPI and a defended forecast | Are we getting the value we are paying for | Progress claimed on effort spent rather than work done |
| Risk | A quantified register and a contingency position | What could still go wrong, and is contingency enough | A register nobody has opened since the kick-off workshop |
| Change control | A change log tied to the baseline | What moved, who approved it, what it cost | Scope creeping in through drawings rather than change notices |
| Reporting | A report by exception, aimed at a decision | What must someone decide this month | Forty pages, no decision |

The functions are not separable in practice. A schedule slip becomes a cost forecast, a cost trend becomes a contingency drawdown, and a change becomes both.

## Who does project controls?

On a large capital project it is a team: a planning engineer or scheduler, a cost engineer or cost controller, a risk analyst, a document controller, and a project controls manager who owns the integrated position.

On a smaller project it is one person doing all of it badly on a Wednesday, plus a project manager doing the rest of it in a spreadsheet. That is not a criticism. It is why the discipline is worth certifying: on small projects the whole competence sits in one head.

The function reports to the project manager but must be able to publish a number the project manager does not like. Where controls reports only through delivery, forecasts drift optimistic, and the correction arrives at handover when nothing can be done about it.

## How project controls differs from project management

They are answering different questions on the same project.

| | Project management | Project controls |
|---|---|---|
| Core question | What do we do next? | Where are we, and where will we end up? |
| Owns | Scope, team, stakeholders, decisions | Baseline, measurement, forecast, reporting |
| Output | Direction | Evidence |
| Accountable for | Delivery of the outcome | Integrity of the numbers behind it |
| Typical failure | Optimism | Precision about the wrong thing |

A project manager who does their own controls is marking their own homework. A controls function with no delivery understanding produces immaculate reports about work it does not understand. Both failure modes are common, and both are expensive.

## The monthly cycle, with the money shown

Most of project controls runs on a repeating cadence. Here is a normal month on a construction package, with the arithmetic.

**Cut-off.** The cost cut-off and the progress cut-off are set to the same date. If they are not, earned value is measured against costs from a different fortnight and every index is wrong.

**Commitments.** A purchase order is raised for £600,000 of precast units. Nothing has been spent, but £600,000 of budget is now spoken for. The commitment is the earliest honest signal of cost, and it lands months before the invoice.

**Accruals.** By cut-off, 40% of the units have been delivered, so £240,000 of value has been received. Invoices received total £180,000. The accrual is 240,000 − 180,000 = **£60,000**, and it goes into the cost report as cost incurred but not yet invoiced.

**Actuals.** Ledger actuals for the month arrive from finance. Where they disagree with the accrual, the difference is investigated rather than smoothed, because a repeated accrual error is a systematic bias, not noise.

**Earned value.** Progress is measured against earning rules agreed in advance, not by asking the foreman for a percentage.

**Forecast.** The cost engineer produces the estimate at completion and, more importantly, writes down which method was used and why.

**Report.** One page that names the two or three decisions the sponsor has to take, with the evidence attached behind it.

Miss the cut-off discipline and everything downstream is decoration.

## The two numbers people get wrong

**Progress.** Percent complete claimed by the person doing the work is the least reliable input in the discipline. Earning rules fix it: units complete for repeatable work, milestone weighting for engineering, 0/100 for short-duration tasks, level of effort only for genuine support functions.

**Cost incurred.** Invoices received is not cost incurred. A project reporting only invoices understates its position by whatever sits in the delivery-to-invoice gap, which on a busy month is the size of the variance everyone is arguing about.

## Float, and why the critical path is not a list of important tasks

The critical path is the longest path of dependent activities through the network. It sets the finish date, and it has zero total float by definition.

Take four activities: A takes 5 days, B takes 8 and follows A, C takes 3 and also follows A, and D takes 6 and needs both B and C.

Forward pass. A runs 0 to 5. B runs 5 to 13. C runs 5 to 8. D cannot start until both are done, so it starts at 13 and finishes at **19 days**.

Backward pass. D must start by 13. B must finish by 13, so it must start by 5, giving zero float. C must finish by 13 but finishes at 8, so C has **5 days of total float**.

The critical path is A–B–D. C is not unimportant; it simply has slack. If C slips six days it becomes critical and the project finishes a day late, which is the whole point of tracking float rather than tracking activities people feel strongly about.

## Where the money is actually lost

A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy. They are almost never examined on a critical path or an earning rule.

An engineer is examined on float and progress measurement. They are almost never examined on cut-off or on what makes something a contract asset rather than a receivable.

A project lives in the overlap. The classic loss is a package that reports 62% complete on the delivery side while the finance side has recognised revenue on a different measure, and nobody reconciles the two until the year-end audit asks why the contract asset moved.

Earned value is a control number, not a revenue number. Treating one as the other is how a project can be simultaneously ahead on the cost report and in trouble in the accounts. The PCI credentials are built around that overlap: the [PCL-AI Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) sets its 13 domains and 61 knowledge areas in the proportions 40 / 40 / 20 across project accounting and finance, project management principles and governed AI.

## What AI has changed, and what it has not

Data assembly has changed. Pulling actuals, timesheets and progress into one shape used to consume the first week of every month, and much of that is now automated.

Detection has changed. A model can rank which activities are likely to slip, or flag cost codes behaving unlike their history, faster than a person reading a 4,000-line report.

Accountability has not changed at all. A forecast produced by a model is still signed by a human, still has to be explained to a sponsor, and still has to survive an auditor asking how it was derived. That is why governed AI is examined as a competence in its own right rather than treated as a tool tip.

## How to get into project controls

Most people arrive sideways, from site engineering, quantity surveying, finance or planning support. The fastest useful skill is the monthly cycle above, because it is the only thing every controls job has in common.

After that, depth beats breadth: learn one scheduling tool properly, learn cost coding structures, and learn to read a set of accounts well enough to know what your cost report is feeding.

The certification routes, and what each is actually evidence of, are set out in [the certification pillar](https://projectcontrolsinstitute.org/project-controls-certification). If you want the measurement side first, start with [earned value management](https://projectcontrolsinstitute.org/earned-value-management).

## Frequently asked questions

**Is project controls a good career?**
It is durable work, because every capital project needs someone who can say what the position really is. Demand concentrates where large projects are, which currently means energy, infrastructure, data centres and the Gulf programmes. The trade-off is honest: month-end has a rhythm, deadlines are fixed, and you will sometimes deliver news nobody wants.

**What qualifications do I need to start?**
None are mandatory to enter, and people arrive from engineering, surveying, finance and the trades. Employers look for the monthly cycle, one scheduling tool used properly, and the ability to explain a variance. Certification matters more at the point you want to be trusted with the integrated position rather than one input to it.

**What is the difference between a planner and a cost engineer?**
A planner owns time: network logic, progress, float and the forecast finish date. A cost engineer owns money: commitments, accruals, actuals, the estimate at completion and the contingency position. Both feed earned value, which is where their numbers have to agree, and disagreement between them is usually the first sign of a real problem.

**Do small projects need project controls?**
Yes, in proportion. A £2m fit-out does not need a five-person team, but it still needs a cut-off date, an earning rule, a commitment register and a forecast someone can defend. The failure on small projects is not scale; it is that the same person estimates, reports and approves, so nothing is independently checked.

**Which software does project controls use?**
Scheduling usually runs on Primavera P6 or Microsoft Project, cost lives in the ERP plus a controls layer, and reporting increasingly sits in Power BI. Tools change every few years, so employers hire for the method and train the tool. Learning one scheduler deeply teaches you more than sampling four.

**Is project controls stressful?**
It has a fixed rhythm rather than constant crisis: cut-off, forecast, report, repeat. The pressure comes at month-end and when a forecast is unwelcome. People who like a defensible answer and a clear cadence tend to stay in it for decades.

---

*Internal links: this pillar should link to [the certification pillar](https://projectcontrolsinstitute.org/project-controls-certification) with the anchor "certification routes in project controls", to the [earned value management pillar](https://projectcontrolsinstitute.org/earned-value-management) with the anchor "earned value management", and to the [PCL-AI Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) with the anchor "PCL-AI Body of Knowledge"; the project controls versus project management comparison, the cost control guides and the planning engineer career pieces all link back here with the anchor "what project controls is".*
