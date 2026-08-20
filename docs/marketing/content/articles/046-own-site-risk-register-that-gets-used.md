---
platform:      Own site — projectcontrolsinstitute.org
type:          how-to
title:         How to build a risk register that gets used, not filed
meta:          A risk register that gets used has a cause, an event, an owner and a number on every row. The ten columns, the arithmetic and the review cycle.
primary_kw:    risk register that gets used
secondary_kw:  risk register template, expected value contingency, risk breakdown structure, qualitative risk analysis
pillar:        Risk management
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        HowTo
word_count:    1795
hashtags:      n/a (own site)
ab_id:         AB-01037
---

# How to build a risk register that gets used, not filed

A risk register that gets used has rows that each name a cause, an event and an effect, carry one named owner, have money or days attached, and come back on a fixed date with a decision. Registers get ignored when they hold two hundred unowned, unquantified rows nobody can act on.

The format below takes about a day to set up and about two hours a month to run.

## What is a risk register?

A risk register is the controlled list of uncertain events that would change a project's cost, schedule, scope or safety if they happened, with the assessed likelihood, the assessed effect, the accountable owner and the agreed response for each one.

## What makes a risk register that gets used?

It is a decision tool, not an archive. If a row cannot change what anybody does this month, it does not belong near the top of it.

Two things get filed in registers that are not risks. An issue has already happened and belongs in the issues log. An assumption is something you have chosen to rely on, and it becomes a risk only when someone writes down what happens if it fails.

## What goes in each column?

Ten columns. Anything beyond them is optional and mostly will not be maintained past month three.

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

Movement is the column senior people read first. A risk that has not moved in four months is either well managed or not being looked at, and the register should show which.

## How do you write a risk so somebody can act on it?

Use cause, event, effect: because *cause*, *event* may happen, which would result in *effect*.

"Ground conditions" is a topic, not a risk. It cannot be owned, priced or closed.

"Because the ground investigation covered eleven boreholes across a fourteen-hectare site, obstructions may be found outside the investigated area, which would add excavation and disposal cost to the substructure package" can be owned. It also tells you the three available responses: extend the investigation, price the disposal now, or accept it and hold contingency against it.

The test is simple. Read the row aloud and ask what you would do on Monday. If there is no answer, the row is not finished.

## How do you score a risk without pretending to be precise?

Most teams start with a five-by-five matrix, scoring likelihood and impact one to five and reading off a colour. It is a fast triage tool, worth keeping for that.

It is not arithmetic. The scores are ranks, not quantities: an impact scored 4 is not twice an impact scored 2, so multiplying the two ranks produces a number with no units. Two risks both scoring 12 can differ by an order of magnitude in money.

Use the matrix to triage. Use currency and days to decide anything that costs money.

## Three ways of sizing a risk, compared

| Method | What it gives you | What it assumes | Effort | Where it fails |
|---|---|---|---|---|
| Five-by-five matrix | A rank order and a colour | That ordinal scores are enough to triage | An hour in a workshop | Ranks get multiplied and treated as values; ties hide big differences |
| Expected value (probability × impact) | A single mean figure per risk and a total | That you can state a probability and an impact you would defend | Half a day | It is a mean, so it never happens; ignores correlation and the tail |
| Three-point ranges plus Monte Carlo | A distribution and confidence levels such as P50 and P80 | Ranges and correlations that came from somewhere real | Two to five days | Weak inputs produce a confident-looking wrong curve |

Run all three in that order. The matrix picks the twenty rows worth quantifying, expected value gives the opening contingency figure, and [a Monte Carlo run over the same inputs](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) tests whether it survives the tail.

## The arithmetic, shown

Take six quantified risks on a substructure and steel package. Probabilities are the ones the owners signed up to; impacts are the most likely cost if the event occurs.

| Risk | Probability | Cost impact | Expected value |
|---|---:|---:|---:|
| Obstructions outside the investigated area | 0.30 | £900,000 | £270,000 |
| Vendor drawings late, steel erection delayed 3 weeks | 0.45 | £260,000 | £117,000 |
| Consent condition requires an acoustic barrier | 0.20 | £140,000 | £28,000 |
| Third-party utility diversion slips past the window | 0.35 | £320,000 | £112,000 |
| Roof package pushed into winter working | 0.55 | £90,000 | £49,500 |
| Commissioning spares shortfall | 0.15 | £60,000 | £9,000 |
| **Total** | | | **£585,500** |

The steel impact of £260,000 is not a guess. Three weeks is fifteen working days; time-related site costs run at £14,000 a day, giving £210,000, and the erection subcontractor has quoted £50,000 for an extended shift pattern. 15 × 14,000 + 50,000 = £260,000. The arithmetic assumes those fifteen days land on the path driving completion, and [whether risk days actually reach the completion date](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) is settled in the schedule model rather than in the register.

