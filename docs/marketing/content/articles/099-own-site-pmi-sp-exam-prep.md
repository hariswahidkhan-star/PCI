---
platform:      Own site — credentialfinder.org
type:          guide
title:         PMI-SP exam prep: a realistic study plan for planners
meta:          A twelve-week PMI-SP exam prep plan built around the published content outline, the hand calculations that fail candidates, and what to skip entirely.
primary_kw:    PMI-SP exam prep
secondary_kw:  PMI-SP certification, schedule compression, earned schedule, total float
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: credentialfinder.org
canonical:     original
schema:        HowTo
word_count:    1518
hashtags:      n/a (own site)
ab_id:         —
---

# PMI-SP exam prep: a realistic study plan for planners

PMI-SP exam prep works best as eighty to a hundred hours across twelve weeks, built around PMI's published exam content outline rather than a textbook. Spend the first third on hand calculations, the middle third on PMI's process vocabulary, and the last third on timed scenario practice. Candidates who reverse that order usually fail on the scenarios.

PMI publishes the item count, the time limit, the domain weightings and the eligibility hours in its own exam content outline and handbook. Download both, free, and take those numbers from there rather than from a study forum.

## What the PMI-SP examines

The PMI Scheduling Professional certification covers developing, maintaining and controlling a schedule inside PMI's process framework, plus communicating schedule information to people who did not build it.

Two things follow from that, and they shape how you prepare. The examination is framework-led, so PMI's terms and process boundaries matter as much as the network arithmetic. And it is scenario-based, so items describe a situation and ask what you would do next.

If your working life is Primavera P6 on an EPC site, expect a vocabulary gap before you expect a technique gap.

## A twelve-week PMI-SP exam prep plan

Eight hours a week, front-loaded on arithmetic. Adjust the calendar, not the order.

| Weeks | Focus | Hours | What you should be able to do at the end |
|---|---|---|---|
| 1–2 | Network arithmetic by hand | 16 | Forward and backward pass, total float, free float, on paper, no tool |
| 3–4 | Schedule development and estimating | 16 | Build a WBS-driven network, apply durations and leads or lags, and defend them |
| 5–6 | PMI's process framework and terminology | 16 | Map your own site process onto PMI's terms without arguing with them |
| 7 | Schedule compression and resource levelling | 8 | Crash a network for least cost and say what the levelling did to float |
| 8 | Schedule control, variance and forecasting | 8 | Status a schedule and explain a slip with numbers |
| 9 | Risk, reserves and probabilistic durations | 8 | Explain what a three-point estimate and a simulation do and do not tell you |
| 10 | Communication and reporting | 6 | Write a schedule narrative a commercial manager will read |
| 11–12 | Timed scenario practice and gap repair | 14 | Sit a full-length timed set at pace, then fix what broke |

That is 92 hours. Track them honestly, because the plan that fails is the one where week three quietly becomes week seven.

## The arithmetic that fails candidates

Two calculations account for most of the avoidable losses. Neither is difficult and both are easy to get wrong at speed.

### Schedule compression, done for least cost

A programme has two paths. Path one runs A (10 days) then B (8 days) then C (12 days), for **30 days**. Path two runs D (15 days) then E (12 days), for **27 days**. The client wants **26 days**.

Cost slope is the price of buying one day: (crash cost − normal cost) ÷ (normal duration − crash duration).

| Activity | Normal | Normal cost | Crash | Crash cost | Days available | Cost slope |
|---|---|---|---|---|---|---|
| A | 10 d | £40,000 | 7 d | £58,000 | 3 | **£6,000/day** |
| B | 8 d | £30,000 | 6 d | £39,000 | 2 | **£4,500/day** |
| C | 12 d | £52,000 | 9 d | £73,000 | 3 | **£7,000/day** |

Buy the cheapest days on the critical path first. Crash B by 2 days at £4,500 = **£9,000**, and path one drops to 28 days. Crash A by 1 day at £6,000, and path one drops to 27 days.

Path two is now also at 27 days, so both are critical. Getting to 26 means buying a day on both paths at once: one more day of A at **£6,000** plus the cheapest day on path two at **£5,500**, so that single day costs **£11,500**.

Total = 9,000 + 12,000 + 5,500 = **£26,500** for four days. The first day cost £4,500 and the fourth cost £11,500, and being able to show that curve is what stops a client asking for a fifth.

The wrong answer is instructive. Crashing C, the longest activity, buys three days for £21,000 and still misses the date, because the second path was never touched.

### Why SPI lies at the end of a late job

Schedule performance index is earned value divided by planned value, and both are money.

At completion, earned value equals the budget at completion, and so does planned value. So SV = EV − PV = **zero** and SPI = **1.0** on a job that finished three months late. The index recovers to perfect exactly when the schedule news is worst.

