---
platform:      Own site — pciworld.org
type:          qa-list
title:         Project controls interview questions and strong answers
meta:          Project controls interview questions and strong answers: earned value, the four EAC methods, IFRS 15 progress, cash days and governed AI, all worked.
primary_kw:    project controls interview questions
secondary_kw:  EAC methods, IFRS 15 five-step model, cash conversion cycle, cost accrual
pillar:        Certification and careers
credential:    suite
target_domain: pciworld.org
canonical:     original
schema:        FAQPage
word_count:    2403
hashtags:      n/a (own site)
ab_id:         —
---

# Project controls interview questions and strong answers

Project controls interview questions test four things: whether you can measure progress honestly, whether you can forecast and defend the result, whether you understand where the schedule meets the accounts, and whether you will hold a position under pressure. Eighteen questions that genuinely come up are answered below, with the arithmetic shown.

One dataset runs through the cost answers so you can follow it end to end: budget at completion (BAC) £12.60m, planned value (PV) £5.80m, earned value (EV) £5.25m, actual cost (AC) £6.10m.

| Group | Questions | What is really being tested |
|---|---|---|
| Measurement and earned value | 1–5 | Whether your reporting would survive an audit |
| Forecasting | 6–9 | Whether you can defend a method, not just produce a number |
| The finance boundary | 10–14 | Whether you can work with accountants rather than around them |
| Governed AI | 15–16 | Whether you can measure a model instead of trusting it |
| Behaviour | 17–18 | Whether you have ever been the unpopular person in the room |

## Project controls interview questions on measurement and earned value

**1. What is project controls, in one sentence?**
The discipline that establishes what a project is meant to cost and take, measures what it is actually costing and taking, and forecasts where it will land in time for someone to do something about it. The last clause is the one that matters — analysis delivered after the decision is history, not control.

**2. What does earned value actually tell you? Work it on the numbers.**
Cost variance CV = EV − AC = 5.25 − 6.10 = **−£0.85m**. Schedule variance SV = EV − PV = 5.25 − 5.80 = **−£0.55m**. Cost performance index CPI = 5.25 ÷ 6.10 = **0.861**. Schedule performance index SPI = 5.25 ÷ 5.80 = **0.905**. The project buys about 86p of work per pound spent and has delivered about 90% of the value it planned by now. Behind and overspending together is the combination with no benign reading.

**3. How would you set the earning rules for a package?**
Before the work starts, in writing, matched to how the work is actually done. Units installed for repeatable work, weighted milestones for sequential work, level of effort only for genuine support. The test is whether a stranger could apply the rule to the same site and reach the same number. A rule chosen after seeing the result is not a rule.

**4. A package has reported 90% complete for three months. What do you do?**
Stop taking the percentage and go and count something. The pattern almost always means the rule allows earning against effort rather than output, or that the remaining 10% is a different kind of work — commissioning, snagging, documentation — that was never separately budgeted. Re-baseline the earning rule for that residual scope, and report the correction rather than absorbing it quietly.

**5. What is the difference between committed, accrued and actual cost?**
Committed is what you are contractually obliged to spend, from the moment the order is placed. Accrued is work or goods received in the period but not yet invoiced. Actual is what the ledger has posted. Reporting only actuals makes a project look cheap at exactly the moment it is running away, because the invoices are still in the post.

## Forecasting: the questions that decide the interview

**6. Run all four estimate at completion methods on that dataset.**

| EAC method | Calculation | Result | What it assumes |
|---|---|---:|---|
| AC + (BAC − EV) | 6.10 + (12.60 − 5.25) | **£13.45m** | The overspend was a one-off; the remaining work runs at plan |
| BAC ÷ CPI | 12.60 ÷ 0.861 | **£14.64m** | Efficiency to date is the best predictor of efficiency to come |
| AC + (BAC − EV) ÷ (CPI × SPI) | 6.10 + 7.35 ÷ 0.779 | **£15.53m** | Schedule pressure keeps driving cost, and both continue |
| AC + bottom-up estimate to complete | 6.10 + a re-estimate of the remaining scope | Whatever the re-estimate says | The people doing the work have re-priced it; the only method that can honestly come in lower |

The spread is £13.45m to £15.53m — **£2.08m** produced by nothing except which assumption is signed.

