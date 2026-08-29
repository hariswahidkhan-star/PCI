---
platform:      Own site — projectcontrolsinstitute.org
type:          guide
title:         Certified planning engineer: what it actually proves
meta:          What being a certified planning engineer proves and what it cannot: the competence tiers, a worked SPI and earned schedule forecast, and the four EAC methods.
primary_kw:    certified planning engineer
secondary_kw:  schedule performance index, earned schedule, estimate at completion, issuer independence
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,672
hashtags:      n/a (own site)
ab_id:         —
---

# Certified planning engineer: what it actually proves

A certified planning engineer has been examined by an independent issuer against a published standard on a stated date. That proves knowledge and method. It does not prove that your last schedule was sound, that your durations came from evidence, or that anyone downstream trusted your dates. Those are portfolio questions.

The useful way to read any such credential is to ask which of three tiers the examination reached, and who decided the answer.

## What does a certified planning engineer have to prove?

Competence in this role stacks in three tiers, and most examinations stop at the second.

**Tier one, knowledge.** Definitions, formulas, the mechanics of the critical path method, the difference between total and free float. Testable by multiple choice, and the cheapest tier to examine.

**Tier two, method.** Building a network that carries analysis, applying an earning rule, statusing to a data date, running a delay analysis to a recognised technique. Testable by worked problem.

**Tier three, judgement.** Choosing between two defensible answers and being able to say why. Which forecasting method matches the cause of the variance. Whether a 20-day duration is a plan or a hope. What the slippage costs.

| Claim on a CV | What evidences it | Who can examine it | Where it breaks |
|---|---|---|---|
| "I know CPM" | Tier one examination | Any issuer | Passing says nothing about whether your networks are sound |
| "I can build and status a schedule" | Tier two examination, plus a live schedule | Most credible issuers | Tool fluency is often mistaken for this |
| "My forecast holds up" | Tier three examination, plus a track record | Few issuers reach here | Cannot be demonstrated in an hour without arithmetic |
| "I can price a delay" | Tier three, cost-integrated | Fewer still | Usually delegated to a quantity surveyor and never learned |
| "My last three programmes finished near the date I forecast" | Referees and a portfolio | Nobody but your employers | No certificate substitutes for it |

Read that table as the honest limit of certification. It buys a stranger's confidence and a structured syllabus. It does not buy the track record, and no issuer should imply that it does.

## The question that separates tier two from tier three

Here is the question, with the arithmetic. A programme has a budget at completion of £24.0m and a baseline duration of 300 days. At day 180 the planned value is £13.2m, the earned value is £11.4m and the actual cost is £12.6m.

The variances come first.

- Schedule variance = EV − PV = 11.4 − 13.2 = **−£1.8m**
- Cost variance = EV − AC = 11.4 − 12.6 = **−£1.2m**
- Schedule performance index = EV / PV = 11.4 / 13.2 = **0.864**
- Cost performance index = EV / AC = 11.4 / 12.6 = **0.905**

A tier two answer stops there and reports two red indices. A tier three answer converts them into a date and a number, and states the assumption behind each.

**The time forecast.** The simplest is duration divided by SPI: 300 / 0.864 = **347 days**, roughly 47 days late.

Earned schedule gives a second reading. The planned value curve passed £11.4m at day 158, so earned schedule is 158 against an actual time of 180. SPI(t) = 158 / 180 = **0.878**, and the forecast duration is 300 / 0.878 = **342 days**.

The two agree here, and they will not agree later. SPI is built from cost units, so it climbs back to exactly 1.0 at completion however late the project finishes, because earned value must eventually equal planned value. SPI(t) does not, which is why it stays informative in the final third when the cost-based index has stopped saying anything.

## The four EAC methods and what each assumes

Four ways to answer "what will it finish at", each resting on a different belief about the future. Same data as above: BAC £24.0m, EV £11.4m, AC £12.6m, CPI 0.905, SPI 0.864.

| Method | Formula | The assumption it makes | Result | Variance at completion |
|---|---|---|---:|---:|
| Remaining work at budget | AC + (BAC − EV) | The overrun was a one-off; the rest goes to plan | £25.20m | −£1.20m |
| Performance continues | BAC / CPI | To-date cost efficiency is the best predictor | £26.53m | −£2.53m |
| Cost and schedule pressure | AC + (BAC − EV) / (CPI × SPI) | Both problems persist and compound | £28.73m | −£4.73m |
| Bottom-up estimate to complete | AC + new ETC | The past no longer predicts; re-estimate the remainder | £26.50m | −£2.50m |

