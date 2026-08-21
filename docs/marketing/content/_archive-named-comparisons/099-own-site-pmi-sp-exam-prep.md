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
schema:        HowTo + FAQPage
word_count:    1,785
hashtags:      n/a (own site)
ab_id:         —
---

# PMI-SP exam prep: a realistic study plan for planners

PMI-SP exam prep works best as eighty to a hundred hours across twelve weeks, built around PMI's published exam content outline rather than a textbook. Spend the first third on hand calculations, the middle third on PMI's process vocabulary, and the last third on timed scenario practice. Reversing that order leaves the scenario practice until there is no time left to repair what it exposes.

> **Who publishes this page.** credentialfinder.org is published by Project Controls Institute
> Global, which awards the PCI credentials discussed below. It is not an independent comparison
> service. Figures for AACE, PMI and other bodies are taken from their own published pages and
> should be checked there before you decide anything, because they change and because we are not
> a neutral party about one of the entries.

PMI publishes the item count, the time limit, the domain weightings and the eligibility hours in its own exam content outline and handbook. Download both, free, and take those numbers from there rather than from a study forum.

## What the PMI-SP examines

The PMI Scheduling Professional certification covers developing, maintaining and controlling a schedule inside PMI's process framework, plus communicating schedule information to people who did not build it.

Two things follow from that, and they shape how you prepare. The examination is framework-led, so PMI's terms and process boundaries matter as much as the network arithmetic. And it is scenario-based, so items describe a situation and ask what you would do next.

If your working life is Primavera P6 on an EPC site, expect a vocabulary gap before you expect a technique gap. Whether the credential pays off in that market at all is the prior question, answered in [a candid look at who PMI-SP suits](https://credentialfinder.org/pmi-sp-worth-it).

## A twelve-week PMI-SP exam prep plan

About eight hours a week, front-loaded on arithmetic. If [the forward and backward pass by hand](https://projectcontrolsinstitute.org/critical-path-method) is not yet automatic, weeks one and two are the whole plan. Adjust the calendar, not the order.

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

That is 92 hours across twelve weeks — an average just under eight a week, with the load sitting in the first half. Track them honestly, because the plan that fails is the one where week three quietly becomes week seven.

## The arithmetic that fails candidates

Two calculations account for most of the avoidable losses. Neither is difficult and both are easy to get wrong at speed.

### Schedule compression, done for least cost

A programme has two paths. Path one runs A (10 days) then B (8 days) then C (12 days), for **30 days**. Path two runs D (15 days) then E (12 days), for **27 days**. The client wants **26 days**.

Cost slope is the price of buying one day: (crash cost − normal cost) ÷ (normal duration − crash duration).

| Activity | Path | Normal | Normal cost | Crash | Crash cost | Days available | Cost slope |
|---|---|---|---|---|---|---|---|
| A | one | 10 d | £40,000 | 7 d | £58,000 | 3 | **£6,000/day** |
| B | one | 8 d | £30,000 | 6 d | £39,000 | 2 | **£4,500/day** |
| C | one | 12 d | £52,000 | 9 d | £73,000 | 3 | **£7,000/day** |
| D | two | 15 d | £62,000 | 12 d | £78,500 | 3 | **£5,500/day** |
| E | two | 12 d | £45,000 | 10 d | £58,000 | 2 | **£6,500/day** |

Both paths belong in the table, because the moment they are equal in length you will be buying from both.

Buy the cheapest days on the critical path first. Crash B by 2 days at £4,500 = **£9,000**, and path one drops to 28 days. Crash A by 1 day at £6,000, and path one drops to 27 days.

Path two is now also at 27 days, so both are critical. Getting to 26 means buying a day on each path at once: one more day of A at **£6,000** plus the cheapest day on path two, which is D at **£5,500**, so that single day of programme costs **£11,500**.

Total = 9,000 on B + 12,000 on A + 5,500 on D = **£26,500** for four days. The first day cost £4,500 and the fourth cost £11,500, and being able to show that curve is what stops a client asking for a fifth.

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

The same tonnage on the same day is worth **£250,000** more or less depending on a rule written months earlier. That number leaves the programme, sets the cost performance index, and eventually influences what the finance function may recognise as revenue. Which credentials cover that join and which stop at the programme is set out in [the planner's credential shortlist](https://credentialfinder.org/best-certification-for-planning-engineers).

Which is a study problem as much as a syllabus one. Nothing in a twelve-week PMI-SP plan asks what that £250,000 does after it leaves the programme, and no scheduling paper will mark you on it.

The PCI AI Project Controls Leader (PCL-AI) takes the programme, the cost report and the reporting consequence as one subject, across 13 domains and 61 knowledge areas proportioned 40% finance and reporting, 40% project management and 20% governed AI. That is a different examination from the one this plan prepares you for, not a competing one — sit PMI-SP if PMI-SP is what your market names, and treat this as what to read next.

PCI is not accredited by ANAB, UKAS, IAS or any other ISO/IEC 17024 accreditation body and does not claim to be, and it claims no recognition, endorsement, affiliation or partnership with PMI.

## Frequently asked questions

**How long does PMI-SP preparation really take?**
Eighty to a hundred hours for a working planner, spread across about three months. Experienced schedulers often need less on technique and more on PMI's vocabulary, which is the reverse of what they expect. Book the examination when your timed practice is consistent, not when the calendar says week twelve.

**Do I need PMP before PMI-SP?**
No. PMI-SP has its own eligibility route based on scheduling experience and education, published in PMI's handbook. Holding PMP first makes the framework language familiar, which shortens the middle of the plan, but it is a convenience rather than a prerequisite.

**Is PMI-SP better than AACE PSP?**
Neither is better in the abstract, and [where the two scheduling papers actually differ](https://credentialfinder.org/pmi-sp-vs-aace-psp) matters more than which is harder. PMI-SP suits organisations standardised on PMI's framework; PSP suits engineering, construction and energy markets that name AACE credentials and tests production through practical exercises. Read six live job advertisements for the role you want and count which acronym appears.

**Can I prepare without a course?**
Yes, and many do. The free exam content outline, a current scheduling reference and disciplined hand practice cover the ground. A course helps most with pacing and with the framework vocabulary, so buy it for structure rather than for content you could read yourself.

**What do candidates most often under-prepare?**
Decisions, as against definitions. Reciting a float definition is quick and feels like progress; working a compression problem or a delay scenario against a clock is slow and is what the scenario items ask for. If your practice contains no timed scenarios, that is the gap to close first, whatever the rest of the plan says.

**Does the credential expire?**
It runs on a professional development cycle, so it lapses if you stop maintaining it. Budget the renewal cost and the development hours for as long as you expect to hold it, because over a career the cycle costs more than the original sitting.

---

*Linking note: one cross-estate link is in the body, to the hub's worked [forward and backward pass by hand](https://projectcontrolsinstitute.org/critical-path-method), placed at the start of the twelve-week plan, where the reader is told weeks one and two are the whole plan if that arithmetic is not automatic. Three same-domain links sit where the question is raised: whether PMI-SP suits the reader's market, after the note about a vocabulary gap; the planner shortlist where the piece shows what the scheduling paper leaves untested; and the PMI-SP versus PSP comparison in the FAQ that asks which is better. The earlier note also proposed the hub's total float page — a second link to the same domain, so it was dropped — and the PSP guide, which the PSP comparison link now covers more directly. Reciprocal link worth making: the PMI-SP versus PSP comparison should point back here once, for the reader who has already chosen PMI-SP.*
