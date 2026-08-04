---
id: BPG-06
series: S09
series_name: Best Practice Guides
title: Progress measurement and rules of credit
subtitle: Choosing a technique determines how easy the number is to argue with
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 16
summary: >
  How physical progress is measured and credited: the rules-of-credit register as a controlled
  artefact, the six measurement techniques and what each one costs in objectivity, how to set milestone
  weights so they reflect budget rather than duration, what evidence has to exist before a credit is
  taken, and how packages roll up. Includes a worked incremental-milestone measurement of a piping
  package, the same package under mis-set weights, and the arithmetic of level-of-effort dilution.
linkedin:
  format: article
  hook: >
    "The spools are all up, so we're about eighty per cent." Under the signed rules of credit the same
    physical state earned 64.25 per cent — a gap of 212,625 on one package, and nobody was lying.
  tags: [ProjectControls, ProgressMeasurement, EarnedValue, CostEngineering, Construction]
  asset: checklist-pdf
gated: false
related: [BPG-02, BPG-04, BPG-05, BPG-08, TPL-05, TPL-07]
bok_domains: [5, 6, 10]
sources: []
placeholders: 0
---

# Progress measurement and rules of credit

> How physical progress becomes a number, and why the choice of technique decides how hard that number
> is to argue with.

**In one paragraph.** How physical progress is measured and credited: the rules-of-credit register as a
controlled artefact, the six measurement techniques and what each one costs in objectivity, how to set
milestone weights so they reflect budget rather than duration, what evidence has to exist before a
credit is taken, and how packages roll up. Includes a worked incremental-milestone measurement of a
piping package, the same package under mis-set weights, and the arithmetic of level-of-effort dilution.

**Who this is for.** Cost engineers and planners who collect progress; control account managers who
sign it; construction and engineering managers who are asked for a percentage every month and would
prefer the question to have a defined answer.

---

## 1. A percentage is a measurement, not an opinion

Ask five people what percentage complete a work package is and you will get five answers, all sincere,
spanning thirty points. This is not a failure of honesty. It is what happens when a question has no
defined method: each person answers a slightly different question — how much of the effort feels done,
how much of the duration has elapsed, how much of the money has been spent, how much of the quantity is
installed — and all four are legitimate answers to questions nobody asked.

Rules of credit remove the ambiguity by fixing, in advance and in writing, exactly what physical state
earns exactly what credit. After that, progress is read off a rule rather than judged, the monthly
conversation moves from *what percentage are you?* to *has this step been completed and is the evidence
there?*, and the number becomes something a control account manager can sign.

The consequence runs further than tidiness. Earned value, the cost performance index, the forecast at
completion and every recovery argument built on them all rest on this measurement. `BPG-08 — Earned
value in practice` covers what to do with the number; this guide covers whether the number means
anything before you do it.

## 2. The rules-of-credit register

The register is a controlled document listing, for every work package: the measurement technique, the
steps or units that earn credit, the weight attached to each, the evidence that closes each step, and
the person authorised to confirm it.

Three properties make it work.

**It is fixed before measurement begins.** A rule agreed after the first progress claim is not a rule;
it is a negotiation. If a rule must change — because it was genuinely wrong — the change applies to the
whole package retrospectively and is disclosed, because it will move earned value without anything
physical happening.

**It names evidence, not confirmation.** "Progress confirmed by site" is not evidence. "Weld map signed
by the quality assurance inspector, referencing the joint numbers" is. The difference is whether a
reviewer six months later can reconstruct why the credit was taken.

**It covers every package, including the awkward ones.** Packages with no discrete output are the ones
most often left out and most often abused. Give them a technique explicitly — usually level of effort —
and record their share of the baseline, so the dilution in §7 is visible rather than accidental.

`TPL-05 — Progress measurement and rules of credit sheet` provides the instrument.

## 3. The techniques

Six techniques cover almost all work. They differ along one axis that matters more than any other: how
much room they leave for judgement, and therefore how easy the resulting number is to move.

**Units completed.** Credit is the ratio of quantity installed to total quantity. Objective, verifiable,
and the natural choice for repetitive measurable work — metres of cable, cubic metres of concrete,
welds, drawings issued. Its weakness is that it treats all units as equal, which is wrong wherever the
units differ in difficulty; the fix is to weight by a proxy for effort such as diameter-inch or tonnage
rather than by count.

**Incremental milestone.** The package is divided into sequential steps, each carrying a share of the
value, and credit is taken as each step completes. Objective if the steps are physically observable and
the weights honest. This is the workhorse technique for anything with a defined production sequence,
and §9 works one through.

**Weighted milestone.** Similar, but the milestones need not be sequential and are typically fewer and
larger — suitable for engineering and procurement packages where the interim states are document
issues or contractual events rather than physical steps.