The arithmetic, so it can be checked. Method one: 12.6 + (24.0 − 11.4) = 12.6 + 12.6 = 25.20. Method two: 24.0 / 0.905 = 26.53. Method three: CPI × SPI = 0.905 × 0.864 = 0.781, and 12.6 + (12.6 / 0.781) = 12.6 + 16.13 = 28.73. Method four uses a fresh estimate to complete of £13.9m.

The spread is £3.53m on the same four inputs. Anyone can compute all four; the examinable skill is choosing.

Method one is defensible only when you can name the one-off and show it has closed. Method two is the default once a project is materially advanced, because cost performance tends to be stubborn; treat that as a working rule to test against your own portfolio rather than a published finding.

Method three is for a project under schedule pressure that is buying its way back with overtime and acceleration. Method four is the honest answer when scope has changed, and it is the most expensive to produce, which is why it is the one most often skipped.

One more figure closes the argument. The to-complete performance index is (BAC − EV) / (BAC − AC) = 12.6 / 11.4 = **1.105**. The remaining work must run 10.5% more efficiently than budget, on a project that has so far run 9.5% worse than budget.

Stating that gap out loud is what tier three sounds like. The methods are set out in full in [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas).

## How to test whether the issuer is independent

Anyone can print a certificate. Five questions separate a credential from a receipt.

**Who decided you passed?** If the organisation that trained you also marked you, the assessment is not independent. That is not a scandal, but it is a different product.

**Is the syllabus published in full, before you pay?** A body of knowledge you can read is a body of knowledge you can be examined against.

**Is there a pass standard, and is it fixed?** A stated pass mark that does not move to fit the cohort is the difference between an examination and an attendance record.

**Is there a maintenance requirement?** Currency matters in a field where the tooling changes every two years. A credential that never expires is a photograph, not a licence.

**Is there an appeals route, and a published policy?** Bodies that expect to be challenged write the process down.

Apply those five to every issuer, PCI included. Any body that objects to being asked has answered the question.

## Where PCI sits, and why the syllabus crosses over

PCI AI Project Controls Leader (PCL-AI) covers **13 domains and 61 knowledge areas**, with a Body of Knowledge weighted **40 / 40 / 20** across finance and reporting, project management and governed AI, and resting on **113 mandatory PCI Standards carrying 532 process requirements**.

The 40% on finance is deliberate, and the worked example above is the reason. The forecast that came out of the schedule position is the same number that drives cost-to-cost progress, and therefore reported revenue. A planner who produces a date and hands the money question to somebody else has produced half an answer.

That is the overlap PCI examines: the accountant who cannot read a float path and the planner who cannot price one are looking at the same project and missing the same problem. The wider scope is set out in [what project controls covers](https://projectcontrolsinstitute.org/what-is-project-controls), and the route comparison in [the planning engineer certification routes compared](https://projectcontrolsinstitute.org/planning-engineer-certification).

## Frequently asked questions

**Is "certified planning engineer" a protected title?**
No. Unlike chartered engineering titles in some jurisdictions, it is a description rather than a protected designation, and different issuers attach it to very different examinations. That is precisely why the issuer matters more than the phrase, and why the five independence questions above are worth asking before you pay anything.

**How long does it take to prepare?**
Budget by gap rather than by hours. A planner who statuses a live schedule monthly usually needs work on forecasting method and delay technique. Someone arriving from estimating or commercial usually needs work on logic, calendars and float. Find out which by attempting the arithmetic in this article without looking at the answers.

**Does the credential expire?**
It should. Any credential worth holding carries a maintenance cycle, because scheduling practice, tooling and the governance expected around automated forecasting all move. PCI credentials are maintained on a three-year continuing professional development cycle with a mandatory AI-currency element.

**Do employers actually check?**
The serious ones verify, and the number that do is rising as verification became a lookup rather than a phone call. That is the practical argument for choosing an issuer with a public verification route: an unverifiable claim on a CV is worth less than no claim at all when someone tries to check it.

**Will a certification raise my salary?**
Nobody can honestly promise that, and any issuer quoting an uplift figure should be asked for the sample. What a credential reliably changes is which conversations you get into: a shortlist you would otherwise miss, and a starting assumption of competence that you then have to justify. The salary follows the work, not the certificate.

---

*Internal links: this guide should link to [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) with that anchor, to [the planning engineer certification routes compared](https://projectcontrolsinstitute.org/planning-engineer-certification) with that anchor, and to [what project controls covers](https://projectcontrolsinstitute.org/what-is-project-controls) with that anchor; the earned value management pillar and the project scheduler certification piece should link back here with the anchor "what a certified planning engineer actually proves".*
