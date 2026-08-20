---
platform:      Own site — projectcontrolsinstitute.org
type:          practice
title:         Project controls exam questions: 25 problems solved
meta:          Twenty-five project controls exam questions with full worked answers: earned value, forecasting, float, revenue recognition, cash, contingency and metrics.
primary_kw:    project controls exam questions
secondary_kw:  EVM practice problems, EAC methods, total float, cost-to-cost revenue
pillar:        Certification and careers
credential:    suite
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    2,493
hashtags:      n/a (own site)
ab_id:         AB-00171
---

# Project controls exam questions: 25 problems solved

Good project controls exam questions make you choose a method and defend it. Weak ones ask you to divide two numbers. Twenty-five problems follow, with full worked answers, covering earned value, forecasting, float, revenue recognition, cash, contingency and model metrics.

Work each one on paper before reading the answer, and write the sentence you would say in a review. That sentence is the part actually being examined.

These are practice problems written for this page. They are not drawn from any certification body's question bank, and nothing here implies how any examination is weighted.

## Project controls exam questions on earned value and variance

**Q1.** A control account has a budget at completion of £12.00m. At the cut-off, PV = £5.40m, EV = £4.86m and AC = £5.25m. Give the four core measures.

*Answer.* CV = 4.86 − 5.25 = **−£0.39m**. SV = 4.86 − 5.40 = **−£0.54m**. CPI = 4.86 / 5.25 = **0.926**. SPI = 4.86 / 5.40 = **0.900**. Behind and overspending together, which is the combination with no benign explanation.

**Q2.** For the same account, give percent complete and percent spent, and say what the gap means.

*Answer.* Percent complete = EV / BAC = 4.86 / 12.00 = **40.5%**. Percent spent = AC / BAC = 5.25 / 12.00 = **43.75%**. The 3.25-point gap is £0.39m of budget that bought no work. Percent spent is not progress, and reporting it as progress is the most common single error in cost reporting.

**Q3.** Give the efficiency required from here to finish on budget, and then to finish at an estimate of £12.96m.

*Answer.* TCPI to BAC = (12.00 − 4.86) / (12.00 − 5.25) = 7.14 / 6.75 = **1.058**. TCPI to EAC = 7.14 / (12.96 − 5.25) = 7.14 / 7.71 = **0.926**. The second equals CPI exactly, because £12.96m is BAC / CPI. Forecasting at BAC / CPI is arithmetically identical to assuming today's efficiency continues.

**Q4.** A pipeline package is budgeted at £1.50m for 15 km. Six kilometres are laid, four are tested, none commissioned. Compare a units-installed rule with a 60 / 30 / 10 rule for lay, test and commission.

*Answer.* Units installed: EV = 6/15 × 1.50 = **£0.60m**. Weighted rule: (0.400 × 0.60) + (0.267 × 0.30) = 0.240 + 0.080 = 0.320, so EV = 0.320 × 1.50 = **£0.48m**. The £120,000 difference is a policy choice, not progress. Publish the rule before the work starts.

**Q5.** A project holds £9.0m of discrete work and £1.5m of project management as level of effort. Discrete PV is £4.50m and discrete EV £3.60m; the level-of-effort account shows PV and EV both £0.60m. Give the honest schedule measure.

*Answer.* Blended SPI = (3.60 + 0.60) / (4.50 + 0.60) = 4.20 / 5.10 = **0.824**. Discrete SPI = 3.60 / 4.50 = **0.800**. Level of effort earns to plan by definition, so it can only flatter the result. Report discrete SPI and hold level of effort separately.

**Q6.** A project with BAC £18.00m, EV £6.30m and AC £7.20m receives an approved variation of £1.20m. What happens to CPI on approval?

*Answer.* Nothing. BAC becomes **£19.20m**, but EV and AC are both historic, so CPI stays 6.30 / 7.20 = **0.875**. The error to avoid is adding £1.20m to earned value at approval: budget arrives with the variation, earned value arrives only when the work does.

## Section two: forecasting the outturn

**Q7.** For BAC £18.00m, EV £6.30m, AC £7.20m and PV £6.75m, run all four estimate at completion methods.

*Answer.* CPI = 6.30 / 7.20 = **0.875**; SPI = 6.30 / 6.75 = **0.933**.

| Method | Calculation | Result |
|---|---|---|
| Remaining work at budget | 7.20 + (18.00 − 6.30) | **£18.90m** |
| Performance continues | 18.00 / 0.875 | **£20.57m** |
| Cost and schedule pressure | 7.20 + 11.70 / (0.875 × 0.933) | **£21.53m** |
| Fresh bottom-up estimate to complete | 7.20 + 12.90 | **£20.10m** |

A spread of **£2.63m** from four correct calculations on identical inputs.

**Q8.** The variance is traced to a single re-tendered subcontract, now let at a fixed price with no further exposure. Which method, and why?

*Answer.* Remaining work at budget, **£18.90m**. That method assumes the overrun is closed, which is exactly what a let, fixed-price subcontract means. The defence sentence is the cause, not the formula: "the overrun was a one-off re-tender, the work is now fixed price, and the remaining scope is unaffected."