**7. Which one would you sign?**
Whichever matches the cause of the variance, and say the cause out loud. A one-off event that is now closed points to the first method. Systemic under-performance in productivity points to the second. Cost being driven by schedule pressure that is still running points to the third. Anything late in the project, or after a re-plan, points to a bottom-up re-estimate. The wrong answer is a method with no stated cause.

**8. What is the to-complete performance index, and when do you quote it?**
TCPI = (BAC − EV) ÷ (BAC − AC) = 7.35 ÷ 6.50 = **1.131**. The remaining work must run 13% better than plan against the 0.861 achieved so far. Quote it whenever someone presents a recovery to budget, because it converts optimism into a required efficiency that people can argue with.

**9. How would you set contingency, and who owns it?**
From the risk model, not from a percentage. Quantify the ranged uncertainty and the discrete risks, take the difference between the deterministic estimate and the chosen confidence level, and hold it above the control accounts. Ownership matters more than the amount: contingency held inside a package is spent by the package, and drawdown should require a named approver and a reason recorded against a risk.

## The finance boundary — where most candidates lose it

**10. How does site progress become reported revenue?**
Where revenue is recognised over time, progress drives it. The five-step model in IFRS 15, in plain terms: identify the enforceable contract with the customer; identify the distinct performance obligations promised within it; determine the transaction price, bringing in variable amounts such as variations and claims only where a significant reversal is not expected; allocate that price across the obligations; then recognise revenue as each obligation is satisfied, over time where the customer controls the asset as it is built.

On a construction contract the whole works is often a single obligation, because the elements are integrated rather than distinct. Progress is then measured by an output method or by an input method such as cost-to-cost. This is a description of the model, not accounting advice — your entity's policy and its auditor's view govern.

**11. Show me why that makes the forecast an accounting matter.**
Cost-to-cost progress is costs incurred divided by estimated total costs, and estimated total costs is the EAC. Take a transaction price of £14.40m and costs incurred of £6.10m.

Sign the £13.45m forecast and progress is 6.10 ÷ 13.45 = 45.35%, so revenue is 0.4535 × 14.40 = **£6.53m**. Sign the £15.53m forecast and progress is 6.10 ÷ 15.53 = 39.28%, so revenue is **£5.66m**.

The same project, the same month, the same costs — and **£0.87m** of difference in reported revenue, decided by which estimate at completion the project controls function signed. That is why the forecast is not an internal document.

**12. What is a contract asset, and how would one arise here?**
A contract asset is revenue earned but not yet unconditionally receivable — the customer's payment still depends on something beyond the passage of time, typically certification. If revenue of £6.53m is recognised while only £5.90m has been certified, the £0.63m difference sits as a contract asset rather than a receivable. Invoice ahead of the work instead, and the balance is a contract liability.

**13. What is cut-off, and why does it decide the result?**
Cut-off is the line that says which costs and which progress belong to this period. Move it and you move profit between periods without a single thing changing on site. A fixed data date, an accrual for goods received but not invoiced, and progress measured to that same instant are what make one month comparable with the next.

**14. Explain the cash conversion cycle for a contractor.**
It is the number of days between paying for work and being paid for it: days sales outstanding, plus the days work sits as uncertified work in progress, minus days payable outstanding. With DSO 68, work in progress 31 days and DPO 52 days, the cycle is 68 + 31 − 52 = **47 days**.

At £30m of annual turnover a day is 30,000,000 ÷ 365 = **£82,192**, so 47 days is **£3.86m** of working capital the business funds before the project pays for itself. Cost and cash are different questions, and only one of them breaches a covenant.

## Governed AI — the newest questions on the list

**15. What would you let an AI model do in a cost process?**
Triage, not sign. Say a model flags 300 transactions as miscoded and 210 of them really are: precision = 210 ÷ 300 = **0.70**. If 350 transactions were miscoded in total, recall = 210 ÷ 350 = **0.60**, and F1 = (2 × 0.70 × 0.60) ÷ 1.30 = **0.646**.

Read that plainly: 90 clean transactions get queried needlessly and **140 miscoded ones still reach the ledger**. That is a useful first pass and a completely inadequate control, and being able to say so with the numbers is the answer being looked for.

