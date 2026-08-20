---
platform:      Medium
type:          how-to
title:         How to build a risk register that gets used every month
meta:          A risk register that gets used carries a cause, an event, an owner and a number on every row. The ten columns, the arithmetic and the review cycle.
primary_kw:    risk register that gets used
secondary_kw:  risk register template, expected value contingency, risk breakdown structure, qualitative risk analysis
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     canonical -> /risk-register-that-gets-used (own site #046)
schema:        HowTo + FAQPage
word_count:    1,964
hashtags:      #ProjectControls #RiskManagement #PMO #CostEngineering #ProjectManagement
ab_id:         AB-01037
---

# How to build a risk register that gets used every month

A risk register that gets used has rows that each name a cause, an event and an effect, carry one named owner, have money or days attached, and come back on a fixed date with a decision. Registers get ignored when they hold two hundred unowned, unquantified rows nobody can act on.

The format below takes roughly a day to set up and two hours a month to run.

## What is a risk register?

A risk register is the controlled list of uncertain events that would change a project's cost, schedule, scope or safety if they occurred, with the assessed likelihood, the assessed effect, the accountable owner and the agreed response recorded for each one.

It is a decision tool, not an archive. If a row cannot change what somebody does this month, it does not belong near the top of it.

## Why most registers stop being read

Two things get filed in registers that are not risks, and both corrupt the list.

An issue has already happened. Its probability is one, so it needs an action and a date rather than a score, and it belongs in the issues log.

An assumption is something the project has chosen to rely on. It becomes a risk only when somebody writes down what happens if it fails.

## The ten columns a risk register that gets used needs

Ten columns. Anything beyond them is optional and mostly will not survive past month three.

| Column | What goes in it | Why it earns its place |
|---|---|---|
| ID | Stable reference, never reused | Lets minutes, change papers and drawdowns cite a specific row |
| Risk statement | Cause, event, effect in one sentence | Forces the row to describe something manageable |
| Category | Node from the risk breakdown structure | Shows where risk clusters; the total never does |
| Owner | One named person, never a team | An unowned risk is a note |
| Likelihood | A probability you would actually bet on | Feeds the arithmetic |
| Cost impact | Range or single figure, in currency | Turns a colour into a decision |
| Schedule impact | Working days on the affected path | Separates a delay from a nuisance |
| Response | Avoid, reduce, transfer or accept, plus the action | The only column that changes the future |
| Next action date and status | A date and a state | Makes review possible instead of ceremonial |
| Movement | Value at last review against value now | The most-read line on the page |

Movement is the column senior people read first. A risk that has not moved in four months is either well managed or not being looked at, and the register should say which.

## Write the risk so somebody can act on it

Use cause, event, effect: because *cause*, *event* may happen, which would result in *effect*.

"Ground conditions" is a topic, not a risk. It cannot be owned, priced or closed.

"Because the ground investigation covered eleven boreholes across a fourteen-hectare site, obstructions may be found outside the investigated area, which would add excavation and disposal cost to the substructure package" can be owned. It also names its own three responses: extend the investigation, price the disposal now, or accept it and hold contingency against it.

The test is short. Read the row aloud and ask what you would do on Monday. If there is no answer, the row is not finished.

## Score it without pretending to be precise

Most teams start with a five-by-five matrix, scoring likelihood and impact one to five and reading off a colour. As a triage tool it is fast and worth keeping.

It is not arithmetic. The scores are ranks rather than quantities, so an impact scored 4 is not twice an impact scored 2, and multiplying two ranks produces a number with no units.

Two risks both scoring 12 can differ by an order of magnitude in money. Use the matrix to triage, and use currency and days to decide anything that costs money.

## Three ways of sizing a risk, compared

| Method | What it gives you | What it assumes | Effort | Where it fails |
|---|---|---|---|---|
| Five-by-five matrix | A rank order and a colour | That ordinal scores are enough to triage | An hour in a workshop | Ranks get multiplied and treated as values; ties hide big differences |
| Expected value (probability × impact) | A single mean figure per risk and a total | That you can state a probability and an impact you would defend | Half a day | It is a mean, so it never happens; ignores correlation and the tail |
| Three-point ranges plus Monte Carlo | A distribution and confidence levels such as P50 and P80 | Ranges and correlations that came from somewhere real | Two to five days | Weak inputs produce a confident-looking wrong curve |

Run all three in that order. The matrix picks the rows worth quantifying, expected value produces the opening contingency figure, and [running the simulation across the same inputs](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) tests whether that figure survives the tail.

## The arithmetic, shown

Six quantified risks on a substructure and steel package, shown here as a worked illustration rather than as any real project's register. Each probability is the risk owner's own assessment, recorded with the reasoning that produced it. Each impact is the most likely cost if the event occurs, built up from quantities and rates in the way the two derivations below are.

| Risk | Probability | Cost impact | Expected value |
|---|---:|---:|---:|
| Obstructions outside the investigated area | 0.30 | £900,000 | £270,000 |
| Vendor drawings late, steel erection delayed 3 weeks | 0.45 | £260,000 | £117,000 |
| Consent condition requires an acoustic barrier | 0.20 | £140,000 | £28,000 |
| Third-party utility diversion slips past the window | 0.35 | £320,000 | £112,000 |
| Roof package pushed into winter working | 0.55 | £90,000 | £49,500 |
| Commissioning spares shortfall | 0.15 | £60,000 | £9,000 |
| **Total** | | | **£585,500** |

The steel impact of £260,000 is not a feeling. Three weeks is fifteen working days, time-related site costs run at £14,000 a day, and the subcontractor has quoted £50,000 for an extended shift pattern: 15 × 14,000 + 50,000 = **£260,000**.

The consent condition is built the same way. The barrier is 220 metres of acoustic fencing at £550 a metre, which is £121,000, plus £19,000 of design and discharge work: 220 × 550 + 19,000 = **£140,000**.

Every row in that table has to come apart like those two. A number the owner cannot take apart in front of the room is a feeling with a currency symbol on it, and it gets argued away in the first review where it matters.

Two things about the £585,500. It is a mean, and a mean is the one outcome that will not occur, so it is a reasonable opening contingency figure and a poor closing one.

And the top two rows carry £387,000 of it, which is **66 per cent**. The management effort belongs on two risks, not six, and the register should be sorted so that is obvious.

## What ownership actually means

One person. Named. Present at the review, or represented by somebody who can answer for the row.

Ownership means four things: the owner maintains the probability and impact, executes the agreed response, raises it when the response stops working, and closes it when the exposure has gone.

A risk owned by "the project team" or "the contractor" is unowned. So is a risk owned by the risk manager, who administers the register and cannot deliver a single response inside it.

## How often to review, and with whom

| Tier | Which rows | Cadence | Who is in the room |
|---|---|---|---|
| Top exposures | Ten highest by expected value | Fortnightly | Project manager, package leads, risk owner |
| Quantified register | All rows carrying money or days | Monthly, aligned to the cost report | Project controls, package leads |
| Full register | Everything, including accepted rows | Quarterly, and at every gate | Sponsor, project manager, controls |
| Emergent | New rows | On arrival | Whoever raised it, plus the named owner |

Align the monthly review with the cost report date rather than holding a separate risk meeting. The forecast and the contingency position are one conversation, and splitting them is how the two end up disagreeing.

The chair should be the project manager rather than the risk manager. The person who chairs is the person the room believes will act.

## Where the register meets the money

Contingency comes out of the register. That is the point of quantifying it, and it is where project controls and finance have to agree on vocabulary before an auditor does it for them.

Contingency held inside a project budget against identified risks is a control account, not an accounting provision. A provision has recognition criteria of its own under the accounting standards, tested by the finance team rather than settled in a risk workshop.

Nothing PCI publishes is legal, tax or accounting advice, and the treatment depends on the contract.

This is the overlap the discipline keeps failing at. An engineer is examined on probability and float, almost never on when an obligation may be recognised. An accountant is examined on recognition, almost never on a risk-adjusted forecast. The contingency line belongs to both.

Publish the drawdown curve beside the register: contingency at sanction, drawn down to date, remaining, and remaining exposure. Where remaining contingency has fallen faster than remaining exposure, the project is in trouble and the register shows it first.

## Where this is examined

The PCI AI Project Controls Leader (PCL-AI) examines **13 domains across 61 knowledge areas**, with risk management sitting alongside cost control, scheduling and earned value rather than in a stream of its own.

The Body of Knowledge runs in a **40 / 40 / 20** proportion across finance and reporting, project management, and governed AI. Risk quantification sits in the middle block; what a contingency release does to a reported forecast sits in the first.

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation.

## Frequently asked questions

**How many rows should a risk register have?**
Enough to cover the material exposure and few enough to review properly. On a mid-sized capital project that is usually twenty to forty live rows, of which ten to fifteen are quantified. A register of a hundred and forty rows is a filing exercise: nobody reads past row twenty, and the rows that matter are buried.

**What is the difference between a risk and an issue?**
A risk has not happened yet and carries a probability. An issue has happened, so its probability is one and it needs an action and a date rather than a score. Keeping both in one list corrupts the arithmetic, because a certainty inflates the expected-value total that contingency is set from.

**Should the register include opportunities as well as threats?**
Yes, scored the same way with a negative cost impact. Opportunities get dropped because nobody owns them and nobody is measured on capturing them. If you include them, give each one an owner and a date in the same review cycle, otherwise they become decoration on the bottom of the page.

**How do you set contingency from the register?**
Expected value gives the opening figure and a simulation across the same inputs gives the distribution you commit against. Most owners hold contingency between the mean and the P80, with the gap between P50 and P80 held separately as management reserve so that releasing it is a governance decision rather than a project one.

**How long should a monthly risk review take?**
About two hours for a register of thirty rows, if the owners have updated their rows beforehand. Reviews that run to four hours are usually being used to update the register rather than to decide anything, which is the fastest way to lose the attendance of the people whose decisions the register exists to inform.

---

*First published on projectcontrolsinstitute.org; the canonical points there. Medium links are nofollow, so this republish is here for readers rather than for link equity.*

*Internal links: one is now placed in the body. The Monte Carlo how-to (projectcontrolsinstitute.org) sits on "running the simulation across the same inputs", in the sentence describing the third sizing method — the reader has just been told expected value is a mean that never happens, so the question of how the tail gets tested is live at exactly that point. The note also proposed the schedule risk analysis pillar and the month-end close piece; both are dropped from this republish. This copy is published on Medium rather than on a PCI domain, so every projectcontrolsinstitute.org link in it is a cross-estate link and the estate caps those at one per domain per piece. On the own-site original at /risk-register-that-gets-used the same three URLs are internal links, which the architecture asks for two to three of and which carry no scheme risk, so that is where all three belong. Reciprocal: the month-end close piece should link back here with the anchor "the contingency drawdown curve behind the forecast", since it reports the position this register produces.*
