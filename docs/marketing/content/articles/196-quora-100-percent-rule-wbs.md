---
platform:      Quora
type:          qa-list
title:         What is the 100% rule in a work breakdown structure?
meta:          The 100% rule WBS decomposition must satisfy: children sum to their parent, no gaps, no overlaps. Worked arithmetic on missing and double-counted scope.
primary_kw:    100% rule WBS
secondary_kw:  work breakdown structure, scope baseline, work package, control account
pillar:        Planning and scheduling
credential:    PML-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        FAQPage
word_count:    1452
hashtags:      n/a (Quora)
ab_id:         AB-00269
---

# What is the 100% rule in a work breakdown structure?

A work breakdown structure must contain 100% of the project's scope and nothing beyond it — that is the 100% rule WBS decomposition must satisfy. At every level, the child elements must add up to exactly their parent. Work that is not in the WBS is not in the project: it has no budget, no owner and no way to earn value.

The rule runs in two directions at once. Vertically, the split must be complete and non-overlapping. Horizontally, the top of the structure must equal the scope the project committed to deliver.

## What is the 100% rule WBS decomposition must satisfy?

It requires three things of every branch. The children must cover all of the parent's scope, none of them may duplicate another, and nothing may appear that the parent does not include.

That last clause is the one teams forget. Adding useful-looking work to a WBS is a 100% rule breach in the upward direction, and it is how gold plating gets a budget code.

The rule is about scope, not effort. A WBS element is a noun — a deliverable or a component of one — so "reinforced concrete bases" belongs and "pour concrete" does not. Verbs live in the schedule, one level below where the WBS stops.

## What happens when a WBS is missing scope?

Take a substation upgrade estimated at £12.00m. The WBS carries four level-two elements: civils £4.20m, electrical £5.10m, control systems £1.40m, and project management and controls £0.90m.

Those add to £11.60m. The WBS holds 11.60 ÷ 12.00 = 96.67% of the scope, and the missing £0.40m is site-wide temporary works that nobody claimed.

The work still gets done, and the cost still arrives. It lands against civils, because that is the nearest live cost code.

Now watch what it does to a branch that is performing perfectly. At month nine, civils has earned value of £2.10m against an actual cost of £2.10m, so CPI = 2.10 ÷ 2.10 = 1.000.

Add the stray £0.40m and the actual cost becomes £2.50m. CPI = 2.10 ÷ 2.50 = 0.840, and EAC = BAC ÷ CPI = 4.20 ÷ 0.840 = **£5.00m**.