The total of £585,500 is a mean, and a mean is the one outcome that will not occur. It is a reasonable opening contingency figure and a poor closing one. Note also that the top two rows carry £387,000 of the £585,500, which is 66 per cent — so the management effort belongs on two risks, not six.

## Who owns a risk, and what does ownership mean?

One person. Named. Present at the review or represented by someone who can answer.

Ownership means four specific things: the owner maintains the probability and impact, executes the agreed response, raises it when the response stops working, and closes it when the exposure has genuinely gone.

A risk owned by "the project team" or "the contractor" is unowned. So is a risk owned by the risk manager, who administers the register and cannot deliver a single response in it.

## How often should the register be reviewed?

| Tier | Which rows | Cadence | Who is in the room |
|---|---|---|---|
| Top exposures | Ten highest by expected value | Fortnightly | Project manager, package leads, risk owner |
| Quantified register | All rows carrying money or days | Monthly, aligned to the cost report | Project controls, package leads |
| Full register | Everything, including accepted rows | Quarterly, and at every gate | Sponsor, project manager, controls |
| Emergent | New rows | On arrival | Whoever raised it, plus the named owner |

Align the monthly review with the cost report date rather than holding a separate risk meeting. The forecast and the contingency position are the same conversation.

## Where the register meets the money

Contingency comes out of the register. That is the point of quantifying it, and it is where project controls and finance have to agree on vocabulary.

Contingency held inside a project budget for identified risks is a control account, not an accounting provision. A provision has recognition criteria of its own under the accounting standards, tested by the finance team rather than set in a risk workshop.

The two vocabularies meet at the estimate at completion, which is where [a forecast becomes reported profit](https://projectcontrolsinstitute.org/eac-accounting) and the treatment of contingency has to hold up in both.

This is the overlap the discipline keeps failing at. An engineer is examined on probability and float, almost never on when an obligation may be recognised. An accountant is examined on recognition, almost never on a risk-adjusted forecast. The contingency line sits in both.

Publish the drawdown curve next to the register: contingency at sanction, drawn down to date, remaining, and remaining exposure from the register. If remaining contingency has fallen faster than remaining exposure, the project is in trouble and the register shows it first.

## Where this is examined

The PCI AI Project Controls Leader (PCL-AI) examines 13 domains across 61 knowledge areas, with risk management sitting alongside cost control, scheduling and earned value rather than in a stream of its own.

The Body of Knowledge runs in a 40 / 40 / 20 proportion across finance and reporting, project management, and governed AI. Risk quantification sits in the middle block; what the contingency does to a reported forecast sits in the first.

## Frequently asked questions

**How many rows should a risk register have?**
Enough to cover the material exposure and few enough to review properly. On a mid-sized capital project that is usually twenty to forty live rows, of which ten to fifteen are quantified. Registers of a hundred and forty rows are filing exercises: nobody reads past row twenty, and the rows that matter are buried among rows that never will.

**What is the difference between a risk and an issue?**
A risk has not happened yet and carries a probability. An issue has happened, so its probability is one and it needs an action and a date, not a score. Keeping them in one list corrupts the arithmetic, because an issue with a certainty of occurring inflates the expected-value total that contingency is set from.

**Should the register include opportunities as well as threats?**
Yes, and score them the same way with a negative cost impact. Opportunities get dropped because no one owns them and no one is measured on capturing them. If you include them, give each one an owner and a date in the same review, or they become decoration.

**How do you set contingency from the register?**
Expected value gives the opening figure, and a Monte Carlo simulation across the same inputs gives the distribution you actually commit against. Most owners hold contingency somewhere between the mean and the P80, with the gap between P50 and P80 held separately as management reserve so that its use is a governance decision rather than a project one.

**Who should chair the risk review?**
The project manager, not the risk manager. The person who chairs it is the person the room believes will act on it. A risk manager can prepare the register, run the arithmetic and challenge the numbers, but if they chair, the meeting reads as an administrative return rather than a decision.

---

*Internal links now in the body, all on this domain: [a Monte Carlo run over the same inputs](https://projectcontrolsinstitute.org/monte-carlo-cost-simulation) sits where the three sizing methods are ordered and the reader asks what the third one involves; [whether risk days actually reach the completion date](https://projectcontrolsinstitute.org/quantitative-schedule-risk-analysis) sits under the £260,000 delay arithmetic, which rests on those days driving completion; and [a forecast becomes reported profit](https://projectcontrolsinstitute.org/eac-accounting) sits where contingency is distinguished from an accounting provision. The schedule risk analysis pillar was dropped in favour of the EAC link: three same-domain links is the limit, and two schedule-risk links in one piece would have said the same thing twice. Reciprocal worth making: the [month-end close for projects](https://projectcontrolsinstitute.org/month-end-close-for-projects) guide should link back with the anchor "the contingency position in the risk register".*