**Fixed formula (0/100, 50/50, 25/75).** A fixed split between start and completion, with no interim
judgement at all. Maximum objectivity, minimum resolution. Appropriate for short packages — as a rule
of thumb, those spanning no more than one or two reporting periods — where the cost of measuring
interim progress exceeds the value of knowing it. The 0/100 variant never over-credits, which is why it
suits packages where optimism is the main risk.

**Percent complete estimate.** A judgement of physical completion by a competent assessor. The most
flexible and the easiest to move, and therefore the technique that requires the most support: it should
be used only where the assessor's judgement is anchored to something — a quantity take-off, a document
count, a defined set of completion criteria — and never as the default because the package was hard to
decompose.

**Level of effort and apportioned effort.** Two rules for work with no output of its own. **Level of
effort** earns by the calendar: earned value is set equal to planned value as time passes, so the
package can never show a schedule variance. Site supervision, project management and the controls
function itself are the usual examples. **Apportioned effort** earns in proportion to another package's
progress — quality inspection credited pro rata with the fabrication it inspects — which is the better
choice whenever the support work genuinely tracks a measurable base, because unlike level of effort it
can show a variance.

## 4. Choosing the technique: the gaming test

The choice is usually presented as a trade between accuracy and effort. The more useful frame is: **how
much would the reported number move if the person reporting it were under pressure to move it?**

Run the test explicitly when the register is written. For each package, ask what the maximum defensible
overstatement is under the proposed rule, and what evidence would be needed to sustain it.

Under units completed, overstatement requires claiming quantities that are not installed, which a
physical count disproves. Under incremental milestone, it requires claiming a step that has not been
completed, which the named evidence disproves. Under fixed formula, there is almost nothing to
overstate. Under percent complete estimate, overstatement requires only a different opinion — and an
opinion is not disprovable, which is why the technique needs an anchor.

Two practical rules follow. **Prefer the most objective technique the work will support**, not the most
precise. And **where percent complete estimate is unavoidable, cap its share**: record what proportion
of the baseline is measured by judgement, and treat a high figure as a control weakness in its own
right, because it is the proportion of your reported progress that cannot be independently checked.

## 5. Setting the weights

Incremental and weighted milestone techniques are only as good as their weights, and the most common
error is to set weights by **duration** rather than by **budget**.

The reasoning behind the error is intuitive: welding takes the longest, so welding should be worth the
most. But earned value is a budget measure — the credit represents the budget of the work performed —
so a step's weight should be the share of the package's budget that step consumes. Where duration and
cost profiles differ, weighting by duration systematically misstates progress, and §9.3 shows the size
of the effect.

Three disciplines keep weights honest. Derive them from the estimate build-up rather than from
opinion, so they can be traced. Have them set by someone other than the person who will report against
them. And keep the front-loading modest: a first step worth 25 % for "materials received" credits a
quarter of the value for work that is procurement, not production, and it is the single easiest way to
make a package look healthy early.

## 6. Evidence and who signs

A credit that nobody signed is a credit nobody owns. The register should name, per step, both the
evidence and the role authorised to confirm it — and the two should not be the same person as the one
claiming the progress.

The evidence should already exist for another reason. Quality records, inspection releases, delivery
notes, document transmittals, survey reports and test certificates are all produced anyway, and using
them as progress evidence costs nothing extra while making the progress claim auditable. Evidence
invented for progress measurement alone tends to become a form-filling exercise that certifies itself.

One rule is worth stating flatly: **credit is not acceptance**. A package credited 100 % under its
rules of credit has completed the physical work its rule describes. It has not necessarily been
accepted by the client, and the two states must be tracked separately — a spool credited on
installation that later fails a pressure test has not become 90 % complete; the rework is either scope
that was always required and is now late, or it is a change. `BPG-04 — Baselining and baseline change
control` covers which.

## 7. Rolling up, and the level-of-effort problem

Progress rolls up as a **budget-weighted average**, never as a simple mean of percentages. Averaging
percentages across packages of different sizes produces a number that belongs to no one and is wrong in
a direction nobody can predict.

The level-of-effort problem is the roll-up's characteristic distortion. Because level-of-effort packages
earn by the calendar, their earned value equals their planned value by construction and their schedule
performance index is 1.000 whatever is happening. Aggregate them with discrete work and they pull the
combined index towards 1.000 in proportion to their share. §9.4 quantifies this.

Two responses. **Segregate** level-of-effort packages into their own control accounts so the discrete
performance can be read on its own. And **report the share**: state, every period, what proportion of
the measured baseline earns by calendar. A reader who does not know that figure cannot interpret the
index they are being shown.

## 8. How this goes wrong