**Q9.** Using the performance-continues forecast, give variance at completion and express it as a percentage.

*Answer.* VAC = 18.00 − 20.57 = **−£2.57m**, which is **14.3%** over budget. State it both ways in a report: the absolute number sizes the problem for finance, the percentage sizes it for the account owner.

**Q10.** The contract price is £19.50m. What do the four forecasts do to the commercial position?

*Answer.* At £18.90m the contract earns **£0.60m**. At £20.10m it loses **£0.60m**. At £20.57m it loses **£1.07m**, and at £21.53m it loses **£2.03m**. Where a contract is expected to be loss-making overall, the whole expected loss is recognised as soon as it is expected rather than spread across remaining progress. The forecasting choice is therefore also a reporting event.

## Section three: schedule and critical path

**Q11.** A network runs A (6 days), then B (9) and C (4) in parallel, then D (7) after B and E (11) after C, then F (3) after both D and E. Run the forward and backward pass.

*Answer.*

| Activity | Duration | ES | EF | LS | LF | Total float | Free float |
|---|---:|---:|---:|---:|---:|---:|---:|
| A | 6 | 0 | 6 | 0 | 6 | 0 | 0 |
| B | 9 | 6 | 15 | 6 | 15 | 0 | 0 |
| C | 4 | 6 | 10 | 7 | 11 | 1 | 0 |
| D | 7 | 15 | 22 | 15 | 22 | 0 | 0 |
| E | 11 | 10 | 21 | 11 | 22 | 1 | 1 |
| F | 3 | 22 | 25 | 22 | 25 | 0 | 0 |

Duration is **25 days** and the critical path is A–B–D–F: 6 + 9 + 7 + 3 = 25.

**Q12.** C shows one day of total float and no free float. Explain the difference to a site manager.

*Answer.* Total float is how long an activity can slip before the project finish moves. Free float is how long it can slip before the next activity's early start moves. C can absorb one day without delaying the project, but it cannot absorb any without pushing E. Float belongs to the path, not to the activity holding it, which is why the first team to use it takes it from everybody behind them.

**Q13.** Path A–C–E–F totals 24 days against a 25-day critical path. Why is calling A–B–D–F "the" critical path misleading?

