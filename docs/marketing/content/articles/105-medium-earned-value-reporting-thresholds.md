---
platform:      Medium
type:          guide
title:         Earned value reporting thresholds that trigger action
meta:          Earned value reporting thresholds that catch real money: a two-gate design, phase settings, trend triggers, an escalation ladder and a cash gate.
primary_kw:    earned value reporting thresholds
secondary_kw:  variance analysis report, control account, cash conversion cycle, escalation
pillar:        Earned value management
credential:    PFL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /earned-value-reporting-thresholds (own site #025)
schema:        Article
word_count:    1734
hashtags:      #ProjectControls #EarnedValue #ProjectFinance #PMO #RiskManagement
ab_id:         AB-00214
---

# Earned value reporting thresholds that trigger action

Earned value reporting thresholds are the values at which a variance stops being noise and becomes something a named person must explain and act on. A workable set pairs a percentage gate with an absolute one, adds a higher absolute trigger that fires regardless of percentage, and gives each an owner.

Set them wrong in either direction and the reporting system stops working. Too loose and the money escapes; too tight and everyone learns to ignore the alerts.

## Why a single percentage gate fails

Take two control accounts on one project, at the same cut-off, under a rule that any variance beyond 10% must be explained.

**CA-A**, a small builder's-work account. EV £74k, AC £112k. CV = −£38k, so CV% = −38 ÷ 74 = **−51.4%**.

**CA-B**, the main mechanical package. EV £3,900k, AC £4,050k. CV = −£150k, so CV% = −150 ÷ 3,900 = **−3.85%**.

Under the percentage rule, CA-A produces a written variance analysis over £38k and CA-B produces nothing at all over £150k. The rule has inverted the priorities exactly.

Run that across forty control accounts and the monthly pack fills with explanations of small accounts while the largest money on the project passes unremarked.

## The two-gate design

The fix is two triggers rather than one, deliberately different in shape.

| Gate | Rule | Fires on these accounts | Who responds |
|---|---|---|---|
| Tier 1 — significance | Variance exceeds 10% **and** £50k | CA-A: no (£38k). CA-B: no (3.85%) | Control account manager writes the analysis |
| Tier 2 — materiality | Variance exceeds £100k, whatever the percentage | CA-A: no. CA-B: **yes** | Same analysis, plus project manager sign-off |
| Watchlist | Variance exceeds 25% on any account | CA-A: **yes** | Named and tracked, no written report required |

CA-B now reports, because £150k is real money on any project. CA-A lands on a watchlist, which costs a line rather than an afternoon, and if it repeats for three periods it escalates on trend instead of on size.

Scale the absolute figures to the project. A defensible default is 1.5% of project BAC or a fixed sum, whichever is smaller, so the same procedure works on a £4m fit-out and a £400m programme.

## Setting earned value reporting thresholds by phase

A threshold that is right in month twenty is wrong in month two, because early variances are computed on small denominators and swing violently.

| Phase | Percentage gate | Absolute gate | Reason |
|---|---|---|---|
| Under 15% complete | 20% | Full tier 2 value | Small samples throw large percentages that mean little |
| 15% to 80% complete | 10% | Standard | The stable window, where the numbers are most trustworthy |
| Over 80% complete | 10% on cost, trend only on schedule | Half the tier 2 value | SPI drifts to 1.00 near the end; remaining money is small but recoverable |

Write the phase boundaries into the cost control procedure at baseline. Deciding them at month-end, once you can see which accounts would trigger, is not threshold setting.

## Cumulative, period and trend

Three different triggers, and a system needs all three, because they detect different failures.

**Cumulative** variances are slow and stable, good for forecasting and poor at noticing that this month collapsed. A cumulative CPI barely moves when a single bad period lands inside a large denominator.

**Period** variances catch the bad month immediately and produce false alarms whenever an accrual lands late. Pair them with an accrual completeness check before anyone acts on one.

**Trend** triggers catch what neither size test can see: three consecutive periods of decline, each individually under the gate. That pattern is the most common route to a large overrun that never generated a single variance report.

A trend rule worth having: any account whose CPI has fallen in three consecutive periods reports, however small each fall was.

## Thresholds beyond cost

Cost variance is the familiar trigger. It is rarely the earliest signal available.

| Domain | Trigger | Why this value |
|---|---|---|
| Schedule | Any path with less than 10 days of total float, or 5 days of erosion in one period | Float disappears before dates move; the date is the lagging indicator |
| Forecast | EAC moves by more than 2% of BAC period on period | Large swings mean the inputs are unreliable, whichever way they move |
| Forecast, hard stop | EAC exceeds the contract price | This one goes to finance the same day, not in the monthly pack |
| Change | Any instruction above £25k unapproved for more than 30 days | Work delivered before it is authorised is the classic unrecoverable cost |
| Data quality | Accruals below 95% of the previous period's pattern, or a cut-off mismatch | A forecast built on incomplete actuals is worse than no forecast |

The hard stop deserves its own line in the procedure. When the forecast crosses the contract price the contract is expected to lose money, and under the applicable financial reporting standards an expected loss is generally recognised in full in the period it becomes apparent rather than spread over the remaining work.

That is a finance consequence with a date attached, created by a controls number. Nothing PCI publishes is accounting advice, but the operating rule is simple: the month the forecast crosses the price, finance hears about it.

## The cash threshold most projects never set

Cost variance tells you whether the project is profitable. It says nothing about whether it can pay for itself next month, and those two fail in different ways.

The cash conversion cycle measures how long money is tied up between paying for work and being paid for it. CCC = DSO + DIO − DPO, where DSO is days sales outstanding, DIO here is unbilled work in progress expressed in days, and DPO is days payable outstanding.

Take a delivery business with DSO of 68 days, unbilled work in progress equivalent to 41 days, and DPO of 52 days.

CCC = 68 + 41 − 52 = **57 days**.

Against annual cost of sales of £48m, one day is 48,000,000 ÷ 365 = **£131,507**. So 57 days ties up 57 × 131,507 = **£7.50m** of working capital.

Now a payment application is rejected and DSO moves from 68 to 79 days. CCC becomes 79 + 41 − 52 = **68 days**, and the funding requirement rises to 68 × 131,507 = **£8.94m**.

An extra £1.45m of cash, out of eleven days, with no change whatever in cost performance. At an 8% cost of funds that is roughly £116k a year of interest bought by one rejected application.

So set a threshold on it. DSO rising more than five days in a period, or any unbilled work in progress older than 60 days, should trigger the same written response a cost variance does.

## The escalation ladder

A threshold with no named owner is a threshold nobody crosses.

| Level | Trigger | Owner | Decision they hold | Deadline |
|---|---|---|---|---|
| 1 | Tier 1 gate | Control account manager | Cause, corrective action, revised ETC | With the report |
| 2 | Tier 2 gate, or three-period trend | Project manager | Accept, re-plan, or request contingency | 5 working days |
| 3 | EAC above contract price, or float below 10 days on the driving path | Project director and finance | Commercial position, provision, client notification | Same period |
| 4 | Repeat level 3 in consecutive periods | Sponsor or board | Continue, restructure or stop | Next governance cycle |

Level 3 is shared between delivery and finance on purpose. A forecast crossing the contract price is a project problem and an accounting event at the same time, and organisations that route it to one side only tend to meet the other side during an audit.

That shared level is the finance and delivery overlap in one row, and it is what the [PCI AI Project Finance Leader (PFL-AI) credential](https://projectcontrolsinstitute.org/finance-and-project-management-certification) is built to examine.

## Who may move a threshold

Nobody, mid-period, informally. Thresholds are baseline documents, and changing one is a change-control item with a stated reason and a date.

The reason is behavioural rather than technical. Once a team learns that an inconvenient threshold can be adjusted, the threshold stops measuring the project and starts measuring the appetite for writing reports.

Keep the record. An auditor comparing threshold history against variance history will find any pattern of convenient adjustments in about ten minutes, and that finding does more damage than the overrun would have.

## Frequently asked questions

**What percentage threshold do most projects use?**
Ten per cent on cost and schedule variance at control account level is the common starting point, loosened early in a project and paired with an absolute value. The percentage is the part people copy and the absolute gate is the part that makes it work, so treat 10% as a default to be tested against your own account sizes rather than a standard.

**At what level should thresholds be applied?**
Control account, because that is the level with a named manager who can explain a variance and act on it. Testing only at project level hides offsetting variances, where an underspend on one package masks an overrun on another and the total looks calm. Roll the results up; do not test at the top.

**How many variance reports a month is reasonable?**
Enough that the review can discuss each one properly, which for most projects is between five and ten. Gates producing thirty are too tight, and the reports will be templated within two cycles. Gates producing none for several periods on a project that is visibly struggling are a reason to check the earning rules before congratulating anyone.

**Should thresholds be symmetric for underruns?**
Yes, at a wider gate. A large favourable variance is usually a measurement problem rather than good news, most often unclaimed cost, a late accrual or a generous earning rule. Setting the favourable gate at roughly double the adverse one catches the material cases without generating reports that celebrate arithmetic errors.

**Do thresholds apply to schedule float as well as cost?**
They should, and float is often the earlier signal. Dates do not move until float has already been consumed, so a threshold on float erosion gives weeks of warning that a threshold on the finish date cannot. Measure it across every path with low float, not only the one currently named critical, because the driving path changes as work slips.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so this republish is for readers and for being found, not for link equity.*

*Internal links: this piece should link to [the earned value management pillar](https://projectcontrolsinstitute.org/earned-value-management) with the anchor "the earned value management pillar", to [the metrics-to-decisions guide](https://projectcontrolsinstitute.org/project-performance-management) with the anchor "turning project metrics into decisions", and to [the EAC method guide](https://projectcontrolsinstitute.org/eac-formulas) with the anchor "the four EAC formulas behind the forecast trigger".*