**The rule is chosen after the first claim.** The foreman says 80 %, the cost engineer needs a rule
that produces something defensible, and the rule that gets written is the one closest to 80 %. Every
subsequent period inherits the precedent.

**Percent complete estimate becomes the default.** It is used for packages that could have been
measured by units or milestones, because decomposing them was work. The proportion of the baseline
measured by judgement climbs quietly past half, and no index computed from it can be independently
checked.

**Weights are set by the person who reports against them.** Not usually dishonestly — but a production
manager who believes the hard part is welding will weight welding heavily, and the schedule will then
show rapid progress through the phase they consider difficult.

**Credit is taken on delivery to site.** Materials arriving earns a large share of a package's value.
Progress looks strong for two months and then flattens completely during installation, and the flat
period is read as a productivity problem when it is a measurement artefact.

**Level of effort is used because the work was hard to define.** Packages with genuine discrete output
are put on level of effort to avoid the measurement work. They then cannot show a variance, and the
project's schedule performance index becomes progressively less informative as their share grows.

**The 90 % plateau.** Packages reach 90 % quickly and stay there for months. This is the signature of a
percent-complete rule with no defined final step: the last 10 % has no evidence attached, so nobody can
say when it is earned. Every rule needs a defined closing condition.

**Progress and acceptance are conflated.** A package credited complete is reported as handed over. When
the client's punch list arrives, the project has no budget left for work it has already claimed as
earned, and the argument is about the measurement rather than about the work.

## 9. Worked example

*Illustrative figures.* One work package at one data date. Quantities are in diameter-inch (a
conventional measure of pipework quantity that weights each joint by pipe diameter, so that a large
joint counts for more than a small one). Currency in generic units. Percentages are stated against the
package total. No real project is implied.

### 9.1 The package and its rules of credit

Work package: process pipework installation, one area. Budget **1,350,000**. Total quantity **5,400**
diameter-inch. Technique: incremental milestone, weighted by budget share.

```
Value per diameter-inch = 1,350,000 ÷ 5,400 = 250.00
```

| Step | Weight | Evidence that closes it |
|---|---:|---|
| Material received and verified | 10 % | Goods receipt plus material certificate check, signed by materials control |
| Spool set and aligned | 30 % | Survey confirmation of position and alignment, signed by the site engineer |
| Welded out | 35 % | Weld map signed by the welding supervisor, joints listed |
| Non-destructive testing accepted | 15 % | Test report accepted by quality assurance |
| Supports, insulation and punch-list clear | 10 % | Completion certificate signed by the area superintendent |
| **Total** | **100 %** | |

Check: 10 + 30 + 35 + 15 + 10 = 100.

### 9.2 The measurement at the data date

| Step | Weight | Quantity reaching this step (dia-in) | Earned dia-in |
|---|---:|---:|---:|
| Material received and verified | 0.10 | 5,400 | 540.0 |
| Spool set and aligned | 0.30 | 4,100 | 1,230.0 |
| Welded out | 0.35 | 3,250 | 1,137.5 |
| Non-destructive testing accepted | 0.15 | 2,480 | 372.0 |
| Supports, insulation and punch-list clear | 0.10 | 1,900 | 190.0 |
| **Total earned** | | | **3,469.5** |

Checks: 5,400 × 0.10 = 540.0; 4,100 × 0.30 = 1,230.0; 3,250 × 0.35 = 1,137.5; 2,480 × 0.15 = 372.0;
1,900 × 0.10 = 190.0. Sum = 540.0 + 1,230.0 + 1,137.5 + 372.0 + 190.0 = 3,469.5.

```
Physical percent complete = 3,469.5 ÷ 5,400 = 0.6425 = 64.25 %
Earned value = 0.6425 × 1,350,000 = 867,375
       or    = 3,469.5 × 250.00 = 867,375   (the same number by the other route)
```

### 9.3 What the same package looks like without the register

**The claim.** The area superintendent reports "about 80 % — the spools are all up." The statement is
sincere and partly true: setting is the visible milestone, and 4,100 ÷ 5,400 = **75.9 %** of the
quantity has been set. But under the register, material and setting together are worth at most
10 + 30 = 40 % of the package, and only some of the welding, testing and completion has been done.

```
Credit under the claim   = 0.80 × 1,350,000 = 1,080,000
Credit under the register = 867,375
Difference                = 1,080,000 − 867,375 = 212,625
As a share of the package = 212,625 ÷ 1,350,000 = 15.75 %
```

Nobody has lied. The claim answers "how far through the visible sequence are we?"; the register answers
"what share of this package's budget has been earned?". Only the second belongs in a cost report.

