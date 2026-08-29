---
platform:      Own site — projectcontrolsinstitute.org
type:          template
title:         Earned value practice questions and how to use them
meta:          Ten earned value practice questions with full worked answers, a marking scheme that separates method from arithmetic, and a four-week study loop.
primary_kw:    earned value practice questions
secondary_kw:  EVM exam questions, CPI and SPI calculation, EAC practice problems, cut-off correction
pillar:        Certification and careers
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,899
hashtags:      n/a (own site)
ab_id:         AB-00070
---

# Earned value practice questions and how to use them

Useful earned value practice questions make you correct a cut-off, choose between forecasts and defend the one you picked. Weak ones ask you to put three numbers into CPI = EV / AC. Ten questions follow, with worked answers, a marking scheme and a four-week loop.

The set is free and needs nothing but paper. Work each question before reading the answer, and write the sentence you would say in a review, because that sentence is what is actually being examined.

## The formulas you need before you start

| Measure | Formula | Reads as |
|---|---|---|
| Cost variance | CV = EV − AC | Money over or under for the work done |
| Schedule variance | SV = EV − PV | Work ahead of or behind the plan, in money |
| Cost performance index | CPI = EV / AC | Budgeted work bought per pound spent |
| Schedule performance index | SPI = EV / PV | Share of planned work actually completed |
| Variance at completion | VAC = BAC − EAC | Expected over or underspend at the end |
| To-complete performance index | TCPI = (BAC − EV) / (BAC − AC) | Efficiency required from here to hit budget |

Four forecasting methods sit on top of these. EAC = AC + (BAC − EV) assumes the overrun was a closed one-off. EAC = BAC / CPI assumes performance to date continues. EAC = AC + (BAC − EV) / (CPI × SPI) assumes schedule pressure keeps damaging cost. EAC = AC + a fresh bottom-up estimate to complete assumes the remaining work is different in kind.

## Tier one: computation

**Question 1.** A control account has a budget at completion of £8.00m. At month nine, PV is £4.60m, EV is £4.10m and AC is £4.75m. Give CV, SV, CPI, SPI and the efficiency needed to still finish on budget.

*Answer.* CV = 4.10 − 4.75 = **−£0.65m**. SV = 4.10 − 4.60 = **−£0.50m**. CPI = 4.10 / 4.75 = **0.863**. SPI = 4.10 / 4.60 = **0.891**. TCPI = (8.00 − 4.10) / (8.00 − 4.75) = 3.90 / 3.25 = **1.200**. The account has run at 0.863 for nine months and must now run at 1.200, a 39% improvement, with no plan attached.

**Question 2.** The cost report shows AC £3.10m and EV £3.35m for the period. You then find £0.72m of subcontract work performed and not invoiced, a £0.18m invoice booked in the period for steel delivered after cut-off, and £0.06m of prepaid site insurance for next period. Correct the position.

*Answer.* Reported CPI = 3.35 / 3.10 = **1.081**. Corrected AC = 3.10 + 0.72 − 0.18 − 0.06 = **£3.58m**. Corrected CPI = 3.35 / 3.58 = **0.936**. The account crossed from apparently profitable to overspending on accrual discipline alone, without a single new transaction.

**Question 3.** A labour package budgeted 4,000 hours at £62 per hour. Work earned to date is 3,800 budgeted hours. Actual expenditure is 4,350 hours at £68 per hour. Split the cost variance into rate and efficiency.

*Answer.* EV = 3,800 × 62 = **£235,600**. AC = 4,350 × 68 = **£295,800**. CV = **−£60,200**. Rate variance = (62 − 68) × 4,350 = **−£26,100**. Efficiency variance = (3,800 − 4,350) × 62 = **−£34,100**. The two sum to −£60,200, which is the check. Roughly 57% of the loss is hours, not rates, so a rate renegotiation fixes the smaller half.

## Tier two: interpretation

**Question 4.** A £20m project planned for 24 months completes in month 30. At month 30, EV and PV are both £20m, so SPI is 1.00. Explain, and give the honest measure.

*Answer.* SPI is money against money, and planned value stops growing once the baseline runs out, so SPI always returns to 1.00 at completion however late the project is. Earned schedule reads the same data in time: the baseline first planned £20m at month 24, so ES = 24 and SPI(t) = 24 / 30 = **0.80**. The project delivered 80 pence of planned time per month elapsed.

**Question 5.** An account reports CPI 1.14 and SPI 0.76. Name the most likely cause and what you would check.

*Answer.* Work has not started rather than run efficiently. Underspend and underachievement together usually mean resources never arrived, so the money saved is unstarted scope, not productivity. Check whether the resource curve is behind the plan, whether completed work is sitting unclaimed because a rules-of-credit gate was missed, whether procurement slipped, and whether the baseline was time-phased against a start date that was never achievable.

**Question 6.** Cumulative CPI reads 0.960 at month four, 0.950 at month five and 0.940 at month six. Cumulative EV and AC are 6.00 / 6.25, 7.60 / 8.00 and 9.10 / 9.68. BAC is £20.0m. What is actually happening?

*Answer.* Period CPI for month five = (7.60 − 6.00) / (8.00 − 6.25) = 1.60 / 1.75 = **0.914**. For month six = (9.10 − 7.60) / (9.68 − 8.00) = 1.50 / 1.68 = **0.893**. Cumulative CPI is a weighted average that flatters recent months. Forecasting at the period rate gives 9.68 + 10.90 / 0.893 = **£21.89m**, against BAC / cumulative CPI of 20 / 0.940 = **£21.28m**. A £0.61m gap, hidden by an average.

## Tier three: forecasting

