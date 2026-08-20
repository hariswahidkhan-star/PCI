---
platform:      Own site — pciworld.org
type:          guide
title:         What does a project controls engineer do? A day in the role
meta:          What does a project controls engineer do? The weekly rhythm, the month-end cycle, the earned value arithmetic behind the pack, and where finance starts.
primary_kw:    what does a project controls engineer do
secondary_kw:  project controls engineer role, earned value analysis, month-end close, cash conversion cycle
pillar:        Project controls fundamentals
credential:    suite
target_domain: pciworld.org
canonical:     original
schema:        Article
word_count:    1798
hashtags:      n/a (own site)
ab_id:         —
---

# What does a project controls engineer do?

What does a project controls engineer do? Keep the cost and schedule baseline honest, measure what has actually been built rather than what has been spent, and turn that into a forecast someone can act on. The day is progress data in the morning, variance analysis by midday, and a defence of one number in the afternoon.

The role exists because a project generates two versions of the truth — the site's and the ledger's — and somebody has to reconcile them before the board sees either.

## What does a project controls engineer do in a normal day?

| Time | What happens | What goes wrong |
|---|---|---|
| 07:30 | Collect progress returns from package leads and subcontractors | Returns arrive as percentages with no basis; someone reports 90% for the third week running |
| 09:00 | Status the schedule at the data date, apply actuals, let the dates move | Progress entered after the data date, or an actual start dated in the future |
| 10:30 | Reconcile the cost ledger: commitments, accruals, invoices posted | Costs coded to the wrong control account, so the variance appears in the wrong package |
| 12:00 | Calculate earned value and variances, write the narrative | The narrative describes the number instead of explaining the cause |
| 14:00 | Sit in the review and defend it | The forecast has no stated method, so the meeting negotiates the number |
| 16:00 | Update the forecast and the change log | A variation is added to earned value on approval rather than on delivery |

Roughly half the day is data handling and half is argument. New entrants expect the reverse.

## The month, cycle by cycle

The daily rhythm sits inside a monthly one, and that is where the role is judged.

| Stage of the cycle | What the project controls engineer does | The failure mode |
|---|---|---|
| Cut-off | Fix the data date and freeze what counts as this period | A moving cut-off, which makes every period comparison meaningless |
| Progress measurement | Apply the published earning rules to physical work | Inventing a rule after seeing the result |
| Cost capture | Match committed, accrued and actual cost to the same period | Goods received but not invoiced, left out, so the period looks cheap |
| Analysis | Variance, trend, driver, forecast | Reporting percent spent as if it were progress |
| Reporting | The pack, the narrative, the exception list | Fifty pages with no sentence saying what to do |
| Review and action | Agree the recovery actions and log them | Actions with no owner, no date and no measure |