**Weights set by duration instead of budget.** Suppose the weights had been assigned by how long each
step takes — welding is the longest activity, testing the shortest — giving material 5 %, setting 25 %,
welding 50 %, testing 5 %, completion 15 % (sum = 100 %). The same physical state then earns:

```
5,400 × 0.05 = 270.0
4,100 × 0.25 = 1,025.0
3,250 × 0.50 = 1,625.0
2,480 × 0.05 = 124.0
1,900 × 0.15 = 285.0
Total        = 3,329.0 dia-inch equivalent
Percent      = 3,329.0 ÷ 5,400 = 61.65 %
Earned value = 3,329.0 × 250.00 = 832,250
```

Check: 270.0 + 1,025.0 + 1,625.0 + 124.0 + 285.0 = 3,329.0.

The difference from the budget-weighted register is 867,375 − 832,250 = **35,125**, or 2.6 % of the
package budget. The direction of the error depends on the actual profile and would reverse at a
different point in the sequence — which is exactly why weights must be derived from the estimate
build-up rather than argued each period.

### 9.4 Level-of-effort dilution

**Assumptions.** A control account with planned value at the data date of 2,350,000, of which 350,000
is a level-of-effort package (site supervision). The discrete work has earned 1,640,000 against a
planned value of 2,000,000. The level-of-effort package earns its planned value by construction.

```
Discrete SPI  = 1,640,000 ÷ 2,000,000 = 0.820
Blended SPI   = (1,640,000 + 350,000) ÷ (2,000,000 + 350,000)
              = 1,990,000 ÷ 2,350,000
              = 0.847
Level-of-effort share of planned value = 350,000 ÷ 2,350,000 = 14.9 %
```

The reported index is 0.847 while the work that can actually be late is running at 0.820 — 2.7 index
points of pure arithmetic, produced by 14.9 % of the baseline that cannot be late by construction. The
larger the level-of-effort share, the closer the reported index sits to 1.000 regardless of
performance. This is the reason to segregate, and the reason to publish the share every period.

## 10. Checklist

Take this into the meeting where progress is agreed, or into the review of a rules-of-credit register.

**The register**

- [ ] Does every work package have a technique recorded, including the awkward ones?
- [ ] Was every rule fixed before the first claim against it?
- [ ] Has any rule changed mid-package, and if so was the change applied retrospectively and disclosed?
- [ ] Does every step name evidence a reviewer could find six months later — a document, not a confirmation?
- [ ] Does every step name the role that confirms it, and is that role independent of the claimant?

**Technique choice**

- [ ] What proportion of the baseline is measured by percent complete estimate?
- [ ] For each such package, what is it anchored to, and could a more objective technique have been used?
- [ ] Is anything on level of effort that has a genuine discrete output?
- [ ] Where support work tracks a measurable base, has apportioned effort been considered instead?
- [ ] Does every rule have a defined closing condition, or does the package stop at 90 %?

**Weights**

- [ ] Were milestone weights derived from the estimate build-up, and can that derivation be shown?
- [ ] Were they set by someone other than the person who reports against them?
- [ ] How much value is earned before production starts — receipt, mobilisation, procurement?

**Roll-up and reporting**

- [ ] Is progress rolled up budget-weighted, not as a mean of percentages?
- [ ] Is the level-of-effort share of the measured baseline stated in the report every period?
- [ ] Are level-of-effort packages segregated so discrete performance can be read alone?
- [ ] Are credit and client acceptance tracked as separate states?

**Sanity**

- [ ] If a claim exceeds what the register can support, has the register been checked before the claim is disputed?
- [ ] Which packages have not moved for two periods, and is that production or measurement?

---

## Related

- `BPG-02 — The work breakdown structure` — the work packages these rules are written against, and how deep to decompose.
- `BPG-04 — Baselining and baseline change control` — what to do when a rule was wrong, and why rework is a scope question.
- `BPG-05 — Schedule quality — a practical review` — the duration checks that determine which packages need interim milestones.
- `BPG-08 — Earned value in practice` — what happens to the number this guide produces.
- `TPL-05 — Progress measurement and rules of credit sheet` — the register, with every field defined.
- `TPL-07 — Earned value calculation sheet` — the roll-up, with formulas stated and verified.

## Sources and standards

Drawn from the Institute's Body of Knowledge: Domain 6 (Earned Value Management and Forecasting) for
earning rules and the distinction between physical and cost percent complete, Domain 5 (Cost Management
and Cost Control) for control accounts and work packages, and Domain 10 (Project Scheduling) for
progress updating and duration.

The measurement techniques described here are long-established practice across the profession and are
explained in the Institute's own words. The gaming test in §4, the budget-versus-duration weighting rule
in §5 and the recommendation to publish the level-of-effort share every period are PCI recommended
practice. No external standard, table or scoring scheme is reproduced.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