*Answer.* One day of separation is inside the uncertainty of any duration estimate on either path. Where two near-equal paths merge at F, both must finish for F to start, so the expected finish is later than either path predicts on its own. Report the near-critical paths alongside the critical one and test them, which is the purpose of [schedule risk analysis](https://projectcontrolsinstitute.org/schedule-risk-analysis).

**Q14.** An activity shows −4 days of total float. What does that mean and what should you not do?

*Answer.* A constraint or an imposed finish date is earlier than the logic can achieve, so the backward pass produces late dates before early dates. It is a statement of infeasibility, not a scheduling error. Do not shorten durations in the tool to clear it. Either the logic changes, the scope changes, or the date changes, and one of those three conversations has to happen.

## Section four: cost accounting and revenue

**Q15.** A package reports AC £5.40m and EV £5.55m. You then find £0.62m of work performed and not invoiced, £0.15m invoiced this period for materials delivered after cut-off, and £0.07m of prepaid insurance for next period. Correct the position.

*Answer.* Reported CPI = 5.55 / 5.40 = **1.028**. Corrected AC = 5.40 + 0.62 − 0.15 − 0.07 = **£5.80m**. Corrected CPI = 5.55 / 5.80 = **0.957**. The package crossed from under to over on cut-off discipline alone.

**Q16.** A contract is priced at £52.0m with forecast cost £45.0m. Costs incurred are £18.0m. Give progress, cumulative revenue and cumulative margin under a cost-to-cost input measure.

*Answer.* Progress = 18.0 / 45.0 = **40.00%**. Revenue = 0.40 × 52.0 = **£20.80m**. Margin = 20.80 − 18.00 = **£2.80m**. Cross-check: expected margin is 52.0 − 45.0 = £7.0m, and 40% of £7.0m is £2.80m. Agreement between the two routes is how you know the entry is right.

**Q17.** The forecast moves to £49.0m. Restate the position.

*Answer.* Progress = 18.0 / 49.0 = **36.73%**. Revenue = **£19.10m**. Margin = **£1.10m**, and 36.73% of the new £3.0m expected margin confirms it. So **£1.70m** of previously reported margin reverses in the period the forecast moved, with no cash movement and no new transaction.

**Q18.** The forecast moves again, to £54.0m against the same £52.0m price. What now?

*Answer.* The contract is expected to be loss-making by **£2.0m**, and the whole expected loss is recognised immediately rather than spread over remaining progress. The step from margin erosion to a provision is a cliff, not a slope, which is why forecasts approaching break-even deserve the most scrutiny.

**Q19.** Revenue recognised to date is £20.80m, certified and invoiced £17.90m, cash received £15.60m. Split the balance sheet position.

*Answer.* Contract asset = 20.80 − 17.90 = **£2.90m**, being work earned but not yet certified. Trade receivable = 17.90 − 15.60 = **£2.30m**, being certified work not yet paid. Both are recoverable, but only the second one has a payment date attached, and only the first one depends on somebody agreeing your measurement.

## Section five: cash and working capital

**Q20.** Days sales outstanding is 74, unbilled work in progress is 21 days, days payable outstanding is 46, and turnover is £62m. Give the cash conversion cycle and what it funds.

*Answer.* Cycle = 74 + 21 − 46 = **49 days**. One day of turnover = 62,000,000 / 365 = **£169,863**. The cycle therefore ties up 49 × 169,863 = **£8.32m** of working capital, funded by the business until the last invoice clears.

**Q21.** Certification lag falls by 11 days. Quantify the benefit and name it correctly.

*Answer.* 11 × 169,863 = **£1.87m** released. It is a one-off release of working capital, not profit, and it does not repeat next year. Describing it as a saving in a board paper is the fastest way to lose credibility with a finance director.

## Section six: risk and contingency

**Q22.** A project holds £2.20m of contingency against a £22.0m base estimate. It is 35% complete by earned value and £1.15m of contingency has been drawn. Forecast the final draw.

*Answer.* Drawdown = 1.15 / 2.20 = **52.3%** against 35% of the work. At the same rate, the final draw is 1.15 / 0.35 = **£3.29m**, which is **£1.09m** more than was provided. No new risk needs to appear for that shortfall to be real.

**Q23.** Three risks: £1.20m at 30%, £0.80m at 50%, £2.50m at 15%. Give the expected value and its limitation.

*Answer.* (1.20 × 0.30) + (0.80 × 0.50) + (2.50 × 0.15) = 0.36 + 0.40 + 0.375 = **£1.135m**. No single outcome produces £1.135m: each risk either happens at full value or does not happen at all. Expected value sizes a portfolio provision; it never sizes an individual exposure, and holding £1.135m against a £2.50m risk covers less than half of it.

## Section seven: governed AI metrics

**Q24.** An anomaly model flags 240 invoices for review. Ninety are genuine exceptions, and 60 genuine exceptions were not flagged. Give precision, recall and F1.

*Answer.* Precision = 90 / 240 = **0.375**. Recall = 90 / 150 = **0.600**. F1 = 2 × (0.375 × 0.600) / (0.375 + 0.600) = 0.450 / 0.975 = **0.462**. In practice: a reviewer works through 150 false alarms to find 90 real ones, and still misses 60.

**Q25.** The score threshold is raised. Flags fall to 100, of which 70 are genuine, and 80 genuine exceptions are missed. Which setting do you adopt?

*Answer.* Precision = 70 / 100 = **0.700**, recall = 70 / 150 = **0.467**, F1 = 0.653 / 1.167 = **0.560**. F1 improves from 0.462 to 0.560, but F1 does not decide it. The cost of a missed exception against the cost of review time decides it, and that trade-off is a governance decision with a named owner, recorded before the model runs, not a default left in a configuration file.

## How to mark yourself

Award the method before the arithmetic. A candidate who picks the right forecasting method and slips a decimal is closer to competent than one who computes four forecasts flawlessly and cannot say which to use.

Score three things per question: did you choose correctly, did you compute correctly, and can you state the cause in one sentence a project director would accept. Two out of three is a pass on the day and a gap to close before the examination.

Work each question again a week later. Recognition fades faster than method, and the second attempt is the one that tells you what you actually know.

## Frequently asked questions

**Are these real certification exam questions?**
No. They are practice problems written for this page in the style of a technical examination. PCI does not publish its question bank and nothing here indicates how any examination is weighted. Use them to find gaps, not to predict a paper.

**How many should I get right before booking an examination?**
Treat consistent method choice as the threshold rather than a score. If you can name the right approach on every question in two passes a week apart, and your errors are arithmetic rather than judgement, you are ready. If you are still guessing between forecasting methods, more practice will help more than an earlier booking.

**Should I memorise the formulas?**
Memorise the six core ones, being CV, SV, CPI, SPI, VAC and TCPI, because you will use them under time pressure. Do not stop there: a formula sheet tells you how to calculate an estimate at completion and never tells you which of the four to use, which is the part that is actually examined.

**Where can I get more practice?**
Work through the [earned value practice questions](https://projectcontrolsinstitute.org/earned-value-practice-questions) next, then structured practice on [Certuvo](https://projectcontrolsinstitute.org/certuvo), PCI's platform for examination practice and training. Doing questions from two sources is worth more than repeating one set until you remember the answers.

---

*Internal links: this set should link to [earned value practice questions](https://projectcontrolsinstitute.org/earned-value-practice-questions) with that anchor, to [schedule risk analysis](https://projectcontrolsinstitute.org/schedule-risk-analysis) with that anchor, and to [Certuvo](https://projectcontrolsinstitute.org/certuvo) with that anchor; the project controls certification pillar and the four EAC formulas piece should link back here with the anchor "25 worked project controls exam questions".*
