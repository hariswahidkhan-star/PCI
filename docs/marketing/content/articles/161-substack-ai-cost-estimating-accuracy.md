---
platform:      Substack
type:          faq
title:         AI cost estimating accuracy: how to test the claim
meta:          AI cost estimating accuracy is a property of a test, not a product. How to back-test a model on your own completed jobs, with the arithmetic shown.
primary_kw:    AI cost estimating accuracy
secondary_kw:  mean absolute percentage error, estimate bias, back-testing, cost-to-cost input method
pillar:        AI in project controls
credential:    PFL-AI
target_domain: pciai.org
canonical:     original
schema:        FAQPage
word_count:    1565
hashtags:      n/a (Substack — no hashtags)
ab_id:         AB-00284
---

# AI cost estimating accuracy: how to test the claim

AI cost estimating accuracy is a property of a test, not of a product. A model is as accurate as the resemblance between the job in front of you and the jobs it learned from. The only figure worth quoting is the one measured on your own completed estimates: mean absolute percentage error, with the bias sitting underneath it.

*Written first for this newsletter, on numbers that appear nowhere else. Every figure below is illustrative and exists to show the method. Run it against your own last ten jobs and you will have something a vendor cannot give you.*

## What does AI cost estimating accuracy actually mean?

Two different things, and they are usually conflated. Error size is how far a forecast lands from the outturn. Bias is whether the misses fall on the same side.

A model with a mean absolute percentage error of 6% and no bias is useful. A model with the same 6% error that is low every single time is not a model, it is a discount, and someone will price against it.

There is a third property nobody asks about: stability. An error of 6% that becomes 19% on a building type you have not priced before tells you the model has memorised rather than generalised.

## How do you back-test a model on your own data?

Take the completed projects you have final accounts for. Hold each one out of the training set in turn, ask the model to estimate it from the information you actually had at the estimate date, then compare with the outturn.

Three conditions decide whether the test means anything. The model must not see the outturn, and the information it is given must match the definition maturity of the estimate date.

The third is normalisation. Every figure on both sides has to sit at one price base, and this is where most in-house tests quietly fail.

Comparing a 2022 estimate with a 2025 final account measures inflation as well as the model, and inflation usually wins.

## Worked example: six completed jobs

Six projects, all figures in millions and all illustrative. Estimate is the model's output at the same definition maturity; final is the outturn.

| Project | Model estimate | Final cost | Error | Absolute % error |
|---|---:|---:|---:|---:|
| Warehouse fit-out | 4.10 | 4.45 | −0.35 | 7.87% |
| Substation | 11.80 | 13.90 | −2.10 | 15.11% |
| Primary school | 8.60 | 8.35 | +0.25 | 2.99% |
| Water pumping station | 6.30 | 7.10 | −0.80 | 11.27% |
| Office Cat B fit-out | 3.20 | 3.05 | +0.15 | 4.92% |
| Process tie-in | 9.40 | 11.60 | −2.20 | 18.97% |

Mean absolute percentage error is the average of the last column: (7.87 + 15.11 + 2.99 + 11.27 + 4.92 + 18.97) ÷ 6 = **10.19%**.

Mean percentage error keeps the sign: (−7.87 − 15.11 + 2.99 − 11.27 + 4.92 − 18.97) ÷ 6 = **−7.55%**.

The gap between 10.19% and −7.55% is the finding. Roughly three quarters of the error is one-directional, so this model does not scatter around the answer, it sits below it.

Check the portfolio too. Total estimated 43.40 against total outturn 48.45 is an aggregate shortfall of **10.42%**, which is what a business plan built on these estimates would have been out by.

## Why the mix matters more than the model

Split the same six by type and the single headline number falls apart.

| Group | Projects | MAPE | Mean % error |
|---|---|---:|---:|
| Repeat building types | Warehouse, school, office fit-out | 5.26% | +0.02% |
| Engineered and process work | Substation, pumping station, tie-in | 15.11% | −15.11% |

On repeatable buildings the model is close and unbiased. On engineered work it is out by a sixth, and always low. A vendor quoting 10.19% across the set has told you nothing about which half of your pipeline you can trust it on.

This is the whole argument for holding out a test set that matches your actual work mix. A model trained mostly on commercial fit-out will report a flattering average and fail on the plant you are about to sanction.

Report accuracy by segment or do not report it. One number over a mixed portfolio is an average of two different tools.

## What to ask a vendor, and what an answer looks like

| The claim | The question that tests it | A usable answer |
|---|---|---|
| "95% accurate" | Accurate against what, measured how? | MAPE on a named held-out set, with the count of projects |
| "Trained on thousands of projects" | How many resemble ours, by type and size? | A segment breakdown, not a total |
| "Learns from your data" | How many of our projects before it beats our estimator? | A stated minimum, and what happens below it |
| "Reduces estimating time by half" | Time to first draft, or time to signed estimate? | The second one, including review |
| "Continuously improving" | What is the error trend, and on which set? | A dated series on a fixed test set |