Earned schedule fixes it by converting to time. Take a planned duration of **18 months**. At month 14, earned value equals the level the baseline said would be reached at month **11.6**.

SPI(t) = ES ÷ AT = 11.6 ÷ 14 = **0.829**.
Independent estimate of completion in time = PD ÷ SPI(t) = 18 ÷ 0.829 = **21.7 months**.

That is a forecast a planner can defend in a review, from three numbers already in the report. Knowing when the conventional index stops working is worth more marks than knowing the formula.

## What to skip

Do not memorise process input and output lists as lists. Items ask what you would do in a situation, and a memorised list does not produce a decision.

Do not study software. The examination is tool-neutral, and P6 fluency will not earn a single mark, though it will make the scenarios feel familiar.

Do not chase every practice-question bank you can find. Thirty items re-derived from blank paper beats three hundred read once, and the second approach produces the specific illusion of readiness that fails people.

## How to read a scenario item

Find the role first. The item usually tells you whether you are the scheduler, the project manager or the sponsor, and the defensible action differs by role.

Find the stage next. Something being asked during planning has a different answer from the same thing asked during execution, and PMI's framing is consistent about this.

Then choose the option that is a process step rather than a rescue. Where two answers look right, the one that updates the schedule and informs stakeholders usually beats the one that fixes the problem heroically and tells nobody.

## Where the scheduling paper stops

PMI-SP examines the programme. It does not examine what the programme does to the cost report.

Progress measurement is the join. A steelwork package budgeted at **£6.5m** with 740 of 1,850 tonnes erected earns 740 ÷ 1,850 × 6.5 = **£2.60m** on a units-installed rule. On a 70/30 rule split between erection and bolt-out, with 500 tonnes bolted out, it earns (0.400 × 0.70) + (0.270 × 0.30) = 0.361, so 0.361 × 6.5 = **£2.35m**.

The same tonnage on the same day is worth **£250,000** more or less depending on a rule written months earlier. That number leaves the programme, sets the cost performance index, and eventually influences what the finance function may recognise as revenue.

A chartered accountant is examined on recognition and provisions but not on float. An engineer is examined on float but not on cut-off. PCI AI Project Controls Leader (PCL-AI) examines both sides in one syllabus across 13 domains and 61 knowledge areas, with every PCI Body of Knowledge holding the same proportions: 40% finance and reporting, 40% project management, 20% governed AI.

PCI is not accredited by ANAB, UKAS, IAS or any other ISO/IEC 17024 accreditation body and does not claim to be, and it claims no recognition, endorsement, affiliation or partnership with PMI.

## Frequently asked questions

**How long does PMI-SP preparation really take?**
Eighty to a hundred hours for a working planner, spread across about three months. Experienced schedulers often need less on technique and more on PMI's vocabulary, which is the reverse of what they expect. Book the examination when your timed practice is consistent, not when the calendar says week twelve.

**Do I need PMP before PMI-SP?**
No. PMI-SP has its own eligibility route based on scheduling experience and education, published in PMI's handbook. Holding PMP first makes the framework language familiar, which shortens the middle of the plan, but it is a convenience rather than a prerequisite.

**Is PMI-SP better than AACE PSP?**
Neither is better in the abstract. PMI-SP suits organisations standardised on PMI's framework; PSP suits engineering, construction and energy markets that name AACE credentials and tests production through practical exercises. Read six live job advertisements for the role you want and count which acronym appears.

**Can I prepare without a course?**
Yes, and many do. The free exam content outline, a current scheduling reference and disciplined hand practice cover the ground. A course helps most with pacing and with the framework vocabulary, so buy it for structure rather than for content you could read yourself.

**What is the most common reason for failing?**
Studying definitions instead of decisions. Candidates who can recite float definitions but have not worked a compression problem or a delay scenario under time pressure tend to lose exactly the scenario items the examination is built from.

**Does the credential expire?**
It runs on a professional development cycle, so it lapses if you stop maintaining it. Budget the renewal cost and the development hours for as long as you expect to hold it, because over a career the cycle costs more than the original sitting.

---

*Internal links: this page should link to [how the critical path is calculated](https://projectcontrolsinstitute.org/critical-path-method) with that anchor, to [what total float really means](https://projectcontrolsinstitute.org/total-float) with that anchor, to [best certification for planning engineers](https://credentialfinder.org/best-certification-for-planning-engineers) with that anchor, and to [the AACE PSP certification guide](https://credentialfinder.org/aace-psp-certification-guide) with that anchor; the PMI-SP versus PSP comparison should link back here with the anchor "a realistic PMI-SP study plan".*