A branch delivering exactly to budget now forecasts an £0.80m overrun. No one on the civils team can explain it, because the cause is not in their scope. It is in the gap. Run the same figures against [a full month of earned value worked end to end](https://projectcontrolsinstitute.org/earned-value-worked-example) and nothing in the method differs — only whether every cost had a budget line to land on.

## What happens when scope is counted twice?

Overlap is the same failure with the sign reversed. Suppose cable terminations at £0.25m appear under both electrical and control systems, because two estimators wrote the same line.

The project now holds £0.50m of budget for £0.25m of work. When the terminations are finished, both branches claim their £0.25m, so earned value is overstated by £0.25m and both CPI and SPI look better than the job is.

Overlaps are harder to find than gaps, because nothing is obviously wrong until the double-counted budget runs out with the work already done. A gap announces itself as a cost with no home; an overlap hides as good news.

## How do you test a WBS against the 100% rule?

| Direction | The test | Failure mode | What it looks like in the numbers |
|---|---|---|---|
| Downward | Do the children sum to the parent, exactly? | Gap | Actual cost with no matching budget; CPI falls on a branch that is performing |
| Downward | Does any scope appear under two parents? | Overlap | Earned value claimed twice; CPI and SPI both flattered |
| Upward | Does the top equal the committed scope? | Missing deliverable | A late surprise: work in the contract, absent from the baseline |
| Upward | Does anything appear that the contract does not require? | Gold plating | Budget consumed on scope nobody will pay for |
| Across | Does every element have one owner? | Orphan scope | Variance nobody signs for |

The fifth row is not part of the classical rule, but it is the one that makes the other four enforceable. Scope with no owner fails the rule quietly, every month, until closeout.

## Does the 100% rule cover project management effort?

Yes. Management, planning, cost control, quality assurance, commissioning support and the temporary works that enable the permanent works are all scope, and all of it belongs in the WBS.

Leaving them out is the most common single breach. They are real cost, they are usually level of effort rather than discrete work, and when they sit outside the structure their cost has to be absorbed by branches that did not budget for it.

Put them in one named element, budget them, and measure them by an appropriate method. A level-of-effort element earns value with time rather than with output, which is honest as long as it is labelled that way and kept small.

## Where does the 100% rule meet the accounts?

Where progress towards a performance obligation is measured by a cost-based input method, the measure is costs incurred divided by total expected costs. Total expected costs is the sum of the WBS branches, so a WBS that omits scope understates the denominator.

Run the substation numbers on a £14.00m contract price. With £4.00m of cost incurred and the incomplete £11.60m forecast, progress reads 4.00 ÷ 11.60 = 34.48%, and cumulative revenue is 34.48% × 14.00 = £4.828m.

With the true £12.00m, progress is 4.00 ÷ 12.00 = 33.33% and revenue is £4.667m. The gap in the WBS has pulled £0.161m of revenue into the wrong period, and every month it reverses later.

That is a scope-decomposition error arriving in the profit and loss account. It is the reason PCI examines finance and delivery in one credential: the PCI Project Management Leader – AI (PML-AI) covers 16 domains and 63 knowledge areas, with a Body of Knowledge weighted 40% finance and reporting, 40% project management and 20% governed AI. Treatment depends on the contract and the reporting framework applied, and nothing PCI publishes is accounting advice.

## Frequently asked questions

**Does the 100% rule apply to the schedule as well?**
Not in the same form. The schedule inherits the rule through the work packages: every activity must belong to exactly one work package, and every work package must be represented by activities. If a schedule contains activities that map to no WBS element, the project is doing work it has not budgeted, which is the same failure the 100% rule exists to catch.

**Is the 100% rule about deliverables or activities?**
Deliverables. The WBS decomposes what will be produced, and the schedule decomposes what will be done to produce it. A WBS written in verbs tends to break the rule quickly, because it is easy to list activities that leave a deliverable half-covered and hard to notice that you have.

**Where does contingency sit under the 100% rule?**
Contingency is not scope, so it does not decompose like scope. It is usually held as a distributed allowance within control accounts or as a separately identified element, and management reserve normally sits outside the performance measurement baseline entirely. What matters is that it is visible and that drawing on it is a recorded decision, not a silent adjustment to a work package.

**Does the 100% rule mean every branch has the same number of levels?**
No, and forcing symmetry is a common mistake. A £6m civils branch may need five levels before the work is estimable and measurable; a £0.3m training branch may need two. The rule constrains completeness at each split, not the depth of the tree.

**What about scope that is explicitly excluded from the contract?**
Exclusions belong in the scope statement and the WBS dictionary, not as WBS elements. Recording them matters, because an exclusion that is never written down reappears as an assumption during delivery, and by then someone has usually started the work.

**How do you check a WBS against the 100% rule quickly?**
Sum every level and compare it to its parent, then read the contract's deliverables list against the level-two elements one line at a time. Ten minutes of arithmetic and twenty minutes of reading finds most gaps. The overlaps take longer, and the fastest route to them is asking two branch owners who is doing a given interface.

---

*Internal links: one, in the body. [A full month of earned value worked end to end](https://projectcontrolsinstitute.org/earned-value-worked-example) sits with the stray-cost arithmetic, where a CPI of 0.840 on a branch that is performing raises the question of what the same calculation looks like when every cost has a budget line. The critical path page in the original note was dropped rather than placed: it would have been a second link to the same domain, and this answer is about scope coverage, not about logic and float — the schedule question is properly the subject of the WBS versus schedule answer, which is where that link belongs. No reciprocal link is proposed: Quora links are nofollow, so this earns qualified readers rather than equity.*