**16. What governance would you put around a tool that touches the report?**
A named owner, a documented purpose, a measured error rate reviewed on a schedule, a record of which version produced which output, and a human who signs the number. Add a rule that no model output reaches a board pack without a person who can explain how it was produced. The governance question is not whether the tool is clever; it is whether you could reconstruct the number a year later.

## The behavioural two

**17. Tell me about a number you defended that nobody wanted.**
Answer with the specific case: the number, the pressure, what you did, and what happened. Interviewers are checking whether the reporting you produce would bend. Candidates who cannot name a single occasion are usually telling the truth about that, which is the problem.

**18. Tell me about a forecast you got wrong.**
Name it, say why it was wrong, and say what changed in your method afterwards — a different EAC method, a bottom-up re-estimate earlier, an earning rule tightened. Everyone who has forecast anything has been wrong. Only some have changed the method afterwards, and that is the distinction being tested.

## Where a credential fits

Questions 10 to 14 are where good delivery candidates lose interviews, and questions 2 to 9 are where good finance candidates lose them. That is not a coincidence.

A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy — almost never on a critical path or an earning rule. An engineer is examined on float and progress measurement — almost never on cut-off or a contract asset. A project lives in the overlap, and the £0.87m in question 11 is what the overlap costs when nobody owns it.

PCI examines both sides across three credentials: PCI AI Project Controls Leader (PCL-AI) at 13 domains and 61 knowledge areas, PCI AI Project Finance Leader (PFL-AI) at 16 domains and 61 knowledge areas, and PCI Project Management Leader – AI (PML-AI) at 16 domains and 63 knowledge areas.

The Bodies of Knowledge are weighted **40 / 40 / 20** across finance and reporting, project management and governed AI, carry **92 sector case studies** across three volumes, and rest on **113 mandatory PCI Standards with 532 process requirements**. The calculation material behind PFL-AI and PML-AI has been verified by **15,613 machine calculation checks, all passing** — a suite covering PFL-AI and PML-AI only, with no equivalent for PCL-AI.

Those are statements about the examined material, not about anyone's career. For what an independent credential can actually evidence, see [what a certified project controls professional proves](https://projectcontrolsinstitute.org/certified-project-controls-professional).

## Frequently asked questions

**How should I prepare for a project controls interview?**
Take one real project you worked on and be able to walk through its baseline, its earning rules, one month's variance and the forecast you produced, with numbers. Then practise the earned value and EAC arithmetic by hand until it is automatic. Depth on one project beats a shallow tour of five, because the follow-up questions go down, not across.

**Will I be given a numerical test?**
Frequently, and usually a short one: variances and indices from a small dataset, sometimes an EAC and a to-complete index. Practise it on paper. The single most common failure is not the mathematics but reaching for a spreadsheet when the interviewer wanted to see the method. Working through [project controls exam questions](https://projectcontrolsinstitute.org/project-controls-exam-questions) is close preparation.

**Do I need to know accounting standards?**
You need the shape of the model and the vocabulary — performance obligation, transaction price, over-time recognition, contract asset, cut-off, accrual. Nobody expects a project controls candidate to quote a standard. Being able to explain how your progress figure reaches the revenue line puts you ahead of most of the field. Start with [IFRS for project controls](https://projectcontrolsinstitute.org/ifrs-for-project-controls).

**How much scheduling do I need if I am applying on the cost side?**
Enough to read a network and to know what a driving path is, because your forecast depends on it. The schedule questions asked of cost candidates are usually about connection rather than technique. The scheduling-specific set is covered in [planning engineer interview questions](https://pciworld.org/planning-engineer-interview-questions).

**What should I ask the interviewer?**
Who owns the baseline and who may change it. Whether the earning rules are written down. Whether the controls function reports to delivery or independently of it. Whether AI tools already touch the reporting and who signs their output. The answers tell you whether you would be controlling anything or decorating a spreadsheet.

---

*Internal links: link to [what a certified project controls professional proves](https://projectcontrolsinstitute.org/certified-project-controls-professional), [project controls exam questions](https://projectcontrolsinstitute.org/project-controls-exam-questions), [IFRS for project controls](https://projectcontrolsinstitute.org/ifrs-for-project-controls) and [planning engineer interview questions](https://pciworld.org/planning-engineer-interview-questions), each with that anchor; the what does a project controls engineer do and senior planning engineer career path pieces should link back here with the anchor "project controls interview questions".*