**Question 7.** BAC £40.0m, EV £18.0m, AC £20.0m, PV £19.0m. A bottom-up estimate to complete comes back at £23.60m. Produce all four forecasts and the TCPI.

*Answer.* CPI = 18 / 20 = **0.900**. SPI = 18 / 19 = **0.947**. Work remaining in budget = £22.0m.

| Method | Formula | EAC | VAC |
|---|---|---|---|
| Remaining at budget | 20 + 22 | **£42.00m** | −£2.00m |
| Remaining at current CPI | 40 / 0.900 | **£44.44m** | −£4.44m |
| Remaining at CPI × SPI | 20 + 22 / 0.853 | **£45.80m** | −£5.80m |
| Bottom-up ETC | 20 + 23.60 | **£43.60m** | −£3.60m |

TCPI to budget = 22 / (40 − 20) = **1.100**. Demonstrated performance is 0.900, so the budget needs a sustained 22% improvement. A £3.80m spread on identical data is produced entirely by the choice of method.

**Question 8.** The cause of the overrun is one mis-sequenced piling campaign, now finished. Which forecast do you report?

*Answer.* Method one, but only after testing it. TCPI against a £42.00m forecast is 22 / (42 − 20) = **1.000**, so reporting £42.00m claims the remainder will run exactly at budget having run at 0.900. That claim needs the piling cost isolated in the ledger, evidence the sequence is corrected, and a named owner. Without those three, method two is the defensible answer.

## Tier four: judgement

**Question 9.** Your threshold rule is a corrective plan when CPI falls outside 0.95 to 1.05 or CV exceeds ±£250k. Apply it.

| Account | BAC | EV | AC | CV | CPI | Reportable |
|---|---|---|---|---|---|---|
| CA-100 | 4.00 | 2.40 | 2.52 | −0.12 | 0.952 | No |
| CA-200 | 9.00 | 5.60 | 5.92 | −0.32 | 0.946 | Both breached |
| CA-300 | 12.00 | 7.80 | 8.09 | −0.29 | 0.964 | Value only |
| CA-400 | 1.20 | 0.62 | 0.68 | −0.06 | 0.912 | Percentage only |

*Answer.* A percentage-only rule misses CA-300, where £0.29m is at stake. A value-only rule misses CA-400, running at 0.912 on a small budget, which is usually where a systemic problem shows first because the sample is small enough to notice. Both tests are needed, and the small account is the early warning.

**Question 10.** A commercial manager asks you to move a completed £0.9m scope item, which cost £1.4m, out of the baseline and into a variation. Using question seven's figures, what happens?

*Answer.* Remaining baseline: BAC 39.1, EV 17.1, AC 18.6, so CPI = **0.919**. BAC / CPI = **£42.53m**, plus the £1.4m now sitting outside, giving £43.93m against £44.44m before. The reported outturn improved by £0.51m while nothing about the project changed. Whether the item is a variation is a contract question settled by the change record, and a genuine variation carries its own budget and earns its own value, so it never leaves the forecast.

## How earned value practice questions should be marked

| Band | What it looks like | Verdict |
|---|---|---|
| Arithmetic only | Correct numbers, no sentence | Not yet ready |
| Numbers plus a cause | Correct numbers, one plausible cause named | Working level |
| Numbers, cause, evidence | States what document would prove the cause | Reviewable |
| The above plus a decision | Names the forecast reported and what would change it | Defensible |

Mark the sentence, not the sum. In every review that matters, the arithmetic is assumed and the reasoning is attacked.

## A four-week loop

Week one, questions 1 to 3, then repeat them on your own control accounts until each takes under two minutes. Week two, questions 4 to 6, and trace one real cut-off from ledger to cost report.

Week three, questions 7 and 8, then produce four forecasts for three live packages and write a defending paragraph for each. Week four, questions 9 and 10, then set a threshold rule and write it into a procedure.

Give the paragraphs to someone who will argue with them. That step is the whole exercise; everything before it is preparation.

## Frequently asked questions

**Are these questions representative of a certification examination?**
They cover the same method, not a published blueprint. PCI does not release examination weightings, so treat any set claiming to mirror a paper with suspicion. What is public is the syllabus itself: the [PCL-AI Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) lists the domains and knowledge areas, and studying against that is more useful than guessing at question counts.

**How many practice questions should I work through?**
Fewer than most people think, done properly. Ten questions marked on reasoning beat two hundred marked on arithmetic. Once you can produce four forecasts and defend one in a paragraph, more repetition of the same computation adds nothing. Move to your own project data instead, where the numbers are messy and the cut-off is real.

**Should I use a spreadsheet?**
Not while practising. Hand calculation forces you to notice when EV and AC come from different dates, which is the error a spreadsheet quietly propagates. Use a spreadsheet once you can do twenty positions on paper without hesitating, because hesitation in a review reads as uncertainty about the method.

**What is the single most common wrong answer?**
Treating the ledger as actual cost. Question 2 exists for that reason. Accruals for work performed and not invoiced, and reversals for costs booked before the work happened, move CPI by more than most genuine performance changes, and they move it in the flattering direction by default.

**Do I need a course before attempting these?**
No. The measures are public and the arithmetic is short. What a course buys is someone marking your reasoning, which you can also get from a colleague who will argue. If you want the wider method first, start with [the earned value management pillar](https://projectcontrolsinstitute.org/earned-value-management).

---

*Internal links: this piece should link to [the earned value management pillar](https://projectcontrolsinstitute.org/earned-value-management) with that anchor, to [the four EAC formulas worked through](https://projectcontrolsinstitute.org/four-eac-formulas) with that anchor, and to [earned value reporting thresholds](https://projectcontrolsinstitute.org/earned-value-reporting-thresholds) with that anchor; the EVM training and certification pieces should link back here with the anchor "earned value practice questions".*