Cut-off discipline is the part that separates a controls function from a spreadsheet. The mechanics are set out in [the project month-end close](https://projectcontrolsinstitute.org/month-end-close-for-projects).

## The arithmetic behind the pack

This is the calculation that sits under almost every project controls conversation, worked on stated numbers.

A project has a budget at completion (BAC) of £16.0m. At the month-nine data date, planned value (PV) is £7.2m, earned value (EV) is £6.6m and actual cost (AC) is £7.4m.

- Cost variance: CV = EV − AC = 6.6 − 7.4 = **−£0.8m**
- Schedule variance: SV = EV − PV = 6.6 − 7.2 = **−£0.6m**
- Cost performance index: CPI = EV ÷ AC = 6.6 ÷ 7.4 = **0.892**
- Schedule performance index: SPI = EV ÷ PV = 6.6 ÷ 7.2 = **0.917**

Read plainly: the project is buying about 89p of work for every pound it spends, and has delivered about 92% of the value it planned to by now.

Percent complete is EV ÷ BAC = 6.6 ÷ 16.0 = **41.25%**. Percent spent is AC ÷ BAC = 7.4 ÷ 16.0 = **46.25%**. The five-point gap is £0.8m of budget that bought no work, and confusing those two figures is the most common error in project reporting.

Then the forecast, by each of the four estimate at completion methods.

| EAC method | Calculation on these inputs | Result | What it assumes |
|---|---|---:|---|
| AC + (BAC − EV) | 7.4 + (16.0 − 6.6) | **£16.80m** | The overspend was a one-off; remaining work runs at plan |
| BAC ÷ CPI | 16.0 ÷ 0.892 | **£17.94m** | Efficiency to date is the best predictor of efficiency to come |
| AC + (BAC − EV) ÷ (CPI × SPI) | 7.4 + 9.4 ÷ 0.818 | **£18.90m** | Schedule pressure keeps driving cost, and both continue |
| AC + bottom-up estimate to complete | 7.4 + a re-estimate of the remaining scope | Whatever the re-estimate says | The people doing the work have re-priced it; the only method that can honestly come in lower |

The spread is £16.80m to £18.90m — **£2.10m** on one dataset, produced entirely by which assumption is signed. The project controls engineer's job is to name the assumption in the same sentence as the number, every time.

To-complete performance index: TCPI = (BAC − EV) ÷ (BAC − AC) = 9.4 ÷ 8.6 = **1.093**. The remaining work has to run 9% better than plan against the 0.892 achieved so far, which is the honest test of any recovery narrative.

## Where the role touches finance

This is the half of the job that adverts rarely describe and interviews always test.

**Cut-off and accrual.** Work done in the period but not yet invoiced still belongs to the period. Anyone who cannot produce a defensible accrual hands the reporting to someone with less information about the work.

**Progress feeding revenue.** Where revenue is recognised over time, the progress measure drives it. A change in earning rule is therefore a change in reported revenue, which is why the rule must be published before the work starts.

**Cash, not just cost.** Cost is what the project consumed; cash is what the business had to fund. Only one of them causes a covenant problem.

The cash conversion cycle makes that concrete. Take a contractor whose average days sales outstanding is 62, whose work sits as uncertified work in progress for 28 days, and who pays its supply chain in 45 days.

Cash conversion cycle = 62 + 28 − 45 = **45 days**.

At an annual turnover of £24m, a day of turnover is 24,000,000 ÷ 365 = **£65,753**. Forty-five days is therefore **£2.96m** of working capital the business funds before the project pays for itself.

Getting certification ten days earlier — usually a document and evidence problem rather than a commercial one — releases 10 × 65,753 = **£658,000** of cash without changing the profit by a penny. That is a project controls output, and it is the one finance directors remember.

## Project controls engineer, planner, cost engineer, project manager

| Role | Owns | Judged on | First question they ask in a meeting |
|---|---|---|---|
| Project controls engineer | The integrated cost and schedule picture | Whether the forecast held | "What does this do to the outturn?" |
| Planning engineer | Logic, durations, the date | Whether the driving path was right | "What is driving the date?" |
| Cost engineer | Budgets, commitments, actuals, accruals | Whether the ledger reconciles | "Is that committed or accrued?" |
| Project manager | Delivery, scope, people, the client | Whether the project delivered | "What do you need me to decide?" |

On a small project one person does all four. On major capital work they are separate seats, and the controls engineer is the one who has to make the other three agree on a single set of numbers. The route into the second of those seats is set out in [how people arrive at a first planning job](https://pciworld.org/how-to-become-a-planning-engineer).

## What the role is not

**It is not reporting.** Reporting is the output. The work is measurement discipline and the argument that follows it.

**It is not neutral administration.** A project controls engineer takes positions: this is the progress, this is the accrual, this is the forecast. Having no view is why some controls functions get ignored.

## Where a credential fits

The month-nine problem above is a delivery question and an accounting question at once. The EAC that gets signed becomes an estimate in the accounts; the earning rule becomes reported progress; the certification lag becomes cash.

A chartered accountant is examined on when revenue may be recognised, almost never on a float path. An engineer is examined on progress measurement, almost never on cut-off or a contract asset. The overlap is where the money goes missing.

PCI examines both sides across three credentials: PCI AI Project Controls Leader (PCL-AI) at 13 domains and 61 knowledge areas, PCI AI Project Finance Leader (PFL-AI) at 16 domains and 61 knowledge areas, and PCI Project Management Leader – AI (PML-AI) at 16 domains and 63 knowledge areas.

The Bodies of Knowledge are weighted **40 / 40 / 20** across finance and reporting, project management and governed AI, and rest on **113 mandatory PCI Standards carrying 532 process requirements**. The calculation material behind PFL-AI and PML-AI has been verified by **15,613 machine calculation checks, all passing** — a suite covering PFL-AI and PML-AI only, with no equivalent for PCL-AI.

## Frequently asked questions

**Is a project controls engineer the same as a project manager?**
No. A project manager decides and delivers; a project controls engineer measures and forecasts, and is deliberately separate so that the measurement is not written by the person being measured. Many organisations blur this on small projects, which is exactly where optimistic reporting tends to appear.

**Which software do I need?**
Primavera P6 or Microsoft Project on the schedule side, an ERP cost module such as SAP or Oracle on the cost side, and a spreadsheet for everything the systems will not do. Learn the method underneath rather than the menus, because the method transfers between employers and the menus do not.

**Where does the role lead?**
Usually to project controls lead, then manager, then head of function; sideways moves into commercial management, risk or finance business partnering are common because the vocabulary transfers. The step that matters is from producing the number to answering for it.

**What qualifications does a project controls engineer need?**
An engineering, construction or quantity surveying degree is the common entry route, but it is not the only one, and employers weight demonstrated competence more heavily than the subject of the degree. What is tested at interview is whether you can derive an earned value position, defend a forecast method and explain how progress was measured. A certification helps where it examines those things rather than attendance.

**How is a project controls engineer different from a cost engineer?**
A cost engineer owns the money: estimate, commitments, accruals, actuals and the cost forecast. A project controls engineer owns the integration of cost with the schedule, the risk position and the reporting that follows, so the cost role sits inside the controls function on larger projects. On smaller ones the same person does both, which is where cut-off errors between the two datasets tend to start.

---

*Internal links: link to [the project month-end close](https://projectcontrolsinstitute.org/month-end-close-for-projects), [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas), [project cash flow forecasting](https://projectcontrolsinstitute.org/project-cash-flow-forecasting) and [what project controls covers](https://projectcontrolsinstitute.org/what-is-project-controls), each with that anchor; the project controls interview questions and how to become a planning engineer pieces should link back here with the anchor "what does a project controls engineer do".*