If the answer to the first row is a demonstration rather than a number, you have a sales meeting rather than a measurement.

Write whichever figures you obtain into the estimate basis document, alongside the model version and the test date. An estimate that cannot say where its numbers came from is not auditable, whatever produced it.

## Judging a flagging tool by different numbers

Some tools do not price. They read a draft estimate and flag probable omissions, and those are scored as a classifier, not as a forecast.

On one review the tool raises 40 flags. The estimator agrees with 17, dismisses 23, and separately finds 5 genuine omissions the tool missed.

Precision = 17 ÷ 40 = **0.425**. Recall = 17 ÷ 22 = **0.773**. F1, the harmonic mean, = 2 × (0.425 × 0.773) ÷ (0.425 + 0.773) = 0.657 ÷ 1.198 = **0.548**.

Precision below a half sounds like failure and is not, because the cost of a false flag is two minutes of an estimator's time and the cost of a missed omission is a variation. For omission-hunting, buy recall and pay for it in review time. For anything that writes a rate into the estimate, the trade runs the other way.

## Where the estimate error lands in the accounts

An estimate is not only a bid. Where progress is measured by a cost-based input method, the total expected cost is the denominator of percentage complete, so a model that runs 15% low on engineered work overstates progress from the first month.

Overstated progress means revenue recognised early, and a catch-up reversal in the period the forecast is corrected. The estimating error becomes a reporting error without anyone touching the ledger, which is [how a cost forecast turns into reported profit](https://projectcontrolsinstitute.org/eac-accounting) whether or not anyone intended it to.

This is the overlap PCI was built for. A chartered accountant is examined on when revenue may be recognised and what a provision must satisfy, almost never on how an estimate was produced. An engineer is examined on quantities and rates, almost never on cut-off.

The money is lost between the two, and a model that is quietly biased low is one of the cleaner ways to lose it.

The calculation worked examples across the PCI AI Project Finance Leader (PFL-AI) and PCI Project Management Leader – AI (PML-AI) Bodies of Knowledge are verified by a machine suite of 15,613 calculation checks, all currently passing. The PCI AI Project Controls Leader (PCL-AI) volume has no equivalent suite.

## Frequently asked questions

**How many completed projects do I need for a fair test?**
Enough to have several in each segment you actually bid, which in practice means a minimum of five or six per type rather than a large total. Twelve projects spread across four types gives you three per type and an error bar too wide to act on. Test the segments you win work in and leave the rest untested rather than pretending.

**Is a model more accurate than an experienced estimator?**
On repeat work with good historical data, often yes, and faster. On first-of-a-kind work it is worse, because it has no case to reason from and will still return a confident number, which is the split that runs through [how estimators are actually using models today](https://pciai.org/ai-for-cost-estimating-in-construction). The productive arrangement is the model producing the first pass and a check against comparables, and the estimator owning the judgement about whether this job resembles the history at all.

**Does a wide accuracy range mean the estimate is bad?**
No. Early estimates are prepared at low definition and carry a wide expected range by design; that is what the estimate class system exists to communicate. A narrow range quoted at concept stage is the warning sign, not a wide one. What matters is that the stated range matches the definition maturity and that the model was tested at that same maturity.

**Can a model estimate a project type it has never seen?**
It will produce a number, which is the problem. Without comparable history it interpolates from whatever it has, and the error is unbounded rather than merely large. Treat an out-of-distribution job as an estimator-led exercise with the model used only for cross-checks on the parts that are conventional.

**Where should the accuracy figure be recorded?**
In the basis of estimate, with the model version, the test set, the date and the segment MAPE. It belongs beside the other assumptions a reviewer is entitled to challenge. A tool described only as AI-assisted, with no figure attached, cannot be relied on in a sanction paper and should not survive a review.

---

*Written newsletter-first for Substack as an original. Substack sets no canonical, so nothing here is a copy of a page on the PCI site.*

*Linking note: two links are now in the body, one per domain. "How a cost forecast turns into reported profit" sits where the estimating error becomes a reporting error (https://projectcontrolsinstitute.org/eac-accounting), because that sentence raises the question of how the forecast reaches the accounts at all. "How estimators are actually using models today" sits in the FAQ comparing a model with an experienced estimator (https://pciai.org/ai-for-cost-estimating-in-construction). The AI in project controls pillar was dropped, because two links to pciai.org from one piece is the tell the architecture warns about, and the cost estimating page is the closer answer. The hub link was retargeted from IFRS 15 to EAC accounting, which is what the sentence actually asks. Reciprocal: the pciai.org cost estimating page has a real reason to cite the back-testing method here, and that is the link back worth making.*
